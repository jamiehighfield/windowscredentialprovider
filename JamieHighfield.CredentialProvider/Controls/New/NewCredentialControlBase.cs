using System;
using JamieHighfield.CredentialProvider.Credentials;
using JamieHighfield.CredentialProvider.Interop;

namespace JamieHighfield.CredentialProvider.Controls.New
{
    public abstract class NewCredentialControlBase
    {
        protected NewCredentialControlBase(CredentialBase credential, CredentialFieldVisibilities visibility)
        {
            Credential = credential ?? throw new ArgumentNullException(nameof(credential));
            Visibility = visibility;
        }

        /// <summary>
        /// Gets the <see cref="CredentialBase"/> that underpins this control.
        /// </summary>
        public CredentialBase Credential { get; }

        /// <summary>
        /// Gets the visibility for this control;
        /// </summary>
        public CredentialFieldVisibilities Visibility { get; }

        /// <summary>
        /// Gets or sets the <see cref="CredentialField"/> assigned to this control.
        /// </summary>
        internal CredentialField Field { get; set; }
        
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
            _CREDENTIAL_PROVIDER_FIELD_STATE visibility = _CREDENTIAL_PROVIDER_FIELD_STATE.CPFS_HIDDEN;

            if (Visibility.HasFlag(CredentialFieldVisibilities.SelectedCredential) == true)
            {
                if (Visibility.HasFlag(CredentialFieldVisibilities.DeselectedCredential) == true)
                {
                    visibility = _CREDENTIAL_PROVIDER_FIELD_STATE.CPFS_DISPLAY_IN_BOTH;
                }
                else
                {
                    visibility = _CREDENTIAL_PROVIDER_FIELD_STATE.CPFS_DISPLAY_IN_SELECTED_TILE;
                }
            }
            else if (Visibility.HasFlag(CredentialFieldVisibilities.DeselectedCredential) == true)
            {
                visibility = _CREDENTIAL_PROVIDER_FIELD_STATE.CPFS_DISPLAY_IN_DESELECTED_TILE;
            }

            return visibility;
        }
    }
}