using JamieHighfield.CredentialProvider.Credentials;

namespace JamieHighfield.CredentialProvider.Controls.Descriptors
{
    public sealed class TextBoxDescriptor<TCredentialType> : DescriptorBase<TCredentialType>
        where TCredentialType : CredentialBase
    {
        internal TextBoxDescriptor(TextBoxDescriptorOptions<TCredentialType> options)
            : base(options)
        { }
    }
}