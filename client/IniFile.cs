using myseq;
using System.Text;

namespace Structures
{
    public class IniFile
    {
        private readonly string path;

        public IniFile(string INIPath)
        {
            path = FileOps.CombineCfgDir(INIPath);
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