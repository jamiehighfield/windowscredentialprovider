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
using JamieHighfield.CredentialProvider.Logging;
using JamieHighfield.CredentialProvider.Providers;
using System;

namespace JamieHighfield.CredentialProvider.Controls
{
    public sealed class TextBoxControl : LabelledCredentialControlBase
    {
        internal TextBoxControl()
            : base(CredentialControlTypes.TextBox)
        { }
        
        #region Variables

        private string _text;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the text for this <see cref="TextBoxControl"/>.
        /// </summary>
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;

                TextChanged?.Invoke(this, new TextBoxControlTextChangedEventArgs(Credential, this));

                if (Credential != null)
                {
                    Credential.Events.SetFieldString(Credential, (uint)Field.FieldId, Text);
                }
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
            _text = text;

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