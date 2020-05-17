using JamieHighfield.CredentialProvider.Credentials;
using JamieHighfield.CredentialProvider.Logging;
using JamieHighfield.CredentialProvider.Sample.Forms;
using System.Threading;
using System.Windows.Forms;

namespace JamieHighfield.CredentialProvider.Sample.Credentials
{
    public sealed class PreLogonAccessProviderCredentialSample : ConnectableCredentialBase
    {
        public PreLogonAccessProviderCredentialSample()
        {
            ConnectionFactory = (environment, connection) =>
            {
                //frmConnectVpn form = new frmConnectVpn();

                //if (form.ShowDialog(MainWindowHandle) == DialogResult.OK)
                //{
                //    Thread.Sleep(100);

                //    if ((new frmConnecting()).ShowDialog(MainWindowHandle) == DialogResult.OK)
                //    {

                //    }
                //}
            };
        }

        private bool t = false;

        public override void Initialise()
        {
            GlobalLogger.Log(LogLevels.Information, "Reloading...");

            if (t == false)
            {
                t = true;

                //ManagedCredentialProvider.InvokeSubmit();
            }

            return;
        }

        public override void Loaded()
        {
            //ManagedCredentialProvider.InvokeSubmit();
        }
    }
}