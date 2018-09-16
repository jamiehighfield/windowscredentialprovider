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

using JamieHighfield.CredentialProvider.Interop;
using static JamieHighfield.CredentialProvider.Constants;

namespace JamieHighfield.CredentialProvider.Credentials.Events
{
    public sealed class ConnectedEventArgs
    {
        internal ConnectedEventArgs(IQueryContinueWithStatus status)
        {
            Status = status;
        }

        #region Variables



        #endregion

        #region Properties

        private IQueryContinueWithStatus Status { get; set; }

        public bool Cancelled
        {
            get
            {
                return (Status.QueryContinue() != HRESULT.S_OK);
            }
        }

        #endregion

        #region Methods

        public void SetStatusMessage(string message)
        {
            Status.SetStatusMessage(message);
        }

        #endregion
    }
}