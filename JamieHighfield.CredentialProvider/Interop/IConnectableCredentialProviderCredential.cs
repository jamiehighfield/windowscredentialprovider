﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace JamieHighfield.CredentialProvider.Interop
{
    [SuppressUnmanagedCodeSecurity]
    [Guid("9387928B-AC75-4BF9-8AB2-2B93C4A55290")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport]
    public interface IConnectableCredentialProviderCredential : ICredentialProviderCredential
    {
        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        new ResultHandles Advise([MarshalAs(UnmanagedType.Interface), In] ICredentialProviderCredentialEvents pcpce);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        new ResultHandles UnAdvise();

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        new ResultHandles SetSelected(out int pbAutoLogon);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        new ResultHandles SetDeselected();

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        new ResultHandles GetFieldState(
          [In] uint dwFieldID,
          out CREDENTIAL_PROVIDER_FIELD_STATE pcpfs,
          out CREDENTIAL_PROVIDER_FIELD_INTERACTIVE_STATE pcpfis);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        new ResultHandles GetStringValue([In] uint dwFieldID, [MarshalAs(UnmanagedType.LPWStr)] out string ppsz);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        new ResultHandles GetBitmapValue([In] uint dwFieldID, out IntPtr phbmp);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        new ResultHandles GetCheckboxValue([In] uint dwFieldID, out int pbChecked, [MarshalAs(UnmanagedType.LPWStr)] out string ppszLabel);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        new ResultHandles GetSubmitButtonValue([In] uint dwFieldID, out uint pdwAdjacentTo);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        new ResultHandles GetComboBoxValueCount([In] uint dwFieldID, out uint pcItems, out uint pdwSelectedItem);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        new ResultHandles GetComboBoxValueAt([In] uint dwFieldID, uint dwItem, [MarshalAs(UnmanagedType.LPWStr)] out string ppszItem);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        new ResultHandles SetStringValue([In] uint dwFieldID, [MarshalAs(UnmanagedType.LPWStr), In] string psz);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        new ResultHandles SetCheckboxValue([In] uint dwFieldID, [In] int bChecked);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        new ResultHandles SetComboBoxSelectedValue([In] uint dwFieldID, [In] uint dwSelectedItem);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        new ResultHandles CommandLinkClicked([In] uint dwFieldID);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        new ResultHandles GetSerialization(
          out CREDENTIAL_PROVIDER_GET_SERIALIZATION_RESPONSE pcpgsr,
          out CREDENTIAL_PROVIDER_CREDENTIAL_SERIALIZATION pcpcs,
          [MarshalAs(UnmanagedType.LPWStr)] out string ppszOptionalStatusText,
          out CREDENTIAL_PROVIDER_STATUS_ICON pcpsiOptionalStatusIcon);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        new ResultHandles ReportResult(
          [In] int ntsStatus,
          [In] int ntsSubstatus,
          [MarshalAs(UnmanagedType.LPWStr)] out string ppszOptionalStatusText,
          out CREDENTIAL_PROVIDER_STATUS_ICON pcpsiOptionalStatusIcon);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        ResultHandles Connect([MarshalAs(UnmanagedType.Interface), In] IQueryContinueWithStatus pqcws);

        [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [return: MarshalAs(UnmanagedType.Error)]
        ResultHandles Disconnect();
    }
}