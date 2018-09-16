using JamieHighfield.CredentialProvider.Credentials.Events;
using JamieHighfield.CredentialProvider.Interop;
using JamieHighfield.CredentialProvider.Logon;
using System;
using static JamieHighfield.CredentialProvider.Constants;

namespace JamieHighfield.CredentialProvider.Credentials
{
    public abstract class ConnectableCredential2Base : Credential2Base, IConnectableCredentialProviderCredential
    {
        public ConnectableCredential2Base() { }

        public ConnectableCredential2Base(LogonSequencePipelineBase logonSequencePipeline)
            : base(logonSequencePipeline)
        { }

        #region Variables



        #endregion

        #region Properties



        #endregion

        #region Methods

        #region Credential Provider Interface Methods

        public int Connect(IQueryContinueWithStatus pqcws)
        {
            if (BridgedCredential != null)
            {
                if (BridgedCredential is IConnectableCredentialProviderCredential)
                {
                    Connecting?.Invoke(this, new ConnectingEventArgs(pqcws));

                    int result = HRESULT.E_FAIL;

                    result = ((IConnectableCredentialProviderCredential)BridgedCredential).Connect(pqcws);

                    if (result != HRESULT.S_OK)
                    {
                        return result;
                    }

                    Connected?.Invoke(this, new ConnectedEventArgs(pqcws));
                }
            }

            Connecting?.Invoke(this, new ConnectingEventArgs(pqcws));

            Connected?.Invoke(this, new ConnectedEventArgs(pqcws));

            return HRESULT.S_OK;
        }

        public int Disconnect()
        {
            if (BridgedCredential != null)
            {
                if (BridgedCredential is IConnectableCredentialProviderCredential)
                {
                    int result = HRESULT.E_FAIL;

                    result = ((IConnectableCredentialProviderCredential)BridgedCredential).Disconnect();

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

        #region Events

        public event EventHandler<ConnectingEventArgs> Connecting;

        public event EventHandler<ConnectedEventArgs> Connected;

        #endregion
    }
}