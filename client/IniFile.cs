using System.Text;
using myseq;

namespace Structures
{
    public class IniFile
    {
        private readonly string _path;

        public IniFile(string iniPath)
        {
            _path = FileOps.CombineCfgDir(iniPath);
        }

        public void WriteValue(string section, string key, string value) => SafeNativeMethods.WritePrivateProfileString(section, key, value, _path);

        public string ReadValue(string section, string key, string defaultValue)
        {
            StringBuilder buffer = new StringBuilder(255);

            SafeNativeMethods.GetPrivateProfileString(section, key, defaultValue, buffer, 255, _path);

            return buffer.ToString();
        }
    }
}