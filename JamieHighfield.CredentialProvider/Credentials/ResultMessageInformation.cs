using System;

namespace JamieHighfield.CredentialProvider.Credentials
{
    /// <summary>
    /// A wrapper class to wrap resulting messages in.
    /// </summary>
    public sealed class ResultMessageInformation
    {
        internal ResultMessageInformation(string message, ResultMessageInformationIcons icon)
        {
            Message = message ?? throw new ArgumentNullException(nameof(message));
            Icon = icon;
        }

        /// <summary>
        /// Gets the message.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Gets the icon associated with the message.
        /// </summary>
        public ResultMessageInformationIcons Icon { get; }
    }

    public enum ResultMessageInformationIcons
    {
        /// <summary>
        /// None.
        /// </summary>
        None,
        /// <summary>
        /// Information the user should be aware of.
        /// </summary>
        Information,
        /// <summary>
        /// An error or problem has occurred.
        /// </summary>
        Warning,
        /// <summary>
        /// An unexpected and unforseen error has occurred.
        /// </summary>
        Error
    }
}
