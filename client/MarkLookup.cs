using Structures;
using System.Text.RegularExpressions;

namespace myseq
{
    public interface IMarkLookup
    {
        void MarkLookups(string name, bool filterMob = false);
    }

    public class MarkLookup : IMarkLookup
    {
        private EQData eq;
        private string search1 = "";
        private string search2 = "";
        private string search3 = "";
        private string search4 = "";
        private string search5 = "";
        private bool filter1 = false;
        private bool filter2 = false;
        private bool filter3 = false;
        private bool filter4 = false;
        private bool filter5 = false;

        public MarkLookup()
        { }

        public void SetComponents(EQData eq)
        {
            this.eq = eq;
        }

        public void MarkLookups(string name, bool filterMob = false)
        {
            // Trim the "Mob_Search" placeholder if it exists.
            if (name.Length > 2 && name.Substring(2) == "Mob_Search")
            {
                name = name.Substring(0, 2);
            }

            // Set search variables based on the prefix.
            SetSearchParameters(name, filterMob);

            // Loop through all spawns and update their properties based on the search criteria.
            foreach (Spawninfo sp in eq.GetMobsReadonly().Values)
            {
                UpdateSpawnLookup(sp);
            }
        }

        // Helper method to set search parameters.
        private void SetSearchParameters(string name, bool filterMob)
        {
            var prefix = name.Substring(0, 2);
            var searchText = name.Length > 2 ? name.Substring(2) : "";

            switch (prefix)
            {
                case "1:":
                    search1 = searchText;
                    filter1 = filterMob;
                    break;

                case "2:":
                    search2 = searchText;
                    filter2 = filterMob;
                    break;

                case "3:":
                    search3 = searchText;
                    filter3 = filterMob;
                    break;

                case "4:":
                    search4 = searchText;
                    filter4 = filterMob;
                    break;

                case "5:":
                    search5 = searchText;
                    filter5 = filterMob;
                    break;
            }
        }

        // Helper method to update the lookup properties of a spawn.
        private void UpdateSpawnLookup(Spawninfo sp)
        {
            // Reset the spawn properties.
            sp.isLookup = false;
            sp.lookupNumber = "";

            // Check each search parameter and update accordingly.
            var searches = new (string search, bool filter, string lookupNumber)[]
            {
        (search1, filter1, "1"),
        (search2, filter2, "2"),
        (search3, filter3, "3"),
        (search4, filter4, "4"),
        (search5, filter5, "5")
            };

            foreach (var (search, filter, lookupNumber) in searches)
            {
                if (string.IsNullOrEmpty(search))
                    continue;

                // Check for level-based search (e.g., "L:20").
                bool levelCheck = false;
                if (search.Length > 2 && search.Substring(0, 2).ToUpper() == "L:")
                {
                    int.TryParse(search.Substring(2), out int searchLevel);
                    if (searchLevel != 0 && sp.Level == searchLevel)
                    {
                        levelCheck = true;
                    }
                }

                // Create a regex to match the spawn's name.
                Regex regEx = new Regex(".*" + search + ".*", RegexOptions.IgnoreCase);

                // If level matches or the regex finds a match in the name, update the spawn properties.
                if (levelCheck || regEx.IsMatch(sp.Name))
                {
                    sp.isLookup = true;
                    sp.lookupNumber = lookupNumber;
                    sp.hidden = filter;
                    break;  // Exit the loop once a match is found to prevent overriding.
                }
            }
        }
    }
}