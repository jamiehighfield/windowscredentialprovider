using JamieHighfield.CredentialProvider.Controls.New;
using JamieHighfield.CredentialProvider.Credentials;
using System;

namespace JamieHighfield.CredentialProvider.Controls.Descriptors
{
    /// <summary>
    /// Options for the link descriptor.
    /// </summary>
    public sealed class LinkDescriptorOptions<TCredentialType> : DescriptorOptionsBase<TCredentialType>
        where TCredentialType : CredentialBase
    {
        /// <summary>
        /// Gets or sets the text for the control. This property is modifiable at runtime.
        /// </summary>
        public Func<TCredentialType, LinkControl, string> Text { get; set; }

        /// <summary>
        /// Gets or sets the delegate that is invoked when the text value is changed.
        /// </summary>
        public Func<TCredentialType, LinkControl, EventHandler> TextChanged { get; set; }

        /// <summary>
        /// Gets or sets the click event for this link. This property is modifiable at runtime.
        /// </summary>
        public Func<TCredentialType, LinkControl, EventHandler> Click { get; set; }
    }
}