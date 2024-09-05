﻿using myseq.Properties;
using Structures;
using System;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace myseq
{
    public partial class OptionsForm : Form
    {
        private readonly FormMethods formMethod = new FormMethods();

        public OptionsForm()
        {
            InitializeComponent();
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

        private void OptCautionBeep_CheckedChanged(object sender, EventArgs e) => Settings.Default.BeepOnCaution = optCautionBeep.Checked;

        private void OptDangerBeep_CheckedChanged(object sender, EventArgs e) => Settings.Default.BeepOnDanger = optDangerBeep.Checked;

        private void ChkAlertMatchFull_CheckedChanged(object sender, EventArgs e) => Settings.Default.MatchFullTextA = chkAlertMatchFull.Checked;

        #region MapOptions

        private void ChkZoneNames_CheckedChanged(object sender, EventArgs e) => Settings.Default.DrawOptions ^= DrawOptions.ZoneText;

        private void CheckMap_CheckedChanged(object sender, EventArgs e) => Settings.Default.DrawOptions ^= DrawOptions.DrawMap;

        private void CheckPlayer_CheckedChanged(object sender, EventArgs e) => Settings.Default.DrawOptions ^= DrawOptions.Player;

        private void DrawGround_CheckedChanged(object sender, EventArgs e) => Settings.Default.DrawOptions ^= DrawOptions.GroundItems;

        private void AdjustMap_CheckedChanged(object sender, EventArgs e) => Settings.Default.DrawOptions ^= DrawOptions.Readjust;

        private void CheckSpawns_CheckedChanged(object sender, EventArgs e) => Settings.Default.DrawOptions ^= DrawOptions.Spawns;

        private void DrawTrails_CheckedChanged(object sender, EventArgs e) => Settings.Default.DrawOptions ^= DrawOptions.SpawnTrails;

        private void HighlightMerch_CheckedChanged(object sender, EventArgs e) => Settings.Default.DrawOptions ^= DrawOptions.SpawnRings;

        private void DrawGrid_CheckedChanged(object sender, EventArgs e) => Settings.Default.DrawOptions ^= DrawOptions.GridLines;

        private void SpawnTimers_CheckChanged(object sender, EventArgs e) => Settings.Default.DrawOptions ^= DrawOptions.SpawnTimers;

        private void DirectionLine_CheckChanged(object sender, EventArgs e) => Settings.Default.DrawOptions ^= DrawOptions.DirectionLines;

        private void LineToPoint_CheckChanged(object sender, EventArgs e) => Settings.Default.DrawOptions ^= DrawOptions.SpotLine;

        #endregion MapOptions

        private void Ip_Address1_Changed(object sender, EventArgs e) => Settings.Default.IPAddress1 = txtIPAddress1.Text;

        private void Ip_Address2_Changed(object sender, EventArgs e) => Settings.Default.IPAddress2 = txtIPAddress2.Text;

        private void Ip_Address3_Changed(object sender, EventArgs e) => Settings.Default.IPAddress3 = txtIPAddress3.Text;

        private void Ip_Address4_Changed(object sender, EventArgs e) => Settings.Default.IPAddress4 = txtIPAddress4.Text;

        private void Ip_Address5_Changed(object sender, EventArgs e) => Settings.Default.IPAddress5 = txtIPAddress5.Text;

        private void Port_Changed(object sender, EventArgs e)
        {
            //int.TryParse(txtPortNo.Text, out int port);
            Settings.Default.Port = int.Parse(txtPortNo.Text); //port;
        }

        private void DangerPrefix_Changed(object sender, EventArgs e) => Settings.Default.DangerPrefix = txtDangerPrefix.Text;

        private void CautionPrefix_Changed(object sender, EventArgs e) => Settings.Default.CautionPrefix = txtCautionPrefix.Text;

        private void HuntPrefix_Changed(object sender, EventArgs e) => Settings.Default.HuntPrefix = txtHuntPrefix.Text;

        private void WindowName_Changed(object sender, EventArgs e) => Settings.Default.IPAddress2 = txtWindowName.Text;

        private void AlertPrefix_Changed(object sender, EventArgs e) => Settings.Default.AlertPrefix = txtAlertPrefix.Text;

        private void OverrideLevel_Changed(object sender, EventArgs e) => Settings.Default.LevelOverride = (int)spnOverrideLevel.Value;

        #endregion Changing User Settings
    }
}