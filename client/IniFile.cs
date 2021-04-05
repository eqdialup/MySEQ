using System.Text;
using myseq;

namespace Structures
{
    public class IniFile
    {
        private readonly string path;

        public IniFile(string INIPath)
        {
            path = INIPath;
        }

        public void WriteValue(string Section, string Key, string Value) => SafeNativeMethods.WritePrivateProfileString(Section, Key, Value, path);

        public string ReadValue(string Section, string Key, string Default)
        {
            StringBuilder buffer = new StringBuilder(255);

            SafeNativeMethods.GetPrivateProfileString(Section, Key, Default, buffer, 255, path);

            return buffer.ToString();
        }
    }
}