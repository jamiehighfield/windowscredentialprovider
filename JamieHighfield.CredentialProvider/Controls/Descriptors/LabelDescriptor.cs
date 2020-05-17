using JamieHighfield.CredentialProvider.Credentials;

namespace JamieHighfield.CredentialProvider.Controls.Descriptors
{
    public sealed class LabelDescriptor<TCredentialType> : DescriptorBase<TCredentialType>
        where TCredentialType : CredentialBase
    {
        internal LabelDescriptor(LabelDescriptorOptions<TCredentialType> options)
            : base(options)
        { }
    }
}