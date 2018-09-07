using CredProvider.NET.Interop2;

namespace JamieHighfield.CredentialProvider.Fields
{
    public sealed class ButtonField : CredentialFieldBase
    {
        public ButtonField()
            : base(CredentialFieldTypes.Button)
        { }

        #region Variables



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