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
        public ImageControl(Bitmap image)
            : base(CredentialControlTypes.Image)
        { }

        public ImageControl(CredentialFieldVisibilities visibility, Bitmap image)
            : base(CredentialControlTypes.Image, visibility)
        {
            Image = image ?? throw new ArgumentNullException(nameof(image));
        }

        public ImageControl(Func<CredentialProviderUsageScenarios, CredentialFieldVisibilities> visibilityDelegate, Bitmap image)
            : base(CredentialControlTypes.Image, visibilityDelegate)
        {
            Image = image ?? throw new ArgumentNullException(nameof(image));
        }

        #region Variables



        #endregion

        #region Properties

        internal Bitmap Image { get; private set; }

        #endregion

        #region Methods

        internal override _CREDENTIAL_PROVIDER_FIELD_TYPE GetNativeFieldType()
        {
            return _CREDENTIAL_PROVIDER_FIELD_TYPE.CPFT_TILE_IMAGE;
        }

        #endregion
    }
}