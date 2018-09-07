namespace JamieHighfield.CredentialProvider.Fields.Events
{
    public sealed class TextFieldTextChangedEventArgs
    {
        internal TextFieldTextChangedEventArgs(TextField textField)
        {
            TextField = textField;
        }

        #region Variables



        #endregion

        #region Properties

        public TextField TextField { get; private set; }

        #endregion

        #region Methods



        #endregion
    }
}