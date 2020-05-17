using JamieHighfield.CredentialProvider.Credentials.Interfaces;
using JamieHighfield.CredentialProvider.Interop;

namespace JamieHighfield.CredentialProvider.Credentials
{
    public static class CredentialExtensions
    {
        /// <summary>
        /// Start a batch of control updates. Changes to the UI won't be processed until <see cref="EndUpdate{TCredentialType}(TCredentialType)"/> is called.
        /// </summary>
        /// <typeparam name="TCredentialType"></typeparam>
        /// <param name="credential"></param>
        public static void BeginUpdate<TCredentialType>(this TCredentialType credential)
            where TCredentialType : CredentialBase, IExtendedCredential
        {
            if (credential.Events is ICredentialProviderCredentialEvents2 extendedCredentialEvents)
            {
                extendedCredentialEvents.BeginFieldUpdates();
            }
        }

        /// <summary>
        /// End a batch of control updates. This must be preceeded by a call to <see cref="BeginUpdate{TCredentialType}(TCredentialType)"/>.
        /// </summary>
        /// <typeparam name="TCredentialType"></typeparam>
        /// <param name="credential"></param>
        public static void EndUpdate<TCredentialType>(this TCredentialType credential)
            where TCredentialType : CredentialBase, IExtendedCredential
        {
            if (credential.Events is ICredentialProviderCredentialEvents2 extendedCredentialEvents)
            {
                extendedCredentialEvents.EndFieldUpdates();
            }
        }
    }
}