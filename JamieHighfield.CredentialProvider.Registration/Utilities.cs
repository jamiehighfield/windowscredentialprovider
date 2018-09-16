using System;
using System.Runtime.InteropServices;

namespace JamieHighfield.CredentialProvider.Registration
{
    public static class Utilities
    {
        #region Platform Invocation

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct CREDUI_INFO
        {
            public int cbSize;
            public IntPtr hwndParent;
            public string pszMessageText;
            public string pszCaptionText;
            public IntPtr hbmBanner;
        }

        [DllImport("credui.dll", CharSet = CharSet.Auto)]
        private static extern int CredUIPromptForWindowsCredentials(ref CREDUI_INFO uiInfo, int authError, ref uint authPackage, IntPtr InAuthBuffer, uint InAuthBufferSize, out IntPtr refOutAuthBuffer, out uint refOutAuthBufferSize, ref bool fSave, PromptForWindowsCredentialsFlags flags);
        
        [DllImport("ole32.dll")]
        private static extern void CoTaskMemFree(IntPtr ptr);

        #endregion

        #region Variables



        #endregion

        #region Properties



        #endregion

        #region Methods

        public static void ShowCredentialsDialog()
        {
            CREDUI_INFO uiInfo = new CREDUI_INFO();
            uiInfo.cbSize = Marshal.SizeOf(uiInfo);
            uiInfo.pszCaptionText = "Example";
            uiInfo.pszMessageText = "Example message.";

            uint authenticationPackage = 0;

            bool save = false;

            int result = CredUIPromptForWindowsCredentials(ref uiInfo, 0, ref authenticationPackage, IntPtr.Zero, 0, out IntPtr credentialBuffer, out uint credentialSize, ref save, 0);

            CoTaskMemFree(credentialBuffer);
        }

        #endregion
    }

    internal enum PromptForWindowsCredentialsFlags
    {
        CREDUIWIN_GENERIC = 0x1,
        CREDUIWIN_CHECKBOX = 0x2,
        CREDUIWIN_AUTHPACKAGE_ONLY = 0x10,
        CREDUIWIN_IN_CRED_ONLY = 0x20,
        CREDUIWIN_ENUMERATE_ADMINS = 0x100,
        CREDUIWIN_ENUMERATE_CURRENT_USER = 0x200,
        CREDUIWIN_SECURE_PROMPT = 0x1000,
        CREDUIWIN_PACK_32_WOW = 0x10000000,
    }
}