using System.Windows.Forms;

namespace Structures

{
    #region GroundItem class

    public class GroundItem {
        public float X{ get; set; }

        public float Y{ get; set; }

        public float Z{ get; set; }

        public bool isHunt{ get; set; }

        public bool isCaution{ get; set; }

        public bool isDanger{ get; set; }

        public bool isAlert{ get; set; }

        public string Name{ get; set; }

        public string Desc{ get; set; }

        public ListViewItem listitem{ get; set; }

        public int gone{ get; set; }

        public bool filtered{ get; set; }
    }

    #endregion

    #region ListItem class

    public struct ListItem
    {
        public string ActorDef;
        public int ID {get; set; }
        public string Name { get; set; }
    }

#endregion
}