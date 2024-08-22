using System;
using System.IO;

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
        }
        else
        {
            // log error message to a separate file or console
            try
            {
                File.AppendAllText("error.log", $"{msg}: {ex.Message}\n{ex.StackTrace}\n");
            }
            catch
            {
                Console.WriteLine($"Error logging failed: {msg}: {ex.Message}\n{ex.StackTrace}\n");
            }
        }
    }

    public static void WriteLine(string msg, LogLevel logLevel)
    {
        if (logLevel <= maxLogLevel && logLevel > LogLevel.Off)
        {
            var logfile = $"{DateTime.Now:MM-dd-yyyy}.txt";

            try
            {
                using (FileStream fs = new FileStream(FileOps.CombineLog(logfile), FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                using (StreamWriter outLog = new StreamWriter(fs))
                {
                    outLog.WriteLine($"[{(int)logLevel}] {DateTime.Now:MM/dd/yyyy HH:mm:ss.ff} - {msg}");
                }
            }
            catch (Exception ex)
            {
                // log error message to a separate file or console
                try
                {
                    File.AppendAllText("error.log", $"Error writing to log file: {ex.Message}\n{ex.StackTrace}\n");
                }
                catch
                {
                    Console.WriteLine($"Error logging failed: {ex.Message}\n{ex.StackTrace}\n");
                }
            }
        }
    }
}


    #endregion LogLib class
}