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