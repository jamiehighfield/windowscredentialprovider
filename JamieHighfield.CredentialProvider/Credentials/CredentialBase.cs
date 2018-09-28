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
using JamieHighfield.CredentialProvider.Interfaces;
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
    public abstract class CredentialBase : ICredentialProviderCredential, ICurrentEnvironment
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

        #region ICurrentEnvironment

        /// <summary>
        /// Gets the <see cref="CredentialProviderUsageScenarios"/> that this credential provider will support when requested by Windows.
        /// </summary>
        public CredentialProviderUsageScenarios SupportedUsageScenarios => CredentialProvider.SupportedUsageScenarios;

        /// <summary>
        /// Gets the <see cref="CredentialProviderUsageScenarios"/> representing the current scenario under which the credential provider is operating.
        /// </summary>
        public CredentialProviderUsageScenarios CurrentUsageScenario => CredentialProvider.CurrentUsageScenario;

        /// <summary>
        /// Gets the handle of the main window for the parent process.
        /// </summary>
        public WindowHandle MainWindowHandle => CredentialProvider.MainWindowHandle;

        #endregion

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
        internal LogonSequencePipelineBase LogonSequencePipeline { get; private set; }
        
        /// <summary>
        /// Gets whether the connect operation was cancelled or cancelled by the user clicking the 'Cancel' button.
        /// </summary>
        private bool ConnectCancelled { get; set; }

        /// <summary>
        /// Gets the error message to be displayed if the connect operation was cancelled.
        /// </summary>
        private string ConnectCancelledMessage { get; set; }

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
        
        #endregion

        #region Methods

        /// <summary>
        /// This method is called to get a <see cref="Logon.LogonResponse"/> containing a logon package that can be used to logon to Windows. By default, this method runs the <see cref="Logon.LogonSequencePipeline"/> within this credential and returns the <see cref="Logon.LogonResponse"/> from that.This method will be ignored if an underlying credential has been specified. This method can be overridden to provide different behaviour. 
        /// </summary>
        /// <returns></returns>
        public virtual LogonResponse ProcessLogon()
        {
            if (LogonSequencePipeline == null)
            {
                GlobalLogger.Log(LogLevels.Warning, "The default behaviour of this credential provider is to process a logon sequence pipeline. The provided logon sequence pipeline was either null or in the incorrect format.");

                return new LogonResponse(LogonResponseErrorTypes.Error, "The default behaviour of this credential provider is to process a logon sequence pipeline. The provided logon sequence pipeline was either null or in the incorrect format. Please contact your system administrator.");
            }
            else
            {
                LogonResponse logonResponse = LogonSequencePipeline.ProcessSequencePipeline(new LogonPackage(this));

                if (logonResponse == null || logonResponse?.ErrorMessage != null)
                {
                    GlobalLogger.Log(LogLevels.Warning, "The default behaviour of this credential provider is to process a logon sequence pipeline. The provided logon sequence pipeline returned either null or an object in the incorrect format."
                        + Environment.NewLine
                        + Environment.NewLine
                        + "The provided error message was:"
                        + Environment.NewLine
                        + Environment.NewLine
                        + logonResponse.ErrorMessage);
                }

                return logonResponse;
            }
        }

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

        public string GetNativeStringValue(int fieldId)
        {
            int result = UnderlyingCredential.GetStringValue((uint)fieldId, out string value);

            if (result != HRESULT.S_OK)
            {
                return string.Empty;
            }

            return value;
        }

        #region Credential Provider Interface Methods

        #region ICredentialProviderCredential

        /// <summary>
        /// Statutory method from <see cref="ICredentialProviderCredential"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-icredentialprovidercredential for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        int ICredentialProviderCredential.Advise(ICredentialProviderCredentialEvents pcpce)
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
        int ICredentialProviderCredential.UnAdvise()
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
        int ICredentialProviderCredential.SetSelected(out int pbAutoLogon)
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

            CredentialProvider.CurrentCredential = this;

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
        int ICredentialProviderCredential.SetDeselected()
        {
            GlobalLogger.LogMethodCall();

            int result;

            if (UnderlyingCredential == null)
            {
                result = HRESULT.E_NOTIMPL;
            }
            else
            {
                result = UnderlyingCredential.SetDeselected();
            }

            CredentialProvider.CurrentCredential = null;

            return result;
        }

        /// <summary>
        /// Statutory method from <see cref="ICredentialProviderCredential"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-icredentialprovidercredential for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        int ICredentialProviderCredential.GetFieldState(uint dwFieldID, out _CREDENTIAL_PROVIDER_FIELD_STATE pcpfs, out _CREDENTIAL_PROVIDER_FIELD_INTERACTIVE_STATE pcpfis)
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
        int ICredentialProviderCredential.GetStringValue(uint dwFieldID, out string ppsz)
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
        int ICredentialProviderCredential.GetBitmapValue(uint dwFieldID, out IntPtr phbmp)
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
        int ICredentialProviderCredential.GetCheckboxValue(uint dwFieldID, out int pbChecked, out string ppszLabel)
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
        int ICredentialProviderCredential.GetSubmitButtonValue(uint dwFieldID, out uint pdwAdjacentTo)
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
        int ICredentialProviderCredential.GetComboBoxValueCount(uint dwFieldID, out uint pcItems, out uint pdwSelectedItem)
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
        int ICredentialProviderCredential.GetComboBoxValueAt(uint dwFieldID, uint dwItem, out string ppszItem)
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
        int ICredentialProviderCredential.SetStringValue(uint dwFieldID, string psz)
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
        int ICredentialProviderCredential.SetCheckboxValue(uint dwFieldID, int bChecked)
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
        int ICredentialProviderCredential.SetComboBoxSelectedValue(uint dwFieldID, uint dwSelectedItem)
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
        int ICredentialProviderCredential.CommandLinkClicked(uint dwFieldID)
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
        int ICredentialProviderCredential.GetSerialization(out _CREDENTIAL_PROVIDER_GET_SERIALIZATION_RESPONSE pcpgsr, out _CREDENTIAL_PROVIDER_CREDENTIAL_SERIALIZATION pcpcs, out string ppszOptionalStatusText, out _CREDENTIAL_PROVIDER_STATUS_ICON pcpsiOptionalStatusIcon)
        {
            GlobalLogger.LogMethodCall();

            Logon?.Invoke(this, new LogonEventArgs());

            if (UnderlyingCredential == null)
            {
                try
                {
                    LogonResponse logonResponse = ProcessLogon();

                    if (logonResponse == null || logonResponse?.ErrorMessage != null)
                    {
                        pcpgsr = _CREDENTIAL_PROVIDER_GET_SERIALIZATION_RESPONSE.CPGSR_NO_CREDENTIAL_NOT_FINISHED;
                        pcpcs = new _CREDENTIAL_PROVIDER_CREDENTIAL_SERIALIZATION() { ulAuthenticationPackage = 1234 };
                        ppszOptionalStatusText = (logonResponse?.ErrorMessage ?? "An unknown error occurred. Please contact your system administrator.");
                        pcpsiOptionalStatusIcon = _CREDENTIAL_PROVIDER_STATUS_ICON.CPSI_ERROR;

                        return HRESULT.S_OK;
                    }

                    if (WindowsLogon == true)
                    {
                        WindowsLogonPackage windowsLogonPackage = logonResponse.WindowsLogonPackage;

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

                                pcpcs.clsidCredentialProvider = CredentialProvider.ComGuid;
                                pcpcs.rgbSerialization = credentialBuffer;
                                pcpcs.cbSerialization = (uint)credentialSize;

                                NegotiateAuthentication(out uint negotiateAuthenticationPackage);
                                pcpcs.ulAuthenticationPackage = negotiateAuthenticationPackage;

                                return HRESULT.S_OK;
                            }

                            pcpgsr = _CREDENTIAL_PROVIDER_GET_SERIALIZATION_RESPONSE.CPGSR_NO_CREDENTIAL_NOT_FINISHED;
                            pcpcs = new _CREDENTIAL_PROVIDER_CREDENTIAL_SERIALIZATION() { ulAuthenticationPackage = 1234 };
                            ppszOptionalStatusText = "An unknown error occurred. Please contact your system administrator.";
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
        int ICredentialProviderCredential.ReportResult(int ntsStatus, int ntsSubstatus, out string ppszOptionalStatusText, out _CREDENTIAL_PROVIDER_STATUS_ICON pcpsiOptionalStatusIcon)
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

        #endregion

        #endregion

        #region Events

        /// <summary>
        /// This event is raised after this credential is selected.
        /// </summary>
        public event EventHandler<LoadedEventArgs> Loaded;

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