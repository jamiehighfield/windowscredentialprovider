using CredProvider.NET.Interop2;
using JamieHighfield.CredentialProvider.Fields;
using System;
using System.Drawing;

namespace JamieHighfield.CredentialProvider.Credentials
{
    public sealed class CredentialImage
    {
        public CredentialImage(Bitmap image)
            : this(image, CredentialFieldStates.Both)
        { }

        public CredentialImage(Bitmap image, CredentialFieldStates state)
        {
            Image = image ?? throw new ArgumentNullException(nameof(image));
            State = state;
        }

        #region Variables



        #endregion

        #region Properties

        public Bitmap Image { get; private set; }

        public CredentialFieldStates State { get; private set; }

        #endregion

        #region Methods
        
        internal _CREDENTIAL_PROVIDER_FIELD_DESCRIPTOR GetFieldDescriptor()
        {
            _CREDENTIAL_PROVIDER_FIELD_TYPE type = _CREDENTIAL_PROVIDER_FIELD_TYPE.CPFT_TILE_IMAGE;

            return new _CREDENTIAL_PROVIDER_FIELD_DESCRIPTOR()
            {
                dwFieldID = 0,
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