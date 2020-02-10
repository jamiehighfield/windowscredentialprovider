using JamieHighfield.CredentialProvider.Credentials;
using JamieHighfield.CredentialProvider.Interop;
using System;

namespace JamieHighfield.CredentialProvider.Providers.Interfaces
{
    public abstract class ManagedCredentialProvider
    {
        public CredentialProviderUsageScenarios SupportedUsageScenarios { get; set; }

        public CredentialProviderUsageScenarios CurrentUsageScenario { get; set; }

        internal ICredentialProvider UnderlyingCredentialProvider { get; set; }

        internal CredentialBase CurrentCredential { get; set; }

        public Guid ComGuid { get; }
    }
}