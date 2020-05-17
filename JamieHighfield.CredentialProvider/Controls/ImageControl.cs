using JamieHighfield.CredentialProvider.Credentials;
using JamieHighfield.CredentialProvider.Interop;
using System;
using System.Drawing;

namespace JamieHighfield.CredentialProvider.Controls.New
{
    public sealed class ImageControl : CredentialControlBase
    {
        internal ImageControl(CredentialBase credential, CredentialFieldVisibilities visibility, bool forwardToField, Func<ImageControl, Func<CredentialBase, ImageControl, Bitmap>> image)
            : base(credential, visibility, forwardToField)
        {
            if (image is null)
            {
                _image = new DynamicPropertyStore<Bitmap>(this, (innerCredential) => null);
            }
            else
            {
                _image = new DynamicPropertyStore<Bitmap>(this, (innerCredential) => (image.Invoke(this)?.Invoke(innerCredential, this) ?? null));
            }
        }

        private DynamicPropertyStore<Bitmap> _image;

        public Bitmap Image
        {
            get
            {
                return _image.Value;
            }
        }
    }
}