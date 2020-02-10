using JamieHighfield.CredentialProvider.Credentials;
using System;
using System.Drawing;

namespace JamieHighfield.CredentialProvider.Controls.Descriptors
{
    public sealed class ImageDescriptorOptions : DescriptorOptionsBase
    {
        /// <summary>
        /// Gets or sets the image for the control. This property is not modifiable at runtime.
        /// </summary>
        public Func<CredentialBase, Bitmap> Image { get; set; }
    }
}