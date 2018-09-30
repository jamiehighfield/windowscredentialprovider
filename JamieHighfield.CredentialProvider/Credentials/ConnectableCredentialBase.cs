using System;
using JamieHighfield.CredentialProvider.Interop;
using JamieHighfield.CredentialProvider.Logging;
using JamieHighfield.CredentialProvider.Logon;
using static JamieHighfield.CredentialProvider.Constants;

namespace JamieHighfield.CredentialProvider.Credentials
{
    /// <summary>
    /// /// Extend this class to expose a class as a credential.
    /// /// </summary>
    public abstract class ConnectableCredentialBase : CredentialBase, IConnectableCredentialProviderCredential
    {
        /// <summary>
        /// Instantiate a new <see cref="ConnectableCredentialBase"/> object.
        /// </summary>
        protected ConnectableCredentialBase() { }

        /// <summary>
        /// Instantiate a new <see cref="ConnectableCredentialBase"/> object.
        /// </summary>
        /// <param name="logonSequencePipeline">The <see cref="LogonSequencePipelineBase"/> instance used during the logon sequence.</param>
        protected ConnectableCredentialBase(LogonSequencePipelineBase logonSequencePipeline)
            : base(logonSequencePipeline)
        { }

        #region Variables



        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the <see cref="Logon.LogonResponse"/> which is returned from the output of running the <see cref="LogonSequencePipeline"/>. This property can be used as a link between the <see cref="ProcessConnection(Connection)"/> and <see cref="ProcessLogon"/> methods.
        /// </summary>
        public LogonResponse LogonResponse { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Credentials.Connection"/> which is used during the credential's connection phase.
        /// </summary>
        private Connection Connection { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// This method is called to get a <see cref="Logon.LogonResponse"/> containing a logon package that can be used to logon to Windows. By default, this method runs the <see cref="LogonSequencePipeline"/> within this credential and returns the <see cref="Logon.LogonResponse"/> from that. This method will be ignored if an underlying credential has been specified or <see cref="CredentialBase.WindowsLogon"/> is set to true. This method can be overridden to provide different behaviour.
        /// </summary>
        /// <returns>The <see cref="Logon.LogonResponse"/> to be parsed to Windows for authentication.</returns>
        public override LogonResponse ProcessLogon()
        {
            if (WindowsLogon == true)
            {
                if (LogonSequencePipeline == null)
                {
                    GlobalLogger.Log(LogLevels.Warning, "The default behaviour of this credential provider is to process a logon sequence pipeline. The provided logon sequence pipeline was either null or in the incorrect format.");

                    return new LogonResponse(ErrorMessageIcons.Error, "The default behaviour of this credential provider is to process a logon sequence pipeline. The provided logon sequence pipeline was either null or in the incorrect format. Please contact your system administrator.");
                }
                else
                {
                    if (LogonResponse == null || LogonResponse?.ErrorMessage != null)
                    {
                        GlobalLogger.Log(LogLevels.Warning, "The default behaviour of this credential provider is to process a logon sequence pipeline. The provided logon sequence pipeline returned either null or an object in the incorrect format."
                            + Environment.NewLine
                            + Environment.NewLine
                            + "The provided error message was:"
                            + Environment.NewLine
                            + Environment.NewLine
                            + LogonResponse.ErrorMessage);
                    }

                    return LogonResponse;
                }
            }

            return null;
        }

        /// <summary>
        /// This method is called to before <see cref="ProcessLogon"/> is called and should be used to perform time consuming operations. This method will be ignored if <see cref="CredentialBase.WindowsLogon"/> is set to true. This method can be overridden to provide different behaviour. 
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public virtual void ProcessConnection(Connection connection)
        {
            if (WindowsLogon == true)
            {
                if (LogonSequencePipeline == null)
                {
                    GlobalLogger.Log(LogLevels.Warning, "The default behaviour of this credential provider is to process a logon sequence pipeline. The provided logon sequence pipeline was either null or in the incorrect format.");

                    connection.Cancel(ErrorMessageIcons.Error, "This credential provider is not currently accepting credentials. Please contact your system administrator.");
                }
                else
                {
                    LogonResponse = LogonSequencePipeline.ProcessSequencePipeline(new IncomingLogonPackage(this, this));
                }
            }

            return;
        }

        #region Credential Provider Interface Methods

        #region IConnectableCredentialProviderCredential

        /// <summary>
        /// Statutory method from <see cref="IConnectableCredentialProviderCredential"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-iconnectablecredentialprovidercredential for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        int IConnectableCredentialProviderCredential.Advise(ICredentialProviderCredentialEvents pcpce)
        {
            return ((ICredentialProviderCredential)this).Advise(pcpce);
        }

        /// <summary>
        /// Statutory method from <see cref="IConnectableCredentialProviderCredential"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-iconnectablecredentialprovidercredential for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        int IConnectableCredentialProviderCredential.UnAdvise()
        {
            return ((ICredentialProviderCredential)this).UnAdvise();
        }

        /// <summary>
        /// Statutory method from <see cref="IConnectableCredentialProviderCredential"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-iconnectablecredentialprovidercredential for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        int IConnectableCredentialProviderCredential.SetSelected(out int pbAutoLogon)
        {
            return ((ICredentialProviderCredential)this).SetSelected(out pbAutoLogon);
        }

        /// <summary>
        /// Statutory method from <see cref="IConnectableCredentialProviderCredential"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-iconnectablecredentialprovidercredential for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        int IConnectableCredentialProviderCredential.SetDeselected()
        {
            return ((ICredentialProviderCredential)this).SetDeselected();
        }

        /// <summary>
        /// Statutory method from <see cref="IConnectableCredentialProviderCredential"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-iconnectablecredentialprovidercredential for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        int IConnectableCredentialProviderCredential.GetFieldState(uint dwFieldID, out _CREDENTIAL_PROVIDER_FIELD_STATE pcpfs, out _CREDENTIAL_PROVIDER_FIELD_INTERACTIVE_STATE pcpfis)
        {
            return ((ICredentialProviderCredential)this).GetFieldState(dwFieldID, out pcpfs, out pcpfis);
        }

        /// <summary>
        /// Statutory method from <see cref="IConnectableCredentialProviderCredential"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-iconnectablecredentialprovidercredential for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        int IConnectableCredentialProviderCredential.GetStringValue(uint dwFieldID, out string ppsz)
        {
            return ((ICredentialProviderCredential)this).GetStringValue(dwFieldID, out ppsz);
        }

        /// <summary>
        /// Statutory method from <see cref="IConnectableCredentialProviderCredential"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-iconnectablecredentialprovidercredential for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        int IConnectableCredentialProviderCredential.GetBitmapValue(uint dwFieldID, out IntPtr phbmp)
        {
            return ((ICredentialProviderCredential)this).GetBitmapValue(dwFieldID, out phbmp);
        }

        /// <summary>
        /// Statutory method from <see cref="IConnectableCredentialProviderCredential"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-iconnectablecredentialprovidercredential for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        int IConnectableCredentialProviderCredential.GetCheckboxValue(uint dwFieldID, out int pbChecked, out string ppszLabel)
        {
            return ((ICredentialProviderCredential)this).GetCheckboxValue(dwFieldID, out pbChecked, out ppszLabel);
        }

        /// <summary>
        /// Statutory method from <see cref="IConnectableCredentialProviderCredential"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-iconnectablecredentialprovidercredential for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        int IConnectableCredentialProviderCredential.GetSubmitButtonValue(uint dwFieldID, out uint pdwAdjacentTo)
        {
            return ((ICredentialProviderCredential)this).GetSubmitButtonValue(dwFieldID, out pdwAdjacentTo);
        }

        /// <summary>
        /// Statutory method from <see cref="IConnectableCredentialProviderCredential"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-iconnectablecredentialprovidercredential for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        int IConnectableCredentialProviderCredential.GetComboBoxValueCount(uint dwFieldID, out uint pcItems, out uint pdwSelectedItem)
        {
            return ((ICredentialProviderCredential)this).GetComboBoxValueCount(dwFieldID, out pcItems, out pdwSelectedItem);
        }

        /// <summary>
        /// Statutory method from <see cref="IConnectableCredentialProviderCredential"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-iconnectablecredentialprovidercredential for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        int IConnectableCredentialProviderCredential.GetComboBoxValueAt(uint dwFieldID, uint dwItem, out string ppszItem)
        {
            return ((ICredentialProviderCredential)this).GetComboBoxValueAt(dwFieldID, dwItem, out ppszItem);
        }

        /// <summary>
        /// Statutory method from <see cref="IConnectableCredentialProviderCredential"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-iconnectablecredentialprovidercredential for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        int IConnectableCredentialProviderCredential.SetStringValue(uint dwFieldID, string psz)
        {
            return ((ICredentialProviderCredential)this).SetStringValue(dwFieldID, psz);
        }

        /// <summary>
        /// Statutory method from <see cref="IConnectableCredentialProviderCredential"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-iconnectablecredentialprovidercredential for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        int IConnectableCredentialProviderCredential.SetCheckboxValue(uint dwFieldID, int bChecked)
        {
            return ((ICredentialProviderCredential)this).SetCheckboxValue(dwFieldID, bChecked);
        }

        /// <summary>
        /// Statutory method from <see cref="IConnectableCredentialProviderCredential"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-iconnectablecredentialprovidercredential for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        int IConnectableCredentialProviderCredential.SetComboBoxSelectedValue(uint dwFieldID, uint dwSelectedItem)
        {
            return ((ICredentialProviderCredential)this).SetComboBoxSelectedValue(dwFieldID, dwSelectedItem);
        }

        /// <summary>
        /// Statutory method from <see cref="IConnectableCredentialProviderCredential"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-iconnectablecredentialprovidercredential for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        int IConnectableCredentialProviderCredential.CommandLinkClicked(uint dwFieldID)
        {
            return ((ICredentialProviderCredential)this).CommandLinkClicked(dwFieldID);
        }

        /// <summary>
        /// Statutory method from <see cref="IConnectableCredentialProviderCredential"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-iconnectablecredentialprovidercredential for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        int IConnectableCredentialProviderCredential.GetSerialization(out _CREDENTIAL_PROVIDER_GET_SERIALIZATION_RESPONSE pcpgsr, out _CREDENTIAL_PROVIDER_CREDENTIAL_SERIALIZATION pcpcs, out string ppszOptionalStatusText, out _CREDENTIAL_PROVIDER_STATUS_ICON pcpsiOptionalStatusIcon)
        {
            return ((ICredentialProviderCredential)this).GetSerialization(out pcpgsr, out pcpcs, out ppszOptionalStatusText, out pcpsiOptionalStatusIcon);
        }

        /// <summary>
        /// Statutory method from <see cref="IConnectableCredentialProviderCredential"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-iconnectablecredentialprovidercredential for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        int IConnectableCredentialProviderCredential.ReportResult(int ntsStatus, int ntsSubstatus, out string ppszOptionalStatusText, out _CREDENTIAL_PROVIDER_STATUS_ICON pcpsiOptionalStatusIcon)
        {
            return ((ICredentialProviderCredential)this).ReportResult(ntsStatus, ntsSubstatus, out ppszOptionalStatusText, out pcpsiOptionalStatusIcon);
        }

        /// <summary>
        /// Statutory method from <see cref="IConnectableCredentialProviderCredential"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-iconnectablecredentialprovidercredential for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        int IConnectableCredentialProviderCredential.Connect(IQueryContinueWithStatus pqcws)
        {
            GlobalLogger.LogMethodCall();

            Connection connection = new Connection(pqcws);

            ProcessConnection(connection);

            if (connection.Cancelled == false)
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
        int IConnectableCredentialProviderCredential.Disconnect()
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

        #endregion

        #endregion
    }
}
