using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Structures

{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]

    public partial class SPAWNINFO
    {
        public SPAWNINFO() { }

        private string BytesToString(byte[] b, int start, int maxlen)
        {
            var i = 0;

            // look for a null

            while (i < maxlen && b[start + i] != '\0')
            {
                i++;
            }

            return Encoding.ASCII.GetString(b, start, i);
        }

        public void Frombytes(byte[] b, int offset)
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

            if (flags == PacketType.Player)
            {
                m_isPlayer = true;
            }

            //            Guild = BitConverter.ToInt32(b, 100 + offset);
        }

        public string Name = "";

        public float Y;

        public float X;

        public float Z;

        public float Heading;

        public float SpeedRun;

        public int SpawnID;

        public int OwnerID;

        public byte Type;

        public byte Class;

        public int Race;

        public int Primary;

        public int Offhand;

        //        public int Guild;

        public byte Level;

        public byte Hide;

        public string Lastname = "";

        public int gone;

        public int refresh;

        public PacketType flags;

        public bool isHunt;

        public bool isCaution;

        public bool isDanger;

        public bool isAlert;

        public bool isPet;

        public bool isMerc;

        public bool isCorpse;

        public bool isMount;

        public bool isFamiliar;

        public bool isLDONObject;

        public bool isEventController;

        public bool isLookup;

        public string lookupNumber = "";

        public bool hidden;

        public bool filtered;

        public ListViewItem listitem;

        public bool m_isPlayer;

        public bool m_isMyCorpse;

        public bool delFromList;

        public bool proxAlert;

        public bool alertMob;

        public string SpawnLoc = "";

        public string ZoneSpawnLoc = "";

        public bool IsPlayer => m_isPlayer;
    }
}

