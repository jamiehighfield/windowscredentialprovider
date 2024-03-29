﻿/* COPYRIGHT NOTICE
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
using JamieHighfield.CredentialProvider.Controls.New;
using JamieHighfield.CredentialProvider.Credentials.Interfaces;
using JamieHighfield.CredentialProvider.Interfaces;
using JamieHighfield.CredentialProvider.Interop;
using JamieHighfield.CredentialProvider.Logging;
using JamieHighfield.CredentialProvider.Logon;
using JamieHighfield.CredentialProvider.Providers;
using JamieHighfield.CredentialProvider.Providers.Interfaces;
using JamieHighfield.CredentialProvider.WindowsAuthentication;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        protected CredentialBase()
        {
            Controls = new List<CredentialControlBase>();
            Fields = new CredentialFieldCollection();
        }

        /// <summary>
        /// Instantiate a new <see cref="CredentialBase"/> object.
        /// </summary>
        /// <param name="logonSequencePipeline">The <see cref="LogonSequencePipelineBase"/> instance used during the logon sequence.</param>
        protected CredentialBase(LogonSequencePipelineBase logonSequencePipeline)
        {
            LogonSequencePipeline = logonSequencePipeline;

            Controls = new List<CredentialControlBase>();
            Fields = new CredentialFieldCollection();
        }

        #region Variables



        #endregion

        #region Properties

        #region ICurrentEnvironment

        /// <summary>
        /// Gets the <see cref="CredentialProviderUsageScenarios"/> that this credential provider will support when requested by Windows.
        /// </summary>
        public CredentialProviderUsageScenarios SupportedUsageScenarios => ManagedCredentialProvider.SupportedUsageScenarios;

        /// <summary>
        /// Gets the <see cref="CredentialProviderUsageScenarios"/> representing the current scenario under which the credential provider is operating.
        /// </summary>
        public CredentialProviderUsageScenarios CurrentUsageScenario => ManagedCredentialProvider.CurrentUsageScenario;

        /// <summary>
        /// Gets the handle of the main window for the parent process.
        /// </summary>
        public WindowHandle MainWindowHandle => new WindowHandle(Process.GetCurrentProcess().MainWindowHandle);

        #endregion

        /// <summary>
        /// Gets or sets the <see cref="CredentialProviderBase"/> that enumerates this credential.
        /// </summary>
        public ManagedCredentialProvider ManagedCredentialProvider { get; internal set; }

        /// <summary>
        /// Gets or sets the <see cref="ICredentialProviderCredential"/> that underlies this <see cref="CredentialBase"/> (only pertinent to when this credential is wrapping another credential).
        /// </summary>
        internal ICredentialProviderCredential UnderlyingCredential { get; set; }

        /// <summary>
        /// Gets a read-only collection of <see cref="CredentialControlBase"/>. The <see cref="CredentialControlBase"/> enumerated here are specific to this credential, and any changes here won't be reflected in any other credentials.
        /// </summary>
        public List<CredentialControlBase> Controls { get; internal set; }

        /// <summary>
        /// Gets the <see cref="LogonSequencePipelineBase"/> used during the logon sequence to authenticate, authorise and manipulate incoming credentials.
        /// </summary>
        public LogonSequencePipelineBase LogonSequencePipeline { get; }
        
        /// <summary>
        /// Gets or sets whether this credential should forward an authentication package to Windows. This property will be ignored if this credential has an underlying credential.
        /// </summary>
        public bool WindowsLogon { get; set; }
        
        /// <summary>
        /// Gets or sets the <see cref="EventsWrapper"/> that is used to wrap an <see cref="ICredentialProviderCredentialEvents"/>. This allows for the correct enumerated credential to be used when raising events (only pertinent to when this credential is wrapping another credential).
        /// </summary>
        internal EventsWrapper Events { get; private set; }

        /// <summary>
        /// Gets a <see cref="CredentialFieldCollection"/> of <see cref="CredentialField"/> representing the raw field data with the associated control. This will only include managed fields; those from wrapped credentials will not be included, although, the field ID will appear as though all fields are included.
        /// </summary>
        internal CredentialFieldCollection Fields { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Logon.LogonResponse"/> which is returned from the output of running the <see cref="LogonSequencePipeline"/>. This property can be used as a link between the <see cref="ProcessConnection(Connection)"/> and <see cref="ProcessLogon"/> methods.
        /// </summary>
        public LogonResponse LogonResponse { get; set; }

        /// <summary>
        /// Gets or sets the delegate that will be run on connection.
        /// </summary>
        public Action<ICurrentEnvironment, Connection> ConnectionFactory { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Credentials.Connection"/> which is used during the credential's connection phase.
        /// </summary>
        private Connection Connection { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// This method is called during the initialisation of the credential and should be used to manipulate <see cref="CredentialControlBase"/> objects parsed from the <see cref="CredentialProviderBase"/>.
        /// </summary>
        public virtual void Initialise() { }

        /// <summary>
        /// This method is called after this credential is selected.
        /// </summary>
        public virtual void Loaded() { }

        /// <summary>
        /// This method is called before the <see cref="ProcessLogon"/> method is called when a logon is initiated. If <see cref="WindowsLogon"/> is set to true, <see cref="ProcessLogon"/> will not be called; instead, this is the only method that will be called.
        /// </summary>
        public virtual void BeforeLogon() { }

        internal virtual ResultMessageInformation ResultMessage()
        {
            if (this is IConnectableCredential)
            {
                if (Connection != null)
                {
                    if (Connection.Cancelled == true)
                    {
                        return new ResultMessageInformation(Connection.ErrorMessage, Connection.Icon);
                    }
                }
            }

            return null;
        }

        public void InvokeSubmit()
        {

        }

        /// <summary>
        /// This method is called to get a <see cref="LogonResponse"/> containing a logon package that can be used to logon to Windows. By default, this method runs the <see cref="Logon.LogonSequencePipeline"/> within this credential and returns the <see cref="LogonResponse"/> from that. This method will be ignored if an underlying credential has been specified or <see cref="WindowsLogon"/> is set to true. This method can be overridden to provide different behaviour.
        /// </summary>
        /// <returns>The <see cref="LogonResponse"/> to be parsed to Windows for authentication.</returns>
        public virtual LogonResponse ProcessLogon()
        {
            if (WindowsLogon == true)
            {
                if (LogonSequencePipeline == null)
                {
                    GlobalLogger.Log(LogLevels.Warning, "The default behaviour of this credential provider is to process a logon sequence pipeline. The provided logon sequence pipeline was either null or in the incorrect format.");

                    return new LogonResponse(ErrorMessageIcons.Error, "The default behaviour of this credential provider is to process a logon sequence pipeline. The provided logon sequence pipeline was either null or in the incorrect format. Please contact your system administrator.");
                }

                LogonResponse logonResponse = LogonSequencePipeline.ProcessSequencePipeline(new IncomingLogonPackage(this, this));

                GlobalLogger.Log(LogLevels.Information, "HERE");

                if (logonResponse == null || logonResponse.ErrorMessage != null)
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

            return null;
        }

        #region Credential Provider Interface Methods

        #region ICredentialProviderCredential

        /// <summary>
        /// Statutory method from <see cref="ICredentialProviderCredential"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-icredentialprovidercredential for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public int Advise(ICredentialProviderCredentialEvents pcpce)
        {
            GlobalLogger.LogMethodCall();

            if (pcpce != null)
            {
                if (pcpce is ICredentialProviderCredentialEvents2 extendedCredentialProviderEvents)
                {
                    Events = new ExtendedEventsWrapper(this, extendedCredentialProviderEvents);
                }
                else
                {
                    Events = new EventsWrapper(this, pcpce);
                }

                Marshal.AddRef(Marshal.GetIUnknownForObject(pcpce));

                int result = Events.OnCreatingWindow(out _RemotableHandle remotableHandle);

                if (result != HRESULT.S_OK)
                {
                    return result;
                }

                IntPtr handle = Marshal.AllocHGlobal(Marshal.SizeOf(remotableHandle));

                Marshal.StructureToPtr(remotableHandle, handle, false);

                //MainWindowHandle = new WindowHandle(handle);
            }

            if (UnderlyingCredential != null)
            {
                int result = UnderlyingCredential.Advise(Events);

                if (result != HRESULT.S_OK)
                {
                    return result;
                }
            }

            return HRESULT.S_OK;
        }

        /// <summary>
        /// Statutory method from <see cref="ICredentialProviderCredential"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-icredentialprovidercredential for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
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

            return UnderlyingCredential.UnAdvise();
        }

        /// <summary>
        /// Statutory method from <see cref="ICredentialProviderCredential"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-icredentialprovidercredential for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
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

            ManagedCredentialProvider.CurrentCredential = this;

            pbAutoLogon = 0;

            Loaded();

            return HRESULT.S_OK;
        }

        /// <summary>
        /// Statutory method from <see cref="ICredentialProviderCredential"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-icredentialprovidercredential for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public int SetDeselected()
        {
            GlobalLogger.LogMethodCall();

            int result = UnderlyingCredential?.SetDeselected() ?? HRESULT.E_NOTIMPL;

            ManagedCredentialProvider.CurrentCredential = null;

            return result;
        }

        /// <summary>
        /// Statutory method from <see cref="ICredentialProviderCredential"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-icredentialprovidercredential for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public int GetFieldState(uint dwFieldID, out _CREDENTIAL_PROVIDER_FIELD_STATE pcpfs, out _CREDENTIAL_PROVIDER_FIELD_INTERACTIVE_STATE pcpfis)
        {
            GlobalLogger.LogMethodCall();

            uint count = 0;

            if (dwFieldID == 25)
            {

            }

            int result = ManagedCredentialProvider.UnderlyingCredentialProvider?.GetFieldDescriptorCount(out count) ?? HRESULT.S_OK;

            if (result != HRESULT.S_OK)
            {
                pcpfs = _CREDENTIAL_PROVIDER_FIELD_STATE.CPFS_HIDDEN;
                pcpfis = _CREDENTIAL_PROVIDER_FIELD_INTERACTIVE_STATE.CPFIS_NONE;

                return result;
            }

            if (UnderlyingCredential == null || dwFieldID >= count)
            {
                if (dwFieldID - count >= Fields.Count)
                {
                    pcpfs = _CREDENTIAL_PROVIDER_FIELD_STATE.CPFS_HIDDEN;
                    pcpfis = _CREDENTIAL_PROVIDER_FIELD_INTERACTIVE_STATE.CPFIS_NONE;

                    return HRESULT.E_INVALIDARG;
                }

                CredentialField identifiedField = Fields[(int)dwFieldID];

                pcpfs = identifiedField.FieldState;

                if (identifiedField.Control is TextBoxControl)
                {
                    if (((TextBoxControl)identifiedField.Control).Focussed == true)
                    {
                        pcpfis = _CREDENTIAL_PROVIDER_FIELD_INTERACTIVE_STATE.CPFIS_FOCUSED;
                    }
                    else
                    {
                        pcpfis = _CREDENTIAL_PROVIDER_FIELD_INTERACTIVE_STATE.CPFIS_NONE;
                    }
                }
                else
                {
                    pcpfis = _CREDENTIAL_PROVIDER_FIELD_INTERACTIVE_STATE.CPFIS_NONE;
                }

                return HRESULT.S_OK;
            }

            return UnderlyingCredential.GetFieldState(dwFieldID, out pcpfs, out pcpfis);
        }

        /// <summary>
        /// Statutory method from <see cref="ICredentialProviderCredential"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-icredentialprovidercredential for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public int GetStringValue(uint dwFieldID, out string ppsz)
        {
            GlobalLogger.LogMethodCall();

            uint count = 0;

            int result = (ManagedCredentialProvider.UnderlyingCredentialProvider?.GetFieldDescriptorCount(out count) ?? HRESULT.S_OK);

            if (result != HRESULT.S_OK)
            {
                ppsz = string.Empty;

                return result;
            }

            if (UnderlyingCredential == null || dwFieldID >= count)
            {
                if (dwFieldID - count >= Fields.Count)
                {
                    ppsz = string.Empty;

                    return HRESULT.E_INVALIDARG;
                }

                CredentialField identifiedField = Fields[(int)dwFieldID];

                if (identifiedField.Control is LabelControl labelControl)
                {
                    ppsz = labelControl.Text;
                }
                else if (identifiedField.Control is TextBoxControl textBoxControl)
                {
                    ppsz = textBoxControl.Text;
                }
                else if (identifiedField.Control is LinkControl linkControl)
                {
                    ppsz = linkControl.Text;
                }
                else
                {
                    ppsz = string.Empty;

                    return HRESULT.E_INVALIDARG;
                }

                return HRESULT.S_OK;
            }

            return UnderlyingCredential.GetStringValue(dwFieldID, out ppsz);
        }

        /// <summary>
        /// Statutory method from <see cref="ICredentialProviderCredential"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-icredentialprovidercredential for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public int GetBitmapValue(uint dwFieldID, out IntPtr phbmp)
        {
            GlobalLogger.LogMethodCall();

            uint count = 0;

            int result = ManagedCredentialProvider.UnderlyingCredentialProvider?.GetFieldDescriptorCount(out count) ?? HRESULT.S_OK;

            if (result != HRESULT.S_OK)
            {
                phbmp = IntPtr.Zero;

                return result;
            }

            if (UnderlyingCredential == null || dwFieldID >= count)
            {
                if (dwFieldID - count >= Fields.Count)
                {
                    phbmp = IntPtr.Zero;

                    return HRESULT.E_INVALIDARG;
                }

                CredentialField identifiedField = Fields[(int)dwFieldID];

                if (identifiedField.Control is ImageControl imageControl)
                {
                    phbmp = imageControl.Image.GetHbitmap();
                }
                else
                {
                    phbmp = IntPtr.Zero;

                    return HRESULT.E_INVALIDARG;
                }

                return HRESULT.S_OK;
            }

            return UnderlyingCredential.GetBitmapValue(dwFieldID, out phbmp);
        }

        /// <summary>
        /// Statutory method from <see cref="ICredentialProviderCredential"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-icredentialprovidercredential for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public int GetCheckboxValue(uint dwFieldID, out int pbChecked, out string ppszLabel)
        {
            GlobalLogger.LogMethodCall();

            uint count = 0;

            int result = ManagedCredentialProvider.UnderlyingCredentialProvider?.GetFieldDescriptorCount(out count) ?? HRESULT.S_OK;

            if (result != HRESULT.S_OK)
            {
                pbChecked = 0;
                ppszLabel = string.Empty;

                return result;
            }

            if (UnderlyingCredential == null || (dwFieldID >= count))
            {
                if (dwFieldID - count >= Fields.Count)
                {
                    pbChecked = 0;
                    ppszLabel = string.Empty;

                    return HRESULT.E_INVALIDARG;
                }

                CredentialField identifiedField = Fields[(int)dwFieldID];

                if (identifiedField.Control is CheckBoxControl checkBoxControl)
                {
                    pbChecked = checkBoxControl.Checked ? 1 : 0;
                    ppszLabel = checkBoxControl.Label;
                }
                else
                {
                    pbChecked = 0;
                    ppszLabel = string.Empty;

                    return HRESULT.E_INVALIDARG;
                }

                return HRESULT.S_OK;
            }

            return UnderlyingCredential.GetCheckboxValue(dwFieldID, out pbChecked, out ppszLabel);
        }

        /// <summary>
        /// Statutory method from <see cref="ICredentialProviderCredential"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-icredentialprovidercredential for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public int GetSubmitButtonValue(uint dwFieldID, out uint pdwAdjacentTo)
        {
            GlobalLogger.LogMethodCall();

            uint count = 0;

            int result = ManagedCredentialProvider.UnderlyingCredentialProvider?.GetFieldDescriptorCount(out count) ?? HRESULT.S_OK;

            if (result != HRESULT.S_OK)
            {
                pdwAdjacentTo = 0;

                return result;
            }

            if (UnderlyingCredential == null || (dwFieldID >= count))
            {
                if (dwFieldID - count >= Fields.Count)
                {
                    pdwAdjacentTo = 0;

                    return HRESULT.E_INVALIDARG;
                }

                CredentialField identifiedField = Fields[(int)dwFieldID];

                if (identifiedField.Control is ButtonControl buttonControl)
                {
                    CredentialField adjacentField = Fields
                        .FirstOrDefault(field => field.Control == buttonControl.AdjacentControl);

                    if (adjacentField == null)
                    {
                        pdwAdjacentTo = 0;

                        return HRESULT.E_INVALIDARG;
                    }

                    pdwAdjacentTo = (uint)adjacentField.FieldId;
                }
                else
                {
                    pdwAdjacentTo = 2;

                    return HRESULT.E_INVALIDARG;
                }

                return HRESULT.S_OK;
            }

            return UnderlyingCredential.GetSubmitButtonValue(dwFieldID, out pdwAdjacentTo);
        }

        /// <summary>
        /// Statutory method from <see cref="ICredentialProviderCredential"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-icredentialprovidercredential for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public int GetComboBoxValueCount(uint dwFieldID, out uint pcItems, out uint pdwSelectedItem)
        {
            GlobalLogger.LogMethodCall();

            uint count = 0;

            int result = ManagedCredentialProvider.UnderlyingCredentialProvider?.GetFieldDescriptorCount(out count) ?? HRESULT.S_OK;

            if (result != HRESULT.S_OK)
            {
                pcItems = 0;
                pdwSelectedItem = 0;

                return result;
            }

            if (UnderlyingCredential == null || dwFieldID >= count)
            {
                pcItems = 0;
                pdwSelectedItem = 0;

                return HRESULT.E_NOTIMPL;
            }

            return UnderlyingCredential.GetComboBoxValueCount(dwFieldID, out pcItems, out pdwSelectedItem);
        }

        /// <summary>
        /// Statutory method from <see cref="ICredentialProviderCredential"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-icredentialprovidercredential for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public int GetComboBoxValueAt(uint dwFieldID, uint dwItem, out string ppszItem)
        {
            GlobalLogger.LogMethodCall();

            uint count = 0;

            int result = ManagedCredentialProvider.UnderlyingCredentialProvider?.GetFieldDescriptorCount(out count) ?? HRESULT.S_OK;

            if (result != HRESULT.S_OK)
            {
                ppszItem = string.Empty;

                return result;
            }

            if (UnderlyingCredential == null || dwFieldID >= count)
            {
                ppszItem = string.Empty;

                return HRESULT.E_NOTIMPL;
            }

            return UnderlyingCredential.GetComboBoxValueAt(dwFieldID, dwItem, out ppszItem);
        }

        /// <summary>
        /// Statutory method from <see cref="ICredentialProviderCredential"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-icredentialprovidercredential for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public int SetStringValue(uint dwFieldID, string psz)
        {
            GlobalLogger.LogMethodCall();

            uint count = 0;

            int result = ManagedCredentialProvider.UnderlyingCredentialProvider?.GetFieldDescriptorCount(out count) ?? HRESULT.S_OK;

            if (result != HRESULT.S_OK)
            {
                return result;
            }

            if (UnderlyingCredential == null || dwFieldID >= count)
            {
                if (dwFieldID - count >= Fields.Count)
                {
                    return HRESULT.E_INVALIDARG;
                }

                CredentialField identifiedField = Fields[(int)dwFieldID];

                if (identifiedField.Control is LabelControl labelControl)
                {
                    //labelControl.UpdateText(psz);
                }
                else if (identifiedField.Control is TextBoxControl textBoxControl)
                {
                    textBoxControl.UpdateText(psz);
                }
                else
                {
                    return HRESULT.E_INVALIDARG;
                }

                return HRESULT.S_OK;
            }

            return UnderlyingCredential.SetStringValue(dwFieldID, psz);
        }

        /// <summary>
        /// Statutory method from <see cref="ICredentialProviderCredential"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-icredentialprovidercredential for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public int SetCheckboxValue(uint dwFieldID, int bChecked)
        {
            GlobalLogger.LogMethodCall();

            uint count = 0;

            int result = ManagedCredentialProvider.UnderlyingCredentialProvider?.GetFieldDescriptorCount(out count) ?? HRESULT.S_OK;

            if (result != HRESULT.S_OK)
            {
                return result;
            }

            if (UnderlyingCredential == null || (dwFieldID >= count))
            {
                if (dwFieldID - count >= Fields.Count)
                {
                    return HRESULT.E_INVALIDARG;
                }

                CredentialField identifiedField = Fields[(int)dwFieldID];

                if (identifiedField.Control is CheckBoxControl checkBoxControl)
                {
                    checkBoxControl.Checked = bChecked == 1;
                }
                else
                {
                    return HRESULT.E_INVALIDARG;
                }

                return HRESULT.S_OK;
            }

            return UnderlyingCredential.SetCheckboxValue(dwFieldID, bChecked);
        }

        /// <summary>
        /// Statutory method from <see cref="ICredentialProviderCredential"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-icredentialprovidercredential for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public int SetComboBoxSelectedValue(uint dwFieldID, uint dwSelectedItem)
        {
            GlobalLogger.LogMethodCall();

            uint count = 0;

            int result = ManagedCredentialProvider.UnderlyingCredentialProvider?.GetFieldDescriptorCount(out count) ?? HRESULT.S_OK;

            if (result != HRESULT.S_OK)
            {
                return result;
            }

            if (UnderlyingCredential == null || dwFieldID >= count)
            {
                return HRESULT.E_NOTIMPL;
            }

            return UnderlyingCredential.SetComboBoxSelectedValue(dwFieldID, dwSelectedItem);
        }

        /// <summary>
        /// Statutory method from <see cref="ICredentialProviderCredential"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-icredentialprovidercredential for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public int CommandLinkClicked(uint dwFieldID)
        {
            GlobalLogger.LogMethodCall();

            uint count = 0;

            int result = ManagedCredentialProvider.UnderlyingCredentialProvider?.GetFieldDescriptorCount(out count) ?? HRESULT.S_OK;

            if (result != HRESULT.S_OK)
            {
                return result;
            }

            if (UnderlyingCredential == null || dwFieldID >= count)
            {
                if (dwFieldID - count >= Fields.Count)
                {
                    return HRESULT.E_INVALIDARG;
                }

                CredentialField identifiedField = Fields[(int)dwFieldID];

                if (identifiedField.Control is LinkControl linkCcontrol)
                {
                    linkCcontrol.Click?.Invoke(this, new EventArgs());
                }
                else
                {
                    return HRESULT.E_INVALIDARG;
                }

                return HRESULT.S_OK;
            }

            return UnderlyingCredential.CommandLinkClicked(dwFieldID);
        }

        /// <summary>
        /// Statutory method from <see cref="ICredentialProviderCredential"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-icredentialprovidercredential for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public int GetSerialization(out _CREDENTIAL_PROVIDER_GET_SERIALIZATION_RESPONSE pcpgsr, out _CREDENTIAL_PROVIDER_CREDENTIAL_SERIALIZATION pcpcs, out string ppszOptionalStatusText, out _CREDENTIAL_PROVIDER_STATUS_ICON pcpsiOptionalStatusIcon)
        {
            GlobalLogger.LogMethodCall();

            ResultMessageInformation resultMessageInformation = ResultMessage();

            if (resultMessageInformation != null)
            {
                pcpgsr = _CREDENTIAL_PROVIDER_GET_SERIALIZATION_RESPONSE.CPGSR_NO_CREDENTIAL_NOT_FINISHED;
                pcpcs = new _CREDENTIAL_PROVIDER_CREDENTIAL_SERIALIZATION { ulAuthenticationPackage = 1234 };
                ppszOptionalStatusText = (resultMessageInformation.Message ?? string.Empty);

                if (resultMessageInformation.Icon == ResultMessageInformationIcons.None)
                {
                    pcpsiOptionalStatusIcon = _CREDENTIAL_PROVIDER_STATUS_ICON.CPSI_NONE;
                }
                else if (resultMessageInformation.Icon == ResultMessageInformationIcons.Information)
                {
                    pcpsiOptionalStatusIcon = _CREDENTIAL_PROVIDER_STATUS_ICON.CPSI_SUCCESS;
                }
                else if (resultMessageInformation.Icon == ResultMessageInformationIcons.Warning)
                {
                    pcpsiOptionalStatusIcon = _CREDENTIAL_PROVIDER_STATUS_ICON.CPSI_WARNING;
                }
                else
                {
                    pcpsiOptionalStatusIcon = _CREDENTIAL_PROVIDER_STATUS_ICON.CPSI_ERROR;
                }

                return HRESULT.S_OK;
            }

            BeforeLogon();

            if (UnderlyingCredential == null)
            {
                try
                {
                    if (WindowsLogon)
                    {
                        LogonResponse logonResponse = ProcessLogon();

                        if (logonResponse == null || logonResponse?.ErrorMessage != null)
                        {
                            pcpgsr = _CREDENTIAL_PROVIDER_GET_SERIALIZATION_RESPONSE.CPGSR_NO_CREDENTIAL_NOT_FINISHED;
                            pcpcs = new _CREDENTIAL_PROVIDER_CREDENTIAL_SERIALIZATION { ulAuthenticationPackage = 1234 };
                            ppszOptionalStatusText = (logonResponse?.ErrorMessage ?? "An unknown error occurred. Please contact your system administrator.");
                            pcpsiOptionalStatusIcon = _CREDENTIAL_PROVIDER_STATUS_ICON.CPSI_ERROR;

                            return HRESULT.S_OK;
                        }

                        WindowsLogonPackage windowsLogonPackage = logonResponse.WindowsLogonPackage;

                        string username = windowsLogonPackage.Domain + @"\" + windowsLogonPackage.Username;
                        string password = new NetworkCredential(string.Empty, windowsLogonPackage.Password).Password;

                        NegotiateAuthenticationPackage negotiateAuthenticationPackage = NegotiateAuthentication.CreateNegotiateAuthenticationPackage(username, password);

                        if (negotiateAuthenticationPackage == null)
                        {
                            pcpgsr = _CREDENTIAL_PROVIDER_GET_SERIALIZATION_RESPONSE.CPGSR_NO_CREDENTIAL_NOT_FINISHED;
                            pcpcs = new _CREDENTIAL_PROVIDER_CREDENTIAL_SERIALIZATION { ulAuthenticationPackage = 1234 };
                            ppszOptionalStatusText = "An unknown error occurred. Please contact your system administrator.";
                            pcpsiOptionalStatusIcon = _CREDENTIAL_PROVIDER_STATUS_ICON.CPSI_ERROR;

                            return HRESULT.S_OK;
                        }

                        pcpgsr = _CREDENTIAL_PROVIDER_GET_SERIALIZATION_RESPONSE.CPGSR_RETURN_CREDENTIAL_FINISHED;
                        pcpcs = new _CREDENTIAL_PROVIDER_CREDENTIAL_SERIALIZATION
                        {
                            clsidCredentialProvider = ManagedCredentialProvider.ComGuid,
                            rgbSerialization = negotiateAuthenticationPackage.CredentialInformation,
                            cbSerialization = (uint)negotiateAuthenticationPackage.CredentialSize,
                            ulAuthenticationPackage = (uint)negotiateAuthenticationPackage.AuthenticationPackage
                        };

                        ppszOptionalStatusText = string.Empty;
                        pcpsiOptionalStatusIcon = _CREDENTIAL_PROVIDER_STATUS_ICON.CPSI_SUCCESS;

                        return HRESULT.S_OK;
                    }

                    pcpgsr = _CREDENTIAL_PROVIDER_GET_SERIALIZATION_RESPONSE.CPGSR_NO_CREDENTIAL_FINISHED;
                    pcpcs = new _CREDENTIAL_PROVIDER_CREDENTIAL_SERIALIZATION { ulAuthenticationPackage = 1234 };
                    ppszOptionalStatusText = string.Empty;
                    pcpsiOptionalStatusIcon = _CREDENTIAL_PROVIDER_STATUS_ICON.CPSI_SUCCESS;

                    return HRESULT.S_OK;
                }
                catch (Exception exception)
                {
                    GlobalLogger.Log(LogLevels.Warning, exception.Message, exception);
                }

                pcpgsr = _CREDENTIAL_PROVIDER_GET_SERIALIZATION_RESPONSE.CPGSR_NO_CREDENTIAL_NOT_FINISHED;
                pcpcs = new _CREDENTIAL_PROVIDER_CREDENTIAL_SERIALIZATION { ulAuthenticationPackage = 1234 };
                ppszOptionalStatusText = string.Empty;
                pcpsiOptionalStatusIcon = _CREDENTIAL_PROVIDER_STATUS_ICON.CPSI_SUCCESS;

                return HRESULT.S_OK;
            }

            return UnderlyingCredential.GetSerialization(out pcpgsr, out pcpcs, out ppszOptionalStatusText, out pcpsiOptionalStatusIcon);
        }

        /// <summary>
        /// Statutory method from <see cref="ICredentialProviderCredential"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-icredentialprovidercredential for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public int ReportResult(int ntsStatus, int ntsSubstatus, out string ppszOptionalStatusText, out _CREDENTIAL_PROVIDER_STATUS_ICON pcpsiOptionalStatusIcon)
        {
            GlobalLogger.LogMethodCall();

            if (UnderlyingCredential == null)
            {
                ppszOptionalStatusText = string.Empty;
                pcpsiOptionalStatusIcon = _CREDENTIAL_PROVIDER_STATUS_ICON.CPSI_NONE;

                return HRESULT.S_OK;
            }

            return UnderlyingCredential.ReportResult(ntsStatus, ntsSubstatus, out ppszOptionalStatusText, out pcpsiOptionalStatusIcon);
        }

        #endregion

        #region IConnectableCredentialProviderCredential

        /// <summary>
        /// Statutory method from <see cref="IConnectableCredentialProviderCredential"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-iconnectablecredentialprovidercredential for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public int Connect(IQueryContinueWithStatus pqcws)
        {
            GlobalLogger.LogMethodCall();

            Connection = new Connection(pqcws);

            ConnectionFactory?.Invoke(this, Connection);

            if (Connection.Cancelled == false)
            {
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
            }

            return HRESULT.S_OK;
        }

        /// <summary>
        /// Statutory method from <see cref="IConnectableCredentialProviderCredential"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-iconnectablecredentialprovidercredential for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public int Disconnect()
        {
            GlobalLogger.LogMethodCall();

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

            return HRESULT.S_OK;
        }

        #endregion

        #region ICredentialProviderSetUserArray

        /// <summary>
        /// Statutory method from <see cref="ICredentialProviderCredential2"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-icredentialprovidercredential2 for more information.
        /// 
        /// Currently, no managed API has been created for the <see cref="ICredentialProviderCredential2"/> interface other than wrapping an existing credential.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
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

        #region ICredentialProviderCredentialWithFieldOptions

        public int GetFieldOptions(uint fieldID, out CREDENTIAL_PROVIDER_CREDENTIAL_FIELD_OPTIONS options)
        {
            GlobalLogger.LogMethodCall();

            if (UnderlyingCredential != null)
            {
                if (UnderlyingCredential is ICredentialProviderCredentialWithFieldOptions credentialProviderCredentialWithFieldOptions)
                {
                    int result = HRESULT.E_FAIL;

                    result = credentialProviderCredentialWithFieldOptions.GetFieldOptions(fieldID, out options);

                    if (result != HRESULT.S_OK)
                    {
                        options = default;

                        return result;
                    }
                }
                else
                {
                    options = default;
                }
            }
            else
            {
                options = default;
            }

            return HRESULT.S_OK;
        }

        #endregion

        #endregion

        #endregion
    }
}