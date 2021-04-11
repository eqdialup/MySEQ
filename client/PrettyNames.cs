using System.Windows.Forms;
using myseq;

namespace Structures
{
    public static class PrettyNames
    {
        private static readonly string[] s_Spawntypes = new[]
                                        { "Player", "NPC", "Corpse", "Any", "Pet" };

        private static readonly string[] s_HideTypes = new[]
                                        { "Visible", "Invisible", "Hidden/Stealth", "Invis to Undead", "Invis to Animals" };

        private static string ArrayIndextoStr(string[] source, int index)
        {
            return index < source.GetLowerBound(0) || index > source.GetUpperBound(0) ? $"{index}: Unknown" : source[index];
        }

        public static string GetSpawnType(byte index)
        {
            return ArrayIndextoStr(s_Spawntypes, index);
        }

        public static string GetHideStatus(byte index)
        {
            return ArrayIndextoStr(s_HideTypes, index);
        }

        public static void LookupBoxMatch(Spawninfo si, MainForm f1)
        {
            si.isLookup = false;
            BoxMatch(f1.toolStripLookupBox, si);
            BoxMatch(f1.toolStripLookupBox1, si);
            BoxMatch(f1.toolStripLookupBox2, si);
            BoxMatch(f1.toolStripLookupBox3, si);
            BoxMatch(f1.toolStripLookupBox4, si);
        }

        private static void BoxMatch(ToolStripTextBox boxtext, Spawninfo si)
        {
            if (boxtext.Text.Length > 1
                && boxtext.Text != "Mob Search"
                && RegexHelper.GetRegex(boxtext.Text).Match(si.Name).Success)
            {
                si.isLookup = true;
            }
        }
    }
}