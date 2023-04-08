using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

namespace JamieHighfield.CredentialProvider.Interop
{
    [ComConversionLoss]
    [Guid("FA6FA76B-66B7-4B11-95F1-86171118E816")]
    [SuppressUnmanagedCodeSecurity]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport]
    public interface ICredentialProviderCredentialEvents
    {
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        ResultHandles SetFieldState([MarshalAs(UnmanagedType.Interface), In] ICredentialProviderCredential pcpc, [In] uint dwFieldID, [In] CREDENTIAL_PROVIDER_FIELD_STATE cpfs);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        ResultHandles SetFieldInteractiveState([MarshalAs(UnmanagedType.Interface), In] ICredentialProviderCredential pcpc, [In] uint dwFieldID, [In] CREDENTIAL_PROVIDER_FIELD_INTERACTIVE_STATE cpfis);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        ResultHandles SetFieldString([MarshalAs(UnmanagedType.Interface), In] ICredentialProviderCredential pcpc, [In] uint dwFieldID, [MarshalAs(UnmanagedType.LPWStr), In] string psz);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        ResultHandles SetFieldCheckbox([MarshalAs(UnmanagedType.Interface), In] ICredentialProviderCredential pcpc, [In] uint dwFieldID, [In] int bChecked, [MarshalAs(UnmanagedType.LPWStr), In] string pszLabel);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        ResultHandles SetFieldBitmap([MarshalAs(UnmanagedType.Interface), In] ICredentialProviderCredential pcpc, [In] uint dwFieldID, [ComAliasName("OTP.Provider.Interop.wireHBITMAP"), In] IntPtr hbmp);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        ResultHandles SetFieldComboBoxSelectedItem([MarshalAs(UnmanagedType.Interface), In] ICredentialProviderCredential pcpc, [In] uint dwFieldID, [In] int dwSelectedItem);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        ResultHandles DeleteFieldComboBoxItem([MarshalAs(UnmanagedType.Interface), In] ICredentialProviderCredential pcpc, [In] uint dwFieldID, [In] uint dwItem);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        ResultHandles AppendFieldComboBoxItem([MarshalAs(UnmanagedType.Interface), In] ICredentialProviderCredential pcpc, [In] uint dwFieldID, [MarshalAs(UnmanagedType.LPWStr), In] string pszItem);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        ResultHandles SetFieldSubmitButton([MarshalAs(UnmanagedType.Interface), In] ICredentialProviderCredential pcpc, [In] uint dwFieldID, [In] uint dwAdjacentTo);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        ResultHandles OnCreatingWindow([ComAliasName("OTP.Provider.Interop.wireHWND"), Out] out IntPtr phwndOwner);
    }
}