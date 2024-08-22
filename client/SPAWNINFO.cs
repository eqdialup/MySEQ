using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using myseq.Properties;

namespace Structures

{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class Spawninfo
    {
        public string Name {get; set; } = "";
        public string Lastname {get; set; } = "";
         public int SpawnID {get; set; }

        public int OwnerID {get; set; }

        public float X {get; set; }

        public float Y {get; set; }

        public float Z {get; set; }

        public byte Class {get; set; }

        public byte Level {get; set; }
        public int Race {get; set; }

        public byte Type {get; set; }

        public float Heading {get; set; }

        public float SpeedRun {get; set; }

        public int Primary {get; set; }
        public int Offhand {get; set; }
        public string PrimaryName {get; set; }
        public string OffhandName {get; set; }

//        public int Guild {get; set; }

        public byte Hide {get; set; }

        public int gone {get; set; }

        public int refresh {get; set; }

        public PacketType flags {get; set; }

        public bool isHunt {get; set; }

        public bool isCaution {get; set; }

        public bool isDanger {get; set; }

        public bool isAlert {get; set; }

        public bool isPet {get; set; }

        public bool isMerc {get; set; }

        public bool isCorpse {get; set; }

        public bool isMount {get; set; }

        public bool isFamiliar {get; set; }

        public bool isLDONObject {get; private set; }

        public bool isEventController {get; private set; }

        public bool isLookup {get; set; }

        public string lookupNumber {get; set; } = "";

        public bool hidden {get; set; }

        public bool filtered {get; set; }

        public ListViewItem listitem {get; set; }

        public bool m_isPlayer {get; set; }

        public bool m_isMyCorpse {get; set; }

        public bool delFromList {get; set; }

        public bool proxAlert {get; set; }

        public bool alertMob {get; set; }

        public string SpawnLoc {get; set; } = "";

        public string ZoneSpawnLoc {get; set; } = "";

        public bool IsPlayer => m_isPlayer;

        public bool IsMyCorpse => m_isMyCorpse;

        public bool IsSpawnLDON(Spawninfo si)
        {
            if (si.Class == 62)
            {
                si.isLDONObject = true;
                return true;
            }
            return false;
        }
        public bool IsSpawnController(Spawninfo si)
        {
            if ((si.Race == 127) && si.Name.IndexOf("_") == 0) // Invisible Man Race
            {
                si.isEventController = true;
                
                if (!Settings.Default.ShowInvis)
                {
                    si.hidden = true;
                }
                return true;
            }
            return false;
        }

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

        public float SpawnDistance(Spawninfo si, Spawninfo gamerInfo)
        {
            float dx = si.X - gamerInfo.X;
            float dy = si.Y - gamerInfo.Y;
            float dz = si.Z - gamerInfo.Z;
            return (float)Math.Sqrt((dx * dx) + (dy * dy) + (dz * dz));
        }
    }
}