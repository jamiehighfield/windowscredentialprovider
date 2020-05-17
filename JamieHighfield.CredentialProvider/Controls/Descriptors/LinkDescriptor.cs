using JamieHighfield.CredentialProvider.Credentials;

namespace JamieHighfield.CredentialProvider.Controls.Descriptors
{
    public sealed class LinkDescriptor<TCredentialType> : DescriptorBase<TCredentialType>
        where TCredentialType : CredentialBase
    {
        internal LinkDescriptor(LinkDescriptorOptions<TCredentialType> options)
            : base(options)
        { }
    }
}