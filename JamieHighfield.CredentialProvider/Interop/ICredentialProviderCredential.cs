﻿using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

namespace JamieHighfield.CredentialProvider.Interop
{
    [SuppressUnmanagedCodeSecurity]
    [Guid("63913A93-40C1-481A-818D-4072FF8C70CC")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport]
    public interface ICredentialProviderCredential
    {
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        ResultHandles Advise([MarshalAs(UnmanagedType.Interface), In] ICredentialProviderCredentialEvents pcpce);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        ResultHandles UnAdvise();

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        ResultHandles SetSelected(out int pbAutoLogon);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        ResultHandles SetDeselected();

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        ResultHandles GetFieldState(
          [In] uint dwFieldID,
          out CREDENTIAL_PROVIDER_FIELD_STATE pcpfs,
          out CREDENTIAL_PROVIDER_FIELD_INTERACTIVE_STATE pcpfis);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        ResultHandles GetStringValue([In] uint dwFieldID, [MarshalAs(UnmanagedType.LPWStr)] out string ppsz);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        ResultHandles GetBitmapValue([In] uint dwFieldID, out IntPtr phbmp);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        ResultHandles GetCheckboxValue([In] uint dwFieldID, out int pbChecked, [MarshalAs(UnmanagedType.LPWStr)] out string ppszLabel);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        ResultHandles GetSubmitButtonValue([In] uint dwFieldID, out uint pdwAdjacentTo);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        ResultHandles GetComboBoxValueCount([In] uint dwFieldID, out uint pcItems, out uint pdwSelectedItem);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        ResultHandles GetComboBoxValueAt([In] uint dwFieldID, uint dwItem, [MarshalAs(UnmanagedType.LPWStr)] out string ppszItem);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        ResultHandles SetStringValue([In] uint dwFieldID, [MarshalAs(UnmanagedType.LPWStr), In] string psz);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        ResultHandles SetCheckboxValue([In] uint dwFieldID, [In] int bChecked);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        ResultHandles SetComboBoxSelectedValue([In] uint dwFieldID, [In] uint dwSelectedItem);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        ResultHandles CommandLinkClicked([In] uint dwFieldID);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        ResultHandles GetSerialization(
          out CREDENTIAL_PROVIDER_GET_SERIALIZATION_RESPONSE pcpgsr,
          out CREDENTIAL_PROVIDER_CREDENTIAL_SERIALIZATION pcpcs,
          [MarshalAs(UnmanagedType.LPWStr)] out string ppszOptionalStatusText,
          out CREDENTIAL_PROVIDER_STATUS_ICON pcpsiOptionalStatusIcon);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        ResultHandles ReportResult(
          [In] int ntsStatus,
          [In] int ntsSubstatus,
          [MarshalAs(UnmanagedType.LPWStr)] out string ppszOptionalStatusText,
          out CREDENTIAL_PROVIDER_STATUS_ICON pcpsiOptionalStatusIcon);
    }
}