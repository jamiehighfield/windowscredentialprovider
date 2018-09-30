using System;
using System.Runtime.InteropServices;
using static JamieHighfield.CredentialProvider.WindowsAuthentication.NegotiateAuthentication;

namespace JamieHighfield.CredentialProvider.WindowsAuthentication
{
    internal sealed class LocalSecurityAuthorityWrapper : IDisposable
    {
        internal LocalSecurityAuthorityWrapper(string value)
        {
            LocalAuthorityString = new LSA_STRING();

            LocalAuthorityString.Length = (ushort)value.Length;
            LocalAuthorityString.MaximumLength = (ushort)value.Length;
            LocalAuthorityString.Buffer = Marshal.StringToHGlobalAnsi(value);
        }

        #region Properties

        internal LSA_STRING LocalAuthorityString;

        #endregion

        #region Methods

        ~LocalSecurityAuthorityWrapper()
        {
            Dispose(false);
        }

        void IDisposable.Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (LocalAuthorityString.Buffer != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(LocalAuthorityString.Buffer);
                LocalAuthorityString.Buffer = IntPtr.Zero;

            }

            if (disposing == true)
            {
                GC.SuppressFinalize(this);
            }
        }

        #endregion
    }
}