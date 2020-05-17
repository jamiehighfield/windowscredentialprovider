using JamieHighfield.CredentialProvider.Controls;
using JamieHighfield.CredentialProvider.Controls.Descriptors;
using JamieHighfield.CredentialProvider.Controls.New;
using JamieHighfield.CredentialProvider.Credentials;
using JamieHighfield.CredentialProvider.Logging;
using JamieHighfield.CredentialProvider.Providers;
using JamieHighfield.CredentialProvider.Providers.Interfaces;
using JamieHighfield.CredentialProvider.Sample.Credentials;
using System.Runtime.InteropServices;

namespace JamieHighfield.CredentialProvider.Sample.Providers
{
    [ComVisible(true)]
    [Guid("5A09A8E2-2138-4F7C-BF74-3AD9DD710123")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("WrappedCredentialProviderSample")]
    public sealed class WrappedCredentialProviderSample : CredentialProviderBase<WrappedCredentialSample>, IUserArrayCredentialProvider
    {
        public WrappedCredentialProviderSample()
            : base(SystemCredentialProviders.Password, CredentialProviderUsageScenarios.All)
        {
            GlobalLogger.Enabled = false;

            IncomingCredentialFactory = (environment) =>
            {
                //For wrapped credential providers, each credential that is enumerated by the wrapped credential provider must be
                //wrapped in a managed credential. If the incoming credential has extended the functionality by using one of the extension
                //interfaces, such as 'IConnectableCredential', then the credential returned here must extend the correct base classes in
                //order to maintain the wrapped functionality. This delegate is used to provide this.

                //For most use cases, the Windows credential provider does not verify credentials in the credentials UI dialog. In most cases,
                //it would not make sense for additional connections to be made using the 'IConnectableCredential' interface.

                if (environment.CurrentUsageScenario == CredentialProviderUsageScenarios.CredentialsDialog)
                {
                    return new WrappedCredentialSample();
                }
                else
                {
                    return new ConnectableWrappedCredentialSample();
                }
            };

            DescriptorsFactory = (environment, descriptors) =>
            {
                //Add all the control descriptors. While the controls available are applied at the credential provider level, the
                //actual contents of the controls can differ between credentials.

                descriptors
                    .AddLink(CredentialFieldVisibilities.SelectedCredential, (options) =>
                    {
                        options.Text = (credential, control) => "Reset Password";

                        options.Click = (credential, control) => (sender, eventArgs) =>
                        {
                            //Because the credential implementation we are using implements the 'IExtendedCredential' interface (in line with the underlying
                            //credential implementing the 'ICredentialProviderCredential2' interface), we get access to the
                            //'ICredentialProviderCredentialEvents2' events interface. Therefore, we can call 'BeginUpdate()' here to block update the UI with
                            //UI changes to be more efficient. Make sure to call 'EndUpdate()' to process update changes.

                            credential.BeginUpdate();

                            //MessageBox.Show(environment.MainWindowHandle, "Reset Password for user '" + credential.Controls.FirstOfControlType<TextBoxControl>().Text + "'.");

                            credential.Controls.FirstOfControlType<TextBoxControl>().Text = "qwe";

                            credential.EndUpdate();
                        };
                    });
            };
        }

        #region Variables



        #endregion

        #region Properties



        #endregion

        #region Methods



        #endregion
    }
}