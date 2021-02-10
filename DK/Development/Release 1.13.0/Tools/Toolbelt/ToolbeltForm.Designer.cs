namespace Toolbelt
{
    partial class ToolbeltForm
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
            this.fetchAppPoolsBtn = new System.Windows.Forms.Button();
            this.AppPoolsOnServer = new System.Windows.Forms.CheckedListBox();
            this.restartAppPoolsBtn = new System.Windows.Forms.Button();
            this.serversGroupBox = new System.Windows.Forms.GroupBox();
            this.vDkCrm2Btn = new System.Windows.Forms.RadioButton();
            this.vDkCrmBtn = new System.Windows.Forms.RadioButton();
            this.vDkCrm2AccBtn = new System.Windows.Forms.RadioButton();
            this.vDkCrm1AccBtn = new System.Windows.Forms.RadioButton();
            this.vDkCrmUtvBtn = new System.Windows.Forms.RadioButton();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.appPoolsTab = new System.Windows.Forms.TabPage();
            this.appPoolsLogClearBtn = new System.Windows.Forms.Button();
            this.appPoolsLogTextBox = new System.Windows.Forms.TextBox();
            this.appPoolsLogTitle = new System.Windows.Forms.TextBox();
            this.sitesTab = new System.Windows.Forms.TabPage();
            this.sitesLogClearBtn = new System.Windows.Forms.Button();
            this.sitesLogTextBox = new System.Windows.Forms.TextBox();
            this.sitesLogTitle = new System.Windows.Forms.TextBox();
            this.restartSitesBtn = new System.Windows.Forms.Button();
            this.sitesList = new System.Windows.Forms.CheckedListBox();
            this.fetchSitesBtn = new System.Windows.Forms.Button();
            this.serversGroupBox2 = new System.Windows.Forms.GroupBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.radioButton5 = new System.Windows.Forms.RadioButton();
            this.applicationPoolBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.serversGroupBox.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.appPoolsTab.SuspendLayout();
            this.sitesTab.SuspendLayout();
            this.serversGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.applicationPoolBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // fetchAppPoolsBtn
            // 
            this.fetchAppPoolsBtn.Location = new System.Drawing.Point(6, 99);
            this.fetchAppPoolsBtn.Name = "fetchAppPoolsBtn";
            this.fetchAppPoolsBtn.Size = new System.Drawing.Size(102, 23);
            this.fetchAppPoolsBtn.TabIndex = 2;
            this.fetchAppPoolsBtn.Text = "Fetch App Pools";
            this.fetchAppPoolsBtn.UseVisualStyleBackColor = true;
            this.fetchAppPoolsBtn.Click += new System.EventHandler(this.fetchAppPoolsBtn_Click);
            // 
            // AppPoolsOnServer
            // 
            this.AppPoolsOnServer.CheckOnClick = true;
            this.AppPoolsOnServer.FormattingEnabled = true;
            this.AppPoolsOnServer.Location = new System.Drawing.Point(6, 144);
            this.AppPoolsOnServer.Name = "AppPoolsOnServer";
            this.AppPoolsOnServer.Size = new System.Drawing.Size(400, 259);
            this.AppPoolsOnServer.TabIndex = 3;
            // 
            // restartAppPoolsBtn
            // 
            this.restartAppPoolsBtn.Location = new System.Drawing.Point(301, 409);
            this.restartAppPoolsBtn.Name = "restartAppPoolsBtn";
            this.restartAppPoolsBtn.Size = new System.Drawing.Size(105, 23);
            this.restartAppPoolsBtn.TabIndex = 4;
            this.restartAppPoolsBtn.Text = "Restart app pools";
            this.restartAppPoolsBtn.UseVisualStyleBackColor = true;
            this.restartAppPoolsBtn.Click += new System.EventHandler(this.restartAppPoolsBtn_Click);
            // 
            // serversGroupBox
            // 
            this.serversGroupBox.Controls.Add(this.vDkCrm2Btn);
            this.serversGroupBox.Controls.Add(this.vDkCrmBtn);
            this.serversGroupBox.Controls.Add(this.vDkCrm2AccBtn);
            this.serversGroupBox.Controls.Add(this.vDkCrm1AccBtn);
            this.serversGroupBox.Controls.Add(this.vDkCrmUtvBtn);
            this.serversGroupBox.Location = new System.Drawing.Point(6, 7);
            this.serversGroupBox.Name = "serversGroupBox";
            this.serversGroupBox.Size = new System.Drawing.Size(400, 86);
            this.serversGroupBox.TabIndex = 5;
            this.serversGroupBox.TabStop = false;
            this.serversGroupBox.Text = "Servers";
            // 
            // vDkCrm2Btn
            // 
            this.vDkCrm2Btn.AutoSize = true;
            this.vDkCrm2Btn.Location = new System.Drawing.Point(217, 43);
            this.vDkCrm2Btn.Name = "vDkCrm2Btn";
            this.vDkCrm2Btn.Size = new System.Drawing.Size(80, 17);
            this.vDkCrm2Btn.TabIndex = 4;
            this.vDkCrm2Btn.TabStop = true;
            this.vDkCrm2Btn.Text = "V-DKCRM2";
            this.vDkCrm2Btn.UseVisualStyleBackColor = true;
            // 
            // vDkCrmBtn
            // 
            this.vDkCrmBtn.AutoSize = true;
            this.vDkCrmBtn.Location = new System.Drawing.Point(217, 19);
            this.vDkCrmBtn.Name = "vDkCrmBtn";
            this.vDkCrmBtn.Size = new System.Drawing.Size(74, 17);
            this.vDkCrmBtn.TabIndex = 3;
            this.vDkCrmBtn.TabStop = true;
            this.vDkCrmBtn.Text = "V-DKCRM";
            this.vDkCrmBtn.UseVisualStyleBackColor = true;
            // 
            // vDkCrm2AccBtn
            // 
            this.vDkCrm2AccBtn.AutoSize = true;
            this.vDkCrm2AccBtn.Location = new System.Drawing.Point(112, 43);
            this.vDkCrm2AccBtn.Name = "vDkCrm2AccBtn";
            this.vDkCrm2AccBtn.Size = new System.Drawing.Size(104, 17);
            this.vDkCrm2AccBtn.TabIndex = 2;
            this.vDkCrm2AccBtn.TabStop = true;
            this.vDkCrm2AccBtn.Text = "V-DKCRM2-ACC";
            this.vDkCrm2AccBtn.UseVisualStyleBackColor = true;
            // 
            // vDkCrm1AccBtn
            // 
            this.vDkCrm1AccBtn.AutoSize = true;
            this.vDkCrm1AccBtn.Location = new System.Drawing.Point(112, 19);
            this.vDkCrm1AccBtn.Name = "vDkCrm1AccBtn";
            this.vDkCrm1AccBtn.Size = new System.Drawing.Size(98, 17);
            this.vDkCrm1AccBtn.TabIndex = 1;
            this.vDkCrm1AccBtn.TabStop = true;
            this.vDkCrm1AccBtn.Text = "V-DKCRM-ACC";
            this.vDkCrm1AccBtn.UseVisualStyleBackColor = true;
            // 
            // vDkCrmUtvBtn
            // 
            this.vDkCrmUtvBtn.AutoSize = true;
            this.vDkCrmUtvBtn.Location = new System.Drawing.Point(7, 20);
            this.vDkCrmUtvBtn.Name = "vDkCrmUtvBtn";
            this.vDkCrmUtvBtn.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.vDkCrmUtvBtn.Size = new System.Drawing.Size(99, 17);
            this.vDkCrmUtvBtn.TabIndex = 0;
            this.vDkCrmUtvBtn.TabStop = true;
            this.vDkCrmUtvBtn.Text = "V-DKCRM-UTV";
            this.vDkCrmUtvBtn.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.appPoolsTab);
            this.tabControl1.Controls.Add(this.sitesTab);
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(432, 689);
            this.tabControl1.TabIndex = 6;
            // 
            // appPoolsTab
            // 
            this.appPoolsTab.Controls.Add(this.appPoolsLogClearBtn);
            this.appPoolsTab.Controls.Add(this.appPoolsLogTextBox);
            this.appPoolsTab.Controls.Add(this.appPoolsLogTitle);
            this.appPoolsTab.Controls.Add(this.serversGroupBox);
            this.appPoolsTab.Controls.Add(this.restartAppPoolsBtn);
            this.appPoolsTab.Controls.Add(this.fetchAppPoolsBtn);
            this.appPoolsTab.Controls.Add(this.AppPoolsOnServer);
            this.appPoolsTab.Location = new System.Drawing.Point(4, 22);
            this.appPoolsTab.Name = "appPoolsTab";
            this.appPoolsTab.Padding = new System.Windows.Forms.Padding(3);
            this.appPoolsTab.Size = new System.Drawing.Size(424, 663);
            this.appPoolsTab.TabIndex = 0;
            this.appPoolsTab.Text = "App Pools";
            this.appPoolsTab.UseVisualStyleBackColor = true;
            // 
            // appPoolsLogClearBtn
            // 
            this.appPoolsLogClearBtn.Location = new System.Drawing.Point(84, 454);
            this.appPoolsLogClearBtn.Name = "appPoolsLogClearBtn";
            this.appPoolsLogClearBtn.Size = new System.Drawing.Size(75, 23);
            this.appPoolsLogClearBtn.TabIndex = 8;
            this.appPoolsLogClearBtn.Text = "Clear log";
            this.appPoolsLogClearBtn.UseVisualStyleBackColor = true;
            this.appPoolsLogClearBtn.Click += new System.EventHandler(this.appPoolsLogClearBtn_Click);
            // 
            // appPoolsLogTextBox
            // 
            this.appPoolsLogTextBox.Location = new System.Drawing.Point(13, 484);
            this.appPoolsLogTextBox.Multiline = true;
            this.appPoolsLogTextBox.Name = "appPoolsLogTextBox";
            this.appPoolsLogTextBox.ReadOnly = true;
            this.appPoolsLogTextBox.Size = new System.Drawing.Size(400, 173);
            this.appPoolsLogTextBox.TabIndex = 7;
            // 
            // appPoolsLogTitle
            // 
            this.appPoolsLogTitle.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.appPoolsLogTitle.Location = new System.Drawing.Point(13, 459);
            this.appPoolsLogTitle.Name = "appPoolsLogTitle";
            this.appPoolsLogTitle.Size = new System.Drawing.Size(100, 13);
            this.appPoolsLogTitle.TabIndex = 6;
            this.appPoolsLogTitle.Text = "LOG";
            // 
            // sitesTab
            // 
            this.sitesTab.Controls.Add(this.sitesLogClearBtn);
            this.sitesTab.Controls.Add(this.sitesLogTextBox);
            this.sitesTab.Controls.Add(this.sitesLogTitle);
            this.sitesTab.Controls.Add(this.restartSitesBtn);
            this.sitesTab.Controls.Add(this.sitesList);
            this.sitesTab.Controls.Add(this.fetchSitesBtn);
            this.sitesTab.Controls.Add(this.serversGroupBox2);
            this.sitesTab.Location = new System.Drawing.Point(4, 22);
            this.sitesTab.Name = "sitesTab";
            this.sitesTab.Padding = new System.Windows.Forms.Padding(3);
            this.sitesTab.Size = new System.Drawing.Size(424, 663);
            this.sitesTab.TabIndex = 1;
            this.sitesTab.Text = "Sites";
            this.sitesTab.UseVisualStyleBackColor = true;
            // 
            // sitesLogClearBtn
            // 
            this.sitesLogClearBtn.Location = new System.Drawing.Point(112, 470);
            this.sitesLogClearBtn.Name = "sitesLogClearBtn";
            this.sitesLogClearBtn.Size = new System.Drawing.Size(75, 23);
            this.sitesLogClearBtn.TabIndex = 13;
            this.sitesLogClearBtn.Text = "Clear log";
            this.sitesLogClearBtn.UseVisualStyleBackColor = true;
            this.sitesLogClearBtn.Click += new System.EventHandler(this.sitesLogClearBtn_Click);
            // 
            // sitesLogTextBox
            // 
            this.sitesLogTextBox.Location = new System.Drawing.Point(6, 499);
            this.sitesLogTextBox.Multiline = true;
            this.sitesLogTextBox.Name = "sitesLogTextBox";
            this.sitesLogTextBox.ReadOnly = true;
            this.sitesLogTextBox.Size = new System.Drawing.Size(388, 158);
            this.sitesLogTextBox.TabIndex = 12;
            // 
            // sitesLogTitle
            // 
            this.sitesLogTitle.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.sitesLogTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sitesLogTitle.Location = new System.Drawing.Point(6, 472);
            this.sitesLogTitle.Name = "sitesLogTitle";
            this.sitesLogTitle.Size = new System.Drawing.Size(100, 19);
            this.sitesLogTitle.TabIndex = 11;
            this.sitesLogTitle.Text = "LOG";
            // 
            // restartSitesBtn
            // 
            this.restartSitesBtn.Location = new System.Drawing.Point(305, 410);
            this.restartSitesBtn.Name = "restartSitesBtn";
            this.restartSitesBtn.Size = new System.Drawing.Size(102, 23);
            this.restartSitesBtn.TabIndex = 9;
            this.restartSitesBtn.Text = "Restart sites";
            this.restartSitesBtn.UseVisualStyleBackColor = true;
            this.restartSitesBtn.Click += new System.EventHandler(this.restartSitesBtn_Click);
            // 
            // sitesList
            // 
            this.sitesList.CheckOnClick = true;
            this.sitesList.FormattingEnabled = true;
            this.sitesList.Location = new System.Drawing.Point(6, 132);
            this.sitesList.Name = "sitesList";
            this.sitesList.Size = new System.Drawing.Size(401, 274);
            this.sitesList.TabIndex = 8;
            // 
            // fetchSitesBtn
            // 
            this.fetchSitesBtn.Location = new System.Drawing.Point(6, 98);
            this.fetchSitesBtn.Name = "fetchSitesBtn";
            this.fetchSitesBtn.Size = new System.Drawing.Size(106, 28);
            this.fetchSitesBtn.TabIndex = 7;
            this.fetchSitesBtn.Text = "Fetch sites";
            this.fetchSitesBtn.UseVisualStyleBackColor = true;
            this.fetchSitesBtn.Click += new System.EventHandler(this.fetchSitesBtn_Click);
            // 
            // serversGroupBox2
            // 
            this.serversGroupBox2.Controls.Add(this.radioButton1);
            this.serversGroupBox2.Controls.Add(this.radioButton2);
            this.serversGroupBox2.Controls.Add(this.radioButton3);
            this.serversGroupBox2.Controls.Add(this.radioButton4);
            this.serversGroupBox2.Controls.Add(this.radioButton5);
            this.serversGroupBox2.Location = new System.Drawing.Point(6, 6);
            this.serversGroupBox2.Name = "serversGroupBox2";
            this.serversGroupBox2.Size = new System.Drawing.Size(401, 86);
            this.serversGroupBox2.TabIndex = 6;
            this.serversGroupBox2.TabStop = false;
            this.serversGroupBox2.Text = "Servers";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(217, 43);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(80, 17);
            this.radioButton1.TabIndex = 4;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "V-DKCRM2";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(217, 19);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(74, 17);
            this.radioButton2.TabIndex = 3;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "V-DKCRM";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(112, 43);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(104, 17);
            this.radioButton3.TabIndex = 2;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "V-DKCRM2-ACC";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Location = new System.Drawing.Point(112, 19);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(98, 17);
            this.radioButton4.TabIndex = 1;
            this.radioButton4.TabStop = true;
            this.radioButton4.Text = "V-DKCRM-ACC";
            this.radioButton4.UseVisualStyleBackColor = true;
            // 
            // radioButton5
            // 
            this.radioButton5.AutoSize = true;
            this.radioButton5.Location = new System.Drawing.Point(7, 20);
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.radioButton5.Size = new System.Drawing.Size(99, 17);
            this.radioButton5.TabIndex = 0;
            this.radioButton5.TabStop = true;
            this.radioButton5.Text = "V-DKCRM-UTV";
            this.radioButton5.UseVisualStyleBackColor = true;
            // 
            // applicationPoolBindingSource
            // 
            this.applicationPoolBindingSource.DataSource = typeof(Toolbelt.Classes.ApplicationPool);
            // 
            // ToolbeltForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(447, 704);
            this.Controls.Add(this.tabControl1);
            this.Name = "ToolbeltForm";
            this.Text = "Toolbelt";
            this.serversGroupBox.ResumeLayout(false);
            this.serversGroupBox.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.appPoolsTab.ResumeLayout(false);
            this.appPoolsTab.PerformLayout();
            this.sitesTab.ResumeLayout(false);
            this.sitesTab.PerformLayout();
            this.serversGroupBox2.ResumeLayout(false);
            this.serversGroupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.applicationPoolBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button fetchAppPoolsBtn;
        private System.Windows.Forms.CheckedListBox AppPoolsOnServer;
        private System.Windows.Forms.BindingSource applicationPoolBindingSource;
        private System.Windows.Forms.Button restartAppPoolsBtn;
        private System.Windows.Forms.GroupBox serversGroupBox;
        private System.Windows.Forms.RadioButton vDkCrm1AccBtn;
        private System.Windows.Forms.RadioButton vDkCrmUtvBtn;
        private System.Windows.Forms.RadioButton vDkCrm2Btn;
        private System.Windows.Forms.RadioButton vDkCrmBtn;
        private System.Windows.Forms.RadioButton vDkCrm2AccBtn;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage appPoolsTab;
        private System.Windows.Forms.TabPage sitesTab;
        private System.Windows.Forms.Button fetchSitesBtn;
        private System.Windows.Forms.GroupBox serversGroupBox2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.RadioButton radioButton5;
        private System.Windows.Forms.CheckedListBox sitesList;
        private System.Windows.Forms.Button restartSitesBtn;
        private System.Windows.Forms.TextBox sitesLogTextBox;
        private System.Windows.Forms.TextBox sitesLogTitle;
        private System.Windows.Forms.Button sitesLogClearBtn;
        private System.Windows.Forms.TextBox appPoolsLogTextBox;
        private System.Windows.Forms.TextBox appPoolsLogTitle;
        private System.Windows.Forms.Button appPoolsLogClearBtn;
    }
}

