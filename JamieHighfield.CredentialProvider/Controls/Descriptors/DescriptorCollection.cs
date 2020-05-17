using JamieHighfield.CredentialProvider.Credentials;
using System.Collections.Generic;

namespace JamieHighfield.CredentialProvider.Controls.Descriptors
{
    public sealed class DescriptorCollection<TCredentialType> : List<DescriptorBase<TCredentialType>>
        where TCredentialType : CredentialBase
    { }
}