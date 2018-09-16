/* COPYRIGHT NOTICE
 * 
 * Copyright © Jamie Highfield 2018. All rights reserved.
 * 
 * This library is protected by UK, EU & international copyright laws and treaties. Unauthorised
 * reproduction of this library outside of the constraints of the accompanied license, or any
 * portion of it, may result in severe criminal penalties that will be prosecuted to the
 * maximum extent possible under the law.
 * 
 */

namespace JamieHighfield.CredentialProvider.RegistrationUI
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.mmuMain = new System.Windows.Forms.MainMenu(this.components);
            this.mimRegistration = new System.Windows.Forms.MenuItem();
            this.mimLoadAssembly = new System.Windows.Forms.MenuItem();
            this.mimSeparator1 = new System.Windows.Forms.MenuItem();
            this.mimRegisterAssembly = new System.Windows.Forms.MenuItem();
            this.mimUnregisterAssembly = new System.Windows.Forms.MenuItem();
            this.sbrMain = new System.Windows.Forms.StatusBar();
            this.sbpStatus = new System.Windows.Forms.StatusBarPanel();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.lvwMain = new System.Windows.Forms.ListView();
            this.chrComGuid = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chrComName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chrComRegistered = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.mimTools = new System.Windows.Forms.MenuItem();
            this.mimCredentialsDialog = new System.Windows.Forms.MenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.sbpStatus)).BeginInit();
            this.pnlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // mmuMain
            // 
            this.mmuMain.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mimRegistration,
            this.mimTools});
            // 
            // mimRegistration
            // 
            this.mimRegistration.Index = 0;
            this.mimRegistration.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mimLoadAssembly,
            this.mimSeparator1,
            this.mimRegisterAssembly,
            this.mimUnregisterAssembly});
            this.mimRegistration.Text = "&Registration";
            // 
            // mimLoadAssembly
            // 
            this.mimLoadAssembly.Index = 0;
            this.mimLoadAssembly.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
            this.mimLoadAssembly.Text = "&Load Assembly...";
            this.mimLoadAssembly.Click += new System.EventHandler(this.mimLoadAssembly_Click);
            // 
            // mimSeparator1
            // 
            this.mimSeparator1.Index = 1;
            this.mimSeparator1.Text = "-";
            // 
            // mimRegisterAssembly
            // 
            this.mimRegisterAssembly.Index = 2;
            this.mimRegisterAssembly.Text = "&Register Assembly";
            this.mimRegisterAssembly.Click += new System.EventHandler(this.mimRegisterAssembly_Click);
            // 
            // mimUnregisterAssembly
            // 
            this.mimUnregisterAssembly.Index = 3;
            this.mimUnregisterAssembly.Text = "&Unregister Assembly";
            this.mimUnregisterAssembly.Click += new System.EventHandler(this.mimUnregisterAssembly_Click);
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
            // sbpStatus
            // 
            this.sbpStatus.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
            this.sbpStatus.Name = "sbpStatus";
            this.sbpStatus.Text = "Done";
            this.sbpStatus.Width = 1069;
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
            this.lvwMain.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chrComGuid,
            this.chrComName,
            this.chrComRegistered});
            this.lvwMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvwMain.FullRowSelect = true;
            this.lvwMain.HideSelection = false;
            this.lvwMain.Location = new System.Drawing.Point(0, 0);
            this.lvwMain.MultiSelect = false;
            this.lvwMain.Name = "lvwMain";
            this.lvwMain.Size = new System.Drawing.Size(1065, 660);
            this.lvwMain.TabIndex = 0;
            this.lvwMain.UseCompatibleStateImageBehavior = false;
            this.lvwMain.View = System.Windows.Forms.View.Details;
            // 
            // chrComGuid
            // 
            this.chrComGuid.Text = "COM GUID";
            // 
            // chrComName
            // 
            this.chrComName.Text = "COM Name";
            // 
            // chrComRegistered
            // 
            this.chrComRegistered.Text = "Registered";
            // 
            // mimTools
            // 
            this.mimTools.Index = 1;
            this.mimTools.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mimCredentialsDialog});
            this.mimTools.Text = "&Tools";
            // 
            // mimCredentialsDialog
            // 
            this.mimCredentialsDialog.Index = 0;
            this.mimCredentialsDialog.Text = "&Credentials Dialog...";
            this.mimCredentialsDialog.Click += new System.EventHandler(this.mimCredentialsDialog_Click);
            // 
            // frmMain
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
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Credential Provider Registration";
            this.Shown += new System.EventHandler(this.frmMain_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.sbpStatus)).EndInit();
            this.pnlMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MainMenu mmuMain;
        private System.Windows.Forms.MenuItem mimRegistration;
        private System.Windows.Forms.MenuItem mimLoadAssembly;
        private System.Windows.Forms.StatusBar sbrMain;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.ListView lvwMain;
        private System.Windows.Forms.StatusBarPanel sbpStatus;
        private System.Windows.Forms.ColumnHeader chrComGuid;
        private System.Windows.Forms.ColumnHeader chrComName;
        private System.Windows.Forms.ColumnHeader chrComRegistered;
        private System.Windows.Forms.MenuItem mimSeparator1;
        private System.Windows.Forms.MenuItem mimRegisterAssembly;
        private System.Windows.Forms.MenuItem mimUnregisterAssembly;
        private System.Windows.Forms.MenuItem mimTools;
        private System.Windows.Forms.MenuItem mimCredentialsDialog;
    }
}

