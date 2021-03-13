// Class Files

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
        public string Version = "";

        public Filters filters = new Filters();

        public string curZone = "map_pane";

        public string currentIPAddress = "";

        public float rawScale = 1.0f;

        public string BaseTitle = "MySEQ Open";

        private Point addTextFormLocation = new Point(0, 0);

        public MapCon mapCon;

        public MapPane mapPane = new MapPane();

        public ListViewPanel SpawnList = new ListViewPanel(0);

        public ListViewPanel SpawnTimerList = new ListViewPanel(1);

        public ListViewPanel GroundItemList = new ListViewPanel(2);

        private EQData eq;

        private EQCommunications comm;

        private EQMap map;

        public DrawOptions DrawOpts=DrawOptions.DrawNormal;

        public ArrayList colProcesses = new ArrayList();

        public ProcessInfo CurrentProcess = new ProcessInfo(0,"");

        public int processcount = 0;

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
        private ToolStripMenuItem mnuShowPCGuild;
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
        private ToolStripMenuItem mnuShowPCGuild2;
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

        private DeserializeDockContent m_deserializeDockContent;
        private ToolStripMenuItem mnuShowListSearchBox;
        private ToolStripMenuItem mnuSmallTargetInfo;
        private ToolStripMenuItem mnuSmallTargetInfo2;
        private ToolStripSeparator toolStripSeparator19;
//        private ToolStripMenuItem addZoneEmailAlertFilterToolStripMenuItem;
        private ToolStripMenuItem mnuAutoConnect;
        public ToolStripComboBox toolStripLevel;
        public int gLastconLevel = -1;
        public int gconLevel = -1;
        public string gConBaseName = "";
        private ToolStripMenuItem toolStripBasecon;

        private bool bIsRunning = false;
        private bool bFilter0 = false;
        private bool bFilter1 = false;
        private bool bFilter2 = false;
        private bool bFilter3 = false;
        private bool bFilter4 = false;
        private bool bFilter5 = false;

        public void StopListening()

        {
            // Stop the Timer

            timPackets.Stop();
            timDelayAlerts.Stop();
            eq.DisablePlayAlerts();

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

            string myPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MySEQ");

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

            SpawnList.ColumnsAdd("Guild", Settings.Default.c14w, HorizontalAlignment.Left);

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

            mapCon.SetInitialParams();

            eq.InitLookups();

            timPackets.Interval = Settings.Default.UpdateDelay;

            // This is delay that stops emails and alert sounds right after zoning
            timDelayAlerts.Interval = 10000;

            // This is for processing timers, do it once per second.
            timProcessTimers.Interval = 1000;

            SetUpdateSteps();

            Text = BaseTitle;

            if (Settings.Default.AutoConnect)
                StartListening();
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
            this.components = new Container();
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
            this.mnuMainMenu = new MenuStrip();
            this.mnuFileMain = new ToolStripMenuItem();
            this.mnuOptions = new ToolStripMenuItem();
            this.mnuSavePrefs = new ToolStripMenuItem();
            this.toolStripSeparator2 = new ToolStripSeparator();
            this.mnuOpenMap = new ToolStripMenuItem();
            this.mnuSaveMobs = new ToolStripMenuItem();
            this.toolStripSeparator1 = new ToolStripSeparator();
            this.mnuConnect = new ToolStripMenuItem();
            this.mnuAutoConnect = new ToolStripMenuItem();
            this.mnuServerSelection = new ToolStripMenuItem();
            this.mnuIPAddress1 = new ToolStripMenuItem();
            this.mnuIPAddress2 = new ToolStripMenuItem();
            this.mnuIPAddress3 = new ToolStripMenuItem();
            this.mnuIPAddress4 = new ToolStripMenuItem();
            this.mnuIPAddress5 = new ToolStripMenuItem();
            this.mnuCharSelect = new ToolStripMenuItem();
            this.mnuChar1 = new ToolStripMenuItem();
            this.mnuChar2 = new ToolStripMenuItem();
            this.mnuChar3 = new ToolStripMenuItem();
            this.mnuChar4 = new ToolStripMenuItem();
            this.mnuChar5 = new ToolStripMenuItem();
            this.mnuChar6 = new ToolStripMenuItem();
            this.mnuChar7 = new ToolStripMenuItem();
            this.mnuChar8 = new ToolStripMenuItem();
            this.mnuChar9 = new ToolStripMenuItem();
            this.mnuChar10 = new ToolStripMenuItem();
            this.mnuChar11 = new ToolStripMenuItem();
            this.mnuChar12 = new ToolStripMenuItem();
            this.menuItem13 = new ToolStripSeparator();
            this.mnuCharRefresh = new ToolStripMenuItem();
            this.toolStripSeparator3 = new ToolStripSeparator();
            this.mnuExit = new ToolStripMenuItem();
            this.mnuEditMain = new ToolStripMenuItem();
            this.mnuChangeColor = new ToolStripMenuItem();
            this.mnuGridColor = new ToolStripMenuItem();
            this.mnuGridLabelColor = new ToolStripMenuItem();
            this.mnuListColor = new ToolStripMenuItem();
            this.mnuBackgroungColor = new ToolStripMenuItem();
            this.mnuChangeFont = new ToolStripMenuItem();
            this.mnuSpawnListFont = new ToolStripMenuItem();
            this.mnuTargetInfoFont = new ToolStripMenuItem();
            this.mnuMapLabelsFont = new ToolStripMenuItem();
            this.toolStripSeparator18 = new ToolStripSeparator();
            this.mnuReloadAlerts = new ToolStripMenuItem();
            this.toolStripSeparator4 = new ToolStripSeparator();
            this.mnuEditGlobalAlerts = new ToolStripMenuItem();
            this.mnuEditZoneAlerts = new ToolStripMenuItem();
            this.toolStripSeparator5 = new ToolStripSeparator();
            this.mnuRefreshSpawnList = new ToolStripMenuItem();
            this.mnuClearSavedTimers = new ToolStripMenuItem();
            this.mnuSaveSpawnLog = new ToolStripMenuItem();
            this.mnuViewMain = new ToolStripMenuItem();
            this.toolbarsToolStripMenuItem = new ToolStripMenuItem();
            this.mnuViewMenuBar = new ToolStripMenuItem();
            this.mnuViewStatusBar = new ToolStripMenuItem();
            this.mnuViewDepthFilterBar = new ToolStripMenuItem();
            this.toolStripSeparator9 = new ToolStripSeparator();
            this.mnuShowSpawnList = new ToolStripMenuItem();
            this.mnuShowSpawnListTimer = new ToolStripMenuItem();
            this.mnuShowGroundItemList = new ToolStripMenuItem();
            this.toolStripSeparator7 = new ToolStripSeparator();
            this.mnuShowListGridLines = new ToolStripMenuItem();
            this.mnuShowListSearchBox = new ToolStripMenuItem();
            this.mnuShowGridLines = new ToolStripMenuItem();
            this.toolStripSeparator8 = new ToolStripSeparator();
            this.mnuShowCorpses = new ToolStripMenuItem();
            this.mnuShowPCCorpses = new ToolStripMenuItem();
            this.mnuShowMyCorpse = new ToolStripMenuItem();
            this.mnuShowPlayers = new ToolStripMenuItem();
            this.mnuShowInvis = new ToolStripMenuItem();
            this.mnuShowMounts = new ToolStripMenuItem();
            this.mnuShowFamiliars = new ToolStripMenuItem();
            this.mnuShowPets = new ToolStripMenuItem();
            this.mnuShowNPCs = new ToolStripMenuItem();
            this.mnuShowLookupText = new ToolStripMenuItem();
            this.mnuShowLookupNumber = new ToolStripMenuItem();
            this.mnuAlwaysOnTop = new ToolStripMenuItem();
            this.mnuMapSettingsMain = new ToolStripMenuItem();
            this.mnuDepthFilter = new ToolStripMenuItem();
            this.menuItem3 = new ToolStripMenuItem();
            this.mnuDynamicAlpha = new ToolStripMenuItem();
            this.mnuFilterMapLines = new ToolStripMenuItem();
            this.mnuFilterMapText = new ToolStripMenuItem();
            this.mnuFilterNPCs = new ToolStripMenuItem();
            this.mnuFilterNPCCorpses = new ToolStripMenuItem();
            this.mnuFilterPlayers = new ToolStripMenuItem();
            this.mnuFilterPlayerCorpses = new ToolStripMenuItem();
            this.mnuFilterGroundItems = new ToolStripMenuItem();
            this.mnuFilterSpawnPoints = new ToolStripMenuItem();
            this.mnuForceDistinct = new ToolStripMenuItem();
            this.mnuForceDistinctText = new ToolStripMenuItem();
            this.toolStripSeparator12 = new ToolStripSeparator();
            this.mnuLabelShow = new ToolStripMenuItem();
            this.mnuShowNPCLevels = new ToolStripMenuItem();
            this.mnuShowNPCNames = new ToolStripMenuItem();
            this.mnuShowNPCCorpseNames = new ToolStripMenuItem();
            this.mnuShowPCNames = new ToolStripMenuItem();
            this.mnuShowPlayerCorpseNames = new ToolStripMenuItem();
            this.mnuShowPCGuild = new ToolStripMenuItem();
            this.mnuSpawnCountdown = new ToolStripMenuItem();
            this.mnuShowSpawnPoints = new ToolStripMenuItem();
            this.mnuShowZoneText = new ToolStripMenuItem();
            this.mnuShowLayer1 = new ToolStripMenuItem();
            this.mnuShowLayer2 = new ToolStripMenuItem();
            this.mnuShowLayer3 = new ToolStripMenuItem();
            this.mnuShowPVP = new ToolStripMenuItem();
            this.mnuShowPVPLevel = new ToolStripMenuItem();
            this.mnuCollectMobTrails = new ToolStripMenuItem();
            this.mnuShowMobTrails = new ToolStripMenuItem();
            this.mnuConColors = new ToolStripMenuItem();
            this.mnuConDefault = new ToolStripMenuItem();
            this.mnuConSoD = new ToolStripMenuItem();
            this.mnuConSoF = new ToolStripMenuItem();
            this.mnuGridInterval = new ToolStripMenuItem();
            this.mnuGridInterval100 = new ToolStripMenuItem();
            this.mnuGridInterval250 = new ToolStripMenuItem();
            this.mnuGridInterval500 = new ToolStripMenuItem();
            this.mnuGridInterval1000 = new ToolStripMenuItem();
            this.mnuShowTargetInfo = new ToolStripMenuItem();
            this.mnuSmallTargetInfo = new ToolStripMenuItem();
            this.mnuAutoSelectEQTarget = new ToolStripMenuItem();
            this.toolStripSeparator10 = new ToolStripSeparator();
            this.mnuFollowNone = new ToolStripMenuItem();
            this.mnuFollowPlayer = new ToolStripMenuItem();
            this.mnuFollowTarget = new ToolStripMenuItem();
            this.toolStripSeparator11 = new ToolStripSeparator();
            this.mnuKeepCentered = new ToolStripMenuItem();
            this.mnuAutoExpand = new ToolStripMenuItem();
            this.toolStripSeparator13 = new ToolStripSeparator();
            this.mnuMapReset = new ToolStripMenuItem();
            this.mnuHelpMain = new ToolStripMenuItem();
            this.mnuAbout = new ToolStripMenuItem();
            this.mnuContext = new ContextMenuStrip(this.components);
            this.mnuDepthFilter2 = new ToolStripMenuItem();
            this.toolStripMenuItem2 = new ToolStripMenuItem();
            this.mnuDynamicAlpha2 = new ToolStripMenuItem();
            this.mnuFilterMapLines2 = new ToolStripMenuItem();
            this.mnuFilterMapText2 = new ToolStripMenuItem();
            this.mnuFilterNPCs2 = new ToolStripMenuItem();
            this.mnuFilterNPCCorpses2 = new ToolStripMenuItem();
            this.mnuFilterPlayers2 = new ToolStripMenuItem();
            this.mnuFilterPlayerCorpses2 = new ToolStripMenuItem();
            this.mnuFilterGroundItems2 = new ToolStripMenuItem();
            this.mnuFilterSpawnPoints2 = new ToolStripMenuItem();
            this.mnuForceDistinct2 = new ToolStripMenuItem();
            this.mnuForceDistinctText2 = new ToolStripMenuItem();
            this.toolStripSeparator6 = new ToolStripSeparator();
            this.addMapTextToolStripMenuItem = new ToolStripMenuItem();
            this.mnuLabelShow2 = new ToolStripMenuItem();
            this.mnuShowNPCLevels2 = new ToolStripMenuItem();
            this.mnuShowNPCNames2 = new ToolStripMenuItem();
            this.mnuShowNPCCorpseNames2 = new ToolStripMenuItem();
            this.mnuShowPCNames2 = new ToolStripMenuItem();
            this.mnuShowPlayerCorpseNames2 = new ToolStripMenuItem();
            this.mnuShowPCGuild2 = new ToolStripMenuItem();
            this.mnuSpawnCountdown2 = new ToolStripMenuItem();
            this.mnuShowSpawnPoints2 = new ToolStripMenuItem();
            this.mnuShowZoneText2 = new ToolStripMenuItem();
            this.mnuShowLayer21 = new ToolStripMenuItem();
            this.mnuShowLayer22 = new ToolStripMenuItem();
            this.mnuShowLayer23 = new ToolStripMenuItem();
            this.mnuShowPVP2 = new ToolStripMenuItem();
            this.mnuShowPVPLevel2 = new ToolStripMenuItem();
            this.mnuShowTargetInfo2 = new ToolStripMenuItem();
            this.mnuSmallTargetInfo2 = new ToolStripMenuItem();
            this.mnuAutoSelectEQTarget2 = new ToolStripMenuItem();
            this.toolStripSeparator15 = new ToolStripSeparator();
            this.mnuFollowNone2 = new ToolStripMenuItem();
            this.mnuFollowPlayer2 = new ToolStripMenuItem();
            this.mnuFollowTarget2 = new ToolStripMenuItem();
            this.toolStripSeparator16 = new ToolStripSeparator();
            this.mnuKeepCentered2 = new ToolStripMenuItem();
            this.mnuAutoExpand2 = new ToolStripMenuItem();
            this.toolStripSeparator17 = new ToolStripSeparator();
            this.mnuShowMenuBar = new ToolStripMenuItem();
            this.mnuMapReset2 = new ToolStripMenuItem();
            this.openFileDialog = new OpenFileDialog();
            this.fontDialog1 = new FontDialog();
            this.mnuContextAddFilter = new ContextMenuStrip(this.components);
            this.mnuMobName = new ToolStripMenuItem();
            this.menuItem11 = new ToolStripSeparator();
            this.mnuAddHuntFilter = new ToolStripMenuItem();
            this.mnuAddCautionFilter = new ToolStripMenuItem();
            this.mnuAddDangerFilter = new ToolStripMenuItem();
            this.mnuAddAlertFilter = new ToolStripMenuItem();
            this.toolStripBasecon = new ToolStripMenuItem();
            this.mnuSepAddFilter = new ToolStripSeparator();
            this.mnuAddMapLabel = new ToolStripMenuItem();
            this.toolStripSepAddMapLabel = new ToolStripSeparator();
            this.mnuSearchAllakhazam = new ToolStripMenuItem();
            this.timPackets = new Timer(this.components);
            this.timDelayAlerts = new System.Timers.Timer();
            this.timProcessTimers = new System.Timers.Timer();
            this.colorPicker = new ColorDialog();
            this.mnuShowListNPCs = new ToolStripMenuItem();
            this.mnuShowListCorpses = new ToolStripMenuItem();
            this.mnuShowListPlayers = new ToolStripMenuItem();
            this.mnuShowListInvis = new ToolStripMenuItem();
            this.mnuShowListMounts = new ToolStripMenuItem();
            this.mnuShowListFamiliars = new ToolStripMenuItem();
            this.mnuShowListPets = new ToolStripMenuItem();
            this.statusBarStrip = new StatusStrip();
            this.toolStripMouseLocation = new ToolStripStatusLabel();
            this.toolStripDistance = new ToolStripStatusLabel();
            this.toolStripSpring = new ToolStripStatusLabel();
            this.toolStripVersion = new ToolStripStatusLabel();
            this.toolStripServerAddress = new ToolStripStatusLabel();
            this.toolStripCoPStatus = new ToolStripStatusLabel();
            this.toolStripShortName = new ToolStripStatusLabel();
            this.toolStripFPS = new ToolStripStatusLabel();
            this.toolBarStrip = new ToolStrip();
            this.toolStripStartStop = new ToolStripButton();
            this.toolStripLevel = new ToolStripComboBox();
            this.toolStripSeparator14 = new ToolStripSeparator();
            this.toolStripZoomIn = new ToolStripButton();
            this.toolStripZoomOut = new ToolStripButton();
            this.toolStripScale = new ToolStripComboBox();
            this.toolStripDepthFilterButton = new ToolStripButton();
            this.toolStripZPosLabel = new ToolStripLabel();
            this.toolStripZPos = new ToolStripTextBox();
            this.toolStripZPosDown = new ToolStripButton();
            this.toolStripZPosUp = new ToolStripButton();
            this.toolStripZOffsetLabel = new ToolStripLabel();
            this.toolStripZNeg = new ToolStripTextBox();
            this.toolStripZNegUp = new ToolStripButton();
            this.toolStripZNegDown = new ToolStripButton();
            this.toolStripResetDepthFilter = new ToolStripButton();
            this.toolStripOptions = new ToolStripButton();
            this.toolStripSeparator19 = new ToolStripSeparator();
            this.toolStripLabel1 = new ToolStripLabel();
            this.toolStripLookupBox = new ToolStripTextBox();
            this.toolStripCheckLookup = new ToolStripButton();
            this.toolStripResetLookup = new ToolStripButton();
            this.toolStripLookupBox1 = new ToolStripTextBox();
            this.toolStripCheckLookup1 = new ToolStripButton();
            this.toolStripResetLookup1 = new ToolStripButton();
            this.toolStripLookupBox2 = new ToolStripTextBox();
            this.toolStripCheckLookup2 = new ToolStripButton();
            this.toolStripResetLookup2 = new ToolStripButton();
            this.toolStripLookupBox3 = new ToolStripTextBox();
            this.toolStripCheckLookup3 = new ToolStripButton();
            this.toolStripResetLookup3 = new ToolStripButton();
            this.toolStripLookupBox4 = new ToolStripTextBox();
            this.toolStripCheckLookup4 = new ToolStripButton();
            this.toolStripResetLookup4 = new ToolStripButton();
            this.toolStripLookupBox5 = new ToolStripTextBox();
            this.toolStripCheckLookup5 = new ToolStripButton();
            this.toolStripResetLookup5 = new ToolStripButton();
            this.dockPanel = new DockPanel();
            this.mnuMainMenu.SuspendLayout();
            this.mnuContext.SuspendLayout();
            this.mnuContextAddFilter.SuspendLayout();
            this.timDelayAlerts.BeginInit();
            this.timProcessTimers.BeginInit();
            this.statusBarStrip.SuspendLayout();
            this.toolBarStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // mnuMainMenu
            // 
            this.mnuMainMenu.BackColor = SystemColors.ControlLight;
            this.mnuMainMenu.Items.AddRange(new ToolStripItem[] {
            this.mnuFileMain,
            this.mnuEditMain,
            this.mnuViewMain,
            this.mnuMapSettingsMain,
            this.mnuHelpMain});
            this.mnuMainMenu.Location = new Point(0, 0);
            this.mnuMainMenu.Name = "mnuMainMenu";
            this.mnuMainMenu.Size = new Size(1464, 24);
            this.mnuMainMenu.TabIndex = 0;
            this.mnuMainMenu.Text = "mnuMainMenu";
            // 
            // mnuFileMain
            // 
            this.mnuFileMain.DropDownItems.AddRange(new ToolStripItem[] {
            this.mnuOptions,
            this.mnuSavePrefs,
            this.toolStripSeparator2,
            this.mnuOpenMap,
            this.mnuSaveMobs,
            this.toolStripSeparator1,
            this.mnuConnect,
            this.mnuAutoConnect,
            this.mnuServerSelection,
            this.mnuCharSelect,
            this.toolStripSeparator3,
            this.mnuExit});
            this.mnuFileMain.Name = "mnuFileMain";
            this.mnuFileMain.Size = new Size(37, 20);
            this.mnuFileMain.Text = "&File";
            this.mnuFileMain.DropDownOpening += new EventHandler(this.mnuFileMain_DropDownOpening);
            // 
            // mnuOptions
            // 
            this.mnuOptions.Image = ((Image)(resources.GetObject("mnuOptions.Image")));
            this.mnuOptions.ImageTransparentColor = Color.Magenta;
            this.mnuOptions.Name = "mnuOptions";
            this.mnuOptions.ShortcutKeys = Keys.F1;
            this.mnuOptions.Size = new Size(180, 22);
            this.mnuOptions.Text = "&Options";
            this.mnuOptions.Click += new EventHandler(this.mnuOptions_Click);
            // 
            // mnuSavePrefs
            // 
            this.mnuSavePrefs.Image = ((Image)(resources.GetObject("mnuSavePrefs.Image")));
            this.mnuSavePrefs.Name = "mnuSavePrefs";
            this.mnuSavePrefs.Size = new Size(180, 22);
            this.mnuSavePrefs.Text = "Save &Prefs";
            this.mnuSavePrefs.Click += new EventHandler(this.mnuSavePrefs_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new Size(177, 6);
            // 
            // mnuOpenMap
            // 
            this.mnuOpenMap.Image = ((Image)(resources.GetObject("mnuOpenMap.Image")));
            this.mnuOpenMap.Name = "mnuOpenMap";
            this.mnuOpenMap.ShortcutKeys = ((Keys)((Keys.Control | Keys.O)));
            this.mnuOpenMap.Size = new Size(180, 22);
            this.mnuOpenMap.Text = "&Open Map";
            this.mnuOpenMap.Click += new EventHandler(this.mnuOpenMap_Click);
            // 
            // mnuSaveMobs
            // 
            this.mnuSaveMobs.Name = "mnuSaveMobs";
            this.mnuSaveMobs.ShortcutKeys = ((Keys)((Keys.Control | Keys.S)));
            this.mnuSaveMobs.Size = new Size(180, 22);
            this.mnuSaveMobs.Text = "&Save Mobs";
            this.mnuSaveMobs.Click += new EventHandler(this.mnuSaveMobs_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new Size(177, 6);
            // 
            // mnuConnect
            // 
            this.mnuConnect.Image = ((Image)(resources.GetObject("mnuConnect.Image")));
            this.mnuConnect.ImageScaling = ToolStripItemImageScaling.None;
            this.mnuConnect.Name = "mnuConnect";
            this.mnuConnect.Size = new Size(180, 22);
            this.mnuConnect.Text = "&Connect";
            this.mnuConnect.Click += new EventHandler(this.cmdCommand_Click);
            // 
            // mnuAutoConnect
            // 
            this.mnuAutoConnect.Name = "mnuAutoConnect";
            this.mnuAutoConnect.Size = new Size(180, 22);
            this.mnuAutoConnect.Text = "Connect on Startup";
            this.mnuAutoConnect.Click += new EventHandler(this.mnuAutoConnect_Click);
            // 
            // mnuServerSelection
            // 
            this.mnuServerSelection.DropDownItems.AddRange(new ToolStripItem[] {
            this.mnuIPAddress1,
            this.mnuIPAddress2,
            this.mnuIPAddress3,
            this.mnuIPAddress4,
            this.mnuIPAddress5});
            this.mnuServerSelection.Name = "mnuServerSelection";
            this.mnuServerSelection.Size = new Size(180, 22);
            this.mnuServerSelection.Text = "&Server Selection";
            // 
            // mnuIPAddress1
            // 
            this.mnuIPAddress1.Name = "mnuIPAddress1";
            this.mnuIPAddress1.ShortcutKeys = ((Keys)((Keys.Control | Keys.D1)));
            this.mnuIPAddress1.Size = new Size(180, 22);
            this.mnuIPAddress1.Click += new EventHandler(this.mnuIPAddress1_Click);
            // 
            // mnuIPAddress2
            // 
            this.mnuIPAddress2.Name = "mnuIPAddress2";
            this.mnuIPAddress2.ShortcutKeys = ((Keys)((Keys.Control | Keys.D2)));
            this.mnuIPAddress2.Size = new Size(180, 22);
            this.mnuIPAddress2.Click += new EventHandler(this.mnuIPAddress2_Click);
            // 
            // mnuIPAddress3
            // 
            this.mnuIPAddress3.Name = "mnuIPAddress3";
            this.mnuIPAddress3.ShortcutKeys = ((Keys)((Keys.Control | Keys.D3)));
            this.mnuIPAddress3.Size = new Size(180, 22);
            this.mnuIPAddress3.Click += new EventHandler(this.mnuIPAddress3_Click);
            // 
            // mnuIPAddress4
            // 
            this.mnuIPAddress4.Name = "mnuIPAddress4";
            this.mnuIPAddress4.ShortcutKeys = ((Keys)((Keys.Control | Keys.D4)));
            this.mnuIPAddress4.Size = new Size(180, 22);
            this.mnuIPAddress4.Click += new EventHandler(this.mnuIPAddress4_Click);
            // 
            // mnuIPAddress5
            // 
            this.mnuIPAddress5.Name = "mnuIPAddress5";
            this.mnuIPAddress5.ShortcutKeys = ((Keys)((Keys.Control | Keys.D5)));
            this.mnuIPAddress5.Size = new Size(180, 22);
            this.mnuIPAddress5.Click += new EventHandler(this.mnuIPAddress5_Click);
            // 
            // mnuCharSelect
            // 
            this.mnuCharSelect.DropDownItems.AddRange(new ToolStripItem[] {
            this.mnuChar1,
            this.mnuChar2,
            this.mnuChar3,
            this.mnuChar4,
            this.mnuChar5,
            this.mnuChar6,
            this.mnuChar7,
            this.mnuChar8,
            this.mnuChar9,
            this.mnuChar10,
            this.mnuChar11,
            this.mnuChar12,
            this.menuItem13,
            this.mnuCharRefresh});
            this.mnuCharSelect.Name = "mnuCharSelect";
            this.mnuCharSelect.Overflow = ToolStripItemOverflow.Always;
            this.mnuCharSelect.Size = new Size(180, 22);
            this.mnuCharSelect.Text = "&Character Selection";
            // 
            // mnuChar1
            // 
            this.mnuChar1.Checked = true;
            this.mnuChar1.CheckState = CheckState.Checked;
            this.mnuChar1.Name = "mnuChar1";
            this.mnuChar1.ShortcutKeys = ((Keys)(((Keys.Control | Keys.Shift)
            | Keys.D1)));
            this.mnuChar1.Size = new Size(188, 22);
            this.mnuChar1.Text = "Char 1";
            this.mnuChar1.Visible = false;
            this.mnuChar1.Click += new EventHandler(this.mnuChar1_Click);
            // 
            // mnuChar2
            // 
            this.mnuChar2.Name = "mnuChar2";
            this.mnuChar2.ShortcutKeys = ((Keys)(((Keys.Control | Keys.Shift) 
            | Keys.D2)));
            this.mnuChar2.Size = new Size(188, 22);
            this.mnuChar2.Text = "Char 2";
            this.mnuChar2.Visible = false;
            this.mnuChar2.Click += new EventHandler(this.mnuChar2_Click);
            // 
            // mnuChar3
            // 
            this.mnuChar3.Name = "mnuChar3";
            this.mnuChar3.ShortcutKeys = ((Keys)(((Keys.Control | Keys.Shift) 
            | Keys.D3)));
            this.mnuChar3.Size = new Size(188, 22);
            this.mnuChar3.Text = "Char 3";
            this.mnuChar3.Visible = false;
            this.mnuChar3.Click += new EventHandler(this.mnuChar3_Click);
            // 
            // mnuChar4
            // 
            this.mnuChar4.Name = "mnuChar4";
            this.mnuChar4.ShortcutKeys = ((Keys)(((Keys.Control | Keys.Shift) 
            | Keys.D4)));
            this.mnuChar4.Size = new Size(188, 22);
            this.mnuChar4.Text = "Char 4";
            this.mnuChar4.Visible = false;
            this.mnuChar4.Click += new EventHandler(this.mnuChar4_Click);
            // 
            // mnuChar5
            // 
            this.mnuChar5.Name = "mnuChar5";
            this.mnuChar5.ShortcutKeys = ((Keys)(((Keys.Control | Keys.Shift) 
            | Keys.D5)));
            this.mnuChar5.Size = new Size(188, 22);
            this.mnuChar5.Text = "Char 5";
            this.mnuChar5.Visible = false;
            this.mnuChar5.Click += new EventHandler(this.mnuChar5_Click);
            // 
            // mnuChar6
            // 
            this.mnuChar6.Name = "mnuChar6";
            this.mnuChar6.ShortcutKeys = ((Keys)(((Keys.Control | Keys.Shift) 
            | Keys.D6)));
            this.mnuChar6.Size = new Size(188, 22);
            this.mnuChar6.Text = "Char 6";
            this.mnuChar6.Visible = false;
            this.mnuChar6.Click += new EventHandler(this.mnuChar6_Click);
            // 
            // mnuChar7
            // 
            this.mnuChar7.Name = "mnuChar7";
            this.mnuChar7.ShortcutKeys = ((Keys)(((Keys.Control | Keys.Shift) 
            | Keys.D7)));
            this.mnuChar7.Size = new Size(188, 22);
            this.mnuChar7.Text = "Char 7";
            this.mnuChar7.Visible = false;
            this.mnuChar7.Click += new EventHandler(this.mnuChar7_Click);
            // 
            // mnuChar8
            // 
            this.mnuChar8.Name = "mnuChar8";
            this.mnuChar8.ShortcutKeys = ((Keys)(((Keys.Control | Keys.Shift) 
            | Keys.D8)));
            this.mnuChar8.Size = new Size(188, 22);
            this.mnuChar8.Text = "Char 8";
            this.mnuChar8.Visible = false;
            this.mnuChar8.Click += new EventHandler(this.mnuChar8_Click);
            // 
            // mnuChar9
            // 
            this.mnuChar9.Name = "mnuChar9";
            this.mnuChar9.ShortcutKeys = ((Keys)(((Keys.Control | Keys.Shift) 
            | Keys.D9)));
            this.mnuChar9.Size = new Size(188, 22);
            this.mnuChar9.Text = "Char 9";
            this.mnuChar9.Visible = false;
            this.mnuChar9.Click += new EventHandler(this.mnuChar9_Click);
            // 
            // mnuChar10
            // 
            this.mnuChar10.Name = "mnuChar10";
            this.mnuChar10.ShortcutKeys = ((Keys)(((Keys.Control | Keys.Shift) 
            | Keys.D0)));
            this.mnuChar10.Size = new Size(188, 22);
            this.mnuChar10.Text = "Char 10";
            this.mnuChar10.Visible = false;
            this.mnuChar10.Click += new EventHandler(this.mnuChar10_Click);
            // 
            // mnuChar11
            // 
            this.mnuChar11.Name = "mnuChar11";
            this.mnuChar11.ShortcutKeys = ((Keys)(((Keys.Control | Keys.Shift) 
            | Keys.B)));
            this.mnuChar11.Size = new Size(188, 22);
            this.mnuChar11.Text = "Char 11";
            this.mnuChar11.Visible = false;
            this.mnuChar11.Click += new EventHandler(this.mnuChar11_Click);
            // 
            // mnuChar12
            // 
            this.mnuChar12.Name = "mnuChar12";
            this.mnuChar12.ShortcutKeys = ((Keys)(((Keys.Control | Keys.Shift) 
            | Keys.C)));
            this.mnuChar12.Size = new Size(188, 22);
            this.mnuChar12.Text = "Char 12";
            this.mnuChar12.Visible = false;
            this.mnuChar12.Click += new EventHandler(this.mnuChar12_Click);
            // 
            // menuItem13
            // 
            this.menuItem13.Name = "menuItem13";
            this.menuItem13.Size = new Size(185, 6);
            this.menuItem13.Visible = false;
            // 
            // mnuCharRefresh
            // 
            this.mnuCharRefresh.Name = "mnuCharRefresh";
            this.mnuCharRefresh.Size = new Size(188, 22);
            this.mnuCharRefresh.Text = "Refresh List";
            this.mnuCharRefresh.Visible = false;
            this.mnuCharRefresh.Click += new EventHandler(this.mnuCharRefresh_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new Size(177, 6);
            // 
            // mnuExit
            // 
            this.mnuExit.Name = "mnuExit";
            this.mnuExit.ShortcutKeys = ((Keys)((Keys.Control | Keys.X)));
            this.mnuExit.Size = new Size(180, 22);
            this.mnuExit.Text = "E&xit";
            this.mnuExit.Click += new EventHandler(this.mnuExit_Click);
            // 
            // mnuEditMain
            // 
            this.mnuEditMain.DropDownItems.AddRange(new ToolStripItem[] {
            this.mnuChangeColor,
            this.mnuChangeFont,
            this.toolStripSeparator18,
            this.mnuReloadAlerts,
            this.toolStripSeparator4,
            this.mnuEditGlobalAlerts,
            this.mnuEditZoneAlerts,
            this.toolStripSeparator5,
            this.mnuRefreshSpawnList,
            this.mnuClearSavedTimers,
            this.mnuSaveSpawnLog});
            this.mnuEditMain.Name = "mnuEditMain";
            this.mnuEditMain.Size = new Size(39, 20);
            this.mnuEditMain.Text = "&Edit";
            // 
            // mnuChangeColor
            // 
            this.mnuChangeColor.DropDownItems.AddRange(new ToolStripItem[] {
            this.mnuGridColor,
            this.mnuGridLabelColor,
            this.mnuListColor,
            this.mnuBackgroungColor});
            this.mnuChangeColor.Name = "mnuChangeColor";
            this.mnuChangeColor.Size = new Size(173, 22);
            this.mnuChangeColor.Text = "C&hange Color";
            // 
            // mnuGridColor
            // 
            this.mnuGridColor.Name = "mnuGridColor";
            this.mnuGridColor.Size = new Size(197, 22);
            this.mnuGridColor.Text = "Grid Color";
            this.mnuGridColor.Click += new EventHandler(this.mnuGridColor_Click);
            // 
            // mnuGridLabelColor
            // 
            this.mnuGridLabelColor.Name = "mnuGridLabelColor";
            this.mnuGridLabelColor.Size = new Size(197, 22);
            this.mnuGridLabelColor.Text = "Grid Label Color";
            this.mnuGridLabelColor.Click += new EventHandler(this.mnuGridLabelColor_Click);
            // 
            // mnuListColor
            // 
            this.mnuListColor.Name = "mnuListColor";
            this.mnuListColor.Size = new Size(197, 22);
            this.mnuListColor.Text = "Spawn List Color";
            this.mnuListColor.Click += new EventHandler(this.mnuListColor_Click);
            // 
            // mnuBackgroungColor
            // 
            this.mnuBackgroungColor.Name = "mnuBackgroungColor";
            this.mnuBackgroungColor.Size = new Size(197, 22);
            this.mnuBackgroungColor.Text = "Map Background Color";
            this.mnuBackgroungColor.Click += new EventHandler(this.mnuBackgroundColor_Click);
            // 
            // mnuChangeFont
            // 
            this.mnuChangeFont.DropDownItems.AddRange(new ToolStripItem[] {
            this.mnuSpawnListFont,
            this.mnuTargetInfoFont,
            this.mnuMapLabelsFont});
            this.mnuChangeFont.Name = "mnuChangeFont";
            this.mnuChangeFont.Size = new Size(173, 22);
            this.mnuChangeFont.Text = "Change &Font";
            // 
            // mnuSpawnListFont
            // 
            this.mnuSpawnListFont.Name = "mnuSpawnListFont";
            this.mnuSpawnListFont.Size = new Size(161, 22);
            this.mnuSpawnListFont.Text = "Spawn List Font";
            this.mnuSpawnListFont.Click += new EventHandler(this.mnuSpawnListFont_Click);
            // 
            // mnuTargetInfoFont
            // 
            this.mnuTargetInfoFont.Name = "mnuTargetInfoFont";
            this.mnuTargetInfoFont.Size = new Size(161, 22);
            this.mnuTargetInfoFont.Text = "Target Info Font";
            this.mnuTargetInfoFont.Click += new EventHandler(this.mnuTargetInfoFont_Click);
            // 
            // mnuMapLabelsFont
            // 
            this.mnuMapLabelsFont.Name = "mnuMapLabelsFont";
            this.mnuMapLabelsFont.Size = new Size(161, 22);
            this.mnuMapLabelsFont.Text = "Map Labels Font";
            this.mnuMapLabelsFont.Click += new EventHandler(this.mnuMapLabelsFont_Click);
            // 
            // toolStripSeparator18
            // 
            this.toolStripSeparator18.Name = "toolStripSeparator18";
            this.toolStripSeparator18.Size = new Size(170, 6);
            // 
            // mnuReloadAlerts
            // 
            this.mnuReloadAlerts.Image = ((Image)(resources.GetObject("mnuReloadAlerts.Image")));
            this.mnuReloadAlerts.ImageScaling = ToolStripItemImageScaling.None;
            this.mnuReloadAlerts.Name = "mnuReloadAlerts";
            this.mnuReloadAlerts.Size = new Size(173, 22);
            this.mnuReloadAlerts.Text = "&Reload Alerts";
            this.mnuReloadAlerts.Click += new EventHandler(this.mnuReloadAlerts_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new Size(170, 6);
            // 
            // mnuEditGlobalAlerts
            // 
            this.mnuEditGlobalAlerts.Name = "mnuEditGlobalAlerts";
            this.mnuEditGlobalAlerts.Size = new Size(173, 22);
            this.mnuEditGlobalAlerts.Text = "Edit &Global Alerts";
            this.mnuEditGlobalAlerts.Click += new EventHandler(this.mnuGlobalAlerts_Click);
            // 
            // mnuEditZoneAlerts
            // 
            this.mnuEditZoneAlerts.Name = "mnuEditZoneAlerts";
            this.mnuEditZoneAlerts.Size = new Size(173, 22);
            this.mnuEditZoneAlerts.Text = "Edit &Zone Alerts";
            this.mnuEditZoneAlerts.Click += new EventHandler(this.mnuAddEditAlerts_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new Size(170, 6);
            // 
            // mnuRefreshSpawnList
            // 
            this.mnuRefreshSpawnList.Name = "mnuRefreshSpawnList";
            this.mnuRefreshSpawnList.Size = new Size(173, 22);
            this.mnuRefreshSpawnList.Text = "&Refresh Spawn List";
            this.mnuRefreshSpawnList.Click += new EventHandler(this.mnuRefreshSpawnList_Click);
            // 
            // mnuClearSavedTimers
            // 
            this.mnuClearSavedTimers.Name = "mnuClearSavedTimers";
            this.mnuClearSavedTimers.Size = new Size(173, 22);
            this.mnuClearSavedTimers.Text = "Clear Saved &Timers";
            this.mnuClearSavedTimers.Click += new EventHandler(this.mnuClearSavedTimers_Click);
            // 
            // mnuSaveSpawnLog
            // 
            this.mnuSaveSpawnLog.Name = "mnuSaveSpawnLog";
            this.mnuSaveSpawnLog.Size = new Size(173, 22);
            this.mnuSaveSpawnLog.Text = "Save Spawn Log";
            this.mnuSaveSpawnLog.Click += new EventHandler(this.mnuSaveSpawnLog_Click);
            // 
            // mnuViewMain
            // 
            this.mnuViewMain.DropDownItems.AddRange(new ToolStripItem[] {
            this.toolbarsToolStripMenuItem,
            this.toolStripSeparator9,
            this.mnuShowSpawnList,
            this.mnuShowSpawnListTimer,
            this.mnuShowGroundItemList,
            this.toolStripSeparator7,
            this.mnuShowListGridLines,
            this.mnuShowListSearchBox,
            this.mnuShowGridLines,
            this.toolStripSeparator8,
            this.mnuShowCorpses,
            this.mnuShowPCCorpses,
            this.mnuShowMyCorpse,
            this.mnuShowPlayers,
            this.mnuShowInvis,
            this.mnuShowMounts,
            this.mnuShowFamiliars,
            this.mnuShowPets,
            this.mnuShowNPCs,
            this.mnuShowLookupText,
            this.mnuShowLookupNumber,
            this.mnuAlwaysOnTop});
            this.mnuViewMain.Name = "mnuViewMain";
            this.mnuViewMain.Size = new Size(44, 20);
            this.mnuViewMain.Text = "&View";
            // 
            // toolbarsToolStripMenuItem
            // 
            this.toolbarsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
            this.mnuViewMenuBar,
            this.mnuViewStatusBar,
            this.mnuViewDepthFilterBar});
            this.toolbarsToolStripMenuItem.Name = "toolbarsToolStripMenuItem";
            this.toolbarsToolStripMenuItem.Size = new Size(198, 22);
            this.toolbarsToolStripMenuItem.Text = "Toolbars";
            // 
            // mnuViewMenuBar
            // 
            this.mnuViewMenuBar.Name = "mnuViewMenuBar";
            this.mnuViewMenuBar.ShortcutKeys = Keys.F2;
            this.mnuViewMenuBar.Size = new Size(162, 22);
            this.mnuViewMenuBar.Text = "&Main Menu";
            this.mnuViewMenuBar.Click += new EventHandler(this.mnuShowMenuBar_Click);
            // 
            // mnuViewStatusBar
            // 
            this.mnuViewStatusBar.Name = "mnuViewStatusBar";
            this.mnuViewStatusBar.ShortcutKeys = Keys.F3;
            this.mnuViewStatusBar.Size = new Size(162, 22);
            this.mnuViewStatusBar.Text = "&Status";
            this.mnuViewStatusBar.Click += new EventHandler(this.mnuViewStatusBar_Click);
            // 
            // mnuViewDepthFilterBar
            // 
            this.mnuViewDepthFilterBar.Name = "mnuViewDepthFilterBar";
            this.mnuViewDepthFilterBar.ShortcutKeys = Keys.F4;
            this.mnuViewDepthFilterBar.Size = new Size(162, 22);
            this.mnuViewDepthFilterBar.Text = "&Tool Bar Strip";
            this.mnuViewDepthFilterBar.Click += new EventHandler(this.mnuViewDepthFilterToolBar_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new Size(195, 6);
            // 
            // mnuShowSpawnList
            // 
            this.mnuShowSpawnList.Name = "mnuShowSpawnList";
            this.mnuShowSpawnList.Size = new Size(198, 22);
            this.mnuShowSpawnList.Text = "Spawn &List";
            this.mnuShowSpawnList.Click += new EventHandler(this.mnuShowSpawnList_Click);
            // 
            // mnuShowSpawnListTimer
            // 
            this.mnuShowSpawnListTimer.Name = "mnuShowSpawnListTimer";
            this.mnuShowSpawnListTimer.Size = new Size(198, 22);
            this.mnuShowSpawnListTimer.Text = "Spawn &Timer List";
            this.mnuShowSpawnListTimer.Click += new EventHandler(this.mnuShowSpawnListTimer_Click);
            // 
            // mnuShowGroundItemList
            // 
            this.mnuShowGroundItemList.Name = "mnuShowGroundItemList";
            this.mnuShowGroundItemList.Size = new Size(198, 22);
            this.mnuShowGroundItemList.Text = "Ground &Item List";
            this.mnuShowGroundItemList.Click += new EventHandler(this.mnuShowGroundItemList_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new Size(195, 6);
            // 
            // mnuShowListGridLines
            // 
            this.mnuShowListGridLines.Name = "mnuShowListGridLines";
            this.mnuShowListGridLines.Size = new Size(198, 22);
            this.mnuShowListGridLines.Text = "List &Grid Lines";
            this.mnuShowListGridLines.Click += new EventHandler(this.mnuShowListGridLines_Click);
            // 
            // mnuShowListSearchBox
            // 
            this.mnuShowListSearchBox.Name = "mnuShowListSearchBox";
            this.mnuShowListSearchBox.Size = new Size(198, 22);
            this.mnuShowListSearchBox.Text = "List Search Box";
            this.mnuShowListSearchBox.Click += new EventHandler(this.mnuShowListSearchBox_Click);
            // 
            // mnuShowGridLines
            // 
            this.mnuShowGridLines.Image = ((Image)(resources.GetObject("mnuShowGridLines.Image")));
            this.mnuShowGridLines.Name = "mnuShowGridLines";
            this.mnuShowGridLines.ShortcutKeys = Keys.F6;
            this.mnuShowGridLines.Size = new Size(198, 22);
            this.mnuShowGridLines.Text = "Map Grid Lines";
            this.mnuShowGridLines.Click += new EventHandler(this.mnuShowGridLines_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new Size(195, 6);
            // 
            // mnuShowCorpses
            // 
            this.mnuShowCorpses.Name = "mnuShowCorpses";
            this.mnuShowCorpses.Size = new Size(198, 22);
            this.mnuShowCorpses.Text = "NPC Corpses";
            this.mnuShowCorpses.Click += new EventHandler(this.mnuShowCorpses_Click);
            // 
            // mnuShowPCCorpses
            // 
            this.mnuShowPCCorpses.Name = "mnuShowPCCorpses";
            this.mnuShowPCCorpses.Size = new Size(198, 22);
            this.mnuShowPCCorpses.Text = "PC Corpses";
            this.mnuShowPCCorpses.Click += new EventHandler(this.mnuShowPCCorpses_Click);
            // 
            // mnuShowMyCorpse
            // 
            this.mnuShowMyCorpse.Name = "mnuShowMyCorpse";
            this.mnuShowMyCorpse.Size = new Size(198, 22);
            this.mnuShowMyCorpse.Text = "My Corpse";
            this.mnuShowMyCorpse.Click += new EventHandler(this.mnuShowMyCorpse_Click);
            // 
            // mnuShowPlayers
            // 
            this.mnuShowPlayers.Name = "mnuShowPlayers";
            this.mnuShowPlayers.Size = new Size(198, 22);
            this.mnuShowPlayers.Text = "Players";
            this.mnuShowPlayers.Click += new EventHandler(this.mnuShowPlayers_Click);
            // 
            // mnuShowInvis
            // 
            this.mnuShowInvis.Name = "mnuShowInvis";
            this.mnuShowInvis.Size = new Size(198, 22);
            this.mnuShowInvis.Text = "Invis Mobs";
            this.mnuShowInvis.Click += new EventHandler(this.mnuShowInvis_Click);
            // 
            // mnuShowMounts
            // 
            this.mnuShowMounts.Name = "mnuShowMounts";
            this.mnuShowMounts.Size = new Size(198, 22);
            this.mnuShowMounts.Text = "Mounts";
            this.mnuShowMounts.Click += new EventHandler(this.mnuShowMounts_Click);
            // 
            // mnuShowFamiliars
            // 
            this.mnuShowFamiliars.Name = "mnuShowFamiliars";
            this.mnuShowFamiliars.Size = new Size(198, 22);
            this.mnuShowFamiliars.Text = "Familiars";
            this.mnuShowFamiliars.Click += new EventHandler(this.mnuShowFamiliars_Click);
            // 
            // mnuShowPets
            // 
            this.mnuShowPets.Name = "mnuShowPets";
            this.mnuShowPets.Size = new Size(198, 22);
            this.mnuShowPets.Text = "Pets";
            this.mnuShowPets.Click += new EventHandler(this.mnuShowPets_Click);
            // 
            // mnuShowNPCs
            // 
            this.mnuShowNPCs.Name = "mnuShowNPCs";
            this.mnuShowNPCs.Size = new Size(198, 22);
            this.mnuShowNPCs.Text = "NPCs";
            this.mnuShowNPCs.Click += new EventHandler(this.mnuShowNPCs_Click);
            // 
            // mnuShowLookupText
            // 
            this.mnuShowLookupText.Name = "mnuShowLookupText";
            this.mnuShowLookupText.Size = new Size(198, 22);
            this.mnuShowLookupText.Text = "Lookup Text";
            this.mnuShowLookupText.Click += new EventHandler(this.mnuShowLookupText_Click);
            // 
            // mnuShowLookupNumber
            // 
            this.mnuShowLookupNumber.Name = "mnuShowLookupNumber";
            this.mnuShowLookupNumber.Size = new Size(198, 22);
            this.mnuShowLookupNumber.Text = "Lookup Name/Number";
            this.mnuShowLookupNumber.Click += new EventHandler(this.mnuShowLookupNumber_Click);
            // 
            // mnuAlwaysOnTop
            // 
            this.mnuAlwaysOnTop.Name = "mnuAlwaysOnTop";
            this.mnuAlwaysOnTop.ShortcutKeys = ((Keys)((Keys.Alt | Keys.T)));
            this.mnuAlwaysOnTop.Size = new Size(198, 22);
            this.mnuAlwaysOnTop.Text = "Always On Top";
            this.mnuAlwaysOnTop.Click += new EventHandler(this.mnuAlwaysOnTop_Click);
            // 
            // mnuMapSettingsMain
            // 
            this.mnuMapSettingsMain.DropDownItems.AddRange(new ToolStripItem[] {
            this.mnuDepthFilter,
            this.menuItem3,
            this.mnuForceDistinct,
            this.mnuForceDistinctText,
            this.toolStripSeparator12,
            this.mnuLabelShow,
            this.mnuCollectMobTrails,
            this.mnuShowMobTrails,
            this.mnuConColors,
            this.mnuGridInterval,
            this.mnuShowTargetInfo,
            this.mnuSmallTargetInfo,
            this.mnuAutoSelectEQTarget,
            this.toolStripSeparator10,
            this.mnuFollowNone,
            this.mnuFollowPlayer,
            this.mnuFollowTarget,
            this.toolStripSeparator11,
            this.mnuKeepCentered,
            this.mnuAutoExpand,
            this.toolStripSeparator13,
            this.mnuMapReset});
            this.mnuMapSettingsMain.Name = "mnuMapSettingsMain";
            this.mnuMapSettingsMain.Size = new Size(43, 20);
            this.mnuMapSettingsMain.Text = "&Map";
            // 
            // mnuDepthFilter
            // 
            this.mnuDepthFilter.Name = "mnuDepthFilter";
            this.mnuDepthFilter.ShortcutKeys = Keys.F5;
            this.mnuDepthFilter.Size = new Size(195, 22);
            this.mnuDepthFilter.Text = "&Depth Filter On/Off";
            this.mnuDepthFilter.ToolTipText = "Z-Axis Depth Filtering";
            this.mnuDepthFilter.Click += new EventHandler(this.mnuDepthFilter_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.DropDownItems.AddRange(new ToolStripItem[] {
            this.mnuDynamicAlpha,
            this.mnuFilterMapLines,
            this.mnuFilterMapText,
            this.mnuFilterNPCs,
            this.mnuFilterNPCCorpses,
            this.mnuFilterPlayers,
            this.mnuFilterPlayerCorpses,
            this.mnuFilterGroundItems,
            this.mnuFilterSpawnPoints});
            this.menuItem3.Name = "menuItem3";
            this.menuItem3.Size = new Size(195, 22);
            this.menuItem3.Text = "Depth &Filter Settings";
            // 
            // mnuDynamicAlpha
            // 
            this.mnuDynamicAlpha.Name = "mnuDynamicAlpha";
            this.mnuDynamicAlpha.Size = new Size(220, 22);
            this.mnuDynamicAlpha.Text = "Dynamic &Alpha Faded Lines";
            this.mnuDynamicAlpha.ToolTipText = "Faded Depth Filtered Lines.";
            this.mnuDynamicAlpha.Click += new EventHandler(this.mnuDynamicAlpha_Click);
            // 
            // mnuFilterMapLines
            // 
            this.mnuFilterMapLines.Name = "mnuFilterMapLines";
            this.mnuFilterMapLines.Size = new Size(220, 22);
            this.mnuFilterMapLines.Text = "Filter &Map Lines";
            this.mnuFilterMapLines.Click += new EventHandler(this.mnuFilterMapLines_Click);
            // 
            // mnuFilterMapText
            // 
            this.mnuFilterMapText.Name = "mnuFilterMapText";
            this.mnuFilterMapText.Size = new Size(220, 22);
            this.mnuFilterMapText.Text = "Filter Map &Text";
            this.mnuFilterMapText.Click += new EventHandler(this.mnuFilterMapText_Click);
            // 
            // mnuFilterNPCs
            // 
            this.mnuFilterNPCs.Name = "mnuFilterNPCs";
            this.mnuFilterNPCs.Size = new Size(220, 22);
            this.mnuFilterNPCs.Text = "Filter &NPCs";
            this.mnuFilterNPCs.Click += new EventHandler(this.mnuFilterNPCs_Click);
            // 
            // mnuFilterNPCCorpses
            // 
            this.mnuFilterNPCCorpses.Name = "mnuFilterNPCCorpses";
            this.mnuFilterNPCCorpses.Size = new Size(220, 22);
            this.mnuFilterNPCCorpses.Text = "Filter NPC &Corpses";
            this.mnuFilterNPCCorpses.Click += new EventHandler(this.mnuFilterNPCCorpses_Click);
            // 
            // mnuFilterPlayers
            // 
            this.mnuFilterPlayers.Name = "mnuFilterPlayers";
            this.mnuFilterPlayers.Size = new Size(220, 22);
            this.mnuFilterPlayers.Text = "Filter &Players";
            this.mnuFilterPlayers.Click += new EventHandler(this.mnuFilterPlayers_Click);
            // 
            // mnuFilterPlayerCorpses
            // 
            this.mnuFilterPlayerCorpses.Name = "mnuFilterPlayerCorpses";
            this.mnuFilterPlayerCorpses.Size = new Size(220, 22);
            this.mnuFilterPlayerCorpses.Text = "Filter Pl&ayer Corpses";
            this.mnuFilterPlayerCorpses.Click += new EventHandler(this.mnuFilterPlayerCorpses_Click);
            // 
            // mnuFilterGroundItems
            // 
            this.mnuFilterGroundItems.Name = "mnuFilterGroundItems";
            this.mnuFilterGroundItems.Size = new Size(220, 22);
            this.mnuFilterGroundItems.Text = "Filter &Ground Items";
            this.mnuFilterGroundItems.Click += new EventHandler(this.mnuFilterGroundItems_Click);
            // 
            // mnuFilterSpawnPoints
            // 
            this.mnuFilterSpawnPoints.Name = "mnuFilterSpawnPoints";
            this.mnuFilterSpawnPoints.Size = new Size(220, 22);
            this.mnuFilterSpawnPoints.Text = "Filter &Spawn Points";
            this.mnuFilterSpawnPoints.Click += new EventHandler(this.mnuFilterSpawnPoints_Click);
            // 
            // mnuForceDistinct
            // 
            this.mnuForceDistinct.Name = "mnuForceDistinct";
            this.mnuForceDistinct.Size = new Size(195, 22);
            this.mnuForceDistinct.Text = "&Force Distinct Lines";
            this.mnuForceDistinct.Click += new EventHandler(this.mnuForceDistinct_Click);
            // 
            // mnuForceDistinctText
            // 
            this.mnuForceDistinctText.Name = "mnuForceDistinctText";
            this.mnuForceDistinctText.Size = new Size(195, 22);
            this.mnuForceDistinctText.Text = "Force Distinct &Text";
            this.mnuForceDistinctText.Click += new EventHandler(this.mnuForceDistinctText_Click);
            // 
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            this.toolStripSeparator12.Size = new Size(192, 6);
            // 
            // mnuLabelShow
            // 
            this.mnuLabelShow.DropDownItems.AddRange(new ToolStripItem[] {
            this.mnuShowNPCLevels,
            this.mnuShowNPCNames,
            this.mnuShowNPCCorpseNames,
            this.mnuShowPCNames,
            this.mnuShowPlayerCorpseNames,
            this.mnuShowPCGuild,
            this.mnuSpawnCountdown,
            this.mnuShowSpawnPoints,
            this.mnuShowZoneText,
            this.mnuShowLayer1,
            this.mnuShowLayer2,
            this.mnuShowLayer3,
            this.mnuShowPVP,
            this.mnuShowPVPLevel});
            this.mnuLabelShow.Name = "mnuLabelShow";
            this.mnuLabelShow.Size = new Size(195, 22);
            this.mnuLabelShow.Text = "&Show on Map";
            // 
            // mnuShowNPCLevels
            // 
            this.mnuShowNPCLevels.Name = "mnuShowNPCLevels";
            this.mnuShowNPCLevels.Size = new Size(186, 22);
            this.mnuShowNPCLevels.Text = "NPC L&evels";
            this.mnuShowNPCLevels.ToolTipText = "Show NPC Levels on map.";
            this.mnuShowNPCLevels.Click += new EventHandler(this.mnuShowNPCLevels_Click);
            // 
            // mnuShowNPCNames
            // 
            this.mnuShowNPCNames.Name = "mnuShowNPCNames";
            this.mnuShowNPCNames.Size = new Size(186, 22);
            this.mnuShowNPCNames.Text = "&NPC Names";
            this.mnuShowNPCNames.ToolTipText = "Show NPC Names on map.";
            this.mnuShowNPCNames.Click += new EventHandler(this.mnuShowNPCNames_Click);
            // 
            // mnuShowNPCCorpseNames
            // 
            this.mnuShowNPCCorpseNames.Name = "mnuShowNPCCorpseNames";
            this.mnuShowNPCCorpseNames.Size = new Size(186, 22);
            this.mnuShowNPCCorpseNames.Text = "NPC &Corpse Names";
            this.mnuShowNPCCorpseNames.ToolTipText = "Show NPC Corpse Names on map.";
            this.mnuShowNPCCorpseNames.Click += new EventHandler(this.mnuShowNPCCorpseNames_Click);
            // 
            // mnuShowPCNames
            // 
            this.mnuShowPCNames.Name = "mnuShowPCNames";
            this.mnuShowPCNames.Size = new Size(186, 22);
            this.mnuShowPCNames.Text = "&Player Names";
            this.mnuShowPCNames.ToolTipText = "Show Player Names on map.";
            this.mnuShowPCNames.Click += new EventHandler(this.mnuShowPCNames_Click);
            // 
            // mnuShowPlayerCorpseNames
            // 
            this.mnuShowPlayerCorpseNames.Name = "mnuShowPlayerCorpseNames";
            this.mnuShowPlayerCorpseNames.Size = new Size(186, 22);
            this.mnuShowPlayerCorpseNames.Text = "Player Corpse &Names";
            this.mnuShowPlayerCorpseNames.ToolTipText = "Show Player Corpse Names on map.";
            this.mnuShowPlayerCorpseNames.Click += new EventHandler(this.mnuShowPlayerCorpseNames_Click);
            // 
            // mnuShowPCGuild
            // 
            this.mnuShowPCGuild.Name = "mnuShowPCGuild";
            this.mnuShowPCGuild.Size = new Size(186, 22);
            this.mnuShowPCGuild.Text = "&Player Guild";
            this.mnuShowPCGuild.ToolTipText = "Show Player Guild on map.";
            this.mnuShowPCGuild.Click += new EventHandler(this.mnuShowPCGuild_Click);
            // 
            // mnuSpawnCountdown
            // 
            this.mnuSpawnCountdown.Name = "mnuSpawnCountdown";
            this.mnuSpawnCountdown.Size = new Size(186, 22);
            this.mnuSpawnCountdown.Text = "Spawn Countdown";
            this.mnuSpawnCountdown.ToolTipText = "Show spawn countdown timers on map.";
            this.mnuSpawnCountdown.Click += new EventHandler(this.mnuSpawnCountdown_Click);
            // 
            // mnuShowSpawnPoints
            // 
            this.mnuShowSpawnPoints.Name = "mnuShowSpawnPoints";
            this.mnuShowSpawnPoints.Size = new Size(186, 22);
            this.mnuShowSpawnPoints.Text = "&Spawn Points";
            this.mnuShowSpawnPoints.ToolTipText = "Draw a cross at spawn point on map.";
            this.mnuShowSpawnPoints.Click += new EventHandler(this.mnuShowSpawnPoints_Click);
            // 
            // mnuShowZoneText
            // 
            this.mnuShowZoneText.Name = "mnuShowZoneText";
            this.mnuShowZoneText.Size = new Size(186, 22);
            this.mnuShowZoneText.Text = "&Zone Text";
            this.mnuShowZoneText.Click += new EventHandler(this.mnuShowZoneText_Click);
            // 
            // mnuShowLayer1
            // 
            this.mnuShowLayer1.Name = "mnuShowLayer1";
            this.mnuShowLayer1.Size = new Size(186, 22);
            this.mnuShowLayer1.Text = "&Show Layer 1";
            this.mnuShowLayer1.Click += new EventHandler(this.mnuShowLayer1_Click);
            // 
            // mnuShowLayer2
            // 
            this.mnuShowLayer2.Name = "mnuShowLayer2";
            this.mnuShowLayer2.Size = new Size(186, 22);
            this.mnuShowLayer2.Text = "&Show Layer 2";
            this.mnuShowLayer2.Click += new EventHandler(this.mnuShowLayer2_Click);
            // 
            // mnuShowLayer3
            // 
            this.mnuShowLayer3.Name = "mnuShowLayer3";
            this.mnuShowLayer3.Size = new Size(186, 22);
            this.mnuShowLayer3.Text = "&Show Layer 3";
            this.mnuShowLayer3.Click += new EventHandler(this.mnuShowLayer3_Click);
            // 
            // mnuShowPVP
            // 
            this.mnuShowPVP.Name = "mnuShowPVP";
            this.mnuShowPVP.Size = new Size(186, 22);
            this.mnuShowPVP.Text = "P&VP";
            this.mnuShowPVP.Click += new EventHandler(this.mnuShowPVP_Click);
            // 
            // mnuShowPVPLevel
            // 
            this.mnuShowPVPLevel.Name = "mnuShowPVPLevel";
            this.mnuShowPVPLevel.Size = new Size(186, 22);
            this.mnuShowPVPLevel.Text = "PVP &Level";
            this.mnuShowPVPLevel.Click += new EventHandler(this.mnuShowPVPLevel_Click);
            // 
            // mnuCollectMobTrails
            // 
            this.mnuCollectMobTrails.Name = "mnuCollectMobTrails";
            this.mnuCollectMobTrails.Size = new Size(195, 22);
            this.mnuCollectMobTrails.Text = "&Collect Mob Trails";
            this.mnuCollectMobTrails.Click += new EventHandler(this.mnuCollectMobTrails_Click);
            // 
            // mnuShowMobTrails
            // 
            this.mnuShowMobTrails.Name = "mnuShowMobTrails";
            this.mnuShowMobTrails.ShortcutKeys = Keys.F7;
            this.mnuShowMobTrails.Size = new Size(195, 22);
            this.mnuShowMobTrails.Text = "Show &Mob Trails";
            this.mnuShowMobTrails.Click += new EventHandler(this.mnuShowMobTrails_Click);
            // 
            // mnuConColors
            // 
            this.mnuConColors.DropDownItems.AddRange(new ToolStripItem[] {
            this.mnuConDefault,
            this.mnuConSoD,
            this.mnuConSoF});
            this.mnuConColors.Name = "mnuConColors";
            this.mnuConColors.Size = new Size(195, 22);
            this.mnuConColors.Text = "Con Colors";
            // 
            // mnuConDefault
            // 
            this.mnuConDefault.Name = "mnuConDefault";
            this.mnuConDefault.Size = new Size(180, 22);
            this.mnuConDefault.Text = "Default";
            this.mnuConDefault.Click += new EventHandler(this.mnuConDefault_Click);
            // 
            // mnuConSoD
            // 
            this.mnuConSoD.Name = "mnuConSoD";
            this.mnuConSoD.Size = new Size(180, 22);
            this.mnuConSoD.Text = "SoD / Titanium";
            this.mnuConSoD.Click += new EventHandler(this.mnuSodTitanium_Click);
            // 
            // mnuConSoF
            // 
            this.mnuConSoF.Name = "mnuConSoF";
            this.mnuConSoF.Size = new Size(180, 22);
            this.mnuConSoF.Text = "Secrets of Faydwer";
            this.mnuConSoF.Click += new EventHandler(this.mnuConSoF_Click);
            // 
            // mnuGridInterval
            // 
            this.mnuGridInterval.DropDownItems.AddRange(new ToolStripItem[] {
            this.mnuGridInterval100,
            this.mnuGridInterval250,
            this.mnuGridInterval500,
            this.mnuGridInterval1000});
            this.mnuGridInterval.Name = "mnuGridInterval";
            this.mnuGridInterval.Size = new Size(195, 22);
            this.mnuGridInterval.Text = "Grid &Interval";
            // 
            // mnuGridInterval100
            // 
            this.mnuGridInterval100.Name = "mnuGridInterval100";
            this.mnuGridInterval100.Size = new Size(180, 22);
            this.mnuGridInterval100.Text = "100";
            this.mnuGridInterval100.Click += new EventHandler(this.mnuGridInterval_Click);
            // 
            // mnuGridInterval250
            // 
            this.mnuGridInterval250.Name = "mnuGridInterval250";
            this.mnuGridInterval250.Size = new Size(180, 22);
            this.mnuGridInterval250.Text = "250";
            this.mnuGridInterval250.Click += new EventHandler(this.mnuGridInterval_Click);
            // 
            // mnuGridInterval500
            // 
            this.mnuGridInterval500.Checked = true;
            this.mnuGridInterval500.CheckState = CheckState.Checked;
            this.mnuGridInterval500.Name = "mnuGridInterval500";
            this.mnuGridInterval500.Size = new Size(180, 22);
            this.mnuGridInterval500.Text = "500";
            this.mnuGridInterval500.Click += new EventHandler(this.mnuGridInterval_Click);
            // 
            // mnuGridInterval1000
            // 
            this.mnuGridInterval1000.Name = "mnuGridInterval1000";
            this.mnuGridInterval1000.Size = new Size(180, 22);
            this.mnuGridInterval1000.Text = "1000";
            this.mnuGridInterval1000.Click += new EventHandler(this.mnuGridInterval_Click);
            // 
            // mnuShowTargetInfo
            // 
            this.mnuShowTargetInfo.Name = "mnuShowTargetInfo";
            this.mnuShowTargetInfo.ShortcutKeys = Keys.F9;
            this.mnuShowTargetInfo.Size = new Size(195, 22);
            this.mnuShowTargetInfo.Text = "Show &Target Info";
            this.mnuShowTargetInfo.Click += new EventHandler(this.mnuShowTargetInfo_Click);
            // 
            // mnuSmallTargetInfo
            // 
            this.mnuSmallTargetInfo.Name = "mnuSmallTargetInfo";
            this.mnuSmallTargetInfo.Size = new Size(195, 22);
            this.mnuSmallTargetInfo.Text = "Small Target &Info";
            this.mnuSmallTargetInfo.Click += new EventHandler(this.mnuSmallTargetInfo_Click);
            // 
            // mnuAutoSelectEQTarget
            // 
            this.mnuAutoSelectEQTarget.Name = "mnuAutoSelectEQTarget";
            this.mnuAutoSelectEQTarget.Size = new Size(195, 22);
            this.mnuAutoSelectEQTarget.Text = "Auto Select &EQ Target";
            this.mnuAutoSelectEQTarget.Click += new EventHandler(this.mnuAutoSelectEQTarget_Click);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new Size(192, 6);
            // 
            // mnuFollowNone
            // 
            this.mnuFollowNone.ImageScaling = ToolStripItemImageScaling.None;
            this.mnuFollowNone.ImageTransparentColor = Color.Magenta;
            this.mnuFollowNone.Name = "mnuFollowNone";
            this.mnuFollowNone.Size = new Size(195, 22);
            this.mnuFollowNone.Text = "No Follow";
            this.mnuFollowNone.Click += new EventHandler(this.mnuFollowNone_Click);
            // 
            // mnuFollowPlayer
            // 
            this.mnuFollowPlayer.Image = ((Image)(resources.GetObject("mnuFollowPlayer.Image")));
            this.mnuFollowPlayer.ImageScaling = ToolStripItemImageScaling.None;
            this.mnuFollowPlayer.ImageTransparentColor = Color.Magenta;
            this.mnuFollowPlayer.Name = "mnuFollowPlayer";
            this.mnuFollowPlayer.Size = new Size(195, 22);
            this.mnuFollowPlayer.Text = "Follow Player";
            this.mnuFollowPlayer.Click += new EventHandler(this.mnuFollowPlayer_Click);
            // 
            // mnuFollowTarget
            // 
            this.mnuFollowTarget.ImageScaling = ToolStripItemImageScaling.None;
            this.mnuFollowTarget.ImageTransparentColor = Color.Magenta;
            this.mnuFollowTarget.Name = "mnuFollowTarget";
            this.mnuFollowTarget.Size = new Size(195, 22);
            this.mnuFollowTarget.Text = "Follow Target";
            this.mnuFollowTarget.Click += new EventHandler(this.mnuFollowTarget_Click);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new Size(192, 6);
            // 
            // mnuKeepCentered
            // 
            this.mnuKeepCentered.Name = "mnuKeepCentered";
            this.mnuKeepCentered.Size = new Size(195, 22);
            this.mnuKeepCentered.Text = "Keep Centered";
            this.mnuKeepCentered.Click += new EventHandler(this.mnuKeepCentered_Click);
            // 
            // mnuAutoExpand
            // 
            this.mnuAutoExpand.Name = "mnuAutoExpand";
            this.mnuAutoExpand.Size = new Size(195, 22);
            this.mnuAutoExpand.Text = "Auto Expand";
            this.mnuAutoExpand.Click += new EventHandler(this.mnuAutoExpand_Click);
            // 
            // toolStripSeparator13
            // 
            this.toolStripSeparator13.Name = "toolStripSeparator13";
            this.toolStripSeparator13.Size = new Size(192, 6);
            // 
            // mnuMapReset
            // 
            this.mnuMapReset.Name = "mnuMapReset";
            this.mnuMapReset.Size = new Size(195, 22);
            this.mnuMapReset.Text = "Reset Map";
            this.mnuMapReset.Click += new EventHandler(this.mnuMapReset_Click);
            // 
            // mnuHelpMain
            // 
            this.mnuHelpMain.DropDownItems.AddRange(new ToolStripItem[] {
            this.mnuAbout});
            this.mnuHelpMain.Name = "mnuHelpMain";
            this.mnuHelpMain.Size = new Size(44, 20);
            this.mnuHelpMain.Text = "&Help";
            // 
            // mnuAbout
            // 
            this.mnuAbout.Name = "mnuAbout";
            this.mnuAbout.Size = new Size(107, 22);
            this.mnuAbout.Text = "About";
            this.mnuAbout.Click += new EventHandler(this.mnuAbout_Click);
            // 
            // mnuContext
            // 
            this.mnuContext.Items.AddRange(new ToolStripItem[] {
            this.mnuDepthFilter2,
            this.toolStripMenuItem2,
            this.mnuForceDistinct2,
            this.mnuForceDistinctText2,
            this.toolStripSeparator6,
            this.addMapTextToolStripMenuItem,
            this.mnuLabelShow2,
            this.mnuShowTargetInfo2,
            this.mnuSmallTargetInfo2,
            this.mnuAutoSelectEQTarget2,
            this.toolStripSeparator15,
            this.mnuFollowNone2,
            this.mnuFollowPlayer2,
            this.mnuFollowTarget2,
            this.toolStripSeparator16,
            this.mnuKeepCentered2,
            this.mnuAutoExpand2,
            this.toolStripSeparator17,
            this.mnuShowMenuBar,
            this.mnuMapReset2});
            this.mnuContext.Name = "mnuContext";
            this.mnuContext.Size = new Size(196, 380);
            // 
            // mnuDepthFilter2
            // 
            this.mnuDepthFilter2.Name = "mnuDepthFilter2";
            this.mnuDepthFilter2.ShortcutKeys = Keys.F5;
            this.mnuDepthFilter2.Size = new Size(195, 22);
            this.mnuDepthFilter2.Text = "&Depth Filter On/Off";
            this.mnuDepthFilter2.Click += new EventHandler(this.mnuDepthFilter_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.DropDownItems.AddRange(new ToolStripItem[] {
            this.mnuDynamicAlpha2,
            this.mnuFilterMapLines2,
            this.mnuFilterMapText2,
            this.mnuFilterNPCs2,
            this.mnuFilterNPCCorpses2,
            this.mnuFilterPlayers2,
            this.mnuFilterPlayerCorpses2,
            this.mnuFilterGroundItems2,
            this.mnuFilterSpawnPoints2});
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new Size(195, 22);
            this.toolStripMenuItem2.Text = "Depth &Filter Settings";
            // 
            // mnuDynamicAlpha2
            // 
            this.mnuDynamicAlpha2.Name = "mnuDynamicAlpha2";
            this.mnuDynamicAlpha2.Size = new Size(220, 22);
            this.mnuDynamicAlpha2.Text = "Dynamic &Alpha Faded Lines";
            this.mnuDynamicAlpha2.Click += new EventHandler(this.mnuDynamicAlpha_Click);
            // 
            // mnuFilterMapLines2
            // 
            this.mnuFilterMapLines2.Name = "mnuFilterMapLines2";
            this.mnuFilterMapLines2.Size = new Size(220, 22);
            this.mnuFilterMapLines2.Text = "Filter &Map Lines";
            this.mnuFilterMapLines2.Click += new EventHandler(this.mnuFilterMapLines_Click);
            // 
            // mnuFilterMapText2
            // 
            this.mnuFilterMapText2.Name = "mnuFilterMapText2";
            this.mnuFilterMapText2.Size = new Size(220, 22);
            this.mnuFilterMapText2.Text = "Filter Map &Text";
            this.mnuFilterMapText2.Click += new EventHandler(this.mnuFilterMapText_Click);
            // 
            // mnuFilterNPCs2
            // 
            this.mnuFilterNPCs2.Name = "mnuFilterNPCs2";
            this.mnuFilterNPCs2.Size = new Size(220, 22);
            this.mnuFilterNPCs2.Text = "Filter &NPCs";
            this.mnuFilterNPCs2.Click += new EventHandler(this.mnuFilterNPCs_Click);
            // 
            // mnuFilterNPCCorpses2
            // 
            this.mnuFilterNPCCorpses2.Name = "mnuFilterNPCCorpses2";
            this.mnuFilterNPCCorpses2.Size = new Size(220, 22);
            this.mnuFilterNPCCorpses2.Text = "Filter NPC &Corpses";
            this.mnuFilterNPCCorpses2.Click += new EventHandler(this.mnuFilterNPCCorpses_Click);
            // 
            // mnuFilterPlayers2
            // 
            this.mnuFilterPlayers2.Name = "mnuFilterPlayers2";
            this.mnuFilterPlayers2.Size = new Size(220, 22);
            this.mnuFilterPlayers2.Text = "Filter &Players";
            this.mnuFilterPlayers2.Click += new EventHandler(this.mnuFilterPlayers_Click);
            // 
            // mnuFilterPlayerCorpses2
            // 
            this.mnuFilterPlayerCorpses2.Name = "mnuFilterPlayerCorpses2";
            this.mnuFilterPlayerCorpses2.Size = new Size(220, 22);
            this.mnuFilterPlayerCorpses2.Text = "Filter Pl&ayer Corpses";
            this.mnuFilterPlayerCorpses2.Click += new EventHandler(this.mnuFilterPlayerCorpses_Click);
            // 
            // mnuFilterGroundItems2
            // 
            this.mnuFilterGroundItems2.Name = "mnuFilterGroundItems2";
            this.mnuFilterGroundItems2.Size = new Size(220, 22);
            this.mnuFilterGroundItems2.Text = "Filter &Ground Items";
            this.mnuFilterGroundItems2.Click += new EventHandler(this.mnuFilterGroundItems_Click);
            // 
            // mnuFilterSpawnPoints2
            // 
            this.mnuFilterSpawnPoints2.Name = "mnuFilterSpawnPoints2";
            this.mnuFilterSpawnPoints2.Size = new Size(220, 22);
            this.mnuFilterSpawnPoints2.Text = "Filter &Spawn Points";
            this.mnuFilterSpawnPoints2.Click += new EventHandler(this.mnuFilterSpawnPoints_Click);
            // 
            // mnuForceDistinct2
            // 
            this.mnuForceDistinct2.Name = "mnuForceDistinct2";
            this.mnuForceDistinct2.Size = new Size(195, 22);
            this.mnuForceDistinct2.Text = "&Force Distinct Lines";
            this.mnuForceDistinct2.Click += new EventHandler(this.mnuForceDistinct_Click);
            // 
            // mnuForceDistinctText2
            // 
            this.mnuForceDistinctText2.Name = "mnuForceDistinctText2";
            this.mnuForceDistinctText2.Size = new Size(195, 22);
            this.mnuForceDistinctText2.Text = "Force Distinct &Text";
            this.mnuForceDistinctText2.Click += new EventHandler(this.mnuForceDistinctText_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new Size(192, 6);
            // 
            // addMapTextToolStripMenuItem
            // 
            this.addMapTextToolStripMenuItem.Name = "addMapTextToolStripMenuItem";
            this.addMapTextToolStripMenuItem.Size = new Size(195, 22);
            this.addMapTextToolStripMenuItem.Text = "Add Map Text";
            this.addMapTextToolStripMenuItem.ToolTipText = "Add Map Text to your current location.";
            this.addMapTextToolStripMenuItem.Click += new EventHandler(this.addMapTextToolStripMenuItem_Click);
            // 
            // mnuLabelShow2
            // 
            this.mnuLabelShow2.DropDownItems.AddRange(new ToolStripItem[] {
            this.mnuShowNPCLevels2,
            this.mnuShowNPCNames2,
            this.mnuShowNPCCorpseNames2,
            this.mnuShowPCNames2,
            this.mnuShowPlayerCorpseNames2,
            this.mnuShowPCGuild2,
            this.mnuSpawnCountdown2,
            this.mnuShowSpawnPoints2,
            this.mnuShowZoneText2,
            this.mnuShowLayer21,
            this.mnuShowLayer22,
            this.mnuShowLayer23,
            this.mnuShowPVP2,
            this.mnuShowPVPLevel2});
            this.mnuLabelShow2.Name = "mnuLabelShow2";
            this.mnuLabelShow2.Size = new Size(195, 22);
            this.mnuLabelShow2.Text = "&Show on Map";
            // 
            // mnuShowNPCLevels2
            // 
            this.mnuShowNPCLevels2.Name = "mnuShowNPCLevels2";
            this.mnuShowNPCLevels2.Size = new Size(186, 22);
            this.mnuShowNPCLevels2.Text = "NPC L&evels";
            this.mnuShowNPCLevels2.Click += new EventHandler(this.mnuShowNPCLevels_Click);
            // 
            // mnuShowNPCNames2
            // 
            this.mnuShowNPCNames2.Name = "mnuShowNPCNames2";
            this.mnuShowNPCNames2.Size = new Size(186, 22);
            this.mnuShowNPCNames2.Text = "&NPC Names";
            this.mnuShowNPCNames2.Click += new EventHandler(this.mnuShowNPCNames_Click);
            // 
            // mnuShowNPCCorpseNames2
            // 
            this.mnuShowNPCCorpseNames2.Name = "mnuShowNPCCorpseNames2";
            this.mnuShowNPCCorpseNames2.Size = new Size(186, 22);
            this.mnuShowNPCCorpseNames2.Text = "NPC &Corpse Names";
            this.mnuShowNPCCorpseNames2.Click += new EventHandler(this.mnuShowNPCCorpseNames_Click);
            // 
            // mnuShowPCNames2
            // 
            this.mnuShowPCNames2.Name = "mnuShowPCNames2";
            this.mnuShowPCNames2.Size = new Size(186, 22);
            this.mnuShowPCNames2.Text = "&Player Names";
            this.mnuShowPCNames2.Click += new EventHandler(this.mnuShowPCNames_Click);
            // 
            // mnuShowPlayerCorpseNames2
            // 
            this.mnuShowPlayerCorpseNames2.Name = "mnuShowPlayerCorpseNames2";
            this.mnuShowPlayerCorpseNames2.Size = new Size(186, 22);
            this.mnuShowPlayerCorpseNames2.Text = "Player Corpse &Names";
            this.mnuShowPlayerCorpseNames2.Click += new EventHandler(this.mnuShowPlayerCorpseNames_Click);
            // 
            // mnuShowPCGuild2
            // 
            this.mnuShowPCGuild2.Name = "mnuShowPCGuild2";
            this.mnuShowPCGuild2.Size = new Size(186, 22);
            this.mnuShowPCGuild2.Text = "&Player Guild";
            this.mnuShowPCGuild2.Click += new EventHandler(this.mnuShowPCGuild_Click);
            // 
            // mnuSpawnCountdown2
            // 
            this.mnuSpawnCountdown2.Name = "mnuSpawnCountdown2";
            this.mnuSpawnCountdown2.Size = new Size(186, 22);
            this.mnuSpawnCountdown2.Text = "Spawn Countdown";
            this.mnuSpawnCountdown2.Click += new EventHandler(this.mnuSpawnCountdown_Click);
            // 
            // mnuShowSpawnPoints2
            // 
            this.mnuShowSpawnPoints2.Name = "mnuShowSpawnPoints2";
            this.mnuShowSpawnPoints2.Size = new Size(186, 22);
            this.mnuShowSpawnPoints2.Text = "&Spawn Points";
            this.mnuShowSpawnPoints2.Click += new EventHandler(this.mnuShowSpawnPoints_Click);
            // 
            // mnuShowZoneText2
            // 
            this.mnuShowZoneText2.Name = "mnuShowZoneText2";
            this.mnuShowZoneText2.Size = new Size(186, 22);
            this.mnuShowZoneText2.Text = "&Zone Text";
            this.mnuShowZoneText2.Click += new EventHandler(this.mnuShowZoneText_Click);
            // 
            // mnuShowLayer21
            // 
            this.mnuShowLayer21.Name = "mnuShowLayer21";
            this.mnuShowLayer21.Size = new Size(186, 22);
            this.mnuShowLayer21.Text = "&Show Layer 1";
            this.mnuShowLayer21.Click += new EventHandler(this.mnuShowLayer1_Click);
            // 
            // mnuShowLayer22
            // 
            this.mnuShowLayer22.Name = "mnuShowLayer22";
            this.mnuShowLayer22.Size = new Size(186, 22);
            this.mnuShowLayer22.Text = "&Show Layer 2";
            this.mnuShowLayer22.Click += new EventHandler(this.mnuShowLayer2_Click);
            // 
            // mnuShowLayer23
            // 
            this.mnuShowLayer23.Name = "mnuShowLayer23";
            this.mnuShowLayer23.Size = new Size(186, 22);
            this.mnuShowLayer23.Text = "&Show Layer 3";
            this.mnuShowLayer23.Click += new EventHandler(this.mnuShowLayer3_Click);
            // 
            // mnuShowPVP2
            // 
            this.mnuShowPVP2.Name = "mnuShowPVP2";
            this.mnuShowPVP2.Size = new Size(186, 22);
            this.mnuShowPVP2.Text = "P&VP";
            this.mnuShowPVP2.Click += new EventHandler(this.mnuShowPVP_Click);
            // 
            // mnuShowPVPLevel2
            // 
            this.mnuShowPVPLevel2.Name = "mnuShowPVPLevel2";
            this.mnuShowPVPLevel2.Size = new Size(186, 22);
            this.mnuShowPVPLevel2.Text = "PVP &Level";
            this.mnuShowPVPLevel2.Click += new EventHandler(this.mnuShowPVPLevel_Click);
            // 
            // mnuShowTargetInfo2
            // 
            this.mnuShowTargetInfo2.Name = "mnuShowTargetInfo2";
            this.mnuShowTargetInfo2.ShortcutKeys = Keys.F9;
            this.mnuShowTargetInfo2.Size = new Size(195, 22);
            this.mnuShowTargetInfo2.Text = "Show &Target Info";
            this.mnuShowTargetInfo2.Click += new EventHandler(this.mnuShowTargetInfo_Click);
            // 
            // mnuSmallTargetInfo2
            // 
            this.mnuSmallTargetInfo2.Name = "mnuSmallTargetInfo2";
            this.mnuSmallTargetInfo2.Size = new Size(195, 22);
            this.mnuSmallTargetInfo2.Text = "Small Target &Info";
            this.mnuSmallTargetInfo2.Click += new EventHandler(this.mnuSmallTargetInfo_Click);
            // 
            // mnuAutoSelectEQTarget2
            // 
            this.mnuAutoSelectEQTarget2.Name = "mnuAutoSelectEQTarget2";
            this.mnuAutoSelectEQTarget2.Size = new Size(195, 22);
            this.mnuAutoSelectEQTarget2.Text = "Auto Select &EQ Target";
            this.mnuAutoSelectEQTarget2.Click += new EventHandler(this.mnuAutoSelectEQTarget_Click);
            // 
            // toolStripSeparator15
            // 
            this.toolStripSeparator15.Name = "toolStripSeparator15";
            this.toolStripSeparator15.Size = new Size(192, 6);
            // 
            // mnuFollowNone2
            // 
            this.mnuFollowNone2.ImageScaling = ToolStripItemImageScaling.None;
            this.mnuFollowNone2.ImageTransparentColor = Color.Magenta;
            this.mnuFollowNone2.Name = "mnuFollowNone2";
            this.mnuFollowNone2.Size = new Size(195, 22);
            this.mnuFollowNone2.Text = "No Follow";
            this.mnuFollowNone2.Click += new EventHandler(this.mnuFollowNone_Click);
            // 
            // mnuFollowPlayer2
            // 
            this.mnuFollowPlayer2.Image = ((Image)(resources.GetObject("mnuFollowPlayer2.Image")));
            this.mnuFollowPlayer2.ImageScaling = ToolStripItemImageScaling.None;
            this.mnuFollowPlayer2.ImageTransparentColor = Color.Magenta;
            this.mnuFollowPlayer2.Name = "mnuFollowPlayer2";
            this.mnuFollowPlayer2.Size = new Size(195, 22);
            this.mnuFollowPlayer2.Text = "Follow Player";
            this.mnuFollowPlayer2.Click += new EventHandler(this.mnuFollowPlayer_Click);
            // 
            // mnuFollowTarget2
            // 
            this.mnuFollowTarget2.ImageScaling = ToolStripItemImageScaling.None;
            this.mnuFollowTarget2.ImageTransparentColor = Color.Magenta;
            this.mnuFollowTarget2.Name = "mnuFollowTarget2";
            this.mnuFollowTarget2.Size = new Size(195, 22);
            this.mnuFollowTarget2.Text = "Follow Target";
            this.mnuFollowTarget2.Click += new EventHandler(this.mnuFollowTarget_Click);
            // 
            // toolStripSeparator16
            // 
            this.toolStripSeparator16.Name = "toolStripSeparator16";
            this.toolStripSeparator16.Size = new Size(192, 6);
            // 
            // mnuKeepCentered2
            // 
            this.mnuKeepCentered2.Name = "mnuKeepCentered2";
            this.mnuKeepCentered2.Size = new Size(195, 22);
            this.mnuKeepCentered2.Text = "Keep Centered";
            this.mnuKeepCentered2.Click += new EventHandler(this.mnuKeepCentered_Click);
            // 
            // mnuAutoExpand2
            // 
            this.mnuAutoExpand2.Name = "mnuAutoExpand2";
            this.mnuAutoExpand2.Size = new Size(195, 22);
            this.mnuAutoExpand2.Text = "Auto Expand";
            this.mnuAutoExpand2.Click += new EventHandler(this.mnuAutoExpand_Click);
            // 
            // toolStripSeparator17
            // 
            this.toolStripSeparator17.Name = "toolStripSeparator17";
            this.toolStripSeparator17.Size = new Size(192, 6);
            // 
            // mnuShowMenuBar
            // 
            this.mnuShowMenuBar.Name = "mnuShowMenuBar";
            this.mnuShowMenuBar.Size = new Size(195, 22);
            this.mnuShowMenuBar.Text = "Show Menu Bar";
            this.mnuShowMenuBar.Click += new EventHandler(this.mnuShowMenuBar_Click);
            // 
            // mnuMapReset2
            // 
            this.mnuMapReset2.Name = "mnuMapReset2";
            this.mnuMapReset2.Size = new Size(195, 22);
            this.mnuMapReset2.Text = "Reset Map";
            this.mnuMapReset2.Click += new EventHandler(this.mnuMapReset_Click);
            // 
            // mnuContextAddFilter
            // 
            this.mnuContextAddFilter.Items.AddRange(new ToolStripItem[] {
            this.mnuMobName,
            this.menuItem11,
            this.mnuAddHuntFilter,
            this.mnuAddCautionFilter,
            this.mnuAddDangerFilter,
            this.mnuAddAlertFilter,
            this.toolStripBasecon,
            this.mnuSepAddFilter,
            this.mnuAddMapLabel,
            this.toolStripSepAddMapLabel,
            this.mnuSearchAllakhazam});
            this.mnuContextAddFilter.Name = "mnuContextAddFilter";
            this.mnuContextAddFilter.Size = new Size(229, 198);
            // 
            // mnuMobName
            // 
            this.mnuMobName.Enabled = false;
            this.mnuMobName.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            this.mnuMobName.Name = "mnuMobName";
            this.mnuMobName.Size = new Size(228, 22);
            this.mnuMobName.Text = "MobName PlaceHolder";
            // 
            // menuItem11
            // 
            this.menuItem11.Name = "menuItem11";
            this.menuItem11.Size = new Size(225, 6);
            // 
            // mnuAddHuntFilter
            // 
            this.mnuAddHuntFilter.Name = "mnuAddHuntFilter";
            this.mnuAddHuntFilter.Size = new Size(228, 22);
            this.mnuAddHuntFilter.Text = "Add Zone Hunt Alert Filter";
            this.mnuAddHuntFilter.Click += new EventHandler(this.mnuAddHuntFilter_Click);
            // 
            // mnuAddCautionFilter
            // 
            this.mnuAddCautionFilter.Name = "mnuAddCautionFilter";
            this.mnuAddCautionFilter.Size = new Size(228, 22);
            this.mnuAddCautionFilter.Text = "Add Zone Caution Alert Filter";
            this.mnuAddCautionFilter.Click += new EventHandler(this.mnuAddCautionFilter_Click);
            // 
            // mnuAddDangerFilter
            // 
            this.mnuAddDangerFilter.Name = "mnuAddDangerFilter";
            this.mnuAddDangerFilter.Size = new Size(228, 22);
            this.mnuAddDangerFilter.Text = "Add Zone Danger Alert Filter";
            this.mnuAddDangerFilter.Click += new EventHandler(this.mnuAddDangerFilter_Click);
            // 
            // mnuAddAlertFilter
            // 
            this.mnuAddAlertFilter.Name = "mnuAddAlertFilter";
            this.mnuAddAlertFilter.Size = new Size(228, 22);
            this.mnuAddAlertFilter.Text = "Add Zone Rare Alert Filter";
            this.mnuAddAlertFilter.Click += new EventHandler(this.mnuAddAlertFilter_Click);
            // 
            // toolStripBasecon
            // 
            this.toolStripBasecon.CheckOnClick = true;
            this.toolStripBasecon.Font = new Font("Tahoma", 8.400001F, FontStyle.Bold);
            this.toolStripBasecon.Image = Resources.BlackX;
            this.toolStripBasecon.ImageTransparentColor = Color.Magenta;
            this.toolStripBasecon.Name = "toolStripBasecon";
            this.toolStripBasecon.Size = new Size(228, 22);
            this.toolStripBasecon.Text = "Base Con on this Spawn";
            // 
            // mnuSepAddFilter
            // 
            this.mnuSepAddFilter.Name = "mnuSepAddFilter";
            this.mnuSepAddFilter.Size = new Size(225, 6);
            // 
            // mnuAddMapLabel
            // 
            this.mnuAddMapLabel.Name = "mnuAddMapLabel";
            this.mnuAddMapLabel.Size = new Size(228, 22);
            this.mnuAddMapLabel.Text = "Add Map Label";
            this.mnuAddMapLabel.Click += new EventHandler(this.mnuAddMapLabel_Click);
            // 
            // toolStripSepAddMapLabel
            // 
            this.toolStripSepAddMapLabel.Name = "toolStripSepAddMapLabel";
            this.toolStripSepAddMapLabel.Size = new Size(225, 6);
            // 
            // mnuSearchAllakhazam
            // 
            this.mnuSearchAllakhazam.Image = ((Image)(resources.GetObject("mnuSearchAllakhazam.Image")));
            this.mnuSearchAllakhazam.ImageTransparentColor = Color.Magenta;
            this.mnuSearchAllakhazam.Name = "mnuSearchAllakhazam";
            this.mnuSearchAllakhazam.Size = new Size(228, 22);
            this.mnuSearchAllakhazam.Text = "Search Allakhazam";
            this.mnuSearchAllakhazam.Click += new EventHandler(this.mnuSearchAllakhazam_Click);
            // 
            // timPackets
            // 
            this.timPackets.Tick += new EventHandler(this.timPackets_Tick);
            // 
            // timDelayAlerts
            // 
            this.timDelayAlerts.SynchronizingObject = this;
            this.timDelayAlerts.Elapsed += new System.Timers.ElapsedEventHandler(this.timDelayPlay_Tick);
            // 
            // timProcessTimers
            // 
            this.timProcessTimers.Enabled = true;
            this.timProcessTimers.SynchronizingObject = this;
            this.timProcessTimers.Elapsed += new System.Timers.ElapsedEventHandler(this.timProcessTimers_Tick);
            // 
            // mnuShowListNPCs
            // 
            this.mnuShowListNPCs.Name = "mnuShowListNPCs";
            this.mnuShowListNPCs.Size = new Size(32, 19);
            // 
            // mnuShowListCorpses
            // 
            this.mnuShowListCorpses.Name = "mnuShowListCorpses";
            this.mnuShowListCorpses.Size = new Size(32, 19);
            // 
            // mnuShowListPlayers
            // 
            this.mnuShowListPlayers.Name = "mnuShowListPlayers";
            this.mnuShowListPlayers.Size = new Size(32, 19);
            // 
            // mnuShowListInvis
            // 
            this.mnuShowListInvis.Name = "mnuShowListInvis";
            this.mnuShowListInvis.Size = new Size(32, 19);
            // 
            // mnuShowListMounts
            // 
            this.mnuShowListMounts.Name = "mnuShowListMounts";
            this.mnuShowListMounts.Size = new Size(32, 19);
            // 
            // mnuShowListFamiliars
            // 
            this.mnuShowListFamiliars.Name = "mnuShowListFamiliars";
            this.mnuShowListFamiliars.Size = new Size(32, 19);
            // 
            // mnuShowListPets
            // 
            this.mnuShowListPets.Name = "mnuShowListPets";
            this.mnuShowListPets.Size = new Size(32, 19);
            // 
            // statusBarStrip
            // 
            this.statusBarStrip.Items.AddRange(new ToolStripItem[] {
            this.toolStripMouseLocation,
            this.toolStripDistance,
            this.toolStripSpring,
            this.toolStripVersion,
            this.toolStripServerAddress,
            this.toolStripCoPStatus,
            this.toolStripShortName,
            this.toolStripFPS});
            this.statusBarStrip.Location = new Point(0, 507);
            this.statusBarStrip.Name = "statusBarStrip";
            this.statusBarStrip.Size = new Size(1464, 22);
            this.statusBarStrip.TabIndex = 0;
            this.statusBarStrip.Text = "statusStrip1";
            // 
            // toolStripMouseLocation
            // 
            this.toolStripMouseLocation.AutoSize = false;
            this.toolStripMouseLocation.BorderSides = ((ToolStripStatusLabelBorderSides)((((ToolStripStatusLabelBorderSides.Left | ToolStripStatusLabelBorderSides.Top) 
            | ToolStripStatusLabelBorderSides.Right) 
            | ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripMouseLocation.ImageScaling = ToolStripItemImageScaling.None;
            this.toolStripMouseLocation.Name = "toolStripMouseLocation";
            this.toolStripMouseLocation.Size = new Size(150, 17);
            this.toolStripMouseLocation.TextAlign = ContentAlignment.MiddleLeft;
            this.toolStripMouseLocation.ToolTipText = "Mouse Location";
            // 
            // toolStripDistance
            // 
            this.toolStripDistance.AutoSize = false;
            this.toolStripDistance.BorderSides = ((ToolStripStatusLabelBorderSides)((((ToolStripStatusLabelBorderSides.Left | ToolStripStatusLabelBorderSides.Top) 
            | ToolStripStatusLabelBorderSides.Right) 
            | ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripDistance.Name = "toolStripDistance";
            this.toolStripDistance.Size = new Size(100, 17);
            this.toolStripDistance.ToolTipText = "Game Distance from Player to Cursor";
            // 
            // toolStripSpring
            // 
            this.toolStripSpring.BorderSides = ((ToolStripStatusLabelBorderSides)((((ToolStripStatusLabelBorderSides.Left | ToolStripStatusLabelBorderSides.Top) 
            | ToolStripStatusLabelBorderSides.Right) 
            | ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripSpring.Name = "toolStripSpring";
            this.toolStripSpring.Size = new Size(955, 17);
            this.toolStripSpring.Spring = true;
            // 
            // toolStripVersion
            // 
            this.toolStripVersion.AutoSize = false;
            this.toolStripVersion.BorderSides = ((ToolStripStatusLabelBorderSides)((((ToolStripStatusLabelBorderSides.Left | ToolStripStatusLabelBorderSides.Top) 
            | ToolStripStatusLabelBorderSides.Right) 
            | ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripVersion.Name = "toolStripVersion";
            this.toolStripVersion.Size = new Size(60, 17);
            // 
            // toolStripServerAddress
            // 
            this.toolStripServerAddress.AutoSize = false;
            this.toolStripServerAddress.BorderSides = ((ToolStripStatusLabelBorderSides)((((ToolStripStatusLabelBorderSides.Left | ToolStripStatusLabelBorderSides.Top) 
            | ToolStripStatusLabelBorderSides.Right) 
            | ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripServerAddress.Name = "toolStripServerAddress";
            this.toolStripServerAddress.Size = new Size(90, 17);
            // 
            // toolStripCoPStatus
            // 
            this.toolStripCoPStatus.AutoSize = false;
            this.toolStripCoPStatus.BorderSides = ((ToolStripStatusLabelBorderSides)((((ToolStripStatusLabelBorderSides.Left | ToolStripStatusLabelBorderSides.Top) 
            | ToolStripStatusLabelBorderSides.Right) 
            | ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripCoPStatus.Name = "toolStripCoPStatus";
            this.toolStripCoPStatus.Size = new Size(30, 17);
            this.toolStripCoPStatus.Click += new EventHandler(this.ToolStripCoPStatus_Click);
            // 
            // toolStripShortName
            // 
            this.toolStripShortName.BorderSides = ((ToolStripStatusLabelBorderSides)((((ToolStripStatusLabelBorderSides.Left | ToolStripStatusLabelBorderSides.Top) 
            | ToolStripStatusLabelBorderSides.Right) 
            | ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripShortName.Name = "toolStripShortName";
            this.toolStripShortName.Size = new Size(4, 17);
            // 
            // toolStripFPS
            // 
            this.toolStripFPS.AutoSize = false;
            this.toolStripFPS.BorderSides = ((ToolStripStatusLabelBorderSides)((((ToolStripStatusLabelBorderSides.Left | ToolStripStatusLabelBorderSides.Top) 
            | ToolStripStatusLabelBorderSides.Right) 
            | ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripFPS.Name = "toolStripFPS";
            this.toolStripFPS.Size = new Size(60, 17);
            // 
            // toolBarStrip
            // 
            this.toolBarStrip.AutoSize = false;
            this.toolBarStrip.BackColor = SystemColors.ControlLight;
            this.toolBarStrip.BackgroundImage = Resources.toolbar;
            this.toolBarStrip.Items.AddRange(new ToolStripItem[] {
            this.toolStripStartStop,
            this.toolStripLevel,
            this.toolStripSeparator14,
            this.toolStripZoomIn,
            this.toolStripZoomOut,
            this.toolStripScale,
            this.toolStripDepthFilterButton,
            this.toolStripZPosLabel,
            this.toolStripZPos,
            this.toolStripZPosDown,
            this.toolStripZPosUp,
            this.toolStripZOffsetLabel,
            this.toolStripZNeg,
            this.toolStripZNegUp,
            this.toolStripZNegDown,
            this.toolStripResetDepthFilter,
            this.toolStripOptions,
            this.toolStripSeparator19,
            this.toolStripLabel1,
            this.toolStripLookupBox,
            this.toolStripCheckLookup,
            this.toolStripResetLookup,
            this.toolStripLookupBox1,
            this.toolStripCheckLookup1,
            this.toolStripResetLookup1,
            this.toolStripLookupBox2,
            this.toolStripCheckLookup2,
            this.toolStripResetLookup2,
            this.toolStripLookupBox3,
            this.toolStripCheckLookup3,
            this.toolStripResetLookup3,
            this.toolStripLookupBox4,
            this.toolStripCheckLookup4,
            this.toolStripResetLookup4,
            this.toolStripLookupBox5,
            this.toolStripCheckLookup5,
            this.toolStripResetLookup5});
            this.toolBarStrip.Location = new Point(0, 24);
            this.toolBarStrip.Name = "toolBarStrip";
            this.toolBarStrip.Size = new Size(1464, 25);
            this.toolBarStrip.TabIndex = 0;
            this.toolBarStrip.Text = "toolBarStrip";
            // 
            // toolStripStartStop
            // 
            this.toolStripStartStop.Image = ((Image)(resources.GetObject("toolStripStartStop.Image")));
            this.toolStripStartStop.ImageScaling = ToolStripItemImageScaling.None;
            this.toolStripStartStop.ImageTransparentColor = Color.Magenta;
            this.toolStripStartStop.Name = "toolStripStartStop";
            this.toolStripStartStop.Size = new Size(42, 22);
            this.toolStripStartStop.Text = "Go";
            this.toolStripStartStop.ToolTipText = "Connect to Server";
            this.toolStripStartStop.Click += new EventHandler(this.cmdCommand_Click);
            // 
            // toolStripLevel
            // 
            this.toolStripLevel.DropDownHeight = 200;
            this.toolStripLevel.DropDownWidth = 30;
            this.toolStripLevel.IntegralHeight = false;
            this.toolStripLevel.Items.AddRange(new object[] {
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
            this.toolStripLevel.MaxDropDownItems = 80;
            this.toolStripLevel.MaxLength = 4;
            this.toolStripLevel.Name = "toolStripLevel";
            this.toolStripLevel.Size = new Size(75, 25);
            this.toolStripLevel.Text = "Auto";
            this.toolStripLevel.ToolTipText = "Auto or 1-115 to filter mobcolors accordingly";
            this.toolStripLevel.DropDownClosed += new EventHandler(this.toolStripLevel_DropDownClosed);
            this.toolStripLevel.TextUpdate += new EventHandler(this.toolStripLevel_TextUpdate);
            this.toolStripLevel.Leave += new EventHandler(this.toolStripLevel_Leave);
            this.toolStripLevel.KeyPress += new KeyPressEventHandler(this.toolStripLevel_KeyPress);
            // 
            // toolStripSeparator14
            // 
            this.toolStripSeparator14.Name = "toolStripSeparator14";
            this.toolStripSeparator14.Size = new Size(6, 25);
            // 
            // toolStripZoomIn
            // 
            this.toolStripZoomIn.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripZoomIn.Image = ((Image)(resources.GetObject("toolStripZoomIn.Image")));
            this.toolStripZoomIn.ImageScaling = ToolStripItemImageScaling.None;
            this.toolStripZoomIn.ImageTransparentColor = Color.Transparent;
            this.toolStripZoomIn.Name = "toolStripZoomIn";
            this.toolStripZoomIn.Size = new Size(23, 22);
            this.toolStripZoomIn.Text = "toolStripButton2";
            this.toolStripZoomIn.ToolTipText = "Increase Magnification on Map";
            this.toolStripZoomIn.Click += new EventHandler(this.toolStripZoomIn_Click);
            // 
            // toolStripZoomOut
            // 
            this.toolStripZoomOut.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripZoomOut.Image = ((Image)(resources.GetObject("toolStripZoomOut.Image")));
            this.toolStripZoomOut.ImageScaling = ToolStripItemImageScaling.None;
            this.toolStripZoomOut.ImageTransparentColor = Color.Transparent;
            this.toolStripZoomOut.Name = "toolStripZoomOut";
            this.toolStripZoomOut.Size = new Size(23, 22);
            this.toolStripZoomOut.Text = "toolStripButton3";
            this.toolStripZoomOut.ToolTipText = "Decrease Magnification on Map";
            this.toolStripZoomOut.Click += new EventHandler(this.toolStripZoomOut_Click);
            // 
            // toolStripScale
            // 
            this.toolStripScale.BackColor = SystemColors.Window;
            this.toolStripScale.Items.AddRange(new object[] {
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
            this.toolStripScale.Margin = new Padding(0);
            this.toolStripScale.Name = "toolStripScale";
            this.toolStripScale.Size = new Size(75, 25);
            this.toolStripScale.Text = "100%";
            this.toolStripScale.ToolTipText = "Select or Enter a value for amount of map zoom.";
            this.toolStripScale.DropDownClosed += new EventHandler(this.toolStripScale_DropDownClosed);
            this.toolStripScale.TextUpdate += new EventHandler(this.toolStripScale_TextUpdate);
            this.toolStripScale.Leave += new EventHandler(this.toolStripScale_Leave);
            this.toolStripScale.KeyPress += new KeyPressEventHandler(this.toolStripScale_KeyPress);
            // 
            // toolStripDepthFilterButton
            // 
            this.toolStripDepthFilterButton.CheckOnClick = true;
            this.toolStripDepthFilterButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripDepthFilterButton.Image = Resources.ShrinkSpaceHS;
            this.toolStripDepthFilterButton.ImageTransparentColor = Color.White;
            this.toolStripDepthFilterButton.Name = "toolStripDepthFilterButton";
            this.toolStripDepthFilterButton.Size = new Size(23, 22);
            this.toolStripDepthFilterButton.Text = "Depth Filter";
            this.toolStripDepthFilterButton.ToolTipText = "Toggle Depth Filter On/Off";
            this.toolStripDepthFilterButton.Click += new EventHandler(this.mnuDepthFilter_Click);
            // 
            // toolStripZPosLabel
            // 
            this.toolStripZPosLabel.Name = "toolStripZPosLabel";
            this.toolStripZPosLabel.Size = new Size(38, 22);
            this.toolStripZPosLabel.Text = "Z-Pos";
            this.toolStripZPosLabel.ToolTipText = "The range above the player that is not depth filtered.";
            // 
            // toolStripZPos
            // 
            this.toolStripZPos.Font = new Font("Segoe UI", 9F);
            this.toolStripZPos.Margin = new Padding(0);
            this.toolStripZPos.Name = "toolStripZPos";
            this.toolStripZPos.Size = new Size(40, 25);
            this.toolStripZPos.Text = "75";
            this.toolStripZPos.TextBoxTextAlign = HorizontalAlignment.Center;
            this.toolStripZPos.ToolTipText = "Enter a value for Z-Pos between 0 and 3500.";
            this.toolStripZPos.Leave += new EventHandler(this.toolStripZPos_Leave);
            this.toolStripZPos.KeyPress += new KeyPressEventHandler(this.toolStripZPos_KeyPress);
            this.toolStripZPos.TextChanged += new EventHandler(this.toolStripZPos_TextChanged);
            // 
            // toolStripZPosDown
            // 
            this.toolStripZPosDown.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripZPosDown.Image = ((Image)(resources.GetObject("toolStripZPosDown.Image")));
            this.toolStripZPosDown.ImageScaling = ToolStripItemImageScaling.None;
            this.toolStripZPosDown.ImageTransparentColor = Color.Magenta;
            this.toolStripZPosDown.Name = "toolStripZPosDown";
            this.toolStripZPosDown.Size = new Size(23, 22);
            this.toolStripZPosDown.Text = "toolStripButton1";
            this.toolStripZPosDown.ToolTipText = "Decrease Z-Pos above player for depth filter.";
            this.toolStripZPosDown.Click += new EventHandler(this.toolStripZPosDown_Click);
            // 
            // toolStripZPosUp
            // 
            this.toolStripZPosUp.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripZPosUp.Image = ((Image)(resources.GetObject("toolStripZPosUp.Image")));
            this.toolStripZPosUp.ImageScaling = ToolStripItemImageScaling.None;
            this.toolStripZPosUp.ImageTransparentColor = Color.Magenta;
            this.toolStripZPosUp.Name = "toolStripZPosUp";
            this.toolStripZPosUp.Size = new Size(23, 22);
            this.toolStripZPosUp.Text = "toolStripButton4";
            this.toolStripZPosUp.ToolTipText = "Increase Z-Pos above player for depth filter.";
            this.toolStripZPosUp.Click += new EventHandler(this.toolStripZPosUp_Click);
            // 
            // toolStripZOffsetLabel
            // 
            this.toolStripZOffsetLabel.Name = "toolStripZOffsetLabel";
            this.toolStripZOffsetLabel.Size = new Size(41, 22);
            this.toolStripZOffsetLabel.Text = "Z-Neg";
            this.toolStripZOffsetLabel.ToolTipText = "The range below the player that is not depth filtered.";
            // 
            // toolStripZNeg
            // 
            this.toolStripZNeg.Font = new Font("Segoe UI", 9F);
            this.toolStripZNeg.Margin = new Padding(0);
            this.toolStripZNeg.Name = "toolStripZNeg";
            this.toolStripZNeg.Size = new Size(40, 25);
            this.toolStripZNeg.Text = "75";
            this.toolStripZNeg.TextBoxTextAlign = HorizontalAlignment.Center;
            this.toolStripZNeg.ToolTipText = "Enter a value for Z-Neg between 0 and 3500.";
            this.toolStripZNeg.Leave += new EventHandler(this.toolStripZNeg_Leave);
            this.toolStripZNeg.KeyPress += new KeyPressEventHandler(this.toolStripZNeg_KeyPress);
            this.toolStripZNeg.TextChanged += new EventHandler(this.toolStripZNeg_TextChanged);
            // 
            // toolStripZNegUp
            // 
            this.toolStripZNegUp.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripZNegUp.Image = ((Image)(resources.GetObject("toolStripZNegUp.Image")));
            this.toolStripZNegUp.ImageScaling = ToolStripItemImageScaling.None;
            this.toolStripZNegUp.ImageTransparentColor = Color.Magenta;
            this.toolStripZNegUp.Name = "toolStripZNegUp";
            this.toolStripZNegUp.Size = new Size(23, 22);
            this.toolStripZNegUp.Text = "toolStripButton4";
            this.toolStripZNegUp.ToolTipText = "Increase Z-Neg below player for depth filter.";
            this.toolStripZNegUp.Click += new EventHandler(this.toolStripZNegUp_Click);
            // 
            // toolStripZNegDown
            // 
            this.toolStripZNegDown.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripZNegDown.Image = ((Image)(resources.GetObject("toolStripZNegDown.Image")));
            this.toolStripZNegDown.ImageScaling = ToolStripItemImageScaling.None;
            this.toolStripZNegDown.ImageTransparentColor = Color.Magenta;
            this.toolStripZNegDown.Name = "toolStripZNegDown";
            this.toolStripZNegDown.Size = new Size(23, 22);
            this.toolStripZNegDown.Text = "toolStripButton1";
            this.toolStripZNegDown.ToolTipText = "Decrease Z-Neg below player for depth filter.";
            this.toolStripZNegDown.Click += new EventHandler(this.toolStripZNegDown_Click);
            // 
            // toolStripResetDepthFilter
            // 
            this.toolStripResetDepthFilter.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripResetDepthFilter.Image = ((Image)(resources.GetObject("toolStripResetDepthFilter.Image")));
            this.toolStripResetDepthFilter.ImageScaling = ToolStripItemImageScaling.None;
            this.toolStripResetDepthFilter.ImageTransparentColor = Color.Transparent;
            this.toolStripResetDepthFilter.Name = "toolStripResetDepthFilter";
            this.toolStripResetDepthFilter.Size = new Size(23, 22);
            this.toolStripResetDepthFilter.Text = "toolStripResetDepthFilter";
            this.toolStripResetDepthFilter.ToolTipText = "Reset Depth Filter Settings";
            this.toolStripResetDepthFilter.Click += new EventHandler(this.toolStripResetDepthFilter_Click);
            // 
            // toolStripOptions
            // 
            this.toolStripOptions.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripOptions.Image = ((Image)(resources.GetObject("toolStripOptions.Image")));
            this.toolStripOptions.ImageScaling = ToolStripItemImageScaling.None;
            this.toolStripOptions.ImageTransparentColor = Color.Magenta;
            this.toolStripOptions.Name = "toolStripOptions";
            this.toolStripOptions.Size = new Size(23, 22);
            this.toolStripOptions.Text = "Options";
            this.toolStripOptions.ToolTipText = "Open Options Dialog";
            this.toolStripOptions.Click += new EventHandler(this.mnuOptions_Click);
            // 
            // toolStripSeparator19
            // 
            this.toolStripSeparator19.Name = "toolStripSeparator19";
            this.toolStripSeparator19.Size = new Size(6, 25);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new Size(30, 22);
            this.toolStripLabel1.Text = "Find";
            this.toolStripLabel1.ToolTipText = "Find and temporarily mark mobs on map.";
            // 
            // toolStripLookupBox
            // 
            this.toolStripLookupBox.Font = new Font("Segoe UI", 9F);
            this.toolStripLookupBox.ForeColor = SystemColors.GrayText;
            this.toolStripLookupBox.Name = "toolStripLookupBox";
            this.toolStripLookupBox.Size = new Size(75, 25);
            this.toolStripLookupBox.Text = "Mob Search";
            this.toolStripLookupBox.ToolTipText = "Type in mob name and press Enter.";
            this.toolStripLookupBox.Leave += new EventHandler(this.toolStripLookupBox_Leave);
            this.toolStripLookupBox.KeyPress += new KeyPressEventHandler(this.toolStripTextBox_KeyPress);
            this.toolStripLookupBox.Click += new EventHandler(this.toolStripLookupBox_Click);
            // 
            // toolStripCheckLookup
            // 
            this.toolStripCheckLookup.BackColor = Color.Gray;
            this.toolStripCheckLookup.Checked = true;
            this.toolStripCheckLookup.CheckOnClick = true;
            this.toolStripCheckLookup.CheckState = CheckState.Checked;
            this.toolStripCheckLookup.ImageTransparentColor = Color.Magenta;
            this.toolStripCheckLookup.Name = "toolStripCheckLookup";
            this.toolStripCheckLookup.Size = new Size(23, 22);
            this.toolStripCheckLookup.Text = "L";
            this.toolStripCheckLookup.ToolTipText = "Lookup or Filter";
            this.toolStripCheckLookup.CheckedChanged += new EventHandler(this.toolStripCheckLookup_CheckChanged);
            // 
            // toolStripResetLookup
            // 
            this.toolStripResetLookup.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.toolStripResetLookup.ImageTransparentColor = Color.Magenta;
            this.toolStripResetLookup.Name = "toolStripResetLookup";
            this.toolStripResetLookup.Size = new Size(39, 22);
            this.toolStripResetLookup.Text = "Reset";
            this.toolStripResetLookup.ToolTipText = "Reset Find Mob Search String";
            this.toolStripResetLookup.Click += new EventHandler(this.toolStripResetLookup_Click);
            // 
            // toolStripLookupBox1
            // 
            this.toolStripLookupBox1.Font = new Font("Segoe UI", 9F);
            this.toolStripLookupBox1.ForeColor = SystemColors.GrayText;
            this.toolStripLookupBox1.Name = "toolStripLookupBox1";
            this.toolStripLookupBox1.Size = new Size(75, 25);
            this.toolStripLookupBox1.Text = "Mob Search";
            this.toolStripLookupBox1.ToolTipText = "Type in mob name and press Enter.";
            this.toolStripLookupBox1.Leave += new EventHandler(this.toolStripLookupBox1_Leave);
            this.toolStripLookupBox1.KeyPress += new KeyPressEventHandler(this.toolStripTextBox1_KeyPress);
            this.toolStripLookupBox1.Click += new EventHandler(this.toolStripLookupBox1_Click);
            // 
            // toolStripCheckLookup1
            // 
            this.toolStripCheckLookup1.BackColor = Color.Gray;
            this.toolStripCheckLookup1.Checked = true;
            this.toolStripCheckLookup1.CheckOnClick = true;
            this.toolStripCheckLookup1.CheckState = CheckState.Checked;
            this.toolStripCheckLookup1.ImageTransparentColor = Color.Magenta;
            this.toolStripCheckLookup1.Name = "toolStripCheckLookup1";
            this.toolStripCheckLookup1.Size = new Size(23, 22);
            this.toolStripCheckLookup1.Text = "L";
            this.toolStripCheckLookup1.ToolTipText = "Lookup or Filter";
            this.toolStripCheckLookup1.CheckedChanged += new EventHandler(this.toolStripCheckLookup1_CheckChanged);
            // 
            // toolStripResetLookup1
            // 
            this.toolStripResetLookup1.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.toolStripResetLookup1.ImageTransparentColor = Color.Magenta;
            this.toolStripResetLookup1.Name = "toolStripResetLookup1";
            this.toolStripResetLookup1.Size = new Size(39, 22);
            this.toolStripResetLookup1.Text = "Reset";
            this.toolStripResetLookup1.ToolTipText = "Reset Find Mob Search String";
            this.toolStripResetLookup1.Click += new EventHandler(this.toolStripResetLookup1_Click);
            // 
            // toolStripLookupBox2
            // 
            this.toolStripLookupBox2.Font = new Font("Segoe UI", 9F);
            this.toolStripLookupBox2.ForeColor = SystemColors.GrayText;
            this.toolStripLookupBox2.Name = "toolStripLookupBox2";
            this.toolStripLookupBox2.Size = new Size(75, 25);
            this.toolStripLookupBox2.Text = "Mob Search";
            this.toolStripLookupBox2.ToolTipText = "Type in mob name and press Enter.";
            this.toolStripLookupBox2.Leave += new EventHandler(this.toolStripLookupBox2_Leave);
            this.toolStripLookupBox2.KeyPress += new KeyPressEventHandler(this.toolStripTextBox2_KeyPress);
            this.toolStripLookupBox2.Click += new EventHandler(this.toolStripLookupBox2_Click);
            // 
            // toolStripCheckLookup2
            // 
            this.toolStripCheckLookup2.BackColor = Color.Gray;
            this.toolStripCheckLookup2.Checked = true;
            this.toolStripCheckLookup2.CheckOnClick = true;
            this.toolStripCheckLookup2.CheckState = CheckState.Checked;
            this.toolStripCheckLookup2.ImageTransparentColor = Color.Magenta;
            this.toolStripCheckLookup2.Name = "toolStripCheckLookup2";
            this.toolStripCheckLookup2.Size = new Size(23, 22);
            this.toolStripCheckLookup2.Text = "L";
            this.toolStripCheckLookup2.ToolTipText = "Lookup or Filter";
            this.toolStripCheckLookup2.CheckedChanged += new EventHandler(this.toolStripCheckLookup2_CheckChanged);
            // 
            // toolStripResetLookup2
            // 
            this.toolStripResetLookup2.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.toolStripResetLookup2.ImageTransparentColor = Color.Magenta;
            this.toolStripResetLookup2.Name = "toolStripResetLookup2";
            this.toolStripResetLookup2.Size = new Size(39, 22);
            this.toolStripResetLookup2.Text = "Reset";
            this.toolStripResetLookup2.ToolTipText = "Reset Find Mob Search String";
            this.toolStripResetLookup2.Click += new EventHandler(this.toolStripResetLookup2_Click);
            // 
            // toolStripLookupBox3
            // 
            this.toolStripLookupBox3.Font = new Font("Segoe UI", 9F);
            this.toolStripLookupBox3.ForeColor = SystemColors.GrayText;
            this.toolStripLookupBox3.Name = "toolStripLookupBox3";
            this.toolStripLookupBox3.Size = new Size(75, 25);
            this.toolStripLookupBox3.Text = "Mob Search";
            this.toolStripLookupBox3.ToolTipText = "Type in mob name and press Enter.";
            this.toolStripLookupBox3.Leave += new EventHandler(this.toolStripLookupBox3_Leave);
            this.toolStripLookupBox3.KeyPress += new KeyPressEventHandler(this.toolStripTextBox3_KeyPress);
            this.toolStripLookupBox3.Click += new EventHandler(this.toolStripLookupBox3_Click);
            // 
            // toolStripCheckLookup3
            // 
            this.toolStripCheckLookup3.BackColor = Color.Gray;
            this.toolStripCheckLookup3.Checked = true;
            this.toolStripCheckLookup3.CheckOnClick = true;
            this.toolStripCheckLookup3.CheckState = CheckState.Checked;
            this.toolStripCheckLookup3.ImageTransparentColor = Color.Magenta;
            this.toolStripCheckLookup3.Name = "toolStripCheckLookup3";
            this.toolStripCheckLookup3.Size = new Size(23, 22);
            this.toolStripCheckLookup3.Text = "L";
            this.toolStripCheckLookup3.ToolTipText = "Lookup or Filter";
            this.toolStripCheckLookup3.CheckedChanged += new EventHandler(this.toolStripCheckLookup3_CheckChanged);
            // 
            // toolStripResetLookup3
            // 
            this.toolStripResetLookup3.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.toolStripResetLookup3.ImageTransparentColor = Color.Magenta;
            this.toolStripResetLookup3.Name = "toolStripResetLookup3";
            this.toolStripResetLookup3.Size = new Size(39, 22);
            this.toolStripResetLookup3.Text = "Reset";
            this.toolStripResetLookup3.ToolTipText = "Reset Find Mob Search String";
            this.toolStripResetLookup3.Click += new EventHandler(this.toolStripResetLookup3_Click);
            // 
            // toolStripLookupBox4
            // 
            this.toolStripLookupBox4.Font = new Font("Segoe UI", 9F);
            this.toolStripLookupBox4.ForeColor = SystemColors.GrayText;
            this.toolStripLookupBox4.Name = "toolStripLookupBox4";
            this.toolStripLookupBox4.Size = new Size(75, 25);
            this.toolStripLookupBox4.Text = "Mob Search";
            this.toolStripLookupBox4.ToolTipText = "Type in mob name and press Enter.";
            this.toolStripLookupBox4.Leave += new EventHandler(this.toolStripLookupBox4_Leave);
            this.toolStripLookupBox4.KeyPress += new KeyPressEventHandler(this.toolStripTextBox4_KeyPress);
            this.toolStripLookupBox4.Click += new EventHandler(this.toolStripLookupBox4_Click);
            // 
            // toolStripCheckLookup4
            // 
            this.toolStripCheckLookup4.BackColor = Color.Gray;
            this.toolStripCheckLookup4.Checked = true;
            this.toolStripCheckLookup4.CheckOnClick = true;
            this.toolStripCheckLookup4.CheckState = CheckState.Checked;
            this.toolStripCheckLookup4.ImageTransparentColor = Color.Magenta;
            this.toolStripCheckLookup4.Name = "toolStripCheckLookup4";
            this.toolStripCheckLookup4.Size = new Size(23, 22);
            this.toolStripCheckLookup4.Text = "L";
            this.toolStripCheckLookup4.ToolTipText = "Lookup or Filter";
            this.toolStripCheckLookup4.CheckedChanged += new EventHandler(this.toolStripCheckLookup4_CheckChanged);
            // 
            // toolStripResetLookup4
            // 
            this.toolStripResetLookup4.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.toolStripResetLookup4.ImageTransparentColor = Color.Magenta;
            this.toolStripResetLookup4.Name = "toolStripResetLookup4";
            this.toolStripResetLookup4.Size = new Size(39, 22);
            this.toolStripResetLookup4.Text = "Reset";
            this.toolStripResetLookup4.ToolTipText = "Reset Find Mob Search String";
            this.toolStripResetLookup4.Click += new EventHandler(this.toolStripResetLookup4_Click);
            // 
            // toolStripLookupBox5
            // 
            this.toolStripLookupBox5.Font = new Font("Segoe UI", 9F);
            this.toolStripLookupBox5.ForeColor = SystemColors.GrayText;
            this.toolStripLookupBox5.Name = "toolStripLookupBox5";
            this.toolStripLookupBox5.Size = new Size(75, 25);
            this.toolStripLookupBox5.Text = "Mob Search";
            this.toolStripLookupBox5.ToolTipText = "Type in mob name and press Enter.";
            this.toolStripLookupBox5.Leave += new EventHandler(this.toolStripLookupBox5_Leave);
            this.toolStripLookupBox5.KeyPress += new KeyPressEventHandler(this.toolStripTextBox5_KeyPress);
            this.toolStripLookupBox5.Click += new EventHandler(this.toolStripLookupBox5_Click);
            // 
            // toolStripCheckLookup5
            // 
            this.toolStripCheckLookup5.BackColor = Color.Gray;
            this.toolStripCheckLookup5.Checked = true;
            this.toolStripCheckLookup5.CheckOnClick = true;
            this.toolStripCheckLookup5.CheckState = CheckState.Checked;
            this.toolStripCheckLookup5.ImageTransparentColor = Color.Magenta;
            this.toolStripCheckLookup5.Name = "toolStripCheckLookup5";
            this.toolStripCheckLookup5.Size = new Size(23, 22);
            this.toolStripCheckLookup5.Text = "L";
            this.toolStripCheckLookup5.ToolTipText = "Lookup or Filter";
            this.toolStripCheckLookup5.CheckedChanged += new EventHandler(this.toolStripCheckLookup5_CheckChanged);
            // 
            // toolStripResetLookup5
            // 
            this.toolStripResetLookup5.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.toolStripResetLookup5.ImageTransparentColor = Color.Magenta;
            this.toolStripResetLookup5.Name = "toolStripResetLookup5";
            this.toolStripResetLookup5.Size = new Size(39, 22);
            this.toolStripResetLookup5.Text = "Reset";
            this.toolStripResetLookup5.ToolTipText = "Reset Find Mob Search String";
            this.toolStripResetLookup5.Click += new EventHandler(this.toolStripResetLookup5_Click);
            // 
            // dockPanel
            // 
            this.dockPanel.ActiveAutoHideContent = null;
            this.dockPanel.BackColor = SystemColors.ControlLight;
            this.dockPanel.Dock = DockStyle.Fill;
            this.dockPanel.DockBackColor = SystemColors.ControlLight;
            this.dockPanel.Location = new Point(0, 49);
            this.dockPanel.Name = "dockPanel";
            this.dockPanel.Size = new Size(1464, 458);
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
            this.dockPanel.Skin = dockPanelSkin1;
            this.dockPanel.TabIndex = 2;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.AutoValidate = AutoValidate.EnablePreventFocusChange;
            this.BackColor = SystemColors.ControlLight;
            this.ClientSize = new Size(1464, 529);
            this.ContextMenuStrip = this.mnuContext;
            this.Controls.Add(this.dockPanel);
            this.Controls.Add(this.toolBarStrip);
            this.Controls.Add(this.statusBarStrip);
            this.Controls.Add(this.mnuMainMenu);
            this.Icon = ((Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.mnuMainMenu;
            this.Name = "frmMain";
            this.StartPosition = FormStartPosition.Manual;
            this.Text = "frmMain";
            this.Closing += new CancelEventHandler(this.frmMain_Closing);
            this.Move += new EventHandler(this.frmMain_Move);
            this.Resize += new EventHandler(this.frmMain_Resize);
            this.mnuMainMenu.ResumeLayout(false);
            this.mnuMainMenu.PerformLayout();
            this.mnuContext.ResumeLayout(false);
            this.mnuContextAddFilter.ResumeLayout(false);
            ((ISupportInitialize)(this.timDelayAlerts)).EndInit();
            ((ISupportInitialize)(this.timProcessTimers)).EndInit();
            this.statusBarStrip.ResumeLayout(false);
            this.statusBarStrip.PerformLayout();
            this.toolBarStrip.ResumeLayout(false);
            this.toolBarStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        [STAThread]

        static void Main()

        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try {Application.Run(new FrmMain());}
            catch (Exception e)

            {
                string s = $"Uncaught exception in Main(): {e.Message}";

                LogLib.WriteLine(s);

                MessageBox.Show(s);

                Application.Exit();
            }
        }

        private void frmMain_Closing(object sender, CancelEventArgs e)

        {
            if (Settings.Default.SaveOnExit)
            {
//                string mypath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MySEQ");
                savePrefs();
//                SmtpSettings.Default.Save(myseqFile);
            }
        }

        public void cmdCommand_Click(object sender, EventArgs e)

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

            if (eq.playerinfo != null)
                eq.playerinfo.Name = "";

            if (mnuIPAddress1.Checked)
                currentIPAddress = Settings.Default.IPAddress1;
            else if (mnuIPAddress2.Checked)
                currentIPAddress = Settings.Default.IPAddress2;
            else if (mnuIPAddress3.Checked)
                currentIPAddress = Settings.Default.IPAddress3;
            else if (mnuIPAddress4.Checked)
                currentIPAddress = Settings.Default.IPAddress4;
            else if (mnuIPAddress5.Checked)
                currentIPAddress = Settings.Default.IPAddress5;

            if (currentIPAddress.Length == 0)
                return;

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
                Text += " - " + eq.longname;

            if (Settings.Default.ShowCharName && eq.playerinfo?.Name.Length > 1)
                Text += " - " + eq.playerinfo.Name;
        }

        public void LoadPrefs()

        {
            //SetGridInterval();

//            Settings.Default.Load(filename);

            // Always want these off on starting up.

            Settings.Default.CollectMobTrails = false;
//            Settings.Default.EmailAlerts = false;
            Settings.Default.DepthFilter = false;

            SetGridInterval();

            // restore the normal windows state, if we were closed maximized
            if (Settings.Default.WindowState == FormWindowState.Maximized)
            {
                Location = Settings.Default.WindowsLocation;
                //this.Size = Settings.Default.WindowsSize;
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
                BaseTitle = Settings.Default.TitleBar;

            mnuShowSpawnList.Checked = Settings.Default.ShowMobList;

            mnuShowSpawnListTimer.Checked = Settings.Default.ShowMobListTimer;

            mnuShowGroundItemList.Checked = Settings.Default.ShowGroundItemList;

            mnuShowGridLines.Checked = (Settings.Default.DrawOptions & DrawOptions.GridLines) != DrawOptions.DrawNone;

            mnuShowZoneText.Checked = mnuShowZoneText2.Checked = (Settings.Default.DrawOptions & DrawOptions.ZoneText) != DrawOptions.DrawNone;

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

            mnuShowTargetInfo.Checked = mnuShowTargetInfo2.Checked =Settings.Default.ShowTargetInfo;

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

            mnuShowSpawnPoints.Checked = mnuShowSpawnPoints2.Checked = (Settings.Default.DrawOptions & DrawOptions.SpawnTimers) != DrawOptions.DrawNone;

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
                toolStripDepthFilterButton.Image = Resources.ExpandSpaceHS;

            mnuDynamicAlpha.Checked = mnuDynamicAlpha2.Checked = Settings.Default.UseDynamicAlpha;

            mnuForceDistinct.Checked = mnuForceDistinct2.Checked = Settings.Default.ForceDistinct;

            mnuForceDistinctText.Checked = mnuForceDistinctText2.Checked = Settings.Default.ForceDistinctText;

            mnuCollectMobTrails.Checked = Settings.Default.CollectMobTrails;

            mnuShowMobTrails.Checked = (Settings.Default.DrawOptions & DrawOptions.SpawnTrails) != DrawOptions.DrawNone;

            mnuConSoD.Checked = Settings.Default.SoDCon;

            mnuConDefault.Checked = Settings.Default.DefaultCon;

            mnuConSoF.Checked = Settings.Default.SoFCon;

            mnuShowPCNames.Checked = mnuShowPCNames2.Checked = Settings.Default.ShowPCNames;

            mnuShowNPCNames.Checked = mnuShowNPCNames2.Checked = Settings.Default.ShowNPCNames;

            this.mnuShowPCGuild.Checked = this.mnuShowPCGuild2.Checked = Settings.Default.ShowPCGuild;

            this.mnuSaveSpawnLog.Checked = Settings.Default.SaveSpawnLogs;

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
                statusBarStrip.Hide();

            if (!Settings.Default.ShowToolBar)
                toolBarStrip.Hide();

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
                Settings.Default.CurrentIPAddress = 1;

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

            if (Settings.Default.MapDir?.Length == 0 || !Directory.Exists(Settings.Default.MapDir))

            {
                Settings.Default.MapDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "maps");

                if (!Directory.Exists(Settings.Default.MapDir)) Directory.CreateDirectory(Settings.Default.MapDir);
            }

            if (Settings.Default.FilterDir?.Length == 0 || !Directory.Exists(Settings.Default.FilterDir))

            {
                Settings.Default.FilterDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "filters");

                if (!Directory.Exists(Settings.Default.FilterDir)) Directory.CreateDirectory(Settings.Default.FilterDir);
            }

            if (Settings.Default.CfgDir?.Length == 0 || !Directory.Exists(Settings.Default.CfgDir))

            {
                Settings.Default.CfgDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cfg");

                if (!Directory.Exists(Settings.Default.CfgDir)) Directory.CreateDirectory(Settings.Default.CfgDir);
            }

            if (Settings.Default.LogDir?.Length == 0 || !Directory.Exists(Settings.Default.LogDir))

            {
                Settings.Default.LogDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");

                if (!Directory.Exists(Settings.Default.LogDir)) Directory.CreateDirectory(Settings.Default.LogDir);
            }

            if (Settings.Default.TimerDir?.Length == 0 || !Directory.Exists(Settings.Default.TimerDir))

            {
                Settings.Default.TimerDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "timers");

                if (!Directory.Exists(Settings.Default.TimerDir)) Directory.CreateDirectory(Settings.Default.TimerDir);
            }

            DrawOpts = Settings.Default.DrawOptions;

            timProcessTimers.Start();
        }

        public void savePrefs()//string filename
        {
            Settings.Default.Save();

//            string myPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MySEQ");

//            if (!Directory.Exists(myPath))
//                Directory.CreateDirectory(myPath);

//            string strAppDir = AppDomain.CurrentDomain.BaseDirectory;

//            string oldposfile = Path.Combine(strAppDir, "positions.xml");

//            string strPosFile = Path.Combine(myPath, "positions.xml");

//            Settings.Default.c1w = SpawnList.listView.Columns[0].Width;

//            Settings.Default.c2w = SpawnList.listView.Columns[1].Width;

//            Settings.Default.c3w = SpawnList.listView.Columns[2].Width;

//            Settings.Default.c4w = SpawnList.listView.Columns[3].Width;

//            Settings.Default.c5w = SpawnList.listView.Columns[4].Width;

//            Settings.Default.c6w = SpawnList.listView.Columns[5].Width;

//            Settings.Default.c7w = SpawnList.listView.Columns[6].Width;

//            Settings.Default.c8w = SpawnList.listView.Columns[7].Width;

//            Settings.Default.c9w = SpawnList.listView.Columns[8].Width;

//            Settings.Default.c10w = SpawnList.listView.Columns[9].Width;

//            Settings.Default.c11w = SpawnList.listView.Columns[10].Width;

//            Settings.Default.c12w = SpawnList.listView.Columns[11].Width;

//            Settings.Default.c13w = SpawnList.listView.Columns[12].Width;

//            Settings.Default.c14w = SpawnList.listView.Columns[13].Width;

//            if (WindowState == FormWindowState.Minimized)
//                WindowState = FormWindowState.Normal;

//            Settings.Default.WindowState = WindowState;
//            Settings.Default.WindowsPosition = FormStartPosition.Manual;

//            if (WindowState == FormWindowState.Maximized)
//            {
//                Settings.Default.WindowsSize = RestoreBounds.Size;
//                Settings.Default.WindowsLocation = RestoreBounds.Location;
//                Settings.Default.WindowsPosition = FormStartPosition.Manual;
//            }
//            else
//            {
//                Settings.Default.WindowsSize = Size;
//                Settings.Default.WindowsLocation = Location;
//                Rectangle window_bounds = new Rectangle(Location, Size);
//                Rectangle option_bounds = new Rectangle(Settings.Default.OptionsWindowsLocation, Settings.Default.OptionsWindowsSize);
//                bool found_onscreen = false;
//                foreach (Screen screen in Screen.AllScreens)
//                {
//                    if (screen.WorkingArea.IntersectsWith(window_bounds))
//                    {
//                        found_onscreen = true;
//                        break;
//                    }
//                }
//                if (!found_onscreen)
//                {
//                    Settings.Default.WindowsPosition = FormStartPosition.Manual;
//                    Settings.Default.WindowsSize = new Size(800, 600);
//                    Settings.Default.WindowsLocation = new Point(20, 20);
//                }
//                found_onscreen = false;
//                foreach (Screen screen in Screen.AllScreens)
//                {
//                    if (screen.WorkingArea.IntersectsWith(option_bounds))
//                    {
//                        found_onscreen = true;
//                        break;
//                    }
//                }
//                if (!found_onscreen)
//                {
//                    Settings.Default.OptionsWindowsLocation = new Point(20, 20);
//                    Settings.Default.OptionsWindowsSize = new Size(296, 480);
//                }
//            }

////            Settings.Default.Save(filename);
//            dockPanel.SaveAsXml(strPosFile); // positions file

//            // Old positions file - get rid of it, since we saved new one to application data folder
//            if (File.Exists(oldposfile))
//                File.Delete(oldposfile);
        }

        public bool loadmap(string filename)

        {
            try

            {
                if (filename.EndsWith(".map"))

                {
                    if (!map.loadMap(filename))

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

        private void frmMain_Move(object sender, EventArgs e)

        {
            if (WindowState == FormWindowState.Normal)

            {
                Settings.Default.WindowsLocation = Location;
            }
        }

        private void frmMain_Resize(object sender, EventArgs e)

        {
            if (WindowState == FormWindowState.Normal)

            {
                Settings.Default.WindowsSize = Size;
            }

            Settings.Default.WindowState = WindowState;

            mapCon?.ReAdjust();
        }

        public void checkMobs()

        {
            eq.CheckMobs(SpawnList, GroundItemList);
        }

        private void timPackets_Tick(object sender, EventArgs e)

        {
            DrawOpts = Settings.Default.DrawOptions;

            comm.Tick();
            mapCon.Tick();
        }

        private void timDelayPlay_Tick(object sender, EventArgs e)
        {
            // our alert sound/email delay interval has passed.
            eq.EnablePlayAlerts();
            timDelayAlerts.Stop();
        }

        private void timProcessTimers_Tick(object sender, EventArgs e)
        {
            // allow processing timers.
            ProcessSpawnTimer();

            if (!bIsRunning && mapCon != null)
                mapCon.Invalidate();
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
            ProcessInfo PI = new ProcessInfo((int)si.SpawnID, si.Name);

            if (si.SpawnID==0)

            {
                PI.SCharName = "";
                CurrentProcess = PI;
                if (comm != null)
                    comm.newProcessID = 0;
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

                //if (colProcesses.Count == 1)
                //{
                //    mnuChar2.Visible = false;
                //    mnuChar2.Text = "Char 2";
                //    mnuChar2.Checked = false;

                //}
                //else if (colProcesses.Count == 2)
                //{
                //    mnuChar3.Visible = false;
                //    mnuChar3.Text = "Char 3";
                //    mnuChar3.Checked = false;
                //}
                //else if (colProcesses.Count == 3)
                //{
                //    mnuChar4.Visible = false;
                //    mnuChar4.Text = "Char 4";
                //    mnuChar4.Checked = false;
                //}
                //else if (colProcesses.Count == 4)
                //{
                //    mnuChar5.Visible = false;
                //    mnuChar5.Text = "Char 5";
                //    mnuChar5.Checked = false;
                //}
                //else if (colProcesses.Count == 5)
                //{
                //    mnuChar6.Visible = false;
                //    mnuChar6.Text = "Char 6";
                //    mnuChar6.Checked = false;
                //}
                //else if (colProcesses.Count == 6)
                //{
                //    mnuChar7.Visible = false;
                //    mnuChar7.Text = "Char 7";
                //    mnuChar7.Checked = false;
                //}
                //else if (colProcesses.Count == 7)
                //{
                //    mnuChar8.Visible = false;
                //    mnuChar8.Text = "Char 8";
                //    mnuChar8.Checked = false;
                //}
                //else if (colProcesses.Count == 8)
                //{
                //    mnuChar9.Visible = false;
                //    mnuChar9.Text = "Char 9";
                //    mnuChar9.Checked = false;
                //}
                //else if (colProcesses.Count == 9)
                //{
                //    mnuChar10.Visible = false;
                //    mnuChar10.Text = "Char 10";
                //    mnuChar10.Checked = false;
                //}
                //else if (colProcesses.Count == 10)
                //{
                //    mnuChar11.Visible = false;
                //    mnuChar11.Text = "Char 11";
                //    mnuChar11.Checked = false;
                //}
                //else if (colProcesses.Count == 11)
                //{
                //    mnuChar12.Visible = false;
                //    mnuChar12.Text = "Char 12";
                //    mnuChar12.Checked = false;
                //}
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
                    fn = fn.Substring(0, location);

                LogLib.WriteLine($"Using Short Zone Name: ({fn})");

                f += fn;

                toolStripShortName.Text = fn.ToUpper();

                string newzonename = fn.ToUpper().Trim();

                curZone = newzonename;

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
                eq.DisablePlayAlerts();

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
                                    ToggleDepthFilter();
                            }
                        }
                        else
                        {
                            // We dont currently have a setting file, so set depth filter to off
                            if (Settings.Default.DepthFilter)
                                ToggleDepthFilter();
                        }
                    }

                    if (curZone.Length == 0 || curZone == "CLZ" || curZone == "DEFAULT")

                    {
                        if (Settings.Default.DepthFilter)
                            ToggleDepthFilter();

                        eq.Zoning = true;

                        map.loadDummyMap(curZone);

                        //this.Text = BaseTitle;

                        foundmap = true;

                        mapnameWithLabels = "";
                    }

                    // Try it as a SEQ map first   

                    else if (loadmap(f + ".map"))
                    {
                        eq.Zoning = false;
                        foundmap = true;

                        mapnameWithLabels = f + ".map";
                    } else {
                        eq.Zoning = false;
                        // If it didn't work, try an SOE map

                        if (loadmap(f + ".txt"))
                            foundmap = true;

                        if (Settings.Default.ShowLayer1 && loadmap(f + "_1.txt"))
                            foundmap = true;

                        if (Settings.Default.ShowLayer2 && loadmap(f + "_2.txt"))
                            foundmap = true;

                        if (Settings.Default.ShowLayer3 && loadmap(f + "_3.txt"))
                            foundmap = true;

                        // use _3.txt file for map labels
                        if (foundmap)
                            mapnameWithLabels = $"{f}_3.txt";

                        //SetTitle();

                    }
                    //... Missing map

                    if (!foundmap)
                    {
                        map.loadDummyMap(fn);
                    }
                }
                catch (Exception ex)

                {
                    LogLib.WriteLine("Error in ProcessMap() Load Map: ", ex);

                    map.loadDummyMap(fn);
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
                eq.mobsTimers.UpdateList(SpawnTimerList);
        }

        #endregion

        private void SetGridInterval()

        {
            mnuGridInterval100.Checked = false;

            mnuGridInterval250.Checked = false;

            mnuGridInterval500.Checked = false;

            mnuGridInterval1000.Checked = false;

            if (Settings.Default.GridInterval<=100)
                mnuGridInterval100.Checked = true;
            else if (Settings.Default.GridInterval<=250)
                mnuGridInterval250.Checked = true;
            else if (Settings.Default.GridInterval<=500)
                mnuGridInterval500.Checked = true;
            else
                mnuGridInterval1000.Checked = true;
        }

        private int GridInterval()

        {
            if (mnuGridInterval100.Checked)
                return 100;
            else if (mnuGridInterval250.Checked)
                return 250;
            else if (mnuGridInterval500.Checked)
                return 500;
            else
                return 1000;
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
                addMapTextToolStripMenuItem.Enabled = eq.longname.Length > 0 && eq.playerinfo?.Name.Length > 0;
                mnuShowMenuBar.Visible = !Settings.Default.ShowMenuBar;
            }
        }

        private void mnuOpenMap_Click(object sender, EventArgs e)

        {
            openFileDialog.InitialDirectory = Settings.Default.MapDir;

            openFileDialog.Filter = "Map Files (*.map;*.txt)|*.map;*.txt|All Files (*.*)|*.*";

            openFileDialog.FilterIndex = 1;

            if (openFileDialog.ShowDialog() == DialogResult.OK)

            {
                mapnameWithLabels = "";

                string filename = openFileDialog.FileName;

                loadmap(filename);

                int lastSlashIndex = filename.LastIndexOf("\\");

                if (lastSlashIndex > 0)
                    filename = filename.Substring(lastSlashIndex+1);

                filename = filename.Substring(0, filename.Length - 4);

                if (filename.EndsWith("_1"))
                    filename = filename.Substring(0, filename.Length - 2);

                toolStripShortName.Text = filename.ToUpper();

                mapPane.TabText = filename.ToLower();

                curZone = filename.ToUpper();
            }
        }

        private void mnuSaveMobs_Click(object sender, EventArgs e)

        {
            eq.SaveMobs();
        }

        private void mnuSavePrefs_Click(object sender, EventArgs e)

        {
            savePrefs();
        }

        private void mnuExit_Click(object sender, EventArgs e)

        {
            StopListening();

            Close();

            Application.Exit();
        }

        private void mnuOptions_Click(object sender, EventArgs e)

        {
            frmOptions f3 = new frmOptions();
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
            reloadAlertFiles();
            resetMapPens();
            SpawnList.listView.BackColor = Settings.Default.ListBackColor;

            SpawnTimerList.listView.BackColor = Settings.Default.ListBackColor;

            GroundItemList.listView.BackColor = Settings.Default.ListBackColor;

            if (Settings.Default.TitleBar.Length > 0)
                BaseTitle = Settings.Default.TitleBar;

            DrawOpts = Settings.Default.DrawOptions;

            mnuShowGridLines.Checked = (Settings.Default.DrawOptions & DrawOptions.GridLines) != DrawOptions.DrawNone;
            mnuShowZoneText.Checked = (Settings.Default.DrawOptions & DrawOptions.ZoneText) != DrawOptions.DrawNone;
            mnuShowLayer1.Checked = Settings.Default.ShowLayer1;
            mnuShowLayer2.Checked = Settings.Default.ShowLayer2;
            mnuShowLayer3.Checked = Settings.Default.ShowLayer3;
            mnuShowSpawnPoints.Checked = (Settings.Default.DrawOptions & DrawOptions.SpawnTimers) != DrawOptions.DrawNone;

            ServerSelection();

            eq?.LoadSpawnInfo();

            SetTitle();

            savePrefs();

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

        private void mnuRefreshSpawnList_Click(object sender, EventArgs e)

        {
            eq.DisablePlayAlerts();

            eq.mobsTimers.ResetTimers();

            map.ClearMap();

            eq.mobsTimers.LoadTimers();
        }

        private void mnuDepthFilter_Click(object sender, EventArgs e)

        {
            ToggleDepthFilter();
            if (curZone.Length > 0 && curZone != "CLZ" && curZone != "DEFAULT")
            {
                try
                {
                    // Save depth filter settings to file
                    string myPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MySEQ");
                    if (!Directory.Exists(myPath))
                        Directory.CreateDirectory(myPath);
                    string ConfigFile = Path.Combine(myPath, "config.ini");

                    IniFile ConIni = new IniFile(ConfigFile);
                    if (Settings.Default.DepthFilter)
                        ConIni.WriteValue("Zones", curZone, "1");
                    else
                        ConIni.WriteValue("Zones", curZone, "0");
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

        private void mnuDynamicAlpha_Click(object sender, EventArgs e)

        {
            mnuDynamicAlpha.Checked = !mnuDynamicAlpha.Checked;
            mnuDynamicAlpha2.Checked = mnuDynamicAlpha.Checked;

            Settings.Default.UseDynamicAlpha = mnuDynamicAlpha.Checked;
        }

        private void mnuForceDistinct_Click(object sender, EventArgs e)

        {
            mnuForceDistinct.Checked = !mnuForceDistinct.Checked;
            mnuForceDistinct2.Checked = mnuForceDistinct.Checked;

            Settings.Default.ForceDistinct = mnuForceDistinct.Checked;

            resetMapPens();
        }

        private void mnuShowGridLines_Click(object sender, EventArgs e)

        {
            mnuShowGridLines.Checked = !mnuShowGridLines.Checked;

            Settings.Default.DrawOptions = mnuShowGridLines.Checked
                ? Settings.Default.DrawOptions | DrawOptions.GridLines
                : Settings.Default.DrawOptions & (DrawOptions.DrawAll ^ DrawOptions.GridLines);

            DrawOpts = Settings.Default.DrawOptions;

            mapCon?.Invalidate();
        }

        private void mnuShowListGridLines_Click(object sender, EventArgs e)

        {
            mnuShowListGridLines.Checked = !mnuShowListGridLines.Checked;

            Settings.Default.ShowListGridLines = mnuShowListGridLines.Checked;

            SpawnList.listView.GridLines = mnuShowListGridLines.Checked;

            SpawnTimerList.listView.GridLines = mnuShowListGridLines.Checked;

            GroundItemList.listView.GridLines = mnuShowListGridLines.Checked;
        }

        private void mnuGridInterval_Click(object sender, EventArgs e)

        {
            mnuGridInterval100.Checked = sender.Equals(mnuGridInterval100);

            mnuGridInterval250.Checked = sender.Equals(mnuGridInterval250);

            mnuGridInterval500.Checked = sender.Equals(mnuGridInterval500);

            mnuGridInterval1000.Checked = sender.Equals(mnuGridInterval1000);

            Settings.Default.GridInterval = GridInterval();

            mapCon?.Invalidate();
        }

        private void mnuGridColor_Click(object sender, EventArgs e)

        {
            if(colorPicker.ShowDialog() != DialogResult.Cancel)

            {
                if (Settings.Default.GridColor != colorPicker.Color)

                {
                    Settings.Default.GridColor = colorPicker.Color;

                    mapCon?.Invalidate();
                }
            }
        }

        private void mnuBackgroundColor_Click(object sender, EventArgs e)

        {
            if (colorPicker.ShowDialog() != DialogResult.Cancel && colorPicker.Color != Settings.Default.BackColor)

            {
                Settings.Default.BackColor = colorPicker.Color;

                resetMapPens();
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

        private void mnuFollowNone_Click(object sender, EventArgs e)

        {
            SetFollowOption(FollowOption.None);
        }

        private void mnuFollowPlayer_Click(object sender, EventArgs e)

        {
            SetFollowOption(FollowOption.Player);
        }

        private void mnuFollowTarget_Click(object sender, EventArgs e)

        {
            SetFollowOption(FollowOption.Target);
        }

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
        private void mnuReloadAlerts_Click(object sender, EventArgs e)

        {
            if (!bIsRunning)
                return;

            filters.ClearArrays();

            filters.LoadAlerts(curZone);

            timDelayAlerts.Start();

            eq.DisablePlayAlerts();

            eq.mobsTimers.ResetTimers();
            map?.ClearMap();

            eq.mobsTimers.LoadTimers();
        }

        private void mnuAddEditAlerts_Click(object sender, EventArgs e)

        {
            filters.EditAlertFile(curZone);
        }

        private void mnuSpawnListFont_Click(object sender, EventArgs e)

        {
            fontDialog1.Font = SpawnList.listView.Font;

            fontDialog1.ShowApply = true;

            if(fontDialog1.ShowDialog() != DialogResult.Cancel)

            {
                SpawnList.listView.Font = fontDialog1.Font;

                SpawnTimerList.listView.Font = fontDialog1.Font;
                reloadAlertFiles();
            }
        }

        private void mnuCollectMobTrails_Click(object sender, EventArgs e)

        {
            mnuCollectMobTrails.Checked = !mnuCollectMobTrails.Checked;

            Settings.Default.CollectMobTrails = mnuCollectMobTrails.Checked;

            mapCon?.Invalidate();
        }

        private void mnuShowSpawnList_Click(object sender, EventArgs e)

        {
            mnuShowSpawnList.Checked = !mnuShowSpawnList.Checked;

            Settings.Default.ShowMobList = mnuShowSpawnList.Checked;

            if (Settings.Default.ShowMobList)
                SpawnList.Show(dockPanel);
            else
                SpawnList.Hide();
        }

        private void mnuShowSpawnListTimer_Click(object sender, EventArgs e)

        {
            mnuShowSpawnListTimer.Checked = !mnuShowSpawnListTimer.Checked;

            Settings.Default.ShowMobListTimer = mnuShowSpawnListTimer.Checked;

            if (Settings.Default.ShowMobListTimer)
                SpawnTimerList.Show(dockPanel);
            else
                SpawnTimerList.Hide();
        }

        private void mnuShowGroundItemList_Click(object sender, EventArgs e)
        {
            mnuShowGroundItemList.Checked = !mnuShowGroundItemList.Checked;

            Settings.Default.ShowGroundItemList = mnuShowGroundItemList.Checked;

            if (Settings.Default.ShowGroundItemList)
                GroundItemList.Show(dockPanel);
            else
                GroundItemList.Hide();
        }

        private void mnuShowMobTrails_Click(object sender, EventArgs e)

        {
            mnuShowMobTrails.Checked = !mnuShowMobTrails.Checked;

            Settings.Default.DrawOptions = mnuShowMobTrails.Checked
                ? Settings.Default.DrawOptions | DrawOptions.SpawnTrails
                : Settings.Default.DrawOptions & (DrawOptions.DrawAll ^ DrawOptions.SpawnTrails);

            mapCon?.Invalidate();
        }

        private void mnuAbout_Click(object sender, EventArgs e)

        {
            AboutDialog ab = new AboutDialog();
            TopMost = false;
            ab.ShowDialog();
            TopMost = mnuAlwaysOnTop.Checked;
        }

        private void mnuShowTargetInfo_Click(object sender, EventArgs e)

        {
            Settings.Default.ShowTargetInfo = !Settings.Default.ShowTargetInfo;

            mnuShowTargetInfo.Checked = Settings.Default.ShowTargetInfo;
            mnuShowTargetInfo2.Checked = Settings.Default.ShowTargetInfo;

            mapCon?.Invalidate();
        }

        private void mnuListColor_Click(object sender, EventArgs e)

        {
            if(colorPicker.ShowDialog() != DialogResult.Cancel)

            {
                Settings.Default.ListBackColor = colorPicker.Color;

                SpawnList.listView.BackColor = Settings.Default.ListBackColor;

                SpawnTimerList.listView.BackColor = Settings.Default.ListBackColor;

                GroundItemList.listView.BackColor = Settings.Default.ListBackColor;
            }
        }

        private void mnuAutoSelectEQTarget_Click(object sender, EventArgs e)

        {
            Settings.Default.AutoSelectEQTarget = !Settings.Default.AutoSelectEQTarget;

            mnuAutoSelectEQTarget.Checked = Settings.Default.AutoSelectEQTarget;
            mnuAutoSelectEQTarget2.Checked = Settings.Default.AutoSelectEQTarget;
        }

        private void mnuGlobalAlerts_Click(object sender, EventArgs e)

        {
            filters.EditAlertFile("global");
        }

        private void mnuShowNPCs_Click(object sender, EventArgs e)

        {
            mnuShowNPCs.Checked = !mnuShowNPCs.Checked;

            Settings.Default.ShowNPCs = mnuShowNPCs.Checked;

            comm.UpdateHidden();
        }

        private void mnuShowLookupText_Click(object sender, EventArgs e)

        {
            mnuShowLookupText.Checked = !mnuShowLookupText.Checked;

            Settings.Default.ShowLookupText = mnuShowLookupText.Checked;

            comm.UpdateHidden();
        }
        private void mnuAlwaysOnTop_Click(object sender, EventArgs e)

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

        private void mnuShowLookupNumber_Click(object sender, EventArgs e)

        {
            mnuShowLookupNumber.Checked = !mnuShowLookupNumber.Checked;

            Settings.Default.ShowLookupNumber = mnuShowLookupNumber.Checked;

            comm.UpdateHidden();
        }

        private void mnuShowCorpses_Click(object sender, EventArgs e)

        {
            mnuShowCorpses.Checked = !mnuShowCorpses.Checked;

            Settings.Default.ShowCorpses = mnuShowCorpses.Checked;

            comm.UpdateHidden();
        }

        private void mnuShowPlayers_Click(object sender, EventArgs e)

        {
            mnuShowPlayers.Checked = !mnuShowPlayers.Checked;

            Settings.Default.ShowPlayers = mnuShowPlayers.Checked;

            comm.UpdateHidden();
        }

        private void mnuShowInvis_Click(object sender, EventArgs e)

        {
            mnuShowInvis.Checked = !mnuShowInvis.Checked;

            Settings.Default.ShowInvis = mnuShowInvis.Checked;

            comm.UpdateHidden();
        }

        private void mnuShowMounts_Click(object sender, EventArgs e)

        {
            mnuShowMounts.Checked = !mnuShowMounts.Checked;

            Settings.Default.ShowMounts = mnuShowMounts.Checked;

            comm.UpdateHidden();
        }

        private void mnuShowFamiliars_Click(object sender, EventArgs e)

        {
            mnuShowFamiliars.Checked = !mnuShowFamiliars.Checked;

            Settings.Default.ShowFamiliars = mnuShowFamiliars.Checked;

            comm.UpdateHidden();
        }

        private void mnuShowPets_Click(object sender, EventArgs e)

        {
            mnuShowPets.Checked = !mnuShowPets.Checked;

            Settings.Default.ShowPets = mnuShowPets.Checked;

            comm.UpdateHidden();
        }

        private void mnuTargetInfoFont_Click(object sender, EventArgs e)

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

        private void mnuShowSpawnPoints_Click(object sender, EventArgs e)

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

        private void mnuIPAddress1_Click(object sender, EventArgs e)

        {
            ResetMenu(1);

            Restart();
        }

        private void mnuIPAddress2_Click(object sender, EventArgs e)

        {
            ResetMenu(2);

            Restart();
        }

        private void mnuIPAddress3_Click(object sender, EventArgs e)

        {
            ResetMenu(3);

            Restart();
        }

        private void mnuIPAddress4_Click(object sender, EventArgs e)

        {
            ResetMenu(4);

            Restart();
        }

        private void mnuIPAddress5_Click(object sender, EventArgs e)

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
                StopListening();

            StartListening();
        }

        private void mnuCharRefresh_Click(object sender, EventArgs e)

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

        private void mnuChar1_Click(object sender, EventArgs e)
        {
            if (!mnuChar1.Checked && comm.CanSwitchChars())
                SwitchCharacter(1);
        }

        private void mnuChar2_Click(object sender, EventArgs e)
        {
            if (!mnuChar2.Checked && comm.CanSwitchChars())
                SwitchCharacter(2);
        }

        private void mnuChar3_Click(object sender, EventArgs e)

        {
             SwitchCharacter(3);
        }

        private void mnuChar4_Click(object sender, EventArgs e)

        {
             SwitchCharacter(4);
        }

        private void mnuChar5_Click(object sender, EventArgs e)

        {
             SwitchCharacter(5);
        }

        private void mnuChar6_Click(object sender, EventArgs e)
        {
            SwitchCharacter(6);
        }

        private void mnuChar7_Click(object sender, EventArgs e)
        {
            SwitchCharacter(7);
        }

        private void mnuChar8_Click(object sender, EventArgs e)
        {
            SwitchCharacter(8);
        }

        private void mnuChar9_Click(object sender, EventArgs e)
        {
            SwitchCharacter(9);
        }

        private void mnuChar10_Click(object sender, EventArgs e)
        {
            SwitchCharacter(10);
        }

        private void mnuChar11_Click(object sender, EventArgs e)
        {
            SwitchCharacter(11);
        }

        private void mnuChar12_Click(object sender, EventArgs e)
        {
            SwitchCharacter(12);
        }

        private void mnuKeepCentered_Click(object sender, EventArgs e)

        {
            Settings.Default.KeepCentered = !Settings.Default.KeepCentered;

            mnuKeepCentered.Checked = Settings.Default.KeepCentered;
            mnuKeepCentered2.Checked = Settings.Default.KeepCentered;
        }

        public void ReAdjust()
        {
            mapCon?.ReAdjust();
        }

        public void reloadAlertFiles()

        {
            filters.ClearArrays();

            filters.LoadAlerts(curZone);

            timDelayAlerts.Start();

            eq.DisablePlayAlerts();

            eq.mobsTimers.ResetTimers();

            map.ClearMap();

            eq.mobsTimers.LoadTimers();

            if (toolStripLookupBox.Text.Length > 0 && toolStripLookupBox.Text != "Mob Search")
                eq.MarkLookups("0:" + toolStripLookupBox.Text, bFilter0);

            if (toolStripLookupBox1.Text.Length > 0 && toolStripLookupBox1.Text != "Mob Search")
                eq.MarkLookups("1:" + toolStripLookupBox1.Text, bFilter1);

            if (toolStripLookupBox2.Text.Length > 0 && toolStripLookupBox2.Text != "Mob Search")
                eq.MarkLookups("2:" + toolStripLookupBox2.Text, bFilter2);

            if (toolStripLookupBox3.Text.Length > 0 && toolStripLookupBox2.Text != "Mob Search")
                eq.MarkLookups("3:" + toolStripLookupBox3.Text, bFilter3);

            if (toolStripLookupBox4.Text.Length > 0 && toolStripLookupBox2.Text != "Mob Search")
                eq.MarkLookups("4:" + toolStripLookupBox4.Text, bFilter4);

            if (toolStripLookupBox5.Text.Length > 0 && toolStripLookupBox2.Text != "Mob Search")
                eq.MarkLookups("5:" + toolStripLookupBox5.Text, bFilter5);
        }

        public void resetMapPens()
        {
            eq.CalculateMapLinePens();

            if (mapCon != null)

            {
                mapCon.SetDistinctPens();

                mapCon.Invalidate();
            }
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

                    eq.ProcessPlayer(si, this);

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

                    if (eq.playerinfo?.Name.Length > 1)
                        SetCharSelection(eq.playerinfo.Name);

                    break;

                default:

                    Text = "Unknown Packet Type: " + si.flags.ToString();

                    break;
            }
        }

        private void mnuMapLabelsFont_Click(object sender, EventArgs e)

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

        private void mnuClearSavedTimers_Click(object sender, EventArgs e)

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

        private void mnuGridLabelColor_Click(object sender, EventArgs e)

        {
            if (colorPicker.ShowDialog() != DialogResult.Cancel)

            {
                Settings.Default.GridLabelColor = colorPicker.Color;

                mapCon?.Invalidate();
            }
        }

        public void addMapText(string textToAdd)
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
                            try{
                            File.AppendAllText(mapnameWithLabels, soe_maptext);
                                }
                            catch (ArgumentNullException accVexc)
                            {
                                MessageBox.Show($"Access Violation {accVexc}", "Error");
                                    }
                        }
                        else
                            eq.DeleteMapText(work);
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
                            File.AppendAllText(mapnameWithLabels, seq_maptext);
                        else
                            eq.DeleteMapText(work);
                    }
                }
            }
        }

        public bool dialogBox(string titleText, string labelText, string dialogText)

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

        //private void mnuSaveAlerts_Click(object sender, EventArgs e)

        //{
        //    filters.WriteAlertFile(curZone);

        //    filters.WriteAlertFile("global");
        //}

        private void mnuShowZoneText_Click(object sender, EventArgs e)

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

        private void mnuMapReset_Click(object sender, EventArgs e)

        {
            mapCon.MapReset();
        }

        private void mnuShowLayer1_Click(object sender, EventArgs e)

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

            //string f = Settings.Default.MapDir + "\\" + eq.shortname;
            //bool fm = false;
            //if (loadmap(f + ".txt"))
            //    fm = true;

            //if (Settings.Default.ShowLayer1 && loadmap(f + "_1.txt"))
            //    fm = true;

            //if (Settings.Default.ShowLayer2 && loadmap(f + "_2.txt"))
            //    fm = true;

            //if (Settings.Default.ShowLayer3 && loadmap(f + "_3.txt"))
            //    fm = true;
        }

        private void mnuShowLayer2_Click(object sender, EventArgs e)

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

            //string f = Settings.Default.MapDir + "\\" + eq.shortname;
            //bool fm = false;
            //if (loadmap(f + ".txt"))
            //    fm = true;

            //if (Settings.Default.ShowLayer1 && loadmap(f + "_1.txt"))
            //    fm = true;

            //if (Settings.Default.ShowLayer2 && loadmap(f + "_2.txt"))
            //    fm = true;

            //if (Settings.Default.ShowLayer3 && loadmap(f + "_3.txt"))
            //    fm = true;
        }

        private void mnuShowLayer3_Click(object sender, EventArgs e)

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

            //string f = Settings.Default.MapDir + "\\" + eq.shortname;
            //bool fm = false;
            //if (loadmap(f + ".txt"))
            //    fm = true;

            //if (Settings.Default.ShowLayer1 && loadmap(f + "_1.txt"))
            //    fm = true;

            //if (Settings.Default.ShowLayer2 && loadmap(f + "_2.txt"))
            //    fm = true;

            //if (Settings.Default.ShowLayer3 && loadmap(f + "_3.txt"))
            //    fm = true;
        }

        private void mnuSodTitanium_Click(object sender, EventArgs e)

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

        private void mnuConDefault_Click(object sender, EventArgs e)

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

        private void mnuConSoF_Click(object sender, EventArgs e)

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

        private void mnuShowPCNames_Click(object sender, EventArgs e)

        {
            Settings.Default.ShowPCNames = !Settings.Default.ShowPCNames;

            mnuShowPCNames.Checked = Settings.Default.ShowPCNames;
            mnuShowPCNames2.Checked = Settings.Default.ShowPCNames;
        }

        private void mnuShowNPCNames_Click(object sender, EventArgs e)

        {
            Settings.Default.ShowNPCNames = !Settings.Default.ShowNPCNames;

            mnuShowNPCNames.Checked = Settings.Default.ShowNPCNames;
            mnuShowNPCNames2.Checked = Settings.Default.ShowNPCNames;
        }

        private void mnuShowNPCCorpseNames_Click(object sender, EventArgs e)

        {
            Settings.Default.ShowNPCCorpseNames = !Settings.Default.ShowNPCCorpseNames;

            mnuShowNPCCorpseNames.Checked = Settings.Default.ShowNPCCorpseNames;
            mnuShowNPCCorpseNames2.Checked = Settings.Default.ShowNPCCorpseNames;
        }

        private void mnuShowPlayerCorpseNames_Click(object sender, EventArgs e)

        {
            Settings.Default.ShowPlayerCorpseNames = !Settings.Default.ShowPlayerCorpseNames;

            mnuShowPlayerCorpseNames.Checked = Settings.Default.ShowPlayerCorpseNames;
            mnuShowPlayerCorpseNames2.Checked = Settings.Default.ShowPlayerCorpseNames;
        }

        private void mnuShowPCGuild_Click(object sender, EventArgs e)

        {
            Settings.Default.ShowPCGuild = !Settings.Default.ShowPCGuild;

            mnuShowPCGuild.Checked = Settings.Default.ShowPCGuild;
            mnuShowPCGuild2.Checked = Settings.Default.ShowPCGuild;
        }

        private void mnuFilterMapLines_Click(object sender, EventArgs e)

        {
            Settings.Default.FilterMapLines = !Settings.Default.FilterMapLines;

            mnuFilterMapLines.Checked = Settings.Default.FilterMapLines;
            mnuFilterMapLines2.Checked = Settings.Default.FilterMapLines;
        }

        private void mnuFilterMapText_Click(object sender, EventArgs e)
        {
            Settings.Default.FilterMapText = !Settings.Default.FilterMapText;

            mnuFilterMapText.Checked = Settings.Default.FilterMapText;
            mnuFilterMapText2.Checked = Settings.Default.FilterMapText;
        }

        private void mnuFilterNPCs_Click(object sender, EventArgs e)

        {
            Settings.Default.FilterNPCs = !Settings.Default.FilterNPCs;

            mnuFilterNPCs.Checked = Settings.Default.FilterNPCs;
            mnuFilterNPCs2.Checked = Settings.Default.FilterNPCs;
        }

        private void mnuFilterPlayers_Click(object sender, EventArgs e)

        {
            Settings.Default.FilterPlayers = !Settings.Default.FilterPlayers;

            mnuFilterPlayers.Checked = Settings.Default.FilterPlayers;
            mnuFilterPlayers2.Checked = Settings.Default.FilterPlayers;
        }

        private void mnuFilterSpawnPoints_Click(object sender, EventArgs e)

        {
            Settings.Default.FilterSpawnPoints = !Settings.Default.FilterSpawnPoints;

            mnuFilterSpawnPoints.Checked = Settings.Default.FilterSpawnPoints;
            mnuFilterSpawnPoints2.Checked = Settings.Default.FilterSpawnPoints;
        }

        private void mnuFilterPlayerCorpses_Click(object sender, EventArgs e)

        {
            Settings.Default.FilterPlayerCorpses = !Settings.Default.FilterPlayerCorpses;

            mnuFilterPlayerCorpses.Checked = Settings.Default.FilterPlayerCorpses;
            mnuFilterPlayerCorpses2.Checked = Settings.Default.FilterPlayerCorpses;
        }

        private void mnuFilterGroundItems_Click(object sender, EventArgs e)
        {
            Settings.Default.FilterGroundItems = !Settings.Default.FilterGroundItems;

            mnuFilterGroundItems.Checked = Settings.Default.FilterGroundItems;
            mnuFilterGroundItems2.Checked = Settings.Default.FilterGroundItems;
        }

        private void mnuFilterNPCCorpses_Click(object sender, EventArgs e)

        {
            Settings.Default.FilterNPCCorpses = !Settings.Default.FilterNPCCorpses;

            mnuFilterNPCCorpses.Checked = Settings.Default.FilterNPCCorpses;
            mnuFilterNPCCorpses2.Checked = Settings.Default.FilterNPCCorpses;
        }

        private void mnuShowPVP_Click(object sender, EventArgs e)

        {
            Settings.Default.ShowPVP = !Settings.Default.ShowPVP;

            mnuShowPVP.Checked = Settings.Default.ShowPVP;
            mnuShowPVP2.Checked = Settings.Default.ShowPVP;
        }

        private void mnuShowPVPLevel_Click(object sender, EventArgs e)

        {
            Settings.Default.ShowPVPLevel = !Settings.Default.ShowPVPLevel;

            mnuShowPVPLevel.Checked = Settings.Default.ShowPVPLevel;
            mnuShowPVPLevel2.Checked = Settings.Default.ShowPVPLevel;
        }

        private void mnuShowNPCLevels_Click(object sender, EventArgs e)

        {
            Settings.Default.ShowNPCLevels = !Settings.Default.ShowNPCLevels;

            mnuShowNPCLevels.Checked = Settings.Default.ShowNPCLevels;
            mnuShowNPCLevels2.Checked = Settings.Default.ShowNPCLevels;
        }

        private void mnuAutoExpand_Click(object sender, EventArgs e)

        {
            Settings.Default.AutoExpand = !Settings.Default.AutoExpand;

            mnuAutoExpand.Checked = Settings.Default.AutoExpand;
            mnuAutoExpand2.Checked = Settings.Default.AutoExpand;
        }

        private void mnuSaveSpawnLog_Click(object sender, EventArgs e)

        {
            mnuSaveSpawnLog.Checked = !mnuSaveSpawnLog.Checked;

            Settings.Default.SaveSpawnLogs = mnuSaveSpawnLog.Checked;
        }

        private void mnuSpawnCountdown_Click(object sender, EventArgs e)

        {
            Settings.Default.SpawnCountdown = !Settings.Default.SpawnCountdown;

            mnuSpawnCountdown.Checked = Settings.Default.SpawnCountdown;
            mnuSpawnCountdown2.Checked = Settings.Default.SpawnCountdown;
        }

        private void mnuShowPCCorpses_Click(object sender, EventArgs e)

        {
            mnuShowPCCorpses.Checked = !mnuShowPCCorpses.Checked;

            Settings.Default.ShowPCCorpses = mnuShowPCCorpses.Checked;

            comm.UpdateHidden();
        }

        private void mnuShowMyCorpse_Click(object sender, EventArgs e)

        {
            mnuShowMyCorpse.Checked = !mnuShowMyCorpse.Checked;

            Settings.Default.ShowMyCorpse = mnuShowMyCorpse.Checked;

            comm.UpdateHidden();
        }

        private void mnuForceDistinctText_Click(object sender, EventArgs e)
        {
            Settings.Default.ForceDistinctText = !Settings.Default.ForceDistinctText;

            mnuForceDistinctText.Checked = Settings.Default.ForceDistinctText;
            mnuForceDistinctText2.Checked = Settings.Default.ForceDistinctText;

            resetMapPens();
        }
        #region filters

        private void mnuAddHuntFilter_Click(object sender, EventArgs e)
        {
            if (dialogBox("Add to Zone Hunt Filters", "Add name to Hunt list:", alertAddmobname))
            {
                filters.AddToAlerts(filters.Hunt, alertAddmobname);

                filters.WriteAlertFile(curZone);

                reloadAlertFiles();
            }
        }

        private void mnuAddCautionFilter_Click(object sender, EventArgs e)
        {
            if (dialogBox("Add to Zone Caution Filters", "Add name to Caution list:", alertAddmobname))
            {
                filters.AddToAlerts(filters.Caution, alertAddmobname);

                filters.WriteAlertFile(curZone);

                reloadAlertFiles();
            }
        }

        private void mnuAddDangerFilter_Click(object sender, EventArgs e)
        {
            if (dialogBox("Add to Zone Danger Alert Filters", "Add name to Danger list:", alertAddmobname))
            {
                filters.AddToAlerts(filters.Danger, alertAddmobname);

                filters.WriteAlertFile(curZone);

                reloadAlertFiles();
            }
        }

        private void mnuAddAlertFilter_Click(object sender, EventArgs e)
        {
            if (dialogBox("Add to Zone Rare Alert Filters", "Add name to Rare list:", alertAddmobname))
            {
                filters.AddToAlerts(filters.Alert, alertAddmobname);

                filters.WriteAlertFile(curZone);

                reloadAlertFiles();
            }
        }
#endregion
        private void mnuSearchAllakhazam_Click(object sender, EventArgs e)
        {
            var searchname = RegexHelper.SearchName(alertAddmobname);

            if (searchname.Length > 0)
            {
                var searchURL = string.Format(Settings.Default.SearchString, searchname);

                Process.Start(searchURL);
            }
        }

        private void mnuShowMenuBar_Click(object sender, EventArgs e)
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

        private void mnuViewStatusBar_Click(object sender, EventArgs e)
        {
            Settings.Default.ShowStatusBar = !Settings.Default.ShowStatusBar;

            mnuViewStatusBar.Checked = Settings.Default.ShowStatusBar;
            if (Settings.Default.ShowStatusBar)
                statusBarStrip.Show();
            else
                statusBarStrip.Hide();
        }

        #region depth filter
        private void mnuViewDepthFilterToolBar_Click(object sender, EventArgs e)
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

        private void toolStripZPos_TextChanged(object sender, EventArgs e)
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
                    validnum = true;
            }
            if (!validnum)
            {
                toolStripZPos.Text = $"{mapPane.filterzpos.Value}";
                MessageBox.Show("Enter a number between 0 and 3500", "Invalid Z-Pos Value Entered.");
            }
        }

        private void toolStripZPosUp_Click(object sender, EventArgs e)
        {
            decimal current_val = mapPane.filterzpos.Value;
            current_val += 5;
            if (current_val > mapPane.filterzpos.Maximum)
                current_val = mapPane.filterzpos.Maximum;
            mapPane.filterzpos.Value = current_val;
            toolStripZPos.Text = $"{current_val}";
        }

        private void toolStripZPosDown_Click(object sender, EventArgs e)
        {
            decimal current_val = mapPane.filterzpos.Value;
            current_val -= 5;
            if (current_val < mapPane.filterzpos.Minimum)
                current_val = mapPane.filterzpos.Minimum;
            mapPane.filterzpos.Value = current_val;
            toolStripZPos.Text = $"{current_val}";
        }

        private void toolStripZNegDown_Click(object sender, EventArgs e)
        {
            decimal current_val = mapPane.filterzneg.Value;
            current_val -= 5;
            if (current_val < mapPane.filterzneg.Minimum)
                current_val = mapPane.filterzneg.Minimum;
            mapPane.filterzneg.Value = current_val;
            toolStripZNeg.Text = $"{current_val}";
        }

        private void toolStripZNegUp_Click(object sender, EventArgs e)
        {
            decimal current_val = mapPane.filterzneg.Value;
            current_val += 5;
            if (current_val > mapPane.filterzneg.Maximum)
                current_val = mapPane.filterzneg.Maximum;
            mapPane.filterzneg.Value = current_val;
            toolStripZNeg.Text = $"{current_val}";
        }

        private void toolStripResetDepthFilter_Click(object sender, EventArgs e)
        {
            mapPane.filterzneg.Value = 75;
            mapPane.filterzpos.Value = 75;
            toolStripZNeg.Text = $"{75}";
            toolStripZPos.Text = $"{75}";
        }

        private void toolStripZPos_Leave(object sender, EventArgs e)
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
                toolStripZPos.Text = $"{mapPane.filterzpos.Value}";
        }

        private void toolStripZNeg_TextChanged(object sender, EventArgs e)
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
                    validnum = true;
            }
            if (!validnum)
            {
                toolStripZNeg.Text = $"{mapPane.filterzneg.Value}";
                MessageBox.Show("Enter a number between 0 and 3500", "Invalid Z-Neg Value Entered.");
            }
        }

        private void toolStripZNeg_Leave(object sender, EventArgs e)
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
                toolStripZNeg.Text = $"{mapPane.filterzneg.Value}";
        }
        private void toolStripZPos_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                mapCon.Focus();
                e.Handled = true;
            }
        }

        private void toolStripZNeg_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                mapCon.Focus();
                e.Handled = true;
            }
        }
        #endregion

        private void mnuAddMapLabel_Click(object sender, EventArgs e)
        {
            addMapText(alertAddmobname);
        }

        #region zoom
        private void toolStripZoomIn_Click(object sender, EventArgs e)
        {
            decimal current_val = mapPane.scale.Value;
            if (current_val < 100) {
                current_val += 10;
                if (current_val > 100)
                    current_val = 100;
            } else if (current_val < 200) {
                current_val += 25;
                if (current_val > 200)
                    current_val = 200;
            } else if (current_val < 300) {
                current_val += 25;
                if (current_val > 300)
                    current_val = 300;
            } else if (current_val < 500) {
                current_val += 50;
                if (current_val > 500)
                    current_val = 500;
            } else {
                current_val += 100;
            }

            if (current_val >= mapPane.scale.Minimum && current_val <= mapPane.scale.Maximum)
                mapPane.scale.Value = current_val;
        }

        private void toolStripZoomOut_Click(object sender, EventArgs e)
        {
            decimal current_val = mapPane.scale.Value;
            if (current_val <= 100)
            {
                current_val -= 10;
                if (current_val < 10)
                    current_val = 10;
            }
            else if (current_val <= 200)
            {
                current_val -= 25;
                if (current_val < 100)
                    current_val = 100;
            }
            else if (current_val <= 300)
            {
                current_val -= 25;
                if (current_val <= 200)
                    current_val = 200;
            }
            else if (current_val <= 400)
            {
                current_val -= 25;
                if (current_val < 300)
                    current_val = 300;
            }
            else if (current_val <= 500)
            {
                current_val -= 25;
                if (current_val < 400)
                    current_val = 400;
            }
            else
            {
                current_val -= 100;
            }

            if (current_val >= mapPane.scale.Minimum && current_val <= mapPane.scale.Maximum)
                mapPane.scale.Value = current_val;
        }
        private void toolStripScale_TextUpdate(object sender, EventArgs e)
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
        private void toolStripScale_Leave(object sender, EventArgs e)
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
        private void toolStripScale_KeyPress(object sender, KeyPressEventArgs e)
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
        private void toolStripScale_DropDownClosed(object sender, EventArgs e)
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
                update_steps = 3;

            int update_ticks = 250 / Settings.Default.UpdateDelay;
            if (update_ticks < 1)
                update_ticks = 1;

            mapCon.UpdateSteps = update_steps;
            mapCon.UpdateTicks = update_ticks;
        }

        private void addMapTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // add map text, to where the player is currently located
            if (eq.longname.Length > 0)
            {
                if (eq.playerinfo?.Name.Length > 0)
                {
                    alertX = eq.playerinfo.X;
                    alertY = eq.playerinfo.Y;
                    alertZ = eq.playerinfo.Z;
                    addMapText("Text to Add");
                }
            }
        }
        private IDockContent GetContentFromPersistString(string persistString)
        {
            if (persistString == "SpawnList")
                return SpawnList;
            else if (persistString == "SpawnTimerList")
                return SpawnTimerList;
            else if (persistString == "GroundSpawnList")
                return GroundItemList;
            else
                return mapPane;
        }

        private void mnuShowListSearchBox_Click(object sender, EventArgs e)
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

        private void mnuSmallTargetInfo_Click(object sender, EventArgs e)
        {
            Settings.Default.SmallTargetInfo = !Settings.Default.SmallTargetInfo;

            mnuSmallTargetInfo.Checked = Settings.Default.SmallTargetInfo;
            mnuSmallTargetInfo2.Checked = Settings.Default.SmallTargetInfo;
        }

        private void mnuAutoConnect_Click(object sender, EventArgs e)
        {
            Settings.Default.AutoConnect = !Settings.Default.AutoConnect;
            mnuAutoConnect.Checked = Settings.Default.AutoConnect;
        }

        #region lookupbox
        private void toolStripResetLookup_Click(object sender, EventArgs e)
        {
            toolStripLookupBox.Text = "";
            toolStripLookupBox.Focus();
            eq.MarkLookups("0:");
        }
        private void toolStripResetLookup1_Click(object sender, EventArgs e)
        {
            toolStripLookupBox1.Text = "";
            toolStripLookupBox1.Focus();
            eq.MarkLookups("1:");
        }
        private void toolStripResetLookup2_Click(object sender, EventArgs e)
        {
            toolStripLookupBox2.Text = "";
            toolStripLookupBox2.Focus();
            eq.MarkLookups("2:");
        }
        private void toolStripResetLookup3_Click(object sender, EventArgs e)
        {
            toolStripLookupBox3.Text = "";
            toolStripLookupBox3.Focus();
            eq.MarkLookups("3:");
        }
        private void toolStripResetLookup4_Click(object sender, EventArgs e)
        {
            toolStripLookupBox4.Text = "";
            toolStripLookupBox4.Focus();
            eq.MarkLookups("4:");
        }
        private void toolStripResetLookup5_Click(object sender, EventArgs e)
        {
            toolStripLookupBox5.Text = "";
            toolStripLookupBox5.Focus();
            eq.MarkLookups("5:");
        }
        private void toolStripCheckLookup_CheckChanged(object sender, EventArgs e)
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
        private void toolStripCheckLookup1_CheckChanged(object sender, EventArgs e)
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
        private void toolStripCheckLookup2_CheckChanged(object sender, EventArgs e)
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
        private void toolStripCheckLookup3_CheckChanged(object sender, EventArgs e)
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
        private void toolStripCheckLookup4_CheckChanged(object sender, EventArgs e)
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
        private void toolStripCheckLookup5_CheckChanged(object sender, EventArgs e)
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

        private void toolStripTextBox_KeyPress(object sender, KeyPressEventArgs e)
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
        private void toolStripTextBox1_KeyPress(object sender, KeyPressEventArgs e)
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
        private void toolStripTextBox2_KeyPress(object sender, KeyPressEventArgs e)
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
        private void toolStripTextBox3_KeyPress(object sender, KeyPressEventArgs e)
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
        private void toolStripTextBox4_KeyPress(object sender, KeyPressEventArgs e)
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
        private void toolStripTextBox5_KeyPress(object sender, KeyPressEventArgs e)
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

        private void toolStripLookupBox_Click(object sender, EventArgs e)
        {
            if (toolStripLookupBox.Text == "Mob Search")
            {
                toolStripLookupBox.Text = "";
                toolStripLookupBox.ForeColor = SystemColors.WindowText;
            }
        }
        private void toolStripLookupBox1_Click(object sender, EventArgs e)
        {
            if (toolStripLookupBox1.Text == "Mob Search")
            {
                toolStripLookupBox1.Text = "";
                toolStripLookupBox1.ForeColor = SystemColors.WindowText;
            }
        }
        private void toolStripLookupBox2_Click(object sender, EventArgs e)
        {
            if (toolStripLookupBox2.Text == "Mob Search")
            {
                toolStripLookupBox2.Text = "";
                toolStripLookupBox2.ForeColor = SystemColors.WindowText;
            }
        }
        private void toolStripLookupBox3_Click(object sender, EventArgs e)
        {
            if (toolStripLookupBox3.Text == "Mob Search")
            {
                toolStripLookupBox3.Text = "";
                toolStripLookupBox3.ForeColor = SystemColors.WindowText;
            }
        }
        private void toolStripLookupBox4_Click(object sender, EventArgs e)
        {
            if (toolStripLookupBox4.Text == "Mob Search")
            {
                toolStripLookupBox4.Text = "";
                toolStripLookupBox4.ForeColor = SystemColors.WindowText;
            }
        }
        private void toolStripLookupBox5_Click(object sender, EventArgs e)
        {
            if (toolStripLookupBox5.Text == "Mob Search")
            {
                toolStripLookupBox5.Text = "";
                toolStripLookupBox5.ForeColor = SystemColors.WindowText;
            }
        }
        private void toolStripLookupBox_Leave(object sender, EventArgs e)
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

        private void toolStripLookupBox1_Leave(object sender, EventArgs e)
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

        private void toolStripLookupBox2_Leave(object sender, EventArgs e)
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

        private void toolStripLookupBox3_Leave(object sender, EventArgs e)
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

        private void toolStripLookupBox4_Leave(object sender, EventArgs e)
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

        private void toolStripLookupBox5_Leave(object sender, EventArgs e)
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

        private void mnuFileMain_DropDownOpening(object sender, EventArgs e)
        {
            // Update the Character Selection list
            // colProcesses.Clear();

            VisChar();
        }

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

        public void StartNewPackets()
        {
            processcount = 0;
        }

// <summary>
// These do almost exactly the same, can probaly remove and link all to one super method.
        private void toolStripLevel_TextUpdate(object sender, EventArgs e)
        {
            string Str = toolStripLevel.Text.Trim();

//            bool validnum = true; // only one operation, apply the false outcome directly.
            if (!string.IsNullOrEmpty(Str)) // better filter than "if (Str.Length > 0)"
            {
                bool isNum = int.TryParse(Str, out var Num);
                if (isNum && (Num < 1 || Num > 115))
                {
                    MessageBox.Show("1. Enter a number between 1-115 or select Auto");
                    //                    validnum = false;
                }
            }

            //if (!validnum)
            //{ 
            //    MessageBox.Show("1. Enter a number between 1-115 or select Auto");
            //} // 
        }
        private void toolStripLevel_Leave(object sender, EventArgs e)
        {
            string Str = toolStripLevel.Text.Trim();

            bool validnum = true;
            if (Str.Length > 0)
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
                else
                {
                    toolStripLevel.Text = Num.ToString();
                    Settings.Default.LevelOverride = Num;
                    gconLevel = Num;
                }
            }

            if (!validnum)
            {
                MessageBox.Show("2. Enter a number between 1-115 or Auto");
            } else {
                gConBaseName = "";
            }
        }
        private void toolStripLevel_DropDownClosed(object sender, EventArgs e)
        {
            string Str = toolStripLevel.SelectedItem.ToString();

            if (!string.IsNullOrEmpty(Str))
            {
                bool isNum = int.TryParse(Str, out var Num);
                gConBaseName = "";
                if (isNum && Num >= 1 && Num <= 115)
                {
                    Settings.Default.LevelOverride = Num;
                    gconLevel = Num;
                }
                else if (Str == "Auto")
                {
                    Settings.Default.LevelOverride = -1;
                    gconLevel = Num;
                }
            }
        }
        private void toolStripLevel_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                string Str = toolStripLevel.Text.Trim();

                if (!string.IsNullOrEmpty(Str))
                {
                    bool isNum = int.TryParse(Str, out var Num);

                    if (isNum && Num >= 1 && Num <= 115)
                    {
                        //do stuff
                        Settings.Default.LevelOverride = Num;
                        gconLevel = Num;
                    }
                }
                toolStripScale.Focus();

                e.Handled = true;
            }
        }
    }
}



