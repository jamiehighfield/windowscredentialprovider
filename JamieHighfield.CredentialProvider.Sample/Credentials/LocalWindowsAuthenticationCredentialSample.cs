using JamieHighfield.CredentialProvider.Credentials;

namespace JamieHighfield.CredentialProvider.Sample.Credentials
{
    public sealed class LocalWindowsAuthenticationCredentialSample : ExtendedCredentialBase
    {
        public LocalWindowsAuthenticationCredentialSample()
            : base(Logon.LogonSequencePipeline.CreatePipeline()
                  .AddAuthentication(new CredentialProviderAuthenticationSample()))
        {
            WindowsLogon = true;
        }

        #region Variables



        #endregion

        #region Properties



        #endregion

        #region Methods



        #endregion
    }
}