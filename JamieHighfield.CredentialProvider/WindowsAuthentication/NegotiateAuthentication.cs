using System;
using System.Runtime.InteropServices;

namespace JamieHighfield.CredentialProvider.WindowsAuthentication
{
    internal static class NegotiateAuthentication
    {
        #region Platform Invocation

        [DllImport("credui.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool CredPackAuthenticationBuffer(int dwFlags, string pszUserName, string pszPassword, IntPtr pPackedCredentials, ref int pcbPackedCredentials);

        [DllImport("secur32.dll", SetLastError = false)]
        private static extern uint LsaConnectUntrusted([Out] out IntPtr lsaHandle);

        [DllImport("secur32.dll", SetLastError = false)]
        private static extern uint LsaLookupAuthenticationPackage([In] IntPtr lsaHandle, [In] ref LSA_STRING packageName, [Out] out UInt32 authenticationPackage);
        
        #endregion

        #region Structures

        [StructLayout(LayoutKind.Sequential)]
        internal struct LSA_STRING
        {
            internal UInt16 Length;
            internal UInt16 MaximumLength;
            internal IntPtr Buffer;
        }

        #endregion

        #region Variables



        #endregion

        #region Properties



        #endregion

        #region Methods

        internal static NegotiateAuthenticationPackage CreateNegotiateAuthenticationPackage(string username, string password)
        {
            int credentialSize = 0;
            IntPtr credentialBuffer = Marshal.AllocCoTaskMem(0);

            if (CredPackAuthenticationBuffer(0, username, password, credentialBuffer, ref credentialSize) == false)
            {
                Marshal.FreeCoTaskMem(credentialBuffer);
                credentialBuffer = Marshal.AllocCoTaskMem(credentialSize);

                if (CredPackAuthenticationBuffer(0, username, password, credentialBuffer, ref credentialSize) == true)
                {
                    int result = (int)LsaConnectUntrusted(out IntPtr localSecurityAuthorityHandle);

                    uint authenticationPackage;

                    using (LocalSecurityAuthorityWrapper localSecurityAuthorityWrapper = new LocalSecurityAuthorityWrapper("Negotiate"))
                    {
                        result = (int)LsaLookupAuthenticationPackage(localSecurityAuthorityHandle, ref localSecurityAuthorityWrapper.LocalAuthorityString, out authenticationPackage);
                    }

                    return new NegotiateAuthenticationPackage(credentialBuffer, credentialSize, (int)authenticationPackage);
                }
            }

            return null;
        }

        #endregion
    }
}