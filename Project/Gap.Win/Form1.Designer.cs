namespace Gap.Win
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
            this.btnConnect = new System.Windows.Forms.Button();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.lbOnlineUsers = new System.Windows.Forms.ListBox();
            this.lblOnlineUsers = new System.Windows.Forms.Label();
            this.lbLogs = new System.Windows.Forms.ListBox();
            this.lblLog = new System.Windows.Forms.Label();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(197, 12);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(48, 14);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(143, 20);
            this.txtUsername.TabIndex = 0;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(12, 17);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(38, 13);
            this.lblName.TabIndex = 2;
            this.lblName.Text = "Name:";
            // 
            // lbOnlineUsers
            // 
            this.lbOnlineUsers.FormattingEnabled = true;
            this.lbOnlineUsers.Location = new System.Drawing.Point(15, 76);
            this.lbOnlineUsers.Name = "lbOnlineUsers";
            this.lbOnlineUsers.Size = new System.Drawing.Size(257, 95);
            this.lbOnlineUsers.TabIndex = 3;
            this.lbOnlineUsers.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lbOnlineUsers_MouseDoubleClick);
            // 
            // lblOnlineUsers
            // 
            this.lblOnlineUsers.AutoSize = true;
            this.lblOnlineUsers.Location = new System.Drawing.Point(12, 57);
            this.lblOnlineUsers.Name = "lblOnlineUsers";
            this.lblOnlineUsers.Size = new System.Drawing.Size(68, 13);
            this.lblOnlineUsers.TabIndex = 4;
            this.lblOnlineUsers.Text = "Online users:";
            // 
            // lbLogs
            // 
            this.lbLogs.FormattingEnabled = true;
            this.lbLogs.Location = new System.Drawing.Point(15, 233);
            this.lbLogs.Name = "lbLogs";
            this.lbLogs.Size = new System.Drawing.Size(257, 95);
            this.lbLogs.TabIndex = 5;
            // 
            // lblLog
            // 
            this.lblLog.AutoSize = true;
            this.lblLog.Location = new System.Drawing.Point(12, 213);
            this.lblLog.Name = "lblLog";
            this.lblLog.Size = new System.Drawing.Size(28, 13);
            this.lblLog.TabIndex = 6;
            this.lblLog.Text = "Log:";
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(197, 41);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(75, 23);
            this.btnDisconnect.TabIndex = 1;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 348);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.lblLog);
            this.Controls.Add(this.lbLogs);
            this.Controls.Add(this.lblOnlineUsers);
            this.Controls.Add(this.lbOnlineUsers);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.btnConnect);
            this.Name = "Form1";
            this.Text = "Gap Client";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.ListBox lbOnlineUsers;
        private System.Windows.Forms.Label lblOnlineUsers;
        private System.Windows.Forms.ListBox lbLogs;
        private System.Windows.Forms.Label lblLog;
        private System.Windows.Forms.Button btnDisconnect;

    }
}

