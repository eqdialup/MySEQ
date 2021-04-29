//using System.Collections.Generic;
//using System.Threading;
//using myseq.Properties;
//using Structures;

//namespace myseq
//{
//    public class AlertStatus
//    {
//        private bool AffixStars = true;
//        private bool CorpseAlerts = true;

//        private string HuntPrefix = "";
//        private string AlertPrefix = "";
//        private string DangerPrefix = "";
//        private string CautionPrefix = "";

//        private bool FullTxtA;

//        private bool FullTxtC;

//        private bool FullTxtD;

//        private bool FullTxtH;

//        private bool Prefix = true;

//        private static void AudioMatch(string mobname, string TalkDescr, bool TalkOnMatch, bool PlayOnMatch, bool BeepOnMatch, string AudioFile)
//        {
//            if (TalkOnMatch)
//            {
//                ThreadStart threadDelegate = new ThreadStart(new Talker
//                {
//                    SpeakingText = $"{TalkDescr}, {RegexHelper.SearchName(mobname)}, is up."
//                }.SpeakText);

//                new Thread(threadDelegate).Start();
//            }
//            else if (PlayOnMatch)
//            {
//                SAudio.Play(AudioFile.Replace("\\", "\\\\"));
//            }
//            else if (BeepOnMatch)
//            {
//                SafeNativeMethods.Beep(300, 100);
//            }
//        }

//        private bool FindMatches(List<string> filterlist, string mobname, bool MatchFullText)
//        {
//            foreach (string str in filterlist)
//            {
//                var matched = false;

//                // if "match full text" is ON...

//                if (MatchFullText)
//                {
//                    if (string.Compare("name:"+mobname, str, true) == 0)
//                    {
//                        matched = true;
//                    }
//                }
//                else if (RegexHelper.IsSubstring(mobname, str))
//                {
//                    matched = true;
//                }
//                // if item has been matched...

//                if (matched)
//                {
//                    return true;
//                }
//            }

//            return false;
//        }

//        private string PrefixAffixLabel(string mname, string prefix)
//        {
//            if (Prefix)
//            {
//                mname = $"{prefix} {mname}";
//            }

//            if (AffixStars)
//            {
//                mname += $" {prefix}";
//            }

//            return mname;
//        }

//        public void AssignAlertStatus(Filters filters, Spawninfo si, string matchmobname, ref bool alert, ref string mobnameWithInfo)
//        {
//            if ((!si.isCorpse || CorpseAlerts) && !alert)
//            {
//                if (FindMatches(filters.Hunt, matchmobname, FullTxtH) || FindMatches(filters.GlobalHunt, matchmobname, FullTxtH))
//                {
//                    alert = true;
//                    si.isHunt = true;
//                    mobnameWithInfo = PrefixAffixLabel(mobnameWithInfo, HuntPrefix);
//                }

//                // [caution]
//                if (FindMatches(filters.Caution, matchmobname, FullTxtC) || FindMatches(filters.GlobalCaution, matchmobname, FullTxtC))
//                {
//                    alert = true;
//                    si.isCaution = true;
//                    mobnameWithInfo = PrefixAffixLabel(mobnameWithInfo, CautionPrefix);
//                }

//                // [danger]
//                if (((!si.isCorpse || CorpseAlerts) && FindMatches(filters.Danger, matchmobname, FullTxtD)) || FindMatches(filters.GlobalDanger, matchmobname, FullTxtD))
//                {
//                    alert = true;
//                    si.isDanger = true;
//                    mobnameWithInfo = PrefixAffixLabel(mobnameWithInfo, DangerPrefix);
//                }

//                // [rare]
//                if (FindMatches(filters.Alert, matchmobname, FullTxtA) || FindMatches(filters.GlobalAlert, matchmobname, FullTxtA))
//                {
//                    alert = true;
//                    si.isAlert = true;
//                    mobnameWithInfo = PrefixAffixLabel(mobnameWithInfo, AlertPrefix);
//                }
//                // [Email]
//                //if (filters.EmailAlert.Count > 0 && !si.isCorpse && FindMatches(filters.EmailAlert, matchmobname, true))
//                //{
//                //    // Flag on map as an alert mob
//                //    si.isAlert = true;
//                //}

//                // [Wielded Items]
//                // Acts like a hunt mob.
//                if (FindMatches(filters.WieldedItems, si.PrimaryName, FullTxtH) || FindMatches(filters.WieldedItems, si.OffhandName, FullTxtH))
//                {
//                    si.isHunt = true;
//                    mobnameWithInfo = PrefixAffixLabel(mobnameWithInfo, HuntPrefix);
//                }
//            }
//        }

//        public void CheckGrounditemForAlerts(Filters filters, GroundItem gi, string itemname)
//        {
//            // [hunt]
//            if (FindMatches(filters.Hunt, itemname, FullTxtH) || FindMatches(filters.GlobalHunt, itemname, FullTxtH))
//            {
//                gi.IsHunt = true;
//            }

//            // [caution]
//            if (FindMatches(filters.Caution, itemname, FullTxtC) || FindMatches(filters.GlobalCaution, itemname, FullTxtC))
//            {
//                gi.IsCaution = true;
//            }

//            // [danger]
//            if (FindMatches(filters.Danger, itemname, FullTxtD) || FindMatches(filters.GlobalDanger, itemname, FullTxtD))
//            {
//                gi.IsDanger = true;
//            }

//            // [rare]
//            if (FindMatches(filters.Alert, itemname, FullTxtA) || FindMatches(filters.GlobalAlert, itemname, FullTxtA))
//            {
//                gi.IsAlert = true;
//            }
//        }

//        public void LoadSpawnInfo()
//        {
//            // Used to improve packet processing speed

//            Prefix = Settings.Default.PrefixStars;

//            AffixStars = Settings.Default.AffixStars;

//            CorpseAlerts = Settings.Default.CorpseAlerts;

//            FullTxtH = Settings.Default.MatchFullTextH;

//            FullTxtC = Settings.Default.MatchFullTextC;

//            FullTxtD = Settings.Default.MatchFullTextD;

//            FullTxtA = Settings.Default.MatchFullTextA;

//            HuntPrefix = Settings.Default.HuntPrefix;

//            CautionPrefix = Settings.Default.CautionPrefix;

//            DangerPrefix = Settings.Default.DangerPrefix;

//            AlertPrefix = Settings.Default.AlertPrefix;
//        }

//        public void PlayAudioMatch(Spawninfo si, string matchmobname)
//        {
//            if (Settings.Default.playAlerts)
//            {
//                if (si.isHunt && !Settings.Default.NoneOnHunt)
//                {
//                    AudioMatch(matchmobname, "Hunt Mob", Settings.Default.TalkOnHunt, Settings.Default.PlayOnHunt, Settings.Default.BeepOnHunt, Settings.Default.HuntAudioFile);
//                }
//                if (si.isCaution && !Settings.Default.NoneOnCaution)
//                {
//                    AudioMatch(matchmobname, "Caution Mob", Settings.Default.TalkOnCaution, Settings.Default.PlayOnCaution, Settings.Default.BeepOnCaution, Settings.Default.CautionAudioFile);
//                }
//                if (si.isDanger && !Settings.Default.NoneOnDanger)
//                {
//                    AudioMatch(matchmobname, "Danger Mob", Settings.Default.TalkOnDanger, Settings.Default.PlayOnDanger, Settings.Default.BeepOnDanger, Settings.Default.DangerAudioFile);
//                }
//                if (si.isAlert && !Settings.Default.NoneOnAlert)
//                {
//                    AudioMatch(matchmobname, "Rare Mob", Settings.Default.TalkOnAlert, Settings.Default.PlayOnAlert, Settings.Default.BeepOnAlert, Settings.Default.AlertAudioFile);
//                }
//            }
//        }
//    }
//}
