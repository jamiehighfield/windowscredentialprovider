using JamieHighfield.CredentialProvider.Controls.New;
using JamieHighfield.CredentialProvider.Credentials;
using System;

namespace JamieHighfield.CredentialProvider.Controls.Descriptors
{
    public sealed class ButtonDescriptorOptions : DescriptorOptionsBase
    {
        /// <summary>
        /// Gets or sets the control that this button should be adjacent to. This property is not modifiable at runtime.
        /// </summary>
        public Func<CredentialBase, NewCredentialControlBase> AdjacentControl { get; set; }
    }
}