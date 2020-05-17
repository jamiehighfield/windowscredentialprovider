using JamieHighfield.CredentialProvider.Credentials;
using JamieHighfield.CredentialProvider.Credentials.Interfaces;
using JamieHighfield.CredentialProvider.Sample.Forms;
using System.Windows.Forms;

namespace JamieHighfield.CredentialProvider.Sample.Credentials
{
    public class ConnectableWrappedCredentialSample : WrappedCredentialSample, IExtendedCredential, IConnectableCredential
    {
        public ConnectableWrappedCredentialSample()
        {
            ConnectionFactory = (environment, connection) =>
            {
                connection.UpdateStatus("Waiting for acceptance of the Acceptable Use Policy (AUP)...");

                //Thread.Sleep(1000);

                //MessageBox.Show((environment.MainWindowHandle == null).ToString());

                if ((new frmAcceptableUsePolicySample()).ShowDialog(environment.MainWindowHandle) != DialogResult.OK)
                {
                    connection.Cancel("You must agree to the Acceptable Use Policy (AUP) to login.", ResultMessageInformationIcons.Warning);
                }
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