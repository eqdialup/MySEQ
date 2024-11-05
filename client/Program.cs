using Structures;
using System;
using System.Windows.Forms;

namespace myseq
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(true);
            try
            {
                Application.Run(new MainForm());
            }
            catch (Exception e)
            {
                var s = $"Uncaught exception in Main(): {e.Message} \n - STACKTRACE {e.StackTrace}";
                LogLib.WriteLine(s);
                Application.Exit();
            }
        }
    }
}