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

using JamieHighfield.CredentialProvider.Credentials;
using JamieHighfield.CredentialProvider.Interfaces;
using System;

namespace JamieHighfield.CredentialProvider.Logon
{
    public sealed class IncomingLogonPackage
    {
        public IncomingLogonPackage(ICurrentEnvironment environment, CredentialBase credential)
        {
            Environment = environment ?? throw new ArgumentNullException(nameof(environment));
            Credential = credential ?? throw new ArgumentNullException(nameof(credential));
        }

        #region Variables



        #endregion

        #region Properties

        public ICurrentEnvironment Environment { get; private set; }

        public CredentialBase Credential { get; private set; }

        #endregion

        #region Methods



        #endregion
    }
}