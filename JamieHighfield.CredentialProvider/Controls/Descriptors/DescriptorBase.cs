using JamieHighfield.CredentialProvider.Credentials;
using System;

namespace JamieHighfield.CredentialProvider.Controls.Descriptors
{
    public abstract class DescriptorBase<TCredentialType>
        where TCredentialType : CredentialBase
    {
        protected DescriptorBase(DescriptorOptionsBase<TCredentialType> options)
        {
            Options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public DescriptorOptionsBase<TCredentialType> Options { get; }
    }
}