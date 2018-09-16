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
using JamieHighfield.CredentialProvider.Interop;
using JamieHighfield.CredentialProvider.Providers;
using System;

namespace JamieHighfield.CredentialProvider.Controls
{
    public sealed class LabelControl : CredentialControlBase
    {
        public LabelControl(LabelControlSizes size, string text)
            : this(CredentialFieldVisibilities.SelectedCredential, size, text)
        { }

        public LabelControl(CredentialFieldVisibilities visibility, LabelControlSizes size, string text)
            : base(CredentialControlTypes.Label, visibility)
        {
            Size = size;
            Text = text;
        }

        public LabelControl(Func<CredentialProviderUsageScenarios, CredentialFieldVisibilities> visibilityDelegate, LabelControlSizes size, string text)
            : base(CredentialControlTypes.Label, visibilityDelegate)
        {
            Size = size;
            Text = text;
        }

        #region Variables

        private string _text;

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
                return _text;
            }
            set
            {
                _text = value;

                TextChanged?.Invoke(this, new LabelControlTextChangedEventArgs(this));

                EventCallback?.Invoke((credential, fieldId) =>
                {
                    //credential.Events.SetFieldString(credential, (uint)fieldId, Text);
                });
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
            _text = text;

            TextChanged?.Invoke(this, new LabelControlTextChangedEventArgs(this));
        }

        #endregion

        #region Events

        public event EventHandler<LabelControlTextChangedEventArgs> TextChanged;

        #endregion
    }
}