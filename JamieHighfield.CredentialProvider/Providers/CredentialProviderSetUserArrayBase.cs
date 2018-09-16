using JamieHighfield.CredentialProvider.Credentials;
using JamieHighfield.CredentialProvider.Interop;
using System;
using static JamieHighfield.CredentialProvider.Constants;

namespace JamieHighfield.CredentialProvider.Providers
{
    public abstract class CredentialProviderSetUserArrayBase : CredentialProviderBase, ICredentialProviderSetUserArray
    {
        /// <summary>
        /// Instantiate a new <see cref="CredentialProviderBase"/> object.
        /// </summary>
        protected CredentialProviderSetUserArrayBase() { }

        /// <summary>
        /// Instantiate a new <see cref="CredentialProviderBase"/> object.
        /// </summary>
        /// <param name="usageScenarios">The <see cref="CredentialProviderUsageScenarios"/> that this credential provider will support when requested by Windows.</param>
        protected CredentialProviderSetUserArrayBase(CredentialProviderUsageScenarios usageScenarios)
            : base(usageScenarios)
        { }

        /// <summary>
        /// Instantiate a new <see cref="CredentialProviderBase"/> object.
        /// </summary>
        /// <param name="credentialsDelegate">The <see cref="Action{CredentialCollection}"/> that will be invoked upon construction.</param>
        /// <param name="controlsDelegate">The <see cref="Action{CredentialControlCollection}"/> that will be invoked upon construction.</param>
        protected CredentialProviderSetUserArrayBase(Action<CredentialCollection> credentialsDelegate, Action<CredentialControlCollection> controlsDelegate)
            : base(credentialsDelegate, controlsDelegate)
        { }

        /// <summary>
        /// Instantiate a new <see cref="CredentialProviderBase"/> object.
        /// </summary>
        /// <param name="usageScenarios">The <see cref="CredentialProviderUsageScenarios"/> that this credential provider will support when requested by Windows.</param>
        /// <param name="credentialsDelegate">The <see cref="Action{CredentialCollection}"/> that will be invoked upon construction.</param>
        /// <param name="controlsDelegate">The <see cref="Action{CredentialControlCollection}"/> that will be invoked upon construction.</param>
        protected CredentialProviderSetUserArrayBase(CredentialProviderUsageScenarios usageScenarios, Action<CredentialCollection> credentialsDelegate, Action<CredentialControlCollection> controlsDelegate)
            : base(usageScenarios, credentialsDelegate, controlsDelegate)
        { }

        /// <summary>
        /// Instantiate a new <see cref="CredentialProviderBase"/> object that will wrap an existing credential provider.
        /// </summary>
        /// <param name="underlyingCredentialProviderGuid">The COM <see cref="Guid"/> of the credential provider that this credential provider should wrap.</param>
        /// <param name="incomingCredentialManipulator">Manipulate an incoming credential provider from the wrapped credential provider.</param>
        protected CredentialProviderSetUserArrayBase(Guid underlyingCredentialProviderGuid, Func<CredentialProviderUsageScenarios, CredentialBase> incomingCredentialManipulator)
            : base(underlyingCredentialProviderGuid, incomingCredentialManipulator)
        { }

        /// <summary>
        /// Instantiate a new <see cref="CredentialProviderBase"/> object that will wrap an existing credential provider.
        /// </summary>
        /// <param name="underlyingCredentialProviderGuid">The COM <see cref="Guid"/> of the credential provider that this credential provider should wrap.</param>
        /// <param name="controlsDelegate">The <see cref="Action{CredentialControlCollection}"/> that will be invoked when fields are requested by Windows.</param>
        /// <param name="incomingCredentialManipulator">Manipulate an incoming credential provider from the wrapped credential provider.</param>
        protected CredentialProviderSetUserArrayBase(Guid underlyingCredentialProviderGuid, Action<CredentialControlCollection> controlsDelegate, Func<CredentialProviderUsageScenarios, CredentialBase> incomingCredentialManipulator)
            : base(underlyingCredentialProviderGuid, controlsDelegate, incomingCredentialManipulator)
        { }

        /// <summary>
        /// Instantiate a new <see cref="CredentialProviderBase"/> object that will wrap an existing credential provider.
        /// </summary>
        /// <param name="underlyingCredentialProviderGuid">The COM <see cref="Guid"/> of the credential provider that this credential provider should wrap.</param>
        /// <param name="usageScenarios">The <see cref="CredentialProviderUsageScenarios"/> that this credential provider will support when requested by Windows.</param>
        /// <param name="controlsDelegate">The <see cref="Action{CredentialControlCollection}"/> that will be invoked upon construction.</param>
        /// <param name="incomingCredentialManipulator">Manipulate an incoming credential provider from the wrapped credential provider.</param>
        protected CredentialProviderSetUserArrayBase(Guid underlyingCredentialProviderGuid, CredentialProviderUsageScenarios usageScenarios, Action<CredentialControlCollection> controlsDelegate, Func<CredentialProviderUsageScenarios, CredentialBase> incomingCredentialManipulator)
            : base(underlyingCredentialProviderGuid, usageScenarios, controlsDelegate, incomingCredentialManipulator)
        { }

        #region Variables



        #endregion

        #region Properties



        #endregion

        #region Methods

        #region Credential Provider Interface Methods

        public int SetUserArray(ICredentialProviderUserArray users)
        {
            if (UnderlyingCredentialProvider != null && UnderlyingCredentialProvider is ICredentialProviderSetUserArray)
            {
                return ((ICredentialProviderSetUserArray)UnderlyingCredentialProvider).SetUserArray(users);
            }

            return HRESULT.E_NOTIMPL;
        }

        #endregion

        #endregion
    }
}