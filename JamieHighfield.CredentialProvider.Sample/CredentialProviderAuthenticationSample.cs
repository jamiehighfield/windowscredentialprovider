using JamieHighfield.CredentialProvider.Logon;
using JamieHighfield.CredentialProvider.Logon.Authentication;
using System;
using System.DirectoryServices.AccountManagement;
using System.Net;

namespace JamieHighfield.CredentialProvider.Sample
{
    public sealed class CredentialProviderAuthenticationSample : CredentialProviderAuthenticationBase
    {
        #region Variables



        #endregion

        #region Properties



        #endregion

        #region Methods

        public override LogonResponse Authenticate(LogonPackage logonPackage, WindowsLogonPackage windowsLogonPackage)
        {
            try
            {
                string username = "jamie";// ((CredentialSample)logonPackage.Credential).Username;
                //SecureString password = ((CredentialSample)logonPackage.Credential).Password;
                string domain = Environment.MachineName;// ((CredentialSample)logonPackage.Credential).Domain;

                using (PrincipalContext principalContext = new PrincipalContext(ContextType.Machine))
                {
                    if (principalContext.ValidateCredentials(username, (new NetworkCredential(string.Empty, "password")).Password) == true)
                    {
                        return new LogonResponse(logonPackage, new WindowsLogonPackage(username, "password", domain));
                    }
                }
            }
            catch { }

            return new LogonResponse("The username or password entered was incorrect.");
        }
        
        #endregion
    }
}