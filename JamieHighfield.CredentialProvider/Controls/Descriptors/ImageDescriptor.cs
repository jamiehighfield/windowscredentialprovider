using JamieHighfield.CredentialProvider.Credentials;

namespace JamieHighfield.CredentialProvider.Controls.Descriptors
{
    public sealed class ImageDescriptor<TCredentialType> : DescriptorBase<TCredentialType>
        where TCredentialType : CredentialBase
    {
        internal ImageDescriptor(ImageDescriptorOptions<TCredentialType> options)
            : base(options)
        { }
    }
}