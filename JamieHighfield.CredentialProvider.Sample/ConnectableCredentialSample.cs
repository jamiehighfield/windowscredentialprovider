using JamieHighfield.CredentialProvider.Credentials;
using JamieHighfield.CredentialProvider.Credentials.Events;
using JamieHighfield.CredentialProvider.Credentials.Interfaces;
using System.Threading;

namespace JamieHighfield.CredentialProvider.Sample
{
    public sealed class ConnectableCredentialSample : CredentialBase, IExtendedCredential, IConnectableCredential
    {
        public ConnectableCredentialSample()
        {
            Connecting += ConnectableCredentialSample_Connecting;
        }

        #region Variables



        #endregion

        #region Properties



        #endregion

        #region Methods

        #region Event Handlers

        private void ConnectableCredentialSample_Connecting(object sender, ConnectingEventArgs eventArgs)
        {
            eventArgs.UpdateStatus("Logging in...");

            Thread.Sleep(600);
        }

        #endregion

        #endregion
    }
}