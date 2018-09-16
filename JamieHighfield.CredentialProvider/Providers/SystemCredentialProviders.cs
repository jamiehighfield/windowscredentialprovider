using JamieHighfield.CredentialProvider.Providers.Exceptions;
using System;

namespace JamieHighfield.CredentialProvider.Providers
{
    public static class SystemCredentialProviders
    {
        #region Variables



        #endregion

        #region Properties

        /// <summary>
        /// Gets the default Windows credential provider for password authentication. This credential provider differs between Windows 7/Windows Server 2008 R2 and Windows 8; the correct credential provider will be chosen for the current operating system.
        /// </summary>
        public static Guid Password
        {
            get
            {
                if (Environment.OSVersion.Version.Major < 6)
                {
                    throw new UnsupportedOperatingSystemException();
                }

                if (Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor < 1)
                {
                    throw new UnsupportedOperatingSystemException();
                }

                if (Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor == 1)
                {
                    return Guid.Parse("{6f45dc1e-5384-457a-bc13-2cd81b0d28ed}");
                }
                else
                {
                    return Guid.Parse("{60b78e88-ead8-445c-9cfd-0b87f74ea6cd}");
                }
            }
        }

        #endregion

        #region Methods



        #endregion
    }
}