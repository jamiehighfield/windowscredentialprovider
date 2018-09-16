using JamieHighfield.CredentialProvider.Interop;
using static JamieHighfield.CredentialProvider.Constants;

namespace JamieHighfield.CredentialProvider.Credentials.Events
{
    public sealed class ConnectingEventArgs
    {
        internal ConnectingEventArgs(IQueryContinueWithStatus status)
        {
            Status = status;
        }

        #region Variables



        #endregion

        #region Properties

        private IQueryContinueWithStatus Status { get; set; }

        /// <summary>
        /// Gets whether the operation was cancelled by the user clicking the 'Cancel' button.
        /// </summary>
        public bool UserCancelled
        {
            get
            {
                return (Status.QueryContinue() != HRESULT.S_OK);
            }
        }

        /// <summary>
        /// Gets whether the operation was cancelled. To cancell the operation, you should use the <see cref="Cancel"/> method.
        /// </summary>
        internal bool Cancelled { get; private set; }

        #endregion

        #region Methods

        public void UpdateStatus(string statusText)
        {
            Status.SetStatusMessage(statusText);
        }

        /// <summary>
        /// Cancel the current operation. The current method will continue to run until it has finished, but the login will be cancelled when the current method has finished.
        /// </summary>
        public void Cancel()
        {
            Cancelled = true;
        }

        #endregion
    }
}