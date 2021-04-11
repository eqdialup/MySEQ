using System;
using Structures;

namespace myseq
{
    public interface IMarkLookup
    {
        void MarkLookups(string name, bool filterMob = false);
    }

    public class MarkLookup : IMarkLookup
    {
        private EQData eq;
        private const string dflt = "Mob Search";
        private string search0 = "";
        private string search1 = "";
        private string search2 = "";
        private string search3 = "";
        private string search4 = "";

        private bool filter0;
        private bool filter1;
        private bool filter2;
        private bool filter3;
        private bool filter4;

        public void SetComponents(EQData eq)
        {
            this.eq = eq;
        }

        public void MarkLookups(string name, bool filterMob = false)
        {
            name = GetName(name);
            GetCheckNameLength(name, filterMob, ref search0, ref filter0, "1:");
            GetCheckNameLength(name, filterMob, ref search1, ref filter1, "2");
            GetCheckNameLength(name, filterMob, ref search2, ref filter2, "3");
            GetCheckNameLength(name, filterMob, ref search3, ref filter3, "4");
            GetCheckNameLength(name, filterMob, ref search4, ref filter4, "5");

            foreach (Spawninfo sp in eq.GetMobsReadonly().Values)
            {
                sp.isLookup = false;
                sp.lookupNumber = "";
                GetSubLookup(sp, search0, filter0, "1");
                GetSubLookup(sp, search1, filter1, "2");
                GetSubLookup(sp, search2, filter2, "3");
                GetSubLookup(sp, search3, filter3, "4");
                GetSubLookup(sp, search4, filter4, "5");
            }
        }

        private static string GetName(string name)
        {
            if (name.Length > 2 && name.Substring(2) == dflt)
            { name = name.Substring(0, 2); }

            return name;
        }

        private void GetCheckNameLength(string name, bool filterMob, ref string search, ref bool filter, string rank)
        {
            if (name.Substring(0, 2) == rank)
            {
                CheckNameLength(name, ref search, filterMob, ref filter);
            }
        }

        private void GetSubLookup(Spawninfo sp, string search, bool filter, string rank)
        {
            if (search.Length > 1)
            {
                SubLookup(sp, search, filter, rank);
            }
        }

        private void CheckNameLength(string name, ref string search, bool filterMob, ref bool filter)
        {
            if (name.Length > 2)
            {
                search = name.Substring(2);
                filter = filterMob;
            }
            else
            {
                search = "";
                filter = false;
            }
        }

        private void SubLookup(Spawninfo sp, string search, bool filter, string ln)
        {
            var levelCheck = false;
            if (search.Length > 2 && string.Equals(search.Substring(0, 2), "L:", StringComparison.OrdinalIgnoreCase))
            {
                int.TryParse(search.Substring(2), out var searchLevel);
                levelCheck = LevelCheck(sp, levelCheck, searchLevel);
            }
            if (levelCheck || RegexHelper.GetRegex(search).Match(sp.Name).Success)
            {
                sp.isLookup = true;
                sp.lookupNumber = ln;
                sp.hidden = false;
                FilterHidden(sp, filter);
            }
        }

        private static bool LevelCheck(Spawninfo sp, bool levelCheck, int searchLevel)
        {
            if (searchLevel != 0 && (sp.Level == searchLevel))
            {
                levelCheck = true;
            }

            return levelCheck;
        }

        private static void FilterHidden(Spawninfo sp, bool filter)
        {
            if (filter) { sp.hidden = true; }
        }
    }
}