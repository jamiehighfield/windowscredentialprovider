using JamieHighfield.CredentialProvider.Logon.Authentication;

namespace JamieHighfield.CredentialProvider.Logon
{
    public sealed class LogonSequencePipeline : LogonSequencePipelineBase
    {
        private LogonSequencePipeline()
            : base(new LogonSequenceCollection())
        { }

        #region Variables



        #endregion

        #region Properties



        #endregion

        #region Methods

        public static LogonSequencePipeline CreatePipeline()
        {
            return new LogonSequencePipeline();
        }

        public AuthenticatedLogonSequencePipeline AddAuthentication(CredentialProviderAuthenticationBase authentication)
        {
            AddSequence(authentication);

            return new AuthenticatedLogonSequencePipeline(Sequences);
        }

        #endregion
    }
}