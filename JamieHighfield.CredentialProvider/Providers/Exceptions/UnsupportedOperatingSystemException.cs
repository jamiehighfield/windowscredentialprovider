using System;

namespace JamieHighfield.CredentialProvider.Providers.Exceptions
{
    public sealed class UnsupportedOperatingSystemException : Exception
    {
        internal UnsupportedOperatingSystemException()
            : base("This operating system is not supported. The minimum operating system supported is Windows 7 or Windows Server 2008 R2.")
        { }

        #region Variables



        #endregion

        #region Properties



        #endregion

        #region Methods



        #endregion
    }
}