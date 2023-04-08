using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

namespace JamieHighfield.CredentialProvider.Interop
{
    [Guid("90C119AE-0F18-4520-A1F1-114366A40FE8")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [SuppressUnmanagedCodeSecurity]
    [ComImport]
    public interface ICredentialProviderUserArray
    {
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        int SetProviderFilter([In] ref Guid guidProviderToFilterTo);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        int GetAccountOptions(
          out CREDENTIAL_PROVIDER_ACCOUNT_OPTIONS credentialProviderAccountOptions);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        int GetCount(out uint userCount);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        int GetAt([In] uint userIndex, [MarshalAs(UnmanagedType.Interface)] out ICredentialProviderUser user);
    }
}