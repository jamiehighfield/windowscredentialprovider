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

using JamieHighfield.CredentialProvider.Interop;
using JamieHighfield.CredentialProvider.Providers;
using System;

namespace JamieHighfield.CredentialProvider.Controls
{
    public sealed class ButtonControl : LabelledCredentialControlBase
    {
        internal ButtonControl()
            : base(CredentialControlTypes.Button)
        { }

        #region Variables



        #endregion

        #region Properties

        public CredentialControlBase AdjacentControl { get; internal set; }

        #endregion

        #region Methods

        internal override _CREDENTIAL_PROVIDER_FIELD_TYPE GetNativeFieldType()
        {
            return _CREDENTIAL_PROVIDER_FIELD_TYPE.CPFT_SUBMIT_BUTTON;
        }

        internal override CredentialControlBase Clone()
        {
            return new ButtonControl()
            {
                Visibility = Visibility,
                Label = Label
            };
        }

        #endregion
    }
}