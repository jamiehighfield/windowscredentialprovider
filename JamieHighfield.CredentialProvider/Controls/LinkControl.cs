using JamieHighfield.CredentialProvider.Credentials;
using JamieHighfield.CredentialProvider.Interop;
using System;

namespace JamieHighfield.CredentialProvider.Controls.New
{
    public sealed class LinkControl : CredentialControlBase
    {
        internal LinkControl(CredentialBase credential, CredentialFieldVisibilities visibility, bool forwardToField, Func<LinkControl, Func<CredentialBase, LinkControl, string>> text, Func<LinkControl, Func<CredentialBase, LinkControl, EventHandler>> click)
            : base(credential, visibility, forwardToField)
        {
            if (text is null)
            {
                _text = new DynamicPropertyStore<string>(this, (innerCredential) => string.Empty);
            }
            else
            {
                _text = new DynamicPropertyStore<string>(this, (innerCredential) => (text.Invoke(this)?.Invoke(innerCredential, this) ?? ""));
            }

            if (click is null)
            {
                _click = new DynamicPropertyStore<EventHandler>(this, (innerCredential) => null);
            }
            else
            {
                _click = new DynamicPropertyStore<EventHandler>(this, (innerCredential) => click.Invoke(this)?.Invoke(innerCredential, this));
            }
        }

        private DynamicPropertyStore<string> _text = null;

        private DynamicPropertyStore<EventHandler> _click = null;

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
        /// Gets the click event handler for this control.
        /// </summary>
        public EventHandler Click
        {
            get
            {
                return _click.Value;
            }
        }
    }
}