using JamieHighfield.CredentialProvider.Interop;

namespace JamieHighfield.CredentialProvider.Controls.New
{
    internal class NewCredentialField
    {
        public NewCredentialField(int fieldId, _CREDENTIAL_PROVIDER_FIELD_DESCRIPTOR fieldDescriptor, _CREDENTIAL_PROVIDER_FIELD_STATE fieldState)
        {
            FieldId = fieldId;
            FieldDescriptor = fieldDescriptor;
            FieldState = fieldState;
        }

        /// <summary>
        /// Gets the ID for this field.
        /// </summary>
        internal int FieldId { get; }

        /// <summary>
        /// Gets the field descriptor for this field.
        /// </summary>
        internal _CREDENTIAL_PROVIDER_FIELD_DESCRIPTOR FieldDescriptor { get; }

        /// <summary>
        /// Gets the field state for this field.
        /// </summary>
        internal _CREDENTIAL_PROVIDER_FIELD_STATE FieldState { get; }
    }
}