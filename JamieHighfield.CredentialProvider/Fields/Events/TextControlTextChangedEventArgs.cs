namespace JamieHighfield.CredentialProvider.Controls.Events
{
    public sealed class TextControlTextChangedEventArgs
    {
        internal TextControlTextChangedEventArgs(LabelControl labelControl)
        {
            LabelControl = labelControl;
        }

        #region Variables



        #endregion

        #region Properties

        public LabelControl LabelControl { get; private set; }

        #endregion

        #region Methods



        #endregion
    }
}