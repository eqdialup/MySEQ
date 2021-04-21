using System.Collections.Generic;
using System.Threading;
using myseq.Properties;
using Structures;

namespace myseq
{
    public interface IAlertStatus
    {
        void AssignAlertStatus(Spawninfo si, string matchmobname, ref bool alert, ref string mobnameWithInfo);
        void CheckGrounditemForAlerts(Filters filters, GroundItem gi, string itemname);
        void LoadSpawnInfo();
        void PlayAudioMatch(Spawninfo si, string matchmobname);
    }

    public class AlertStatus : IAlertStatus
    {

        private bool AffixStars = true;

        private string AlertPrefix = "";

        private string CautionPrefix = "";

        private bool CorpseAlerts = true;

        private string DangerPrefix = "";

        private string HuntPrefix = "";

        private bool MatchFullTextA;

        private bool MatchFullTextC;

        private bool MatchFullTextD;

        private bool MatchFullTextH;

        private bool PrefixStars = true;
        private readonly Filters filters = new Filters();

        private static void AudioMatch(string mobname, string TalkDescr, bool TalkOnMatch, bool PlayOnMatch, bool BeepOnMatch, string AudioFile)
        {
            if (TalkOnMatch)
            {
                ThreadStart threadDelegate = new ThreadStart(new Talker
                {
                    SpeakingText = $"{TalkDescr}, {RegexHelper.SearchName(mobname)}, is up."
                }.SpeakText);

                new Thread(threadDelegate).Start();
            }
            else if (PlayOnMatch)
            {
                SAudio.Play(AudioFile.Replace("\\", "\\\\"));
            }
            else if (BeepOnMatch)
            {
                SafeNativeMethods.Beep(300, 100);
            }
        }

        private bool FindMatches(List<string> filterlist, string mobname, bool MatchFullText)
        {
            foreach (string str in filterlist)
            {
                var matched = false;

                // if "match full text" is ON...

                if (MatchFullText)
                {
                    if (string.Compare(mobname, str, true) == 0)
                    {
                        matched = true;
                    }
                }
                else if (RegexHelper.IsSubstring(mobname, str))
                {
                    matched = true;
                }
                // if item has been matched...

                if (matched)
                {
                    return true;
                }
            }

            return false;
        }

        private string PrefixAffixLabel(string mname, string prefix)
        {
            if (PrefixStars)
            {
                mname = $"{prefix} {mname}";
            }

            if (AffixStars)
            {
                mname += $" {prefix}";
            }

            return mname;
        }

        public void AssignAlertStatus(Spawninfo si, string matchmobname, ref bool alert, ref string mobnameWithInfo)
        {
            if ((!si.isCorpse || CorpseAlerts) && !alert)
            {
                if (FindMatches(filters.Hunt, matchmobname, MatchFullTextH) || FindMatches(filters.GlobalHunt, matchmobname, MatchFullTextH))
                {
                    alert = true;
                    si.isHunt = true;
                    mobnameWithInfo = PrefixAffixLabel(mobnameWithInfo, HuntPrefix);
                }

                // [caution]
                if (FindMatches(filters.Caution, matchmobname, MatchFullTextC) || FindMatches(filters.GlobalCaution, matchmobname, MatchFullTextC))
                {
                    alert = true;
                    si.isCaution = true;
                    mobnameWithInfo = PrefixAffixLabel(mobnameWithInfo, CautionPrefix);
                }

                // [danger]
                if (((!si.isCorpse || CorpseAlerts) && FindMatches(filters.Danger, matchmobname, MatchFullTextD)) || FindMatches(filters.GlobalDanger, matchmobname, MatchFullTextD))
                {
                    alert = true;
                    si.isDanger = true;
                    mobnameWithInfo = PrefixAffixLabel(mobnameWithInfo, DangerPrefix);
                }

                // [rare]
                if (FindMatches(filters.Alert, matchmobname, MatchFullTextA) || FindMatches(filters.GlobalAlert, matchmobname, MatchFullTextA))
                {
                    alert = true;
                    si.isAlert = true;
                    mobnameWithInfo = PrefixAffixLabel(mobnameWithInfo, AlertPrefix);
                }
                // [Email]
                //if (filters.EmailAlert.Count > 0 && !si.isCorpse && FindMatches(filters.EmailAlert, matchmobname, true))
                //{
                //    // Flag on map as an alert mob
                //    si.isAlert = true;
                //}

                // [Wielded Items]
                // Acts like a hunt mob.
                if (FindMatches(filters.WieldedItems, si.PrimaryName, MatchFullTextH) || FindMatches(filters.WieldedItems, si.OffhandName, MatchFullTextH))
                {
                    si.isHunt = true;
                    mobnameWithInfo = PrefixAffixLabel(mobnameWithInfo, HuntPrefix);
                }
            }
        }

        public void CheckGrounditemForAlerts(Filters filters, GroundItem gi, string itemname)
        {
            // [hunt]
            if (FindMatches(filters.Hunt, itemname, MatchFullTextH))
            {
                gi.isHunt = true;
            }

            if (FindMatches(filters.GlobalHunt, itemname, MatchFullTextH))
            {
                gi.isHunt = true;
            }

            // [caution]
            if (FindMatches(filters.Caution, itemname, MatchFullTextC))
            {
                gi.isCaution = true;
            }

            if (FindMatches(filters.GlobalCaution, itemname, MatchFullTextC))
            {
                gi.isCaution = true;
            }

            // [danger]
            if (FindMatches(filters.Danger, itemname, MatchFullTextD))
            {
                gi.isDanger = true;
            }

            if (FindMatches(filters.GlobalDanger, itemname, MatchFullTextD))
            {
                gi.isDanger = true;
            }

            // [rare]
            if (FindMatches(filters.Alert, itemname, MatchFullTextA))
            {
                gi.isAlert = true;
            }

            if (FindMatches(filters.GlobalAlert, itemname, MatchFullTextA))
            {
                gi.isAlert = true;
            }
        }

        public void LoadSpawnInfo()
        {
            // Used to improve packet processing speed

            PrefixStars = Settings.Default.PrefixStars;

            AffixStars = Settings.Default.AffixStars;

            CorpseAlerts = Settings.Default.CorpseAlerts;

            MatchFullTextH = Settings.Default.MatchFullTextH;

            MatchFullTextC = Settings.Default.MatchFullTextC;

            MatchFullTextD = Settings.Default.MatchFullTextD;

            MatchFullTextA = Settings.Default.MatchFullTextA;

            HuntPrefix = Settings.Default.HuntPrefix;

            CautionPrefix = Settings.Default.CautionPrefix;

            DangerPrefix = Settings.Default.DangerPrefix;

            AlertPrefix = Settings.Default.AlertPrefix;
        }

        public void PlayAudioMatch(Spawninfo si, string matchmobname)
        {
            if (Settings.Default.playAlerts)
            {
                if (si.isHunt && !Settings.Default.NoneOnHunt)
                {
                    AudioMatch(matchmobname, "Hunt Mob", Settings.Default.TalkOnHunt, Settings.Default.PlayOnHunt, Settings.Default.BeepOnHunt, Settings.Default.HuntAudioFile);
                }
                if (si.isCaution && !Settings.Default.NoneOnCaution)
                {
                    AudioMatch(matchmobname, "Caution Mob", Settings.Default.TalkOnCaution, Settings.Default.PlayOnCaution, Settings.Default.BeepOnCaution, Settings.Default.CautionAudioFile);
                }
                if (si.isDanger && !Settings.Default.NoneOnDanger)
                {
                    AudioMatch(matchmobname, "Danger Mob", Settings.Default.TalkOnDanger, Settings.Default.PlayOnDanger, Settings.Default.BeepOnDanger, Settings.Default.DangerAudioFile);
                }
                if (si.isAlert && !Settings.Default.NoneOnAlert)
                {
                    AudioMatch(matchmobname, "Rare Mob", Settings.Default.TalkOnAlert, Settings.Default.PlayOnAlert, Settings.Default.BeepOnAlert, Settings.Default.AlertAudioFile);
                }
            }
        }
    }
}
