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

using JamieHighfield.CredentialProvider.Controls.Events;
using JamieHighfield.CredentialProvider.Interop;
using JamieHighfield.CredentialProvider.Providers;
using System;

namespace JamieHighfield.CredentialProvider.Controls
{
    public sealed class CheckBoxControl : LabelledCredentialControlBase
    {
        public CheckBoxControl(string label, bool @checked)
            : this(CredentialFieldVisibilities.SelectedCredential, label, @checked)
        { }

        public CheckBoxControl(CredentialFieldVisibilities visibility, string label, bool @checked)
            : base(CredentialControlTypes.TextBox, label)
        {
            Checked = @checked;
        }

        public CheckBoxControl(Func<CredentialProviderUsageScenarios, CredentialFieldVisibilities> visibilityDelegate, string label, bool @checked)
            : base(CredentialControlTypes.TextBox, visibilityDelegate, label)
        {
            Checked = @checked;
        }

        #region Variables



        #endregion

        #region Properties

        public bool Checked { get; internal set; }

        #endregion

        #region Methods

        internal override _CREDENTIAL_PROVIDER_FIELD_TYPE GetNativeFieldType()
        {
            return _CREDENTIAL_PROVIDER_FIELD_TYPE.CPFT_CHECKBOX;
        }

        #endregion

        #region Events

        public event EventHandler<CheckBoxControlCheckChangedEventArgs> CheckChanged;

        #endregion
    }
}