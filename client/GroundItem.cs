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

        public bool ShouldBeDeleted { get; set; }

        public bool Filtered { get; set; }

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