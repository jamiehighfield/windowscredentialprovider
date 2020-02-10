using JamieHighfield.CredentialProvider.Controls;
using JamieHighfield.CredentialProvider.Controls.Descriptors;
using JamieHighfield.CredentialProvider.Controls.New;
using JamieHighfield.CredentialProvider.Logging;
using JamieHighfield.CredentialProvider.Providers;
using JamieHighfield.CredentialProvider.Sample.Credentials;
using JamieHighfield.CredentialProvider.Sample.Properties;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace JamieHighfield.CredentialProvider.Sample.Providers
{
    //[ComVisible(true)]
    //[Guid("B9F4CFA8-9A49-4060-AC75-08DD99C4AC6E")]
    //[ClassInterface(ClassInterfaceType.None)]
    //[ProgId("LocalWindowsAuthenticationCredentialProviderMultipleSample")]
    public sealed class LocalWindowsAuthenticationCredentialProviderMultipleSample : CredentialProviderBase<LocalWindowsAuthenticationCredentialMultipleSample>
    {
        public LocalWindowsAuthenticationCredentialProviderMultipleSample()
            : base(CredentialProviderUsageScenarios.All)
        {
            CredentialsFactory = (credentials) =>
            {
                //Enumerate the appropriate credentials - this is hardcoded, but you could easily perform get which credentials
                //to enumerate from a configuration file or other data source. You could also query Windows for a list of users
                //and enumerate credentials based on that. While the controls available are applied at the credential provider
                //level, the actual contents of the controls can differ between credentials and can be modified in the
                //'CredentialBase.Initiated' method.

                credentials
                    .Add(new LocalWindowsAuthenticationCredentialMultipleSample(Resources.PlaceholderImage1, Environment.MachineName + @"\jsmith"))
                    .Add(new LocalWindowsAuthenticationCredentialMultipleSample(Resources.PlaceholderImage2, Environment.MachineName + @"\jdoe"));
            };

            DescriptorsFactory = (environment, descriptors) =>
            {
                //Add all the control descriptors. While the controls available are applied at the credential provider level, the
                //actual contents of the controls can differ between credentials and can be modified in the 'CredentialBase.Initiated' method.

                descriptors
                    .AddImage((options) =>
                    {
                        //We want different behaviour here. If the usage scenario is the credentials dialog, we only want to
                        //display the credential image on the deselected credential - matching the default behaviour of Windows.
                        //However, only if the operating system is at least Windows 8/Windows Server 2012.

                        OperatingSystem currentOperatingSystem = environment.GetCurrentOperatingSystem();

                        if (environment.CurrentUsageScenario == CredentialProviderUsageScenarios.CredentialsDialog
                            && (currentOperatingSystem.Version.Major >= 6 && currentOperatingSystem.Version.Minor >= 2)) //Windows 8/Windows Server 2012
                        {
                            options.Visibility = CredentialFieldVisibilities.DeselectedCredential;
                        }
                        else
                        {
                            options.Visibility = CredentialFieldVisibilities.Both;
                        }

                        options.Image = (credential) =>
                        {
                            if (((LocalWindowsAuthenticationCredentialMultipleSample)credential).Username == Environment.MachineName + @"\jsmith")
                            {
                                return Resources.PlaceholderImage1;
                            }
                            else
                            {
                                return Resources.PlaceholderImage2;
                            }
                        };
                    })
                    .AddLabel((options) =>
                    {
                        if (environment.CurrentUsageScenario == CredentialProviderUsageScenarios.CredentialsDialog)
                        {
                            options.Visibility = CredentialFieldVisibilities.DeselectedCredential;
                        }
                        else
                        {
                            options.Visibility = CredentialFieldVisibilities.Both;
                        }

                        options.Text = (credential) => ((LocalWindowsAuthenticationCredentialMultipleSample)credential).Username;
                    })
                    .AddTextBox((options) =>
                    {
                        //The values should be the same between all enumerated credentials.

                        options.Visibility = CredentialFieldVisibilities.SelectedCredential;

                        options.Label = "User name";

                        options.TextChanged = (credential) => (sender, eventArgs) =>
                        {
                            credential.Controls.FirstOfControlType<NewLabelControl>(1).Text = eventArgs.Control.Text;
                        };
                    })
                    .AddPasswordTextBox((options) =>
                    {
                        //The values should be the same between all enumerated credentials.

                        options.Visibility = CredentialFieldVisibilities.SelectedCredential;

                        options.Label = "Password";
                    })
                    .AddLabel((options) =>
                    {
                        options.Visibility = CredentialFieldVisibilities.SelectedCredential;

                        options.Text = (credential) => "Domain: ";
                    })
                    .AddLink((options) =>
                    {
                        options.Visibility = CredentialFieldVisibilities.SelectedCredential;

                        options.Text = (credential) => "Reset Password";

                        options.Click = (credential) => (sender, eventHandler) =>
                        {
                            MessageBox.Show(environment.MainWindowHandle, "Reset Password");
                            credential.Controls.FirstOfControlType<NewTextBoxControl>().Text = "abc";
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