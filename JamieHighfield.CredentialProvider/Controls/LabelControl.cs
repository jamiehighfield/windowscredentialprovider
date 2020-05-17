using JamieHighfield.CredentialProvider.Controls.Events;
using JamieHighfield.CredentialProvider.Credentials;
using JamieHighfield.CredentialProvider.Interop;
using System;

namespace JamieHighfield.CredentialProvider.Controls.New
{
    public sealed class LabelControl : CredentialControlBase
    {
        internal LabelControl(CredentialBase credential, CredentialFieldVisibilities visibility, bool forwardToField, Func<LabelControl, Func<CredentialBase, LabelControl, string>> text, LabelControlSizes size, Func<LabelControl, Func<CredentialBase, LabelControl, EventHandler<LabelControlTextChangedEventArgs>>> textChanged)
            : base(credential, visibility, forwardToField)
        {
            Size = size;

            if (text is null)
            {
                _text = new DynamicPropertyStore<string>(this, (innerCredential) => string.Empty);
            }
            else
            {
                _text = new DynamicPropertyStore<string>(this, (innerCredential) => (text.Invoke(this)?.Invoke(innerCredential, this) ?? ""));
            }

            if (textChanged is null)
            {
                _textChanged = new DynamicPropertyStore<EventHandler<LabelControlTextChangedEventArgs>>(this, (innerCredential) => null);
            }
            else
            {
                _textChanged = new DynamicPropertyStore<EventHandler<LabelControlTextChangedEventArgs>>(this, (innerCredential) => textChanged.Invoke(this)?.Invoke(innerCredential, this));
            }
        }

        private DynamicPropertyStore<string> _text = null;

        private DynamicPropertyStore<EventHandler<LabelControlTextChangedEventArgs>> _textChanged = null;
        
        /// <summary>
        /// Gets or sets the text for this control.
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

                TextChanged?.Invoke(this, new LabelControlTextChangedEventArgs(Credential, this));

                Credential?.Events?.SetFieldString(Credential, (uint)Field.FieldId, Text);
            }
        }

        internal void UpdateText(string text)
        {
            _text.Value = text ?? throw new ArgumentNullException(nameof(text));

            TextChanged?.Invoke(this, new LabelControlTextChangedEventArgs(Credential, this));
        }

        /// <summary>
        /// Gets the size for this control.
        /// </summary>
        public LabelControlSizes Size { get; }

        /// <summary>
        /// Gets the text changed event handler for this control.
        /// </summary>
        public EventHandler<LabelControlTextChangedEventArgs> TextChanged
        {
            get
            {
                return _textChanged.Value;
            }
        }
    }
}