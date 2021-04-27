using System;
using System.Drawing.Drawing2D;
using System.IO;
using System.Media;
using System.Windows.Forms;
using myseq.Properties;
using Structures;
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

        public static void SetFgDrawOptions(DrawOptions DrawOpts, OptionsForm options)
        {
            options.chkMap.Checked = (DrawOpts & DrawOptions.DrawMap) != DrawOptions.None;

            options.chkAddjust.Checked = (DrawOpts & DrawOptions.Readjust) != DrawOptions.None;

            options.chkPlayer.Checked = (DrawOpts & DrawOptions.Player) != DrawOptions.None;

            options.chkLineToPoint.Checked = (DrawOpts & DrawOptions.SpotLine) != DrawOptions.None;

            options.chkSpawns.Checked = (DrawOpts & DrawOptions.Spawns) != DrawOptions.None;

            options.chkTrails.Checked = (DrawOpts & DrawOptions.SpawnTrails) != DrawOptions.None;

            options.chkGround.Checked = (DrawOpts & DrawOptions.GroundItems) != DrawOptions.None;

            options.chkTimers.Checked = (DrawOpts & DrawOptions.SpawnTimers) != DrawOptions.None;

            options.chkDirection.Checked = (DrawOpts & DrawOptions.DirectionLines) != DrawOptions.None;

            options.chkHighlight.Checked = (DrawOpts & DrawOptions.SpawnRings) != DrawOptions.None;

            options.chkGrid.Checked = (DrawOpts & DrawOptions.GridLines) != DrawOptions.None;

            options.chkText.Checked = (DrawOpts & DrawOptions.ZoneText) != DrawOptions.None;
        }

        public static void SetOptions(OptionsForm options)
        {
            StyleEnums(options);

            options.cmbHatch.SelectedText = Settings.Default.HatchIndex;

            options.cmbAlertSound.SelectedText = Settings.Default.AlertSound;

            options.txtIPAddress1.Text = Settings.Default.IPAddress1;

            options.txtIPAddress2.Text = Settings.Default.IPAddress2;

            options.txtIPAddress3.Text = Settings.Default.IPAddress3;

            options.txtIPAddress4.Text = Settings.Default.IPAddress4;

            options.txtIPAddress5.Text = Settings.Default.IPAddress5;

            options.txtPortNo.Text = Settings.Default.Port.ToString();

            options.spnOverrideLevel.Value = Settings.Default.LevelOverride;

            options.spnUpdateDelay.Value = Settings.Default.UpdateDelay;

            options.chkSaveOnExit.Checked = Settings.Default.SaveOnExit;

            options.chkPrefixAlerts.Checked = Settings.Default.PrefixStars;

            options.chkAffixAlerts.Checked = Settings.Default.AffixStars;       // affix

            options.chkCorpsesAlerts.Checked = Settings.Default.CorpseAlerts;

            options.txtHuntPrefix.Text = Settings.Default.HuntPrefix;

            options.chkHuntMatchFull.Checked = Settings.Default.MatchFullTextH;  //hunt

            options.optHuntNone.Checked = Settings.Default.NoneOnHunt;

            options.optHuntBeep.Checked = Settings.Default.BeepOnHunt;

            options.optHuntSpeak.Checked = Settings.Default.TalkOnHunt;

            options.optHuntPlay.Checked = Settings.Default.PlayOnHunt;

            options.txtHuntAudioFile.Text = Settings.Default.HuntAudioFile;

            options.txtCautionPrefix.Text = Settings.Default.CautionPrefix;

            options.chkCautionMatchFull.Checked = Settings.Default.MatchFullTextC;  //Caution

            options.optCautionNone.Checked = Settings.Default.NoneOnCaution;

            options.optCautionBeep.Checked = Settings.Default.BeepOnCaution;

            options.optCautionSpeak.Checked = Settings.Default.TalkOnCaution;

            options.optCautionPlay.Checked = Settings.Default.PlayOnCaution;

            options.txtCautionAudioFile.Text = Settings.Default.CautionAudioFile;

            options.txtDangerPrefix.Text = Settings.Default.DangerPrefix;

            options.chkDangerMatchFull.Checked = Settings.Default.MatchFullTextD;  //danger

            options.optDangerNone.Checked = Settings.Default.NoneOnDanger;

            options.optDangerBeep.Checked = Settings.Default.BeepOnDanger;

            options.optDangerSpeak.Checked = Settings.Default.TalkOnDanger;

            options.optDangerPlay.Checked = Settings.Default.PlayOnDanger;

            options.txtDangerAudioFile.Text = Settings.Default.DangerAudioFile;

            options.txtAlertPrefix.Text = Settings.Default.AlertPrefix;

            options.chkAlertMatchFull.Checked = Settings.Default.MatchFullTextA;  //Rare

            options.optAlertNone.Checked = Settings.Default.NoneOnAlert;

            options.optAlertBeep.Checked = Settings.Default.BeepOnAlert;

            options.optAlertSpeak.Checked = Settings.Default.TalkOnAlert;

            options.optAlertPlay.Checked = Settings.Default.PlayOnAlert;

            options.txtAlertAudioFile.Text = Settings.Default.AlertAudioFile;

            options.spnRangeCircle.Value = Settings.Default.RangeCircle;

            options.numMinAlertLevel.Value = Settings.Default.MinAlertLevel;

            options.spnSpawnSize.Value = Settings.Default.SpawnDrawSize;

            options.FadedLines.Value = Settings.Default.FadedLines;

            options.pvpLevels.Value = Settings.Default.PVPLevels;

            options.txtWindowName.Text = Settings.Default.TitleBar;

            options.txtSearchString.Text = Settings.Default.SearchString;

            options.picMapBackgroundColor.BackColor = Settings.Default.BackColor;

            options.picListBackgroundColor.BackColor = Settings.Default.ListBackColor;

            options.picGridColor.BackColor = Settings.Default.GridColor;

            options.picGridLabelColor.BackColor = Settings.Default.GridLabelColor;

            options.picRangeCircleColor.BackColor = Settings.Default.RangeCircleColor;

            options.picPlayerBorder.BackColor = Settings.Default.PCBorderColor;

            options.chkColorRangeCircle.Checked = Settings.Default.AlertInsideRangeCircle;

            options.cmbAlertSound.SelectedItem = Settings.Default.AlertSound;

            options.cmbHatch.SelectedItem = Settings.Default.HatchIndex;

            options.chkDrawFoV.Checked = Settings.Default.DrawFoV;

            options.chkShowZoneName.Checked = Settings.Default.ShowZoneName;

            options.chkShowCharName.Checked = Settings.Default.ShowCharName;

            options.chkShowTargetInfo.Checked = Settings.Default.ShowTargetInfo;

            options.txtMapDir.Text = Settings.Default.MapDir;

            options.txtFilterDir.Text = Settings.Default.FilterDir;

            options.txtCfgDir.Text = Settings.Default.CfgDir;

            options.txtLogDir.Text = Settings.Default.LogDir;

            options.txtTimerDir.Text = Settings.Default.TimerDir;

            options.spnLogLevel.Value = (int)Settings.Default.MaxLogLevel;

            options.chkSelectSpawnList.Checked = Settings.Default.AutoSelectSpawnList;

            SetFgDrawOptions(Settings.Default.DrawOptions, options);
        }

        private static void StyleEnums(OptionsForm options)
        {
            foreach (var styleName in Enum.GetNames(typeof(HatchStyle)))
            {
                options.cmbHatch.Items.Add(styleName);
            }
        }

        public static void SwitchOnSoundSettings()
        {
            if (Settings.Default.AlertSound == "Asterisk")
            {
                SystemSounds.Asterisk.Play();
            }
            else if (Settings.Default.AlertSound == "Beep")
            {
                SystemSounds.Beep.Play();
            }
            else if (Settings.Default.AlertSound == "Exclamation")
            {
                SystemSounds.Exclamation.Play();
            }
            else if (Settings.Default.AlertSound == "Hand")
            {
                SystemSounds.Hand.Play();
            }
            else if (Settings.Default.AlertSound == "Question")
            {
                SystemSounds.Question.Play();
            }
        }

        public void ToolStripLevelCheck(string Str, MainForm f1)
        {
            var validnum = true;
            if (!string.IsNullOrEmpty(Str))
            {
                var isNum = int.TryParse(Str, out var Num);

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
                    f1.toolStripLevel.Text = "Auto";
                }
                else
                {
                    f1.toolStripLevel.Text = Num.ToString();
                    Settings.Default.LevelOverride = Num;
                }
            }

            if (!validnum)
            {
                MessageBox.Show("Enter a number between 1-115 or Auto");
            }
        }

        public void LoadPositionsFromConfigFile(MainForm f1)
        {
            LogLib.WriteLine("Loading Position.Xml", LogLevel.Debug);
            var configFile = Path.Combine(Settings.Default.CfgDir, "positions.xml");

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

                f1.map.Loadmap(filename);

                var lastSlashIndex = filename.LastIndexOf("\\");

                if (lastSlashIndex > 0)
                {
                    filename = filename.Substring(lastSlashIndex + 1);
                }

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

        public static void LookupBoxMatch(Spawninfo si, MainForm f1)
        {
            si.isLookup = false;
            BoxMatch(f1.toolStripLookupBox, si);
            BoxMatch(f1.toolStripLookupBox1, si);
            BoxMatch(f1.toolStripLookupBox2, si);
            BoxMatch(f1.toolStripLookupBox3, si);
            BoxMatch(f1.toolStripLookupBox4, si);
        }

        private static void BoxMatch(ToolStripTextBox boxtext, Spawninfo si)
        {
            if (boxtext.Text.Length > 1
                && boxtext.Text != "Mob Search"
                && RegexHelper.GetRegex(boxtext.Text).Match(si.Name).Success)
            {
                si.isLookup = true;
            }
        }

    }
}
