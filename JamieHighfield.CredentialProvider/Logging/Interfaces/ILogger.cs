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