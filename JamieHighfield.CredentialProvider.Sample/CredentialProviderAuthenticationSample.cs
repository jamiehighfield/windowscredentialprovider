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

        public override LogonResponse Authenticate(IncomingLogonPackage incomingLogonPackage, WindowsLogonPackage windowsLogonPackage)
        {
            try
            {


                //string username = incomingLogonPackage.Credential.;// ((CredentialSample)logonPackage.Credential).Username;
                //SecureString password = ((CredentialSample)logonPackage.Credential).Password;
                string domain = Environment.MachineName;// ((CredentialSample)logonPackage.Credential).Domain;

                using (PrincipalContext principalContext = new PrincipalContext(ContextType.Machine))
                {
                    if (principalContext.ValidateCredentials("jamie", (new NetworkCredential(string.Empty, "password")).Password) == true)
                    {
                        return new LogonResponse(incomingLogonPackage, new WindowsLogonPackage("jamie", "password", domain));
                    }
                }
            }
            catch
            {
                return new LogonResponse(ErrorMessageIcons.Error, "An unknown error occurred. Please contact your system administrator.");
            }

            return new LogonResponse(ErrorMessageIcons.Error, "The username or password entered was incorrect.");
        }
        
        #endregion
    }
}