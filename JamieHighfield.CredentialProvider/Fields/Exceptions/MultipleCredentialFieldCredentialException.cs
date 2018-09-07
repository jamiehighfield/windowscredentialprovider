using System;

namespace JamieHighfield.CredentialProvider.Fields.Exceptions
{
    public sealed class MultipleCredentialFieldCredentialException : Exception
    {
        public MultipleCredentialFieldCredentialException(CredentialFieldBase field)
            : base("The field '" + nameof(field) + "' cannot belong to more than one credential.")
        {
            Field = field;
        }

        #region Variables



        #endregion

        #region Properties

        public CredentialFieldBase Field { get; private set; }

        #endregion

        #region Methods



        #endregion
    }
}