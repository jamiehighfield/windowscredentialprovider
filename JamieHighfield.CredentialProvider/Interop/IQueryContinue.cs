using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

namespace JamieHighfield.CredentialProvider.Interop
{
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [SuppressUnmanagedCodeSecurity]
    [Guid("7307055C-B24A-486B-9F25-163E597A28A9")]
    [ComImport]
    public interface IQueryContinue
    {
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        ResultHandles QueryContinue();
    }
}