using System.Windows.Forms;

namespace myseq
{
    partial class OptionsForm
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblSpawnTimers = new System.Windows.Forms.Label();
            this.lblLogDir = new System.Windows.Forms.Label();
            this.lblFilterDir = new System.Windows.Forms.Label();
            this.lblCfgDir = new System.Windows.Forms.Label();
            this.lblMapDir = new System.Windows.Forms.Label();
            this.grpDanger = new System.Windows.Forms.GroupBox();
            this.optDangerPlay = new System.Windows.Forms.RadioButton();
            this.optDangerSpeak = new System.Windows.Forms.RadioButton();
            this.optDangerBeep = new System.Windows.Forms.RadioButton();
            this.optDangerNone = new System.Windows.Forms.RadioButton();
            this.txtDangerAudioFile = new System.Windows.Forms.TextBox();
            this.txtDangerPrefix = new System.Windows.Forms.TextBox();
            this.lblDangerPrefix = new System.Windows.Forms.Label();
            this.chkDangerMatchFull = new System.Windows.Forms.CheckBox();
            this.grpAlert = new System.Windows.Forms.GroupBox();
            this.optAlertPlay = new System.Windows.Forms.RadioButton();
            this.optAlertSpeak = new System.Windows.Forms.RadioButton();
            this.optAlertBeep = new System.Windows.Forms.RadioButton();
            this.optAlertNone = new System.Windows.Forms.RadioButton();
            this.txtAlertAudioFile = new System.Windows.Forms.TextBox();
            this.txtAlertPrefix = new System.Windows.Forms.TextBox();
            this.lblAlertPrefix = new System.Windows.Forms.Label();
            this.chkAlertMatchFull = new System.Windows.Forms.CheckBox();
            this.grpCaution = new System.Windows.Forms.GroupBox();
            this.optCautionPlay = new System.Windows.Forms.RadioButton();
            this.optCautionSpeak = new System.Windows.Forms.RadioButton();
            this.optCautionBeep = new System.Windows.Forms.RadioButton();
            this.optCautionNone = new System.Windows.Forms.RadioButton();
            this.txtCautionAudioFile = new System.Windows.Forms.TextBox();
            this.txtCautionPrefix = new System.Windows.Forms.TextBox();
            this.lblCautionPrefix = new System.Windows.Forms.Label();
            this.chkCautionMatchFull = new System.Windows.Forms.CheckBox();
            this.grpHunt = new System.Windows.Forms.GroupBox();
            this.optHuntPlay = new System.Windows.Forms.RadioButton();
            this.optHuntSpeak = new System.Windows.Forms.RadioButton();
            this.optHuntBeep = new System.Windows.Forms.RadioButton();
            this.optHuntNone = new System.Windows.Forms.RadioButton();
            this.txtHuntAudioFile = new System.Windows.Forms.TextBox();
            this.txtHuntPrefix = new System.Windows.Forms.TextBox();
            this.lblHuntPrefix = new System.Windows.Forms.Label();
            this.chkHuntMatchFull = new System.Windows.Forms.CheckBox();
            this.lblFadedLines = new System.Windows.Forms.Label();
            this.lblPVPLevels = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbAlertSound = new System.Windows.Forms.ComboBox();
            this.cmbHatch = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.chkColorRangeCircle = new System.Windows.Forms.CheckBox();
            this.spnRangeCircle = new System.Windows.Forms.NumericUpDown();
            this.numMinAlertLevel = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkMap = new System.Windows.Forms.CheckBox();
            this.chkPlayer = new System.Windows.Forms.CheckBox();
            this.chkSpawns = new System.Windows.Forms.CheckBox();
            this.chkAddjust = new System.Windows.Forms.CheckBox();
            this.chkGround = new System.Windows.Forms.CheckBox();
            this.chkTrails = new System.Windows.Forms.CheckBox();
            this.chkHighlight = new System.Windows.Forms.CheckBox();
            this.chkGrid = new System.Windows.Forms.CheckBox();
            this.chkTimers = new System.Windows.Forms.CheckBox();
            this.chkText = new System.Windows.Forms.CheckBox();
            this.chkDirection = new System.Windows.Forms.CheckBox();
            this.chkLineToPoint = new System.Windows.Forms.CheckBox();
            this.lblSpawnSize = new System.Windows.Forms.Label();
            this.lblSearch = new System.Windows.Forms.Label();
            this.lblLogLevel = new System.Windows.Forms.Label();
            this.lblWindowName = new System.Windows.Forms.Label();
            this.lblOverridelevel = new System.Windows.Forms.Label();
            this.gbServer = new System.Windows.Forms.GroupBox();
            this.txtIPAddress5 = new System.Windows.Forms.TextBox();
            this.lblIPAddress5 = new System.Windows.Forms.Label();
            this.txtIPAddress4 = new System.Windows.Forms.TextBox();
            this.lblIPAddress4 = new System.Windows.Forms.Label();
            this.txtIPAddress3 = new System.Windows.Forms.TextBox();
            this.lblIPAddress3 = new System.Windows.Forms.Label();
            this.txtIPAddress2 = new System.Windows.Forms.TextBox();
            this.lblIPAddress2 = new System.Windows.Forms.Label();
            this.txtPortNo = new System.Windows.Forms.TextBox();
            this.txtIPAddress1 = new System.Windows.Forms.TextBox();
            this.lblIPAddress1 = new System.Windows.Forms.Label();
            this.lbltxtPortNo = new System.Windows.Forms.Label();
            this.lblUpdateDelay = new System.Windows.Forms.Label();
            this.tabOptions = new System.Windows.Forms.TabControl();
            this.tabGeneral = new System.Windows.Forms.TabPage();
            this.chkShowCharName = new System.Windows.Forms.CheckBox();
            this.txtSearchString = new System.Windows.Forms.TextBox();
            this.spnLogLevel = new System.Windows.Forms.NumericUpDown();
            this.chkShowZoneName = new System.Windows.Forms.CheckBox();
            this.spnOverrideLevel = new System.Windows.Forms.NumericUpDown();
            this.spnUpdateDelay = new System.Windows.Forms.NumericUpDown();
            this.txtWindowName = new System.Windows.Forms.TextBox();
            this.chkSaveOnExit = new System.Windows.Forms.CheckBox();
            this.tabMap = new System.Windows.Forms.TabPage();
            this.FadedLines = new System.Windows.Forms.NumericUpDown();
            this.pvpLevels = new System.Windows.Forms.NumericUpDown();
            this.chkSelectSpawnList = new System.Windows.Forms.CheckBox();
            this.spnSpawnSize = new System.Windows.Forms.NumericUpDown();
            this.chkShowTargetInfo = new System.Windows.Forms.CheckBox();
            this.chkDrawFoV = new System.Windows.Forms.CheckBox();
            this.tabAlerts = new System.Windows.Forms.TabPage();
            this.chkAffixAlerts = new System.Windows.Forms.CheckBox();
            this.chkPrefixAlerts = new System.Windows.Forms.CheckBox();
            this.chkCorpsesAlerts = new System.Windows.Forms.CheckBox();
            this.tabFolders = new System.Windows.Forms.TabPage();
            this.cmdSpawnTimers = new System.Windows.Forms.Button();
            this.txtTimerDir = new System.Windows.Forms.TextBox();
            this.txtLogDir = new System.Windows.Forms.TextBox();
            this.txtFilterDir = new System.Windows.Forms.TextBox();
            this.txtCfgDir = new System.Windows.Forms.TextBox();
            this.txtMapDir = new System.Windows.Forms.TextBox();
            this.cmdLogDir = new System.Windows.Forms.Button();
            this.cmdFilterDirBrowse = new System.Windows.Forms.Button();
            this.cmdCfgDirBrowse = new System.Windows.Forms.Button();
            this.cmdMapDirBrowse = new System.Windows.Forms.Button();
            this.tabColors = new System.Windows.Forms.TabPage();
            this.picGridLabelColor = new System.Windows.Forms.PictureBox();
            this.cmdGridLabelColor = new System.Windows.Forms.Button();
            this.picPlayerBorder = new System.Windows.Forms.PictureBox();
            this.butPlayerBorder = new System.Windows.Forms.Button();
            this.picListBackgroundColor = new System.Windows.Forms.PictureBox();
            this.cmdListBackgroundColor = new System.Windows.Forms.Button();
            this.picRangeCircleColor = new System.Windows.Forms.PictureBox();
            this.cmdRangeCircleColor = new System.Windows.Forms.Button();
            this.picGridColor = new System.Windows.Forms.PictureBox();
            this.cmdGridColor = new System.Windows.Forms.Button();
            this.picMapBackgroundColor = new System.Windows.Forms.PictureBox();
            this.cmdMapBackgroundColor = new System.Windows.Forms.Button();
            this.cmdCommand = new System.Windows.Forms.Button();
            this.colorOptionPicker = new System.Windows.Forms.ColorDialog();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.grpDanger.SuspendLayout();
            this.grpAlert.SuspendLayout();
            this.grpCaution.SuspendLayout();
            this.grpHunt.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spnRangeCircle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMinAlertLevel)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.gbServer.SuspendLayout();
            this.tabOptions.SuspendLayout();
            this.tabGeneral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spnLogLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spnOverrideLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spnUpdateDelay)).BeginInit();
            this.tabMap.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FadedLines)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pvpLevels)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spnSpawnSize)).BeginInit();
            this.tabAlerts.SuspendLayout();
            this.tabFolders.SuspendLayout();
            this.tabColors.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picGridLabelColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPlayerBorder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picListBackgroundColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picRangeCircleColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGridColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMapBackgroundColor)).BeginInit();
            this.SuspendLayout();
            // 
            // lblSpawnTimers
            // 
            this.lblSpawnTimers.Location = new System.Drawing.Point(8, 168);
            this.lblSpawnTimers.Name = "lblSpawnTimers";
            this.lblSpawnTimers.Size = new System.Drawing.Size(144, 16);
            this.lblSpawnTimers.TabIndex = 38;
            this.lblSpawnTimers.Text = "Spawn Timers";
            // 
            // lblLogDir
            // 
            this.lblLogDir.Location = new System.Drawing.Point(8, 128);
            this.lblLogDir.Name = "lblLogDir";
            this.lblLogDir.Size = new System.Drawing.Size(144, 16);
            this.lblLogDir.TabIndex = 35;
            this.lblLogDir.Text = "Log Folder";
            // 
            // lblFilterDir
            // 
            this.lblFilterDir.Location = new System.Drawing.Point(8, 88);
            this.lblFilterDir.Name = "lblFilterDir";
            this.lblFilterDir.Size = new System.Drawing.Size(144, 16);
            this.lblFilterDir.TabIndex = 32;
            this.lblFilterDir.Text = "Filter Folder";
            // 
            // lblCfgDir
            // 
            this.lblCfgDir.Location = new System.Drawing.Point(8, 48);
            this.lblCfgDir.Name = "lblCfgDir";
            this.lblCfgDir.Size = new System.Drawing.Size(144, 16);
            this.lblCfgDir.TabIndex = 29;
            this.lblCfgDir.Text = "Config Folder";
            // 
            // lblMapDir
            // 
            this.lblMapDir.Location = new System.Drawing.Point(8, 8);
            this.lblMapDir.Name = "lblMapDir";
            this.lblMapDir.Size = new System.Drawing.Size(144, 16);
            this.lblMapDir.TabIndex = 26;
            this.lblMapDir.Text = "Map Folder";
            // 
            // grpDanger
            // 
            this.grpDanger.Controls.Add(this.optDangerPlay);
            this.grpDanger.Controls.Add(this.optDangerSpeak);
            this.grpDanger.Controls.Add(this.optDangerBeep);
            this.grpDanger.Controls.Add(this.optDangerNone);
            this.grpDanger.Controls.Add(this.txtDangerAudioFile);
            this.grpDanger.Controls.Add(this.txtDangerPrefix);
            this.grpDanger.Controls.Add(this.lblDangerPrefix);
            this.grpDanger.Controls.Add(this.chkDangerMatchFull);
            this.grpDanger.Location = new System.Drawing.Point(4, 209);
            this.grpDanger.Name = "grpDanger";
            this.grpDanger.Size = new System.Drawing.Size(248, 78);
            this.grpDanger.TabIndex = 23;
            this.grpDanger.TabStop = false;
            this.grpDanger.Text = "Danger";
            // 
            // optDangerPlay
            // 
            this.optDangerPlay.Location = new System.Drawing.Point(8, 55);
            this.optDangerPlay.Name = "optDangerPlay";
            this.optDangerPlay.Size = new System.Drawing.Size(80, 16);
            this.optDangerPlay.TabIndex = 18;
            this.optDangerPlay.Text = "Play Wav";
            // 
            // optDangerSpeak
            // 
            this.optDangerSpeak.Location = new System.Drawing.Point(167, 35);
            this.optDangerSpeak.Name = "optDangerSpeak";
            this.optDangerSpeak.Size = new System.Drawing.Size(72, 16);
            this.optDangerSpeak.TabIndex = 17;
            this.optDangerSpeak.Text = "Speak";
            // 
            // optDangerBeep
            // 
            this.optDangerBeep.Location = new System.Drawing.Point(88, 35);
            this.optDangerBeep.Name = "optDangerBeep";
            this.optDangerBeep.Size = new System.Drawing.Size(72, 16);
            this.optDangerBeep.TabIndex = 16;
            this.optDangerBeep.Text = "Beep";
            // 
            // optDangerNone
            // 
            this.optDangerNone.Checked = true;
            this.optDangerNone.Location = new System.Drawing.Point(8, 35);
            this.optDangerNone.Name = "optDangerNone";
            this.optDangerNone.Size = new System.Drawing.Size(72, 16);
            this.optDangerNone.TabIndex = 15;
            this.optDangerNone.TabStop = true;
            this.optDangerNone.Text = "None";
            // 
            // txtDangerAudioFile
            // 
            this.txtDangerAudioFile.Location = new System.Drawing.Point(88, 55);
            this.txtDangerAudioFile.Name = "txtDangerAudioFile";
            this.txtDangerAudioFile.Size = new System.Drawing.Size(152, 20);
            this.txtDangerAudioFile.TabIndex = 19;
            // 
            // txtDangerPrefix
            // 
            this.txtDangerPrefix.Location = new System.Drawing.Point(88, 11);
            this.txtDangerPrefix.MaxLength = 5;
            this.txtDangerPrefix.Name = "txtDangerPrefix";
            this.txtDangerPrefix.Size = new System.Drawing.Size(32, 20);
            this.txtDangerPrefix.TabIndex = 13;
            this.txtDangerPrefix.Text = "[D]";
            // 
            // lblDangerPrefix
            // 
            this.lblDangerPrefix.Location = new System.Drawing.Point(8, 14);
            this.lblDangerPrefix.Name = "lblDangerPrefix";
            this.lblDangerPrefix.Size = new System.Drawing.Size(79, 16);
            this.lblDangerPrefix.TabIndex = 12;
            this.lblDangerPrefix.Text = "Prefix/Suffix:";
            // 
            // chkDangerMatchFull
            // 
            this.chkDangerMatchFull.Location = new System.Drawing.Point(136, 11);
            this.chkDangerMatchFull.Name = "chkDangerMatchFull";
            this.chkDangerMatchFull.Size = new System.Drawing.Size(104, 24);
            this.chkDangerMatchFull.TabIndex = 14;
            this.chkDangerMatchFull.Text = "Match Full Text";
            // 
            // grpAlert
            // 
            this.grpAlert.Controls.Add(this.optAlertPlay);
            this.grpAlert.Controls.Add(this.optAlertSpeak);
            this.grpAlert.Controls.Add(this.optAlertBeep);
            this.grpAlert.Controls.Add(this.optAlertNone);
            this.grpAlert.Controls.Add(this.txtAlertAudioFile);
            this.grpAlert.Controls.Add(this.txtAlertPrefix);
            this.grpAlert.Controls.Add(this.lblAlertPrefix);
            this.grpAlert.Controls.Add(this.chkAlertMatchFull);
            this.grpAlert.Location = new System.Drawing.Point(4, 293);
            this.grpAlert.Name = "grpAlert";
            this.grpAlert.Size = new System.Drawing.Size(248, 78);
            this.grpAlert.TabIndex = 20;
            this.grpAlert.TabStop = false;
            this.grpAlert.Text = "Rare";
            // 
            // optAlertPlay
            // 
            this.optAlertPlay.Location = new System.Drawing.Point(8, 55);
            this.optAlertPlay.Name = "optAlertPlay";
            this.optAlertPlay.Size = new System.Drawing.Size(80, 16);
            this.optAlertPlay.TabIndex = 27;
            this.optAlertPlay.Text = "Play Wav";
            // 
            // optAlertSpeak
            // 
            this.optAlertSpeak.Location = new System.Drawing.Point(168, 35);
            this.optAlertSpeak.Name = "optAlertSpeak";
            this.optAlertSpeak.Size = new System.Drawing.Size(72, 16);
            this.optAlertSpeak.TabIndex = 26;
            this.optAlertSpeak.Text = "Speak";
            // 
            // optAlertBeep
            // 
            this.optAlertBeep.Location = new System.Drawing.Point(88, 35);
            this.optAlertBeep.Name = "optAlertBeep";
            this.optAlertBeep.Size = new System.Drawing.Size(72, 16);
            this.optAlertBeep.TabIndex = 25;
            this.optAlertBeep.Text = "Beep";
            // 
            // optAlertNone
            // 
            this.optAlertNone.Checked = true;
            this.optAlertNone.Location = new System.Drawing.Point(8, 35);
            this.optAlertNone.Name = "optAlertNone";
            this.optAlertNone.Size = new System.Drawing.Size(72, 16);
            this.optAlertNone.TabIndex = 24;
            this.optAlertNone.TabStop = true;
            this.optAlertNone.Text = "None";
            // 
            // txtAlertAudioFile
            // 
            this.txtAlertAudioFile.Location = new System.Drawing.Point(88, 55);
            this.txtAlertAudioFile.Name = "txtAlertAudioFile";
            this.txtAlertAudioFile.Size = new System.Drawing.Size(152, 20);
            this.txtAlertAudioFile.TabIndex = 28;
            // 
            // txtAlertPrefix
            // 
            this.txtAlertPrefix.Location = new System.Drawing.Point(88, 11);
            this.txtAlertPrefix.MaxLength = 5;
            this.txtAlertPrefix.Name = "txtAlertPrefix";
            this.txtAlertPrefix.Size = new System.Drawing.Size(32, 20);
            this.txtAlertPrefix.TabIndex = 22;
            this.txtAlertPrefix.Text = "[R]";
            // 
            // lblAlertPrefix
            // 
            this.lblAlertPrefix.Location = new System.Drawing.Point(8, 14);
            this.lblAlertPrefix.Name = "lblAlertPrefix";
            this.lblAlertPrefix.Size = new System.Drawing.Size(79, 16);
            this.lblAlertPrefix.TabIndex = 21;
            this.lblAlertPrefix.Text = "Prefix/Suffix:";
            // 
            // chkAlertMatchFull
            // 
            this.chkAlertMatchFull.Location = new System.Drawing.Point(136, 11);
            this.chkAlertMatchFull.Name = "chkAlertMatchFull";
            this.chkAlertMatchFull.Size = new System.Drawing.Size(104, 24);
            this.chkAlertMatchFull.TabIndex = 23;
            this.chkAlertMatchFull.Text = "Match Full Text";
            // 
            // grpCaution
            // 
            this.grpCaution.Controls.Add(this.optCautionPlay);
            this.grpCaution.Controls.Add(this.optCautionSpeak);
            this.grpCaution.Controls.Add(this.optCautionBeep);
            this.grpCaution.Controls.Add(this.optCautionNone);
            this.grpCaution.Controls.Add(this.txtCautionAudioFile);
            this.grpCaution.Controls.Add(this.txtCautionPrefix);
            this.grpCaution.Controls.Add(this.lblCautionPrefix);
            this.grpCaution.Controls.Add(this.chkCautionMatchFull);
            this.grpCaution.Location = new System.Drawing.Point(3, 125);
            this.grpCaution.Name = "grpCaution";
            this.grpCaution.Size = new System.Drawing.Size(248, 78);
            this.grpCaution.TabIndex = 11;
            this.grpCaution.TabStop = false;
            this.grpCaution.Text = "Caution";
            // 
            // optCautionPlay
            // 
            this.optCautionPlay.Location = new System.Drawing.Point(8, 55);
            this.optCautionPlay.Name = "optCautionPlay";
            this.optCautionPlay.Size = new System.Drawing.Size(80, 16);
            this.optCautionPlay.TabIndex = 18;
            this.optCautionPlay.Text = "Play Wav";
            // 
            // optCautionSpeak
            // 
            this.optCautionSpeak.Location = new System.Drawing.Point(168, 38);
            this.optCautionSpeak.Name = "optCautionSpeak";
            this.optCautionSpeak.Size = new System.Drawing.Size(72, 16);
            this.optCautionSpeak.TabIndex = 17;
            this.optCautionSpeak.Text = "Speak";
            // 
            // optCautionBeep
            // 
            this.optCautionBeep.Location = new System.Drawing.Point(88, 35);
            this.optCautionBeep.Name = "optCautionBeep";
            this.optCautionBeep.Size = new System.Drawing.Size(72, 16);
            this.optCautionBeep.TabIndex = 16;
            this.optCautionBeep.Text = "Beep";
            // 
            // optCautionNone
            // 
            this.optCautionNone.Checked = true;
            this.optCautionNone.Location = new System.Drawing.Point(8, 35);
            this.optCautionNone.Name = "optCautionNone";
            this.optCautionNone.Size = new System.Drawing.Size(72, 16);
            this.optCautionNone.TabIndex = 15;
            this.optCautionNone.TabStop = true;
            this.optCautionNone.Text = "None";
            // 
            // txtCautionAudioFile
            // 
            this.txtCautionAudioFile.Location = new System.Drawing.Point(88, 55);
            this.txtCautionAudioFile.Name = "txtCautionAudioFile";
            this.txtCautionAudioFile.Size = new System.Drawing.Size(152, 20);
            this.txtCautionAudioFile.TabIndex = 19;
            // 
            // txtCautionPrefix
            // 
            this.txtCautionPrefix.Location = new System.Drawing.Point(88, 11);
            this.txtCautionPrefix.MaxLength = 5;
            this.txtCautionPrefix.Name = "txtCautionPrefix";
            this.txtCautionPrefix.Size = new System.Drawing.Size(32, 20);
            this.txtCautionPrefix.TabIndex = 13;
            this.txtCautionPrefix.Text = "[C]";
            // 
            // lblCautionPrefix
            // 
            this.lblCautionPrefix.Location = new System.Drawing.Point(8, 14);
            this.lblCautionPrefix.Name = "lblCautionPrefix";
            this.lblCautionPrefix.Size = new System.Drawing.Size(80, 16);
            this.lblCautionPrefix.TabIndex = 12;
            this.lblCautionPrefix.Text = "Prefix/Suffix:";
            // 
            // chkCautionMatchFull
            // 
            this.chkCautionMatchFull.Location = new System.Drawing.Point(136, 11);
            this.chkCautionMatchFull.Name = "chkCautionMatchFull";
            this.chkCautionMatchFull.Size = new System.Drawing.Size(104, 24);
            this.chkCautionMatchFull.TabIndex = 14;
            this.chkCautionMatchFull.Text = "Match Full Text";
            // 
            // grpHunt
            // 
            this.grpHunt.Controls.Add(this.optHuntPlay);
            this.grpHunt.Controls.Add(this.optHuntSpeak);
            this.grpHunt.Controls.Add(this.optHuntBeep);
            this.grpHunt.Controls.Add(this.optHuntNone);
            this.grpHunt.Controls.Add(this.txtHuntAudioFile);
            this.grpHunt.Controls.Add(this.txtHuntPrefix);
            this.grpHunt.Controls.Add(this.lblHuntPrefix);
            this.grpHunt.Controls.Add(this.chkHuntMatchFull);
            this.grpHunt.Location = new System.Drawing.Point(3, 41);
            this.grpHunt.Name = "grpHunt";
            this.grpHunt.Size = new System.Drawing.Size(248, 78);
            this.grpHunt.TabIndex = 2;
            this.grpHunt.TabStop = false;
            this.grpHunt.Text = "Hunt";
            // 
            // optHuntPlay
            // 
            this.optHuntPlay.Location = new System.Drawing.Point(8, 55);
            this.optHuntPlay.Name = "optHuntPlay";
            this.optHuntPlay.Size = new System.Drawing.Size(80, 16);
            this.optHuntPlay.TabIndex = 9;
            this.optHuntPlay.Text = "Play Wav";
            // 
            // optHuntSpeak
            // 
            this.optHuntSpeak.Location = new System.Drawing.Point(168, 35);
            this.optHuntSpeak.Name = "optHuntSpeak";
            this.optHuntSpeak.Size = new System.Drawing.Size(72, 16);
            this.optHuntSpeak.TabIndex = 8;
            this.optHuntSpeak.Text = "Speak";
            // 
            // optHuntBeep
            // 
            this.optHuntBeep.Location = new System.Drawing.Point(88, 35);
            this.optHuntBeep.Name = "optHuntBeep";
            this.optHuntBeep.Size = new System.Drawing.Size(72, 16);
            this.optHuntBeep.TabIndex = 7;
            this.optHuntBeep.Text = "Beep";
            // 
            // optHuntNone
            // 
            this.optHuntNone.Checked = true;
            this.optHuntNone.Location = new System.Drawing.Point(8, 35);
            this.optHuntNone.Name = "optHuntNone";
            this.optHuntNone.Size = new System.Drawing.Size(72, 16);
            this.optHuntNone.TabIndex = 6;
            this.optHuntNone.TabStop = true;
            this.optHuntNone.Text = "None";
            // 
            // txtHuntAudioFile
            // 
            this.txtHuntAudioFile.Location = new System.Drawing.Point(88, 55);
            this.txtHuntAudioFile.Name = "txtHuntAudioFile";
            this.txtHuntAudioFile.Size = new System.Drawing.Size(152, 20);
            this.txtHuntAudioFile.TabIndex = 10;
            // 
            // txtHuntPrefix
            // 
            this.txtHuntPrefix.Location = new System.Drawing.Point(88, 11);
            this.txtHuntPrefix.MaxLength = 5;
            this.txtHuntPrefix.Name = "txtHuntPrefix";
            this.txtHuntPrefix.Size = new System.Drawing.Size(32, 20);
            this.txtHuntPrefix.TabIndex = 4;
            this.txtHuntPrefix.Text = "[H]";
            // 
            // lblHuntPrefix
            // 
            this.lblHuntPrefix.Location = new System.Drawing.Point(8, 14);
            this.lblHuntPrefix.Name = "lblHuntPrefix";
            this.lblHuntPrefix.Size = new System.Drawing.Size(80, 16);
            this.lblHuntPrefix.TabIndex = 3;
            this.lblHuntPrefix.Text = "Prefix/Suffix:";
            // 
            // chkHuntMatchFull
            // 
            this.chkHuntMatchFull.Location = new System.Drawing.Point(136, 11);
            this.chkHuntMatchFull.Name = "chkHuntMatchFull";
            this.chkHuntMatchFull.Size = new System.Drawing.Size(104, 24);
            this.chkHuntMatchFull.TabIndex = 5;
            this.chkHuntMatchFull.Text = "Match Full Text";
            // 
            // lblFadedLines
            // 
            this.lblFadedLines.AutoSize = true;
            this.lblFadedLines.Location = new System.Drawing.Point(6, 99);
            this.lblFadedLines.Name = "lblFadedLines";
            this.lblFadedLines.Size = new System.Drawing.Size(139, 13);
            this.lblFadedLines.TabIndex = 60;
            this.lblFadedLines.Text = "Dynamic Alpha Faded Lines";
            // 
            // lblPVPLevels
            // 
            this.lblPVPLevels.Location = new System.Drawing.Point(6, 76);
            this.lblPVPLevels.Name = "lblPVPLevels";
            this.lblPVPLevels.Size = new System.Drawing.Size(142, 16);
            this.lblPVPLevels.TabIndex = 58;
            this.lblPVPLevels.Text = "PVP Level Range:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmbAlertSound);
            this.groupBox1.Controls.Add(this.cmbHatch);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.chkColorRangeCircle);
            this.groupBox1.Controls.Add(this.spnRangeCircle);
            this.groupBox1.Controls.Add(this.numMinAlertLevel);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(3, 119);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(258, 116);
            this.groupBox1.TabIndex = 57;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Proximity Alert Settings";
            // 
            // cmbAlertSound
            // 
            this.cmbAlertSound.FormattingEnabled = true;
            this.cmbAlertSound.Items.AddRange(new object[] {
            "Asterisk",
            "Beep",
            "Exclamation",
            "Hand",
            "Question"});
            this.cmbAlertSound.Location = new System.Drawing.Point(104, 67);
            this.cmbAlertSound.Name = "cmbAlertSound";
            this.cmbAlertSound.Size = new System.Drawing.Size(133, 21);
            this.cmbAlertSound.TabIndex = 54;
            this.cmbAlertSound.SelectionChangeCommitted += new System.EventHandler(this.CmbAlertSound_SelectionChangeCommitted);
            // 
            // cmbHatch
            // 
            this.cmbHatch.FormattingEnabled = true;
            this.cmbHatch.Location = new System.Drawing.Point(104, 42);
            this.cmbHatch.Name = "cmbHatch";
            this.cmbHatch.Size = new System.Drawing.Size(133, 21);
            this.cmbHatch.TabIndex = 53;
            this.cmbHatch.Tag = "";
            this.cmbHatch.SelectionChangeCommitted += new System.EventHandler(this.CmbAlertSound_SelectionChangeCommitted);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(4, 70);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(103, 16);
            this.label4.TabIndex = 58;
            this.label4.Text = "Alert Sound";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(4, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 16);
            this.label2.TabIndex = 57;
            this.label2.Text = "Hatch Pattern";
            // 
            // chkColorRangeCircle
            // 
            this.chkColorRangeCircle.Location = new System.Drawing.Point(7, 20);
            this.chkColorRangeCircle.Name = "chkColorRangeCircle";
            this.chkColorRangeCircle.Size = new System.Drawing.Size(100, 16);
            this.chkColorRangeCircle.TabIndex = 5;
            this.chkColorRangeCircle.Text = "Range Circle";
            // 
            // spnRangeCircle
            // 
            this.spnRangeCircle.Location = new System.Drawing.Point(173, 19);
            this.spnRangeCircle.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.spnRangeCircle.Name = "spnRangeCircle";
            this.spnRangeCircle.Size = new System.Drawing.Size(64, 20);
            this.spnRangeCircle.TabIndex = 7;
            this.spnRangeCircle.Value = new decimal(new int[] {
            150,
            0,
            0,
            0});
            // 
            // numMinAlertLevel
            // 
            this.numMinAlertLevel.Location = new System.Drawing.Point(172, 91);
            this.numMinAlertLevel.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numMinAlertLevel.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.numMinAlertLevel.Name = "numMinAlertLevel";
            this.numMinAlertLevel.Size = new System.Drawing.Size(64, 20);
            this.numMinAlertLevel.TabIndex = 55;
            this.numMinAlertLevel.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(4, 93);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(142, 16);
            this.label1.TabIndex = 56;
            this.label1.Text = "Minimum Alert Level";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkMap);
            this.groupBox2.Controls.Add(this.chkPlayer);
            this.groupBox2.Controls.Add(this.chkSpawns);
            this.groupBox2.Controls.Add(this.chkAddjust);
            this.groupBox2.Controls.Add(this.chkGround);
            this.groupBox2.Controls.Add(this.chkTrails);
            this.groupBox2.Controls.Add(this.chkHighlight);
            this.groupBox2.Controls.Add(this.chkGrid);
            this.groupBox2.Controls.Add(this.chkTimers);
            this.groupBox2.Controls.Add(this.chkText);
            this.groupBox2.Controls.Add(this.chkDirection);
            this.groupBox2.Controls.Add(this.chkLineToPoint);
            this.groupBox2.Location = new System.Drawing.Point(3, 241);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(258, 130);
            this.groupBox2.TabIndex = 52;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Map Drawing Settings";
            // 
            // chkMap
            // 
            this.chkMap.Location = new System.Drawing.Point(142, 53);
            this.chkMap.Name = "chkMap";
            this.chkMap.Size = new System.Drawing.Size(94, 20);
            this.chkMap.TabIndex = 30;
            this.chkMap.Text = "Draw Map";
            // 
            // chkPlayer
            // 
            this.chkPlayer.Location = new System.Drawing.Point(142, 70);
            this.chkPlayer.Name = "chkPlayer";
            this.chkPlayer.Size = new System.Drawing.Size(94, 20);
            this.chkPlayer.TabIndex = 50;
            this.chkPlayer.Text = "Draw Player";
            // 
            // chkSpawns
            // 
            this.chkSpawns.Location = new System.Drawing.Point(142, 36);
            this.chkSpawns.Name = "chkSpawns";
            this.chkSpawns.Size = new System.Drawing.Size(94, 20);
            this.chkSpawns.TabIndex = 43;
            this.chkSpawns.Text = "Draw Spawns";
            // 
            // chkAddjust
            // 
            this.chkAddjust.Location = new System.Drawing.Point(6, 19);
            this.chkAddjust.Name = "chkAddjust";
            this.chkAddjust.Size = new System.Drawing.Size(121, 20);
            this.chkAddjust.TabIndex = 29;
            this.chkAddjust.Text = "Readjust Map";
            // 
            // chkGround
            // 
            this.chkGround.Location = new System.Drawing.Point(142, 104);
            this.chkGround.Name = "chkGround";
            this.chkGround.Size = new System.Drawing.Size(111, 20);
            this.chkGround.TabIndex = 51;
            this.chkGround.Text = "Ground Spawns";
            // 
            // chkTrails
            // 
            this.chkTrails.Location = new System.Drawing.Point(6, 87);
            this.chkTrails.Name = "chkTrails";
            this.chkTrails.Size = new System.Drawing.Size(130, 20);
            this.chkTrails.TabIndex = 45;
            this.chkTrails.Text = "Spawn Trails";
            // 
            // chkHighlight
            // 
            this.chkHighlight.Location = new System.Drawing.Point(6, 104);
            this.chkHighlight.Name = "chkHighlight";
            this.chkHighlight.Size = new System.Drawing.Size(130, 20);
            this.chkHighlight.TabIndex = 49;
            this.chkHighlight.Text = "Highlight Merchants";
            // 
            // chkGrid
            // 
            this.chkGrid.Location = new System.Drawing.Point(6, 36);
            this.chkGrid.Name = "chkGrid";
            this.chkGrid.Size = new System.Drawing.Size(130, 20);
            this.chkGrid.TabIndex = 37;
            this.chkGrid.Text = "Show Gridlines";
            // 
            // chkTimers
            // 
            this.chkTimers.Location = new System.Drawing.Point(142, 19);
            this.chkTimers.Name = "chkTimers";
            this.chkTimers.Size = new System.Drawing.Size(116, 20);
            this.chkTimers.TabIndex = 47;
            this.chkTimers.Text = "Spawn Timers";
            // 
            // chkText
            // 
            this.chkText.Location = new System.Drawing.Point(6, 70);
            this.chkText.Name = "chkText";
            this.chkText.Size = new System.Drawing.Size(130, 20);
            this.chkText.TabIndex = 41;
            this.chkText.Text = "Show Zone Text";
            // 
            // chkDirection
            // 
            this.chkDirection.Location = new System.Drawing.Point(142, 87);
            this.chkDirection.Name = "chkDirection";
            this.chkDirection.Size = new System.Drawing.Size(109, 20);
            this.chkDirection.TabIndex = 46;
            this.chkDirection.Text = "Heading Lines";
            // 
            // chkLineToPoint
            // 
            this.chkLineToPoint.Location = new System.Drawing.Point(6, 53);
            this.chkLineToPoint.Name = "chkLineToPoint";
            this.chkLineToPoint.Size = new System.Drawing.Size(130, 20);
            this.chkLineToPoint.TabIndex = 42;
            this.chkLineToPoint.Text = "Draw Line to Point";
            // 
            // lblSpawnSize
            // 
            this.lblSpawnSize.Location = new System.Drawing.Point(6, 51);
            this.lblSpawnSize.Name = "lblSpawnSize";
            this.lblSpawnSize.Size = new System.Drawing.Size(142, 16);
            this.lblSpawnSize.TabIndex = 20;
            this.lblSpawnSize.Text = "Spawn Draw Size:";
            // 
            // lblSearch
            // 
            this.lblSearch.CausesValidation = false;
            this.lblSearch.Location = new System.Drawing.Point(8, 347);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(47, 17);
            this.lblSearch.TabIndex = 25;
            this.lblSearch.Text = "Search:";
            // 
            // lblLogLevel
            // 
            this.lblLogLevel.Location = new System.Drawing.Point(8, 258);
            this.lblLogLevel.Name = "lblLogLevel";
            this.lblLogLevel.Size = new System.Drawing.Size(99, 16);
            this.lblLogLevel.TabIndex = 20;
            this.lblLogLevel.Text = "Error Logging Level:";
            // 
            // lblWindowName
            // 
            this.lblWindowName.Location = new System.Drawing.Point(8, 280);
            this.lblWindowName.Name = "lblWindowName";
            this.lblWindowName.Size = new System.Drawing.Size(74, 16);
            this.lblWindowName.TabIndex = 22;
            this.lblWindowName.Text = "Window Title:";
            // 
            // lblOverridelevel
            // 
            this.lblOverridelevel.Location = new System.Drawing.Point(8, 210);
            this.lblOverridelevel.Name = "lblOverridelevel";
            this.lblOverridelevel.Size = new System.Drawing.Size(99, 16);
            this.lblOverridelevel.TabIndex = 14;
            this.lblOverridelevel.Text = "Override Level:";
            // 
            // gbServer
            // 
            this.gbServer.Controls.Add(this.txtIPAddress5);
            this.gbServer.Controls.Add(this.lblIPAddress5);
            this.gbServer.Controls.Add(this.txtIPAddress4);
            this.gbServer.Controls.Add(this.lblIPAddress4);
            this.gbServer.Controls.Add(this.txtIPAddress3);
            this.gbServer.Controls.Add(this.lblIPAddress3);
            this.gbServer.Controls.Add(this.txtIPAddress2);
            this.gbServer.Controls.Add(this.lblIPAddress2);
            this.gbServer.Controls.Add(this.txtPortNo);
            this.gbServer.Controls.Add(this.txtIPAddress1);
            this.gbServer.Controls.Add(this.lblIPAddress1);
            this.gbServer.Controls.Add(this.lbltxtPortNo);
            this.gbServer.Location = new System.Drawing.Point(8, 8);
            this.gbServer.Name = "gbServer";
            this.gbServer.Size = new System.Drawing.Size(248, 168);
            this.gbServer.TabIndex = 0;
            this.gbServer.TabStop = false;
            this.gbServer.Text = "Server";
            // 
            // txtIPAddress5
            // 
            this.txtIPAddress5.Location = new System.Drawing.Point(128, 112);
            this.txtIPAddress5.Name = "txtIPAddress5";
            this.txtIPAddress5.Size = new System.Drawing.Size(112, 20);
            this.txtIPAddress5.TabIndex = 10;
            // 
            // lblIPAddress5
            // 
            this.lblIPAddress5.Location = new System.Drawing.Point(8, 112);
            this.lblIPAddress5.Name = "lblIPAddress5";
            this.lblIPAddress5.Size = new System.Drawing.Size(120, 16);
            this.lblIPAddress5.TabIndex = 9;
            this.lblIPAddress5.Text = "IP Address 5: (Ctrl + 5)";
            // 
            // txtIPAddress4
            // 
            this.txtIPAddress4.Location = new System.Drawing.Point(128, 88);
            this.txtIPAddress4.Name = "txtIPAddress4";
            this.txtIPAddress4.Size = new System.Drawing.Size(112, 20);
            this.txtIPAddress4.TabIndex = 8;
            // 
            // lblIPAddress4
            // 
            this.lblIPAddress4.Location = new System.Drawing.Point(8, 88);
            this.lblIPAddress4.Name = "lblIPAddress4";
            this.lblIPAddress4.Size = new System.Drawing.Size(120, 16);
            this.lblIPAddress4.TabIndex = 7;
            this.lblIPAddress4.Text = "IP Address 4: (Ctrl + 4)";
            // 
            // txtIPAddress3
            // 
            this.txtIPAddress3.Location = new System.Drawing.Point(128, 64);
            this.txtIPAddress3.Name = "txtIPAddress3";
            this.txtIPAddress3.Size = new System.Drawing.Size(112, 20);
            this.txtIPAddress3.TabIndex = 6;
            // 
            // lblIPAddress3
            // 
            this.lblIPAddress3.Location = new System.Drawing.Point(8, 64);
            this.lblIPAddress3.Name = "lblIPAddress3";
            this.lblIPAddress3.Size = new System.Drawing.Size(120, 16);
            this.lblIPAddress3.TabIndex = 5;
            this.lblIPAddress3.Text = "IP Address 3: (Ctrl + 3)";
            // 
            // txtIPAddress2
            // 
            this.txtIPAddress2.Location = new System.Drawing.Point(128, 40);
            this.txtIPAddress2.Name = "txtIPAddress2";
            this.txtIPAddress2.Size = new System.Drawing.Size(112, 20);
            this.txtIPAddress2.TabIndex = 4;
            // 
            // lblIPAddress2
            // 
            this.lblIPAddress2.Location = new System.Drawing.Point(8, 40);
            this.lblIPAddress2.Name = "lblIPAddress2";
            this.lblIPAddress2.Size = new System.Drawing.Size(120, 16);
            this.lblIPAddress2.TabIndex = 3;
            this.lblIPAddress2.Text = "IP Address 2: (Ctrl + 2)";
            // 
            // txtPortNo
            // 
            this.txtPortNo.Location = new System.Drawing.Point(128, 136);
            this.txtPortNo.Name = "txtPortNo";
            this.txtPortNo.Size = new System.Drawing.Size(112, 20);
            this.txtPortNo.TabIndex = 12;
            this.txtPortNo.Text = "5555";
            // 
            // txtIPAddress1
            // 
            this.txtIPAddress1.Location = new System.Drawing.Point(128, 16);
            this.txtIPAddress1.Name = "txtIPAddress1";
            this.txtIPAddress1.Size = new System.Drawing.Size(112, 20);
            this.txtIPAddress1.TabIndex = 2;
            this.txtIPAddress1.Text = "localhost";
            // 
            // lblIPAddress1
            // 
            this.lblIPAddress1.Location = new System.Drawing.Point(8, 16);
            this.lblIPAddress1.Name = "lblIPAddress1";
            this.lblIPAddress1.Size = new System.Drawing.Size(120, 16);
            this.lblIPAddress1.TabIndex = 1;
            this.lblIPAddress1.Text = "IP Address 1: (Ctrl + 1)";
            // 
            // lbltxtPortNo
            // 
            this.lbltxtPortNo.Location = new System.Drawing.Point(8, 136);
            this.lbltxtPortNo.Name = "lbltxtPortNo";
            this.lbltxtPortNo.Size = new System.Drawing.Size(120, 16);
            this.lbltxtPortNo.TabIndex = 11;
            this.lbltxtPortNo.Text = "Port:";
            // 
            // lblUpdateDelay
            // 
            this.lblUpdateDelay.Location = new System.Drawing.Point(8, 234);
            this.lblUpdateDelay.Name = "lblUpdateDelay";
            this.lblUpdateDelay.Size = new System.Drawing.Size(99, 15);
            this.lblUpdateDelay.TabIndex = 16;
            this.lblUpdateDelay.Text = "Update Delay (mS):";
            // 
            // tabOptions
            // 
            this.tabOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabOptions.Controls.Add(this.tabGeneral);
            this.tabOptions.Controls.Add(this.tabMap);
            this.tabOptions.Controls.Add(this.tabAlerts);
            this.tabOptions.Controls.Add(this.tabFolders);
            this.tabOptions.Controls.Add(this.tabColors);
            this.tabOptions.Location = new System.Drawing.Point(0, 3);
            this.tabOptions.Name = "tabOptions";
            this.tabOptions.SelectedIndex = 0;
            this.tabOptions.Size = new System.Drawing.Size(274, 404);
            this.tabOptions.TabIndex = 1;
            // 
            // tabGeneral
            // 
            this.tabGeneral.Controls.Add(this.chkShowCharName);
            this.tabGeneral.Controls.Add(this.txtSearchString);
            this.tabGeneral.Controls.Add(this.lblSearch);
            this.tabGeneral.Controls.Add(this.spnLogLevel);
            this.tabGeneral.Controls.Add(this.lblLogLevel);
            this.tabGeneral.Controls.Add(this.chkShowZoneName);
            this.tabGeneral.Controls.Add(this.spnOverrideLevel);
            this.tabGeneral.Controls.Add(this.spnUpdateDelay);
            this.tabGeneral.Controls.Add(this.txtWindowName);
            this.tabGeneral.Controls.Add(this.lblWindowName);
            this.tabGeneral.Controls.Add(this.lblOverridelevel);
            this.tabGeneral.Controls.Add(this.gbServer);
            this.tabGeneral.Controls.Add(this.lblUpdateDelay);
            this.tabGeneral.Controls.Add(this.chkSaveOnExit);
            this.tabGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Size = new System.Drawing.Size(266, 378);
            this.tabGeneral.TabIndex = 0;
            this.tabGeneral.Text = "General";
            // 
            // chkShowCharName
            // 
            this.chkShowCharName.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkShowCharName.Checked = true;
            this.chkShowCharName.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowCharName.Location = new System.Drawing.Point(134, 320);
            this.chkShowCharName.Name = "chkShowCharName";
            this.chkShowCharName.Size = new System.Drawing.Size(114, 24);
            this.chkShowCharName.TabIndex = 27;
            this.chkShowCharName.Text = "Show Char Name";
            this.chkShowCharName.CheckedChanged += new System.EventHandler(this.ChkShowCharName_CheckedChanged);
            // 
            // txtSearchString
            // 
            this.txtSearchString.Location = new System.Drawing.Point(61, 344);
            this.txtSearchString.Name = "txtSearchString";
            this.txtSearchString.Size = new System.Drawing.Size(195, 20);
            this.txtSearchString.TabIndex = 26;
            // 
            // spnLogLevel
            // 
            this.spnLogLevel.Location = new System.Drawing.Point(192, 256);
            this.spnLogLevel.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.spnLogLevel.Name = "spnLogLevel";
            this.spnLogLevel.Size = new System.Drawing.Size(64, 20);
            this.spnLogLevel.TabIndex = 21;
            // 
            // chkShowZoneName
            // 
            this.chkShowZoneName.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkShowZoneName.Checked = true;
            this.chkShowZoneName.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowZoneName.Location = new System.Drawing.Point(8, 320);
            this.chkShowZoneName.Name = "chkShowZoneName";
            this.chkShowZoneName.Size = new System.Drawing.Size(114, 24);
            this.chkShowZoneName.TabIndex = 24;
            this.chkShowZoneName.Text = "Show Zone Name";
            this.chkShowZoneName.CheckedChanged += new System.EventHandler(this.ChkShowZoneName_CheckedChanged);
            // 
            // spnOverrideLevel
            // 
            this.spnOverrideLevel.Location = new System.Drawing.Point(192, 208);
            this.spnOverrideLevel.Maximum = new decimal(new int[] {
            120,
            0,
            0,
            0});
            this.spnOverrideLevel.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.spnOverrideLevel.Name = "spnOverrideLevel";
            this.spnOverrideLevel.Size = new System.Drawing.Size(64, 20);
            this.spnOverrideLevel.TabIndex = 15;
            this.spnOverrideLevel.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // spnUpdateDelay
            // 
            this.spnUpdateDelay.Location = new System.Drawing.Point(192, 232);
            this.spnUpdateDelay.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.spnUpdateDelay.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.spnUpdateDelay.Name = "spnUpdateDelay";
            this.spnUpdateDelay.Size = new System.Drawing.Size(64, 20);
            this.spnUpdateDelay.TabIndex = 17;
            this.spnUpdateDelay.Value = new decimal(new int[] {
            250,
            0,
            0,
            0});
            // 
            // txtWindowName
            // 
            this.txtWindowName.Location = new System.Drawing.Point(8, 296);
            this.txtWindowName.Name = "txtWindowName";
            this.txtWindowName.Size = new System.Drawing.Size(248, 20);
            this.txtWindowName.TabIndex = 23;
            // 
            // chkSaveOnExit
            // 
            this.chkSaveOnExit.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkSaveOnExit.Checked = true;
            this.chkSaveOnExit.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSaveOnExit.Location = new System.Drawing.Point(8, 178);
            this.chkSaveOnExit.Name = "chkSaveOnExit";
            this.chkSaveOnExit.Size = new System.Drawing.Size(248, 24);
            this.chkSaveOnExit.TabIndex = 13;
            this.chkSaveOnExit.Text = "Save Preferences On Exit:";
            this.chkSaveOnExit.CheckedChanged += new System.EventHandler(this.ChkSaveOnExit_CheckedChanged);
            // 
            // tabMap
            // 
            this.tabMap.Controls.Add(this.FadedLines);
            this.tabMap.Controls.Add(this.lblFadedLines);
            this.tabMap.Controls.Add(this.lblPVPLevels);
            this.tabMap.Controls.Add(this.pvpLevels);
            this.tabMap.Controls.Add(this.groupBox1);
            this.tabMap.Controls.Add(this.groupBox2);
            this.tabMap.Controls.Add(this.lblSpawnSize);
            this.tabMap.Controls.Add(this.chkSelectSpawnList);
            this.tabMap.Controls.Add(this.spnSpawnSize);
            this.tabMap.Controls.Add(this.chkShowTargetInfo);
            this.tabMap.Controls.Add(this.chkDrawFoV);
            this.tabMap.Location = new System.Drawing.Point(4, 22);
            this.tabMap.Name = "tabMap";
            this.tabMap.Size = new System.Drawing.Size(266, 378);
            this.tabMap.TabIndex = 2;
            this.tabMap.Text = "Map";
            // 
            // FadedLines
            // 
            this.FadedLines.Location = new System.Drawing.Point(175, 99);
            this.FadedLines.Name = "FadedLines";
            this.FadedLines.Size = new System.Drawing.Size(64, 20);
            this.FadedLines.TabIndex = 61;
            this.FadedLines.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // pvpLevels
            // 
            this.pvpLevels.Location = new System.Drawing.Point(175, 74);
            this.pvpLevels.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.pvpLevels.Name = "pvpLevels";
            this.pvpLevels.Size = new System.Drawing.Size(64, 20);
            this.pvpLevels.TabIndex = 59;
            this.pvpLevels.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.pvpLevels.ValueChanged += new System.EventHandler(this.PvpLevels_ValueChanged);
            // 
            // chkSelectSpawnList
            // 
            this.chkSelectSpawnList.Location = new System.Drawing.Point(8, 32);
            this.chkSelectSpawnList.Name = "chkSelectSpawnList";
            this.chkSelectSpawnList.Size = new System.Drawing.Size(231, 16);
            this.chkSelectSpawnList.TabIndex = 4;
            this.chkSelectSpawnList.Text = "Auto Select Spawn in the Spawn List";
            // 
            // spnSpawnSize
            // 
            this.spnSpawnSize.Location = new System.Drawing.Point(175, 49);
            this.spnSpawnSize.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.spnSpawnSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spnSpawnSize.Name = "spnSpawnSize";
            this.spnSpawnSize.Size = new System.Drawing.Size(64, 20);
            this.spnSpawnSize.TabIndex = 21;
            this.spnSpawnSize.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.spnSpawnSize.ValueChanged += new System.EventHandler(this.SpnSpawnSize_ValueChanged);
            // 
            // chkShowTargetInfo
            // 
            this.chkShowTargetInfo.Location = new System.Drawing.Point(8, 12);
            this.chkShowTargetInfo.Name = "chkShowTargetInfo";
            this.chkShowTargetInfo.Size = new System.Drawing.Size(231, 16);
            this.chkShowTargetInfo.TabIndex = 3;
            this.chkShowTargetInfo.Text = "Show Target Information Window";
            // 
            // chkDrawFoV
            // 
            this.chkDrawFoV.Location = new System.Drawing.Point(8, -24);
            this.chkDrawFoV.Name = "chkDrawFoV";
            this.chkDrawFoV.Size = new System.Drawing.Size(248, 16);
            this.chkDrawFoV.TabIndex = 2;
            this.chkDrawFoV.Text = "Draw Field of View (FoV)";
            // 
            // tabAlerts
            // 
            this.tabAlerts.Controls.Add(this.grpDanger);
            this.tabAlerts.Controls.Add(this.grpAlert);
            this.tabAlerts.Controls.Add(this.grpCaution);
            this.tabAlerts.Controls.Add(this.grpHunt);
            this.tabAlerts.Controls.Add(this.chkAffixAlerts);
            this.tabAlerts.Controls.Add(this.chkPrefixAlerts);
            this.tabAlerts.Controls.Add(this.chkCorpsesAlerts);
            this.tabAlerts.Location = new System.Drawing.Point(4, 22);
            this.tabAlerts.Name = "tabAlerts";
            this.tabAlerts.Size = new System.Drawing.Size(266, 378);
            this.tabAlerts.TabIndex = 1;
            this.tabAlerts.Text = "Filters";
            // 
            // chkAffixAlerts
            // 
            this.chkAffixAlerts.Checked = true;
            this.chkAffixAlerts.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAffixAlerts.Location = new System.Drawing.Point(4, 22);
            this.chkAffixAlerts.Name = "chkAffixAlerts";
            this.chkAffixAlerts.Size = new System.Drawing.Size(112, 24);
            this.chkAffixAlerts.TabIndex = 1;
            this.chkAffixAlerts.Text = "Attach Suffix Text";
            // 
            // chkPrefixAlerts
            // 
            this.chkPrefixAlerts.Checked = true;
            this.chkPrefixAlerts.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPrefixAlerts.Location = new System.Drawing.Point(4, 3);
            this.chkPrefixAlerts.Name = "chkPrefixAlerts";
            this.chkPrefixAlerts.Size = new System.Drawing.Size(120, 24);
            this.chkPrefixAlerts.TabIndex = 0;
            this.chkPrefixAlerts.Text = "Attach Prefix Text";
            // 
            // chkCorpsesAlerts
            // 
            this.chkCorpsesAlerts.Checked = true;
            this.chkCorpsesAlerts.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCorpsesAlerts.Location = new System.Drawing.Point(152, 3);
            this.chkCorpsesAlerts.Name = "chkCorpsesAlerts";
            this.chkCorpsesAlerts.Size = new System.Drawing.Size(100, 24);
            this.chkCorpsesAlerts.TabIndex = 24;
            this.chkCorpsesAlerts.Text = "Match Corpses";
            // 
            // tabFolders
            // 
            this.tabFolders.Controls.Add(this.cmdSpawnTimers);
            this.tabFolders.Controls.Add(this.txtTimerDir);
            this.tabFolders.Controls.Add(this.txtLogDir);
            this.tabFolders.Controls.Add(this.txtFilterDir);
            this.tabFolders.Controls.Add(this.txtCfgDir);
            this.tabFolders.Controls.Add(this.txtMapDir);
            this.tabFolders.Controls.Add(this.lblSpawnTimers);
            this.tabFolders.Controls.Add(this.cmdLogDir);
            this.tabFolders.Controls.Add(this.lblLogDir);
            this.tabFolders.Controls.Add(this.cmdFilterDirBrowse);
            this.tabFolders.Controls.Add(this.lblFilterDir);
            this.tabFolders.Controls.Add(this.cmdCfgDirBrowse);
            this.tabFolders.Controls.Add(this.lblCfgDir);
            this.tabFolders.Controls.Add(this.cmdMapDirBrowse);
            this.tabFolders.Controls.Add(this.lblMapDir);
            this.tabFolders.Location = new System.Drawing.Point(4, 22);
            this.tabFolders.Name = "tabFolders";
            this.tabFolders.Size = new System.Drawing.Size(266, 378);
            this.tabFolders.TabIndex = 4;
            this.tabFolders.Text = "Folders";
            // 
            // cmdSpawnTimers
            // 
            this.cmdSpawnTimers.Location = new System.Drawing.Point(232, 184);
            this.cmdSpawnTimers.Name = "cmdSpawnTimers";
            this.cmdSpawnTimers.Size = new System.Drawing.Size(24, 23);
            this.cmdSpawnTimers.TabIndex = 40;
            this.cmdSpawnTimers.Text = "...";
            this.cmdSpawnTimers.Click += new System.EventHandler(this.CmdSpawnTimers_Click);
            // 
            // txtTimerDir
            // 
            this.txtTimerDir.BackColor = System.Drawing.Color.White;
            this.txtTimerDir.Location = new System.Drawing.Point(8, 184);
            this.txtTimerDir.Name = "txtTimerDir";
            this.txtTimerDir.Size = new System.Drawing.Size(216, 20);
            this.txtTimerDir.TabIndex = 39;
            // 
            // txtLogDir
            // 
            this.txtLogDir.BackColor = System.Drawing.Color.White;
            this.txtLogDir.Location = new System.Drawing.Point(8, 144);
            this.txtLogDir.Name = "txtLogDir";
            this.txtLogDir.Size = new System.Drawing.Size(216, 20);
            this.txtLogDir.TabIndex = 36;
            // 
            // txtFilterDir
            // 
            this.txtFilterDir.BackColor = System.Drawing.Color.White;
            this.txtFilterDir.Location = new System.Drawing.Point(8, 104);
            this.txtFilterDir.Name = "txtFilterDir";
            this.txtFilterDir.Size = new System.Drawing.Size(216, 20);
            this.txtFilterDir.TabIndex = 33;
            // 
            // txtCfgDir
            // 
            this.txtCfgDir.BackColor = System.Drawing.Color.White;
            this.txtCfgDir.Location = new System.Drawing.Point(8, 64);
            this.txtCfgDir.Name = "txtCfgDir";
            this.txtCfgDir.Size = new System.Drawing.Size(216, 20);
            this.txtCfgDir.TabIndex = 30;
            // 
            // txtMapDir
            // 
            this.txtMapDir.BackColor = System.Drawing.Color.White;
            this.txtMapDir.Location = new System.Drawing.Point(8, 24);
            this.txtMapDir.Name = "txtMapDir";
            this.txtMapDir.Size = new System.Drawing.Size(216, 20);
            this.txtMapDir.TabIndex = 27;
            // 
            // cmdLogDir
            // 
            this.cmdLogDir.Location = new System.Drawing.Point(232, 144);
            this.cmdLogDir.Name = "cmdLogDir";
            this.cmdLogDir.Size = new System.Drawing.Size(24, 23);
            this.cmdLogDir.TabIndex = 37;
            this.cmdLogDir.Text = "...";
            this.cmdLogDir.Click += new System.EventHandler(this.CmdLogDir_Click);
            // 
            // cmdFilterDirBrowse
            // 
            this.cmdFilterDirBrowse.Location = new System.Drawing.Point(232, 104);
            this.cmdFilterDirBrowse.Name = "cmdFilterDirBrowse";
            this.cmdFilterDirBrowse.Size = new System.Drawing.Size(24, 23);
            this.cmdFilterDirBrowse.TabIndex = 34;
            this.cmdFilterDirBrowse.Text = "...";
            this.cmdFilterDirBrowse.Click += new System.EventHandler(this.CmdFilterDirBrowse_Click);
            // 
            // cmdCfgDirBrowse
            // 
            this.cmdCfgDirBrowse.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCfgDirBrowse.Location = new System.Drawing.Point(232, 64);
            this.cmdCfgDirBrowse.Name = "cmdCfgDirBrowse";
            this.cmdCfgDirBrowse.Size = new System.Drawing.Size(24, 23);
            this.cmdCfgDirBrowse.TabIndex = 31;
            this.cmdCfgDirBrowse.Text = "...";
            this.cmdCfgDirBrowse.Click += new System.EventHandler(this.CmdCfgDirBrowse_Click);
            // 
            // cmdMapDirBrowse
            // 
            this.cmdMapDirBrowse.Location = new System.Drawing.Point(232, 24);
            this.cmdMapDirBrowse.Name = "cmdMapDirBrowse";
            this.cmdMapDirBrowse.Size = new System.Drawing.Size(24, 23);
            this.cmdMapDirBrowse.TabIndex = 28;
            this.cmdMapDirBrowse.Text = "...";
            this.cmdMapDirBrowse.Click += new System.EventHandler(this.CmdMapDirBrowse_Click);
            // 
            // tabColors
            // 
            this.tabColors.Controls.Add(this.picGridLabelColor);
            this.tabColors.Controls.Add(this.cmdGridLabelColor);
            this.tabColors.Controls.Add(this.picPlayerBorder);
            this.tabColors.Controls.Add(this.butPlayerBorder);
            this.tabColors.Controls.Add(this.picListBackgroundColor);
            this.tabColors.Controls.Add(this.cmdListBackgroundColor);
            this.tabColors.Controls.Add(this.picRangeCircleColor);
            this.tabColors.Controls.Add(this.cmdRangeCircleColor);
            this.tabColors.Controls.Add(this.picGridColor);
            this.tabColors.Controls.Add(this.cmdGridColor);
            this.tabColors.Controls.Add(this.picMapBackgroundColor);
            this.tabColors.Controls.Add(this.cmdMapBackgroundColor);
            this.tabColors.Location = new System.Drawing.Point(4, 22);
            this.tabColors.Name = "tabColors";
            this.tabColors.Size = new System.Drawing.Size(266, 378);
            this.tabColors.TabIndex = 3;
            this.tabColors.Text = "Colors";
            // 
            // picGridLabelColor
            // 
            this.picGridLabelColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picGridLabelColor.Location = new System.Drawing.Point(152, 164);
            this.picGridLabelColor.Name = "picGridLabelColor";
            this.picGridLabelColor.Size = new System.Drawing.Size(104, 24);
            this.picGridLabelColor.TabIndex = 13;
            this.picGridLabelColor.TabStop = false;
            this.picGridLabelColor.Click += new System.EventHandler(this.CmdGridLabelColor_Click);
            // 
            // cmdGridLabelColor
            // 
            this.cmdGridLabelColor.Location = new System.Drawing.Point(8, 164);
            this.cmdGridLabelColor.Name = "cmdGridLabelColor";
            this.cmdGridLabelColor.Size = new System.Drawing.Size(136, 24);
            this.cmdGridLabelColor.TabIndex = 12;
            this.cmdGridLabelColor.Text = "Grid Label Color";
            this.cmdGridLabelColor.Click += new System.EventHandler(this.CmdGridLabelColor_Click);
            // 
            // picPlayerBorder
            // 
            this.picPlayerBorder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picPlayerBorder.Location = new System.Drawing.Point(152, 134);
            this.picPlayerBorder.Name = "picPlayerBorder";
            this.picPlayerBorder.Size = new System.Drawing.Size(104, 24);
            this.picPlayerBorder.TabIndex = 11;
            this.picPlayerBorder.TabStop = false;
            this.picPlayerBorder.Click += new System.EventHandler(this.ButPlayerBorder_Click);
            // 
            // butPlayerBorder
            // 
            this.butPlayerBorder.Location = new System.Drawing.Point(8, 134);
            this.butPlayerBorder.Name = "butPlayerBorder";
            this.butPlayerBorder.Size = new System.Drawing.Size(136, 24);
            this.butPlayerBorder.TabIndex = 10;
            this.butPlayerBorder.Text = "PC Highlight Color";
            this.butPlayerBorder.Click += new System.EventHandler(this.ButPlayerBorder_Click);
            // 
            // picListBackgroundColor
            // 
            this.picListBackgroundColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picListBackgroundColor.Location = new System.Drawing.Point(152, 104);
            this.picListBackgroundColor.Name = "picListBackgroundColor";
            this.picListBackgroundColor.Size = new System.Drawing.Size(104, 24);
            this.picListBackgroundColor.TabIndex = 7;
            this.picListBackgroundColor.TabStop = false;
            this.picListBackgroundColor.Click += new System.EventHandler(this.CmdListBackgroundColor_Click);
            // 
            // cmdListBackgroundColor
            // 
            this.cmdListBackgroundColor.Location = new System.Drawing.Point(8, 104);
            this.cmdListBackgroundColor.Name = "cmdListBackgroundColor";
            this.cmdListBackgroundColor.Size = new System.Drawing.Size(136, 24);
            this.cmdListBackgroundColor.TabIndex = 6;
            this.cmdListBackgroundColor.Text = "List Background";
            this.cmdListBackgroundColor.Click += new System.EventHandler(this.CmdListBackgroundColor_Click);
            // 
            // picRangeCircleColor
            // 
            this.picRangeCircleColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picRangeCircleColor.Location = new System.Drawing.Point(152, 72);
            this.picRangeCircleColor.Name = "picRangeCircleColor";
            this.picRangeCircleColor.Size = new System.Drawing.Size(104, 24);
            this.picRangeCircleColor.TabIndex = 5;
            this.picRangeCircleColor.TabStop = false;
            this.picRangeCircleColor.Click += new System.EventHandler(this.CmdRangeCircleColor_Click);
            // 
            // cmdRangeCircleColor
            // 
            this.cmdRangeCircleColor.Location = new System.Drawing.Point(8, 72);
            this.cmdRangeCircleColor.Name = "cmdRangeCircleColor";
            this.cmdRangeCircleColor.Size = new System.Drawing.Size(136, 24);
            this.cmdRangeCircleColor.TabIndex = 2;
            this.cmdRangeCircleColor.Text = "Range Circle";
            this.cmdRangeCircleColor.Click += new System.EventHandler(this.CmdRangeCircleColor_Click);
            // 
            // picGridColor
            // 
            this.picGridColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picGridColor.Location = new System.Drawing.Point(152, 40);
            this.picGridColor.Name = "picGridColor";
            this.picGridColor.Size = new System.Drawing.Size(104, 24);
            this.picGridColor.TabIndex = 3;
            this.picGridColor.TabStop = false;
            this.picGridColor.Click += new System.EventHandler(this.CmdGridColor_Click);
            // 
            // cmdGridColor
            // 
            this.cmdGridColor.Location = new System.Drawing.Point(8, 40);
            this.cmdGridColor.Name = "cmdGridColor";
            this.cmdGridColor.Size = new System.Drawing.Size(136, 24);
            this.cmdGridColor.TabIndex = 1;
            this.cmdGridColor.Text = "Grid";
            this.cmdGridColor.Click += new System.EventHandler(this.CmdGridColor_Click);
            // 
            // picMapBackgroundColor
            // 
            this.picMapBackgroundColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picMapBackgroundColor.Location = new System.Drawing.Point(152, 8);
            this.picMapBackgroundColor.Name = "picMapBackgroundColor";
            this.picMapBackgroundColor.Size = new System.Drawing.Size(104, 24);
            this.picMapBackgroundColor.TabIndex = 1;
            this.picMapBackgroundColor.TabStop = false;
            this.picMapBackgroundColor.Click += new System.EventHandler(this.CmdMapBackgroundColor_Click);
            // 
            // cmdMapBackgroundColor
            // 
            this.cmdMapBackgroundColor.Location = new System.Drawing.Point(8, 8);
            this.cmdMapBackgroundColor.Name = "cmdMapBackgroundColor";
            this.cmdMapBackgroundColor.Size = new System.Drawing.Size(136, 24);
            this.cmdMapBackgroundColor.TabIndex = 0;
            this.cmdMapBackgroundColor.Text = "Map Background";
            this.cmdMapBackgroundColor.Click += new System.EventHandler(this.CmdMapBackgroundColor_Click);
            // 
            // cmdCommand
            // 
            this.cmdCommand.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCommand.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdCommand.Location = new System.Drawing.Point(87, 412);
            this.cmdCommand.Name = "cmdCommand";
            this.cmdCommand.Size = new System.Drawing.Size(85, 22);
            this.cmdCommand.TabIndex = 0;
            this.cmdCommand.Text = "&Save";
            this.cmdCommand.Click += new System.EventHandler(this.CmdCommand_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(179, 412);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(85, 22);
            this.cmdCancel.TabIndex = 2;
            this.cmdCancel.Text = "Cancel";
            // 
            // OptionsForm
            // 
            this.AcceptButton = this.cmdCommand;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CancelButton = this.cmdCommand;
            this.ClientSize = new System.Drawing.Size(277, 447);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.tabOptions);
            this.Controls.Add(this.cmdCommand);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.TopMost = true;
            this.grpDanger.ResumeLayout(false);
            this.grpDanger.PerformLayout();
            this.grpAlert.ResumeLayout(false);
            this.grpAlert.PerformLayout();
            this.grpCaution.ResumeLayout(false);
            this.grpCaution.PerformLayout();
            this.grpHunt.ResumeLayout(false);
            this.grpHunt.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spnRangeCircle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMinAlertLevel)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.gbServer.ResumeLayout(false);
            this.gbServer.PerformLayout();
            this.tabOptions.ResumeLayout(false);
            this.tabGeneral.ResumeLayout(false);
            this.tabGeneral.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spnLogLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spnOverrideLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spnUpdateDelay)).EndInit();
            this.tabMap.ResumeLayout(false);
            this.tabMap.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FadedLines)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pvpLevels)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spnSpawnSize)).EndInit();
            this.tabAlerts.ResumeLayout(false);
            this.tabFolders.ResumeLayout(false);
            this.tabFolders.PerformLayout();
            this.tabColors.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picGridLabelColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPlayerBorder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picListBackgroundColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picRangeCircleColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGridColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMapBackgroundColor)).EndInit();
            this.ResumeLayout(false);

        }

        private Button butPlayerBorder;
        private Button cmdCancel;
        private Button cmdCfgDirBrowse;
        private Button cmdCommand;
        private Button cmdFilterDirBrowse;
        private Button cmdGridColor;
        private Button cmdGridLabelColor;
        private Button cmdListBackgroundColor;
        private Button cmdLogDir;
        private Button cmdMapBackgroundColor;
        private Button cmdMapDirBrowse;
        private Button cmdRangeCircleColor;
        private Button cmdSpawnTimers;

        public CheckBox chkAddjust;
        public CheckBox chkAffixAlerts;
        public CheckBox chkAlertMatchFull;
        public CheckBox chkCautionMatchFull;
        public CheckBox chkColorRangeCircle;
        public CheckBox chkCorpsesAlerts;
        public CheckBox chkDangerMatchFull;
        public CheckBox chkDirection;
        public CheckBox chkDrawFoV;
        public CheckBox chkGrid;
        public CheckBox chkGround;
        public CheckBox chkHighlight;
        public CheckBox chkHuntMatchFull;
        public CheckBox chkLineToPoint;
        public CheckBox chkMap;
        public CheckBox chkPlayer;
        public CheckBox chkPrefixAlerts;
        public CheckBox chkSaveOnExit;
        public CheckBox chkSelectSpawnList;
        public CheckBox chkShowCharName;
        public CheckBox chkShowTargetInfo;
        public CheckBox chkShowZoneName;
        public CheckBox chkSpawns;
        public CheckBox chkText;
        public CheckBox chkTimers;
        public CheckBox chkTrails;
        public ComboBox cmbAlertSound;
        public ComboBox cmbHatch;

        private ColorDialog colorOptionPicker;

        public NumericUpDown FadedLines;
        public NumericUpDown numMinAlertLevel;
        public NumericUpDown pvpLevels;
        public NumericUpDown spnLogLevel;
        public NumericUpDown spnOverrideLevel;
        public NumericUpDown spnRangeCircle;
        public NumericUpDown spnSpawnSize;
        public NumericUpDown spnUpdateDelay;

        private GroupBox gbServer;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox grpAlert;
        private GroupBox grpCaution;
        private GroupBox grpDanger;
        private GroupBox grpHunt;

        private Label label1;
        private Label label2;
        private Label label4;
        private Label lblAlertPrefix;
        private Label lblCautionPrefix;
        private Label lblCfgDir;
        private Label lblDangerPrefix;
        private Label lblFadedLines;
        private Label lblFilterDir;
        private Label lblHuntPrefix;
        private Label lblIPAddress1;
        private Label lblIPAddress2;
        private Label lblIPAddress3;
        private Label lblIPAddress4;
        private Label lblIPAddress5;
        private Label lblLogDir;
        private Label lblLogLevel;
        private Label lblMapDir;
        private Label lblOverridelevel;
        private Label lblPVPLevels;
        private Label lblSearch;
        private Label lblSpawnSize;
        private Label lblSpawnTimers;
        private Label lbltxtPortNo;
        private Label lblUpdateDelay;
        private Label lblWindowName;

        public RadioButton optAlertBeep;
        public RadioButton optAlertNone;
        public RadioButton optAlertPlay;
        public RadioButton optAlertSpeak;
        public RadioButton optCautionBeep;
        public RadioButton optCautionNone;
        public RadioButton optCautionPlay;
        public RadioButton optCautionSpeak;
        public RadioButton optDangerBeep;
        public RadioButton optDangerNone;
        public RadioButton optDangerPlay;
        public RadioButton optDangerSpeak;
        public RadioButton optHuntBeep;
        public RadioButton optHuntNone;
        public RadioButton optHuntPlay;
        public RadioButton optHuntSpeak;

        public PictureBox picGridColor;
        public PictureBox picGridLabelColor;
        public PictureBox picListBackgroundColor;
        public PictureBox picMapBackgroundColor;
        public PictureBox picPlayerBorder;
        public PictureBox picRangeCircleColor;

        private TabPage tabAlerts;
        private TabPage tabColors;
        private TabPage tabFolders;
        private TabPage tabGeneral;
        private TabPage tabMap;

        private TabControl tabOptions;

        public TextBox txtAlertAudioFile;
        public TextBox txtAlertPrefix;
        public TextBox txtCautionAudioFile;
        public TextBox txtCautionPrefix;
        public TextBox txtCfgDir;
        public TextBox txtDangerAudioFile;
        public TextBox txtDangerPrefix;
        public TextBox txtFilterDir;
        public TextBox txtHuntAudioFile;
        public TextBox txtHuntPrefix;
        public TextBox txtIPAddress1;
        public TextBox txtIPAddress2;
        public TextBox txtIPAddress3;
        public TextBox txtIPAddress4;
        public TextBox txtIPAddress5;
        public TextBox txtLogDir;
        public TextBox txtMapDir;
        public TextBox txtPortNo;
        public TextBox txtSearchString;
        public TextBox txtTimerDir;
        public TextBox txtWindowName;
        #endregion
    }
}