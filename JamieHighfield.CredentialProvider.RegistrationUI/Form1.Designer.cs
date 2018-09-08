namespace JamieHighfield.CredentialProvider.RegistrationUI
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.mmuMain = new System.Windows.Forms.MainMenu(this.components);
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.sbrMain = new System.Windows.Forms.StatusBar();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.lvwMain = new System.Windows.Forms.ListView();
            this.sbpStatus = new System.Windows.Forms.StatusBarPanel();
            this.pnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sbpStatus)).BeginInit();
            this.SuspendLayout();
            // 
            // mmuMain
            // 
            this.mmuMain.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1});
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem2});
            this.menuItem1.Text = "&Registration";
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 0;
            this.menuItem2.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
            this.menuItem2.Text = "&Load Assembly...";
            // 
            // sbrMain
            // 
            this.sbrMain.Location = new System.Drawing.Point(0, 664);
            this.sbrMain.Name = "sbrMain";
            this.sbrMain.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.sbpStatus});
            this.sbrMain.ShowPanels = true;
            this.sbrMain.Size = new System.Drawing.Size(1069, 36);
            this.sbrMain.SizingGrip = false;
            this.sbrMain.TabIndex = 0;
            this.sbrMain.Text = "statusBar1";
            // 
            // pnlMain
            // 
            this.pnlMain.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlMain.Controls.Add(this.lvwMain);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(1069, 664);
            this.pnlMain.TabIndex = 1;
            // 
            // lvwMain
            // 
            this.lvwMain.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvwMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvwMain.Location = new System.Drawing.Point(0, 0);
            this.lvwMain.Name = "lvwMain";
            this.lvwMain.Size = new System.Drawing.Size(1065, 660);
            this.lvwMain.TabIndex = 0;
            this.lvwMain.UseCompatibleStateImageBehavior = false;
            // 
            // sbpStatus
            // 
            this.sbpStatus.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
            this.sbpStatus.Name = "sbpStatus";
            this.sbpStatus.Text = "Done";
            this.sbpStatus.Width = 1069;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1069, 700);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.sbrMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Menu = this.mmuMain;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Credential Provider Registration";
            this.pnlMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sbpStatus)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MainMenu mmuMain;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.StatusBar sbrMain;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.ListView lvwMain;
        private System.Windows.Forms.StatusBarPanel sbpStatus;
    }
}

