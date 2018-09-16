using JamieHighfield.CredentialProvider.Sample;
using System;
using System.Runtime.InteropServices;

namespace ConsoleApp1
{
    class Program
    {
        [DllImport("ole32.dll", EntryPoint = "CoCreateInstance", CallingConvention = CallingConvention.StdCall)]
        static extern UInt32 CoCreateInstance([In, MarshalAs(UnmanagedType.LPStruct)] Guid rclsid, IntPtr pUnkOuter, UInt32 dwClsContext, [In, MarshalAs(UnmanagedType.LPStruct)] Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out object rReturnedComObject);

        [Flags]
        enum CLSCTX : uint
        {
            CLSCTX_INPROC_SERVER = 0x1,
            CLSCTX_INPROC_HANDLER = 0x2,
            CLSCTX_LOCAL_SERVER = 0x4,
            CLSCTX_INPROC_SERVER16 = 0x8,
            CLSCTX_REMOTE_SERVER = 0x10,
            CLSCTX_INPROC_HANDLER16 = 0x20,
            CLSCTX_RESERVED1 = 0x40,
            CLSCTX_RESERVED2 = 0x80,
            CLSCTX_RESERVED3 = 0x100,
            CLSCTX_RESERVED4 = 0x200,
            CLSCTX_NO_CODE_DOWNLOAD = 0x400,
            CLSCTX_RESERVED5 = 0x800,
            CLSCTX_NO_CUSTOM_MARSHAL = 0x1000,
            CLSCTX_ENABLE_CODE_DOWNLOAD = 0x2000,
            CLSCTX_NO_FAILURE_LOG = 0x4000,
            CLSCTX_DISABLE_AAA = 0x8000,
            CLSCTX_ENABLE_AAA = 0x10000,
            CLSCTX_FROM_DEFAULT_CONTEXT = 0x20000,
            CLSCTX_ACTIVATE_32_BIT_SERVER = 0x40000,
            CLSCTX_ACTIVATE_64_BIT_SERVER = 0x80000,
            CLSCTX_INPROC = CLSCTX_INPROC_SERVER | CLSCTX_INPROC_HANDLER,
            CLSCTX_SERVER = CLSCTX_INPROC_SERVER | CLSCTX_LOCAL_SERVER | CLSCTX_REMOTE_SERVER,
            CLSCTX_ALL = CLSCTX_SERVER | CLSCTX_INPROC_HANDLER
        }

        [STAThread]
        static void Main(string[] args)
        {
            //try
            //{
            //    var clsid = "{60b78e88-ead8-445c-9cfd-0b87f74ea6cd}";
            //    var type = Type.GetTypeFromCLSID(Guid.Parse(clsid));
            //    var obj = Activator.CreateInstance(type, true);

            //    var a = type.GetMethod("GetFieldDescriptorCount");

            //    ((ICredentialProvider)obj).GetFieldDescriptorCount(out uint count);

            //    Console.WriteLine(count);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("ERROR: " + ex.Message);
            //}

            CredentialProviderWrappedSample s = new CredentialProviderWrappedSample();

            s.GetFieldDescriptorCount(out uint count);

            Console.WriteLine(count);

            Console.ReadLine();
        }
        public enum HRESULT
        {
            S_OK = 0x00000000,
            S_FALSE = 0x00000001,
            E_ACCESSDENIED = unchecked((int)0x80070005),
            E_FAIL = unchecked((int)0x80004005),
            E_INVALIDARG = unchecked((int)0x80070057),
            E_OUTOFMEMORY = unchecked((int)0x8007000E),
            E_POINTER = unchecked((int)0x80004003),
            E_UNEXPECTED = unchecked((int)0x8000FFFF),
            E_ABORT = unchecked((int)0x80004004),
            E_HANDLE = unchecked((int)0x80070006),
            E_NOINTERFACE = unchecked((int)0x80004002),
            E_NOTIMPL = unchecked((int)0x80004001)
        }
    }
}