using JamieHighfield.CredentialProvider.Controls;
using System;
using System.Runtime.InteropServices;

namespace JamieHighfield.CredentialProvider.Sample
{
    [ComVisible(true)]
    [Guid("00016d50-0000-0000-b090-00006b0b0000")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("credsample")]
    public sealed class CredentialProviderSample : CredentialProviderBase
    {
        public CredentialProviderSample()
            : base(CredentialProviderUsageScenarios.CredentialsDialog | CredentialProviderUsageScenarios.Logon, new CredentialSample())
        { }

        #region Variables



        #endregion

        #region Properties



        #endregion

        #region Methods



        #endregion
    }
}