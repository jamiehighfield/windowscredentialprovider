using JamieHighfield.CredentialProvider.Credentials;

namespace JamieHighfield.CredentialProvider.Controls.Descriptors
{
    public sealed class ButtonDescriptor<TCredentialType> : DescriptorBase<TCredentialType>
        where TCredentialType : CredentialBase
    {
        internal ButtonDescriptor(ButtonDescriptorOptions<TCredentialType> options)
            : base(options)
        { }
    }
}