using CredProvider.NET.Interop2;
using System;

namespace JamieHighfield.CredentialProvider.Controls
{
    public abstract class LabelledCredentialControlBase : CredentialControlBase
    {
        internal LabelledCredentialControlBase(CredentialFieldTypes type, string label)
            : this(type, label, CredentialFieldVisibilities.SelectedCredential)
        { }

        internal LabelledCredentialControlBase(CredentialFieldTypes type, string label, CredentialFieldVisibilities visibility)
            : base(type, visibility)
        {
            Label = label;
        }

        #region Variables



        #endregion

        #region Properties

        public string Label { get; private set; }

        #endregion

        #region Methods
        
        internal override _CREDENTIAL_PROVIDER_FIELD_DESCRIPTOR GetFieldDescriptor(int fieldId)
        {
            _CREDENTIAL_PROVIDER_FIELD_TYPE type = GetNativeFieldType();

            return new _CREDENTIAL_PROVIDER_FIELD_DESCRIPTOR()
            {
                dwFieldID = (uint)fieldId,
                cpft = type,
                pszLabel = Label,
                guidFieldType = default(Guid)
            };
        }

        #endregion
    }
}