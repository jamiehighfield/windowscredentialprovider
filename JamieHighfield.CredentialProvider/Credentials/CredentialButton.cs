using CredProvider.NET.Interop2;
using System;

namespace JamieHighfield.CredentialProvider.Credentials
{
    public sealed class CredentialButton
    {
        public CredentialButton(string label)
        {
            Label = label ?? throw new ArgumentNullException(nameof(label));
        }

        #region Variables



        #endregion

        #region Properties

        internal int FieldId { get; set; }

        public string Label { get; private set; }

        #endregion

        #region Methods

        internal _CREDENTIAL_PROVIDER_FIELD_DESCRIPTOR GetFieldDescriptor()
        {
            _CREDENTIAL_PROVIDER_FIELD_TYPE type = _CREDENTIAL_PROVIDER_FIELD_TYPE.CPFT_SUBMIT_BUTTON;

            return new _CREDENTIAL_PROVIDER_FIELD_DESCRIPTOR()
            {
                dwFieldID = (uint)FieldId,
                cpft = type,
                pszLabel = Label,
                guidFieldType = default(Guid)
            };
        }

        internal _CREDENTIAL_PROVIDER_FIELD_STATE GetFieldState()
        {
            _CREDENTIAL_PROVIDER_FIELD_STATE state = _CREDENTIAL_PROVIDER_FIELD_STATE.CPFS_DISPLAY_IN_SELECTED_TILE;

            return state;
        }

        #endregion
    }
}