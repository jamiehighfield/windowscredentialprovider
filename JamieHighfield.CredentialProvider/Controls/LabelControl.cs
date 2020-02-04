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
using JamieHighfield.CredentialProvider.Providers;
using System;

namespace JamieHighfield.CredentialProvider.Controls
{
    /// <summary>
    /// A non-interactable text control.
    /// </summary>
    public sealed class LabelControl : CredentialControlBase
    {
        internal LabelControl()
            : base(CredentialControlTypes.Label)
        {

        }

        internal LabelControl(Func<CredentialBase, CredentialFieldVisibilities> visibility, Func<CredentialBase, string> text)
            : base(CredentialControlTypes.Label, visibility)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            _text = new DynamicPropertyStore<string>(this, text);
        }

        internal LabelControl(Func<CredentialBase, CredentialFieldVisibilities> visibility, Func<CredentialBase, string> text, LabelControlSizes size)
            :base(CredentialControlTypes.Label, visibility)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            _text = new DynamicPropertyStore<string>(this, text);
            Size = size;
        }
        
        #region Variables
        
        private DynamicPropertyStore<string> _text;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the size of either <see cref="LabelControlSizes.Small"/> or <see cref="LabelControlSizes.Large"/> for this <see cref="LabelControl"/>.
        /// </summary>
        public LabelControlSizes Size { get; private set; }

        /// <summary>
        /// Gets or sets the text for this <see cref="LabelControl"/>.
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

        #endregion

        #region Methods

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

        internal void UpdateText(string text)
        {
            _text.Value = text;

            TextChanged?.Invoke(this, new LabelControlTextChangedEventArgs(Credential, this));
        }

        internal override CredentialControlBase Clone()
        {
            return new LabelControl()
            {
                Visibility = Visibility,
                Size = Size,
                Text = Text
            };
        }

        #endregion

        #region Events

        /// <summary>
        /// This event is fired when the text for this label is changed.
        /// </summary>
        public event EventHandler<LabelControlTextChangedEventArgs> TextChanged;

        #endregion
    }
}