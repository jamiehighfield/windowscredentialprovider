using System;

namespace JamieHighfield.CredentialProvider
{
    [Flags]
    public enum CredentialProviderUsageScenarios
    {
        Logon = 2,
        UnlockWorkstation = 4,
        ChangePassword = 8,
        CredentialsDialog = 16
    }
}