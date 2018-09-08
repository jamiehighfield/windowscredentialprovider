using CredProvider.NET.Interop2;
using JamieHighfield.CredentialProvider.Credentials;
using System;

namespace JamieHighfield.CredentialProvider.Controls
{
    public abstract class CredentialControlBase
    {
        internal CredentialControlBase(CredentialFieldTypes type)
            : this(type, CredentialFieldVisibilities.SelectedCredential)
        { }

        internal CredentialControlBase(CredentialFieldTypes type, CredentialFieldVisibilities visibility)
        {
            Type = type;
            State = visibility;
        }

        #region Variables



        #endregion

        #region Properties

        internal Action<Action<CredentialBase, int>> EventCallback { get; set; }

        internal CredentialFieldTypes Type { get; private set; }
        
        public CredentialFieldVisibilities State { get; private set; }

        #endregion

        #region Methods

        internal abstract _CREDENTIAL_PROVIDER_FIELD_TYPE GetNativeFieldType();

        internal virtual _CREDENTIAL_PROVIDER_FIELD_DESCRIPTOR GetFieldDescriptor(int fieldId)
        {
            _CREDENTIAL_PROVIDER_FIELD_TYPE type = GetNativeFieldType();

            return new _CREDENTIAL_PROVIDER_FIELD_DESCRIPTOR()
            {
                dwFieldID = (uint)fieldId,
                cpft = type,
                pszLabel = string.Empty,
                guidFieldType = default(Guid)
            };
        }

        internal _CREDENTIAL_PROVIDER_FIELD_STATE GetFieldState()
        {
            _CREDENTIAL_PROVIDER_FIELD_STATE state = _CREDENTIAL_PROVIDER_FIELD_STATE.CPFS_HIDDEN;

            if (State.HasFlag(CredentialFieldVisibilities.SelectedCredential) == true)
            {
                if (State.HasFlag(CredentialFieldVisibilities.DeselectedCredential) == true)
                {
                    state = _CREDENTIAL_PROVIDER_FIELD_STATE.CPFS_DISPLAY_IN_BOTH;
                }
                else
                {
                    state = _CREDENTIAL_PROVIDER_FIELD_STATE.CPFS_DISPLAY_IN_SELECTED_TILE;
                }
            }
            else if (State.HasFlag(CredentialFieldVisibilities.DeselectedCredential) == true)
            {
                state = _CREDENTIAL_PROVIDER_FIELD_STATE.CPFS_DISPLAY_IN_DESELECTED_TILE;
            }

            return state;
        }

        #endregion
    }
}