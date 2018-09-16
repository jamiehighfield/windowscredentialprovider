using JamieHighfield.CredentialProvider.Registration;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace JamieHighfield.CredentialProvider.RegistrationUI
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        #region Variables



        #endregion

        #region Properties

        private AssemblyName AssemblyName { get; set; }

        private Assembly Assembly { get; set; }

        #endregion

        #region Methods

        public void LoadAssembly()
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();

                openFileDialog.Title = "Open";
                openFileDialog.Filter = "Assemblies (*.exe;*.dll)|*.exe;*.dll";
                openFileDialog.Multiselect = false;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    AssemblyName = AssemblyName.GetAssemblyName(openFileDialog.FileName);

                    LoadAssembly(Assembly);
                }
            }
            catch
            {
                MessageBox.Show("This assembly contains no compatible credential providers or is otherwise invalid.", "Invalid Assembly", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public void LoadAssembly(Assembly assembly)
        {
            try
            {
                lvwMain.Items.Clear();

                Assembly = Assembly.Load(AssemblyName);

                AssemblyCredentialProviderCollection assemblyCredentialProviders = AssemblyCredentialProviders.GetCredentialProviders(Assembly);

                if (assemblyCredentialProviders.Count == 0)
                {
                    MessageBox.Show("This assembly contains no compatible credential providers or is otherwise invalid.", "Invalid Assembly", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

                sbpStatus.Text = Assembly.FullName + " (" + AssemblyName.ProcessorArchitecture + ")";

                foreach (AssemblyCredentialProvider assemblyCredentialProvider in assemblyCredentialProviders)
                {
                    ListViewItem assemblyCredentialProviderListViewItem = new ListViewItem();

                    assemblyCredentialProviderListViewItem.Text = "{" + assemblyCredentialProvider.ComGuid.ToString() + "}";
                    assemblyCredentialProviderListViewItem.SubItems.Add(assemblyCredentialProvider.ComName);
                    assemblyCredentialProviderListViewItem.SubItems.Add(
                        ((assemblyCredentialProvider.ComRegistered == true && assemblyCredentialProvider.CredentialProviderRegistered == true ? "Registered" : "Not Registered")));

                    lvwMain.Items.Add(assemblyCredentialProviderListViewItem);
                }
            }
            catch
            {
                MessageBox.Show("This assembly contains no compatible credential providers or is otherwise invalid.", "Invalid Assembly", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        #region Event Handlers

        private void frmMain_Shown(object sender, EventArgs e)
        {
            LoadAssembly();
        }

        private void mimLoadAssembly_Click(object sender, EventArgs e)
        {
            LoadAssembly();
        }

        private void mimRegisterAssembly_Click(object sender, EventArgs e)
        {
            try
            {
                AssemblyCredentialProviders.RegisterCredentialProviders(Assembly);

                LoadAssembly(Assembly);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void mimUnregisterAssembly_Click(object sender, EventArgs e)
        {
            try
            {
                AssemblyCredentialProviders.UnregisterCredentialProviders(Assembly);

                LoadAssembly(Assembly);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void mimCredentialsDialog_Click(object sender, EventArgs e)
        {
            Utilities.ShowCredentialsDialog();
        }

        #endregion

        #endregion
    }
}