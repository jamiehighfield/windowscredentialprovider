using CredProvider.NET.Interop2;
using JamieHighfield.CredentialProvider.Fields.Events;
using System;

namespace JamieHighfield.CredentialProvider.Fields
{
    public sealed class TextBoxField : LabelledCredentialFieldBase
    {
        public TextBoxField(string label, bool password)
            : this(label, password, CredentialFieldStates.SelectedCredential)
        { }

        public TextBoxField(string label, bool password, CredentialFieldStates state)
            : base(CredentialFieldTypes.TextBox, label, state)
        {
            Password = password;

            Text = string.Empty;
        }

        public TextBoxField(string label, bool password, EventHandler<TextBoxFieldTextChangedEventArgs> textChanged)
            : this(label, password, textChanged, CredentialFieldStates.SelectedCredential)
        { }

        public TextBoxField(string label, bool password, EventHandler<TextBoxFieldTextChangedEventArgs> textChanged, CredentialFieldStates state)
            : base(CredentialFieldTypes.TextBox, label, state)
        {
            Password = password;
            TextChanged += textChanged;

            Text = string.Empty;
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
            internal set
            {
                _text = value;

                TextChanged?.Invoke(this, new TextBoxFieldTextChangedEventArgs(this));

                Credential?.Events?.SetFieldString(Credential, (uint)FieldId, Text);
            }
        }

        public bool Password { get; private set; }

        #endregion

        #region Methods

        internal override _CREDENTIAL_PROVIDER_FIELD_TYPE GetNativeFieldType()
        {
            if (Password == true)
            {
                return _CREDENTIAL_PROVIDER_FIELD_TYPE.CPFT_PASSWORD_TEXT;
            }
            else
            {
                return _CREDENTIAL_PROVIDER_FIELD_TYPE.CPFT_EDIT_TEXT;
            }
        }

        internal void UpdateText(string text)
        {
            _text = text;

            TextChanged?.Invoke(this, new TextBoxFieldTextChangedEventArgs(this));
        }

        #endregion

        #region Events

        public event EventHandler<TextBoxFieldTextChangedEventArgs> TextChanged;

        #endregion
    }
}