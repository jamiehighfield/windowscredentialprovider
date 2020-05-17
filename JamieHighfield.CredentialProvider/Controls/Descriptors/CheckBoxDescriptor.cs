using JamieHighfield.CredentialProvider.Credentials;

namespace JamieHighfield.CredentialProvider.Controls.Descriptors
{
    public sealed class CheckBoxDescriptor<TCredentialType> : DescriptorBase<TCredentialType>
        where TCredentialType : CredentialBase
    {
        internal CheckBoxDescriptor(CheckBoxDescriptorOptions<TCredentialType> options)
            : base(options)
        { }
    }
}