using System;

using System.IO;

using System.Xml;

using System.Text;

using System.Text.RegularExpressions;

using System.Drawing;

using System.Security;

using System.Security.Cryptography;

using System.Reflection;

using System.Drawing.Drawing2D;

using System.Windows.Forms;

using System.Runtime.InteropServices;

using System.Runtime.Serialization.Formatters.Soap;



namespace Structures

{

    public enum DrawOptions {

        DrawMap             = 0x00000001,       // Do we want to draw the map?

        Readjust            = 0x00000002,       // Readjust?

        Player              = 0x00000004,       // Draw the player?

        SpotLine            = 0x00000008,       // Draw the shift-click line?

        Spawns              = 0x00000010,       // Draw all spawns?

        SpawnTrails         = 0x00000020,       // Draw the mob trails?

        GroundItems         = 0x00000040,       // Draw the ground items?

        SpawnTimers         = 0x00000080,       // Draw the spawn timers?

        DirectionLines      = 0x00000100,       // Draw Direction lines (direction lines and such)

        SpawnRings          = 0x00000200,       // Draw Shopkeeper(etc) rings around mobs

        GridLines           = 0x00000400,       // Draw grid lines

        ZoneText            = 0x00000800,       // Draw zone text

        //

        DrawAll             = 0x0fffffff,

        DrawNormal          = DrawMap           // Standard, nice, savings

                              +Readjust

                              +Player

                              +SpotLine

                              +Spawns

                              +GroundItems

                              +SpawnTimers

                              +DirectionLines

                              +GridLines

                              +ZoneText,   

        DrawLess            = DrawNormal        // Takes away more uncritically things

                              -DirectionLines

                              -GroundItems

                              -SpawnTimers

                              -ZoneText,

        DrawEvenLess        = DrawLess          // Takes away substantially

                              -Readjust,

        DrawNone            = 0x00000000,       // Maximum savings - all enabled

    };
    #region SMTPSettings class

    [Serializable]

    public sealed class SMTPSettings 
    
    {
        private volatile static SMTPSettings instance;

        private static object syncObj = new object();

        // SMTP Server

        private string smtpServer = "";

        private string smtpDomain = "";

        private string smtpUsername = "";

        private string smtpPassword = "";

        private int smtpPort = 25;

        private string fromEmail = "";

        private string toEmail = "";

        private string ccEmail = "";

        private bool useNetworkCredentials = false;

        private bool useSSL = false;

        private bool savePassword = false;

        private String prefsDir = "";

        public string SmtpServer { get { return smtpServer; } set { smtpServer = value; } }

        public string SmtpDomain { get { return smtpDomain; } set { smtpDomain = value; } }

        public string SmtpUsername { get { return smtpUsername; } set { smtpUsername = value; } }

        public string SmtpPassword {
            get
            {
                if (smtpPassword == string.Empty)
                {
                    return "";
                }
                else
                {
                    Assembly asm = Assembly.GetExecutingAssembly();
                    return DecryptString(smtpPassword, asm.GetType().GUID.ToString());
                }
            }
            set {
                if (value == string.Empty)
                {
                    smtpPassword = "";
                }else{
                    Assembly asm = Assembly.GetExecutingAssembly();

                smtpPassword =EncryptString(value, asm.GetType().GUID.ToString()); 
                } 
            }
        }

        public int SmtpPort { get { return smtpPort; } set { smtpPort = value; } }
        
        public string FromEmail { get { return fromEmail; } set { fromEmail = value; } }

        public string ToEmail { get { return toEmail; } set { toEmail = value; } }

        public string CCEmail { get { return ccEmail; } set { ccEmail = value; } }

        public bool UseNetworkCredentials { get { return useNetworkCredentials; } set { useNetworkCredentials = value; } }

        public bool UseSSL { get { return useSSL; } set { useSSL = value; } }

        public bool SavePassword { get { return savePassword; } set { savePassword = value; } }

        private SMTPSettings() {}

        public static SMTPSettings Instance
        {

            get
            {

                // only create a new instance if one doesn't already exist.

                if (instance == null)

                    // use this lock to ensure that only one thread is access this block of code at once.

                    lock (syncObj)

                        if (instance == null)

                            instance = new SMTPSettings();

                // return instance where it was just created or already existed.

                return instance;

            }

            set { instance = value; }

        }

        public void Save(string filename)
        {

            if (prefsDir == string.Empty)
            {
                
                prefsDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MySEQ");

            }


            // Dont save password if we are not selecting to save it
            string curpass = "";
            if (SMTPSettings.Instance.SavePassword == false) {
                curpass = SMTPSettings.Instance.SmtpPassword;
                SMTPSettings.Instance.SmtpPassword = "";
            }

            FileStream fs = new FileStream(filename, FileMode.Create);

            SoapFormatter sf1 = new SoapFormatter();

            sf1.Serialize(fs, SMTPSettings.Instance);

            if (SMTPSettings.Instance.SavePassword == false) {
                SMTPSettings.Instance.SmtpPassword = curpass;
                curpass = "";
            }

            fs.Close();

            String oldconfigFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "myseq.xml");
            if (File.Exists(oldconfigFile))
                File.Delete(oldconfigFile);

        }



        public void Load(string filename) {

            FileStream fs = null;

            try {

                fs = new FileStream(filename, FileMode.Open);

                SoapFormatter sf1 = new SoapFormatter();


                SMTPSettings.Instance = (SMTPSettings)sf1.Deserialize(fs);

                String myPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MySEQ");

                SMTPSettings.Instance.prefsDir = myPath;

            }

            catch (Exception ex) { LogLib.WriteLine("Error in SMTPSettings.Load(): ", ex); }

            if (fs != null) fs.Close();

        }

        public static string DecryptString(string Message, string _Pass)
        {
            byte[] _Res;

            UTF8Encoding _UTF8 = new UTF8Encoding();
            MD5CryptoServiceProvider _Hash = new MD5CryptoServiceProvider();

            byte[] _Key = _Hash.ComputeHash(_UTF8.GetBytes(_Pass));
            byte[] _Data = Convert.FromBase64String(Message);

            TripleDESCryptoServiceProvider _Service = new TripleDESCryptoServiceProvider();
            _Service.Key = _Key;
            _Service.Mode = CipherMode.ECB;
            _Service.Padding = PaddingMode.PKCS7;

            try
            {
                ICryptoTransform _Decrypter = _Service.CreateDecryptor();
                _Res = _Decrypter.TransformFinalBlock(_Data, 0, _Data.Length);
            }
            catch (Exception ex)
            {
                LogLib.WriteLine("Error in DecryptString(): ", ex);

                return "";
            }
            finally
            {
                _Hash.Clear();
                _Service.Clear();
            }

            return _UTF8.GetString(_Res);
        }

        public static string EncryptString(string Message, string _Pass)
        {
            byte[] _Res;
            
            UTF8Encoding _UTF8 = new UTF8Encoding();
            MD5CryptoServiceProvider _Hash = new MD5CryptoServiceProvider();

            byte[] _Key = _Hash.ComputeHash(_UTF8.GetBytes(_Pass));
            byte[] _Data = _UTF8.GetBytes(Message);

            TripleDESCryptoServiceProvider _Service = new TripleDESCryptoServiceProvider();

            _Service.Key = _Key;
            _Service.Mode = CipherMode.ECB;
            _Service.Padding = PaddingMode.PKCS7;

            try
            {
                ICryptoTransform _Encrypter = _Service.CreateEncryptor();
                _Res = _Encrypter.TransformFinalBlock(_Data, 0, _Data.Length);
            }
            catch (Exception ex)
            {
                LogLib.WriteLine("Error in EncryptString(): ", ex);
                return "";
            }
            finally
            {
                _Hash.Clear();
                _Service.Clear();
            }
            return Convert.ToBase64String(_Res);
        }
    }

    #endregion

    #region Settings class

    // Settings singleton

    [Serializable]

    public sealed class Settings  

    {

        private volatile static Settings instance;

        private static object syncObj = new object();

        private FormWindowState windowState = FormWindowState.Normal;

        private Size windowsSize = new Size(800, 600);

        private Point windowsLocation = new Point(20, 20);

        private Point optionWindowsLocation = new Point(20, 20);

        private Size optionWindowsSize = new Size(296, 480);

        private FormStartPosition windowsPosition = FormStartPosition.Manual;

        private string ipAddress1 = "127.0.0.1";

        private string ipAddress2 = "";

        private string ipAddress3 = "";

        private string ipAddress4 = "";

        private string ipAddress5 = "";

        private int currentIPAddress = 0;

        private int port = 5555;

        private bool autoConnect = true;

        private bool prefixStars = true;        

        private bool affixStars = true; // affix

        private bool corpseAlerts = true;

        private int rangeCircle = 150;

        private int levelOverride = -1;

        private int splitterLoc = 256;

        private int updateDelay = 200;

        private bool depthFilter = false;

        private bool collectMobTrails = false;

        private bool saveOnExit = true;

        private LogLevel maxLogLevel = 0;

        private Color selectedAddMapText = Color.White;

        private bool showPCNames = false;

        private bool showNPCNames = false;

        private bool showNPCCorpseNames = false;

        private bool showPlayerCorpseNames = false;

        private bool showMenuBar = true;

        private bool showStatusBar = true;

        private bool showToolBar = true;

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

        

        private string fontName = "Microsoft Sans Serif";

        private float fontSize = 6.75f;

        private FontStyle fontStyle = 0;



        private string maplabelfontName = "Microsoft Sans Serif";

        private float maplabelfontSize = 6.75f;

        private FontStyle maplabelfontStyle = 0;



        private string targetFontName = "Microsoft Sans Serif";

        private float targetFontSize = 6.75f;

        private FontStyle targetFontStyle = 0;



        private Color backColor = Color.Black;

        private Color gridColor = Color.DarkGreen;

        private Color gridLabelColor = Color.Yellow;

        private Color rangeCircleColor = Color.DarkGray;

        private Color listBackColor = Color.White;

        private Color pcBorderColor = Color.Fuchsia;



        private int gridInterval  = 500;

        private bool showListGridLines = false;

        private bool showListSearchBox = false;

        private bool showMobList = true;

        private bool showMobListTimer = true;

        private bool showGroundItemList = true;



        private string Title = "";

        private string searchString = "http://everquest.allakhazam.com/search.html?q={0}";

        private int SpawnSize = 3;

        private int pvpLevels = 10;

        private int minAlertLevel = -1;

        private bool colorRangeCircle = false;

        private string hatchselected = "Percent05";

        private string alertsound = "Beep";

        private bool drawFoV = true;

        private bool showZoneName = true;

        private bool showCharName = true;

        private bool sodCon = false;

        private bool sofCon = false;

        private bool defaultCon = true;

        private bool showTargetInfo = true;

        private bool smallTargetInfo = true;

        private bool autoSelectEQTarget = true;

        private string mapDir = "";

        private string filterDir = "";

        private string cfgDir = "";

        private string logDir = "";

        private string timerDir = "";



        // Alerts

        private string mHuntPrefix = " [H]"; // hunt

        private bool matchFullTextH = false; 

        private bool noneOnHunt = true;

        private bool beepOnHunt = false;

        private bool talkOnHunt = false;

        private bool playOnHunt = false;

        private string mHuntAudioFile = "";

        private string mCautionPrefix = " [C]"; // caution

        private bool matchFullTextC = false; 

        private bool noneOnCaution = true;

        private bool beepOnCaution = false;

        private bool talkOnCaution = false;

        private bool playOnCaution = false;

        private string mCautionAudioFile = "";



        private string mDangerPrefix = " [D]"; // danger

        private bool matchFullTextD = false;

        private bool noneOnDanger = true;

        private bool beepOnDanger = false;

        private bool talkOnDanger = false;

        private bool playOnDanger = false;

        private string mDangerAudioFile = "";



        private string mAlertPrefix = " [R]"; // rare

        private bool matchFullTextA = false;

        private bool noneOnAlert = true;

        private bool beepOnAlert = false;

        private bool talkOnAlert = false;

        private bool playOnAlert = false;

        private string mAlertAudioFile = "";

        // Email Alerts

        private bool emailAlerts = false;

        // new filters stuff

        private bool showPlayers = true;

        private bool showNPCs = true;

        private bool showCorpses = true;

        private bool showPCCorpses = true;

        private bool showMyCorpse = true;

        private bool showInvis = true;

        private bool showMounts = true;

        private bool showFamiliars = true;

        private bool showPets = true;

        private bool showPVP = false;

        private bool showPVPLevel = false;

        private bool showNPCLevels = false;

        private bool autoSelectSpawnList = true;

        

        private myseq.FollowOption followOption = myseq.FollowOption.Player;

        private bool keepCentered = false;

        private bool autoExpand = true;

        

        public DrawOptions DrawOptions = DrawOptions.DrawNormal;

        

        public bool saveSpawnTimers = true;

        private bool saveSpawnLogs = false;

        private bool spawnCountdown = false;



        // Advanced map drawing

        private bool forceDistinct = false;

        private bool forceDistinctText = false;

        private bool useDynamicAlpha = true;

        private int fadedLines = 10;

        private bool filterGroundItems = true;

        private bool filterNPCs = true;

        private bool filterPlayers = true;

        private bool filterMapLines = true;

        private bool filterMapText = true;

        private bool filterSpawnPoints = true;

        private bool filterNPCCorpses = true;

        private bool filterPlayerCorpses = true;



        // need to remember where prefs file was opened

        private String prefsDir = "";



        public bool KeepCentered{get{return keepCentered;} set{keepCentered = value;}}

        public bool AutoExpand { get { return autoExpand; } set { autoExpand = value; } }

        public bool AutoSelectSpawnList {get{return autoSelectSpawnList;} set{autoSelectSpawnList = value;}}

        public bool ShowPlayers { get { return showPlayers; } set { showPlayers = value; } }

        public bool ShowNPCs { get { return showNPCs; } set { showNPCs = value; } }

        public bool ShowCorpses { get { return showCorpses; } set { showCorpses = value; } }

        public bool ShowPCCorpses { get { return showPCCorpses; } set { showPCCorpses = value; } }

        public bool ShowMyCorpse { get { return showMyCorpse; } set { showMyCorpse = value; } }

        public bool ShowInvis { get { return showInvis; } set { showInvis = value; } }

        public bool ShowMounts { get { return showMounts; } set { showMounts = value; } }

        public bool ShowFamiliars { get { return showFamiliars; } set { showFamiliars = value; } }

        public bool ShowPets { get { return showPets; } set { showPets = value; } }

        public bool AutoSelectEQTarget { get { return autoSelectEQTarget; } set { autoSelectEQTarget = value; } }

        public bool ForceDistinct { get { return forceDistinct; } set { forceDistinct = value; } }

        public bool ForceDistinctText { get { return forceDistinctText; } set { forceDistinctText = value; } }

        public bool UseDynamicAlpha { get { return useDynamicAlpha; } set { useDynamicAlpha = value; } }

        public int FadedLines { get { return fadedLines; } set { fadedLines = value; } }

        public bool FilterGroundItems { get { return filterGroundItems; } set { filterGroundItems = value; } }

        public bool FilterNPCs { get { return filterNPCs; } set { filterNPCs = value; } }

        public bool FilterMapLines { get { return filterMapLines; } set { filterMapLines = value; } }

        public bool FilterMapText { get { return filterMapText; } set { filterMapText = value; } }

        public bool FilterPlayers { get { return filterPlayers; } set { filterPlayers = value; } }

        public bool FilterSpawnPoints { get { return filterSpawnPoints; } set { filterSpawnPoints = value; } }

        public bool FilterPlayerCorpses { get { return filterPlayerCorpses; } set { filterPlayerCorpses = value; } }

        public bool FilterNPCCorpses { get { return filterNPCCorpses; } set { filterNPCCorpses = value; } }

        public bool ShowTargetInfo { get { return showTargetInfo; } set { showTargetInfo = value; } }

        public bool SmallTargetInfo { get { return smallTargetInfo; } set { smallTargetInfo = value; } }

        public bool ShowZoneName { get { return showZoneName; } set { showZoneName = value; } }

        public bool ShowCharName { get { return showCharName; } set { showCharName = value; } }

        public bool ShowPVP { get { return showPVP; } set { showPVP = value; } }

        public bool ShowPVPLevel { get { return showPVPLevel; } set { showPVPLevel = value; } }

        public bool ShowNPCLevels { get { return showNPCLevels; } set { showNPCLevels = value; } }

        public bool SoDCon { get { return sodCon; } set { sodCon = value; } }

        public bool SoFCon { get { return sofCon; } set { sofCon = value; } }

        public bool DefaultCon { get { return defaultCon; } set { defaultCon = value; } }

        public bool DrawFoV {get{return drawFoV;} set{drawFoV = value;}}

        public bool ColorRangeCircle {get{return colorRangeCircle;} set{colorRangeCircle = value;}}

        public Color BackColor {get{return backColor;} set{backColor = value;}}

        public string HatchIndex { get { return hatchselected; } set { hatchselected = value; } }

        public string AlertSound { get { return alertsound; } set { alertsound = value; } }

        public Color GridColor { get { return gridColor; } set { gridColor = value; } }

        public Color GridLabelColor { get { return gridLabelColor; } set { gridLabelColor = value; } }

        public Color RangeCircleColor { get { return rangeCircleColor; } set { rangeCircleColor = value; } }

        public Color ListBackColor {get{return listBackColor;} set{listBackColor = value;}}

        public Color PCBorderColor {get{return pcBorderColor;} set{pcBorderColor = value;}}

        public myseq.FollowOption FollowOption {get {return followOption;} set{followOption = value;}}

        public bool ShowMobList {get{return showMobList;} set{showMobList = value;}}

        public bool ShowMobListTimer {get{return showMobListTimer;} set{showMobListTimer = value; } }

        public bool ShowGroundItemList { get { return showGroundItemList; } set { showGroundItemList = value; } }

        public bool NoneOnHunt {get{return noneOnHunt;} set{noneOnHunt = value;}}

        public bool NoneOnCaution {get{return noneOnCaution;} set{noneOnCaution = value;}}

        public bool NoneOnDanger { get { return noneOnDanger; } set { noneOnDanger = value; } }

        public bool NoneOnAlert {get{return noneOnAlert;} set{noneOnAlert = value;}}

        public bool BeepOnHunt {get{return beepOnHunt;} set{beepOnHunt = value;}}

        public bool BeepOnCaution { get { return beepOnCaution; } set { beepOnCaution = value; } }

        public bool BeepOnDanger { get { return beepOnDanger; } set { beepOnDanger = value; } }

        public bool BeepOnAlert { get { return beepOnAlert; } set { beepOnAlert = value; } }

        public bool PlayOnHunt {get{return playOnHunt;} set{playOnHunt = value;}}

        public bool PlayOnCaution { get { return playOnCaution; } set { playOnCaution = value; } }

        public bool PlayOnDanger { get { return playOnDanger; } set { playOnDanger = value; } }

        public bool PlayOnAlert { get { return playOnAlert; } set { playOnAlert = value; } }

        public bool TalkOnHunt {get{return talkOnHunt;} set{talkOnHunt = value;}}

        public bool TalkOnCaution { get { return talkOnCaution; } set { talkOnCaution = value; } }

        public bool TalkOnDanger { get { return talkOnDanger; } set { talkOnDanger = value; } }

        public bool TalkOnAlert { get { return talkOnAlert; } set { talkOnAlert = value; } }

        public bool EmailAlerts { get { return emailAlerts; } set { emailAlerts = value; } }

        public string HuntAudioFile {get{return mHuntAudioFile;} set{mHuntAudioFile = value;}}

        public string CautionAudioFile { get { return mCautionAudioFile; } set { mCautionAudioFile = value; } }

        public string DangerAudioFile { get { return mDangerAudioFile; } set { mDangerAudioFile = value; } }

        public string AlertAudioFile { get { return mAlertAudioFile; } set { mAlertAudioFile = value; } }

        public int SpawnDrawSize { get { return SpawnSize; } set { SpawnSize = value; } }

        public int PVPLevels { get { return pvpLevels; } set { pvpLevels = value; } }

        public int MinAlertLevel { get { return minAlertLevel; } set { minAlertLevel = value; } }

        public string TitleBar {get{return Title;} set{Title = value;}}

        public string SearchString { get { return searchString; } set { searchString = value; } }

        public bool CollectMobTrails {get{return collectMobTrails;} set{collectMobTrails = value;}}

        public float MapLabelFontSize { get { return maplabelfontSize; } set { maplabelfontSize = value; } }

        public string MapLabelFontName { get { return maplabelfontName; } set { maplabelfontName = value; } }

        public FontStyle MapLabelFontStyle { get { return maplabelfontStyle; } set { maplabelfontStyle = value; } }

        public float ListFontSize { get { return fontSize; } set { fontSize = value; } }

        public string ListFontName { get { return fontName; } set { fontName = value; } }

        public FontStyle ListFontStyle {get{return fontStyle;} set{fontStyle = value;}}

        public float TargetInfoFontSize {get{return targetFontSize;} set{targetFontSize = value;}}

        public string TargetInfoFontName {get{return targetFontName;} set{targetFontName = value;}}

        public FontStyle TargetInfoFontStyle {get{return targetFontStyle;} set{targetFontStyle = value;}}

        public bool DepthFilter {get{return depthFilter;} set{depthFilter = value;}}

        public int UpdateDelay {get{return updateDelay;} set{updateDelay = value;}}

        public bool MatchFullTextH { get { return matchFullTextH; } set { matchFullTextH = value; } }

        public bool MatchFullTextC { get { return matchFullTextC; } set { matchFullTextC = value; } }

        public bool MatchFullTextD { get { return matchFullTextD; } set { matchFullTextD = value; } }

        public bool MatchFullTextA { get { return matchFullTextA; } set { matchFullTextA = value; } }

        public string HuntPrefix {get{return mHuntPrefix;} set{mHuntPrefix = value;}}

        public string CautionPrefix {get{return mCautionPrefix;} set{mCautionPrefix = value;}}

        public string DangerPrefix { get { return mDangerPrefix; } set { mDangerPrefix = value; } }

        public string AlertPrefix {get{return mAlertPrefix;} set{mAlertPrefix = value;}}

        public bool SaveOnExit { get { return saveOnExit; } set { saveOnExit = value; } }

        public bool ShowMenuBar { get { return showMenuBar; } set { showMenuBar = value; } }

        public bool ShowStatusBar { get { return showStatusBar; } set { showStatusBar = value; } }
        
        public bool ShowToolBar { get { return showToolBar; } set { showToolBar = value; } }
      
        public bool SaveSpawnLogs { get { return saveSpawnLogs; } set { saveSpawnLogs = value; } }

        public bool SpawnCountdown { get { return spawnCountdown; } set { spawnCountdown = value; } }

        public bool ShowNPCNames { get { return showNPCNames; } set { showNPCNames = value; } }

        public bool ShowPCNames { get { return showPCNames; } set { showPCNames = value; } }

        public bool ShowNPCCorpseNames { get { return showNPCCorpseNames; } set { showNPCCorpseNames = value; } }

        public bool ShowPlayerCorpseNames { get { return showPlayerCorpseNames; } set { showPlayerCorpseNames = value; } }

        public int SplitterLoc { get { return splitterLoc; } set { splitterLoc = value; } }

        public FormWindowState WindowState {get{return windowState;} set{windowState = value;}}

        public Size WindowsSize { get { return windowsSize; } set { windowsSize = value; } }

        public Point WindowsLocation { get { return windowsLocation; } set { windowsLocation = value; } }

        public Size OptionsWindowsSize { get { return optionWindowsSize; } set { optionWindowsSize = value; } }

        public Point OptionsWindowsLocation { get { return optionWindowsLocation; } set { optionWindowsLocation = value; } }

        public FormStartPosition WindowsPosition { get { return windowsPosition; } set { windowsPosition = value; } }
        
        public bool PrefixStars {get{return prefixStars;} set{prefixStars = value;}}

        public bool AffixStars {get{return affixStars;} set{affixStars = value;}}

        public bool CorpseAlerts { get { return corpseAlerts; } set { corpseAlerts = value; } }

        public int LevelOverride {get{return levelOverride;} set{levelOverride = value;}}

        public int RangeCircle {get{return rangeCircle;} set{rangeCircle = value;}}

        public string IPAddress1 {get{return ipAddress1;} set{ipAddress1 = value;}}

        public string IPAddress2 {get{return ipAddress2;} set{ipAddress2 = value;}}

        public string IPAddress3 {get{return ipAddress3;} set{ipAddress3 = value;}}

        public string IPAddress4 {get{return ipAddress4;} set{ipAddress4 = value;}}

        public string IPAddress5 {get{return ipAddress5;} set{ipAddress5 = value;}}

        public int CurrentIPAddress {get{return currentIPAddress;} set{currentIPAddress = value;}}

        public int Port {get{return port;} set{port = value;}}

        public bool AutoConnect { get { return autoConnect; } set { autoConnect = value; } }

        public int GridInterval{get {return gridInterval;} set {gridInterval = value;}}

        public bool ShowListGridLines { get { return showListGridLines; } set { showListGridLines = value; } }

        public bool ShowListSearchBox { get { return showListSearchBox; } set { showListSearchBox = value; } }

        public string MapDir{get {return mapDir;} set {mapDir = value;}}

        public string FilterDir{get {return filterDir;} set {filterDir = value;}}

        public string CfgDir{get {return cfgDir;} set {cfgDir = value;}}

        public string LogDir{get {return logDir;} set {logDir = value;}}

        public string TimerDir{get {return timerDir;} set {timerDir = value;}}

        public LogLevel MaxLogLevel{get {return maxLogLevel;} set {maxLogLevel = value;}}

        public Color SelectedAddMapText { get { return selectedAddMapText; } set { selectedAddMapText = value; } }

        public static Settings Instance {

            get {

                // only create a new instance if one doesn't already exist.

                if (instance == null)

                    // use this lock to ensure that only one thread is access this block of code at once.

                    lock(syncObj) 

                        if (instance == null)

                            instance = new Settings();

                

                // return instance where it was just created or already existed.

                return instance;

            }

            set{instance = value;}

        }



        public void Save(string filename) {


            if (prefsDir == string.Empty)
            {

                prefsDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MySEQ");

            }

            FileStream fs = new FileStream(filename, FileMode.Create);

            SoapFormatter sf1 = new SoapFormatter();

            sf1.Serialize(fs, Settings.Instance);

            fs.Close();

            String oldconfigFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "prefs.xml");
            if (File.Exists(oldconfigFile))
                File.Delete(oldconfigFile);
        }



        public void Load(string filename) {

            FileStream fs = null;
            bool otherLoad = false;
            try {

                fs = new FileStream(filename, FileMode.Open);

                SoapFormatter sf1 = new SoapFormatter();

                Settings.Instance = (Settings)sf1.Deserialize(fs);

            }

            catch (Exception ex) {
                LogLib.WriteLine("Error in Settings.Load(): ", ex);
                otherLoad = true;
            }

            String myPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MySEQ");

            Settings.Instance.prefsDir = myPath;

            if (fs != null) fs.Close();

            if (otherLoad)
            {
                // Try manually loading directly
                try {
                    XmlTextReader reader = new XmlTextReader(filename);
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

                catch (Exception ex)
                {
                    LogLib.WriteLine("Error in Settings.Load(): ", ex);
                    otherLoad = true;
                }

            }

        }

    }

    #endregion



}