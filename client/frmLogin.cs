using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Structures;

namespace myseq
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
            LoadSettings();
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            SaveSettings();
            this.Hide();
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
        private void SaveSettings()
        {
            // Username
            if (this.textSMTPUsername.Text.ToString().Length == 0)
                SMTPSettings.Instance.SmtpUsername = "";
            if (this.textSMTPUsername.Text.ToString().Length > 0)
                SMTPSettings.Instance.SmtpUsername = this.textSMTPUsername.Text.ToString();
            // Password
            if (this.textSMTPPassword.Text.ToString().Length > 0 && this.textSMTPPassword.Text.ToString() != "Password:::1")
            {
                SMTPSettings.Instance.SmtpPassword = this.textSMTPPassword.Text.ToString();
                //char[] pass = this.textSMTPPassword.Text.ToCharArray();
               // Settings.Instance.SmtpPassword.Clear();
                //foreach (char d in pass)
                //{
                //    Settings.Instance.SmtpPassword.AppendChar(d);
                //}
                //this.textSMTPPassword.Text = "Password:::1";
            }

            if (this.textSMTPPassword.Text.ToString().Length == 0)
                SMTPSettings.Instance.SmtpPassword = "";
                //Settings.Instance.SmtpPassword.Clear();

            Settings.Instance.EmailAlerts = !Settings.Instance.EmailAlerts;

        }
        private void LoadSettings()
        {
            this.textSMTPUsername.Text = SMTPSettings.Instance.SmtpUsername.ToString();
            if (SMTPSettings.Instance.SmtpPassword.ToString().Length > 0)
                this.textSMTPPassword.Text = "Password:::1";
            else
                this.textSMTPPassword.Text = "";
            
            if (SMTPSettings.Instance.UseNetworkCredentials)
            {
                this.textSMTPUsername.Enabled = false;
                this.textSMTPPassword.Enabled = false;
                this.lblSMTPPassword.Enabled = false;
                this.lblSMTPUsername.Enabled = false;
            }
            else
            {
                this.textSMTPUsername.Enabled = true;
                this.textSMTPPassword.Enabled = true;
                this.lblSMTPPassword.Enabled = true;
                this.lblSMTPUsername.Enabled = true;
            }
            checkBoxSavePassword.Checked = SMTPSettings.Instance.SavePassword;
            
        }

        private void checkBoxSavePassword_Click(object sender, EventArgs e)
        {
            SMTPSettings.Instance.SavePassword = !SMTPSettings.Instance.SavePassword;

            checkBoxSavePassword.Checked = SMTPSettings.Instance.SavePassword;
        }
    }
}
