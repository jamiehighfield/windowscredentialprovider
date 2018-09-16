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

using JamieHighfield.CredentialProvider.Interop;
using JamieHighfield.CredentialProvider.Providers;
using System;

namespace JamieHighfield.CredentialProvider.Controls
{
    public sealed class ButtonControl : LabelledCredentialControlBase
    {
        public ButtonControl(string label, CredentialControlBase adjacentControl)
            : this(CredentialFieldVisibilities.SelectedCredential, label, adjacentControl)
        { }

        public ButtonControl(CredentialFieldVisibilities visibility, string label, CredentialControlBase adjacentControl)
            : base(CredentialControlTypes.TextBox, label)
        {
            AdjacentControl = adjacentControl;
        }

        public ButtonControl(Func<CredentialProviderUsageScenarios, CredentialFieldVisibilities> visibilityDelegate, string label, CredentialControlBase adjacentControl)
            : base(CredentialControlTypes.TextBox, visibilityDelegate, label)
        {
            AdjacentControl = adjacentControl;
        }

        #region Variables



        #endregion

        #region Properties

        public CredentialControlBase AdjacentControl { get; private set; }

        #endregion

        #region Methods

        internal override _CREDENTIAL_PROVIDER_FIELD_TYPE GetNativeFieldType()
        {
            return _CREDENTIAL_PROVIDER_FIELD_TYPE.CPFT_SUBMIT_BUTTON;
        }
        
        #endregion
    }
}