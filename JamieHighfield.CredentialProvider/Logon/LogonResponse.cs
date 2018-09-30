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
        public LogonResponse(IncomingLogonPackage logonPackage, WindowsLogonPackage windowsLogonPackage)
        {
            LogonPackage = logonPackage ?? throw new ArgumentNullException(nameof(logonPackage));
            WindowsLogonPackage = windowsLogonPackage ?? throw new ArgumentNullException(nameof(windowsLogonPackage));
        }

        public LogonResponse(ErrorMessageIcons errorMessageIcon, string errorMessage)
        {
            ErrorMessageIcon = errorMessageIcon;
            ErrorMessage = errorMessage ?? throw new ArgumentNullException(nameof(errorMessage));
        }

        #region Variables



        #endregion

        #region Properties

        public IncomingLogonPackage LogonPackage { get; private set; }

        public WindowsLogonPackage WindowsLogonPackage { get; private set; }

        /// <summary>
        /// Gets the <see cref="ErrorMessageIcons"/> to be displayed alongside the <see cref="ErrorMessage"/>. This property will be ignored in all usage scenarios in Windows 8/Windows Server 2012 and above.
        /// </summary>
        public ErrorMessageIcons ErrorMessageIcon { get; private set; }

        /// <summary>
        /// Gets the error message to be displayed by Windows.
        /// </summary>
        public string ErrorMessage { get; private set; }

        #endregion

        #region Methods



        #endregion
    }
}