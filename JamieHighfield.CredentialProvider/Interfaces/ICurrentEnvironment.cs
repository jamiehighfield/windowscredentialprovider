using JamieHighfield.CredentialProvider.Credentials;
using JamieHighfield.CredentialProvider.Providers;

namespace JamieHighfield.CredentialProvider.Interfaces
{
    /// <summary>
    /// This interface is implemented by <see cref="CredentialProviderBase"/> and <see cref="CredentialBase"/> classes; used to parse selected properties to delegates.
    /// </summary>
    public interface ICurrentEnvironment
    {
        #region Variables



        #endregion

        #region Properties

        /// <summary>
        /// Gets the <see cref="CredentialProviderUsageScenarios"/> that this credential provider will support when requested by Windows.
        /// </summary>
        CredentialProviderUsageScenarios SupportedUsageScenarios { get; }

        /// <summary>
        /// Gets the <see cref="CredentialProviderUsageScenarios"/> representing the current scenario under which the credential provider is operating.
        /// </summary>
        CredentialProviderUsageScenarios CurrentUsageScenario { get; }
        
        /// <summary>
        /// Gets the handle of the main window for the parent process.
        /// </summary>
        WindowHandle MainWindowHandle { get; }

        #endregion

        #region Methods



        #endregion
    }
}