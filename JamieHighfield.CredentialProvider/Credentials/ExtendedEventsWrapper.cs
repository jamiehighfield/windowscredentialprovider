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

using JamieHighfield.CredentialProvider.Interop;

namespace JamieHighfield.CredentialProvider.Credentials
{
    internal class ExtendedEventsWrapper : EventsWrapper, ICredentialProviderCredentialEvents2
    {
        internal ExtendedEventsWrapper(CredentialBase bridgedCredential, ICredentialProviderCredentialEvents2 bridgedEvents)
            : base(bridgedCredential, bridgedEvents)
        { }

        #region Variables



        #endregion

        #region Properties



        #endregion

        #region Methods

        #region Credential Provider Interface Methods

        public int BeginFieldUpdates()
        {
            return ((ICredentialProviderCredentialEvents2)BridgedEvents).BeginFieldUpdates();
        }

        public int EndFieldUpdates()
        {
            return ((ICredentialProviderCredentialEvents2)BridgedEvents).EndFieldUpdates();
        }

        public int SetFieldOptions(ICredentialProviderCredential credential, uint fieldID, CREDENTIAL_PROVIDER_CREDENTIAL_FIELD_OPTIONS options)
        {
            return ((ICredentialProviderCredentialEvents2)BridgedEvents).SetFieldOptions(credential, fieldID, options);
        }

        #endregion

        #endregion
    }
}