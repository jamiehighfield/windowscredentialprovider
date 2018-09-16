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

        internal LogonResponse ProcessSequencePipeline(LogonPackage logonPackage)
        {
            if (logonPackage == null)
            {
                throw new ArgumentNullException(nameof(logonPackage));
            }

            LogonResponse currentLogonResponse = null;

            LogonPackage currentLogonPackage = logonPackage;

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