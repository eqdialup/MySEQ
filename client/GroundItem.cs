using myseq;
using System.Windows.Forms;

namespace Structures
{
    #region GroundItem class

    public class GroundItem
    {
        public Point3D ItemLocation { get; set; }

        public bool IsHunt;

        public bool IsCaution;

        public bool IsDanger;

        public bool IsAlert;

        public string Name { get; set; }

        public string Desc { get; set; }

        public ListViewItem Listitem { get; set; }

        public bool ShouldBeDeleted { get; set; }

        public bool Filtered { get; set; }

        public GroundItem(Spawninfo si)
        {
            ItemLocation = new Point3D(si.X, si.Y, si.Z);
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