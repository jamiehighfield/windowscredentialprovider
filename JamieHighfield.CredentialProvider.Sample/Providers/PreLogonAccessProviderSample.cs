using JamieHighfield.CredentialProvider.Controls;
using JamieHighfield.CredentialProvider.Controls.Descriptors;
using JamieHighfield.CredentialProvider.Controls.New;
using JamieHighfield.CredentialProvider.Logging;
using JamieHighfield.CredentialProvider.Providers;
using JamieHighfield.CredentialProvider.Sample.Credentials;
using System.Runtime.InteropServices;

namespace JamieHighfield.CredentialProvider.Sample.Providers
{
    [ComVisible(true)]
    [Guid("E8C5E3AB-AE39-4543-9FC6-5C9CD5719DD3")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("PreLogonAccessProviderSample")]
    public sealed class PreLogonAccessProviderSample : CredentialProviderBase<PreLogonAccessProviderCredentialSample>
    {
        public PreLogonAccessProviderSample()
            : base(CredentialProviderUsageScenarios.PreLogonAccessProvider)
        {
            GlobalLogger.Enabled = true;

            CredentialsFactory = (credentials) =>
            {
                credentials.Add(new PreLogonAccessProviderCredentialSample());
            };

            DescriptorsFactory = (environment, descriptors) =>
            {
                //Add all the control descriptors. While the controls available are applied at the credential provider level, the
                //actual contents of the controls can differ between credentials and can be modified in the 'CredentialBase.Initiated' method.

                descriptors
                    .AddLabel((options) =>
                    {
                        options.Visibility = CredentialFieldVisibilities.SelectedCredential;

                        options.Size = LabelControlSizes.Small;

                        options.Text = (credential, control) => "In order to login to the network, you first need to connect to the VPN. Enter your VPN user name and password below.";
                    })
                    .AddTextBox((options) =>
                    {
                        options.Visibility = CredentialFieldVisibilities.SelectedCredential;

                        options.Label = "VPN User name";

                        options.Focussed = (credential, control) => true;
                    })
                    .AddPasswordTextBox((options) =>
                    {
                        options.Visibility = CredentialFieldVisibilities.SelectedCredential;

                        options.Label = "Password";
                    });
                    //.AddButton((options) =>
                    //{
                    //    options.Visibility = CredentialFieldVisibilities.SelectedCredential;

                //    options.AdjacentControl = (credential) => credential.Controls.FirstOfControlType<TextBoxControl>(1);
                //});
            };
        }
    }
}