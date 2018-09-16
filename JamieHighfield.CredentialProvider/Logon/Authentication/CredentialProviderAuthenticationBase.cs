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