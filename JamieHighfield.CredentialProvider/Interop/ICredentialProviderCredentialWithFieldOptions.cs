using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

namespace JamieHighfield.CredentialProvider.Interop
{
    [SuppressUnmanagedCodeSecurity]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("DBC6FB30-C843-49E3-A645-573E6F39446A")]
    [ComImport]
    public interface ICredentialProviderCredentialWithFieldOptions
    {
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        ResultHandles GetFieldOptions(
          [In] uint fieldID,
          out CREDENTIAL_PROVIDER_CREDENTIAL_FIELD_OPTIONS options);
    }
}