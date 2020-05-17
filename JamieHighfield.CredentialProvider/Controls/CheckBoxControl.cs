using JamieHighfield.CredentialProvider.Controls.Events;
using JamieHighfield.CredentialProvider.Credentials;
using JamieHighfield.CredentialProvider.Interop;
using System;
using System.Windows.Forms;

namespace JamieHighfield.CredentialProvider.Controls.New
{
    public sealed class CheckBoxControl : CredentialControlBase
    {
        internal CheckBoxControl(CredentialBase credential, CredentialFieldVisibilities visibility, bool forwardToField, string label, Func<CheckBoxControl, Func<CredentialBase, CheckBoxControl, bool>> @checked, Func<CheckBoxControl, Func<CredentialBase, CheckBoxControl, EventHandler<CheckBoxControlCheckChangedEventArgs>>> checkChanged)
            : base(credential, visibility, forwardToField)
        {
            Label = label ?? throw new ArgumentNullException(nameof(label));

            if (@checked is null)
            {
                _checked = new DynamicPropertyStore<bool>(this, (innerCredential) => false);
            }
            else
            {
                _checked = new DynamicPropertyStore<bool>(this, (innerCredential) => (@checked.Invoke(this)?.Invoke(innerCredential, this) ?? false));
            }

            if (checkChanged is null)
            {
                _checkChanged = new DynamicPropertyStore<EventHandler<CheckBoxControlCheckChangedEventArgs>>(this, (innerCredential) => null);
            }
            else
            {
                _checkChanged = new DynamicPropertyStore<EventHandler<CheckBoxControlCheckChangedEventArgs>>(this, (innerCredential) => checkChanged.Invoke(this)?.Invoke(innerCredential, this));
            }
        }

        private DynamicPropertyStore<bool> _checked = null;

        private DynamicPropertyStore<EventHandler<CheckBoxControlCheckChangedEventArgs>> _checkChanged = null;

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
    }
}