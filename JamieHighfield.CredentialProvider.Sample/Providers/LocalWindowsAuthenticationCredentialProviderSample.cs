using JamieHighfield.CredentialProvider.Controls;
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
    //[Guid("213631ED-C1E7-4CF3-B863-BC0B4EC34D1B")]
    //[ClassInterface(ClassInterfaceType.None)]
    //[ProgId("LocalWindowsAuthenticationCredentialProviderSample")]
    public sealed class LocalWindowsAuthenticationCredentialProviderSample : CredentialProviderBase
    {
        public LocalWindowsAuthenticationCredentialProviderSample()
            : base(CredentialProviderUsageScenarios.All, (credentials) =>
            {
                credentials
                    .Add(new LocalWindowsAuthenticationCredentialSample());
            }, (environment, controls) =>
            {
                //Add controls as appropriate that this credential provider will expose.

                controls
                    .AddImage(() =>
                    {
                        //We want different behaviour here. If the usage scenario is the credentials dialog, we only want to
                        //display the credential image on the deselected credential - matching the default behaviour of Windows.
                        //However, only if the operating system is at least Windows 8/Windows Server 2012.

                        OperatingSystem currentOperatingSystem = environment.GetCurrentOperatingSystem();

                        if (environment.CurrentUsageScenario == CredentialProviderUsageScenarios.CredentialsDialog
                            && (currentOperatingSystem.Version.Major >= 6 && currentOperatingSystem.Version.Minor >= 2))
                        {
                            return CredentialFieldVisibilities.DeselectedCredential;
                        }

                        return CredentialFieldVisibilities.Both;
                    }, Resources.PlaceholderImage1)
                    .AddLabel(() =>
                    {
                        //We want different behaviour here. If the usage scenario is the credentials dialog, we only want to
                        //display the credential title on the deselected credential - matching the default behaviour of Windows.

                        if (environment.CurrentUsageScenario == CredentialProviderUsageScenarios.CredentialsDialog)
                        {
                            return CredentialFieldVisibilities.DeselectedCredential;
                        }

                        return CredentialFieldVisibilities.Both;
                    }, LabelControlSizes.Large, "Local Windows Authentication")
                    .AddTextBox(CredentialFieldVisibilities.SelectedCredential, "User name", false)
                    .AddTextBox(CredentialFieldVisibilities.SelectedCredential, "Password", true)
                    .AddLink(CredentialFieldVisibilities.SelectedCredential, "Reset Password", (sender, eventArgs) =>
                    {
                        //Use the main window handle parsed in from the current environment.

                        //Use 'eventArgs.Control' here to access the control or use 'eventArgs.Credential' to access the credential
                        //currently selected. The control created here will differ from the control created in the credential and
                        //changes here may not necessarily take effect.

                        if (string.IsNullOrEmpty(eventArgs.Credential.Controls.FirstOfControlType<TextBoxControl>().Text))
                        {
                            MessageBox.Show("Please enter a username to reset your password.");
                        }
                        else
                        {
                            MessageBox.Show(environment.MainWindowHandle, "Reset Password for user '" + eventArgs.Credential.Controls.FirstOfControlType<TextBoxControl>().Text + "'");
                        }
                    });
                    //.AddButton("Login", controls.FirstOfControlType<TextBoxControl>(1));
            })
        {
            //Enable logging for diagnostic purposes.

            GlobalLogger.Enabled = true;
        }

        #region Variables



        #endregion

        #region Properties



        #endregion

        #region Methods



        #endregion
    }
}