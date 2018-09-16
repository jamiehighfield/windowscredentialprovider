/* COPYRIGHT NOTICE
 * 
 * Copyright © Jamie Highfield 2018. All rights reserved.
 * 
 * This library is protected by UK, EU & international copyright laws and treaties. Unauthorised
 * reproduction of this library outside of the constraints of the accompanied license, or any
 * portion of it, may result in severe criminal penalties that will be prosecuted to the
 * maximum extent possible under the law.
 * 
 */

using JamieHighfield.CredentialProvider.Controls;
using JamieHighfield.CredentialProvider.Controls.Events;
using JamieHighfield.CredentialProvider.Interop;
using JamieHighfield.CredentialProvider.Logon;
using JamieHighfield.CredentialProvider.Providers;
using JamieHighfield.CredentialProvider.UI;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using static JamieHighfield.CredentialProvider.Constants;

namespace JamieHighfield.CredentialProvider.Credentials
{
    public abstract class CredentialBase : ICredentialProviderCredential
    {
        public CredentialBase() { }

        public CredentialBase(LogonSequencePipelineBase logonSequencePipeline)
        {
            LogonSequencePipeline = logonSequencePipeline;
        }

        #region Platform Invocation

        [DllImport("credui.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool CredPackAuthenticationBuffer(int dwFlags, string pszUserName, string pszPassword, IntPtr pPackedCredentials, ref int pcbPackedCredentials);

        #endregion

        #region Variables



        #endregion

        #region Properties

        #region Credential Configuration

        internal CredentialProviderBase CredentialProvider { get; set; }

        internal ICredentialProviderCredential BridgedCredential { get; set; }

        private LogonSequencePipelineBase LogonSequencePipeline { get; set; }

        /// <summary>
        /// Gets or sets whether this credential should forward an authentication package to Windows.
        /// </summary>
        public bool WindowsLogon { get; set; }

        #endregion

        private EventsWrapper Events { get; set; }

        internal CredentialFieldCollection Fields
        {
            get
            {
                return CredentialProvider.Fields;
            }
        }

        #region Miscellaneous

        public WindowHandle MainWindowHandle => new WindowHandle(Process.GetCurrentProcess().MainWindowHandle);

        #endregion

        #endregion

        #region Methods

        internal int NegotiateAuthentication(out uint negotiateAuthenticationPackage)
        {
            // TODO: better checking on the return codes

            var status = PInvoke.LsaConnectUntrusted(out var lsaHandle);

            using (var name = new PInvoke.LsaStringWrapper("Negotiate"))
            {
                status = PInvoke.LsaLookupAuthenticationPackage(lsaHandle, ref name._string, out negotiateAuthenticationPackage);
            }

            PInvoke.LsaDeregisterLogonProcess(lsaHandle);

            return (int)status;
        }

        #region Credential Provider Interface Methods

        public int Advise(ICredentialProviderCredentialEvents pcpce)
        {
            if (pcpce != null)
            {
                Events = new EventsWrapper(this, pcpce);

                Marshal.AddRef(Marshal.GetIUnknownForObject(pcpce));
            }

            if (BridgedCredential == null)
            {
                return HRESULT.S_OK;
            }
            else
            {
                return BridgedCredential.Advise(Events);
            }
        }

        public int UnAdvise()
        {
            if (Events != null)
            {
                Marshal.Release(Marshal.GetIUnknownForObject(Events.BridgedEvents));

                Events = null;
            }

            if (BridgedCredential == null)
            {

                return HRESULT.S_OK;
            }
            else
            {
                return BridgedCredential.UnAdvise();
            }
        }

        public int SetSelected(out int pbAutoLogon)
        {
            if (BridgedCredential == null)
            {
                pbAutoLogon = 0;

                Load?.Invoke(this, new EventArgs());

                return HRESULT.S_OK;
            }
            else
            {
                return BridgedCredential.SetSelected(out pbAutoLogon);
            }
        }

        public int SetDeselected()
        {
            if (BridgedCredential == null)
            {
                return HRESULT.E_NOTIMPL;
            }
            else
            {
                return BridgedCredential.SetDeselected();
            }
        }

        public int GetFieldState(uint dwFieldID, out _CREDENTIAL_PROVIDER_FIELD_STATE pcpfs, out _CREDENTIAL_PROVIDER_FIELD_INTERACTIVE_STATE pcpfis)
        {
            uint count = 0;

            int result = HRESULT.E_FAIL;

            result = (CredentialProvider.UnderlyingCredentialProvider?.GetFieldDescriptorCount(out count) ?? HRESULT.S_OK);

            if (result != HRESULT.S_OK)
            {
                pcpfs = _CREDENTIAL_PROVIDER_FIELD_STATE.CPFS_HIDDEN;
                pcpfis = _CREDENTIAL_PROVIDER_FIELD_INTERACTIVE_STATE.CPFIS_NONE;

                return result;
            }

            if (BridgedCredential == null || (dwFieldID >= count))
            {
                if ((dwFieldID - count) >= Fields.Count)
                {
                    pcpfs = _CREDENTIAL_PROVIDER_FIELD_STATE.CPFS_HIDDEN;
                    pcpfis = _CREDENTIAL_PROVIDER_FIELD_INTERACTIVE_STATE.CPFIS_NONE;

                    return HRESULT.E_INVALIDARG;
                }

                CredentialField identifiedField = Fields[(int)(dwFieldID - count)];

                pcpfs = identifiedField.GetState();

                if (identifiedField.Control is TextBoxControl)
                {
                    //TODO
                    //if (((TextBoxControl)field.Control).Focussed == true)
                    //{
                    //    pcpfis = _CREDENTIAL_PROVIDER_FIELD_INTERACTIVE_STATE.CPFIS_FOCUSED;
                    //}
                    //else
                    //{
                    pcpfis = _CREDENTIAL_PROVIDER_FIELD_INTERACTIVE_STATE.CPFIS_NONE;
                    //}
                }
                else
                {
                    pcpfis = _CREDENTIAL_PROVIDER_FIELD_INTERACTIVE_STATE.CPFIS_NONE;
                }

                return HRESULT.S_OK;
            }
            else
            {
                return BridgedCredential.GetFieldState(dwFieldID, out pcpfs, out pcpfis);
            }
        }

        public int GetStringValue(uint dwFieldID, out string ppsz)
        {
            uint count = 0;

            int result = HRESULT.E_FAIL;

            result = (CredentialProvider.UnderlyingCredentialProvider?.GetFieldDescriptorCount(out count) ?? HRESULT.S_OK);

            if (result != HRESULT.S_OK)
            {
                ppsz = string.Empty;

                return result;
            }

            if (BridgedCredential == null || (dwFieldID >= count))
            {

                if ((dwFieldID - count) >= Fields.Count)
                {
                    ppsz = string.Empty;

                    return HRESULT.E_INVALIDARG;
                }

                CredentialField identifiedField = Fields[(int)(dwFieldID - count)];

                if (identifiedField.Control is LabelControl)
                {
                    ppsz = ((LabelControl)identifiedField.Control).Text;
                }
                else if (identifiedField.Control is TextBoxControl)
                {
                    ppsz = ((TextBoxControl)identifiedField.Control).Text;
                }
                else if (identifiedField.Control is LinkControl)
                {
                    ppsz = ((LinkControl)identifiedField.Control).Text;
                }
                else
                {
                    ppsz = string.Empty;

                    return HRESULT.E_INVALIDARG;
                }

                return HRESULT.S_OK;
            }
            else
            {
                return BridgedCredential.GetStringValue(dwFieldID, out ppsz);
            }
        }

        public int GetBitmapValue(uint dwFieldID, out IntPtr phbmp)
        {
            uint count = 0;

            int result = HRESULT.E_FAIL;

            result = (CredentialProvider.UnderlyingCredentialProvider?.GetFieldDescriptorCount(out count) ?? HRESULT.S_OK);

            if (result != HRESULT.S_OK)
            {
                phbmp = IntPtr.Zero;

                return result;
            }

            if (BridgedCredential == null || (dwFieldID >= count))
            {
                if ((dwFieldID - count) >= Fields.Count)
                {
                    phbmp = IntPtr.Zero;

                    return HRESULT.E_INVALIDARG;
                }

                CredentialField identifiedField = Fields[(int)(dwFieldID - count)];

                if (identifiedField.Control is ImageControl)
                {
                    phbmp = ((ImageControl)identifiedField.Control).Image.GetHbitmap();
                }
                else
                {
                    phbmp = IntPtr.Zero;

                    return HRESULT.E_INVALIDARG;
                }

                return HRESULT.S_OK;
            }
            else
            {
                return BridgedCredential.GetBitmapValue(dwFieldID, out phbmp);
            }
        }

        public int GetCheckboxValue(uint dwFieldID, out int pbChecked, out string ppszLabel)
        {
            uint count = 0;

            int result = HRESULT.E_FAIL;

            result = (CredentialProvider.UnderlyingCredentialProvider?.GetFieldDescriptorCount(out count) ?? HRESULT.S_OK);

            if (result != HRESULT.S_OK)
            {
                pbChecked = 0;
                ppszLabel = string.Empty;

                return result;
            }

            if (BridgedCredential == null || (dwFieldID >= count))
            {
                if ((dwFieldID - count) >= Fields.Count)
                {
                    pbChecked = 0;
                    ppszLabel = string.Empty;

                    return HRESULT.E_INVALIDARG;
                }

                CredentialField identifiedField = Fields[(int)(dwFieldID - count)];

                if (identifiedField.Control is CheckBoxControl)
                {
                    pbChecked = (((CheckBoxControl)identifiedField.Control).Checked == true ? 1 : 0);
                    ppszLabel = ((CheckBoxControl)identifiedField.Control).Label;
                }
                else
                {
                    pbChecked = 0;
                    ppszLabel = string.Empty;

                    return HRESULT.E_INVALIDARG;
                }

                return HRESULT.S_OK;
            }
            else
            {
                return BridgedCredential.GetCheckboxValue(dwFieldID, out pbChecked, out ppszLabel);
            }
        }

        public int GetSubmitButtonValue(uint dwFieldID, out uint pdwAdjacentTo)
        {
            uint count = 0;

            int result = HRESULT.E_FAIL;

            result = (CredentialProvider.UnderlyingCredentialProvider?.GetFieldDescriptorCount(out count) ?? HRESULT.S_OK);

            if (result != HRESULT.S_OK)
            {
                pdwAdjacentTo = 0;

                return result;
            }

            if (BridgedCredential == null || (dwFieldID >= count))
            {
                if ((dwFieldID - count) >= Fields.Count)
                {
                    pdwAdjacentTo = 0;

                    return HRESULT.E_INVALIDARG;
                }

                CredentialField identifiedField = Fields[(int)(dwFieldID - count)];

                if (identifiedField.Control is ButtonControl)
                {
                    CredentialField adjacentField = Fields
                        .Where((field) => field.Control == ((ButtonControl)identifiedField.Control).AdjacentControl)
                        .FirstOrDefault();

                    if (adjacentField == null)
                    {
                        pdwAdjacentTo = 0;

                        return HRESULT.E_INVALIDARG;
                    }

                    pdwAdjacentTo = (uint)adjacentField.FieldId;
                }
                else
                {
                    pdwAdjacentTo = 0;

                    return HRESULT.E_INVALIDARG;
                }

                return HRESULT.S_OK;
            }
            else
            {
                return BridgedCredential.GetSubmitButtonValue(dwFieldID, out pdwAdjacentTo);
            }
        }

        public int GetComboBoxValueCount(uint dwFieldID, out uint pcItems, out uint pdwSelectedItem)
        {
            uint count = 0;

            int result = HRESULT.E_FAIL;

            result = (CredentialProvider.UnderlyingCredentialProvider?.GetFieldDescriptorCount(out count) ?? HRESULT.S_OK);

            if (result != HRESULT.S_OK)
            {
                pcItems = 0;
                pdwSelectedItem = 0;

                return result;
            }

            if (BridgedCredential == null || (dwFieldID >= count))
            {
                pcItems = 0;
                pdwSelectedItem = 0;

                return HRESULT.E_NOTIMPL;
            }
            else
            {
                return BridgedCredential.GetComboBoxValueCount(dwFieldID, out pcItems, out pdwSelectedItem);
            }
        }

        public int GetComboBoxValueAt(uint dwFieldID, uint dwItem, out string ppszItem)
        {
            uint count = 0;

            int result = HRESULT.E_FAIL;

            result = (CredentialProvider.UnderlyingCredentialProvider?.GetFieldDescriptorCount(out count) ?? HRESULT.S_OK);

            if (result != HRESULT.S_OK)
            {
                ppszItem = string.Empty;

                return result;
            }

            if (BridgedCredential == null || (dwFieldID >= count))
            {
                ppszItem = string.Empty;

                return HRESULT.E_NOTIMPL;
            }
            else
            {
                return BridgedCredential.GetComboBoxValueAt(dwFieldID, dwItem, out ppszItem);
            }
        }

        public int SetStringValue(uint dwFieldID, string psz)
        {
            uint count = 0;

            int result = HRESULT.E_FAIL;

            result = (CredentialProvider.UnderlyingCredentialProvider?.GetFieldDescriptorCount(out count) ?? HRESULT.S_OK);

            if (result != HRESULT.S_OK)
            {
                return result;
            }

            if (BridgedCredential == null || (dwFieldID >= count))
            {
                if ((dwFieldID - count) >= Fields.Count)
                {
                    return HRESULT.E_INVALIDARG;
                }

                CredentialField identifiedField = Fields[(int)(dwFieldID - count)];

                if (identifiedField.Control is LabelControl)
                {
                    ((LabelControl)identifiedField.Control).UpdateText(psz);
                }
                else if (identifiedField.Control is TextBoxControl)
                {
                    ((TextBoxControl)identifiedField.Control).UpdateText(psz);
                }
                else
                {
                    return HRESULT.E_INVALIDARG;
                }

                return HRESULT.S_OK;
            }
            else
            {
                return BridgedCredential.SetStringValue(dwFieldID, psz);
            }
        }

        public int SetCheckboxValue(uint dwFieldID, int bChecked)
        {
            uint count = 0;

            int result = HRESULT.E_FAIL;

            result = (CredentialProvider.UnderlyingCredentialProvider?.GetFieldDescriptorCount(out count) ?? HRESULT.S_OK);

            if (result != HRESULT.S_OK)
            {
                return result;
            }

            if (BridgedCredential == null || (dwFieldID >= count))
            {
                if ((dwFieldID - count) >= Fields.Count)
                {
                    return HRESULT.E_INVALIDARG;
                }

                CredentialField identifiedField = Fields[(int)(dwFieldID - count)];

                if (identifiedField.Control is CheckBoxControl)
                {
                    ((CheckBoxControl)identifiedField.Control).Checked = (bChecked == 1 ? true : false);
                }
                else
                {
                    return HRESULT.E_INVALIDARG;
                }

                return HRESULT.S_OK;
            }
            else
            {
                return BridgedCredential.SetCheckboxValue(dwFieldID, bChecked);
            }
        }

        public int SetComboBoxSelectedValue(uint dwFieldID, uint dwSelectedItem)
        {
            uint count = 0;

            int result = HRESULT.E_FAIL;

            result = (CredentialProvider.UnderlyingCredentialProvider?.GetFieldDescriptorCount(out count) ?? HRESULT.S_OK);

            if (result != HRESULT.S_OK)
            {
                return result;
            }

            if (BridgedCredential == null || (dwFieldID >= count))
            {
                return HRESULT.E_NOTIMPL;
            }
            else
            {
                return BridgedCredential.SetComboBoxSelectedValue(dwFieldID, dwSelectedItem);
            }
        }

        public int CommandLinkClicked(uint dwFieldID)
        {
            uint count = 0;

            int result = HRESULT.E_FAIL;

            result = (CredentialProvider.UnderlyingCredentialProvider?.GetFieldDescriptorCount(out count) ?? HRESULT.S_OK);

            if (result != HRESULT.S_OK)
            {
                return result;
            }

            if (BridgedCredential == null || (dwFieldID >= count))
            {
                if ((dwFieldID - count) >= Fields.Count)
                {
                    return HRESULT.E_INVALIDARG;
                }

                CredentialField identifiedField = Fields[(int)(dwFieldID - count)];

                if (identifiedField.Control is LinkControl)
                {
                    ((LinkControl)identifiedField.Control).InvokeClicked(this, new LinkControlClickedEventArgs((LinkControl)identifiedField.Control));
                }
                else
                {
                    return HRESULT.E_INVALIDARG;
                }

                return HRESULT.S_OK;
            }
            else
            {
                return BridgedCredential.CommandLinkClicked(dwFieldID);
            }
        }

        public int GetSerialization(out _CREDENTIAL_PROVIDER_GET_SERIALIZATION_RESPONSE pcpgsr, out _CREDENTIAL_PROVIDER_CREDENTIAL_SERIALIZATION pcpcs, out string ppszOptionalStatusText, out _CREDENTIAL_PROVIDER_STATUS_ICON pcpsiOptionalStatusIcon)
        {
            if (BridgedCredential == null)
            {
                try
                {
                    Logon?.Invoke(this, new EventArgs());

                    if (WindowsLogon == true)
                    {
                        WindowsLogonPackage windowsLogonPackage = null;

                        if (LogonSequencePipeline != null)
                        {
                            LogonResponse logonResponse = LogonSequencePipeline?.ProcessSequencePipeline(new LogonPackage(CredentialProvider.CurrentUsageScenario, this));

                            if (logonResponse == null || logonResponse?.Successful == false)
                            {
                                pcpgsr = _CREDENTIAL_PROVIDER_GET_SERIALIZATION_RESPONSE.CPGSR_NO_CREDENTIAL_NOT_FINISHED;
                                pcpcs = new _CREDENTIAL_PROVIDER_CREDENTIAL_SERIALIZATION() { ulAuthenticationPackage = 1234 };
                                ppszOptionalStatusText = (logonResponse.ErrorMessage ?? "Invalid logon sequence.");
                                pcpsiOptionalStatusIcon = _CREDENTIAL_PROVIDER_STATUS_ICON.CPSI_ERROR;

                                return HRESULT.S_OK;
                            }

                            windowsLogonPackage = logonResponse.WindowsLogonPackage;
                        }
                        else
                        {
                            pcpgsr = _CREDENTIAL_PROVIDER_GET_SERIALIZATION_RESPONSE.CPGSR_NO_CREDENTIAL_FINISHED;
                            pcpcs = new _CREDENTIAL_PROVIDER_CREDENTIAL_SERIALIZATION() { ulAuthenticationPackage = 1234 };
                            ppszOptionalStatusText = string.Empty;
                            pcpsiOptionalStatusIcon = _CREDENTIAL_PROVIDER_STATUS_ICON.CPSI_SUCCESS;

                            return HRESULT.S_OK;
                        }

                        pcpgsr = _CREDENTIAL_PROVIDER_GET_SERIALIZATION_RESPONSE.CPGSR_RETURN_CREDENTIAL_FINISHED;
                        pcpcs = new _CREDENTIAL_PROVIDER_CREDENTIAL_SERIALIZATION();

                        string username = windowsLogonPackage.Domain + @"\" + windowsLogonPackage.Username;
                        string password = (new NetworkCredential(string.Empty, windowsLogonPackage.Password)).Password;

                        int credentialSize = 0;
                        IntPtr credentialBuffer = Marshal.AllocCoTaskMem(0);

                        if (CredPackAuthenticationBuffer(0, username, password, credentialBuffer, ref credentialSize) == false)
                        {

                            Marshal.FreeCoTaskMem(credentialBuffer);
                            credentialBuffer = Marshal.AllocCoTaskMem(credentialSize);

                            if (CredPackAuthenticationBuffer(0, username, password, credentialBuffer, ref credentialSize) == true)
                            {
                                ppszOptionalStatusText = "Welcome";
                                pcpsiOptionalStatusIcon = _CREDENTIAL_PROVIDER_STATUS_ICON.CPSI_SUCCESS;

                                pcpcs.clsidCredentialProvider = CredentialProvider.Guid;
                                pcpcs.rgbSerialization = credentialBuffer;
                                pcpcs.cbSerialization = (uint)credentialSize;

                                NegotiateAuthentication(out uint negotiateAuthenticationPackage);
                                pcpcs.ulAuthenticationPackage = negotiateAuthenticationPackage;

                                return HRESULT.S_OK;
                            }

                            pcpgsr = _CREDENTIAL_PROVIDER_GET_SERIALIZATION_RESPONSE.CPGSR_NO_CREDENTIAL_NOT_FINISHED;
                            pcpcs = new _CREDENTIAL_PROVIDER_CREDENTIAL_SERIALIZATION() { ulAuthenticationPackage = 1234 };
                            ppszOptionalStatusText = "Invalid logon sequence.";
                            pcpsiOptionalStatusIcon = _CREDENTIAL_PROVIDER_STATUS_ICON.CPSI_ERROR;

                            return HRESULT.E_FAIL;
                        }
                    }
                    else
                    {
                        pcpgsr = _CREDENTIAL_PROVIDER_GET_SERIALIZATION_RESPONSE.CPGSR_NO_CREDENTIAL_FINISHED;
                        pcpcs = new _CREDENTIAL_PROVIDER_CREDENTIAL_SERIALIZATION() { ulAuthenticationPackage = 1234 };
                        ppszOptionalStatusText = string.Empty;
                        pcpsiOptionalStatusIcon = _CREDENTIAL_PROVIDER_STATUS_ICON.CPSI_SUCCESS;

                        return HRESULT.S_OK;
                    }
                }
                catch { }

                pcpgsr = _CREDENTIAL_PROVIDER_GET_SERIALIZATION_RESPONSE.CPGSR_NO_CREDENTIAL_NOT_FINISHED;
                pcpcs = new _CREDENTIAL_PROVIDER_CREDENTIAL_SERIALIZATION() { ulAuthenticationPackage = 1234 };
                ppszOptionalStatusText = string.Empty;
                pcpsiOptionalStatusIcon = _CREDENTIAL_PROVIDER_STATUS_ICON.CPSI_SUCCESS;

                return HRESULT.S_OK;
            }
            else
            {
                return BridgedCredential.GetSerialization(out pcpgsr, out pcpcs, out ppszOptionalStatusText, out pcpsiOptionalStatusIcon);
            }
        }

        public int ReportResult(int ntsStatus, int ntsSubstatus, out string ppszOptionalStatusText, out _CREDENTIAL_PROVIDER_STATUS_ICON pcpsiOptionalStatusIcon)
        {
            if (BridgedCredential == null)
            {
                ppszOptionalStatusText = string.Empty;
                pcpsiOptionalStatusIcon = _CREDENTIAL_PROVIDER_STATUS_ICON.CPSI_NONE;

                return HRESULT.S_OK;
            }
            else
            {
                return BridgedCredential.ReportResult(ntsStatus, ntsSubstatus, out ppszOptionalStatusText, out pcpsiOptionalStatusIcon);
            }
        }
        
        #endregion

        #endregion

        #region Events

        public event EventHandler Load;

        public event EventHandler Logon;
        
        #endregion
    }
}