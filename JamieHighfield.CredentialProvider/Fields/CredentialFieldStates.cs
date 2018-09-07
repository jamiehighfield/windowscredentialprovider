namespace JamieHighfield.CredentialProvider.Fields
{
    public enum CredentialFieldStates
    {
        /// <summary>
        /// Not displayed.
        /// </summary>
        Hidden = 0,
        /// <summary>
        /// Only displayed when this credential is active.
        /// </summary>
        SelectedCredential = 2,
        /// <summary>
        /// Only displayed when this credential isn't active.
        /// </summary>
        DeselectedCredential = 4,
        /// <summary>
        /// Displayed both when this credential is and isn't active.
        /// </summary>
        Both = SelectedCredential | DeselectedCredential
    }
}