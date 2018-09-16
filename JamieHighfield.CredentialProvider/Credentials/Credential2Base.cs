using JamieHighfield.CredentialProvider.Interop;
using JamieHighfield.CredentialProvider.Logon;
using static JamieHighfield.CredentialProvider.Constants;

namespace JamieHighfield.CredentialProvider.Credentials
{
    public abstract class Credential2Base : CredentialBase, ICredentialProviderCredential2
    {
        public Credential2Base() { }

        public Credential2Base(LogonSequencePipelineBase logonSequencePipeline)
            : base(logonSequencePipeline)
        { }

        #region Variables



        #endregion

        #region Properties



        #endregion

        #region Methods

        #region Credential Provider Interface Methods

        public int GetUserSid(out string sid)
        {
            if (BridgedCredential != null)
            {
                if (BridgedCredential is ICredentialProviderCredential2)
                {
                    int result = HRESULT.E_FAIL;

                    result = ((ICredentialProviderCredential2)BridgedCredential).GetUserSid(out sid);

                    if (result != HRESULT.S_OK)
                    {
                        sid = string.Empty;

                        return result;
                    }
                }
                else
                {
                    sid = string.Empty;
                }
            }
            else
            {
                sid = string.Empty;
            }

            return HRESULT.S_OK;
        }

        #endregion

        #endregion
    }
}