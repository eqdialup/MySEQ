using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Soap;

namespace Structures
{
    public enum LogLevel{
        Off = 0,            // Set maxLogLevel to Off and no logging occurs
        Error = 1,          // Used for exceptions and other errors
        Warning = 2,        // Used for les
        Info = 3,           // Used for information ("Loaded map XYZ")
        Debug = 4,          // Used for debug stuff, not too often though
        DebugHeavy = 5,     // Used for debug stuff which creates loads of data
        Trace = 9,          // Used specifically for "Entering XYZ"..."Exiting XYZ", generates absolutely crazy amounts of data
        //
        Default = Error,        // Used when WriteLine is called without a level
        DefaultMaxLevel = Error // Starting log level
    };
    
    #region LogLib class
    public class LogLib
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
                try
                {
                    WriteLine(msg + ex.Message + "\n" + ex.StackTrace, LogLevel.Error);
                }            
                catch { /* How does one log an error if the logging function is the one erroring? */ }
            }
        }

        public static void WriteLine(string msg, LogLevel logLevel)
        {
            if (logLevel <= maxLogLevel && logLevel > LogLevel.Off)
            {
                try
                {
                    string logpath = Settings.Instance.LogDir;
                    string logfile = String.Format("{0}.txt", DateTime.Now.ToString("MM-dd-yyyy"));
                    
                    if (!Directory.Exists(logpath)) Directory.CreateDirectory(logpath);
                    
                    FileStream fs = new FileStream(Path.Combine(logpath, logfile),FileMode.Append,FileAccess.Write,FileShare.ReadWrite);
                    StreamWriter outLog = new StreamWriter(fs);
                    outLog.WriteLine(String.Format("[{2}] {0} - {1}", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.ff"), msg, (int)logLevel));
                    outLog.Close();
                    fs.Close();
                }
                catch { /* How does one log an error if the logging function is the one erroring? */ }
            }
        }
    }
    #endregion

}