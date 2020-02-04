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

using JamieHighfield.CredentialProvider.Credentials;
using JamieHighfield.CredentialProvider.Interop;
using System;
using System.Drawing;

namespace JamieHighfield.CredentialProvider.Controls
{
    public sealed class ImageControl : CredentialControlBase
    {
        internal ImageControl()
            : base(CredentialControlTypes.Image)
        { }

        internal ImageControl(Func<CredentialBase, CredentialFieldVisibilities> visibility, Func<CredentialBase, Bitmap> image)
            : base(CredentialControlTypes.Image, visibility)
        {
            Visibility = visibility ?? throw new ArgumentNullException(nameof(visibility));

            _image = new DynamicPropertyStore<Bitmap>(this, image);
        }


        #region Variables

        private DynamicPropertyStore<Bitmap> _image;

        #endregion

        #region Properties

        public Func<CredentialBase, CredentialFieldVisibilities> Visibility { get; }

        public Bitmap Image
        {
            get
            {
                return _image.Value;
            }
            internal set
            {
                _image.Value = value;
            }
        }

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
                Image = Image
            };
        }

        #endregion
    }
}