using JamieHighfield.CredentialProvider.Controls;
using JamieHighfield.CredentialProvider.Credentials;
using JamieHighfield.CredentialProvider.Providers;
using System.Runtime.InteropServices;

namespace JamieHighfield.CredentialProvider.Sample
{
    [ComVisible(true)]
    [Guid("00016d50-0000-0000-b090-00006b0b0000")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("credsample1")]
    public sealed class CredentialProviderSample : CredentialProviderSetUserArrayBase
    {
        public CredentialProviderSample()
            : base(SystemCredentialProviders.Password, (usageScenario) =>
            {
                if (usageScenario == CredentialProviderUsageScenarios.CredentialsDialog)
                {
                    return new CredentialSample();
                }

                return new ConnectableCredentialSample();
            })
        { }

        #region Variables



        #endregion

        #region Properties



        #endregion

        #region Methods

        public override void AddCredentials(CredentialCollection credentials)
        {
            //credentials.Add(new CredentialSample());
        }

        public override void AddControls(CredentialControlCollection controls)
        {
            controls
                //.AddImage((usageScenario) =>
                //{
                //    if (usageScenario == CredentialProviderUsageScenarios.CredentialsDialog)
                //    {
                //        return CredentialFieldVisibilities.DeselectedCredential;
                //    }

                //    return CredentialFieldVisibilities.Both;
                //}, Properties.Resources.Novelllogo)
                //.AddLabel((usageScenario) =>
                //{
                //    if (usageScenario == CredentialProviderUsageScenarios.CredentialsDialog)
                //    {
                //        return CredentialFieldVisibilities.DeselectedCredential;
                //    }

                //    return CredentialFieldVisibilities.Both;
                //}, LabelControlSizes.Large, "Novell Login")
                //.AddTextBox(CredentialFieldVisibilities.SelectedCredential, "User name", false, (sender, eventArgs) =>
                //{
                //    //Domain stuff
                //})
                //.AddTextBox(CredentialFieldVisibilities.SelectedCredential, "Password", true)
                .AddLink("Reset Password", (sender, eventArgs) =>
                {
                    System.Windows.Forms.MessageBox.Show(MainWindowHandle, "abc");
                })
            //.AddButton((usageScenario) =>
            //{
            //    if (usageScenario == CredentialProviderUsageScenarios.CredentialsDialog)
            //    {
            //        return CredentialFieldVisibilities.Hidden;
            //    }

            //    return CredentialFieldVisibilities.SelectedCredential;
            //}, "Login", controls[3])
            ;
        }

        #endregion
    }
}