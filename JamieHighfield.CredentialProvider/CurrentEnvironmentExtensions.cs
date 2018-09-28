using JamieHighfield.CredentialProvider.Credentials;
using JamieHighfield.CredentialProvider.Interfaces;
using JamieHighfield.CredentialProvider.Providers;
using System;

namespace JamieHighfield.CredentialProvider
{
    /// <summary>
    /// Extension methods for the <see cref="ICurrentEnvironment"/> interface.
    /// </summary>
    public static class CurrentEnvironmentExtensions
    {
        #region Variables



        #endregion

        #region Properties



        #endregion

        #region Methods

        /// <summary>
        /// Gets a <see cref="Boolean"/> to indicate whether the current usage scenario is supported.
        /// </summary>
        /// <param name="environment">This interface is implemented by <see cref="CredentialProviderBase"/> and <see cref="CredentialBase"/> classes; used to parse selected properties to delegates. When used as an extension method, an argument for this parameter is automatically parsed.</param>
        /// <returns></returns>
        public static bool IsCurrentUsageScenarioSupported(this ICurrentEnvironment environment)
        {
            return environment.SupportedUsageScenarios.HasFlag(environment.CurrentUsageScenario);
        }

        #endregion
    }
}