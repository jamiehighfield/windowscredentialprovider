using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

namespace JamieHighfield.CredentialProvider.Interop
{
    [SuppressUnmanagedCodeSecurity]
    [Guid("B53C00B6-9922-4B78-B1F4-DDFE774DC39B")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport]
    public interface ICredentialProviderCredentialEvents2 : ICredentialProviderCredentialEvents
    {
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        new ResultHandles SetFieldState([MarshalAs(UnmanagedType.Interface), In] ICredentialProviderCredential pcpc, [In] uint dwFieldID, [In] CREDENTIAL_PROVIDER_FIELD_STATE cpfs);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        new ResultHandles SetFieldInteractiveState([MarshalAs(UnmanagedType.Interface), In] ICredentialProviderCredential pcpc, [In] uint dwFieldID, [In] CREDENTIAL_PROVIDER_FIELD_INTERACTIVE_STATE cpfis);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        new ResultHandles SetFieldString([MarshalAs(UnmanagedType.Interface), In] ICredentialProviderCredential pcpc, [In] uint dwFieldID, [MarshalAs(UnmanagedType.LPWStr), In] string psz);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        new ResultHandles SetFieldCheckbox([MarshalAs(UnmanagedType.Interface), In] ICredentialProviderCredential pcpc, [In] uint dwFieldID, [In] int bChecked, [MarshalAs(UnmanagedType.LPWStr), In] string pszLabel);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        new ResultHandles SetFieldBitmap([MarshalAs(UnmanagedType.Interface), In] ICredentialProviderCredential pcpc, [In] uint dwFieldID, [ComAliasName("OTP.Provider.Interop.wireHBITMAP"), In] IntPtr hbmp);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        new ResultHandles SetFieldComboBoxSelectedItem([MarshalAs(UnmanagedType.Interface), In] ICredentialProviderCredential pcpc, [In] uint dwFieldID, [In] int dwSelectedItem);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        new ResultHandles DeleteFieldComboBoxItem([MarshalAs(UnmanagedType.Interface), In] ICredentialProviderCredential pcpc, [In] uint dwFieldID, [In] uint dwItem);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        new ResultHandles AppendFieldComboBoxItem([MarshalAs(UnmanagedType.Interface), In] ICredentialProviderCredential pcpc, [In] uint dwFieldID, [MarshalAs(UnmanagedType.LPWStr), In] string pszItem);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        new ResultHandles SetFieldSubmitButton([MarshalAs(UnmanagedType.Interface), In] ICredentialProviderCredential pcpc, [In] uint dwFieldID, [In] uint dwAdjacentTo);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        new ResultHandles OnCreatingWindow([ComAliasName("OTP.Provider.Interop.wireHWND"), Out] out IntPtr phwndOwner);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        ResultHandles BeginFieldUpdates();

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        ResultHandles EndFieldUpdates();

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        ResultHandles SetFieldOptions([MarshalAs(UnmanagedType.Interface), In] ICredentialProviderCredential credential, [In] uint fieldID,[In] CREDENTIAL_PROVIDER_CREDENTIAL_FIELD_OPTIONS options);
    }
}