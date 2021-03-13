using System;
using System.IO;
using myseq.Properties;

namespace Structures
{
    public enum LogLevel{
        Off = 0,            // Set maxLogLevel to Off and no logging occurs
        Error = 1,          // Used for exceptions and other errors
        Warning = 2,        // Used for les
        Info = 3,           // Used for information ("Loaded map XYZ")
        Debug = 4,          // Used for debug stuff, not too often though
        //
        Default = Error,        // Used when WriteLine is called without a level
        DefaultMaxLevel = Error // Starting log level
    };

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
                WriteLine($"{msg}{ex.Message}\n{ex.StackTrace}", LogLevel.Error);
            }/* How does one log an error if the logging function is the one erroring? */
        }

        public static void WriteLine(string msg, LogLevel logLevel)
        {
            if (logLevel <= maxLogLevel && logLevel > LogLevel.Off)
            {
                    string logpath = Settings.Default.LogDir;
                    string logfile = $"{DateTime.Now:MM-dd-yyyy}.txt";

                    if (!Directory.Exists(logpath)) Directory.CreateDirectory(logpath);

                    FileStream fs = new FileStream(Path.Combine(logpath, logfile),FileMode.Append,FileAccess.Write,FileShare.ReadWrite);
                    StreamWriter outLog = new StreamWriter(fs);
                    outLog.WriteLine($"[{(int)logLevel}] {DateTime.Now:MM/dd/yyyy HH:mm:ss.ff} - {msg}");
                    outLog.Close();
                    fs.Close();
/* How does one log an error if the logging function is the one erroring? */
            }
        }
    }
    #endregion
}