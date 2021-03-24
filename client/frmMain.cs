
using myseq.Properties;
using Structures;

using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace myseq
{
    [Serializable]

    public class FrmMain : Form
    {
        public string Version = Application.ProductVersion;

        private Filters filters = new Filters();

        public string curZone = "map_pane";

        private string currentIPAddress = "";

//        public float rawScale = 1.0f;

        private string BaseTitle = "MySEQ Open";

        private Point addTextFormLocation = new Point(0, 0);

        public MapCon mapCon;

        public MapPane mapPane = new MapPane();

        public ListViewPanel SpawnList = new ListViewPanel(0);

        public ListViewPanel SpawnTimerList = new ListViewPanel(1);

        public ListViewPanel GroundItemList = new ListViewPanel(2);

        private readonly EQData eq;

        private readonly EQCommunications comm;

        private readonly EQMap map;

        public DrawOptions DrawOpts=DrawOptions.DrawNormal;

        private ArrayList colProcesses = new ArrayList();

        private ProcessInfo CurrentProcess = new ProcessInfo(0,"");

        public int processcount;

        #region System Components

        private MenuStrip mnuMainMenu;

        private StatusStrip statusBarStrip;

        private Timer timPackets;

        private System.Timers.Timer timDelayAlerts;

        private System.Timers.Timer timProcessTimers;

        private IContainer components;

        public ColorDialog colorPicker;

        private OpenFileDialog openFileDialog;

        private FontDialog fontDialog1;

        private ContextMenuStrip mnuContext;

        private ContextMenuStrip mnuContextAddFilter;

        private ToolStripMenuItem mnuMobName;

        private ToolStripMenuItem mnuAddHuntFilter;

        private ToolStripMenuItem mnuAddCautionFilter;

        private ToolStripMenuItem mnuAddDangerFilter;

        private ToolStripMenuItem mnuAddAlertFilter;

        private ToolStripMenuItem mnuAddMapLabel;

        public string alertAddmobname = "";
        public float alertX = 0.0f;
        public float alertY = 0.0f;
        public float alertZ = 0.0f;

        public string mapnameWithLabels = "";

        private ToolStripMenuItem mnuSearchAllakhazam;

        private ToolStripMenuItem mnuShowListNPCs;

        private ToolStripMenuItem mnuShowListCorpses;

        private ToolStripMenuItem mnuShowListPlayers;

        private ToolStripMenuItem mnuShowListInvis;

        private ToolStripMenuItem mnuShowListMounts;

        private ToolStripMenuItem mnuShowListFamiliars;

        private ToolStripMenuItem mnuShowListPets;

        private ToolStripMenuItem mnuFileMain;
        private ToolStripMenuItem mnuConnect;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem mnuOpenMap;
        private ToolStripMenuItem mnuSaveMobs;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem mnuSavePrefs;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripMenuItem mnuExit;
        private ToolStripMenuItem mnuEditMain;
        private ToolStripMenuItem mnuReloadAlerts;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripMenuItem mnuEditGlobalAlerts;
        private ToolStripMenuItem mnuEditZoneAlerts;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripMenuItem mnuRefreshSpawnList;
        private ToolStripMenuItem mnuClearSavedTimers;
        private ToolStripMenuItem mnuSaveSpawnLog;
        private ToolStripMenuItem mnuViewMain;
        private ToolStripMenuItem mnuShowSpawnList;
        private ToolStripMenuItem mnuShowSpawnListTimer;
        private ToolStripMenuItem mnuShowGroundItemList;
        private ToolStripSeparator toolStripSeparator7;
        private ToolStripMenuItem mnuShowListGridLines;
        private ToolStripMenuItem mnuShowGridLines;
        private ToolStripMenuItem mnuMapSettingsMain;
        private ToolStripMenuItem mnuDepthFilter;
        private ToolStripMenuItem menuItem3;
        private ToolStripMenuItem mnuDynamicAlpha;
        private ToolStripMenuItem mnuForceDistinct;
        private ToolStripMenuItem mnuForceDistinctText;
        private ToolStripMenuItem mnuFilterMapLines;
        private ToolStripMenuItem mnuFilterMapText;
        private ToolStripMenuItem mnuFilterNPCs;
        private ToolStripMenuItem mnuFilterNPCCorpses;
        private ToolStripMenuItem mnuFilterPlayers;
        private ToolStripMenuItem mnuFilterPlayerCorpses;
        private ToolStripMenuItem mnuFilterGroundItems;
        private ToolStripMenuItem mnuFilterSpawnPoints;
        private ToolStripMenuItem mnuLabelShow;
        private ToolStripMenuItem mnuShowNPCLevels;
        private ToolStripMenuItem mnuShowNPCNames;
        private ToolStripMenuItem mnuShowNPCCorpseNames;
        private ToolStripMenuItem mnuShowPCNames;
        private ToolStripMenuItem mnuShowPlayerCorpseNames;
//        private ToolStripMenuItem mnuShowPCGuild;
        private ToolStripMenuItem mnuSpawnCountdown;
        private ToolStripMenuItem mnuShowSpawnPoints;
        private ToolStripMenuItem mnuShowZoneText;
        private ToolStripMenuItem mnuShowLayer1;
        private ToolStripMenuItem mnuShowLayer2;
        private ToolStripMenuItem mnuShowLayer3;
        private ToolStripMenuItem mnuShowPVP;
        private ToolStripMenuItem mnuShowPVPLevel;
        private ToolStripMenuItem mnuCollectMobTrails;
        private ToolStripMenuItem mnuShowMobTrails;
        private ToolStripMenuItem mnuConColors;
        private ToolStripMenuItem mnuConDefault;
        private ToolStripMenuItem mnuConSoD;
        private ToolStripMenuItem mnuConSoF;
        private ToolStripMenuItem mnuGridInterval;
        private ToolStripMenuItem mnuGridInterval100;
        private ToolStripMenuItem mnuGridInterval250;
        private ToolStripMenuItem mnuGridInterval500;
        private ToolStripMenuItem mnuGridInterval1000;
        private ToolStripMenuItem mnuShowTargetInfo;
        private ToolStripMenuItem mnuAutoSelectEQTarget;
        private ToolStripSeparator toolStripSeparator10;
        private ToolStripMenuItem mnuFollowNone;
        private ToolStripMenuItem mnuFollowPlayer;
        private ToolStripMenuItem mnuFollowTarget;
        private ToolStripMenuItem mnuKeepCentered;
        private ToolStripMenuItem mnuAutoExpand;
        private ToolStripMenuItem mnuMapReset;
        private ToolStripMenuItem mnuHelpMain;
        private ToolStripMenuItem mnuAbout;

        private ToolStripSeparator toolStripSeparator11;
        private ToolStripSeparator toolStripSeparator12;
        private ToolStripSeparator toolStripSeparator13;
        public ToolStripStatusLabel toolStripMouseLocation;
        public ToolStripStatusLabel toolStripDistance;
        public ToolStripStatusLabel toolStripSpring;
        public ToolStripStatusLabel toolStripVersion;
        public ToolStripStatusLabel toolStripServerAddress;
        public ToolStripStatusLabel toolStripCoPStatus;
        public ToolStripStatusLabel toolStripShortName;
        public ToolStripStatusLabel toolStripFPS;
        private ToolStrip toolBarStrip;
        private ToolStripButton toolStripStartStop;
        private ToolStripButton toolStripZoomIn;
        private ToolStripButton toolStripZoomOut;
        private ToolStripSeparator toolStripSeparator14;
        public ToolStripComboBox toolStripScale;
        private ToolStripLabel toolStripZPosLabel;
        public ToolStripTextBox toolStripZPos;
        private ToolStripLabel toolStripZOffsetLabel;
        public ToolStripTextBox toolStripZNeg;
        private ToolStripButton toolStripZPosUp;
        private ToolStripSeparator menuItem11;
        private ToolStripSeparator mnuSepAddFilter;
        private ToolStripButton toolStripZPosDown;
        private ToolStripButton toolStripDepthFilterButton;
        private ToolStripButton toolStripOptions;
        private ToolStripMenuItem mnuOptions;
        private ToolStripMenuItem mnuDepthFilter2;
        private ToolStripMenuItem toolStripMenuItem2;
        private ToolStripMenuItem mnuDynamicAlpha2;
        private ToolStripMenuItem mnuForceDistinct2;
        private ToolStripMenuItem mnuForceDistinctText2;
        private ToolStripMenuItem mnuFilterMapLines2;
        private ToolStripMenuItem mnuFilterMapText2;
        private ToolStripMenuItem mnuFilterNPCs2;
        private ToolStripMenuItem mnuFilterNPCCorpses2;
        private ToolStripMenuItem mnuFilterPlayers2;
        private ToolStripMenuItem mnuFilterPlayerCorpses2;
        private ToolStripMenuItem mnuFilterGroundItems2;
        private ToolStripMenuItem mnuFilterSpawnPoints2;
        private ToolStripSeparator toolStripSeparator6;
        private ToolStripMenuItem mnuLabelShow2;
        private ToolStripMenuItem mnuShowNPCLevels2;
        private ToolStripMenuItem mnuShowNPCNames2;
        private ToolStripMenuItem mnuShowNPCCorpseNames2;
        private ToolStripMenuItem mnuShowPCNames2;
        private ToolStripMenuItem mnuShowPlayerCorpseNames2;
//        private ToolStripMenuItem mnuShowPCGuild2;
        private ToolStripMenuItem mnuSpawnCountdown2;
        private ToolStripMenuItem mnuShowSpawnPoints2;
        private ToolStripMenuItem mnuShowZoneText2;
        private ToolStripMenuItem mnuShowLayer21;
        private ToolStripMenuItem mnuShowLayer22;
        private ToolStripMenuItem mnuShowLayer23;
        private ToolStripMenuItem mnuShowPVP2;
        private ToolStripMenuItem mnuShowPVPLevel2;
        private ToolStripMenuItem mnuShowTargetInfo2;
        private ToolStripMenuItem mnuAutoSelectEQTarget2;
        private ToolStripSeparator toolStripSeparator15;
        private ToolStripMenuItem mnuFollowNone2;
        private ToolStripMenuItem mnuFollowPlayer2;
        private ToolStripMenuItem mnuFollowTarget2;
        private ToolStripSeparator toolStripSeparator16;
        private ToolStripMenuItem mnuKeepCentered2;
        private ToolStripMenuItem mnuAutoExpand2;
        private ToolStripSeparator toolStripSeparator17;
        private ToolStripMenuItem mnuShowMenuBar;
        private ToolStripMenuItem mnuMapReset2;
        private ToolStripButton toolStripZNegDown;
        private ToolStripButton toolStripZNegUp;

        #endregion
        private DockPanel dockPanel;
        private ToolStripButton toolStripResetDepthFilter;
        private ToolStripMenuItem mnuServerSelection;
        private ToolStripMenuItem mnuIPAddress1;
        private ToolStripMenuItem mnuIPAddress2;
        private ToolStripMenuItem mnuIPAddress3;
        private ToolStripMenuItem mnuIPAddress4;
        private ToolStripMenuItem mnuIPAddress5;
        private ToolStripMenuItem mnuCharSelect;
        private ToolStripMenuItem mnuChar1;
        private ToolStripMenuItem mnuChar2;
        private ToolStripMenuItem mnuChar3;
        private ToolStripMenuItem mnuChar4;
        private ToolStripMenuItem mnuChar5;
        private ToolStripMenuItem mnuChar6;
        private ToolStripMenuItem mnuChar7;
        private ToolStripMenuItem mnuChar8;
        private ToolStripMenuItem mnuChar9;
        private ToolStripMenuItem mnuChar10;
        private ToolStripMenuItem mnuChar11;
        private ToolStripMenuItem mnuChar12;
        private ToolStripMenuItem mnuCharRefresh;
        private ToolStripMenuItem mnuChangeColor;
        private ToolStripMenuItem mnuGridColor;
        private ToolStripMenuItem mnuGridLabelColor;
        private ToolStripMenuItem mnuListColor;
        private ToolStripMenuItem mnuBackgroungColor;
        private ToolStripMenuItem mnuChangeFont;
        private ToolStripMenuItem mnuSpawnListFont;
        private ToolStripMenuItem mnuTargetInfoFont;
        private ToolStripMenuItem mnuMapLabelsFont;
        private ToolStripSeparator toolStripSeparator18;
        private ToolStripSeparator menuItem13;
        private ToolStripMenuItem mnuShowNPCs;
        private ToolStripMenuItem mnuShowCorpses;
        private ToolStripMenuItem mnuShowPCCorpses;
        private ToolStripMenuItem mnuShowMyCorpse;
        private ToolStripMenuItem mnuShowPlayers;
        private ToolStripMenuItem mnuShowInvis;
        private ToolStripMenuItem mnuShowMounts;
        private ToolStripMenuItem mnuShowFamiliars;
        private ToolStripMenuItem mnuShowPets;
        private ToolStripMenuItem mnuShowLookupText;
        private ToolStripMenuItem mnuShowLookupNumber;
        private ToolStripMenuItem mnuAlwaysOnTop;
        private ToolStripSeparator toolStripSeparator8;
        private ToolStripSeparator toolStripSeparator9;
        private ToolStripLabel toolStripLabel1;
        public ToolStripTextBox toolStripLookupBox;
        public ToolStripTextBox toolStripLookupBox1;
        public ToolStripTextBox toolStripLookupBox2;
        public ToolStripTextBox toolStripLookupBox3;
        public ToolStripTextBox toolStripLookupBox4;
        public ToolStripTextBox toolStripLookupBox5;
        private ToolStripButton toolStripResetLookup;
        private ToolStripButton toolStripResetLookup1;
        private ToolStripButton toolStripResetLookup2;
        private ToolStripButton toolStripResetLookup3;
        private ToolStripButton toolStripResetLookup4;
        private ToolStripButton toolStripResetLookup5;
        private ToolStripButton toolStripCheckLookup;
        private ToolStripButton toolStripCheckLookup1;
        private ToolStripButton toolStripCheckLookup2;
        private ToolStripButton toolStripCheckLookup3;
        private ToolStripButton toolStripCheckLookup4;
        private ToolStripButton toolStripCheckLookup5;
        private ToolStripSeparator toolStripSepAddMapLabel;
        private ToolStripMenuItem toolbarsToolStripMenuItem;
        private ToolStripMenuItem mnuViewMenuBar;
        private ToolStripMenuItem mnuViewStatusBar;
        private ToolStripMenuItem mnuViewDepthFilterBar;
        private ToolStripMenuItem addMapTextToolStripMenuItem;

        private readonly DeserializeDockContent m_deserializeDockContent;
        private ToolStripMenuItem mnuShowListSearchBox;
        private ToolStripMenuItem mnuSmallTargetInfo;
        private ToolStripMenuItem mnuSmallTargetInfo2;
        private ToolStripSeparator toolStripSeparator19;

        private ToolStripMenuItem mnuAutoConnect;
        public ToolStripComboBox toolStripLevel;

// These are all in the realm of EQDATA, ProcessGamer region.
// Classes communicate through the SETTINGS class.
//        public int gLastconLevel = -1;
//        public int gconLevel = -1;  
//        public string gConBaseName = "";

        private ToolStripMenuItem toolStripBasecon;

        private bool bIsRunning;
        private bool bFilter0;
        private bool bFilter1;
        private bool bFilter2;
        private bool bFilter3;
        private bool bFilter4;
        private bool bFilter5;

        public bool playAlerts;

        public void StopListening()

        {
            // Stop the Timer

            timPackets.Stop();
            timDelayAlerts.Stop();
            DisablePlayAlerts();

            comm.StopListening();

            mapPane.cmdCommand.Text = "GO";

            mnuConnect.Text = "&Connect";

            mnuConnect.Image = Resources.PlayHS;

            toolStripStartStop.Text = "Go";
            toolStripStartStop.ToolTipText = "Connect to Server";
            toolStripStartStop.Image = Resources.PlayHS;

            bIsRunning = false;

            toolStripServerAddress.Text = "";
        }

        public FrmMain()

        {
            // This shuts up the error messages when running under a debugger

            CheckForIllegalCrossThreadCalls = false;

            m_deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);

            eq = new EQData();

            comm = new EQCommunications(eq,this);

            map = new EQMap();

            InitializeComponent();

            LogLib.maxLogLevel = LogLevel.DefaultMaxLevel;
            LoadPrefs();

            LogLib.WriteLine("MySEQ Open Version: " + Version);

            LogLib.WriteLine("Loaded Prefs");

            mapCon = mapPane.mapCon;

            // Set Map Window Options
            mapPane.DockAreas = DockAreas.Document;
            mapPane.CloseButtonVisible = false;
            mapPane.TabText = "map_pane";

            LogLib.WriteLine("Creating SpawnList Window");

            // Set Spawn List Window Options
            SpawnList.HideOnClose = true;
            SpawnList.TabText = "Spawn List";

            SpawnList.VisibleChanged += new EventHandler(SpawnList_VisibleChanged);

            LogLib.WriteLine("Creating SpawnTimerList Window");

            // Set Spawn Timer Window Options
            SpawnTimerList.HideOnClose = true;
            SpawnTimerList.TabText = "Spawn Timer List";
            SpawnTimerList.VisibleChanged += new EventHandler(SpawnTimerList_VisibleChanged);

            LogLib.WriteLine("Creating GroundItemList Window");

            // Set Ground Item Window Options
            GroundItemList.HideOnClose = true;
            GroundItemList.TabText = "Ground Items";

            GroundItemList.VisibleChanged += new EventHandler(GroundItemList_VisibleChanged);

            mapCon.SetComponents(this,mapPane,eq,map);

            mapPane.SetComponents(this,eq);

            SpawnList.SetComponents(eq,mapCon,filters,this);

            SpawnTimerList.SetComponents(eq, mapCon, filters, this);

            GroundItemList.SetComponents(eq, mapCon, filters, this);

            map.SetComponents(mapCon,SpawnList,SpawnTimerList,GroundItemList,mapPane,eq);

            eq.mobsTimers.SetComponents(map);

            LogLib.WriteLine("Loading Position.Xml");

            string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "positions.xml");

            string myPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MySEQ");
            // This in the application data folder.
            string newConfigFile = Path.Combine(myPath, "positions.xml");

            if (File.Exists(configFile))
            {
                try
                {
                    dockPanel.LoadFromXml(configFile, m_deserializeDockContent);
                }
                catch (Exception ex)
                {
                    LogLib.WriteLine("Error loading config from positions.xml: ", ex);

                    // Re-Set up initial windows - might have bad or incompatible positions file
                    mapPane.Show(dockPanel, DockState.Document);

                    SpawnList.Show(dockPanel, DockState.DockLeft);

                    SpawnTimerList.Show(dockPanel, DockState.DockTop);

                    GroundItemList.Show(dockPanel, DockState.DockBottom);

                    SpawnTimerList.DockState = DockState.DockLeft;

                    GroundItemList.DockState = DockState.DockLeft;
                }
            }
            else if (File.Exists(newConfigFile))
            {
                try
                {
                    dockPanel.LoadFromXml(newConfigFile, m_deserializeDockContent);
                }
                catch (Exception ex)
                {
                    LogLib.WriteLine("Error loading config from AppData positions.xml: ", ex);

                    // Re-Set up initial windows - might have bad or incompatible positions file
                    mapPane.Show(dockPanel, DockState.Document);

                    SpawnList.Show(dockPanel, DockState.DockLeft);

                    SpawnTimerList.Show(dockPanel, DockState.DockTop);

                    GroundItemList.Show(dockPanel, DockState.DockBottom);

                    SpawnTimerList.DockState = DockState.DockLeft;

                    GroundItemList.DockState = DockState.DockLeft;
                }
            }
            else
            {
                // Set up initial windows, when no previous window layout exists
                mapPane.Show(dockPanel, DockState.Document);

                SpawnList.Show(dockPanel, DockState.DockLeft);

                SpawnTimerList.Show(dockPanel, DockState.DockTop);

                GroundItemList.Show(dockPanel, DockState.DockBottom);

                SpawnTimerList.DockState = DockState.DockLeft;

                GroundItemList.DockState = DockState.DockLeft;
            }

            if (Settings.Default.AlwaysOnTop)
            {
                TopMost = true;
                TopLevel = true;
            }

            // Add the Columns to the Spawn List Window

            SpawnList.ColumnsAdd("Name", Settings.Default.c1w, HorizontalAlignment.Left);

            SpawnList.ColumnsAdd("Level", Settings.Default.c2w, HorizontalAlignment.Left);

            SpawnList.ColumnsAdd("Class", Settings.Default.c3w, HorizontalAlignment.Left);

            SpawnList.ColumnsAdd("Primary", Settings.Default.c3w, HorizontalAlignment.Left);

            SpawnList.ColumnsAdd("Offhand", Settings.Default.c3w, HorizontalAlignment.Left);

            SpawnList.ColumnsAdd("Race", Settings.Default.c4w, HorizontalAlignment.Left);

            SpawnList.ColumnsAdd("Owner", Settings.Default.c4w, HorizontalAlignment.Left);

            SpawnList.ColumnsAdd("Last Name", Settings.Default.c5w, HorizontalAlignment.Left);

            SpawnList.ColumnsAdd("Type", Settings.Default.c6w, HorizontalAlignment.Left);

            SpawnList.ColumnsAdd("Invis", Settings.Default.c7w, HorizontalAlignment.Left);

            SpawnList.ColumnsAdd("Run Speed", Settings.Default.c8w, HorizontalAlignment.Left);

            SpawnList.ColumnsAdd("SpawnID", Settings.Default.c9w, HorizontalAlignment.Left);

            SpawnList.ColumnsAdd("Spawn Time", Settings.Default.c10w, HorizontalAlignment.Left);

            SpawnList.ColumnsAdd("X", Settings.Default.c11w, HorizontalAlignment.Left);

            SpawnList.ColumnsAdd("Y", Settings.Default.c12w, HorizontalAlignment.Left);

            SpawnList.ColumnsAdd("Z", Settings.Default.c13w, HorizontalAlignment.Left);

            SpawnList.ColumnsAdd("Distance", Settings.Default.c14w, HorizontalAlignment.Left);

//            SpawnList.ColumnsAdd("Guild", Settings.Default.c14w, HorizontalAlignment.Left);

            // Set the Font, Size, Style for the Spawn List Window

            try {SpawnList.listView.Font = new Font(Settings.Default.ListFont.FontFamily, Settings.Default.ListFont.Size, Settings.Default.ListFont.Style);}
            catch (Exception ex) {LogLib.WriteLine("Error setting spawn list font: ", ex);}

            // Add the Columns to the Spawn Timer Window

            SpawnTimerList.ColumnsAdd("Spawn Name", Settings.Default.c1w, HorizontalAlignment.Left);

            SpawnTimerList.ColumnsAdd("Remain", Settings.Default.c10w, HorizontalAlignment.Left);

            SpawnTimerList.ColumnsAdd("Interval", Settings.Default.c10w, HorizontalAlignment.Left);

            SpawnTimerList.ColumnsAdd("Zone", Settings.Default.c10w, HorizontalAlignment.Left);

            SpawnTimerList.ColumnsAdd("X", Settings.Default.c12w, HorizontalAlignment.Left);

            SpawnTimerList.ColumnsAdd("Y", Settings.Default.c11w, HorizontalAlignment.Left);

            SpawnTimerList.ColumnsAdd("Z", Settings.Default.c13w, HorizontalAlignment.Left);

            SpawnTimerList.ColumnsAdd("Count", Settings.Default.c9w, HorizontalAlignment.Left);

            SpawnTimerList.ColumnsAdd("Spawn Time", Settings.Default.c10w, HorizontalAlignment.Left);

            SpawnTimerList.ColumnsAdd("Kill Time", Settings.Default.c10w, HorizontalAlignment.Left);

            SpawnTimerList.ColumnsAdd("Next Spawn", Settings.Default.c10w, HorizontalAlignment.Left);

            // Set the Font, Size, Style for the Spawn List Timer Window

            try { SpawnTimerList.listView.Font = new Font(Settings.Default.ListFont.Name, Settings.Default.ListFont.Size, Settings.Default.ListFont.Style); }
            catch (Exception ex) { LogLib.WriteLine("Error setting spawn timer list font: ", ex); }

            GroundItemList.ColumnsAdd("Description", Settings.Default.c1w, HorizontalAlignment.Left);

            GroundItemList.ColumnsAdd("Name", Settings.Default.c1w, HorizontalAlignment.Left);

            GroundItemList.ColumnsAdd("Spawn Time", Settings.Default.c10w, HorizontalAlignment.Left);

            GroundItemList.ColumnsAdd("X", Settings.Default.c12w, HorizontalAlignment.Left);

            GroundItemList.ColumnsAdd("Y", Settings.Default.c11w, HorizontalAlignment.Left);

            GroundItemList.ColumnsAdd("Z", Settings.Default.c13w, HorizontalAlignment.Left);

            // Set the Font, Size, Style for the Spawn List Window

            try { GroundItemList.listView.Font = Settings.Default.ListFontStyle; }
            catch (Exception ex) { LogLib.WriteLine("Error setting ground item list font: ", ex); }

            // Set the Font, Size, Style for the Map Labels

            try {
                mapCon.drawFont = Settings.Default.MapLabel;
                mapCon.drawFont1 = new Font(Settings.Default.MapLabel.Name, Settings.Default.MapLabel.Size * 0.9f, Settings.Default.MapLabel.Style);
                mapCon.drawFont3 = new Font(Settings.Default.MapLabel.Name, Settings.Default.MapLabel.Size * 1.1f, Settings.Default.MapLabel.Style);
            }
            catch (Exception ex) { LogLib.WriteLine("Error setting map label font: ", ex); }

            // Set the Font, Size, Style to the Spawn Info Window

            try {
                mapCon.lblMobInfo.Font = Settings.Default.TargetInfoFont;

                mapCon.lblGameClock.Font = new Font(Settings.Default.TargetInfoFont, FontStyle.Bold);
            }
            catch (Exception ex) { LogLib.WriteLine("Error setting Target Info font: ", ex); }

            toolStripVersion.Text = Version;

            ReAdjust();

            eq.InitLookups();

            timPackets.Interval = Settings.Default.UpdateDelay;

            // This is delay that stops emails and alert sounds right after zoning
            timDelayAlerts.Interval = 10000;

            // This is for processing timers, do it once per second.
            timProcessTimers.Interval = 1000;

            SetUpdateSteps();

            Text = BaseTitle;

            if (Settings.Default.AutoConnect)
            {
                StartListening();
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing )

        {
            if( disposing )

            {
                //if (components != null)  {
                //  components.Dispose();
                //}
            }

            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()

        {
            components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(FrmMain));
            DockPanelSkin dockPanelSkin1 = new DockPanelSkin();
            AutoHideStripSkin autoHideStripSkin1 = new AutoHideStripSkin();
            DockPanelGradient dockPanelGradient1 = new DockPanelGradient();
            TabGradient tabGradient1 = new TabGradient();
            DockPaneStripSkin dockPaneStripSkin1 = new DockPaneStripSkin();
            DockPaneStripGradient dockPaneStripGradient1 = new DockPaneStripGradient();
            TabGradient tabGradient2 = new TabGradient();
            DockPanelGradient dockPanelGradient2 = new DockPanelGradient();
            TabGradient tabGradient3 = new TabGradient();
            DockPaneStripToolWindowGradient dockPaneStripToolWindowGradient1 = new DockPaneStripToolWindowGradient();
            TabGradient tabGradient4 = new TabGradient();
            TabGradient tabGradient5 = new TabGradient();
            DockPanelGradient dockPanelGradient3 = new DockPanelGradient();
            TabGradient tabGradient6 = new TabGradient();
            TabGradient tabGradient7 = new TabGradient();
            mnuMainMenu = new MenuStrip();
            mnuFileMain = new ToolStripMenuItem();
            mnuOptions = new ToolStripMenuItem();
            mnuSavePrefs = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            mnuOpenMap = new ToolStripMenuItem();
            mnuSaveMobs = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            mnuConnect = new ToolStripMenuItem();
            mnuAutoConnect = new ToolStripMenuItem();
            mnuServerSelection = new ToolStripMenuItem();
            mnuIPAddress1 = new ToolStripMenuItem();
            mnuIPAddress2 = new ToolStripMenuItem();
            mnuIPAddress3 = new ToolStripMenuItem();
            mnuIPAddress4 = new ToolStripMenuItem();
            mnuIPAddress5 = new ToolStripMenuItem();
            mnuCharSelect = new ToolStripMenuItem();
            mnuChar1 = new ToolStripMenuItem();
            mnuChar2 = new ToolStripMenuItem();
            mnuChar3 = new ToolStripMenuItem();
            mnuChar4 = new ToolStripMenuItem();
            mnuChar5 = new ToolStripMenuItem();
            mnuChar6 = new ToolStripMenuItem();
            mnuChar7 = new ToolStripMenuItem();
            mnuChar8 = new ToolStripMenuItem();
            mnuChar9 = new ToolStripMenuItem();
            mnuChar10 = new ToolStripMenuItem();
            mnuChar11 = new ToolStripMenuItem();
            mnuChar12 = new ToolStripMenuItem();
            menuItem13 = new ToolStripSeparator();
            mnuCharRefresh = new ToolStripMenuItem();
            toolStripSeparator3 = new ToolStripSeparator();
            mnuExit = new ToolStripMenuItem();
            mnuEditMain = new ToolStripMenuItem();
            mnuChangeColor = new ToolStripMenuItem();
            mnuGridColor = new ToolStripMenuItem();
            mnuGridLabelColor = new ToolStripMenuItem();
            mnuListColor = new ToolStripMenuItem();
            mnuBackgroungColor = new ToolStripMenuItem();
            mnuChangeFont = new ToolStripMenuItem();
            mnuSpawnListFont = new ToolStripMenuItem();
            mnuTargetInfoFont = new ToolStripMenuItem();
            mnuMapLabelsFont = new ToolStripMenuItem();
            toolStripSeparator18 = new ToolStripSeparator();
            mnuReloadAlerts = new ToolStripMenuItem();
            toolStripSeparator4 = new ToolStripSeparator();
            mnuEditGlobalAlerts = new ToolStripMenuItem();
            mnuEditZoneAlerts = new ToolStripMenuItem();
            toolStripSeparator5 = new ToolStripSeparator();
            mnuRefreshSpawnList = new ToolStripMenuItem();
            mnuClearSavedTimers = new ToolStripMenuItem();
            mnuSaveSpawnLog = new ToolStripMenuItem();
            mnuViewMain = new ToolStripMenuItem();
            toolbarsToolStripMenuItem = new ToolStripMenuItem();
            mnuViewMenuBar = new ToolStripMenuItem();
            mnuViewStatusBar = new ToolStripMenuItem();
            mnuViewDepthFilterBar = new ToolStripMenuItem();
            toolStripSeparator9 = new ToolStripSeparator();
            mnuShowSpawnList = new ToolStripMenuItem();
            mnuShowSpawnListTimer = new ToolStripMenuItem();
            mnuShowGroundItemList = new ToolStripMenuItem();
            toolStripSeparator7 = new ToolStripSeparator();
            mnuShowListGridLines = new ToolStripMenuItem();
            mnuShowListSearchBox = new ToolStripMenuItem();
            mnuShowGridLines = new ToolStripMenuItem();
            toolStripSeparator8 = new ToolStripSeparator();
            mnuShowCorpses = new ToolStripMenuItem();
            mnuShowPCCorpses = new ToolStripMenuItem();
            mnuShowMyCorpse = new ToolStripMenuItem();
            mnuShowPlayers = new ToolStripMenuItem();
            mnuShowInvis = new ToolStripMenuItem();
            mnuShowMounts = new ToolStripMenuItem();
            mnuShowFamiliars = new ToolStripMenuItem();
            mnuShowPets = new ToolStripMenuItem();
            mnuShowNPCs = new ToolStripMenuItem();
            mnuShowLookupText = new ToolStripMenuItem();
            mnuShowLookupNumber = new ToolStripMenuItem();
            mnuAlwaysOnTop = new ToolStripMenuItem();
            mnuMapSettingsMain = new ToolStripMenuItem();
            mnuDepthFilter = new ToolStripMenuItem();
            menuItem3 = new ToolStripMenuItem();
            mnuDynamicAlpha = new ToolStripMenuItem();
            mnuFilterMapLines = new ToolStripMenuItem();
            mnuFilterMapText = new ToolStripMenuItem();
            mnuFilterNPCs = new ToolStripMenuItem();
            mnuFilterNPCCorpses = new ToolStripMenuItem();
            mnuFilterPlayers = new ToolStripMenuItem();
            mnuFilterPlayerCorpses = new ToolStripMenuItem();
            mnuFilterGroundItems = new ToolStripMenuItem();
            mnuFilterSpawnPoints = new ToolStripMenuItem();
            mnuForceDistinct = new ToolStripMenuItem();
            mnuForceDistinctText = new ToolStripMenuItem();
            toolStripSeparator12 = new ToolStripSeparator();
            mnuLabelShow = new ToolStripMenuItem();
            mnuShowNPCLevels = new ToolStripMenuItem();
            mnuShowNPCNames = new ToolStripMenuItem();
            mnuShowNPCCorpseNames = new ToolStripMenuItem();
            mnuShowPCNames = new ToolStripMenuItem();
            mnuShowPlayerCorpseNames = new ToolStripMenuItem();
//            mnuShowPCGuild = new ToolStripMenuItem();
            mnuSpawnCountdown = new ToolStripMenuItem();
            mnuShowSpawnPoints = new ToolStripMenuItem();
            mnuShowZoneText = new ToolStripMenuItem();
            mnuShowLayer1 = new ToolStripMenuItem();
            mnuShowLayer2 = new ToolStripMenuItem();
            mnuShowLayer3 = new ToolStripMenuItem();
            mnuShowPVP = new ToolStripMenuItem();
            mnuShowPVPLevel = new ToolStripMenuItem();
            mnuCollectMobTrails = new ToolStripMenuItem();
            mnuShowMobTrails = new ToolStripMenuItem();
            mnuConColors = new ToolStripMenuItem();
            mnuConDefault = new ToolStripMenuItem();
            mnuConSoD = new ToolStripMenuItem();
            mnuConSoF = new ToolStripMenuItem();
            mnuGridInterval = new ToolStripMenuItem();
            mnuGridInterval100 = new ToolStripMenuItem();
            mnuGridInterval250 = new ToolStripMenuItem();
            mnuGridInterval500 = new ToolStripMenuItem();
            mnuGridInterval1000 = new ToolStripMenuItem();
            mnuShowTargetInfo = new ToolStripMenuItem();
            mnuSmallTargetInfo = new ToolStripMenuItem();
            mnuAutoSelectEQTarget = new ToolStripMenuItem();
            toolStripSeparator10 = new ToolStripSeparator();
            mnuFollowNone = new ToolStripMenuItem();
            mnuFollowPlayer = new ToolStripMenuItem();
            mnuFollowTarget = new ToolStripMenuItem();
            toolStripSeparator11 = new ToolStripSeparator();
            mnuKeepCentered = new ToolStripMenuItem();
            mnuAutoExpand = new ToolStripMenuItem();
            toolStripSeparator13 = new ToolStripSeparator();
            mnuMapReset = new ToolStripMenuItem();
            mnuHelpMain = new ToolStripMenuItem();
            mnuAbout = new ToolStripMenuItem();
            mnuContext = new ContextMenuStrip(components);
            mnuDepthFilter2 = new ToolStripMenuItem();
            toolStripMenuItem2 = new ToolStripMenuItem();
            mnuDynamicAlpha2 = new ToolStripMenuItem();
            mnuFilterMapLines2 = new ToolStripMenuItem();
            mnuFilterMapText2 = new ToolStripMenuItem();
            mnuFilterNPCs2 = new ToolStripMenuItem();
            mnuFilterNPCCorpses2 = new ToolStripMenuItem();
            mnuFilterPlayers2 = new ToolStripMenuItem();
            mnuFilterPlayerCorpses2 = new ToolStripMenuItem();
            mnuFilterGroundItems2 = new ToolStripMenuItem();
            mnuFilterSpawnPoints2 = new ToolStripMenuItem();
            mnuForceDistinct2 = new ToolStripMenuItem();
            mnuForceDistinctText2 = new ToolStripMenuItem();
            toolStripSeparator6 = new ToolStripSeparator();
            addMapTextToolStripMenuItem = new ToolStripMenuItem();
            mnuLabelShow2 = new ToolStripMenuItem();
            mnuShowNPCLevels2 = new ToolStripMenuItem();
            mnuShowNPCNames2 = new ToolStripMenuItem();
            mnuShowNPCCorpseNames2 = new ToolStripMenuItem();
            mnuShowPCNames2 = new ToolStripMenuItem();
            mnuShowPlayerCorpseNames2 = new ToolStripMenuItem();
//            mnuShowPCGuild2 = new ToolStripMenuItem();
            mnuSpawnCountdown2 = new ToolStripMenuItem();
            mnuShowSpawnPoints2 = new ToolStripMenuItem();
            mnuShowZoneText2 = new ToolStripMenuItem();
            mnuShowLayer21 = new ToolStripMenuItem();
            mnuShowLayer22 = new ToolStripMenuItem();
            mnuShowLayer23 = new ToolStripMenuItem();
            mnuShowPVP2 = new ToolStripMenuItem();
            mnuShowPVPLevel2 = new ToolStripMenuItem();
            mnuShowTargetInfo2 = new ToolStripMenuItem();
            mnuSmallTargetInfo2 = new ToolStripMenuItem();
            mnuAutoSelectEQTarget2 = new ToolStripMenuItem();
            toolStripSeparator15 = new ToolStripSeparator();
            mnuFollowNone2 = new ToolStripMenuItem();
            mnuFollowPlayer2 = new ToolStripMenuItem();
            mnuFollowTarget2 = new ToolStripMenuItem();
            toolStripSeparator16 = new ToolStripSeparator();
            mnuKeepCentered2 = new ToolStripMenuItem();
            mnuAutoExpand2 = new ToolStripMenuItem();
            toolStripSeparator17 = new ToolStripSeparator();
            mnuShowMenuBar = new ToolStripMenuItem();
            mnuMapReset2 = new ToolStripMenuItem();
            openFileDialog = new OpenFileDialog();
            fontDialog1 = new FontDialog();
            mnuContextAddFilter = new ContextMenuStrip(components);
            mnuMobName = new ToolStripMenuItem();
            menuItem11 = new ToolStripSeparator();
            mnuAddHuntFilter = new ToolStripMenuItem();
            mnuAddCautionFilter = new ToolStripMenuItem();
            mnuAddDangerFilter = new ToolStripMenuItem();
            mnuAddAlertFilter = new ToolStripMenuItem();
            toolStripBasecon = new ToolStripMenuItem();
            mnuSepAddFilter = new ToolStripSeparator();
            mnuAddMapLabel = new ToolStripMenuItem();
            toolStripSepAddMapLabel = new ToolStripSeparator();
            mnuSearchAllakhazam = new ToolStripMenuItem();
            timPackets = new Timer(components);
            timDelayAlerts = new System.Timers.Timer();
            timProcessTimers = new System.Timers.Timer();
            colorPicker = new ColorDialog();
            mnuShowListNPCs = new ToolStripMenuItem();
            mnuShowListCorpses = new ToolStripMenuItem();
            mnuShowListPlayers = new ToolStripMenuItem();
            mnuShowListInvis = new ToolStripMenuItem();
            mnuShowListMounts = new ToolStripMenuItem();
            mnuShowListFamiliars = new ToolStripMenuItem();
            mnuShowListPets = new ToolStripMenuItem();
            statusBarStrip = new StatusStrip();
            toolStripMouseLocation = new ToolStripStatusLabel();
            toolStripDistance = new ToolStripStatusLabel();
            toolStripSpring = new ToolStripStatusLabel();
            toolStripVersion = new ToolStripStatusLabel();
            toolStripServerAddress = new ToolStripStatusLabel();
            toolStripCoPStatus = new ToolStripStatusLabel();
            toolStripShortName = new ToolStripStatusLabel();
            toolStripFPS = new ToolStripStatusLabel();
            toolBarStrip = new ToolStrip();
            toolStripStartStop = new ToolStripButton();
            toolStripLevel = new ToolStripComboBox();
            toolStripSeparator14 = new ToolStripSeparator();
            toolStripZoomIn = new ToolStripButton();
            toolStripZoomOut = new ToolStripButton();
            toolStripScale = new ToolStripComboBox();
            toolStripDepthFilterButton = new ToolStripButton();
            toolStripZPosLabel = new ToolStripLabel();
            toolStripZPos = new ToolStripTextBox();
            toolStripZPosDown = new ToolStripButton();
            toolStripZPosUp = new ToolStripButton();
            toolStripZOffsetLabel = new ToolStripLabel();
            toolStripZNeg = new ToolStripTextBox();
            toolStripZNegUp = new ToolStripButton();
            toolStripZNegDown = new ToolStripButton();
            toolStripResetDepthFilter = new ToolStripButton();
            toolStripOptions = new ToolStripButton();
            toolStripSeparator19 = new ToolStripSeparator();
            toolStripLabel1 = new ToolStripLabel();
            toolStripLookupBox = new ToolStripTextBox();
            toolStripCheckLookup = new ToolStripButton();
            toolStripResetLookup = new ToolStripButton();
            toolStripLookupBox1 = new ToolStripTextBox();
            toolStripCheckLookup1 = new ToolStripButton();
            toolStripResetLookup1 = new ToolStripButton();
            toolStripLookupBox2 = new ToolStripTextBox();
            toolStripCheckLookup2 = new ToolStripButton();
            toolStripResetLookup2 = new ToolStripButton();
            toolStripLookupBox3 = new ToolStripTextBox();
            toolStripCheckLookup3 = new ToolStripButton();
            toolStripResetLookup3 = new ToolStripButton();
            toolStripLookupBox4 = new ToolStripTextBox();
            toolStripCheckLookup4 = new ToolStripButton();
            toolStripResetLookup4 = new ToolStripButton();
            toolStripLookupBox5 = new ToolStripTextBox();
            toolStripCheckLookup5 = new ToolStripButton();
            toolStripResetLookup5 = new ToolStripButton();
            dockPanel = new DockPanel();
            mnuMainMenu.SuspendLayout();
            mnuContext.SuspendLayout();
            mnuContextAddFilter.SuspendLayout();
            timDelayAlerts.BeginInit();
            timProcessTimers.BeginInit();
            statusBarStrip.SuspendLayout();
            toolBarStrip.SuspendLayout();
            SuspendLayout();
            // 
            // mnuMainMenu
            // 
            mnuMainMenu.BackColor = SystemColors.ControlLight;
            mnuMainMenu.Items.AddRange(new ToolStripItem[] {
            mnuFileMain,
            mnuEditMain,
            mnuViewMain,
            mnuMapSettingsMain,
            mnuHelpMain});
            mnuMainMenu.Location = new Point(0, 0);
            mnuMainMenu.Name = "mnuMainMenu";
            mnuMainMenu.Size = new Size(1464, 24);
            mnuMainMenu.TabIndex = 0;
            mnuMainMenu.Text = "mnuMainMenu";
            // 
            // mnuFileMain
            // 
            mnuFileMain.DropDownItems.AddRange(new ToolStripItem[] {
            mnuOptions,
            mnuSavePrefs,
            toolStripSeparator2,
            mnuOpenMap,
            mnuSaveMobs,
            toolStripSeparator1,
            mnuConnect,
            mnuAutoConnect,
            mnuServerSelection,
            mnuCharSelect,
            toolStripSeparator3,
            mnuExit});
            mnuFileMain.Name = "mnuFileMain";
            mnuFileMain.Size = new Size(37, 20);
            mnuFileMain.Text = "&File";
            mnuFileMain.DropDownOpening += new EventHandler(MnuFileMain_DropDownOpening);
            // 
            // mnuOptions
            // 
            mnuOptions.Image = (Image)resources.GetObject("mnuOptions.Image");
            mnuOptions.ImageTransparentColor = Color.Magenta;
            mnuOptions.Name = "mnuOptions";
            mnuOptions.ShortcutKeys = Keys.F1;
            mnuOptions.Size = new Size(180, 22);
            mnuOptions.Text = "&Options";
            mnuOptions.Click += new EventHandler(MnuOptions_Click);
            // 
            // mnuSavePrefs
            // 
            mnuSavePrefs.Image = (Image)resources.GetObject("mnuSavePrefs.Image");
            mnuSavePrefs.Name = "mnuSavePrefs";
            mnuSavePrefs.Size = new Size(180, 22);
            mnuSavePrefs.Text = "Save &Prefs";
            mnuSavePrefs.Click += new EventHandler(MnuSavePrefs_Click);
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(177, 6);
            // 
            // mnuOpenMap
            // 
            mnuOpenMap.Image = (Image)resources.GetObject("mnuOpenMap.Image");
            mnuOpenMap.Name = "mnuOpenMap";
            mnuOpenMap.ShortcutKeys = Keys.Control | Keys.O;
            mnuOpenMap.Size = new Size(180, 22);
            mnuOpenMap.Text = "&Open Map";
            mnuOpenMap.Click += new EventHandler(MnuOpenMap_Click);
            // 
            // mnuSaveMobs
            // 
            mnuSaveMobs.Name = "mnuSaveMobs";
            mnuSaveMobs.ShortcutKeys = Keys.Control | Keys.S;
            mnuSaveMobs.Size = new Size(180, 22);
            mnuSaveMobs.Text = "&Save Mobs";
            mnuSaveMobs.Click += new EventHandler(MnuSaveMobs_Click);
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(177, 6);
            // 
            // mnuConnect
            // 
            mnuConnect.Image = (Image)resources.GetObject("mnuConnect.Image");
            mnuConnect.ImageScaling = ToolStripItemImageScaling.None;
            mnuConnect.Name = "mnuConnect";
            mnuConnect.Size = new Size(180, 22);
            mnuConnect.Text = "&Connect";
            mnuConnect.Click += new EventHandler(CmdCommand_Click);
            // 
            // mnuAutoConnect
            // 
            mnuAutoConnect.Name = "mnuAutoConnect";
            mnuAutoConnect.Size = new Size(180, 22);
            mnuAutoConnect.Text = "Connect on Startup";
            mnuAutoConnect.Click += new EventHandler(MnuAutoConnect_Click);
            // 
            // mnuServerSelection
            // 
            mnuServerSelection.DropDownItems.AddRange(new ToolStripItem[] {
            mnuIPAddress1,
            mnuIPAddress2,
            mnuIPAddress3,
            mnuIPAddress4,
            mnuIPAddress5});
            mnuServerSelection.Name = "mnuServerSelection";
            mnuServerSelection.Size = new Size(180, 22);
            mnuServerSelection.Text = "&Server Selection";
            // 
            // mnuIPAddress1
            // 
            mnuIPAddress1.Name = "mnuIPAddress1";
            mnuIPAddress1.ShortcutKeys = Keys.Control | Keys.D1;
            mnuIPAddress1.Size = new Size(180, 22);
            mnuIPAddress1.Click += new EventHandler(MnuIPAddress1_Click);
            // 
            // mnuIPAddress2
            // 
            mnuIPAddress2.Name = "mnuIPAddress2";
            mnuIPAddress2.ShortcutKeys = Keys.Control | Keys.D2;
            mnuIPAddress2.Size = new Size(180, 22);
            mnuIPAddress2.Click += new EventHandler(MnuIPAddress2_Click);
            // 
            // mnuIPAddress3
            // 
            mnuIPAddress3.Name = "mnuIPAddress3";
            mnuIPAddress3.ShortcutKeys = Keys.Control | Keys.D3;
            mnuIPAddress3.Size = new Size(180, 22);
            mnuIPAddress3.Click += new EventHandler(MnuIPAddress3_Click);
            // 
            // mnuIPAddress4
            // 
            mnuIPAddress4.Name = "mnuIPAddress4";
            mnuIPAddress4.ShortcutKeys = Keys.Control | Keys.D4;
            mnuIPAddress4.Size = new Size(180, 22);
            mnuIPAddress4.Click += new EventHandler(MnuIPAddress4_Click);
            // 
            // mnuIPAddress5
            // 
            mnuIPAddress5.Name = "mnuIPAddress5";
            mnuIPAddress5.ShortcutKeys = Keys.Control | Keys.D5;
            mnuIPAddress5.Size = new Size(180, 22);
            mnuIPAddress5.Click += new EventHandler(MnuIPAddress5_Click);
            // 
            // mnuCharSelect
            // 
            mnuCharSelect.DropDownItems.AddRange(new ToolStripItem[] {
            mnuChar1,
            mnuChar2,
            mnuChar3,
            mnuChar4,
            mnuChar5,
            mnuChar6,
            mnuChar7,
            mnuChar8,
            mnuChar9,
            mnuChar10,
            mnuChar11,
            mnuChar12,
            menuItem13,
            mnuCharRefresh});
            mnuCharSelect.Name = "mnuCharSelect";
            mnuCharSelect.Overflow = ToolStripItemOverflow.Always;
            mnuCharSelect.Size = new Size(180, 22);
            mnuCharSelect.Text = "&Character Selection";
            // 
            // mnuChar1
            // 
            mnuChar1.Checked = true;
            mnuChar1.CheckState = CheckState.Checked;
            mnuChar1.Name = "mnuChar1";
            mnuChar1.ShortcutKeys = Keys.Control | Keys.Shift
            | Keys.D1;
            mnuChar1.Size = new Size(188, 22);
            mnuChar1.Text = "Char 1";
            mnuChar1.Visible = false;
            mnuChar1.Click += new EventHandler(MnuChar1_Click);
            // 
            // mnuChar2
            // 
            mnuChar2.Name = "mnuChar2";
            mnuChar2.ShortcutKeys = Keys.Control | Keys.Shift
            | Keys.D2;
            mnuChar2.Size = new Size(188, 22);
            mnuChar2.Text = "Char 2";
            mnuChar2.Visible = false;
            mnuChar2.Click += new EventHandler(MnuChar2_Click);
            // 
            // mnuChar3
            // 
            mnuChar3.Name = "mnuChar3";
            mnuChar3.ShortcutKeys = Keys.Control | Keys.Shift
            | Keys.D3;
            mnuChar3.Size = new Size(188, 22);
            mnuChar3.Text = "Char 3";
            mnuChar3.Visible = false;
            mnuChar3.Click += new EventHandler(MnuChar3_Click);
            // 
            // mnuChar4
            // 
            mnuChar4.Name = "mnuChar4";
            mnuChar4.ShortcutKeys = Keys.Control | Keys.Shift
            | Keys.D4;
            mnuChar4.Size = new Size(188, 22);
            mnuChar4.Text = "Char 4";
            mnuChar4.Visible = false;
            mnuChar4.Click += new EventHandler(MnuChar4_Click);
            // 
            // mnuChar5
            // 
            mnuChar5.Name = "mnuChar5";
            mnuChar5.ShortcutKeys = Keys.Control | Keys.Shift
            | Keys.D5;
            mnuChar5.Size = new Size(188, 22);
            mnuChar5.Text = "Char 5";
            mnuChar5.Visible = false;
            mnuChar5.Click += new EventHandler(MnuChar5_Click);
            // 
            // mnuChar6
            // 
            mnuChar6.Name = "mnuChar6";
            mnuChar6.ShortcutKeys = Keys.Control | Keys.Shift
            | Keys.D6;
            mnuChar6.Size = new Size(188, 22);
            mnuChar6.Text = "Char 6";
            mnuChar6.Visible = false;
            mnuChar6.Click += new EventHandler(MnuChar6_Click);
            // 
            // mnuChar7
            // 
            mnuChar7.Name = "mnuChar7";
            mnuChar7.ShortcutKeys = Keys.Control | Keys.Shift
            | Keys.D7;
            mnuChar7.Size = new Size(188, 22);
            mnuChar7.Text = "Char 7";
            mnuChar7.Visible = false;
            mnuChar7.Click += new EventHandler(MnuChar7_Click);
            // 
            // mnuChar8
            // 
            mnuChar8.Name = "mnuChar8";
            mnuChar8.ShortcutKeys = Keys.Control | Keys.Shift
            | Keys.D8;
            mnuChar8.Size = new Size(188, 22);
            mnuChar8.Text = "Char 8";
            mnuChar8.Visible = false;
            mnuChar8.Click += new EventHandler(MnuChar8_Click);
            // 
            // mnuChar9
            // 
            mnuChar9.Name = "mnuChar9";
            mnuChar9.ShortcutKeys = Keys.Control | Keys.Shift
            | Keys.D9;
            mnuChar9.Size = new Size(188, 22);
            mnuChar9.Text = "Char 9";
            mnuChar9.Visible = false;
            mnuChar9.Click += new EventHandler(MnuChar9_Click);
            // 
            // mnuChar10
            // 
            mnuChar10.Name = "mnuChar10";
            mnuChar10.ShortcutKeys = Keys.Control | Keys.Shift
            | Keys.D0;
            mnuChar10.Size = new Size(188, 22);
            mnuChar10.Text = "Char 10";
            mnuChar10.Visible = false;
            mnuChar10.Click += new EventHandler(MnuChar10_Click);
            // 
            // mnuChar11
            // 
            mnuChar11.Name = "mnuChar11";
            mnuChar11.ShortcutKeys = Keys.Control | Keys.Shift
            | Keys.B;
            mnuChar11.Size = new Size(188, 22);
            mnuChar11.Text = "Char 11";
            mnuChar11.Visible = false;
            mnuChar11.Click += new EventHandler(MnuChar11_Click);
            // 
            // mnuChar12
            // 
            mnuChar12.Name = "mnuChar12";
            mnuChar12.ShortcutKeys = Keys.Control | Keys.Shift
            | Keys.C;
            mnuChar12.Size = new Size(188, 22);
            mnuChar12.Text = "Char 12";
            mnuChar12.Visible = false;
            mnuChar12.Click += new EventHandler(MnuChar12_Click);
            // 
            // menuItem13
            // 
            menuItem13.Name = "menuItem13";
            menuItem13.Size = new Size(185, 6);
            menuItem13.Visible = false;
            // 
            // mnuCharRefresh
            // 
            mnuCharRefresh.Name = "mnuCharRefresh";
            mnuCharRefresh.Size = new Size(188, 22);
            mnuCharRefresh.Text = "Refresh List";
            mnuCharRefresh.Visible = false;
            mnuCharRefresh.Click += new EventHandler(MnuCharRefresh_Click);
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(177, 6);
            // 
            // mnuExit
            // 
            mnuExit.Name = "mnuExit";
            mnuExit.ShortcutKeys = Keys.Control | Keys.X;
            mnuExit.Size = new Size(180, 22);
            mnuExit.Text = "E&xit";
            mnuExit.Click += new EventHandler(MnuExit_Click);
            // 
            // mnuEditMain
            // 
            mnuEditMain.DropDownItems.AddRange(new ToolStripItem[] {
            mnuChangeColor,
            mnuChangeFont,
            toolStripSeparator18,
            mnuReloadAlerts,
            toolStripSeparator4,
            mnuEditGlobalAlerts,
            mnuEditZoneAlerts,
            toolStripSeparator5,
            mnuRefreshSpawnList,
            mnuClearSavedTimers,
            mnuSaveSpawnLog});
            mnuEditMain.Name = "mnuEditMain";
            mnuEditMain.Size = new Size(39, 20);
            mnuEditMain.Text = "&Edit";
            // 
            // mnuChangeColor
            // 
            mnuChangeColor.DropDownItems.AddRange(new ToolStripItem[] {
            mnuGridColor,
            mnuGridLabelColor,
            mnuListColor,
            mnuBackgroungColor});
            mnuChangeColor.Name = "mnuChangeColor";
            mnuChangeColor.Size = new Size(173, 22);
            mnuChangeColor.Text = "C&hange Color";
            // 
            // mnuGridColor
            // 
            mnuGridColor.Name = "mnuGridColor";
            mnuGridColor.Size = new Size(197, 22);
            mnuGridColor.Text = "Grid Color";
            mnuGridColor.Click += new EventHandler(MnuGridColor_Click);
            // 
            // mnuGridLabelColor
            // 
            mnuGridLabelColor.Name = "mnuGridLabelColor";
            mnuGridLabelColor.Size = new Size(197, 22);
            mnuGridLabelColor.Text = "Grid Label Color";
            mnuGridLabelColor.Click += new EventHandler(MnuGridLabelColor_Click);
            // 
            // mnuListColor
            // 
            mnuListColor.Name = "mnuListColor";
            mnuListColor.Size = new Size(197, 22);
            mnuListColor.Text = "Spawn List Color";
            mnuListColor.Click += new EventHandler(MnuListColor_Click);
            // 
            // mnuBackgroungColor
            // 
            mnuBackgroungColor.Name = "mnuBackgroungColor";
            mnuBackgroungColor.Size = new Size(197, 22);
            mnuBackgroungColor.Text = "Map Background Color";
            mnuBackgroungColor.Click += new EventHandler(MnuBackgroundColor_Click);
            // 
            // mnuChangeFont
            // 
            mnuChangeFont.DropDownItems.AddRange(new ToolStripItem[] {
            mnuSpawnListFont,
            mnuTargetInfoFont,
            mnuMapLabelsFont});
            mnuChangeFont.Name = "mnuChangeFont";
            mnuChangeFont.Size = new Size(173, 22);
            mnuChangeFont.Text = "Change &Font";
            // 
            // mnuSpawnListFont
            // 
            mnuSpawnListFont.Name = "mnuSpawnListFont";
            mnuSpawnListFont.Size = new Size(161, 22);
            mnuSpawnListFont.Text = "Spawn List Font";
            mnuSpawnListFont.Click += new EventHandler(MnuSpawnListFont_Click);
            // 
            // mnuTargetInfoFont
            // 
            mnuTargetInfoFont.Name = "mnuTargetInfoFont";
            mnuTargetInfoFont.Size = new Size(161, 22);
            mnuTargetInfoFont.Text = "Target Info Font";
            mnuTargetInfoFont.Click += new EventHandler(MnuTargetInfoFont_Click);
            // 
            // mnuMapLabelsFont
            // 
            mnuMapLabelsFont.Name = "mnuMapLabelsFont";
            mnuMapLabelsFont.Size = new Size(161, 22);
            mnuMapLabelsFont.Text = "Map Labels Font";
            mnuMapLabelsFont.Click += new EventHandler(MnuMapLabelsFont_Click);
            // 
            // toolStripSeparator18
            // 
            toolStripSeparator18.Name = "toolStripSeparator18";
            toolStripSeparator18.Size = new Size(170, 6);
            // 
            // mnuReloadAlerts
            // 
            mnuReloadAlerts.Image = (Image)resources.GetObject("mnuReloadAlerts.Image");
            mnuReloadAlerts.ImageScaling = ToolStripItemImageScaling.None;
            mnuReloadAlerts.Name = "mnuReloadAlerts";
            mnuReloadAlerts.Size = new Size(173, 22);
            mnuReloadAlerts.Text = "&Reload Alerts";
            mnuReloadAlerts.Click += new EventHandler(MnuReloadAlerts_Click);
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new Size(170, 6);
            // 
            // mnuEditGlobalAlerts
            // 
            mnuEditGlobalAlerts.Name = "mnuEditGlobalAlerts";
            mnuEditGlobalAlerts.Size = new Size(173, 22);
            mnuEditGlobalAlerts.Text = "Edit &Global Alerts";
            mnuEditGlobalAlerts.Click += new EventHandler(MnuGlobalAlerts_Click);
            // 
            // mnuEditZoneAlerts
            // 
            mnuEditZoneAlerts.Name = "mnuEditZoneAlerts";
            mnuEditZoneAlerts.Size = new Size(173, 22);
            mnuEditZoneAlerts.Text = "Edit &Zone Alerts";
            mnuEditZoneAlerts.Click += new EventHandler(MnuAddEditAlerts_Click);
            // 
            // toolStripSeparator5
            // 
            toolStripSeparator5.Name = "toolStripSeparator5";
            toolStripSeparator5.Size = new Size(170, 6);
            // 
            // mnuRefreshSpawnList
            // 
            mnuRefreshSpawnList.Name = "mnuRefreshSpawnList";
            mnuRefreshSpawnList.Size = new Size(173, 22);
            mnuRefreshSpawnList.Text = "&Refresh Spawn List";
            mnuRefreshSpawnList.Click += new EventHandler(MnuRefreshSpawnList_Click);
            // 
            // mnuClearSavedTimers
            // 
            mnuClearSavedTimers.Name = "mnuClearSavedTimers";
            mnuClearSavedTimers.Size = new Size(173, 22);
            mnuClearSavedTimers.Text = "Clear Saved &Timers";
            mnuClearSavedTimers.Click += new EventHandler(MnuClearSavedTimers_Click);
            // 
            // mnuSaveSpawnLog
            // 
            mnuSaveSpawnLog.Name = "mnuSaveSpawnLog";
            mnuSaveSpawnLog.Size = new Size(173, 22);
            mnuSaveSpawnLog.Text = "Save Spawn Log";
            mnuSaveSpawnLog.Click += new EventHandler(MnuSaveSpawnLog_Click);
            // 
            // mnuViewMain
            // 
            mnuViewMain.DropDownItems.AddRange(new ToolStripItem[] {
            toolbarsToolStripMenuItem,
            toolStripSeparator9,
            mnuShowSpawnList,
            mnuShowSpawnListTimer,
            mnuShowGroundItemList,
            toolStripSeparator7,
            mnuShowListGridLines,
            mnuShowListSearchBox,
            mnuShowGridLines,
            toolStripSeparator8,
            mnuShowCorpses,
            mnuShowPCCorpses,
            mnuShowMyCorpse,
            mnuShowPlayers,
            mnuShowInvis,
            mnuShowMounts,
            mnuShowFamiliars,
            mnuShowPets,
            mnuShowNPCs,
            mnuShowLookupText,
            mnuShowLookupNumber,
            mnuAlwaysOnTop});
            mnuViewMain.Name = "mnuViewMain";
            mnuViewMain.Size = new Size(44, 20);
            mnuViewMain.Text = "&View";
            // 
            // toolbarsToolStripMenuItem
            // 
            toolbarsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
            mnuViewMenuBar,
            mnuViewStatusBar,
            mnuViewDepthFilterBar});
            toolbarsToolStripMenuItem.Name = "toolbarsToolStripMenuItem";
            toolbarsToolStripMenuItem.Size = new Size(198, 22);
            toolbarsToolStripMenuItem.Text = "Toolbars";
            // 
            // mnuViewMenuBar
            // 
            mnuViewMenuBar.Name = "mnuViewMenuBar";
            mnuViewMenuBar.ShortcutKeys = Keys.F2;
            mnuViewMenuBar.Size = new Size(162, 22);
            mnuViewMenuBar.Text = "&Main Menu";
            mnuViewMenuBar.Click += new EventHandler(MnuShowMenuBar_Click);
            // 
            // mnuViewStatusBar
            // 
            mnuViewStatusBar.Name = "mnuViewStatusBar";
            mnuViewStatusBar.ShortcutKeys = Keys.F3;
            mnuViewStatusBar.Size = new Size(162, 22);
            mnuViewStatusBar.Text = "&Status";
            mnuViewStatusBar.Click += new EventHandler(MnuViewStatusBar_Click);
            // 
            // mnuViewDepthFilterBar
            // 
            mnuViewDepthFilterBar.Name = "mnuViewDepthFilterBar";
            mnuViewDepthFilterBar.ShortcutKeys = Keys.F4;
            mnuViewDepthFilterBar.Size = new Size(162, 22);
            mnuViewDepthFilterBar.Text = "&Tool Bar Strip";
            mnuViewDepthFilterBar.Click += new EventHandler(MnuViewDepthFilterToolBar_Click);
            // 
            // toolStripSeparator9
            // 
            toolStripSeparator9.Name = "toolStripSeparator9";
            toolStripSeparator9.Size = new Size(195, 6);
            // 
            // mnuShowSpawnList
            // 
            mnuShowSpawnList.Name = "mnuShowSpawnList";
            mnuShowSpawnList.Size = new Size(198, 22);
            mnuShowSpawnList.Text = "Spawn &List";
            mnuShowSpawnList.Click += new EventHandler(MnuShowSpawnList_Click);
            // 
            // mnuShowSpawnListTimer
            // 
            mnuShowSpawnListTimer.Name = "mnuShowSpawnListTimer";
            mnuShowSpawnListTimer.Size = new Size(198, 22);
            mnuShowSpawnListTimer.Text = "Spawn &Timer List";
            mnuShowSpawnListTimer.Click += new EventHandler(MnuShowSpawnListTimer_Click);
            // 
            // mnuShowGroundItemList
            // 
            mnuShowGroundItemList.Name = "mnuShowGroundItemList";
            mnuShowGroundItemList.Size = new Size(198, 22);
            mnuShowGroundItemList.Text = "Ground &Item List";
            mnuShowGroundItemList.Click += new EventHandler(MnuShowGroundItemList_Click);
            // 
            // toolStripSeparator7
            // 
            toolStripSeparator7.Name = "toolStripSeparator7";
            toolStripSeparator7.Size = new Size(195, 6);
            // 
            // mnuShowListGridLines
            // 
            mnuShowListGridLines.Name = "mnuShowListGridLines";
            mnuShowListGridLines.Size = new Size(198, 22);
            mnuShowListGridLines.Text = "List &Grid Lines";
            mnuShowListGridLines.Click += new EventHandler(MnuShowListGridLines_Click);
            // 
            // mnuShowListSearchBox
            // 
            mnuShowListSearchBox.Name = "mnuShowListSearchBox";
            mnuShowListSearchBox.Size = new Size(198, 22);
            mnuShowListSearchBox.Text = "List Search Box";
            mnuShowListSearchBox.Click += new EventHandler(MnuShowListSearchBox_Click);
            // 
            // mnuShowGridLines
            // 
            mnuShowGridLines.Image = (Image)resources.GetObject("mnuShowGridLines.Image");
            mnuShowGridLines.Name = "mnuShowGridLines";
            mnuShowGridLines.ShortcutKeys = Keys.F6;
            mnuShowGridLines.Size = new Size(198, 22);
            mnuShowGridLines.Text = "Map Grid Lines";
            mnuShowGridLines.Click += new EventHandler(MnuShowGridLines_Click);
            // 
            // toolStripSeparator8
            // 
            toolStripSeparator8.Name = "toolStripSeparator8";
            toolStripSeparator8.Size = new Size(195, 6);
            // 
            // mnuShowCorpses
            // 
            mnuShowCorpses.Name = "mnuShowCorpses";
            mnuShowCorpses.Size = new Size(198, 22);
            mnuShowCorpses.Text = "NPC Corpses";
            mnuShowCorpses.Click += new EventHandler(MnuShowCorpses_Click);
            // 
            // mnuShowPCCorpses
            // 
            mnuShowPCCorpses.Name = "mnuShowPCCorpses";
            mnuShowPCCorpses.Size = new Size(198, 22);
            mnuShowPCCorpses.Text = "PC Corpses";
            mnuShowPCCorpses.Click += new EventHandler(MnuShowPCCorpses_Click);
            // 
            // mnuShowMyCorpse
            // 
            mnuShowMyCorpse.Name = "mnuShowMyCorpse";
            mnuShowMyCorpse.Size = new Size(198, 22);
            mnuShowMyCorpse.Text = "My Corpse";
            mnuShowMyCorpse.Click += new EventHandler(MnuShowMyCorpse_Click);
            // 
            // mnuShowPlayers
            // 
            mnuShowPlayers.Name = "mnuShowPlayers";
            mnuShowPlayers.Size = new Size(198, 22);
            mnuShowPlayers.Text = "Players";
            mnuShowPlayers.Click += new EventHandler(MnuShowPlayers_Click);
            // 
            // mnuShowInvis
            // 
            mnuShowInvis.Name = "mnuShowInvis";
            mnuShowInvis.Size = new Size(198, 22);
            mnuShowInvis.Text = "Invis Mobs";
            mnuShowInvis.Click += new EventHandler(MnuShowInvis_Click);
            // 
            // mnuShowMounts
            // 
            mnuShowMounts.Name = "mnuShowMounts";
            mnuShowMounts.Size = new Size(198, 22);
            mnuShowMounts.Text = "Mounts";
            mnuShowMounts.Click += new EventHandler(MnuShowMounts_Click);
            // 
            // mnuShowFamiliars
            // 
            mnuShowFamiliars.Name = "mnuShowFamiliars";
            mnuShowFamiliars.Size = new Size(198, 22);
            mnuShowFamiliars.Text = "Familiars";
            mnuShowFamiliars.Click += new EventHandler(MnuShowFamiliars_Click);
            // 
            // mnuShowPets
            // 
            mnuShowPets.Name = "mnuShowPets";
            mnuShowPets.Size = new Size(198, 22);
            mnuShowPets.Text = "Pets";
            mnuShowPets.Click += new EventHandler(MnuShowPets_Click);
            // 
            // mnuShowNPCs
            // 
            mnuShowNPCs.Name = "mnuShowNPCs";
            mnuShowNPCs.Size = new Size(198, 22);
            mnuShowNPCs.Text = "NPCs";
            mnuShowNPCs.Click += new EventHandler(MnuShowNPCs_Click);
            // 
            // mnuShowLookupText
            // 
            mnuShowLookupText.Name = "mnuShowLookupText";
            mnuShowLookupText.Size = new Size(198, 22);
            mnuShowLookupText.Text = "Lookup Text";
            mnuShowLookupText.Click += new EventHandler(MnuShowLookupText_Click);
            // 
            // mnuShowLookupNumber
            // 
            mnuShowLookupNumber.Name = "mnuShowLookupNumber";
            mnuShowLookupNumber.Size = new Size(198, 22);
            mnuShowLookupNumber.Text = "Lookup Name/Number";
            mnuShowLookupNumber.Click += new EventHandler(MnuShowLookupNumber_Click);
            // 
            // mnuAlwaysOnTop
            // 
            mnuAlwaysOnTop.Name = "mnuAlwaysOnTop";
            mnuAlwaysOnTop.ShortcutKeys = Keys.Alt | Keys.T;
            mnuAlwaysOnTop.Size = new Size(198, 22);
            mnuAlwaysOnTop.Text = "Always On Top";
            mnuAlwaysOnTop.Click += new EventHandler(MnuAlwaysOnTop_Click);
            // 
            // mnuMapSettingsMain
            // 
            mnuMapSettingsMain.DropDownItems.AddRange(new ToolStripItem[] {
            mnuDepthFilter,
            menuItem3,
            mnuForceDistinct,
            mnuForceDistinctText,
            toolStripSeparator12,
            mnuLabelShow,
            mnuCollectMobTrails,
            mnuShowMobTrails,
            mnuConColors,
            mnuGridInterval,
            mnuShowTargetInfo,
            mnuSmallTargetInfo,
            mnuAutoSelectEQTarget,
            toolStripSeparator10,
            mnuFollowNone,
            mnuFollowPlayer,
            mnuFollowTarget,
            toolStripSeparator11,
            mnuKeepCentered,
            mnuAutoExpand,
            toolStripSeparator13,
            mnuMapReset});
            mnuMapSettingsMain.Name = "mnuMapSettingsMain";
            mnuMapSettingsMain.Size = new Size(43, 20);
            mnuMapSettingsMain.Text = "&Map";
            // 
            // mnuDepthFilter
            // 
            mnuDepthFilter.Name = "mnuDepthFilter";
            mnuDepthFilter.ShortcutKeys = Keys.F5;
            mnuDepthFilter.Size = new Size(195, 22);
            mnuDepthFilter.Text = "&Depth Filter On/Off";
            mnuDepthFilter.ToolTipText = "Z-Axis Depth Filtering";
            mnuDepthFilter.Click += new EventHandler(MnuDepthFilter_Click);
            // 
            // menuItem3
            // 
            menuItem3.DropDownItems.AddRange(new ToolStripItem[] {
            mnuDynamicAlpha,
            mnuFilterMapLines,
            mnuFilterMapText,
            mnuFilterNPCs,
            mnuFilterNPCCorpses,
            mnuFilterPlayers,
            mnuFilterPlayerCorpses,
            mnuFilterGroundItems,
            mnuFilterSpawnPoints});
            menuItem3.Name = "menuItem3";
            menuItem3.Size = new Size(195, 22);
            menuItem3.Text = "Depth &Filter Settings";
            // 
            // mnuDynamicAlpha
            // 
            mnuDynamicAlpha.Name = "mnuDynamicAlpha";
            mnuDynamicAlpha.Size = new Size(220, 22);
            mnuDynamicAlpha.Text = "Dynamic &Alpha Faded Lines";
            mnuDynamicAlpha.ToolTipText = "Faded Depth Filtered Lines.";
            mnuDynamicAlpha.Click += new EventHandler(MnuDynamicAlpha_Click);
            // 
            // mnuFilterMapLines
            // 
            mnuFilterMapLines.Name = "mnuFilterMapLines";
            mnuFilterMapLines.Size = new Size(220, 22);
            mnuFilterMapLines.Text = "Filter &Map Lines";
            mnuFilterMapLines.Click += new EventHandler(MnuFilterMapLines_Click);
            // 
            // mnuFilterMapText
            // 
            mnuFilterMapText.Name = "mnuFilterMapText";
            mnuFilterMapText.Size = new Size(220, 22);
            mnuFilterMapText.Text = "Filter Map &Text";
            mnuFilterMapText.Click += new EventHandler(MnuFilterMapText_Click);
            // 
            // mnuFilterNPCs
            // 
            mnuFilterNPCs.Name = "mnuFilterNPCs";
            mnuFilterNPCs.Size = new Size(220, 22);
            mnuFilterNPCs.Text = "Filter &NPCs";
            mnuFilterNPCs.Click += new EventHandler(MnuFilterNPCs_Click);
            // 
            // mnuFilterNPCCorpses
            // 
            mnuFilterNPCCorpses.Name = "mnuFilterNPCCorpses";
            mnuFilterNPCCorpses.Size = new Size(220, 22);
            mnuFilterNPCCorpses.Text = "Filter NPC &Corpses";
            mnuFilterNPCCorpses.Click += new EventHandler(MnuFilterNPCCorpses_Click);
            // 
            // mnuFilterPlayers
            // 
            mnuFilterPlayers.Name = "mnuFilterPlayers";
            mnuFilterPlayers.Size = new Size(220, 22);
            mnuFilterPlayers.Text = "Filter &Players";
            mnuFilterPlayers.Click += new EventHandler(MnuFilterPlayers_Click);
            // 
            // mnuFilterPlayerCorpses
            // 
            mnuFilterPlayerCorpses.Name = "mnuFilterPlayerCorpses";
            mnuFilterPlayerCorpses.Size = new Size(220, 22);
            mnuFilterPlayerCorpses.Text = "Filter Pl&ayer Corpses";
            mnuFilterPlayerCorpses.Click += new EventHandler(MnuFilterPlayerCorpses_Click);
            // 
            // mnuFilterGroundItems
            // 
            mnuFilterGroundItems.Name = "mnuFilterGroundItems";
            mnuFilterGroundItems.Size = new Size(220, 22);
            mnuFilterGroundItems.Text = "Filter &Ground Items";
            mnuFilterGroundItems.Click += new EventHandler(MnuFilterGroundItems_Click);
            // 
            // mnuFilterSpawnPoints
            // 
            mnuFilterSpawnPoints.Name = "mnuFilterSpawnPoints";
            mnuFilterSpawnPoints.Size = new Size(220, 22);
            mnuFilterSpawnPoints.Text = "Filter &Spawn Points";
            mnuFilterSpawnPoints.Click += new EventHandler(MnuFilterSpawnPoints_Click);
            // 
            // mnuForceDistinct
            // 
            mnuForceDistinct.Name = "mnuForceDistinct";
            mnuForceDistinct.Size = new Size(195, 22);
            mnuForceDistinct.Text = "&Force Distinct Lines";
            mnuForceDistinct.Click += new EventHandler(MnuForceDistinct_Click);
            // 
            // mnuForceDistinctText
            // 
            mnuForceDistinctText.Name = "mnuForceDistinctText";
            mnuForceDistinctText.Size = new Size(195, 22);
            mnuForceDistinctText.Text = "Force Distinct &Text";
            mnuForceDistinctText.Click += new EventHandler(MnuForceDistinctText_Click);
            // 
            // toolStripSeparator12
            // 
            toolStripSeparator12.Name = "toolStripSeparator12";
            toolStripSeparator12.Size = new Size(192, 6);
            // 
            // mnuLabelShow
            // 
            mnuLabelShow.DropDownItems.AddRange(new ToolStripItem[] {
            mnuShowNPCLevels,
            mnuShowNPCNames,
            mnuShowNPCCorpseNames,
            mnuShowPCNames,
            mnuShowPlayerCorpseNames,
//            mnuShowPCGuild,
            mnuSpawnCountdown,
            mnuShowSpawnPoints,
            mnuShowZoneText,
            mnuShowLayer1,
            mnuShowLayer2,
            mnuShowLayer3,
            mnuShowPVP,
            mnuShowPVPLevel});
            mnuLabelShow.Name = "mnuLabelShow";
            mnuLabelShow.Size = new Size(195, 22);
            mnuLabelShow.Text = "&Show on Map";
            // 
            // mnuShowNPCLevels
            // 
            mnuShowNPCLevels.Name = "mnuShowNPCLevels";
            mnuShowNPCLevels.Size = new Size(186, 22);
            mnuShowNPCLevels.Text = "NPC L&evels";
            mnuShowNPCLevels.ToolTipText = "Show NPC Levels on map.";
            mnuShowNPCLevels.Click += new EventHandler(MnuShowNPCLevels_Click);
            // 
            // mnuShowNPCNames
            // 
            mnuShowNPCNames.Name = "mnuShowNPCNames";
            mnuShowNPCNames.Size = new Size(186, 22);
            mnuShowNPCNames.Text = "&NPC Names";
            mnuShowNPCNames.ToolTipText = "Show NPC Names on map.";
            mnuShowNPCNames.Click += new EventHandler(MnuShowNPCNames_Click);
            // 
            // mnuShowNPCCorpseNames
            // 
            mnuShowNPCCorpseNames.Name = "mnuShowNPCCorpseNames";
            mnuShowNPCCorpseNames.Size = new Size(186, 22);
            mnuShowNPCCorpseNames.Text = "NPC &Corpse Names";
            mnuShowNPCCorpseNames.ToolTipText = "Show NPC Corpse Names on map.";
            mnuShowNPCCorpseNames.Click += new EventHandler(MnuShowNPCCorpseNames_Click);
            // 
            // mnuShowPCNames
            // 
            mnuShowPCNames.Name = "mnuShowPCNames";
            mnuShowPCNames.Size = new Size(186, 22);
            mnuShowPCNames.Text = "&Player Names";
            mnuShowPCNames.ToolTipText = "Show Player Names on map.";
            mnuShowPCNames.Click += new EventHandler(MnuShowPCNames_Click);
            // 
            // mnuShowPlayerCorpseNames
            // 
            mnuShowPlayerCorpseNames.Name = "mnuShowPlayerCorpseNames";
            mnuShowPlayerCorpseNames.Size = new Size(186, 22);
            mnuShowPlayerCorpseNames.Text = "Player Corpse &Names";
            mnuShowPlayerCorpseNames.ToolTipText = "Show Player Corpse Names on map.";
            mnuShowPlayerCorpseNames.Click += new EventHandler(MnuShowPlayerCorpseNames_Click);
            // 
            // mnuShowPCGuild
            // 
//            mnuShowPCGuild.Name = "mnuShowPCGuild";
//            mnuShowPCGuild.Size = new Size(186, 22);
//            mnuShowPCGuild.Text = "&Player Guild";
//            mnuShowPCGuild.ToolTipText = "Show Player Guild on map.";
////            mnuShowPCGuild.Click += new EventHandler(MnuShowPCGuild_Click);
            // 
            // mnuSpawnCountdown
            // 
            mnuSpawnCountdown.Name = "mnuSpawnCountdown";
            mnuSpawnCountdown.Size = new Size(186, 22);
            mnuSpawnCountdown.Text = "Spawn Countdown";
            mnuSpawnCountdown.ToolTipText = "Show spawn countdown timers on map.";
            mnuSpawnCountdown.Click += new EventHandler(MnuSpawnCountdown_Click);
            // 
            // mnuShowSpawnPoints
            // 
            mnuShowSpawnPoints.Name = "mnuShowSpawnPoints";
            mnuShowSpawnPoints.Size = new Size(186, 22);
            mnuShowSpawnPoints.Text = "&Spawn Points";
            mnuShowSpawnPoints.ToolTipText = "Draw a cross at spawn point on map.";
            mnuShowSpawnPoints.Click += new EventHandler(MnuShowSpawnPoints_Click);
            // 
            // mnuShowZoneText
            // 
            mnuShowZoneText.Name = "mnuShowZoneText";
            mnuShowZoneText.Size = new Size(186, 22);
            mnuShowZoneText.Text = "&Zone Text";
            mnuShowZoneText.Click += new EventHandler(MnuShowZoneText_Click);
            // 
            // mnuShowLayer1
            // 
            mnuShowLayer1.Name = "mnuShowLayer1";
            mnuShowLayer1.Size = new Size(186, 22);
            mnuShowLayer1.Text = "&Show Layer 1";
            mnuShowLayer1.Click += new EventHandler(MnuShowLayer1_Click);
            // 
            // mnuShowLayer2
            // 
            mnuShowLayer2.Name = "mnuShowLayer2";
            mnuShowLayer2.Size = new Size(186, 22);
            mnuShowLayer2.Text = "&Show Layer 2";
            mnuShowLayer2.Click += new EventHandler(MnuShowLayer2_Click);
            // 
            // mnuShowLayer3
            // 
            mnuShowLayer3.Name = "mnuShowLayer3";
            mnuShowLayer3.Size = new Size(186, 22);
            mnuShowLayer3.Text = "&Show Layer 3";
            mnuShowLayer3.Click += new EventHandler(MnuShowLayer3_Click);
            // 
            // mnuShowPVP
            // 
            mnuShowPVP.Name = "mnuShowPVP";
            mnuShowPVP.Size = new Size(186, 22);
            mnuShowPVP.Text = "P&VP";
            mnuShowPVP.Click += new EventHandler(MnuShowPVP_Click);
            // 
            // mnuShowPVPLevel
            // 
            mnuShowPVPLevel.Name = "mnuShowPVPLevel";
            mnuShowPVPLevel.Size = new Size(186, 22);
            mnuShowPVPLevel.Text = "PVP &Level";
            mnuShowPVPLevel.Click += new EventHandler(MnuShowPVPLevel_Click);
            // 
            // mnuCollectMobTrails
            // 
            mnuCollectMobTrails.Name = "mnuCollectMobTrails";
            mnuCollectMobTrails.Size = new Size(195, 22);
            mnuCollectMobTrails.Text = "&Collect Mob Trails";
            mnuCollectMobTrails.Click += new EventHandler(MnuCollectMobTrails_Click);
            // 
            // mnuShowMobTrails
            // 
            mnuShowMobTrails.Name = "mnuShowMobTrails";
            mnuShowMobTrails.ShortcutKeys = Keys.F7;
            mnuShowMobTrails.Size = new Size(195, 22);
            mnuShowMobTrails.Text = "Show &Mob Trails";
            mnuShowMobTrails.Click += new EventHandler(MnuShowMobTrails_Click);
            // 
            // mnuConColors
            // 
            mnuConColors.DropDownItems.AddRange(new ToolStripItem[] {
            mnuConDefault,
            mnuConSoD,
            mnuConSoF});
            mnuConColors.Name = "mnuConColors";
            mnuConColors.Size = new Size(195, 22);
            mnuConColors.Text = "Con Colors";
            // 
            // mnuConDefault
            // 
            mnuConDefault.Name = "mnuConDefault";
            mnuConDefault.Size = new Size(180, 22);
            mnuConDefault.Text = "Default";
            mnuConDefault.Click += new EventHandler(MnuConDefault_Click);
            // 
            // mnuConSoD
            // 
            mnuConSoD.Name = "mnuConSoD";
            mnuConSoD.Size = new Size(180, 22);
            mnuConSoD.Text = "SoD / Titanium";
            mnuConSoD.Click += new EventHandler(MnuSodTitanium_Click);
            // 
            // mnuConSoF
            // 
            mnuConSoF.Name = "mnuConSoF";
            mnuConSoF.Size = new Size(180, 22);
            mnuConSoF.Text = "Secrets of Faydwer";
            mnuConSoF.Click += new EventHandler(MnuConSoF_Click);
            // 
            // mnuGridInterval
            // 
            mnuGridInterval.DropDownItems.AddRange(new ToolStripItem[] {
            mnuGridInterval100,
            mnuGridInterval250,
            mnuGridInterval500,
            mnuGridInterval1000});
            mnuGridInterval.Name = "mnuGridInterval";
            mnuGridInterval.Size = new Size(195, 22);
            mnuGridInterval.Text = "Grid &Interval";
            // 
            // mnuGridInterval100
            // 
            mnuGridInterval100.Name = "mnuGridInterval100";
            mnuGridInterval100.Size = new Size(180, 22);
            mnuGridInterval100.Text = "100";
            mnuGridInterval100.Click += new EventHandler(MnuGridInterval_Click);
            // 
            // mnuGridInterval250
            // 
            mnuGridInterval250.Name = "mnuGridInterval250";
            mnuGridInterval250.Size = new Size(180, 22);
            mnuGridInterval250.Text = "250";
            mnuGridInterval250.Click += new EventHandler(MnuGridInterval_Click);
            // 
            // mnuGridInterval500
            // 
            mnuGridInterval500.Checked = true;
            mnuGridInterval500.CheckState = CheckState.Checked;
            mnuGridInterval500.Name = "mnuGridInterval500";
            mnuGridInterval500.Size = new Size(180, 22);
            mnuGridInterval500.Text = "500";
            mnuGridInterval500.Click += new EventHandler(MnuGridInterval_Click);
            // 
            // mnuGridInterval1000
            // 
            mnuGridInterval1000.Name = "mnuGridInterval1000";
            mnuGridInterval1000.Size = new Size(180, 22);
            mnuGridInterval1000.Text = "1000";
            mnuGridInterval1000.Click += new EventHandler(MnuGridInterval_Click);
            // 
            // mnuShowTargetInfo
            // 
            mnuShowTargetInfo.Name = "mnuShowTargetInfo";
            mnuShowTargetInfo.ShortcutKeys = Keys.F9;
            mnuShowTargetInfo.Size = new Size(195, 22);
            mnuShowTargetInfo.Text = "Show &Target Info";
            mnuShowTargetInfo.Click += new EventHandler(MnuShowTargetInfo_Click);
            // 
            // mnuSmallTargetInfo
            // 
            mnuSmallTargetInfo.Name = "mnuSmallTargetInfo";
            mnuSmallTargetInfo.Size = new Size(195, 22);
            mnuSmallTargetInfo.Text = "Small Target &Info";
            mnuSmallTargetInfo.Click += new EventHandler(MnuSmallTargetInfo_Click);
            // 
            // mnuAutoSelectEQTarget
            // 
            mnuAutoSelectEQTarget.Name = "mnuAutoSelectEQTarget";
            mnuAutoSelectEQTarget.Size = new Size(195, 22);
            mnuAutoSelectEQTarget.Text = "Auto Select &EQ Target";
            mnuAutoSelectEQTarget.Click += new EventHandler(MnuAutoSelectEQTarget_Click);
            // 
            // toolStripSeparator10
            // 
            toolStripSeparator10.Name = "toolStripSeparator10";
            toolStripSeparator10.Size = new Size(192, 6);
            // 
            // mnuFollowNone
            // 
            mnuFollowNone.ImageScaling = ToolStripItemImageScaling.None;
            mnuFollowNone.ImageTransparentColor = Color.Magenta;
            mnuFollowNone.Name = "mnuFollowNone";
            mnuFollowNone.Size = new Size(195, 22);
            mnuFollowNone.Text = "No Follow";
            mnuFollowNone.Click += new EventHandler(MnuFollowNone_Click);
            // 
            // mnuFollowPlayer
            // 
            mnuFollowPlayer.Image = (Image)resources.GetObject("mnuFollowPlayer.Image");
            mnuFollowPlayer.ImageScaling = ToolStripItemImageScaling.None;
            mnuFollowPlayer.ImageTransparentColor = Color.Magenta;
            mnuFollowPlayer.Name = "mnuFollowPlayer";
            mnuFollowPlayer.Size = new Size(195, 22);
            mnuFollowPlayer.Text = "Follow Player";
            mnuFollowPlayer.Click += new EventHandler(MnuFollowPlayer_Click);
            // 
            // mnuFollowTarget
            // 
            mnuFollowTarget.ImageScaling = ToolStripItemImageScaling.None;
            mnuFollowTarget.ImageTransparentColor = Color.Magenta;
            mnuFollowTarget.Name = "mnuFollowTarget";
            mnuFollowTarget.Size = new Size(195, 22);
            mnuFollowTarget.Text = "Follow Target";
            mnuFollowTarget.Click += new EventHandler(MnuFollowTarget_Click);
            // 
            // toolStripSeparator11
            // 
            toolStripSeparator11.Name = "toolStripSeparator11";
            toolStripSeparator11.Size = new Size(192, 6);
            // 
            // mnuKeepCentered
            // 
            mnuKeepCentered.Name = "mnuKeepCentered";
            mnuKeepCentered.Size = new Size(195, 22);
            mnuKeepCentered.Text = "Keep Centered";
            mnuKeepCentered.Click += new EventHandler(MnuKeepCentered_Click);
            // 
            // mnuAutoExpand
            // 
            mnuAutoExpand.Name = "mnuAutoExpand";
            mnuAutoExpand.Size = new Size(195, 22);
            mnuAutoExpand.Text = "Auto Expand";
            mnuAutoExpand.Click += new EventHandler(MnuAutoExpand_Click);
            // 
            // toolStripSeparator13
            // 
            toolStripSeparator13.Name = "toolStripSeparator13";
            toolStripSeparator13.Size = new Size(192, 6);
            // 
            // mnuMapReset
            // 
            mnuMapReset.Name = "mnuMapReset";
            mnuMapReset.Size = new Size(195, 22);
            mnuMapReset.Text = "Reset Map";
            mnuMapReset.Click += new EventHandler(MnuMapReset_Click);
            // 
            // mnuHelpMain
            // 
            mnuHelpMain.DropDownItems.AddRange(new ToolStripItem[] {
            mnuAbout});
            mnuHelpMain.Name = "mnuHelpMain";
            mnuHelpMain.Size = new Size(44, 20);
            mnuHelpMain.Text = "&Help";
            // 
            // mnuAbout
            // 
            mnuAbout.Name = "mnuAbout";
            mnuAbout.Size = new Size(107, 22);
            mnuAbout.Text = "About";
            mnuAbout.Click += new EventHandler(MnuAbout_Click);
            // 
            // mnuContext
            // 
            mnuContext.Items.AddRange(new ToolStripItem[] {
            mnuDepthFilter2,
            toolStripMenuItem2,
            mnuForceDistinct2,
            mnuForceDistinctText2,
            toolStripSeparator6,
            addMapTextToolStripMenuItem,
            mnuLabelShow2,
            mnuShowTargetInfo2,
            mnuSmallTargetInfo2,
            mnuAutoSelectEQTarget2,
            toolStripSeparator15,
            mnuFollowNone2,
            mnuFollowPlayer2,
            mnuFollowTarget2,
            toolStripSeparator16,
            mnuKeepCentered2,
            mnuAutoExpand2,
            toolStripSeparator17,
            mnuShowMenuBar,
            mnuMapReset2});
            mnuContext.Name = "mnuContext";
            mnuContext.Size = new Size(196, 380);
            // 
            // mnuDepthFilter2
            // 
            mnuDepthFilter2.Name = "mnuDepthFilter2";
            mnuDepthFilter2.ShortcutKeys = Keys.F5;
            mnuDepthFilter2.Size = new Size(195, 22);
            mnuDepthFilter2.Text = "&Depth Filter On/Off";
            mnuDepthFilter2.Click += new EventHandler(MnuDepthFilter_Click);
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.DropDownItems.AddRange(new ToolStripItem[] {
            mnuDynamicAlpha2,
            mnuFilterMapLines2,
            mnuFilterMapText2,
            mnuFilterNPCs2,
            mnuFilterNPCCorpses2,
            mnuFilterPlayers2,
            mnuFilterPlayerCorpses2,
            mnuFilterGroundItems2,
            mnuFilterSpawnPoints2});
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new Size(195, 22);
            toolStripMenuItem2.Text = "Depth &Filter Settings";
            // 
            // mnuDynamicAlpha2
            // 
            mnuDynamicAlpha2.Name = "mnuDynamicAlpha2";
            mnuDynamicAlpha2.Size = new Size(220, 22);
            mnuDynamicAlpha2.Text = "Dynamic &Alpha Faded Lines";
            mnuDynamicAlpha2.Click += new EventHandler(MnuDynamicAlpha_Click);
            // 
            // mnuFilterMapLines2
            // 
            mnuFilterMapLines2.Name = "mnuFilterMapLines2";
            mnuFilterMapLines2.Size = new Size(220, 22);
            mnuFilterMapLines2.Text = "Filter &Map Lines";
            mnuFilterMapLines2.Click += new EventHandler(MnuFilterMapLines_Click);
            // 
            // mnuFilterMapText2
            // 
            mnuFilterMapText2.Name = "mnuFilterMapText2";
            mnuFilterMapText2.Size = new Size(220, 22);
            mnuFilterMapText2.Text = "Filter Map &Text";
            mnuFilterMapText2.Click += new EventHandler(MnuFilterMapText_Click);
            // 
            // mnuFilterNPCs2
            // 
            mnuFilterNPCs2.Name = "mnuFilterNPCs2";
            mnuFilterNPCs2.Size = new Size(220, 22);
            mnuFilterNPCs2.Text = "Filter &NPCs";
            mnuFilterNPCs2.Click += new EventHandler(MnuFilterNPCs_Click);
            // 
            // mnuFilterNPCCorpses2
            // 
            mnuFilterNPCCorpses2.Name = "mnuFilterNPCCorpses2";
            mnuFilterNPCCorpses2.Size = new Size(220, 22);
            mnuFilterNPCCorpses2.Text = "Filter NPC &Corpses";
            mnuFilterNPCCorpses2.Click += new EventHandler(MnuFilterNPCCorpses_Click);
            // 
            // mnuFilterPlayers2
            // 
            mnuFilterPlayers2.Name = "mnuFilterPlayers2";
            mnuFilterPlayers2.Size = new Size(220, 22);
            mnuFilterPlayers2.Text = "Filter &Players";
            mnuFilterPlayers2.Click += new EventHandler(MnuFilterPlayers_Click);
            // 
            // mnuFilterPlayerCorpses2
            // 
            mnuFilterPlayerCorpses2.Name = "mnuFilterPlayerCorpses2";
            mnuFilterPlayerCorpses2.Size = new Size(220, 22);
            mnuFilterPlayerCorpses2.Text = "Filter Pl&ayer Corpses";
            mnuFilterPlayerCorpses2.Click += new EventHandler(MnuFilterPlayerCorpses_Click);
            // 
            // mnuFilterGroundItems2
            // 
            mnuFilterGroundItems2.Name = "mnuFilterGroundItems2";
            mnuFilterGroundItems2.Size = new Size(220, 22);
            mnuFilterGroundItems2.Text = "Filter &Ground Items";
            mnuFilterGroundItems2.Click += new EventHandler(MnuFilterGroundItems_Click);
            // 
            // mnuFilterSpawnPoints2
            // 
            mnuFilterSpawnPoints2.Name = "mnuFilterSpawnPoints2";
            mnuFilterSpawnPoints2.Size = new Size(220, 22);
            mnuFilterSpawnPoints2.Text = "Filter &Spawn Points";
            mnuFilterSpawnPoints2.Click += new EventHandler(MnuFilterSpawnPoints_Click);
            // 
            // mnuForceDistinct2
            // 
            mnuForceDistinct2.Name = "mnuForceDistinct2";
            mnuForceDistinct2.Size = new Size(195, 22);
            mnuForceDistinct2.Text = "&Force Distinct Lines";
            mnuForceDistinct2.Click += new EventHandler(MnuForceDistinct_Click);
            // 
            // mnuForceDistinctText2
            // 
            mnuForceDistinctText2.Name = "mnuForceDistinctText2";
            mnuForceDistinctText2.Size = new Size(195, 22);
            mnuForceDistinctText2.Text = "Force Distinct &Text";
            mnuForceDistinctText2.Click += new EventHandler(MnuForceDistinctText_Click);
            // 
            // toolStripSeparator6
            // 
            toolStripSeparator6.Name = "toolStripSeparator6";
            toolStripSeparator6.Size = new Size(192, 6);
            // 
            // addMapTextToolStripMenuItem
            // 
            addMapTextToolStripMenuItem.Name = "addMapTextToolStripMenuItem";
            addMapTextToolStripMenuItem.Size = new Size(195, 22);
            addMapTextToolStripMenuItem.Text = "Add Map Text";
            addMapTextToolStripMenuItem.ToolTipText = "Add Map Text to your current location.";
            addMapTextToolStripMenuItem.Click += new EventHandler(AddMapTextToolStripMenuItem_Click);
            // 
            // mnuLabelShow2
            // 
            mnuLabelShow2.DropDownItems.AddRange(new ToolStripItem[] {
            mnuShowNPCLevels2,
            mnuShowNPCNames2,
            mnuShowNPCCorpseNames2,
            mnuShowPCNames2,
            mnuShowPlayerCorpseNames2,
//            mnuShowPCGuild2,
            mnuSpawnCountdown2,
            mnuShowSpawnPoints2,
            mnuShowZoneText2,
            mnuShowLayer21,
            mnuShowLayer22,
            mnuShowLayer23,
            mnuShowPVP2,
            mnuShowPVPLevel2});
            mnuLabelShow2.Name = "mnuLabelShow2";
            mnuLabelShow2.Size = new Size(195, 22);
            mnuLabelShow2.Text = "&Show on Map";
            // 
            // mnuShowNPCLevels2
            // 
            mnuShowNPCLevels2.Name = "mnuShowNPCLevels2";
            mnuShowNPCLevels2.Size = new Size(186, 22);
            mnuShowNPCLevels2.Text = "NPC L&evels";
            mnuShowNPCLevels2.Click += new EventHandler(MnuShowNPCLevels_Click);
            // 
            // mnuShowNPCNames2
            // 
            mnuShowNPCNames2.Name = "mnuShowNPCNames2";
            mnuShowNPCNames2.Size = new Size(186, 22);
            mnuShowNPCNames2.Text = "&NPC Names";
            mnuShowNPCNames2.Click += new EventHandler(MnuShowNPCNames_Click);
            // 
            // mnuShowNPCCorpseNames2
            // 
            mnuShowNPCCorpseNames2.Name = "mnuShowNPCCorpseNames2";
            mnuShowNPCCorpseNames2.Size = new Size(186, 22);
            mnuShowNPCCorpseNames2.Text = "NPC &Corpse Names";
            mnuShowNPCCorpseNames2.Click += new EventHandler(MnuShowNPCCorpseNames_Click);
            // 
            // mnuShowPCNames2
            // 
            mnuShowPCNames2.Name = "mnuShowPCNames2";
            mnuShowPCNames2.Size = new Size(186, 22);
            mnuShowPCNames2.Text = "&Player Names";
            mnuShowPCNames2.Click += new EventHandler(MnuShowPCNames_Click);
            // 
            // mnuShowPlayerCorpseNames2
            // 
            mnuShowPlayerCorpseNames2.Name = "mnuShowPlayerCorpseNames2";
            mnuShowPlayerCorpseNames2.Size = new Size(186, 22);
            mnuShowPlayerCorpseNames2.Text = "Player Corpse &Names";
            mnuShowPlayerCorpseNames2.Click += new EventHandler(MnuShowPlayerCorpseNames_Click);
            //// 
            //// mnuShowPCGuild2
            //// 
            //mnuShowPCGuild2.Name = "mnuShowPCGuild2";
            //mnuShowPCGuild2.Size = new Size(186, 22);
            //mnuShowPCGuild2.Text = "&Player Guild";
            //mnuShowPCGuild2.Click += new EventHandler(MnuShowPCGuild_Click);
            // 
            // mnuSpawnCountdown2
            // 
            mnuSpawnCountdown2.Name = "mnuSpawnCountdown2";
            mnuSpawnCountdown2.Size = new Size(186, 22);
            mnuSpawnCountdown2.Text = "Spawn Countdown";
            mnuSpawnCountdown2.Click += new EventHandler(MnuSpawnCountdown_Click);
            // 
            // mnuShowSpawnPoints2
            // 
            mnuShowSpawnPoints2.Name = "mnuShowSpawnPoints2";
            mnuShowSpawnPoints2.Size = new Size(186, 22);
            mnuShowSpawnPoints2.Text = "&Spawn Points";
            mnuShowSpawnPoints2.Click += new EventHandler(MnuShowSpawnPoints_Click);
            // 
            // mnuShowZoneText2
            // 
            mnuShowZoneText2.Name = "mnuShowZoneText2";
            mnuShowZoneText2.Size = new Size(186, 22);
            mnuShowZoneText2.Text = "&Zone Text";
            mnuShowZoneText2.Click += new EventHandler(MnuShowZoneText_Click);
            // 
            // mnuShowLayer21
            // 
            mnuShowLayer21.Name = "mnuShowLayer21";
            mnuShowLayer21.Size = new Size(186, 22);
            mnuShowLayer21.Text = "&Show Layer 1";
            mnuShowLayer21.Click += new EventHandler(MnuShowLayer1_Click);
            // 
            // mnuShowLayer22
            // 
            mnuShowLayer22.Name = "mnuShowLayer22";
            mnuShowLayer22.Size = new Size(186, 22);
            mnuShowLayer22.Text = "&Show Layer 2";
            mnuShowLayer22.Click += new EventHandler(MnuShowLayer2_Click);
            // 
            // mnuShowLayer23
            // 
            mnuShowLayer23.Name = "mnuShowLayer23";
            mnuShowLayer23.Size = new Size(186, 22);
            mnuShowLayer23.Text = "&Show Layer 3";
            mnuShowLayer23.Click += new EventHandler(MnuShowLayer3_Click);
            // 
            // mnuShowPVP2
            // 
            mnuShowPVP2.Name = "mnuShowPVP2";
            mnuShowPVP2.Size = new Size(186, 22);
            mnuShowPVP2.Text = "P&VP";
            mnuShowPVP2.Click += new EventHandler(MnuShowPVP_Click);
            // 
            // mnuShowPVPLevel2
            // 
            mnuShowPVPLevel2.Name = "mnuShowPVPLevel2";
            mnuShowPVPLevel2.Size = new Size(186, 22);
            mnuShowPVPLevel2.Text = "PVP &Level";
            mnuShowPVPLevel2.Click += new EventHandler(MnuShowPVPLevel_Click);
            // 
            // mnuShowTargetInfo2
            // 
            mnuShowTargetInfo2.Name = "mnuShowTargetInfo2";
            mnuShowTargetInfo2.ShortcutKeys = Keys.F9;
            mnuShowTargetInfo2.Size = new Size(195, 22);
            mnuShowTargetInfo2.Text = "Show &Target Info";
            mnuShowTargetInfo2.Click += new EventHandler(MnuShowTargetInfo_Click);
            // 
            // mnuSmallTargetInfo2
            // 
            mnuSmallTargetInfo2.Name = "mnuSmallTargetInfo2";
            mnuSmallTargetInfo2.Size = new Size(195, 22);
            mnuSmallTargetInfo2.Text = "Small Target &Info";
            mnuSmallTargetInfo2.Click += new EventHandler(MnuSmallTargetInfo_Click);
            // 
            // mnuAutoSelectEQTarget2
            // 
            mnuAutoSelectEQTarget2.Name = "mnuAutoSelectEQTarget2";
            mnuAutoSelectEQTarget2.Size = new Size(195, 22);
            mnuAutoSelectEQTarget2.Text = "Auto Select &EQ Target";
            mnuAutoSelectEQTarget2.Click += new EventHandler(MnuAutoSelectEQTarget_Click);
            // 
            // toolStripSeparator15
            // 
            toolStripSeparator15.Name = "toolStripSeparator15";
            toolStripSeparator15.Size = new Size(192, 6);
            // 
            // mnuFollowNone2
            // 
            mnuFollowNone2.ImageScaling = ToolStripItemImageScaling.None;
            mnuFollowNone2.ImageTransparentColor = Color.Magenta;
            mnuFollowNone2.Name = "mnuFollowNone2";
            mnuFollowNone2.Size = new Size(195, 22);
            mnuFollowNone2.Text = "No Follow";
            mnuFollowNone2.Click += new EventHandler(MnuFollowNone_Click);
            // 
            // mnuFollowPlayer2
            // 
            mnuFollowPlayer2.Image = (Image)resources.GetObject("mnuFollowPlayer2.Image");
            mnuFollowPlayer2.ImageScaling = ToolStripItemImageScaling.None;
            mnuFollowPlayer2.ImageTransparentColor = Color.Magenta;
            mnuFollowPlayer2.Name = "mnuFollowPlayer2";
            mnuFollowPlayer2.Size = new Size(195, 22);
            mnuFollowPlayer2.Text = "Follow Player";
            mnuFollowPlayer2.Click += new EventHandler(MnuFollowPlayer_Click);
            // 
            // mnuFollowTarget2
            // 
            mnuFollowTarget2.ImageScaling = ToolStripItemImageScaling.None;
            mnuFollowTarget2.ImageTransparentColor = Color.Magenta;
            mnuFollowTarget2.Name = "mnuFollowTarget2";
            mnuFollowTarget2.Size = new Size(195, 22);
            mnuFollowTarget2.Text = "Follow Target";
            mnuFollowTarget2.Click += new EventHandler(MnuFollowTarget_Click);
            // 
            // toolStripSeparator16
            // 
            toolStripSeparator16.Name = "toolStripSeparator16";
            toolStripSeparator16.Size = new Size(192, 6);
            // 
            // mnuKeepCentered2
            // 
            mnuKeepCentered2.Name = "mnuKeepCentered2";
            mnuKeepCentered2.Size = new Size(195, 22);
            mnuKeepCentered2.Text = "Keep Centered";
            mnuKeepCentered2.Click += new EventHandler(MnuKeepCentered_Click);
            // 
            // mnuAutoExpand2
            // 
            mnuAutoExpand2.Name = "mnuAutoExpand2";
            mnuAutoExpand2.Size = new Size(195, 22);
            mnuAutoExpand2.Text = "Auto Expand";
            mnuAutoExpand2.Click += new EventHandler(MnuAutoExpand_Click);
            // 
            // toolStripSeparator17
            // 
            toolStripSeparator17.Name = "toolStripSeparator17";
            toolStripSeparator17.Size = new Size(192, 6);
            // 
            // mnuShowMenuBar
            // 
            mnuShowMenuBar.Name = "mnuShowMenuBar";
            mnuShowMenuBar.Size = new Size(195, 22);
            mnuShowMenuBar.Text = "Show Menu Bar";
            mnuShowMenuBar.Click += new EventHandler(MnuShowMenuBar_Click);
            // 
            // mnuMapReset2
            // 
            mnuMapReset2.Name = "mnuMapReset2";
            mnuMapReset2.Size = new Size(195, 22);
            mnuMapReset2.Text = "Reset Map";
            mnuMapReset2.Click += new EventHandler(MnuMapReset_Click);
            // 
            // mnuContextAddFilter
            // 
            mnuContextAddFilter.Items.AddRange(new ToolStripItem[] {
            mnuMobName,
            menuItem11,
            mnuAddHuntFilter,
            mnuAddCautionFilter,
            mnuAddDangerFilter,
            mnuAddAlertFilter,
            toolStripBasecon,
            mnuSepAddFilter,
            mnuAddMapLabel,
            toolStripSepAddMapLabel,
            mnuSearchAllakhazam});
            mnuContextAddFilter.Name = "mnuContextAddFilter";
            mnuContextAddFilter.Size = new Size(229, 198);
            // 
            // mnuMobName
            // 
            mnuMobName.Enabled = false;
            mnuMobName.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            mnuMobName.Name = "mnuMobName";
            mnuMobName.Size = new Size(228, 22);
            mnuMobName.Text = "MobName PlaceHolder";
            // 
            // menuItem11
            // 
            menuItem11.Name = "menuItem11";
            menuItem11.Size = new Size(225, 6);
            // 
            // mnuAddHuntFilter
            // 
            mnuAddHuntFilter.Name = "mnuAddHuntFilter";
            mnuAddHuntFilter.Size = new Size(228, 22);
            mnuAddHuntFilter.Text = "Add Zone Hunt Alert Filter";
            mnuAddHuntFilter.Click += new EventHandler(MnuAddHuntFilter_Click);
            // 
            // mnuAddCautionFilter
            // 
            mnuAddCautionFilter.Name = "mnuAddCautionFilter";
            mnuAddCautionFilter.Size = new Size(228, 22);
            mnuAddCautionFilter.Text = "Add Zone Caution Alert Filter";
            mnuAddCautionFilter.Click += new EventHandler(MnuAddCautionFilter_Click);
            // 
            // mnuAddDangerFilter
            // 
            mnuAddDangerFilter.Name = "mnuAddDangerFilter";
            mnuAddDangerFilter.Size = new Size(228, 22);
            mnuAddDangerFilter.Text = "Add Zone Danger Alert Filter";
            mnuAddDangerFilter.Click += new EventHandler(MnuAddDangerFilter_Click);
            // 
            // mnuAddAlertFilter
            // 
            mnuAddAlertFilter.Name = "mnuAddAlertFilter";
            mnuAddAlertFilter.Size = new Size(228, 22);
            mnuAddAlertFilter.Text = "Add Zone Rare Alert Filter";
            mnuAddAlertFilter.Click += new EventHandler(MnuAddAlertFilter_Click);
            // 
            // toolStripBasecon
            // 
            toolStripBasecon.CheckOnClick = true;
            toolStripBasecon.Font = new Font("Tahoma", 8.400001F, FontStyle.Bold);
            toolStripBasecon.Image = Resources.BlackX;
            toolStripBasecon.ImageTransparentColor = Color.Magenta;
            toolStripBasecon.Name = "toolStripBasecon";
            toolStripBasecon.Size = new Size(228, 22);
            toolStripBasecon.Text = "Base Con on this Spawn";
            // 
            // mnuSepAddFilter
            // 
            mnuSepAddFilter.Name = "mnuSepAddFilter";
            mnuSepAddFilter.Size = new Size(225, 6);
            // 
            // mnuAddMapLabel
            // 
            mnuAddMapLabel.Name = "mnuAddMapLabel";
            mnuAddMapLabel.Size = new Size(228, 22);
            mnuAddMapLabel.Text = "Add Map Label";
            mnuAddMapLabel.Click += new EventHandler(MnuAddMapLabel_Click);
            // 
            // toolStripSepAddMapLabel
            // 
            toolStripSepAddMapLabel.Name = "toolStripSepAddMapLabel";
            toolStripSepAddMapLabel.Size = new Size(225, 6);
            // 
            // mnuSearchAllakhazam
            // 
            mnuSearchAllakhazam.Image = (Image)resources.GetObject("mnuSearchAllakhazam.Image");
            mnuSearchAllakhazam.ImageTransparentColor = Color.Magenta;
            mnuSearchAllakhazam.Name = "mnuSearchAllakhazam";
            mnuSearchAllakhazam.Size = new Size(228, 22);
            mnuSearchAllakhazam.Text = "Search Allakhazam";
            mnuSearchAllakhazam.Click += new EventHandler(MnuSearchAllakhazam_Click);
            // 
            // timPackets
            // 
            timPackets.Tick += new EventHandler(TimPackets_Tick);
            // 
            // timDelayAlerts
            // 
            timDelayAlerts.SynchronizingObject = this;
            timDelayAlerts.Elapsed += new System.Timers.ElapsedEventHandler(TimDelayPlay_Tick);
            // 
            // timProcessTimers
            // 
            timProcessTimers.Enabled = true;
            timProcessTimers.SynchronizingObject = this;
            timProcessTimers.Elapsed += new System.Timers.ElapsedEventHandler(TimProcessTimers_Tick);
            // 
            // mnuShowListNPCs
            // 
            mnuShowListNPCs.Name = "mnuShowListNPCs";
            mnuShowListNPCs.Size = new Size(32, 19);
            // 
            // mnuShowListCorpses
            // 
            mnuShowListCorpses.Name = "mnuShowListCorpses";
            mnuShowListCorpses.Size = new Size(32, 19);
            // 
            // mnuShowListPlayers
            // 
            mnuShowListPlayers.Name = "mnuShowListPlayers";
            mnuShowListPlayers.Size = new Size(32, 19);
            // 
            // mnuShowListInvis
            // 
            mnuShowListInvis.Name = "mnuShowListInvis";
            mnuShowListInvis.Size = new Size(32, 19);
            // 
            // mnuShowListMounts
            // 
            mnuShowListMounts.Name = "mnuShowListMounts";
            mnuShowListMounts.Size = new Size(32, 19);
            // 
            // mnuShowListFamiliars
            // 
            mnuShowListFamiliars.Name = "mnuShowListFamiliars";
            mnuShowListFamiliars.Size = new Size(32, 19);
            // 
            // mnuShowListPets
            // 
            mnuShowListPets.Name = "mnuShowListPets";
            mnuShowListPets.Size = new Size(32, 19);
            // 
            // statusBarStrip
            // 
            statusBarStrip.Items.AddRange(new ToolStripItem[] {
            toolStripMouseLocation,
            toolStripDistance,
            toolStripSpring,
            toolStripVersion,
            toolStripServerAddress,
            toolStripCoPStatus,
            toolStripShortName,
            toolStripFPS});
            statusBarStrip.Location = new Point(0, 507);
            statusBarStrip.Name = "statusBarStrip";
            statusBarStrip.Size = new Size(1464, 22);
            statusBarStrip.TabIndex = 0;
            statusBarStrip.Text = "statusStrip1";
            // 
            // toolStripMouseLocation
            // 
            toolStripMouseLocation.AutoSize = false;
            toolStripMouseLocation.BorderSides = ToolStripStatusLabelBorderSides.Left | ToolStripStatusLabelBorderSides.Top
            | ToolStripStatusLabelBorderSides.Right
            | ToolStripStatusLabelBorderSides.Bottom;
            toolStripMouseLocation.ImageScaling = ToolStripItemImageScaling.None;
            toolStripMouseLocation.Name = "toolStripMouseLocation";
            toolStripMouseLocation.Size = new Size(150, 17);
            toolStripMouseLocation.TextAlign = ContentAlignment.MiddleLeft;
            toolStripMouseLocation.ToolTipText = "Mouse Location";
            // 
            // toolStripDistance
            // 
            toolStripDistance.AutoSize = false;
            toolStripDistance.BorderSides = ToolStripStatusLabelBorderSides.Left | ToolStripStatusLabelBorderSides.Top
            | ToolStripStatusLabelBorderSides.Right
            | ToolStripStatusLabelBorderSides.Bottom;
            toolStripDistance.Name = "toolStripDistance";
            toolStripDistance.Size = new Size(100, 17);
            toolStripDistance.ToolTipText = "Game Distance from Player to Cursor";
            // 
            // toolStripSpring
            // 
            toolStripSpring.BorderSides = ToolStripStatusLabelBorderSides.Left | ToolStripStatusLabelBorderSides.Top
            | ToolStripStatusLabelBorderSides.Right
            | ToolStripStatusLabelBorderSides.Bottom;
            toolStripSpring.Name = "toolStripSpring";
            toolStripSpring.Size = new Size(955, 17);
            toolStripSpring.Spring = true;
            // 
            // toolStripVersion
            // 
            toolStripVersion.AutoSize = false;
            toolStripVersion.BorderSides = ToolStripStatusLabelBorderSides.Left | ToolStripStatusLabelBorderSides.Top
            | ToolStripStatusLabelBorderSides.Right
            | ToolStripStatusLabelBorderSides.Bottom;
            toolStripVersion.Name = "toolStripVersion";
            toolStripVersion.Size = new Size(60, 17);
            // 
            // toolStripServerAddress
            // 
            toolStripServerAddress.AutoSize = false;
            toolStripServerAddress.BorderSides = ToolStripStatusLabelBorderSides.Left | ToolStripStatusLabelBorderSides.Top
            | ToolStripStatusLabelBorderSides.Right
            | ToolStripStatusLabelBorderSides.Bottom;
            toolStripServerAddress.Name = "toolStripServerAddress";
            toolStripServerAddress.Size = new Size(90, 17);
            // 
            // toolStripCoPStatus
            // 
            toolStripCoPStatus.AutoSize = false;
            toolStripCoPStatus.BorderSides = ToolStripStatusLabelBorderSides.Left | ToolStripStatusLabelBorderSides.Top
            | ToolStripStatusLabelBorderSides.Right
            | ToolStripStatusLabelBorderSides.Bottom;
            toolStripCoPStatus.Name = "toolStripCoPStatus";
            toolStripCoPStatus.Size = new Size(30, 17);
            toolStripCoPStatus.Click += new EventHandler(ToolStripCoPStatus_Click);
            // 
            // toolStripShortName
            // 
            toolStripShortName.BorderSides = ToolStripStatusLabelBorderSides.Left | ToolStripStatusLabelBorderSides.Top
            | ToolStripStatusLabelBorderSides.Right
            | ToolStripStatusLabelBorderSides.Bottom;
            toolStripShortName.Name = "toolStripShortName";
            toolStripShortName.Size = new Size(4, 17);
            // 
            // toolStripFPS
            // 
            toolStripFPS.AutoSize = false;
            toolStripFPS.BorderSides = ToolStripStatusLabelBorderSides.Left | ToolStripStatusLabelBorderSides.Top
            | ToolStripStatusLabelBorderSides.Right
            | ToolStripStatusLabelBorderSides.Bottom;
            toolStripFPS.Name = "toolStripFPS";
            toolStripFPS.Size = new Size(60, 17);
            // 
            // toolBarStrip
            // 
            toolBarStrip.AutoSize = false;
            toolBarStrip.BackColor = SystemColors.ControlLight;
            toolBarStrip.BackgroundImage = Resources.toolbar;
            toolBarStrip.Items.AddRange(new ToolStripItem[] {
            toolStripStartStop,
            toolStripLevel,
            toolStripSeparator14,
            toolStripZoomIn,
            toolStripZoomOut,
            toolStripScale,
            toolStripDepthFilterButton,
            toolStripZPosLabel,
            toolStripZPos,
            toolStripZPosDown,
            toolStripZPosUp,
            toolStripZOffsetLabel,
            toolStripZNeg,
            toolStripZNegUp,
            toolStripZNegDown,
            toolStripResetDepthFilter,
            toolStripOptions,
            toolStripSeparator19,
            toolStripLabel1,
            toolStripLookupBox,
            toolStripCheckLookup,
            toolStripResetLookup,
            toolStripLookupBox1,
            toolStripCheckLookup1,
            toolStripResetLookup1,
            toolStripLookupBox2,
            toolStripCheckLookup2,
            toolStripResetLookup2,
            toolStripLookupBox3,
            toolStripCheckLookup3,
            toolStripResetLookup3,
            toolStripLookupBox4,
            toolStripCheckLookup4,
            toolStripResetLookup4,
            toolStripLookupBox5,
            toolStripCheckLookup5,
            toolStripResetLookup5});
            toolBarStrip.Location = new Point(0, 24);
            toolBarStrip.Name = "toolBarStrip";
            toolBarStrip.Size = new Size(1464, 25);
            toolBarStrip.TabIndex = 0;
            toolBarStrip.Text = "toolBarStrip";
            // 
            // toolStripStartStop
            // 
            toolStripStartStop.Image = (Image)resources.GetObject("toolStripStartStop.Image");
            toolStripStartStop.ImageScaling = ToolStripItemImageScaling.None;
            toolStripStartStop.ImageTransparentColor = Color.Magenta;
            toolStripStartStop.Name = "toolStripStartStop";
            toolStripStartStop.Size = new Size(42, 22);
            toolStripStartStop.Text = "Go";
            toolStripStartStop.ToolTipText = "Connect to Server";
            toolStripStartStop.Click += new EventHandler(CmdCommand_Click);
            // 
            // toolStripLevel
            // 
            toolStripLevel.DropDownHeight = 200;
            toolStripLevel.DropDownWidth = 30;
            toolStripLevel.IntegralHeight = false;
            toolStripLevel.Items.AddRange(new object[] {
            "Auto",
            "1",
            "5",
            "10",
            "15",
            "20",
            "25",
            "30",
            "35",
            "40",
            "45",
            "50",
            "55",
            "60",
            "65",
            "70",
            "75",
            "80",
            "85",
            "90",
            "95",
            "100",
            "105",
            "110",
            "115"});
            toolStripLevel.MaxDropDownItems = 80;
            toolStripLevel.MaxLength = 4;
            toolStripLevel.Name = "toolStripLevel";
            toolStripLevel.Size = new Size(75, 25);
            toolStripLevel.Text = "Auto";
            toolStripLevel.ToolTipText = "Auto or 1-115 to filter mobcolors accordingly";
            toolStripLevel.DropDownClosed += new EventHandler(ToolStripLevel_DropDownClosed);
            toolStripLevel.TextUpdate += new EventHandler(ToolStripLevel_TextUpdate);
            toolStripLevel.Leave += new EventHandler(ToolStripLevel_Leave);
            toolStripLevel.KeyPress += new KeyPressEventHandler(ToolStripLevel_KeyPress);
            // 
            // toolStripSeparator14
            // 
            toolStripSeparator14.Name = "toolStripSeparator14";
            toolStripSeparator14.Size = new Size(6, 25);
            // 
            // toolStripZoomIn
            // 
            toolStripZoomIn.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripZoomIn.Image = (Image)resources.GetObject("toolStripZoomIn.Image");
            toolStripZoomIn.ImageScaling = ToolStripItemImageScaling.None;
            toolStripZoomIn.ImageTransparentColor = Color.Transparent;
            toolStripZoomIn.Name = "toolStripZoomIn";
            toolStripZoomIn.Size = new Size(23, 22);
            toolStripZoomIn.Text = "toolStripButton2";
            toolStripZoomIn.ToolTipText = "Increase Magnification on Map";
            toolStripZoomIn.Click += new EventHandler(ToolStripZoomIn_Click);
            // 
            // toolStripZoomOut
            // 
            toolStripZoomOut.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripZoomOut.Image = (Image)resources.GetObject("toolStripZoomOut.Image");
            toolStripZoomOut.ImageScaling = ToolStripItemImageScaling.None;
            toolStripZoomOut.ImageTransparentColor = Color.Transparent;
            toolStripZoomOut.Name = "toolStripZoomOut";
            toolStripZoomOut.Size = new Size(23, 22);
            toolStripZoomOut.Text = "toolStripButton3";
            toolStripZoomOut.ToolTipText = "Decrease Magnification on Map";
            toolStripZoomOut.Click += new EventHandler(ToolStripZoomOut_Click);
            // 
            // toolStripScale
            // 
            toolStripScale.BackColor = SystemColors.Window;
            toolStripScale.Items.AddRange(new object[] {
            "10%",
            "25%",
            "50%",
            "75%",
            "100%",
            "125%",
            "150%",
            "175%",
            "200%",
            "250%",
            "300%",
            "400%",
            "500%",
            "1000%",
            "2000%"});
            toolStripScale.Margin = new Padding(0);
            toolStripScale.Name = "toolStripScale";
            toolStripScale.Size = new Size(75, 25);
            toolStripScale.Text = "100%";
            toolStripScale.ToolTipText = "Select or Enter a value for amount of map zoom.";
            toolStripScale.DropDownClosed += new EventHandler(ToolStripScale_DropDownClosed);
            toolStripScale.TextUpdate += new EventHandler(ToolStripScale_TextUpdate);
            toolStripScale.Leave += new EventHandler(ToolStripScale_Leave);
            toolStripScale.KeyPress += new KeyPressEventHandler(ToolStripScale_KeyPress);
            // 
            // toolStripDepthFilterButton
            // 
            toolStripDepthFilterButton.CheckOnClick = true;
            toolStripDepthFilterButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripDepthFilterButton.Image = Resources.ShrinkSpaceHS;
            toolStripDepthFilterButton.ImageTransparentColor = Color.White;
            toolStripDepthFilterButton.Name = "toolStripDepthFilterButton";
            toolStripDepthFilterButton.Size = new Size(23, 22);
            toolStripDepthFilterButton.Text = "Depth Filter";
            toolStripDepthFilterButton.ToolTipText = "Toggle Depth Filter On/Off";
            toolStripDepthFilterButton.Click += new EventHandler(MnuDepthFilter_Click);
            // 
            // toolStripZPosLabel
            // 
            toolStripZPosLabel.Name = "toolStripZPosLabel";
            toolStripZPosLabel.Size = new Size(38, 22);
            toolStripZPosLabel.Text = "Z-Pos";
            toolStripZPosLabel.ToolTipText = "The range above the player that is not depth filtered.";
            // 
            // toolStripZPos
            // 
            toolStripZPos.Font = new Font("Segoe UI", 9F);
            toolStripZPos.Margin = new Padding(0);
            toolStripZPos.Name = "toolStripZPos";
            toolStripZPos.Size = new Size(40, 25);
            toolStripZPos.Text = "75";
            toolStripZPos.TextBoxTextAlign = HorizontalAlignment.Center;
            toolStripZPos.ToolTipText = "Enter a value for Z-Pos between 0 and 3500.";
            toolStripZPos.Leave += new EventHandler(ToolStripZPos_Leave);
            toolStripZPos.KeyPress += new KeyPressEventHandler(ToolStripZPos_KeyPress);
            toolStripZPos.TextChanged += new EventHandler(ToolStripZPos_TextChanged);
            // 
            // toolStripZPosDown
            // 
            toolStripZPosDown.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripZPosDown.Image = (Image)resources.GetObject("toolStripZPosDown.Image");
            toolStripZPosDown.ImageScaling = ToolStripItemImageScaling.None;
            toolStripZPosDown.ImageTransparentColor = Color.Magenta;
            toolStripZPosDown.Name = "toolStripZPosDown";
            toolStripZPosDown.Size = new Size(23, 22);
            toolStripZPosDown.Text = "toolStripButton1";
            toolStripZPosDown.ToolTipText = "Decrease Z-Pos above player for depth filter.";
            toolStripZPosDown.Click += new EventHandler(ToolStripZPosDown_Click);
            // 
            // toolStripZPosUp
            // 
            toolStripZPosUp.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripZPosUp.Image = (Image)resources.GetObject("toolStripZPosUp.Image");
            toolStripZPosUp.ImageScaling = ToolStripItemImageScaling.None;
            toolStripZPosUp.ImageTransparentColor = Color.Magenta;
            toolStripZPosUp.Name = "toolStripZPosUp";
            toolStripZPosUp.Size = new Size(23, 22);
            toolStripZPosUp.Text = "toolStripButton4";
            toolStripZPosUp.ToolTipText = "Increase Z-Pos above player for depth filter.";
            toolStripZPosUp.Click += new EventHandler(ToolStripZPosUp_Click);
            // 
            // toolStripZOffsetLabel
            // 
            toolStripZOffsetLabel.Name = "toolStripZOffsetLabel";
            toolStripZOffsetLabel.Size = new Size(41, 22);
            toolStripZOffsetLabel.Text = "Z-Neg";
            toolStripZOffsetLabel.ToolTipText = "The range below the player that is not depth filtered.";
            // 
            // toolStripZNeg
            // 
            toolStripZNeg.Font = new Font("Segoe UI", 9F);
            toolStripZNeg.Margin = new Padding(0);
            toolStripZNeg.Name = "toolStripZNeg";
            toolStripZNeg.Size = new Size(40, 25);
            toolStripZNeg.Text = "75";
            toolStripZNeg.TextBoxTextAlign = HorizontalAlignment.Center;
            toolStripZNeg.ToolTipText = "Enter a value for Z-Neg between 0 and 3500.";
            toolStripZNeg.Leave += new EventHandler(ToolStripZNeg_Leave);
            toolStripZNeg.KeyPress += new KeyPressEventHandler(ToolStripZNeg_KeyPress);
            toolStripZNeg.TextChanged += new EventHandler(ToolStripZNeg_TextChanged);
            // 
            // toolStripZNegUp
            // 
            toolStripZNegUp.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripZNegUp.Image = (Image)resources.GetObject("toolStripZNegUp.Image");
            toolStripZNegUp.ImageScaling = ToolStripItemImageScaling.None;
            toolStripZNegUp.ImageTransparentColor = Color.Magenta;
            toolStripZNegUp.Name = "toolStripZNegUp";
            toolStripZNegUp.Size = new Size(23, 22);
            toolStripZNegUp.Text = "toolStripButton4";
            toolStripZNegUp.ToolTipText = "Increase Z-Neg below player for depth filter.";
            toolStripZNegUp.Click += new EventHandler(ToolStripZNegUp_Click);
            // 
            // toolStripZNegDown
            // 
            toolStripZNegDown.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripZNegDown.Image = (Image)resources.GetObject("toolStripZNegDown.Image");
            toolStripZNegDown.ImageScaling = ToolStripItemImageScaling.None;
            toolStripZNegDown.ImageTransparentColor = Color.Magenta;
            toolStripZNegDown.Name = "toolStripZNegDown";
            toolStripZNegDown.Size = new Size(23, 22);
            toolStripZNegDown.Text = "toolStripButton1";
            toolStripZNegDown.ToolTipText = "Decrease Z-Neg below player for depth filter.";
            toolStripZNegDown.Click += new EventHandler(ToolStripZNegDown_Click);
            // 
            // toolStripResetDepthFilter
            // 
            toolStripResetDepthFilter.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripResetDepthFilter.Image = (Image)resources.GetObject("toolStripResetDepthFilter.Image");
            toolStripResetDepthFilter.ImageScaling = ToolStripItemImageScaling.None;
            toolStripResetDepthFilter.ImageTransparentColor = Color.Transparent;
            toolStripResetDepthFilter.Name = "toolStripResetDepthFilter";
            toolStripResetDepthFilter.Size = new Size(23, 22);
            toolStripResetDepthFilter.Text = "toolStripResetDepthFilter";
            toolStripResetDepthFilter.ToolTipText = "Reset Depth Filter Settings";
            toolStripResetDepthFilter.Click += new EventHandler(ToolStripResetDepthFilter_Click);
            // 
            // toolStripOptions
            // 
            toolStripOptions.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripOptions.Image = (Image)resources.GetObject("toolStripOptions.Image");
            toolStripOptions.ImageScaling = ToolStripItemImageScaling.None;
            toolStripOptions.ImageTransparentColor = Color.Magenta;
            toolStripOptions.Name = "toolStripOptions";
            toolStripOptions.Size = new Size(23, 22);
            toolStripOptions.Text = "Options";
            toolStripOptions.ToolTipText = "Open Options Dialog";
            toolStripOptions.Click += new EventHandler(MnuOptions_Click);
            // 
            // toolStripSeparator19
            // 
            toolStripSeparator19.Name = "toolStripSeparator19";
            toolStripSeparator19.Size = new Size(6, 25);
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new Size(30, 22);
            toolStripLabel1.Text = "Find";
            toolStripLabel1.ToolTipText = "Find and temporarily mark mobs on map.";
            // 
            // toolStripLookupBox
            // 
            toolStripLookupBox.Font = new Font("Segoe UI", 9F);
            toolStripLookupBox.ForeColor = SystemColors.GrayText;
            toolStripLookupBox.Name = "toolStripLookupBox";
            toolStripLookupBox.Size = new Size(75, 25);
            toolStripLookupBox.Text = "Mob Search";
            toolStripLookupBox.ToolTipText = "Type in mob name and press Enter.";
            toolStripLookupBox.Leave += new EventHandler(ToolStripLookupBox_Leave);
            toolStripLookupBox.KeyPress += new KeyPressEventHandler(ToolStripTextBox_KeyPress);
            toolStripLookupBox.Click += new EventHandler(ToolStripLookupBox_Click);
            // 
            // toolStripCheckLookup
            // 
            toolStripCheckLookup.BackColor = Color.Gray;
            toolStripCheckLookup.Checked = true;
            toolStripCheckLookup.CheckOnClick = true;
            toolStripCheckLookup.CheckState = CheckState.Checked;
            toolStripCheckLookup.ImageTransparentColor = Color.Magenta;
            toolStripCheckLookup.Name = "toolStripCheckLookup";
            toolStripCheckLookup.Size = new Size(23, 22);
            toolStripCheckLookup.Text = "L";
            toolStripCheckLookup.ToolTipText = "Lookup or Filter";
            toolStripCheckLookup.CheckedChanged += new EventHandler(ToolStripCheckLookup_CheckChanged);
            // 
            // toolStripResetLookup
            // 
            toolStripResetLookup.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripResetLookup.ImageTransparentColor = Color.Magenta;
            toolStripResetLookup.Name = "toolStripResetLookup";
            toolStripResetLookup.Size = new Size(39, 22);
            toolStripResetLookup.Text = "Reset";
            toolStripResetLookup.ToolTipText = "Reset Find Mob Search String";
            toolStripResetLookup.Click += new EventHandler(ToolStripResetLookup_Click);
            // 
            // toolStripLookupBox1
            // 
            toolStripLookupBox1.Font = new Font("Segoe UI", 9F);
            toolStripLookupBox1.ForeColor = SystemColors.GrayText;
            toolStripLookupBox1.Name = "toolStripLookupBox1";
            toolStripLookupBox1.Size = new Size(75, 25);
            toolStripLookupBox1.Text = "Mob Search";
            toolStripLookupBox1.ToolTipText = "Type in mob name and press Enter.";
            toolStripLookupBox1.Leave += new EventHandler(ToolStripLookupBox1_Leave);
            toolStripLookupBox1.KeyPress += new KeyPressEventHandler(ToolStripTextBox1_KeyPress);
            toolStripLookupBox1.Click += new EventHandler(ToolStripLookupBox1_Click);
            // 
            // toolStripCheckLookup1
            // 
            toolStripCheckLookup1.BackColor = Color.Gray;
            toolStripCheckLookup1.Checked = true;
            toolStripCheckLookup1.CheckOnClick = true;
            toolStripCheckLookup1.CheckState = CheckState.Checked;
            toolStripCheckLookup1.ImageTransparentColor = Color.Magenta;
            toolStripCheckLookup1.Name = "toolStripCheckLookup1";
            toolStripCheckLookup1.Size = new Size(23, 22);
            toolStripCheckLookup1.Text = "L";
            toolStripCheckLookup1.ToolTipText = "Lookup or Filter";
            toolStripCheckLookup1.CheckedChanged += new EventHandler(ToolStripCheckLookup1_CheckChanged);
            // 
            // toolStripResetLookup1
            // 
            toolStripResetLookup1.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripResetLookup1.ImageTransparentColor = Color.Magenta;
            toolStripResetLookup1.Name = "toolStripResetLookup1";
            toolStripResetLookup1.Size = new Size(39, 22);
            toolStripResetLookup1.Text = "Reset";
            toolStripResetLookup1.ToolTipText = "Reset Find Mob Search String";
            toolStripResetLookup1.Click += new EventHandler(ToolStripResetLookup1_Click);
            // 
            // toolStripLookupBox2
            // 
            toolStripLookupBox2.Font = new Font("Segoe UI", 9F);
            toolStripLookupBox2.ForeColor = SystemColors.GrayText;
            toolStripLookupBox2.Name = "toolStripLookupBox2";
            toolStripLookupBox2.Size = new Size(75, 25);
            toolStripLookupBox2.Text = "Mob Search";
            toolStripLookupBox2.ToolTipText = "Type in mob name and press Enter.";
            toolStripLookupBox2.Leave += new EventHandler(ToolStripLookupBox2_Leave);
            toolStripLookupBox2.KeyPress += new KeyPressEventHandler(ToolStripTextBox2_KeyPress);
            toolStripLookupBox2.Click += new EventHandler(ToolStripLookupBox2_Click);
            // 
            // toolStripCheckLookup2
            // 
            toolStripCheckLookup2.BackColor = Color.Gray;
            toolStripCheckLookup2.Checked = true;
            toolStripCheckLookup2.CheckOnClick = true;
            toolStripCheckLookup2.CheckState = CheckState.Checked;
            toolStripCheckLookup2.ImageTransparentColor = Color.Magenta;
            toolStripCheckLookup2.Name = "toolStripCheckLookup2";
            toolStripCheckLookup2.Size = new Size(23, 22);
            toolStripCheckLookup2.Text = "L";
            toolStripCheckLookup2.ToolTipText = "Lookup or Filter";
            toolStripCheckLookup2.CheckedChanged += new EventHandler(ToolStripCheckLookup2_CheckChanged);
            // 
            // toolStripResetLookup2
            // 
            toolStripResetLookup2.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripResetLookup2.ImageTransparentColor = Color.Magenta;
            toolStripResetLookup2.Name = "toolStripResetLookup2";
            toolStripResetLookup2.Size = new Size(39, 22);
            toolStripResetLookup2.Text = "Reset";
            toolStripResetLookup2.ToolTipText = "Reset Find Mob Search String";
            toolStripResetLookup2.Click += new EventHandler(ToolStripResetLookup2_Click);
            // 
            // toolStripLookupBox3
            // 
            toolStripLookupBox3.Font = new Font("Segoe UI", 9F);
            toolStripLookupBox3.ForeColor = SystemColors.GrayText;
            toolStripLookupBox3.Name = "toolStripLookupBox3";
            toolStripLookupBox3.Size = new Size(75, 25);
            toolStripLookupBox3.Text = "Mob Search";
            toolStripLookupBox3.ToolTipText = "Type in mob name and press Enter.";
            toolStripLookupBox3.Leave += new EventHandler(ToolStripLookupBox3_Leave);
            toolStripLookupBox3.KeyPress += new KeyPressEventHandler(ToolStripTextBox3_KeyPress);
            toolStripLookupBox3.Click += new EventHandler(ToolStripLookupBox3_Click);
            // 
            // toolStripCheckLookup3
            // 
            toolStripCheckLookup3.BackColor = Color.Gray;
            toolStripCheckLookup3.Checked = true;
            toolStripCheckLookup3.CheckOnClick = true;
            toolStripCheckLookup3.CheckState = CheckState.Checked;
            toolStripCheckLookup3.ImageTransparentColor = Color.Magenta;
            toolStripCheckLookup3.Name = "toolStripCheckLookup3";
            toolStripCheckLookup3.Size = new Size(23, 22);
            toolStripCheckLookup3.Text = "L";
            toolStripCheckLookup3.ToolTipText = "Lookup or Filter";
            toolStripCheckLookup3.CheckedChanged += new EventHandler(ToolStripCheckLookup3_CheckChanged);
            // 
            // toolStripResetLookup3
            // 
            toolStripResetLookup3.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripResetLookup3.ImageTransparentColor = Color.Magenta;
            toolStripResetLookup3.Name = "toolStripResetLookup3";
            toolStripResetLookup3.Size = new Size(39, 22);
            toolStripResetLookup3.Text = "Reset";
            toolStripResetLookup3.ToolTipText = "Reset Find Mob Search String";
            toolStripResetLookup3.Click += new EventHandler(ToolStripResetLookup3_Click);
            // 
            // toolStripLookupBox4
            // 
            toolStripLookupBox4.Font = new Font("Segoe UI", 9F);
            toolStripLookupBox4.ForeColor = SystemColors.GrayText;
            toolStripLookupBox4.Name = "toolStripLookupBox4";
            toolStripLookupBox4.Size = new Size(75, 25);
            toolStripLookupBox4.Text = "Mob Search";
            toolStripLookupBox4.ToolTipText = "Type in mob name and press Enter.";
            toolStripLookupBox4.Leave += new EventHandler(ToolStripLookupBox4_Leave);
            toolStripLookupBox4.KeyPress += new KeyPressEventHandler(ToolStripTextBox4_KeyPress);
            toolStripLookupBox4.Click += new EventHandler(ToolStripLookupBox4_Click);
            // 
            // toolStripCheckLookup4
            // 
            toolStripCheckLookup4.BackColor = Color.Gray;
            toolStripCheckLookup4.Checked = true;
            toolStripCheckLookup4.CheckOnClick = true;
            toolStripCheckLookup4.CheckState = CheckState.Checked;
            toolStripCheckLookup4.ImageTransparentColor = Color.Magenta;
            toolStripCheckLookup4.Name = "toolStripCheckLookup4";
            toolStripCheckLookup4.Size = new Size(23, 22);
            toolStripCheckLookup4.Text = "L";
            toolStripCheckLookup4.ToolTipText = "Lookup or Filter";
            toolStripCheckLookup4.CheckedChanged += new EventHandler(ToolStripCheckLookup4_CheckChanged);
            // 
            // toolStripResetLookup4
            // 
            toolStripResetLookup4.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripResetLookup4.ImageTransparentColor = Color.Magenta;
            toolStripResetLookup4.Name = "toolStripResetLookup4";
            toolStripResetLookup4.Size = new Size(39, 22);
            toolStripResetLookup4.Text = "Reset";
            toolStripResetLookup4.ToolTipText = "Reset Find Mob Search String";
            toolStripResetLookup4.Click += new EventHandler(ToolStripResetLookup4_Click);
            // 
            // toolStripLookupBox5
            // 
            toolStripLookupBox5.Font = new Font("Segoe UI", 9F);
            toolStripLookupBox5.ForeColor = SystemColors.GrayText;
            toolStripLookupBox5.Name = "toolStripLookupBox5";
            toolStripLookupBox5.Size = new Size(75, 25);
            toolStripLookupBox5.Text = "Mob Search";
            toolStripLookupBox5.ToolTipText = "Type in mob name and press Enter.";
            toolStripLookupBox5.Leave += new EventHandler(ToolStripLookupBox5_Leave);
            toolStripLookupBox5.KeyPress += new KeyPressEventHandler(ToolStripTextBox5_KeyPress);
            toolStripLookupBox5.Click += new EventHandler(ToolStripLookupBox5_Click);
            // 
            // toolStripCheckLookup5
            // 
            toolStripCheckLookup5.BackColor = Color.Gray;
            toolStripCheckLookup5.Checked = true;
            toolStripCheckLookup5.CheckOnClick = true;
            toolStripCheckLookup5.CheckState = CheckState.Checked;
            toolStripCheckLookup5.ImageTransparentColor = Color.Magenta;
            toolStripCheckLookup5.Name = "toolStripCheckLookup5";
            toolStripCheckLookup5.Size = new Size(23, 22);
            toolStripCheckLookup5.Text = "L";
            toolStripCheckLookup5.ToolTipText = "Lookup or Filter";
            toolStripCheckLookup5.CheckedChanged += new EventHandler(ToolStripCheckLookup5_CheckChanged);
            // 
            // toolStripResetLookup5
            // 
            toolStripResetLookup5.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripResetLookup5.ImageTransparentColor = Color.Magenta;
            toolStripResetLookup5.Name = "toolStripResetLookup5";
            toolStripResetLookup5.Size = new Size(39, 22);
            toolStripResetLookup5.Text = "Reset";
            toolStripResetLookup5.ToolTipText = "Reset Find Mob Search String";
            toolStripResetLookup5.Click += new EventHandler(ToolStripResetLookup5_Click);
            // 
            // dockPanel
            // 
            dockPanel.ActiveAutoHideContent = null;
            dockPanel.BackColor = SystemColors.ControlLight;
            dockPanel.Dock = DockStyle.Fill;
            dockPanel.DockBackColor = SystemColors.ControlLight;
            dockPanel.Location = new Point(0, 49);
            dockPanel.Name = "dockPanel";
            dockPanel.Size = new Size(1464, 458);
            dockPanelGradient1.EndColor = SystemColors.ControlLight;
            dockPanelGradient1.StartColor = SystemColors.ControlLight;
            autoHideStripSkin1.DockStripGradient = dockPanelGradient1;
            tabGradient1.EndColor = SystemColors.Control;
            tabGradient1.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            tabGradient1.StartColor = SystemColors.ControlLightLight;
            tabGradient1.TextColor = SystemColors.ControlDarkDark;
            autoHideStripSkin1.TabGradient = tabGradient1;
            autoHideStripSkin1.TextFont = new Font("Tahoma", 8.25F);
            dockPanelSkin1.AutoHideStripSkin = autoHideStripSkin1;
            tabGradient2.EndColor = SystemColors.GradientInactiveCaption;
            tabGradient2.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            tabGradient2.StartColor = SystemColors.ControlLight;
            tabGradient2.TextColor = SystemColors.ControlText;
            dockPaneStripGradient1.ActiveTabGradient = tabGradient2;
            dockPanelGradient2.EndColor = SystemColors.Control;
            dockPanelGradient2.StartColor = SystemColors.Control;
            dockPaneStripGradient1.DockStripGradient = dockPanelGradient2;
            tabGradient3.EndColor = SystemColors.ControlLight;
            tabGradient3.StartColor = SystemColors.ControlLight;
            tabGradient3.TextColor = SystemColors.ControlText;
            dockPaneStripGradient1.InactiveTabGradient = tabGradient3;
            dockPaneStripSkin1.DocumentGradient = dockPaneStripGradient1;
            dockPaneStripSkin1.TextFont = new Font("Tahoma", 8.25F);
            tabGradient4.EndColor = SystemColors.ActiveCaption;
            tabGradient4.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            tabGradient4.StartColor = SystemColors.GradientActiveCaption;
            tabGradient4.TextColor = SystemColors.ActiveCaptionText;
            dockPaneStripToolWindowGradient1.ActiveCaptionGradient = tabGradient4;
            tabGradient5.EndColor = SystemColors.Control;
            tabGradient5.StartColor = SystemColors.Control;
            tabGradient5.TextColor = SystemColors.ControlText;
            dockPaneStripToolWindowGradient1.ActiveTabGradient = tabGradient5;
            dockPanelGradient3.EndColor = SystemColors.ControlLight;
            dockPanelGradient3.StartColor = SystemColors.ControlLight;
            dockPaneStripToolWindowGradient1.DockStripGradient = dockPanelGradient3;
            tabGradient6.EndColor = SystemColors.ActiveBorder;
            tabGradient6.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            tabGradient6.StartColor = SystemColors.ActiveBorder;
            tabGradient6.TextColor = SystemColors.ControlText;
            dockPaneStripToolWindowGradient1.InactiveCaptionGradient = tabGradient6;
            tabGradient7.EndColor = Color.Transparent;
            tabGradient7.StartColor = Color.Transparent;
            tabGradient7.TextColor = SystemColors.ControlDarkDark;
            dockPaneStripToolWindowGradient1.InactiveTabGradient = tabGradient7;
            dockPaneStripSkin1.ToolWindowGradient = dockPaneStripToolWindowGradient1;
            dockPanelSkin1.DockPaneStripSkin = dockPaneStripSkin1;
            dockPanel.Skin = dockPanelSkin1;
            dockPanel.TabIndex = 2;
            // 
            // frmMain
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoValidate = AutoValidate.EnablePreventFocusChange;
            BackColor = SystemColors.ControlLight;
            ClientSize = new Size(1464, 529);
            ContextMenuStrip = mnuContext;
            Controls.Add(dockPanel);
            Controls.Add(toolBarStrip);
            Controls.Add(statusBarStrip);
            Controls.Add(mnuMainMenu);
            Icon = (Icon)resources.GetObject("$this.Icon");
            IsMdiContainer = true;
            MainMenuStrip = mnuMainMenu;
            Name = "frmMain";
            StartPosition = FormStartPosition.Manual;
            Text = "frmMain";
            Closing += new CancelEventHandler(FrmMain_Closing);
            Move += new EventHandler(FrmMain_Move);
            Resize += new EventHandler(FrmMain_Resize);
            mnuMainMenu.ResumeLayout(false);
            mnuMainMenu.PerformLayout();
            mnuContext.ResumeLayout(false);
            mnuContextAddFilter.ResumeLayout(false);
            ((ISupportInitialize)timDelayAlerts).EndInit();
            ((ISupportInitialize)timProcessTimers).EndInit();
            statusBarStrip.ResumeLayout(false);
            statusBarStrip.PerformLayout();
            toolBarStrip.ResumeLayout(false);
            toolBarStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

//        [STAThread]

        //static void Main()

        //{
        //    Application.EnableVisualStyles();
        //    Application.SetCompatibleTextRenderingDefault(false);
        //    try {Application.Run(new FrmMain());}
        //    catch (Exception e)

        //    {
        //        string s = $"Uncaught exception in Main(): {e.Message}";

        //        LogLib.WriteLine(s);

        //        MessageBox.Show(s);

        //        Application.Exit();
        //    }
        //}

        private void FrmMain_Closing(object sender, CancelEventArgs e)

        {
            if (Settings.Default.SaveOnExit)
            {
//                string mypath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MySEQ");
                SavePrefs();
//                SmtpSettings.Default.Save(myseqFile);
            }
        }

        public void CmdCommand_Click(object sender, EventArgs e)

        {
            if (!bIsRunning)

            {
                StartListening();
            }
            else
            {
                StopListening();
            }
        }

        //public void NewMap()
        //{
        //    map?.NewMap();
        //}

        public void StartListening()
        {
            colProcesses?.Clear();

            if (eq.gamerInfo != null)
            {
                eq.gamerInfo.Name = "";
            }

            if (mnuIPAddress1.Checked)
            {
                currentIPAddress = Settings.Default.IPAddress1;
            }
            else if (mnuIPAddress2.Checked)
            {
                currentIPAddress = Settings.Default.IPAddress2;
            }
            else if (mnuIPAddress3.Checked)
            {
                currentIPAddress = Settings.Default.IPAddress3;
            }
            else if (mnuIPAddress4.Checked)
            {
                currentIPAddress = Settings.Default.IPAddress4;
            }
            else if (mnuIPAddress5.Checked)
            {
                currentIPAddress = Settings.Default.IPAddress5;
            }

            if (currentIPAddress.Length == 0)
            {
                return;
            }

            // Try to connect to the server         

            if (!comm.ConnectToServer(currentIPAddress, Settings.Default.Port))

            {
                return;
            }
            else
            {
                toolStripServerAddress.Text = currentIPAddress;
            }

            // Clear map

            map.ClearMap();

            // Start the timer

            timPackets.Start();

            mapCon.Focus();

            mapPane.cmdCommand.Text = "Stop";

            mnuConnect.Text = "&Disconnect";

            mnuConnect.Image = Resources.RedDelete;

            toolStripStartStop.Text = "Stop";
            toolStripStartStop.ToolTipText = "Disconnect from Server";
            toolStripStartStop.Image = Resources.RedDelete;

            bIsRunning = true;
        }

        public void SetCharSelection(string player_name)
        {
            mnuChar1.Checked = player_name == mnuChar1.Text;

            mnuChar2.Checked = player_name == mnuChar2.Text;

            mnuChar3.Checked = player_name == mnuChar3.Text;

            mnuChar4.Checked = player_name == mnuChar4.Text;

            mnuChar5.Checked = player_name == mnuChar5.Text;

            mnuChar6.Checked = player_name == mnuChar6.Text;

            mnuChar7.Checked = player_name == mnuChar7.Text;

            mnuChar8.Checked = player_name == mnuChar8.Text;

            mnuChar9.Checked = player_name == mnuChar9.Text;

            mnuChar10.Checked = player_name == mnuChar10.Text;

            mnuChar10.Checked = player_name == mnuChar10.Text;

            mnuChar11.Checked = player_name == mnuChar11.Text;

            mnuChar12.Checked = player_name == mnuChar12.Text;
        }

        public void SetTitle()
        {
            Text = BaseTitle;

            if (Settings.Default.ShowZoneName && eq.longname.Length > 0 && eq.longname != "map_pane")
            {
                Text += " - " + eq.longname;
            }

            if (Settings.Default.ShowCharName && eq.gamerInfo?.Name.Length > 1)
            {
                Text += " - " + eq.gamerInfo.Name;
            }
        }

        public void LoadPrefs()

        {
            // Always want these off on starting up.

            Settings.Default.CollectMobTrails = false;
            //            Settings.Default.EmailAlerts = false;
            Settings.Default.DepthFilter = false;

            SetGridInterval();

            // restore the normal windows state, if we were closed maximized
            if (Settings.Default.WindowState == FormWindowState.Maximized)
            {
                Location = Settings.Default.WindowsLocation;
                StartPosition = FormStartPosition.Manual;
                WindowState = FormWindowState.Maximized;
            }
            else
            {
                WindowState = Settings.Default.WindowState;
                Size = Settings.Default.WindowsSize;
                Location = Settings.Default.WindowsLocation;
                StartPosition = Settings.Default.WindowsPosition;
            }

            if (Settings.Default.TitleBar.Length > 0)
            {
                BaseTitle = Settings.Default.TitleBar;
            }

            mnuShowSpawnList.Checked = Settings.Default.ShowMobList;

            mnuShowSpawnListTimer.Checked = Settings.Default.ShowMobListTimer;

            mnuShowGroundItemList.Checked = Settings.Default.ShowGroundItemList;

            mnuShowGridLines.Checked = (Settings.Default.DrawOptions & DrawOptions.GridLines) != DrawOptions.None;

            mnuShowZoneText.Checked = mnuShowZoneText2.Checked = (Settings.Default.DrawOptions & DrawOptions.ZoneText) != DrawOptions.None;

            mnuShowLayer1.Checked = mnuShowLayer21.Checked = Settings.Default.ShowLayer1;
            mnuShowLayer2.Checked = mnuShowLayer22.Checked = Settings.Default.ShowLayer2;
            mnuShowLayer3.Checked = mnuShowLayer23.Checked = Settings.Default.ShowLayer3;

            mnuShowListGridLines.Checked = Settings.Default.ShowListGridLines;
            SpawnList.listView.GridLines = Settings.Default.ShowListGridLines;
            SpawnTimerList.listView.GridLines = Settings.Default.ShowListGridLines;
            GroundItemList.listView.GridLines = Settings.Default.ShowListGridLines;

            mnuShowListSearchBox.Checked = Settings.Default.ShowListSearchBox;
            if (!Settings.Default.ShowListSearchBox)
            {
                SpawnList.HideSearchBox();
                SpawnTimerList.HideSearchBox();
                GroundItemList.HideSearchBox();
            }

            SetFollowOption(Settings.Default.FollowOption);

            mnuKeepCentered.Checked = mnuKeepCentered2.Checked = Settings.Default.KeepCentered;

            mnuShowTargetInfo.Checked = mnuShowTargetInfo2.Checked = Settings.Default.ShowTargetInfo;

            mnuSmallTargetInfo.Checked = mnuSmallTargetInfo2.Checked = Settings.Default.SmallTargetInfo;

            mnuAutoSelectEQTarget.Checked = mnuAutoSelectEQTarget2.Checked = Settings.Default.AutoSelectEQTarget;

            mnuAutoExpand.Checked = mnuAutoExpand2.Checked = Settings.Default.AutoExpand;

            // new filter stuff

            mnuShowNPCs.Checked = Settings.Default.ShowNPCs;

            mnuShowCorpses.Checked = Settings.Default.ShowCorpses;

            mnuShowPCCorpses.Checked = Settings.Default.ShowPCCorpses;

            mnuShowMyCorpse.Checked = Settings.Default.ShowMyCorpse;

            mnuShowPlayers.Checked = Settings.Default.ShowPlayers;

            mnuShowInvis.Checked = Settings.Default.ShowInvis;

            mnuShowMounts.Checked = Settings.Default.ShowMounts;

            mnuShowFamiliars.Checked = Settings.Default.ShowFamiliars;

            mnuShowPets.Checked = Settings.Default.ShowPets;

            mnuShowLookupText.Checked = Settings.Default.ShowLookupText;

            mnuAlwaysOnTop.Checked = Settings.Default.AlwaysOnTop;

            mnuShowLookupNumber.Checked = Settings.Default.ShowLookupNumber;

            mnuShowSpawnPoints.Checked = mnuShowSpawnPoints2.Checked = (Settings.Default.DrawOptions & DrawOptions.SpawnTimers) != DrawOptions.None;

            mnuDepthFilter.Checked = mnuDepthFilter2.Checked = Settings.Default.DepthFilter;

            // update the toolbar settings
            toolStripDepthFilterButton.Checked = Settings.Default.DepthFilter;
            toolStripZNegUp.Enabled = Settings.Default.DepthFilter;
            toolStripZNeg.Enabled = Settings.Default.DepthFilter;
            toolStripZNegDown.Enabled = Settings.Default.DepthFilter;
            toolStripZPosDown.Enabled = Settings.Default.DepthFilter;
            toolStripZOffsetLabel.Enabled = Settings.Default.DepthFilter;
            toolStripZPosUp.Enabled = Settings.Default.DepthFilter;
            toolStripZPos.Enabled = Settings.Default.DepthFilter;
            toolStripZPosLabel.Enabled = Settings.Default.DepthFilter;
            toolStripResetDepthFilter.Enabled = Settings.Default.DepthFilter;

            if (Settings.Default.DepthFilter)
            {
                toolStripDepthFilterButton.Image = Resources.ExpandSpaceHS;
            }

            mnuDynamicAlpha.Checked = mnuDynamicAlpha2.Checked = Settings.Default.UseDynamicAlpha;

            mnuForceDistinct.Checked = mnuForceDistinct2.Checked = Settings.Default.ForceDistinct;

            mnuForceDistinctText.Checked = mnuForceDistinctText2.Checked = Settings.Default.ForceDistinctText;

            mnuCollectMobTrails.Checked = Settings.Default.CollectMobTrails;

            mnuShowMobTrails.Checked = (Settings.Default.DrawOptions & DrawOptions.SpawnTrails) != DrawOptions.None;

            mnuConSoD.Checked = Settings.Default.SoDCon;

            mnuConDefault.Checked = Settings.Default.DefaultCon;

            mnuConSoF.Checked = Settings.Default.SoFCon;

            mnuShowPCNames.Checked = mnuShowPCNames2.Checked = Settings.Default.ShowPCNames;

            mnuShowNPCNames.Checked = mnuShowNPCNames2.Checked = Settings.Default.ShowNPCNames;

//            mnuShowPCGuild.Checked = mnuShowPCGuild2.Checked = Settings.Default.ShowPCGuild;

            mnuSaveSpawnLog.Checked = Settings.Default.SaveSpawnLogs;

            mnuShowNPCCorpseNames.Checked = mnuShowNPCCorpseNames2.Checked = Settings.Default.ShowNPCCorpseNames;

            mnuShowPlayerCorpseNames.Checked = mnuShowPlayerCorpseNames2.Checked = Settings.Default.ShowPlayerCorpseNames;

            mnuShowPVP.Checked = mnuShowPVP2.Checked = Settings.Default.ShowPVP;

            mnuShowPVPLevel.Checked = mnuShowPVPLevel2.Checked = Settings.Default.ShowPVPLevel;

            mnuShowNPCLevels.Checked = mnuShowNPCLevels2.Checked = Settings.Default.ShowNPCLevels;

            mnuShowLookupText.Checked = Settings.Default.ShowLookupText;

            mnuAlwaysOnTop.Checked = Settings.Default.AlwaysOnTop;

            mnuShowLookupNumber.Checked = Settings.Default.ShowLookupNumber;

            mnuFilterMapLines.Checked = mnuFilterMapLines2.Checked = Settings.Default.FilterMapLines;

            mnuFilterMapText.Checked = mnuFilterMapText2.Checked = Settings.Default.FilterMapText;

            mnuFilterNPCs.Checked = mnuFilterNPCs2.Checked = Settings.Default.FilterNPCs;

            mnuFilterPlayers.Checked = mnuFilterPlayers2.Checked = Settings.Default.FilterPlayers;

            mnuFilterSpawnPoints.Checked = mnuFilterSpawnPoints2.Checked = Settings.Default.FilterSpawnPoints;

            mnuFilterPlayerCorpses.Checked = mnuFilterPlayerCorpses2.Checked = Settings.Default.FilterPlayerCorpses;

            mnuFilterGroundItems.Checked = mnuFilterGroundItems2.Checked = Settings.Default.FilterGroundItems;

            mnuFilterNPCCorpses.Checked = mnuFilterNPCCorpses2.Checked = Settings.Default.FilterNPCCorpses;

            mnuSpawnCountdown.Checked = mnuSpawnCountdown2.Checked = Settings.Default.SpawnCountdown;

            mnuShowMenuBar.Checked = Settings.Default.ShowMenuBar;

            mnuViewStatusBar.Checked = Settings.Default.ShowStatusBar;

            mnuViewDepthFilterBar.Checked = Settings.Default.ShowToolBar;

            if (!Settings.Default.ShowStatusBar)
            {
                statusBarStrip.Hide();
            }

            if (!Settings.Default.ShowToolBar)
            {
                toolBarStrip.Hide();
            }

            mnuViewMenuBar.Checked = Settings.Default.ShowMenuBar;

            if (Settings.Default.ShowMenuBar)
            {
                mnuMainMenu.Show();
            }
            else
            {
                mnuMainMenu.Hide();
            }

            if (Settings.Default.CurrentIPAddress == 0 && Settings.Default.IPAddress1.Length > 0)
            {
                Settings.Default.CurrentIPAddress = 1;
            }

            ResetMenu(Settings.Default.CurrentIPAddress);

            ServerSelection();

            mnuAutoConnect.Checked = Settings.Default.AutoConnect;

            eq.LoadSpawnInfo();

            // SpawnList

            SpawnList.listView.BackColor = Settings.Default.ListBackColor;

            SpawnTimerList.listView.BackColor = Settings.Default.ListBackColor;

            GroundItemList.listView.BackColor = Settings.Default.ListBackColor;

            SpawnList.listView.GridLines = Settings.Default.ShowListGridLines;

            SpawnTimerList.listView.GridLines = Settings.Default.ShowListGridLines;

            GroundItemList.listView.GridLines = Settings.Default.ShowListGridLines;

            LogLib.maxLogLevel = Settings.Default.MaxLogLevel;

            CreateFolders();

            DrawOpts = Settings.Default.DrawOptions;

            timProcessTimers.Start();
        }

        private static void CreateFolders()
        {
            if (Settings.Default.MapDir?.Length == 0 || !Directory.Exists(Settings.Default.MapDir))

            {
                Settings.Default.MapDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "maps");

                if (!Directory.Exists(Settings.Default.MapDir))
                {
                    Directory.CreateDirectory(Settings.Default.MapDir);
                }
            }

            if (Settings.Default.FilterDir?.Length == 0 || !Directory.Exists(Settings.Default.FilterDir))

            {
                Settings.Default.FilterDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "filters");

                if (!Directory.Exists(Settings.Default.FilterDir))
                {
                    Directory.CreateDirectory(Settings.Default.FilterDir);
                }
            }

            if (Settings.Default.CfgDir?.Length == 0 || !Directory.Exists(Settings.Default.CfgDir))

            {
                Settings.Default.CfgDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cfg");

                if (!Directory.Exists(Settings.Default.CfgDir))
                {
                    Directory.CreateDirectory(Settings.Default.CfgDir);
                }
            }

            if (Settings.Default.LogDir?.Length == 0 || !Directory.Exists(Settings.Default.LogDir))

            {
                Settings.Default.LogDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");

                if (!Directory.Exists(Settings.Default.LogDir))
                {
                    Directory.CreateDirectory(Settings.Default.LogDir);
                }
            }

            if (Settings.Default.TimerDir?.Length == 0 || !Directory.Exists(Settings.Default.TimerDir))

            {
                Settings.Default.TimerDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "timers");

                if (!Directory.Exists(Settings.Default.TimerDir))
                {
                    Directory.CreateDirectory(Settings.Default.TimerDir);
                }
            }
        }

        public void SavePrefs()//string filename
        {
            Settings.Default.Save();
        }

        public bool Loadmap(string filename)

        {
            try

            {
                if (filename.EndsWith(".map"))

                {
                    if (!map.LoadMap(filename))

                    {
                        //this.Text = BaseTitle + " ERROR LOADING MAP: " + filename;

                        return false;
                    }
                }
                else if (filename.EndsWith(".txt"))

                {
                    if (filename.EndsWith("_1.txt") || filename.EndsWith("_2.txt") || filename.EndsWith("_3.txt"))

                    {
                        if (!map.LoadLoYMap(filename, false))

                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (!map.LoadLoYMap(filename, true))

                        {
                            //this.Text = BaseTitle + " ERROR LOADING MAP: " + filename;

                            return false;
                        }
                    }
                }

                SetTitle();

                return true;
            }
            catch(Exception ex)

            {
                string msg = $"Failed to load map {filename}: {ex.Message}";

                LogLib.WriteLine(msg);

                MessageBox.Show(msg);

                return false;
            }
        }

        private void FrmMain_Move(object sender, EventArgs e)

        {
            if (WindowState == FormWindowState.Normal)

            {
                Settings.Default.WindowsLocation = Location;
            }
        }

        private void FrmMain_Resize(object sender, EventArgs e)

        {
            if (WindowState == FormWindowState.Normal)

            {
                Settings.Default.WindowsSize = Size;
            }

            Settings.Default.WindowState = WindowState;

            ReAdjust();
        }

        public void CheckMobs()

        {
            eq.CheckMobs(SpawnList, GroundItemList);
        }

        private void TimPackets_Tick(object sender, EventArgs e)

        {
            DrawOpts = Settings.Default.DrawOptions;

            comm.Tick();
            mapCon.Tick();
        }

        private void TimDelayPlay_Tick(object sender, EventArgs e)
        {
            // our alert sound/email delay interval has passed.
            EnablePlayAlerts();
            timDelayAlerts.Stop();
        }

        private void TimProcessTimers_Tick(object sender, EventArgs e)
        {
            // allow processing timers.
            ProcessSpawnTimer();

            if (!bIsRunning && mapCon != null)
            {
                mapCon.Invalidate();
            }
        }

        private void SpawnList_VisibleChanged(object sender, EventArgs e)

        {
            Settings.Default.ShowMobList = SpawnList.Visible;

            mnuShowSpawnList.Checked = SpawnList.Visible;
        }

        private void SpawnTimerList_VisibleChanged(object sender, EventArgs e)

        {
            Settings.Default.ShowMobListTimer = SpawnTimerList.Visible;

            mnuShowSpawnListTimer.Checked = SpawnTimerList.Visible;
        }

        private void GroundItemList_VisibleChanged(object sender, EventArgs e)
        {
            Settings.Default.ShowGroundItemList = GroundItemList.Visible;

            mnuShowGroundItemList.Checked = GroundItemList.Visible;
        }

        #region ProcessProcessInfo

        private void ProcessProcessInfo(SPAWNINFO si)

        {
            ProcessInfo PI = new ProcessInfo(si.SpawnID, si.Name);

            if (si.SpawnID==0)

            {
                PI.SCharName = "";
                CurrentProcess = PI;
                if (comm != null)
                {
                    comm.newProcessID = 0;
                }
            }
            else
            {
                processcount++;

                while (colProcesses.Count > 0 && colProcesses.Count >= processcount) //si.Level)

                {
                    colProcesses.Remove(colProcesses[colProcesses.Count-1]);
                }

                colProcesses.Add(PI);

                if (colProcesses.Count == 1)
                {
                    mnuChar1.Text = si.Name;
                    mnuChar1.Visible = true;

                    mnuChar2.Visible = false;
                    mnuChar2.Text = "Char 2";
                    mnuChar2.Checked = false;

                    mnuChar1.Checked = (CurrentProcess != null) && (CurrentProcess.ProcessID == PI.ProcessID);
                }
                else if (colProcesses.Count == 2)
                {
                    mnuChar2.Text = si.Name;
                    mnuChar2.Visible = true;

                    mnuChar3.Visible = false;
                    mnuChar3.Text = "Char 3";
                    mnuChar3.Checked = false;

                    mnuChar2.Checked = (CurrentProcess != null) && (CurrentProcess.ProcessID == PI.ProcessID);
                }
                else if (colProcesses.Count == 3)
                {
                    mnuChar3.Text = si.Name;
                    mnuChar3.Visible = true;

                    mnuChar4.Visible = false;
                    mnuChar4.Text = "Char 4";
                    mnuChar4.Checked = false;

                    mnuChar3.Checked = (CurrentProcess != null) && (CurrentProcess.ProcessID == PI.ProcessID);
                }
                else if (colProcesses.Count == 4)
                {
                    mnuChar4.Text = si.Name;
                    mnuChar4.Visible = true;

                    mnuChar5.Visible = false;
                    mnuChar5.Text = "Char 5";
                    mnuChar5.Checked = false;

                    mnuChar4.Checked = (CurrentProcess != null) && (CurrentProcess.ProcessID == PI.ProcessID);
                }
                else if (colProcesses.Count == 5)
                {
                    mnuChar5.Text = si.Name;
                    mnuChar5.Visible = true;

                    mnuChar6.Visible = false;
                    mnuChar6.Text = "Char 6";
                    mnuChar6.Checked = false;

                    mnuChar5.Checked = (CurrentProcess != null) && (CurrentProcess.ProcessID == PI.ProcessID);
                }
                else if (colProcesses.Count == 6)
                {
                    mnuChar6.Text = si.Name;
                    mnuChar6.Visible = true;

                    mnuChar7.Visible = false;
                    mnuChar7.Text = "Char 7";
                    mnuChar7.Checked = false;

                    mnuChar6.Checked = (CurrentProcess != null) && (CurrentProcess.ProcessID == PI.ProcessID);
                }
                else if (colProcesses.Count == 7)
                {
                    mnuChar7.Text = si.Name;
                    mnuChar7.Visible = true;

                    mnuChar8.Visible = false;
                    mnuChar8.Text = "Char 8";
                    mnuChar8.Checked = false;

                    mnuChar7.Checked = (CurrentProcess != null) && (CurrentProcess.ProcessID == PI.ProcessID);
                }
                else if (colProcesses.Count == 8)
                {
                    mnuChar8.Text = si.Name;
                    mnuChar8.Visible = true;

                    mnuChar9.Visible = false;
                    mnuChar9.Text = "Char 9";
                    mnuChar9.Checked = false;

                    mnuChar8.Checked = (CurrentProcess != null) && (CurrentProcess.ProcessID == PI.ProcessID);
                }
                else if (colProcesses.Count == 9)
                {
                    mnuChar9.Text = si.Name;
                    mnuChar9.Visible = true;

                    mnuChar10.Visible = false;
                    mnuChar10.Text = "Char 10";
                    mnuChar10.Checked = false;

                    mnuChar9.Checked = (CurrentProcess != null) && (CurrentProcess.ProcessID == PI.ProcessID);
                }
                else if (colProcesses.Count == 10)
                {
                    mnuChar10.Text = si.Name;
                    mnuChar10.Visible = true;

                    mnuChar11.Visible = false;
                    mnuChar11.Text = "Char 11";
                    mnuChar11.Checked = false;

                    mnuChar10.Checked = (CurrentProcess != null) && (CurrentProcess.ProcessID == PI.ProcessID);
                }
                else if (colProcesses.Count == 11)
                {
                    mnuChar11.Text = si.Name;
                    mnuChar11.Visible = true;
                    mnuChar12.Visible = false;
                    mnuChar12.Text = "Char 12";
                    mnuChar12.Checked = false;

                    mnuChar11.Checked = (CurrentProcess != null) && (CurrentProcess.ProcessID == PI.ProcessID);
                }
                else if (colProcesses.Count == 12)
                {
                    mnuChar12.Text = si.Name;
                    mnuChar12.Visible = true;
                    mnuChar12.Checked = (CurrentProcess != null) && (CurrentProcess.ProcessID == PI.ProcessID);
                }
            }
        }

        #endregion

        #region ProccessMap

        public void ProcessMap(SPAWNINFO si)

        {
            mapnameWithLabels = "";

            try

            {
                LogLib.WriteLine($"ProcesssMap: Short Zone Name: ({si.Name})");

                bool foundmap = false;

                string f = Settings.Default.MapDir + "\\";

                string fn = si.Name.Trim();

                int location = fn.IndexOf("_", 0);

                if (location > 0)
                {
                    fn = fn.Substring(0, location);
                }

                LogLib.WriteLine($"Using Short Zone Name: ({fn})");

                f += fn;

                toolStripShortName.Text = fn.ToUpper();

                curZone = fn.ToUpper().Trim();

                string ZonesFile = Path.Combine(Settings.Default.CfgDir, "Zones.ini");

                if (File.Exists(ZonesFile))
                {
                    IniFile Ini = new IniFile(ZonesFile);

                    string sIniValue = "";

                    if (curZone.Length == 0)
                    {
                        mapPane.TabText = "map_pane";
                    }
                    else
                    {
                        sIniValue = Ini.ReadValue("Zones", curZone, curZone.ToLower());
                        mapPane.TabText = sIniValue;
                    }
                }
                else
                {
                    mapPane.TabText = curZone.Length > 0 ? curZone.ToLower() : "map_pane";
                }

                // Turn off collecting mob trails anytime load a new map

                mnuCollectMobTrails.Checked = false;

                Settings.Default.CollectMobTrails = false;

                // Start Delay for doing spawn alerts.  This stops sounds and emails.
                timDelayAlerts.Start();
                DisablePlayAlerts();

                try

                {
                    if (curZone.Length > 0 && curZone != "CLZ" && curZone != "DEFAULT")
                    {
                        // Try loading depth filter settings from file
                        string myPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MySEQ");

                        string ConfigFile = Path.Combine(myPath, "config.ini");
                        if (File.Exists(ConfigFile))
                        {
                            IniFile ConIni = new IniFile(ConfigFile);

//                            string strIniValue = "";

                            var strIniValue = ConIni.ReadValue("Zones", curZone, "");
                            if (strIniValue.Length > 0)
                            {
                                if ((strIniValue == "0" && Settings.Default.DepthFilter) ||
                                (strIniValue == "1" && !Settings.Default.DepthFilter))
                                {
                                    ToggleDepthFilter();
                                }
                            }
                            else
                            {
                                // We dont currently have a setting for this zone, so set to off
                                if (Settings.Default.DepthFilter)
                                {
                                    ToggleDepthFilter();
                                }
                            }
                        }
                        else
                        {
                            // We dont currently have a setting file, so set depth filter to off
                            if (Settings.Default.DepthFilter)
                            {
                                ToggleDepthFilter();
                            }
                        }
                    }

                    if (curZone.Length == 0 || curZone == "CLZ" || curZone == "DEFAULT")

                    {
                        if (Settings.Default.DepthFilter)
                        {
                            ToggleDepthFilter();
                        }

                        eq.Zoning = true;

                        map.LoadDummyMap(curZone);

                        //this.Text = BaseTitle;

                        foundmap = true;

                        mapnameWithLabels = "";
                    }

                    // Try it as a SEQ map first   

                    else if (Loadmap(f + ".map"))
                    {
                        eq.Zoning = false;
                        foundmap = true;

                        mapnameWithLabels = f + ".map";
                    } else {
                        eq.Zoning = false;
                        // If it didn't work, try an SOE map

                        if (Loadmap(f + ".txt"))
                        {
                            foundmap = true;
                        }

                        if (Settings.Default.ShowLayer1 && Loadmap(f + "_1.txt"))
                        {
                            foundmap = true;
                        }

                        if (Settings.Default.ShowLayer2 && Loadmap(f + "_2.txt"))
                        {
                            foundmap = true;
                        }

                        if (Settings.Default.ShowLayer3 && Loadmap(f + "_3.txt"))
                        {
                            foundmap = true;
                        }

                        // use _3.txt file for map labels
                        if (foundmap)
                        {
                            mapnameWithLabels = $"{f}_3.txt";
                        }

                        //SetTitle();

                    }
                    //... Missing map

                    if (!foundmap)
                    {
                        map.LoadDummyMap(fn);
                    }
                }
                catch (Exception ex)

                {
                    LogLib.WriteLine("Error in ProcessMap() Load Map: ", ex);

                    map.LoadDummyMap(fn);
                }

                eq.longname = mapPane.TabText;

                filters.ClearArrays();

                filters.LoadAlerts(fn);

                SetTitle();

                //this.Text = BaseTitle;

            }
            catch (Exception ex) {LogLib.WriteLine("Error in ProcessMap(): ", ex);}
        }

        #endregion

        #region ProcessSpawnList

        public void ProcessSpawnList()

        {
            eq.ProcessSpawnList(SpawnList);
        }

        #endregion

        #region ProcessGroundItemList

        public void ProcessGroundItemList()
        {
            eq.ProcessGroundItemList(GroundItemList);
        }

        #endregion

        #region ProcessSpawnTimer

        public void ProcessSpawnTimer()

        {
            if (eq.mobsTimers.mobsTimer2.Count > 0)
            {
                eq.mobsTimers.UpdateList(SpawnTimerList);
            }
        }

        #endregion

        private void SetGridInterval()

        {
            mnuGridInterval100.Checked = false;

            mnuGridInterval250.Checked = false;

            mnuGridInterval500.Checked = false;

            mnuGridInterval1000.Checked = false;

            if (Settings.Default.GridInterval<=100)
            {
                mnuGridInterval100.Checked = true;
            }
            else if (Settings.Default.GridInterval<=250)
            {
                mnuGridInterval250.Checked = true;
            }
            else if (Settings.Default.GridInterval<=500)
            {
                mnuGridInterval500.Checked = true;
            }
            else
            {
                mnuGridInterval1000.Checked = true;
            }
        }

        private int GridInterval()

        {
            if (mnuGridInterval100.Checked)
            {
                return 100;
            }
            else if (mnuGridInterval250.Checked)
            {
                return 250;
            }
            else if (mnuGridInterval500.Checked)
            {
                return 500;
            }
            else
            {
                return 1000;
            }
        }

        public void SetContextMenu()
        {
            if (alertAddmobname.Length > 0)
            {
                ContextMenuStrip = mnuContextAddFilter;
                // set text for mob name in the top
                mnuMobName.Text = "'" + alertAddmobname + "'";
                mnuMobName.Enabled = true;
                mnuMobName.Visible = true;
                // dont add email alerts for ground items
//                addZoneEmailAlertFilterToolStripMenuItem.Enabled = notground;
                mnuAddMapLabel.Enabled = mapnameWithLabels.Length > 0;
            }
            else
            {
                // Set the default context menu, since we don't have a proper name to work with
                ContextMenuStrip = mnuContext;
                addMapTextToolStripMenuItem.Enabled = eq.longname.Length > 0 && eq.gamerInfo?.Name.Length > 0;
                mnuShowMenuBar.Visible = !Settings.Default.ShowMenuBar;
            }
        }

        private void MnuOpenMap_Click(object sender, EventArgs e)

        {
            openFileDialog.InitialDirectory = Settings.Default.MapDir;

            openFileDialog.Filter = "Map Files (*.map;*.txt)|*.map;*.txt|All Files (*.*)|*.*";

            openFileDialog.FilterIndex = 1;

            if (openFileDialog.ShowDialog() == DialogResult.OK)

            {
                mapnameWithLabels = "";

                string filename = openFileDialog.FileName;

                Loadmap(filename);

                int lastSlashIndex = filename.LastIndexOf("\\");

                if (lastSlashIndex > 0)
                {
                    filename = filename.Substring(lastSlashIndex+1);
                }

                filename = filename.Substring(0, filename.Length - 4);

                if (filename.EndsWith("_1"))
                {
                    filename = filename.Substring(0, filename.Length - 2);
                }

                toolStripShortName.Text = filename.ToUpper();

                mapPane.TabText = filename.ToLower();

                curZone = filename.ToUpper();
            }
        }

        private void MnuSaveMobs_Click(object sender, EventArgs e)

        {
            eq.SaveMobs();
        }

        private void MnuSavePrefs_Click(object sender, EventArgs e)

        {
            SavePrefs();
        }

        private void MnuExit_Click(object sender, EventArgs e)

        {
            StopListening();

            Close();

            Application.Exit();
        }

        private void MnuOptions_Click(object sender, EventArgs e)

        {
            FrmOptions f3 = new FrmOptions();
            if (Settings.Default.OptionsWindowsLocation.X != 0 && Settings.Default.OptionsWindowsLocation.Y != 0)
            {
                f3.StartPosition = FormStartPosition.CenterParent;
                f3.Location = Settings.Default.OptionsWindowsLocation;
                f3.Size = Settings.Default.OptionsWindowsSize;
            }
            // Options form now handles getting and changing the values to settings. poor practice to let a method in a diff class do it. 

            f3.ShowDialog();
            if (f3.DialogResult.ToString() == "Cancel")
            {
                f3.Close();
                return;
            }
            timPackets.Interval = Settings.Default.UpdateDelay;
            SetUpdateSteps();
            ReloadAlertFiles();
            ResetMapPens();
            SpawnList.listView.BackColor = Settings.Default.ListBackColor;

            SpawnTimerList.listView.BackColor = Settings.Default.ListBackColor;

            GroundItemList.listView.BackColor = Settings.Default.ListBackColor;

            if (Settings.Default.TitleBar.Length > 0)
            {
                BaseTitle = Settings.Default.TitleBar;
            }

            DrawOpts = Settings.Default.DrawOptions;

            mnuShowGridLines.Checked = (Settings.Default.DrawOptions & DrawOptions.GridLines) != DrawOptions.None;
            mnuShowZoneText.Checked = (Settings.Default.DrawOptions & DrawOptions.ZoneText) != DrawOptions.None;
            mnuShowLayer1.Checked = Settings.Default.ShowLayer1;
            mnuShowLayer2.Checked = Settings.Default.ShowLayer2;
            mnuShowLayer3.Checked = Settings.Default.ShowLayer3;
            mnuShowSpawnPoints.Checked = (Settings.Default.DrawOptions & DrawOptions.SpawnTimers) != DrawOptions.None;

            ServerSelection();

            eq?.LoadSpawnInfo();

            SetTitle();

            SavePrefs();

            mapCon?.Invalidate();
        }

        private void ServerSelection()

        {
            mnuIPAddress1.Text = Settings.Default.IPAddress1;

            mnuIPAddress2.Text = Settings.Default.IPAddress2;

            mnuIPAddress3.Text = Settings.Default.IPAddress3;

            mnuIPAddress4.Text = Settings.Default.IPAddress4;

            mnuIPAddress5.Text = Settings.Default.IPAddress5;

            mnuIPAddress1.Enabled = mnuIPAddress1.Visible = Convert.ToBoolean(mnuIPAddress1.Text.Length);

            mnuIPAddress2.Enabled = mnuIPAddress2.Visible = Convert.ToBoolean(mnuIPAddress2.Text.Length);

            mnuIPAddress3.Enabled = mnuIPAddress3.Visible = Convert.ToBoolean(mnuIPAddress3.Text.Length);

            mnuIPAddress4.Enabled = mnuIPAddress4.Visible = Convert.ToBoolean(mnuIPAddress4.Text.Length);

            mnuIPAddress5.Enabled = mnuIPAddress5.Visible = Convert.ToBoolean(mnuIPAddress5.Text.Length);
        }

        private void MnuRefreshSpawnList_Click(object sender, EventArgs e)

        {
            DisablePlayAlerts();

            eq.mobsTimers.ResetTimers();

            map.ClearMap();

            eq.mobsTimers.LoadTimers();
        }

        private void MnuDepthFilter_Click(object sender, EventArgs e)

        {
            ToggleDepthFilter();
            if (curZone.Length > 0 && curZone != "CLZ" && curZone != "DEFAULT")
            {
                try
                {
                    // Save depth filter settings to file
                    string myPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MySEQ");
                    if (!Directory.Exists(myPath))
                    {
                        Directory.CreateDirectory(myPath);
                    }

                    string ConfigFile = Path.Combine(myPath, "config.ini");

                    IniFile ConIni = new IniFile(ConfigFile);
                    if (Settings.Default.DepthFilter)
                    {
                        ConIni.WriteValue("Zones", curZone, "1");
                    }
                    else
                    {
                        ConIni.WriteValue("Zones", curZone, "0");
                    }
                }
                catch (Exception ex) { LogLib.WriteLine("Error writing depth filter setting to ini file: ", ex); }
            }
        }

        private void ToggleDepthFilter()
        {
            mnuDepthFilter.Checked = !mnuDepthFilter.Checked;
            mnuDepthFilter2.Checked = mnuDepthFilter.Checked;

            Settings.Default.DepthFilter = mnuDepthFilter.Checked;

            // update the toolbar settings
            toolStripDepthFilterButton.Checked = Settings.Default.DepthFilter;
            toolStripZNegUp.Enabled = Settings.Default.DepthFilter;
            toolStripZNeg.Enabled = Settings.Default.DepthFilter;
            toolStripZNegDown.Enabled = Settings.Default.DepthFilter;
            toolStripZPosDown.Enabled = Settings.Default.DepthFilter;
            toolStripZOffsetLabel.Enabled = Settings.Default.DepthFilter;
            toolStripZPosUp.Enabled = Settings.Default.DepthFilter;
            toolStripZPos.Enabled = Settings.Default.DepthFilter;
            toolStripZPosLabel.Enabled = Settings.Default.DepthFilter;
            toolStripResetDepthFilter.Enabled = Settings.Default.DepthFilter;

            toolStripDepthFilterButton.Image = Settings.Default.DepthFilter ? Resources.ExpandSpaceHS : Resources.ShrinkSpaceHS;
        }

        private void MnuDynamicAlpha_Click(object sender, EventArgs e)

        {
            mnuDynamicAlpha.Checked = !mnuDynamicAlpha.Checked;
            mnuDynamicAlpha2.Checked = mnuDynamicAlpha.Checked;

            Settings.Default.UseDynamicAlpha = mnuDynamicAlpha.Checked;
        }

        private void MnuForceDistinct_Click(object sender, EventArgs e)

        {
            mnuForceDistinct.Checked = !mnuForceDistinct.Checked;
            mnuForceDistinct2.Checked = mnuForceDistinct.Checked;

            Settings.Default.ForceDistinct = mnuForceDistinct.Checked;

            ResetMapPens();
        }

        private void MnuShowGridLines_Click(object sender, EventArgs e)

        {
            mnuShowGridLines.Checked = !mnuShowGridLines.Checked;

            Settings.Default.DrawOptions = mnuShowGridLines.Checked
                ? Settings.Default.DrawOptions | DrawOptions.GridLines
                : Settings.Default.DrawOptions & (DrawOptions.DrawAll ^ DrawOptions.GridLines);

            DrawOpts = Settings.Default.DrawOptions;

            mapCon?.Invalidate();
        }

        private void MnuShowListGridLines_Click(object sender, EventArgs e)

        {
            mnuShowListGridLines.Checked = !mnuShowListGridLines.Checked;

            Settings.Default.ShowListGridLines = mnuShowListGridLines.Checked;

            SpawnList.listView.GridLines = mnuShowListGridLines.Checked;

            SpawnTimerList.listView.GridLines = mnuShowListGridLines.Checked;

            GroundItemList.listView.GridLines = mnuShowListGridLines.Checked;
        }

        private void MnuGridInterval_Click(object sender, EventArgs e)

        {
            mnuGridInterval100.Checked = sender.Equals(mnuGridInterval100);

            mnuGridInterval250.Checked = sender.Equals(mnuGridInterval250);

            mnuGridInterval500.Checked = sender.Equals(mnuGridInterval500);

            mnuGridInterval1000.Checked = sender.Equals(mnuGridInterval1000);

            Settings.Default.GridInterval = GridInterval();

            mapCon?.Invalidate();
        }

        private void MnuGridColor_Click(object sender, EventArgs e)

        {
            if (colorPicker.ShowDialog() != DialogResult.Cancel && Settings.Default.GridColor != colorPicker.Color)

            {
                Settings.Default.GridColor = colorPicker.Color;

                mapCon?.Invalidate();
            }
        }

        private void MnuBackgroundColor_Click(object sender, EventArgs e)

        {
            if (colorPicker.ShowDialog() != DialogResult.Cancel && colorPicker.Color != Settings.Default.BackColor)

            {
                Settings.Default.BackColor = colorPicker.Color;

                ResetMapPens();
            }
        }

        private void SetFollowOption(FollowOption NewFollowOption)

        {
            Settings.Default.FollowOption = NewFollowOption;

            if (NewFollowOption == FollowOption.None)
            {
                toolStripCoPStatus.Text = "NoF";
                mnuFollowNone.Image = Resources.BlackX;
                mnuFollowNone2.Image = Resources.BlackX;
            }
            else
            {
                mnuFollowNone.Image = null;
                mnuFollowNone2.Image = null;
            }
            if (NewFollowOption == FollowOption.Player)
            {
                toolStripCoPStatus.Text = "CoP";
                mapPane.offsetx.Value = 0;
                mapPane.offsety.Value = 0;

                mnuFollowPlayer.Image = Resources.BlackX;
                mnuFollowPlayer2.Image = Resources.BlackX;
            }
            else
            {
                mnuFollowPlayer.Image = null;
                mnuFollowPlayer2.Image = null;
            }
            if (NewFollowOption == FollowOption.Target)
            {
                toolStripCoPStatus.Text = "CoT";
                mapPane.offsetx.Value = 0;
                mapPane.offsety.Value = 0;

                mnuFollowTarget.Image = Resources.BlackX;
                mnuFollowTarget2.Image = Resources.BlackX;
            }
            else
            {
                mnuFollowTarget.Image = null;
                mnuFollowTarget2.Image = null;
            }
        }

        private void MnuFollowNone_Click(object sender, EventArgs e) => SetFollowOption(FollowOption.None);

        private void MnuFollowPlayer_Click(object sender, EventArgs e) => SetFollowOption(FollowOption.Player);

        private void MnuFollowTarget_Click(object sender, EventArgs e) => SetFollowOption(FollowOption.Target);

        private void ToolStripCoPStatus_Click(object sender, EventArgs e)
        {
            if (Settings.Default.FollowOption == FollowOption.None)
            {
                SetFollowOption(FollowOption.Player);
            }
            else if (Settings.Default.FollowOption == FollowOption.Player)
            {
                SetFollowOption(FollowOption.Target);
            }
            else if (Settings.Default.FollowOption == FollowOption.Target)
            {
                SetFollowOption(FollowOption.None);
            }
        }
        private void MnuReloadAlerts_Click(object sender, EventArgs e)
        {
            if (bIsRunning)
            {
                filters.ClearArrays();

                filters.LoadAlerts(curZone);

                timDelayAlerts.Start();

                DisablePlayAlerts();

                eq.mobsTimers.ResetTimers();
                map?.ClearMap();

                eq.mobsTimers.LoadTimers();
            }
        }

        private void MnuAddEditAlerts_Click(object sender, EventArgs e) => filters.EditAlertFile(curZone);

        private void MnuSpawnListFont_Click(object sender, EventArgs e)

        {
            fontDialog1.Font = SpawnList.listView.Font;

            fontDialog1.ShowApply = true;

            if(fontDialog1.ShowDialog() != DialogResult.Cancel)

            {
                SpawnList.listView.Font = fontDialog1.Font;

                SpawnTimerList.listView.Font = fontDialog1.Font;
                ReloadAlertFiles();
            }
        }

        private void MnuCollectMobTrails_Click(object sender, EventArgs e)

        {
            mnuCollectMobTrails.Checked = !mnuCollectMobTrails.Checked;

            Settings.Default.CollectMobTrails = mnuCollectMobTrails.Checked;

            mapCon?.Invalidate();
        }

        private void MnuShowSpawnList_Click(object sender, EventArgs e)

        {
            mnuShowSpawnList.Checked = !mnuShowSpawnList.Checked;

            Settings.Default.ShowMobList = mnuShowSpawnList.Checked;

            if (Settings.Default.ShowMobList)
            {
                SpawnList.Show(dockPanel);
            }
            else
            {
                SpawnList.Hide();
            }
        }

        private void MnuShowSpawnListTimer_Click(object sender, EventArgs e)

        {
            mnuShowSpawnListTimer.Checked = !mnuShowSpawnListTimer.Checked;

            Settings.Default.ShowMobListTimer = mnuShowSpawnListTimer.Checked;

            if (Settings.Default.ShowMobListTimer)
            {
                SpawnTimerList.Show(dockPanel);
            }
            else
            {
                SpawnTimerList.Hide();
            }
        }

        private void MnuShowGroundItemList_Click(object sender, EventArgs e)
        {
            mnuShowGroundItemList.Checked = !mnuShowGroundItemList.Checked;

            Settings.Default.ShowGroundItemList = mnuShowGroundItemList.Checked;

            if (Settings.Default.ShowGroundItemList)
            {
                GroundItemList.Show(dockPanel);
            }
            else
            {
                GroundItemList.Hide();
            }
        }

        private void MnuShowMobTrails_Click(object sender, EventArgs e)

        {
            mnuShowMobTrails.Checked = !mnuShowMobTrails.Checked;

            Settings.Default.DrawOptions = mnuShowMobTrails.Checked
                ? Settings.Default.DrawOptions | DrawOptions.SpawnTrails
                : Settings.Default.DrawOptions & (DrawOptions.DrawAll ^ DrawOptions.SpawnTrails);

            mapCon?.Invalidate();
        }

        private void MnuAbout_Click(object sender, EventArgs e)

        {
            AboutDialog ab = new AboutDialog();
            TopMost = false;
            ab.ShowDialog();
            TopMost = mnuAlwaysOnTop.Checked;
        }

        private void MnuShowTargetInfo_Click(object sender, EventArgs e)

        {
            Settings.Default.ShowTargetInfo = !Settings.Default.ShowTargetInfo;

            mnuShowTargetInfo.Checked = Settings.Default.ShowTargetInfo;
            mnuShowTargetInfo2.Checked = Settings.Default.ShowTargetInfo;

            mapCon?.Invalidate();
        }

        private void MnuListColor_Click(object sender, EventArgs e)

        {
            if(colorPicker.ShowDialog() != DialogResult.Cancel)

            {
                Settings.Default.ListBackColor = colorPicker.Color;

                SpawnList.listView.BackColor = Settings.Default.ListBackColor;

                SpawnTimerList.listView.BackColor = Settings.Default.ListBackColor;

                GroundItemList.listView.BackColor = Settings.Default.ListBackColor;
            }
        }

        private void MnuAutoSelectEQTarget_Click(object sender, EventArgs e)

        {
            Settings.Default.AutoSelectEQTarget = !Settings.Default.AutoSelectEQTarget;

            mnuAutoSelectEQTarget.Checked = Settings.Default.AutoSelectEQTarget;
            mnuAutoSelectEQTarget2.Checked = Settings.Default.AutoSelectEQTarget;
        }

        private void MnuGlobalAlerts_Click(object sender, EventArgs e) => filters.EditAlertFile("global");

        private void MnuShowNPCs_Click(object sender, EventArgs e)

        {
            mnuShowNPCs.Checked = !mnuShowNPCs.Checked;

            Settings.Default.ShowNPCs = mnuShowNPCs.Checked;

            comm.UpdateHidden();
        }

        private void MnuShowLookupText_Click(object sender, EventArgs e)

        {
            mnuShowLookupText.Checked = !mnuShowLookupText.Checked;

            Settings.Default.ShowLookupText = mnuShowLookupText.Checked;

            comm.UpdateHidden();
        }

        private void MnuAlwaysOnTop_Click(object sender, EventArgs e)

        {
            mnuAlwaysOnTop.Checked = !mnuAlwaysOnTop.Checked;

            Settings.Default.AlwaysOnTop = mnuAlwaysOnTop.Checked;

            if (mnuAlwaysOnTop.Checked)
            {
                TopMost = true;
                TopLevel = true;
            } else
            {
                TopMost = false;
            }
        }

        private void MnuShowLookupNumber_Click(object sender, EventArgs e)

        {
            mnuShowLookupNumber.Checked = !mnuShowLookupNumber.Checked;

            Settings.Default.ShowLookupNumber = mnuShowLookupNumber.Checked;

            comm.UpdateHidden();
        }

        private void MnuShowCorpses_Click(object sender, EventArgs e)

        {
            mnuShowCorpses.Checked = !mnuShowCorpses.Checked;

            Settings.Default.ShowCorpses = mnuShowCorpses.Checked;

            comm.UpdateHidden();
        }

        private void MnuShowPlayers_Click(object sender, EventArgs e)

        {
            mnuShowPlayers.Checked = !mnuShowPlayers.Checked;

            Settings.Default.ShowPlayers = mnuShowPlayers.Checked;

            comm.UpdateHidden();
        }

        private void MnuShowInvis_Click(object sender, EventArgs e)

        {
            mnuShowInvis.Checked = !mnuShowInvis.Checked;

            Settings.Default.ShowInvis = mnuShowInvis.Checked;

            comm.UpdateHidden();
        }

        private void MnuShowMounts_Click(object sender, EventArgs e)

        {
            mnuShowMounts.Checked = !mnuShowMounts.Checked;

            Settings.Default.ShowMounts = mnuShowMounts.Checked;

            comm.UpdateHidden();
        }

        private void MnuShowFamiliars_Click(object sender, EventArgs e)

        {
            mnuShowFamiliars.Checked = !mnuShowFamiliars.Checked;

            Settings.Default.ShowFamiliars = mnuShowFamiliars.Checked;

            comm.UpdateHidden();
        }

        private void MnuShowPets_Click(object sender, EventArgs e)

        {
            mnuShowPets.Checked = !mnuShowPets.Checked;

            Settings.Default.ShowPets = mnuShowPets.Checked;

            comm.UpdateHidden();
        }

        private void MnuTargetInfoFont_Click(object sender, EventArgs e)

        {
            fontDialog1.Font = mapCon.lblMobInfo.Font;

            fontDialog1.ShowApply = true;

            if(fontDialog1.ShowDialog() != DialogResult.Cancel)

            {
                mapCon.lblMobInfo.Font = fontDialog1.Font;

                mapCon.lblGameClock.Font = new Font(fontDialog1.Font.FontFamily.Name,fontDialog1.Font.Size, FontStyle.Bold);

                Settings.Default.TargetInfoFont = fontDialog1.Font;
            }
        }

        private void MnuShowSpawnPoints_Click(object sender, EventArgs e)

        {
            if (sender.Equals(mnuShowSpawnPoints))
            {
                mnuShowSpawnPoints.Checked = !mnuShowSpawnPoints.Checked;
                mnuShowSpawnPoints2.Checked = mnuShowSpawnPoints.Checked;
            }
            else
            {
                mnuShowSpawnPoints2.Checked = !mnuShowSpawnPoints2.Checked;
                mnuShowSpawnPoints.Checked = mnuShowSpawnPoints2.Checked;
            }

            Settings.Default.DrawOptions = mnuShowSpawnPoints.Checked
                ? Settings.Default.DrawOptions | DrawOptions.SpawnTimers
                : Settings.Default.DrawOptions & (DrawOptions.DrawAll ^ DrawOptions.SpawnTimers);
        }

        private void MnuIPAddress1_Click(object sender, EventArgs e)

        {
            ResetMenu(1);

            Restart();
        }

        private void MnuIPAddress2_Click(object sender, EventArgs e)

        {
            ResetMenu(2);

            Restart();
        }

        private void MnuIPAddress3_Click(object sender, EventArgs e)

        {
            ResetMenu(3);

            Restart();
        }

        private void MnuIPAddress4_Click(object sender, EventArgs e)

        {
            ResetMenu(4);

            Restart();
        }

        private void MnuIPAddress5_Click(object sender, EventArgs e)

        {
            ResetMenu(5);

            Restart();
        }

        private void ResetMenu(int isCheck)

        {
            mnuIPAddress1.Checked = false;

            mnuIPAddress2.Checked = false;

            mnuIPAddress3.Checked = false;

            mnuIPAddress4.Checked = false;

            mnuIPAddress5.Checked = false;

            Settings.Default.CurrentIPAddress = isCheck;

            switch (isCheck)

            {
                case 1: {mnuIPAddress1.Checked = true; currentIPAddress = Settings.Default.IPAddress1; break;}

                case 2: {mnuIPAddress2.Checked = true; currentIPAddress = Settings.Default.IPAddress2; break;}

                case 3: {mnuIPAddress3.Checked = true; currentIPAddress = Settings.Default.IPAddress3; break;}

                case 4: {mnuIPAddress4.Checked = true; currentIPAddress = Settings.Default.IPAddress4; break;}

                case 5: {mnuIPAddress5.Checked = true; currentIPAddress = Settings.Default.IPAddress5; break;}
            }
        }

        private void Restart()
        {
            comm.StopListening();

            if (bIsRunning)
            {
                StopListening();
            }

            StartListening();
        }

        private void MnuCharRefresh_Click(object sender, EventArgs e)

        {
            colProcesses.Clear();
            VisChar();
        }

        private void SwitchCharacter(int CharacterIndex)

        {
            if (colProcesses.Count>=CharacterIndex)

            {
                ProcessInfo PI = (ProcessInfo) colProcesses[CharacterIndex-1];

                comm.SwitchCharacter(PI);
            }
        }

        private void MnuChar1_Click(object sender, EventArgs e)
        {
            if (!mnuChar1.Checked && comm.CanSwitchChars())
            {
                SwitchCharacter(1);
            }
        }

        private void MnuChar2_Click(object sender, EventArgs e)
        {
            if (!mnuChar2.Checked && comm.CanSwitchChars())
            {
                SwitchCharacter(2);
            }
        }

        private void MnuChar3_Click(object sender, EventArgs e) => SwitchCharacter(3);

        private void MnuChar4_Click(object sender, EventArgs e) => SwitchCharacter(4);

        private void MnuChar5_Click(object sender, EventArgs e) => SwitchCharacter(5);

        private void MnuChar6_Click(object sender, EventArgs e) => SwitchCharacter(6);

        private void MnuChar7_Click(object sender, EventArgs e) => SwitchCharacter(7);

        private void MnuChar8_Click(object sender, EventArgs e) => SwitchCharacter(8);

        private void MnuChar9_Click(object sender, EventArgs e) => SwitchCharacter(9);

        private void MnuChar10_Click(object sender, EventArgs e) => SwitchCharacter(10);

        private void MnuChar11_Click(object sender, EventArgs e) => SwitchCharacter(11);

        private void MnuChar12_Click(object sender, EventArgs e) => SwitchCharacter(12);

        private void MnuKeepCentered_Click(object sender, EventArgs e)
        {
            Settings.Default.KeepCentered = !Settings.Default.KeepCentered;

            mnuKeepCentered.Checked = mnuKeepCentered2.Checked = Settings.Default.KeepCentered;
        }

        public void ReAdjust() => mapCon?.ReAdjust();

        public void ReloadAlertFiles()

        {
            filters.ClearArrays();

            filters.LoadAlerts(curZone);

            timDelayAlerts.Start();

            DisablePlayAlerts();

            eq.mobsTimers.ResetTimers();

            map.ClearMap();

            eq.mobsTimers.LoadTimers();
        }

        public void ResetMapPens()
        {
            eq.CalculateMapLinePens();
                mapCon?.Invalidate();
        }

        public void ProcessPacket(SPAWNINFO si, bool update_hidden)

        {
            // SPAWN  // si.flags == 0  

            // Target // si.flags == 1

            //  MAP   // si.flags == 4                      

            // GROUND // si.flags == 5  

            //ProcInfo// si.flags == 6

            //World//    si.flags == 8

            // PLAYER // si.flags == 253                

            switch (si.flags)

            {
                case SPAWNINFO.PacketType.Zone:

                    ProcessMap(si);

                    break;

                case SPAWNINFO.PacketType.Player:

                    eq.ProcessGamer(si, this);

                    break;

                case SPAWNINFO.PacketType.GroundItem:

                    eq.ProcessGroundItems(si,filters);//,GroundItemList);

                    break;

                case SPAWNINFO.PacketType.Target:

                    eq.ProcessTarget(si);

                    break;

                case SPAWNINFO.PacketType.World:

                    eq.ProcessWorld(si);

                    break;

                case SPAWNINFO.PacketType.Spawn:

                    eq.ProcessSpawns(si, this, SpawnList, filters, mapPane, update_hidden);

                    break;

                case SPAWNINFO.PacketType.GetProcessInfo:

                    ProcessProcessInfo(si);
                    break;

                default:

                    Text = "Unknown Packet Type: " + si.flags.ToString();

                    break;
            }
        }

        private void MnuMapLabelsFont_Click(object sender, EventArgs e)

        {
            fontDialog1.Font = mapCon.drawFont;

            fontDialog1.ShowApply = true;

            if(fontDialog1.ShowDialog() != DialogResult.Cancel)

            {
                mapCon.drawFont = fontDialog1.Font;

                mapCon.drawFont1 = new Font(Settings.Default.MapLabel.Name, Settings.Default.MapLabel.Size * 0.9f, Settings.Default.MapLabel.Style);
                mapCon.drawFont3 = new Font(Settings.Default.MapLabel.Name, Settings.Default.MapLabel.Size * 1.1f, Settings.Default.MapLabel.Style);

                //map.ClearMap();      

            }
        }

        private void MnuClearSavedTimers_Click(object sender, EventArgs e)

        {
            // Clear Saved Spawn Timers

            eq.mobsTimers.ClearSavedTimers();

            SpawnTimerList.listView.BeginUpdate();

            SpawnTimerList.listView.Items.Clear();

            SpawnTimerList.listView.EndUpdate();

            eq.SpawnX = -1.0f;

            eq.SpawnY = -1.0f;

            map.ClearMap();
        }

        private void MnuGridLabelColor_Click(object sender, EventArgs e)

        {
            if (colorPicker.ShowDialog() != DialogResult.Cancel)

            {
                Settings.Default.GridLabelColor = colorPicker.Color;

                mapCon?.Invalidate();
            }
        }

        public void AddMapText(string textToAdd)
        {
            FrmAddMapText mapBox = new FrmAddMapText
            {
                txtColr = Settings.Default.SelectedAddMapText
            };
            string new_text = textToAdd.Replace("#","");

            if (addTextFormLocation.X != 0 && addTextFormLocation.Y != 0)
            {
                mapBox.StartPosition = FormStartPosition.CenterParent;
                mapBox.Location = addTextFormLocation;
            }

            mapBox.txtAdd = new_text.Length > 0 ? new_text : "Enter Text Label";
            mapBox.txtBkg = Settings.Default.BackColor;
            if (mapnameWithLabels.Length > 4  && (mapnameWithLabels.EndsWith(".txt") || mapnameWithLabels.EndsWith(".map")))
            {
                int lastSlashIndex = mapnameWithLabels.LastIndexOf("\\");
                if (lastSlashIndex > 0)
                {
                    mapBox.mapName = "Add to Map: ";
                    mapBox.mapName += mapnameWithLabels.Substring(lastSlashIndex + 1);
                }
                else
                {
                    return;
                }
            }
            else
            {
                // we dont have a good map name
                return;
            }

            addTextFormLocation = mapBox.Location;
            if (mapBox.ShowDialog() == DialogResult.OK)
            {
                // save selected color

                // we have a valid addition of text
                new_text = mapBox.txtAdd.TrimEnd('_', ' ');

                // add it to map now
                if (new_text.Length > 0)
                {
                    if(mapnameWithLabels.EndsWith(".txt")) {
                        MapText work = new MapText
                        {
                            text = new_text,
                            x = (int)alertX,
                            y = (int)alertY,
                            z = (int)alertZ,
                            size = mapBox.txtSize,
                            color = new SolidBrush(mapBox.txtColr)
                        };
                        work.draw_color = eq.GetDistinctColor(work.color);
                        eq.AddMapText(work);

                        // string to append to map file
                        new_text = new_text.Replace(" ", "_");
                        string soe_maptext = $"P {alertX * -1:f4}, {alertY * -1:f4}, {alertZ:f4}," +
                            $"{mapBox.txtColr.R}, {mapBox.txtColr.G}, {mapBox.txtColr.B}, {mapBox.txtSize}, {new_text}\n";

                        if (DialogResult.Yes == MessageBox.Show($"Do you want to write the label to {mapBox.mapName}?" +
                            $"{Environment.NewLine}{Environment.NewLine}{soe_maptext}", "Write label to map file?",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                        {
                            try
                            {
                                File.AppendAllText(mapnameWithLabels, soe_maptext);
                            }
                            catch (ArgumentNullException accVexc)
                            {
                                MessageBox.Show($"Access Violation {accVexc}", "Error");
                            }
                        }
                        else
                        {
                            eq.DeleteMapText(work);
                        }
                    }
                    else if (mapnameWithLabels.EndsWith(".map"))
                    {
                        // string to append to .map file
                        var seq_maptext = $"P,{new_text},{mapBox.txtColr.Name},{alertX:f0},{alertY:f0},{alertZ:f0}\n";

                        MapText work = new MapText
                        {
                            text = new_text,
                            x = (int)alertX,
                            y = (int)alertY,
                            z = (int)alertZ,
                            color = new SolidBrush(mapBox.txtColr)
                        };
                        work.draw_color = eq.GetDistinctColor(work.color);
                        eq.AddMapText(work);

                        if (DialogResult.Yes == MessageBox.Show($"Do you want to write the label to {mapBox.mapName}?" +
                            $"{Environment.NewLine}{Environment.NewLine}{seq_maptext}", "Write label to map file?",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                        {
                            File.AppendAllText(mapnameWithLabels, seq_maptext);
                        }
                        else
                        {
                            eq.DeleteMapText(work);
                        }
                    }
                }
            }
        }

        public bool DialogBox(string titleText, string labelText, string dialogText)

        {
            frmDialogBox dlgBox = new frmDialogBox
            {
                dlgTitle = titleText,

                dlgLabel = labelText,

                dlgTextBox = dialogText,

                TopMost = true
            };

            if (dlgBox.ShowDialog() == DialogResult.OK && dlgBox.dlgTextBox.Length > 0)

            {
                SpawnList.mobname = dlgBox.dlgTextBox;

                SpawnTimerList.mobname = dlgBox.dlgTextBox;

                return true;
            }
            else
            {
                return false;
            }
        }

        private void MnuShowZoneText_Click(object sender, EventArgs e)

        {
            if (sender.Equals(mnuShowZoneText))
            {
                mnuShowZoneText.Checked = !mnuShowZoneText.Checked;
                mnuShowZoneText2.Checked = mnuShowZoneText.Checked;
            }
            else
            {
                mnuShowZoneText2.Checked = !mnuShowZoneText2.Checked;
                mnuShowZoneText.Checked = mnuShowZoneText2.Checked;
            }

            Settings.Default.DrawOptions = mnuShowZoneText.Checked
                ? Settings.Default.DrawOptions | DrawOptions.ZoneText
                : Settings.Default.DrawOptions & (DrawOptions.DrawAll ^ DrawOptions.ZoneText);

            DrawOpts = Settings.Default.DrawOptions;

            mapCon?.Invalidate();
        }

        private void MnuMapReset_Click(object sender, EventArgs e) => mapCon.MapReset();

        private void MnuShowLayer1_Click(object sender, EventArgs e)

        {
            if (sender.Equals(mnuShowLayer1))
            {
                mnuShowLayer1.Checked = !mnuShowLayer1.Checked;
                mnuShowLayer21.Checked = mnuShowLayer1.Checked;
            }
            else
            {
                mnuShowLayer21.Checked = !mnuShowLayer21.Checked;
                mnuShowLayer1.Checked = mnuShowLayer21.Checked;
            }

            Settings.Default.ShowLayer1 = mnuShowLayer1.Checked;
        }

        private void MnuShowLayer2_Click(object sender, EventArgs e)

        {
            if (sender.Equals(mnuShowLayer2))
            {
                mnuShowLayer2.Checked = !mnuShowLayer2.Checked;
                mnuShowLayer22.Checked = mnuShowLayer2.Checked;
            }
            else
            {
                mnuShowLayer22.Checked = !mnuShowLayer22.Checked;
                mnuShowLayer2.Checked = mnuShowLayer22.Checked;
            }

            Settings.Default.ShowLayer2 = mnuShowLayer2.Checked;

        }

        private void MnuShowLayer3_Click(object sender, EventArgs e)

        {
            if (sender.Equals(mnuShowLayer3))
            {
                mnuShowLayer3.Checked = !mnuShowLayer3.Checked;
                mnuShowLayer23.Checked = mnuShowLayer3.Checked;
            }
            else
            {
                mnuShowLayer23.Checked = !mnuShowLayer23.Checked;
                mnuShowLayer3.Checked = mnuShowLayer23.Checked;
            }

            Settings.Default.ShowLayer3 = mnuShowLayer3.Checked;
        }

        private void MnuSodTitanium_Click(object sender, EventArgs e)

        {
            mnuConSoD.Checked = true;

            mnuConSoF.Checked = false;

            mnuConDefault.Checked = false;

            Settings.Default.SoDCon = true;

            Settings.Default.SoFCon = false;

            Settings.Default.DefaultCon = false;

            eq.FillConColors(this);

            eq.UpdateMobListColors();
        }

        private void MnuConDefault_Click(object sender, EventArgs e)
        {
            mnuConSoD.Checked = false;

            mnuConSoF.Checked = false;

            mnuConDefault.Checked = true;

            Settings.Default.SoDCon = false;

            Settings.Default.SoFCon = false;

            Settings.Default.DefaultCon = true;

            eq.FillConColors(this);

            eq.UpdateMobListColors();
        }

        private void MnuConSoF_Click(object sender, EventArgs e)

        {
            mnuConSoD.Checked = false;

            mnuConSoF.Checked = true;

            mnuConDefault.Checked = false;

            Settings.Default.SoDCon = false;

            Settings.Default.SoFCon = true;

            Settings.Default.DefaultCon = false;

            eq.FillConColors(this);

            eq.UpdateMobListColors();
        }

        private void MnuShowPCNames_Click(object sender, EventArgs e)

        {
            Settings.Default.ShowPCNames = !Settings.Default.ShowPCNames;

            mnuShowPCNames.Checked = Settings.Default.ShowPCNames;
            mnuShowPCNames2.Checked = Settings.Default.ShowPCNames;
        }

        private void MnuShowNPCNames_Click(object sender, EventArgs e)

        {
            Settings.Default.ShowNPCNames = !Settings.Default.ShowNPCNames;

            mnuShowNPCNames.Checked = Settings.Default.ShowNPCNames;
            mnuShowNPCNames2.Checked = Settings.Default.ShowNPCNames;
        }

        private void MnuShowNPCCorpseNames_Click(object sender, EventArgs e)

        {
            Settings.Default.ShowNPCCorpseNames = !Settings.Default.ShowNPCCorpseNames;

            mnuShowNPCCorpseNames.Checked = Settings.Default.ShowNPCCorpseNames;
            mnuShowNPCCorpseNames2.Checked = Settings.Default.ShowNPCCorpseNames;
        }

        private void MnuShowPlayerCorpseNames_Click(object sender, EventArgs e)

        {
            Settings.Default.ShowPlayerCorpseNames = !Settings.Default.ShowPlayerCorpseNames;

            mnuShowPlayerCorpseNames.Checked = Settings.Default.ShowPlayerCorpseNames;
            mnuShowPlayerCorpseNames2.Checked = Settings.Default.ShowPlayerCorpseNames;
        }

        //private void MnuShowPCGuild_Click(object sender, EventArgs e)

        //{
        //    Settings.Default.ShowPCGuild = !Settings.Default.ShowPCGuild;

        //    mnuShowPCGuild.Checked = Settings.Default.ShowPCGuild;
        //    mnuShowPCGuild2.Checked = Settings.Default.ShowPCGuild;
        //}

        private void MnuFilterMapLines_Click(object sender, EventArgs e)

        {
            Settings.Default.FilterMapLines = !Settings.Default.FilterMapLines;

            mnuFilterMapLines.Checked = Settings.Default.FilterMapLines;
            mnuFilterMapLines2.Checked = Settings.Default.FilterMapLines;
        }

        private void MnuFilterMapText_Click(object sender, EventArgs e)
        {
            Settings.Default.FilterMapText = !Settings.Default.FilterMapText;

            mnuFilterMapText.Checked = Settings.Default.FilterMapText;
            mnuFilterMapText2.Checked = Settings.Default.FilterMapText;
        }

        private void MnuFilterNPCs_Click(object sender, EventArgs e)

        {
            Settings.Default.FilterNPCs = !Settings.Default.FilterNPCs;

            mnuFilterNPCs.Checked = Settings.Default.FilterNPCs;
            mnuFilterNPCs2.Checked = Settings.Default.FilterNPCs;
        }

        private void MnuFilterPlayers_Click(object sender, EventArgs e)

        {
            Settings.Default.FilterPlayers = !Settings.Default.FilterPlayers;

            mnuFilterPlayers.Checked = Settings.Default.FilterPlayers;
            mnuFilterPlayers2.Checked = Settings.Default.FilterPlayers;
        }

        private void MnuFilterSpawnPoints_Click(object sender, EventArgs e)

        {
            Settings.Default.FilterSpawnPoints = !Settings.Default.FilterSpawnPoints;

            mnuFilterSpawnPoints.Checked = Settings.Default.FilterSpawnPoints;
            mnuFilterSpawnPoints2.Checked = Settings.Default.FilterSpawnPoints;
        }

        private void MnuFilterPlayerCorpses_Click(object sender, EventArgs e)

        {
            Settings.Default.FilterPlayerCorpses = !Settings.Default.FilterPlayerCorpses;

            mnuFilterPlayerCorpses.Checked = Settings.Default.FilterPlayerCorpses;
            mnuFilterPlayerCorpses2.Checked = Settings.Default.FilterPlayerCorpses;
        }

        private void MnuFilterGroundItems_Click(object sender, EventArgs e)
        {
            Settings.Default.FilterGroundItems = !Settings.Default.FilterGroundItems;

            mnuFilterGroundItems.Checked = Settings.Default.FilterGroundItems;
            mnuFilterGroundItems2.Checked = Settings.Default.FilterGroundItems;
        }

        private void MnuFilterNPCCorpses_Click(object sender, EventArgs e)

        {
            Settings.Default.FilterNPCCorpses = !Settings.Default.FilterNPCCorpses;

            mnuFilterNPCCorpses.Checked = Settings.Default.FilterNPCCorpses;
            mnuFilterNPCCorpses2.Checked = Settings.Default.FilterNPCCorpses;
        }

        private void MnuShowPVP_Click(object sender, EventArgs e)

        {
            Settings.Default.ShowPVP = !Settings.Default.ShowPVP;

            mnuShowPVP.Checked = Settings.Default.ShowPVP;
            mnuShowPVP2.Checked = Settings.Default.ShowPVP;
        }

        private void MnuShowPVPLevel_Click(object sender, EventArgs e)

        {
            Settings.Default.ShowPVPLevel = !Settings.Default.ShowPVPLevel;

            mnuShowPVPLevel.Checked = Settings.Default.ShowPVPLevel;
            mnuShowPVPLevel2.Checked = Settings.Default.ShowPVPLevel;
        }

        private void MnuShowNPCLevels_Click(object sender, EventArgs e)

        {
            Settings.Default.ShowNPCLevels = !Settings.Default.ShowNPCLevels;

            mnuShowNPCLevels.Checked = Settings.Default.ShowNPCLevels;
            mnuShowNPCLevels2.Checked = Settings.Default.ShowNPCLevels;
        }

        private void MnuAutoExpand_Click(object sender, EventArgs e)

        {
            Settings.Default.AutoExpand = !Settings.Default.AutoExpand;

            mnuAutoExpand.Checked = Settings.Default.AutoExpand;
            mnuAutoExpand2.Checked = Settings.Default.AutoExpand;
        }

        private void MnuSaveSpawnLog_Click(object sender, EventArgs e)

        {
            mnuSaveSpawnLog.Checked = !mnuSaveSpawnLog.Checked;

            Settings.Default.SaveSpawnLogs = mnuSaveSpawnLog.Checked;
        }

        private void MnuSpawnCountdown_Click(object sender, EventArgs e)

        {
            Settings.Default.SpawnCountdown = !Settings.Default.SpawnCountdown;

            mnuSpawnCountdown.Checked = Settings.Default.SpawnCountdown;
            mnuSpawnCountdown2.Checked = Settings.Default.SpawnCountdown;
        }

        private void MnuShowPCCorpses_Click(object sender, EventArgs e)

        {
            mnuShowPCCorpses.Checked = !mnuShowPCCorpses.Checked;

            Settings.Default.ShowPCCorpses = mnuShowPCCorpses.Checked;

            comm.UpdateHidden();
        }

        private void MnuShowMyCorpse_Click(object sender, EventArgs e)

        {
            mnuShowMyCorpse.Checked = !mnuShowMyCorpse.Checked;

            Settings.Default.ShowMyCorpse = mnuShowMyCorpse.Checked;

            comm.UpdateHidden();
        }

        private void MnuForceDistinctText_Click(object sender, EventArgs e)
        {
            Settings.Default.ForceDistinctText = !Settings.Default.ForceDistinctText;

            mnuForceDistinctText.Checked = Settings.Default.ForceDistinctText;
            mnuForceDistinctText2.Checked = Settings.Default.ForceDistinctText;

            ResetMapPens();
        }
        #region filters

        private void MnuAddHuntFilter_Click(object sender, EventArgs e)
        {
            if (DialogBox("Add to Zone Hunt Filters", "Add name to Hunt list:", alertAddmobname))
            {
                filters.AddToAlerts(filters.Hunt, alertAddmobname);

                filters.WriteAlertFile(curZone);

                ReloadAlertFiles();
            }
        }

        private void MnuAddCautionFilter_Click(object sender, EventArgs e)
        {
            if (DialogBox("Add to Zone Caution Filters", "Add name to Caution list:", alertAddmobname))
            {
                filters.AddToAlerts(filters.Caution, alertAddmobname);

                filters.WriteAlertFile(curZone);

                ReloadAlertFiles();
            }
        }

        private void MnuAddDangerFilter_Click(object sender, EventArgs e)
        {
            if (DialogBox("Add to Zone Danger Alert Filters", "Add name to Danger list:", alertAddmobname))
            {
                filters.AddToAlerts(filters.Danger, alertAddmobname);

                filters.WriteAlertFile(curZone);

                ReloadAlertFiles();
            }
        }

        private void MnuAddAlertFilter_Click(object sender, EventArgs e)
        {
            if (DialogBox("Add to Zone Rare Alert Filters", "Add name to Rare list:", alertAddmobname))
            {
                filters.AddToAlerts(filters.Alert, alertAddmobname);

                filters.WriteAlertFile(curZone);

                ReloadAlertFiles();
            }
        }
#endregion
        private void MnuSearchAllakhazam_Click(object sender, EventArgs e)
        {
            var searchname = RegexHelper.SearchName(alertAddmobname);

            if (searchname.Length > 0)
            {
                var searchURL = string.Format(Settings.Default.SearchString, searchname);

                Process.Start(searchURL);
            }
        }

        private void MnuShowMenuBar_Click(object sender, EventArgs e)
        {
            Settings.Default.ShowMenuBar = !Settings.Default.ShowMenuBar;

            mnuShowMenuBar.Checked = Settings.Default.ShowMenuBar;
            mnuViewMenuBar.Checked = Settings.Default.ShowMenuBar;

            if (Settings.Default.ShowMenuBar)
            {
                mnuMainMenu.Show();
            }
            else
            {
                mnuMainMenu.Hide();
            }
        }

        private void MnuViewStatusBar_Click(object sender, EventArgs e)
        {
            Settings.Default.ShowStatusBar = !Settings.Default.ShowStatusBar;

            mnuViewStatusBar.Checked = Settings.Default.ShowStatusBar;
            if (Settings.Default.ShowStatusBar)
            {
                statusBarStrip.Show();
            }
            else
            {
                statusBarStrip.Hide();
            }
        }

        #region depth filter
        private void MnuViewDepthFilterToolBar_Click(object sender, EventArgs e)
        {
            Settings.Default.ShowToolBar = !Settings.Default.ShowToolBar;

            mnuViewDepthFilterBar.Checked = Settings.Default.ShowToolBar;
            if (Settings.Default.ShowToolBar)
            {
                toolBarStrip.Show();
            }
            else
            {
                toolBarStrip.Hide();
            }
        }

        private void ToolStripZPos_TextChanged(object sender, EventArgs e)
        {
            // validate that text is a usable number
            // allow a value of 0 to 3500
            string Str = toolStripZPos.Text.Trim();

            bool validnum = true;
            if (Str.Length > 0)
            {
                bool isNum = decimal.TryParse(Str, out var Num);
                validnum = false;
                if (isNum)
                {
                    validnum = Num >= 0 && Num <= 3500;
                }
                if (Str.Length == 1 && Str == ".")
                {
                    validnum = true;
                }
            }
            if (!validnum)
            {
                toolStripZPos.Text = $"{mapPane.filterzpos.Value}";
                MessageBox.Show("Enter a number between 0 and 3500", "Invalid Z-Pos Value Entered.");
            }
        }

        private void ToolStripZPosUp_Click(object sender, EventArgs e)
        {
            decimal current_val = mapPane.filterzpos.Value;
            current_val += 5;
            if (current_val > mapPane.filterzpos.Maximum)
            {
                current_val = mapPane.filterzpos.Maximum;
            }

            mapPane.filterzpos.Value = current_val;
            toolStripZPos.Text = $"{current_val}";
        }

        private void ToolStripZPosDown_Click(object sender, EventArgs e)
        {
            decimal current_val = mapPane.filterzpos.Value;
            current_val -= 5;
            if (current_val < mapPane.filterzpos.Minimum)
            {
                current_val = mapPane.filterzpos.Minimum;
            }

            mapPane.filterzpos.Value = current_val;
            toolStripZPos.Text = $"{current_val}";
        }

        private void ToolStripZNegDown_Click(object sender, EventArgs e)
        {
            decimal current_val = mapPane.filterzneg.Value;
            current_val -= 5;
            if (current_val < mapPane.filterzneg.Minimum)
            {
                current_val = mapPane.filterzneg.Minimum;
            }

            mapPane.filterzneg.Value = current_val;
            toolStripZNeg.Text = $"{current_val}";
        }

        private void ToolStripZNegUp_Click(object sender, EventArgs e)
        {
            decimal current_val = mapPane.filterzneg.Value;
            current_val += 5;
            if (current_val > mapPane.filterzneg.Maximum)
            {
                current_val = mapPane.filterzneg.Maximum;
            }

            mapPane.filterzneg.Value = current_val;
            toolStripZNeg.Text = $"{current_val}";
        }

        private void ToolStripResetDepthFilter_Click(object sender, EventArgs e)
        {
            mapPane.filterzneg.Value = 75;
            mapPane.filterzpos.Value = 75;
            toolStripZNeg.Text = $"{75}";
            toolStripZPos.Text = $"{75}";
        }

        private void ToolStripZPos_Leave(object sender, EventArgs e)
        {
            // update Z-Pos value
            string Str = toolStripZPos.Text.Trim();
            bool validnum = false;
            if (Str.Length > 0)
            {
                bool isNum = decimal.TryParse(Str, out var Num);
                if (isNum && Num >= 0 && Num <= 3500)
                {
                    mapPane.filterzpos.Value = Num;
                    validnum = true;
                }
            }
            if (!validnum)
            {
                toolStripZPos.Text = $"{mapPane.filterzpos.Value}";
            }
        }

        private void ToolStripZNeg_TextChanged(object sender, EventArgs e)
        {
            // validate that text is a usable number
            // allow a value of 0 to 3500
            string Str = toolStripZNeg.Text.Trim();

            bool validnum = true;
            if (Str.Length > 0)
            {
                bool isNum = decimal.TryParse(Str, out var Num);
                validnum = false;
                if (isNum)
                {
                    validnum = Num >= 0 && Num <= 3500;
                }
                if (Str.Length == 1 && Str == ".")
                {
                    validnum = true;
                }
            }
            if (!validnum)
            {
                toolStripZNeg.Text = $"{mapPane.filterzneg.Value}";
                MessageBox.Show("Enter a number between 0 and 3500", "Invalid Z-Neg Value Entered.");
            }
        }

        private void ToolStripZNeg_Leave(object sender, EventArgs e)
        {
            // update Z-Pos value
            string Str = toolStripZNeg.Text.Trim();
            bool validnum = false;
            if (Str.Length > 0)
            {
                bool isNum = decimal.TryParse(Str, out var Num);
                if (isNum && Num >= 0 && Num <= 3500)
                {
                    mapPane.filterzneg.Value = Num;
                    validnum = true;
                }
            }
            if (!validnum)
            {
                toolStripZNeg.Text = $"{mapPane.filterzneg.Value}";
            }
        }
        private void ToolStripZPos_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                mapCon.Focus();
                e.Handled = true;
            }
        }

        private void ToolStripZNeg_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                mapCon.Focus();
                e.Handled = true;
            }
        }
        #endregion

        private void MnuAddMapLabel_Click(object sender, EventArgs e)
        => AddMapText(alertAddmobname);

        #region zoom
        private void ToolStripZoomIn_Click(object sender, EventArgs e)
        {
            decimal current_val = mapPane.scale.Value;
            if (current_val < 100) {
                current_val += 10;
                if (current_val > 100)
                {
                    current_val = 100;
                }
            } else if (current_val < 200) {
                current_val += 25;
                if (current_val > 200)
                {
                    current_val = 200;
                }
            } else if (current_val < 300) {
                current_val += 25;
                if (current_val > 300)
                {
                    current_val = 300;
                }
            } else if (current_val < 500) {
                current_val += 50;
                if (current_val > 500)
                {
                    current_val = 500;
                }
            } else {
                current_val += 100;
            }

            if (current_val >= mapPane.scale.Minimum && current_val <= mapPane.scale.Maximum)
            {
                mapPane.scale.Value = current_val;
            }
        }

        private void ToolStripZoomOut_Click(object sender, EventArgs e)
        {
            decimal current_val = mapPane.scale.Value;
            if (current_val <= 100)
            {
                current_val -= 10;
                if (current_val < 10)
                {
                    current_val = 10;
                }
            }
            else if (current_val <= 200)
            {
                current_val -= 25;
                if (current_val < 100)
                {
                    current_val = 100;
                }
            }
            else if (current_val <= 300)
            {
                current_val -= 25;
                if (current_val <= 200)
                {
                    current_val = 200;
                }
            }
            else if (current_val <= 400)
            {
                current_val -= 25;
                if (current_val < 300)
                {
                    current_val = 300;
                }
            }
            else if (current_val <= 500)
            {
                current_val -= 25;
                if (current_val < 400)
                {
                    current_val = 400;
                }
            }
            else
            {
                current_val -= 100;
            }

            if (current_val >= mapPane.scale.Minimum && current_val <= mapPane.scale.Maximum)
            {
                mapPane.scale.Value = current_val;
            }
        }
        private void ToolStripScale_TextUpdate(object sender, EventArgs e)
        {
            string Str = toolStripScale.Text.Trim();

            bool validnum = false;

            if (!string.IsNullOrEmpty(Str))
            {
                Str = Str.Replace("%", "");
                bool isNum = decimal.TryParse(Str, out var Num);

                if (isNum)
                {
                    validnum = Num >= 1 && Num <= mapPane.scale.Maximum;
                }
            }

            if (!validnum)
            {
                toolStripScale.Text = $"{mapPane.scale.Value / 100:0%}";
                MessageBox.Show($"1. Enter a number between {mapPane.scale.Minimum} and {mapPane.scale.Maximum}", "Invalid Value Entered.");
            }
        }
        private void ToolStripScale_Leave(object sender, EventArgs e)
        {
            string Str = toolStripScale.Text.Trim();
            bool validnum = false;
            if (!string.IsNullOrEmpty(Str))
            {
                Str = Str.Replace("%", "");
                bool isNum = decimal.TryParse(Str, out var Num);

                if (isNum && Num >= mapPane.scale.Minimum && Num <= mapPane.scale.Maximum)
                {
                    mapPane.scale.Value = Num;
                    validnum = true;
                }
            }

            if (!validnum)
            {
                toolStripScale.Text = $"{mapPane.scale.Value / 100:0%}";
                MessageBox.Show($"2. Enter a number between {mapPane.scale.Minimum} and {mapPane.scale.Maximum}", "Invalid Value Entered.");
            }
            else
            {
                toolStripScale.Text = $"{mapPane.scale.Value / 100:0%}";
            }
        }
        private void ToolStripScale_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                string Str = toolStripScale.Text.Trim();
                Str = Str.Replace("%", "");

                if (Str.Length > 0)
                {
                    bool isNum = decimal.TryParse(Str, out var Num);

                    if (isNum && Num >= mapPane.scale.Minimum && Num <= mapPane.scale.Maximum)
                    {
                        mapPane.scale.Value = Num;
                    }
                }
                toolStripScale.Focus();

                e.Handled = true;
            }
        }
        private void ToolStripScale_DropDownClosed(object sender, EventArgs e)
        {
            string Str = toolStripScale.SelectedItem.ToString();

            if (!string.IsNullOrEmpty(Str))

            {
                Str = Str.Replace("%", "");
                bool isNum = decimal.TryParse(Str, out var Num);

                if (isNum && Num >= mapPane.scale.Minimum && Num <= mapPane.scale.Maximum)
                {
                    mapPane.scale.Value = Num; // lots of unneccessary number conversions removed. (string to double and back to decimal)
                }
            }
        }
        #endregion
        private void SetUpdateSteps()
        {
            int update_steps = (1000 / Settings.Default.UpdateDelay) + 1;
            if (update_steps < 3)
            {
                update_steps = 3;
            }

            int update_ticks = 250 / Settings.Default.UpdateDelay;
            if (update_ticks < 1)
            {
                update_ticks = 1;
            }

            mapCon.UpdateSteps = update_steps;
            mapCon.UpdateTicks = update_ticks;
        }

        private void AddMapTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // add map text, to where the player is currently located
            if (eq.longname.Length > 0)
            {
                if (eq.gamerInfo?.Name.Length > 0)
                {
                    alertX = eq.gamerInfo.X;
                    alertY = eq.gamerInfo.Y;
                    alertZ = eq.gamerInfo.Z;
                    AddMapText("Text to Add");
                }
            }
        }

        private IDockContent GetContentFromPersistString(string persistString)
        {
            if (persistString == "SpawnList")
            {
                return SpawnList;
            }
            else if (persistString == "SpawnTimerList")
            {
                return SpawnTimerList;
            }
            else if (persistString == "GroundSpawnList")
            {
                return GroundItemList;
            }
            else
            {
                return mapPane;
            }
        }

        private void MnuShowListSearchBox_Click(object sender, EventArgs e)
        {
            Settings.Default.ShowListSearchBox = !Settings.Default.ShowListSearchBox;

            mnuShowListSearchBox.Checked = Settings.Default.ShowListSearchBox;

            if (Settings.Default.ShowListSearchBox)
            {
                SpawnList.ShowSearchBox();
                SpawnTimerList.ShowSearchBox();
                GroundItemList.ShowSearchBox();
            }
            else
            {
                SpawnList.HideSearchBox();
                SpawnTimerList.HideSearchBox();
                GroundItemList.HideSearchBox();
            }
        }

        private void MnuSmallTargetInfo_Click(object sender, EventArgs e)
        {
            Settings.Default.SmallTargetInfo = !Settings.Default.SmallTargetInfo;

            mnuSmallTargetInfo.Checked = Settings.Default.SmallTargetInfo;
            mnuSmallTargetInfo2.Checked = Settings.Default.SmallTargetInfo;
        }

        private void MnuAutoConnect_Click(object sender, EventArgs e)
        {
            Settings.Default.AutoConnect = !Settings.Default.AutoConnect;
            mnuAutoConnect.Checked = Settings.Default.AutoConnect;
        }

        #region lookupbox
        private void ToolStripResetLookup_Click(object sender, EventArgs e)
        {
            toolStripLookupBox.Text = "";
            toolStripLookupBox.Focus();
            eq.MarkLookups("0:");
        }
        private void ToolStripResetLookup1_Click(object sender, EventArgs e)
        {
            toolStripLookupBox1.Text = "";
            toolStripLookupBox1.Focus();
            eq.MarkLookups("1:");
        }
        private void ToolStripResetLookup2_Click(object sender, EventArgs e)
        {
            toolStripLookupBox2.Text = "";
            toolStripLookupBox2.Focus();
            eq.MarkLookups("2:");
        }
        private void ToolStripResetLookup3_Click(object sender, EventArgs e)
        {
            toolStripLookupBox3.Text = "";
            toolStripLookupBox3.Focus();
            eq.MarkLookups("3:");
        }
        private void ToolStripResetLookup4_Click(object sender, EventArgs e)
        {
            toolStripLookupBox4.Text = "";
            toolStripLookupBox4.Focus();
            eq.MarkLookups("4:");
        }
        private void ToolStripResetLookup5_Click(object sender, EventArgs e)
        {
            toolStripLookupBox5.Text = "";
            toolStripLookupBox5.Focus();
            eq.MarkLookups("5:");
        }
        private void ToolStripCheckLookup_CheckChanged(object sender, EventArgs e)
        {
            if (toolStripCheckLookup.Checked)
            {
                toolStripCheckLookup.Text = "L";
                bFilter0 = false;
            }
            else
            {
                toolStripCheckLookup.Text = "F";
                bFilter0 = true;
            }
            string new_text = toolStripLookupBox.Text.Replace(" ", "_");
            eq.MarkLookups("0:" + new_text, bFilter0);
        }
        private void ToolStripCheckLookup1_CheckChanged(object sender, EventArgs e)
        {
            if (toolStripCheckLookup1.Checked)
            {
                toolStripCheckLookup1.Text = "L";
                bFilter1 = false;
            }
            else
            {
                toolStripCheckLookup1.Text = "F";
                bFilter1 = true;
            }
            string new_text = toolStripLookupBox1.Text.Replace(" ", "_");
            eq.MarkLookups("1:" + new_text, bFilter1);
        }
        private void ToolStripCheckLookup2_CheckChanged(object sender, EventArgs e)
        {
            if (toolStripCheckLookup2.Checked)
            {
                toolStripCheckLookup2.Text = "L";
                bFilter2 = false;
            }
            else
            {
                toolStripCheckLookup2.Text = "F";
                bFilter2 = true;
            }
            string new_text = toolStripLookupBox2.Text.Replace(" ", "_");
            eq.MarkLookups("2:" + new_text, bFilter2);
        }
        private void ToolStripCheckLookup3_CheckChanged(object sender, EventArgs e)
        {
            if (toolStripCheckLookup3.Checked)
            {
                toolStripCheckLookup3.Text = "L";
                bFilter3 = false;
            }
            else
            {
                toolStripCheckLookup3.Text = "F";
                bFilter3 = true;
            }
            string new_text = toolStripLookupBox3.Text.Replace(" ", "_");
            eq.MarkLookups("3:" + new_text, bFilter3);
        }
        private void ToolStripCheckLookup4_CheckChanged(object sender, EventArgs e)
        {
            if (toolStripCheckLookup4.Checked)
            {
                toolStripCheckLookup4.Text = "L";
                bFilter4 = false;
            }
            else
            {
                toolStripCheckLookup4.Text = "F";
                bFilter4 = true;
            }
            string new_text = toolStripLookupBox4.Text.Replace(" ", "_");
            eq.MarkLookups("4:" + new_text, bFilter4);
        }
        private void ToolStripCheckLookup5_CheckChanged(object sender, EventArgs e)
        {
            if (toolStripCheckLookup5.Checked)
            {
                toolStripCheckLookup5.Text = "L";
                bFilter5 = false;
            }
            else
            { toolStripCheckLookup5.Text = "F";
                bFilter5 = true;
            }
            string new_text = toolStripLookupBox5.Text.Replace(" ", "_");
            eq.MarkLookups("5:" + new_text, bFilter5);
        }

        private void ToolStripTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (toolStripLookupBox.Text.Length > 0)
                {
                    string new_text = toolStripLookupBox.Text.Replace(" ", "_");
                    eq.MarkLookups("0:" + new_text, bFilter0);
                    mapCon?.Focus();
                }
                else
                {
                    // text is blank, enter was pressed, but leave focus here
                    eq.MarkLookups("0:");
                }

                e.Handled = true;
            }
        }
        private void ToolStripTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (toolStripLookupBox1.Text.Length > 0)
                {
                    string new_text = toolStripLookupBox1.Text.Replace(" ", "_");
                    eq.MarkLookups("1:" + new_text, bFilter1);
                    mapCon?.Focus();
                }
                else
                {
                    // text is blank, enter was pressed, but leave focus here
                    eq.MarkLookups("1:");
                }

                e.Handled = true;
            }
        }
        private void ToolStripTextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (toolStripLookupBox2.Text.Length > 0)
                {
                    string new_text = toolStripLookupBox2.Text.Replace(" ", "_");
                    eq.MarkLookups("2:" + new_text, bFilter2);
                    mapCon?.Focus();
                }
                else
                {
                    // text is blank, enter was pressed, but leave focus here
                    eq.MarkLookups("2:");
                }

                e.Handled = true;
            }
        }
        private void ToolStripTextBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (toolStripLookupBox3.Text.Length > 0)
                {
                    string new_text = toolStripLookupBox3.Text.Replace(" ", "_");
                    eq.MarkLookups("3:" + new_text, bFilter3);
                    mapCon?.Focus();
                }
                else
                {
                    // text is blank, enter was pressed, but leave focus here
                    eq.MarkLookups("3:");
                }

                e.Handled = true;
            }
        }
        private void ToolStripTextBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (toolStripLookupBox4.Text.Length > 0)
                {
                    string new_text = toolStripLookupBox4.Text.Replace(" ", "_");
                    eq.MarkLookups("4:" + new_text, bFilter4);
                    mapCon?.Focus();
                }
                else
                {
                    // text is blank, enter was pressed, but leave focus here
                    eq.MarkLookups("4:");
                }

                e.Handled = true;
            }
        }
        private void ToolStripTextBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (toolStripLookupBox5.Text.Length > 0)
                {
                    string new_text = toolStripLookupBox5.Text.Replace(" ", "_");
                    eq.MarkLookups("5:" + new_text, bFilter5
                        );
                    mapCon?.Focus();
                }
                else
                {
                    // text is blank, enter was pressed, but leave focus here
                    eq.MarkLookups("5:");
                }

                e.Handled = true;
            }
        }

        private void ToolStripLookupBox_Click(object sender, EventArgs e)
        {
            if (toolStripLookupBox.Text == "Mob Search")
            {
                toolStripLookupBox.Text = "";
                toolStripLookupBox.ForeColor = SystemColors.WindowText;
            }
        }
        private void ToolStripLookupBox1_Click(object sender, EventArgs e)
        {
            if (toolStripLookupBox1.Text == "Mob Search")
            {
                toolStripLookupBox1.Text = "";
                toolStripLookupBox1.ForeColor = SystemColors.WindowText;
            }
        }
        private void ToolStripLookupBox2_Click(object sender, EventArgs e)
        {
            if (toolStripLookupBox2.Text == "Mob Search")
            {
                toolStripLookupBox2.Text = "";
                toolStripLookupBox2.ForeColor = SystemColors.WindowText;
            }
        }
        private void ToolStripLookupBox3_Click(object sender, EventArgs e)
        {
            if (toolStripLookupBox3.Text == "Mob Search")
            {
                toolStripLookupBox3.Text = "";
                toolStripLookupBox3.ForeColor = SystemColors.WindowText;
            }
        }
        private void ToolStripLookupBox4_Click(object sender, EventArgs e)
        {
            if (toolStripLookupBox4.Text == "Mob Search")
            {
                toolStripLookupBox4.Text = "";
                toolStripLookupBox4.ForeColor = SystemColors.WindowText;
            }
        }
        private void ToolStripLookupBox5_Click(object sender, EventArgs e)
        {
            if (toolStripLookupBox5.Text == "Mob Search")
            {
                toolStripLookupBox5.Text = "";
                toolStripLookupBox5.ForeColor = SystemColors.WindowText;
            }
        }
        private void ToolStripLookupBox_Leave(object sender, EventArgs e)
        {
            if (toolStripLookupBox.Text.Length > 0 && toolStripLookupBox.Text != "Mob Search")
            {
                string new_text = toolStripLookupBox.Text.Replace(" ", "_");
                eq.MarkLookups("0:" + new_text, bFilter0);
            }
            else
            {
                toolStripLookupBox.ForeColor = SystemColors.GrayText;
                toolStripLookupBox.Text = "Mob Search";
            }
        }

        private void ToolStripLookupBox1_Leave(object sender, EventArgs e)
        {
            if (toolStripLookupBox1.Text.Length > 0 && toolStripLookupBox1.Text != "Mob Search")
            {
                string new_text = toolStripLookupBox1.Text.Replace(" ", "_");
                eq.MarkLookups("1:" + new_text, bFilter1);
            }
            else
            {
                toolStripLookupBox1.ForeColor = SystemColors.GrayText;
                toolStripLookupBox1.Text = "Mob Search";
            }
        }

        private void ToolStripLookupBox2_Leave(object sender, EventArgs e)
        {
            if (toolStripLookupBox2.Text.Length > 0 && toolStripLookupBox2.Text != "Mob Search")
            {
                string new_text = toolStripLookupBox2.Text.Replace(" ", "_");
                eq.MarkLookups("2:" + new_text, bFilter2);
            }
            else
            {
                toolStripLookupBox2.ForeColor = SystemColors.GrayText;
                toolStripLookupBox2.Text = "Mob Search";
            }
        }

        private void ToolStripLookupBox3_Leave(object sender, EventArgs e)
        {
            if (toolStripLookupBox3.Text.Length > 0 && toolStripLookupBox3.Text != "Mob Search")
            {
                string new_text = toolStripLookupBox3.Text.Replace(" ", "_");
                eq.MarkLookups("3:" + new_text, bFilter3);
            }
            else
            {
                toolStripLookupBox3.ForeColor = SystemColors.GrayText;
                toolStripLookupBox3.Text = "Mob Search";
            }
        }

        private void ToolStripLookupBox4_Leave(object sender, EventArgs e)
        {
            if (toolStripLookupBox4.Text.Length > 0 && toolStripLookupBox4.Text != "Mob Search")
            {
                string new_text = toolStripLookupBox4.Text.Replace(" ", "_");
                eq.MarkLookups("4:" + new_text, bFilter4);
            }
            else
            {
                toolStripLookupBox4.ForeColor = SystemColors.GrayText;
                toolStripLookupBox4.Text = "Mob Search";
            }
        }

        private void ToolStripLookupBox5_Leave(object sender, EventArgs e)
        {
            if (toolStripLookupBox5.Text.Length > 0 && toolStripLookupBox5.Text != "Mob Search")
            {
                string new_text = toolStripLookupBox5.Text.Replace(" ", "_");
                eq.MarkLookups("5:" + new_text, bFilter5);
            }
            else
            {
                toolStripLookupBox5.ForeColor = SystemColors.GrayText;
                toolStripLookupBox5.Text = "Mob Search";
            }
        }

        #endregion

        private void MnuFileMain_DropDownOpening(object sender, EventArgs e) => VisChar();
            // Update the Character Selection list

        private void VisChar()
        {
            mnuChar1.Visible = false;

            mnuChar1.Text = "Char 1";

            mnuChar2.Visible = false;

            mnuChar2.Text = "Char 2";

            mnuChar3.Visible = false;

            mnuChar3.Text = "Char 3";

            mnuChar4.Visible = false;

            mnuChar4.Text = "Char 4";

            mnuChar5.Visible = false;

            mnuChar5.Text = "Char 5";

            mnuChar6.Visible = false;

            mnuChar6.Text = "Char 6";

            mnuChar7.Visible = false;

            mnuChar7.Text = "Char 7";

            mnuChar8.Visible = false;

            mnuChar8.Text = "Char 8";

            mnuChar9.Visible = false;

            mnuChar9.Text = "Char 9";

            mnuChar10.Visible = false;

            mnuChar10.Text = "Char 10";

            mnuChar11.Visible = false;

            mnuChar11.Text = "Char 11";

            mnuChar12.Visible = false;

            mnuChar12.Text = "Char 12";

            comm.CharRefresh();
        }

        public void StartNewPackets() => processcount = 0;

        // <summary>
        // These do almost exactly the same, can link all to one method.
        private void ToolStripLevel_TextUpdate(object sender, EventArgs e)
        {
            string Str = toolStripLevel.Text.Trim();

            ToolStripLevelCheck(Str);
        }

        private void ToolStripLevelCheck(string Str)
        {
            bool validnum = true;
            if (!string.IsNullOrEmpty(Str))
            {
                bool isNum = int.TryParse(Str, out var Num);

                if (isNum && (Num < 1 || Num > 115))
                {
                    validnum = false;
                }
                else if (Str != "Auto" && !isNum)
                {
                    validnum = false;
                }
                else if (Str == "Auto")
                {
                    validnum = true;
                    Settings.Default.LevelOverride = -1;
                    toolStripLevel.Text = "Auto";
                    }
                else
                {
                    toolStripLevel.Text = Num.ToString();
                    Settings.Default.LevelOverride = Num;
                    //gconLevel = Num;
                }
            }

            if (!validnum)
            {
                MessageBox.Show("Enter a number between 1-115 or Auto");
                //} else {
                //    gConBaseName = "";
            }
        }

        private void ToolStripLevel_Leave(object sender, EventArgs e)
        {
            string Str = toolStripLevel.Text.Trim();

            ToolStripLevelCheck(Str);
        }
        private void ToolStripLevel_DropDownClosed(object sender, EventArgs e)
        {
            var Str = toolStripLevel.SelectedItem.ToString();

           ToolStripLevelCheck(Str);
        }
        private void ToolStripLevel_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                string Str = toolStripLevel.Text.Trim();

                ToolStripLevelCheck(Str);
                toolStripScale.Focus();

                e.Handled = true;
            }
        }

        public void EnablePlayAlerts() => playAlerts = true;

        public void DisablePlayAlerts() => playAlerts = false;
    }
}