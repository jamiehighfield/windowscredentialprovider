namespace JamieHighfield.CredentialProvider.Credentials.Events
{
    public sealed class LoadedEventArgs
    {
        public LoadedEventArgs(bool autoLogon)
        {
            AutoLogon = autoLogon;
            WindowsLogon = true;
        }

        #region Variables



        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets whether this credential should automatically logon.
        /// </summary>
        public bool AutoLogon { get; set; }

        /// <summary>
        /// Gets or sets whether this credential should forward an authentication package to Windows. This property will be ignored if this credential forms part of a wrapped credential.
        /// </summary>
        public bool WindowsLogon { get; set; }

        #endregion

        #region Methods



        #endregion
    }
}