using System;
using System.Windows.Forms;

namespace JamieHighfield.CredentialProvider.Sample.Forms
{
    public partial class frmConnectVpn : Form
    {
        public frmConnectVpn()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}