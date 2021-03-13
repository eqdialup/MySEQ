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
            Application.SetCompatibleTextRenderingDefault(false);
            try { Application.Run(new FrmMain()); }
            catch (NullReferenceException e)
            {
                string s = $"Uncaught exception in Main(): {e.Message}";

                
                LogLib.WriteLine(s);

                MessageBox.Show(s);
                Application.Exit();
            }
        }
    }
}