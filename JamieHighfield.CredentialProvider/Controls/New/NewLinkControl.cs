using JamieHighfield.CredentialProvider.Credentials;
using JamieHighfield.CredentialProvider.Interop;
using System;

namespace JamieHighfield.CredentialProvider.Controls.New
{
    public sealed class NewLinkControl : NewCredentialControlBase
    {
        internal NewLinkControl(CredentialBase credential, CredentialFieldVisibilities visibility, Func<CredentialBase, string> text, Func<CredentialBase, EventHandler> textChanged, Func<CredentialBase, EventHandler> click)
            : base(credential, visibility)
        {
            _text = new NewDynamicPropertyStore<string>(this, text);

            _textChanged = new NewDynamicPropertyStore<EventHandler>(this, textChanged);
            _click = new NewDynamicPropertyStore<EventHandler>(this, click);
        }

        private NewDynamicPropertyStore<string> _text = null;

        private NewDynamicPropertyStore<EventHandler> _textChanged = null;

        private NewDynamicPropertyStore<EventHandler> _click = null;

        /// <summary>
        /// Gets the text for this control.
        /// </summary>
        public string Text
        {
            get
            {
                return _text.Value;
            }
        }

        /// <summary>
        /// Gets the text changed event handler for this control.
        /// </summary>
        public EventHandler TextChanged
        {
            get
            {
                return _textChanged.Value;
            }
        }

        /// <summary>
        /// Gets the click event handler for this control.
        /// </summary>
        public EventHandler Click
        {
            get
            {
                return _click.Value;
            }
        }

        internal override _CREDENTIAL_PROVIDER_FIELD_TYPE GetNativeFieldType()
        {
            return _CREDENTIAL_PROVIDER_FIELD_TYPE.CPFT_COMMAND_LINK;
        }
    }
}