using JamieHighfield.CredentialProvider.Controls.Events;
using JamieHighfield.CredentialProvider.Controls.New;
using JamieHighfield.CredentialProvider.Credentials;
using System;

namespace JamieHighfield.CredentialProvider.Controls.Descriptors
{
    /// <summary>
    /// Options for the label descriptor.
    /// </summary>
    public sealed class LabelDescriptorOptions<TCredentialType> : DescriptorOptionsBase<TCredentialType>
        where TCredentialType : CredentialBase
    {
        /// <summary>
        /// Gets or sets the text for the control. This property is modifiable at runtime.
        /// </summary>
        public Func<TCredentialType, LabelControl, string> Text { get; set; }

        /// <summary>
        /// Gets or sets the size of text for the label. This property is not modifiable at runtime.
        /// </summary>
        public LabelControlSizes Size { get; set; }

        /// <summary>
        /// Gets or sets the delegate that is invoked when the text value is changed.
        /// </summary>
        public Func<TCredentialType, LabelControl, EventHandler<LabelControlTextChangedEventArgs>> TextChanged { get; set; }
    }
}