using JamieHighfield.CredentialProvider.Credentials;
using JamieHighfield.CredentialProvider.Sample.Forms;

namespace JamieHighfield.CredentialProvider.Sample.Credentials
{
    public sealed class PreLogonAccessProviderCredentialSample : ConnectableCredentialBase
    {
        public PreLogonAccessProviderCredentialSample()
        {
            ConnectionFactory = (environment, connection) =>
            {

            };
        }

        public override void Loaded()
        {
            frmConnectVpn form = new frmConnectVpn();

            form.ShowDialog(MainWindowHandle);
        }
    }
}