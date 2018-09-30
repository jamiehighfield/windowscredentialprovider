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

using System;
using JamieHighfield.CredentialProvider.Controls.Events;
using JamieHighfield.CredentialProvider.Interop;
using JamieHighfield.CredentialProvider.Providers;

namespace JamieHighfield.CredentialProvider.Controls
{
    public sealed class LinkControl : CredentialControlBase
    {
        internal LinkControl()
            : base(CredentialControlTypes.Link)
        { }
        
        #region Variables



        #endregion

        #region Properties

        public string Text { get; set; }

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