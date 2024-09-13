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
    }
}