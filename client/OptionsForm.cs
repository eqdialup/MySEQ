using myseq.Properties;
using Structures;
using System;
using System.Drawing.Drawing2D;
using System.IO;
using System.Media;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace myseq
{
    public partial class OptionsForm : Form
    {
        private readonly FormMethods formMethod = new FormMethods();
        private string selectedCautionWav;
        private readonly string[] buttonTexts = { "None", "Beep", "Speech", "Play wav" };
        private int currentIndex = 0;

        public OptionsForm()
        {
            InitializeComponent();
            SetCautionTextFromSettings();
            SetHuntTextFromSettings();
            SetAlertTextFromSettings();
            SetDangerTextFromSettings();
            SetOptions(this);
        }

        private static void SetFgDrawOptions(DrawOptions DrawOpts, OptionsForm options)
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

            options.HuntAudioFileBox.Text = Settings.Default.HuntAudioFile;

            options.txtCautionPrefix.Text = Settings.Default.CautionPrefix;

            options.chkCautionMatchFull.Checked = Settings.Default.MatchFullTextC;  //Caution

            options.CautionAudioFileBox.Text = Settings.Default.CautionAudioFile;

            options.txtDangerPrefix.Text = Settings.Default.DangerPrefix;

            options.chkDangerMatchFull.Checked = Settings.Default.MatchFullTextD;  //danger

            options.DangerAudioFileBox.Text = Settings.Default.DangerAudioFile;

            options.txtAlertPrefix.Text = Settings.Default.AlertPrefix;

            options.chkAlertMatchFull.Checked = Settings.Default.MatchFullTextA;  //Rare

            options.AlertAudioFileBox.Text = Settings.Default.AlertAudioFile;

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

        #region ClickResponders

        private void CmdCommand_Click(object sender, EventArgs e)
        {
            if (chkSaveOnExit.Checked)
            {
                Settings.Default.Save();
            }
            Hide();
        }

        private void CmdMapBackgroundColor_Click(object sender, EventArgs e)
        {
            colorOptionPicker.Color = picMapBackgroundColor.BackColor;

            if (colorOptionPicker.ShowDialog() != DialogResult.Cancel)
            {
                picMapBackgroundColor.BackColor = colorOptionPicker.Color;
            }
        }

        private void CmdListBackgroundColor_Click(object sender, EventArgs e)

        {
            colorOptionPicker.Color = picListBackgroundColor.BackColor;

            if (colorOptionPicker.ShowDialog() != DialogResult.Cancel)

            {
                Settings.Default.ListBackColor = colorOptionPicker.Color;

                picListBackgroundColor.BackColor = colorOptionPicker.Color;
            }
        }

        private void CmdGridColor_Click(object sender, EventArgs e)

        {
            colorOptionPicker.Color = picGridColor.BackColor;

            if (colorOptionPicker.ShowDialog() != DialogResult.Cancel)

            {
                Settings.Default.GridColor = colorOptionPicker.Color;

                picGridColor.BackColor = colorOptionPicker.Color;
            }
        }

        private void CmdRangeCircleColor_Click(object sender, EventArgs e)
        {
            colorOptionPicker.Color = picRangeCircleColor.BackColor;

            if (colorOptionPicker.ShowDialog() != DialogResult.Cancel)

            {
                Settings.Default.RangeCircleColor = colorOptionPicker.Color;

                picRangeCircleColor.BackColor = colorOptionPicker.Color;
            }
        }

        private void CmdMapDirBrowse_Click(object sender, EventArgs e)
        {
            txtMapDir.Text = formMethod.FolderBrowser("Map Directory", Settings.Default.MapDir);
            if (!string.IsNullOrEmpty(txtMapDir.Text)) Settings.Default.MapDir = txtMapDir.Text;
        }

        private void CmdCfgDirBrowse_Click(object sender, EventArgs e)
        {
            txtCfgDir.Text = formMethod.FolderBrowser("Config Directory", Settings.Default.CfgDir);
            if (!string.IsNullOrEmpty(txtCfgDir.Text)) Settings.Default.CfgDir = txtCfgDir.Text;
        }

        private void CmdFilterDirBrowse_Click(object sender, EventArgs e)
        {
            txtFilterDir.Text = formMethod.FolderBrowser("Filter Directory", Settings.Default.FilterDir);
            if (!string.IsNullOrEmpty(txtFilterDir.Text)) Settings.Default.FilterDir = txtFilterDir.Text;
        }

        private void CmdLogDir_Click(object sender, EventArgs e)
        {
            txtLogDir.Text = formMethod.FolderBrowser("Log Directory", Settings.Default.LogDir);
            if (!string.IsNullOrEmpty(txtLogDir.Text)) Settings.Default.LogDir = txtLogDir.Text;
        }

        private void CmdSpawnTimers_Click(object sender, EventArgs e)
        {
            txtTimerDir.Text = formMethod.FolderBrowser("Timers log Directory", Settings.Default.TimerDir);
            if (!string.IsNullOrEmpty(txtTimerDir.Text)) Settings.Default.TimerDir = txtTimerDir.Text;
        }

        private void CmdGridLabelColor_Click(object sender, EventArgs e)
        {
            colorOptionPicker.Color = picGridLabelColor.BackColor;

            if (colorOptionPicker.ShowDialog() != DialogResult.Cancel)
            {
                Settings.Default.GridLabelColor = colorOptionPicker.Color;

                picGridLabelColor.BackColor = colorOptionPicker.Color;
            }
        }

        private void ButPlayerBorder_Click(object sender, EventArgs e)
        {
            colorOptionPicker.Color = picPlayerBorder.BackColor;

            if (colorOptionPicker.ShowDialog() != DialogResult.Cancel)
            {
                picPlayerBorder.BackColor = colorOptionPicker.Color;
            }
        }

        #endregion ClickResponders

        #region Changing User Settings

        private void CmbAlertSound_SelectionChangeCommitted(object sender, EventArgs e) => Settings.Default.AlertSound = cmbAlertSound.SelectedItem.ToString();

        private void CmbHatchPattern_SelectionChangeCommitted(object sender, EventArgs e) => Settings.Default.HatchIndex = cmbHatch.SelectedItem.ToString();

        private void SpnSpawnSize_ValueChanged(object sender, EventArgs e) => Settings.Default.SpawnDrawSize = (int)spnSpawnSize.Value;

        private void SpnLogLevel_ValueChanged(object sender, EventArgs e) => Settings.Default.MaxLogLevel = (LogLevel)spnLogLevel.Value;

        private void PvpLevels_ValueChanged(object sender, EventArgs e) => Settings.Default.PVPLevels = (int)pvpLevels.Value;

        private void MinAlertLevel_ValueChanged(object sender, EventArgs e) => Settings.Default.MinAlertLevel = (int)numMinAlertLevel.Value;

        private void SpnRangeCircle_ValueChanged(object sender, EventArgs e) => Settings.Default.RangeCircle = (int)spnRangeCircle.Value;

        private void SpnUpdateDelay_ValueChanged(object sender, EventArgs e) => Settings.Default.UpdateDelay = (int)spnUpdateDelay.Value;

        private void SpnFadedLines_ValueChanged(object sender, EventArgs e) => Settings.Default.FadedLines = (int)FadedLines.Value;

        private void ChkShowZoneName_CheckedChanged(object sender, EventArgs e) => Settings.Default.ShowZoneName = chkShowZoneName.Checked;

        private void ChkShowCharName_CheckedChanged(object sender, EventArgs e) => Settings.Default.ShowCharName = chkShowZoneName.Checked;

        private void ChkSaveOnExit_CheckedChanged(object sender, EventArgs e) => Settings.Default.SaveOnExit = chkSaveOnExit.Checked;

        private void ChkPrefixAlerts_CheckedChanged(object sender, EventArgs e) => Settings.Default.PrefixStars = chkPrefixAlerts.Checked;

        private void ChkCorpsesAlerts_CheckedChanged(object sender, EventArgs e) => Settings.Default.CorpseAlerts = chkCorpsesAlerts.Checked;

        private void ChkAffixAlerts_CheckedChanged(object sender, EventArgs e) => Settings.Default.AffixStars = chkAffixAlerts.Checked;

        private void ChkHuntMatchFull_CheckedChanged(object sender, EventArgs e) => Settings.Default.MatchFullTextH = chkHuntMatchFull.Checked;

        private void ChkCautionMatchFull_CheckedChanged(object sender, EventArgs e) => Settings.Default.MatchFullTextC = chkCautionMatchFull.Checked;

        private void OptDangerBeep_CheckedChanged(object sender, EventArgs e) => Settings.Default.MatchFullTextD = chkDangerMatchFull.Checked;

        private void ChkAlertMatchFull_CheckedChanged(object sender, EventArgs e) => Settings.Default.MatchFullTextA = chkAlertMatchFull.Checked;

        #region MapOptions

        private void ToggleDrawOption(CheckBox checkBox, DrawOptions option)
        {
            if (checkBox.Checked)
            {
                Settings.Default.DrawOptions |= option;  // Set the flag
            }
            else
            {
                Settings.Default.DrawOptions &= ~option;  // Clear the flag
            }
        }

        private void ChkZoneNames_CheckedChanged(object sender, EventArgs e)
        {
            ToggleDrawOption(sender as CheckBox, DrawOptions.ZoneText);
        }

        private void CheckMap_CheckedChanged(object sender, EventArgs e)
        {
            ToggleDrawOption(sender as CheckBox, DrawOptions.DrawMap);
        }

        private void CheckPlayer_CheckedChanged(object sender, EventArgs e)
        {
            ToggleDrawOption(sender as CheckBox, DrawOptions.Player);
        }

        private void DrawGround_CheckedChanged(object sender, EventArgs e)
        {
            ToggleDrawOption(sender as CheckBox, DrawOptions.GroundItems);
        }

        private void AdjustMap_CheckedChanged(object sender, EventArgs e)
        {
            ToggleDrawOption(sender as CheckBox, DrawOptions.Readjust);
        }

        private void CheckSpawns_CheckedChanged(object sender, EventArgs e)
        {
            ToggleDrawOption(sender as CheckBox, DrawOptions.Spawns);
        }

        private void DrawTrails_CheckedChanged(object sender, EventArgs e)
        {
            ToggleDrawOption(sender as CheckBox, DrawOptions.SpawnTrails);
        }

        private void HighlightMerch_CheckedChanged(object sender, EventArgs e)
        {
            ToggleDrawOption(sender as CheckBox, DrawOptions.SpawnRings);
        }

        private void DrawGrid_CheckedChanged(object sender, EventArgs e)
        {
            ToggleDrawOption(sender as CheckBox, DrawOptions.GridLines);
        }

        private void SpawnTimers_CheckChanged(object sender, EventArgs e)
        {
            ToggleDrawOption(sender as CheckBox, DrawOptions.SpawnTimers);
        }

        private void DirectionLine_CheckChanged(object sender, EventArgs e)
        {
            ToggleDrawOption(sender as CheckBox, DrawOptions.DirectionLines);
        }

        private void LineToPoint_CheckChanged(object sender, EventArgs e)
        {
            ToggleDrawOption(sender as CheckBox, DrawOptions.SpotLine);
        }

        #endregion MapOptions

        private void Ip_Address1_Changed(object sender, EventArgs e) => Settings.Default.IPAddress1 = txtIPAddress1.Text;

        private void Ip_Address2_Changed(object sender, EventArgs e) => Settings.Default.IPAddress2 = txtIPAddress2.Text;

        private void Ip_Address3_Changed(object sender, EventArgs e) => Settings.Default.IPAddress3 = txtIPAddress3.Text;

        private void Ip_Address4_Changed(object sender, EventArgs e) => Settings.Default.IPAddress4 = txtIPAddress4.Text;

        private void Ip_Address5_Changed(object sender, EventArgs e) => Settings.Default.IPAddress5 = txtIPAddress5.Text;

        private void Port_Changed(object sender, EventArgs e)
        {
            Settings.Default.Port = int.Parse(txtPortNo.Text);
        }

        private void DangerPrefix_Changed(object sender, EventArgs e) => Settings.Default.DangerPrefix = txtDangerPrefix.Text;

        private void CautionPrefix_Changed(object sender, EventArgs e) => Settings.Default.CautionPrefix = txtCautionPrefix.Text;

        private void HuntPrefix_Changed(object sender, EventArgs e) => Settings.Default.HuntPrefix = txtHuntPrefix.Text;

        private void WindowName_Changed(object sender, EventArgs e) => Settings.Default.IPAddress2 = txtWindowName.Text;

        private void AlertPrefix_Changed(object sender, EventArgs e) => Settings.Default.AlertPrefix = txtAlertPrefix.Text;

        private void OverrideLevel_Changed(object sender, EventArgs e) => Settings.Default.LevelOverride = (int)spnOverrideLevel.Value;

        #endregion Changing User Settings

        #region CautionAlerts

        private void CautionCycleButton_Click(object sender, EventArgs e)
        {
            CautionCycleButton.Text = $"Will alert as: {buttonTexts[currentIndex]}";
            UpdateCautionSettings(buttonTexts[currentIndex]); // Update settings when button text changes
            currentIndex = (currentIndex + 1) % buttonTexts.Length; // Cycle through the array
        }

        private void UpdateCautionSettings(string currentText)
        {
            // Reset all settings to false
            Settings.Default.NoneOnCaution = false;
            Settings.Default.BeepOnCaution = false;
            Settings.Default.TalkOnCaution = false;
            Settings.Default.PlayOnCaution = false;

            // Enable the relevant setting
            switch (currentText)
            {
                case "None":
                    Settings.Default.NoneOnCaution = true;
                    break;

                case "Beep":
                    Settings.Default.BeepOnCaution = true;
                    break;

                case "Speech":
                    Settings.Default.TalkOnCaution = true;
                    break;

                case "Play wav":
                    Settings.Default.PlayOnCaution = true;
                    break;
            }

            Settings.Default.Save(); // Save the settings if necessary
        }

        private void SetCautionTextFromSettings()
        {
            if (Settings.Default.NoneOnCaution)
                currentIndex = 0;
            else if (Settings.Default.BeepOnCaution)
                currentIndex = 1;
            else if (Settings.Default.TalkOnCaution)
                currentIndex = 2;
            else if (Settings.Default.PlayOnCaution)
                currentIndex = 3;

            CautionCycleButton.Text = buttonTexts[currentIndex]; // Set the initial button text
        }

        private void CautionTest_Click(object sender, EventArgs e)
        {
            if (Settings.Default.BeepOnCaution)
            {
                Console.Beep(300, 250);
            }
            else if (Settings.Default.TalkOnCaution)
            {
                var talker = new Talker($"Caution Testing!");
                Task.Run(() => talker.SpeakText());
            }
            else if (Settings.Default.PlayOnCaution)
            {
                if (!string.IsNullOrEmpty(Settings.Default.CautionAudioFile) && File.Exists(Settings.Default.CautionAudioFile))
                {
                    using (SoundPlayer player = new SoundPlayer(selectedCautionWav))
                    {
                        player.Play(); // Play the WAV file asynchronously
                    }
                }
            }
        }

        private void CautionAudioFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "WAV files (*.wav)|*.wav";
                openFileDialog.Title = "Select a WAV file";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Store the selected file path
                    selectedCautionWav = openFileDialog.FileName;
                    // Display the file path in a label or textbox
                    CautionAudioFileBox.Text = selectedCautionWav;
                }
            }
        }

        #endregion CautionAlerts

        #region HuntAlerts

        private void HuntCycleButton_Click(object sender, EventArgs e)
        {
            HuntCycleButton.Text = $"Will alert as: {buttonTexts[currentIndex]}";
            UpdateHuntSettings(buttonTexts[currentIndex]); // Update settings when button text changes
            currentIndex = (currentIndex + 1) % buttonTexts.Length; // Cycle through the array
        }

        private void UpdateHuntSettings(string currentText)
        {
            // Reset all settings to false
            Settings.Default.NoneOnHunt = false;
            Settings.Default.BeepOnHunt = false;
            Settings.Default.TalkOnHunt = false;
            Settings.Default.PlayOnHunt = false;

            // Enable the relevant setting
            switch (currentText)
            {
                case "None":
                    Settings.Default.NoneOnHunt = true;
                    break;

                case "Beep":
                    Settings.Default.BeepOnHunt = true;
                    break;

                case "Speech":
                    Settings.Default.TalkOnHunt = true;
                    break;

                case "Play wav":
                    Settings.Default.PlayOnHunt = true;
                    break;
            }

            Settings.Default.Save(); // Save the settings if necessary
        }

        private void SetHuntTextFromSettings()
        {
            if (Settings.Default.NoneOnHunt)
                currentIndex = 0;
            else if (Settings.Default.BeepOnHunt)
                currentIndex = 1;
            else if (Settings.Default.TalkOnHunt)
                currentIndex = 2;
            else if (Settings.Default.PlayOnHunt)
                currentIndex = 3;
            HuntCycleButton.Text = buttonTexts[currentIndex]; // Set the initial button text
        }

        private void HuntAudioFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "WAV files (*.wav)|*.wav";
                openFileDialog.Title = "Select a WAV file";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Store the selected file path
                    Settings.Default.HuntAudioFile = openFileDialog.FileName;
                    // Display the file path in a label or textbox
                    HuntAudioFileBox.Text = Settings.Default.HuntAudioFile;
                }
            }
        }

        #endregion HuntAlerts

        #region Rarealerts

        private void RareCycleButton_Click(object sender, EventArgs e)
        {
            RareCycleButton.Text = $"Will alert as: {buttonTexts[currentIndex]}";
            UpdateRareSettings(buttonTexts[currentIndex]); // Update settings when button text changes
            currentIndex = (currentIndex + 1) % buttonTexts.Length; // Cycle through the array
        }

        private void UpdateRareSettings(string currentText)
        {
            // Reset all settings to false
            Settings.Default.NoneOnAlert = false;
            Settings.Default.BeepOnAlert = false;
            Settings.Default.TalkOnAlert = false;
            Settings.Default.PlayOnAlert = false;

            // Enable the relevant setting
            switch (currentText)
            {
                case "None":
                    Settings.Default.NoneOnAlert = true;
                    break;

                case "Beep":
                    Settings.Default.BeepOnAlert = true;
                    break;

                case "Speech":
                    Settings.Default.TalkOnAlert = true;
                    break;

                case "Play wav":
                    Settings.Default.PlayOnAlert = true;
                    break;
            }

            Settings.Default.Save(); // Save the settings if necessary
        }

        private void SetAlertTextFromSettings()
        {
            if (Settings.Default.NoneOnAlert)
                currentIndex = 0;
            else if (Settings.Default.BeepOnAlert)
                currentIndex = 1;
            else if (Settings.Default.TalkOnAlert)
                currentIndex = 2;
            else if (Settings.Default.PlayOnAlert)
                currentIndex = 3;
            RareCycleButton.Text = buttonTexts[currentIndex]; // Set the initial button text
        }

        private void AlertAudioFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "WAV files (*.wav)|*.wav";
                openFileDialog.Title = "Select a WAV file";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Store the selected file path
                    Settings.Default.AlertAudioFile = openFileDialog.FileName;
                    // Display the file path in a label or textbox
                    HuntAudioFileBox.Text = Settings.Default.AlertAudioFile;
                }
            }
        }

        #endregion Rarealerts

        #region DangerAlerts

        private void DangerCycleButton_Click(object sender, EventArgs e)
        {
            DangerCycleButton.Text = $"Will alert as: {buttonTexts[currentIndex]}";
            UpdateDangerSettings(buttonTexts[currentIndex]); // Update settings when button text changes
            currentIndex = (currentIndex + 1) % buttonTexts.Length; // Cycle through the array
        }

        private void UpdateDangerSettings(string currentText)
        {
            // Reset all settings to false
            Settings.Default.NoneOnDanger = false;
            Settings.Default.BeepOnDanger = false;
            Settings.Default.TalkOnDanger = false;
            Settings.Default.PlayOnDanger = false;

            // Enable the relevant setting
            switch (currentText)
            {
                case "None":
                    Settings.Default.NoneOnDanger = true;
                    break;

                case "Beep":
                    Settings.Default.BeepOnDanger = true;
                    break;

                case "Speech":
                    Settings.Default.TalkOnDanger = true;
                    break;

                case "Play wav":
                    Settings.Default.PlayOnDanger = true;
                    break;
            }

            Settings.Default.Save(); // Save the settings if necessary
        }

        private void SetDangerTextFromSettings()
        {
            if (Settings.Default.NoneOnDanger)
                currentIndex = 0;
            else if (Settings.Default.BeepOnDanger)
                currentIndex = 1;
            else if (Settings.Default.TalkOnDanger)
                currentIndex = 2;
            else if (Settings.Default.PlayOnDanger)
                currentIndex = 3;
            DangerCycleButton.Text = buttonTexts[currentIndex]; // Set the initial button text
        }

        private void DangerAudioFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "WAV files (*.wav)|*.wav";
                openFileDialog.Title = "Select a WAV file";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Store the selected file path
                    Settings.Default.DangerAudioFile = openFileDialog.FileName;
                    // Display the file path in a label or textbox
                    DangerAudioFileBox.Text = Settings.Default.DangerAudioFile;
                }
            }
        }

        #endregion DangerAlerts
    }
}