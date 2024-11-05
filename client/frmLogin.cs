using myseq.Properties;
using Structures;
using System;
using System.ComponentModel;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace myseq
{
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
            LoadInitialSettings();
        }

        private void LoadInitialSettings()
        {
            textSMTPAddress.Text = SmtpSettings.Instance.SmtpServer;
            textSMTPPort.Text = SmtpSettings.Instance.SmtpPort.ToString();
            textBoxSMTPDomain.Text = SmtpSettings.Instance.SmtpDomain;
            textSMTPUsername.Text = SmtpSettings.Instance.SmtpUsername;

            textSMTPPassword.Text = SmtpSettings.Instance.SmtpPassword.Length > 0 ? "Password:::1" : "";

            textSMTPToEmail.Text = SmtpSettings.Instance.ToEmail;
            textSMTPFromEmail.Text = SmtpSettings.Instance.FromEmail;
            textSMTPCCEmail.Text = SmtpSettings.Instance.CCEmail;
            SMTPUseNetworkCreds.Checked = SmtpSettings.Instance.UseNetworkCredentials;
            SMTPUseSecureAuthn.Checked = SmtpSettings.Instance.UseSSL;
            checkBoxSavePassword.Checked = SmtpSettings.Instance.SavePassword;

            UpdateControlState();
        }

        private void UpdateControlState()
        {
            bool useNetworkCredentials = SmtpSettings.Instance.UseNetworkCredentials;
            textSMTPUsername.Enabled = !useNetworkCredentials;
            textSMTPPassword.Enabled = !useNetworkCredentials;
            lblSMTPPassword.Enabled = !useNetworkCredentials;
            lblSMTPUsername.Enabled = !useNetworkCredentials;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            SaveSettings();
            Hide();
        }

        private void BtnTestEmail_Click(object sender, EventArgs e)
        {
            UpdateSMTPSettings();

            string errorMessage = ValidateEmailSettings();
            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                MessageBox.Show($"{errorMessage}\r\nSending Test Email Aborted.",
                                "Some Email Settings Missing.",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            try
            {
                SendTestEmail();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Send Mail Error: {ex.Message}",
                                "Mail Message Failed to Send.",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        private string ValidateEmailSettings()
        {
            var errmsg = new StringBuilder();

            if (string.IsNullOrWhiteSpace(textSMTPAddress.Text))
            {
                errmsg.Append(string.IsNullOrWhiteSpace(textSMTPPort.Text)
                    ? "Enter a valid SMTP Server Address and Port.\r\n"
                    : "Enter a valid SMTP Server Address.\r\n");
            }
            else if (string.IsNullOrWhiteSpace(textSMTPPort.Text))
            {
                errmsg.Append("Enter a valid SMTP Server Port.\r\n");
            }

            if (string.IsNullOrWhiteSpace(textSMTPFromEmail.Text))
            {
                errmsg.Append(string.IsNullOrWhiteSpace(textSMTPToEmail.Text)
                    ? "Valid From and To Email Addresses are required.\r\n"
                    : "Enter a valid From Email Address.\r\n");
            }
            else if (string.IsNullOrWhiteSpace(textSMTPToEmail.Text))
            {
                errmsg.Append("Enter a valid To Email Address.\r\n");
            }

            return errmsg.ToString();
        }

        private void SendTestEmail()
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(SmtpSettings.Instance.FromEmail),
                Subject = "MySEQ Spawn Alert",
                Body = "This is a test MySEQ Email Alert."
            };

            AddEmailAddresses(mailMessage.To, SmtpSettings.Instance.ToEmail);
            AddEmailAddresses(mailMessage.CC, SmtpSettings.Instance.CCEmail);

            using (var smtpClient = new SmtpClient(SmtpSettings.Instance.SmtpServer, SmtpSettings.Instance.SmtpPort))
            {
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.EnableSsl = SmtpSettings.Instance.UseSSL;

                if (SmtpSettings.Instance.UseNetworkCredentials)
                {
                    smtpClient.UseDefaultCredentials = true;
                }
                else
                {
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = string.IsNullOrWhiteSpace(SmtpSettings.Instance.SmtpDomain)
                        ? new System.Net.NetworkCredential(SmtpSettings.Instance.SmtpUsername, SmtpSettings.Instance.SmtpPassword)
                        : new System.Net.NetworkCredential(SmtpSettings.Instance.SmtpUsername, SmtpSettings.Instance.SmtpPassword, SmtpSettings.Instance.SmtpDomain);
                }

                smtpClient.SendCompleted += SmtpClient_OnCompleted;
                smtpClient.SendAsync(mailMessage, mailMessage);
            }
        }

        private void AddEmailAddresses(MailAddressCollection addressCollection, string emailList)
        {
            var delimiters = new[] { ',', ';' };
            var addresses = emailList.Split(delimiters);

            foreach (var address in addresses)
            {
                var trimmedAddress = address.Trim();
                if (IsValidEmail(trimmedAddress, string.Empty))
                {
                    addressCollection.Add(new MailAddress(trimmedAddress));
                }
            }
        }

        private void CheckBoxSavePassword_Click(object sender, EventArgs e)
        {
            SmtpSettings.Instance.SavePassword = !SmtpSettings.Instance.SavePassword;
            checkBoxSavePassword.Checked = SmtpSettings.Instance.SavePassword;
        }

        private void CheckBoxSMTPUseNetworkCredentials_Click(object sender, EventArgs e)
        {
            SmtpSettings.Instance.UseNetworkCredentials = !SmtpSettings.Instance.UseNetworkCredentials;
            SMTPUseNetworkCreds.Checked = SmtpSettings.Instance.UseNetworkCredentials;
            UpdateControlState();
        }

        private void CheckBoxSMTPUseSecureAuthentication_Click(object sender, EventArgs e)
        {
            bool useSsl = !SmtpSettings.Instance.UseSSL;
            SmtpSettings.Instance.UseSSL = useSsl;
            SmtpSettings.Instance.SmtpPort = useSsl ? 587 : 25;
            textSMTPPort.Text = SmtpSettings.Instance.SmtpPort.ToString();
            SMTPUseSecureAuthn.Checked = useSsl;
        }

        private void SaveSettings()
        {
            SmtpSettings.Instance.SmtpUsername = string.IsNullOrWhiteSpace(textSMTPUsername.Text) ? "" : textSMTPUsername.Text;

            SmtpSettings.Instance.SmtpPassword = !string.IsNullOrWhiteSpace(textSMTPPassword.Text) && textSMTPPassword.Text != "Password:::1"
                ? textSMTPPassword.Text
                : "";

            Settings.Default.EmailAlerts = !Settings.Default.EmailAlerts;
        }

        private void SmtpClient_OnCompleted(object sender, AsyncCompletedEventArgs e)
        {
            var mail = (MailMessage)e.UserState;
            var subject = mail.Subject;

            if (e.Cancelled)
            {
                MessageBox.Show($"Send canceled for [{subject}].", "Sending Mail Canceled", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (e.Error != null)
            {
                MessageBox.Show($"Error sending [{subject}]\r\n{e.Error}", "Error Sending Mail", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show($"Message [{subject}] sent successfully.", "SMTP Configured Properly", MessageBoxButtons.OK);
            }
        }

        private void UpdateSMTPSettings()
        {
            SmtpSettings.Instance.UseSSL = SMTPUseSecureAuthn.Checked;
            UpdateSmtpPassword();
            SmtpSettings.Instance.SmtpUsername = textSMTPUsername.Text;
            SmtpSettings.Instance.SmtpDomain = textBoxSMTPDomain.Text;
            UpdateSmtpPort();
            SmtpSettings.Instance.FromEmail = UpdateEmailAddress(textSMTPFromEmail.Text);
            SmtpSettings.Instance.ToEmail = UpdateEmailList(textSMTPToEmail.Text);
            SmtpSettings.Instance.CCEmail = UpdateEmailList(textSMTPCCEmail.Text);
            SmtpSettings.Instance.SmtpServer = UpdateSmtpServer(textSMTPAddress.Text);

            textSMTPToEmail.Text = SmtpSettings.Instance.ToEmail;
            textSMTPFromEmail.Text = SmtpSettings.Instance.FromEmail;
            textSMTPCCEmail.Text = SmtpSettings.Instance.CCEmail;
            textSMTPAddress.Text = SmtpSettings.Instance.SmtpServer;
        }

        private void UpdateSmtpPassword()
        {
            SmtpSettings.Instance.SmtpPassword = !string.IsNullOrWhiteSpace(textSMTPPassword.Text) && textSMTPPassword.Text != "Password:::1"
                ? textSMTPPassword.Text
                : string.Empty;
        }

        private void UpdateSmtpPort()
        {
            if (int.TryParse(textSMTPPort.Text, out var port) && port > 0)
            {
                SmtpSettings.Instance.SmtpPort = port;
            }
            else
            {
                textSMTPPort.Text = SmtpSettings.Instance.SmtpPort.ToString();
            }
        }
        private const string emailPattern = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
        private string UpdateEmailAddress(string email)
        {
            return IsValidEmail(email, emailPattern) ? email : string.Empty;
        }

        private string UpdateEmailList(string emailList)
        {
            var result = PostSplitString(new StringBuilder(), new Regex(emailPattern), emailList);
            return string.IsNullOrWhiteSpace(result) ? string.Empty : result;
        }

        private string UpdateSmtpServer(string serverAddress)
        {
            const string hostPattern = @"^((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            return IsValidEmail(serverAddress, hostPattern) ? serverAddress : string.Empty;
        }

        private bool IsValidEmail(string email, string pattern)
        {
            return !string.IsNullOrWhiteSpace(email) && new Regex(pattern).IsMatch(email);
        }

        private static string PostSplitString(StringBuilder addr, Regex pattern, string input)
        {
            var result = new StringBuilder();
            const string delimiters = ",;<>[] ";
            var delimiterArray = delimiters.ToCharArray();

            foreach (var segment in input.Split(delimiterArray, StringSplitOptions.RemoveEmptyEntries))
            {
                var trimmedSegment = segment.Trim();

                if (pattern.IsMatch(trimmedSegment))
                {
                    if (result.Length > 0)
                    {
                        result.Append(" ");
                    }

                    result.Append(trimmedSegment);
                }
            }

            addr.Append(result.ToString());
            return result.ToString();
        }
    }
}
