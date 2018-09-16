using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JamieHighfield.CredentialProvider.Sample
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
            ControlBox = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            Close();
        }
    }
}
