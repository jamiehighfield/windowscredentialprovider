using CredProvider.NET.Interop2;
using JamieHighfield.CredentialProvider.Credentials;
using System;

namespace JamieHighfield.CredentialProvider.Fields
{
    public abstract class CredentialFieldBase
    {
        internal CredentialFieldBase(CredentialFieldTypes type)
            : this(type, CredentialFieldStates.SelectedCredential)
        { }

        internal CredentialFieldBase(CredentialFieldTypes type, CredentialFieldStates state)
        {
            Type = type;
            State = state;
        }

        #region Variables



        #endregion

        #region Properties

        internal CredentialBase Credential { get; set; }

        internal int FieldId { get; set; }

        public CredentialFieldTypes Type { get; private set; }
        
        public CredentialFieldStates State { get; private set; }

        #endregion

        #region Methods

        internal abstract _CREDENTIAL_PROVIDER_FIELD_TYPE GetNativeFieldType();

        internal virtual _CREDENTIAL_PROVIDER_FIELD_DESCRIPTOR GetFieldDescriptor()
        {
            _CREDENTIAL_PROVIDER_FIELD_TYPE type = GetNativeFieldType();

            return new _CREDENTIAL_PROVIDER_FIELD_DESCRIPTOR()
            {
                dwFieldID = (uint)FieldId,
                cpft = type,
                pszLabel = string.Empty,
                guidFieldType = default(Guid)
            };
        }

        internal _CREDENTIAL_PROVIDER_FIELD_STATE GetFieldState()
        {
            _CREDENTIAL_PROVIDER_FIELD_STATE state = _CREDENTIAL_PROVIDER_FIELD_STATE.CPFS_HIDDEN;

            if (State.HasFlag(CredentialFieldStates.SelectedCredential) == true)
            {
                if (State.HasFlag(CredentialFieldStates.DeselectedCredential) == true)
                {
                    state = _CREDENTIAL_PROVIDER_FIELD_STATE.CPFS_DISPLAY_IN_BOTH;
                }
                else
                {
                    state = _CREDENTIAL_PROVIDER_FIELD_STATE.CPFS_DISPLAY_IN_SELECTED_TILE;
                }
            }
            else if (State.HasFlag(CredentialFieldStates.DeselectedCredential) == true)
            {
                state = _CREDENTIAL_PROVIDER_FIELD_STATE.CPFS_DISPLAY_IN_DESELECTED_TILE;
            }

            return state;
        }

        #endregion
    }
}