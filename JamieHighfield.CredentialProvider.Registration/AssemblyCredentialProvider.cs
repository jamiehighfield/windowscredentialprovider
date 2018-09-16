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