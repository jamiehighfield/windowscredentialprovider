using JamieHighfield.CredentialProvider.Credentials;
using JamieHighfield.CredentialProvider.Interop;
using JamieHighfield.CredentialProvider.Logging;
using System;
using System.Runtime.InteropServices;
using static JamieHighfield.CredentialProvider.Constants;

namespace JamieHighfield.CredentialProvider.Providers
{
    /// <summary>
    /// Extend this class to create a new credential provider to be used in Windows. You must also add the attributes <see cref="ComVisibleAttribute"/>, <see cref="GuidAttribute"/> (for the COM GUID), <see cref="ProgIdAttribute"/> (for the COM identifier) and <see cref="ClassInterfaceAttribute"/> in order for this class to be correctly registered.
    /// 
    /// This wraps the functionality of the 'ICredentialProvider' and 'ICredentialProviderSetUserArray' interfaces.
    /// </summary>
    public abstract class CredentialProviderSetUserArrayBase<TCredentialType> : CredentialProviderBase<TCredentialType>, ICredentialProviderSetUserArray
        where TCredentialType : CredentialBase
    {
        /// <summary>
        /// Instantiate a new <see cref="CredentialProviderSetUserArrayBase"/> object.
        /// </summary>
        /// <param name="usageScenarios">The <see cref="CredentialProviderUsageScenarios"/> that this credential provider will support when requested by Windows.</param>
        protected CredentialProviderSetUserArrayBase(CredentialProviderUsageScenarios usageScenarios)
            : base(usageScenarios)
        { }

        /// <summary>
        /// Instantiate a new <see cref="CredentialProviderSetUserArrayBase"/> object that will wrap an existing credential provider.
        /// </summary>
        /// <param name="underlyingCredentialProviderGuid">The COM <see cref="Guid"/> of the credential provider that this credential provider should wrap.</param>
        /// <param name="usageScenarios">The <see cref="CredentialProviderUsageScenarios"/> that this credential provider will support when requested by Windows.</param>
        protected CredentialProviderSetUserArrayBase(Guid underlyingCredentialProviderGuid, CredentialProviderUsageScenarios usageScenarios)
            : base(underlyingCredentialProviderGuid, usageScenarios)
        { }

        #region Variables



        #endregion

        #region Properties



        #endregion

        #region Methods

        #region Credential Provider Interface Methods

        #region ICredentialProviderSetUserArray

        /// <summary>
        /// Statutory method from <see cref="ICredentialProviderSetUserArray"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-icredentialprovidersetuserarray for more information.
        /// 
        /// Currently, no managed API has been created for the <see cref="ICredentialProviderSetUserArray"/> interface other than wrapping an existing credential provider.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        int ICredentialProviderSetUserArray.SetUserArray(ICredentialProviderUserArray users)
        {
            GlobalLogger.LogMethodCall();

            if (UnderlyingCredentialProvider != null && UnderlyingCredentialProvider is ICredentialProviderSetUserArray)
            {
                return ((ICredentialProviderSetUserArray)UnderlyingCredentialProvider).SetUserArray(users);
            }

            return HRESULT.E_NOTIMPL;
        }

        #endregion

        #endregion

        #endregion
    }
}