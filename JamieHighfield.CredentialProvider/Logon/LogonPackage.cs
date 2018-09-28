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

using JamieHighfield.CredentialProvider.Interfaces;
using System;

namespace JamieHighfield.CredentialProvider.Logon
{
    public sealed class LogonPackage
    {
        public LogonPackage(ICurrentEnvironment environment)
        {
            Environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        #region Variables



        #endregion

        #region Properties

        public ICurrentEnvironment Environment { get; private set; }

        #endregion

        #region Methods



        #endregion
    }
}