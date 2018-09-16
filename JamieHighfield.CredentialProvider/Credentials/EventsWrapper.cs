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
    internal sealed class EventsWrapper : ICredentialProviderCredentialEvents
    {
        internal EventsWrapper(CredentialBase bridgedCredential, ICredentialProviderCredentialEvents bridgedEvents)
        {
            BridgedCredential = bridgedCredential;
            BridgedEvents = bridgedEvents;
        }

        #region Variables



        #endregion

        #region Properties

        private CredentialBase BridgedCredential { get; set; }

        internal ICredentialProviderCredentialEvents BridgedEvents { get; set; }

        #endregion

        #region Methods

        #region Credential Provider Interface Methods
        
        public int SetFieldState(ICredentialProviderCredential pcpc, uint dwFieldID, _CREDENTIAL_PROVIDER_FIELD_STATE cpfs)
        {
            return BridgedEvents.SetFieldState(BridgedCredential, dwFieldID, cpfs);
        }

        public int SetFieldInteractiveState(ICredentialProviderCredential pcpc, uint dwFieldID, _CREDENTIAL_PROVIDER_FIELD_INTERACTIVE_STATE cpfis)
        {
            return BridgedEvents.SetFieldInteractiveState(BridgedCredential, dwFieldID, cpfis);
        }

        public int SetFieldString(ICredentialProviderCredential pcpc, uint dwFieldID, string psz)
        {
            return BridgedEvents.SetFieldString(BridgedCredential, dwFieldID, psz);
        }

        public int SetFieldCheckbox(ICredentialProviderCredential pcpc, uint dwFieldID, int bChecked, string pszLabel)
        {
            return BridgedEvents.SetFieldCheckbox(BridgedCredential, dwFieldID, bChecked, pszLabel);
        }

        public int SetFieldBitmap(ICredentialProviderCredential pcpc, uint dwFieldID, ref _userHBITMAP hbmp)
        {
            return BridgedEvents.SetFieldBitmap(BridgedCredential, dwFieldID, ref hbmp);
        }

        public int SetFieldComboBoxSelectedItem(ICredentialProviderCredential pcpc, uint dwFieldID, uint dwSelectedItem)
        {
            return BridgedEvents.SetFieldComboBoxSelectedItem(BridgedCredential, dwFieldID, dwSelectedItem);
        }

        public int DeleteFieldComboBoxItem(ICredentialProviderCredential pcpc, uint dwFieldID, uint dwItem)
        {
            return BridgedEvents.DeleteFieldComboBoxItem(BridgedCredential, dwFieldID, dwItem);
        }

        public int AppendFieldComboBoxItem(ICredentialProviderCredential pcpc, uint dwFieldID, string pszItem)
        {
            return BridgedEvents.AppendFieldComboBoxItem(BridgedCredential, dwFieldID, pszItem);
        }

        public int SetFieldSubmitButton(ICredentialProviderCredential pcpc, uint dwFieldID, uint dwAdjacentTo)
        {
            return BridgedEvents.SetFieldSubmitButton(BridgedCredential, dwFieldID, dwAdjacentTo);
        }

        public int OnCreatingWindow(out _RemotableHandle phwndOwner)
        {
            return BridgedEvents.OnCreatingWindow(out phwndOwner);
        }

        #endregion

        #endregion
    }
}