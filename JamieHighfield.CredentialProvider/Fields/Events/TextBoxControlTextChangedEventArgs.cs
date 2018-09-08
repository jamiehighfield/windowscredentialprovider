namespace JamieHighfield.CredentialProvider.Controls.Events
{
    public sealed class TextBoxControlTextChangedEventArgs
    {
        internal TextBoxControlTextChangedEventArgs(TextBoxControl textBoxControl)
        {
            TextBoxControl = textBoxControl;
        }

        #region Variables



        #endregion

        #region Properties

        public TextBoxControl TextBoxControl { get; private set; }

        #endregion

        #region Methods



        #endregion
    }
}