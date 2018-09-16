using JamieHighfield.CredentialProvider.Credentials;
using JamieHighfield.CredentialProvider.UI;
using System.Diagnostics;
using System.Threading;

namespace JamieHighfield.CredentialProvider.Sample
{
    public sealed class ConnectableCredentialSample : ConnectableCredential2Base
    {
        public ConnectableCredentialSample()
        {
            Connecting += (sender, eventArgs) =>
            {
                eventArgs.UpdateStatus("Logging into the System Security network...");

                Thread.Sleep(600);

                if (eventArgs.UserCancelled == true)
                {
                    return;
                }

                frmLogin frmLogin = new frmLogin();

                frmLogin.ShowDialog(MainWindowHandle);

                frmAcceptableUsePolicy f = new frmAcceptableUsePolicy();

                f.ShowDialog(MainWindowHandle);

                Thread.Sleep(600);
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