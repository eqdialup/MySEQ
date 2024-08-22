using System.Windows.Forms;

namespace Structures
{
    #region GroundItem class

    public class GroundItem
    {
        public float X { get; set; }

        public float Y { get; set; }

        public float Z { get; set; }

        public bool IsHunt { get; set; }

        public bool IsCaution { get; set; }

        public bool IsDanger { get; set; }

        public bool IsAlert { get; set; }

        public string Name { get; set; }

        public string Desc { get; set; }

        public ListViewItem Listitem { get; set; }

        public int Gone { get; set; }

        public bool Filtered { get; set; }

        public GroundItem(float x, float y, float z, bool isHunt, bool isCaution, bool isDanger, bool isAlert, string name, ListViewItem listitem, int gone, bool filtered)
        {
            X = x;
            Y = y;
            Z = z;
            IsHunt = isHunt;
            IsCaution = isCaution;
            IsDanger = isDanger;
            IsAlert = isAlert;
            Name = name;
            Listitem = listitem;
            Gone = gone;
            Filtered = filtered;
        }

        public GroundItem(Spawninfo si)
        {
            X = si.X;
            Y = si.Y;
            Z = si.Z;
            Name = si.Name;
        }
    }

    #endregion GroundItem class

    public struct ListItem
    {
        public string ActorDef { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
    }
}