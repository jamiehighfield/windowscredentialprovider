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

using JamieHighfield.CredentialProvider.Providers;
using System;
using System.Collections.Generic;

namespace JamieHighfield.CredentialProvider.Credentials
{
    public sealed class CredentialCollection<TCredentialType> : List<TCredentialType>
        where TCredentialType : CredentialBase
    {
        internal CredentialCollection(CredentialProviderBase<TCredentialType> credentialProvider)
        {
            CredentialProvider = credentialProvider;
        }

        #region Variables



        #endregion

        #region Properties

        private CredentialProviderBase<TCredentialType> CredentialProvider { get; set; }

        #endregion

        #region Methods

        public new CredentialCollection<TCredentialType> Add(TCredentialType credential)
        {
            if (credential == null)
            {
                throw new ArgumentNullException(nameof(credential));
            }

            credential.CredentialProvider = CredentialProvider;

            base.Add(credential);

            return this;
        }

        #endregion
    }
}