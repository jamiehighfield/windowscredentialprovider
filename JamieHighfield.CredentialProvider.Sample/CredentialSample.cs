using JamieHighfield.CredentialProvider.Credentials;
using JamieHighfield.CredentialProvider.Fields;
using System;
using System.Drawing;
using System.Linq;

namespace JamieHighfield.CredentialProvider.Sample
{
    public sealed class CredentialSample : CredentialBase
    {
        public CredentialSample()
            : base((fields) =>
            {
                fields
                    .Add(
                        new TextField("Reset password", TextFieldSizes.Large, CredentialFieldStates.DeselectedCredential));
            }, new CredentialImage(Properties.Resources.Novelllogo))
        {
            Fields
                .Add(
                    new TextBoxField("User name", false, (sender, eventArgs) =>
                    {
                        if (Fields.Count > 2)
                        {
                            ((TextField)Fields[3]).Text = "Domain: " + GetDomain(eventArgs.TextBoxField.Text);
                        }
                    }))
                .Add(
                    new TextBoxField("Password", true))
                .Add(
                    new TextField("Domain: " + GetDomain(string.Empty), TextFieldSizes.Small));
        }

        #region Variables



        #endregion

        #region Properties



        #endregion

        #region Methods

        private string GetDomain(string username)
        {
            if (username == null)
            {
                throw new ArgumentNullException(nameof(username));
            }

            string domain = string.Empty;

            if (username.Count((character) => character == '\\') == 0)
            {
                if (username.Count((character) => character == '@') == 0)
                {
                    domain = Environment.MachineName;
                }
                else
                {
                    domain = username.Split('@')[1];
                }
            }
            else
            {
                domain = username.Split('\\')[0];

                if (domain == ".")
                {
                    domain = Environment.MachineName;
                }
            }

            return domain;
        }

        #endregion
    }
}