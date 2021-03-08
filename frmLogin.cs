using System;
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
            Hide();
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Hide();
        }
        private void SaveSettings()
        {
            // Username
            if (textSMTPUsername.Text.Length == 0)
                SmtpSettings.Instance.SmtpUsername = "";
            if (textSMTPUsername.Text.Length > 0)
                SmtpSettings.Instance.SmtpUsername = textSMTPUsername.Text;
            // Password
            if (textSMTPPassword.Text.Length > 0 && textSMTPPassword.Text != "Password:::1")
            {
                SmtpSettings.Instance.SmtpPassword = textSMTPPassword.Text;
            }

            if (textSMTPPassword.Text.Length == 0)
                SmtpSettings.Instance.SmtpPassword = "";


            Settings.Instance.EmailAlerts = !Settings.Instance.EmailAlerts;
        }
        private void LoadSettings()
        {
            textSMTPUsername.Text = SmtpSettings.Instance.SmtpUsername;
            textSMTPPassword.Text = SmtpSettings.Instance.SmtpPassword.Length > 0 ? "Password:::1" : "";

            if (SmtpSettings.Instance.UseNetworkCredentials)
            {
                textSMTPUsername.Enabled = false;
                textSMTPPassword.Enabled = false;
                lblSMTPPassword.Enabled = false;
                lblSMTPUsername.Enabled = false;
            }
            else
            {
                textSMTPUsername.Enabled = true;
                textSMTPPassword.Enabled = true;
                lblSMTPPassword.Enabled = true;
                lblSMTPUsername.Enabled = true;
            }
            checkBoxSavePassword.Checked = SmtpSettings.Instance.SavePassword;
        }

        private void checkBoxSavePassword_Click(object sender, EventArgs e)
        {
            SmtpSettings.Instance.SavePassword = !SmtpSettings.Instance.SavePassword;

            checkBoxSavePassword.Checked = SmtpSettings.Instance.SavePassword;
        }
    }
}
