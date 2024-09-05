﻿using myseq.Properties;
using Structures;
using System;
using System.IO;
using System.Media;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace myseq
{
    internal class FormMethods
    {
        private readonly FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
        private readonly OpenFileDialog openFileDialog = new OpenFileDialog();

        public string FolderBrowser(string desc, string sPath)
        {
            folderBrowser.Description = desc;
            folderBrowser.SelectedPath = sPath;
            return folderBrowser.ShowDialog() == DialogResult.OK ? folderBrowser.SelectedPath : null;
        }

        public static void SwitchOnSoundSettings()
        {
            switch (Settings.Default.AlertSound)
            {
                case "Asterisk":
                    SystemSounds.Asterisk.Play();
                    break;
                case "Beep":
                    SystemSounds.Beep.Play();
                    break;
                case "Exclamation":
                    SystemSounds.Exclamation.Play();
                    break;
                case "Hand":
                    SystemSounds.Hand.Play();
                    break;
                case "Question":
                    SystemSounds.Question.Play();
                    break;
            }
        }

        public void ToolStripLevelCheck(string input, MainForm f1)
        {
            if (string.IsNullOrEmpty(input))
                return;

            if (input.Equals("Auto", StringComparison.OrdinalIgnoreCase))
            {
                // Set to Auto
                Settings.Default.LevelOverride = -1;
                f1.toolStripLevel.Text = "Auto";
            }
            else if (int.TryParse(input, out int level) && level >= 1 && level <= 125)
            {
                // Set to a valid level
                Settings.Default.LevelOverride = level;
                f1.toolStripLevel.Text = level.ToString();
            }
            else
            {
                // Show invalid input message
                MessageBox.Show("Enter a number between 1-125 or 'Auto'.");
            }
        }

        public void LoadPositionsFromConfigFile(MainForm f1)
        {
            LogLib.WriteLine("Loading Position.Xml", LogLevel.Debug);
            var configFile = FileOps.CombineCfgDir("positions.xml");

            if (File.Exists(configFile))
            {
                try
                {
                    f1.dockPanel.LoadFromXml(configFile, f1.m_deserializeDockContent);
                }
                catch (Exception ex)
                {
                    LogLib.WriteLine("Error loading config from positions.xml: ", ex);
                    // Re-Set up initial windows - might have bad or incompatible positions file
                    defaultstates();
                }
            }
            else
            {
                // Set up initial windows, when no previous window layout exists
                defaultstates();
            }

            void defaultstates()
            {
                f1.mapPane.Show(f1.dockPanel, DockState.Document);

                f1.SpawnList.Show(f1.dockPanel, DockState.DockLeft);

                f1.SpawnTimerList.Show(f1.dockPanel, DockState.DockTop);

                f1.GroundItemList.Show(f1.dockPanel, DockState.DockBottom);

                f1.SpawnTimerList.DockState = DockState.DockLeft;

                f1.GroundItemList.DockState = DockState.DockLeft;
            }
        }

        internal void CreateSpawnlistView(MainForm f1)
        {
            // Add Columns to Spawnlist window
            f1.SpawnList.ColumnsAdd("Name", Settings.Default.c1w, HorizontalAlignment.Left);
            f1.SpawnList.ColumnsAdd("Level", Settings.Default.c2w, HorizontalAlignment.Left);
            f1.SpawnList.ColumnsAdd("Class", Settings.Default.c3w, HorizontalAlignment.Left);
            f1.SpawnList.ColumnsAdd("Primary", Settings.Default.c3w, HorizontalAlignment.Left);
            f1.SpawnList.ColumnsAdd("Offhand", Settings.Default.c3w, HorizontalAlignment.Left);
            f1.SpawnList.ColumnsAdd("Race", Settings.Default.c4w, HorizontalAlignment.Left);
            f1.SpawnList.ColumnsAdd("Owner", Settings.Default.c4w, HorizontalAlignment.Left);
            f1.SpawnList.ColumnsAdd("Last Name", Settings.Default.c5w, HorizontalAlignment.Left);
            f1.SpawnList.ColumnsAdd("Type", Settings.Default.c6w, HorizontalAlignment.Left);
            f1.SpawnList.ColumnsAdd("Invis", Settings.Default.c7w, HorizontalAlignment.Left);
            f1.SpawnList.ColumnsAdd("Run Speed", Settings.Default.c8w, HorizontalAlignment.Left);
            f1.SpawnList.ColumnsAdd("SpawnID", Settings.Default.c9w, HorizontalAlignment.Left);
            f1.SpawnList.ColumnsAdd("Spawn Time", Settings.Default.c10w, HorizontalAlignment.Left);
            f1.SpawnList.ColumnsAdd("X", Settings.Default.c11w, HorizontalAlignment.Left);
            f1.SpawnList.ColumnsAdd("Y", Settings.Default.c12w, HorizontalAlignment.Left);
            f1.SpawnList.ColumnsAdd("Z", Settings.Default.c13w, HorizontalAlignment.Left);
            f1.SpawnList.ColumnsAdd("Distance", Settings.Default.c14w, HorizontalAlignment.Left);
            //            SpawnList.ColumnsAdd("Guild", Settings.Default.c14w, HorizontalAlignment.Left); //17

            // Add the Columns to the Spawn Timer Window
            f1.SpawnTimerList.ColumnsAdd("Spawn Name", Settings.Default.c1w, HorizontalAlignment.Left);
            f1.SpawnTimerList.ColumnsAdd("Remain", Settings.Default.c10w, HorizontalAlignment.Left);
            f1.SpawnTimerList.ColumnsAdd("Interval", Settings.Default.c10w, HorizontalAlignment.Left);
            f1.SpawnTimerList.ColumnsAdd("Zone", Settings.Default.c10w, HorizontalAlignment.Left);
            f1.SpawnTimerList.ColumnsAdd("X", Settings.Default.c12w, HorizontalAlignment.Left);
            f1.SpawnTimerList.ColumnsAdd("Y", Settings.Default.c11w, HorizontalAlignment.Left);
            f1.SpawnTimerList.ColumnsAdd("Z", Settings.Default.c13w, HorizontalAlignment.Left);
            f1.SpawnTimerList.ColumnsAdd("Count", Settings.Default.c9w, HorizontalAlignment.Left);
            f1.SpawnTimerList.ColumnsAdd("Spawn Time", Settings.Default.c10w, HorizontalAlignment.Left);
            f1.SpawnTimerList.ColumnsAdd("Kill Time", Settings.Default.c10w, HorizontalAlignment.Left);
            f1.SpawnTimerList.ColumnsAdd("Next Spawn", Settings.Default.c10w, HorizontalAlignment.Left);

            // Add Columns to Ground Items window
            f1.GroundItemList.ColumnsAdd("Description", Settings.Default.c1w, HorizontalAlignment.Left);
            f1.GroundItemList.ColumnsAdd("Name", Settings.Default.c1w, HorizontalAlignment.Left);
            f1.GroundItemList.ColumnsAdd("Spawn Time", Settings.Default.c10w, HorizontalAlignment.Left);
            f1.GroundItemList.ColumnsAdd("X", Settings.Default.c12w, HorizontalAlignment.Left);
            f1.GroundItemList.ColumnsAdd("Y", Settings.Default.c11w, HorizontalAlignment.Left);
            f1.GroundItemList.ColumnsAdd("Z", Settings.Default.c13w, HorizontalAlignment.Left);
        }

        internal void MnuOpenMap(MainForm f1)
        {
            openFileDialog.InitialDirectory = Settings.Default.MapDir;

            openFileDialog.Filter = "Map Files (*.txt)|*.txt|All Files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                f1.mapnameWithLabels = "";

                var filename = openFileDialog.FileName;

                f1.map.LoadMap(filename);

                filename = filename.GetLastSlash();

                filename = filename.Substring(0, filename.Length - 4);

                if (filename.EndsWith("_1"))
                {
                    filename = filename.Substring(0, filename.Length - 2);
                }

                f1.toolStripShortName.Text = filename.ToUpper();

                f1.mapPane.TabText = filename.ToLower();

                f1.curZone = filename.ToUpper();
            }
        }
    }
}