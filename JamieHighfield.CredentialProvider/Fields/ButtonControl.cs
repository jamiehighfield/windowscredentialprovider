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