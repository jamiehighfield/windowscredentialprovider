using JamieHighfield.CredentialProvider.Controls.Events;
using JamieHighfield.CredentialProvider.Credentials;
using System;

namespace JamieHighfield.CredentialProvider.Controls.Descriptors
{
    /// <summary>
    /// Options for the textbox descriptor.
    /// </summary>
    public sealed class TextBoxDescriptorOptions : DescriptorOptionsBase
    {
        /// <summary>
        /// Gets or sets the label for the control. This property is not modifiable at runtime.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the text for the control. This property is modifiable at runtime.
        /// </summary>
        public Func<CredentialBase, string> Text { get; set; }
        
        /// <summary>
        /// Gets or sets the delegate that is invoked when the text value is changed.
        /// </summary>
        public Func<CredentialBase, EventHandler<TextBoxControlTextChangedEventArgs>> TextChanged { get; set; }

        /// <summary>
        /// Gets or sets whether this textbox should be a password textbox or not. This property is not modifiable at runtime.
        /// </summary>
        public bool Password { get; set; }

        /// <summary>
        /// Gets or sets whether this textbox is auto-focussed. If another textbox has <see cref="Focussed"/> set to true, and it comes after this one in the list of descriptors, then that textbox shall be focussed instead. This property is not modifiable at runtime.
        /// </summary>
        public Func<CredentialBase, bool> Focussed { get; set; }
    }
}