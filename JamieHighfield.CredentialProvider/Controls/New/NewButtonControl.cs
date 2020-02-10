using JamieHighfield.CredentialProvider.Credentials;
using JamieHighfield.CredentialProvider.Interop;
using System;

namespace JamieHighfield.CredentialProvider.Controls.New
{
    public sealed class NewButtonControl : NewCredentialControlBase
    {
        internal NewButtonControl(CredentialBase credential, CredentialFieldVisibilities visibility, Func<CredentialBase, NewCredentialControlBase> adjacentControl)
            : base(credential, visibility)
        {
            _adjacentControl = new NewDynamicPropertyStore<NewCredentialControlBase>(this, adjacentControl);
        }

        private NewDynamicPropertyStore<NewCredentialControlBase> _adjacentControl = null;
        
        /// <summary>
        /// Gets the adjacent control for this control.
        /// </summary>
        public NewCredentialControlBase AdjacentControl
        {
            get
            {
                return _adjacentControl.Value;
            }
        }

        internal override _CREDENTIAL_PROVIDER_FIELD_TYPE GetNativeFieldType()
        {
            return _CREDENTIAL_PROVIDER_FIELD_TYPE.CPFT_SUBMIT_BUTTON;
        }
    }
}