
using System.Windows.Forms;

namespace Structures

{
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
}