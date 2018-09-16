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
using JamieHighfield.CredentialProvider.Credentials.Events;
using JamieHighfield.CredentialProvider.Interop;
using JamieHighfield.CredentialProvider.Logging;
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
    /// <summary>
    /// Extend this class to expose a class as a credential.
    /// </summary>
    public abstract class CredentialBase : ICredentialProviderCredential
    {
        /// <summary>
        /// Instantiate a new <see cref="CredentialBase"/> object.
        /// </summary>
        protected CredentialBase() { }

        /// <summary>
        /// Instantiate a new <see cref="CredentialBase"/> object.
        /// </summary>
        /// <param name="logonSequencePipeline">The <see cref="LogonSequencePipelineBase"/> instance used during the logon sequence.</param>
        protected CredentialBase(LogonSequencePipelineBase logonSequencePipeline)
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

        /// <summary>
        /// Gets or sets the <see cref="CredentialProviderBase"/> that enumerates this credential.
        /// </summary>
        internal CredentialProviderBase CredentialProvider { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ICredentialProviderCredential"/> that underlies this <see cref="CredentialBase"/> (only pertinent to when this credential is wrapping another credential).
        /// </summary>
        internal ICredentialProviderCredential UnderlyingCredential { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="LogonSequencePipelineBase"/> instance used during the logon sequence.
        /// </summary>
        private LogonSequencePipelineBase LogonSequencePipeline { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="LogonResponse"/> which is returned from the output of running the <see cref="LogonSequencePipeline"/>.
        /// </summary>
        private LogonResponse LogonResponse { get; set; }

        /// <summary>
        /// Gets or sets whether this credential should forward an authentication package to Windows. This property will be ignored if this credential forms part of a wrapped credential.
        /// </summary>
        internal bool WindowsLogon { get; set; }

        #endregion

        /// <summary>
        /// Gets or sets the <see cref="EventsWrapper"/> that is used to wrap an <see cref="ICredentialProviderCredentialEvents"/>. This allows for the correct enumerated credential to be used when raising events (only pertinent to when this credential is wrapping another credential).
        /// </summary>
        private EventsWrapper Events { get; set; }

        /// <summary>
        /// Gets a <see cref="CredentialFieldCollection"/> of <see cref="CredentialField"/> representing the raw field data with the associated control. This will only include managed fields; those from wrapped credentials will not be included, although, the field ID will appear as though all fields are included.
        /// </summary>
        internal CredentialFieldCollection Fields
        {
            get
            {
                return CredentialProvider.Fields;
            }
        }

        #region Miscellaneous

        /// <summary>
        /// The <see cref="IntPtr"/> handle of the curernt main window. When this credential is used on the Windows login screen, this will be the handle of the Logon UI and when this credential is used in a credentials dialog, this will be the handle of that dialog. The <see cref="WindowHandle"/> that is returned can be used in the <see cref="Form.ShowDialog(System.Windows.Forms.IWin32Window)"/> method and <see cref="MessageBox.Show(IWin32Window, string)"/> family of methods.
        /// </summary>
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

        #region ICredentialProviderCredential

        /// <summary>
        /// Statutory method from <see cref="ICredentialProviderCredential"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-icredentialprovidercredential for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        public int Advise(ICredentialProviderCredentialEvents pcpce)
        {
            GlobalLogger.LogMethodCall();

            if (pcpce != null)
            {
                Events = new EventsWrapper(this, pcpce);

                Marshal.AddRef(Marshal.GetIUnknownForObject(pcpce));
            }

            if (UnderlyingCredential == null)
            {
                return HRESULT.S_OK;
            }
            else
            {
                return UnderlyingCredential.Advise(Events);
            }
        }

        /// <summary>
        /// Statutory method from <see cref="ICredentialProviderCredential"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-icredentialprovidercredential for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        public int UnAdvise()
        {
            GlobalLogger.LogMethodCall();

            if (Events != null)
            {
                Marshal.Release(Marshal.GetIUnknownForObject(Events.BridgedEvents));

                Events = null;
            }

            if (UnderlyingCredential == null)
            {

                return HRESULT.S_OK;
            }
            else
            {
                return UnderlyingCredential.UnAdvise();
            }
        }

        /// <summary>
        /// Statutory method from <see cref="ICredentialProviderCredential"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-icredentialprovidercredential for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        public int SetSelected(out int pbAutoLogon)
        {
            GlobalLogger.LogMethodCall();

            if (UnderlyingCredential == null)
            {
                pbAutoLogon = 0;
            }
            else
            {
                int result = UnderlyingCredential.SetSelected(out pbAutoLogon);

                if (result != HRESULT.S_OK)
                {
                    return result;
                }
            }

            LoadedEventArgs loadedEventArgs = new LoadedEventArgs((pbAutoLogon == 1 ? true : false));

            Loaded?.Invoke(this, loadedEventArgs);

            pbAutoLogon = (loadedEventArgs.AutoLogon == true ? 1 : 0);
            WindowsLogon = loadedEventArgs.WindowsLogon;

            return HRESULT.S_OK;
        }

        /// <summary>
        /// Statutory method from <see cref="ICredentialProviderCredential"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-icredentialprovidercredential for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        public int SetDeselected()
        {
            GlobalLogger.LogMethodCall();

            if (UnderlyingCredential == null)
            {
                return HRESULT.E_NOTIMPL;
            }
            else
            {
                return UnderlyingCredential.SetDeselected();
            }
        }

        /// <summary>
        /// Statutory method from <see cref="ICredentialProviderCredential"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-icredentialprovidercredential for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        public int GetFieldState(uint dwFieldID, out _CREDENTIAL_PROVIDER_FIELD_STATE pcpfs, out _CREDENTIAL_PROVIDER_FIELD_INTERACTIVE_STATE pcpfis)
        {
            GlobalLogger.LogMethodCall();

            uint count = 0;

            int result = HRESULT.E_FAIL;

            result = (CredentialProvider.UnderlyingCredentialProvider?.GetFieldDescriptorCount(out count) ?? HRESULT.S_OK);

            if (result != HRESULT.S_OK)
            {
                pcpfs = _CREDENTIAL_PROVIDER_FIELD_STATE.CPFS_HIDDEN;
                pcpfis = _CREDENTIAL_PROVIDER_FIELD_INTERACTIVE_STATE.CPFIS_NONE;

                return result;
            }

            if (UnderlyingCredential == null || (dwFieldID >= count))
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
                return UnderlyingCredential.GetFieldState(dwFieldID, out pcpfs, out pcpfis);
            }
        }

        /// <summary>
        /// Statutory method from <see cref="ICredentialProviderCredential"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-icredentialprovidercredential for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        public int GetStringValue(uint dwFieldID, out string ppsz)
        {
            GlobalLogger.LogMethodCall();

            uint count = 0;

            int result = HRESULT.E_FAIL;

            result = (CredentialProvider.UnderlyingCredentialProvider?.GetFieldDescriptorCount(out count) ?? HRESULT.S_OK);

            if (result != HRESULT.S_OK)
            {
                ppsz = string.Empty;

                return result;
            }

            if (UnderlyingCredential == null || (dwFieldID >= count))
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
                return UnderlyingCredential.GetStringValue(dwFieldID, out ppsz);
            }
        }

        /// <summary>
        /// Statutory method from <see cref="ICredentialProviderCredential"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-icredentialprovidercredential for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        public int GetBitmapValue(uint dwFieldID, out IntPtr phbmp)
        {
            GlobalLogger.LogMethodCall();

            uint count = 0;

            int result = HRESULT.E_FAIL;

            result = (CredentialProvider.UnderlyingCredentialProvider?.GetFieldDescriptorCount(out count) ?? HRESULT.S_OK);

            if (result != HRESULT.S_OK)
            {
                phbmp = IntPtr.Zero;

                return result;
            }

            if (UnderlyingCredential == null || (dwFieldID >= count))
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
                return UnderlyingCredential.GetBitmapValue(dwFieldID, out phbmp);
            }
        }

        /// <summary>
        /// Statutory method from <see cref="ICredentialProviderCredential"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-icredentialprovidercredential for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        public int GetCheckboxValue(uint dwFieldID, out int pbChecked, out string ppszLabel)
        {
            GlobalLogger.LogMethodCall();

            uint count = 0;

            int result = HRESULT.E_FAIL;

            result = (CredentialProvider.UnderlyingCredentialProvider?.GetFieldDescriptorCount(out count) ?? HRESULT.S_OK);

            if (result != HRESULT.S_OK)
            {
                pbChecked = 0;
                ppszLabel = string.Empty;

                return result;
            }

            if (UnderlyingCredential == null || (dwFieldID >= count))
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
                return UnderlyingCredential.GetCheckboxValue(dwFieldID, out pbChecked, out ppszLabel);
            }
        }

        /// <summary>
        /// Statutory method from <see cref="ICredentialProviderCredential"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-icredentialprovidercredential for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        public int GetSubmitButtonValue(uint dwFieldID, out uint pdwAdjacentTo)
        {
            GlobalLogger.LogMethodCall();

            uint count = 0;

            int result = HRESULT.E_FAIL;

            result = (CredentialProvider.UnderlyingCredentialProvider?.GetFieldDescriptorCount(out count) ?? HRESULT.S_OK);

            if (result != HRESULT.S_OK)
            {
                pdwAdjacentTo = 0;

                return result;
            }

            if (UnderlyingCredential == null || (dwFieldID >= count))
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
                return UnderlyingCredential.GetSubmitButtonValue(dwFieldID, out pdwAdjacentTo);
            }
        }

        /// <summary>
        /// Statutory method from <see cref="ICredentialProviderCredential"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-icredentialprovidercredential for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        public int GetComboBoxValueCount(uint dwFieldID, out uint pcItems, out uint pdwSelectedItem)
        {
            GlobalLogger.LogMethodCall();

            uint count = 0;

            int result = HRESULT.E_FAIL;

            result = (CredentialProvider.UnderlyingCredentialProvider?.GetFieldDescriptorCount(out count) ?? HRESULT.S_OK);

            if (result != HRESULT.S_OK)
            {
                pcItems = 0;
                pdwSelectedItem = 0;

                return result;
            }

            if (UnderlyingCredential == null || (dwFieldID >= count))
            {
                pcItems = 0;
                pdwSelectedItem = 0;

                return HRESULT.E_NOTIMPL;
            }
            else
            {
                return UnderlyingCredential.GetComboBoxValueCount(dwFieldID, out pcItems, out pdwSelectedItem);
            }
        }

        /// <summary>
        /// Statutory method from <see cref="ICredentialProviderCredential"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-icredentialprovidercredential for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        public int GetComboBoxValueAt(uint dwFieldID, uint dwItem, out string ppszItem)
        {
            GlobalLogger.LogMethodCall();

            uint count = 0;

            int result = HRESULT.E_FAIL;

            result = (CredentialProvider.UnderlyingCredentialProvider?.GetFieldDescriptorCount(out count) ?? HRESULT.S_OK);

            if (result != HRESULT.S_OK)
            {
                ppszItem = string.Empty;

                return result;
            }

            if (UnderlyingCredential == null || (dwFieldID >= count))
            {
                ppszItem = string.Empty;

                return HRESULT.E_NOTIMPL;
            }
            else
            {
                return UnderlyingCredential.GetComboBoxValueAt(dwFieldID, dwItem, out ppszItem);
            }
        }

        /// <summary>
        /// Statutory method from <see cref="ICredentialProviderCredential"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-icredentialprovidercredential for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        public int SetStringValue(uint dwFieldID, string psz)
        {
            GlobalLogger.LogMethodCall();

            uint count = 0;

            int result = HRESULT.E_FAIL;

            result = (CredentialProvider.UnderlyingCredentialProvider?.GetFieldDescriptorCount(out count) ?? HRESULT.S_OK);

            if (result != HRESULT.S_OK)
            {
                return result;
            }

            if (UnderlyingCredential == null || (dwFieldID >= count))
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
                return UnderlyingCredential.SetStringValue(dwFieldID, psz);
            }
        }

        /// <summary>
        /// Statutory method from <see cref="ICredentialProviderCredential"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-icredentialprovidercredential for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        public int SetCheckboxValue(uint dwFieldID, int bChecked)
        {
            GlobalLogger.LogMethodCall();

            uint count = 0;

            int result = HRESULT.E_FAIL;

            result = (CredentialProvider.UnderlyingCredentialProvider?.GetFieldDescriptorCount(out count) ?? HRESULT.S_OK);

            if (result != HRESULT.S_OK)
            {
                return result;
            }

            if (UnderlyingCredential == null || (dwFieldID >= count))
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
                return UnderlyingCredential.SetCheckboxValue(dwFieldID, bChecked);
            }
        }

        /// <summary>
        /// Statutory method from <see cref="ICredentialProviderCredential"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-icredentialprovidercredential for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        public int SetComboBoxSelectedValue(uint dwFieldID, uint dwSelectedItem)
        {
            GlobalLogger.LogMethodCall();

            uint count = 0;

            int result = HRESULT.E_FAIL;

            result = (CredentialProvider.UnderlyingCredentialProvider?.GetFieldDescriptorCount(out count) ?? HRESULT.S_OK);

            if (result != HRESULT.S_OK)
            {
                return result;
            }

            if (UnderlyingCredential == null || (dwFieldID >= count))
            {
                return HRESULT.E_NOTIMPL;
            }
            else
            {
                return UnderlyingCredential.SetComboBoxSelectedValue(dwFieldID, dwSelectedItem);
            }
        }

        /// <summary>
        /// Statutory method from <see cref="ICredentialProviderCredential"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-icredentialprovidercredential for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        public int CommandLinkClicked(uint dwFieldID)
        {
            GlobalLogger.LogMethodCall();

            uint count = 0;

            int result = HRESULT.E_FAIL;

            result = (CredentialProvider.UnderlyingCredentialProvider?.GetFieldDescriptorCount(out count) ?? HRESULT.S_OK);

            if (result != HRESULT.S_OK)
            {
                return result;
            }

            if (UnderlyingCredential == null || (dwFieldID >= count))
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
                return UnderlyingCredential.CommandLinkClicked(dwFieldID);
            }
        }

        /// <summary>
        /// Statutory method from <see cref="ICredentialProviderCredential"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-icredentialprovidercredential for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        public int GetSerialization(out _CREDENTIAL_PROVIDER_GET_SERIALIZATION_RESPONSE pcpgsr, out _CREDENTIAL_PROVIDER_CREDENTIAL_SERIALIZATION pcpcs, out string ppszOptionalStatusText, out _CREDENTIAL_PROVIDER_STATUS_ICON pcpsiOptionalStatusIcon)
        {
            GlobalLogger.LogMethodCall();

            Logon?.Invoke(this, new LogonEventArgs());

            if (UnderlyingCredential == null)
            {
                try
                {
                    if (WindowsLogon == true)
                    {
                        if (this is IConnectableCredentialProviderCredential)
                        {
                            if (LogonResponse == null || LogonResponse?.Successful == false)
                            {
                                GlobalLogger.Log(LogLevels.Warning, "The logon response from the logon sequence pipeline was either null or unsuccessful.");

                                pcpgsr = _CREDENTIAL_PROVIDER_GET_SERIALIZATION_RESPONSE.CPGSR_NO_CREDENTIAL_NOT_FINISHED;
                                pcpcs = new _CREDENTIAL_PROVIDER_CREDENTIAL_SERIALIZATION() { ulAuthenticationPackage = 1234 };
                                ppszOptionalStatusText = (LogonResponse.ErrorMessage ?? "Invalid logon sequence.");
                                pcpsiOptionalStatusIcon = _CREDENTIAL_PROVIDER_STATUS_ICON.CPSI_ERROR;

                                return HRESULT.S_OK;
                            }
                        }
                        else
                        {
                            if (LogonSequencePipeline == null)
                            {
                                GlobalLogger.Log(LogLevels.Warning, "The logon sequence pipeline was null.");

                                pcpgsr = _CREDENTIAL_PROVIDER_GET_SERIALIZATION_RESPONSE.CPGSR_NO_CREDENTIAL_NOT_FINISHED;
                                pcpcs = new _CREDENTIAL_PROVIDER_CREDENTIAL_SERIALIZATION() { ulAuthenticationPackage = 1234 };
                                ppszOptionalStatusText = (LogonResponse.ErrorMessage ?? "Invalid logon sequence.");
                                pcpsiOptionalStatusIcon = _CREDENTIAL_PROVIDER_STATUS_ICON.CPSI_ERROR;

                                return HRESULT.S_OK;
                            }

                            LogonResponse = LogonSequencePipeline?.ProcessSequencePipeline(new LogonPackage(CredentialProvider.CurrentUsageScenario, this));

                            if (LogonResponse == null || LogonResponse?.Successful == false)
                            {
                                GlobalLogger.Log(LogLevels.Warning, "The logon response from the logon sequence pipeline was either null or unsuccessful.");

                                pcpgsr = _CREDENTIAL_PROVIDER_GET_SERIALIZATION_RESPONSE.CPGSR_NO_CREDENTIAL_NOT_FINISHED;
                                pcpcs = new _CREDENTIAL_PROVIDER_CREDENTIAL_SERIALIZATION() { ulAuthenticationPackage = 1234 };
                                ppszOptionalStatusText = (LogonResponse.ErrorMessage ?? "Invalid logon sequence.");
                                pcpsiOptionalStatusIcon = _CREDENTIAL_PROVIDER_STATUS_ICON.CPSI_ERROR;

                                return HRESULT.S_OK;
                            }
                        }

                        WindowsLogonPackage windowsLogonPackage = LogonResponse.WindowsLogonPackage;

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
                catch (Exception exception)
                {
                    GlobalLogger.Log(LogLevels.Warning, exception.Message, exception);
                }

                pcpgsr = _CREDENTIAL_PROVIDER_GET_SERIALIZATION_RESPONSE.CPGSR_NO_CREDENTIAL_NOT_FINISHED;
                pcpcs = new _CREDENTIAL_PROVIDER_CREDENTIAL_SERIALIZATION() { ulAuthenticationPackage = 1234 };
                ppszOptionalStatusText = string.Empty;
                pcpsiOptionalStatusIcon = _CREDENTIAL_PROVIDER_STATUS_ICON.CPSI_SUCCESS;

                return HRESULT.S_OK;
            }
            else
            {
                return UnderlyingCredential.GetSerialization(out pcpgsr, out pcpcs, out ppszOptionalStatusText, out pcpsiOptionalStatusIcon);
            }
        }

        /// <summary>
        /// Statutory method from <see cref="ICredentialProviderCredential"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-icredentialprovidercredential for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        public int ReportResult(int ntsStatus, int ntsSubstatus, out string ppszOptionalStatusText, out _CREDENTIAL_PROVIDER_STATUS_ICON pcpsiOptionalStatusIcon)
        {
            GlobalLogger.LogMethodCall();

            if (UnderlyingCredential == null)
            {
                ppszOptionalStatusText = string.Empty;
                pcpsiOptionalStatusIcon = _CREDENTIAL_PROVIDER_STATUS_ICON.CPSI_NONE;

                return HRESULT.S_OK;
            }
            else
            {
                return UnderlyingCredential.ReportResult(ntsStatus, ntsSubstatus, out ppszOptionalStatusText, out pcpsiOptionalStatusIcon);
            }
        }

        #endregion

        #region ICredentialProviderCredential2

        /// <summary>
        /// Statutory method from <see cref="ICredentialProviderCredential2"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-icredentialprovidercredential2 for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        public int GetUserSid(out string sid)
        {
            GlobalLogger.LogMethodCall();

            if (UnderlyingCredential != null)
            {
                if (UnderlyingCredential is ICredentialProviderCredential2)
                {
                    int result = HRESULT.E_FAIL;

                    result = ((ICredentialProviderCredential2)UnderlyingCredential).GetUserSid(out sid);

                    if (result != HRESULT.S_OK)
                    {
                        sid = string.Empty;

                        return result;
                    }
                }
                else
                {
                    sid = string.Empty;
                }
            }
            else
            {
                sid = string.Empty;
            }

            return HRESULT.S_OK;
        }

        #endregion

        #region IConnectableCredentialProviderCredential

        /// <summary>
        /// Statutory method from <see cref="IConnectableCredentialProviderCredential"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-iconnectablecredentialprovidercredential for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        public int Connect(IQueryContinueWithStatus pqcws)
        {
            GlobalLogger.LogMethodCall();

            Connecting?.Invoke(this, new ConnectingEventArgs(pqcws));

            if (UnderlyingCredential != null)
            {
                if (UnderlyingCredential is IConnectableCredentialProviderCredential)
                {
                    int result = ((IConnectableCredentialProviderCredential)UnderlyingCredential).Connect(pqcws);

                    if (result != HRESULT.S_OK)
                    {
                        return result;
                    }
                }
            }

            Connected?.Invoke(this, new ConnectedEventArgs(pqcws));

            return HRESULT.S_OK;
        }

        /// <summary>
        /// Statutory method from <see cref="IConnectableCredentialProviderCredential"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-iconnectablecredentialprovidercredential for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        public int Disconnect()
        {
            GlobalLogger.LogMethodCall();

            Disconnecting?.Invoke(this, new EventArgs());

            if (UnderlyingCredential != null)
            {
                if (UnderlyingCredential is IConnectableCredentialProviderCredential)
                {
                    int result = HRESULT.E_FAIL;

                    result = ((IConnectableCredentialProviderCredential)UnderlyingCredential).Disconnect();

                    if (result != HRESULT.S_OK)
                    {
                        return result;
                    }
                }
            }

            Disconnected?.Invoke(this, new EventArgs());

            return HRESULT.S_OK;
        }

        #endregion

        #endregion

        #endregion

        #region Events

        /// <summary>
        /// This event is raised after this credential is selected.
        /// </summary>
        public event EventHandler<LoadedEventArgs> Loaded;

        /// <summary>
        /// This event is raised before any connect operation is started when the user submits the credentials (only pertinent to when this credential is wrapping another credential). This credential will need to implement the <see cref="IConnectableCredential"/> interface.
        /// </summary>
        public event EventHandler<ConnectingEventArgs> Connecting;

        /// <summary>
        /// This event is raised after any connect operation is started when the user submits the credentials (only pertinent to when this credential is wrapping another credential). This credential will need to implement the <see cref="IConnectableCredential"/> interface. When this credential is not wrapping another credential, this event should not be used, although will still be raised. Instead, you should use the <see cref="Connecting"/> event.
        /// </summary>
        public event EventHandler<ConnectedEventArgs> Connected;

        /// <summary>
        /// This event is raised before any disconnect operation is started (only pertinent to when this credential is wrapping another credential). This credential will need to implement the <see cref="IConnectableCredential"/> interface.
        /// </summary>
        public event EventHandler Disconnecting;

        /// <summary>
        /// This event is raised after any disconnect operation is started (only pertinent to when this credential is wrapping another credential). This credential will need to implement the <see cref="IConnectableCredential"/> interface. When this credential is not wrapping another credential, this event should not be used, although will still be raised. Instead, you should use the <see cref="Disconnecting"/> event.
        /// </summary>
        public event EventHandler Disconnected;

        /// <summary>
        /// This event is raised after any connect operation when the user submits the credentials.
        /// </summary>
        public event EventHandler<LogonEventArgs> Logon;

        #endregion
    }
}