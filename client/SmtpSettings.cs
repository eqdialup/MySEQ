using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

namespace Structures
{
    [Serializable]
    public sealed class SmtpSettings
    {
        private static readonly object SyncObj = new object();
        private static SmtpSettings instance;

        private string smtpPassword = "";
        private string prefsDir = "";

        public string SmtpServer { get; set; } = "";
        public string SmtpDomain { get; set; } = "";
        public string SmtpUsername { get; set; } = "";

        public string SmtpPassword
        {
            get => string.IsNullOrEmpty(smtpPassword) ? "" : DecryptString(smtpPassword, GetAssemblyGuid());
            set => smtpPassword = string.IsNullOrEmpty(value) ? "" : EncryptString(value, GetAssemblyGuid());
        }

        public int SmtpPort { get; set; } = 25;
        public string FromEmail { get; set; } = "";
        public string ToEmail { get; set; } = "";
        public string CCEmail { get; set; } = "";
        public bool UseNetworkCredentials { get; set; }
        public bool UseSSL { get; set; }
        public bool SavePassword { get; set; } = false;

        private SmtpSettings() { }

        public static SmtpSettings Instance
        {
            get
            {
                lock (SyncObj)
                {
                    if (instance == null)
                    {
                        instance = new SmtpSettings();
                    }
                }
                return instance;
            }
        }

        public void Save(string filename)
        {
            if (string.IsNullOrEmpty(prefsDir))
            {
                prefsDir = FileOps.StartPath("Prefs");
            }

            var savedPassword = SavePassword ? SmtpPassword : "";
            if (!SavePassword)
            {
                SmtpPassword = "";
            }

            using (var fs = new FileStream(filename, FileMode.Create))
            {
                var serializer = new XmlSerializer(typeof(SmtpSettings));
                serializer.Serialize(fs, Instance);
            }

            if (!SavePassword)
            {
                SmtpPassword = savedPassword;
            }

            var oldConfigFile = FileOps.StartPath("myseq.xml");
            FileOps.DeleteFile(oldConfigFile);
        }

        public void Load(string filename)
        {
            try
            {
                using (var fs = new FileStream(filename, FileMode.Open))
                {
                    var serializer = new XmlSerializer(typeof(SmtpSettings));
                    instance = (SmtpSettings)serializer.Deserialize(fs);
                }
                Instance.prefsDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MySEQ");
            }
            catch (Exception ex)
            {
                LogLib.WriteLine("Error: SMTPSettings.Load()", ex);
            }
        }

        private static string DecryptString(string message, string key)
        {
            var aes = Aes.Create();
            var decryptor = aes.CreateDecryptor(GetKey(key), aes.IV);
            var data = Convert.FromBase64String(message);

            try
            {
                return Encoding.UTF8.GetString(decryptor.TransformFinalBlock(data, 0, data.Length));
            }
            catch (Exception ex)
            {
                LogLib.WriteLine("Error in DecryptString()", ex);
                return "";
            }
        }

        private static string EncryptString(string message, string key)
        {
            var aes = Aes.Create();
            var encryptor = aes.CreateEncryptor(GetKey(key), aes.IV);
            var data = Encoding.UTF8.GetBytes(message);

            try
            {
                var encrypted = encryptor.TransformFinalBlock(data, 0, data.Length);
                return Convert.ToBase64String(encrypted);
            }
            catch (Exception ex)
            {
                LogLib.WriteLine("Error in EncryptString()", ex);
                return "";
            }
        }

        private static byte[] GetKey(string key)
        {
            var hash = SHA256.Create();
            return hash.ComputeHash(Encoding.UTF8.GetBytes(key));
        }

        private static string GetAssemblyGuid()
        {
            var asm = Assembly.GetExecutingAssembly();
            return asm.GetType().GUID.ToString();
        }
    }
}
