using JamieHighfield.CredentialProvider.Credentials;
using JamieHighfield.CredentialProvider.Fields;
using JamieHighfield.CredentialProvider.Fields.Exceptions;
using System;
using System.Collections.Generic;

namespace JamieHighfield.CredentialProvider
{
    public sealed class CredentialFieldCollection : List<CredentialFieldBase>
    {
        public CredentialFieldCollection(CredentialBase credential)
        {
            Credential = credential;
        }

        #region Variables



        #endregion

        #region Properties

        private CredentialBase Credential { get; set; }

        #endregion

        #region Methods

        public new CredentialFieldCollection Add(CredentialFieldBase field)
        {
            if (field == null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            if (field.Credential != null)
            {
                throw new MultipleCredentialFieldCredentialException(field);
            }

            field.Credential = Credential;

            field.FieldId = (Credential.Image == null ? Count : (Count + 1));

            base.Add(field);

            return this;
        }

        #endregion
    }
}