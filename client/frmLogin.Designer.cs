namespace myseq
{
    partial class frmLogin
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
            this.groupBox1.SuspendLayout();
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
            this.textSMTPUsername.Size = new System.Drawing.Size(152, 20);
            this.textSMTPUsername.TabIndex = 1;
            // 
            // textSMTPPassword
            // 
            this.textSMTPPassword.Location = new System.Drawing.Point(67, 42);
            this.textSMTPPassword.Name = "textSMTPPassword";
            this.textSMTPPassword.PasswordChar = '*';
            this.textSMTPPassword.Size = new System.Drawing.Size(152, 20);
            this.textSMTPPassword.TabIndex = 2;
            this.textSMTPPassword.UseSystemPasswordChar = true;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(163, 109);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "&Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(75, 109);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBoxSavePassword);
            this.groupBox1.Controls.Add(this.textSMTPPassword);
            this.groupBox1.Controls.Add(this.textSMTPUsername);
            this.groupBox1.Controls.Add(this.lblSMTPPassword);
            this.groupBox1.Controls.Add(this.lblSMTPUsername);
            this.groupBox1.Location = new System.Drawing.Point(8, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(230, 95);
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
            this.checkBoxSavePassword.Click += new System.EventHandler(this.checkBoxSavePassword_Click);
            // 
            // frmLogin
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(246, 144);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SMTP Email Settings";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

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

    }
}