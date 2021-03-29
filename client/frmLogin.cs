using System;
using System.ComponentModel;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using myseq.Properties;
using Structures;

namespace myseq
{
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
            textSMTPAddress.Text = SmtpSettings.Instance.SmtpServer;
            textSMTPPort.Text = SmtpSettings.Instance.SmtpPort.ToString();
            textBoxSMTPDomain.Text = SmtpSettings.Instance.SmtpDomain;
            textSMTPUsername.Text = SmtpSettings.Instance.SmtpUsername;

            if (SmtpSettings.Instance.SmtpPassword.Length > 0)
                textSMTPPassword.Text = "Password:::1";
            else
                textSMTPPassword.Text = "";
            textSMTPToEmail.Text = SmtpSettings.Instance.ToEmail;
            textSMTPFromEmail.Text = SmtpSettings.Instance.FromEmail;
            textSMTPCCEmail.Text = SmtpSettings.Instance.CCEmail;
            checkBoxSMTPUseNetworkCredentials.Checked = SmtpSettings.Instance.UseNetworkCredentials;
            checkBoxSMTPUseSecureAuthentication.Checked = SmtpSettings.Instance.UseSSL;
            checkBoxSavePassword.Checked = SmtpSettings.Instance.SavePassword;

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
            LoadSettings();
        }
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            SaveSettings();
            this.Hide();
        }
        private void BtnTestEmail_Click(object sender, EventArgs e)
        {
            UpdateSMTPSettings();
            // check email parameters all filled out
            string errmsg = "";
            if (textSMTPAddress.Text.Length == 0)
            {
                if (textSMTPPort.Text.Length == 0)
                    errmsg += "Enter a valid SMTP Server Address and Port.\r\n";
                else
                    errmsg += "Enter a valid SMTP Server Address.\r\n";
            }
            else if (textSMTPPort.Text.Length == 0)
            {
                errmsg += "Enter a valid SMTP Server Port.\r\n";
            }
            if (textSMTPFromEmail.Text.Length == 0)
            {
                if (textSMTPToEmail.Text.Length == 0)
                    errmsg += "Valid From and To Email Addresses are required.\r\n";
                else
                    errmsg += "Enter a valid From Email Address.\r\n";
            }
            else if (textSMTPToEmail.Text.Length == 0)
            {
                errmsg += "Enter a valid To Email Address.\r\n";
            }
            if (errmsg != string.Empty)
            {
                errmsg += "\r\nSending Test Email Aborted.";
                MessageBox.Show(errmsg, "Some Email Settings Missing.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                // Set up the message
                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress(SmtpSettings.Instance.FromEmail)
                };

                // To Email Address could contain multiple addresses
                string presplit = SmtpSettings.Instance.ToEmail;
                const string delim = ",;";
                char[] delimarray = delim.ToCharArray();
                string[] split = presplit.Split(delimarray);
                foreach (string s in split)
                {
                    if (s.Trim().Length > 0)
                        mailMessage.To.Add(new MailAddress(s.Trim()));
                }

                // CC email addresses
                presplit = SmtpSettings.Instance.CCEmail;
                split = presplit.Split(delimarray);
                foreach (string s in split)
                {
                    if (s.Trim().Length > 0)
                        mailMessage.CC.Add(new MailAddress(s.Trim()));
                }

                mailMessage.Subject = "MySEQ Spawn Alert";
                mailMessage.Body = "This is a test MySEQ Email Alert.";

                SmtpClient smtpClient = new SmtpClient(SmtpSettings.Instance.SmtpServer, SmtpSettings.Instance.SmtpPort)
                {
                    DeliveryMethod = SmtpDeliveryMethod.Network
                };

                if (SmtpSettings.Instance.UseNetworkCredentials)
                {
                    smtpClient.UseDefaultCredentials = true;
                }
                else
                {
                    smtpClient.UseDefaultCredentials = false;
                    if (SmtpSettings.Instance.SmtpDomain != string.Empty)
                        smtpClient.Credentials = new System.Net.NetworkCredential(SmtpSettings.Instance.SmtpUsername, SmtpSettings.Instance.SmtpPassword, SmtpSettings.Instance.SmtpDomain);
                    else
                        smtpClient.Credentials = new System.Net.NetworkCredential(SmtpSettings.Instance.SmtpUsername, SmtpSettings.Instance.SmtpPassword);
                }
                // using SSL to authenticate?
                smtpClient.EnableSsl = SmtpSettings.Instance.UseSSL;
                smtpClient.SendCompleted += new SendCompletedEventHandler(SmtpClient_OnCompleted);

                object UserState = mailMessage;
                // Send away ....
                try { smtpClient.SendAsync(mailMessage, UserState); }
                catch (Exception Ex) { MessageBox.Show("Send Mail Error: " + Ex.Message, "Mail Message Failed to Send.", MessageBoxButtons.OK, MessageBoxIcon.Error); }
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

            checkBoxSMTPUseNetworkCredentials.Checked = SmtpSettings.Instance.UseNetworkCredentials;

            if (SmtpSettings.Instance.UseNetworkCredentials)
            {
                this.textSMTPPassword.Enabled = false;
                this.textSMTPUsername.Enabled = false;
                this.lblSMTPPassword.Enabled = false;
                this.lblSMTPUsername.Enabled = false;
            }
            else
            {
                this.textSMTPPassword.Enabled = true;
                this.textSMTPUsername.Enabled = true;
                this.lblSMTPPassword.Enabled = true;
                this.lblSMTPUsername.Enabled = true;
            }
        }

        private void CheckBoxSMTPUseSecureAuthentication_Click(object sender, EventArgs e)
        {
            SmtpSettings.Instance.UseSSL = !SmtpSettings.Instance.UseSSL;

            if (SmtpSettings.Instance.UseSSL)
            {
                SmtpSettings.Instance.SmtpPort = 587;
                textSMTPPort.Text = SmtpSettings.Instance.SmtpPort.ToString();
            }
            else
            {
                SmtpSettings.Instance.SmtpPort = 25;
                textSMTPPort.Text = SmtpSettings.Instance.SmtpPort.ToString();
            }

            checkBoxSMTPUseSecureAuthentication.Checked = SmtpSettings.Instance.UseSSL;
        }

        private void LoadSettings()
        {
            this.textSMTPUsername.Text = SmtpSettings.Instance.SmtpUsername;
            if (SmtpSettings.Instance.SmtpPassword.Length > 0)
                this.textSMTPPassword.Text = "Password:::1";
            else
                this.textSMTPPassword.Text = "";

            if (SmtpSettings.Instance.UseNetworkCredentials)
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
            checkBoxSavePassword.Checked = SmtpSettings.Instance.SavePassword;
        }

        private void SaveSettings()
        {
            // Username
            if (this.textSMTPUsername.Text.Length == 0)
                SmtpSettings.Instance.SmtpUsername = "";
            if (this.textSMTPUsername.Text.Length > 0)
                SmtpSettings.Instance.SmtpUsername = this.textSMTPUsername.Text;
            // Password
            if (this.textSMTPPassword.Text.Length > 0 && this.textSMTPPassword.Text != "Password:::1")
            {
                SmtpSettings.Instance.SmtpPassword = this.textSMTPPassword.Text;
            }

            if (this.textSMTPPassword.Text.Length == 0)
                SmtpSettings.Instance.SmtpPassword = "";

            Settings.Default.EmailAlerts = !Settings.Default.EmailAlerts;
        }
        private void SmtpClient_OnCompleted(object sender, AsyncCompletedEventArgs e)
        {
            //Get the Original MailMessage object
            MailMessage mail = (MailMessage)e.UserState;

            //write out the subject
            string subject = mail.Subject;

            if (e.Cancelled)
            {
                MessageBox.Show("Send canceled for [" + subject + "].", "Sending Mail Canceled", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (e.Error != null)
            {
                MessageBox.Show("Error sending [" + subject + "\r\n" + e.Error.ToString(), "Error Sending Mail", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Message [" + subject + "] sent successfully.", "SMTP Configured Properly", MessageBoxButtons.OK);
            }
        }

        private void UpdateSMTPSettings()
        {
            StringBuilder addr = new StringBuilder();
            SmtpSettings.Instance.UseSSL = this.checkBoxSMTPUseSecureAuthentication.Checked;
            if (this.textSMTPPassword.ToString().Length > 0 && this.textSMTPPassword.Text != "Password:::1") {
                SmtpSettings.Instance.SmtpPassword = this.textSMTPPassword.Text;
            }

            if (this.textSMTPPassword.ToString().Length == 0 && SmtpSettings.Instance.SmtpPassword.Length > 0)
                SmtpSettings.Instance.SmtpPassword = "";
            SmtpSettings.Instance.SmtpUsername = this.textSMTPUsername.Text;
            SmtpSettings.Instance.SmtpDomain = this.textBoxSMTPDomain.Text;

            // make sure value for port is an int
            if (this.textSMTPPort.Text.Length > 0)
            {
                string Str = this.textSMTPPort.Text;
                bool isNum = int.TryParse(Str, out var Num);
                if (isNum && Num > 0)
                {
                    SmtpSettings.Instance.SmtpPort = Num;
                }
                else
                {
                    this.textSMTPPort.Text = SmtpSettings.Instance.SmtpPort.ToString();
                }
            }

            // Check if emails entered look like email addresses
            const string emailPattern = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(emailPattern);

            // From Email Address
            if (textSMTPFromEmail.Text.Length > 0 && re.IsMatch(textSMTPFromEmail.Text))
                SmtpSettings.Instance.FromEmail = textSMTPFromEmail.Text;

            // To Email Address (can contain multiple addresses seperated by comma or semicolon
            if (textSMTPToEmail.Text.Length == 0)
                SmtpSettings.Instance.ToEmail = "";
            // split of To: email string with a , or ; delimiter, and verify
            if (textSMTPToEmail.Text.Length > 0)
            {
                string presplit = textSMTPToEmail.Text;
                string postsplit = "";
                const string delim = ",;<>[] ";
                char[] delimarray = delim.ToCharArray();
                foreach (string s in presplit.Split(delimarray))
                {
                    if (re.IsMatch(s.Trim()))
                    {
                        if (postsplit.Length > 0)
                        {
                            addr.Append(s);
                            postsplit = addr.ToString();
                        }
                        else
                        {
                            postsplit = s.Trim();
                        }
                    }
                }
                if (postsplit.Length > 0)
                {
                    SmtpSettings.Instance.ToEmail = postsplit;
                }
                else
                {
                    SmtpSettings.Instance.ToEmail = "";
                }
            }

            // CC Email Address (can contain multiple addresses seperated by comma or semicolon

            if (textSMTPCCEmail.Text.Length == 0)
                SmtpSettings.Instance.CCEmail = "";
            if (textSMTPCCEmail.Text.Length > 0)
            {
                string presplit = textSMTPCCEmail.Text;
                string postsplit = "";
                const string delim = ",;<>[] ";
                char[] delimarray = delim.ToCharArray();
                foreach (string s in presplit.Split(delimarray))
                {
                    if (re.IsMatch(s.Trim()))
                    {
                        if (postsplit.Length > 0)
                        {
                            addr.Append(s);
                            postsplit = addr.ToString();
                        }
                        else
                        {
                            postsplit = s.Trim();
                        }
                    }
                }
                if (postsplit.Length > 0)
                {
                    SmtpSettings.Instance.CCEmail = postsplit;
                }
                else
                {
                    SmtpSettings.Instance.CCEmail = "";
                }
            }

            textSMTPToEmail.Text = SmtpSettings.Instance.ToEmail;
            textSMTPFromEmail.Text = SmtpSettings.Instance.FromEmail;
            textSMTPCCEmail.Text = SmtpSettings.Instance.CCEmail;

            // Check if the SMTP Server Address looks like a host name
            const string hostPattern = @"^((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            if (textSMTPAddress.Text.Length == 0)
                SmtpSettings.Instance.SmtpServer = "";
            if (textSMTPAddress.Text.Length > 0 && new Regex(hostPattern).IsMatch(textSMTPAddress.Text))
                SmtpSettings.Instance.SmtpServer = textSMTPAddress.Text;
            textSMTPAddress.Text = SmtpSettings.Instance.SmtpServer;
        }
    }
}
