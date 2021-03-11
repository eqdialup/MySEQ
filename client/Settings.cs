using System;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Soap;
using System.Windows.Forms;
using System.Xml;

namespace Structures

{
    public enum DrawOptions
    {
        DrawMap = 0x00000001,       // Do we want to draw the map?

        Readjust = 0x00000002,       // Readjust?

        Player = 0x00000004,       // Draw the player?

        SpotLine = 0x00000008,       // Draw the shift-click line?

        Spawns = 0x00000010,       // Draw all spawns?

        SpawnTrails = 0x00000020,       // Draw the mob trails?

        GroundItems = 0x00000040,       // Draw the ground items?

        SpawnTimers = 0x00000080,       // Draw the spawn timers?

        DirectionLines = 0x00000100,       // Draw Direction lines (direction lines and such)

        SpawnRings = 0x00000200,       // Draw Shopkeeper(etc) rings around mobs

        GridLines = 0x00000400,       // Draw grid lines

        ZoneText = 0x00000800,       // Draw zone text

        //

        DrawAll = 0x0fffffff,

        DrawNormal = DrawMap           // Standard, nice, savings

                              + Readjust

                              + Player

                              + SpotLine

                              + Spawns

                              + GroundItems

                              + SpawnTimers

                              + DirectionLines

                              + GridLines

                              + ZoneText,

        DrawLess = DrawNormal        // Takes away more uncritically things

                              - DirectionLines

                              - GroundItems

                              - SpawnTimers

                              - ZoneText,

        DrawEvenLess = DrawLess          // Takes away substantially

                              - Readjust,

        DrawNone = 0x00000000,       // Maximum savings - all enabled
    };

    #region Settings class

    // Settings singleton

    [Serializable]
    public sealed class Settings

    {
        private volatile static Settings instance;

        private static readonly object syncObj = new object();

        public int c1w = 125;

        public int c2w = 40;

        public int c3w = 50;

        public int c4w = 50;

        public int c5w = 75;

        public int c6w = 50;

        public int c7w = 50;

        public int c8w = 75;

        public int c9w = 50;

        public int c10w = 100;

        public int c11w = 50;

        public int c12w = 50;

        public int c13w = 50;

        public int c14w = 50;

        public int c15w = 50;

        public DrawOptions DrawOptions = DrawOptions.DrawNormal;

        public bool saveSpawnTimers = true;

        // need to remember where prefs file was opened

        private string prefsDir = "";

        public bool KeepCentered { get; set; }

        public bool AutoExpand { get; set; } = true;

        public bool AutoSelectSpawnList { get; set; } = true;

        public bool ShowPlayers { get; set; } = true;

        public bool ShowNPCs { get; set; } = true;

        public bool ShowLookupText { get; set; }

        public bool ShowLookupNumber { get; set; } = true;

        public bool AlwaysOnTop { get; set; }

        public bool ShowCorpses { get; set; } = true;

        public bool ShowPCCorpses { get; set; } = true;

        public bool ShowMyCorpse { get; set; } = true;

        public bool ShowInvis { get; set; } = true;

        public bool ShowMounts { get; set; } = true;

        public bool ShowFamiliars { get; set; } = true;

        public bool ShowPets { get; set; } = true;

        public bool AutoSelectEQTarget { get; set; } = true;

        public bool ForceDistinct { get; set; }

        public bool ForceDistinctText { get; set; }

        public bool UseDynamicAlpha { get; set; } = true;

        public int FadedLines { get; set; } = 10;

        public bool FilterGroundItems { get; set; } = true;

        public bool FilterNPCs { get; set; } = true;

        public bool FilterMapLines { get; set; } = true;

        public bool FilterMapText { get; set; } = true;

        public bool FilterPlayers { get; set; } = true;

        public bool FilterSpawnPoints { get; set; } = true;

        public bool FilterPlayerCorpses { get; set; } = true;

        public bool FilterNPCCorpses { get; set; } = true;

        public bool ShowTargetInfo { get; set; } = true;

        public bool SmallTargetInfo { get; set; } = true;

        public bool ShowZoneName { get; set; } = true;

        public bool ShowCharName { get; set; } = true;

        public bool ShowPVP { get; set; }

        public bool ShowPVPLevel { get; set; }

        public bool ShowNPCLevels { get; set; }

        public bool SoDCon { get; set; }

        public bool SoFCon { get; set; }

        public bool DefaultCon { get; set; } = true;

        public bool DrawFoV { get; set; } = true;

        public bool ColorRangeCircle { get; set; }

        public Color BackColor { get; set; } = Color.Black;

        public string HatchIndex { get; set; } = "Percent05";

        public string AlertSound { get; set; } = "Beep";

        public Color GridColor { get; set; } = Color.DarkGreen;

        public Color GridLabelColor { get; set; } = Color.Yellow;

        public Color RangeCircleColor { get; set; } = Color.DarkGray;

        public Color ListBackColor { get; set; } = Color.White;

        public Color PCBorderColor { get; set; } = Color.Fuchsia;

        public myseq.FollowOption FollowOption { get; set; } = myseq.FollowOption.Player;

        public bool ShowMobList { get; set; } = true;

        public bool ShowMobListTimer { get; set; } = true;

        public bool ShowGroundItemList { get; set; } = true;

        public bool ShowLayer1 { get; set; } = true;
        public bool ShowLayer2 { get; set; } = true;

        public bool ShowLayer3 { get; set; } = true;

        public bool NoneOnHunt { get; set; } = true;

        public bool NoneOnCaution { get; set; } = true;

        public bool NoneOnDanger { get; set; } = true;

        public bool NoneOnAlert { get; set; } = true;

        public bool BeepOnHunt { get; set; }

        public bool BeepOnCaution { get; set; }

        public bool BeepOnDanger { get; set; }

        public bool BeepOnAlert { get; set; }

        public bool PlayOnHunt { get; set; }

        public bool PlayOnCaution { get; set; }

        public bool PlayOnDanger { get; set; }

        public bool PlayOnAlert { get; set; }

        public bool TalkOnHunt { get; set; }

        public bool TalkOnCaution { get; set; }

        public bool TalkOnDanger { get; set; }

        public bool TalkOnAlert { get; set; }

        public bool EmailAlerts { get; set; }

        public string HuntAudioFile { get; set; } = "";

        public string CautionAudioFile { get; set; } = "";

        public string DangerAudioFile { get; set; } = "";

        public string AlertAudioFile { get; set; } = "";

        public int SpawnDrawSize { get; set; } = 3;

        public int PVPLevels { get; set; } = 10;

        public int MinAlertLevel { get; set; } = -1;

        public string TitleBar { get; set; } = "";

        public string SearchString { get; set; } = "http://everquest.allakhazam.com/search.html?q={0}";

        public bool CollectMobTrails { get; set; }

        public float MapLabelFontSize { get; set; } = 6.75f;

        public string MapLabelFontName { get; set; } = "Microsoft Sans Serif";

        public FontStyle MapLabelFontStyle { get; set; }

        public float ListFontSize { get; set; } = 6.75f;

        public string ListFontName { get; set; } = "Microsoft Sans Serif";

        public FontStyle ListFontStyle { get; set; }

        public float TargetInfoFontSize { get; set; } = 6.75f;

        public string TargetInfoFontName { get; set; } = "Microsoft Sans Serif";

        public FontStyle TargetInfoFontStyle { get; set; }

        public bool DepthFilter { get; set; }

        public int UpdateDelay { get; set; } = 200;

        public bool MatchFullTextH { get; set; }

        public bool MatchFullTextC { get; set; }

        public bool MatchFullTextD { get; set; }

        public bool MatchFullTextA { get; set; }

        public string HuntPrefix { get; set; } = " [H]";

        public string CautionPrefix { get; set; } = " [C]";

        public string DangerPrefix { get; set; } = " [D]";

        public string AlertPrefix { get; set; } = " [R]";

        public bool SaveOnExit { get; set; } = true;

        public bool ShowMenuBar { get; set; } = true;

        public bool ShowStatusBar { get; set; } = true;

        public bool ShowToolBar { get; set; } = true;

        public bool SaveSpawnLogs { get; set; }

        public bool SpawnCountdown { get; set; }

        public bool ShowNPCNames { get; set; }

        public bool ShowPCNames { get; set; }

        public bool ShowPCGuild { get; set; }

        public bool ShowNPCCorpseNames { get; set; }

        public bool ShowPlayerCorpseNames { get; set; }

        public int SplitterLoc { get; set; } = 256;

        public FormWindowState WindowState { get; set; }

        public Size WindowsSize { get; set; } = new Size(800, 600);

        public Point WindowsLocation { get; set; } = new Point(20, 20);

        public Size OptionsWindowsSize { get; set; } = new Size(296, 480);

        public Point OptionsWindowsLocation { get; set; } = new Point(20, 20);

        public FormStartPosition WindowsPosition { get; set; }

        public bool PrefixStars { get; set; } = true;

        public bool AffixStars { get; set; } = true;

        public bool CorpseAlerts { get; set; } = true;

        public int LevelOverride { get; set; } = -1;

        public int RangeCircle { get; set; } = 150;

        public string IPAddress1 { get; set; } = "127.0.0.1";

        public string IPAddress2 { get; set; } = "";

        public string IPAddress3 { get; set; } = "";

        public string IPAddress4 { get; set; } = "";

        public string IPAddress5 { get; set; } = "";

        public int CurrentIPAddress { get; set; }

        public int Port { get; set; } = 5555;

        public bool AutoConnect { get; set; } = true;

        public int GridInterval { get; set; } = 500;

        public bool ShowListGridLines { get; set; }

        public bool ShowListSearchBox { get; set; }

        public string MapDir { get; set; } = "";

        public string FilterDir { get; set; } = "";

        public string CfgDir { get; set; } = "";

        public string LogDir { get; set; } = "";

        public string TimerDir { get; set; } = "";

        public LogLevel MaxLogLevel { get; set; }

        public Color SelectedAddMapText { get; set; } = Color.White;

        public static Settings Instance
        {
            get
            {
                // only create a new instance if one doesn't already exist.

                if (instance == null)
                {
                    // use this lock to ensure that only one thread is access this block of code at once.

                    lock (syncObj)
                    {
                        if (instance == null)
                            instance = new Settings();
                    }
                }

                // return instance where it was just created or already existed.

                return instance;
            }

            set { instance = value; }
        }

        public void Save(string filename)
        {
            if (prefsDir?.Length == 0)
            {
                prefsDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MySEQ");
            }

            FileStream fs = new FileStream(filename, FileMode.Create);

            SoapFormatter sf1 = new SoapFormatter();

            sf1.Serialize(fs, Instance);

            fs.Close();

            string oldconfigFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "prefs.xml");
            if (File.Exists(oldconfigFile))
                File.Delete(oldconfigFile);
        }

        public void Load(string filename)
        {
            FileStream fs = null;
            bool otherLoad = false;
            try
            {
                fs = new FileStream(filename, FileMode.Open);

                SoapFormatter sf1 = new SoapFormatter();

                Instance = (Settings)sf1.Deserialize(fs);
            }
            catch (Exception)
            {
                //                LogLib.WriteLine("Error in Settings.Load(): ", ex);
                otherLoad = true;
            }

            Instance.prefsDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MySEQ");

            fs?.Close();

            if (otherLoad)
            {
                // Try manually loading directly
                XmlTextReader reader = new XmlTextReader(url: filename);
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        switch (reader.Name)
                        {
                            case "ipAddress1":
                                IPAddress1 = reader.ReadElementContentAsString();
                                break;

                            case "ipAddress2":
                                IPAddress2 = reader.ReadElementContentAsString();
                                break;

                            case "ipAddress3":
                                IPAddress3 = reader.ReadElementContentAsString();
                                break;

                            case "ipAddress4":
                                IPAddress4 = reader.ReadElementContentAsString();
                                break;

                            case "ipAddress5":
                                IPAddress5 = reader.ReadElementContentAsString();
                                break;

                            case "currentIPAddress":
                                CurrentIPAddress = reader.ReadElementContentAsInt();
                                break;

                            case "port":
                                Port = reader.ReadElementContentAsInt();
                                break;

                            case "autoConnect":
                                AutoConnect = reader.ReadElementContentAsBoolean();
                                break;

                            case "rangeCircle":
                                RangeCircle = reader.ReadElementContentAsInt();
                                break;

                            case "updateDelay":
                                UpdateDelay = reader.ReadElementContentAsInt();
                                break;

                            case "showPCNames":
                                ShowPCNames = reader.ReadElementContentAsBoolean();
                                break;

                            case "showNPCNames":
                                ShowNPCNames = reader.ReadElementContentAsBoolean();
                                break;

                            case "showNPCCorpseNames":
                                ShowNPCCorpseNames = reader.ReadElementContentAsBoolean();
                                break;

                            case "showPlayerCorpseNames":
                                ShowPlayerCorpseNames = reader.ReadElementContentAsBoolean();
                                break;

                            case "showLayer1":
                                ShowLayer1 = reader.ReadElementContentAsBoolean();
                                break;

                            case "showLayer2":
                                ShowLayer2 = reader.ReadElementContentAsBoolean();
                                break;

                            case "showLayer3":
                                ShowLayer3 = reader.ReadElementContentAsBoolean();
                                break;

                            case "gridInterval":
                                GridInterval = reader.ReadElementContentAsInt();
                                break;

                            case "SpawnSize":
                                SpawnDrawSize = reader.ReadElementContentAsInt();
                                break;

                            case "colorRangeCircle":
                                ColorRangeCircle = reader.ReadElementContentAsBoolean();
                                break;

                            case "drawFoV":
                                DrawFoV = reader.ReadElementContentAsBoolean();
                                break;

                            case "showZoneName":
                                ShowZoneName = reader.ReadElementContentAsBoolean();
                                break;

                            case "showCharName":
                                ShowCharName = reader.ReadElementContentAsBoolean();
                                break;

                            case "sodCon":
                                SoDCon = reader.ReadElementContentAsBoolean();
                                break;

                            case "sofCon":
                                SoFCon = reader.ReadElementContentAsBoolean();
                                break;

                            case "defaultCon":
                                DefaultCon = reader.ReadElementContentAsBoolean();
                                break;

                            case "showTargetInfo":
                                ShowTargetInfo = reader.ReadElementContentAsBoolean();
                                break;

                            case "smallTargetInfo":
                                SmallTargetInfo = reader.ReadElementContentAsBoolean();
                                break;

                            case "autoSelectEQTarget":
                                AutoSelectEQTarget = reader.ReadElementContentAsBoolean();
                                break;

                            case "mapDir":
                                MapDir = reader.ReadElementContentAsString();
                                break;

                            case "FilterDir":
                                FilterDir = reader.ReadElementContentAsString();
                                break;

                            case "cfgDir":
                                CfgDir = reader.ReadElementContentAsString();
                                break;

                            case "logDir":
                                LogDir = reader.ReadElementContentAsString();
                                break;

                            case "timerDir":
                                TimerDir = reader.ReadElementContentAsString();
                                break;

                            case "showPlayers":
                                ShowPlayers = reader.ReadElementContentAsBoolean();
                                break;

                            case "showNPCs":
                                ShowNPCs = reader.ReadElementContentAsBoolean();
                                break;

                            case "showCorpses":
                                ShowCorpses = reader.ReadElementContentAsBoolean();
                                break;

                            case "showPCCorpses":
                                ShowPCCorpses = reader.ReadElementContentAsBoolean();
                                break;

                            case "showMyCorpse":
                                ShowMyCorpse = reader.ReadElementContentAsBoolean();
                                break;

                            case "showInvis":
                                ShowInvis = reader.ReadElementContentAsBoolean();
                                break;

                            case "showMounts":
                                ShowMounts = reader.ReadElementContentAsBoolean();
                                break;

                            case "showFamiliars":
                                ShowFamiliars = reader.ReadElementContentAsBoolean();
                                break;

                            case "showPets":
                                ShowPets = reader.ReadElementContentAsBoolean();
                                break;

                            case "showPVP":
                                ShowPVP = reader.ReadElementContentAsBoolean();
                                break;

                            case "showPVPLevel":
                                ShowPVPLevel = reader.ReadElementContentAsBoolean();
                                break;

                            case "showNPCLevels":
                                ShowNPCLevels = reader.ReadElementContentAsBoolean();
                                break;

                            case "showLookupText":
                                ShowLookupText = reader.ReadElementContentAsBoolean();
                                break;

                            case "showLookupNumber":
                                ShowLookupNumber = reader.ReadElementContentAsBoolean();
                                break;

                            case "autoSelectSpawnList":
                                AutoSelectSpawnList = reader.ReadElementContentAsBoolean();
                                break;

                            case "keepCentered":
                                KeepCentered = reader.ReadElementContentAsBoolean();
                                break;

                            case "autoExpand":
                                AutoExpand = reader.ReadElementContentAsBoolean();
                                break;

                            case "saveSpawnLogs":
                                SaveSpawnLogs = reader.ReadElementContentAsBoolean();
                                break;

                            case "spawnCountdown":
                                SpawnCountdown = reader.ReadElementContentAsBoolean();
                                break;

                            case "forceDistinct":
                                ForceDistinct = reader.ReadElementContentAsBoolean();
                                break;

                            case "forceDistinctText":
                                ForceDistinctText = reader.ReadElementContentAsBoolean();
                                break;

                            case "useDynamicAlpha":
                                UseDynamicAlpha = reader.ReadElementContentAsBoolean();
                                break;

                            case "fadedLines":
                                FadedLines = reader.ReadElementContentAsInt();
                                break;

                            case "filterGroundItems":
                                FilterGroundItems = reader.ReadElementContentAsBoolean();
                                break;

                            case "filterNPCs":
                                FilterNPCs = reader.ReadElementContentAsBoolean();
                                break;

                            case "filterPlayers":
                                FilterPlayers = reader.ReadElementContentAsBoolean();
                                break;

                            case "filterMapLines":
                                FilterMapLines = reader.ReadElementContentAsBoolean();
                                break;

                            case "filterMapText":
                                FilterMapText = reader.ReadElementContentAsBoolean();
                                break;

                            case "filterSpawnPoints":
                                FilterSpawnPoints = reader.ReadElementContentAsBoolean();
                                break;

                            case "filterNPCCorpses":
                                FilterNPCCorpses = reader.ReadElementContentAsBoolean();
                                break;

                            case "filterPlayerCorpses":
                                FilterPlayerCorpses = reader.ReadElementContentAsBoolean();
                                break;
                        }
                    }
                }
                reader.Close();
            }
        }
    }

    #endregion Settings class
}