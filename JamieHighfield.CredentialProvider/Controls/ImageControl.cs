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
using System.Drawing;

namespace JamieHighfield.CredentialProvider.Controls
{
    public sealed class ImageControl : CredentialControlBase
    {
        internal ImageControl()
            : base(CredentialControlTypes.Image)
        { }

        #region Variables



        #endregion

        #region Properties

        public Bitmap Image { get; internal set; }

        #endregion

        #region Methods

        internal override _CREDENTIAL_PROVIDER_FIELD_TYPE GetNativeFieldType()
        {
            return _CREDENTIAL_PROVIDER_FIELD_TYPE.CPFT_TILE_IMAGE;
        }

        internal override CredentialControlBase Clone()
        {
            return new ImageControl()
            {
                Image = Image,
                Visibility = Visibility
            };
        }

        #endregion
    }
}