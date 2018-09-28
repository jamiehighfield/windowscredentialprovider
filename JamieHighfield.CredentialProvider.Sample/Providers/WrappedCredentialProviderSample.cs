using JamieHighfield.CredentialProvider.Controls;
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
    public sealed class WrappedCredentialProviderSample : CredentialProviderSetUserArrayBase
    {
        public WrappedCredentialProviderSample()
            : base(SystemCredentialProviders.Password, (environment, controls) =>
            {
                //Add a new link control to the controls that this credential provider will expose (on top of the wrapped fields). This
                //will appear at the end of the wrapped fields.

                controls
                    .AddLink(CredentialFieldVisibilities.SelectedCredential, "Reset Password", (_sender, _eventArgs) =>
                    {
                        //Use the main window handle parsed in from the current environment.

                        MessageBox.Show(environment.MainWindowHandle, "Reset Password");
                    });
            }, (environment) =>
            {
                //For wrapped credential providers, for each credential that is enumerated by the wrapped credential provider, it must be
                //wrapped in a credential. If the incoming credential has extended the functionality by using one of the extension interfaces,
                //such as 'IConnectableCredential', then the credential returned here must extend the correct base classes in order to maintain,
                //the wrapped functionality. This delegate is used to provide this.

                return new WrappedCredentialSample();
            })
        { }

        #region Variables



        #endregion

        #region Properties



        #endregion

        #region Methods

        public override void AddControls(CredentialControlCollection controls)
        {

        }

        #endregion
    }
}