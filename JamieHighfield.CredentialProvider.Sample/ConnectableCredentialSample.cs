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