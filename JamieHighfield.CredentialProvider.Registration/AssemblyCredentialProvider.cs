using System;

namespace JamieHighfield.CredentialProvider.Registration
{
    public sealed class AssemblyCredentialProvider
    {
        public AssemblyCredentialProvider(Guid comGuid, string comName, bool comRegistered, bool credentialProviderRegistered)
        {
            ComGuid = comGuid;
            ComName = comName;
            ComRegistered = comRegistered;
            CredentialProviderRegistered = credentialProviderRegistered;
        }

        #region Variables



        #endregion

        #region Properties

        public Guid ComGuid { get; private set; }

        public string ComName { get; private set; }

        public bool ComRegistered { get; private set; }
        
        public bool CredentialProviderRegistered { get; private set; }

        #endregion

        #region Methods



        #endregion
    }
}