using JamieHighfield.CredentialProvider.Credentials;
using JamieHighfield.CredentialProvider.Controls;
using System;
using System.Drawing;
using System.Linq;

namespace JamieHighfield.CredentialProvider.Sample
{
    public sealed class CredentialSample : CredentialBase
    {
        public CredentialSample()
            : base(new ImageControl(Properties.Resources.Novelllogo))
        {
            Load += (sender, eventArgs) =>
            {

            };

            Controls
                .Add(new LabelControl("Reset password", LabelControlSizes.Large, CredentialFieldVisibilities.DeselectedCredential))
                .Add(new TextBoxControl("User name", false, (sender, eventArgs) =>
                    {
                        if (Controls.Count > 2)
                        {
                            ((LabelControl)Controls[3]).Text = "Domain: " + GetDomain(eventArgs.Control.Text);
                        }
                    })
                {
                    Focussed = true
                })
                .Add(new TextBoxControl("Password", true, (sender, eventArgs) =>
                {
                    if (eventArgs.Control.Text == "abc")
                    {
                        Form1 f = new Form1();
                        f.ShowDialog(WindowHandle);
                    }
                }))
                .Add(new LabelControl("Domain: " + GetDomain(string.Empty), LabelControlSizes.Small))
                .Add(new CheckBoxControl("Change password on login", false));
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