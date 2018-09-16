/* COPYRIGHT NOTICE
 * 
 * Copyright © Jamie Highfield 2018. All rights reserved.
 * 
 * This library is protected by UK, EU & international copyright laws and treaties. Unauthorised
 * reproduction of this library outside of the constraints of the accompanied license, or any
 * portion of it, may result in severe criminal penalties that will be prosecuted to the
 * maximum extent possible under the law.
 * 
 */

using JamieHighfield.CredentialProvider.Logging.Interfaces;
using System;
using System.Diagnostics;
using System.Reflection;

namespace JamieHighfield.CredentialProvider.Logging
{
    /// <summary>
    /// Use this class to log events in a credential provider or a credential that it enumerates.
    /// </summary>
    public static class GlobalLogger
    {
        static GlobalLogger()
        {
            Logger = new StandardLogger();
        }

        #region Variables



        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets whether logging should be enabled.
        /// </summary>
        public static bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ILogger"/> implementation that the logger will use.
        /// </summary>
        public static  ILogger Logger { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Log an event.
        /// </summary>
        /// <param name="level">The severity level of the event.</param>
        /// <param name="message">The message describing the event.</param>
        public static void Log(LogLevels level, string message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (Enabled == true)
            {
                Logger?.Log(level, message);
            }
        }

        /// <summary>
        /// Log an event.
        /// </summary>
        /// <param name="level">The severity level of the event.</param>
        /// <param name="message">The message describing the event.</param>
        /// <param name="exception">The associated exception with the event.</param>
        public static void Log(LogLevels level, string message, Exception exception)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            if (Enabled == true)
            {
                Logger?.Log(level, message, exception);
            }
        }

        /// <summary>
        /// Log a method call event.
        /// </summary>
        public static void LogMethodCall()
        {
            if (Enabled == true)
            {
                MethodBase method = new StackTrace().GetFrame(1).GetMethod();

                string methodName = method.DeclaringType?.Name + "." + method.Name;

                Logger?.Log(LogLevels.Information, "[" + methodName + "] called.");
            }
        }

        #endregion
    }
}