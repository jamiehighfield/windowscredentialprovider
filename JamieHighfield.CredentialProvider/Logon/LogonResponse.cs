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