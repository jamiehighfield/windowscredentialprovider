using CredProvider.NET.Interop2;
using JamieHighfield.CredentialProvider.Controls;
using System;
using System.Drawing;

namespace JamieHighfield.CredentialProvider.Credentials
{
    public sealed class CredentialImage
    {
        public CredentialImage(Bitmap image)
            : this(image, CredentialFieldVisibilities.Both)
        { }

        public CredentialImage(Bitmap image, CredentialFieldVisibilities state)
        {
            Image = image ?? throw new ArgumentNullException(nameof(image));
            State = state;
        }

        #region Variables



        #endregion

        #region Properties

        public Bitmap Image { get; private set; }

        public CredentialFieldVisibilities State { get; private set; }

        #endregion

        #region Methods
        
        internal _CREDENTIAL_PROVIDER_FIELD_DESCRIPTOR GetFieldDescriptor(int fieldId)
        {
            _CREDENTIAL_PROVIDER_FIELD_TYPE type = _CREDENTIAL_PROVIDER_FIELD_TYPE.CPFT_TILE_IMAGE;

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