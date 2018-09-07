using CredProvider.NET.Interop2;
using JamieHighfield.CredentialProvider.Fields.Events;
using System;

namespace JamieHighfield.CredentialProvider.Fields
{
    public sealed class TextField : CredentialFieldBase
    {
        public TextField(string text, TextFieldSizes size)
            : this(text, size, CredentialFieldStates.SelectedCredential)
        { }

        public TextField(string text, TextFieldSizes size, CredentialFieldStates state)
            : base(CredentialFieldTypes.Text, state)
        {
            Text = text;
            Size = size;
        }

        public TextField(string text, TextFieldSizes size, EventHandler<TextFieldTextChangedEventArgs> textChanged)
            : this(text, size, textChanged, CredentialFieldStates.SelectedCredential)
        { }

        public TextField(string text, TextFieldSizes size, EventHandler<TextFieldTextChangedEventArgs> textChanged, CredentialFieldStates state)
            : base(CredentialFieldTypes.TextBox, state)
        {
            Text = text;
            Size = size;
            TextChanged += textChanged;
        }

        #region Variables

        private string _text;

        #endregion

        #region Properties

        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;

                TextChanged?.Invoke(this, new TextFieldTextChangedEventArgs(this));

                Credential?.Events?.SetFieldString(Credential, (uint)FieldId, Text);
            }
        }

        public TextFieldSizes Size { get; private set; }

        #endregion

        #region Methods

        internal override _CREDENTIAL_PROVIDER_FIELD_TYPE GetNativeFieldType()
        {
            if (Size == TextFieldSizes.Small)
            {
                return _CREDENTIAL_PROVIDER_FIELD_TYPE.CPFT_SMALL_TEXT;
            }
            else
            {
                return _CREDENTIAL_PROVIDER_FIELD_TYPE.CPFT_LARGE_TEXT;
            }
        }

        internal void UpdateText(string text)
        {
            _text = text;

            TextChanged?.Invoke(this, new TextFieldTextChangedEventArgs(this));
        }

        #endregion

        #region Events

        public event EventHandler<TextFieldTextChangedEventArgs> TextChanged;

        #endregion
    }
}