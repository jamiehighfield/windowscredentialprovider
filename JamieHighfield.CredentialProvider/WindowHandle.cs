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

namespace JamieHighfield.CredentialProvider
{
    /// <summary>
    /// A wrapper class around <see cref="IWin32Window"/>; an interface used by <see cref="Form"/> and <see cref="MessageBox"/> classes, among others to set the parent window.
    /// </summary>
    public sealed class WindowHandle : IWin32Window
    {
        /// <summary>
        /// Instantiate a new <see cref="WindowHandle"/>.
        /// </summary>
        /// <param name="handle">The underlying <see cref="IntPtr"/> window handle.</param>
        public WindowHandle(IntPtr handle)
        {
            Handle = handle;
        }

        #region Variables



        #endregion

        #region Properties

        /// <summary>
        /// Gets the underlying <see cref="IntPtr"/> window handle.
        /// </summary>
        public IntPtr Handle { get; private set; }

        #endregion

        #region Methods



        #endregion
    }
}