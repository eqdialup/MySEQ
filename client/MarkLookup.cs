using System;
using Structures;

namespace myseq
{
    public interface IMarkLookup
    {
        void MarkLookups(string name, bool filterMob = false);
    }

    class MarkLookup : IMarkLookup
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
            if (name.Length > 2 && name.Substring(2) == dflt) { name = name.Substring(0, 2); }
            if (name.Substring(0, 2) == "0:")
            {
                CheckNameLength(name, ref search0, filterMob, ref filter0);
            }
            else if (name.Substring(0, 2) == "1:")
            {
                CheckNameLength(name, ref search1, filterMob, ref filter1);
            }
            else if (name.Substring(0, 2) == "2:")
            {
                CheckNameLength(name, ref search2, filterMob, ref filter2);
            }
            else if (name.Substring(0, 2) == "3:")
            {
                CheckNameLength(name, ref search3, filterMob, ref filter3);
            }
            else if (name.Substring(0, 2) == "4:")
            {
                CheckNameLength(name, ref search4, filterMob, ref filter4);
            }

            foreach (Spawninfo sp in eq.GetMobsReadonly().Values)
            {
                sp.isLookup = false;
                sp.lookupNumber = "";
                if (search0.Length > 1)
                {
                    SubLookup(sp, search0, filter0, "1");
                }
                if (search1.Length > 1)
                {
                    SubLookup(sp, search1, filter1, "2");
                }
                if (search2.Length > 1)
                {
                    SubLookup(sp, search2, filter2, "3");
                }
                if (search3.Length > 1)
                {
                    SubLookup(sp, search3, filter3, "4");
                }
                if (search4.Length > 1)
                {
                    SubLookup(sp, search4, filter4, "5");
                }
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
                if (searchLevel != 0 && (sp.Level == searchLevel))
                {
                    levelCheck = true;
                }
            }
            if (levelCheck || RegexHelper.GetRegex(search).Match(sp.Name).Success)
            {
                sp.isLookup = true;
                sp.lookupNumber = ln;
                sp.hidden = false;
                if (filter) { sp.hidden = true; }
            }
        }
    }
}
