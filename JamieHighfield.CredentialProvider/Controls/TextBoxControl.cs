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

using CredProvider.NET.Interop2;
using JamieHighfield.CredentialProvider.Controls.Events;
using System;

namespace JamieHighfield.CredentialProvider.Controls
{
    public sealed class TextBoxControl : LabelledCredentialControlBase
    {
        public TextBoxControl(string label, bool password)
            : this(label, password, CredentialFieldVisibilities.SelectedCredential)
        { }

        public TextBoxControl(string label, bool password, CredentialFieldVisibilities visibility)
            : base(CredentialFieldTypes.TextBox, label, visibility)
        {
            Password = password;

            Text = string.Empty;
        }

        public TextBoxControl(string label, bool password, EventHandler<CredentialControlChangedEventArgs<TextBoxControl>> textChanged)
            : this(label, password, textChanged, CredentialFieldVisibilities.SelectedCredential)
        { }

        public TextBoxControl(string label, bool password, EventHandler<CredentialControlChangedEventArgs<TextBoxControl>> textChanged, CredentialFieldVisibilities state)
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
            set
            {
                _text = value;

                TextChanged?.Invoke(this, new CredentialControlChangedEventArgs<TextBoxControl>(this));

                EventCallback?.Invoke((credential, fieldId) =>
                {
                    credential.Events.SetFieldString(credential, (uint)fieldId, Text);
                });
            }
        }

        public bool Password { get; private set; }

        public bool Focussed { get; set; }

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

            TextChanged?.Invoke(this, new CredentialControlChangedEventArgs<TextBoxControl>(this));
        }

        #endregion

        #region Events

        public event EventHandler<CredentialControlChangedEventArgs<TextBoxControl>> TextChanged;

        #endregion
    }
}