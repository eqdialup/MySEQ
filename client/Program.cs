using System;
using System.Windows.Forms;
using Structures;

namespace myseq
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(true);
            try { Application.Run(new FrmMain()); }
            catch (NullReferenceException e)
            {
                var s = $"Uncaught exception in Main(): {e.Message}";
                LogLib.WriteLine(s);
                MessageBox.Show(s);
                Application.Exit();
            }
        }
    }
}