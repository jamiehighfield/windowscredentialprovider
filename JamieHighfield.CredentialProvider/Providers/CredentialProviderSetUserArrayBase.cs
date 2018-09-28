using JamieHighfield.CredentialProvider.Credentials;
using JamieHighfield.CredentialProvider.Interfaces;
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
    public abstract class CredentialProviderSetUserArrayBase : CredentialProviderBase, ICredentialProviderSetUserArray
    {
        /// <summary>
        /// Instantiate a new <see cref="CredentialProviderSetUserArrayBase"/> object.
        /// </summary>
        protected CredentialProviderSetUserArrayBase()
            : base()
        { }

        /// <summary>
        /// Instantiate a new <see cref="CredentialProviderSetUserArrayBase"/> object.
        /// </summary>
        /// <param name="usageScenarios">The <see cref="CredentialProviderUsageScenarios"/> that this credential provider will support when requested by Windows.</param>
        protected CredentialProviderSetUserArrayBase(CredentialProviderUsageScenarios usageScenarios)
            : base(usageScenarios)
        { }

        /// <summary>
        /// Instantiate a new <see cref="CredentialProviderSetUserArrayBase"/> object.
        /// </summary>
        /// <param name="credentialsDelegate">The <see cref="Action{CredentialCollection}"/> that will be invoked upon construction.</param>
        /// <param name="controlsDelegate">The <see cref="Action{CredentialControlCollection}"/> that will be invoked upon construction.</param>
        protected CredentialProviderSetUserArrayBase(Action<CredentialCollection> credentialsDelegate, Action<ICurrentEnvironment, CredentialControlCollection> controlsDelegate)
            : base(credentialsDelegate, controlsDelegate)
        { }

        /// <summary>
        /// Instantiate a new <see cref="CredentialProviderSetUserArrayBase"/> object.
        /// </summary>
        /// <param name="usageScenarios">The <see cref="CredentialProviderUsageScenarios"/> that this credential provider will support when requested by Windows.</param>
        /// <param name="credentialsDelegate">The <see cref="Action{CredentialCollection}"/> that will be invoked upon construction.</param>
        /// <param name="controlsDelegate">The <see cref="Action{CredentialControlCollection}"/> that will be invoked upon construction.</param>
        protected CredentialProviderSetUserArrayBase(CredentialProviderUsageScenarios usageScenarios, Action<CredentialCollection> credentialsDelegate, Action<ICurrentEnvironment, CredentialControlCollection> controlsDelegate)
            : base(usageScenarios, credentialsDelegate, controlsDelegate)
        { }

        /// <summary>
        /// Instantiate a new <see cref="CredentialProviderSetUserArrayBase"/> object that will wrap an existing credential provider.
        /// </summary>
        /// <param name="underlyingCredentialProviderGuid">The COM <see cref="Guid"/> of the credential provider that this credential provider should wrap.</param>
        /// <param name="incomingCredentialManipulator">Manipulate an incoming credential provider from the wrapped credential provider.</param>
        protected CredentialProviderSetUserArrayBase(Guid underlyingCredentialProviderGuid, Func<ICurrentEnvironment, CredentialBase> incomingCredentialManipulator)
            : base(underlyingCredentialProviderGuid, incomingCredentialManipulator)
        { }

        /// <summary>
        /// Instantiate a new <see cref="CredentialProviderSetUserArrayBase"/> object that will wrap an existing credential provider.
        /// </summary>
        /// <param name="underlyingCredentialProviderGuid">The COM <see cref="Guid"/> of the credential provider that this credential provider should wrap.</param>
        /// <param name="controlsDelegate">The <see cref="Action{CredentialControlCollection}"/> that will be invoked when fields are requested by Windows.</param>
        /// <param name="incomingCredentialManipulator">Manipulate an incoming credential provider from the wrapped credential provider.</param>
        protected CredentialProviderSetUserArrayBase(Guid underlyingCredentialProviderGuid, Action<ICurrentEnvironment, CredentialControlCollection> controlsDelegate, Func<ICurrentEnvironment, CredentialBase> incomingCredentialManipulator)
            : base(underlyingCredentialProviderGuid, controlsDelegate, incomingCredentialManipulator)
        { }

        /// <summary>
        /// Instantiate a new <see cref="CredentialProviderSetUserArrayBase"/> object that will wrap an existing credential provider.
        /// </summary>
        /// <param name="underlyingCredentialProviderGuid">The COM <see cref="Guid"/> of the credential provider that this credential provider should wrap.</param>
        /// <param name="usageScenarios">The <see cref="CredentialProviderUsageScenarios"/> that this credential provider will support when requested by Windows.</param>
        /// <param name="controlsDelegate">The <see cref="Action{CredentialControlCollection}"/> that will be invoked upon construction.</param>
        /// <param name="incomingCredentialManipulator">Manipulate an incoming credential provider from the wrapped credential provider.</param>
        protected CredentialProviderSetUserArrayBase(Guid underlyingCredentialProviderGuid, CredentialProviderUsageScenarios usageScenarios, Action<ICurrentEnvironment, CredentialControlCollection> controlsDelegate, Func<ICurrentEnvironment, CredentialBase> incomingCredentialManipulator)
            : base(underlyingCredentialProviderGuid, usageScenarios, controlsDelegate, incomingCredentialManipulator)
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