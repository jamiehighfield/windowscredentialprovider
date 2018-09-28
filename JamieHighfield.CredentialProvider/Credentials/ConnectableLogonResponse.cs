using JamieHighfield.CredentialProvider.Logon;

namespace JamieHighfield.CredentialProvider.Credentials
{
    /// <summary>
    /// This class is used in conjunction with the <see cref="ConnectableCredentialBase.ProcessConnection"/> method.
    /// </summary>
    public sealed class ConnectableLogonResponse
    {
        /// <summary>
        /// Instantiate a new <see cref="ConnectableLogonResponse"/>.
        /// </summary>
        public ConnectableLogonResponse() { }

        /// <summary>
        /// Instantiate a new <see cref="ConnectableLogonResponse"/>.
        /// </summary>
        public ConnectableLogonResponse(string _responseMessage)
        {
            ErrorMessage = ErrorMessage;
        }

        #region Variables



        #endregion

        #region Properties

        public LogonResponse LogonResponse { get; private set; }

        public string ErrorMessage { get; private set; }

        #endregion

        #region Methods



        #endregion
    }
}