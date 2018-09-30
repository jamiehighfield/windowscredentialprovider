using JamieHighfield.CredentialProvider.Credentials;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace JamieHighfield.CredentialProvider.Controls
{
    public sealed class ReadOnlyCredentialControlCollection : ReadOnlyCollection<CredentialControlBase>
    {
        internal ReadOnlyCredentialControlCollection(List<CredentialControlBase> controls)
            : base(controls)
        { }

        internal ReadOnlyCredentialControlCollection(List<CredentialControlBase> controls, CredentialBase credential)
            : base(controls)
        {
            foreach (CredentialControlBase control in controls)
            {
                control.Credential = credential;
            }
        }

        #region Variables



        #endregion

        #region Properties



        #endregion

        #region Methods



        #endregion
    }
}