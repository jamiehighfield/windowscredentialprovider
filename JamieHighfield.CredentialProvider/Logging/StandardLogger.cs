using System;
using JamieHighfield.CredentialProvider.Logging.Interfaces;

namespace JamieHighfield.CredentialProvider.Logging
{
    public sealed class StandardLogger : ILogger
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
        public void Log(LogLevels level, string message)
        {
            Console.WriteLine("Information: " + message);
        }

        /// <summary>
        /// Log an event.
        /// </summary>
        /// <param name="level">The severity level of the event.</param>
        /// <param name="message">The message describing the event.</param>
        /// <param name="exception">The associated exception with the event.</param>
        public void Log(LogLevels level, string message, Exception exception)
        {
            Console.WriteLine("Warning: " + message);
        }

        #endregion
    }
}