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

namespace JamieHighfield.CredentialProvider.Credentials.Events
{
    public sealed class LoadedEventArgs
    {
        public LoadedEventArgs(bool autoLogon)
        {
            AutoLogon = autoLogon;
            WindowsLogon = true;
        }

        #region Variables



        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets whether this credential should automatically logon.
        /// </summary>
        public bool AutoLogon { get; set; }

        /// <summary>
        /// Gets or sets whether this credential should forward an authentication package to Windows. This property will be ignored if this credential forms part of a wrapped credential.
        /// </summary>
        public bool WindowsLogon { get; set; }

        #endregion

        #region Methods



        #endregion
    }
}