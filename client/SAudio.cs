using System.Threading;

namespace myseq
{
    //public class ListBoxComparerSpawnTimerList : IComparer

    //{
    //    public int Compare(object x, object y)

    //    {
    //        ListViewItem sa = (ListViewItem)x;

    //        ListViewItem sb = (ListViewItem)y;

    //        int res = 0;

    //        if (Column == 0 || Column == 3)    // Last Spawn = 0  // zone = 3
    //        {
    //            res = string.Compare(sa.Text, sb.Text);
    //        }
    //        else if (Column == 1 ||   // Countdown
    //           Column == 2 || // SpawnTimer
    //           Column == 7)   // SpawnCount

    //        {
    //            int ia = int.Parse(sa.SubItems[1].Text);

    //            int ib = int.Parse(sb.SubItems[1].Text);

    //            if (ia < ib) res = -1;
    //            else res = ia > ib ? 1 : 0;
    //        }
    //        else if (

    //            (Column == 4) ||   // X

    //            (Column == 5) ||   // Y

    //            (Column == 6)    // Z

    //            )

    //        {
    //            float fa = float.Parse(sa.SubItems[Column].Text);

    //            float fb = float.Parse(sb.SubItems[Column].Text);

    //            if (fa < fb) res = -1;
    //            else res = fa > fb ? 1 : 0;
    //        }
    //        else if (Column == 8 || // SpawnTime

    //            Column == 9 || // KillTime

    //            Column == 10) // NextSpawn

    //        {
    //            if (sa.SubItems[Column].Text.Length == 0)

    //            {
    //                res = 1;
    //            }
    //            else if (sb.SubItems[Column].Text.Length == 0)

    //            {
    //                res = -1;
    //            }
    //            else
    //            {
    //                DateTime dta = DateTime.Parse(sa.SubItems[Column].Text);

    //                DateTime dtb = DateTime.Parse(sb.SubItems[Column].Text);

    //                res = DateTime.Compare(dta, dtb);
    //            }
    //        }

    //        if (Descending) res = -res;

    //        return res;
    //    }

    //    public ListBoxComparerSpawnTimerList(ListView.ListViewItemCollection spawns, bool descending, int column)

    //    {
    //        Spawns = spawns;

    //        Descending = descending;

    //        Column = column;
    //    }

    //    private readonly ListView.ListViewItemCollection Spawns;

    //    private bool Descending;

    //    private int Column;
    //}

    //public class ListBoxComparerGroundItemsList : IComparer
    //{
    //    public int Compare(object x, object y)
    //    {
    //        var sa = (ListViewItem)x;

    //        var sb = (ListViewItem)y;

    //        var res = 0;

    //        if (Column == 0)    // Description
    //        {
    //            res = string.Compare(sa.Text, sb.Text);
    //        }
    //        else if ((Column == 1) ||   // ActorDef

    //           (Column == 2))    // Spawn Time
    //        {
    //            res = string.Compare(sa.SubItems[Column].Text, sb.SubItems[Column].Text);
    //        }
    //        else if ((Column == 3) ||   // X

    //           (Column == 4) ||   // Y

    //           (Column == 5)) // Z
    //        {
    //            float fa = float.Parse(sa.SubItems[Column].Text);

    //            float fb = float.Parse(sb.SubItems[Column].Text);

    //            if (fa < fb) res = -1;
    //            else res = fa > fb ? 1 : 0;
    //        }

    //        if (Descending) res = -res;

    //        return res;
    //    }

    //    public ListBoxComparerGroundItemsList(ListView.ListViewItemCollection spawns, bool descending, int column)
    //    {
    //        Spawns = spawns;

    //        Descending = descending;

    //        Column = column;
    //    }

    //    private ListView.ListViewItemCollection Spawns;

    //    private bool Descending;

    //    private int Column;
    //}

    public static class SAudio
    {
        private static string sndFile;

        private static void PlaySnd()
        {
            SafeNativeMethods.PlaySound(sndFile, 0, 0);
        }

        public static void Play(string fileName)
        {
            sndFile = fileName;

            ThreadStart entry = new ThreadStart(PlaySnd);

            Thread thrd = new Thread(entry);

            thrd.Start();
        }
    }
}
