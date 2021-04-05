using System.Collections;
using System.Windows.Forms;

namespace myseq
{
    public class ListComparer : IComparer
    {
        private readonly int col;

        public ListComparer()
        {
            col = 0;
        }

        public ListComparer(int column)
        {
            col = column;
        }

        public int Compare(object x, object y)
        {
            return string.Compare(((ListViewItem)x).SubItems[col].Text, ((ListViewItem)y).SubItems[col].Text);
        }

        //public int Compare(object x, object y)

        //{
        //    ListViewItem sa = (ListViewItem)x;

        //    ListViewItem sb = (ListViewItem)y;

        //    int res = 0;

        //    if (Column == 0)    // Name
        //    {
        //        res = string.Compare(sa.Text, sb.Text);
        //    }
        //    else if (Column == 1)   // Level

        //    {
        //        int ia = int.Parse(sa.SubItems[1].Text);

        //        int ib = int.Parse(sb.SubItems[1].Text);

        //        if (ia < ib) res = -1;
        //        else res = ia > ib ? 1 : 0;
        //    }
        //    else if ((Column == 2) ||   // Class

        //        (Column == 3) ||    // Primary

        //        (Column == 4) ||    // Offhand

        //        (Column == 5) ||    // Race

        //        (Column == 6) ||    // Owner

        //        (Column == 7) ||    // Last Name

        //        (Column == 8) ||    // Type

        //        (Column == 9) ||    // Invis

        //        (Column == 17))     // Guild

        //    {
        //        res = string.Compare(sa.SubItems[Column].Text, sb.SubItems[Column].Text);
        //    }
        //    else if ((Column == 10) ||   // Run Speed

        //       (Column == 13) ||   // X

        //       (Column == 14) ||   // Y

        //       (Column == 15) ||   // Z

        //       (Column == 16))     // Distance

        //    {
        //        float fa = float.Parse(sa.SubItems[Column].Text);

        //        float fb = float.Parse(sb.SubItems[Column].Text);

        //        if (fa < fb) res = -1;
        //        else res = fa > fb ? 1 : 0;
        //    }
        //    else if (Column == 11)

        //    { // SpawnID
        //        uint ia = uint.Parse(sa.SubItems[11].Text);

        //        uint ib = uint.Parse(sb.SubItems[11].Text);

        //        if (ia < ib) res = -1;
        //        else res = ia > ib ? 1 : 0;
        //    }
        //    else if (Column == 12)

        //    {
        //        DateTime dta = DateTime.Parse(sa.SubItems[12].Text);

        //        DateTime dtb = DateTime.Parse(sb.SubItems[12].Text);

        //        res = DateTime.Compare(dta, dtb);
        //    }

        //    if (Descending) res = -res;

        //    return res;
        //}

        //public ListBoxComparerSpawnList(ListView.ListViewItemCollection spawns, bool descending, int column)

        //{
        //    Spawns = spawns;

        //    Descending = descending;

        //    Column = column;
        //}

        //private ListView.ListViewItemCollection Spawns;

        //private bool Descending;

        //private int Column;
    }
}