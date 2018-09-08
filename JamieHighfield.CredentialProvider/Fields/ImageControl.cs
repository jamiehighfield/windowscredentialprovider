using System;
using System.Drawing;
using CredProvider.NET.Interop2;

namespace JamieHighfield.CredentialProvider.Controls
{
    public sealed class ImageControl : CredentialControlBase
    {
        public ImageControl(Bitmap image)
            : this(image, CredentialFieldVisibilities.Both)
        { }

        public ImageControl(Bitmap image, CredentialFieldVisibilities visibility)
            : base(CredentialFieldTypes.Image, visibility)
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