
using System.Windows.Forms;

namespace Structures

{
    #region GroundItem class

    public class GroundItem {
        public float X;

        public float Y;

        public float Z;

        public bool isHunt;

        public bool isCaution;

        public bool isDanger;

        public bool isAlert;

        public string Name = "";

        public string Desc = "";

        public ListViewItem listitem;

        public int gone;

        public bool filtered;
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
}