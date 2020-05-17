using JamieHighfield.CredentialProvider.Credentials;
using JamieHighfield.CredentialProvider.Interop;
using System;

namespace JamieHighfield.CredentialProvider.Controls.New
{
    public sealed class ButtonControl : CredentialControlBase
    {
        internal ButtonControl(CredentialBase credential, CredentialFieldVisibilities visibility, bool forwardToField, Func<CredentialBase, CredentialControlBase> adjacentControl)
            : base(credential, visibility, forwardToField)
        {
            _adjacentControl = new DynamicPropertyStore<CredentialControlBase>(this, adjacentControl);
        }

        private DynamicPropertyStore<CredentialControlBase> _adjacentControl = null;
        
        /// <summary>
        /// Gets the adjacent control for this control.
        /// </summary>
        public CredentialControlBase AdjacentControl
        {
            get
            {
                return _adjacentControl.Value;
            }
        }
    }
}