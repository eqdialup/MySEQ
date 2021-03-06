
using System.Runtime.InteropServices;
using System.Text;

namespace Structures

{
    public class IniFile
    {
        public string path;

        [DllImport("kernel32")]

        private static extern long WritePrivateProfileString(string section,

            string key,string val,string filePath);

        [DllImport("kernel32")]

        private static extern int GetPrivateProfileString(string section,

            string key,string def, StringBuilder retVal, int size,string filePath);

        public IniFile(string INIPath){
            path = INIPath;
        }

        public void WriteValue(string Section, string Key, string Value) {
            WritePrivateProfileString(Section, Key, Value, path);
        }

        public string ReadValue(string Section, string Key, string Default) {
            StringBuilder buffer = new StringBuilder(255);

            GetPrivateProfileString(Section, Key, Default, buffer, 255, path);

            return buffer.ToString();
        }
    }
}