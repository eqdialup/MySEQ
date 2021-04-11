using System.Runtime.InteropServices;
using System.Text;

namespace myseq
{
    /// <summary>
    /// Small classes with static methods, and have little to no impact on experience.
    /// moved to parasoll file for easier overview and reduce clutter in the larger classes.
    /// </summary>
    internal static class SafeNativeMethods
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        internal static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        internal static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
        internal static extern bool Beep(uint freq, uint dur);

        [DllImport("winmm.dll")]
        internal static extern long PlaySound(string lpszName, System.IntPtr hModule, System.IntPtr dwFlags);
    }
}