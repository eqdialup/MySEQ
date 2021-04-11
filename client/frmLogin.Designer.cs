namespace myseq
{
    partial class FrmLogin
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
            this.lblSMTPUsername = new System.Windows.Forms.Label();
            this.lblSMTPPassword = new System.Windows.Forms.Label();
            this.textSMTPUsername = new System.Windows.Forms.TextBox();
            this.textSMTPPassword = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBoxSavePassword = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lblSMTPDomain = new System.Windows.Forms.Label();
            this.textBoxSMTPDomain = new System.Windows.Forms.TextBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.SMTPUseNetworkCreds = new System.Windows.Forms.CheckBox();
            this.SMTPUseSecureAuthn = new System.Windows.Forms.CheckBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.lblSMTPPort = new System.Windows.Forms.Label();
            this.textSMTPPort = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblSMTPAddress = new System.Windows.Forms.Label();
            this.textSMTPAddress = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblCCEmail = new System.Windows.Forms.Label();
            this.textSMTPCCEmail = new System.Windows.Forms.TextBox();
            this.btnTestEmail = new System.Windows.Forms.Button();
            this.textSMTPToEmail = new System.Windows.Forms.TextBox();
            this.lblToAddress = new System.Windows.Forms.Label();
            this.textSMTPFromEmail = new System.Windows.Forms.TextBox();
            this.lblFromAddress = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblSMTPUsername
            // 
            this.lblSMTPUsername.AutoSize = true;
            this.lblSMTPUsername.Location = new System.Drawing.Point(9, 22);
            this.lblSMTPUsername.Name = "lblSMTPUsername";
            this.lblSMTPUsername.Size = new System.Drawing.Size(58, 13);
            this.lblSMTPUsername.TabIndex = 5;
            this.lblSMTPUsername.Text = "Username:";
            // 
            // lblSMTPPassword
            // 
            this.lblSMTPPassword.AutoSize = true;
            this.lblSMTPPassword.Location = new System.Drawing.Point(5, 45);
            this.lblSMTPPassword.Name = "lblSMTPPassword";
            this.lblSMTPPassword.Size = new System.Drawing.Size(56, 13);
            this.lblSMTPPassword.TabIndex = 6;
            this.lblSMTPPassword.Text = "Password:";
            // 
            // textSMTPUsername
            // 
            this.textSMTPUsername.Location = new System.Drawing.Point(67, 19);
            this.textSMTPUsername.Name = "textSMTPUsername";
            this.textSMTPUsername.Size = new System.Drawing.Size(163, 20);
            this.textSMTPUsername.TabIndex = 1;
            // 
            // textSMTPPassword
            // 
            this.textSMTPPassword.Location = new System.Drawing.Point(67, 42);
            this.textSMTPPassword.Name = "textSMTPPassword";
            this.textSMTPPassword.PasswordChar = '*';
            this.textSMTPPassword.Size = new System.Drawing.Size(163, 20);
            this.textSMTPPassword.TabIndex = 2;
            this.textSMTPPassword.UseSystemPasswordChar = true;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(165, 438);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "&Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.BtnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(77, 438);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBoxSavePassword);
            this.groupBox1.Controls.Add(this.textSMTPPassword);
            this.groupBox1.Controls.Add(this.textSMTPUsername);
            this.groupBox1.Controls.Add(this.lblSMTPPassword);
            this.groupBox1.Controls.Add(this.lblSMTPUsername);
            this.groupBox1.Location = new System.Drawing.Point(8, 333);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(238, 95);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "SMTP Login";
            // 
            // checkBoxSavePassword
            // 
            this.checkBoxSavePassword.AutoSize = true;
            this.checkBoxSavePassword.Location = new System.Drawing.Point(67, 68);
            this.checkBoxSavePassword.Name = "checkBoxSavePassword";
            this.checkBoxSavePassword.Size = new System.Drawing.Size(100, 17);
            this.checkBoxSavePassword.TabIndex = 3;
            this.checkBoxSavePassword.Text = "Save Password";
            this.checkBoxSavePassword.UseVisualStyleBackColor = true;
            this.checkBoxSavePassword.Click += new System.EventHandler(this.CheckBoxSavePassword_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lblSMTPDomain);
            this.groupBox3.Controls.Add(this.textBoxSMTPDomain);
            this.groupBox3.Controls.Add(this.checkBox1);
            this.groupBox3.Controls.Add(this.SMTPUseNetworkCreds);
            this.groupBox3.Controls.Add(this.SMTPUseSecureAuthn);
            this.groupBox3.Controls.Add(this.textBox1);
            this.groupBox3.Controls.Add(this.lblSMTPPort);
            this.groupBox3.Controls.Add(this.textSMTPPort);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.lblSMTPAddress);
            this.groupBox3.Controls.Add(this.textSMTPAddress);
            this.groupBox3.Controls.Add(this.textBox2);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Location = new System.Drawing.Point(8, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(239, 213);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "SMTP Setup";
            // 
            // lblSMTPDomain
            // 
            this.lblSMTPDomain.AutoSize = true;
            this.lblSMTPDomain.Location = new System.Drawing.Point(5, 59);
            this.lblSMTPDomain.Name = "lblSMTPDomain";
            this.lblSMTPDomain.Size = new System.Drawing.Size(89, 13);
            this.lblSMTPDomain.TabIndex = 11;
            this.lblSMTPDomain.Text = "Domain (optional)";
            // 
            // textBoxSMTPDomain
            // 
            this.textBoxSMTPDomain.Location = new System.Drawing.Point(100, 56);
            this.textBoxSMTPDomain.Name = "textBoxSMTPDomain";
            this.textBoxSMTPDomain.Size = new System.Drawing.Size(133, 20);
            this.textBoxSMTPDomain.TabIndex = 2;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(85, 183);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(100, 17);
            this.checkBox1.TabIndex = 7;
            this.checkBox1.Text = "Save Password";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.Click += new System.EventHandler(this.CheckBoxSavePassword_Click);
            // 
            // checkBoxSMTPUseNetworkCredentials
            // 
            this.SMTPUseNetworkCreds.AutoSize = true;
            this.SMTPUseNetworkCreds.Location = new System.Drawing.Point(8, 105);
            this.SMTPUseNetworkCreds.Name = "checkBoxSMTPUseNetworkCredentials";
            this.SMTPUseNetworkCreds.Size = new System.Drawing.Size(143, 17);
            this.SMTPUseNetworkCreds.TabIndex = 4;
            this.SMTPUseNetworkCreds.Text = "Use Network Credentials";
            this.SMTPUseNetworkCreds.UseVisualStyleBackColor = true;
            this.SMTPUseNetworkCreds.Click += new System.EventHandler(this.CheckBoxSMTPUseNetworkCredentials_Click);
            // 
            // checkBoxSMTPUseSecureAuthentication
            // 
            this.SMTPUseSecureAuthn.AutoSize = true;
            this.SMTPUseSecureAuthn.Location = new System.Drawing.Point(8, 82);
            this.SMTPUseSecureAuthn.Name = "checkBoxSMTPUseSecureAuthentication";
            this.SMTPUseSecureAuthn.Size = new System.Drawing.Size(153, 17);
            this.SMTPUseSecureAuthn.TabIndex = 3;
            this.SMTPUseSecureAuthn.Text = "Use Secure Authentication";
            this.SMTPUseSecureAuthn.UseVisualStyleBackColor = true;
            this.SMTPUseSecureAuthn.Click += new System.EventHandler(this.CheckBoxSMTPUseSecureAuthentication_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(85, 157);
            this.textBox1.Name = "textBox1";
            this.textBox1.PasswordChar = '*';
            this.textBox1.Size = new System.Drawing.Size(145, 20);
            this.textBox1.TabIndex = 6;
            this.textBox1.UseSystemPasswordChar = true;
            // 
            // lblSMTPPort
            // 
            this.lblSMTPPort.AutoSize = true;
            this.lblSMTPPort.Location = new System.Drawing.Point(193, 16);
            this.lblSMTPPort.Name = "lblSMTPPort";
            this.lblSMTPPort.Size = new System.Drawing.Size(29, 13);
            this.lblSMTPPort.TabIndex = 8;
            this.lblSMTPPort.Text = "Port:";
            // 
            // textSMTPPort
            // 
            this.textSMTPPort.Location = new System.Drawing.Point(191, 32);
            this.textSMTPPort.Name = "textSMTPPort";
            this.textSMTPPort.Size = new System.Drawing.Size(42, 20);
            this.textSMTPPort.TabIndex = 1;
            this.textSMTPPort.Text = "25";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 131);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Username:";
            // 
            // lblSMTPAddress
            // 
            this.lblSMTPAddress.AutoSize = true;
            this.lblSMTPAddress.Location = new System.Drawing.Point(5, 16);
            this.lblSMTPAddress.Name = "lblSMTPAddress";
            this.lblSMTPAddress.Size = new System.Drawing.Size(115, 13);
            this.lblSMTPAddress.TabIndex = 5;
            this.lblSMTPAddress.Text = "SMTP Server Address:";
            // 
            // textSMTPAddress
            // 
            this.textSMTPAddress.Location = new System.Drawing.Point(8, 32);
            this.textSMTPAddress.Name = "textSMTPAddress";
            this.textSMTPAddress.Size = new System.Drawing.Size(177, 20);
            this.textSMTPAddress.TabIndex = 0;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(85, 128);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(145, 20);
            this.textBox2.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 160);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Password:";
            // 
            // lblCCEmail
            // 
            this.lblCCEmail.AutoSize = true;
            this.lblCCEmail.Location = new System.Drawing.Point(14, 281);
            this.lblCCEmail.Name = "lblCCEmail";
            this.lblCCEmail.Size = new System.Drawing.Size(24, 13);
            this.lblCCEmail.TabIndex = 19;
            this.lblCCEmail.Text = "CC:";
            // 
            // textSMTPCCEmail
            // 
            this.textSMTPCCEmail.Location = new System.Drawing.Point(44, 278);
            this.textSMTPCCEmail.Name = "textSMTPCCEmail";
            this.textSMTPCCEmail.Size = new System.Drawing.Size(194, 20);
            this.textSMTPCCEmail.TabIndex = 17;
            // 
            // btnTestEmail
            // 
            this.btnTestEmail.Location = new System.Drawing.Point(44, 304);
            this.btnTestEmail.Name = "btnTestEmail";
            this.btnTestEmail.Size = new System.Drawing.Size(154, 23);
            this.btnTestEmail.TabIndex = 18;
            this.btnTestEmail.Text = "Send Test Email";
            this.btnTestEmail.UseVisualStyleBackColor = true;
            this.btnTestEmail.Click += new System.EventHandler(this.BtnTestEmail_Click);
            // 
            // textSMTPToEmail
            // 
            this.textSMTPToEmail.Location = new System.Drawing.Point(44, 252);
            this.textSMTPToEmail.Name = "textSMTPToEmail";
            this.textSMTPToEmail.Size = new System.Drawing.Size(194, 20);
            this.textSMTPToEmail.TabIndex = 16;
            // 
            // lblToAddress
            // 
            this.lblToAddress.AutoSize = true;
            this.lblToAddress.Location = new System.Drawing.Point(15, 255);
            this.lblToAddress.Name = "lblToAddress";
            this.lblToAddress.Size = new System.Drawing.Size(23, 13);
            this.lblToAddress.TabIndex = 14;
            this.lblToAddress.Text = "To:";
            // 
            // textSMTPFromEmail
            // 
            this.textSMTPFromEmail.Location = new System.Drawing.Point(44, 225);
            this.textSMTPFromEmail.Name = "textSMTPFromEmail";
            this.textSMTPFromEmail.Size = new System.Drawing.Size(194, 20);
            this.textSMTPFromEmail.TabIndex = 15;
            // 
            // lblFromAddress
            // 
            this.lblFromAddress.AutoSize = true;
            this.lblFromAddress.Location = new System.Drawing.Point(5, 228);
            this.lblFromAddress.Name = "lblFromAddress";
            this.lblFromAddress.Size = new System.Drawing.Size(33, 13);
            this.lblFromAddress.TabIndex = 13;
            this.lblFromAddress.Text = "From:";
            // 
            // FrmLogin
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(248, 468);
            this.ControlBox = false;
            this.Controls.Add(this.lblCCEmail);
            this.Controls.Add(this.textSMTPCCEmail);
            this.Controls.Add(this.btnTestEmail);
            this.Controls.Add(this.textSMTPToEmail);
            this.Controls.Add(this.lblToAddress);
            this.Controls.Add(this.textSMTPFromEmail);
            this.Controls.Add(this.lblFromAddress);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SMTP Email Settings";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSMTPUsername;
        private System.Windows.Forms.Label lblSMTPPassword;
        private System.Windows.Forms.TextBox textSMTPUsername;
        private System.Windows.Forms.TextBox textSMTPPassword;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBoxSavePassword;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label lblSMTPDomain;
        private System.Windows.Forms.TextBox textBoxSMTPDomain;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox SMTPUseNetworkCreds;
        private System.Windows.Forms.CheckBox SMTPUseSecureAuthn;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label lblSMTPPort;
        private System.Windows.Forms.TextBox textSMTPPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblSMTPAddress;
        private System.Windows.Forms.TextBox textSMTPAddress;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblCCEmail;
        private System.Windows.Forms.TextBox textSMTPCCEmail;
        private System.Windows.Forms.Button btnTestEmail;
        private System.Windows.Forms.TextBox textSMTPToEmail;
        private System.Windows.Forms.Label lblToAddress;
        private System.Windows.Forms.TextBox textSMTPFromEmail;
        private System.Windows.Forms.Label lblFromAddress;
    }
}