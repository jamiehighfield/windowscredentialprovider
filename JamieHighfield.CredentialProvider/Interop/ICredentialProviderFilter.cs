using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

namespace JamieHighfield.CredentialProvider.Interop
{
    [Guid("A5DA53F9-D475-4080-A120-910C4A739880")]
    [SuppressUnmanagedCodeSecurity]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport]
    public interface ICredentialProviderFilter
    {
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        unsafe int Filter([In] CREDENTIAL_PROVIDER_USAGE_SCENARIO cpus, [In] uint dwFlags, [In] Guid* rgclsidProviders, [In, Out] int* rgbAllow, [In] uint cProviders);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        int UpdateRemoteCredential([In] ref CREDENTIAL_PROVIDER_CREDENTIAL_SERIALIZATION pcpcsIn, out CREDENTIAL_PROVIDER_CREDENTIAL_SERIALIZATION pcpcsOut);
    }
}