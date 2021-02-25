using System;

using SpeechLib;

using System.IO;

using System.Net;

using System.Drawing;

using System.Reflection;

using System.Collections;

using System.Collections.Generic;

using System.Diagnostics;

using System.Windows.Forms;

using System.ComponentModel;

using WeifenLuo.WinFormsUI.Docking;

using System.Text.RegularExpressions;

using System.Runtime.InteropServices;



// Class Files

using SocketSystem;

using Structures;



namespace myseq 

{

    [Serializable]

    public class frmMain : System.Windows.Forms.Form    

    {

        public string Version = "";     

        public Filters filters = new Filters();

        public string curZone = "map_pane";

        public string currentIPAddress = "";

        public float rawScale = 1.0f;

        public string BaseTitle = "MySEQ Open";

        private Point addTextFormLocation = new Point(0, 0);

        public myseq.MapCon mapCon;

        public myseq.MapPane mapPane = new MapPane();        

        public myseq.ListViewPanel SpawnList = new ListViewPanel(0);

        public myseq.ListViewPanel SpawnTimerList = new ListViewPanel(1);

        public myseq.ListViewPanel GroundItemList = new ListViewPanel(2);

        private EQData eq;

        private EQCommunications comm;

        private EQMap map;

        public RegexHelper reHelper = new RegexHelper();



        public DrawOptions DrawOpts=DrawOptions.DrawNormal;



        public System.Collections.ArrayList colProcesses = new ArrayList();

        public ProcessInfo CurrentProcess = new ProcessInfo(0,"");

        public int processcount = 0;

        #region System Components

        private System.Windows.Forms.MenuStrip mnuMainMenu;

        private System.Windows.Forms.StatusStrip statusBarStrip;

        private System.Windows.Forms.Timer timPackets;

        private System.Timers.Timer timDelayAlerts;

        private System.Timers.Timer timProcessTimers;

        private System.ComponentModel.IContainer components;

        public  System.Windows.Forms.ColorDialog colorPicker;

        private System.Windows.Forms.OpenFileDialog openFileDialog;

        private System.Windows.Forms.FontDialog fontDialog1;

        private System.Windows.Forms.ContextMenuStrip mnuContext;

        private System.Windows.Forms.ContextMenuStrip mnuContextAddFilter;

        private System.Windows.Forms.ToolStripMenuItem mnuMobName;

        private System.Windows.Forms.ToolStripMenuItem mnuAddHuntFilter;

        private System.Windows.Forms.ToolStripMenuItem mnuAddCautionFilter;

        private System.Windows.Forms.ToolStripMenuItem mnuAddDangerFilter;

        private System.Windows.Forms.ToolStripMenuItem mnuAddAlertFilter;

        private System.Windows.Forms.ToolStripMenuItem mnuAddMapLabel;

        public string alertAddmobname = "";
        public float alertX = 0.0f;
        public float alertY = 0.0f;
        public float alertZ = 0.0f;

        public string mapnameWithLabels = "";

        private System.Windows.Forms.ToolStripMenuItem mnuSearchAllakhazam;

        private System.Windows.Forms.ToolStripMenuItem mnuShowListNPCs;

        private System.Windows.Forms.ToolStripMenuItem mnuShowListCorpses;

        private System.Windows.Forms.ToolStripMenuItem mnuShowListPlayers;

        private System.Windows.Forms.ToolStripMenuItem mnuShowListInvis;

        private System.Windows.Forms.ToolStripMenuItem mnuShowListMounts;

        private System.Windows.Forms.ToolStripMenuItem mnuShowListFamiliars;

        private System.Windows.Forms.ToolStripMenuItem mnuShowListPets;

        

        private ToolStripMenuItem mnuFileMain;
        private ToolStripMenuItem mnuConnect;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem mnuOpenMap;
        private ToolStripMenuItem mnuSaveMobs;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem mnuSavePrefs;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripMenuItem mnuExit;
        private ToolStripMenuItem menuItem5;
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
        private ToolStripMenuItem mnuCharacterSelection;
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
        private ToolStripButton toolStripEmailAlerts;
        private ToolStripSeparator toolStripSeparator19;
        private ToolStripMenuItem addZoneEmailAlertFilterToolStripMenuItem;
        private ToolStripMenuItem mnuAutoConnect;
        public ToolStripComboBox toolStripLevel;
        public int gLastconLevel = -1;
        public int gconLevel = -1;
        public string gConBaseName = "";
        private ToolStripMenuItem toolStripBasecon;

        bool bIsRunning = false;
        bool bFilter0 = false;
        bool bFilter1 = false;
        bool bFilter2 = false;
        bool bFilter3 = false;
        bool bFilter4 = false;
        bool bFilter5 = false;

        public void StopListening() 

        {

            // Stop the Timer

            timPackets.Stop();
            timDelayAlerts.Stop();
            eq.DisablePlayAlerts();

            comm.StopListening();

            

            mapPane.cmdCommand.Text = "GO";

            this.mnuConnect.Text = "&Connect";

            this.mnuConnect.Image = global::myseq.Properties.Resources.PlayHS;

            this.toolStripStartStop.Text = "Go";
            this.toolStripStartStop.ToolTipText = "Connect to Server";
            this.toolStripStartStop.Image = global::myseq.Properties.Resources.PlayHS;

            bIsRunning = false;

            toolStripServerAddress.Text = "";

        }

        public frmMain()         

        {

            // This shuts up the error messages when running under a debugger

            Control.CheckForIllegalCrossThreadCalls = false;

            m_deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);
        
            eq = new EQData();

            comm = new EQCommunications(eq,this);

            map = new EQMap();

            InitializeComponent();

            LogLib.maxLogLevel = LogLevel.DefaultMaxLevel;

            String myPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MySEQ");

            String newFile = Path.Combine(myPath, "prefs.xml");

            if (File.Exists(newFile))
                LoadPrefs(newFile);
            else
                LoadPrefs("prefs.xml");

            LogLib.WriteLine("MySEQ Open Version: " + Version);
            
           // Version = Application.ProductVersion.ToString().Substring(0, Application.ProductVersion.Length - 2);

            LogLib.WriteLine("Loaded Prefs.Xml");

            String newSMTPFile = Path.Combine(myPath, "myseq.xml");

            if (File.Exists(newSMTPFile))
                SMTPSettings.Instance.Load(newSMTPFile);
            else
                SMTPSettings.Instance.Load("myseq.xml");

            mapCon = mapPane.mapCon;
            
            // Set Map Window Options
            mapPane.DockAreas = ((DockAreas)(DockAreas.Document));
            mapPane.CloseButtonVisible = false;
            mapPane.TabText = "map_pane";
            
            LogLib.WriteLine("Creating SpawnList Window");

            // Set Spawn List Window Options
            SpawnList.HideOnClose = true;
            SpawnList.TabText = "Spawn List";

            this.SpawnList.VisibleChanged += new System.EventHandler(SpawnList_VisibleChanged);

            LogLib.WriteLine("Creating SpawnTimerList Window");

            // Set Spawn Timer Window Options
            SpawnTimerList.HideOnClose = true;
            SpawnTimerList.TabText = "Spawn Timer List";
            this.SpawnTimerList.VisibleChanged += new System.EventHandler(SpawnTimerList_VisibleChanged);
            
            LogLib.WriteLine("Creating GroundItemList Window");

            // Set Ground Item Window Options
            this.GroundItemList.HideOnClose = true;
            this.GroundItemList.TabText = "Ground Items";
            
            this.GroundItemList.VisibleChanged += new System.EventHandler(GroundItemList_VisibleChanged);

            
            // [42!] Make all the components known to each other

            // TODO: use this to remove all the implicit interdependencies between the comps ("Parent"...)

            mapCon.SetComponents(this,mapPane,eq,map);

            mapPane.SetComponents(this,eq);

            SpawnList.SetComponents(eq,mapCon,mapPane,filters,this);

            SpawnTimerList.SetComponents(eq, mapCon, mapPane, filters, this);

            GroundItemList.SetComponents(eq, mapCon, mapPane, filters, this);

            map.SetComponents(mapCon,SpawnList,SpawnTimerList,GroundItemList,mapPane,eq);

            eq.mobsTimers.SetComponents(map);

            LogLib.WriteLine("Loading Position.Xml");

            String configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "positions.xml");

            // This in the application data folder.
            String newConfigFile = Path.Combine(myPath, "positions.xml");

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

            if (Settings.Instance.AlwaysOnTop)
            {
                this.TopMost = true;
                this.TopLevel = true;
            }

            // Add the Columns to the Spawn List Window

            SpawnList.ColumnsAdd("Name", Settings.Instance.c1w, HorizontalAlignment.Left);

            SpawnList.ColumnsAdd("Level", Settings.Instance.c2w, HorizontalAlignment.Left);

            SpawnList.ColumnsAdd("Class", Settings.Instance.c3w, HorizontalAlignment.Left);

            SpawnList.ColumnsAdd("Primary", Settings.Instance.c3w, HorizontalAlignment.Left);

            SpawnList.ColumnsAdd("Offhand", Settings.Instance.c3w, HorizontalAlignment.Left);

            SpawnList.ColumnsAdd("Race", Settings.Instance.c4w, HorizontalAlignment.Left);

            SpawnList.ColumnsAdd("Owner", Settings.Instance.c4w, HorizontalAlignment.Left);

            SpawnList.ColumnsAdd("Last Name", Settings.Instance.c5w, HorizontalAlignment.Left);

            SpawnList.ColumnsAdd("Type", Settings.Instance.c6w, HorizontalAlignment.Left);

            SpawnList.ColumnsAdd("Invis", Settings.Instance.c7w, HorizontalAlignment.Left);

            SpawnList.ColumnsAdd("Run Speed", Settings.Instance.c8w, HorizontalAlignment.Left);

            SpawnList.ColumnsAdd("SpawnID", Settings.Instance.c9w, HorizontalAlignment.Left);

            SpawnList.ColumnsAdd("Spawn Time", Settings.Instance.c10w, HorizontalAlignment.Left);

            SpawnList.ColumnsAdd("X", Settings.Instance.c11w, HorizontalAlignment.Left);

            SpawnList.ColumnsAdd("Y", Settings.Instance.c12w, HorizontalAlignment.Left);

            SpawnList.ColumnsAdd("Z", Settings.Instance.c13w, HorizontalAlignment.Left);

            SpawnList.ColumnsAdd("Distance", Settings.Instance.c14w, HorizontalAlignment.Left);

 
            // Set the Font, Size, Style for the Spawn List Window

            try {SpawnList.listView.Font = new Font(Settings.Instance.ListFontName, Settings.Instance.ListFontSize, Settings.Instance.ListFontStyle);}

            catch (Exception ex) {LogLib.WriteLine("Error setting spawn list font: ", ex);}

            

            // Add the Columns to the Spawn Timer Window

            SpawnTimerList.ColumnsAdd("Spawn Name", Settings.Instance.c1w, HorizontalAlignment.Left);

            SpawnTimerList.ColumnsAdd("Remain", Settings.Instance.c10w, HorizontalAlignment.Left);

            SpawnTimerList.ColumnsAdd("Interval", Settings.Instance.c10w, HorizontalAlignment.Left);

            SpawnTimerList.ColumnsAdd("Zone", Settings.Instance.c10w, HorizontalAlignment.Left);

            SpawnTimerList.ColumnsAdd("X", Settings.Instance.c12w, HorizontalAlignment.Left);

            SpawnTimerList.ColumnsAdd("Y", Settings.Instance.c11w, HorizontalAlignment.Left);

            SpawnTimerList.ColumnsAdd("Z", Settings.Instance.c13w, HorizontalAlignment.Left);

            SpawnTimerList.ColumnsAdd("Count", Settings.Instance.c9w, HorizontalAlignment.Left);

            SpawnTimerList.ColumnsAdd("Spawn Time", Settings.Instance.c10w, HorizontalAlignment.Left);

            SpawnTimerList.ColumnsAdd("Kill Time", Settings.Instance.c10w, HorizontalAlignment.Left);

            SpawnTimerList.ColumnsAdd("Next Spawn", Settings.Instance.c10w, HorizontalAlignment.Left);



            // Set the Font, Size, Style for the Spawn List Timer Window

            try { SpawnTimerList.listView.Font = new Font(Settings.Instance.ListFontName, Settings.Instance.ListFontSize, Settings.Instance.ListFontStyle); }

            catch (Exception ex) { LogLib.WriteLine("Error setting spawn timer list font: ", ex); }


            GroundItemList.ColumnsAdd("Description", Settings.Instance.c1w, HorizontalAlignment.Left);

            GroundItemList.ColumnsAdd("Name", Settings.Instance.c1w, HorizontalAlignment.Left);

            GroundItemList.ColumnsAdd("Spawn Time", Settings.Instance.c10w, HorizontalAlignment.Left);

            GroundItemList.ColumnsAdd("X", Settings.Instance.c12w, HorizontalAlignment.Left);

            GroundItemList.ColumnsAdd("Y", Settings.Instance.c11w, HorizontalAlignment.Left);

            GroundItemList.ColumnsAdd("Z", Settings.Instance.c13w, HorizontalAlignment.Left);

           


            // Set the Font, Size, Style for the Spawn List Window

            try { GroundItemList.listView.Font = new Font(Settings.Instance.ListFontName, Settings.Instance.ListFontSize, Settings.Instance.ListFontStyle); }

            catch (Exception ex) { LogLib.WriteLine("Error setting ground item list font: ", ex); }



            // Set the Font, Size, Style for the Map Labels

            try { 
                mapCon.drawFont = new Font(Settings.Instance.MapLabelFontName, Settings.Instance.MapLabelFontSize, Settings.Instance.MapLabelFontStyle);
                mapCon.drawFont1 = new Font(Settings.Instance.MapLabelFontName, Settings.Instance.MapLabelFontSize * 0.9f, Settings.Instance.MapLabelFontStyle);
                mapCon.drawFont3 = new Font(Settings.Instance.MapLabelFontName, Settings.Instance.MapLabelFontSize * 1.1f, Settings.Instance.MapLabelFontStyle);
            }

            catch (Exception ex) { LogLib.WriteLine("Error setting map label font: ", ex); }



            // Set the Font, Size, Style to the Spawn Info Window

            try {

                mapCon.lblMobInfo.Font = new Font(Settings.Instance.TargetInfoFontName, Settings.Instance.TargetInfoFontSize, Settings.Instance.TargetInfoFontStyle);

                mapCon.lblGameClock.Font = new Font(Settings.Instance.TargetInfoFontName, Settings.Instance.TargetInfoFontSize, FontStyle.Bold);

            }

            catch (Exception ex) { LogLib.WriteLine("Error setting Target Info font: ", ex); }

            toolStripVersion.Text = Version;


            mapCon.SetInitialParams();

            eq.InitLookups();

            timPackets.Interval = Settings.Instance.UpdateDelay;

            // This is delay that stops emails and alert sounds right after zoning
            timDelayAlerts.Interval = 10000;

            // This is for processing timers, do it once per second.
            timProcessTimers.Interval = 1000;

            SetUpdateSteps();

            this.Text = BaseTitle;

            if (Settings.Instance.AutoConnect)
                AutoConnect();

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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            WeifenLuo.WinFormsUI.Docking.DockPanelSkin dockPanelSkin1 = new WeifenLuo.WinFormsUI.Docking.DockPanelSkin();
            WeifenLuo.WinFormsUI.Docking.AutoHideStripSkin autoHideStripSkin1 = new WeifenLuo.WinFormsUI.Docking.AutoHideStripSkin();
            WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient1 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient1 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.DockPaneStripSkin dockPaneStripSkin1 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripSkin();
            WeifenLuo.WinFormsUI.Docking.DockPaneStripGradient dockPaneStripGradient1 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient2 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient2 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient3 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.DockPaneStripToolWindowGradient dockPaneStripToolWindowGradient1 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripToolWindowGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient4 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient5 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient3 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient6 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient7 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            this.mnuMainMenu = new System.Windows.Forms.MenuStrip();
            this.mnuFileMain = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSavePrefs = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuOpenMap = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSaveMobs = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuConnect = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAutoConnect = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuServerSelection = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuIPAddress1 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuIPAddress2 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuIPAddress3 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuIPAddress4 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuIPAddress5 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCharacterSelection = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuChar1 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuChar2 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuChar3 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuChar4 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuChar5 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuChar6 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuChar7 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuChar8 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuChar9 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuChar10 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuChar11 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuChar12 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem13 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuCharRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuEditMain = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuChangeColor = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuGridColor = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuGridLabelColor = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuListColor = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuBackgroungColor = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuChangeFont = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSpawnListFont = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTargetInfoFont = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuMapLabelsFont = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator18 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuReloadAlerts = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuEditGlobalAlerts = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuEditZoneAlerts = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuRefreshSpawnList = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuClearSavedTimers = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSaveSpawnLog = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuViewMain = new System.Windows.Forms.ToolStripMenuItem();
            this.toolbarsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuViewMenuBar = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuViewStatusBar = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuViewDepthFilterBar = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuShowSpawnList = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShowSpawnListTimer = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShowGroundItemList = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuShowListGridLines = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShowListSearchBox = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShowGridLines = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuShowCorpses = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShowPCCorpses = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShowMyCorpse = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShowPlayers = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShowInvis = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShowMounts = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShowFamiliars = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShowPets = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShowNPCs = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuMapSettingsMain = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDepthFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDynamicAlpha = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFilterMapLines = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFilterMapText = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFilterNPCs = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFilterNPCCorpses = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFilterPlayers = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFilterPlayerCorpses = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFilterGroundItems = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFilterSpawnPoints = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShowLookupText = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShowLookupNumber = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAlwaysOnTop = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuForceDistinct = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuForceDistinctText = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuLabelShow = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShowNPCLevels = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShowNPCNames = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShowNPCCorpseNames = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShowPCNames = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShowPlayerCorpseNames = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSpawnCountdown = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShowSpawnPoints = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShowZoneText = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShowLayer1 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShowLayer2 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShowLayer3 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShowPVP = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShowPVPLevel = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCollectMobTrails = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShowMobTrails = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuConColors = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuConDefault = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuConSoD = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuConSoF = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuGridInterval = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuGridInterval100 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuGridInterval250 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuGridInterval500 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuGridInterval1000 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShowTargetInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSmallTargetInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAutoSelectEQTarget = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuFollowNone = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFollowPlayer = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFollowTarget = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuKeepCentered = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAutoExpand = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuMapReset = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHelpMain = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuContext = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuDepthFilter2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDynamicAlpha2 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFilterMapLines2 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFilterMapText2 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFilterNPCs2 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFilterNPCCorpses2 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFilterPlayers2 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFilterPlayerCorpses2 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFilterGroundItems2 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFilterSpawnPoints2 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuForceDistinct2 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuForceDistinctText2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.addMapTextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuLabelShow2 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShowNPCLevels2 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShowNPCNames2 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShowNPCCorpseNames2 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShowPCNames2 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShowPlayerCorpseNames2 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSpawnCountdown2 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShowSpawnPoints2 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShowZoneText2 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShowLayer21 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShowLayer22 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShowLayer23 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShowPVP2 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShowPVPLevel2 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShowTargetInfo2 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSmallTargetInfo2 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAutoSelectEQTarget2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator15 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuFollowNone2 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFollowPlayer2 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFollowTarget2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator16 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuKeepCentered2 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAutoExpand2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator17 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuShowMenuBar = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuMapReset2 = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.mnuContextAddFilter = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuMobName = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem11 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuAddHuntFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAddCautionFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAddDangerFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAddAlertFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.addZoneEmailAlertFilterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripBasecon = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSepAddFilter = new System.Windows.Forms.ToolStripSeparator();
            this.mnuAddMapLabel = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSepAddMapLabel = new System.Windows.Forms.ToolStripSeparator();
            this.mnuSearchAllakhazam = new System.Windows.Forms.ToolStripMenuItem();
            this.timPackets = new System.Windows.Forms.Timer(this.components);
            this.timDelayAlerts = new System.Timers.Timer();
            this.timProcessTimers = new System.Timers.Timer();
            this.colorPicker = new System.Windows.Forms.ColorDialog();
            this.mnuShowListNPCs = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShowListCorpses = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShowListPlayers = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShowListInvis = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShowListMounts = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShowListFamiliars = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShowListPets = new System.Windows.Forms.ToolStripMenuItem();
            this.statusBarStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripMouseLocation = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripDistance = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripSpring = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripVersion = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripServerAddress = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripCoPStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripShortName = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripFPS = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolBarStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripStartStop = new System.Windows.Forms.ToolStripButton();
            this.toolStripLevel = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator14 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripZoomIn = new System.Windows.Forms.ToolStripButton();
            this.toolStripZoomOut = new System.Windows.Forms.ToolStripButton();
            this.toolStripScale = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripDepthFilterButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripZPosLabel = new System.Windows.Forms.ToolStripLabel();
            this.toolStripZPos = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripZPosDown = new System.Windows.Forms.ToolStripButton();
            this.toolStripZPosUp = new System.Windows.Forms.ToolStripButton();
            this.toolStripZOffsetLabel = new System.Windows.Forms.ToolStripLabel();
            this.toolStripZNeg = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripZNegUp = new System.Windows.Forms.ToolStripButton();
            this.toolStripZNegDown = new System.Windows.Forms.ToolStripButton();
            this.toolStripResetDepthFilter = new System.Windows.Forms.ToolStripButton();
            this.toolStripEmailAlerts = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator19 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLookupBox = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripResetLookup = new System.Windows.Forms.ToolStripButton();
            this.toolStripLookupBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripResetLookup1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripLookupBox2 = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripResetLookup2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripLookupBox3 = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripResetLookup3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripLookupBox4 = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripResetLookup4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripLookupBox5 = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripCheckLookup = new System.Windows.Forms.ToolStripButton();
            this.toolStripCheckLookup1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripCheckLookup2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripCheckLookup3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripCheckLookup4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripCheckLookup5 = new System.Windows.Forms.ToolStripButton();
            this.toolStripResetLookup5 = new System.Windows.Forms.ToolStripButton();
            this.toolStripOptions = new System.Windows.Forms.ToolStripButton();
            this.dockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.mnuMainMenu.SuspendLayout();
            this.mnuContext.SuspendLayout();
            this.mnuContextAddFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timDelayAlerts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timProcessTimers)).BeginInit();
            this.statusBarStrip.SuspendLayout();
            this.toolBarStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // mnuMainMenu
            // 
            this.mnuMainMenu.BackColor = System.Drawing.SystemColors.ControlLight;
            this.mnuMainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFileMain,
            this.mnuEditMain,
            this.mnuViewMain,
            this.mnuMapSettingsMain,
            this.mnuHelpMain});
            this.mnuMainMenu.Location = new System.Drawing.Point(0, 0);
            this.mnuMainMenu.Name = "mnuMainMenu";
            this.mnuMainMenu.Size = new System.Drawing.Size(800, 24);
            this.mnuMainMenu.TabIndex = 0;
            this.mnuMainMenu.Text = "mnuMainMenu";
            // 
            // mnuFileMain
            // 
            this.mnuFileMain.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuOptions,
            this.mnuSavePrefs,
            this.toolStripSeparator2,
            this.mnuOpenMap,
            this.mnuSaveMobs,
            this.toolStripSeparator1,
            this.mnuConnect,
            this.mnuAutoConnect,
            this.mnuServerSelection,
            this.mnuCharacterSelection,
            this.toolStripSeparator3,
            this.mnuExit,
            this.menuItem5});
            this.mnuFileMain.Name = "mnuFileMain";
            this.mnuFileMain.Size = new System.Drawing.Size(37, 20);
            this.mnuFileMain.Text = "&File";
            this.mnuFileMain.DropDownOpening += new System.EventHandler(this.mnuFileMain_DropDownOpening);
            // 
            // mnuOptions
            // 
            this.mnuOptions.Image = ((System.Drawing.Image)(resources.GetObject("mnuOptions.Image")));
            this.mnuOptions.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnuOptions.Name = "mnuOptions";
            this.mnuOptions.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.mnuOptions.Size = new System.Drawing.Size(177, 22);
            this.mnuOptions.Text = "&Options";
            this.mnuOptions.Click += new System.EventHandler(this.mnuOptions_Click);
            // 
            // mnuSavePrefs
            // 
            this.mnuSavePrefs.Image = ((System.Drawing.Image)(resources.GetObject("mnuSavePrefs.Image")));
            this.mnuSavePrefs.Name = "mnuSavePrefs";
            this.mnuSavePrefs.Size = new System.Drawing.Size(177, 22);
            this.mnuSavePrefs.Text = "Save &Prefs";
            this.mnuSavePrefs.Click += new System.EventHandler(this.mnuSavePrefs_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(174, 6);
            // 
            // mnuOpenMap
            // 
            this.mnuOpenMap.Image = ((System.Drawing.Image)(resources.GetObject("mnuOpenMap.Image")));
            this.mnuOpenMap.Name = "mnuOpenMap";
            this.mnuOpenMap.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.mnuOpenMap.Size = new System.Drawing.Size(177, 22);
            this.mnuOpenMap.Text = "&Open Map";
            this.mnuOpenMap.Click += new System.EventHandler(this.mnuOpenMap_Click);
            // 
            // mnuSaveMobs
            // 
            this.mnuSaveMobs.Name = "mnuSaveMobs";
            this.mnuSaveMobs.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.mnuSaveMobs.Size = new System.Drawing.Size(177, 22);
            this.mnuSaveMobs.Text = "&Save Mobs";
            this.mnuSaveMobs.Click += new System.EventHandler(this.mnuSaveMobs_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(174, 6);
            // 
            // mnuConnect
            // 
            this.mnuConnect.Image = ((System.Drawing.Image)(resources.GetObject("mnuConnect.Image")));
            this.mnuConnect.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.mnuConnect.Name = "mnuConnect";
            this.mnuConnect.Size = new System.Drawing.Size(177, 22);
            this.mnuConnect.Text = "&Connect";
            this.mnuConnect.Click += new System.EventHandler(this.cmdCommand_Click);
            // 
            // mnuAutoConnect
            // 
            this.mnuAutoConnect.Name = "mnuAutoConnect";
            this.mnuAutoConnect.Size = new System.Drawing.Size(177, 22);
            this.mnuAutoConnect.Text = "Connect on Startup";
            this.mnuAutoConnect.Click += new System.EventHandler(this.mnuAutoConnect_Click);
            // 
            // mnuServerSelection
            // 
            this.mnuServerSelection.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuIPAddress1,
            this.mnuIPAddress2,
            this.mnuIPAddress3,
            this.mnuIPAddress4,
            this.mnuIPAddress5});
            this.mnuServerSelection.Name = "mnuServerSelection";
            this.mnuServerSelection.Size = new System.Drawing.Size(177, 22);
            this.mnuServerSelection.Text = "&Server Selection";
            // 
            // mnuIPAddress1
            // 
            this.mnuIPAddress1.Name = "mnuIPAddress1";
            this.mnuIPAddress1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D1)));
            this.mnuIPAddress1.Size = new System.Drawing.Size(107, 22);
            this.mnuIPAddress1.Click += new System.EventHandler(this.mnuIPAddress1_Click);
            // 
            // mnuIPAddress2
            // 
            this.mnuIPAddress2.Name = "mnuIPAddress2";
            this.mnuIPAddress2.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D2)));
            this.mnuIPAddress2.Size = new System.Drawing.Size(107, 22);
            this.mnuIPAddress2.Click += new System.EventHandler(this.mnuIPAddress2_Click);
            // 
            // mnuIPAddress3
            // 
            this.mnuIPAddress3.Name = "mnuIPAddress3";
            this.mnuIPAddress3.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D3)));
            this.mnuIPAddress3.Size = new System.Drawing.Size(107, 22);
            this.mnuIPAddress3.Click += new System.EventHandler(this.mnuIPAddress3_Click);
            // 
            // mnuIPAddress4
            // 
            this.mnuIPAddress4.Name = "mnuIPAddress4";
            this.mnuIPAddress4.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D4)));
            this.mnuIPAddress4.Size = new System.Drawing.Size(107, 22);
            this.mnuIPAddress4.Click += new System.EventHandler(this.mnuIPAddress4_Click);
            // 
            // mnuIPAddress5
            // 
            this.mnuIPAddress5.Name = "mnuIPAddress5";
            this.mnuIPAddress5.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D5)));
            this.mnuIPAddress5.Size = new System.Drawing.Size(107, 22);
            this.mnuIPAddress5.Click += new System.EventHandler(this.mnuIPAddress5_Click);
            // 
            // mnuCharacterSelection
            // 
            this.mnuCharacterSelection.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
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
            this.mnuCharacterSelection.Name = "mnuCharacterSelection";
            this.mnuCharacterSelection.Overflow = System.Windows.Forms.ToolStripItemOverflow.Always;
            this.mnuCharacterSelection.Size = new System.Drawing.Size(177, 22);
            this.mnuCharacterSelection.Text = "&Character Selection";
            // 
            // mnuChar1
            // 
            this.mnuChar1.Checked = true;
            this.mnuChar1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mnuChar1.Name = "mnuChar1";
            this.mnuChar1.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D1)));
            this.mnuChar1.Size = new System.Drawing.Size(188, 22);
            this.mnuChar1.Text = "Char 1";
            this.mnuChar1.Visible = false;
            this.mnuChar1.Click += new System.EventHandler(this.mnuChar1_Click);
            // 
            // mnuChar2
            // 
            this.mnuChar2.Name = "mnuChar2";
            this.mnuChar2.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D2)));
            this.mnuChar2.Size = new System.Drawing.Size(188, 22);
            this.mnuChar2.Text = "Char 2";
            this.mnuChar2.Visible = false;
            this.mnuChar2.Click += new System.EventHandler(this.mnuChar2_Click);
            // 
            // mnuChar3
            // 
            this.mnuChar3.Name = "mnuChar3";
            this.mnuChar3.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D3)));
            this.mnuChar3.Size = new System.Drawing.Size(188, 22);
            this.mnuChar3.Text = "Char 3";
            this.mnuChar3.Visible = false;
            this.mnuChar3.Click += new System.EventHandler(this.mnuChar3_Click);
            // 
            // mnuChar4
            // 
            this.mnuChar4.Name = "mnuChar4";
            this.mnuChar4.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D4)));
            this.mnuChar4.Size = new System.Drawing.Size(188, 22);
            this.mnuChar4.Text = "Char 4";
            this.mnuChar4.Visible = false;
            this.mnuChar4.Click += new System.EventHandler(this.mnuChar4_Click);
            // 
            // mnuChar5
            // 
            this.mnuChar5.Name = "mnuChar5";
            this.mnuChar5.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D5)));
            this.mnuChar5.Size = new System.Drawing.Size(188, 22);
            this.mnuChar5.Text = "Char 5";
            this.mnuChar5.Visible = false;
            this.mnuChar5.Click += new System.EventHandler(this.mnuChar5_Click);
            // 
            // mnuChar6
            // 
            this.mnuChar6.Name = "mnuChar6";
            this.mnuChar6.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D6)));
            this.mnuChar6.Size = new System.Drawing.Size(188, 22);
            this.mnuChar6.Text = "Char 6";
            this.mnuChar6.Visible = false;
            this.mnuChar6.Click += new System.EventHandler(this.mnuChar6_Click);
            // 
            // mnuChar7
            // 
            this.mnuChar7.Name = "mnuChar7";
            this.mnuChar7.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D7)));
            this.mnuChar7.Size = new System.Drawing.Size(188, 22);
            this.mnuChar7.Text = "Char 7";
            this.mnuChar7.Visible = false;
            this.mnuChar7.Click += new System.EventHandler(this.mnuChar7_Click);
            // 
            // mnuChar8
            // 
            this.mnuChar8.Name = "mnuChar8";
            this.mnuChar8.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D8)));
            this.mnuChar8.Size = new System.Drawing.Size(188, 22);
            this.mnuChar8.Text = "Char 8";
            this.mnuChar8.Visible = false;
            this.mnuChar8.Click += new System.EventHandler(this.mnuChar8_Click);
            // 
            // mnuChar9
            // 
            this.mnuChar9.Name = "mnuChar9";
            this.mnuChar9.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D9)));
            this.mnuChar9.Size = new System.Drawing.Size(188, 22);
            this.mnuChar9.Text = "Char 9";
            this.mnuChar9.Visible = false;
            this.mnuChar9.Click += new System.EventHandler(this.mnuChar9_Click);
            // 
            // mnuChar10
            // 
            this.mnuChar10.Name = "mnuChar10";
            this.mnuChar10.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D0)));
            this.mnuChar10.Size = new System.Drawing.Size(188, 22);
            this.mnuChar10.Text = "Char 10";
            this.mnuChar10.Visible = false;
            this.mnuChar10.Click += new System.EventHandler(this.mnuChar10_Click);
            // 
            // mnuChar11
            // 
            this.mnuChar11.Name = "mnuChar11";
            this.mnuChar11.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.B)));
            this.mnuChar11.Size = new System.Drawing.Size(188, 22);
            this.mnuChar11.Text = "Char 11";
            this.mnuChar11.Visible = false;
            this.mnuChar11.Click += new System.EventHandler(this.mnuChar11_Click);
            // 
            // mnuChar12
            // 
            this.mnuChar12.Name = "mnuChar12";
            this.mnuChar12.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.C)));
            this.mnuChar12.Size = new System.Drawing.Size(188, 22);
            this.mnuChar12.Text = "Char 12";
            this.mnuChar12.Visible = false;
            this.mnuChar12.Click += new System.EventHandler(this.mnuChar12_Click);
            // 
            // menuItem13
            // 
            this.menuItem13.Name = "menuItem13";
            this.menuItem13.Size = new System.Drawing.Size(185, 6);
            this.menuItem13.Visible = false;
            // 
            // mnuCharRefresh
            // 
            this.mnuCharRefresh.Name = "mnuCharRefresh";
            this.mnuCharRefresh.Size = new System.Drawing.Size(188, 22);
            this.mnuCharRefresh.Text = "Refresh List";
            this.mnuCharRefresh.Visible = false;
            this.mnuCharRefresh.Click += new System.EventHandler(this.mnuCharRefresh_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(174, 6);
            // 
            // mnuExit
            // 
            this.mnuExit.Name = "mnuExit";
            this.mnuExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.mnuExit.Size = new System.Drawing.Size(177, 22);
            this.mnuExit.Text = "E&xit";
            this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
            // 
            // menuItem5
            // 
            this.menuItem5.Enabled = false;
            this.menuItem5.Name = "menuItem5";
            this.menuItem5.Size = new System.Drawing.Size(177, 22);
            this.menuItem5.Text = "Add Test";
            this.menuItem5.Visible = false;
            this.menuItem5.Click += new System.EventHandler(this.menuItem5_Click);
            // 
            // mnuEditMain
            // 
            this.mnuEditMain.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
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
            this.mnuEditMain.Size = new System.Drawing.Size(39, 20);
            this.mnuEditMain.Text = "&Edit";
            // 
            // mnuChangeColor
            // 
            this.mnuChangeColor.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuGridColor,
            this.mnuGridLabelColor,
            this.mnuListColor,
            this.mnuBackgroungColor});
            this.mnuChangeColor.Name = "mnuChangeColor";
            this.mnuChangeColor.Size = new System.Drawing.Size(173, 22);
            this.mnuChangeColor.Text = "C&hange Color";
            // 
            // mnuGridColor
            // 
            this.mnuGridColor.Name = "mnuGridColor";
            this.mnuGridColor.Size = new System.Drawing.Size(197, 22);
            this.mnuGridColor.Text = "Grid Color";
            this.mnuGridColor.Click += new System.EventHandler(this.mnuGridColor_Click);
            // 
            // mnuGridLabelColor
            // 
            this.mnuGridLabelColor.Name = "mnuGridLabelColor";
            this.mnuGridLabelColor.Size = new System.Drawing.Size(197, 22);
            this.mnuGridLabelColor.Text = "Grid Label Color";
            this.mnuGridLabelColor.Click += new System.EventHandler(this.mnuGridLabelColor_Click);
            // 
            // mnuListColor
            // 
            this.mnuListColor.Name = "mnuListColor";
            this.mnuListColor.Size = new System.Drawing.Size(197, 22);
            this.mnuListColor.Text = "Spawn List Color";
            this.mnuListColor.Click += new System.EventHandler(this.mnuListColor_Click);
            // 
            // mnuBackgroungColor
            // 
            this.mnuBackgroungColor.Name = "mnuBackgroungColor";
            this.mnuBackgroungColor.Size = new System.Drawing.Size(197, 22);
            this.mnuBackgroungColor.Text = "Map Background Color";
            this.mnuBackgroungColor.Click += new System.EventHandler(this.mnuBackgroungColor_Click);
            // 
            // mnuChangeFont
            // 
            this.mnuChangeFont.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuSpawnListFont,
            this.mnuTargetInfoFont,
            this.mnuMapLabelsFont});
            this.mnuChangeFont.Name = "mnuChangeFont";
            this.mnuChangeFont.Size = new System.Drawing.Size(173, 22);
            this.mnuChangeFont.Text = "Change &Font";
            // 
            // mnuSpawnListFont
            // 
            this.mnuSpawnListFont.Name = "mnuSpawnListFont";
            this.mnuSpawnListFont.Size = new System.Drawing.Size(161, 22);
            this.mnuSpawnListFont.Text = "Spawn List Font";
            this.mnuSpawnListFont.Click += new System.EventHandler(this.mnuSpawnListFont_Click);
            // 
            // mnuTargetInfoFont
            // 
            this.mnuTargetInfoFont.Name = "mnuTargetInfoFont";
            this.mnuTargetInfoFont.Size = new System.Drawing.Size(161, 22);
            this.mnuTargetInfoFont.Text = "Target Info Font";
            this.mnuTargetInfoFont.Click += new System.EventHandler(this.mnuTargetInfoFont_Click);
            // 
            // mnuMapLabelsFont
            // 
            this.mnuMapLabelsFont.Name = "mnuMapLabelsFont";
            this.mnuMapLabelsFont.Size = new System.Drawing.Size(161, 22);
            this.mnuMapLabelsFont.Text = "Map Labels Font";
            this.mnuMapLabelsFont.Click += new System.EventHandler(this.mnuMapLabelsFont_Click);
            // 
            // toolStripSeparator18
            // 
            this.toolStripSeparator18.Name = "toolStripSeparator18";
            this.toolStripSeparator18.Size = new System.Drawing.Size(170, 6);
            // 
            // mnuReloadAlerts
            // 
            this.mnuReloadAlerts.Image = ((System.Drawing.Image)(resources.GetObject("mnuReloadAlerts.Image")));
            this.mnuReloadAlerts.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.mnuReloadAlerts.Name = "mnuReloadAlerts";
            this.mnuReloadAlerts.Size = new System.Drawing.Size(173, 22);
            this.mnuReloadAlerts.Text = "&Reload Alerts";
            this.mnuReloadAlerts.Click += new System.EventHandler(this.mnuReloadAlerts_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(170, 6);
            // 
            // mnuEditGlobalAlerts
            // 
            this.mnuEditGlobalAlerts.Name = "mnuEditGlobalAlerts";
            this.mnuEditGlobalAlerts.Size = new System.Drawing.Size(173, 22);
            this.mnuEditGlobalAlerts.Text = "Edit &Global Alerts";
            this.mnuEditGlobalAlerts.Click += new System.EventHandler(this.mnuGlobalAlerts_Click);
            // 
            // mnuEditZoneAlerts
            // 
            this.mnuEditZoneAlerts.Name = "mnuEditZoneAlerts";
            this.mnuEditZoneAlerts.Size = new System.Drawing.Size(173, 22);
            this.mnuEditZoneAlerts.Text = "Edit &Zone Alerts";
            this.mnuEditZoneAlerts.Click += new System.EventHandler(this.mnuAddEditAlerts_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(170, 6);
            // 
            // mnuRefreshSpawnList
            // 
            this.mnuRefreshSpawnList.Name = "mnuRefreshSpawnList";
            this.mnuRefreshSpawnList.Size = new System.Drawing.Size(173, 22);
            this.mnuRefreshSpawnList.Text = "&Refresh Spawn List";
            this.mnuRefreshSpawnList.Click += new System.EventHandler(this.mnuRefreshSpawnList_Click);
            // 
            // mnuClearSavedTimers
            // 
            this.mnuClearSavedTimers.Name = "mnuClearSavedTimers";
            this.mnuClearSavedTimers.Size = new System.Drawing.Size(173, 22);
            this.mnuClearSavedTimers.Text = "Clear Saved &Timers";
            this.mnuClearSavedTimers.Click += new System.EventHandler(this.mnuClearSavedTimers_Click);
            // 
            // mnuSaveSpawnLog
            // 
            this.mnuSaveSpawnLog.Name = "mnuSaveSpawnLog";
            this.mnuSaveSpawnLog.Size = new System.Drawing.Size(173, 22);
            this.mnuSaveSpawnLog.Text = "Save Spawn Log";
            this.mnuSaveSpawnLog.Click += new System.EventHandler(this.mnuSaveSpawnLog_Click);
            // 
            // mnuViewMain
            // 
            this.mnuViewMain.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
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
            this.mnuViewMain.Size = new System.Drawing.Size(44, 20);
            this.mnuViewMain.Text = "&View";
            // 
            // toolbarsToolStripMenuItem
            // 
            this.toolbarsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuViewMenuBar,
            this.mnuViewStatusBar,
            this.mnuViewDepthFilterBar});
            this.toolbarsToolStripMenuItem.Name = "toolbarsToolStripMenuItem";
            this.toolbarsToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.toolbarsToolStripMenuItem.Text = "Toolbars";
            // 
            // mnuViewMenuBar
            // 
            this.mnuViewMenuBar.Name = "mnuViewMenuBar";
            this.mnuViewMenuBar.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this.mnuViewMenuBar.Size = new System.Drawing.Size(162, 22);
            this.mnuViewMenuBar.Text = "&Main Menu";
            this.mnuViewMenuBar.Click += new System.EventHandler(this.mnuShowMenuBar_Click);
            // 
            // mnuViewStatusBar
            // 
            this.mnuViewStatusBar.Name = "mnuViewStatusBar";
            this.mnuViewStatusBar.ShortcutKeys = System.Windows.Forms.Keys.F3;
            this.mnuViewStatusBar.Size = new System.Drawing.Size(162, 22);
            this.mnuViewStatusBar.Text = "&Status";
            this.mnuViewStatusBar.Click += new System.EventHandler(this.mnuViewStatusBar_Click);
            // 
            // mnuViewDepthFilterBar
            // 
            this.mnuViewDepthFilterBar.Name = "mnuViewDepthFilterBar";
            this.mnuViewDepthFilterBar.ShortcutKeys = System.Windows.Forms.Keys.F4;
            this.mnuViewDepthFilterBar.Size = new System.Drawing.Size(162, 22);
            this.mnuViewDepthFilterBar.Text = "&Tool Bar Strip";
            this.mnuViewDepthFilterBar.Click += new System.EventHandler(this.mnuViewDepthFilterToolBar_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(169, 6);
            // 
            // mnuShowSpawnList
            // 
            this.mnuShowSpawnList.Name = "mnuShowSpawnList";
            this.mnuShowSpawnList.Size = new System.Drawing.Size(172, 22);
            this.mnuShowSpawnList.Text = "Spawn &List";
            this.mnuShowSpawnList.Click += new System.EventHandler(this.mnuShowSpawnList_Click);
            // 
            // mnuShowSpawnListTimer
            // 
            this.mnuShowSpawnListTimer.Name = "mnuShowSpawnListTimer";
            this.mnuShowSpawnListTimer.Size = new System.Drawing.Size(172, 22);
            this.mnuShowSpawnListTimer.Text = "Spawn &Timer List";
            this.mnuShowSpawnListTimer.Click += new System.EventHandler(this.mnuShowSpawnListTimer_Click);
            // 
            // mnuShowGroundItemList
            // 
            this.mnuShowGroundItemList.Name = "mnuShowGroundItemList";
            this.mnuShowGroundItemList.Size = new System.Drawing.Size(172, 22);
            this.mnuShowGroundItemList.Text = "Ground &Item List";
            this.mnuShowGroundItemList.Click += new System.EventHandler(this.mnuShowGroundItemList_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(169, 6);
            // 
            // mnuShowListGridLines
            // 
            this.mnuShowListGridLines.Name = "mnuShowListGridLines";
            this.mnuShowListGridLines.Size = new System.Drawing.Size(172, 22);
            this.mnuShowListGridLines.Text = "List &Grid Lines";
            this.mnuShowListGridLines.Click += new System.EventHandler(this.mnuShowListGridLines_Click);
            // 
            // mnuShowListSearchBox
            // 
            this.mnuShowListSearchBox.Name = "mnuShowListSearchBox";
            this.mnuShowListSearchBox.Size = new System.Drawing.Size(172, 22);
            this.mnuShowListSearchBox.Text = "List Search Box";
            this.mnuShowListSearchBox.Click += new System.EventHandler(this.mnuShowListSearchBox_Click);
            // 
            // mnuShowGridLines
            // 
            this.mnuShowGridLines.Image = ((System.Drawing.Image)(resources.GetObject("mnuShowGridLines.Image")));
            this.mnuShowGridLines.Name = "mnuShowGridLines";
            this.mnuShowGridLines.ShortcutKeys = System.Windows.Forms.Keys.F6;
            this.mnuShowGridLines.Size = new System.Drawing.Size(172, 22);
            this.mnuShowGridLines.Text = "Map Grid Lines";
            this.mnuShowGridLines.Click += new System.EventHandler(this.mnuShowGridLines_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(169, 6);
            // 
            // mnuShowCorpses
            // 
            this.mnuShowCorpses.Name = "mnuShowCorpses";
            this.mnuShowCorpses.Size = new System.Drawing.Size(172, 22);
            this.mnuShowCorpses.Text = "NPC Corpses";
            this.mnuShowCorpses.Click += new System.EventHandler(this.mnuShowCorpses_Click);
            // 
            // mnuShowPCCorpses
            // 
            this.mnuShowPCCorpses.Name = "mnuShowPCCorpses";
            this.mnuShowPCCorpses.Size = new System.Drawing.Size(172, 22);
            this.mnuShowPCCorpses.Text = "PC Corpses";
            this.mnuShowPCCorpses.Click += new System.EventHandler(this.mnuShowPCCorpses_Click);
            // 
            // mnuShowMyCorpse
            // 
            this.mnuShowMyCorpse.Name = "mnuShowMyCorpse";
            this.mnuShowMyCorpse.Size = new System.Drawing.Size(172, 22);
            this.mnuShowMyCorpse.Text = "My Corpse";
            this.mnuShowMyCorpse.Click += new System.EventHandler(this.mnuShowMyCorpse_Click);
            // 
            // mnuShowPlayers
            // 
            this.mnuShowPlayers.Name = "mnuShowPlayers";
            this.mnuShowPlayers.Size = new System.Drawing.Size(172, 22);
            this.mnuShowPlayers.Text = "Players";
            this.mnuShowPlayers.Click += new System.EventHandler(this.mnuShowPlayers_Click);
            // 
            // mnuShowInvis
            // 
            this.mnuShowInvis.Name = "mnuShowInvis";
            this.mnuShowInvis.Size = new System.Drawing.Size(172, 22);
            this.mnuShowInvis.Text = "Invis Mobs";
            this.mnuShowInvis.Click += new System.EventHandler(this.mnuShowInvis_Click);
            // 
            // mnuShowMounts
            // 
            this.mnuShowMounts.Name = "mnuShowMounts";
            this.mnuShowMounts.Size = new System.Drawing.Size(172, 22);
            this.mnuShowMounts.Text = "Mounts";
            this.mnuShowMounts.Click += new System.EventHandler(this.mnuShowMounts_Click);
            // 
            // mnuShowFamiliars
            // 
            this.mnuShowFamiliars.Name = "mnuShowFamiliars";
            this.mnuShowFamiliars.Size = new System.Drawing.Size(172, 22);
            this.mnuShowFamiliars.Text = "Familiars";
            this.mnuShowFamiliars.Click += new System.EventHandler(this.mnuShowFamiliars_Click);
            // 
            // mnuShowPets
            // 
            this.mnuShowPets.Name = "mnuShowPets";
            this.mnuShowPets.Size = new System.Drawing.Size(172, 22);
            this.mnuShowPets.Text = "Pets";
            this.mnuShowPets.Click += new System.EventHandler(this.mnuShowPets_Click);
            // 
            // mnuShowNPCs
            // 
            this.mnuShowNPCs.Name = "mnuShowNPCs";
            this.mnuShowNPCs.Size = new System.Drawing.Size(172, 22);
            this.mnuShowNPCs.Text = "NPCs";
            this.mnuShowNPCs.Click += new System.EventHandler(this.mnuShowNPCs_Click);
            // 
            // mnuShowLookupText
            // 
            this.mnuShowLookupText.Name = "mnuShowLookupText";
            this.mnuShowLookupText.Size = new System.Drawing.Size(172, 22);
            this.mnuShowLookupText.Text = "Lookup Text";
            this.mnuShowLookupText.Click += new System.EventHandler(this.mnuShowLookupText_Click);
            // 
            // mnuShowLookupNumber
            // 
            this.mnuShowLookupNumber.Name = "mnuShowLookupNumber";
            this.mnuShowLookupNumber.Size = new System.Drawing.Size(172, 22);
            this.mnuShowLookupNumber.Text = "Lookup Name/Number";
            this.mnuShowLookupNumber.Click += new System.EventHandler(this.mnuShowLookupNumber_Click);
            // 
            // mnuAlwaysOnTop
            // 
            this.mnuAlwaysOnTop.Name = "mnuAlwaysOnTop";
            this.mnuAlwaysOnTop.Size = new System.Drawing.Size(172, 22);
            this.mnuAlwaysOnTop.Text = "Always On Top";
            this.mnuAlwaysOnTop.Click += new System.EventHandler(this.mnuAlwaysOnTop_Click);
            // 
            // mnuMapSettingsMain
            // 
            this.mnuMapSettingsMain.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
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
            this.mnuMapSettingsMain.Size = new System.Drawing.Size(43, 20);
            this.mnuMapSettingsMain.Text = "&Map";
            // 
            // mnuDepthFilter
            // 
            this.mnuDepthFilter.Name = "mnuDepthFilter";
            this.mnuDepthFilter.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.mnuDepthFilter.Size = new System.Drawing.Size(195, 22);
            this.mnuDepthFilter.Text = "&Depth Filter On/Off";
            this.mnuDepthFilter.ToolTipText = "Z-Axis Depth Filtering";
            this.mnuDepthFilter.Click += new System.EventHandler(this.mnuDepthFilter_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
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
            this.menuItem3.Size = new System.Drawing.Size(195, 22);
            this.menuItem3.Text = "Depth &Filter Settings";
            // 
            // mnuDynamicAlpha
            // 
            this.mnuDynamicAlpha.Name = "mnuDynamicAlpha";
            this.mnuDynamicAlpha.Size = new System.Drawing.Size(220, 22);
            this.mnuDynamicAlpha.Text = "Dynamic &Alpha Faded Lines";
            this.mnuDynamicAlpha.ToolTipText = "Faded Depth Filtered Lines.";
            this.mnuDynamicAlpha.Click += new System.EventHandler(this.mnuDynamicAlpha_Click);
            // 
            // mnuFilterMapLines
            // 
            this.mnuFilterMapLines.Name = "mnuFilterMapLines";
            this.mnuFilterMapLines.Size = new System.Drawing.Size(220, 22);
            this.mnuFilterMapLines.Text = "Filter &Map Lines";
            this.mnuFilterMapLines.Click += new System.EventHandler(this.mnuFilterMapLines_Click);
            // 
            // mnuFilterMapText
            // 
            this.mnuFilterMapText.Name = "mnuFilterMapText";
            this.mnuFilterMapText.Size = new System.Drawing.Size(220, 22);
            this.mnuFilterMapText.Text = "Filter Map &Text";
            this.mnuFilterMapText.Click += new System.EventHandler(this.mnuFilterMapText_Click);
            // 
            // mnuFilterNPCs
            // 
            this.mnuFilterNPCs.Name = "mnuFilterNPCs";
            this.mnuFilterNPCs.Size = new System.Drawing.Size(220, 22);
            this.mnuFilterNPCs.Text = "Filter &NPCs";
            this.mnuFilterNPCs.Click += new System.EventHandler(this.mnuFilterNPCs_Click);
            // 
            // mnuFilterNPCCorpses
            // 
            this.mnuFilterNPCCorpses.Name = "mnuFilterNPCCorpses";
            this.mnuFilterNPCCorpses.Size = new System.Drawing.Size(220, 22);
            this.mnuFilterNPCCorpses.Text = "Filter NPC &Corpses";
            this.mnuFilterNPCCorpses.Click += new System.EventHandler(this.mnuFilterNPCCorpses_Click);
            // 
            // mnuFilterPlayers
            // 
            this.mnuFilterPlayers.Name = "mnuFilterPlayers";
            this.mnuFilterPlayers.Size = new System.Drawing.Size(220, 22);
            this.mnuFilterPlayers.Text = "Filter &Players";
            this.mnuFilterPlayers.Click += new System.EventHandler(this.mnuFilterPlayers_Click);
            // 
            // mnuFilterPlayerCorpses
            // 
            this.mnuFilterPlayerCorpses.Name = "mnuFilterPlayerCorpses";
            this.mnuFilterPlayerCorpses.Size = new System.Drawing.Size(220, 22);
            this.mnuFilterPlayerCorpses.Text = "Filter Pl&ayer Corpses";
            this.mnuFilterPlayerCorpses.Click += new System.EventHandler(this.mnuFilterPlayerCorpses_Click);
            // 
            // mnuFilterGroundItems
            // 
            this.mnuFilterGroundItems.Name = "mnuFilterGroundItems";
            this.mnuFilterGroundItems.Size = new System.Drawing.Size(220, 22);
            this.mnuFilterGroundItems.Text = "Filter &Ground Items";
            this.mnuFilterGroundItems.Click += new System.EventHandler(this.mnuFilterGroundItems_Click);
            // 
            // mnuFilterSpawnPoints
            // 
            this.mnuFilterSpawnPoints.Name = "mnuFilterSpawnPoints";
            this.mnuFilterSpawnPoints.Size = new System.Drawing.Size(220, 22);
            this.mnuFilterSpawnPoints.Text = "Filter &Spawn Points";
            this.mnuFilterSpawnPoints.Click += new System.EventHandler(this.mnuFilterSpawnPoints_Click);
            // 
            // mnuForceDistinct
            // 
            this.mnuForceDistinct.Name = "mnuForceDistinct";
            this.mnuForceDistinct.Size = new System.Drawing.Size(195, 22);
            this.mnuForceDistinct.Text = "&Force Distinct Lines";
            this.mnuForceDistinct.Click += new System.EventHandler(this.mnuForceDistinct_Click);
            // 
            // mnuForceDistinctText
            // 
            this.mnuForceDistinctText.Name = "mnuForceDistinctText";
            this.mnuForceDistinctText.Size = new System.Drawing.Size(195, 22);
            this.mnuForceDistinctText.Text = "Force Distinct &Text";
            this.mnuForceDistinctText.Click += new System.EventHandler(this.mnuForceDistinctText_Click);
            // 
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            this.toolStripSeparator12.Size = new System.Drawing.Size(192, 6);
            // 
            // mnuLabelShow
            // 
            this.mnuLabelShow.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuShowNPCLevels,
            this.mnuShowNPCNames,
            this.mnuShowNPCCorpseNames,
            this.mnuShowPCNames,
            this.mnuShowPlayerCorpseNames,
            this.mnuSpawnCountdown,
            this.mnuShowSpawnPoints,
            this.mnuShowZoneText,
            this.mnuShowLayer1,
            this.mnuShowLayer2,
            this.mnuShowLayer3,
            this.mnuShowPVP,
            this.mnuShowPVPLevel});
            this.mnuLabelShow.Name = "mnuLabelShow";
            this.mnuLabelShow.Size = new System.Drawing.Size(195, 22);
            this.mnuLabelShow.Text = "&Show on Map";
            // 
            // mnuShowNPCLevels
            // 
            this.mnuShowNPCLevels.Name = "mnuShowNPCLevels";
            this.mnuShowNPCLevels.Size = new System.Drawing.Size(186, 22);
            this.mnuShowNPCLevels.Text = "NPC L&evels";
            this.mnuShowNPCLevels.ToolTipText = "Show NPC Levels on map.";
            this.mnuShowNPCLevels.Click += new System.EventHandler(this.mnuShowNPCLevels_Click);
            // 
            // mnuShowNPCNames
            // 
            this.mnuShowNPCNames.Name = "mnuShowNPCNames";
            this.mnuShowNPCNames.Size = new System.Drawing.Size(186, 22);
            this.mnuShowNPCNames.Text = "&NPC Names";
            this.mnuShowNPCNames.ToolTipText = "Show NPC Names on map.";
            this.mnuShowNPCNames.Click += new System.EventHandler(this.mnuShowNPCNames_Click);
            // 
            // mnuShowNPCCorpseNames
            // 
            this.mnuShowNPCCorpseNames.Name = "mnuShowNPCCorpseNames";
            this.mnuShowNPCCorpseNames.Size = new System.Drawing.Size(186, 22);
            this.mnuShowNPCCorpseNames.Text = "NPC &Corpse Names";
            this.mnuShowNPCCorpseNames.ToolTipText = "Show NPC Corpse Names on map.";
            this.mnuShowNPCCorpseNames.Click += new System.EventHandler(this.mnuShowNPCCorpseNames_Click);
            // 
            // mnuShowPCNames
            // 
            this.mnuShowPCNames.Name = "mnuShowPCNames";
            this.mnuShowPCNames.Size = new System.Drawing.Size(186, 22);
            this.mnuShowPCNames.Text = "&Player Names";
            this.mnuShowPCNames.ToolTipText = "Show Player Names on map.";
            this.mnuShowPCNames.Click += new System.EventHandler(this.mnuShowPCNames_Click);
            // 
            // mnuShowPlayerCorpseNames
            // 
            this.mnuShowPlayerCorpseNames.Name = "mnuShowPlayerCorpseNames";
            this.mnuShowPlayerCorpseNames.Size = new System.Drawing.Size(186, 22);
            this.mnuShowPlayerCorpseNames.Text = "Player Corpse &Names";
            this.mnuShowPlayerCorpseNames.ToolTipText = "Show Player Corpse Names on map.";
            this.mnuShowPlayerCorpseNames.Click += new System.EventHandler(this.mnuShowPlayerCorpseNames_Click);
            // 
            // mnuSpawnCountdown
            // 
            this.mnuSpawnCountdown.Name = "mnuSpawnCountdown";
            this.mnuSpawnCountdown.Size = new System.Drawing.Size(186, 22);
            this.mnuSpawnCountdown.Text = "Spawn Countdown";
            this.mnuSpawnCountdown.ToolTipText = "Show spawn countdown timers on map.";
            this.mnuSpawnCountdown.Click += new System.EventHandler(this.mnuSpawnCountdown_Click);
            // 
            // mnuShowSpawnPoints
            // 
            this.mnuShowSpawnPoints.Name = "mnuShowSpawnPoints";
            this.mnuShowSpawnPoints.Size = new System.Drawing.Size(186, 22);
            this.mnuShowSpawnPoints.Text = "&Spawn Points";
            this.mnuShowSpawnPoints.ToolTipText = "Draw a cross at spawn point on map.";
            this.mnuShowSpawnPoints.Click += new System.EventHandler(this.mnuShowSpawnPoints_Click);
            // 
            // mnuShowZoneText
            // 
            this.mnuShowZoneText.Name = "mnuShowZoneText";
            this.mnuShowZoneText.Size = new System.Drawing.Size(186, 22);
            this.mnuShowZoneText.Text = "&Zone Text";
            this.mnuShowZoneText.Click += new System.EventHandler(this.mnuShowZoneText_Click);
            // 
            // mnuShowLayer1
            // 
            this.mnuShowLayer1.Name = "mnuShowLayer1";
            this.mnuShowLayer1.Size = new System.Drawing.Size(186, 22);
            this.mnuShowLayer1.Text = "&Show Layer 1";
            this.mnuShowLayer1.Click += new System.EventHandler(this.mnuShowLayer1_Click);
            // 
            // mnuShowLayer2
            // 
            this.mnuShowLayer2.Name = "mnuShowLayer2";
            this.mnuShowLayer2.Size = new System.Drawing.Size(186, 22);
            this.mnuShowLayer2.Text = "&Show Layer 2";
            this.mnuShowLayer2.Click += new System.EventHandler(this.mnuShowLayer2_Click);
            // 
            // mnuShowLayer3
            // 
            this.mnuShowLayer3.Name = "mnuShowLayer23";
            this.mnuShowLayer3.Size = new System.Drawing.Size(186, 22);
            this.mnuShowLayer3.Text = "&Show Layer 3";
            this.mnuShowLayer3.Click += new System.EventHandler(this.mnuShowLayer3_Click);
            // 
            // mnuShowPVP
            // 
            this.mnuShowPVP.Name = "mnuShowPVP";
            this.mnuShowPVP.Size = new System.Drawing.Size(186, 22);
            this.mnuShowPVP.Text = "P&VP";
            this.mnuShowPVP.Click += new System.EventHandler(this.mnuShowPVP_Click);
            // 
            // mnuShowPVPLevel
            // 
            this.mnuShowPVPLevel.Name = "mnuShowPVPLevel";
            this.mnuShowPVPLevel.Size = new System.Drawing.Size(186, 22);
            this.mnuShowPVPLevel.Text = "PVP &Level";
            this.mnuShowPVPLevel.Click += new System.EventHandler(this.mnuShowPVPLevel_Click);
            // 
            // mnuCollectMobTrails
            // 
            this.mnuCollectMobTrails.Name = "mnuCollectMobTrails";
            this.mnuCollectMobTrails.Size = new System.Drawing.Size(195, 22);
            this.mnuCollectMobTrails.Text = "&Collect Mob Trails";
            this.mnuCollectMobTrails.Click += new System.EventHandler(this.mnuCollectMobTrails_Click);
            // 
            // mnuShowMobTrails
            // 
            this.mnuShowMobTrails.Name = "mnuShowMobTrails";
            this.mnuShowMobTrails.ShortcutKeys = System.Windows.Forms.Keys.F7;
            this.mnuShowMobTrails.Size = new System.Drawing.Size(195, 22);
            this.mnuShowMobTrails.Text = "Show &Mob Trails";
            this.mnuShowMobTrails.Click += new System.EventHandler(this.mnuShowMobTrails_Click);
            // 
            // mnuConColors
            // 
            this.mnuConColors.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuConDefault,
            this.mnuConSoD,
            this.mnuConSoF});
            this.mnuConColors.Name = "mnuConColors";
            this.mnuConColors.Size = new System.Drawing.Size(195, 22);
            this.mnuConColors.Text = "Con Colors";
            // 
            // mnuConDefault
            // 
            this.mnuConDefault.Name = "mnuConDefault";
            this.mnuConDefault.Size = new System.Drawing.Size(172, 22);
            this.mnuConDefault.Text = "Default";
            this.mnuConDefault.Click += new System.EventHandler(this.mnuConDefault_Click);
            // 
            // mnuConSoD
            // 
            this.mnuConSoD.Name = "mnuConSoD";
            this.mnuConSoD.Size = new System.Drawing.Size(172, 22);
            this.mnuConSoD.Text = "SoD / Titanium";
            this.mnuConSoD.Click += new System.EventHandler(this.mnuSodTitanium_Click);
            // 
            // mnuConSoF
            // 
            this.mnuConSoF.Name = "mnuConSoF";
            this.mnuConSoF.Size = new System.Drawing.Size(172, 22);
            this.mnuConSoF.Text = "Secrets of Faydwer";
            this.mnuConSoF.Click += new System.EventHandler(this.mnuConSoF_Click);
            // 
            // mnuGridInterval
            // 
            this.mnuGridInterval.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuGridInterval100,
            this.mnuGridInterval250,
            this.mnuGridInterval500,
            this.mnuGridInterval1000});
            this.mnuGridInterval.Name = "mnuGridInterval";
            this.mnuGridInterval.Size = new System.Drawing.Size(195, 22);
            this.mnuGridInterval.Text = "Grid &Interval";
            // 
            // mnuGridInterval100
            // 
            this.mnuGridInterval100.Name = "mnuGridInterval100";
            this.mnuGridInterval100.Size = new System.Drawing.Size(98, 22);
            this.mnuGridInterval100.Text = "100";
            this.mnuGridInterval100.Click += new System.EventHandler(this.mnuGridInterval_Click);
            // 
            // mnuGridInterval250
            // 
            this.mnuGridInterval250.Name = "mnuGridInterval250";
            this.mnuGridInterval250.Size = new System.Drawing.Size(98, 22);
            this.mnuGridInterval250.Text = "250";
            this.mnuGridInterval250.Click += new System.EventHandler(this.mnuGridInterval_Click);
            // 
            // mnuGridInterval500
            // 
            this.mnuGridInterval500.Checked = true;
            this.mnuGridInterval500.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mnuGridInterval500.Name = "mnuGridInterval500";
            this.mnuGridInterval500.Size = new System.Drawing.Size(98, 22);
            this.mnuGridInterval500.Text = "500";
            this.mnuGridInterval500.Click += new System.EventHandler(this.mnuGridInterval_Click);
            // 
            // mnuGridInterval1000
            // 
            this.mnuGridInterval1000.Name = "mnuGridInterval1000";
            this.mnuGridInterval1000.Size = new System.Drawing.Size(98, 22);
            this.mnuGridInterval1000.Text = "1000";
            this.mnuGridInterval1000.Click += new System.EventHandler(this.mnuGridInterval_Click);
            // 
            // mnuShowTargetInfo
            // 
            this.mnuShowTargetInfo.Name = "mnuShowTargetInfo";
            this.mnuShowTargetInfo.ShortcutKeys = System.Windows.Forms.Keys.F9;
            this.mnuShowTargetInfo.Size = new System.Drawing.Size(195, 22);
            this.mnuShowTargetInfo.Text = "Show &Target Info";
            this.mnuShowTargetInfo.Click += new System.EventHandler(this.mnuShowTargetInfo_Click);
            // 
            // mnuSmallTargetInfo
            // 
            this.mnuSmallTargetInfo.Name = "mnuSmallTargetInfo";
            this.mnuSmallTargetInfo.Size = new System.Drawing.Size(195, 22);
            this.mnuSmallTargetInfo.Text = "Small Target &Info";
            this.mnuSmallTargetInfo.Click += new System.EventHandler(this.mnuSmallTargetInfo_Click);
            // 
            // mnuAutoSelectEQTarget
            // 
            this.mnuAutoSelectEQTarget.Name = "mnuAutoSelectEQTarget";
            this.mnuAutoSelectEQTarget.Size = new System.Drawing.Size(195, 22);
            this.mnuAutoSelectEQTarget.Text = "Auto Select &EQ Target";
            this.mnuAutoSelectEQTarget.Click += new System.EventHandler(this.mnuAutoSelectEQTarget_Click);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(192, 6);
            // 
            // mnuFollowNone
            // 
            this.mnuFollowNone.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.mnuFollowNone.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnuFollowNone.Name = "mnuFollowNone";
            this.mnuFollowNone.Size = new System.Drawing.Size(195, 22);
            this.mnuFollowNone.Text = "No Follow";
            this.mnuFollowNone.Click += new System.EventHandler(this.mnuFollowNone_Click);
            // 
            // mnuFollowPlayer
            // 
            this.mnuFollowPlayer.Image = ((System.Drawing.Image)(resources.GetObject("mnuFollowPlayer.Image")));
            this.mnuFollowPlayer.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.mnuFollowPlayer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnuFollowPlayer.Name = "mnuFollowPlayer";
            this.mnuFollowPlayer.Size = new System.Drawing.Size(195, 22);
            this.mnuFollowPlayer.Text = "Follow Player";
            this.mnuFollowPlayer.Click += new System.EventHandler(this.mnuFollowPlayer_Click);
            // 
            // mnuFollowTarget
            // 
            this.mnuFollowTarget.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.mnuFollowTarget.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnuFollowTarget.Name = "mnuFollowTarget";
            this.mnuFollowTarget.Size = new System.Drawing.Size(195, 22);
            this.mnuFollowTarget.Text = "Follow Target";
            this.mnuFollowTarget.Click += new System.EventHandler(this.mnuFollowTarget_Click);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(192, 6);
            // 
            // mnuKeepCentered
            // 
            this.mnuKeepCentered.Name = "mnuKeepCentered";
            this.mnuKeepCentered.Size = new System.Drawing.Size(195, 22);
            this.mnuKeepCentered.Text = "Keep Centered";
            this.mnuKeepCentered.Click += new System.EventHandler(this.mnuKeepCentered_Click);
            // 
            // mnuAutoExpand
            // 
            this.mnuAutoExpand.Name = "mnuAutoExpand";
            this.mnuAutoExpand.Size = new System.Drawing.Size(195, 22);
            this.mnuAutoExpand.Text = "Auto Expand";
            this.mnuAutoExpand.Click += new System.EventHandler(this.mnuAutoExpand_Click);
            // 
            // toolStripSeparator13
            // 
            this.toolStripSeparator13.Name = "toolStripSeparator13";
            this.toolStripSeparator13.Size = new System.Drawing.Size(192, 6);
            // 
            // mnuMapReset
            // 
            this.mnuMapReset.Name = "mnuMapReset";
            this.mnuMapReset.Size = new System.Drawing.Size(195, 22);
            this.mnuMapReset.Text = "Reset Map";
            this.mnuMapReset.Click += new System.EventHandler(this.mnuMapReset_Click);
            // 
            // mnuHelpMain
            // 
            this.mnuHelpMain.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuAbout});
            this.mnuHelpMain.Name = "mnuHelpMain";
            this.mnuHelpMain.Size = new System.Drawing.Size(44, 20);
            this.mnuHelpMain.Text = "&Help";
            // 
            // mnuAbout
            // 
            this.mnuAbout.Name = "mnuAbout";
            this.mnuAbout.Size = new System.Drawing.Size(107, 22);
            this.mnuAbout.Text = "About";
            this.mnuAbout.Click += new System.EventHandler(this.mnuAbout_Click);
            // 
            // mnuContext
            // 
            this.mnuContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
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
            this.mnuContext.Size = new System.Drawing.Size(196, 380);
            // 
            // mnuDepthFilter2
            // 
            this.mnuDepthFilter2.Name = "mnuDepthFilter2";
            this.mnuDepthFilter2.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.mnuDepthFilter2.Size = new System.Drawing.Size(195, 22);
            this.mnuDepthFilter2.Text = "&Depth Filter On/Off";
            this.mnuDepthFilter2.Click += new System.EventHandler(this.mnuDepthFilter_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
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
            this.toolStripMenuItem2.Size = new System.Drawing.Size(195, 22);
            this.toolStripMenuItem2.Text = "Depth &Filter Settings";
            // 
            // mnuDynamicAlpha2
            // 
            this.mnuDynamicAlpha2.Name = "mnuDynamicAlpha2";
            this.mnuDynamicAlpha2.Size = new System.Drawing.Size(220, 22);
            this.mnuDynamicAlpha2.Text = "Dynamic &Alpha Faded Lines";
            this.mnuDynamicAlpha2.Click += new System.EventHandler(this.mnuDynamicAlpha_Click);
            // 
            // mnuFilterMapLines2
            // 
            this.mnuFilterMapLines2.Name = "mnuFilterMapLines2";
            this.mnuFilterMapLines2.Size = new System.Drawing.Size(220, 22);
            this.mnuFilterMapLines2.Text = "Filter &Map Lines";
            this.mnuFilterMapLines2.Click += new System.EventHandler(this.mnuFilterMapLines_Click);
            // 
            // mnuFilterMapText2
            // 
            this.mnuFilterMapText2.Name = "mnuFilterMapText2";
            this.mnuFilterMapText2.Size = new System.Drawing.Size(220, 22);
            this.mnuFilterMapText2.Text = "Filter Map &Text";
            this.mnuFilterMapText2.Click += new System.EventHandler(this.mnuFilterMapText_Click);
            // 
            // mnuFilterNPCs2
            // 
            this.mnuFilterNPCs2.Name = "mnuFilterNPCs2";
            this.mnuFilterNPCs2.Size = new System.Drawing.Size(220, 22);
            this.mnuFilterNPCs2.Text = "Filter &NPCs";
            this.mnuFilterNPCs2.Click += new System.EventHandler(this.mnuFilterNPCs_Click);
            // 
            // mnuFilterNPCCorpses2
            // 
            this.mnuFilterNPCCorpses2.Name = "mnuFilterNPCCorpses2";
            this.mnuFilterNPCCorpses2.Size = new System.Drawing.Size(220, 22);
            this.mnuFilterNPCCorpses2.Text = "Filter NPC &Corpses";
            this.mnuFilterNPCCorpses2.Click += new System.EventHandler(this.mnuFilterNPCCorpses_Click);
            // 
            // mnuFilterPlayers2
            // 
            this.mnuFilterPlayers2.Name = "mnuFilterPlayers2";
            this.mnuFilterPlayers2.Size = new System.Drawing.Size(220, 22);
            this.mnuFilterPlayers2.Text = "Filter &Players";
            this.mnuFilterPlayers2.Click += new System.EventHandler(this.mnuFilterPlayers_Click);
            // 
            // mnuFilterPlayerCorpses2
            // 
            this.mnuFilterPlayerCorpses2.Name = "mnuFilterPlayerCorpses2";
            this.mnuFilterPlayerCorpses2.Size = new System.Drawing.Size(220, 22);
            this.mnuFilterPlayerCorpses2.Text = "Filter Pl&ayer Corpses";
            this.mnuFilterPlayerCorpses2.Click += new System.EventHandler(this.mnuFilterPlayerCorpses_Click);
            // 
            // mnuFilterGroundItems2
            // 
            this.mnuFilterGroundItems2.Name = "mnuFilterGroundItems2";
            this.mnuFilterGroundItems2.Size = new System.Drawing.Size(220, 22);
            this.mnuFilterGroundItems2.Text = "Filter &Ground Items";
            this.mnuFilterGroundItems2.Click += new System.EventHandler(this.mnuFilterGroundItems_Click);
            // 
            // mnuFilterSpawnPoints2
            // 
            this.mnuFilterSpawnPoints2.Name = "mnuFilterSpawnPoints2";
            this.mnuFilterSpawnPoints2.Size = new System.Drawing.Size(220, 22);
            this.mnuFilterSpawnPoints2.Text = "Filter &Spawn Points";
            this.mnuFilterSpawnPoints2.Click += new System.EventHandler(this.mnuFilterSpawnPoints_Click);
            // 
            // mnuForceDistinct2
            // 
            this.mnuForceDistinct2.Name = "mnuForceDistinct2";
            this.mnuForceDistinct2.Size = new System.Drawing.Size(195, 22);
            this.mnuForceDistinct2.Text = "&Force Distinct Lines";
            this.mnuForceDistinct2.Click += new System.EventHandler(this.mnuForceDistinct_Click);
            // 
            // mnuForceDistinctText2
            // 
            this.mnuForceDistinctText2.Name = "mnuForceDistinctText2";
            this.mnuForceDistinctText2.Size = new System.Drawing.Size(195, 22);
            this.mnuForceDistinctText2.Text = "Force Distinct &Text";
            this.mnuForceDistinctText2.Click += new System.EventHandler(this.mnuForceDistinctText_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(192, 6);
            // 
            // addMapTextToolStripMenuItem
            // 
            this.addMapTextToolStripMenuItem.Name = "addMapTextToolStripMenuItem";
            this.addMapTextToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.addMapTextToolStripMenuItem.Text = "Add Map Text";
            this.addMapTextToolStripMenuItem.ToolTipText = "Add Map Text to your current location.";
            this.addMapTextToolStripMenuItem.Click += new System.EventHandler(this.addMapTextToolStripMenuItem_Click);
            // 
            // mnuLabelShow2
            // 
            this.mnuLabelShow2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuShowNPCLevels2,
            this.mnuShowNPCNames2,
            this.mnuShowNPCCorpseNames2,
            this.mnuShowPCNames2,
            this.mnuShowPlayerCorpseNames2,
            this.mnuSpawnCountdown2,
            this.mnuShowSpawnPoints2,
            this.mnuShowZoneText2,
            this.mnuShowLayer21,
            this.mnuShowLayer22,
            this.mnuShowLayer23,
            this.mnuShowPVP2,
            this.mnuShowPVPLevel2});
            this.mnuLabelShow2.Name = "mnuLabelShow2";
            this.mnuLabelShow2.Size = new System.Drawing.Size(195, 22);
            this.mnuLabelShow2.Text = "&Show on Map";
            // 
            // mnuShowNPCLevels2
            // 
            this.mnuShowNPCLevels2.Name = "mnuShowNPCLevels2";
            this.mnuShowNPCLevels2.Size = new System.Drawing.Size(186, 22);
            this.mnuShowNPCLevels2.Text = "NPC L&evels";
            this.mnuShowNPCLevels2.Click += new System.EventHandler(this.mnuShowNPCLevels_Click);
            // 
            // mnuShowNPCNames2
            // 
            this.mnuShowNPCNames2.Name = "mnuShowNPCNames2";
            this.mnuShowNPCNames2.Size = new System.Drawing.Size(186, 22);
            this.mnuShowNPCNames2.Text = "&NPC Names";
            this.mnuShowNPCNames2.Click += new System.EventHandler(this.mnuShowNPCNames_Click);
            // 
            // mnuShowNPCCorpseNames2
            // 
            this.mnuShowNPCCorpseNames2.Name = "mnuShowNPCCorpseNames2";
            this.mnuShowNPCCorpseNames2.Size = new System.Drawing.Size(186, 22);
            this.mnuShowNPCCorpseNames2.Text = "NPC &Corpse Names";
            this.mnuShowNPCCorpseNames2.Click += new System.EventHandler(this.mnuShowNPCCorpseNames_Click);
            // 
            // mnuShowPCNames2
            // 
            this.mnuShowPCNames2.Name = "mnuShowPCNames2";
            this.mnuShowPCNames2.Size = new System.Drawing.Size(186, 22);
            this.mnuShowPCNames2.Text = "&Player Names";
            this.mnuShowPCNames2.Click += new System.EventHandler(this.mnuShowPCNames_Click);
            // 
            // mnuShowPlayerCorpseNames2
            // 
            this.mnuShowPlayerCorpseNames2.Name = "mnuShowPlayerCorpseNames2";
            this.mnuShowPlayerCorpseNames2.Size = new System.Drawing.Size(186, 22);
            this.mnuShowPlayerCorpseNames2.Text = "Player Corpse &Names";
            this.mnuShowPlayerCorpseNames2.Click += new System.EventHandler(this.mnuShowPlayerCorpseNames_Click);
            // 
            // mnuSpawnCountdown2
            // 
            this.mnuSpawnCountdown2.Name = "mnuSpawnCountdown2";
            this.mnuSpawnCountdown2.Size = new System.Drawing.Size(186, 22);
            this.mnuSpawnCountdown2.Text = "Spawn Countdown";
            this.mnuSpawnCountdown2.Click += new System.EventHandler(this.mnuSpawnCountdown_Click);
            // 
            // mnuShowSpawnPoints2
            // 
            this.mnuShowSpawnPoints2.Name = "mnuShowSpawnPoints2";
            this.mnuShowSpawnPoints2.Size = new System.Drawing.Size(186, 22);
            this.mnuShowSpawnPoints2.Text = "&Spawn Points";
            this.mnuShowSpawnPoints2.Click += new System.EventHandler(this.mnuShowSpawnPoints_Click);
            // 
            // mnuShowZoneText2
            // 
            this.mnuShowZoneText2.Name = "mnuShowZoneText2";
            this.mnuShowZoneText2.Size = new System.Drawing.Size(186, 22);
            this.mnuShowZoneText2.Text = "&Zone Text";
            this.mnuShowZoneText2.Click += new System.EventHandler(this.mnuShowZoneText_Click);
            // 
            // mnuShowLayer21
            // 
            this.mnuShowLayer21.Name = "mnuShowLayer21";
            this.mnuShowLayer21.Size = new System.Drawing.Size(186, 22);
            this.mnuShowLayer21.Text = "&Show Layer 1";
            this.mnuShowLayer21.Click += new System.EventHandler(this.mnuShowLayer1_Click);
            // 
            // mnuShowLayer22
            // 
            this.mnuShowLayer22.Name = "mnuShowLayer22";
            this.mnuShowLayer22.Size = new System.Drawing.Size(186, 22);
            this.mnuShowLayer22.Text = "&Show Layer 2";
            this.mnuShowLayer22.Click += new System.EventHandler(this.mnuShowLayer2_Click);
            // 
            // mnuShowLayer23
            // 
            this.mnuShowLayer23.Name = "mnuShowLayer23";
            this.mnuShowLayer23.Size = new System.Drawing.Size(186, 22);
            this.mnuShowLayer23.Text = "&Show Layer 3";
            this.mnuShowLayer23.Click += new System.EventHandler(this.mnuShowLayer3_Click);
            // 
            // mnuShowPVP2
            // 
            this.mnuShowPVP2.Name = "mnuShowPVP2";
            this.mnuShowPVP2.Size = new System.Drawing.Size(186, 22);
            this.mnuShowPVP2.Text = "P&VP";
            this.mnuShowPVP2.Click += new System.EventHandler(this.mnuShowPVP_Click);
            // 
            // mnuShowPVPLevel2
            // 
            this.mnuShowPVPLevel2.Name = "mnuShowPVPLevel2";
            this.mnuShowPVPLevel2.Size = new System.Drawing.Size(186, 22);
            this.mnuShowPVPLevel2.Text = "PVP &Level";
            this.mnuShowPVPLevel2.Click += new System.EventHandler(this.mnuShowPVPLevel_Click);
            // 
            // mnuShowTargetInfo2
            // 
            this.mnuShowTargetInfo2.Name = "mnuShowTargetInfo2";
            this.mnuShowTargetInfo2.ShortcutKeys = System.Windows.Forms.Keys.F9;
            this.mnuShowTargetInfo2.Size = new System.Drawing.Size(195, 22);
            this.mnuShowTargetInfo2.Text = "Show &Target Info";
            this.mnuShowTargetInfo2.Click += new System.EventHandler(this.mnuShowTargetInfo_Click);
            // 
            // mnuSmallTargetInfo2
            // 
            this.mnuSmallTargetInfo2.Name = "mnuSmallTargetInfo2";
            this.mnuSmallTargetInfo2.Size = new System.Drawing.Size(195, 22);
            this.mnuSmallTargetInfo2.Text = "Small Target &Info";
            this.mnuSmallTargetInfo2.Click += new System.EventHandler(this.mnuSmallTargetInfo2_Click);
            // 
            // mnuAutoSelectEQTarget2
            // 
            this.mnuAutoSelectEQTarget2.Name = "mnuAutoSelectEQTarget2";
            this.mnuAutoSelectEQTarget2.Size = new System.Drawing.Size(195, 22);
            this.mnuAutoSelectEQTarget2.Text = "Auto Select &EQ Target";
            this.mnuAutoSelectEQTarget2.Click += new System.EventHandler(this.mnuAutoSelectEQTarget_Click);
            // 
            // toolStripSeparator15
            // 
            this.toolStripSeparator15.Name = "toolStripSeparator15";
            this.toolStripSeparator15.Size = new System.Drawing.Size(192, 6);
            // 
            // mnuFollowNone2
            // 
            this.mnuFollowNone2.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.mnuFollowNone2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnuFollowNone2.Name = "mnuFollowNone2";
            this.mnuFollowNone2.Size = new System.Drawing.Size(195, 22);
            this.mnuFollowNone2.Text = "No Follow";
            this.mnuFollowNone2.Click += new System.EventHandler(this.mnuFollowNone_Click);
            // 
            // mnuFollowPlayer2
            // 
            this.mnuFollowPlayer2.Image = ((System.Drawing.Image)(resources.GetObject("mnuFollowPlayer2.Image")));
            this.mnuFollowPlayer2.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.mnuFollowPlayer2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnuFollowPlayer2.Name = "mnuFollowPlayer2";
            this.mnuFollowPlayer2.Size = new System.Drawing.Size(195, 22);
            this.mnuFollowPlayer2.Text = "Follow Player";
            this.mnuFollowPlayer2.Click += new System.EventHandler(this.mnuFollowPlayer_Click);
            // 
            // mnuFollowTarget2
            // 
            this.mnuFollowTarget2.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.mnuFollowTarget2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnuFollowTarget2.Name = "mnuFollowTarget2";
            this.mnuFollowTarget2.Size = new System.Drawing.Size(195, 22);
            this.mnuFollowTarget2.Text = "Follow Target";
            this.mnuFollowTarget2.Click += new System.EventHandler(this.mnuFollowTarget_Click);
            // 
            // toolStripSeparator16
            // 
            this.toolStripSeparator16.Name = "toolStripSeparator16";
            this.toolStripSeparator16.Size = new System.Drawing.Size(192, 6);
            // 
            // mnuKeepCentered2
            // 
            this.mnuKeepCentered2.Name = "mnuKeepCentered2";
            this.mnuKeepCentered2.Size = new System.Drawing.Size(195, 22);
            this.mnuKeepCentered2.Text = "Keep Centered";
            this.mnuKeepCentered2.Click += new System.EventHandler(this.mnuKeepCentered_Click);
            // 
            // mnuAutoExpand2
            // 
            this.mnuAutoExpand2.Name = "mnuAutoExpand2";
            this.mnuAutoExpand2.Size = new System.Drawing.Size(195, 22);
            this.mnuAutoExpand2.Text = "Auto Expand";
            this.mnuAutoExpand2.Click += new System.EventHandler(this.mnuAutoExpand_Click);
            // 
            // toolStripSeparator17
            // 
            this.toolStripSeparator17.Name = "toolStripSeparator17";
            this.toolStripSeparator17.Size = new System.Drawing.Size(192, 6);
            // 
            // mnuShowMenuBar
            // 
            this.mnuShowMenuBar.Name = "mnuShowMenuBar";
            this.mnuShowMenuBar.Size = new System.Drawing.Size(195, 22);
            this.mnuShowMenuBar.Text = "Show Menu Bar";
            this.mnuShowMenuBar.Click += new System.EventHandler(this.mnuShowMenuBar_Click);
            // 
            // mnuMapReset2
            // 
            this.mnuMapReset2.Name = "mnuMapReset2";
            this.mnuMapReset2.Size = new System.Drawing.Size(195, 22);
            this.mnuMapReset2.Text = "Reset Map";
            this.mnuMapReset2.Click += new System.EventHandler(this.mnuMapReset_Click);
            // 
            // mnuContextAddFilter
            // 
            this.mnuContextAddFilter.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuMobName,
            this.menuItem11,
            this.mnuAddHuntFilter,
            this.mnuAddCautionFilter,
            this.mnuAddDangerFilter,
            this.mnuAddAlertFilter,
            this.addZoneEmailAlertFilterToolStripMenuItem,
            this.toolStripBasecon,
            this.mnuSepAddFilter,
            this.mnuAddMapLabel,
            this.toolStripSepAddMapLabel,
            this.mnuSearchAllakhazam});
            this.mnuContextAddFilter.Name = "mnuContextAddFilter";
            this.mnuContextAddFilter.Size = new System.Drawing.Size(229, 220);
            // 
            // mnuMobName
            // 
            this.mnuMobName.Enabled = false;
            this.mnuMobName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.mnuMobName.Name = "mnuMobName";
            this.mnuMobName.Size = new System.Drawing.Size(228, 22);
            this.mnuMobName.Text = "MobName PlaceHolder";
            // 
            // menuItem11
            // 
            this.menuItem11.Name = "menuItem11";
            this.menuItem11.Size = new System.Drawing.Size(225, 6);
            // 
            // mnuAddHuntFilter
            // 
            this.mnuAddHuntFilter.Name = "mnuAddHuntFilter";
            this.mnuAddHuntFilter.Size = new System.Drawing.Size(228, 22);
            this.mnuAddHuntFilter.Text = "Add Zone Hunt Alert Filter";
            this.mnuAddHuntFilter.Click += new System.EventHandler(this.mnuAddHuntFilter_Click);
            // 
            // mnuAddCautionFilter
            // 
            this.mnuAddCautionFilter.Name = "mnuAddCautionFilter";
            this.mnuAddCautionFilter.Size = new System.Drawing.Size(228, 22);
            this.mnuAddCautionFilter.Text = "Add Zone Caution Alert Filter";
            this.mnuAddCautionFilter.Click += new System.EventHandler(this.mnuAddCautionFilter_Click);
            // 
            // mnuAddDangerFilter
            // 
            this.mnuAddDangerFilter.Name = "mnuAddDangerFilter";
            this.mnuAddDangerFilter.Size = new System.Drawing.Size(228, 22);
            this.mnuAddDangerFilter.Text = "Add Zone Danger Alert Filter";
            this.mnuAddDangerFilter.Click += new System.EventHandler(this.mnuAddDangerFilter_Click);
            // 
            // mnuAddAlertFilter
            // 
            this.mnuAddAlertFilter.Name = "mnuAddAlertFilter";
            this.mnuAddAlertFilter.Size = new System.Drawing.Size(228, 22);
            this.mnuAddAlertFilter.Text = "Add Zone Rare Alert Filter";
            this.mnuAddAlertFilter.Click += new System.EventHandler(this.mnuAddAlertFilter_Click);
            // 
            // addZoneEmailAlertFilterToolStripMenuItem
            // 
            this.addZoneEmailAlertFilterToolStripMenuItem.Name = "addZoneEmailAlertFilterToolStripMenuItem";
            this.addZoneEmailAlertFilterToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
            this.addZoneEmailAlertFilterToolStripMenuItem.Text = "Add Email Alert Filter";
            this.addZoneEmailAlertFilterToolStripMenuItem.Click += new System.EventHandler(this.addZoneEmailAlertFilterToolStripMenuItem_Click);
            // 
            // toolStripBasecon
            // 
            this.toolStripBasecon.CheckOnClick = true;
            this.toolStripBasecon.Font = new System.Drawing.Font("Tahoma", 8.400001F, System.Drawing.FontStyle.Bold);
            this.toolStripBasecon.Image = global::myseq.Properties.Resources.BlackX;
            this.toolStripBasecon.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBasecon.Name = "toolStripBasecon";
            this.toolStripBasecon.Size = new System.Drawing.Size(228, 22);
            this.toolStripBasecon.Text = "Base Con on this Spawn";
            // 
            // mnuSepAddFilter
            // 
            this.mnuSepAddFilter.Name = "mnuSepAddFilter";
            this.mnuSepAddFilter.Size = new System.Drawing.Size(225, 6);
            // 
            // mnuAddMapLabel
            // 
            this.mnuAddMapLabel.Name = "mnuAddMapLabel";
            this.mnuAddMapLabel.Size = new System.Drawing.Size(228, 22);
            this.mnuAddMapLabel.Text = "Add Map Label";
            this.mnuAddMapLabel.Click += new System.EventHandler(this.mnuAddMapLabel_Click);
            // 
            // toolStripSepAddMapLabel
            // 
            this.toolStripSepAddMapLabel.Name = "toolStripSepAddMapLabel";
            this.toolStripSepAddMapLabel.Size = new System.Drawing.Size(225, 6);
            // 
            // mnuSearchAllakhazam
            // 
            this.mnuSearchAllakhazam.Image = ((System.Drawing.Image)(resources.GetObject("mnuSearchAllakhazam.Image")));
            this.mnuSearchAllakhazam.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnuSearchAllakhazam.Name = "mnuSearchAllakhazam";
            this.mnuSearchAllakhazam.Size = new System.Drawing.Size(228, 22);
            this.mnuSearchAllakhazam.Text = "Search Allakhazam";
            this.mnuSearchAllakhazam.Click += new System.EventHandler(this.mnuSearchAllakhazam_Click);
            // 
            // timPackets
            // 
            this.timPackets.Tick += new System.EventHandler(this.timPackets_Tick);
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
            this.mnuShowListNPCs.Size = new System.Drawing.Size(32, 19);
            // 
            // mnuShowListCorpses
            // 
            this.mnuShowListCorpses.Name = "mnuShowListCorpses";
            this.mnuShowListCorpses.Size = new System.Drawing.Size(32, 19);
            // 
            // mnuShowListPlayers
            // 
            this.mnuShowListPlayers.Name = "mnuShowListPlayers";
            this.mnuShowListPlayers.Size = new System.Drawing.Size(32, 19);
            // 
            // mnuShowListInvis
            // 
            this.mnuShowListInvis.Name = "mnuShowListInvis";
            this.mnuShowListInvis.Size = new System.Drawing.Size(32, 19);
            // 
            // mnuShowListMounts
            // 
            this.mnuShowListMounts.Name = "mnuShowListMounts";
            this.mnuShowListMounts.Size = new System.Drawing.Size(32, 19);
            // 
            // mnuShowListFamiliars
            // 
            this.mnuShowListFamiliars.Name = "mnuShowListFamiliars";
            this.mnuShowListFamiliars.Size = new System.Drawing.Size(32, 19);
            // 
            // mnuShowListPets
            // 
            this.mnuShowListPets.Name = "mnuShowListPets";
            this.mnuShowListPets.Size = new System.Drawing.Size(32, 19);
            // 
            // statusBarStrip
            // 
            this.statusBarStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMouseLocation,
            this.toolStripDistance,
            this.toolStripSpring,
            this.toolStripVersion,
            this.toolStripServerAddress,
            this.toolStripCoPStatus,
            this.toolStripShortName,
            this.toolStripFPS});
            this.statusBarStrip.Location = new System.Drawing.Point(0, 507);
            this.statusBarStrip.Name = "statusBarStrip";
            this.statusBarStrip.Size = new System.Drawing.Size(800, 22);
            this.statusBarStrip.TabIndex = 0;
            this.statusBarStrip.Text = "statusStrip1";
            // 
            // toolStripMouseLocation
            // 
            this.toolStripMouseLocation.AutoSize = false;
            this.toolStripMouseLocation.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripMouseLocation.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripMouseLocation.Name = "toolStripMouseLocation";
            this.toolStripMouseLocation.Size = new System.Drawing.Size(150, 17);
            this.toolStripMouseLocation.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolStripMouseLocation.ToolTipText = "Mouse Location";
            // 
            // toolStripDistance
            // 
            this.toolStripDistance.AutoSize = false;
            this.toolStripDistance.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripDistance.Name = "toolStripDistance";
            this.toolStripDistance.Size = new System.Drawing.Size(100, 17);
            this.toolStripDistance.ToolTipText = "Game Distance from Player to Cursor";
            // 
            // toolStripSpring
            // 
            this.toolStripSpring.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripSpring.Name = "toolStripSpring";
            this.toolStripSpring.Size = new System.Drawing.Size(291, 17);
            this.toolStripSpring.Spring = true;
            // 
            // toolStripVersion
            // 
            this.toolStripVersion.AutoSize = false;
            this.toolStripVersion.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripVersion.Name = "toolStripVersion";
            this.toolStripVersion.Size = new System.Drawing.Size(60, 17);
            // 
            // toolStripServerAddress
            // 
            this.toolStripServerAddress.AutoSize = false;
            this.toolStripServerAddress.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripServerAddress.Name = "toolStripServerAddress";
            this.toolStripServerAddress.Size = new System.Drawing.Size(90, 17);
            // 
            // toolStripCoPStatus
            // 
            this.toolStripCoPStatus.AutoSize = false;
            this.toolStripCoPStatus.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripCoPStatus.Name = "toolStripCoPStatus";
            this.toolStripCoPStatus.Size = new System.Drawing.Size(30, 17);
            // 
            // toolStripShortName
            // 
            this.toolStripShortName.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripShortName.Name = "toolStripShortName";
            this.toolStripShortName.Size = new System.Drawing.Size(4, 17);
            // 
            // toolStripFPS
            // 
            this.toolStripFPS.AutoSize = false;
            this.toolStripFPS.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripFPS.Name = "toolStripFPS";
            this.toolStripFPS.Size = new System.Drawing.Size(60, 17);
            // 
            // toolBarStrip
            // 
            this.toolBarStrip.AutoSize = false;
            this.toolBarStrip.BackColor = System.Drawing.SystemColors.ControlLight;
            this.toolBarStrip.BackgroundImage = global::myseq.Properties.Resources.toolbar;
            this.toolBarStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
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
            this.toolStripEmailAlerts,
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
            this.toolStripResetLookup5,
            this.toolStripOptions});
            this.toolBarStrip.Location = new System.Drawing.Point(0, 24);
            this.toolBarStrip.Name = "toolBarStrip";
            this.toolBarStrip.Size = new System.Drawing.Size(800, 25);
            this.toolBarStrip.TabIndex = 0;
            this.toolBarStrip.Text = "toolBarStrip";
            // 
            // toolStripStartStop
            // 
            this.toolStripStartStop.Image = ((System.Drawing.Image)(resources.GetObject("toolStripStartStop.Image")));
            this.toolStripStartStop.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripStartStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripStartStop.Name = "toolStripStartStop";
            this.toolStripStartStop.Size = new System.Drawing.Size(42, 22);
            this.toolStripStartStop.Text = "Go";
            this.toolStripStartStop.ToolTipText = "Connect to Server";
            this.toolStripStartStop.Click += new System.EventHandler(this.cmdCommand_Click);
            // 
            // toolStripLevel
            // 
            this.toolStripLevel.DropDownHeight = 200;
            this.toolStripLevel.DropDownWidth = 30;
            this.toolStripLevel.IntegralHeight = false;
            this.toolStripLevel.Items.AddRange(new object[] {
            "Auto",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30",
            "31",
            "32",
            "33",
            "34",
            "35",
            "36",
            "37",
            "38",
            "39",
            "40",
            "41",
            "42",
            "43",
            "44",
            "45",
            "46",
            "47",
            "48",
            "49",
            "50",
            "51",
            "52",
            "53",
            "54",
            "55",
            "56",
            "57",
            "58",
            "59",
            "60",
            "61",
            "62",
            "63",
            "64",
            "65",
            "66",
            "67",
            "68",
            "69",
            "70",
            "71",
            "72",
            "73",
            "74",
            "75",
            "76",
            "77",
            "78",
            "79",
            "80",
            "81",
            "82",
            "83",
            "84",
            "85",
            "86",
            "87",
            "88",
            "89",
            "90",
            "91",
            "92",
            "93",
            "94",
            "95",
            "96",
            "97",
            "98",
            "99",
            "100",
            "101",
            "102",
            "103",
            "104",
            "105"});
            this.toolStripLevel.MaxDropDownItems = 80;
            this.toolStripLevel.MaxLength = 4;
            this.toolStripLevel.Name = "toolStripLevel";
            this.toolStripLevel.Size = new System.Drawing.Size(75, 25);
            this.toolStripLevel.Text = "Auto";
            this.toolStripLevel.ToolTipText = "Auto or 1-105 to filter mobcolors accordingly";
            this.toolStripLevel.DropDownClosed += new System.EventHandler(this.toolStripLevel_DropDownClosed);
            this.toolStripLevel.TextUpdate += new System.EventHandler(this.toolStripLevel_TextUpdate);
            this.toolStripLevel.Leave += new System.EventHandler(this.toolStripLevel_Leave);
            this.toolStripLevel.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.toolStripLevel_KeyPress);
            // 
            // toolStripSeparator14
            // 
            this.toolStripSeparator14.Name = "toolStripSeparator14";
            this.toolStripSeparator14.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripZoomIn
            // 
            this.toolStripZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripZoomIn.Image = ((System.Drawing.Image)(resources.GetObject("toolStripZoomIn.Image")));
            this.toolStripZoomIn.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripZoomIn.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.toolStripZoomIn.Name = "toolStripZoomIn";
            this.toolStripZoomIn.Size = new System.Drawing.Size(23, 22);
            this.toolStripZoomIn.Text = "toolStripButton2";
            this.toolStripZoomIn.ToolTipText = "Increase Magnification on Map";
            this.toolStripZoomIn.Click += new System.EventHandler(this.toolStripZoomIn_Click);
            // 
            // toolStripZoomOut
            // 
            this.toolStripZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripZoomOut.Image = ((System.Drawing.Image)(resources.GetObject("toolStripZoomOut.Image")));
            this.toolStripZoomOut.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripZoomOut.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.toolStripZoomOut.Name = "toolStripZoomOut";
            this.toolStripZoomOut.Size = new System.Drawing.Size(23, 22);
            this.toolStripZoomOut.Text = "toolStripButton3";
            this.toolStripZoomOut.ToolTipText = "Decrease Magnification on Map";
            this.toolStripZoomOut.Click += new System.EventHandler(this.toolStripZoomOut_Click);
            // 
            // toolStripScale
            // 
            this.toolStripScale.BackColor = System.Drawing.SystemColors.Window;
            this.toolStripScale.Items.AddRange(new object[] {
            "10.0%",
            "25.0%",
            "50.0%",
            "75.0%",
            "100.0%",
            "125.0%",
            "150.0%",
            "175.0%",
            "200.0%",
            "250.0%",
            "300.0%",
            "400.0%",
            "500.0%",
            "1000.0%",
            "2000.0%"});
            this.toolStripScale.Margin = new System.Windows.Forms.Padding(0);
            this.toolStripScale.Name = "toolStripScale";
            this.toolStripScale.Size = new System.Drawing.Size(75, 25);
            this.toolStripScale.Text = "100.0%";
            this.toolStripScale.ToolTipText = "Select or Enter a value for amount of map zoom.";
            this.toolStripScale.DropDownClosed += new System.EventHandler(this.toolStripScale_DropDownClosed);
            this.toolStripScale.TextUpdate += new System.EventHandler(this.toolStripScale_TextUpdate);
            this.toolStripScale.Leave += new System.EventHandler(this.toolStripScale_Leave);
            this.toolStripScale.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.toolStripScale_KeyPress);
            // 
            // toolStripDepthFilterButton
            // 
            this.toolStripDepthFilterButton.CheckOnClick = true;
            this.toolStripDepthFilterButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDepthFilterButton.Image = global::myseq.Properties.Resources.ShrinkSpaceHS;
            this.toolStripDepthFilterButton.ImageTransparentColor = System.Drawing.Color.White;
            this.toolStripDepthFilterButton.Name = "toolStripDepthFilterButton";
            this.toolStripDepthFilterButton.Size = new System.Drawing.Size(23, 22);
            this.toolStripDepthFilterButton.Text = "Depth Filter";
            this.toolStripDepthFilterButton.ToolTipText = "Toggle Depth Filter On/Off";
            this.toolStripDepthFilterButton.Click += new System.EventHandler(this.mnuDepthFilter_Click);
            // 
            // toolStripZPosLabel
            // 
            this.toolStripZPosLabel.Name = "toolStripZPosLabel";
            this.toolStripZPosLabel.Size = new System.Drawing.Size(38, 22);
            this.toolStripZPosLabel.Text = "Z-Pos";
            this.toolStripZPosLabel.ToolTipText = "The range above the player that is not depth filtered.";
            // 
            // toolStripZPos
            // 
            this.toolStripZPos.Margin = new System.Windows.Forms.Padding(0);
            this.toolStripZPos.Name = "toolStripZPos";
            this.toolStripZPos.Size = new System.Drawing.Size(50, 25);
            this.toolStripZPos.Text = "75.0";
            this.toolStripZPos.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolStripZPos.ToolTipText = "Enter a value for Z-Pos between 0 and 3500.";
            this.toolStripZPos.Leave += new System.EventHandler(this.toolStripZPos_Leave);
            this.toolStripZPos.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.toolStripZPos_KeyPress);
            this.toolStripZPos.TextChanged += new System.EventHandler(this.toolStripZPos_TextChanged);
            // 
            // toolStripZPosDown
            // 
            this.toolStripZPosDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripZPosDown.Image = ((System.Drawing.Image)(resources.GetObject("toolStripZPosDown.Image")));
            this.toolStripZPosDown.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripZPosDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripZPosDown.Name = "toolStripZPosDown";
            this.toolStripZPosDown.Size = new System.Drawing.Size(23, 22);
            this.toolStripZPosDown.Text = "toolStripButton1";
            this.toolStripZPosDown.ToolTipText = "Decrease Z-Pos above player for depth filter.";
            this.toolStripZPosDown.Click += new System.EventHandler(this.toolStripZPosDown_Click);
            // 
            // toolStripZPosUp
            // 
            this.toolStripZPosUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripZPosUp.Image = ((System.Drawing.Image)(resources.GetObject("toolStripZPosUp.Image")));
            this.toolStripZPosUp.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripZPosUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripZPosUp.Name = "toolStripZPosUp";
            this.toolStripZPosUp.Size = new System.Drawing.Size(23, 22);
            this.toolStripZPosUp.Text = "toolStripButton4";
            this.toolStripZPosUp.ToolTipText = "Increase Z-Pos above player for depth filter.";
            this.toolStripZPosUp.Click += new System.EventHandler(this.toolStripZPosUp_Click);
            // 
            // toolStripZOffsetLabel
            // 
            this.toolStripZOffsetLabel.Name = "toolStripZOffsetLabel";
            this.toolStripZOffsetLabel.Size = new System.Drawing.Size(41, 22);
            this.toolStripZOffsetLabel.Text = "Z-Neg";
            this.toolStripZOffsetLabel.ToolTipText = "The range below the player that is not depth filtered.";
            // 
            // toolStripZNeg
            // 
            this.toolStripZNeg.Margin = new System.Windows.Forms.Padding(0);
            this.toolStripZNeg.Name = "toolStripZNeg";
            this.toolStripZNeg.Size = new System.Drawing.Size(50, 25);
            this.toolStripZNeg.Text = "75.0";
            this.toolStripZNeg.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolStripZNeg.ToolTipText = "Enter a value for Z-Neg between 0 and 3500.";
            this.toolStripZNeg.Leave += new System.EventHandler(this.toolStripZNeg_Leave);
            this.toolStripZNeg.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.toolStripZNeg_KeyPress);
            this.toolStripZNeg.TextChanged += new System.EventHandler(this.toolStripZNeg_TextChanged);
            // 
            // toolStripZNegUp
            // 
            this.toolStripZNegUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripZNegUp.Image = ((System.Drawing.Image)(resources.GetObject("toolStripZNegUp.Image")));
            this.toolStripZNegUp.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripZNegUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripZNegUp.Name = "toolStripZNegUp";
            this.toolStripZNegUp.Size = new System.Drawing.Size(23, 22);
            this.toolStripZNegUp.Text = "toolStripButton4";
            this.toolStripZNegUp.ToolTipText = "Increase Z-Neg below player for depth filter.";
            this.toolStripZNegUp.Click += new System.EventHandler(this.toolStripZNegUp_Click);
            // 
            // toolStripZNegDown
            // 
            this.toolStripZNegDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripZNegDown.Image = ((System.Drawing.Image)(resources.GetObject("toolStripZNegDown.Image")));
            this.toolStripZNegDown.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripZNegDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripZNegDown.Name = "toolStripZNegDown";
            this.toolStripZNegDown.Size = new System.Drawing.Size(23, 22);
            this.toolStripZNegDown.Text = "toolStripButton1";
            this.toolStripZNegDown.ToolTipText = "Decrease Z-Neg below player for depth filter.";
            this.toolStripZNegDown.Click += new System.EventHandler(this.toolStripZNegDown_Click);
            // 
            // toolStripResetDepthFilter
            // 
            this.toolStripResetDepthFilter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripResetDepthFilter.Image = ((System.Drawing.Image)(resources.GetObject("toolStripResetDepthFilter.Image")));
            this.toolStripResetDepthFilter.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripResetDepthFilter.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.toolStripResetDepthFilter.Name = "toolStripResetDepthFilter";
            this.toolStripResetDepthFilter.Size = new System.Drawing.Size(23, 22);
            this.toolStripResetDepthFilter.Text = "toolStripResetDepthFilter";
            this.toolStripResetDepthFilter.ToolTipText = "Reset Depth Filter Settings";
            this.toolStripResetDepthFilter.Click += new System.EventHandler(this.toolStripResetDepthFilter_Click);
            // 
            // toolStripEmailAlerts
            // 
            this.toolStripEmailAlerts.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripEmailAlerts.Image = ((System.Drawing.Image)(resources.GetObject("toolStripEmailAlerts.Image")));
            this.toolStripEmailAlerts.ImageTransparentColor = System.Drawing.Color.White;
            this.toolStripEmailAlerts.Name = "toolStripEmailAlerts";
            this.toolStripEmailAlerts.Size = new System.Drawing.Size(23, 22);
            this.toolStripEmailAlerts.Text = "Enable Email Alerts";
            this.toolStripEmailAlerts.Click += new System.EventHandler(this.toolStripEmailAlerts_Click);
            // 
            // toolStripSeparator19
            // 
            this.toolStripSeparator19.Name = "toolStripSeparator19";
            this.toolStripSeparator19.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(58, 22);
            this.toolStripLabel1.Text = "Find Mob";
            this.toolStripLabel1.ToolTipText = "Find and temporarily mark mobs on map.";
            // 
            // toolStripLookupBox
            // 
            this.toolStripLookupBox.ForeColor = System.Drawing.SystemColors.GrayText;
            this.toolStripLookupBox.Name = "toolStripLookupBox";
            this.toolStripLookupBox.Size = new System.Drawing.Size(100, 25);
            this.toolStripLookupBox.Text = "Mob Search";
            this.toolStripLookupBox.ToolTipText = "Type in mob name and press Enter.";
            this.toolStripLookupBox.Leave += new System.EventHandler(this.toolStripLookupBox_Leave);
            this.toolStripLookupBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.toolStripTextBox_KeyPress);
            this.toolStripLookupBox.Click += new System.EventHandler(this.toolStripLookupBox_Click);
            // 
            // toolStripResetLookup
            // 
            this.toolStripResetLookup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripResetLookup.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripResetLookup.Name = "toolStripResetLookup";
            this.toolStripResetLookup.Size = new System.Drawing.Size(39, 19);
            this.toolStripResetLookup.Text = "Reset";
            this.toolStripResetLookup.ToolTipText = "Reset Find Mob Search String";
            this.toolStripResetLookup.Click += new System.EventHandler(this.toolStripResetLookup_Click);
            // 
            // toolStripLookupBox1
            // 
            this.toolStripLookupBox1.ForeColor = System.Drawing.SystemColors.GrayText;
            this.toolStripLookupBox1.Name = "toolStripLookupBox1";
            this.toolStripLookupBox1.Size = new System.Drawing.Size(100, 23);
            this.toolStripLookupBox1.Text = "Mob Search";
            this.toolStripLookupBox1.ToolTipText = "Type in mob name and press Enter.";
            this.toolStripLookupBox1.Leave += new System.EventHandler(this.toolStripLookupBox1_Leave);
            this.toolStripLookupBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.toolStripTextBox1_KeyPress);
            this.toolStripLookupBox1.Click += new System.EventHandler(this.toolStripLookupBox1_Click);
            // 
            // toolStripResetLookup1
            // 
            this.toolStripResetLookup1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripResetLookup1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripResetLookup1.Name = "toolStripResetLookup1";
            this.toolStripResetLookup1.Size = new System.Drawing.Size(39, 19);
            this.toolStripResetLookup1.Text = "Reset";
            this.toolStripResetLookup1.ToolTipText = "Reset Find Mob Search String";
            this.toolStripResetLookup1.Click += new System.EventHandler(this.toolStripResetLookup1_Click);
            // 
            // toolStripLookupBox2
            // 
            this.toolStripLookupBox2.ForeColor = System.Drawing.SystemColors.GrayText;
            this.toolStripLookupBox2.Name = "toolStripLookupBox2";
            this.toolStripLookupBox2.Size = new System.Drawing.Size(132, 23);
            this.toolStripLookupBox2.Text = "Mob Search";
            this.toolStripLookupBox2.ToolTipText = "Type in mob name and press Enter.";
            this.toolStripLookupBox2.Leave += new System.EventHandler(this.toolStripLookupBox2_Leave);
            this.toolStripLookupBox2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.toolStripTextBox2_KeyPress);
            this.toolStripLookupBox2.Click += new System.EventHandler(this.toolStripLookupBox2_Click);
            // 
            // toolStripResetLookup2
            // 
            this.toolStripResetLookup2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripResetLookup2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripResetLookup2.Name = "toolStripResetLookup2";
            this.toolStripResetLookup2.Size = new System.Drawing.Size(39, 19);
            this.toolStripResetLookup2.Text = "Reset";
            this.toolStripResetLookup2.ToolTipText = "Reset Find Mob Search String";
            this.toolStripResetLookup2.Click += new System.EventHandler(this.toolStripResetLookup2_Click);
            // 
            // toolStripLookupBox3
            // 
            this.toolStripLookupBox3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.toolStripLookupBox3.Name = "toolStripLookupBox3";
            this.toolStripLookupBox3.Size = new System.Drawing.Size(132, 23);
            this.toolStripLookupBox3.Text = "Mob Search";
            this.toolStripLookupBox3.ToolTipText = "Type in mob name and press Enter.";
            this.toolStripLookupBox3.Leave += new System.EventHandler(this.toolStripLookupBox3_Leave);
            this.toolStripLookupBox3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.toolStripTextBox3_KeyPress);
            this.toolStripLookupBox3.Click += new System.EventHandler(this.toolStripLookupBox3_Click);
            // 
            // toolStripResetLookup3
            // 
            this.toolStripResetLookup3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripResetLookup3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripResetLookup3.Name = "toolStripResetLookup3";
            this.toolStripResetLookup3.Size = new System.Drawing.Size(39, 19);
            this.toolStripResetLookup3.Text = "Reset";
            this.toolStripResetLookup3.ToolTipText = "Reset Find Mob Search String";
            this.toolStripResetLookup3.Click += new System.EventHandler(this.toolStripResetLookup3_Click);
            // 
            // toolStripLookupBox4
            // 
            this.toolStripLookupBox4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.toolStripLookupBox4.Name = "toolStripLookupBox4";
            this.toolStripLookupBox4.Size = new System.Drawing.Size(132, 23);
            this.toolStripLookupBox4.Text = "Mob Search";
            this.toolStripLookupBox4.ToolTipText = "Type in mob name and press Enter.";
            this.toolStripLookupBox4.Leave += new System.EventHandler(this.toolStripLookupBox2_Leave);
            this.toolStripLookupBox4.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.toolStripTextBox4_KeyPress);
            this.toolStripLookupBox4.Click += new System.EventHandler(this.toolStripLookupBox4_Click);
            // 
            // toolStripResetLookup4
            // 
            this.toolStripResetLookup4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripResetLookup4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripResetLookup4.Name = "toolStripResetLookup4";
            this.toolStripResetLookup4.Size = new System.Drawing.Size(39, 19);
            this.toolStripResetLookup4.Text = "Reset";
            this.toolStripResetLookup4.ToolTipText = "Reset Find Mob Search String";
            this.toolStripResetLookup4.Click += new System.EventHandler(this.toolStripResetLookup4_Click);
            // 
            // toolStripLookupBox5
            // 
            this.toolStripLookupBox5.ForeColor = System.Drawing.SystemColors.GrayText;
            this.toolStripLookupBox5.Name = "toolStripLookupBox5";
            this.toolStripLookupBox5.Size = new System.Drawing.Size(132, 23);
            this.toolStripLookupBox5.Text = "Mob Search";
            this.toolStripLookupBox5.ToolTipText = "Type in mob name and press Enter.";
            this.toolStripLookupBox5.Leave += new System.EventHandler(this.toolStripLookupBox5_Leave);
            this.toolStripLookupBox5.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.toolStripTextBox5_KeyPress);
            this.toolStripLookupBox5.Click += new System.EventHandler(this.toolStripLookupBox5_Click);
            // 
            // toolStripCheckLookup
            // 
            this.toolStripCheckLookup.CheckOnClick = true;
            this.toolStripCheckLookup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText;
            this.toolStripCheckLookup.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripCheckLookup.Name = "toolStripCheckLookup";
            this.toolStripCheckLookup.Size = new System.Drawing.Size(51, 19);
            this.toolStripCheckLookup.Text = "L";
            this.toolStripCheckLookup.ToolTipText = "Lookup or Filter";
            this.toolStripCheckLookup.CheckedChanged += new System.EventHandler(this.toolStripCheckLookup_CheckChanged);
            this.toolStripCheckLookup.Checked = true;
            this.toolStripCheckLookup.BackColor = Color.Gray;
            // 
            // toolStripCheckLookup1
            // 
            this.toolStripCheckLookup1.CheckOnClick = true;
            this.toolStripCheckLookup1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText;
            this.toolStripCheckLookup1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripCheckLookup1.Name = "toolStripCheckLookup1";
            this.toolStripCheckLookup1.Size = new System.Drawing.Size(51, 19);
            this.toolStripCheckLookup1.Text = "L";
            this.toolStripCheckLookup1.ToolTipText = "Lookup or Filter";
            this.toolStripCheckLookup1.CheckedChanged += new System.EventHandler(this.toolStripCheckLookup1_CheckChanged);
            this.toolStripCheckLookup1.Checked = true;
            this.toolStripCheckLookup1.BackColor = Color.Gray;
            // 
            // toolStripCheckLookup2
            // 
            this.toolStripCheckLookup2.CheckOnClick = true;
            this.toolStripCheckLookup2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText;
            this.toolStripCheckLookup2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripCheckLookup2.Name = "toolStripCheckLookup2";
            this.toolStripCheckLookup2.Size = new System.Drawing.Size(51, 19);
            this.toolStripCheckLookup2.Text = "L";
            this.toolStripCheckLookup2.ToolTipText = "Lookup or Filter";
            this.toolStripCheckLookup2.CheckedChanged += new System.EventHandler(this.toolStripCheckLookup2_CheckChanged);
            this.toolStripCheckLookup2.Checked = true;
            this.toolStripCheckLookup2.BackColor = Color.Gray;
            // 
            // toolStripCheckLookup3
            // 
            this.toolStripCheckLookup3.CheckOnClick = true;
            this.toolStripCheckLookup3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText;
            this.toolStripCheckLookup3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripCheckLookup3.Name = "toolStripCheckLookup3";
            this.toolStripCheckLookup3.Size = new System.Drawing.Size(51, 19);
            this.toolStripCheckLookup3.Text = "L";
            this.toolStripCheckLookup3.ToolTipText = "Lookup or Filter";
            this.toolStripCheckLookup3.CheckedChanged += new System.EventHandler(this.toolStripCheckLookup3_CheckChanged);
            this.toolStripCheckLookup3.Checked = true;
            this.toolStripCheckLookup3.BackColor = Color.Gray;
            // 
            // toolStripCheckLookup4
            // 
            this.toolStripCheckLookup4.CheckOnClick = true;
            this.toolStripCheckLookup4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText;
            this.toolStripCheckLookup4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripCheckLookup4.Name = "toolStripCheckLookup4";
            this.toolStripCheckLookup4.Size = new System.Drawing.Size(51, 19);
            this.toolStripCheckLookup4.Text = "L";
            this.toolStripCheckLookup4.ToolTipText = "Lookup or Filter";
            this.toolStripCheckLookup4.CheckedChanged += new System.EventHandler(this.toolStripCheckLookup4_CheckChanged);
            this.toolStripCheckLookup4.Checked = true;
            this.toolStripCheckLookup4.BackColor = Color.Gray;
            // 
            // toolStripCheckLookup5
            // 
            this.toolStripCheckLookup5.CheckOnClick = true;
            this.toolStripCheckLookup5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText;
            this.toolStripCheckLookup5.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripCheckLookup5.Name = "toolStripCheckLookup5";
            this.toolStripCheckLookup5.Size = new System.Drawing.Size(51, 19);
            this.toolStripCheckLookup5.Text = "L";
            this.toolStripCheckLookup5.ToolTipText = "Lookup or Filter";
            this.toolStripCheckLookup5.CheckedChanged += new System.EventHandler(this.toolStripCheckLookup5_CheckChanged);
            this.toolStripCheckLookup5.Checked = true;
            this.toolStripCheckLookup5.BackColor = Color.Gray;
            // 
            // toolStripResetLookup5
            // 
            this.toolStripResetLookup5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripResetLookup5.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripResetLookup5.Name = "toolStripResetLookup5";
            this.toolStripResetLookup5.Size = new System.Drawing.Size(39, 19);
            this.toolStripResetLookup5.Text = "Reset";
            this.toolStripResetLookup5.ToolTipText = "Reset Find Mob Search String";
            this.toolStripResetLookup5.Click += new System.EventHandler(this.toolStripResetLookup5_Click);
            // 
            // toolStripOptions
            // 
            this.toolStripOptions.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripOptions.Image = ((System.Drawing.Image)(resources.GetObject("toolStripOptions.Image")));
            this.toolStripOptions.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripOptions.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripOptions.Name = "toolStripOptions";
            this.toolStripOptions.Size = new System.Drawing.Size(23, 20);
            this.toolStripOptions.Text = "Options";
            this.toolStripOptions.ToolTipText = "Open Options Dialog";
            this.toolStripOptions.Click += new System.EventHandler(this.mnuOptions_Click);
            // 
            // dockPanel
            // 
            this.dockPanel.ActiveAutoHideContent = null;
            this.dockPanel.BackColor = System.Drawing.SystemColors.ControlLight;
            this.dockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dockPanel.DockBackColor = System.Drawing.SystemColors.ControlLight;
            this.dockPanel.Location = new System.Drawing.Point(0, 49);
            this.dockPanel.Name = "dockPanel";
            this.dockPanel.Size = new System.Drawing.Size(800, 458);
            dockPanelGradient1.EndColor = System.Drawing.SystemColors.ControlLight;
            dockPanelGradient1.StartColor = System.Drawing.SystemColors.ControlLight;
            autoHideStripSkin1.DockStripGradient = dockPanelGradient1;
            tabGradient1.EndColor = System.Drawing.SystemColors.Control;
            tabGradient1.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            tabGradient1.StartColor = System.Drawing.SystemColors.ControlLightLight;
            tabGradient1.TextColor = System.Drawing.SystemColors.ControlDarkDark;
            autoHideStripSkin1.TabGradient = tabGradient1;
            autoHideStripSkin1.TextFont = new System.Drawing.Font("Tahoma", 8.25F);
            dockPanelSkin1.AutoHideStripSkin = autoHideStripSkin1;
            tabGradient2.EndColor = System.Drawing.SystemColors.GradientInactiveCaption;
            tabGradient2.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            tabGradient2.StartColor = System.Drawing.SystemColors.ControlLight;
            tabGradient2.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripGradient1.ActiveTabGradient = tabGradient2;
            dockPanelGradient2.EndColor = System.Drawing.SystemColors.Control;
            dockPanelGradient2.StartColor = System.Drawing.SystemColors.Control;
            dockPaneStripGradient1.DockStripGradient = dockPanelGradient2;
            tabGradient3.EndColor = System.Drawing.SystemColors.ControlLight;
            tabGradient3.StartColor = System.Drawing.SystemColors.ControlLight;
            tabGradient3.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripGradient1.InactiveTabGradient = tabGradient3;
            dockPaneStripSkin1.DocumentGradient = dockPaneStripGradient1;
            dockPaneStripSkin1.TextFont = new System.Drawing.Font("Tahoma", 8.25F);
            tabGradient4.EndColor = System.Drawing.SystemColors.ActiveCaption;
            tabGradient4.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            tabGradient4.StartColor = System.Drawing.SystemColors.GradientActiveCaption;
            tabGradient4.TextColor = System.Drawing.SystemColors.ActiveCaptionText;
            dockPaneStripToolWindowGradient1.ActiveCaptionGradient = tabGradient4;
            tabGradient5.EndColor = System.Drawing.SystemColors.Control;
            tabGradient5.StartColor = System.Drawing.SystemColors.Control;
            tabGradient5.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripToolWindowGradient1.ActiveTabGradient = tabGradient5;
            dockPanelGradient3.EndColor = System.Drawing.SystemColors.ControlLight;
            dockPanelGradient3.StartColor = System.Drawing.SystemColors.ControlLight;
            dockPaneStripToolWindowGradient1.DockStripGradient = dockPanelGradient3;
            tabGradient6.EndColor = System.Drawing.SystemColors.ActiveBorder;
            tabGradient6.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            tabGradient6.StartColor = System.Drawing.SystemColors.ActiveBorder;
            tabGradient6.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripToolWindowGradient1.InactiveCaptionGradient = tabGradient6;
            tabGradient7.EndColor = System.Drawing.Color.Transparent;
            tabGradient7.StartColor = System.Drawing.Color.Transparent;
            tabGradient7.TextColor = System.Drawing.SystemColors.ControlDarkDark;
            dockPaneStripToolWindowGradient1.InactiveTabGradient = tabGradient7;
            dockPaneStripSkin1.ToolWindowGradient = dockPaneStripToolWindowGradient1;
            dockPanelSkin1.DockPaneStripSkin = dockPaneStripSkin1;
            this.dockPanel.Skin = dockPanelSkin1;
            this.dockPanel.TabIndex = 2;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(800, 529);
            this.ContextMenuStrip = this.mnuContext;
            this.Controls.Add(this.dockPanel);
            this.Controls.Add(this.toolBarStrip);
            this.Controls.Add(this.statusBarStrip);
            this.Controls.Add(this.mnuMainMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.mnuMainMenu;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "frmMain";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.frmMain_Closing);
            this.Move += new System.EventHandler(this.frmMain_Move);
            this.Resize += new System.EventHandler(this.frmMain_Resize);
            this.mnuMainMenu.ResumeLayout(false);
            this.mnuMainMenu.PerformLayout();
            this.mnuContext.ResumeLayout(false);
            this.mnuContextAddFilter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.timDelayAlerts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timProcessTimers)).EndInit();
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
            try {Application.Run(new frmMain());}

            catch (Exception e) 

            {

                string s = "Uncaught exception in Main(): " + e.Message;

                LogLib.WriteLine(s);

                MessageBox.Show(s);

                Application.Exit();

            }

        }

        

        private void frmMain_Closing(object sender, CancelEventArgs e) 

        {

            if (Settings.Instance.SaveOnExit)
            {
                String mypath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MySEQ");
                String preFile = Path.Combine(mypath, "prefs.xml");
                String myseqFile = Path.Combine(mypath, "myseq.xml");

                savePrefs(preFile);
                SMTPSettings.Instance.Save(myseqFile);
            }

        }



        public void cmdCommand_Click(object sender, System.EventArgs e) 

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

        public void NewMap()
        {
            if (map != null)
                map.NewMap();
        }

        public void AutoConnect()
        {
            if (colProcesses != null)
                colProcesses.Clear();

            if (eq.playerinfo != null)
                eq.playerinfo.Name = "";

            if (mnuIPAddress1.Checked == true)

                currentIPAddress = Settings.Instance.IPAddress1;

            else if (mnuIPAddress2.Checked == true)

                currentIPAddress = Settings.Instance.IPAddress2;

            else if (mnuIPAddress3.Checked == true)

                currentIPAddress = Settings.Instance.IPAddress3;

            else if (mnuIPAddress4.Checked == true)

                currentIPAddress = Settings.Instance.IPAddress4;

            else if (mnuIPAddress5.Checked == true)

                currentIPAddress = Settings.Instance.IPAddress5;



            if (currentIPAddress.Length == 0)

                return;



            // Try to connect to the server         

            if (!comm.ConnectToServer(currentIPAddress, Settings.Instance.Port, false))
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

            this.mnuConnect.Text = "&Disconnect";

            this.mnuConnect.Image = global::myseq.Properties.Resources.RedDelete;

            this.toolStripStartStop.Text = "Stop";
            this.toolStripStartStop.ToolTipText = "Disconnect from Server";
            this.toolStripStartStop.Image = global::myseq.Properties.Resources.RedDelete;

            bIsRunning = true;

        }

        public void StartListening() 

        {
            if (colProcesses != null)
                colProcesses.Clear();

            if (eq.playerinfo != null)
                eq.playerinfo.Name = "";

            if (mnuIPAddress1.Checked == true)

                currentIPAddress = Settings.Instance.IPAddress1;

            else if (mnuIPAddress2.Checked == true)

                currentIPAddress = Settings.Instance.IPAddress2;

            else if (mnuIPAddress3.Checked == true)

                currentIPAddress = Settings.Instance.IPAddress3;

            else if (mnuIPAddress4.Checked == true)

                currentIPAddress = Settings.Instance.IPAddress4;

            else if (mnuIPAddress5.Checked == true)

                currentIPAddress = Settings.Instance.IPAddress5;



            if (currentIPAddress.Length == 0)

                return;



            // Try to connect to the server         

            if (!comm.ConnectToServer(currentIPAddress, Settings.Instance.Port)) 

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

            this.mnuConnect.Text = "&Disconnect";

            this.mnuConnect.Image = global::myseq.Properties.Resources.RedDelete;

            this.toolStripStartStop.Text = "Stop";
            this.toolStripStartStop.ToolTipText = "Disconnect from Server";
            this.toolStripStartStop.Image = global::myseq.Properties.Resources.RedDelete;

            bIsRunning = true;

        }

        public void SetCharSelection(string player_name)
        {
            if (player_name == mnuChar1.Text.ToString())
                mnuChar1.Checked = true;
            else
                mnuChar1.Checked = false;

            if (player_name == mnuChar2.Text.ToString())
                mnuChar2.Checked = true;
            else
                mnuChar2.Checked = false;

            if (player_name == mnuChar3.Text.ToString())
                mnuChar3.Checked = true;
            else
                mnuChar3.Checked = false;

            if (player_name == mnuChar4.Text.ToString())
                mnuChar4.Checked = true;
            else
                mnuChar4.Checked = false;

            if (player_name == mnuChar5.Text.ToString())
                mnuChar5.Checked = true;
            else
                mnuChar5.Checked = false;

            if (player_name == mnuChar6.Text.ToString())
                mnuChar6.Checked = true;
            else
                mnuChar6.Checked = false;
 
            if (player_name == mnuChar7.Text.ToString())
                mnuChar7.Checked = true;
            else
                mnuChar7.Checked = false;

            if (player_name == mnuChar8.Text.ToString())
                mnuChar8.Checked = true;
            else
                mnuChar8.Checked = false;

            if (player_name == mnuChar9.Text.ToString())
                mnuChar9.Checked = true;
            else
                mnuChar9.Checked = false;

            if (player_name == mnuChar10.Text.ToString())
                mnuChar10.Checked = true;
            else
                mnuChar10.Checked = false;

            if (player_name == mnuChar10.Text.ToString())
                mnuChar10.Checked = true;
            else
                mnuChar10.Checked = false;

            if (player_name == mnuChar11.Text.ToString())
                mnuChar11.Checked = true;
            else
                mnuChar11.Checked = false;
 
            if (player_name == mnuChar12.Text.ToString())
                mnuChar12.Checked = true;
            else
                mnuChar12.Checked = false;

        }

        public void SetTitle()
        {
            this.Text = BaseTitle;

            if (Settings.Instance.ShowZoneName && eq.longname.Length > 0 && eq.longname != "map_pane")

                this.Text += " - " + eq.longname; 

            if (Settings.Instance.ShowCharName && eq.playerinfo != null && eq.playerinfo.Name.Length > 1)

                this.Text += " - " + eq.playerinfo.Name;

        }

        public void LoadPrefs(string filename) 

        {

            //SetGridInterval();

            Settings.Instance.Load(filename);

            // Always want these off on starting up.

            Settings.Instance.CollectMobTrails = false;

            Settings.Instance.EmailAlerts = false;

            Settings.Instance.DepthFilter = false;

            SetGridInterval();

            // restore the normal windows state, if we were closed maximized
            if (Settings.Instance.WindowState == FormWindowState.Maximized)
            {
                this.Location = Settings.Instance.WindowsLocation;
                //this.Size = Settings.Instance.WindowsSize;
                this.StartPosition = FormStartPosition.Manual;
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.WindowState = Settings.Instance.WindowState;
                this.Size = Settings.Instance.WindowsSize;
                this.Location = Settings.Instance.WindowsLocation;
                this.StartPosition = Settings.Instance.WindowsPosition;
            }
            

            if (Settings.Instance.TitleBar.ToString().Length > 0)

                BaseTitle = Settings.Instance.TitleBar;

            this.mnuShowSpawnList.Checked = Settings.Instance.ShowMobList;

            this.mnuShowSpawnListTimer.Checked = Settings.Instance.ShowMobListTimer;

            this.mnuShowGroundItemList.Checked = Settings.Instance.ShowGroundItemList;

            this.mnuShowGridLines.Checked = (Settings.Instance.DrawOptions & DrawOptions.GridLines) != DrawOptions.DrawNone;

            this.mnuShowZoneText.Checked = (Settings.Instance.DrawOptions & DrawOptions.ZoneText) != DrawOptions.DrawNone;
            this.mnuShowZoneText2.Checked = (Settings.Instance.DrawOptions & DrawOptions.ZoneText) != DrawOptions.DrawNone;
            this.mnuShowLayer1.Checked = Settings.Instance.ShowLayer1;
            this.mnuShowLayer21.Checked = Settings.Instance.ShowLayer1;
            this.mnuShowLayer2.Checked = Settings.Instance.ShowLayer2;
            this.mnuShowLayer22.Checked = Settings.Instance.ShowLayer2;
            this.mnuShowLayer3.Checked = Settings.Instance.ShowLayer3;
            this.mnuShowLayer23.Checked = Settings.Instance.ShowLayer3;
            this.mnuShowListGridLines.Checked = Settings.Instance.ShowListGridLines;
            this.SpawnList.listView.GridLines = Settings.Instance.ShowListGridLines;
            this.SpawnTimerList.listView.GridLines = Settings.Instance.ShowListGridLines;
            this.GroundItemList.listView.GridLines = Settings.Instance.ShowListGridLines;

            this.mnuShowListSearchBox.Checked = Settings.Instance.ShowListSearchBox;
            if (!Settings.Instance.ShowListSearchBox)
            {
                this.SpawnList.HideSearchBox();
                this.SpawnTimerList.HideSearchBox();
                this.GroundItemList.HideSearchBox();
            }

            SetFollowOption(Settings.Instance.FollowOption);

            this.mnuKeepCentered.Checked = Settings.Instance.KeepCentered;
            this.mnuKeepCentered2.Checked = Settings.Instance.KeepCentered;

            this.mnuShowTargetInfo.Checked = Settings.Instance.ShowTargetInfo;
            this.mnuShowTargetInfo2.Checked = Settings.Instance.ShowTargetInfo;

            this.mnuSmallTargetInfo.Checked = Settings.Instance.SmallTargetInfo;
            this.mnuSmallTargetInfo2.Checked = Settings.Instance.SmallTargetInfo;

            this.mnuAutoSelectEQTarget.Checked = Settings.Instance.AutoSelectEQTarget;
            this.mnuAutoSelectEQTarget2.Checked = Settings.Instance.AutoSelectEQTarget;

            this.mnuAutoExpand.Checked = Settings.Instance.AutoExpand;
            this.mnuAutoExpand2.Checked = Settings.Instance.AutoExpand;

            // new filter stuff

            this.mnuShowNPCs.Checked = Settings.Instance.ShowNPCs;

            this.mnuShowCorpses.Checked = Settings.Instance.ShowCorpses;

            this.mnuShowPCCorpses.Checked = Settings.Instance.ShowPCCorpses;

            this.mnuShowMyCorpse.Checked = Settings.Instance.ShowMyCorpse;

            this.mnuShowPlayers.Checked = Settings.Instance.ShowPlayers;

            this.mnuShowInvis.Checked = Settings.Instance.ShowInvis;

            this.mnuShowMounts.Checked = Settings.Instance.ShowMounts;

            this.mnuShowFamiliars.Checked = Settings.Instance.ShowFamiliars;

            this.mnuShowPets.Checked = Settings.Instance.ShowPets;

            this.mnuShowLookupText.Checked = Settings.Instance.ShowLookupText;

            this.mnuAlwaysOnTop.Checked = Settings.Instance.AlwaysOnTop;

            this.mnuShowLookupNumber.Checked = Settings.Instance.ShowLookupNumber;

            this.mnuShowSpawnPoints.Checked = (Settings.Instance.DrawOptions & DrawOptions.SpawnTimers) != DrawOptions.DrawNone;
            this.mnuShowSpawnPoints2.Checked = (Settings.Instance.DrawOptions & DrawOptions.SpawnTimers) != DrawOptions.DrawNone;

            this.mnuDepthFilter.Checked = Settings.Instance.DepthFilter;
            this.mnuDepthFilter2.Checked = Settings.Instance.DepthFilter;

            // update the toolbar settings
            this.toolStripDepthFilterButton.Checked = Settings.Instance.DepthFilter;
            this.toolStripZNegUp.Enabled = Settings.Instance.DepthFilter;
            this.toolStripZNeg.Enabled = Settings.Instance.DepthFilter;
            this.toolStripZNegDown.Enabled = Settings.Instance.DepthFilter;
            this.toolStripZPosDown.Enabled = Settings.Instance.DepthFilter;
            this.toolStripZOffsetLabel.Enabled = Settings.Instance.DepthFilter;
            this.toolStripZPosUp.Enabled = Settings.Instance.DepthFilter;
            this.toolStripZPos.Enabled = Settings.Instance.DepthFilter;
            this.toolStripZPosLabel.Enabled = Settings.Instance.DepthFilter;
            this.toolStripResetDepthFilter.Enabled = Settings.Instance.DepthFilter;

            if (Settings.Instance.DepthFilter)
                this.toolStripDepthFilterButton.Image = global::myseq.Properties.Resources.ExpandSpaceHS;
            

            this.mnuDynamicAlpha.Checked = Settings.Instance.UseDynamicAlpha;
            this.mnuDynamicAlpha2.Checked = Settings.Instance.UseDynamicAlpha;

            this.mnuForceDistinct.Checked = Settings.Instance.ForceDistinct;
            this.mnuForceDistinct2.Checked = Settings.Instance.ForceDistinct;

            this.mnuForceDistinctText.Checked = Settings.Instance.ForceDistinctText;
            this.mnuForceDistinctText2.Checked = Settings.Instance.ForceDistinctText;

            this.mnuCollectMobTrails.Checked = Settings.Instance.CollectMobTrails;

            this.mnuShowMobTrails.Checked = (Settings.Instance.DrawOptions & DrawOptions.SpawnTrails) != DrawOptions.DrawNone;

            this.mnuConSoD.Checked = Settings.Instance.SoDCon;

            this.mnuConDefault.Checked = Settings.Instance.DefaultCon;

            this.mnuConSoF.Checked = Settings.Instance.SoFCon;

            this.mnuShowPCNames.Checked = Settings.Instance.ShowPCNames;
            this.mnuShowPCNames2.Checked = Settings.Instance.ShowPCNames;

            this.mnuShowNPCNames.Checked = Settings.Instance.ShowNPCNames;
            this.mnuShowNPCNames2.Checked = Settings.Instance.ShowNPCNames;

            this.mnuSaveSpawnLog.Checked = Settings.Instance.SaveSpawnLogs;

            this.mnuShowNPCCorpseNames.Checked = Settings.Instance.ShowNPCCorpseNames;
            this.mnuShowNPCCorpseNames2.Checked = Settings.Instance.ShowNPCCorpseNames;

            this.mnuShowPlayerCorpseNames.Checked = Settings.Instance.ShowPlayerCorpseNames;
            this.mnuShowPlayerCorpseNames2.Checked = Settings.Instance.ShowPlayerCorpseNames;

            this.mnuShowPVP.Checked = Settings.Instance.ShowPVP;
            this.mnuShowPVP2.Checked = Settings.Instance.ShowPVP;

            this.mnuShowPVPLevel.Checked = Settings.Instance.ShowPVPLevel;
            this.mnuShowPVPLevel2.Checked = Settings.Instance.ShowPVPLevel;

            this.mnuShowNPCLevels.Checked = Settings.Instance.ShowNPCLevels;
            this.mnuShowNPCLevels2.Checked = Settings.Instance.ShowNPCLevels;

            this.mnuShowLookupText.Checked = Settings.Instance.ShowLookupText;

            this.mnuAlwaysOnTop.Checked = Settings.Instance.AlwaysOnTop;
            
            this.mnuShowLookupNumber.Checked = Settings.Instance.ShowLookupNumber;

            this.mnuFilterMapLines.Checked = Settings.Instance.FilterMapLines;
            this.mnuFilterMapLines2.Checked = Settings.Instance.FilterMapLines;

            this.mnuFilterMapText.Checked = Settings.Instance.FilterMapText;
            this.mnuFilterMapText2.Checked = Settings.Instance.FilterMapText;

            this.mnuFilterNPCs.Checked = Settings.Instance.FilterNPCs;
            this.mnuFilterNPCs2.Checked = Settings.Instance.FilterNPCs;

            this.mnuFilterPlayers.Checked = Settings.Instance.FilterPlayers;
            this.mnuFilterPlayers2.Checked = Settings.Instance.FilterPlayers;

            this.mnuFilterSpawnPoints.Checked = Settings.Instance.FilterSpawnPoints;
            this.mnuFilterSpawnPoints2.Checked = Settings.Instance.FilterSpawnPoints;

            this.mnuFilterPlayerCorpses.Checked = Settings.Instance.FilterPlayerCorpses;
            this.mnuFilterPlayerCorpses2.Checked = Settings.Instance.FilterPlayerCorpses;

            this.mnuFilterGroundItems.Checked = Settings.Instance.FilterGroundItems;
            this.mnuFilterGroundItems2.Checked = Settings.Instance.FilterGroundItems;

            this.mnuFilterNPCCorpses.Checked = Settings.Instance.FilterNPCCorpses;
            this.mnuFilterNPCCorpses2.Checked = Settings.Instance.FilterNPCCorpses;

            this.mnuSpawnCountdown.Checked = Settings.Instance.SpawnCountdown;
            this.mnuSpawnCountdown2.Checked = Settings.Instance.SpawnCountdown;

            this.mnuShowMenuBar.Checked = Settings.Instance.ShowMenuBar;

            this.mnuViewStatusBar.Checked = Settings.Instance.ShowStatusBar;

            this.mnuViewDepthFilterBar.Checked = Settings.Instance.ShowToolBar;

            if (!Settings.Instance.ShowStatusBar)
                this.statusBarStrip.Hide();

            if (!Settings.Instance.ShowToolBar)
                this.toolBarStrip.Hide();
            
            this.mnuViewMenuBar.Checked = Settings.Instance.ShowMenuBar;
            
            if (Settings.Instance.ShowMenuBar)
            {
                this.mnuMainMenu.Show();
            }
            else
            {
                this.mnuMainMenu.Hide();
            }

            if (Settings.Instance.CurrentIPAddress == 0 && Settings.Instance.IPAddress1.Length > 0)

                Settings.Instance.CurrentIPAddress = 1;

            ResetMenu(Settings.Instance.CurrentIPAddress);

            ServerSelection();

            this.mnuAutoConnect.Checked = Settings.Instance.AutoConnect;
            
            eq.LoadSpawnInfo();

            // SpawnList

            SpawnList.listView.BackColor = Settings.Instance.ListBackColor;

            SpawnTimerList.listView.BackColor = Settings.Instance.ListBackColor;

            GroundItemList.listView.BackColor = Settings.Instance.ListBackColor;

            SpawnList.listView.GridLines = Settings.Instance.ShowListGridLines;

            SpawnTimerList.listView.GridLines = Settings.Instance.ShowListGridLines;

            GroundItemList.listView.GridLines = Settings.Instance.ShowListGridLines;

            LogLib.maxLogLevel = Settings.Instance.MaxLogLevel;



            if (Settings.Instance.MapDir == string.Empty || !Directory.Exists(Settings.Instance.MapDir))

            {

                //MessageBox.Show(String.Format("Map folder not found: {0} Setting to default.", Settings.Instance.MapDir), "Map Folder Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                // Slartibartfast - using .\ fails when the working directory is changed by eg loading a map manually

                //Settings.Instance.MapDir = @".\maps";

                Settings.Instance.MapDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "maps");

                if (!Directory.Exists(Settings.Instance.MapDir)) Directory.CreateDirectory(Settings.Instance.MapDir);

            }

            if (Settings.Instance.FilterDir == string.Empty || !Directory.Exists(Settings.Instance.FilterDir))

            {

                //MessageBox.Show(String.Format("Filter folder not found: {0} Setting to default.", Settings.Instance.FilterDir), "Filter Folder Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                // Slartibartfast - using .\ fails when the working directory is changed by eg loading a map manually

                //Settings.Instance.FilterDir = @".\filters";

                Settings.Instance.FilterDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "filters");

                if (!Directory.Exists(Settings.Instance.FilterDir)) Directory.CreateDirectory(Settings.Instance.FilterDir);

            }

            if (Settings.Instance.CfgDir == string.Empty || !Directory.Exists(Settings.Instance.CfgDir))

            {

                //MessageBox.Show(String.Format("Config folder not found: {0} Setting to default.", Settings.Instance.CfgDir), "Config Folder Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                // Slartibartfast - using .\ fails when the working directory is changed by eg loading a map manually

                //Settings.Instance.CfgDir = @".\cfg";

                Settings.Instance.CfgDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cfg");

                if (!Directory.Exists(Settings.Instance.CfgDir)) Directory.CreateDirectory(Settings.Instance.CfgDir);

            }

            if (Settings.Instance.LogDir == string.Empty || !Directory.Exists(Settings.Instance.LogDir))

            {

                //MessageBox.Show(String.Format("Log folder not found: {0} Setting to default.", Settings.Instance.LogDir), "Log Folder Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                // Slartibartfast - using .\ fails when the working directory is changed by eg loading a map manually

                //Settings.Instance.LogDir = @".\logs";

                Settings.Instance.LogDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");

                if (!Directory.Exists(Settings.Instance.LogDir)) Directory.CreateDirectory(Settings.Instance.LogDir);

            }

            if (Settings.Instance.TimerDir == string.Empty || !Directory.Exists(Settings.Instance.TimerDir))

            {

                //MessageBox.Show(String.Format("Spawn Timer folder not found: {0} Setting to default.", Settings.Instance.TimerDir), "Timers Folder Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                // Slartibartfast - using .\ fails when the working directory is changed by eg loading a map manually

                //Settings.Instance.TimerDir = @".\timers";

                Settings.Instance.TimerDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "timers");

                if (!Directory.Exists(Settings.Instance.TimerDir)) Directory.CreateDirectory(Settings.Instance.TimerDir);

            }

            DrawOpts = Settings.Instance.DrawOptions;

            timProcessTimers.Start();

        }

        public void savePrefs(string filename) 

        {
            String myPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MySEQ");
            
            if (!Directory.Exists(myPath))
                Directory.CreateDirectory(myPath);
           
            String strAppDir = AppDomain.CurrentDomain.BaseDirectory;

            String oldposfile = Path.Combine(strAppDir, "positions.xml");

            String strPosFile = Path.Combine(myPath, "positions.xml");

            Settings.Instance.c1w = SpawnList.listView.Columns[0].Width;

            Settings.Instance.c2w = SpawnList.listView.Columns[1].Width;

            Settings.Instance.c3w = SpawnList.listView.Columns[2].Width;

            Settings.Instance.c4w = SpawnList.listView.Columns[3].Width;

            Settings.Instance.c5w = SpawnList.listView.Columns[4].Width;

            Settings.Instance.c6w = SpawnList.listView.Columns[5].Width;

            Settings.Instance.c7w = SpawnList.listView.Columns[6].Width;

            Settings.Instance.c8w = SpawnList.listView.Columns[7].Width;

            Settings.Instance.c9w = SpawnList.listView.Columns[8].Width;

            Settings.Instance.c10w = SpawnList.listView.Columns[9].Width;

            Settings.Instance.c11w = SpawnList.listView.Columns[10].Width;

            Settings.Instance.c12w = SpawnList.listView.Columns[11].Width;

            Settings.Instance.c13w = SpawnList.listView.Columns[12].Width;

            Settings.Instance.c14w = SpawnList.listView.Columns[13].Width;

            if (this.WindowState == FormWindowState.Minimized)
                this.WindowState = FormWindowState.Normal;

            Settings.Instance.WindowState = this.WindowState;
            Settings.Instance.WindowsPosition = FormStartPosition.Manual;

            if (this.WindowState == FormWindowState.Maximized)
            {
                Settings.Instance.WindowsSize = this.RestoreBounds.Size;
                Settings.Instance.WindowsLocation = this.RestoreBounds.Location;
                Settings.Instance.WindowsPosition = FormStartPosition.Manual;
            }
            else
            {
                Settings.Instance.WindowsSize = this.Size;
                Settings.Instance.WindowsLocation = this.Location;
                Rectangle window_bounds = new Rectangle(this.Location, this.Size);
                Rectangle option_bounds = new Rectangle(Settings.Instance.OptionsWindowsLocation, Settings.Instance.OptionsWindowsSize);
                bool found_onscreen = false;
                foreach (Screen screen in Screen.AllScreens)
                {
                    if (screen.WorkingArea.IntersectsWith(window_bounds))
                    {
                        found_onscreen = true;
                        break;
                    }
                }
                if (!found_onscreen)
                {
                    Settings.Instance.WindowsPosition = FormStartPosition.Manual;
                    Settings.Instance.WindowsSize = new Size(800, 600);
                    Settings.Instance.WindowsLocation = new Point(20, 20);
                }
                found_onscreen = false;
                foreach (Screen screen in Screen.AllScreens)
                {
                    if (screen.WorkingArea.IntersectsWith(option_bounds))
                    {
                        found_onscreen = true;
                        break;
                    }
                }
                if (!found_onscreen)
                {
                    Settings.Instance.OptionsWindowsLocation = new Point(20, 20);
                    Settings.Instance.OptionsWindowsSize = new Size(296, 480);
                }
            }

            Settings.Instance.Save(filename);
            dockPanel.SaveAsXml(strPosFile); // positions file

            // Old positions file - get rid of it, since we saved new one to application data folder
            if (File.Exists(oldposfile))
                File.Delete(oldposfile);

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

                        if (!map.loadLoYMap(filename, false))

                        {

                            return false;

                        }

                    }

                    else

                    {

                        if (!map.loadLoYMap(filename, true))

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

                string msg = String.Format("Failed to load map {0}: {1}", filename, ex.Message);

                LogLib.WriteLine(msg);

                MessageBox.Show(msg);

                return false;

            }

        }

        private void frmMain_Move(object sender, EventArgs e) 

        {

            if (WindowState == FormWindowState.Normal)

            {

                Settings.Instance.WindowsLocation = this.Location;

            }

        }

        private void frmMain_Resize(object sender, EventArgs e) 

        {

            if (WindowState == FormWindowState.Normal)

            {

                Settings.Instance.WindowsSize = this.Size;

            }

            Settings.Instance.WindowState = this.WindowState;

            if (mapCon != null)
                mapCon.ReAdjust();

        }

        public void checkMobs() 

        {
            
            eq.checkMobs(SpawnList, GroundItemList);

        }

        private void timPackets_Tick(object sender, System.EventArgs e) 

        {

            DrawOpts = Settings.Instance.DrawOptions;

            comm.Tick();
            mapCon.Tick();

        }

        private void timDelayPlay_Tick(object sender, System.EventArgs e)
        {
            // our alert sound/email delay interval has passed.
            eq.EnablePlayAlerts();
            timDelayAlerts.Stop();
        }

        private void timProcessTimers_Tick(object sender, System.EventArgs e)
        {
            // allow processing timers.
            ProcessSpawnTimer();

            if (!bIsRunning && mapCon != null)
                mapCon.Invalidate();
        }

        private void SpawnList_VisibleChanged(object sender, System.EventArgs e)

        {

            Settings.Instance.ShowMobList = SpawnList.Visible;

            this.mnuShowSpawnList.Checked = SpawnList.Visible;

        }

        private void SpawnTimerList_VisibleChanged(object sender, System.EventArgs e)

        {

            Settings.Instance.ShowMobListTimer = SpawnTimerList.Visible;

            this.mnuShowSpawnListTimer.Checked = SpawnTimerList.Visible;

        }

        private void GroundItemList_VisibleChanged(object sender, System.EventArgs e)
        {

            Settings.Instance.ShowGroundItemList = GroundItemList.Visible;

            this.mnuShowGroundItemList.Checked = GroundItemList.Visible;

        }



        #region ProcessProcessInfo

        private void ProcessProcessInfo(SPAWNINFO si) 

        {

            ProcessInfo PI = new ProcessInfo((int)si.SpawnID, si.Name);

            if (si.SpawnID==0)

            {
                PI.sCharName = "";
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

                

                switch (colProcesses.Count) 

                {

                    case 1: 

                    {

                        mnuChar1.Text = si.Name;

                        mnuChar1.Visible = true;

                        mnuChar1.Checked = ((CurrentProcess!=null) && (CurrentProcess.ProcessID == PI.ProcessID));

                        break;

                    }

                    case 2: 

                    {

                        mnuChar2.Text = si.Name;

                        mnuChar2.Visible = true;

                        mnuChar2.Checked = ((CurrentProcess!=null) && (CurrentProcess.ProcessID == PI.ProcessID));

                        break;

                    }

                    case 3: 

                    {

                        mnuChar3.Text = si.Name;

                        mnuChar3.Visible = true;

                        mnuChar3.Checked = ((CurrentProcess!=null) && (CurrentProcess.ProcessID == PI.ProcessID));

                        break;

                    }

                    case 4: 

                    {

                        mnuChar4.Text = si.Name;

                        mnuChar4.Visible = true;

                        mnuChar4.Checked = ((CurrentProcess!=null) && (CurrentProcess.ProcessID == PI.ProcessID));

                        break;

                    }

                    case 5: 

                    {

                        mnuChar5.Text = si.Name;

                        mnuChar5.Visible = true;

                        mnuChar5.Checked = ((CurrentProcess!=null) && (CurrentProcess.ProcessID == PI.ProcessID));

                        break;

                    }

                    case 6:
                    {

                        mnuChar6.Text = si.Name;

                        mnuChar6.Visible = true;

                        mnuChar6.Checked = ((CurrentProcess != null) && (CurrentProcess.ProcessID == PI.ProcessID));

                        break;

                    }

                    case 7:
                    {

                        mnuChar7.Text = si.Name;

                        mnuChar7.Visible = true;

                        mnuChar7.Checked = ((CurrentProcess != null) && (CurrentProcess.ProcessID == PI.ProcessID));

                        break;

                    }

                    case 8:
                    {

                        mnuChar8.Text = si.Name;

                        mnuChar8.Visible = true;

                        mnuChar8.Checked = ((CurrentProcess != null) && (CurrentProcess.ProcessID == PI.ProcessID));

                        break;

                    }

                    case 9:
                    {

                        mnuChar9.Text = si.Name;

                        mnuChar9.Visible = true;

                        mnuChar9.Checked = ((CurrentProcess != null) && (CurrentProcess.ProcessID == PI.ProcessID));

                        break;

                    }

                    case 10:
                    {

                        mnuChar10.Text = si.Name;

                        mnuChar10.Visible = true;

                        mnuChar10.Checked = ((CurrentProcess != null) && (CurrentProcess.ProcessID == PI.ProcessID));

                        break;

                    }
  
                    case 11:
                    {

                        mnuChar11.Text = si.Name;

                        mnuChar11.Visible = true;

                        mnuChar11.Checked = ((CurrentProcess != null) && (CurrentProcess.ProcessID == PI.ProcessID));

                        break;

                    }
 
                    case 12:
                    {

                        mnuChar12.Text = si.Name;

                        mnuChar12.Visible = true;

                        mnuChar12.Checked = ((CurrentProcess != null) && (CurrentProcess.ProcessID == PI.ProcessID));

                        break;

                    }
                   

                }

                switch (colProcesses.Count) 

                {

                    case 1: 

                    {

                        mnuChar2.Visible = false;
                        mnuChar2.Text = "Char 2";
                        mnuChar2.Checked = false;

                        goto case 2;

                    }

                    case 2: 

                    {

                        mnuChar3.Visible = false;
                        mnuChar3.Text = "Char 3";
                        mnuChar3.Checked = false;
                        goto case 3;

                    }

                    case 3: 

                    {

                        mnuChar4.Visible = false;
                        mnuChar4.Text = "Char 4";
                        mnuChar4.Checked = false;
                        goto case 4;

                    }

                    case 4: 

                    {

                        mnuChar5.Visible = false;
                        mnuChar5.Text = "Char 5";
                        mnuChar5.Checked = false;
                        goto case 5;

                    }

                    case 5:
                    {

                        mnuChar6.Visible = false;
                        mnuChar6.Text = "Char 6";
                        mnuChar6.Checked = false;
                        goto case 6;

                    }

                    case 6:
                    {

                        mnuChar7.Visible = false;
                        mnuChar7.Text = "Char 7";
                        mnuChar7.Checked = false;
                        goto case 7;

                    }

                    case 7:
                    {

                        mnuChar8.Visible = false;
                        mnuChar8.Text = "Char 8";
                        mnuChar8.Checked = false;
                        goto case 8;

                    }

                    case 8:
                    {

                        mnuChar9.Visible = false;
                        mnuChar9.Text = "Char 9";
                        mnuChar9.Checked = false;
                        goto case 9;

                    }

                    case 9:
                    {

                        mnuChar10.Visible = false;
                        mnuChar10.Text = "Char 10";
                        mnuChar10.Checked = false;
                        goto case 10;

                    }

                    case 10:
                    {

                        mnuChar11.Visible = false;
                        mnuChar11.Text = "Char 11";
                        mnuChar11.Checked = false;
                        goto case 11;

                    }

                    case 11:
                    {

                        mnuChar12.Visible = false;
                        mnuChar12.Text = "Char 12";
                        mnuChar12.Checked = false;
                        break;

                    }


                    default:

                        break;

                }

    

                

            }

        }

        #endregion



        #region ProccessMap

        public void ProcessMap(SPAWNINFO si) 

        {

            mapnameWithLabels = "";

            LogLib.WriteLine("Entering ProcessMap()", LogLevel.Trace);

            try 

            {

                LogLib.WriteLine("Short Zone Name: (" + si.Name + ")");

                bool foundmap = false;                

                string f = Settings.Instance.MapDir + "\\";

                string fn = si.Name.Trim();

                int location = fn.IndexOf("_", 0);

                if (location > 0)

                    fn = fn.Substring(0, location);

                LogLib.WriteLine("Using Short Zone Name: (" + fn + ")");

                f += fn;

                toolStripShortName.Text = fn.ToUpper(); 

                string newzonename = fn.ToUpper().Trim();

                curZone = newzonename;

                String ZonesFile = Path.Combine(Settings.Instance.CfgDir, "Zones.ini");

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
                        mapPane.TabText = sIniValue.ToString();
                    }
                }
                else
                {
                    if (curZone.Length > 0)
                        mapPane.TabText = curZone.ToLower();
                    else
                        mapPane.TabText = "map_pane";
                }

                // Turn off collecting mob trails anytime load a new map

                this.mnuCollectMobTrails.Checked = false;

                Settings.Instance.CollectMobTrails = false;
          
                // Start Delay for doing spawn alerts.  This stops sounds and emails.
                timDelayAlerts.Start();
                eq.DisablePlayAlerts();

                try 

                {
                    if (curZone.Length > 0 && curZone != "CLZ" && curZone != "DEFAULT")
                    {
                        // Try loading depth filter settings from file
                        String myPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MySEQ");

                        String ConfigFile = Path.Combine(myPath, "config.ini");
                        if (File.Exists(ConfigFile))
                        {

                            IniFile ConIni = new IniFile(ConfigFile);

                            string strIniValue = "";

                            strIniValue = ConIni.ReadValue("Zones", curZone, "");
                            if (strIniValue.Length > 0)
                            {
                                if ((strIniValue == "0" && Settings.Instance.DepthFilter) ||
                                (strIniValue == "1" && !Settings.Instance.DepthFilter))
                                    ToggleDepthFilter();
                            }
                            else
                            {
                                // We dont currently have a setting for this zone, so set to off
                                if (Settings.Instance.DepthFilter)
                                    ToggleDepthFilter();
                            }


                        }
                        else
                        {
                            // We dont currently have a setting file, so set depth filter to off
                            if (Settings.Instance.DepthFilter)
                                ToggleDepthFilter();
                        }

                    }

                    if (curZone.Length == 0 || curZone == "CLZ" || curZone == "DEFAULT")

                    {
                        if (Settings.Instance.DepthFilter)
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

                        if (Settings.Instance.ShowLayer1 && loadmap(f + "_1.txt"))
                            foundmap = true;

                        if (Settings.Instance.ShowLayer2 && loadmap(f + "_2.txt"))
                            foundmap = true;

                        if (Settings.Instance.ShowLayer3 && loadmap(f + "_3.txt"))
                            foundmap = true;
                        // try the _1, _2, _3 sequence if the plain .txt had no map lines
                        //if (eq.GetLinesReadonly().Count == 0)
                        //{
                        //    for (int mapnum = 1; mapnum < 4; mapnum++)
                        //    {
                        //        if (loadmap(string.Format("{0}_{1}.txt", f, mapnum)))
                        //        {
                        //            foundmap = true;
                        //        }
                        //    }
                        //}
                        //else
                        //{
                        //    // load labels from the _3.txt file
                        //    if (loadmap(string.Format("{0}_3.txt", f)))
                        //    {
                        //        foundmap = true;
                        //    }
                        //}

                        // use _3.txt file for map labels
                        if (foundmap)
                            mapnameWithLabels = string.Format("{0}_3.txt", f);

                        //SetTitle();

                    }
                    //... Missing map

                    if (!foundmap)
                    {

                        //this.Text = BaseTitle + " - Couldn't find a map for the zone: " + si.Name.ToString();

                        map.loadDummyMap(fn);
                        
                    }

                } 

                catch (Exception ex) 

                {

                    LogLib.WriteLine("Error in ProcessMap() Load Map: ", ex);

                    map.loadDummyMap(fn);

                }

                eq.longname = mapPane.TabText.ToString();

                filters.ClearArrays();

                filters.loadAlerts(fn);

                SetTitle();

                //this.Text = BaseTitle;

            }

            catch (Exception ex) {LogLib.WriteLine("Error in ProcessMap(): ", ex);}

            LogLib.WriteLine("Exiting ProcessMap()", LogLevel.Trace);

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

        /*

                #region AddSpawnTimer

                private void AddSpawnTimer(SPAWNINFO si, DateTime dt) {

                    EQData eq = eq;

                    eq.AddSpawnTimer(si,dt);

                }

                #endregion

        */

        private void SetGridInterval() 

        {

            mnuGridInterval100.Checked = false;

            mnuGridInterval250.Checked = false;

            mnuGridInterval500.Checked = false;

            mnuGridInterval1000.Checked = false;

            if (Settings.Instance.GridInterval<=100)

                mnuGridInterval100.Checked = true;

            else if (Settings.Instance.GridInterval<=250)

                mnuGridInterval250.Checked = true;

            else if (Settings.Instance.GridInterval<=500)

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

        private void SyncMenuText(ToolStripMenuItem sourceMenu, ToolStripMenuItem targetMenu)

        {
            if (sourceMenu.DropDownItems.Count == targetMenu.DropDownItems.Count)

            {

                IEnumerator sourceEnum = sourceMenu.DropDown.Items.GetEnumerator();

                IEnumerator targetEnum = targetMenu.DropDown.Items.GetEnumerator();

                while (sourceEnum.MoveNext())

                {

                    targetEnum.MoveNext();

                    ToolStripMenuItem sMenuItem = (ToolStripMenuItem)sourceEnum.Current;

                    ToolStripMenuItem tMenuItem = (ToolStripMenuItem)targetEnum.Current;

                    tMenuItem.Text = sMenuItem.Text;

                }

            }

        }

        private void SyncMenu(ToolStripMenuItem sourceMenu, ToolStripMenuItem targetMenu)

        {

            foreach (ToolStripMenuItem sMenuItem in sourceMenu.DropDownItems)

            {

                foreach(ToolStripMenuItem tMenuItem in targetMenu.DropDownItems)

                {

                    if (tMenuItem.Text == sMenuItem.Text && tMenuItem.Text != "")

                    {

                        if (sMenuItem.Text == "Server Selection" || sMenuItem.Text == "Character Selection")

                        {

                            SyncMenuText(sMenuItem, tMenuItem);

                        }

                        if (sMenuItem.DropDown.Items.Count > 0)

                        {

                            SyncMenu(sMenuItem, tMenuItem);

                        }

                        tMenuItem.Checked = sMenuItem.Checked;

                        tMenuItem.Visible = sMenuItem.Visible;

                        break;

                    }

                }

            }

        }

        public void SetContextMenu(bool notground = true)
        {
            if (alertAddmobname.Length > 0)
            {
                this.ContextMenuStrip = this.mnuContextAddFilter;
                // set text for mob name in the top
                this.mnuMobName.Text = "'" + alertAddmobname + "'";
                this.mnuMobName.Enabled = true;
                this.mnuMobName.Visible = true;
                // dont add email alerts for ground items
                this.addZoneEmailAlertFilterToolStripMenuItem.Enabled = notground;
                if (mapnameWithLabels.Length > 0)
                    this.mnuAddMapLabel.Enabled = true;
                else
                    this.mnuAddMapLabel.Enabled = false;

            }
            else
            {
                // Set the default context menu, since we don't have a proper name to work with
                this.ContextMenuStrip = this.mnuContext;
                if (eq.longname.Length > 0 && eq.playerinfo != null && eq.playerinfo.Name.Length > 0)
                {
                    addMapTextToolStripMenuItem.Enabled = true;
                }
                else
                {
                    addMapTextToolStripMenuItem.Enabled = false;
                }
                if (Settings.Instance.ShowMenuBar)
                {
                    this.mnuShowMenuBar.Visible = false;
                }
                else
                {
                    this.mnuShowMenuBar.Visible = true;
                }
            }
        }

        private void mnuOpenMap_Click(object sender, System.EventArgs e) 

        {

            openFileDialog.InitialDirectory = Settings.Instance.MapDir;

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

        private void mnuSaveMobs_Click(object sender, System.EventArgs e) 

        {

            eq.SaveMobs();

        }

        private void mnuSavePrefs_Click(object sender, System.EventArgs e) 

        {

            savePrefs("prefs.xml");

        }

        private void mnuExit_Click(object sender, System.EventArgs e) 

        {

            StopListening();

            this.Close();

            Application.Exit();

        } 

        private void mnuOptions_Click(object sender, System.EventArgs e) 

        {

            frmOptions f3 = new frmOptions();
            if (Settings.Instance.OptionsWindowsLocation.X != 0 && Settings.Instance.OptionsWindowsLocation.Y != 0)
            {
                f3.StartPosition = FormStartPosition.Manual;
                f3.Location = Settings.Instance.OptionsWindowsLocation;
                f3.Size = Settings.Instance.OptionsWindowsSize;
            }
            
            // Set the Options

            f3.txtIPAddress1.Text = Settings.Instance.IPAddress1;

            f3.txtIPAddress2.Text = Settings.Instance.IPAddress2;

            f3.txtIPAddress3.Text = Settings.Instance.IPAddress3;

            f3.txtIPAddress4.Text = Settings.Instance.IPAddress4;

            f3.txtIPAddress5.Text = Settings.Instance.IPAddress5;

            f3.txtPortNo.Text = Settings.Instance.Port.ToString();



            f3.spnOverrideLevel.Value = Settings.Instance.LevelOverride;

            f3.spnUpdateDelay.Value = Settings.Instance.UpdateDelay;

            f3.chkSaveOnExit.Checked = Settings.Instance.SaveOnExit;

            f3.chkPrefixAlerts.Checked = Settings.Instance.PrefixStars;

            f3.chkAffixAlerts.Checked = Settings.Instance.AffixStars;       // affix

            f3.chkCorpsesAlerts.Checked = Settings.Instance.CorpseAlerts;

            f3.txtHuntPrefix.Text = Settings.Instance.HuntPrefix;

            f3.chkHuntMatchFull.Checked = Settings.Instance.MatchFullTextH;  //hunt

            f3.optHuntNone.Checked = Settings.Instance.NoneOnHunt;

            f3.optHuntBeep.Checked =  Settings.Instance.BeepOnHunt;

            f3.optHuntSpeak.Checked = Settings.Instance.TalkOnHunt;

            f3.optHuntPlay.Checked = Settings.Instance.PlayOnHunt;

            f3.txtHuntAudioFile.Text = Settings.Instance.HuntAudioFile;



            f3.txtCautionPrefix.Text = Settings.Instance.CautionPrefix;

            f3.chkCautionMatchFull.Checked = Settings.Instance.MatchFullTextC;  //Caution

            f3.optCautionNone.Checked = Settings.Instance.NoneOnCaution;

            f3.optCautionBeep.Checked = Settings.Instance.BeepOnCaution;

            f3.optCautionSpeak.Checked = Settings.Instance.TalkOnCaution;

            f3.optCautionPlay.Checked = Settings.Instance.PlayOnCaution;

            f3.txtCautionAudioFile.Text = Settings.Instance.CautionAudioFile;



            f3.txtDangerPrefix.Text = Settings.Instance.DangerPrefix;

            f3.chkDangerMatchFull.Checked = Settings.Instance.MatchFullTextD;  //danger

            f3.optDangerNone.Checked = Settings.Instance.NoneOnDanger;

            f3.optDangerBeep.Checked = Settings.Instance.BeepOnDanger;

            f3.optDangerSpeak.Checked = Settings.Instance.TalkOnDanger;

            f3.optDangerPlay.Checked = Settings.Instance.PlayOnDanger;

            f3.txtDangerAudioFile.Text = Settings.Instance.DangerAudioFile;



            f3.txtAlertPrefix.Text = Settings.Instance.AlertPrefix;

            f3.chkAlertMatchFull.Checked = Settings.Instance.MatchFullTextA;  //Rare

            f3.optAlertNone.Checked = Settings.Instance.NoneOnAlert;

            f3.optAlertBeep.Checked = Settings.Instance.BeepOnAlert;

            f3.optAlertSpeak.Checked = Settings.Instance.TalkOnAlert;

            f3.optAlertPlay.Checked = Settings.Instance.PlayOnAlert;

            f3.txtAlertAudioFile.Text = Settings.Instance.AlertAudioFile;

            f3.spnRangeCircle.Value = Settings.Instance.RangeCircle;

            f3.numMinAlertLevel.Value = Settings.Instance.MinAlertLevel;

            f3.spnSpawnSize.Value = Settings.Instance.SpawnDrawSize;

            f3.FadedLines.Value = Settings.Instance.FadedLines;

            f3.pvpLevels.Value = Settings.Instance.PVPLevels;

            f3.txtWindowName.Text = Settings.Instance.TitleBar.ToString();

            f3.txtSearchString.Text = Settings.Instance.SearchString.ToString();

            f3.picMapBackgroundColor.BackColor = Settings.Instance.BackColor;

            f3.picListBackgroundColor.BackColor = Settings.Instance.ListBackColor;

            f3.picGridColor.BackColor = Settings.Instance.GridColor;

            f3.picGridLabelColor.BackColor = Settings.Instance.GridLabelColor;

            f3.picRangeCircleColor.BackColor = Settings.Instance.RangeCircleColor;

            f3.picPlayerBorder.BackColor = Settings.Instance.PCBorderColor;

            f3.chkColorRangeCircle.Checked = Settings.Instance.ColorRangeCircle;

            f3.cmbAlertSound.SelectedItem = Settings.Instance.AlertSound;

            f3.cmbHatch.SelectedItem = Settings.Instance.HatchIndex;

            f3.chkDrawFoV.Checked = Settings.Instance.DrawFoV;

            f3.chkShowZoneName.Checked = Settings.Instance.ShowZoneName;

            f3.chkShowCharName.Checked = Settings.Instance.ShowCharName;

            f3.chkShowTargetInfo.Checked = Settings.Instance.ShowTargetInfo;

            f3.txtMapDir.Text = Settings.Instance.MapDir;

            f3.txtFilterDir.Text = Settings.Instance.FilterDir;

            f3.txtCfgDir.Text = Settings.Instance.CfgDir;

            f3.txtLogDir.Text = Settings.Instance.LogDir;

            f3.txtTimerDir.Text = Settings.Instance.TimerDir;

            f3.spnLogLevel.Value = (int)Settings.Instance.MaxLogLevel;

            f3.chkSelectSpawnList.Checked = Settings.Instance.AutoSelectSpawnList;

            f3.SetFgDrawOptions(Settings.Instance.DrawOptions);

            // Show the Option Window

            f3.ShowDialog();
            if (f3.DialogResult.ToString() == "Cancel")
            {
                f3.Hide();
                return;
            }
            // Set the Settings

            if (Settings.Instance.IPAddress1 != f3.txtIPAddress1.Text)
                Settings.Instance.IPAddress1 = f3.txtIPAddress1.Text;

            if (Settings.Instance.IPAddress2 != f3.txtIPAddress2.Text)
                Settings.Instance.IPAddress2 = f3.txtIPAddress2.Text;

            if (Settings.Instance.IPAddress3 != f3.txtIPAddress3.Text)
                Settings.Instance.IPAddress3 = f3.txtIPAddress3.Text;

            if (Settings.Instance.IPAddress4 != f3.txtIPAddress4.Text)
                Settings.Instance.IPAddress4 = f3.txtIPAddress4.Text;

            if (Settings.Instance.IPAddress5 != f3.txtIPAddress5.Text)
                Settings.Instance.IPAddress5 = f3.txtIPAddress5.Text;

            if (Settings.Instance.Port != int.Parse(f3.txtPortNo.Text))
                Settings.Instance.Port = int.Parse(f3.txtPortNo.Text);

            if (Settings.Instance.LevelOverride != (int)f3.spnOverrideLevel.Value)
                Settings.Instance.LevelOverride = (int)f3.spnOverrideLevel.Value;

            if (Settings.Instance.SaveOnExit != f3.chkSaveOnExit.Checked)
                Settings.Instance.SaveOnExit = f3.chkSaveOnExit.Checked;

            if (Settings.Instance.UpdateDelay != (int)f3.spnUpdateDelay.Value)
            {
                Settings.Instance.UpdateDelay = (int)f3.spnUpdateDelay.Value;
                timPackets.Interval = Settings.Instance.UpdateDelay;
                SetUpdateSteps();
            }

            if (Settings.Instance.CorpseAlerts != f3.chkCorpsesAlerts.Checked) {
                Settings.Instance.CorpseAlerts = f3.chkCorpsesAlerts.Checked;
                reloadAlertFiles();
            }

            if (Settings.Instance.BackColor != f3.picMapBackgroundColor.BackColor) {
                Settings.Instance.BackColor = f3.picMapBackgroundColor.BackColor;
                resetMapPens();
            }

            if (Settings.Instance.PCBorderColor != f3.picPlayerBorder.BackColor)
            {
                Settings.Instance.PCBorderColor = f3.picPlayerBorder.BackColor;
                if (mapCon != null)
                    mapCon.UpdatePCBorder();
            }

            Settings.Instance.PrefixStars = f3.chkPrefixAlerts.Checked;

            Settings.Instance.AffixStars = f3.chkAffixAlerts.Checked;

            Settings.Instance.HuntPrefix = f3.txtHuntPrefix.Text;

            Settings.Instance.MatchFullTextH = f3.chkHuntMatchFull.Checked;  //hunt

            Settings.Instance.NoneOnHunt = f3.optHuntNone.Checked;

            Settings.Instance.BeepOnHunt = f3.optHuntBeep.Checked;

            Settings.Instance.TalkOnHunt = f3.optHuntSpeak.Checked;

            Settings.Instance.PlayOnHunt = f3.optHuntPlay.Checked;

            Settings.Instance.HuntAudioFile = f3.txtHuntAudioFile.Text;

            Settings.Instance.CautionPrefix = f3.txtCautionPrefix.Text;

            Settings.Instance.MatchFullTextC = f3.chkCautionMatchFull.Checked;  //Caution

            Settings.Instance.NoneOnCaution = f3.optCautionNone.Checked;

            Settings.Instance.BeepOnCaution = f3.optCautionBeep.Checked;

            Settings.Instance.TalkOnCaution = f3.optCautionSpeak.Checked;

            Settings.Instance.PlayOnCaution = f3.optCautionPlay.Checked;

            Settings.Instance.CautionAudioFile = f3.txtCautionAudioFile.Text;

            Settings.Instance.DangerPrefix = f3.txtDangerPrefix.Text;

            Settings.Instance.MatchFullTextD = f3.chkDangerMatchFull.Checked;  //Caution

            Settings.Instance.NoneOnDanger = f3.optDangerNone.Checked;

            Settings.Instance.BeepOnDanger = f3.optDangerBeep.Checked;

            Settings.Instance.TalkOnDanger = f3.optDangerSpeak.Checked;

            Settings.Instance.PlayOnDanger = f3.optDangerPlay.Checked;

            Settings.Instance.DangerAudioFile = f3.txtDangerAudioFile.Text;

            Settings.Instance.AlertPrefix = f3.txtAlertPrefix.Text;

            Settings.Instance.MatchFullTextA = f3.chkAlertMatchFull.Checked;  //Rare

            Settings.Instance.NoneOnAlert = f3.optAlertNone.Checked;

            Settings.Instance.BeepOnAlert = f3.optAlertBeep.Checked;

            Settings.Instance.TalkOnAlert = f3.optAlertSpeak.Checked;

            Settings.Instance.PlayOnAlert = f3.optAlertPlay.Checked;

            Settings.Instance.AlertAudioFile = f3.txtAlertAudioFile.Text;

            Settings.Instance.RangeCircle = (int)f3.spnRangeCircle.Value;

            Settings.Instance.DrawOptions = f3.GetDrawOptions();

            Settings.Instance.ShowTargetInfo = f3.chkShowTargetInfo.Checked;

            Settings.Instance.ShowZoneName = f3.chkShowZoneName.Checked;

            Settings.Instance.ShowCharName = f3.chkShowCharName.Checked;

            Settings.Instance.DrawFoV = f3.chkDrawFoV.Checked;

            Settings.Instance.ColorRangeCircle = f3.chkColorRangeCircle.Checked;

            Settings.Instance.AlertSound = f3.cmbAlertSound.SelectedItem.ToString();

            Settings.Instance.HatchIndex = f3.cmbHatch.SelectedItem.ToString();

            Settings.Instance.SpawnDrawSize = (int)f3.spnSpawnSize.Value;

            if (Settings.Instance.FadedLines != (int)f3.FadedLines.Value)
            {
                Settings.Instance.FadedLines = (int)f3.FadedLines.Value;
                resetMapPens();
            }

            Settings.Instance.PVPLevels = (int)f3.pvpLevels.Value;

            Settings.Instance.MinAlertLevel = (int)f3.numMinAlertLevel.Value;

            Settings.Instance.TitleBar = f3.txtWindowName.Text;

            Settings.Instance.SearchString = f3.txtSearchString.Text;

            SpawnList.listView.BackColor = Settings.Instance.ListBackColor;

            SpawnTimerList.listView.BackColor = Settings.Instance.ListBackColor;

            GroundItemList.listView.BackColor = Settings.Instance.ListBackColor;

            if (Settings.Instance.TitleBar.ToString().Length > 0)

                BaseTitle = Settings.Instance.TitleBar;

            Settings.Instance.MapDir = f3.txtMapDir.Text;

            Settings.Instance.FilterDir = f3.txtFilterDir.Text;

            Settings.Instance.CfgDir = f3.txtCfgDir.Text;

            Settings.Instance.LogDir = f3.txtLogDir.Text;

            Settings.Instance.TimerDir = f3.txtTimerDir.Text;

            Settings.Instance.AutoSelectSpawnList = f3.chkSelectSpawnList.Checked;

            Settings.Instance.OptionsWindowsLocation = f3.Location;

            Settings.Instance.OptionsWindowsSize = f3.Size;

            Settings.Instance.MaxLogLevel = (LogLevel)f3.spnLogLevel.Value;

            if (Settings.Instance.CurrentIPAddress == 0 && f3.txtIPAddress1.Text.Length > 0)

                Settings.Instance.CurrentIPAddress = 1;

            DrawOpts = Settings.Instance.DrawOptions;

            mnuShowGridLines.Checked = (Settings.Instance.DrawOptions & DrawOptions.GridLines) != DrawOptions.DrawNone;

            mnuShowZoneText.Checked = (Settings.Instance.DrawOptions & DrawOptions.ZoneText) != DrawOptions.DrawNone;
            mnuShowLayer1.Checked = Settings.Instance.ShowLayer1;
            mnuShowLayer2.Checked = Settings.Instance.ShowLayer2;
            mnuShowLayer3.Checked = Settings.Instance.ShowLayer3;
            mnuShowSpawnPoints.Checked = (Settings.Instance.DrawOptions & DrawOptions.SpawnTimers) != DrawOptions.DrawNone;

            ServerSelection();

            if (eq != null)

                eq.LoadSpawnInfo();

            SetTitle();

            savePrefs("prefs.xml");

            if (mapCon != null)

                mapCon.Invalidate();
        }

        private void ServerSelection() 

        {

            mnuIPAddress1.Text = Settings.Instance.IPAddress1;

            mnuIPAddress2.Text = Settings.Instance.IPAddress2;

            mnuIPAddress3.Text = Settings.Instance.IPAddress3;

            mnuIPAddress4.Text = Settings.Instance.IPAddress4;

            mnuIPAddress5.Text = Settings.Instance.IPAddress5;

            mnuIPAddress1.Enabled = mnuIPAddress1.Visible = Convert.ToBoolean(mnuIPAddress1.Text.Length);

            mnuIPAddress2.Enabled = mnuIPAddress2.Visible = Convert.ToBoolean(mnuIPAddress2.Text.Length);

            mnuIPAddress3.Enabled = mnuIPAddress3.Visible = Convert.ToBoolean(mnuIPAddress3.Text.Length);

            mnuIPAddress4.Enabled = mnuIPAddress4.Visible = Convert.ToBoolean(mnuIPAddress4.Text.Length);

            mnuIPAddress5.Enabled = mnuIPAddress5.Visible = Convert.ToBoolean(mnuIPAddress5.Text.Length);

        }

        private void mnuRefreshSpawnList_Click(object sender, System.EventArgs e) 

        {
            eq.DisablePlayAlerts();

            eq.mobsTimers.ResetTimers();

            map.ClearMap();

            eq.mobsTimers.LoadTimers();

        }

        private void mnuDepthFilter_Click(object sender, System.EventArgs e) 

        {
            ToggleDepthFilter();
            if (curZone.Length > 0 && curZone != "CLZ" && curZone != "DEFAULT")
            {
                try
                {
                    // Save depth filter settings to file
                    String myPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MySEQ");
                    if (!Directory.Exists(myPath))
                        Directory.CreateDirectory(myPath);
                    String ConfigFile = Path.Combine(myPath, "config.ini");

                    IniFile ConIni = new IniFile(ConfigFile);
                    if (Settings.Instance.DepthFilter)
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

            Settings.Instance.DepthFilter = mnuDepthFilter.Checked;

            // update the toolbar settings
            this.toolStripDepthFilterButton.Checked = Settings.Instance.DepthFilter;
            this.toolStripZNegUp.Enabled = Settings.Instance.DepthFilter;
            this.toolStripZNeg.Enabled = Settings.Instance.DepthFilter;
            this.toolStripZNegDown.Enabled = Settings.Instance.DepthFilter;
            this.toolStripZPosDown.Enabled = Settings.Instance.DepthFilter;
            this.toolStripZOffsetLabel.Enabled = Settings.Instance.DepthFilter;
            this.toolStripZPosUp.Enabled = Settings.Instance.DepthFilter;
            this.toolStripZPos.Enabled = Settings.Instance.DepthFilter;
            this.toolStripZPosLabel.Enabled = Settings.Instance.DepthFilter;
            this.toolStripResetDepthFilter.Enabled = Settings.Instance.DepthFilter;

            if (Settings.Instance.DepthFilter)
                this.toolStripDepthFilterButton.Image = global::myseq.Properties.Resources.ExpandSpaceHS;
            else
                this.toolStripDepthFilterButton.Image = global::myseq.Properties.Resources.ShrinkSpaceHS;

        }

        private void mnuDynamicAlpha_Click(object sender, System.EventArgs e)

        {

            mnuDynamicAlpha.Checked = !mnuDynamicAlpha.Checked;
            mnuDynamicAlpha2.Checked = mnuDynamicAlpha.Checked;


            Settings.Instance.UseDynamicAlpha = mnuDynamicAlpha.Checked;

        }

        private void mnuForceDistinct_Click(object sender, System.EventArgs e)

        {

            mnuForceDistinct.Checked = !mnuForceDistinct.Checked;
            mnuForceDistinct2.Checked = mnuForceDistinct.Checked;

            Settings.Instance.ForceDistinct = mnuForceDistinct.Checked;

            resetMapPens();

        }

        private void mnuShowGridLines_Click(object sender, System.EventArgs e) 

        {

            mnuShowGridLines.Checked = !mnuShowGridLines.Checked;

            if (mnuShowGridLines.Checked)

            {

                Settings.Instance.DrawOptions = Settings.Instance.DrawOptions | DrawOptions.GridLines;

            }

            else

            {

                Settings.Instance.DrawOptions = Settings.Instance.DrawOptions & (DrawOptions.DrawAll ^ DrawOptions.GridLines);

            }

            DrawOpts = Settings.Instance.DrawOptions;

            if (mapCon != null)

                mapCon.Invalidate();

        }

        private void mnuShowListGridLines_Click(object sender, System.EventArgs e)

        {

            mnuShowListGridLines.Checked = !mnuShowListGridLines.Checked;

            Settings.Instance.ShowListGridLines = mnuShowListGridLines.Checked;

            SpawnList.listView.GridLines = mnuShowListGridLines.Checked;

            SpawnTimerList.listView.GridLines = mnuShowListGridLines.Checked;

            GroundItemList.listView.GridLines = mnuShowListGridLines.Checked;

        }

        private void mnuGridInterval_Click(object sender, System.EventArgs e) 

        {

            mnuGridInterval100.Checked = sender.Equals(mnuGridInterval100);

            mnuGridInterval250.Checked = sender.Equals(mnuGridInterval250);

            mnuGridInterval500.Checked = sender.Equals(mnuGridInterval500);

            mnuGridInterval1000.Checked = sender.Equals(mnuGridInterval1000);

            Settings.Instance.GridInterval = GridInterval();

            if (mapCon != null)

                mapCon.Invalidate();

        }

        private void mnuGridColor_Click(object sender, System.EventArgs e) 

        {

            if(colorPicker.ShowDialog() != DialogResult.Cancel) 

            {

                if (Settings.Instance.GridColor != colorPicker.Color)

                {
                    Settings.Instance.GridColor = colorPicker.Color;

                    if (mapCon != null)

                        mapCon.Invalidate();
                }

            }

        }

        private void mnuBackgroungColor_Click(object sender, System.EventArgs e) 

        {

            if(colorPicker.ShowDialog() != DialogResult.Cancel) 

            {
                if (colorPicker.Color != Settings.Instance.BackColor)
                {

                    Settings.Instance.BackColor = colorPicker.Color;

                    resetMapPens();

                }

            }

        }

        private void SetFollowOption(FollowOption NewFollowOption)

        {

            Settings.Instance.FollowOption = NewFollowOption;

            if (NewFollowOption == FollowOption.None)
            {
                this.mnuFollowNone.Image = global::myseq.Properties.Resources.BlackX;
                this.mnuFollowNone2.Image = global::myseq.Properties.Resources.BlackX;
            }
            else
            {
                this.mnuFollowNone.Image = null;
                this.mnuFollowNone2.Image = null;
            }
            if (NewFollowOption == FollowOption.Player)
            {
                this.mnuFollowPlayer.Image = global::myseq.Properties.Resources.BlackX;
                this.mnuFollowPlayer2.Image = global::myseq.Properties.Resources.BlackX;
            }
            else
            {
                this.mnuFollowPlayer.Image = null;
                this.mnuFollowPlayer2.Image = null;
            }
            if (NewFollowOption == FollowOption.Target)
            {
                this.mnuFollowTarget.Image = global::myseq.Properties.Resources.BlackX;
                this.mnuFollowTarget2.Image = global::myseq.Properties.Resources.BlackX;
            }
            else
            {
                this.mnuFollowTarget.Image = null;
                this.mnuFollowTarget2.Image = null;
            }
            switch (NewFollowOption)

            {

                case FollowOption.None:

                    toolStripCoPStatus.Text = "";

                    break;

                case FollowOption.Player:

                    toolStripCoPStatus.Text = "CoP";

                    mapPane.offsetx.Value = 0;

                    mapPane.offsety.Value = 0;

                    break;

                case FollowOption.Target:

                    toolStripCoPStatus.Text = "CoT";

                    mapPane.offsetx.Value = 0;

                    mapPane.offsety.Value = 0;

                    break;

            }

        }

        private void mnuFollowNone_Click(object sender, System.EventArgs e)

        {

            SetFollowOption(FollowOption.None);

        }

        private void mnuFollowPlayer_Click(object sender, System.EventArgs e) 

        {

            SetFollowOption(FollowOption.Player);

        }

        private void mnuFollowTarget_Click(object sender, System.EventArgs e)

        {

            SetFollowOption(FollowOption.Target);

        }

        private void mnuReloadAlerts_Click(object sender, System.EventArgs e) 

        {
            if (bIsRunning == false)

                return;

            filters.ClearArrays();

            filters.loadAlerts(curZone);

            timDelayAlerts.Start();

            eq.DisablePlayAlerts();

            eq.mobsTimers.ResetTimers();
            if (map != null)
                map.ClearMap();

            eq.mobsTimers.LoadTimers();

        }   

        private void mnuAddEditAlerts_Click(object sender, System.EventArgs e) 

        {

            filters.editAlertFile(curZone);

        }

        private void mnuSpawnListFont_Click(object sender, System.EventArgs e) 

        {

            fontDialog1.Font = SpawnList.listView.Font;

            fontDialog1.ShowApply = true;

            if(fontDialog1.ShowDialog() != DialogResult.Cancel) 

            {

                SpawnList.listView.Font = fontDialog1.Font;

                SpawnTimerList.listView.Font = fontDialog1.Font;

                Settings.Instance.ListFontSize = fontDialog1.Font.Size;

                Settings.Instance.ListFontName = fontDialog1.Font.FontFamily.Name;

                Settings.Instance.ListFontStyle = fontDialog1.Font.Style;

                reloadAlertFiles();

            }

        }

        private void mnuCollectMobTrails_Click(object sender, System.EventArgs e) 

        {

            mnuCollectMobTrails.Checked = !mnuCollectMobTrails.Checked;   

            Settings.Instance.CollectMobTrails = mnuCollectMobTrails.Checked;

            if (mapCon != null)

                mapCon.Invalidate();

        }

        private void mnuShowSpawnList_Click(object sender, System.EventArgs e) 

        {

            mnuShowSpawnList.Checked = !mnuShowSpawnList.Checked;

            Settings.Instance.ShowMobList = mnuShowSpawnList.Checked;

            if (Settings.Instance.ShowMobList)

                SpawnList.Show(dockPanel);

            else

                SpawnList.Hide();

        }

        private void mnuShowSpawnListTimer_Click(object sender, System.EventArgs e)

        {

            mnuShowSpawnListTimer.Checked = !mnuShowSpawnListTimer.Checked;

            Settings.Instance.ShowMobListTimer = mnuShowSpawnListTimer.Checked;

            if (Settings.Instance.ShowMobListTimer)

                SpawnTimerList.Show(dockPanel);

            else

                SpawnTimerList.Hide();

        }

        private void mnuShowGroundItemList_Click(object sender, System.EventArgs e)
        {

            mnuShowGroundItemList.Checked = !mnuShowGroundItemList.Checked;

            Settings.Instance.ShowGroundItemList = mnuShowGroundItemList.Checked;

            if (Settings.Instance.ShowGroundItemList)

                GroundItemList.Show(dockPanel);

            else

                GroundItemList.Hide();

        }

        private void mnuShowMobTrails_Click(object sender, System.EventArgs e) 

        {

            mnuShowMobTrails.Checked = !mnuShowMobTrails.Checked;

            if (mnuShowMobTrails.Checked)

                Settings.Instance.DrawOptions = Settings.Instance.DrawOptions | DrawOptions.SpawnTrails;

            else

                Settings.Instance.DrawOptions = Settings.Instance.DrawOptions & (DrawOptions.DrawAll ^ DrawOptions.SpawnTrails);

            if (mapCon != null)

                mapCon.Invalidate();

        }

        private void mnuAbout_Click(object sender, System.EventArgs e) 

        {

            AboutDlg ab = new AboutDlg();
            this.TopMost = false;
            ab.ShowDialog();
            this.TopMost = mnuAlwaysOnTop.Checked;

        }

        private void mnuShowTargetInfo_Click(object sender, System.EventArgs e) 

        {
            Settings.Instance.ShowTargetInfo = !Settings.Instance.ShowTargetInfo;

            mnuShowTargetInfo.Checked = Settings.Instance.ShowTargetInfo;
            mnuShowTargetInfo2.Checked = Settings.Instance.ShowTargetInfo;

            if (mapCon != null)

                mapCon.Invalidate();

        }

        private void mnuListColor_Click(object sender, System.EventArgs e) 

        {

            if(colorPicker.ShowDialog() != DialogResult.Cancel) 

            {

                Settings.Instance.ListBackColor = colorPicker.Color;

                SpawnList.listView.BackColor = Settings.Instance.ListBackColor;

                SpawnTimerList.listView.BackColor = Settings.Instance.ListBackColor;

                GroundItemList.listView.BackColor = Settings.Instance.ListBackColor;

            }

        }

        private void mnuAutoSelectEQTarget_Click(object sender, System.EventArgs e) 

        {
            Settings.Instance.AutoSelectEQTarget = !Settings.Instance.AutoSelectEQTarget;
            
            mnuAutoSelectEQTarget.Checked = Settings.Instance.AutoSelectEQTarget;
            mnuAutoSelectEQTarget2.Checked = Settings.Instance.AutoSelectEQTarget;

        }

        private void mnuGlobalAlerts_Click(object sender, System.EventArgs e) 

        {

            filters.editAlertFile("global");

        }

        private void mnuShowNPCs_Click(object sender, System.EventArgs e)

        {

            mnuShowNPCs.Checked = !mnuShowNPCs.Checked;

            Settings.Instance.ShowNPCs = mnuShowNPCs.Checked;

            comm.UpdateHidden();

        }

        private void mnuShowLookupText_Click(object sender, System.EventArgs e)

        {

            mnuShowLookupText.Checked = !mnuShowLookupText.Checked;

            Settings.Instance.ShowLookupText = mnuShowLookupText.Checked;

            comm.UpdateHidden();

        }
        private void mnuAlwaysOnTop_Click(object sender, System.EventArgs e)

        {

            mnuAlwaysOnTop.Checked = !mnuAlwaysOnTop.Checked;

            Settings.Instance.AlwaysOnTop = mnuAlwaysOnTop.Checked;

            if (mnuAlwaysOnTop.Checked)
            {
                this.TopMost = true;
                this.TopLevel = true;
            } else
            {
                this.TopMost = false;
            }


        }

        private void mnuShowLookupNumber_Click(object sender, System.EventArgs e)

        {

            mnuShowLookupNumber.Checked = !mnuShowLookupNumber.Checked;

            Settings.Instance.ShowLookupNumber = mnuShowLookupNumber.Checked;

            comm.UpdateHidden();

        }

        private void mnuShowCorpses_Click(object sender, System.EventArgs e)

        {

            mnuShowCorpses.Checked = !mnuShowCorpses.Checked;

            Settings.Instance.ShowCorpses = mnuShowCorpses.Checked;

            comm.UpdateHidden();

        }

        private void mnuShowPlayers_Click(object sender, System.EventArgs e)

        {

            mnuShowPlayers.Checked = !mnuShowPlayers.Checked;

            Settings.Instance.ShowPlayers = mnuShowPlayers.Checked;

            comm.UpdateHidden();

        }

        private void mnuShowInvis_Click(object sender, System.EventArgs e)

        {

            mnuShowInvis.Checked = !mnuShowInvis.Checked;

            Settings.Instance.ShowInvis = mnuShowInvis.Checked;

            comm.UpdateHidden();

        }

        private void mnuShowMounts_Click(object sender, System.EventArgs e)

        {

            mnuShowMounts.Checked = !mnuShowMounts.Checked;

            Settings.Instance.ShowMounts = mnuShowMounts.Checked;

            comm.UpdateHidden();
        }

        private void mnuShowFamiliars_Click(object sender, System.EventArgs e)

        {

            mnuShowFamiliars.Checked = !mnuShowFamiliars.Checked;

            Settings.Instance.ShowFamiliars = mnuShowFamiliars.Checked;

            comm.UpdateHidden();
        }

        private void mnuShowPets_Click(object sender, System.EventArgs e)

        {

            mnuShowPets.Checked = !mnuShowPets.Checked;

            Settings.Instance.ShowPets = mnuShowPets.Checked;

            comm.UpdateHidden();

        }     

        private void mnuTargetInfoFont_Click(object sender, System.EventArgs e) 

        {

            fontDialog1.Font = mapCon.lblMobInfo.Font;

            fontDialog1.ShowApply = true;

            if(fontDialog1.ShowDialog() != DialogResult.Cancel) 

            {

                mapCon.lblMobInfo.Font = fontDialog1.Font;

                mapCon.lblGameClock.Font = new System.Drawing.Font(fontDialog1.Font.FontFamily.Name,fontDialog1.Font.Size, FontStyle.Bold);

                Settings.Instance.TargetInfoFontSize = fontDialog1.Font.Size;

                Settings.Instance.TargetInfoFontName = fontDialog1.Font.FontFamily.Name;

                Settings.Instance.TargetInfoFontStyle = fontDialog1.Font.Style;

            }

        }

        private void mnuShowSpawnPoints_Click(object sender, System.EventArgs e) 

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

            if (mnuShowSpawnPoints.Checked)

            {

                Settings.Instance.DrawOptions = Settings.Instance.DrawOptions | DrawOptions.SpawnTimers;

            }

            else

            {

                Settings.Instance.DrawOptions = Settings.Instance.DrawOptions & (DrawOptions.DrawAll ^ DrawOptions.SpawnTimers);

            }

        }

        private void mnuIPAddress1_Click(object sender, System.EventArgs e) 

        {

            ResetMenu(1);

            Restart();

        }

        private void mnuIPAddress2_Click(object sender, System.EventArgs e) 

        {

            ResetMenu(2);

            Restart();

        }

        private void mnuIPAddress3_Click(object sender, System.EventArgs e) 

        {

            ResetMenu(3);

            Restart();

        }

        private void mnuIPAddress4_Click(object sender, System.EventArgs e) 

        {

            ResetMenu(4);

            Restart();

        }

        private void mnuIPAddress5_Click(object sender, System.EventArgs e) 

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

            Settings.Instance.CurrentIPAddress = isCheck;

            switch (isCheck) 

            {

                case 1: {mnuIPAddress1.Checked = true; currentIPAddress = Settings.Instance.IPAddress1; break;}

                case 2: {mnuIPAddress2.Checked = true; currentIPAddress = Settings.Instance.IPAddress2; break;}

                case 3: {mnuIPAddress3.Checked = true; currentIPAddress = Settings.Instance.IPAddress3; break;}

                case 4: {mnuIPAddress4.Checked = true; currentIPAddress = Settings.Instance.IPAddress4; break;}

                case 5: {mnuIPAddress5.Checked = true; currentIPAddress = Settings.Instance.IPAddress5; break;}

            }

        } 

        private void Restart() 

        {
            comm.StopListening();

            if (bIsRunning)

                StopListening();

            StartListening();

        }

        private void mnuCharRefresh_Click(object sender, System.EventArgs e)

        {

            colProcesses.Clear();

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

        private void SwitchCharacter(int CharacterIndex)

        {

            if (colProcesses.Count>=CharacterIndex)

            {

                ProcessInfo PI = (ProcessInfo) colProcesses[CharacterIndex-1];
                
                comm.SwitchCharacter(PI);

            }

        }



        private void mnuChar1_Click(object sender, System.EventArgs e)
        {
            if (!mnuChar1.Checked && comm.CanSwitchChars())
                SwitchCharacter(1);
        }



        private void mnuChar2_Click(object sender, System.EventArgs e)
        {
            if (!mnuChar2.Checked && comm.CanSwitchChars())
                SwitchCharacter(2);
        }



        private void mnuChar3_Click(object sender, System.EventArgs e)

        {
             SwitchCharacter(3);
        }



        private void mnuChar4_Click(object sender, System.EventArgs e)

        {
             SwitchCharacter(4);
        }



        private void mnuChar5_Click(object sender, System.EventArgs e)

        {
             SwitchCharacter(5);
        }



        private void mnuChar6_Click(object sender, System.EventArgs e)
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
       

        private void mnuKeepCentered_Click(object sender, System.EventArgs e)

        {

            Settings.Instance.KeepCentered = !Settings.Instance.KeepCentered;

            mnuKeepCentered.Checked = Settings.Instance.KeepCentered;
            mnuKeepCentered2.Checked = Settings.Instance.KeepCentered;

        }

        public void ReAdjust()
        {
            if (mapCon !=null)
                mapCon.ReAdjust();
        }

        public void reloadAlertFiles()

        {

            filters.ClearArrays();

            filters.loadAlerts(curZone);

            timDelayAlerts.Start();

            eq.DisablePlayAlerts();

            eq.mobsTimers.ResetTimers();

            map.ClearMap();

            eq.mobsTimers.LoadTimers();

            if (this.toolStripLookupBox.Text.Length > 0 && toolStripLookupBox.Text != "Mob Search")

                eq.MarkLookups("0:" + this.toolStripLookupBox.Text, bFilter0);

            if (this.toolStripLookupBox1.Text.Length > 0 && toolStripLookupBox1.Text != "Mob Search")

                eq.MarkLookups("1:" + this.toolStripLookupBox1.Text, bFilter1);

            if (this.toolStripLookupBox2.Text.Length > 0 && toolStripLookupBox2.Text != "Mob Search")

                eq.MarkLookups("2:" + this.toolStripLookupBox2.Text, bFilter2);

            if (this.toolStripLookupBox3.Text.Length > 0 && toolStripLookupBox2.Text != "Mob Search")

                eq.MarkLookups("3:" + this.toolStripLookupBox2.Text, bFilter3);

            if (this.toolStripLookupBox4.Text.Length > 0 && toolStripLookupBox2.Text != "Mob Search")

                eq.MarkLookups("4:" + this.toolStripLookupBox2.Text, bFilter4);

            if (this.toolStripLookupBox5.Text.Length > 0 && toolStripLookupBox2.Text != "Mob Search")

                eq.MarkLookups("5:" + this.toolStripLookupBox2.Text, bFilter5);
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

                    eq.ProcessGroundItems(si,filters,GroundItemList);

                    break;

                case SPAWNINFO.PacketType.Target:

                    eq.ProcessTarget(si);

                    break;

                case SPAWNINFO.PacketType.World:

                    eq.ProcessWorld(si);

                    break;

                case SPAWNINFO.PacketType.Spawn:

                    eq.ProcessSpawns(si,this,SpawnList,filters,mapPane,reHelper, update_hidden);

                    break;

                case SPAWNINFO.PacketType.GetProcessInfo:

                    ProcessProcessInfo(si);

                    if (eq.playerinfo != null && eq.playerinfo.Name.Length > 1)
                        SetCharSelection(eq.playerinfo.Name.ToString());

                    break;

                default:

                    this.Text = "Unknown Packet Type: " + si.flags.ToString();

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

                Settings.Instance.MapLabelFontSize = fontDialog1.Font.Size;

                Settings.Instance.MapLabelFontName = fontDialog1.Font.FontFamily.Name;

                Settings.Instance.MapLabelFontStyle = fontDialog1.Font.Style;

                mapCon.drawFont1 = new Font(Settings.Instance.MapLabelFontName, Settings.Instance.MapLabelFontSize * 0.9f, Settings.Instance.MapLabelFontStyle);
                mapCon.drawFont3 = new Font(Settings.Instance.MapLabelFontName, Settings.Instance.MapLabelFontSize * 1.1f, Settings.Instance.MapLabelFontStyle);

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

                Settings.Instance.GridLabelColor = colorPicker.Color;

                if (mapCon != null)

                    mapCon.Invalidate();

            }

        }

        public void addMapText(string textToAdd)
        {
            frmAddMapText mapBox = new frmAddMapText();
            mapBox.txtColr = Settings.Instance.SelectedAddMapText;
            string new_text = textToAdd.Replace("#","");

            if (addTextFormLocation.X != 0 && addTextFormLocation.Y != 0)
            {
                mapBox.StartPosition = FormStartPosition.Manual;
                mapBox.Location = addTextFormLocation;
            }

            if (new_text.Length > 0)
                mapBox.txtAdd = new_text;
            else
                mapBox.txtAdd = "Enter Text Label";
            mapBox.txtBkg = Settings.Instance.BackColor;
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

            mapBox.updateColorBoxes();
            addTextFormLocation = mapBox.Location;
            if (mapBox.ShowDialog() == DialogResult.OK)
            {
                // save selected color
                if (Settings.Instance.SelectedAddMapText != mapBox.txtColr)
                    Settings.Instance.SelectedAddMapText = mapBox.txtColr;
                // we have a valid addition of text
                new_text = mapBox.txtAdd.TrimEnd('_', ' ');
                
                // add it to map now
                if (new_text.Length > 0)
                {
                    if(mapnameWithLabels.EndsWith(".txt")) {

                        MapText work = new MapText();
                        work.text = new_text;
                        work.x = (int)alertX;
                        work.y = (int)alertY;
                        work.z = (int)alertZ;
                        work.size = mapBox.txtSize;
                        work.color = new SolidBrush(mapBox.txtColr);
                        work.draw_color = eq.GetDistinctColor(work.color);
                        eq.AddMapText(work);

                        // string to append to map file
                        new_text = new_text.Replace(" ", "_");
                        string soe_maptext = String.Format("P {0:f4}, {1:f4}, {2:f4}, {3}, {4}, {5}, {6}, {7}\n", this.alertX * -1, this.alertY * -1, this.alertZ, mapBox.txtColr.R, mapBox.txtColr.G, mapBox.txtColr.B, mapBox.txtSize, new_text);
                        if (DialogResult.Yes == MessageBox.Show("Do you want to write the label to " + mapBox.mapName + "?" + Environment.NewLine + Environment.NewLine + soe_maptext, "Write label to map file?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                            File.AppendAllText(mapnameWithLabels, soe_maptext);
                        else
                            eq.DeleteMapText(work);
                    }
                    else if (mapnameWithLabels.EndsWith(".map"))
                    {
                        // string to append to .map file
                        string seq_maptext = String.Format("P,{0},{1},{2:f0},{3:f0},{4:f0}\n", new_text, mapBox.txtColr.Name, alertX, alertY, alertZ);
                        
                        MapText work = new MapText();
                        work.text = new_text;
                        work.x = (int)alertX;
                        work.y = (int)alertY;
                        work.z = (int)alertZ;
                        work.color = new SolidBrush(mapBox.txtColr);
                        work.draw_color = eq.GetDistinctColor(work.color);
                        eq.AddMapText(work);

                        if (DialogResult.Yes == MessageBox.Show("Do you want to write the label to " + mapBox.mapName + "?" + Environment.NewLine + Environment.NewLine + seq_maptext, "Write label to map file?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                            File.AppendAllText(mapnameWithLabels, seq_maptext);
                        else
                            eq.DeleteMapText(work);
                    }

                }
            }
        }

        public bool dialogBox(string titleText, string labelText, string dialogText)

        {

            frmDialogBox dlgBox = new frmDialogBox();

            dlgBox.dlgTitle = titleText;

            dlgBox.dlgLabel = labelText;

            dlgBox.dlgTextBox = dialogText;

            dlgBox.TopMost = true;

            if (dlgBox.ShowDialog() == DialogResult.OK)

            {

                if (dlgBox.dlgTextBox.Length > 0)

                {

                    SpawnList.mobname = dlgBox.dlgTextBox;

                    SpawnTimerList.mobname = dlgBox.dlgTextBox;

                    return true;

                }

                else

                    return false;

            }

            else

            {

                return false;

            }

        }



        private void mnuSaveAlerts_Click(object sender, EventArgs e)

        {

            filters.writeAlertFile(curZone);

            filters.writeAlertFile("global");

        }



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
            

            if (mnuShowZoneText.Checked)

            {

                Settings.Instance.DrawOptions = Settings.Instance.DrawOptions | DrawOptions.ZoneText;

            }

            else

            {

                Settings.Instance.DrawOptions = Settings.Instance.DrawOptions & (DrawOptions.DrawAll ^ DrawOptions.ZoneText);

            }

            DrawOpts = Settings.Instance.DrawOptions;

            if (mapCon != null)

                mapCon.Invalidate();

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

            Settings.Instance.ShowLayer1 = mnuShowLayer1.Checked;

            string f = Settings.Instance.MapDir + "\\" + eq.shortname;
            bool foundmap = false;
            if (loadmap(f + ".txt"))
                foundmap = true;

            if (Settings.Instance.ShowLayer1 && loadmap(f + "_1.txt"))
                foundmap = true;

            if (Settings.Instance.ShowLayer2 && loadmap(f + "_2.txt"))
                foundmap = true;

            if (Settings.Instance.ShowLayer3 && loadmap(f + "_3.txt"))
                foundmap = true;

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


            Settings.Instance.ShowLayer2 = mnuShowLayer2.Checked;

            string f = Settings.Instance.MapDir + "\\" + eq.shortname;
            bool foundmap = false;
            if (loadmap(f + ".txt"))
                foundmap = true;

            if (Settings.Instance.ShowLayer1 && loadmap(f + "_1.txt"))
                foundmap = true;

            if (Settings.Instance.ShowLayer2 && loadmap(f + "_2.txt"))
                foundmap = true;

            if (Settings.Instance.ShowLayer3 && loadmap(f + "_3.txt"))
                foundmap = true;

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

            Settings.Instance.ShowLayer3 = mnuShowLayer3.Checked;

            string f = Settings.Instance.MapDir + "\\" + eq.shortname;
            bool foundmap = false;
            if (loadmap(f + ".txt"))
                foundmap = true;

            if (Settings.Instance.ShowLayer1 && loadmap(f + "_1.txt"))
                foundmap = true;

            if (Settings.Instance.ShowLayer2 && loadmap(f + "_2.txt"))
                foundmap = true;

            if (Settings.Instance.ShowLayer3 && loadmap(f + "_3.txt"))
                foundmap = true;

        }


        private void mnuSodTitanium_Click(object sender, EventArgs e)

        {

            mnuConSoD.Checked = true;

            mnuConSoF.Checked = false;

            mnuConDefault.Checked = false;

            Settings.Instance.SoDCon = true;

            Settings.Instance.SoFCon = false;

            Settings.Instance.DefaultCon = false;

            eq.fillConColors(this);

            eq.UpdateMobListColors();

            

        }



        private void mnuConDefault_Click(object sender, EventArgs e)

        {

            mnuConSoD.Checked = false;

            mnuConSoF.Checked = false;

            mnuConDefault.Checked = true;

            Settings.Instance.SoDCon = false;

            Settings.Instance.SoFCon = false;

            Settings.Instance.DefaultCon = true;

            eq.fillConColors(this);

            eq.UpdateMobListColors();

        }



        private void mnuConSoF_Click(object sender, EventArgs e)

        {

            mnuConSoD.Checked = false;

            mnuConSoF.Checked = true;

            mnuConDefault.Checked = false;

            Settings.Instance.SoDCon = false;

            Settings.Instance.SoFCon = true;

            Settings.Instance.DefaultCon = false;

            eq.fillConColors(this);

            eq.UpdateMobListColors();

        }



        private void mnuShowPCNames_Click(object sender, EventArgs e)

        {

            Settings.Instance.ShowPCNames = !Settings.Instance.ShowPCNames;

            mnuShowPCNames.Checked = Settings.Instance.ShowPCNames;
            mnuShowPCNames2.Checked = Settings.Instance.ShowPCNames;

        }



        private void mnuShowNPCNames_Click(object sender, EventArgs e)

        {

            Settings.Instance.ShowNPCNames = !Settings.Instance.ShowNPCNames;

            mnuShowNPCNames.Checked = Settings.Instance.ShowNPCNames;
            mnuShowNPCNames2.Checked = Settings.Instance.ShowNPCNames;

        }



        private void mnuShowNPCCorpseNames_Click(object sender, EventArgs e)

        {

            Settings.Instance.ShowNPCCorpseNames = !Settings.Instance.ShowNPCCorpseNames;

            mnuShowNPCCorpseNames.Checked = Settings.Instance.ShowNPCCorpseNames;
            mnuShowNPCCorpseNames2.Checked = Settings.Instance.ShowNPCCorpseNames;

        }



        private void mnuShowPlayerCorpseNames_Click(object sender, EventArgs e)

        {

            Settings.Instance.ShowPlayerCorpseNames = !Settings.Instance.ShowPlayerCorpseNames;

            mnuShowPlayerCorpseNames.Checked = Settings.Instance.ShowPlayerCorpseNames;
            mnuShowPlayerCorpseNames2.Checked = Settings.Instance.ShowPlayerCorpseNames;

        }

        private void mnuFilterMapLines_Click(object sender, EventArgs e)

        {

            Settings.Instance.FilterMapLines = !Settings.Instance.FilterMapLines;

            mnuFilterMapLines.Checked = Settings.Instance.FilterMapLines;
            mnuFilterMapLines2.Checked = Settings.Instance.FilterMapLines;

        }

        private void mnuFilterMapText_Click(object sender, EventArgs e)
        {

            Settings.Instance.FilterMapText = !Settings.Instance.FilterMapText;

            mnuFilterMapText.Checked = Settings.Instance.FilterMapText;
            mnuFilterMapText2.Checked = Settings.Instance.FilterMapText;

        }

        private void mnuFilterNPCs_Click(object sender, EventArgs e)

        {

            Settings.Instance.FilterNPCs = !Settings.Instance.FilterNPCs;

            mnuFilterNPCs.Checked = Settings.Instance.FilterNPCs;
            mnuFilterNPCs2.Checked = Settings.Instance.FilterNPCs;

        }



        private void mnuFilterPlayers_Click(object sender, EventArgs e)

        {

            Settings.Instance.FilterPlayers = !Settings.Instance.FilterPlayers;

            mnuFilterPlayers.Checked = Settings.Instance.FilterPlayers;
            mnuFilterPlayers2.Checked = Settings.Instance.FilterPlayers;

        }



        private void mnuFilterSpawnPoints_Click(object sender, EventArgs e)

        {

            Settings.Instance.FilterSpawnPoints = !Settings.Instance.FilterSpawnPoints;

            mnuFilterSpawnPoints.Checked = Settings.Instance.FilterSpawnPoints;
            mnuFilterSpawnPoints2.Checked = Settings.Instance.FilterSpawnPoints;

        }

        private void mnuFilterPlayerCorpses_Click(object sender, EventArgs e)

        {

            Settings.Instance.FilterPlayerCorpses = !Settings.Instance.FilterPlayerCorpses;

            mnuFilterPlayerCorpses.Checked = Settings.Instance.FilterPlayerCorpses;
            mnuFilterPlayerCorpses2.Checked = Settings.Instance.FilterPlayerCorpses;

        }

        private void mnuFilterGroundItems_Click(object sender, EventArgs e)
        {

            Settings.Instance.FilterGroundItems = !Settings.Instance.FilterGroundItems;

            mnuFilterGroundItems.Checked = Settings.Instance.FilterGroundItems;
            mnuFilterGroundItems2.Checked = Settings.Instance.FilterGroundItems;

        }

        private void mnuFilterNPCCorpses_Click(object sender, EventArgs e)

        {

            Settings.Instance.FilterNPCCorpses = !Settings.Instance.FilterNPCCorpses;

            mnuFilterNPCCorpses.Checked = Settings.Instance.FilterNPCCorpses;
            mnuFilterNPCCorpses2.Checked = Settings.Instance.FilterNPCCorpses;

        }

        private void mnuShowPVP_Click(object sender, EventArgs e)

        {

            Settings.Instance.ShowPVP = !Settings.Instance.ShowPVP;

            mnuShowPVP.Checked = Settings.Instance.ShowPVP;
            mnuShowPVP2.Checked = Settings.Instance.ShowPVP;

        }

        private void mnuShowPVPLevel_Click(object sender, EventArgs e)

        {

            Settings.Instance.ShowPVPLevel = !Settings.Instance.ShowPVPLevel;

            mnuShowPVPLevel.Checked = Settings.Instance.ShowPVPLevel;
            mnuShowPVPLevel2.Checked = Settings.Instance.ShowPVPLevel;

        }



        private void mnuShowNPCLevels_Click(object sender, EventArgs e)

        {

            Settings.Instance.ShowNPCLevels = !Settings.Instance.ShowNPCLevels;

            mnuShowNPCLevels.Checked = Settings.Instance.ShowNPCLevels;
            mnuShowNPCLevels2.Checked = Settings.Instance.ShowNPCLevels;

        }



        private void mnuAutoExpand_Click(object sender, EventArgs e)

        {

            Settings.Instance.AutoExpand = !Settings.Instance.AutoExpand;

            mnuAutoExpand.Checked = Settings.Instance.AutoExpand;
            mnuAutoExpand2.Checked = Settings.Instance.AutoExpand;

        }



        private void mnuSaveSpawnLog_Click(object sender, EventArgs e)

        {

            mnuSaveSpawnLog.Checked = !mnuSaveSpawnLog.Checked;

            Settings.Instance.SaveSpawnLogs = mnuSaveSpawnLog.Checked;

        }



        private void mnuSpawnCountdown_Click(object sender, EventArgs e)

        {

            Settings.Instance.SpawnCountdown = !Settings.Instance.SpawnCountdown;
            
            mnuSpawnCountdown.Checked = Settings.Instance.SpawnCountdown;
            mnuSpawnCountdown2.Checked = Settings.Instance.SpawnCountdown;

        }



        private void mnuShowPCCorpses_Click(object sender, EventArgs e)

        {

            mnuShowPCCorpses.Checked = !mnuShowPCCorpses.Checked;

            Settings.Instance.ShowPCCorpses = mnuShowPCCorpses.Checked;

            comm.UpdateHidden();
        }



        private void mnuShowMyCorpse_Click(object sender, EventArgs e)

        {

            mnuShowMyCorpse.Checked = !mnuShowMyCorpse.Checked;

            Settings.Instance.ShowMyCorpse = mnuShowMyCorpse.Checked;

            comm.UpdateHidden();

        }

        private void mnuForceDistinctText_Click(object sender, EventArgs e)
        {
            Settings.Instance.ForceDistinctText = !Settings.Instance.ForceDistinctText;

            mnuForceDistinctText.Checked = Settings.Instance.ForceDistinctText;
            mnuForceDistinctText2.Checked = Settings.Instance.ForceDistinctText;

            resetMapPens();

        }

        private void mnuAddHuntFilter_Click(object sender, EventArgs e)
        {
            if (dialogBox("Add to Zone Hunt Filters", "Add name to Hunt list:", alertAddmobname))
            {

                filters.AddToAlerts(filters.hunt, alertAddmobname);

                filters.writeAlertFile(curZone);

                reloadAlertFiles();

            }
        }

        private void mnuAddCautionFilter_Click(object sender, EventArgs e)
        {
            if (dialogBox("Add to Zone Caution Filters", "Add name to Caution list:", alertAddmobname))
            {

                filters.AddToAlerts(filters.caution, alertAddmobname);

                filters.writeAlertFile(curZone);

                reloadAlertFiles();

            }
        }

        private void mnuAddDangerFilter_Click(object sender, EventArgs e)
        {
            if (dialogBox("Add to Zone Danger Alert Filters", "Add name to Danger list:", alertAddmobname))
            {

                filters.AddToAlerts(filters.danger, alertAddmobname);

                filters.writeAlertFile(curZone);

                reloadAlertFiles();

            }
        }

        private void mnuAddAlertFilter_Click(object sender, EventArgs e)
        {
            if (dialogBox("Add to Zone Rare Alert Filters", "Add name to Rare list:", alertAddmobname))
            {

                filters.AddToAlerts(filters.alert, alertAddmobname);

                filters.writeAlertFile(curZone);

                reloadAlertFiles();

            }
        }

        private void mnuSearchAllakhazam_Click(object sender, EventArgs e)
        {
            string searchname = Regex.Replace(alertAddmobname.Replace("_", " "), "[0-9#]", "").Trim();

            if (searchname.Length > 0)
            {

                string searchURL = String.Format(Settings.Instance.SearchString, searchname);

                System.Diagnostics.Process.Start(searchURL);

            }
        }

        private void mnuShowMenuBar_Click(object sender, EventArgs e)
        {
            Settings.Instance.ShowMenuBar = !Settings.Instance.ShowMenuBar;

            mnuShowMenuBar.Checked = Settings.Instance.ShowMenuBar;
            this.mnuViewMenuBar.Checked = Settings.Instance.ShowMenuBar;

            if (Settings.Instance.ShowMenuBar)
            {
                this.mnuMainMenu.Show();
            }
            else
            {
                this.mnuMainMenu.Hide();
            }
        }

        private void menuItem5_Click(object sender, EventArgs e)
        {
            this.addMapText("Testing");
        }

        private void mnuViewStatusBar_Click(object sender, EventArgs e)
        {
            Settings.Instance.ShowStatusBar = !Settings.Instance.ShowStatusBar;

            this.mnuViewStatusBar.Checked = Settings.Instance.ShowStatusBar;
            if (Settings.Instance.ShowStatusBar)
                this.statusBarStrip.Show();
            else
                this.statusBarStrip.Hide();
        }

        private void mnuViewDepthFilterToolBar_Click(object sender, EventArgs e)
        {
            Settings.Instance.ShowToolBar = !Settings.Instance.ShowToolBar;

            this.mnuViewDepthFilterBar.Checked = Settings.Instance.ShowToolBar;
            if (Settings.Instance.ShowToolBar)
            {
                this.toolBarStrip.Show();
            }
            else
            {
                this.toolBarStrip.Hide();
            }
        }

        private void toolStripScale_TextUpdate(object sender, EventArgs e)
        {
            string Str = this.toolStripScale.Text.Trim();
            Str = Str.Replace("%", "");

            double Num;
            bool validnum = false;
            if (Str.Length > 0)
            {
                bool isNum = double.TryParse(Str, out Num);

                if (isNum)
                {
                    if (Num < 1 || Num > (double)mapPane.scale.Maximum)
                        validnum = false;
                    else
                        validnum = true;
                }  
            }


            if (!validnum)
            {
                toolStripScale.Text = String.Format("{0:0.0%}", mapPane.scale.Value / 100);
                MessageBox.Show(String.Format("Enter a number between {0} and {1}", mapPane.scale.Minimum, mapPane.scale.Maximum), "Invalid Value Entered.");
            }

        }

        private void toolStripScale_Leave(object sender, EventArgs e)
        {
            string Str = this.toolStripScale.Text.Trim();
            Str = Str.Replace("%", "");

            double Num;
            bool validnum = false;
            if (Str.Length > 0)
            {
                bool isNum = double.TryParse(Str, out Num);

                if (isNum)
                {
                    if (Num >= (double)mapPane.scale.Minimum && Num <= (double)mapPane.scale.Maximum)
                    {
                        mapPane.scale.Value = (decimal)Num;
                        validnum = true;
                    } else

                        validnum = false;
                }
            }

            if (!validnum)
            {
                toolStripScale.Text = String.Format("{0:0.0%}", mapPane.scale.Value / 100);
                MessageBox.Show(String.Format("Enter a number between {0} and {1}", mapPane.scale.Minimum, mapPane.scale.Maximum), "Invalid Value Entered.");
            } else
                toolStripScale.Text = String.Format("{0:0.0%}", mapPane.scale.Value / 100);
        }

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

        private void toolStripZPos_TextChanged(object sender, EventArgs e)
        {
            // validate that text is a usable number
            // allow a value of 0 to 3500
            string Str = this.toolStripZPos.Text.Trim();
            
            double Num;
            bool validnum = true;
            if (Str.Length > 0)
            {
                bool isNum = double.TryParse(Str, out Num);
                validnum = false;
                if (isNum)
                {
                    if (Num < 0 || Num > 3500)
                        validnum = false;
                    else
                        validnum = true;
                }
                if (Str.Length == 1 & Str == ".")
                    validnum = true;
            }
            if (!validnum)
            {
                toolStripZPos.Text = String.Format("{0:0.0}",mapPane.filterzpos.Value);
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
            toolStripZPos.Text = String.Format("{0:0.0}",current_val);
        }

        private void toolStripZPosDown_Click(object sender, EventArgs e)
        {
            decimal current_val = mapPane.filterzpos.Value;
            current_val -= 5;
            if (current_val < mapPane.filterzpos.Minimum)
                current_val = mapPane.filterzpos.Minimum;
            mapPane.filterzpos.Value = current_val;
            toolStripZPos.Text = String.Format("{0:0.0}", current_val);
        }

        private void toolStripZNegDown_Click(object sender, EventArgs e)
        {
            decimal current_val = mapPane.filterzneg.Value;
            current_val -= 5;
            if (current_val < mapPane.filterzneg.Minimum)
                current_val = mapPane.filterzneg.Minimum;
            mapPane.filterzneg.Value = current_val;
            toolStripZNeg.Text = String.Format("{0:0.0}", current_val);
        }

        private void toolStripZNegUp_Click(object sender, EventArgs e)
        {
            decimal current_val = mapPane.filterzneg.Value;
            current_val += 5;
            if (current_val > mapPane.filterzneg.Maximum)
                current_val = mapPane.filterzneg.Maximum;
            mapPane.filterzneg.Value = current_val;
            toolStripZNeg.Text = String.Format("{0:0.0}", current_val);
        }

        private void toolStripResetDepthFilter_Click(object sender, EventArgs e)
        {
            mapPane.filterzneg.Value = 75;
            mapPane.filterzpos.Value = 75;
            toolStripZNeg.Text = String.Format("{0:0.0}", 75);
            toolStripZPos.Text = String.Format("{0:0.0}", 75);
        }

        private void toolStripZPos_Leave(object sender, EventArgs e)
        {
            // update Z-Pos value
            string Str = this.toolStripZPos.Text.Trim();
            bool validnum = false;
            double Num;
            if (Str.Length > 0)
            {
                bool isNum = double.TryParse(Str, out Num);
                if (isNum)
                {
                    if (Num >= 0 && Num <= 3500)
                    {
                        decimal z_pos = (decimal)Num;
                        mapPane.filterzpos.Value = z_pos;
                        validnum = true;
                    }
                }
            }
            if (!validnum)
                toolStripZPos.Text = String.Format("{0:0.0}", mapPane.filterzpos.Value);
        }

        private void toolStripZNeg_TextChanged(object sender, EventArgs e)
        {
            // validate that text is a usable number
            // allow a value of 0 to 3500
            string Str = this.toolStripZNeg.Text.Trim();

            double Num;
            bool validnum = true;
            if (Str.Length > 0)
            {
                bool isNum = double.TryParse(Str, out Num);
                validnum = false;
                if (isNum)
                {
                    if (Num < 0 || Num > 3500)
                        validnum = false;
                    else
                        validnum = true;
                }
                if (Str.Length == 1 & Str == ".")
                    validnum = true;
            }
            if (!validnum)
            {
                toolStripZNeg.Text = String.Format("{0:0.0}", mapPane.filterzneg.Value);
                MessageBox.Show("Enter a number between 0 and 3500", "Invalid Z-Neg Value Entered.");
            }
        }

        private void toolStripZNeg_Leave(object sender, EventArgs e)
        {
            // update Z-Pos value
            string Str = this.toolStripZNeg.Text.Trim();
            bool validnum = false;
            double Num;
            if (Str.Length > 0)
            {
                bool isNum = double.TryParse(Str, out Num);
                if (isNum)
                {
                    if (Num >= 0 && Num <= 3500)
                    {
                        decimal z_neg = (decimal)Num;
                        mapPane.filterzneg.Value = z_neg;
                        validnum = true;
                    }
                }
            }
            if (!validnum)
                toolStripZNeg.Text = String.Format("{0:0.0}", mapPane.filterzneg.Value);
        }

        private void mnuAddMapLabel_Click(object sender, EventArgs e)
        {
            this.addMapText(alertAddmobname);
        }

        private void toolStripResetLookup_Click(object sender, EventArgs e)
        {
            this.toolStripLookupBox.Text = "";
            this.toolStripLookupBox.Focus();
            eq.MarkLookups("0:");
        }
        private void toolStripResetLookup1_Click(object sender, EventArgs e)
        {
            this.toolStripLookupBox1.Text = "";
            this.toolStripLookupBox1.Focus();
            eq.MarkLookups("1:");
        }
        private void toolStripResetLookup2_Click(object sender, EventArgs e)
        {
            this.toolStripLookupBox2.Text = "";
            this.toolStripLookupBox2.Focus();
            eq.MarkLookups("2:");
        }
        private void toolStripResetLookup3_Click(object sender, EventArgs e)
        {
            this.toolStripLookupBox3.Text = "";
            this.toolStripLookupBox3.Focus();
            eq.MarkLookups("3:");
        }
        private void toolStripResetLookup4_Click(object sender, EventArgs e)
        {
            this.toolStripLookupBox4.Text = "";
            this.toolStripLookupBox4.Focus();
            eq.MarkLookups("4:");
        }
        private void toolStripResetLookup5_Click(object sender, EventArgs e)
        {
            this.toolStripLookupBox5.Text = "";
            this.toolStripLookupBox5.Focus();
            eq.MarkLookups("5:");
        }
        private void toolStripCheckLookup_CheckChanged(object sender, EventArgs e)
        {
            if (this.toolStripCheckLookup.Checked)
            {
                this.toolStripCheckLookup.Text = "L";
                bFilter0 = false;
            }
            else
            {
                this.toolStripCheckLookup.Text = "F";
                bFilter0 = true;
            }
            string new_text = toolStripLookupBox.Text.Replace(" ", "_");
            eq.MarkLookups("0:" + new_text, bFilter0);
        }
        private void toolStripCheckLookup1_CheckChanged(object sender, EventArgs e)
        {
            if (this.toolStripCheckLookup1.Checked)
            {
                this.toolStripCheckLookup1.Text = "L";
                bFilter1 = false;
            }
            else
            {
                this.toolStripCheckLookup1.Text = "F";
                bFilter1 = true;
            }
            string new_text = toolStripLookupBox1.Text.Replace(" ", "_");
            eq.MarkLookups("1:" + new_text, bFilter1);
        }
        private void toolStripCheckLookup2_CheckChanged(object sender, EventArgs e)
        {
            if (this.toolStripCheckLookup2.Checked)
            {
                this.toolStripCheckLookup2.Text = "L";
                bFilter2 = false;
            }
            else
            {
                this.toolStripCheckLookup2.Text = "F";
                bFilter2 = true;
            }
            string new_text = toolStripLookupBox2.Text.Replace(" ", "_");
            eq.MarkLookups("2:" + new_text, bFilter2);
        }
        private void toolStripCheckLookup3_CheckChanged(object sender, EventArgs e)
        {
            if (this.toolStripCheckLookup3.Checked)
            {
                this.toolStripCheckLookup3.Text = "L";
                bFilter3 = false;
            }
            else
            {
                this.toolStripCheckLookup3.Text = "F";
                bFilter3 = true;
            }
            string new_text = toolStripLookupBox3.Text.Replace(" ", "_");
            eq.MarkLookups("3:" + new_text, bFilter3);
        }
        private void toolStripCheckLookup4_CheckChanged(object sender, EventArgs e)
        {
            if (this.toolStripCheckLookup4.Checked)
            {
                this.toolStripCheckLookup4.Text = "L";
                bFilter4 = false;
            }
            else
            {
                this.toolStripCheckLookup4.Text = "F";
                bFilter4 = true;
            }
            string new_text = toolStripLookupBox4.Text.Replace(" ", "_");
            eq.MarkLookups("4:" + new_text, bFilter4);
        }
        private void toolStripCheckLookup5_CheckChanged(object sender, EventArgs e)
        {
            if (this.toolStripCheckLookup5.Checked)
            {
                this.toolStripCheckLookup5.Text = "L";
                bFilter5 = false;
            }
            else
            { this.toolStripCheckLookup5.Text = "F";
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
                    if (mapCon != null)
                        mapCon.Focus();
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
                    if (mapCon != null)
                        mapCon.Focus();
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
                    if (mapCon != null)
                        mapCon.Focus();
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
                    if (mapCon != null)
                        mapCon.Focus();
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
                    if (mapCon != null)
                        mapCon.Focus();
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
                    if (mapCon != null)
                        mapCon.Focus();
                }
                else
                {
                    // text is blank, enter was pressed, but leave focus here
                    eq.MarkLookups("5:");
                }

                e.Handled = true;

            }
        }

        private void toolStripScale_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                string Str = this.toolStripScale.Text.Trim();
                Str = Str.Replace("%", "");

                double Num;

                if (Str.Length > 0)
                {
                    bool isNum = double.TryParse(Str, out Num);

                    if (isNum)
                    {
                        if (Num >= (double)mapPane.scale.Minimum && Num <= (double)mapPane.scale.Maximum)
                            mapPane.scale.Value = (decimal)Num;
                    }
                }
                this.toolStripScale.Focus();

                e.Handled = true;

            }
        }

        private void SetUpdateSteps()
        {
            int update_steps = 1000 / Settings.Instance.UpdateDelay + 1;
            if (update_steps < 3)
                update_steps = 3;

            int update_ticks = 250 / Settings.Instance.UpdateDelay;
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
                if (eq.playerinfo != null && eq.playerinfo.Name.Length > 0)
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
            Settings.Instance.ShowListSearchBox = !Settings.Instance.ShowListSearchBox;

            this.mnuShowListSearchBox.Checked = Settings.Instance.ShowListSearchBox;

            if (Settings.Instance.ShowListSearchBox)
            {
                this.SpawnList.ShowSearchBox();
                this.SpawnTimerList.ShowSearchBox();
                this.GroundItemList.ShowSearchBox();
            }
            else
            {
                this.SpawnList.HideSearchBox();
                this.SpawnTimerList.HideSearchBox();
                this.GroundItemList.HideSearchBox();
            }
        }

        private void mnuSmallTargetInfo_Click(object sender, EventArgs e)
        {        
            Settings.Instance.SmallTargetInfo = !Settings.Instance.SmallTargetInfo;

            mnuSmallTargetInfo.Checked = Settings.Instance.SmallTargetInfo;
            mnuSmallTargetInfo2.Checked = Settings.Instance.SmallTargetInfo;

        }
        private void mnuSmallTargetInfo2_Click(object sender, EventArgs e)
        {
            Settings.Instance.SmallTargetInfo = !Settings.Instance.SmallTargetInfo;

            mnuSmallTargetInfo.Checked = Settings.Instance.SmallTargetInfo;
            mnuSmallTargetInfo2.Checked = Settings.Instance.SmallTargetInfo;

        }

        private void toolStripScale_DropDownClosed(object sender, EventArgs e)
        {
            string Str = this.toolStripScale.SelectedItem.ToString();
            Str = Str.Replace("%", "");
            
            double Num;

            if (Str.Length > 0)
            {
                bool isNum = double.TryParse(Str, out Num);

                if (isNum)
                {
                    if (Num >= (double)mapPane.scale.Minimum && Num <= (double)mapPane.scale.Maximum)
                    {
                        mapPane.scale.Value = (decimal)Num;
                    }
                }
            }
        }

        private void addZoneEmailAlertFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (dialogBox("Add to Zone Email Alert Filters", "Add name to Email list:", alertAddmobname))
            {

                filters.AddToAlerts(filters.emailAlert, alertAddmobname);

                filters.writeAlertFile(curZone);

                reloadAlertFiles();

            }
        }

        private void toolStripEmailAlerts_Click(object sender, EventArgs e)
        {
            if (Settings.Instance.EmailAlerts == true)
            {
                // turn off alerts
                Settings.Instance.EmailAlerts = false;
            }
            else
            {
                // trying to turn on alerts.  Check if To and From emails set.
                if (SMTPSettings.Instance.FromEmail == string.Empty || SMTPSettings.Instance.ToEmail == string.Empty || SMTPSettings.Instance.SmtpServer == string.Empty)
                {
                    MessageBox.Show("Set Email Settings on the SMTP tab in the\r\nOptions Dialog before enabling email alerts.", "Initial Email Settings", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                if (SMTPSettings.Instance.UseNetworkCredentials == true || (SMTPSettings.Instance.SmtpUsername.ToString().Length > 0 && SMTPSettings.Instance.SmtpPassword.ToString().Length > 0))
                {
                    // Check if we can resolve the host
                    IPHostEntry host;
                    try { host = Dns.GetHostEntry(SMTPSettings.Instance.SmtpServer.ToString()); }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error resolving SMTP Server Address\r\n\r\n" + SMTPSettings.Instance.SmtpServer.ToString() + "\r\n\r\n" + ex.Message, "Error Resolving SMTP Server Address", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    // Using network credentials, so no login prompt needed
                    Settings.Instance.EmailAlerts = !Settings.Instance.EmailAlerts;
                }
                else
                {
                    // we got here, so everything must be filled out
                    IPHostEntry host;
                    try { host = Dns.GetHostEntry(SMTPSettings.Instance.SmtpServer.ToString()); }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error resolving SMTP Server Address\r\n\r\n" + SMTPSettings.Instance.SmtpServer.ToString() + "\r\n\r\n" + ex.Message, "Error Resolving SMTP Server Address", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Pop up username/password login screen for email
                    frmLogin f4 = new frmLogin();
                    f4.StartPosition = FormStartPosition.CenterScreen;
                    // the validation in the ShowDialog, will turn on alerts if passes and ok pushed.
                    f4.ShowDialog();
                }
            }
            
            this.toolStripEmailAlerts.Checked = Settings.Instance.EmailAlerts;

            if (Settings.Instance.EmailAlerts)
            {
                toolStripEmailAlerts.ToolTipText = "Disable Email Alerts";
                if (filters.emailAlert.Count > 0)
                    reloadAlertFiles();
            }
            else
            {
                toolStripEmailAlerts.ToolTipText = "Enable Email Alerts";
            }
        }

        private void mnuAutoConnect_Click(object sender, EventArgs e)
        {
            Settings.Instance.AutoConnect = !Settings.Instance.AutoConnect;
            this.mnuAutoConnect.Checked = Settings.Instance.AutoConnect;
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


        private void toolStripLookupBox_Click(object sender, EventArgs e)
        {
            if (toolStripLookupBox.Text == "Mob Search")
            {
                toolStripLookupBox.Text = "";
                this.toolStripLookupBox.ForeColor = System.Drawing.SystemColors.WindowText;
            }
            
        }
        private void toolStripLookupBox1_Click(object sender, EventArgs e)
        {
            if (toolStripLookupBox1.Text == "Mob Search")
            {
                toolStripLookupBox1.Text = "";
                this.toolStripLookupBox1.ForeColor = System.Drawing.SystemColors.WindowText;
            }

        }
        private void toolStripLookupBox2_Click(object sender, EventArgs e)
        {
            if (toolStripLookupBox2.Text == "Mob Search")
            {
                toolStripLookupBox2.Text = "";
                this.toolStripLookupBox2.ForeColor = System.Drawing.SystemColors.WindowText;
            }

        }
        private void toolStripLookupBox3_Click(object sender, EventArgs e)
        {
            if (toolStripLookupBox3.Text == "Mob Search")
            {
                toolStripLookupBox3.Text = "";
                this.toolStripLookupBox3.ForeColor = System.Drawing.SystemColors.WindowText;
            }

        }
        private void toolStripLookupBox4_Click(object sender, EventArgs e)
        {
            if (toolStripLookupBox4.Text == "Mob Search")
            {
                toolStripLookupBox4.Text = "";
                this.toolStripLookupBox4.ForeColor = System.Drawing.SystemColors.WindowText;
            }

        }
        private void toolStripLookupBox5_Click(object sender, EventArgs e)
        {
            if (toolStripLookupBox5.Text == "Mob Search")
            {
                toolStripLookupBox5.Text = "";
                this.toolStripLookupBox5.ForeColor = System.Drawing.SystemColors.WindowText;
            }

        }
        private void toolStripLookupBox_Leave(object sender, EventArgs e)
        {

            if (toolStripLookupBox.Text.Length > 0)
            {
                if (toolStripLookupBox.Text == "Mob Search")
                {
                    this.toolStripLookupBox.ForeColor = System.Drawing.SystemColors.GrayText;
                    eq.MarkLookups("0:");
                }
                else
                {
                    string new_text = toolStripLookupBox.Text.Replace(" ", "_");
                    eq.MarkLookups("0:" + new_text, bFilter0);
                }
            }
            else
            {
                this.toolStripLookupBox.ForeColor = System.Drawing.SystemColors.GrayText;
                toolStripLookupBox.Text = "Mob Search";
                eq.MarkLookups("0:");
            }
        }

        private void toolStripLookupBox1_Leave(object sender, EventArgs e)
        {

            if (toolStripLookupBox1.Text.Length > 0)
            {
                if (toolStripLookupBox1.Text == "Mob Search")
                {
                    this.toolStripLookupBox1.ForeColor = System.Drawing.SystemColors.GrayText;
                    eq.MarkLookups("1:");
                }
                else
                {
                    string new_text = toolStripLookupBox1.Text.Replace(" ", "_");
                    eq.MarkLookups("1:" + new_text, bFilter1);
                }
            }
            else
            {
                this.toolStripLookupBox1.ForeColor = System.Drawing.SystemColors.GrayText;
                toolStripLookupBox1.Text = "Mob Search";
                eq.MarkLookups("1:");
            }
        }

        private void toolStripLookupBox2_Leave(object sender, EventArgs e)
        {

            if (toolStripLookupBox2.Text.Length > 0)
            {
                if (toolStripLookupBox2.Text == "Mob Search")
                {
                    this.toolStripLookupBox2.ForeColor = System.Drawing.SystemColors.GrayText;
                    eq.MarkLookups("2:");
                }
                else
                {
                    string new_text = toolStripLookupBox2.Text.Replace(" ", "_");
                    eq.MarkLookups("2:" + new_text, bFilter2);
                }
            }
            else
            {
                this.toolStripLookupBox2.ForeColor = System.Drawing.SystemColors.GrayText;
                toolStripLookupBox2.Text = "Mob Search";
                eq.MarkLookups("2:");
            }
        }

        private void toolStripLookupBox3_Leave(object sender, EventArgs e)
        {

            if (toolStripLookupBox3.Text.Length > 0)
            {
                if (toolStripLookupBox3.Text == "Mob Search")
                {
                    this.toolStripLookupBox3.ForeColor = System.Drawing.SystemColors.GrayText;
                    eq.MarkLookups("3:");
                }
                else
                {
                    string new_text = toolStripLookupBox3.Text.Replace(" ", "_");
                    eq.MarkLookups("3:" + new_text, bFilter3);
                }
            }
            else
            {
                this.toolStripLookupBox3.ForeColor = System.Drawing.SystemColors.GrayText;
                toolStripLookupBox3.Text = "Mob Search";
                eq.MarkLookups("3:");
            }
        }

        private void toolStripLookupBox4_Leave(object sender, EventArgs e)
        {

            if (toolStripLookupBox4.Text.Length > 0)
            {
                if (toolStripLookupBox4.Text == "Mob Search")
                {
                    this.toolStripLookupBox4.ForeColor = System.Drawing.SystemColors.GrayText;
                    eq.MarkLookups("4:");
                }
                else
                {
                    string new_text = toolStripLookupBox4.Text.Replace(" ", "_");
                    eq.MarkLookups("4:" + new_text, bFilter4);
                }
            }
            else
            {
                this.toolStripLookupBox4.ForeColor = System.Drawing.SystemColors.GrayText;
                toolStripLookupBox4.Text = "Mob Search";
                eq.MarkLookups("4:");
            }
        }

        private void toolStripLookupBox5_Leave(object sender, EventArgs e)
        {

            if (toolStripLookupBox5.Text.Length > 0)
            {
                if (toolStripLookupBox5.Text == "Mob Search")
                {
                    this.toolStripLookupBox5.ForeColor = System.Drawing.SystemColors.GrayText;
                    eq.MarkLookups("5:");
                }
                else
                {
                    string new_text = toolStripLookupBox5.Text.Replace(" ", "_");
                    eq.MarkLookups("5:" + new_text, bFilter5);
                }
            }
            else
            {
                this.toolStripLookupBox5.ForeColor = System.Drawing.SystemColors.GrayText;
                toolStripLookupBox5.Text = "Mob Search";
                eq.MarkLookups("5:");
            }
        }

      
        
        private void mnuFileMain_DropDownOpening(object sender, EventArgs e)
        {
            // Update the Character Selection list
            // colProcesses.Clear();

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

        private void toolStripLevel_TextUpdate(object sender, EventArgs e)
        {
            string Str = this.toolStripLevel.Text.Trim();

            long Num;
            bool validnum = true;
            if (Str.Length > 0)
            {
                bool isNum = long.TryParse(Str, out Num);

                if (isNum)
                {
                    if (Num < 1 || Num > 105)
                    {
                        validnum = false;
                    }
                }
            }


            if (!validnum)
            {
                MessageBox.Show("1. Enter a number between 1-105 or select Auto");
            }
        }

        private void toolStripLevel_Leave(object sender, EventArgs e)
        {
            string Str = this.toolStripLevel.Text.Trim();

            long Num;
            bool validnum = true;
            if (Str.Length > 0)
            {
                bool isNum = long.TryParse(Str, out Num);

                if (isNum)
                {
                    if (Num < 1 || Num > 105)
                    {
                        validnum = false;
                    }
                } else {
                    if (Str != "Auto")
                    {
                        validnum = false;
                    }
                    else
                    {
                        Settings.Instance.LevelOverride = -1;
                        this.gconLevel = -1;
                    }
                }
            }

            if (!validnum)
            {
                MessageBox.Show("2. Enter a number between 1-105 or Auto");
            } else {
                this.gConBaseName = "";
            }
        }

        private void toolStripLevel_DropDownClosed(object sender, EventArgs e)
        {
            string Str = this.toolStripLevel.SelectedItem.ToString();

            long Num;

            if (Str.Length > 0)
            {
                bool isNum = long.TryParse(Str, out Num);
                this.gConBaseName = "";
                if (isNum)
                {
                    if (Num >= 1 && Num <= 105)
                    {
                        //Do Stuff
                        Settings.Instance.LevelOverride = (int)Num;
                        this.gconLevel = (int)Num;
                    }
                }
                else
                {
                    if (Str == "Auto")
                    {
                        Settings.Instance.LevelOverride = -1;
                        this.gconLevel = (int)Num;
                    }
                }
            }
        }

        private void toolStripLevel_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                string Str = this.toolStripLevel.Text.Trim();

                long Num;

                if (Str.Length > 0)
                {
                    bool isNum = long.TryParse(Str, out Num);

                    if (isNum)
                    {
                        if (Num >= 1 && Num <= 105)
                        {
                            //do stuff
                            Settings.Instance.LevelOverride = (int)Num;
                            this.gconLevel = (int)Num;
                        }
                    }
                }
                this.toolStripScale.Focus();

                e.Handled = true;

            }
        }

    }
}



