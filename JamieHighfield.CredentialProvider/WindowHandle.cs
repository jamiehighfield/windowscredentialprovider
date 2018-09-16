/* COPYRIGHT NOTICE
 * 
 * Copyright © Jamie Highfield 2018. All rights reserved.
 * 
 * This library is protected by UK, EU & international copyright laws and treaties. Unauthorised
 * reproduction of this library outside of the constraints of the accompanied license, or any
 * portion of it, may result in severe criminal penalties that will be prosecuted to the
 * maximum extent possible under the law.
 * 
 */

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