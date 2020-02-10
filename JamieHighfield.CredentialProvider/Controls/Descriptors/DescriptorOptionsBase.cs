using JamieHighfield.CredentialProvider.Credentials;
using System;

namespace JamieHighfield.CredentialProvider.Controls.Descriptors
{
    /// <summary>
    /// A base class for all options for control descriptors.
    /// </summary>
    public abstract class DescriptorOptionsBase
    {
        /// <summary>
        /// Gets or sets the visibility for this control. This property is not modifiable at runtime.
        /// </summary>
        public CredentialFieldVisibilities Visibility { get; set; }
    }
}