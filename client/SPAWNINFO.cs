using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Structures

{
    [StructLayout(LayoutKind.Sequential, Pack=1)]

    public partial class SPAWNINFO
    {
        public SPAWNINFO() {}

        private string BytesToString(byte []b, int start, int maxlen) {
            int i=0;

            // look for a null

            while (i <maxlen&&b[start+i]!='\0')
            {
                i++;
            }

            return Encoding.ASCII.GetString(b, start, i);
        }

        public void Frombytes(byte []b, int offset)

        {
            Name = BytesToString(b, 0 + offset, 30);

            Y = BitConverter.ToSingle(b, 30 + offset);

            X = BitConverter.ToSingle(b, 34 + offset);

            Z = BitConverter.ToSingle(b, 38 + offset);

            Heading = BitConverter.ToSingle(b, 42 + offset);

            SpeedRun = BitConverter.ToSingle(b, 46 + offset);

            SpawnID = BitConverter.ToInt32(b, 50 + offset);

            OwnerID = BitConverter.ToInt32(b, 54 + offset);

            Type = b[58 + offset];

            Class = b[59 + offset];

            Race = BitConverter.ToInt32(b, 60 + offset);

            Level = b[64 + offset];

            Hide = b[65 + offset];

            Primary = BitConverter.ToInt32(b, 66 + offset);
            Offhand = BitConverter.ToInt32(b, 70 + offset);

            Lastname = BytesToString(b, 74 + offset, 22);

            flags = (PacketType)BitConverter.ToInt32(b, 96 + offset);

            if (flags==PacketType.Player)
                m_isPlayer = true;

            Guild = BitConverter.ToInt32(b, 100 + offset);
        }

        public string Name = "";

        public float Y = 0;

        public float X = 0;

        public float Z = 0;

        public float Heading = 0;

        public float SpeedRun = 0;

        public int SpawnID = 0;

        public int OwnerID = 0;

        public byte Type = 0;

        public byte Class = 0;

        public int Race = 0;

        public int Primary = 0;

        public int Offhand = 0;

        public int Guild = 0;

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

        public string lookupNumber = "";

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
}

