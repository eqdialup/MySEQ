using myseq.Properties;
using Structures;

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Media;
using System.Windows.Forms;

namespace myseq

{
    /// <summary>
    /// Summary description for frmOptions.
    /// </summary>
    public class frmOptions : Form
    {
        private ColorDialog colorOptionPicker;

        private Button cmdCommand;

        private TabPage tabColors;

        public PictureBox picPlayerBorder;

        private Button butPlayerBorder;

        public PictureBox picListBackgroundColor;

        private Button cmdListBackgroundColor;

        public PictureBox picRangeCircleColor;

        private Button cmdRangeCircleColor;

        public PictureBox picGridColor;

        private Button cmdGridColor;

        public PictureBox picMapBackgroundColor;

        private Button cmdMapBackgroundColor;

        private TabPage tabFolders;

        private Button cmdSpawnTimers;

        public TextBox txtTimerDir;

        public TextBox txtLogDir;

        public TextBox txtFilterDir;

        public TextBox txtCfgDir;

        public TextBox txtMapDir;

        private Label lblSpawnTimers;

        private Button cmdLogDir;

        private Label lblLogDir;

        private Button cmdFilterDirBrowse;

        private Label lblFilterDir;

        private Button cmdCfgDirBrowse;

        private Label lblCfgDir;

        private Button cmdMapDirBrowse;

        private Label lblMapDir;

        private TabPage tabAlerts;

        private GroupBox grpAlert;

        public RadioButton optAlertPlay;

        public RadioButton optAlertSpeak;

        public RadioButton optAlertBeep;

        public RadioButton optAlertNone;

        public TextBox txtAlertAudioFile;

        public TextBox txtAlertPrefix;

        private Label lblAlertPrefix;

        public CheckBox chkAlertMatchFull;

        private GroupBox grpCaution;

        public RadioButton optCautionPlay;

        public RadioButton optCautionSpeak;

        public RadioButton optCautionBeep;

        public RadioButton optCautionNone;

        public TextBox txtCautionAudioFile;

        public TextBox txtCautionPrefix;

        private Label lblCautionPrefix;

        public CheckBox chkCautionMatchFull;

        private GroupBox grpHunt;

        public RadioButton optHuntPlay;

        public RadioButton optHuntSpeak;

        public RadioButton optHuntBeep;

        public RadioButton optHuntNone;

        public TextBox txtHuntAudioFile;

        public TextBox txtHuntPrefix;

        private Label lblHuntPrefix;

        public CheckBox chkHuntMatchFull;

        public CheckBox chkAffixAlerts;

        public CheckBox chkPrefixAlerts;

        public CheckBox chkCorpsesAlerts;

        private TabPage tabMap;

        public NumericUpDown spnSpawnSize;

        private Label lblSpawnSize;

        public CheckBox chkSelectSpawnList;

        public CheckBox chkShowTargetInfo;

        public NumericUpDown spnRangeCircle;

        public CheckBox chkColorRangeCircle;

        public CheckBox chkDrawFoV;

        private TabPage tabGeneral;

        public NumericUpDown spnLogLevel;

        private Label lblLogLevel;

        public CheckBox chkShowZoneName;

        public NumericUpDown spnOverrideLevel;

        public NumericUpDown spnUpdateDelay;

        public TextBox txtWindowName;

        private Label lblWindowName;

        private Label lblOverridelevel;

        private GroupBox gbServer;

        public TextBox txtIPAddress5;

        private Label lblIPAddress5;

        public TextBox txtIPAddress4;

        private Label lblIPAddress4;

        public TextBox txtIPAddress3;

        private Label lblIPAddress3;

        public TextBox txtIPAddress2;

        private Label lblIPAddress2;

        public TextBox txtPortNo;

        public TextBox txtIPAddress1;

        private Label lblIPAddress1;

        private Label lbltxtPortNo;

        private Label lblUpdateDelay;

        public CheckBox chkSaveOnExit;

        private TabControl tabOptions;

        private GroupBox grpDanger;

        public RadioButton optDangerPlay;

        public RadioButton optDangerSpeak;

        public RadioButton optDangerBeep;

        public RadioButton optDangerNone;

        public TextBox txtDangerAudioFile;

        public TextBox txtDangerPrefix;

        private Label lblDangerPrefix;

        public CheckBox chkDangerMatchFull;

        private CheckBox chkMap;

        public CheckBox chkAddjust;

        private CheckBox chkSpawns;

        private CheckBox chkLineToPoint;

        private CheckBox chkText;

        private CheckBox chkGrid;

        private CheckBox chkGround;

        private CheckBox chkPlayer;

        private CheckBox chkHighlight;

        private CheckBox chkTimers;

        private CheckBox chkDirection;

        private CheckBox chkTrails;

        private GroupBox groupBox2;

        public ComboBox cmbHatch;

        public ComboBox cmbAlertSound;

        private Label label1;

        public NumericUpDown numMinAlertLevel;

        private GroupBox groupBox1;

        private Label label4;

        private Label label2;

        public TextBox txtSearchString;

        private Label lblSearch;

        public PictureBox picGridLabelColor;

        private Button cmdGridLabelColor;

        private FolderBrowserDialog fldrBrowser = new FolderBrowserDialog();

        private Label lblPVPLevels;

        public NumericUpDown pvpLevels;
        private TabPage tabPage1;
        private Label lblFadedLines;
        public NumericUpDown FadedLines;
        public CheckBox chkShowCharName;
        private Button cmdCancel;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private Container components = null;

		public frmOptions()  {
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

            foreach (string styleName in Enum.GetNames(typeof(HatchStyle)))
            {
                cmbHatch.Items.Add(styleName);
            }

            cmbHatch.SelectedText = Settings.Default.HatchIndex;

            cmbAlertSound.SelectedText = Settings.Default.AlertSound;
            
            txtIPAddress1.Text = Settings.Default.IPAddress1;

            txtIPAddress2.Text = Settings.Default.IPAddress2;

            txtIPAddress3.Text = Settings.Default.IPAddress3;

            txtIPAddress4.Text = Settings.Default.IPAddress4;

            txtIPAddress5.Text = Settings.Default.IPAddress5;

            txtPortNo.Text = Settings.Default.Port.ToString();

            spnOverrideLevel.Value = Settings.Default.LevelOverride;

            spnUpdateDelay.Value = Settings.Default.UpdateDelay;

            chkSaveOnExit.Checked = Settings.Default.SaveOnExit;

            chkPrefixAlerts.Checked = Settings.Default.PrefixStars;

            chkAffixAlerts.Checked = Settings.Default.AffixStars;       // affix

            chkCorpsesAlerts.Checked = Settings.Default.CorpseAlerts;

            txtHuntPrefix.Text = Settings.Default.HuntPrefix;

            chkHuntMatchFull.Checked = Settings.Default.MatchFullTextH;  //hunt

            optHuntNone.Checked = Settings.Default.NoneOnHunt;

            optHuntBeep.Checked =  Settings.Default.BeepOnHunt;

            optHuntSpeak.Checked = Settings.Default.TalkOnHunt;

            optHuntPlay.Checked = Settings.Default.PlayOnHunt;

            txtHuntAudioFile.Text = Settings.Default.HuntAudioFile;

            txtCautionPrefix.Text = Settings.Default.CautionPrefix;

            chkCautionMatchFull.Checked = Settings.Default.MatchFullTextC;  //Caution

            optCautionNone.Checked = Settings.Default.NoneOnCaution;

            optCautionBeep.Checked = Settings.Default.BeepOnCaution;

            optCautionSpeak.Checked = Settings.Default.TalkOnCaution;

            optCautionPlay.Checked = Settings.Default.PlayOnCaution;

            txtCautionAudioFile.Text = Settings.Default.CautionAudioFile;

            txtDangerPrefix.Text = Settings.Default.DangerPrefix;

            chkDangerMatchFull.Checked = Settings.Default.MatchFullTextD;  //danger

            optDangerNone.Checked = Settings.Default.NoneOnDanger;

            optDangerBeep.Checked = Settings.Default.BeepOnDanger;

            optDangerSpeak.Checked = Settings.Default.TalkOnDanger;

            optDangerPlay.Checked = Settings.Default.PlayOnDanger;

            txtDangerAudioFile.Text = Settings.Default.DangerAudioFile;

            txtAlertPrefix.Text = Settings.Default.AlertPrefix;

            chkAlertMatchFull.Checked = Settings.Default.MatchFullTextA;  //Rare

            optAlertNone.Checked = Settings.Default.NoneOnAlert;

            optAlertBeep.Checked = Settings.Default.BeepOnAlert;

            optAlertSpeak.Checked = Settings.Default.TalkOnAlert;

            optAlertPlay.Checked = Settings.Default.PlayOnAlert;

            txtAlertAudioFile.Text = Settings.Default.AlertAudioFile;

            spnRangeCircle.Value = Settings.Default.RangeCircle;

            numMinAlertLevel.Value = Settings.Default.MinAlertLevel;

            spnSpawnSize.Value = Settings.Default.SpawnDrawSize;

            FadedLines.Value = Settings.Default.FadedLines;

            pvpLevels.Value = Settings.Default.PVPLevels;

            txtWindowName.Text = Settings.Default.TitleBar;

            txtSearchString.Text = Settings.Default.SearchString;

            picMapBackgroundColor.BackColor = Settings.Default.BackColor;

            picListBackgroundColor.BackColor = Settings.Default.ListBackColor;

            picGridColor.BackColor = Settings.Default.GridColor;

            picGridLabelColor.BackColor = Settings.Default.GridLabelColor;

            picRangeCircleColor.BackColor = Settings.Default.RangeCircleColor;

            picPlayerBorder.BackColor = Settings.Default.PCBorderColor;

            chkColorRangeCircle.Checked = Settings.Default.ColorRangeCircle;

            cmbAlertSound.SelectedItem = Settings.Default.AlertSound;

            cmbHatch.SelectedItem = Settings.Default.HatchIndex;

            chkDrawFoV.Checked = Settings.Default.DrawFoV;

            chkShowZoneName.Checked = Settings.Default.ShowZoneName;

            chkShowCharName.Checked = Settings.Default.ShowCharName;

            chkShowTargetInfo.Checked = Settings.Default.ShowTargetInfo;

            txtMapDir.Text = Settings.Default.MapDir;

            txtFilterDir.Text = Settings.Default.FilterDir;

            txtCfgDir.Text = Settings.Default.CfgDir;

            txtLogDir.Text = Settings.Default.LogDir;

            txtTimerDir.Text = Settings.Default.TimerDir;

            spnLogLevel.Value = (int)Settings.Default.MaxLogLevel;

            chkSelectSpawnList.Checked = Settings.Default.AutoSelectSpawnList;

            SetFgDrawOptions(Settings.Default.DrawOptions);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )  {
			if( disposing )  {
				components?.Dispose();
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(frmOptions));
            cmdCommand = new Button();
            colorOptionPicker = new ColorDialog();
            tabColors = new TabPage();
            picGridLabelColor = new PictureBox();
            cmdGridLabelColor = new Button();
            picPlayerBorder = new PictureBox();
            butPlayerBorder = new Button();
            picListBackgroundColor = new PictureBox();
            cmdListBackgroundColor = new Button();
            picRangeCircleColor = new PictureBox();
            cmdRangeCircleColor = new Button();
            picGridColor = new PictureBox();
            cmdGridColor = new Button();
            picMapBackgroundColor = new PictureBox();
            cmdMapBackgroundColor = new Button();
            tabFolders = new TabPage();
            cmdSpawnTimers = new Button();
            txtTimerDir = new TextBox();
            txtLogDir = new TextBox();
            txtFilterDir = new TextBox();
            txtCfgDir = new TextBox();
            txtMapDir = new TextBox();
            lblSpawnTimers = new Label();
            cmdLogDir = new Button();
            lblLogDir = new Label();
            cmdFilterDirBrowse = new Button();
            lblFilterDir = new Label();
            cmdCfgDirBrowse = new Button();
            lblCfgDir = new Label();
            cmdMapDirBrowse = new Button();
            lblMapDir = new Label();
            tabAlerts = new TabPage();
            grpDanger = new GroupBox();
            optDangerPlay = new RadioButton();
            optDangerSpeak = new RadioButton();
            optDangerBeep = new RadioButton();
            optDangerNone = new RadioButton();
            txtDangerAudioFile = new TextBox();
            txtDangerPrefix = new TextBox();
            lblDangerPrefix = new Label();
            chkDangerMatchFull = new CheckBox();
            grpAlert = new GroupBox();
            optAlertPlay = new RadioButton();
            optAlertSpeak = new RadioButton();
            optAlertBeep = new RadioButton();
            optAlertNone = new RadioButton();
            txtAlertAudioFile = new TextBox();
            txtAlertPrefix = new TextBox();
            lblAlertPrefix = new Label();
            chkAlertMatchFull = new CheckBox();
            grpCaution = new GroupBox();
            optCautionPlay = new RadioButton();
            optCautionSpeak = new RadioButton();
            optCautionBeep = new RadioButton();
            optCautionNone = new RadioButton();
            txtCautionAudioFile = new TextBox();
            txtCautionPrefix = new TextBox();
            lblCautionPrefix = new Label();
            chkCautionMatchFull = new CheckBox();
            grpHunt = new GroupBox();
            optHuntPlay = new RadioButton();
            optHuntSpeak = new RadioButton();
            optHuntBeep = new RadioButton();
            optHuntNone = new RadioButton();
            txtHuntAudioFile = new TextBox();
            txtHuntPrefix = new TextBox();
            lblHuntPrefix = new Label();
            chkHuntMatchFull = new CheckBox();
            chkAffixAlerts = new CheckBox();
            chkPrefixAlerts = new CheckBox();
            chkCorpsesAlerts = new CheckBox();
            tabMap = new TabPage();
            FadedLines = new NumericUpDown();
            lblFadedLines = new Label();
            lblPVPLevels = new Label();
            pvpLevels = new NumericUpDown();
            groupBox1 = new GroupBox();
            cmbAlertSound = new ComboBox();
            cmbHatch = new ComboBox();
            label4 = new Label();
            label2 = new Label();
            chkColorRangeCircle = new CheckBox();
            spnRangeCircle = new NumericUpDown();
            numMinAlertLevel = new NumericUpDown();
            label1 = new Label();
            groupBox2 = new GroupBox();
            chkMap = new CheckBox();
            chkPlayer = new CheckBox();
            chkSpawns = new CheckBox();
            chkAddjust = new CheckBox();
            chkGround = new CheckBox();
            chkTrails = new CheckBox();
            chkHighlight = new CheckBox();
            chkGrid = new CheckBox();
            chkTimers = new CheckBox();
            chkText = new CheckBox();
            chkDirection = new CheckBox();
            chkLineToPoint = new CheckBox();
            lblSpawnSize = new Label();
            chkSelectSpawnList = new CheckBox();
            spnSpawnSize = new NumericUpDown();
            chkShowTargetInfo = new CheckBox();
            chkDrawFoV = new CheckBox();
            tabGeneral = new TabPage();
            chkShowCharName = new CheckBox();
            txtSearchString = new TextBox();
            lblSearch = new Label();
            spnLogLevel = new NumericUpDown();
            lblLogLevel = new Label();
            chkShowZoneName = new CheckBox();
            spnOverrideLevel = new NumericUpDown();
            spnUpdateDelay = new NumericUpDown();
            txtWindowName = new TextBox();
            lblWindowName = new Label();
            lblOverridelevel = new Label();
            gbServer = new GroupBox();
            txtIPAddress5 = new TextBox();
            lblIPAddress5 = new Label();
            txtIPAddress4 = new TextBox();
            lblIPAddress4 = new Label();
            txtIPAddress3 = new TextBox();
            lblIPAddress3 = new Label();
            txtIPAddress2 = new TextBox();
            lblIPAddress2 = new Label();
            txtPortNo = new TextBox();
            txtIPAddress1 = new TextBox();
            lblIPAddress1 = new Label();
            lbltxtPortNo = new Label();
            lblUpdateDelay = new Label();
            chkSaveOnExit = new CheckBox();
            tabOptions = new TabControl();
            tabPage1 = new TabPage();
            cmdCancel = new Button();
            tabColors.SuspendLayout();
            ((ISupportInitialize)(picGridLabelColor)).BeginInit();
            ((ISupportInitialize)(picPlayerBorder)).BeginInit();
            ((ISupportInitialize)(picListBackgroundColor)).BeginInit();
            ((ISupportInitialize)(picRangeCircleColor)).BeginInit();
            ((ISupportInitialize)(picGridColor)).BeginInit();
            ((ISupportInitialize)(picMapBackgroundColor)).BeginInit();
            tabFolders.SuspendLayout();
            tabAlerts.SuspendLayout();
            grpDanger.SuspendLayout();
            grpAlert.SuspendLayout();
            grpCaution.SuspendLayout();
            grpHunt.SuspendLayout();
            tabMap.SuspendLayout();
            ((ISupportInitialize)(FadedLines)).BeginInit();
            ((ISupportInitialize)(pvpLevels)).BeginInit();
            groupBox1.SuspendLayout();
            ((ISupportInitialize)(spnRangeCircle)).BeginInit();
            ((ISupportInitialize)(numMinAlertLevel)).BeginInit();
            groupBox2.SuspendLayout();
            ((ISupportInitialize)(spnSpawnSize)).BeginInit();
            tabGeneral.SuspendLayout();
            ((ISupportInitialize)(spnLogLevel)).BeginInit();
            ((ISupportInitialize)(spnOverrideLevel)).BeginInit();
            ((ISupportInitialize)(spnUpdateDelay)).BeginInit();
            gbServer.SuspendLayout();
            tabOptions.SuspendLayout();
            SuspendLayout();
            // 
            // cmdCommand
            // 
            cmdCommand.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
            cmdCommand.DialogResult = System.Windows.Forms.DialogResult.OK;
            cmdCommand.Location = new Point(83, 410);
            cmdCommand.Name = "cmdCommand";
            cmdCommand.Size = new Size(85, 23);
            cmdCommand.TabIndex = 0;
            cmdCommand.Text = "&Save";
            cmdCommand.Click += new EventHandler(cmdCommand_Click);
            // 
            // tabColors
            // 
            tabColors.Controls.Add(picGridLabelColor);
            tabColors.Controls.Add(cmdGridLabelColor);
            tabColors.Controls.Add(picPlayerBorder);
            tabColors.Controls.Add(butPlayerBorder);
            tabColors.Controls.Add(picListBackgroundColor);
            tabColors.Controls.Add(cmdListBackgroundColor);
            tabColors.Controls.Add(picRangeCircleColor);
            tabColors.Controls.Add(cmdRangeCircleColor);
            tabColors.Controls.Add(picGridColor);
            tabColors.Controls.Add(cmdGridColor);
            tabColors.Controls.Add(picMapBackgroundColor);
            tabColors.Controls.Add(cmdMapBackgroundColor);
            tabColors.Location = new Point(4, 22);
            tabColors.Name = "tabColors";
            tabColors.Size = new Size(266, 375);
            tabColors.TabIndex = 3;
            tabColors.Text = "Colors";
            // 
            // picGridLabelColor
            // 
            picGridLabelColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            picGridLabelColor.Location = new Point(152, 164);
            picGridLabelColor.Name = "picGridLabelColor";
            picGridLabelColor.Size = new Size(104, 24);
            picGridLabelColor.TabIndex = 13;
            picGridLabelColor.TabStop = false;
            picGridLabelColor.Click += new EventHandler(cmdGridLabelColor_Click);
            // 
            // cmdGridLabelColor
            // 
            cmdGridLabelColor.Location = new Point(8, 164);
            cmdGridLabelColor.Name = "cmdGridLabelColor";
            cmdGridLabelColor.Size = new Size(136, 24);
            cmdGridLabelColor.TabIndex = 12;
            cmdGridLabelColor.Text = "Grid Label Color";
            cmdGridLabelColor.Click += new EventHandler(cmdGridLabelColor_Click);
            // 
            // picPlayerBorder
            // 
            picPlayerBorder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            picPlayerBorder.Location = new Point(152, 134);
            picPlayerBorder.Name = "picPlayerBorder";
            picPlayerBorder.Size = new Size(104, 24);
            picPlayerBorder.TabIndex = 11;
            picPlayerBorder.TabStop = false;
            picPlayerBorder.Click += new EventHandler(butPlayerBorder_Click);
            // 
            // butPlayerBorder
            // 
            butPlayerBorder.Location = new Point(8, 134);
            butPlayerBorder.Name = "butPlayerBorder";
            butPlayerBorder.Size = new Size(136, 24);
            butPlayerBorder.TabIndex = 10;
            butPlayerBorder.Text = "PC Highlight Color";
            butPlayerBorder.Click += new EventHandler(butPlayerBorder_Click);
            // 
            // picListBackgroundColor
            // 
            picListBackgroundColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            picListBackgroundColor.Location = new Point(152, 104);
            picListBackgroundColor.Name = "picListBackgroundColor";
            picListBackgroundColor.Size = new Size(104, 24);
            picListBackgroundColor.TabIndex = 7;
            picListBackgroundColor.TabStop = false;
            picListBackgroundColor.Click += new EventHandler(cmdListBackgroundColor_Click);
            // 
            // cmdListBackgroundColor
            // 
            cmdListBackgroundColor.Location = new Point(8, 104);
            cmdListBackgroundColor.Name = "cmdListBackgroundColor";
            cmdListBackgroundColor.Size = new Size(136, 24);
            cmdListBackgroundColor.TabIndex = 6;
            cmdListBackgroundColor.Text = "List Background";
            cmdListBackgroundColor.Click += new EventHandler(cmdListBackgroundColor_Click);
            // 
            // picRangeCircleColor
            // 
            picRangeCircleColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            picRangeCircleColor.Location = new Point(152, 72);
            picRangeCircleColor.Name = "picRangeCircleColor";
            picRangeCircleColor.Size = new Size(104, 24);
            picRangeCircleColor.TabIndex = 5;
            picRangeCircleColor.TabStop = false;
            picRangeCircleColor.Click += new EventHandler(cmdRangeCircleColor_Click);
            // 
            // cmdRangeCircleColor
            // 
            cmdRangeCircleColor.Location = new Point(8, 72);
            cmdRangeCircleColor.Name = "cmdRangeCircleColor";
            cmdRangeCircleColor.Size = new Size(136, 24);
            cmdRangeCircleColor.TabIndex = 2;
            cmdRangeCircleColor.Text = "Range Circle";
            cmdRangeCircleColor.Click += new EventHandler(cmdRangeCircleColor_Click);
            // 
            // picGridColor
            // 
            picGridColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            picGridColor.Location = new Point(152, 40);
            picGridColor.Name = "picGridColor";
            picGridColor.Size = new Size(104, 24);
            picGridColor.TabIndex = 3;
            picGridColor.TabStop = false;
            picGridColor.Click += new EventHandler(cmdGridColor_Click);
            // 
            // cmdGridColor
            // 
            cmdGridColor.Location = new Point(8, 40);
            cmdGridColor.Name = "cmdGridColor";
            cmdGridColor.Size = new Size(136, 24);
            cmdGridColor.TabIndex = 1;
            cmdGridColor.Text = "Grid";
            cmdGridColor.Click += new EventHandler(cmdGridColor_Click);
            // 
            // picMapBackgroundColor
            // 
            picMapBackgroundColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            picMapBackgroundColor.Location = new Point(152, 8);
            picMapBackgroundColor.Name = "picMapBackgroundColor";
            picMapBackgroundColor.Size = new Size(104, 24);
            picMapBackgroundColor.TabIndex = 1;
            picMapBackgroundColor.TabStop = false;
            picMapBackgroundColor.Click += new EventHandler(cmdMapBackgroundColor_Click);
            // 
            // cmdMapBackgroundColor
            // 
            cmdMapBackgroundColor.Location = new Point(8, 8);
            cmdMapBackgroundColor.Name = "cmdMapBackgroundColor";
            cmdMapBackgroundColor.Size = new Size(136, 24);
            cmdMapBackgroundColor.TabIndex = 0;
            cmdMapBackgroundColor.Text = "Map Background";
            cmdMapBackgroundColor.Click += new EventHandler(cmdMapBackgroundColor_Click);
            // 
            // tabFolders
            // 
            tabFolders.Controls.Add(cmdSpawnTimers);
            tabFolders.Controls.Add(txtTimerDir);
            tabFolders.Controls.Add(txtLogDir);
            tabFolders.Controls.Add(txtFilterDir);
            tabFolders.Controls.Add(txtCfgDir);
            tabFolders.Controls.Add(txtMapDir);
            tabFolders.Controls.Add(lblSpawnTimers);
            tabFolders.Controls.Add(cmdLogDir);
            tabFolders.Controls.Add(lblLogDir);
            tabFolders.Controls.Add(cmdFilterDirBrowse);
            tabFolders.Controls.Add(lblFilterDir);
            tabFolders.Controls.Add(cmdCfgDirBrowse);
            tabFolders.Controls.Add(lblCfgDir);
            tabFolders.Controls.Add(cmdMapDirBrowse);
            tabFolders.Controls.Add(lblMapDir);
            tabFolders.Location = new Point(4, 22);
            tabFolders.Name = "tabFolders";
            tabFolders.Size = new Size(266, 375);
            tabFolders.TabIndex = 4;
            tabFolders.Text = "Folders";
            // 
            // cmdSpawnTimers
            // 
            cmdSpawnTimers.Location = new Point(232, 184);
            cmdSpawnTimers.Name = "cmdSpawnTimers";
            cmdSpawnTimers.Size = new Size(24, 23);
            cmdSpawnTimers.TabIndex = 40;
            cmdSpawnTimers.Text = "...";
            cmdSpawnTimers.Click += new EventHandler(cmdSpawnTimers_Click);
            // 
            // txtTimerDir
            // 
            txtTimerDir.BackColor = System.Drawing.Color.White;
            txtTimerDir.Location = new Point(8, 184);
            txtTimerDir.Name = "txtTimerDir";
            txtTimerDir.Size = new Size(216, 20);
            txtTimerDir.TabIndex = 39;
            // 
            // txtLogDir
            // 
            txtLogDir.BackColor = System.Drawing.Color.White;
            txtLogDir.Location = new Point(8, 144);
            txtLogDir.Name = "txtLogDir";
            txtLogDir.Size = new Size(216, 20);
            txtLogDir.TabIndex = 36;
            // 
            // txtFilterDir
            // 
            txtFilterDir.BackColor = System.Drawing.Color.White;
            txtFilterDir.Location = new Point(8, 104);
            txtFilterDir.Name = "txtFilterDir";
            txtFilterDir.Size = new Size(216, 20);
            txtFilterDir.TabIndex = 33;
            // 
            // txtCfgDir
            // 
            txtCfgDir.BackColor = System.Drawing.Color.White;
            txtCfgDir.Location = new Point(8, 64);
            txtCfgDir.Name = "txtCfgDir";
            txtCfgDir.Size = new Size(216, 20);
            txtCfgDir.TabIndex = 30;
            // 
            // txtMapDir
            // 
            txtMapDir.BackColor = System.Drawing.Color.White;
            txtMapDir.Location = new Point(8, 24);
            txtMapDir.Name = "txtMapDir";
            txtMapDir.Size = new Size(216, 20);
            txtMapDir.TabIndex = 27;
            // 
            // lblSpawnTimers
            // 
            lblSpawnTimers.Location = new Point(8, 168);
            lblSpawnTimers.Name = "lblSpawnTimers";
            lblSpawnTimers.Size = new Size(144, 16);
            lblSpawnTimers.TabIndex = 38;
            lblSpawnTimers.Text = "Spawn Timers";
            // 
            // cmdLogDir
            // 
            cmdLogDir.Location = new Point(232, 144);
            cmdLogDir.Name = "cmdLogDir";
            cmdLogDir.Size = new Size(24, 23);
            cmdLogDir.TabIndex = 37;
            cmdLogDir.Text = "...";
            cmdLogDir.Click += new EventHandler(cmdLogDir_Click);
            // 
            // lblLogDir
            // 
            lblLogDir.Location = new Point(8, 128);
            lblLogDir.Name = "lblLogDir";
            lblLogDir.Size = new Size(144, 16);
            lblLogDir.TabIndex = 35;
            lblLogDir.Text = "Log Folder";
            // 
            // cmdFilterDirBrowse
            // 
            cmdFilterDirBrowse.Location = new Point(232, 104);
            cmdFilterDirBrowse.Name = "cmdFilterDirBrowse";
            cmdFilterDirBrowse.Size = new Size(24, 23);
            cmdFilterDirBrowse.TabIndex = 34;
            cmdFilterDirBrowse.Text = "...";
            cmdFilterDirBrowse.Click += new EventHandler(cmdFilterDirBrowse_Click);
            // 
            // lblFilterDir
            // 
            lblFilterDir.Location = new Point(8, 88);
            lblFilterDir.Name = "lblFilterDir";
            lblFilterDir.Size = new Size(144, 16);
            lblFilterDir.TabIndex = 32;
            lblFilterDir.Text = "Filter Folder";
            // 
            // cmdCfgDirBrowse
            // 
            cmdCfgDirBrowse.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cmdCfgDirBrowse.Location = new Point(232, 64);
            cmdCfgDirBrowse.Name = "cmdCfgDirBrowse";
            cmdCfgDirBrowse.Size = new Size(24, 23);
            cmdCfgDirBrowse.TabIndex = 31;
            cmdCfgDirBrowse.Text = "...";
            cmdCfgDirBrowse.Click += new EventHandler(cmdCfgDirBrowse_Click);
            // 
            // lblCfgDir
            // 
            lblCfgDir.Location = new Point(8, 48);
            lblCfgDir.Name = "lblCfgDir";
            lblCfgDir.Size = new Size(144, 16);
            lblCfgDir.TabIndex = 29;
            lblCfgDir.Text = "Config Folder";
            // 
            // cmdMapDirBrowse
            // 
            cmdMapDirBrowse.Location = new Point(232, 24);
            cmdMapDirBrowse.Name = "cmdMapDirBrowse";
            cmdMapDirBrowse.Size = new Size(24, 23);
            cmdMapDirBrowse.TabIndex = 28;
            cmdMapDirBrowse.Text = "...";
            cmdMapDirBrowse.Click += new EventHandler(cmdMapDirBrowse_Click);
            // 
            // lblMapDir
            // 
            lblMapDir.Location = new Point(8, 8);
            lblMapDir.Name = "lblMapDir";
            lblMapDir.Size = new Size(144, 16);
            lblMapDir.TabIndex = 26;
            lblMapDir.Text = "Map Folder";
            // 
            // tabAlerts
            // 
            tabAlerts.Controls.Add(grpDanger);
            tabAlerts.Controls.Add(grpAlert);
            tabAlerts.Controls.Add(grpCaution);
            tabAlerts.Controls.Add(grpHunt);
            tabAlerts.Controls.Add(chkAffixAlerts);
            tabAlerts.Controls.Add(chkPrefixAlerts);
            tabAlerts.Controls.Add(chkCorpsesAlerts);
            tabAlerts.Location = new Point(4, 22);
            tabAlerts.Name = "tabAlerts";
            tabAlerts.Size = new Size(266, 375);
            tabAlerts.TabIndex = 1;
            tabAlerts.Text = "Filters";
            // 
            // grpDanger
            // 
            grpDanger.Controls.Add(optDangerPlay);
            grpDanger.Controls.Add(optDangerSpeak);
            grpDanger.Controls.Add(optDangerBeep);
            grpDanger.Controls.Add(optDangerNone);
            grpDanger.Controls.Add(txtDangerAudioFile);
            grpDanger.Controls.Add(txtDangerPrefix);
            grpDanger.Controls.Add(lblDangerPrefix);
            grpDanger.Controls.Add(chkDangerMatchFull);
            grpDanger.Location = new Point(4, 209);
            grpDanger.Name = "grpDanger";
            grpDanger.Size = new Size(248, 78);
            grpDanger.TabIndex = 23;
            grpDanger.TabStop = false;
            grpDanger.Text = "Danger";
            // 
            // optDangerPlay
            // 
            optDangerPlay.Location = new Point(8, 55);
            optDangerPlay.Name = "optDangerPlay";
            optDangerPlay.Size = new Size(80, 16);
            optDangerPlay.TabIndex = 18;
            optDangerPlay.Text = "Play Wav";
            // 
            // optDangerSpeak
            // 
            optDangerSpeak.Location = new Point(167, 35);
            optDangerSpeak.Name = "optDangerSpeak";
            optDangerSpeak.Size = new Size(72, 16);
            optDangerSpeak.TabIndex = 17;
            optDangerSpeak.Text = "Speak";
            // 
            // optDangerBeep
            // 
            optDangerBeep.Location = new Point(88, 35);
            optDangerBeep.Name = "optDangerBeep";
            optDangerBeep.Size = new Size(72, 16);
            optDangerBeep.TabIndex = 16;
            optDangerBeep.Text = "Beep";
            // 
            // optDangerNone
            // 
            optDangerNone.Checked = true;
            optDangerNone.Location = new Point(8, 35);
            optDangerNone.Name = "optDangerNone";
            optDangerNone.Size = new Size(72, 16);
            optDangerNone.TabIndex = 15;
            optDangerNone.TabStop = true;
            optDangerNone.Text = "None";
            // 
            // txtDangerAudioFile
            // 
            txtDangerAudioFile.Location = new Point(88, 55);
            txtDangerAudioFile.Name = "txtDangerAudioFile";
            txtDangerAudioFile.Size = new Size(152, 20);
            txtDangerAudioFile.TabIndex = 19;
            // 
            // txtDangerPrefix
            // 
            txtDangerPrefix.Location = new Point(88, 11);
            txtDangerPrefix.MaxLength = 5;
            txtDangerPrefix.Name = "txtDangerPrefix";
            txtDangerPrefix.Size = new Size(32, 20);
            txtDangerPrefix.TabIndex = 13;
            txtDangerPrefix.Text = "[D]";
            // 
            // lblDangerPrefix
            // 
            lblDangerPrefix.Location = new Point(8, 14);
            lblDangerPrefix.Name = "lblDangerPrefix";
            lblDangerPrefix.Size = new Size(79, 16);
            lblDangerPrefix.TabIndex = 12;
            lblDangerPrefix.Text = "Prefix/Suffix:";
            // 
            // chkDangerMatchFull
            // 
            chkDangerMatchFull.Location = new Point(136, 11);
            chkDangerMatchFull.Name = "chkDangerMatchFull";
            chkDangerMatchFull.Size = new Size(104, 24);
            chkDangerMatchFull.TabIndex = 14;
            chkDangerMatchFull.Text = "Match Full Text";
            // 
            // grpAlert
            // 
            grpAlert.Controls.Add(optAlertPlay);
            grpAlert.Controls.Add(optAlertSpeak);
            grpAlert.Controls.Add(optAlertBeep);
            grpAlert.Controls.Add(optAlertNone);
            grpAlert.Controls.Add(txtAlertAudioFile);
            grpAlert.Controls.Add(txtAlertPrefix);
            grpAlert.Controls.Add(lblAlertPrefix);
            grpAlert.Controls.Add(chkAlertMatchFull);
            grpAlert.Location = new Point(4, 293);
            grpAlert.Name = "grpAlert";
            grpAlert.Size = new Size(248, 78);
            grpAlert.TabIndex = 20;
            grpAlert.TabStop = false;
            grpAlert.Text = "Rare";
            // 
            // optAlertPlay
            // 
            optAlertPlay.Location = new Point(8, 55);
            optAlertPlay.Name = "optAlertPlay";
            optAlertPlay.Size = new Size(80, 16);
            optAlertPlay.TabIndex = 27;
            optAlertPlay.Text = "Play Wav";
            // 
            // optAlertSpeak
            // 
            optAlertSpeak.Location = new Point(168, 35);
            optAlertSpeak.Name = "optAlertSpeak";
            optAlertSpeak.Size = new Size(72, 16);
            optAlertSpeak.TabIndex = 26;
            optAlertSpeak.Text = "Speak";
            // 
            // optAlertBeep
            // 
            optAlertBeep.Location = new Point(88, 35);
            optAlertBeep.Name = "optAlertBeep";
            optAlertBeep.Size = new Size(72, 16);
            optAlertBeep.TabIndex = 25;
            optAlertBeep.Text = "Beep";
            // 
            // optAlertNone
            // 
            optAlertNone.Checked = true;
            optAlertNone.Location = new Point(8, 35);
            optAlertNone.Name = "optAlertNone";
            optAlertNone.Size = new Size(72, 16);
            optAlertNone.TabIndex = 24;
            optAlertNone.TabStop = true;
            optAlertNone.Text = "None";
            // 
            // txtAlertAudioFile
            // 
            txtAlertAudioFile.Location = new Point(88, 55);
            txtAlertAudioFile.Name = "txtAlertAudioFile";
            txtAlertAudioFile.Size = new Size(152, 20);
            txtAlertAudioFile.TabIndex = 28;
            // 
            // txtAlertPrefix
            // 
            txtAlertPrefix.Location = new Point(88, 11);
            txtAlertPrefix.MaxLength = 5;
            txtAlertPrefix.Name = "txtAlertPrefix";
            txtAlertPrefix.Size = new Size(32, 20);
            txtAlertPrefix.TabIndex = 22;
            txtAlertPrefix.Text = "[R]";
            // 
            // lblAlertPrefix
            // 
            lblAlertPrefix.Location = new Point(8, 14);
            lblAlertPrefix.Name = "lblAlertPrefix";
            lblAlertPrefix.Size = new Size(79, 16);
            lblAlertPrefix.TabIndex = 21;
            lblAlertPrefix.Text = "Prefix/Suffix:";
            // 
            // chkAlertMatchFull
            // 
            chkAlertMatchFull.Location = new Point(136, 11);
            chkAlertMatchFull.Name = "chkAlertMatchFull";
            chkAlertMatchFull.Size = new Size(104, 24);
            chkAlertMatchFull.TabIndex = 23;
            chkAlertMatchFull.Text = "Match Full Text";
            // 
            // grpCaution
            // 
            grpCaution.Controls.Add(optCautionPlay);
            grpCaution.Controls.Add(optCautionSpeak);
            grpCaution.Controls.Add(optCautionBeep);
            grpCaution.Controls.Add(optCautionNone);
            grpCaution.Controls.Add(txtCautionAudioFile);
            grpCaution.Controls.Add(txtCautionPrefix);
            grpCaution.Controls.Add(lblCautionPrefix);
            grpCaution.Controls.Add(chkCautionMatchFull);
            grpCaution.Location = new Point(3, 125);
            grpCaution.Name = "grpCaution";
            grpCaution.Size = new Size(248, 78);
            grpCaution.TabIndex = 11;
            grpCaution.TabStop = false;
            grpCaution.Text = "Caution";
            // 
            // optCautionPlay
            // 
            optCautionPlay.Location = new Point(8, 55);
            optCautionPlay.Name = "optCautionPlay";
            optCautionPlay.Size = new Size(80, 16);
            optCautionPlay.TabIndex = 18;
            optCautionPlay.Text = "Play Wav";
            // 
            // optCautionSpeak
            // 
            optCautionSpeak.Location = new Point(168, 38);
            optCautionSpeak.Name = "optCautionSpeak";
            optCautionSpeak.Size = new Size(72, 16);
            optCautionSpeak.TabIndex = 17;
            optCautionSpeak.Text = "Speak";
            // 
            // optCautionBeep
            // 
            optCautionBeep.Location = new Point(88, 35);
            optCautionBeep.Name = "optCautionBeep";
            optCautionBeep.Size = new Size(72, 16);
            optCautionBeep.TabIndex = 16;
            optCautionBeep.Text = "Beep";
            // 
            // optCautionNone
            // 
            optCautionNone.Checked = true;
            optCautionNone.Location = new Point(8, 35);
            optCautionNone.Name = "optCautionNone";
            optCautionNone.Size = new Size(72, 16);
            optCautionNone.TabIndex = 15;
            optCautionNone.TabStop = true;
            optCautionNone.Text = "None";
            // 
            // txtCautionAudioFile
            // 
            txtCautionAudioFile.Location = new Point(88, 55);
            txtCautionAudioFile.Name = "txtCautionAudioFile";
            txtCautionAudioFile.Size = new Size(152, 20);
            txtCautionAudioFile.TabIndex = 19;
            // 
            // txtCautionPrefix
            // 
            txtCautionPrefix.Location = new Point(88, 11);
            txtCautionPrefix.MaxLength = 5;
            txtCautionPrefix.Name = "txtCautionPrefix";
            txtCautionPrefix.Size = new Size(32, 20);
            txtCautionPrefix.TabIndex = 13;
            txtCautionPrefix.Text = "[C]";
            // 
            // lblCautionPrefix
            // 
            lblCautionPrefix.Location = new Point(8, 14);
            lblCautionPrefix.Name = "lblCautionPrefix";
            lblCautionPrefix.Size = new Size(80, 16);
            lblCautionPrefix.TabIndex = 12;
            lblCautionPrefix.Text = "Prefix/Suffix:";
            // 
            // chkCautionMatchFull
            // 
            chkCautionMatchFull.Location = new Point(136, 11);
            chkCautionMatchFull.Name = "chkCautionMatchFull";
            chkCautionMatchFull.Size = new Size(104, 24);
            chkCautionMatchFull.TabIndex = 14;
            chkCautionMatchFull.Text = "Match Full Text";
            // 
            // grpHunt
            // 
            grpHunt.Controls.Add(optHuntPlay);
            grpHunt.Controls.Add(optHuntSpeak);
            grpHunt.Controls.Add(optHuntBeep);
            grpHunt.Controls.Add(optHuntNone);
            grpHunt.Controls.Add(txtHuntAudioFile);
            grpHunt.Controls.Add(txtHuntPrefix);
            grpHunt.Controls.Add(lblHuntPrefix);
            grpHunt.Controls.Add(chkHuntMatchFull);
            grpHunt.Location = new Point(3, 41);
            grpHunt.Name = "grpHunt";
            grpHunt.Size = new Size(248, 78);
            grpHunt.TabIndex = 2;
            grpHunt.TabStop = false;
            grpHunt.Text = "Hunt";
            // 
            // optHuntPlay
            // 
            optHuntPlay.Location = new Point(8, 55);
            optHuntPlay.Name = "optHuntPlay";
            optHuntPlay.Size = new Size(80, 16);
            optHuntPlay.TabIndex = 9;
            optHuntPlay.Text = "Play Wav";
            // 
            // optHuntSpeak
            // 
            optHuntSpeak.Location = new Point(168, 35);
            optHuntSpeak.Name = "optHuntSpeak";
            optHuntSpeak.Size = new Size(72, 16);
            optHuntSpeak.TabIndex = 8;
            optHuntSpeak.Text = "Speak";
            // 
            // optHuntBeep
            // 
            optHuntBeep.Location = new Point(88, 35);
            optHuntBeep.Name = "optHuntBeep";
            optHuntBeep.Size = new Size(72, 16);
            optHuntBeep.TabIndex = 7;
            optHuntBeep.Text = "Beep";
            // 
            // optHuntNone
            // 
            optHuntNone.Checked = true;
            optHuntNone.Location = new Point(8, 35);
            optHuntNone.Name = "optHuntNone";
            optHuntNone.Size = new Size(72, 16);
            optHuntNone.TabIndex = 6;
            optHuntNone.TabStop = true;
            optHuntNone.Text = "None";
            // 
            // txtHuntAudioFile
            // 
            txtHuntAudioFile.Location = new Point(88, 55);
            txtHuntAudioFile.Name = "txtHuntAudioFile";
            txtHuntAudioFile.Size = new Size(152, 20);
            txtHuntAudioFile.TabIndex = 10;
            // 
            // txtHuntPrefix
            // 
            txtHuntPrefix.Location = new Point(88, 11);
            txtHuntPrefix.MaxLength = 5;
            txtHuntPrefix.Name = "txtHuntPrefix";
            txtHuntPrefix.Size = new Size(32, 20);
            txtHuntPrefix.TabIndex = 4;
            txtHuntPrefix.Text = "[H]";
            // 
            // lblHuntPrefix
            // 
            lblHuntPrefix.Location = new Point(8, 14);
            lblHuntPrefix.Name = "lblHuntPrefix";
            lblHuntPrefix.Size = new Size(80, 16);
            lblHuntPrefix.TabIndex = 3;
            lblHuntPrefix.Text = "Prefix/Suffix:";
            // 
            // chkHuntMatchFull
            // 
            chkHuntMatchFull.Location = new Point(136, 11);
            chkHuntMatchFull.Name = "chkHuntMatchFull";
            chkHuntMatchFull.Size = new Size(104, 24);
            chkHuntMatchFull.TabIndex = 5;
            chkHuntMatchFull.Text = "Match Full Text";
            // 
            // chkAffixAlerts
            // 
            chkAffixAlerts.Checked = true;
            chkAffixAlerts.CheckState = System.Windows.Forms.CheckState.Checked;
            chkAffixAlerts.Location = new Point(4, 22);
            chkAffixAlerts.Name = "chkAffixAlerts";
            chkAffixAlerts.Size = new Size(112, 24);
            chkAffixAlerts.TabIndex = 1;
            chkAffixAlerts.Text = "Attach Suffix Text";
            // 
            // chkPrefixAlerts
            // 
            chkPrefixAlerts.Checked = true;
            chkPrefixAlerts.CheckState = System.Windows.Forms.CheckState.Checked;
            chkPrefixAlerts.Location = new Point(4, 3);
            chkPrefixAlerts.Name = "chkPrefixAlerts";
            chkPrefixAlerts.Size = new Size(120, 24);
            chkPrefixAlerts.TabIndex = 0;
            chkPrefixAlerts.Text = "Attach Prefix Text";
            // 
            // chkCorpsesAlerts
            // 
            chkCorpsesAlerts.Checked = true;
            chkCorpsesAlerts.CheckState = System.Windows.Forms.CheckState.Checked;
            chkCorpsesAlerts.Location = new Point(152, 3);
            chkCorpsesAlerts.Name = "chkCorpsesAlerts";
            chkCorpsesAlerts.Size = new Size(100, 24);
            chkCorpsesAlerts.TabIndex = 24;
            chkCorpsesAlerts.Text = "Match Corpses";
            // 
            // tabMap
            // 
            tabMap.Controls.Add(FadedLines);
            tabMap.Controls.Add(lblFadedLines);
            tabMap.Controls.Add(lblPVPLevels);
            tabMap.Controls.Add(pvpLevels);
            tabMap.Controls.Add(groupBox1);
            tabMap.Controls.Add(groupBox2);
            tabMap.Controls.Add(lblSpawnSize);
            tabMap.Controls.Add(chkSelectSpawnList);
            tabMap.Controls.Add(spnSpawnSize);
            tabMap.Controls.Add(chkShowTargetInfo);
            tabMap.Controls.Add(chkDrawFoV);
            tabMap.Location = new Point(4, 22);
            tabMap.Name = "tabMap";
            tabMap.Size = new Size(266, 375);
            tabMap.TabIndex = 2;
            tabMap.Text = "Map";
            // 
            // FadedLines
            // 
            FadedLines.Location = new Point(175, 99);
            FadedLines.Name = "FadedLines";
            FadedLines.Size = new Size(64, 20);
            FadedLines.TabIndex = 61;
            FadedLines.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // lblFadedLines
            // 
            lblFadedLines.AutoSize = true;
            lblFadedLines.Location = new Point(6, 99);
            lblFadedLines.Name = "lblFadedLines";
            lblFadedLines.Size = new Size(139, 13);
            lblFadedLines.TabIndex = 60;
            lblFadedLines.Text = "Dynamic Alpha Faded Lines";
            // 
            // lblPVPLevels
            // 
            lblPVPLevels.Location = new Point(6, 76);
            lblPVPLevels.Name = "lblPVPLevels";
            lblPVPLevels.Size = new Size(142, 16);
            lblPVPLevels.TabIndex = 58;
            lblPVPLevels.Text = "PVP Level Range:";
            // 
            // pvpLevels
            // 
            pvpLevels.Location = new Point(175, 74);
            pvpLevels.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            pvpLevels.Name = "pvpLevels";
            pvpLevels.Size = new Size(64, 20);
            pvpLevels.TabIndex = 59;
            pvpLevels.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            pvpLevels.ValueChanged += new EventHandler(pvpLevels_ValueChanged);
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(cmbAlertSound);
            groupBox1.Controls.Add(cmbHatch);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(chkColorRangeCircle);
            groupBox1.Controls.Add(spnRangeCircle);
            groupBox1.Controls.Add(numMinAlertLevel);
            groupBox1.Controls.Add(label1);
            groupBox1.Location = new Point(3, 119);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(258, 116);
            groupBox1.TabIndex = 57;
            groupBox1.TabStop = false;
            groupBox1.Text = "Proximity Alert Settings";
            // 
            // cmbAlertSound
            // 
            cmbAlertSound.FormattingEnabled = true;
            cmbAlertSound.Items.AddRange(new object[] {
            "Asterisk",
            "Beep",
            "Exclamation",
            "Hand",
            "Question"});
            cmbAlertSound.Location = new Point(104, 67);
            cmbAlertSound.Name = "cmbAlertSound";
            cmbAlertSound.Size = new Size(133, 21);
            cmbAlertSound.TabIndex = 54;
            cmbAlertSound.SelectionChangeCommitted += new EventHandler(cmbAlertSound_SelectionChangeCommitted);
            // 
            // cmbHatch
            // 
            cmbHatch.FormattingEnabled = true;
            cmbHatch.Location = new Point(104, 42);
            cmbHatch.Name = "cmbHatch";
            cmbHatch.Size = new Size(133, 21);
            cmbHatch.TabIndex = 53;
            cmbHatch.Tag = "";
            cmbHatch.SelectionChangeCommitted += new EventHandler(cmbHatch_SelectionChangeCommitted);
            // 
            // label4
            // 
            label4.Location = new Point(4, 70);
            label4.Name = "label4";
            label4.Size = new Size(103, 16);
            label4.TabIndex = 58;
            label4.Text = "Alert Sound";
            // 
            // label2
            // 
            label2.Location = new Point(4, 45);
            label2.Name = "label2";
            label2.Size = new Size(103, 16);
            label2.TabIndex = 57;
            label2.Text = "Hatch Pattern";
            // 
            // chkColorRangeCircle
            // 
            chkColorRangeCircle.Location = new Point(7, 20);
            chkColorRangeCircle.Name = "chkColorRangeCircle";
            chkColorRangeCircle.Size = new Size(108, 16);
            chkColorRangeCircle.TabIndex = 5;
            chkColorRangeCircle.Text = "Range Circle";
            // 
            // spnRangeCircle
            // 
            spnRangeCircle.Location = new Point(173, 19);
            spnRangeCircle.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            spnRangeCircle.Name = "spnRangeCircle";
            spnRangeCircle.Size = new Size(64, 20);
            spnRangeCircle.TabIndex = 7;
            spnRangeCircle.Value = new decimal(new int[] {
            150,
            0,
            0,
            0});
            // 
            // numMinAlertLevel
            // 
            numMinAlertLevel.Location = new Point(172, 91);
            numMinAlertLevel.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            numMinAlertLevel.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            numMinAlertLevel.Name = "numMinAlertLevel";
            numMinAlertLevel.Size = new Size(64, 20);
            numMinAlertLevel.TabIndex = 55;
            numMinAlertLevel.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            label1.Location = new Point(4, 93);
            label1.Name = "label1";
            label1.Size = new Size(142, 16);
            label1.TabIndex = 56;
            label1.Text = "Minimum Alert Level";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(chkMap);
            groupBox2.Controls.Add(chkPlayer);
            groupBox2.Controls.Add(chkSpawns);
            groupBox2.Controls.Add(chkAddjust);
            groupBox2.Controls.Add(chkGround);
            groupBox2.Controls.Add(chkTrails);
            groupBox2.Controls.Add(chkHighlight);
            groupBox2.Controls.Add(chkGrid);
            groupBox2.Controls.Add(chkTimers);
            groupBox2.Controls.Add(chkText);
            groupBox2.Controls.Add(chkDirection);
            groupBox2.Controls.Add(chkLineToPoint);
            groupBox2.Location = new Point(3, 241);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(258, 130);
            groupBox2.TabIndex = 52;
            groupBox2.TabStop = false;
            groupBox2.Text = "Map Drawing Settings";
            // 
            // chkMap
            // 
            chkMap.Location = new Point(142, 53);
            chkMap.Name = "chkMap";
            chkMap.Size = new Size(94, 20);
            chkMap.TabIndex = 30;
            chkMap.Text = "Draw Map";
            // 
            // chkPlayer
            // 
            chkPlayer.Location = new Point(142, 70);
            chkPlayer.Name = "chkPlayer";
            chkPlayer.Size = new Size(94, 20);
            chkPlayer.TabIndex = 50;
            chkPlayer.Text = "Draw Player";
            // 
            // chkSpawns
            // 
            chkSpawns.Location = new Point(142, 36);
            chkSpawns.Name = "chkSpawns";
            chkSpawns.Size = new Size(94, 20);
            chkSpawns.TabIndex = 43;
            chkSpawns.Text = "Draw Spawns";
            // 
            // chkAddjust
            // 
            chkAddjust.Location = new Point(6, 19);
            chkAddjust.Name = "chkAddjust";
            chkAddjust.Size = new Size(121, 20);
            chkAddjust.TabIndex = 29;
            chkAddjust.Text = "Readjust Map";
            // 
            // chkGround
            // 
            chkGround.Location = new Point(142, 104);
            chkGround.Name = "chkGround";
            chkGround.Size = new Size(111, 20);
            chkGround.TabIndex = 51;
            chkGround.Text = "Ground Spawns";
            // 
            // chkTrails
            // 
            chkTrails.Location = new Point(6, 87);
            chkTrails.Name = "chkTrails";
            chkTrails.Size = new Size(130, 20);
            chkTrails.TabIndex = 45;
            chkTrails.Text = "Spawn Trails";
            // 
            // chkHighlight
            // 
            chkHighlight.Location = new Point(6, 104);
            chkHighlight.Name = "chkHighlight";
            chkHighlight.Size = new Size(130, 20);
            chkHighlight.TabIndex = 49;
            chkHighlight.Text = "Highlight Merchants";
            // 
            // chkGrid
            // 
            chkGrid.Location = new Point(6, 36);
            chkGrid.Name = "chkGrid";
            chkGrid.Size = new Size(130, 20);
            chkGrid.TabIndex = 37;
            chkGrid.Text = "Show Gridlines";
            // 
            // chkTimers
            // 
            chkTimers.Location = new Point(142, 19);
            chkTimers.Name = "chkTimers";
            chkTimers.Size = new Size(116, 20);
            chkTimers.TabIndex = 47;
            chkTimers.Text = "Spawn Timers";
            // 
            // chkText
            // 
            chkText.Location = new Point(6, 70);
            chkText.Name = "chkText";
            chkText.Size = new Size(130, 20);
            chkText.TabIndex = 41;
            chkText.Text = "Show Zone Text";
            // 
            // chkDirection
            // 
            chkDirection.Location = new Point(142, 87);
            chkDirection.Name = "chkDirection";
            chkDirection.Size = new Size(109, 20);
            chkDirection.TabIndex = 46;
            chkDirection.Text = "Heading Lines";
            // 
            // chkLineToPoint
            // 
            chkLineToPoint.Location = new Point(6, 53);
            chkLineToPoint.Name = "chkLineToPoint";
            chkLineToPoint.Size = new Size(130, 20);
            chkLineToPoint.TabIndex = 42;
            chkLineToPoint.Text = "Draw Line to Point";
            // 
            // lblSpawnSize
            // 
            lblSpawnSize.Location = new Point(6, 51);
            lblSpawnSize.Name = "lblSpawnSize";
            lblSpawnSize.Size = new Size(142, 16);
            lblSpawnSize.TabIndex = 20;
            lblSpawnSize.Text = "Spawn Draw Size:";
            // 
            // chkSelectSpawnList
            // 
            chkSelectSpawnList.Location = new Point(8, 32);
            chkSelectSpawnList.Name = "chkSelectSpawnList";
            chkSelectSpawnList.Size = new Size(248, 16);
            chkSelectSpawnList.TabIndex = 4;
            chkSelectSpawnList.Text = "Auto Select Spawn in the Spawn List";
            // 
            // spnSpawnSize
            // 
            spnSpawnSize.Location = new Point(175, 49);
            spnSpawnSize.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
            spnSpawnSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            spnSpawnSize.Name = "spnSpawnSize";
            spnSpawnSize.Size = new Size(64, 20);
            spnSpawnSize.TabIndex = 21;
            spnSpawnSize.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            spnSpawnSize.ValueChanged += new EventHandler(spnSpawnSize_ValueChanged);
            // 
            // chkShowTargetInfo
            // 
            chkShowTargetInfo.Location = new Point(8, 12);
            chkShowTargetInfo.Name = "chkShowTargetInfo";
            chkShowTargetInfo.Size = new Size(248, 16);
            chkShowTargetInfo.TabIndex = 3;
            chkShowTargetInfo.Text = "Show Target Information Window";
            // 
            // chkDrawFoV
            // 
            chkDrawFoV.Location = new Point(8, -24);
            chkDrawFoV.Name = "chkDrawFoV";
            chkDrawFoV.Size = new Size(248, 16);
            chkDrawFoV.TabIndex = 2;
            chkDrawFoV.Text = "Draw Field of View (FoV)";
            // 
            // tabGeneral
            // 
            tabGeneral.Controls.Add(chkShowCharName);
            tabGeneral.Controls.Add(txtSearchString);
            tabGeneral.Controls.Add(lblSearch);
            tabGeneral.Controls.Add(spnLogLevel);
            tabGeneral.Controls.Add(lblLogLevel);
            tabGeneral.Controls.Add(chkShowZoneName);
            tabGeneral.Controls.Add(spnOverrideLevel);
            tabGeneral.Controls.Add(spnUpdateDelay);
            tabGeneral.Controls.Add(txtWindowName);
            tabGeneral.Controls.Add(lblWindowName);
            tabGeneral.Controls.Add(lblOverridelevel);
            tabGeneral.Controls.Add(gbServer);
            tabGeneral.Controls.Add(lblUpdateDelay);
            tabGeneral.Controls.Add(chkSaveOnExit);
            tabGeneral.Location = new Point(4, 22);
            tabGeneral.Name = "tabGeneral";
            tabGeneral.Size = new Size(266, 375);
            tabGeneral.TabIndex = 0;
            tabGeneral.Text = "General";
            // 
            // chkShowCharName
            // 
            chkShowCharName.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            chkShowCharName.Checked = true;
            chkShowCharName.CheckState = System.Windows.Forms.CheckState.Checked;
            chkShowCharName.Location = new Point(134, 320);
            chkShowCharName.Name = "chkShowCharName";
            chkShowCharName.Size = new Size(114, 24);
            chkShowCharName.TabIndex = 27;
            chkShowCharName.Text = "Show Char Name";
            // 
            // txtSearchString
            // 
            txtSearchString.Location = new Point(61, 344);
            txtSearchString.Name = "txtSearchString";
            txtSearchString.Size = new Size(195, 20);
            txtSearchString.TabIndex = 26;
            // 
            // lblSearch
            // 
            lblSearch.Location = new Point(8, 347);
            lblSearch.Name = "lblSearch";
            lblSearch.Size = new Size(47, 17);
            lblSearch.TabIndex = 25;
            lblSearch.Text = "Search:";
            // 
            // spnLogLevel
            // 
            spnLogLevel.Location = new Point(192, 256);
            spnLogLevel.Name = "spnLogLevel";
            spnLogLevel.Size = new Size(64, 20);
            spnLogLevel.TabIndex = 21;
            // 
            // lblLogLevel
            // 
            lblLogLevel.Location = new Point(8, 256);
            lblLogLevel.Name = "lblLogLevel";
            lblLogLevel.Size = new Size(136, 16);
            lblLogLevel.TabIndex = 20;
            lblLogLevel.Text = "Error Logging Level:";
            // 
            // chkShowZoneName
            // 
            chkShowZoneName.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            chkShowZoneName.Checked = true;
            chkShowZoneName.CheckState = System.Windows.Forms.CheckState.Checked;
            chkShowZoneName.Location = new Point(8, 320);
            chkShowZoneName.Name = "chkShowZoneName";
            chkShowZoneName.Size = new Size(114, 24);
            chkShowZoneName.TabIndex = 24;
            chkShowZoneName.Text = "Show Zone Name";
            // 
            // spnOverrideLevel
            // 
            spnOverrideLevel.Location = new Point(192, 208);
            spnOverrideLevel.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            spnOverrideLevel.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            spnOverrideLevel.Name = "spnOverrideLevel";
            spnOverrideLevel.Size = new Size(64, 20);
            spnOverrideLevel.TabIndex = 15;
            spnOverrideLevel.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // spnUpdateDelay
            // 
            spnUpdateDelay.Location = new Point(192, 232);
            spnUpdateDelay.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            spnUpdateDelay.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            spnUpdateDelay.Name = "spnUpdateDelay";
            spnUpdateDelay.Size = new Size(64, 20);
            spnUpdateDelay.TabIndex = 17;
            spnUpdateDelay.Value = new decimal(new int[] {
            250,
            0,
            0,
            0});
            // 
            // txtWindowName
            // 
            txtWindowName.Location = new Point(8, 296);
            txtWindowName.Name = "txtWindowName";
            txtWindowName.Size = new Size(248, 20);
            txtWindowName.TabIndex = 23;
            // 
            // lblWindowName
            // 
            lblWindowName.Location = new Point(8, 280);
            lblWindowName.Name = "lblWindowName";
            lblWindowName.Size = new Size(144, 16);
            lblWindowName.TabIndex = 22;
            lblWindowName.Text = "Window Title:";
            // 
            // lblOverridelevel
            // 
            lblOverridelevel.Location = new Point(8, 208);
            lblOverridelevel.Name = "lblOverridelevel";
            lblOverridelevel.Size = new Size(136, 16);
            lblOverridelevel.TabIndex = 14;
            lblOverridelevel.Text = "Override Level:";
            // 
            // gbServer
            // 
            gbServer.Controls.Add(txtIPAddress5);
            gbServer.Controls.Add(lblIPAddress5);
            gbServer.Controls.Add(txtIPAddress4);
            gbServer.Controls.Add(lblIPAddress4);
            gbServer.Controls.Add(txtIPAddress3);
            gbServer.Controls.Add(lblIPAddress3);
            gbServer.Controls.Add(txtIPAddress2);
            gbServer.Controls.Add(lblIPAddress2);
            gbServer.Controls.Add(txtPortNo);
            gbServer.Controls.Add(txtIPAddress1);
            gbServer.Controls.Add(lblIPAddress1);
            gbServer.Controls.Add(lbltxtPortNo);
            gbServer.Location = new Point(8, 8);
            gbServer.Name = "gbServer";
            gbServer.Size = new Size(248, 168);
            gbServer.TabIndex = 0;
            gbServer.TabStop = false;
            gbServer.Text = "Server";
            // 
            // txtIPAddress5
            // 
            txtIPAddress5.Location = new Point(128, 112);
            txtIPAddress5.Name = "txtIPAddress5";
            txtIPAddress5.Size = new Size(112, 20);
            txtIPAddress5.TabIndex = 10;
            // 
            // lblIPAddress5
            // 
            lblIPAddress5.Location = new Point(8, 112);
            lblIPAddress5.Name = "lblIPAddress5";
            lblIPAddress5.Size = new Size(120, 16);
            lblIPAddress5.TabIndex = 9;
            lblIPAddress5.Text = "IP Address 5: (Ctrl + 5)";
            // 
            // txtIPAddress4
            // 
            txtIPAddress4.Location = new Point(128, 88);
            txtIPAddress4.Name = "txtIPAddress4";
            txtIPAddress4.Size = new Size(112, 20);
            txtIPAddress4.TabIndex = 8;
            // 
            // lblIPAddress4
            // 
            lblIPAddress4.Location = new Point(8, 88);
            lblIPAddress4.Name = "lblIPAddress4";
            lblIPAddress4.Size = new Size(120, 16);
            lblIPAddress4.TabIndex = 7;
            lblIPAddress4.Text = "IP Address 4: (Ctrl + 4)";
            // 
            // txtIPAddress3
            // 
            txtIPAddress3.Location = new Point(128, 64);
            txtIPAddress3.Name = "txtIPAddress3";
            txtIPAddress3.Size = new Size(112, 20);
            txtIPAddress3.TabIndex = 6;
            // 
            // lblIPAddress3
            // 
            lblIPAddress3.Location = new Point(8, 64);
            lblIPAddress3.Name = "lblIPAddress3";
            lblIPAddress3.Size = new Size(120, 16);
            lblIPAddress3.TabIndex = 5;
            lblIPAddress3.Text = "IP Address 3: (Ctrl + 3)";
            // 
            // txtIPAddress2
            // 
            txtIPAddress2.Location = new Point(128, 40);
            txtIPAddress2.Name = "txtIPAddress2";
            txtIPAddress2.Size = new Size(112, 20);
            txtIPAddress2.TabIndex = 4;
            // 
            // lblIPAddress2
            // 
            lblIPAddress2.Location = new Point(8, 40);
            lblIPAddress2.Name = "lblIPAddress2";
            lblIPAddress2.Size = new Size(120, 16);
            lblIPAddress2.TabIndex = 3;
            lblIPAddress2.Text = "IP Address 2: (Ctrl + 2)";
            // 
            // txtPortNo
            // 
            txtPortNo.Location = new Point(128, 136);
            txtPortNo.Name = "txtPortNo";
            txtPortNo.Size = new Size(112, 20);
            txtPortNo.TabIndex = 12;
            txtPortNo.Text = "5555";
            // 
            // txtIPAddress1
            // 
            txtIPAddress1.Location = new Point(128, 16);
            txtIPAddress1.Name = "txtIPAddress1";
            txtIPAddress1.Size = new Size(112, 20);
            txtIPAddress1.TabIndex = 2;
            txtIPAddress1.Text = "localhost";
            // 
            // lblIPAddress1
            // 
            lblIPAddress1.Location = new Point(8, 16);
            lblIPAddress1.Name = "lblIPAddress1";
            lblIPAddress1.Size = new Size(120, 16);
            lblIPAddress1.TabIndex = 1;
            lblIPAddress1.Text = "IP Address 1: (Ctrl + 1)";
            // 
            // lbltxtPortNo
            // 
            lbltxtPortNo.Location = new Point(8, 136);
            lbltxtPortNo.Name = "lbltxtPortNo";
            lbltxtPortNo.Size = new Size(120, 16);
            lbltxtPortNo.TabIndex = 11;
            lbltxtPortNo.Text = "Port:";
            // 
            // lblUpdateDelay
            // 
            lblUpdateDelay.Location = new Point(8, 232);
            lblUpdateDelay.Name = "lblUpdateDelay";
            lblUpdateDelay.Size = new Size(136, 16);
            lblUpdateDelay.TabIndex = 16;
            lblUpdateDelay.Text = "Update Delay (mS):";
            // 
            // chkSaveOnExit
            // 
            chkSaveOnExit.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            chkSaveOnExit.Checked = true;
            chkSaveOnExit.CheckState = System.Windows.Forms.CheckState.Checked;
            chkSaveOnExit.Location = new Point(8, 176);
            chkSaveOnExit.Name = "chkSaveOnExit";
            chkSaveOnExit.Size = new Size(197, 24);
            chkSaveOnExit.TabIndex = 13;
            chkSaveOnExit.Text = "Save Preferences On Exit:";
            // 
            // tabOptions
            // 
            tabOptions.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom
            | AnchorStyles.Left
            | AnchorStyles.Right);
            tabOptions.Controls.Add(tabGeneral);
            tabOptions.Controls.Add(tabMap);
            tabOptions.Controls.Add(tabAlerts);
            tabOptions.Controls.Add(tabFolders);
            tabOptions.Controls.Add(tabColors);
            tabOptions.Controls.Add(tabPage1);
            tabOptions.Location = new Point(0, 3);
            tabOptions.Name = "tabOptions";
            tabOptions.SelectedIndex = 0;
            tabOptions.Size = new Size(274, 401);
            tabOptions.TabIndex = 1;
            // 
            // tabPage1
            // 
            tabPage1.Location = new Point(4, 22);
            tabPage1.Name = "tabPage1";
            tabPage1.Size = new Size(264, 374);
            tabPage1.TabIndex = 5;
            // 
            // cmdCancel
            // 
            cmdCancel.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
            cmdCancel.DialogResult = DialogResult.Cancel;
            cmdCancel.Location = new Point(176, 410);
            cmdCancel.Name = "cmdCancel";
            cmdCancel.Size = new Size(85, 23);
            cmdCancel.TabIndex = 2;
            cmdCancel.Text = "Cancel";
            // 
            // frmOptions
            // 
            AcceptButton = cmdCommand;
            AutoScaleBaseSize = new Size(5, 13);
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            CancelButton = cmdCommand;
            ClientSize = new Size(273, 445);
            Controls.Add(cmdCancel);
            Controls.Add(tabOptions);
            Controls.Add(cmdCommand);
            Icon = ((Icon)(resources.GetObject("$this.Icon")));
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmOptions";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Options";
            TopMost = true;
            tabColors.ResumeLayout(false);
            ((ISupportInitialize)(picGridLabelColor)).EndInit();
            ((ISupportInitialize)(picPlayerBorder)).EndInit();
            ((ISupportInitialize)(picListBackgroundColor)).EndInit();
            ((ISupportInitialize)(picRangeCircleColor)).EndInit();
            ((ISupportInitialize)(picGridColor)).EndInit();
            ((ISupportInitialize)(picMapBackgroundColor)).EndInit();
            tabFolders.ResumeLayout(false);
            tabFolders.PerformLayout();
            tabAlerts.ResumeLayout(false);
            grpDanger.ResumeLayout(false);
            grpDanger.PerformLayout();
            grpAlert.ResumeLayout(false);
            grpAlert.PerformLayout();
            grpCaution.ResumeLayout(false);
            grpCaution.PerformLayout();
            grpHunt.ResumeLayout(false);
            grpHunt.PerformLayout();
            tabMap.ResumeLayout(false);
            tabMap.PerformLayout();
            ((ISupportInitialize)(FadedLines)).EndInit();
            ((ISupportInitialize)(pvpLevels)).EndInit();
            groupBox1.ResumeLayout(false);
            ((ISupportInitialize)(spnRangeCircle)).EndInit();
            ((ISupportInitialize)(numMinAlertLevel)).EndInit();
            groupBox2.ResumeLayout(false);
            ((ISupportInitialize)(spnSpawnSize)).EndInit();
            tabGeneral.ResumeLayout(false);
            tabGeneral.PerformLayout();
            ((ISupportInitialize)(spnLogLevel)).EndInit();
            ((ISupportInitialize)(spnOverrideLevel)).EndInit();
            ((ISupportInitialize)(spnUpdateDelay)).EndInit();
            gbServer.ResumeLayout(false);
            gbServer.PerformLayout();
            tabOptions.ResumeLayout(false);
            ResumeLayout(false);
		}

		#endregion

		private void cmdCommand_Click(object sender, EventArgs e)
        {

            if (chkSaveOnExit.Checked)  // This checkbox actually DO something now. 
                {
                SaveSettings();
                }


            bool done = true;

            if (!Directory.Exists(txtMapDir.Text))
            {
                if (DialogResult.Yes == DirBox("Map"))
                {
                    Directory.CreateDirectory(txtMapDir.Text);
                }
                else done = false;
            }

            if (!Directory.Exists(txtFilterDir.Text) && DialogResult.Yes == DirBox("Filters"))
            {
                Directory.CreateDirectory(txtFilterDir.Text);
            }

            if (!Directory.Exists(txtCfgDir.Text))
            {
                if (DialogResult.Yes == DirBox("Config"))
                {
                    Directory.CreateDirectory(txtCfgDir.Text);
                }
                else done = false;
            }

            if (!Directory.Exists(txtLogDir.Text))
            {
                if (DialogResult.Yes == DirBox("Log"))
                {
                    Directory.CreateDirectory(txtLogDir.Text);
                }
                else done = false;
            }

            if (!Directory.Exists(txtTimerDir.Text))
            {
                if (DialogResult.Yes == DirBox("Spawn timer"))
                {
                    Directory.CreateDirectory(txtTimerDir.Text);
                }
                else done = false;
            }

            if (done) Hide();

            DialogResult DirBox(string dir)
            {
                return MessageBox.Show($" {dir} directory doesn't exist.  Create it?", "Directory Not Found", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            }
        }

        private void SaveSettings()
        {
            // Set the Settings

                Settings.Default.IPAddress1 = txtIPAddress1.Text;

                Settings.Default.IPAddress2 = txtIPAddress2.Text;

                Settings.Default.IPAddress3 = txtIPAddress3.Text;

                Settings.Default.IPAddress4 = txtIPAddress4.Text;

                Settings.Default.IPAddress5 = txtIPAddress5.Text;

                Settings.Default.Port = int.Parse(txtPortNo.Text);

                Settings.Default.LevelOverride = (int)spnOverrideLevel.Value;

                Settings.Default.SaveOnExit = chkSaveOnExit.Checked;

                Settings.Default.UpdateDelay = (int)spnUpdateDelay.Value;

                Settings.Default.CorpseAlerts = chkCorpsesAlerts.Checked;

                Settings.Default.BackColor = picMapBackgroundColor.BackColor;

                Settings.Default.PCBorderColor = picPlayerBorder.BackColor;

            Settings.Default.PrefixStars = chkPrefixAlerts.Checked;

            Settings.Default.AffixStars = chkAffixAlerts.Checked;

            Settings.Default.HuntPrefix = txtHuntPrefix.Text;

            Settings.Default.MatchFullTextH = chkHuntMatchFull.Checked;  //hunt

            Settings.Default.NoneOnHunt = optHuntNone.Checked;

            Settings.Default.BeepOnHunt = optHuntBeep.Checked;

            Settings.Default.TalkOnHunt = optHuntSpeak.Checked;

            Settings.Default.PlayOnHunt = optHuntPlay.Checked;

            Settings.Default.HuntAudioFile = txtHuntAudioFile.Text;

            Settings.Default.CautionPrefix = txtCautionPrefix.Text;

            Settings.Default.MatchFullTextC = chkCautionMatchFull.Checked;  //Caution

            Settings.Default.NoneOnCaution = optCautionNone.Checked;

            Settings.Default.BeepOnCaution = optCautionBeep.Checked;

            Settings.Default.TalkOnCaution = optCautionSpeak.Checked;

            Settings.Default.PlayOnCaution = optCautionPlay.Checked;

            Settings.Default.CautionAudioFile = txtCautionAudioFile.Text;

            Settings.Default.DangerPrefix = txtDangerPrefix.Text;

            Settings.Default.MatchFullTextD = chkDangerMatchFull.Checked;  //Caution

            Settings.Default.NoneOnDanger = optDangerNone.Checked;

            Settings.Default.BeepOnDanger = optDangerBeep.Checked;

            Settings.Default.TalkOnDanger = optDangerSpeak.Checked;

            Settings.Default.PlayOnDanger = optDangerPlay.Checked;

            Settings.Default.DangerAudioFile = txtDangerAudioFile.Text;

            Settings.Default.AlertPrefix = txtAlertPrefix.Text;

            Settings.Default.MatchFullTextA = chkAlertMatchFull.Checked;  //Rare

            Settings.Default.NoneOnAlert = optAlertNone.Checked;

            Settings.Default.BeepOnAlert = optAlertBeep.Checked;

            Settings.Default.TalkOnAlert = optAlertSpeak.Checked;

            Settings.Default.PlayOnAlert = optAlertPlay.Checked;

            Settings.Default.AlertAudioFile = txtAlertAudioFile.Text;

            Settings.Default.RangeCircle = (int)spnRangeCircle.Value;

            Settings.Default.DrawOptions = GetDrawOptions();

            Settings.Default.ShowTargetInfo = chkShowTargetInfo.Checked;

            Settings.Default.ShowZoneName = chkShowZoneName.Checked;

            Settings.Default.ShowCharName = chkShowCharName.Checked;

            Settings.Default.DrawFoV = chkDrawFoV.Checked;

            Settings.Default.ColorRangeCircle = chkColorRangeCircle.Checked;

            Settings.Default.AlertSound = cmbAlertSound.SelectedItem.ToString();

            Settings.Default.HatchIndex = cmbHatch.SelectedItem.ToString();

            Settings.Default.SpawnDrawSize = (int)spnSpawnSize.Value;

            Settings.Default.FadedLines = (int)FadedLines.Value;

            Settings.Default.PVPLevels = (int)pvpLevels.Value;

            Settings.Default.MinAlertLevel = (int)numMinAlertLevel.Value;

            Settings.Default.TitleBar = txtWindowName.Text;

            Settings.Default.SearchString = txtSearchString.Text;

            Settings.Default.MapDir = txtMapDir.Text;

            Settings.Default.FilterDir = txtFilterDir.Text;

            Settings.Default.CfgDir = txtCfgDir.Text;

            Settings.Default.LogDir = txtLogDir.Text;

            Settings.Default.TimerDir = txtTimerDir.Text;

            Settings.Default.AutoSelectSpawnList = chkSelectSpawnList.Checked;

            Settings.Default.OptionsWindowsLocation = Location;

            Settings.Default.OptionsWindowsSize = Size;

            Settings.Default.MaxLogLevel = (LogLevel)spnLogLevel.Value;

            if (Settings.Default.CurrentIPAddress == 0 && txtIPAddress1.Text.Length > 0)
                Settings.Default.CurrentIPAddress = 1;

            Settings.Default.Save();
        }

        private void cmdMapBackgroundColor_Click(object sender, EventArgs e)

		{
			colorOptionPicker.Color = picMapBackgroundColor.BackColor;

			if(colorOptionPicker.ShowDialog() != DialogResult.Cancel)

			{
				picMapBackgroundColor.BackColor = colorOptionPicker.Color;
			}
		}

		private void cmdListBackgroundColor_Click(object sender, EventArgs e)

		{
			colorOptionPicker.Color = picListBackgroundColor.BackColor;

			if(colorOptionPicker.ShowDialog() != DialogResult.Cancel)

			{
				Settings.Default.ListBackColor = colorOptionPicker.Color;

				picListBackgroundColor.BackColor = colorOptionPicker.Color;
			}
		}

		private void cmdGridColor_Click(object sender, EventArgs e)

		{
			colorOptionPicker.Color = picGridColor.BackColor;

			if(colorOptionPicker.ShowDialog() != DialogResult.Cancel)

			{
				Settings.Default.GridColor = colorOptionPicker.Color;

				picGridColor.BackColor = colorOptionPicker.Color;
			}
		}

		private void cmdRangeCircleColor_Click(object sender, EventArgs e)

		{
			colorOptionPicker.Color = picRangeCircleColor.BackColor;

			if(colorOptionPicker.ShowDialog() != DialogResult.Cancel)

			{
				Settings.Default.RangeCircleColor  = colorOptionPicker.Color;

				picRangeCircleColor.BackColor = colorOptionPicker.Color;
			}
		}

		private void cmdMapDirBrowse_Click(object sender, EventArgs e)

		{
			fldrBrowser.Description = "Map Directory";

            fldrBrowser.SelectedPath = Settings.Default.MapDir;

			fldrBrowser.ShowDialog();

			if (fldrBrowser.SelectedPath.Trim() != "")
				txtMapDir.Text = fldrBrowser.SelectedPath;
		}

		private void cmdCfgDirBrowse_Click(object sender, EventArgs e)

		{
			fldrBrowser.Description = "Config Directory";

            fldrBrowser.SelectedPath = Settings.Default.CfgDir;

			fldrBrowser.ShowDialog();

            if (fldrBrowser.SelectedPath.Trim() != "")
                txtCfgDir.Text = fldrBrowser.SelectedPath;
		}

		private void cmdFilterDirBrowse_Click(object sender, EventArgs e)

		{
            fldrBrowser.Description = "Filter Directory";

            fldrBrowser.SelectedPath = Settings.Default.FilterDir;

			fldrBrowser.ShowDialog();

            if (fldrBrowser.SelectedPath.Trim() != "")
                txtFilterDir.Text = fldrBrowser.SelectedPath;
		}

		private void cmdLogDir_Click(object sender, EventArgs e)

		{
            fldrBrowser.Description = "Log Directory";

            fldrBrowser.SelectedPath = Settings.Default.LogDir;

			fldrBrowser.ShowDialog();

            if (fldrBrowser.SelectedPath.Trim() != "")
                txtLogDir.Text = fldrBrowser.SelectedPath;
		}

        private void cmdSpawnTimers_Click(object sender, EventArgs e)

        {
            fldrBrowser.Description = "Timers Directory";

            fldrBrowser.SelectedPath = Settings.Default.TimerDir;

            fldrBrowser.ShowDialog();

            if (fldrBrowser.SelectedPath.Trim() != "")
                txtTimerDir.Text = fldrBrowser.SelectedPath;
        }

		public DrawOptions GetDrawOptions()

		{
			DrawOptions DrawOpts = DrawOptions.DrawNone;

			if (chkMap.Checked) DrawOpts|=DrawOptions.DrawMap;

			if (chkAddjust.Checked) DrawOpts|=DrawOptions.Readjust;

			if (chkPlayer.Checked) DrawOpts|=DrawOptions.Player;

			if (chkLineToPoint.Checked) DrawOpts|=DrawOptions.SpotLine;

			if (chkSpawns.Checked) DrawOpts|=DrawOptions.Spawns;

			if (chkTrails.Checked) DrawOpts|=DrawOptions.SpawnTrails;

			if (chkGround.Checked) DrawOpts|=DrawOptions.GroundItems;

			if (chkTimers.Checked) DrawOpts|=DrawOptions.SpawnTimers;

			if (chkDirection.Checked) DrawOpts|=DrawOptions.DirectionLines;

			if (chkHighlight.Checked) DrawOpts|=DrawOptions.SpawnRings;

			if (chkGrid.Checked) DrawOpts|=DrawOptions.GridLines;

			if (chkText.Checked) DrawOpts|=DrawOptions.ZoneText;

			return DrawOpts;
		}

		public void SetFgDrawOptions(DrawOptions DrawOpts)

		{
			chkMap.Checked = (DrawOpts & DrawOptions.DrawMap) != DrawOptions.DrawNone;

			chkAddjust.Checked = (DrawOpts & DrawOptions.Readjust) != DrawOptions.DrawNone;

			chkPlayer.Checked = (DrawOpts & DrawOptions.Player) != DrawOptions.DrawNone;

			chkLineToPoint.Checked = (DrawOpts & DrawOptions.SpotLine) != DrawOptions.DrawNone;

			chkSpawns.Checked = (DrawOpts & DrawOptions.Spawns) != DrawOptions.DrawNone;

			chkTrails.Checked = (DrawOpts & DrawOptions.SpawnTrails) != DrawOptions.DrawNone;

			chkGround.Checked = (DrawOpts & DrawOptions.GroundItems) != DrawOptions.DrawNone;

			chkTimers.Checked = (DrawOpts & DrawOptions.SpawnTimers) != DrawOptions.DrawNone;

			chkDirection.Checked = (DrawOpts & DrawOptions.DirectionLines) != DrawOptions.DrawNone;

			chkHighlight.Checked = (DrawOpts & DrawOptions.SpawnRings) != DrawOptions.DrawNone;

			chkGrid.Checked = (DrawOpts & DrawOptions.GridLines) != DrawOptions.DrawNone;

			chkText.Checked = (DrawOpts & DrawOptions.ZoneText) != DrawOptions.DrawNone;
		}

		private void butPlayerBorder_Click(object sender, EventArgs e)

		{
			colorOptionPicker.Color = picPlayerBorder.BackColor;

            if (colorOptionPicker.ShowDialog() != DialogResult.Cancel)

			{
    			picPlayerBorder.BackColor = colorOptionPicker.Color;
			}
		}

        private void cmbAlertSound_SelectionChangeCommitted(object sender, EventArgs e)

        {
            Settings.Default.AlertSound = cmbAlertSound.SelectedItem.ToString();

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

        private void cmbHatch_SelectionChangeCommitted(object sender, EventArgs e)

        {
            Settings.Default.HatchIndex = cmbHatch.SelectedItem.ToString();
        }

        private void cmdGridLabelColor_Click(object sender, EventArgs e)

        {
            colorOptionPicker.Color = picGridLabelColor.BackColor;

            if (colorOptionPicker.ShowDialog() != DialogResult.Cancel)

            {
                Settings.Default.GridLabelColor = colorOptionPicker.Color;

                picGridLabelColor.BackColor = colorOptionPicker.Color;
            }
        }

        private void spnSpawnSize_ValueChanged(object sender, EventArgs e)

        {
            Settings.Default.SpawnDrawSize = (int)spnSpawnSize.Value;
        }

        private void pvpLevels_ValueChanged(object sender, EventArgs e)

        {
            Settings.Default.PVPLevels = (int)pvpLevels.Value;
        }
	}
}

