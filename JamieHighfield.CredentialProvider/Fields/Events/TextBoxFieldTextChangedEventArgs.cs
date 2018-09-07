namespace JamieHighfield.CredentialProvider.Fields.Events
{
    public sealed class TextBoxFieldTextChangedEventArgs
    {
        internal TextBoxFieldTextChangedEventArgs(TextBoxField textBoxField)
        {
            TextBoxField = textBoxField;
        }

        #region Variables



        #endregion

        #region Properties

        public TextBoxField TextBoxField { get; private set; }

        #endregion

        #region Methods



        #endregion
    }
}