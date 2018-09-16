namespace JamieHighfield.CredentialProvider.Logon
{
    public abstract class LogonSequenceBase
    {
        internal LogonSequenceBase() { }

        #region Variables



        #endregion

        #region Properties



        #endregion

        #region Methods

        internal abstract LogonResponse ProcessSequence(LogonPackage logonPackage, WindowsLogonPackage windowsLogonPackage);
        
        #endregion
    }
}