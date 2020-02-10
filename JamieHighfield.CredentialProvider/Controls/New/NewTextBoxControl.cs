using JamieHighfield.CredentialProvider.Controls.Events;
using JamieHighfield.CredentialProvider.Credentials;
using JamieHighfield.CredentialProvider.Interop;
using System;

namespace JamieHighfield.CredentialProvider.Controls.New
{
    public sealed class NewTextBoxControl : NewCredentialControlBase
    {
        internal NewTextBoxControl(CredentialBase credential, CredentialFieldVisibilities visibility, string label, Func<CredentialBase, string> text,  Func<CredentialBase, bool> focussed, Func<CredentialBase, EventHandler<TextBoxControlTextChangedEventArgs>> textChanged)
            : base(credential, visibility)
        {
            Label = label ?? throw new ArgumentNullException(nameof(label));

            _text = new NewDynamicPropertyStore<string>(this, text);
            _focussed = new NewDynamicPropertyStore<bool>(this, focussed);
            _textChanged = new NewDynamicPropertyStore<EventHandler<TextBoxControlTextChangedEventArgs>>(this, textChanged);
        }

        private NewDynamicPropertyStore<string> _text = null;

        private NewDynamicPropertyStore<bool> _focussed = null;

        private NewDynamicPropertyStore<EventHandler<TextBoxControlTextChangedEventArgs>> _textChanged = null;

        /// <summary>
        /// Gets the label for this control.
        /// </summary>
        public string Label { get; }

        /// <summary>
        /// Gets the text for this control.
        /// </summary>
        public string Text
        {
            get
            {
                return _text.Value;
            }
            set
            {
                _text.Value = value;

                TextChanged?.Invoke(this, new TextBoxControlTextChangedEventArgs(Credential, this));

                Credential?.Events?.SetFieldString(Credential, (uint)Field.FieldId, Text);
            }
        }

        public bool Focussed
        {
            get
            {
                return _focussed.Value;
            }
        }

        internal void UpdateText(string text)
        {
            _text.Value = text ?? throw new ArgumentNullException(nameof(text));

            TextChanged?.Invoke(this, new TextBoxControlTextChangedEventArgs(Credential, this));
        }

        /// <summary>
        /// Gets the text changed event handler for this control.
        /// </summary>
        public EventHandler<TextBoxControlTextChangedEventArgs> TextChanged
        {
            get
            {
                return _textChanged.Value;
            }
        }

        internal override _CREDENTIAL_PROVIDER_FIELD_TYPE GetNativeFieldType()
        {
            return _CREDENTIAL_PROVIDER_FIELD_TYPE.CPFT_EDIT_TEXT;
        }
    }
}