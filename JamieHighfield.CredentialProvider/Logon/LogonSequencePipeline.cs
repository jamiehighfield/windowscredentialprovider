/* COPYRIGHT NOTICE
 * 
 * Copyright © Jamie Highfield 2018. All rights reserved.
 * 
 * This library is protected by UK, EU & international copyright laws and treaties. Unauthorised
 * reproduction of this library outside of the constraints of the accompanied license, or any
 * portion of it, may result in severe criminal penalties that will be prosecuted to the
 * maximum extent possible under the law.
 * 
 */

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