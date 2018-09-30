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

namespace JamieHighfield.CredentialProvider.Logon
{
    public abstract class LogonSequencePipelineBase
    {
        internal LogonSequencePipelineBase(LogonSequenceCollection sequences)
        {
            Sequences = sequences ?? throw new ArgumentNullException(nameof(sequences));
        }

        #region Variables



        #endregion

        #region Properties

        internal LogonSequenceCollection Sequences { get; private set; }

        #endregion

        #region Methods

        internal void AddSequence(LogonSequenceBase sequence)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException(nameof(sequence));
            }

            Sequences.Add(sequence);
        }

        internal LogonResponse ProcessSequencePipeline(IncomingLogonPackage logonPackage)
        {
            if (logonPackage == null)
            {
                throw new ArgumentNullException(nameof(logonPackage));
            }

            LogonResponse currentLogonResponse = null;

            IncomingLogonPackage currentLogonPackage = logonPackage;

            foreach (LogonSequenceBase sequence in Sequences)
            {
                currentLogonResponse = sequence.ProcessSequence(currentLogonPackage, currentLogonResponse?.WindowsLogonPackage);

                currentLogonPackage = currentLogonResponse?.LogonPackage;
            }

            return currentLogonResponse;
        }

        #endregion
    }
}