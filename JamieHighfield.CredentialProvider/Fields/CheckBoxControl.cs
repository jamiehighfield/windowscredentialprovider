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