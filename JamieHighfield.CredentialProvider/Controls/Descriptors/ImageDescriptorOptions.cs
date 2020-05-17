using JamieHighfield.CredentialProvider.Controls.New;
using JamieHighfield.CredentialProvider.Credentials;
using System;
using System.Drawing;

namespace JamieHighfield.CredentialProvider.Controls.Descriptors
{
    public sealed class ImageDescriptorOptions<TCredentialType> : DescriptorOptionsBase<TCredentialType>
        where TCredentialType : CredentialBase
    {
        /// <summary>
        /// Gets or sets the image for the control. This property is not modifiable at runtime.
        /// </summary>
        public Func<TCredentialType, ImageControl, Bitmap> Image { get; set; }
    }
}