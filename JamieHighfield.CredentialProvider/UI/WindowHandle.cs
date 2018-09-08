using System;
using System.Windows.Forms;

namespace JamieHighfield.CredentialProvider.UI
{
    public sealed class WindowHandle : IWin32Window
    {
        public WindowHandle(IntPtr handle)
        {
            Handle = handle;
        }

        #region Variables



        #endregion

        #region Properties

        public IntPtr Handle { get; private set; }

        #endregion

        #region Methods



        #endregion
    }
}