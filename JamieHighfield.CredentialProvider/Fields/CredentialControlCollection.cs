using JamieHighfield.CredentialProvider.Controls;
using JamieHighfield.CredentialProvider.Controls.Exceptions;
using JamieHighfield.CredentialProvider.Credentials;
using System;
using System.Collections.Generic;

namespace JamieHighfield.CredentialProvider
{
    public sealed class CredentialControlCollection : List<CredentialControlBase>
    {
        #region Variables



        #endregion

        #region Properties



        #endregion

        #region Methods

        public new CredentialControlCollection Add(CredentialControlBase control)
        {
            if (control == null)
            {
                throw new ArgumentNullException(nameof(control));
            }

            base.Add(control);

            return this;
        }

        #endregion
    }
}