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
    public sealed class LinkControl : CredentialControlBase
    {
        internal LinkControl()
            : base(CredentialControlTypes.Link)
        { }

        internal LinkControl(Func<CredentialBase, CredentialFieldVisibilities> visibility, Func<CredentialBase, string> text)
            : base(CredentialControlTypes.Link, visibility)
        {
            _text = new DynamicPropertyStore<string>(this, text);
        }

        #region Variables

        private DynamicPropertyStore<string> _text;

        #endregion

        #region Properties
        
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

                //TextChanged?.Invoke(this, new TextBoxControlTextChangedEventArgs(Credential, this));

                Credential?.Events?.SetFieldString(Credential, (uint)Field.FieldId, Text);
            }
        }

        #endregion

        #region Methods

        internal override _CREDENTIAL_PROVIDER_FIELD_TYPE GetNativeFieldType()
        {
            return _CREDENTIAL_PROVIDER_FIELD_TYPE.CPFT_COMMAND_LINK;
        }

        internal void InvokeClicked(object sender, LinkControlClickedEventArgs eventArgs)
        {
            Clicked?.Invoke(sender, eventArgs);
        }

        internal override CredentialControlBase Clone()
        {
            LinkControl linkControl = new LinkControl()
            {
                Text = Text,
                Visibility = Visibility
            };

            if (Clicked != null)
            {
                linkControl.Clicked += Clicked;
            }

            return linkControl;
        }

        #endregion

        #region Events

        public event EventHandler<LinkControlClickedEventArgs> Clicked;

        #endregion
    }
}