/* COPYRIGHT NOTICE
 * 
 * Copyright © Jamie Highfield 2018. All rights reserved.
 * 
 * This library is protected by UK, EU & international copyright laws and treaties. Unauthorised
 * reproduction of this library outside of the constraints of the accompanied license, or any
 * portion of it, may result in severe criminal penalties that will be prosecuted to the
 * maximum extent possible under the law.
 * 
 */

using JamieHighfield.CredentialProvider.Controls.Events;
using JamieHighfield.CredentialProvider.Credentials;
using JamieHighfield.CredentialProvider.Interop;
using System;

namespace JamieHighfield.CredentialProvider.Controls
{
    /// <summary>
    /// An interactable text control.
    /// </summary>
    public sealed class TextBoxControl : LabelledCredentialControlBase
    {
        internal TextBoxControl()
            : base(CredentialControlTypes.TextBox)
        { }

        internal TextBoxControl(Func<CredentialBase, CredentialFieldVisibilities> visibility, Func<CredentialBase, string> label, Func<CredentialBase, string> text, bool password)
            : base(CredentialControlTypes.TextBox, visibility, label)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            _text = new DynamicPropertyStore<string>(this, text);
            Password = password;
        }

        #region Variables

        private DynamicPropertyStore<string> _text;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the text for this <see cref="TextBoxControl"/>.
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

        /// <summary>
        /// Gets whether this <see cref="TextBoxControl"/> should use a password character mark.
        /// </summary>
        public bool Password { get; internal set; }

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
            _text.Value = text ?? throw new ArgumentNullException(nameof(text));

            TextChanged?.Invoke(this, new TextBoxControlTextChangedEventArgs(Credential, this));
        }

        internal override CredentialControlBase Clone()
        {
            return new TextBoxControl()
            {
                Visibility = Visibility,
                Label = Label,
                Password = Password,
                Text = Text
            };
        }

        #endregion

        #region Events

        public event EventHandler<TextBoxControlTextChangedEventArgs> TextChanged;

        #endregion
    }
}