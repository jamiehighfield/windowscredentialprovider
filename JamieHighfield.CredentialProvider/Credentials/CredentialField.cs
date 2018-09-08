using CredProvider.NET.Interop2;
using JamieHighfield.CredentialProvider.Controls;
using System;

namespace JamieHighfield.CredentialProvider.Credentials
{
    public sealed class CredentialField
    {
        internal CredentialField(CredentialControlBase control, int fieldId)
        {
            Control = control ?? throw new ArgumentNullException(nameof(control));
            FieldId = fieldId;
        }

        #region Variables



        #endregion

        #region Properties

        internal CredentialControlBase Control { get; private set; }

        internal int FieldId { get; private set; }

        #endregion

        #region Methods

        internal _CREDENTIAL_PROVIDER_FIELD_DESCRIPTOR GetDescriptor()
        {
            return Control.GetFieldDescriptor(FieldId);
        }

        internal _CREDENTIAL_PROVIDER_FIELD_STATE GetState()
        {
            return Control.GetFieldState();
        }

        #endregion
    }
}