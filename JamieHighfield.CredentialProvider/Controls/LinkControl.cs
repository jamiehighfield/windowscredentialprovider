using System;
using JamieHighfield.CredentialProvider.Controls.Events;
using JamieHighfield.CredentialProvider.Interop;
using JamieHighfield.CredentialProvider.Providers;

namespace JamieHighfield.CredentialProvider.Controls
{
    public sealed class LinkControl : CredentialControlBase
    {
        public LinkControl(string text)
            : this(CredentialFieldVisibilities.SelectedCredential, text)
        { }

        public LinkControl(CredentialFieldVisibilities visibility, string text)
            : base(CredentialControlTypes.Link, visibility)
        {
            Text = text;
        }

        public LinkControl(Func<CredentialProviderUsageScenarios, CredentialFieldVisibilities> visibilityDelegate, string text)
            : base(CredentialControlTypes.Link, visibilityDelegate)
        {
            Text = text;
        }

        #region Variables



        #endregion

        #region Properties

        public string Text { get; set; }

        #endregion

        #region Methods

        internal override _CREDENTIAL_PROVIDER_FIELD_TYPE GetNativeFieldType()
        {
            return _CREDENTIAL_PROVIDER_FIELD_TYPE.CPFT_COMMAND_LINK;
        }

        internal void InvokeClicked(object sender, LinkControlClickedEventArgs eventArgs)
        {
            Clicked?.Invoke(sender, eventArgs);
        }

        #endregion

        #region Events

        public event EventHandler<LinkControlClickedEventArgs> Clicked;

        #endregion
    }
}