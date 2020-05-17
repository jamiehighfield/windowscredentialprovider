using JamieHighfield.CredentialProvider.Controls;
using JamieHighfield.CredentialProvider.Controls.Descriptors;
using JamieHighfield.CredentialProvider.Controls.New;
using JamieHighfield.CredentialProvider.Logging;
using JamieHighfield.CredentialProvider.Providers;
using JamieHighfield.CredentialProvider.Sample.Credentials;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace JamieHighfield.CredentialProvider.Sample.Providers
{
    //[ComVisible(true)]
    //[Guid("0C314077-519C-41E9-A865-F9E648CF76E6")]
    //[ClassInterface(ClassInterfaceType.None)]
    //[ProgId("WrappedCredentialProviderSample")]
    public sealed class ConnectableWrappedCredentialProviderSample : CredentialProviderSetUserArrayBase<ConnectableWrappedCredentialSample>
    {
        public ConnectableWrappedCredentialProviderSample()
            : base(SystemCredentialProviders.Password, CredentialProviderUsageScenarios.All)
        {
            GlobalLogger.Enabled = true;

            IncomingCredentialFactory = (environment) =>
            {
                //For wrapped credential providers, each credential that is enumerated by the wrapped credential provider  must be
                //wrapped in a managed credential. If the incoming credential has extended the functionality by using one of the extension
                //interfaces, such as 'IConnectableCredential', then the credential returned here must extend the correct base classes in
                //order to maintain the wrapped functionality. This delegate is used to provide this.

                //For most use cases, the Windows credential provider does not verify credentials in the credentials UI dialog. In most cases,
                //it would not make sense for additional connections to be made using the 'IConnectableCredential' interface.

                //if (environment.CurrentUsageScenario == CredentialProviderUsageScenarios.CredentialsDialog)
                {
                    //return new WrappedCredentialSample();
                }
                //else
                {
                    return new ConnectableWrappedCredentialSample();
                }
            };

            DescriptorsFactory = (environment, descriptors) =>
            {
                //Add all the control descriptors. While the controls available are applied at the credential provider level, the
                //actual contents of the controls can differ between credentials and can be modified in the 'CredentialBase.Initiated' method.

                descriptors
                    .AddLink((options) =>
                    {
                        options.Visibility = CredentialFieldVisibilities.SelectedCredential;

                        options.Text = (credential, control) => "Reset Password";

                        options.Click = (credential, control) => (sender, eventArgs) =>
                        {
                            MessageBox.Show(environment.MainWindowHandle, "Reset Password");
                        };
                    })
                    .AddLabel((options) =>
                    {
                        options.Visibility = CredentialFieldVisibilities.SelectedCredential;

                        options.Text = (credential, control) => "Checked: False";
                    })
                    .AddCheckBox((options) =>
                    {
                        options.Visibility = CredentialFieldVisibilities.SelectedCredential;

                        options.Label = "Remember Me";

                        options.CheckChange = (credential, control) => (sender, eventArgs) =>
                        {
                            credential.Controls.FirstOfControlType<LabelControl>().Text = "Checked: " + eventArgs.Control.Checked.ToString();
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