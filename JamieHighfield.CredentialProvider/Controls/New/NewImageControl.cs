using JamieHighfield.CredentialProvider.Credentials;
using JamieHighfield.CredentialProvider.Interop;
using System;
using System.Drawing;

namespace JamieHighfield.CredentialProvider.Controls.New
{
    public sealed class NewImageControl : NewCredentialControlBase
    {
        internal NewImageControl(CredentialBase credential, CredentialFieldVisibilities visibility, Func<CredentialBase, Bitmap> image)
            : base(credential, visibility)
        {
            _image = new NewDynamicPropertyStore<Bitmap>(this, image);
        }

        private NewDynamicPropertyStore<Bitmap> _image;

        public Bitmap Image
        {
            get
            {
                return _image.Value;
            }
        }

        internal override _CREDENTIAL_PROVIDER_FIELD_TYPE GetNativeFieldType()
        {
            return _CREDENTIAL_PROVIDER_FIELD_TYPE.CPFT_TILE_IMAGE;
        }
    }
}