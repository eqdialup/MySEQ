using System;
using System.IO;
using System.Windows.Forms;
using myseq.Properties;

namespace Structures
{
    #region LogLib class

    public static class LogLib
    {
        public static LogLevel maxLogLevel;

        public static void WriteLine(string msg)
        {
            // Should only really be used for Error logging
            // Anything else should have an explicit level set
            WriteLine(msg, LogLevel.Default);
        }

        public static void WriteLine(string msg, Exception ex)
        {
            if (maxLogLevel >= LogLevel.Error)
            {
                WriteLine($" {msg} {ex.Message} \n {ex.StackTrace} ", LogLevel.Error);
            }/* How does one log an error if the logging function is the one erroring? */
        }

        public static void WriteLine(string msg, LogLevel logLevel)
        {
            if (logLevel <= maxLogLevel && logLevel > LogLevel.Off)
            {
                var logpath = string.IsNullOrEmpty(Settings.Default.LogDir)
                    ? Path.Combine(Application.ExecutablePath, "logs")
                    : Settings.Default.LogDir;

                var logfile = $"{DateTime.Now:MM-dd-yyyy}.txt";
                Directory.CreateDirectory(logpath);

                FileStream fs = new FileStream(Path.Combine(logpath, logfile), FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                using (StreamWriter outLog = new StreamWriter(fs))
                {
                    outLog.WriteLine($"[{(int)logLevel}] {DateTime.Now:MM/dd/yyyy HH:mm:ss.ff} - {msg}");
                }
                fs.Close();
                /* How does one log an error if the logging function is the one erroring? */
            }
        }
    }

    #endregion LogLib class
}