using System;
using System.Runtime.InteropServices;

namespace JamieHighfield.CredentialProvider.Interop
{
    [ComConversionLoss]
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct CREDENTIAL_PROVIDER_CREDENTIAL_SERIALIZATION
    {
        public uint ulAuthenticationPackage;
        public Guid clsidCredentialProvider;
        public uint cbSerialization;
        [ComConversionLoss]
        public IntPtr rgbSerialization;
    }
}