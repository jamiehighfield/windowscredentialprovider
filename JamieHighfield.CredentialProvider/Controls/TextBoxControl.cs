using JamieHighfield.CredentialProvider.Controls.Events;
using JamieHighfield.CredentialProvider.Credentials;
using System;
using static JamieHighfield.CredentialProvider.Constants;

namespace JamieHighfield.CredentialProvider.Controls.New
{
    public sealed class TextBoxControl : CredentialControlBase
    {
        internal TextBoxControl(CredentialBase credential, CredentialFieldVisibilities visibility, bool forwardToField, string label, Func<TextBoxControl, Func<CredentialBase, TextBoxControl, string>> text, Func<TextBoxControl, Func<CredentialBase, TextBoxControl, bool>> focussed, Func<TextBoxControl, Func<CredentialBase, TextBoxControl, EventHandler<TextBoxControlTextChangedEventArgs>>> textChanged)
            : base(credential, visibility, forwardToField)
        {
            Label = label ?? throw new ArgumentNullException(nameof(label));

            if (text is null)
            {
                _text = new DynamicPropertyStore<string>(this, (innerCredential) => string.Empty);
            }
            else
            {
                _text = new DynamicPropertyStore<string>(this, (innerCredential) => (text.Invoke(this)?.Invoke(innerCredential, this) ?? ""));
            }

            if (focussed is null)
            {
                _focussed = new DynamicPropertyStore<bool>(this, (innerCredential) => false);
            }
            else
            {
                _focussed = new DynamicPropertyStore<bool>(this, (innerCredential) => (focussed.Invoke(this)?.Invoke(innerCredential, this) ?? false));
            }

            if (textChanged is null)
            {
                _textChanged = new DynamicPropertyStore<EventHandler<TextBoxControlTextChangedEventArgs>>(this, (innerCredential) => null);
            }
            else
            {
                _textChanged = new DynamicPropertyStore<EventHandler<TextBoxControlTextChangedEventArgs>>(this, (innerCredential) => textChanged.Invoke(this)?.Invoke(innerCredential, this));
            }
        }

        private DynamicPropertyStore<string> _text = null;

        private DynamicPropertyStore<bool> _focussed = null;

        private DynamicPropertyStore<EventHandler<TextBoxControlTextChangedEventArgs>> _textChanged = null;

        /// <summary>
        /// Gets the label for this control.
        /// </summary>
        public string Label { get; }

        /// <summary>
        /// Gets or sets the text for this control.
        /// </summary>
        public string Text
        {
            get
            {
                if (ForwardToField)
                {
                    int result = Credential.GetStringValue((uint)Field.FieldId, out string value);

                    if (result != HRESULT.S_OK)
                    {
                        return null;
                    }

                    return value;
                }
                else
                {
                    return _text.Value;
                }
            }
            set
            {
                if (ForwardToField == false)
                {
                    _text.Value = value;
                }

                TextChanged?.Invoke(this, new TextBoxControlTextChangedEventArgs(Credential, this));

                Credential?.Events?.SetFieldString(Credential, (uint)Field.FieldId, value);
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
    }
}