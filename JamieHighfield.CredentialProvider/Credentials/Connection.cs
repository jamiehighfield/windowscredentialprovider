using JamieHighfield.CredentialProvider.Interop;
using static JamieHighfield.CredentialProvider.Constants;

namespace JamieHighfield.CredentialProvider.Credentials
{
    /// <summary>
    /// This class is used as a parameter in the <see cref="ConnectableCredentialBase.ProcessConnection"/> method to provide methods for interacting with the Windows logon screen during the connection process.
    /// </summary>
    public sealed class Connection
    {
        /// <summary>
        /// Instantiate a new <see cref="Connection"/>.
        /// </summary>
        /// <param name="status">The underlying <see cref="IQueryContinueWithStatus"/> object; the implementation of which provided by Windows.</param>
        internal Connection(IQueryContinueWithStatus status)
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
        /// Gets whether the operation was cancelled. To cancel the operation, you should use the <see cref="Cancel()"/> method.
        /// </summary>
        public bool Cancelled { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Update the currently displayed status text.
        /// </summary>
        /// <param name="statusText"></param>
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