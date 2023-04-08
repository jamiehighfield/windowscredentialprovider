using System;
using System.Runtime.InteropServices;

namespace JamieHighfield.CredentialProvider.Interop
{
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct _tagpropertykey
    {
        public Guid fmtid;
        public uint pid;
    }
}