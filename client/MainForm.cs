using myseq.Properties;
using Structures;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace myseq
{
    public partial class MainForm : Form
    {
        private readonly string Version = Application.ProductVersion;

        private string BaseTitle = "MySEQ Open";

        public ListViewPanel SpawnList = new ListViewPanel(0);
        public ListViewPanel SpawnTimerList = new ListViewPanel(1);
        public ListViewPanel GroundItemList = new ListViewPanel(2);

        public string curZone { get; set; } = "map_pane";

        public string mapnameWithLabels { get; set; } = "";

        private string currentIPAddress = "";
        private MarkLookup mark = new MarkLookup();
        public MapCon mapCon;

        public MapPane mapPane = new MapPane();
        public readonly Filters filters = new Filters();
        private readonly EQData eq;

        private readonly EQCommunications comm;
        public readonly EQMap map;

        private readonly SpawnColors spawnColors = new SpawnColors();

        public DrawOptions DrawOpts = DrawOptions.DrawNormal;

        public string alertAddmobname = "";
        public float alertX = 0.0f;
        public float alertY = 0.0f;
        public float alertZ = 0.0f;

        private bool bIsRunning;
        private bool bFilter0;
        private bool bFilter1;
        private bool bFilter2;
        private bool bFilter3;
        private bool bFilter4;
        private readonly FormMethods formMethod = new FormMethods();

        public MainForm()
        {
            // This shuts up the error messages when running under a debugger
            CheckForIllegalCrossThreadCalls = false;

            m_deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);

            eq = new EQData();
            map = new EQMap();
            comm = new EQCommunications(eq, this);

            InitializeComponent();

            LogLib.maxLogLevel = LogLevel.DefaultMaxLevel;
            LogLib.WriteLine("MySEQ Open Version: " + Version);

            LoadPrefs();

            mapCon = mapPane.mapCon;

            LogLib.WriteLine("Loaded Prefs");

            // Set Map Window Options
            mapPane.DockAreas = DockAreas.Document;
            mapPane.CloseButton = false;
            mapPane.TabText = "map_pane";

            mapCon.SetComponents(this, mapPane, eq, map);
            mark.SetComponents(eq);
            mapPane.SetComponents(this);

            // Set Spawn List Window Options
            LogLib.WriteLine("Creating SpawnList Window", LogLevel.Debug);
            SpawnList.HideOnClose = true;
            SpawnList.TabText = "Spawn List";
            SpawnList.VisibleChanged += new EventHandler(SpawnList_VisibleChanged);
            SpawnList.SetComponents(eq, filters, this);

            // Set Spawn Timer Window Options
            LogLib.WriteLine("Creating SpawnTimerList Window", LogLevel.Debug);
            SpawnTimerList.HideOnClose = true;
            SpawnTimerList.TabText = "Spawn Timer List";
            SpawnTimerList.VisibleChanged += new EventHandler(SpawnTimerList_VisibleChanged);
            SpawnTimerList.SetComponents(eq, filters, this);

            // Set Ground Item Window Options
            LogLib.WriteLine("Creating GroundItemList Window", LogLevel.Debug);
            GroundItemList.HideOnClose = true;
            GroundItemList.TabText = "Ground Items";
            GroundItemList.VisibleChanged += new EventHandler(GroundItemList_VisibleChanged);
            GroundItemList.SetComponents(eq, filters, this);

            map.SetComponents(mapCon, eq);
            eq.mobsTimers.SetComponents(map);
            formMethod.LoadPositionsFromConfigFile(this);

            if (Settings.Default.AlwaysOnTop)
            {
                TopMost = true;
                TopLevel = true;
            }

            toolStripVersion.Text = Version;
            mapCon.ReAdjust();

            timPackets.Interval = Settings.Default.UpdateDelay;

            // This is delay that stops emails and alert sounds right after zoning
            timDelayAlerts.Interval = 10000;

            // This is for processing timers, do it once per second.
            timProcessTimers.Interval = 1000;

            mapCon.SetUpdateSteps();

            Text = BaseTitle;

            if (Settings.Default.AutoConnect)
            {
                StartListening();
            }
        }

        public void StopListening()
        {
            // Stop the Timer
            timPackets.Stop();
            timDelayAlerts.Stop();
            DisablePlayAlerts();

            comm.StopListening();
            mnuConnect.Text = "&Connect";
            mnuConnect.Image = Resources.PlayHS;

            toolStripStartStop.Text = "Go";
            toolStripStartStop.ToolTipText = "Connect to Server";
            toolStripStartStop.Image = Resources.PlayHS;

            bIsRunning = false;

            toolStripServerAddress.Text = "";
        }

        private void MainForm_Closing(object sender, CancelEventArgs e)
        {
            if (Settings.Default.SaveOnExit)
            {
                SavePrefs();
            }
            StopListening();
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

        private void StartListening()
        {
            comm.ProcessClear();

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

            if (comm.ConnectToServer(currentIPAddress, Settings.Default.Port))
            {
                toolStripServerAddress.Text = currentIPAddress;
            }
            else
            {
                return;
            }

            ClearMap();

            // Start the timer
            timPackets.Start();

            mapCon.Focus();
            //sets some variables | Loads race, class, gi files.
            eq.LoadSpawnInfo();
            eq.InitLookups();

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
            this.Text = BaseTitle;

            if (Settings.Default.ShowZoneName)
            {
                Text += $" - {eq.Longname}";
            }

            if (Settings.Default.ShowCharName)
            {
                Text += $" - {eq.gamerInfo.Name}";
            }
        }

        private void LoadPrefs()
        {
            // Always want these off on starting up.

            Settings.Default.CollectMobTrails = false;
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
            SpawnList.HideSearchBox();
            SpawnTimerList.HideSearchBox();
            GroundItemList.HideSearchBox();

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

            mnuShowSpawnPoints.Checked = mnuShowSpawnPoints2.Checked = (Settings.Default.DrawOptions
                & DrawOptions.SpawnTimers) != DrawOptions.None;

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

            // SpawnList

            SpawnList.listView.BackColor = Settings.Default.ListBackColor;

            SpawnTimerList.listView.BackColor = Settings.Default.ListBackColor;

            GroundItemList.listView.BackColor = Settings.Default.ListBackColor;

            SpawnList.listView.GridLines = Settings.Default.ShowListGridLines;

            SpawnTimerList.listView.GridLines = Settings.Default.ShowListGridLines;

            GroundItemList.listView.GridLines = Settings.Default.ShowListGridLines;

            FileOps.CreateFolders();

            DrawOpts = Settings.Default.DrawOptions;

            timProcessTimers.Start();
        }

        private void SavePrefs() => Settings.Default.Save();

        private void MainForm_Move(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                Settings.Default.WindowsLocation = Location;
            }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                Settings.Default.WindowsSize = Size;
            }
            Settings.Default.WindowState = WindowState;
            //            ReAdjust();
        }

        private void TimPackets_Tick(object sender, EventArgs e)
        {
            DrawOpts = Settings.Default.DrawOptions;
            comm.Tick();
            mapCon.Tick();
            map.trails.CountMobTrails(eq);
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
            //eq.ProcessSpawnTimer(this);

            //eq.CheckMobs(SpawnList, GroundItemList);

            if (eq.mobsTimers.mobsTimer2.Count > 0)
            {
                eq.mobsTimers.UpdateList(SpawnTimerList);
            }

            if (!bIsRunning && mapCon != null)
            {
                MapConInvalidate();
            }
        }

        private void SpawnList_VisibleChanged(object sender, EventArgs e)
            => Settings.Default.ShowMobList = mnuShowSpawnList.Checked = SpawnList.Visible;

        private void SpawnTimerList_VisibleChanged(object sender, EventArgs e)
            => Settings.Default.ShowMobListTimer = mnuShowSpawnListTimer.Checked = SpawnTimerList.Visible;

        private void GroundItemList_VisibleChanged(object sender, EventArgs e)
            => Settings.Default.ShowGroundItemList = mnuShowGroundItemList.Checked = GroundItemList.Visible;

        public void ShowCharsInList(Spawninfo si, ProcessInfo PI)
        {
            var equalProcessID = (comm.CurrentProcess != null) && (comm.CurrentProcess.ProcessID == PI.ProcessID);

            // Array of menu items for easy access
            var menuItems = new[]
            {
                mnuChar1, mnuChar2, mnuChar3, mnuChar4, mnuChar5,
                mnuChar6, mnuChar7, mnuChar8, mnuChar9, mnuChar10,
                mnuChar11, mnuChar12
            };

            // Set the number of collected processes
            int count = comm.ColProcesses.Count;

            // Ensure we do not go out of bounds
            if (count > 12) count = 12;

            // Loop through menu items based on the number of collected processes
            for (int i = 0; i < count; i++)
            {
                var currentItem = menuItems[i];
                var nextItem = i + 1 < menuItems.Length ? menuItems[i + 1] : null;
                var charLabel = $"Char {i + 2}";

                ShowCharInList(si, currentItem, nextItem, charLabel, equalProcessID);
            }

            // Special case for the 12th character, which does not have a "next" item
            if (count == 12)
            {
                menuItems[11].Text = si.Name;
                menuItems[11].Visible = true;
                menuItems[11].Checked = equalProcessID;
            }
        }

        private void ShowCharInList(Spawninfo si, ToolStripMenuItem charThis, ToolStripMenuItem charNext, string charLabel, bool equalProcessID)
        {
            charThis.Text = si.Name;
            charThis.Visible = true;

            if (charNext != null)
            {
                charNext.Visible = false;
                charNext.Text = charLabel;
                charNext.Checked = false;
            }

            charThis.Checked = equalProcessID;
        }

        #region ProccessMap

        public void ProcessMap(Spawninfo si)
        {
            mapnameWithLabels = "";

            try
            {
                LogLib.WriteLine($"ProcesssMap: Short Zone Name: ({si.Name})");
                var mapname = si.Name.Trim().Split('_')[0];
                LogLib.WriteLine($"Using Short Zone Name: ({mapname})");
                toolStripShortName.Text = mapname.ToUpper();

                curZone = mapname.ToUpper();

                CheckZoneFile();

                // Turn off collecting mob trails anytime load a new map

                mnuCollectMobTrails.Checked = false;

                Settings.Default.CollectMobTrails = false;

                // Start Delay for doing spawn alerts.  This stops sounds and emails.
                timDelayAlerts.Start();
                DisablePlayAlerts();

                try
                {
                    FindMapNotZoning(si);
                }
                catch (Exception ex)

                {
                    LogLib.WriteLine("Error in ProcessMap() Load Map: ", ex);

                    map.LoadDummyMap();
                }

                eq.Longname = mapname;
                filters.LoadAlerts(mapname);
                SetTitle();
            }
            catch (Exception ex) { LogLib.WriteLine("Error in ProcessMap(): ", ex); }
        }

        private void FindMapNotZoning(Spawninfo si)
        {
            if (curZone.Length > 0 && curZone != "CLZ" && curZone != "DEFAULT")
            {
                // Try loading depth filter settings from file
                LoadDepthFilter();
            }

            var foundmap = false;
            if (curZone.Length == 0 || curZone == "CLZ" || curZone == "DEFAULT")

            {
                foundmap = ZoningOrCharLoading();
            }
            else
            {
                var fullmap = Path.Combine(Settings.Default.MapDir, si.Name.Trim());
                foundmap = NotZoningShowLayers(foundmap, fullmap);
            }
            //... Missing map
            if (!foundmap)
            {
                map.LoadDummyMap();
            }
        }

        private void LoadDepthFilter()
        {
            var configFile = FileOps.CombineCfgDir("config.ini");

            if (!File.Exists(configFile))
            {
                // No config file exists, so toggle depth filter off
                ToggleDepth();
                return;
            }

            var strIniValue = new IniFile(configFile).ReadValue("Zones", curZone, "");
            if (strIniValue.Length == 0)
            {
                // No setting for this zone, so toggle depth filter off
                ToggleDepth();
            }
            else if ((strIniValue == "0" && Settings.Default.DepthFilter) ||
                     (strIniValue == "1" && !Settings.Default.DepthFilter))
            {
                ToggleDepthFilter();
            }
        }

        private void ToggleDepth()
        {
            if (Settings.Default.DepthFilter)
            {
                ToggleDepthFilter();
            }
        }

        private void CheckZoneFile()
        {
            var ZonesFile = FileOps.CombineCfgDir("Zones.ini");

            if (File.Exists(ZonesFile))
            {
                mapPane.TabText = string.IsNullOrEmpty(curZone) ? "map_pane" : new IniFile(ZonesFile).ReadValue("Zones", curZone, curZone.ToLower());
            }
            else
            {
                mapPane.TabText = curZone.Length > 0 ? curZone.ToLower() : "map_pane";
            }
        }

        private bool NotZoningShowLayers(bool foundmap, string fullmap)
        {
            eq.Zoning = false;
            if (map.LoadMap($"{fullmap}.txt"))
            {
                foundmap = true;
            }

            if (Settings.Default.ShowLayer1 && map.LoadMap($"{fullmap}_1.txt"))
            {
                foundmap = true;
            }

            if (Settings.Default.ShowLayer2 && map.LoadMap($"{fullmap}_2.txt"))
            {
                foundmap = true;
            }

            if (Settings.Default.ShowLayer3 && map.LoadMap($"{fullmap}_3.txt"))
            {
                foundmap = true;
            }

            // use _3.txt file for map labels
            if (foundmap)
            {
                mapnameWithLabels = $"{fullmap}_3.txt";
            }

            SetTitle();
            return foundmap;
        }

        private bool ZoningOrCharLoading()
        {
            bool foundmap;
            if (Settings.Default.DepthFilter)
            {
                ToggleDepthFilter();
            }

            eq.Zoning = true;

            map.LoadDummyMap();

            Text = BaseTitle;

            foundmap = true;

            mapnameWithLabels = "";
            return foundmap;
        }

        #endregion ProccessMap

        private void SetGridInterval()
        {
            mnuGridInterval100.Checked = false;
            mnuGridInterval250.Checked = false;
            mnuGridInterval500.Checked = false;
            mnuGridInterval1000.Checked = false;

            if (Settings.Default.GridInterval <= 100)
            {
                mnuGridInterval100.Checked = true;
            }
            else if (Settings.Default.GridInterval <= 250)
            {
                mnuGridInterval250.Checked = true;
            }
            else if (Settings.Default.GridInterval <= 500)
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
                mnuMobName.Text = $"'{alertAddmobname}'";
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
                addMapTextToolStripMenuItem.Enabled = eq.Longname.Length > 0 && eq.gamerInfo?.Name.Length > 0;
                mnuShowMenuBar.Visible = !Settings.Default.ShowMenuBar;
            }
        }

        private void MnuOpenMap_Click(object sender, EventArgs e)
        {
            formMethod.MnuOpenMap(this);
            eq.Shortname = curZone;
            eq.Longname = eq.Shortname;

            eq.CalcExtents(map.Lines);
        }

        private void ClearMap()
        {
            eq.Clear();
            map.trails.Clear();
            SpawnList.listView.Items.Clear();
            SpawnTimerList.listView.Items.Clear();
            GroundItemList.listView.Items.Clear();

            SpawnList.listView.BeginUpdate();
            SpawnTimerList.listView.BeginUpdate();
            GroundItemList.listView.BeginUpdate();

            if (eq.mobsTimers.mobsTimer2.Count > 0)
            {
                foreach (Spawntimer st in eq.mobsTimers.mobsTimer2.Values)
                {
                    st.ItmSpawnTimerList = null;
                }
            }

            SpawnList.listView.EndUpdate();
            SpawnTimerList.listView.EndUpdate();
            GroundItemList.listView.EndUpdate();

            eq.mobsTimers.ResetTimers();
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
            OptionsForm f3 = new OptionsForm();
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
            mapCon.SetUpdateSteps();
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

            SetTitle();

            SavePrefs();

            MapConInvalidate();
        }

        private void ServerSelection()
        {
            var ipAddresses = new[]
            {
        (mnuIPAddress1, Settings.Default.IPAddress1),
        (mnuIPAddress2, Settings.Default.IPAddress2),
        (mnuIPAddress3, Settings.Default.IPAddress3),
        (mnuIPAddress4, Settings.Default.IPAddress4),
        (mnuIPAddress5, Settings.Default.IPAddress5)
    };

            foreach (var (menuItem, ipAddress) in ipAddresses)
            {
                menuItem.Text = ipAddress;
                menuItem.Enabled = menuItem.Visible = !string.IsNullOrEmpty(ipAddress);
            }
        }

        private void MnuRefreshSpawnList_Click(object sender, EventArgs e)
        {
            ResetTimers();
        }

        private void MnuDepthFilter_Click(object sender, EventArgs e)
        {
            ToggleDepthFilter();

            if (string.IsNullOrEmpty(curZone) || string.Equals(curZone, "CLZ", StringComparison.OrdinalIgnoreCase) || string.Equals(curZone, "DEFAULT", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            try
            {
                // Save depth filter settings to file
                var depthValue = Settings.Default.DepthFilter ? "1" : "0";
                var conIni = new IniFile("DepthConfig.ini");
                conIni.WriteValue("Zones", curZone, depthValue);
            }
            catch (Exception ex)
            {
                LogLib.WriteLine("Error writing depth filter setting to ini file: ", ex);
            }
        }

        private void ToggleDepthFilter()
        {
            var isChecked = !Settings.Default.DepthFilter;
            Settings.Default.DepthFilter = isChecked;
            mnuDepthFilter.Checked = mnuDepthFilter2.Checked = isChecked;

            toolStripDepthFilterButton.Checked = isChecked;
            toolStripZNegUp.Enabled = isChecked;
            toolStripZNeg.Enabled = isChecked;
            toolStripZNegDown.Enabled = isChecked;
            toolStripZPosDown.Enabled = isChecked;
            toolStripZOffsetLabel.Enabled = isChecked;
            toolStripZPosUp.Enabled = isChecked;
            toolStripZPos.Enabled = isChecked;
            toolStripZPosLabel.Enabled = isChecked;
            toolStripResetDepthFilter.Enabled = isChecked;

            toolStripDepthFilterButton.Image = isChecked ? Resources.ExpandSpaceHS : Resources.ShrinkSpaceHS;
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

            MapConInvalidate();
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

            MapConInvalidate();
        }

        private void MnuGridColor_Click(object sender, EventArgs e)

        {
            if (colorPicker.ShowDialog() != DialogResult.Cancel && Settings.Default.GridColor != colorPicker.Color)

            {
                Settings.Default.GridColor = colorPicker.Color;

                MapConInvalidate();
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

        private void SetFollowOption(FollowOption newFollowOption)
        {
            Settings.Default.FollowOption = newFollowOption;

            UpdateFollowUI(newFollowOption);
        }

        private void UpdateFollowUI(FollowOption followOption)
        {
            // Set the toolstrip text based on the follow option
            if (followOption == FollowOption.None)
            {
                toolStripCoPStatus.Text = "NoF";
            }
            else if (followOption == FollowOption.Player)
            {
                toolStripCoPStatus.Text = "CoP";
            }
            else if (followOption == FollowOption.Target)
            {
                toolStripCoPStatus.Text = "CoT";
            }

            // Reset map pane offsets
            mapPane.offsetx.Value = 0;
            mapPane.offsety.Value = 0;

            // Update menu images based on the selected option
            UpdateMenuImages(
                followOption == FollowOption.Target ? Resources.BlackX : null,
                followOption == FollowOption.Player ? Resources.BlackX : null,
                followOption == FollowOption.None ? Resources.BlackX : null
            );
        }

        private void UpdateMenuImages(Image targetImage, Image playerImage, Image noneImage)
        {
            // Follow Target
            mnuFollowTarget.Image = targetImage;
            mnuFollowTarget2.Image = targetImage;

            // Follow Player
            mnuFollowPlayer.Image = playerImage;
            mnuFollowPlayer2.Image = playerImage;

            // Follow None
            mnuFollowNone.Image = noneImage;
            mnuFollowNone2.Image = noneImage;
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
                ReloadAlertFiles();
            }
        }

        private void MnuAddEditAlerts_Click(object sender, EventArgs e) => filters.EditAlertFile(curZone);

        private void MnuSpawnListFont_Click(object sender, EventArgs e)

        {
            fontDialog1.Font = SpawnList.listView.Font;

            fontDialog1.ShowApply = true;

            if (fontDialog1.ShowDialog() != DialogResult.Cancel)

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

            MapConInvalidate();
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

            MapConInvalidate();
        }

        private void MnuAbout_Click(object sender, EventArgs e)
        {
            new AboutDialog().ShowDialog();
        }

        private void MnuShowTargetInfo_Click(object sender, EventArgs e)
        {
            Settings.Default.ShowTargetInfo = !Settings.Default.ShowTargetInfo;

            mnuShowTargetInfo.Checked = Settings.Default.ShowTargetInfo;
            mnuShowTargetInfo2.Checked = Settings.Default.ShowTargetInfo;

            MapConInvalidate();
        }

        private void MnuListColor_Click(object sender, EventArgs e)
        {
            if (colorPicker.ShowDialog() != DialogResult.Cancel)
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
            }
            else
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
            fontDialog1.Font = Settings.Default.TargetInfoFont;
            fontDialog1.ShowApply = true;
            if (fontDialog1.ShowDialog() != DialogResult.Cancel)
            {
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
            Settings.Default.CurrentIPAddress = isCheck;
            if (isCheck == 1)
            {
                mnuIPAddress1.Checked = true; currentIPAddress = Settings.Default.IPAddress1;
            }
            else if (isCheck == 2)
            {
                mnuIPAddress2.Checked = true; currentIPAddress = Settings.Default.IPAddress2;
            }
            else if (isCheck == 3)
            {
                mnuIPAddress3.Checked = true; currentIPAddress = Settings.Default.IPAddress3;
            }
            else if (isCheck == 4)
            {
                mnuIPAddress4.Checked = true; currentIPAddress = Settings.Default.IPAddress4;
            }
            else if (isCheck == 5)
            {
                mnuIPAddress5.Checked = true; currentIPAddress = Settings.Default.IPAddress5;
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
            comm.ProcessClear();
            VisChar();
        }

        private void MnuChar1_Click(object sender, EventArgs e)
        {
            ClickSwitchChar(mnuChar1, 1);
        }

        private void MnuChar2_Click(object sender, EventArgs e)
        {
            ClickSwitchChar(mnuChar2, 2);
        }

        private void MnuChar3_Click(object sender, EventArgs e)
        {
            ClickSwitchChar(mnuChar3, 3);
        }

        private void MnuChar4_Click(object sender, EventArgs e)
        {
            if (!mnuChar4.Checked && comm.CanSwitchChars())
            {
                ClickSwitchChar(mnuChar4, 4);
            }
        }

        private void MnuChar5_Click(object sender, EventArgs e)
        {
            if (!mnuChar5.Checked && comm.CanSwitchChars())
            {
                ClickSwitchChar(mnuChar5, 5);
            }
        }

        private void MnuChar6_Click(object sender, EventArgs e)
        {
            if (!mnuChar6.Checked && comm.CanSwitchChars())
            {
                ClickSwitchChar(mnuChar6, 6);
            }
        }

        private void MnuChar7_Click(object sender, EventArgs e)
        {
            ClickSwitchChar(mnuChar7, 7);
        }

        private void MnuChar8_Click(object sender, EventArgs e)
        {
            ClickSwitchChar(mnuChar8, 8);
        }

        private void MnuChar9_Click(object sender, EventArgs e)
        {
            ClickSwitchChar(mnuChar9, 9);
        }

        private void MnuChar10_Click(object sender, EventArgs e)
        {
            ClickSwitchChar(mnuChar10, 10);
        }

        private void MnuChar11_Click(object sender, EventArgs e)
        {
            ClickSwitchChar(mnuChar11, 11);
        }

        private void MnuChar12_Click(object sender, EventArgs e)
        {
            ClickSwitchChar(mnuChar12, 12);
        }

        private void ClickSwitchChar(ToolStripMenuItem player, int index)
        {
            if (!player.Checked && comm.CanSwitchChars())
            {
                comm.SwitchCharacter(index);
            }
        }

        private void MnuKeepCentered_Click(object sender, EventArgs e)
        {
            Settings.Default.KeepCentered = !Settings.Default.KeepCentered;

            mnuKeepCentered.Checked = mnuKeepCentered2.Checked = Settings.Default.KeepCentered;
        }

        //public void ReAdjust() => mapCon?.ReAdjust();

        public void ReloadAlertFiles()
        {
            filters.ClearLists();

            filters.LoadAlerts(curZone);

            timDelayAlerts.Start();

            ResetTimers();
        }

        private void ResetTimers()
        {
            DisablePlayAlerts();

            eq.mobsTimers.ResetTimers();

            ClearMap();

            eq.mobsTimers.LoadTimers();
        }

        private void ResetMapPens()
        {
            eq.CalculateMapLinePens(map.Lines, map.Texts);
            MapConInvalidate();
        }

        private void MnuMapLabelsFont_Click(object sender, EventArgs e)
        {
            fontDialog1.Font = mapCon.GetdrawFont();

            fontDialog1.ShowApply = true;

            if (fontDialog1.ShowDialog() != DialogResult.Cancel)

            {
                mapCon.SetDrawFont(fontDialog1.Font);
            }
        }

        private void MnuClearSavedTimers_Click(object sender, EventArgs e)
        {
            // Clear Saved Spawn Timers

            eq.mobsTimers.ClearSavedTimers();

            SpawnTimerList.listView.BeginUpdate();

            SpawnTimerList.listView.Items.Clear();

            SpawnTimerList.listView.EndUpdate();

            eq.DefaultSpawnLoc();

            ClearMap();
        }

        private void MnuGridLabelColor_Click(object sender, EventArgs e)
        {
            if (colorPicker.ShowDialog() != DialogResult.Cancel)
            {
                Settings.Default.GridLabelColor = colorPicker.Color;

                MapConInvalidate();
            }
        }

        public void AddMapText(string textToAdd)
        {
            var new_text = textToAdd.Replace("#", "");
            FrmAddMapText mapBox = new FrmAddMapText
            {
                txtAdd = new_text.Length > 0 ? new_text : "Enter Text Label",
                StartPosition = FormStartPosition.CenterParent,
                mapName = "Add to Map: "
            };

            if (mapnameWithLabels.Length > 4 && mapnameWithLabels.EndsWith(".txt"))
            {
                mapBox.mapName += mapnameWithLabels.GetLastSlash();
            }
            else
            {
                // we dont have a good map name
                return;
            }

            if (mapBox.ShowDialog() == DialogResult.OK)
            {
                // we have a valid addition of text
                new_text = mapBox.txtAdd.TrimEnd('_', ' ');

                // add it to map now
                if (new_text.Length > 0)
                {
                    mapBox.SOEMapTextAdd(new_text, this);
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
                SpawnList.Mobname = dlgBox.dlgTextBox;
                SpawnTimerList.Mobname = dlgBox.dlgTextBox;
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

            MapConInvalidate();
        }

        private void MnuMapReset_Click(object sender, EventArgs e) => mapPane.MapReset();

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
            spawnColors.FillConColors(eq.gamerInfo);
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
            spawnColors.FillConColors(eq.gamerInfo);
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
            spawnColors.FillConColors(eq.gamerInfo);
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
                AddToFilter(Filters.Hunt, alertAddmobname);
            }
        }

        private void MnuAddCautionFilter_Click(object sender, EventArgs e)
        {
            if (DialogBox("Add to Zone Caution Filters", "Add name to Caution list:", alertAddmobname))
            {
                AddToFilter(Filters.Caution, alertAddmobname);
            }
        }

        private void MnuAddDangerFilter_Click(object sender, EventArgs e)
        {
            if (DialogBox("Add to Zone Danger Alert Filters", "Add name to Danger list:", alertAddmobname))
            {
                AddToFilter(Filters.Danger, alertAddmobname);
            }
        }

        private void MnuAddAlertFilter_Click(object sender, EventArgs e)
        {
            if (DialogBox("Add to Zone Rare Alert Filters", "Add name to Rare list:", alertAddmobname))
            {
                AddToFilter(Filters.Alert, alertAddmobname);
            }
        }

        private void AddToFilter(List<string> filter, string mob)
        {
            filters.AddToAlerts(filter, mob);

            filters.WriteAlertFile(curZone);

            ReloadAlertFiles();
        }

        #endregion filters

        private void MnuSearchAllakhazam_Click(object sender, EventArgs e)
        {
            alertAddmobname.StartSearch();
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
            var Str = toolStripZPos.Text.Trim();

            if (!Str.ValidateZNum())
            {
                toolStripZPos.Text = $"{mapPane.filterzpos.Value}";
                MessageBox.Show("Enter a number between 0 and 3500", "Invalid Z-Pos Value Entered.");
            }
        }

        private void ToolStripZPosUp_Click(object sender, EventArgs e)
        {
            var current_val = mapPane.filterzpos.Value;
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
            var current_val = mapPane.filterzpos.Value;
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
            var current_val = mapPane.filterzneg.Value;
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
            var current_val = mapPane.filterzneg.Value;
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
            var Str = toolStripZPos.Text.Trim();
            var validnum = false;
            if (Str.Length > 0)
            {
                var isNum = decimal.TryParse(Str, out var Num);
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
            var Str = toolStripZNeg.Text.Trim();

            if (!Str.ValidateZNum())
            {
                toolStripZNeg.Text = $"{mapPane.filterzneg.Value}";
                MessageBox.Show("Enter a number between 0 and 3500", "Invalid Z-Neg Value Entered.");
            }
        }

        private void ToolStripZNeg_Leave(object sender, EventArgs e)
        {
            // update Z-Pos value
            var Str = toolStripZNeg.Text.Trim();
            var validnum = false;
            if (Str.Length > 0)
            {
                var isNum = decimal.TryParse(Str, out var Num);
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

        #endregion depth filter

        private void MnuAddMapLabel_Click(object sender, EventArgs e) => AddMapText(alertAddmobname);

        #region zoom

        private void ToolStripZoomIn_Click(object sender, EventArgs e) => mapPane.ZoomIn();

        private void ToolStripZoomOut_Click(object sender, EventArgs e) => mapPane.ZoomOut();

        private void ToolStripScale_TextUpdate(object sender, EventArgs e) => CheckValidNum(toolStripScale.Text.Trim());

        private void ToolStripScale_Leave(object sender, EventArgs e) => CheckValidNum(toolStripScale.Text.Trim());

        private void CheckValidNum(string Str)
        {
            var validnum = false;
            if (!string.IsNullOrEmpty(Str))
            {
                Str = Str.Replace("%", "");
                var isNum = decimal.TryParse(Str, out var Num);
                if (Num < MapPane.scale.Minimum)
                { Num = MapPane.scale.Minimum; }

                if (isNum && Num >= MapPane.scale.Minimum && Num <= MapPane.scale.Maximum)
                {
                    validnum = mapPane.MapPaneScale(Num);
                }
            }

            if (!validnum)
            {
                toolStripScale.Text = $"{MapPane.scale.Value / 100:0%}";
                MessageBox.Show($"Enter a number between {MapPane.scale.Minimum} and {MapPane.scale.Maximum}", "Invalid Value Entered.");
            }
        }

        private void ToolStripScale_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                CheckValidNum(toolStripScale.Text.Trim());
                e.Handled = true;
            }
        }

        private void ToolStripScale_DropDownClosed(object sender, EventArgs e) => CheckValidNum(toolStripScale.SelectedItem.ToString());

        #endregion zoom

        private void AddMapTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // add map text, to where the player is currently located
            if (eq.Longname.Length > 0 && eq.gamerInfo?.Name.Length > 0)
            {
                alertX = eq.gamerInfo.X;
                alertY = eq.gamerInfo.Y;
                alertZ = eq.gamerInfo.Z;
                AddMapText("");
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
            BoxReset(toolStripLookupBox, "0", bFilter0);
        }

        private void ToolStripResetLookup1_Click(object sender, EventArgs e)
        {
            BoxReset(toolStripLookupBox1, "1", bFilter1);
        }

        private void ToolStripResetLookup2_Click(object sender, EventArgs e)
        {
            BoxReset(toolStripLookupBox2, "2", bFilter2);
        }

        private void ToolStripResetLookup3_Click(object sender, EventArgs e)
        {
            BoxReset(toolStripLookupBox3, "3", bFilter3);
        }

        private void ToolStripResetLookup4_Click(object sender, EventArgs e)
        {
            BoxReset(toolStripLookupBox4, "4", bFilter4);
        }

        private void BoxReset(ToolStripTextBox box, string rank, bool filter)
        {
            box.Text = "";
            box.Focus();
            mark.MarkLookups($"{rank}:", ref filter);
        }

        private void ToolStripCheckLookup_CheckChanged(object sender, EventArgs e)
        {
            BoxCheckChanged(toolStripCheckLookup, toolStripLookupBox, "1", ref bFilter0);
        }

        private void ToolStripCheckLookup1_CheckChanged(object sender, EventArgs e)
        {
            BoxCheckChanged(toolStripCheckLookup1, toolStripLookupBox1, "2", ref bFilter1);
        }

        private void ToolStripCheckLookup2_CheckChanged(object sender, EventArgs e)
        {
            BoxCheckChanged(toolStripCheckLookup2, toolStripLookupBox2, "3", ref bFilter2);
        }

        private void ToolStripCheckLookup3_CheckChanged(object sender, EventArgs e)
        {
            BoxCheckChanged(toolStripCheckLookup3, toolStripLookupBox3, "4", ref bFilter3);
        }

        private void ToolStripCheckLookup4_CheckChanged(object sender, EventArgs e)
        {
            BoxCheckChanged(toolStripCheckLookup4, toolStripLookupBox4, "5", ref bFilter4);
        }

        private void ToolStripTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            BoxKeyPress(e, toolStripLookupBox, "1", ref bFilter0);
        }

        private void ToolStripTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            BoxKeyPress(e, toolStripLookupBox1, "2", ref bFilter1);
        }

        private void ToolStripTextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            BoxKeyPress(e, toolStripLookupBox2, "3", ref bFilter2);
        }

        private void ToolStripTextBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            BoxKeyPress(e, toolStripLookupBox3, "4", ref bFilter3);
        }

        private void ToolStripTextBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            BoxKeyPress(e, toolStripLookupBox4, "5", ref bFilter4);
        }

        private void BoxCheckChanged(ToolStripButton button, ToolStripTextBox box, string rank, ref bool filter)
        {
            if (button.Checked)
            {
                button.Text = "L";
                filter = false;
            }
            else
            {
                button.Text = "F";
                filter = true;
            }
            NewTextMarkup(box, rank, ref filter);
        }

        private void BoxKeyPress(KeyPressEventArgs e, ToolStripTextBox box, string rank, ref bool filter)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (box.Text.Length > 0)
                {
                    NewTextMarkup(box, rank, ref filter);
                    mapCon?.Focus();
                }
                else
                {
                    // text is blank, enter was pressed, but leave focus here
                    mark.MarkLookups($"{rank}:", ref filter);
                }

                e.Handled = true;
            }
        }

        private void LeaveBox(ToolStripTextBox box, string rank, ref bool filter)
        {
            if (box.Text.Length > 0 && box.Text != "Mob Search")
            {
                NewTextMarkup(box, rank, ref filter);
            }
            else
            {
                box.ForeColor = SystemColors.GrayText;
                box.Text = "Mob Search";
            }
        }

        private void NewTextMarkup(ToolStripTextBox box, string rank, ref bool filter)
        {
            var new_text = box.Text.Replace(" ", "_");
            mark.MarkLookups($"{rank}:{new_text}", ref filter);
        }

        private void ToolStripLookupBox_Click(object sender, EventArgs e)
        {
            BoxClick(toolStripLookupBox);
        }

        private void BoxClick(ToolStripTextBox box)
        {
            if (box.Text == "Mob Search")
            {
                box.Text = "";
                box.ForeColor = SystemColors.WindowText;
            }
        }

        private void ToolStripLookupBox1_Click(object sender, EventArgs e)
        {
            BoxClick(toolStripLookupBox1);
        }

        private void ToolStripLookupBox2_Click(object sender, EventArgs e)
        {
            BoxClick(toolStripLookupBox2);
        }

        private void ToolStripLookupBox3_Click(object sender, EventArgs e)
        {
            BoxClick(toolStripLookupBox3);
        }

        private void ToolStripLookupBox4_Click(object sender, EventArgs e)
        {
            BoxClick(toolStripLookupBox4);
        }

        private void ToolStripLookupBox_Leave(object sender, EventArgs e)
        {
            LeaveBox(toolStripLookupBox, "0", ref bFilter0);
        }

        private void ToolStripLookupBox1_Leave(object sender, EventArgs e)
        {
            LeaveBox(toolStripLookupBox1, "1", ref bFilter1);
        }

        private void ToolStripLookupBox2_Leave(object sender, EventArgs e)
        {
            LeaveBox(toolStripLookupBox2, "2", ref bFilter2);
        }

        private void ToolStripLookupBox3_Leave(object sender, EventArgs e)
        {
            LeaveBox(toolStripLookupBox3, "3", ref bFilter3);
        }

        private void ToolStripLookupBox4_Leave(object sender, EventArgs e)
        {
            LeaveBox(toolStripLookupBox4, "4", ref bFilter4);
        }

        #endregion lookupbox

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

        private void ToolStripLevel_TextUpdate(object sender, EventArgs e) => formMethod.ToolStripLevelCheck(toolStripLevel.Text.Trim(), this);

        private void ToolStripLevel_Leave(object sender, EventArgs e) => formMethod.ToolStripLevelCheck(toolStripLevel.Text.Trim(), this);

        private void ToolStripLevel_DropDownClosed(object sender, EventArgs e) => formMethod.ToolStripLevelCheck(toolStripLevel.SelectedItem.ToString(), this);

        private void ToolStripLevel_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                formMethod.ToolStripLevelCheck(toolStripLevel.Text.Trim(), this);
                e.Handled = true;
            }
        }

        private void EnablePlayAlerts() => Settings.Default.playAlerts = true;

        private void DisablePlayAlerts() => Settings.Default.playAlerts = false;

        private void HelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (HelpForm help = new HelpForm())
            {
                help.StartPosition = FormStartPosition.CenterParent;
                help.ShowDialog();
            }
        }

        public void FocusMouse()
        {
            var dockstate = SpawnList.DockState == DockState.DockLeftAutoHide ||
                            SpawnList.DockState == DockState.DockRightAutoHide ||
                            SpawnList.DockState == DockState.DockTopAutoHide ||
                            SpawnList.DockState == DockState.DockBottomAutoHide;
            // focus for docking panel changes, to autohide panels that may be visible
            if (SpawnList.ContainsFocus && dockstate)
            {
                mapCon.Focus();
            }

            if (SpawnTimerList.ContainsFocus && dockstate)
            {
                mapCon.Focus();
            }

            if (GroundItemList.ContainsFocus && dockstate)
            {
                mapCon.Focus();
            }
        }

        public void MapConInvalidate()
        {
            mapCon?.Invalidate();
        }
    }
}