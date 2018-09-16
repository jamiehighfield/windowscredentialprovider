using JamieHighfield.CredentialProvider.Providers;
using System;
using System.Collections.Generic;

namespace JamieHighfield.CredentialProvider.Credentials
{
    public sealed class CredentialCollection : List<CredentialBase>
    {
        internal CredentialCollection(CredentialProviderBase credentialProvider)
        {
            CredentialProvider = credentialProvider;
        }

        #region Variables



        #endregion

        #region Properties

        private CredentialProviderBase CredentialProvider { get; set; }

        #endregion

        #region Methods

        public new CredentialCollection Add(CredentialBase credential)
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