using System;

namespace JamieHighfield.CredentialProvider.WindowsAuthentication
{
    /// <summary>
    /// A wrapper class to wrap native function results for negotiate authentication.
    /// </summary>
    internal sealed class NegotiateAuthenticationPackage
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="credentialInformation">An <see cref="IntPtr"/> pointer to a byte array containing serialised credential information.</param>
        /// <param name="credentialSize">The length of <see cref="CredentialInformation"/>.</param>
        /// <param name="authenticationPackage">An <see cref="int"/> pointer to an authentication package that Windows will use to login.</param>
        internal NegotiateAuthenticationPackage(IntPtr credentialInformation, int credentialSize, int authenticationPackage)
        {
            CredentialInformation = credentialInformation;
            CredentialSize = credentialSize;
            AuthenticationPackage = authenticationPackage;
        }

        #region Variables



        #endregion

        #region Properties

        /// <summary>
        /// Gets an <see cref="IntPtr"/> pointer to a byte array containing serialised credential information.
        /// </summary>
        internal IntPtr CredentialInformation { get; private set; }

        /// <summary>
        /// Gets the length of <see cref="CredentialInformation"/>.
        /// </summary>
        internal int CredentialSize { get; private set; }

        /// <summary>
        /// Gets an <see cref="int"/> pointer to an authentication package that Windows will use to login.
        /// </summary>
        internal int AuthenticationPackage { get; private set; }

        #endregion

        #region Methods



        #endregion
    }
}