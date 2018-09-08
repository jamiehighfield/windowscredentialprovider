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

using CredProvider.NET.Interop2;

namespace JamieHighfield.CredentialProvider.Controls
{
    public sealed class CheckBoxControl : LabelledCredentialControlBase
    {
        public CheckBoxControl(string label, bool @checked)
            : base(CredentialFieldTypes.TextBox, label)
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
    }
}