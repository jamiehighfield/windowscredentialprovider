using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

namespace JamieHighfield.CredentialProvider.Interop
{
    [SuppressUnmanagedCodeSecurity]
    [Guid("34201E5A-A787-41A3-A5A4-BD6DCF2A854E")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport]
    public interface ICredentialProviderEvents
    {
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        int CredentialsChanged([ComAliasName("OTP.Provider.Interop.UINT_PTR"), In] ulong upAdviseContext);
    }
}