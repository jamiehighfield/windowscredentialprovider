using JamieHighfield.CredentialProvider.Controls;
using JamieHighfield.CredentialProvider.Controls.Descriptors;
using JamieHighfield.CredentialProvider.Logging;
using JamieHighfield.CredentialProvider.Providers;
using JamieHighfield.CredentialProvider.Sample.Credentials;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace JamieHighfield.CredentialProvider.Sample.Providers
{
    [ComVisible(true)]
    [Guid("5A09A8E2-2138-4F7C-BF74-3AD9DD710123")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("WrappedCredentialProviderSample")]
    public sealed class WrappedCredentialProviderSample : CredentialProviderSetUserArrayBase<WrappedCredentialSample>
    {
        public WrappedCredentialProviderSample()
            : base(SystemCredentialProviders.Password, CredentialProviderUsageScenarios.All)
        {
            GlobalLogger.Enabled = true;

            IncomingCredentialFactory = (environment) =>
            {
                //For wrapped credential providers, each credential that is enumerated by the wrapped credential provider  must be
                //wrapped in a managed credential. If the incoming credential has extended the functionality by using one of the extension
                //interfaces, such as 'IConnectableCredential', then the credential returned here must extend the correct base classes in
                //order to maintain the wrapped functionality. This delegate is used to provide this.

                return new WrappedCredentialSample();
            };

            DescriptorsFactory = (environment, descriptors) =>
            {
                //Add all the control descriptors. While the controls available are applied at the credential provider level, the
                //actual contents of the controls can differ between credentials and can be modified in the 'CredentialBase.Initiated' method.

                descriptors
                    .AddLink((options) =>
                    {
                        options.Visibility = CredentialFieldVisibilities.SelectedCredential;

                        options.Text = (credential) => "Reset Password";

                        options.Click = (credential) => (sender, eventArgs) =>
                        {
                            MessageBox.Show(environment.MainWindowHandle, "Reset Password");
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