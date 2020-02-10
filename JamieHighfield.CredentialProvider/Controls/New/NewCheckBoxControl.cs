using JamieHighfield.CredentialProvider.Controls.Events;
using JamieHighfield.CredentialProvider.Credentials;
using JamieHighfield.CredentialProvider.Interop;
using System;
using System.Windows.Forms;

namespace JamieHighfield.CredentialProvider.Controls.New
{
    public sealed class NewCheckBoxControl : NewCredentialControlBase
    {
        internal NewCheckBoxControl(CredentialBase credential, CredentialFieldVisibilities visibility, string label, Func<CredentialBase, bool> @checked, Func<CredentialBase, EventHandler<CheckBoxControlCheckChangedEventArgs>> checkChanged)
            : base(credential, visibility)
        {
            Label = label ?? throw new ArgumentNullException(nameof(label));

            _checked = new NewDynamicPropertyStore<bool>(this, @checked);
            _checkChanged = new NewDynamicPropertyStore<EventHandler<CheckBoxControlCheckChangedEventArgs>>(this, checkChanged);
        }

        private NewDynamicPropertyStore<bool> _checked = null;

        private NewDynamicPropertyStore<EventHandler<CheckBoxControlCheckChangedEventArgs>> _checkChanged = null;

        /// <summary>
        /// Gets the label for this control.
        /// </summary>
        public string Label { get; }

        /// <summary>
        /// Gets the check state for this control.
        /// </summary>
        public bool Checked
        {
            get
            {
                return _checked.Value;
            }
            set
            {
                _checked.Value = value;

                CheckChanged?.Invoke(this, new CheckBoxControlCheckChangedEventArgs(Credential, this));

                Credential?.Events?.SetFieldCheckbox(Credential, (uint)Field.FieldId, (Checked == true ? 1 : 0), Label);
            }
        }

        /// <summary>
        /// Gets the text changed event handler for this control.
        /// </summary>
        public EventHandler<CheckBoxControlCheckChangedEventArgs> CheckChanged
        {
            get
            {
                return _checkChanged.Value;
            }
        }

        internal override _CREDENTIAL_PROVIDER_FIELD_TYPE GetNativeFieldType()
        {
            return _CREDENTIAL_PROVIDER_FIELD_TYPE.CPFT_CHECKBOX;
        }
    }
}