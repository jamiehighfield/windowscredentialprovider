using JamieHighfield.CredentialProvider.Credentials;
using JamieHighfield.CredentialProvider.Providers;

namespace JamieHighfield.CredentialProvider.Logon
{
    public sealed class LogonPackage
    {
        public LogonPackage(CredentialProviderUsageScenarios usageScenario, CredentialBase credential)
        {
            UsageScenario = usageScenario;
            Credential = credential ?? throw new System.ArgumentNullException(nameof(credential));
        }

        #region Variables



        #endregion

        #region Properties

        public CredentialProviderUsageScenarios UsageScenario { get; private set; }

        public CredentialBase Credential { get; private set; }

        #endregion

        #region Methods



        #endregion
    }
}