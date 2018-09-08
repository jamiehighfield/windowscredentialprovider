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
    public sealed class LabelControl : CredentialControlBase
    {
        public LabelControl(string text, LabelControlSizes size)
            : this(text, size, CredentialFieldVisibilities.SelectedCredential)
        { }

        public LabelControl(string text, LabelControlSizes size, CredentialFieldVisibilities visibility)
            : base(CredentialFieldTypes.Text, visibility)
        {
            Text = text;
            Size = size;
        }

        public LabelControl(string text, LabelControlSizes size, EventHandler<CredentialControlChangedEventArgs<LabelControl>> textChanged)
            : this(text, size, textChanged, CredentialFieldVisibilities.SelectedCredential)
        { }

        public LabelControl(string text, LabelControlSizes size, EventHandler<CredentialControlChangedEventArgs<LabelControl>> textChanged, CredentialFieldVisibilities visibility)
            : base(CredentialFieldTypes.TextBox, visibility)
        {
            Text = text;
            Size = size;
            Visibility = visibility;
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

                TextChanged?.Invoke(this, new CredentialControlChangedEventArgs<LabelControl>(this));

                EventCallback?.Invoke((credential, fieldId) =>
                {
                    credential.Events.SetFieldString(credential, (uint)fieldId, Text);
                });
            }
        }

        public LabelControlSizes Size { get; private set; }

        public CredentialFieldVisibilities Visibility { get; }

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
            _text = text;

            TextChanged?.Invoke(this, new CredentialControlChangedEventArgs<LabelControl>(this));
        }

        #endregion

        #region Events

        public event EventHandler<CredentialControlChangedEventArgs<LabelControl>> TextChanged;

        #endregion
    }
}