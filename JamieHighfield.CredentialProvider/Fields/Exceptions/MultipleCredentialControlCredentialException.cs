using System;

namespace JamieHighfield.CredentialProvider.Controls.Exceptions
{
    public sealed class MultipleCredentialControlCredentialException : Exception
    {
        public MultipleCredentialControlCredentialException(CredentialControlBase control)
            : base("The control '" + nameof(control) + "' cannot belong to more than one credential.")
        {
            Control = control;
        }

        #region Variables



        #endregion

        #region Properties

        public CredentialControlBase Control { get; private set; }

        #endregion

        #region Methods



        #endregion
    }
}