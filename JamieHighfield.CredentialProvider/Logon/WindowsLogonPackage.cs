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
using System.Net;
using System.Security;

namespace JamieHighfield.CredentialProvider.Logon
{
    public sealed class WindowsLogonPackage
    {
        public WindowsLogonPackage(string username, string password)
            : this(username, password, Environment.MachineName)
        { }

        public WindowsLogonPackage(string username, string password, string domain)
            : this(username, (new NetworkCredential(string.Empty, password)).SecurePassword, domain)
        { }

        public WindowsLogonPackage(string username, SecureString password)
            : this(username, password, Environment.MachineName)
        { }

        public WindowsLogonPackage(string username, SecureString password, string domain)
        {
            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            Username = username ?? throw new ArgumentNullException(nameof(username));
            Password = (new NetworkCredential(string.Empty, password)).SecurePassword;
            Domain = domain ?? throw new ArgumentNullException(nameof(domain));
        }

        #region Variables



        #endregion

        #region Properties

        public string Username { get; private set; }

        public SecureString Password { get; private set; }

        public string Domain { get; private set; }

        #endregion

        #region Methods



        #endregion
    }
}