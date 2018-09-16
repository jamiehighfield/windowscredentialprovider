using System;

namespace JamieHighfield.CredentialProvider.Logon.Authentication
{
    public abstract class CredentialProviderAuthenticationBase : LogonSequenceBase
    {
        #region Variables



        #endregion

        #region Properties



        #endregion

        #region Methods

        internal sealed override LogonResponse ProcessSequence(LogonPackage logonPackage, WindowsLogonPackage windowsLogonPackage)
        {
            if (logonPackage == null)
            {
                throw new ArgumentNullException(nameof(logonPackage));
            }

            return Authenticate(logonPackage, windowsLogonPackage);
        }

        public abstract LogonResponse Authenticate(LogonPackage logonPackage, WindowsLogonPackage windowsLogonPackage);

        #endregion
    }
}