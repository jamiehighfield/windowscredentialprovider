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

namespace JamieHighfield.CredentialProvider.Logon
{
    public sealed class LogonResponse
    {
        public LogonResponse(LogonPackage logonPackage, WindowsLogonPackage windowsLogonPackage)
        {
            Successful = true;
            LogonPackage = logonPackage ?? throw new ArgumentNullException(nameof(logonPackage));
            WindowsLogonPackage = windowsLogonPackage ?? throw new ArgumentNullException(nameof(windowsLogonPackage));
        }

        public LogonResponse(string errorMessage)
        {
            Successful = false;
            ErrorMessage = errorMessage;
        }

        #region Variables



        #endregion

        #region Properties

        public bool Successful { get; private set; }

        public LogonPackage LogonPackage { get; private set; }

        public WindowsLogonPackage WindowsLogonPackage { get; private set; }

        public string ErrorMessage { get; private set; }

        #endregion

        #region Methods



        #endregion
    }
}