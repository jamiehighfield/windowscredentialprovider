using JamieHighfield.CredentialProvider.Controls;
using JamieHighfield.CredentialProvider.Credentials;
using System;
using System.Drawing;

namespace JamieHighfield.CredentialProvider.Sample.Credentials
{
    public sealed class LocalWindowsAuthenticationCredentialMultipleSample : ExtendedCredentialBase
    {
        public LocalWindowsAuthenticationCredentialMultipleSample(Bitmap image, string username)
            : base(Logon.LogonSequencePipeline.CreatePipeline()
                  .AddAuthentication(new CredentialProviderAuthenticationSample()))
        {
            //We have to store the parsed argument for the image and username in a holding variable because the controls haven't been
            //enumerated in the credential at this point. Instead, we use the 'Initialise' override method.

            Image = image;
            Username = username;

            //Use the logon sequence pipeline parsed above to parse user credentials to Windows.

            WindowsLogon = true;
        }

        #region Variables



        #endregion

        #region Properties

        private Bitmap Image { get; set; }

        public string Username { get; private set; }

        #endregion

        #region Methods

        public override void Initialise()
        {
            Console.WriteLine(Username);
            //Controls.FirstOfControlType<ImageControl>().Image = Image;
            //Controls.FirstOfControlType<LabelControl>().Text = Username;
        }

        #endregion
    }
}