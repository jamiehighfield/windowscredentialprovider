using JamieHighfield.CredentialProvider.Controls.Events;
using JamieHighfield.CredentialProvider.Credentials;
using JamieHighfield.CredentialProvider.Interop;
using System;

namespace JamieHighfield.CredentialProvider.Controls.New
{
    public sealed class NewLabelControl : NewCredentialControlBase
    {
        internal NewLabelControl(CredentialBase credential, CredentialFieldVisibilities visibility, Func<CredentialBase, string> text, LabelControlSizes size, Func<CredentialBase, EventHandler<LabelControlTextChangedEventArgs>> textChanged)
            : base(credential, visibility)
        {
            Size = size;

            _text = new NewDynamicPropertyStore<string>(this, text);
            _textChanged = new NewDynamicPropertyStore<EventHandler<LabelControlTextChangedEventArgs>>(this, textChanged);
        }

        private NewDynamicPropertyStore<string> _text = null;

        private NewDynamicPropertyStore<EventHandler<LabelControlTextChangedEventArgs>> _textChanged = null;
        
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

        internal override _CREDENTIAL_PROVIDER_FIELD_TYPE GetNativeFieldType()
        {
            if (Size == LabelControlSizes.Small)
            {
                return _CREDENTIAL_PROVIDER_FIELD_TYPE.CPFT_SMALL_TEXT;
            }
            else
            {
                return _CREDENTIAL_PROVIDER_FIELD_TYPE.CPFT_LARGE_TEXT;
            }
        }
    }
}