using System;

namespace JamieHighfield.CredentialProvider.Controls.Descriptors
{
    public abstract class DescriptorBase
    {
        protected DescriptorBase(DescriptorOptionsBase options)
        {
            Options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public DescriptorOptionsBase Options { get; }
    }
}