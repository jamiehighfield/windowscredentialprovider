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
    public sealed class ButtonControl : LabelledCredentialControlBase
    {
        public ButtonControl(string label)
            : base(CredentialFieldTypes.Text, label, CredentialFieldVisibilities.SelectedCredential)
        { }

        #region Variables

        private string _text;

        #endregion

        #region Properties



        #endregion

        #region Methods

        internal override _CREDENTIAL_PROVIDER_FIELD_TYPE GetNativeFieldType()
        {
            return _CREDENTIAL_PROVIDER_FIELD_TYPE.CPFT_SUBMIT_BUTTON;
        }
        
        #endregion
    }
}