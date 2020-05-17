using JamieHighfield.CredentialProvider.Credentials;
using JamieHighfield.CredentialProvider.Interop;
using System;

namespace JamieHighfield.CredentialProvider.Controls.New
{
    public abstract class CredentialControlBase
    {
        protected CredentialControlBase(CredentialBase credential, CredentialFieldVisibilities visibility, bool forwardToField)
        {
            Credential = credential ?? throw new ArgumentNullException(nameof(credential));
            ForwardToField = forwardToField;

            _visibility = new DynamicPropertyStore<CredentialFieldVisibilities>(this, (_) => visibility);
        }

        private DynamicPropertyStore<CredentialFieldVisibilities> _visibility = null;

        /// <summary>
        /// Gets the <see cref="CredentialBase"/> that underpins this control.
        /// </summary>
        public CredentialBase Credential { get; }

        /// <summary>
        /// Gets the visibility for this control;
        /// </summary>
        public CredentialFieldVisibilities Visibility
        {
            get
            {
                return _visibility.Value;
            }
            set
            {
                _visibility.Value = value;

                //TODO TextChanged?.Invoke(this, new LabelControlTextChangedEventArgs(Credential, this));

                Credential?.Events?.SetFieldState(Credential, (uint)Field.FieldId, (_CREDENTIAL_PROVIDER_FIELD_STATE)(int)Visibility);
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="CredentialField"/> assigned to this control.
        /// </summary>
        internal CredentialField Field { get; set; }

        /// <summary>
        /// Gets a value indicating whether value stores should be redirected to the field for processing.
        /// </summary>
        public bool ForwardToField { get; }
    }
}