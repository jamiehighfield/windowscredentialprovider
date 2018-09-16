using System;

namespace JamieHighfield.CredentialProvider.Logging.Interfaces
{
    public interface ILogger
    {
        #region Variables



        #endregion

        #region Properties



        #endregion

        #region Methods

        /// <summary>
        /// Log an event.
        /// </summary>
        /// <param name="level">The severity level of the event.</param>
        /// <param name="message">The message describing the event.</param>
        void Log(LogLevels level, string message);

        /// <summary>
        /// Log an event.
        /// </summary>
        /// <param name="level">The severity level of the event.</param>
        /// <param name="message">The message describing the event.</param>
        /// <param name="exception">The associated exception with the event.</param>
        void Log(LogLevels level, string message, Exception exception);

        #endregion
    }
}