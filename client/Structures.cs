using System;

using System.IO;

using System.Text;

using System.Text.RegularExpressions;

using System.Drawing;

using System.Windows.Forms;

using System.Runtime.InteropServices;

using System.Runtime.Serialization.Formatters.Soap;



namespace Structures

{

    #region SPAWNINFO class

    [StructLayout(LayoutKind.Sequential, Pack=1)]

    public class SPAWNINFO {

        public SPAWNINFO() {}

        

        private string BytesToString(byte []b, int start, int maxlen) {

            int i=0;

            // look for a null

            for(;i<maxlen&&b[start+i]!='\0';i++);

            return Encoding.ASCII.GetString(b, start, i);

        }



        public void frombytes(byte []b, int offset) 

        {

            //flags = (SPAWNINFO.PacketType)BitConverter.ToInt32(b, 84 + offset);

            Name = BytesToString(b, 0 + offset, 30);

            Y = BitConverter.ToSingle(b, 30 + offset);

            X = BitConverter.ToSingle(b, 34 + offset);

            Z = BitConverter.ToSingle(b, 38 + offset);

            Heading = BitConverter.ToSingle(b, 42 + offset);

            SpeedRun = BitConverter.ToSingle(b, 46 + offset);

            SpawnID = BitConverter.ToUInt32(b, 50 + offset);

            OwnerID = BitConverter.ToUInt32(b, 54 + offset);

            Type = b[58 + offset];

            Class = b[59 + offset];

            Race = BitConverter.ToInt32(b, 60 + offset);

            Level = b[64 + offset];

            Hide = b[65 + offset];

            Primary = BitConverter.ToInt32(b, 66 + offset);
            Offhand = BitConverter.ToInt32(b, 70 + offset);

            Lastname = BytesToString(b, 74 + offset, 22);

            flags = (SPAWNINFO.PacketType)BitConverter.ToInt32(b, 96 + offset);

            if (flags==PacketType.Player)

                m_isPlayer = true;

        }



        public enum PacketType {

            Spawn = 0,

            Target = 1,

            Zone = 4,

            GroundItem = 5,

            GetProcessInfo = 6,

            SetProcess = 7,

            World = 8,

            Player = 253

        }



        public string Name = "";

        public float Y = 0;

        public float X = 0;

        public float Z = 0;

        public float Heading = 0;

        public float SpeedRun = 0;

        public uint SpawnID = 0;

        public uint OwnerID = 0;

        public byte Type = 0;

        public byte Class = 0;

        public int Race = 0;
        
        public int Primary = 0;

        public int Offhand = 0;

        public byte Gender = 0;

        public byte Level = 0;

        public byte Hide;       

        public string Lastname = "";

        public int gone = 0;

        public int refresh = 0;

        public PacketType flags = 0;

        public bool isHunt = false;

        public bool isCaution = false;

        public bool isDanger = false;

        public bool isAlert = false;

        public bool isPet = false;

        public bool isMerc = false;

        public bool isCorpse = false;

        public bool isMount = false;

        public bool isFamiliar = false;

        public bool isLDONObject = false;

        public bool isEventController = false;

        public bool isLookup = false;

        public bool hidden = false;

        public bool filtered = false;

        public ListViewItem listitem = null;

        public Pen DrawPen = null;

        public Brush DrawBrush = null;

        public bool m_isPlayer = false;

        public bool m_isMyCorpse = false;

        public bool delFromList = false;

        public bool proxAlert = false;

        public bool alertMob = false;

        public string SpawnLoc = "";

        public string ZoneSpawnLoc = "";

        public bool IsPlayer() {return m_isPlayer;}

    }

    #endregion



    #region GroundItem class

    public class GroundItem { 

        public float X = 0.0f;

        public float Y = 0.0f;

        public float Z = 0.0f;

        public bool isHunt = false;

        public bool isCaution = false;

        public bool isDanger = false;

        public bool isAlert = false;

        public string Name = "";

        public string Desc = "";

        public ListViewItem listitem = null;

        public int gone = 0;

        public bool filtered = false;

    }

    #endregion

    #region ListItem class

    public class ListItem

    {

        public string ActorDef = "";

        public int ID = -1;

        public string Name = "";

    }

    #endregion

    #region IniFile class

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

            WritePrivateProfileString(Section, Key, Value, this.path);

        }

        

        public string ReadValue(string Section, string Key, string Default) {

            StringBuilder buffer = new StringBuilder(255);

            GetPrivateProfileString(Section, Key, Default, buffer, 255, this.path);



            return buffer.ToString();

        }

    }

    #endregion



    #region RegexHelper

    /// <summary>

    /// This class contains a limited set of pre-compiled Regex expressions

    /// to be used for various filtering capabilities (until such time as we

    /// get enough information from the server not to need them anymore)

    /// 

    /// The need for it's own class is because we're re-using the same expressions

    /// over and over and it's not efficient to re-instantiate them for every

    /// spawn packet.

    /// </summary>

    public class RegexHelper

    {

        public bool IsMount(string mobName)

        {

            if (mobName.IndexOf("s_Mount") > 0)

                return true;

            return false;

        }

        public bool IsFamiliar(string mobName)

        {

            if (mobName.IndexOf("`s_fami") > 0)

                return true;

            return false;

        }

        public bool IsMerc(string mobName)

        {

            if (mobName.IndexOf("'s Merc") > 0)

                return true;

            return false;

        }

    }

    #endregion

}

