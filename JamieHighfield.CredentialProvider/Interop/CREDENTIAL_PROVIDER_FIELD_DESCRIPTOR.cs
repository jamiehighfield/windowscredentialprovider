using System;
using System.Runtime.InteropServices;

namespace JamieHighfield.CredentialProvider.Interop
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct CREDENTIAL_PROVIDER_FIELD_DESCRIPTOR
    {
        public uint dwFieldID;
        public CREDENTIAL_PROVIDER_FIELD_TYPE cpft;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string pszLabel;
        public Guid guidFieldType;
    }
}