using JamieHighfield.CredentialProvider.Controls.Events;
using JamieHighfield.CredentialProvider.Controls.New;
using JamieHighfield.CredentialProvider.Credentials;
using System;

namespace JamieHighfield.CredentialProvider.Controls.Descriptors
{
    public sealed class CheckBoxDescriptorOptions<TCredentialType> : DescriptorOptionsBase<TCredentialType>
        where TCredentialType : CredentialBase
    {
        /// <summary>
        /// Gets or sets the label for the control. This property is not modifiable at runtime.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the default check state for the control. This property is modifiable at runtime.
        /// </summary>
        public Func<TCredentialType, CheckBoxControl, bool> Checked { get; set; }

        /// <summary>
        /// Gets or sets the delegate that is invoked when the check box value is changed.
        /// </summary>
        public Func<TCredentialType, CheckBoxControl, EventHandler<CheckBoxControlCheckChangedEventArgs>> CheckChange { get; set; }
    }
}