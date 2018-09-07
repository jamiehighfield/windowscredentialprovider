using CredProvider.NET.Interop2;
using System;

namespace JamieHighfield.CredentialProvider.Fields
{
    public abstract class LabelledCredentialFieldBase : CredentialFieldBase
    {
        internal LabelledCredentialFieldBase(CredentialFieldTypes type, string label)
            : this(type, label, CredentialFieldStates.SelectedCredential)
        { }

        internal LabelledCredentialFieldBase(CredentialFieldTypes type, string label, CredentialFieldStates state)
            : base(type, state)
        {
            Label = label;
        }

        #region Variables



        #endregion

        #region Properties

        public string Label { get; private set; }

        #endregion

        #region Methods
        
        internal override _CREDENTIAL_PROVIDER_FIELD_DESCRIPTOR GetFieldDescriptor()
        {
            _CREDENTIAL_PROVIDER_FIELD_TYPE type = GetNativeFieldType();

            return new _CREDENTIAL_PROVIDER_FIELD_DESCRIPTOR()
            {
                dwFieldID = (uint)FieldId,
                cpft = type,
                pszLabel = Label,
                guidFieldType = default(Guid)
            };
        }

        #endregion
    }
}