using Structures;
using System;
//using System.Text.RegularExpressions;

using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Media;
using System.Windows.Forms;

//using System.Net.Mail;

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
        //private GroupBox groupBox3;
        //private Label lblSMTPPort;
        //private TextBox textSMTPPort;
        //private Label lblSMTPUsername;
        //private Label lblSMTPAddress;
        //private TextBox textSMTPAddress;
        //private TextBox textSMTPUsername;
        //private Label lblSMTPPassword;
        //private TextBox textSMTPToEmail;
        //private Label lblToAddress;
        //private TextBox textSMTPFromEmail;
        //private Label lblFromAddress;
        //private TextBox textSMTPPassword;
        //private CheckBox checkBoxSMTPUseNetworkCredentials;
        //private CheckBox checkBoxSMTPUseSecureAuthentication;
        //private Button btnTestEmail;
        //private CheckBox checkBoxSavePassword;
        //private Label lblSMTPDomain;
        //private TextBox textBoxSMTPDomain;
        //private Label lblCCEmail;
        //private TextBox textSMTPCCEmail;
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

            //textSMTPAddress.Text = SmtpSettings.Instance.SmtpServer;
            //textSMTPPort.Text = SmtpSettings.Instance.SmtpPort.ToString();
            //textBoxSMTPDomain.Text = SmtpSettings.Instance.SmtpDomain;
            //textSMTPUsername.Text = SmtpSettings.Instance.SmtpUsername;

            //textSMTPPassword.Text = SmtpSettings.Instance.SmtpPassword.Length > 0 ? "Password:::1" : "";
            //textSMTPToEmail.Text = SmtpSettings.Instance.ToEmail;
            //textSMTPFromEmail.Text = SmtpSettings.Instance.FromEmail;
            //textSMTPCCEmail.Text = SmtpSettings.Instance.CCEmail;
            //checkBoxSMTPUseNetworkCredentials.Checked = SmtpSettings.Instance.UseNetworkCredentials;
            //checkBoxSMTPUseSecureAuthentication.Checked = SmtpSettings.Instance.UseSSL;
            //checkBoxSavePassword.Checked = SmtpSettings.Instance.SavePassword;

            //if (SmtpSettings.Instance.UseNetworkCredentials)
            //{
            //    textSMTPUsername.Enabled = false;
            //    textSMTPPassword.Enabled = false;
            //    lblSMTPPassword.Enabled = false;
            //    lblSMTPUsername.Enabled = false;
            //}
            //else
            //{
            //    textSMTPUsername.Enabled = true;
            //    textSMTPPassword.Enabled = true;
            //    lblSMTPPassword.Enabled = true;
            //    lblSMTPUsername.Enabled = true;
            //}
            cmbHatch.SelectedText = Settings.Instance.HatchIndex;

            cmbAlertSound.SelectedText = Settings.Instance.AlertSound;

            
            txtIPAddress1.Text = Settings.Instance.IPAddress1;

            txtIPAddress2.Text = Settings.Instance.IPAddress2;

            txtIPAddress3.Text = Settings.Instance.IPAddress3;

            txtIPAddress4.Text = Settings.Instance.IPAddress4;

            txtIPAddress5.Text = Settings.Instance.IPAddress5;

            txtPortNo.Text = Settings.Instance.Port.ToString();

            spnOverrideLevel.Value = Settings.Instance.LevelOverride;

            spnUpdateDelay.Value = Settings.Instance.UpdateDelay;

            chkSaveOnExit.Checked = Settings.Instance.SaveOnExit;

            chkPrefixAlerts.Checked = Settings.Instance.PrefixStars;

            chkAffixAlerts.Checked = Settings.Instance.AffixStars;       // affix

            chkCorpsesAlerts.Checked = Settings.Instance.CorpseAlerts;

            txtHuntPrefix.Text = Settings.Instance.HuntPrefix;

            chkHuntMatchFull.Checked = Settings.Instance.MatchFullTextH;  //hunt

            optHuntNone.Checked = Settings.Instance.NoneOnHunt;

            optHuntBeep.Checked =  Settings.Instance.BeepOnHunt;

            optHuntSpeak.Checked = Settings.Instance.TalkOnHunt;

            optHuntPlay.Checked = Settings.Instance.PlayOnHunt;

            txtHuntAudioFile.Text = Settings.Instance.HuntAudioFile;

            txtCautionPrefix.Text = Settings.Instance.CautionPrefix;

            chkCautionMatchFull.Checked = Settings.Instance.MatchFullTextC;  //Caution

            optCautionNone.Checked = Settings.Instance.NoneOnCaution;

            optCautionBeep.Checked = Settings.Instance.BeepOnCaution;

            optCautionSpeak.Checked = Settings.Instance.TalkOnCaution;

            optCautionPlay.Checked = Settings.Instance.PlayOnCaution;

            txtCautionAudioFile.Text = Settings.Instance.CautionAudioFile;

            txtDangerPrefix.Text = Settings.Instance.DangerPrefix;

            chkDangerMatchFull.Checked = Settings.Instance.MatchFullTextD;  //danger

            optDangerNone.Checked = Settings.Instance.NoneOnDanger;

            optDangerBeep.Checked = Settings.Instance.BeepOnDanger;

            optDangerSpeak.Checked = Settings.Instance.TalkOnDanger;

            optDangerPlay.Checked = Settings.Instance.PlayOnDanger;

            txtDangerAudioFile.Text = Settings.Instance.DangerAudioFile;

            txtAlertPrefix.Text = Settings.Instance.AlertPrefix;

            chkAlertMatchFull.Checked = Settings.Instance.MatchFullTextA;  //Rare

            optAlertNone.Checked = Settings.Instance.NoneOnAlert;

            optAlertBeep.Checked = Settings.Instance.BeepOnAlert;

            optAlertSpeak.Checked = Settings.Instance.TalkOnAlert;

            optAlertPlay.Checked = Settings.Instance.PlayOnAlert;

            txtAlertAudioFile.Text = Settings.Instance.AlertAudioFile;

            spnRangeCircle.Value = Settings.Instance.RangeCircle;

            numMinAlertLevel.Value = Settings.Instance.MinAlertLevel;

            spnSpawnSize.Value = Settings.Instance.SpawnDrawSize;

            FadedLines.Value = Settings.Instance.FadedLines;

            pvpLevels.Value = Settings.Instance.PVPLevels;

            txtWindowName.Text = Settings.Instance.TitleBar;

            txtSearchString.Text = Settings.Instance.SearchString;

            picMapBackgroundColor.BackColor = Settings.Instance.BackColor;

            picListBackgroundColor.BackColor = Settings.Instance.ListBackColor;

            picGridColor.BackColor = Settings.Instance.GridColor;

            picGridLabelColor.BackColor = Settings.Instance.GridLabelColor;

            picRangeCircleColor.BackColor = Settings.Instance.RangeCircleColor;

            picPlayerBorder.BackColor = Settings.Instance.PCBorderColor;

            chkColorRangeCircle.Checked = Settings.Instance.ColorRangeCircle;

            cmbAlertSound.SelectedItem = Settings.Instance.AlertSound;

            cmbHatch.SelectedItem = Settings.Instance.HatchIndex;

            chkDrawFoV.Checked = Settings.Instance.DrawFoV;

            chkShowZoneName.Checked = Settings.Instance.ShowZoneName;

            chkShowCharName.Checked = Settings.Instance.ShowCharName;

            chkShowTargetInfo.Checked = Settings.Instance.ShowTargetInfo;

            txtMapDir.Text = Settings.Instance.MapDir;

            txtFilterDir.Text = Settings.Instance.FilterDir;

            txtCfgDir.Text = Settings.Instance.CfgDir;

            txtLogDir.Text = Settings.Instance.LogDir;

            txtTimerDir.Text = Settings.Instance.TimerDir;

            spnLogLevel.Value = (int)Settings.Instance.MaxLogLevel;

            chkSelectSpawnList.Checked = Settings.Instance.AutoSelectSpawnList;

            SetFgDrawOptions(Settings.Instance.DrawOptions);
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
            this.cmdCommand = new Button();
            this.colorOptionPicker = new ColorDialog();
            this.tabColors = new TabPage();
            this.picGridLabelColor = new PictureBox();
            this.cmdGridLabelColor = new Button();
            this.picPlayerBorder = new PictureBox();
            this.butPlayerBorder = new Button();
            this.picListBackgroundColor = new PictureBox();
            this.cmdListBackgroundColor = new Button();
            this.picRangeCircleColor = new PictureBox();
            this.cmdRangeCircleColor = new Button();
            this.picGridColor = new PictureBox();
            this.cmdGridColor = new Button();
            this.picMapBackgroundColor = new PictureBox();
            this.cmdMapBackgroundColor = new Button();
            this.tabFolders = new TabPage();
            this.cmdSpawnTimers = new Button();
            this.txtTimerDir = new TextBox();
            this.txtLogDir = new TextBox();
            this.txtFilterDir = new TextBox();
            this.txtCfgDir = new TextBox();
            this.txtMapDir = new TextBox();
            this.lblSpawnTimers = new Label();
            this.cmdLogDir = new Button();
            this.lblLogDir = new Label();
            this.cmdFilterDirBrowse = new Button();
            this.lblFilterDir = new Label();
            this.cmdCfgDirBrowse = new Button();
            this.lblCfgDir = new Label();
            this.cmdMapDirBrowse = new Button();
            this.lblMapDir = new Label();
            this.tabAlerts = new TabPage();
            this.grpDanger = new GroupBox();
            this.optDangerPlay = new RadioButton();
            this.optDangerSpeak = new RadioButton();
            this.optDangerBeep = new RadioButton();
            this.optDangerNone = new RadioButton();
            this.txtDangerAudioFile = new TextBox();
            this.txtDangerPrefix = new TextBox();
            this.lblDangerPrefix = new Label();
            this.chkDangerMatchFull = new CheckBox();
            this.grpAlert = new GroupBox();
            this.optAlertPlay = new RadioButton();
            this.optAlertSpeak = new RadioButton();
            this.optAlertBeep = new RadioButton();
            this.optAlertNone = new RadioButton();
            this.txtAlertAudioFile = new TextBox();
            this.txtAlertPrefix = new TextBox();
            this.lblAlertPrefix = new Label();
            this.chkAlertMatchFull = new CheckBox();
            this.grpCaution = new GroupBox();
            this.optCautionPlay = new RadioButton();
            this.optCautionSpeak = new RadioButton();
            this.optCautionBeep = new RadioButton();
            this.optCautionNone = new RadioButton();
            this.txtCautionAudioFile = new TextBox();
            this.txtCautionPrefix = new TextBox();
            this.lblCautionPrefix = new Label();
            this.chkCautionMatchFull = new CheckBox();
            this.grpHunt = new GroupBox();
            this.optHuntPlay = new RadioButton();
            this.optHuntSpeak = new RadioButton();
            this.optHuntBeep = new RadioButton();
            this.optHuntNone = new RadioButton();
            this.txtHuntAudioFile = new TextBox();
            this.txtHuntPrefix = new TextBox();
            this.lblHuntPrefix = new Label();
            this.chkHuntMatchFull = new CheckBox();
            this.chkAffixAlerts = new CheckBox();
            this.chkPrefixAlerts = new CheckBox();
            this.chkCorpsesAlerts = new CheckBox();
            this.tabMap = new TabPage();
            this.FadedLines = new NumericUpDown();
            this.lblFadedLines = new Label();
            this.lblPVPLevels = new Label();
            this.pvpLevels = new NumericUpDown();
            this.groupBox1 = new GroupBox();
            this.cmbAlertSound = new ComboBox();
            this.cmbHatch = new ComboBox();
            this.label4 = new Label();
            this.label2 = new Label();
            this.chkColorRangeCircle = new CheckBox();
            this.spnRangeCircle = new NumericUpDown();
            this.numMinAlertLevel = new NumericUpDown();
            this.label1 = new Label();
            this.groupBox2 = new GroupBox();
            this.chkMap = new CheckBox();
            this.chkPlayer = new CheckBox();
            this.chkSpawns = new CheckBox();
            this.chkAddjust = new CheckBox();
            this.chkGround = new CheckBox();
            this.chkTrails = new CheckBox();
            this.chkHighlight = new CheckBox();
            this.chkGrid = new CheckBox();
            this.chkTimers = new CheckBox();
            this.chkText = new CheckBox();
            this.chkDirection = new CheckBox();
            this.chkLineToPoint = new CheckBox();
            this.lblSpawnSize = new Label();
            this.chkSelectSpawnList = new CheckBox();
            this.spnSpawnSize = new NumericUpDown();
            this.chkShowTargetInfo = new CheckBox();
            this.chkDrawFoV = new CheckBox();
            this.tabGeneral = new TabPage();
            this.chkShowCharName = new CheckBox();
            this.txtSearchString = new TextBox();
            this.lblSearch = new Label();
            this.spnLogLevel = new NumericUpDown();
            this.lblLogLevel = new Label();
            this.chkShowZoneName = new CheckBox();
            this.spnOverrideLevel = new NumericUpDown();
            this.spnUpdateDelay = new NumericUpDown();
            this.txtWindowName = new TextBox();
            this.lblWindowName = new Label();
            this.lblOverridelevel = new Label();
            this.gbServer = new GroupBox();
            this.txtIPAddress5 = new TextBox();
            this.lblIPAddress5 = new Label();
            this.txtIPAddress4 = new TextBox();
            this.lblIPAddress4 = new Label();
            this.txtIPAddress3 = new TextBox();
            this.lblIPAddress3 = new Label();
            this.txtIPAddress2 = new TextBox();
            this.lblIPAddress2 = new Label();
            this.txtPortNo = new TextBox();
            this.txtIPAddress1 = new TextBox();
            this.lblIPAddress1 = new Label();
            this.lbltxtPortNo = new Label();
            this.lblUpdateDelay = new Label();
            this.chkSaveOnExit = new CheckBox();
            this.tabOptions = new TabControl();
            this.tabPage1 = new TabPage();
            this.cmdCancel = new Button();
            this.tabColors.SuspendLayout();
            ((ISupportInitialize)(this.picGridLabelColor)).BeginInit();
            ((ISupportInitialize)(this.picPlayerBorder)).BeginInit();
            ((ISupportInitialize)(this.picListBackgroundColor)).BeginInit();
            ((ISupportInitialize)(this.picRangeCircleColor)).BeginInit();
            ((ISupportInitialize)(this.picGridColor)).BeginInit();
            ((ISupportInitialize)(this.picMapBackgroundColor)).BeginInit();
            this.tabFolders.SuspendLayout();
            this.tabAlerts.SuspendLayout();
            this.grpDanger.SuspendLayout();
            this.grpAlert.SuspendLayout();
            this.grpCaution.SuspendLayout();
            this.grpHunt.SuspendLayout();
            this.tabMap.SuspendLayout();
            ((ISupportInitialize)(this.FadedLines)).BeginInit();
            ((ISupportInitialize)(this.pvpLevels)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((ISupportInitialize)(this.spnRangeCircle)).BeginInit();
            ((ISupportInitialize)(this.numMinAlertLevel)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((ISupportInitialize)(this.spnSpawnSize)).BeginInit();
            this.tabGeneral.SuspendLayout();
            ((ISupportInitialize)(this.spnLogLevel)).BeginInit();
            ((ISupportInitialize)(this.spnOverrideLevel)).BeginInit();
            ((ISupportInitialize)(this.spnUpdateDelay)).BeginInit();
            this.gbServer.SuspendLayout();
            this.tabOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdCommand
            // 
            this.cmdCommand.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
            this.cmdCommand.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdCommand.Location = new Point(83, 410);
            this.cmdCommand.Name = "cmdCommand";
            this.cmdCommand.Size = new Size(85, 23);
            this.cmdCommand.TabIndex = 0;
            this.cmdCommand.Text = "&Save";
            this.cmdCommand.Click += new EventHandler(this.cmdCommand_Click);
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
            this.tabColors.Location = new Point(4, 22);
            this.tabColors.Name = "tabColors";
            this.tabColors.Size = new Size(266, 375);
            this.tabColors.TabIndex = 3;
            this.tabColors.Text = "Colors";
            // 
            // picGridLabelColor
            // 
            this.picGridLabelColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picGridLabelColor.Location = new Point(152, 164);
            this.picGridLabelColor.Name = "picGridLabelColor";
            this.picGridLabelColor.Size = new Size(104, 24);
            this.picGridLabelColor.TabIndex = 13;
            this.picGridLabelColor.TabStop = false;
            this.picGridLabelColor.Click += new EventHandler(this.cmdGridLabelColor_Click);
            // 
            // cmdGridLabelColor
            // 
            this.cmdGridLabelColor.Location = new Point(8, 164);
            this.cmdGridLabelColor.Name = "cmdGridLabelColor";
            this.cmdGridLabelColor.Size = new Size(136, 24);
            this.cmdGridLabelColor.TabIndex = 12;
            this.cmdGridLabelColor.Text = "Grid Label Color";
            this.cmdGridLabelColor.Click += new EventHandler(this.cmdGridLabelColor_Click);
            // 
            // picPlayerBorder
            // 
            this.picPlayerBorder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picPlayerBorder.Location = new Point(152, 134);
            this.picPlayerBorder.Name = "picPlayerBorder";
            this.picPlayerBorder.Size = new Size(104, 24);
            this.picPlayerBorder.TabIndex = 11;
            this.picPlayerBorder.TabStop = false;
            this.picPlayerBorder.Click += new EventHandler(this.butPlayerBorder_Click);
            // 
            // butPlayerBorder
            // 
            this.butPlayerBorder.Location = new Point(8, 134);
            this.butPlayerBorder.Name = "butPlayerBorder";
            this.butPlayerBorder.Size = new Size(136, 24);
            this.butPlayerBorder.TabIndex = 10;
            this.butPlayerBorder.Text = "PC Highlight Color";
            this.butPlayerBorder.Click += new EventHandler(this.butPlayerBorder_Click);
            // 
            // picListBackgroundColor
            // 
            this.picListBackgroundColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picListBackgroundColor.Location = new Point(152, 104);
            this.picListBackgroundColor.Name = "picListBackgroundColor";
            this.picListBackgroundColor.Size = new Size(104, 24);
            this.picListBackgroundColor.TabIndex = 7;
            this.picListBackgroundColor.TabStop = false;
            this.picListBackgroundColor.Click += new EventHandler(this.cmdListBackgroundColor_Click);
            // 
            // cmdListBackgroundColor
            // 
            this.cmdListBackgroundColor.Location = new Point(8, 104);
            this.cmdListBackgroundColor.Name = "cmdListBackgroundColor";
            this.cmdListBackgroundColor.Size = new Size(136, 24);
            this.cmdListBackgroundColor.TabIndex = 6;
            this.cmdListBackgroundColor.Text = "List Background";
            this.cmdListBackgroundColor.Click += new EventHandler(this.cmdListBackgroundColor_Click);
            // 
            // picRangeCircleColor
            // 
            this.picRangeCircleColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picRangeCircleColor.Location = new Point(152, 72);
            this.picRangeCircleColor.Name = "picRangeCircleColor";
            this.picRangeCircleColor.Size = new Size(104, 24);
            this.picRangeCircleColor.TabIndex = 5;
            this.picRangeCircleColor.TabStop = false;
            this.picRangeCircleColor.Click += new EventHandler(this.cmdRangeCircleColor_Click);
            // 
            // cmdRangeCircleColor
            // 
            this.cmdRangeCircleColor.Location = new Point(8, 72);
            this.cmdRangeCircleColor.Name = "cmdRangeCircleColor";
            this.cmdRangeCircleColor.Size = new Size(136, 24);
            this.cmdRangeCircleColor.TabIndex = 2;
            this.cmdRangeCircleColor.Text = "Range Circle";
            this.cmdRangeCircleColor.Click += new EventHandler(this.cmdRangeCircleColor_Click);
            // 
            // picGridColor
            // 
            this.picGridColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picGridColor.Location = new Point(152, 40);
            this.picGridColor.Name = "picGridColor";
            this.picGridColor.Size = new Size(104, 24);
            this.picGridColor.TabIndex = 3;
            this.picGridColor.TabStop = false;
            this.picGridColor.Click += new EventHandler(this.cmdGridColor_Click);
            // 
            // cmdGridColor
            // 
            this.cmdGridColor.Location = new Point(8, 40);
            this.cmdGridColor.Name = "cmdGridColor";
            this.cmdGridColor.Size = new Size(136, 24);
            this.cmdGridColor.TabIndex = 1;
            this.cmdGridColor.Text = "Grid";
            this.cmdGridColor.Click += new EventHandler(this.cmdGridColor_Click);
            // 
            // picMapBackgroundColor
            // 
            this.picMapBackgroundColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picMapBackgroundColor.Location = new Point(152, 8);
            this.picMapBackgroundColor.Name = "picMapBackgroundColor";
            this.picMapBackgroundColor.Size = new Size(104, 24);
            this.picMapBackgroundColor.TabIndex = 1;
            this.picMapBackgroundColor.TabStop = false;
            this.picMapBackgroundColor.Click += new EventHandler(this.cmdMapBackgroundColor_Click);
            // 
            // cmdMapBackgroundColor
            // 
            this.cmdMapBackgroundColor.Location = new Point(8, 8);
            this.cmdMapBackgroundColor.Name = "cmdMapBackgroundColor";
            this.cmdMapBackgroundColor.Size = new Size(136, 24);
            this.cmdMapBackgroundColor.TabIndex = 0;
            this.cmdMapBackgroundColor.Text = "Map Background";
            this.cmdMapBackgroundColor.Click += new EventHandler(this.cmdMapBackgroundColor_Click);
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
            this.tabFolders.Location = new Point(4, 22);
            this.tabFolders.Name = "tabFolders";
            this.tabFolders.Size = new Size(266, 375);
            this.tabFolders.TabIndex = 4;
            this.tabFolders.Text = "Folders";
            // 
            // cmdSpawnTimers
            // 
            this.cmdSpawnTimers.Location = new Point(232, 184);
            this.cmdSpawnTimers.Name = "cmdSpawnTimers";
            this.cmdSpawnTimers.Size = new Size(24, 23);
            this.cmdSpawnTimers.TabIndex = 40;
            this.cmdSpawnTimers.Text = "...";
            this.cmdSpawnTimers.Click += new EventHandler(this.cmdSpawnTimers_Click);
            // 
            // txtTimerDir
            // 
            this.txtTimerDir.BackColor = System.Drawing.Color.White;
            this.txtTimerDir.Location = new Point(8, 184);
            this.txtTimerDir.Name = "txtTimerDir";
            this.txtTimerDir.Size = new Size(216, 20);
            this.txtTimerDir.TabIndex = 39;
            // 
            // txtLogDir
            // 
            this.txtLogDir.BackColor = System.Drawing.Color.White;
            this.txtLogDir.Location = new Point(8, 144);
            this.txtLogDir.Name = "txtLogDir";
            this.txtLogDir.Size = new Size(216, 20);
            this.txtLogDir.TabIndex = 36;
            // 
            // txtFilterDir
            // 
            this.txtFilterDir.BackColor = System.Drawing.Color.White;
            this.txtFilterDir.Location = new Point(8, 104);
            this.txtFilterDir.Name = "txtFilterDir";
            this.txtFilterDir.Size = new Size(216, 20);
            this.txtFilterDir.TabIndex = 33;
            // 
            // txtCfgDir
            // 
            this.txtCfgDir.BackColor = System.Drawing.Color.White;
            this.txtCfgDir.Location = new Point(8, 64);
            this.txtCfgDir.Name = "txtCfgDir";
            this.txtCfgDir.Size = new Size(216, 20);
            this.txtCfgDir.TabIndex = 30;
            // 
            // txtMapDir
            // 
            this.txtMapDir.BackColor = System.Drawing.Color.White;
            this.txtMapDir.Location = new Point(8, 24);
            this.txtMapDir.Name = "txtMapDir";
            this.txtMapDir.Size = new Size(216, 20);
            this.txtMapDir.TabIndex = 27;
            // 
            // lblSpawnTimers
            // 
            this.lblSpawnTimers.Location = new Point(8, 168);
            this.lblSpawnTimers.Name = "lblSpawnTimers";
            this.lblSpawnTimers.Size = new Size(144, 16);
            this.lblSpawnTimers.TabIndex = 38;
            this.lblSpawnTimers.Text = "Spawn Timers";
            // 
            // cmdLogDir
            // 
            this.cmdLogDir.Location = new Point(232, 144);
            this.cmdLogDir.Name = "cmdLogDir";
            this.cmdLogDir.Size = new Size(24, 23);
            this.cmdLogDir.TabIndex = 37;
            this.cmdLogDir.Text = "...";
            this.cmdLogDir.Click += new EventHandler(this.cmdLogDir_Click);
            // 
            // lblLogDir
            // 
            this.lblLogDir.Location = new Point(8, 128);
            this.lblLogDir.Name = "lblLogDir";
            this.lblLogDir.Size = new Size(144, 16);
            this.lblLogDir.TabIndex = 35;
            this.lblLogDir.Text = "Log Folder";
            // 
            // cmdFilterDirBrowse
            // 
            this.cmdFilterDirBrowse.Location = new Point(232, 104);
            this.cmdFilterDirBrowse.Name = "cmdFilterDirBrowse";
            this.cmdFilterDirBrowse.Size = new Size(24, 23);
            this.cmdFilterDirBrowse.TabIndex = 34;
            this.cmdFilterDirBrowse.Text = "...";
            this.cmdFilterDirBrowse.Click += new EventHandler(this.cmdFilterDirBrowse_Click);
            // 
            // lblFilterDir
            // 
            this.lblFilterDir.Location = new Point(8, 88);
            this.lblFilterDir.Name = "lblFilterDir";
            this.lblFilterDir.Size = new Size(144, 16);
            this.lblFilterDir.TabIndex = 32;
            this.lblFilterDir.Text = "Filter Folder";
            // 
            // cmdCfgDirBrowse
            // 
            this.cmdCfgDirBrowse.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCfgDirBrowse.Location = new Point(232, 64);
            this.cmdCfgDirBrowse.Name = "cmdCfgDirBrowse";
            this.cmdCfgDirBrowse.Size = new Size(24, 23);
            this.cmdCfgDirBrowse.TabIndex = 31;
            this.cmdCfgDirBrowse.Text = "...";
            this.cmdCfgDirBrowse.Click += new EventHandler(this.cmdCfgDirBrowse_Click);
            // 
            // lblCfgDir
            // 
            this.lblCfgDir.Location = new Point(8, 48);
            this.lblCfgDir.Name = "lblCfgDir";
            this.lblCfgDir.Size = new Size(144, 16);
            this.lblCfgDir.TabIndex = 29;
            this.lblCfgDir.Text = "Config Folder";
            // 
            // cmdMapDirBrowse
            // 
            this.cmdMapDirBrowse.Location = new Point(232, 24);
            this.cmdMapDirBrowse.Name = "cmdMapDirBrowse";
            this.cmdMapDirBrowse.Size = new Size(24, 23);
            this.cmdMapDirBrowse.TabIndex = 28;
            this.cmdMapDirBrowse.Text = "...";
            this.cmdMapDirBrowse.Click += new EventHandler(this.cmdMapDirBrowse_Click);
            // 
            // lblMapDir
            // 
            this.lblMapDir.Location = new Point(8, 8);
            this.lblMapDir.Name = "lblMapDir";
            this.lblMapDir.Size = new Size(144, 16);
            this.lblMapDir.TabIndex = 26;
            this.lblMapDir.Text = "Map Folder";
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
            this.tabAlerts.Location = new Point(4, 22);
            this.tabAlerts.Name = "tabAlerts";
            this.tabAlerts.Size = new Size(266, 375);
            this.tabAlerts.TabIndex = 1;
            this.tabAlerts.Text = "Filters";
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
            this.grpDanger.Location = new Point(4, 209);
            this.grpDanger.Name = "grpDanger";
            this.grpDanger.Size = new Size(248, 78);
            this.grpDanger.TabIndex = 23;
            this.grpDanger.TabStop = false;
            this.grpDanger.Text = "Danger";
            // 
            // optDangerPlay
            // 
            this.optDangerPlay.Location = new Point(8, 55);
            this.optDangerPlay.Name = "optDangerPlay";
            this.optDangerPlay.Size = new Size(80, 16);
            this.optDangerPlay.TabIndex = 18;
            this.optDangerPlay.Text = "Play Wav";
            // 
            // optDangerSpeak
            // 
            this.optDangerSpeak.Location = new Point(167, 35);
            this.optDangerSpeak.Name = "optDangerSpeak";
            this.optDangerSpeak.Size = new Size(72, 16);
            this.optDangerSpeak.TabIndex = 17;
            this.optDangerSpeak.Text = "Speak";
            // 
            // optDangerBeep
            // 
            this.optDangerBeep.Location = new Point(88, 35);
            this.optDangerBeep.Name = "optDangerBeep";
            this.optDangerBeep.Size = new Size(72, 16);
            this.optDangerBeep.TabIndex = 16;
            this.optDangerBeep.Text = "Beep";
            // 
            // optDangerNone
            // 
            this.optDangerNone.Checked = true;
            this.optDangerNone.Location = new Point(8, 35);
            this.optDangerNone.Name = "optDangerNone";
            this.optDangerNone.Size = new Size(72, 16);
            this.optDangerNone.TabIndex = 15;
            this.optDangerNone.TabStop = true;
            this.optDangerNone.Text = "None";
            // 
            // txtDangerAudioFile
            // 
            this.txtDangerAudioFile.Location = new Point(88, 55);
            this.txtDangerAudioFile.Name = "txtDangerAudioFile";
            this.txtDangerAudioFile.Size = new Size(152, 20);
            this.txtDangerAudioFile.TabIndex = 19;
            // 
            // txtDangerPrefix
            // 
            this.txtDangerPrefix.Location = new Point(88, 11);
            this.txtDangerPrefix.MaxLength = 5;
            this.txtDangerPrefix.Name = "txtDangerPrefix";
            this.txtDangerPrefix.Size = new Size(32, 20);
            this.txtDangerPrefix.TabIndex = 13;
            this.txtDangerPrefix.Text = "[D]";
            // 
            // lblDangerPrefix
            // 
            this.lblDangerPrefix.Location = new Point(8, 14);
            this.lblDangerPrefix.Name = "lblDangerPrefix";
            this.lblDangerPrefix.Size = new Size(79, 16);
            this.lblDangerPrefix.TabIndex = 12;
            this.lblDangerPrefix.Text = "Prefix/Suffix:";
            // 
            // chkDangerMatchFull
            // 
            this.chkDangerMatchFull.Location = new Point(136, 11);
            this.chkDangerMatchFull.Name = "chkDangerMatchFull";
            this.chkDangerMatchFull.Size = new Size(104, 24);
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
            this.grpAlert.Location = new Point(4, 293);
            this.grpAlert.Name = "grpAlert";
            this.grpAlert.Size = new Size(248, 78);
            this.grpAlert.TabIndex = 20;
            this.grpAlert.TabStop = false;
            this.grpAlert.Text = "Rare";
            // 
            // optAlertPlay
            // 
            this.optAlertPlay.Location = new Point(8, 55);
            this.optAlertPlay.Name = "optAlertPlay";
            this.optAlertPlay.Size = new Size(80, 16);
            this.optAlertPlay.TabIndex = 27;
            this.optAlertPlay.Text = "Play Wav";
            // 
            // optAlertSpeak
            // 
            this.optAlertSpeak.Location = new Point(168, 35);
            this.optAlertSpeak.Name = "optAlertSpeak";
            this.optAlertSpeak.Size = new Size(72, 16);
            this.optAlertSpeak.TabIndex = 26;
            this.optAlertSpeak.Text = "Speak";
            // 
            // optAlertBeep
            // 
            this.optAlertBeep.Location = new Point(88, 35);
            this.optAlertBeep.Name = "optAlertBeep";
            this.optAlertBeep.Size = new Size(72, 16);
            this.optAlertBeep.TabIndex = 25;
            this.optAlertBeep.Text = "Beep";
            // 
            // optAlertNone
            // 
            this.optAlertNone.Checked = true;
            this.optAlertNone.Location = new Point(8, 35);
            this.optAlertNone.Name = "optAlertNone";
            this.optAlertNone.Size = new Size(72, 16);
            this.optAlertNone.TabIndex = 24;
            this.optAlertNone.TabStop = true;
            this.optAlertNone.Text = "None";
            // 
            // txtAlertAudioFile
            // 
            this.txtAlertAudioFile.Location = new Point(88, 55);
            this.txtAlertAudioFile.Name = "txtAlertAudioFile";
            this.txtAlertAudioFile.Size = new Size(152, 20);
            this.txtAlertAudioFile.TabIndex = 28;
            // 
            // txtAlertPrefix
            // 
            this.txtAlertPrefix.Location = new Point(88, 11);
            this.txtAlertPrefix.MaxLength = 5;
            this.txtAlertPrefix.Name = "txtAlertPrefix";
            this.txtAlertPrefix.Size = new Size(32, 20);
            this.txtAlertPrefix.TabIndex = 22;
            this.txtAlertPrefix.Text = "[R]";
            // 
            // lblAlertPrefix
            // 
            this.lblAlertPrefix.Location = new Point(8, 14);
            this.lblAlertPrefix.Name = "lblAlertPrefix";
            this.lblAlertPrefix.Size = new Size(79, 16);
            this.lblAlertPrefix.TabIndex = 21;
            this.lblAlertPrefix.Text = "Prefix/Suffix:";
            // 
            // chkAlertMatchFull
            // 
            this.chkAlertMatchFull.Location = new Point(136, 11);
            this.chkAlertMatchFull.Name = "chkAlertMatchFull";
            this.chkAlertMatchFull.Size = new Size(104, 24);
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
            this.grpCaution.Location = new Point(3, 125);
            this.grpCaution.Name = "grpCaution";
            this.grpCaution.Size = new Size(248, 78);
            this.grpCaution.TabIndex = 11;
            this.grpCaution.TabStop = false;
            this.grpCaution.Text = "Caution";
            // 
            // optCautionPlay
            // 
            this.optCautionPlay.Location = new Point(8, 55);
            this.optCautionPlay.Name = "optCautionPlay";
            this.optCautionPlay.Size = new Size(80, 16);
            this.optCautionPlay.TabIndex = 18;
            this.optCautionPlay.Text = "Play Wav";
            // 
            // optCautionSpeak
            // 
            this.optCautionSpeak.Location = new Point(168, 38);
            this.optCautionSpeak.Name = "optCautionSpeak";
            this.optCautionSpeak.Size = new Size(72, 16);
            this.optCautionSpeak.TabIndex = 17;
            this.optCautionSpeak.Text = "Speak";
            // 
            // optCautionBeep
            // 
            this.optCautionBeep.Location = new Point(88, 35);
            this.optCautionBeep.Name = "optCautionBeep";
            this.optCautionBeep.Size = new Size(72, 16);
            this.optCautionBeep.TabIndex = 16;
            this.optCautionBeep.Text = "Beep";
            // 
            // optCautionNone
            // 
            this.optCautionNone.Checked = true;
            this.optCautionNone.Location = new Point(8, 35);
            this.optCautionNone.Name = "optCautionNone";
            this.optCautionNone.Size = new Size(72, 16);
            this.optCautionNone.TabIndex = 15;
            this.optCautionNone.TabStop = true;
            this.optCautionNone.Text = "None";
            // 
            // txtCautionAudioFile
            // 
            this.txtCautionAudioFile.Location = new Point(88, 55);
            this.txtCautionAudioFile.Name = "txtCautionAudioFile";
            this.txtCautionAudioFile.Size = new Size(152, 20);
            this.txtCautionAudioFile.TabIndex = 19;
            // 
            // txtCautionPrefix
            // 
            this.txtCautionPrefix.Location = new Point(88, 11);
            this.txtCautionPrefix.MaxLength = 5;
            this.txtCautionPrefix.Name = "txtCautionPrefix";
            this.txtCautionPrefix.Size = new Size(32, 20);
            this.txtCautionPrefix.TabIndex = 13;
            this.txtCautionPrefix.Text = "[C]";
            // 
            // lblCautionPrefix
            // 
            this.lblCautionPrefix.Location = new Point(8, 14);
            this.lblCautionPrefix.Name = "lblCautionPrefix";
            this.lblCautionPrefix.Size = new Size(80, 16);
            this.lblCautionPrefix.TabIndex = 12;
            this.lblCautionPrefix.Text = "Prefix/Suffix:";
            // 
            // chkCautionMatchFull
            // 
            this.chkCautionMatchFull.Location = new Point(136, 11);
            this.chkCautionMatchFull.Name = "chkCautionMatchFull";
            this.chkCautionMatchFull.Size = new Size(104, 24);
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
            this.grpHunt.Location = new Point(3, 41);
            this.grpHunt.Name = "grpHunt";
            this.grpHunt.Size = new Size(248, 78);
            this.grpHunt.TabIndex = 2;
            this.grpHunt.TabStop = false;
            this.grpHunt.Text = "Hunt";
            // 
            // optHuntPlay
            // 
            this.optHuntPlay.Location = new Point(8, 55);
            this.optHuntPlay.Name = "optHuntPlay";
            this.optHuntPlay.Size = new Size(80, 16);
            this.optHuntPlay.TabIndex = 9;
            this.optHuntPlay.Text = "Play Wav";
            // 
            // optHuntSpeak
            // 
            this.optHuntSpeak.Location = new Point(168, 35);
            this.optHuntSpeak.Name = "optHuntSpeak";
            this.optHuntSpeak.Size = new Size(72, 16);
            this.optHuntSpeak.TabIndex = 8;
            this.optHuntSpeak.Text = "Speak";
            // 
            // optHuntBeep
            // 
            this.optHuntBeep.Location = new Point(88, 35);
            this.optHuntBeep.Name = "optHuntBeep";
            this.optHuntBeep.Size = new Size(72, 16);
            this.optHuntBeep.TabIndex = 7;
            this.optHuntBeep.Text = "Beep";
            // 
            // optHuntNone
            // 
            this.optHuntNone.Checked = true;
            this.optHuntNone.Location = new Point(8, 35);
            this.optHuntNone.Name = "optHuntNone";
            this.optHuntNone.Size = new Size(72, 16);
            this.optHuntNone.TabIndex = 6;
            this.optHuntNone.TabStop = true;
            this.optHuntNone.Text = "None";
            // 
            // txtHuntAudioFile
            // 
            this.txtHuntAudioFile.Location = new Point(88, 55);
            this.txtHuntAudioFile.Name = "txtHuntAudioFile";
            this.txtHuntAudioFile.Size = new Size(152, 20);
            this.txtHuntAudioFile.TabIndex = 10;
            // 
            // txtHuntPrefix
            // 
            this.txtHuntPrefix.Location = new Point(88, 11);
            this.txtHuntPrefix.MaxLength = 5;
            this.txtHuntPrefix.Name = "txtHuntPrefix";
            this.txtHuntPrefix.Size = new Size(32, 20);
            this.txtHuntPrefix.TabIndex = 4;
            this.txtHuntPrefix.Text = "[H]";
            // 
            // lblHuntPrefix
            // 
            this.lblHuntPrefix.Location = new Point(8, 14);
            this.lblHuntPrefix.Name = "lblHuntPrefix";
            this.lblHuntPrefix.Size = new Size(80, 16);
            this.lblHuntPrefix.TabIndex = 3;
            this.lblHuntPrefix.Text = "Prefix/Suffix:";
            // 
            // chkHuntMatchFull
            // 
            this.chkHuntMatchFull.Location = new Point(136, 11);
            this.chkHuntMatchFull.Name = "chkHuntMatchFull";
            this.chkHuntMatchFull.Size = new Size(104, 24);
            this.chkHuntMatchFull.TabIndex = 5;
            this.chkHuntMatchFull.Text = "Match Full Text";
            // 
            // chkAffixAlerts
            // 
            this.chkAffixAlerts.Checked = true;
            this.chkAffixAlerts.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAffixAlerts.Location = new Point(4, 22);
            this.chkAffixAlerts.Name = "chkAffixAlerts";
            this.chkAffixAlerts.Size = new Size(112, 24);
            this.chkAffixAlerts.TabIndex = 1;
            this.chkAffixAlerts.Text = "Attach Suffix Text";
            // 
            // chkPrefixAlerts
            // 
            this.chkPrefixAlerts.Checked = true;
            this.chkPrefixAlerts.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPrefixAlerts.Location = new Point(4, 3);
            this.chkPrefixAlerts.Name = "chkPrefixAlerts";
            this.chkPrefixAlerts.Size = new Size(120, 24);
            this.chkPrefixAlerts.TabIndex = 0;
            this.chkPrefixAlerts.Text = "Attach Prefix Text";
            // 
            // chkCorpsesAlerts
            // 
            this.chkCorpsesAlerts.Checked = true;
            this.chkCorpsesAlerts.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCorpsesAlerts.Location = new Point(152, 3);
            this.chkCorpsesAlerts.Name = "chkCorpsesAlerts";
            this.chkCorpsesAlerts.Size = new Size(100, 24);
            this.chkCorpsesAlerts.TabIndex = 24;
            this.chkCorpsesAlerts.Text = "Match Corpses";
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
            this.tabMap.Location = new Point(4, 22);
            this.tabMap.Name = "tabMap";
            this.tabMap.Size = new Size(266, 375);
            this.tabMap.TabIndex = 2;
            this.tabMap.Text = "Map";
            // 
            // FadedLines
            // 
            this.FadedLines.Location = new Point(175, 99);
            this.FadedLines.Name = "FadedLines";
            this.FadedLines.Size = new Size(64, 20);
            this.FadedLines.TabIndex = 61;
            this.FadedLines.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // lblFadedLines
            // 
            this.lblFadedLines.AutoSize = true;
            this.lblFadedLines.Location = new Point(6, 99);
            this.lblFadedLines.Name = "lblFadedLines";
            this.lblFadedLines.Size = new Size(139, 13);
            this.lblFadedLines.TabIndex = 60;
            this.lblFadedLines.Text = "Dynamic Alpha Faded Lines";
            // 
            // lblPVPLevels
            // 
            this.lblPVPLevels.Location = new Point(6, 76);
            this.lblPVPLevels.Name = "lblPVPLevels";
            this.lblPVPLevels.Size = new Size(142, 16);
            this.lblPVPLevels.TabIndex = 58;
            this.lblPVPLevels.Text = "PVP Level Range:";
            // 
            // pvpLevels
            // 
            this.pvpLevels.Location = new Point(175, 74);
            this.pvpLevels.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.pvpLevels.Name = "pvpLevels";
            this.pvpLevels.Size = new Size(64, 20);
            this.pvpLevels.TabIndex = 59;
            this.pvpLevels.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.pvpLevels.ValueChanged += new EventHandler(this.pvpLevels_ValueChanged);
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
            this.groupBox1.Location = new Point(3, 119);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(258, 116);
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
            this.cmbAlertSound.Location = new Point(104, 67);
            this.cmbAlertSound.Name = "cmbAlertSound";
            this.cmbAlertSound.Size = new Size(133, 21);
            this.cmbAlertSound.TabIndex = 54;
            this.cmbAlertSound.SelectionChangeCommitted += new EventHandler(this.cmbAlertSound_SelectionChangeCommitted);
            // 
            // cmbHatch
            // 
            this.cmbHatch.FormattingEnabled = true;
            this.cmbHatch.Location = new Point(104, 42);
            this.cmbHatch.Name = "cmbHatch";
            this.cmbHatch.Size = new Size(133, 21);
            this.cmbHatch.TabIndex = 53;
            this.cmbHatch.Tag = "";
            this.cmbHatch.SelectionChangeCommitted += new EventHandler(this.cmbHatch_SelectionChangeCommitted);
            // 
            // label4
            // 
            this.label4.Location = new Point(4, 70);
            this.label4.Name = "label4";
            this.label4.Size = new Size(103, 16);
            this.label4.TabIndex = 58;
            this.label4.Text = "Alert Sound";
            // 
            // label2
            // 
            this.label2.Location = new Point(4, 45);
            this.label2.Name = "label2";
            this.label2.Size = new Size(103, 16);
            this.label2.TabIndex = 57;
            this.label2.Text = "Hatch Pattern";
            // 
            // chkColorRangeCircle
            // 
            this.chkColorRangeCircle.Location = new Point(7, 20);
            this.chkColorRangeCircle.Name = "chkColorRangeCircle";
            this.chkColorRangeCircle.Size = new Size(108, 16);
            this.chkColorRangeCircle.TabIndex = 5;
            this.chkColorRangeCircle.Text = "Range Circle";
            // 
            // spnRangeCircle
            // 
            this.spnRangeCircle.Location = new Point(173, 19);
            this.spnRangeCircle.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.spnRangeCircle.Name = "spnRangeCircle";
            this.spnRangeCircle.Size = new Size(64, 20);
            this.spnRangeCircle.TabIndex = 7;
            this.spnRangeCircle.Value = new decimal(new int[] {
            150,
            0,
            0,
            0});
            // 
            // numMinAlertLevel
            // 
            this.numMinAlertLevel.Location = new Point(172, 91);
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
            this.numMinAlertLevel.Size = new Size(64, 20);
            this.numMinAlertLevel.TabIndex = 55;
            this.numMinAlertLevel.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.Location = new Point(4, 93);
            this.label1.Name = "label1";
            this.label1.Size = new Size(142, 16);
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
            this.groupBox2.Location = new Point(3, 241);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(258, 130);
            this.groupBox2.TabIndex = 52;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Map Drawing Settings";
            // 
            // chkMap
            // 
            this.chkMap.Location = new Point(142, 53);
            this.chkMap.Name = "chkMap";
            this.chkMap.Size = new Size(94, 20);
            this.chkMap.TabIndex = 30;
            this.chkMap.Text = "Draw Map";
            // 
            // chkPlayer
            // 
            this.chkPlayer.Location = new Point(142, 70);
            this.chkPlayer.Name = "chkPlayer";
            this.chkPlayer.Size = new Size(94, 20);
            this.chkPlayer.TabIndex = 50;
            this.chkPlayer.Text = "Draw Player";
            // 
            // chkSpawns
            // 
            this.chkSpawns.Location = new Point(142, 36);
            this.chkSpawns.Name = "chkSpawns";
            this.chkSpawns.Size = new Size(94, 20);
            this.chkSpawns.TabIndex = 43;
            this.chkSpawns.Text = "Draw Spawns";
            // 
            // chkAddjust
            // 
            this.chkAddjust.Location = new Point(6, 19);
            this.chkAddjust.Name = "chkAddjust";
            this.chkAddjust.Size = new Size(121, 20);
            this.chkAddjust.TabIndex = 29;
            this.chkAddjust.Text = "Readjust Map";
            // 
            // chkGround
            // 
            this.chkGround.Location = new Point(142, 104);
            this.chkGround.Name = "chkGround";
            this.chkGround.Size = new Size(111, 20);
            this.chkGround.TabIndex = 51;
            this.chkGround.Text = "Ground Spawns";
            // 
            // chkTrails
            // 
            this.chkTrails.Location = new Point(6, 87);
            this.chkTrails.Name = "chkTrails";
            this.chkTrails.Size = new Size(130, 20);
            this.chkTrails.TabIndex = 45;
            this.chkTrails.Text = "Spawn Trails";
            // 
            // chkHighlight
            // 
            this.chkHighlight.Location = new Point(6, 104);
            this.chkHighlight.Name = "chkHighlight";
            this.chkHighlight.Size = new Size(130, 20);
            this.chkHighlight.TabIndex = 49;
            this.chkHighlight.Text = "Highlight Merchants";
            // 
            // chkGrid
            // 
            this.chkGrid.Location = new Point(6, 36);
            this.chkGrid.Name = "chkGrid";
            this.chkGrid.Size = new Size(130, 20);
            this.chkGrid.TabIndex = 37;
            this.chkGrid.Text = "Show Gridlines";
            // 
            // chkTimers
            // 
            this.chkTimers.Location = new Point(142, 19);
            this.chkTimers.Name = "chkTimers";
            this.chkTimers.Size = new Size(116, 20);
            this.chkTimers.TabIndex = 47;
            this.chkTimers.Text = "Spawn Timers";
            // 
            // chkText
            // 
            this.chkText.Location = new Point(6, 70);
            this.chkText.Name = "chkText";
            this.chkText.Size = new Size(130, 20);
            this.chkText.TabIndex = 41;
            this.chkText.Text = "Show Zone Text";
            // 
            // chkDirection
            // 
            this.chkDirection.Location = new Point(142, 87);
            this.chkDirection.Name = "chkDirection";
            this.chkDirection.Size = new Size(109, 20);
            this.chkDirection.TabIndex = 46;
            this.chkDirection.Text = "Heading Lines";
            // 
            // chkLineToPoint
            // 
            this.chkLineToPoint.Location = new Point(6, 53);
            this.chkLineToPoint.Name = "chkLineToPoint";
            this.chkLineToPoint.Size = new Size(130, 20);
            this.chkLineToPoint.TabIndex = 42;
            this.chkLineToPoint.Text = "Draw Line to Point";
            // 
            // lblSpawnSize
            // 
            this.lblSpawnSize.Location = new Point(6, 51);
            this.lblSpawnSize.Name = "lblSpawnSize";
            this.lblSpawnSize.Size = new Size(142, 16);
            this.lblSpawnSize.TabIndex = 20;
            this.lblSpawnSize.Text = "Spawn Draw Size:";
            // 
            // chkSelectSpawnList
            // 
            this.chkSelectSpawnList.Location = new Point(8, 32);
            this.chkSelectSpawnList.Name = "chkSelectSpawnList";
            this.chkSelectSpawnList.Size = new Size(248, 16);
            this.chkSelectSpawnList.TabIndex = 4;
            this.chkSelectSpawnList.Text = "Auto Select Spawn in the Spawn List";
            // 
            // spnSpawnSize
            // 
            this.spnSpawnSize.Location = new Point(175, 49);
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
            this.spnSpawnSize.Size = new Size(64, 20);
            this.spnSpawnSize.TabIndex = 21;
            this.spnSpawnSize.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.spnSpawnSize.ValueChanged += new EventHandler(this.spnSpawnSize_ValueChanged);
            // 
            // chkShowTargetInfo
            // 
            this.chkShowTargetInfo.Location = new Point(8, 12);
            this.chkShowTargetInfo.Name = "chkShowTargetInfo";
            this.chkShowTargetInfo.Size = new Size(248, 16);
            this.chkShowTargetInfo.TabIndex = 3;
            this.chkShowTargetInfo.Text = "Show Target Information Window";
            // 
            // chkDrawFoV
            // 
            this.chkDrawFoV.Location = new Point(8, -24);
            this.chkDrawFoV.Name = "chkDrawFoV";
            this.chkDrawFoV.Size = new Size(248, 16);
            this.chkDrawFoV.TabIndex = 2;
            this.chkDrawFoV.Text = "Draw Field of View (FoV)";
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
            this.tabGeneral.Location = new Point(4, 22);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Size = new Size(266, 375);
            this.tabGeneral.TabIndex = 0;
            this.tabGeneral.Text = "General";
            // 
            // chkShowCharName
            // 
            this.chkShowCharName.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkShowCharName.Checked = true;
            this.chkShowCharName.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowCharName.Location = new Point(134, 320);
            this.chkShowCharName.Name = "chkShowCharName";
            this.chkShowCharName.Size = new Size(114, 24);
            this.chkShowCharName.TabIndex = 27;
            this.chkShowCharName.Text = "Show Char Name";
            // 
            // txtSearchString
            // 
            this.txtSearchString.Location = new Point(61, 344);
            this.txtSearchString.Name = "txtSearchString";
            this.txtSearchString.Size = new Size(195, 20);
            this.txtSearchString.TabIndex = 26;
            // 
            // lblSearch
            // 
            this.lblSearch.Location = new Point(8, 347);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new Size(47, 17);
            this.lblSearch.TabIndex = 25;
            this.lblSearch.Text = "Search:";
            // 
            // spnLogLevel
            // 
            this.spnLogLevel.Location = new Point(192, 256);
            this.spnLogLevel.Name = "spnLogLevel";
            this.spnLogLevel.Size = new Size(64, 20);
            this.spnLogLevel.TabIndex = 21;
            // 
            // lblLogLevel
            // 
            this.lblLogLevel.Location = new Point(8, 256);
            this.lblLogLevel.Name = "lblLogLevel";
            this.lblLogLevel.Size = new Size(136, 16);
            this.lblLogLevel.TabIndex = 20;
            this.lblLogLevel.Text = "Error Logging Level:";
            // 
            // chkShowZoneName
            // 
            this.chkShowZoneName.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkShowZoneName.Checked = true;
            this.chkShowZoneName.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowZoneName.Location = new Point(8, 320);
            this.chkShowZoneName.Name = "chkShowZoneName";
            this.chkShowZoneName.Size = new Size(114, 24);
            this.chkShowZoneName.TabIndex = 24;
            this.chkShowZoneName.Text = "Show Zone Name";
            // 
            // spnOverrideLevel
            // 
            this.spnOverrideLevel.Location = new Point(192, 208);
            this.spnOverrideLevel.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.spnOverrideLevel.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.spnOverrideLevel.Name = "spnOverrideLevel";
            this.spnOverrideLevel.Size = new Size(64, 20);
            this.spnOverrideLevel.TabIndex = 15;
            this.spnOverrideLevel.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // spnUpdateDelay
            // 
            this.spnUpdateDelay.Location = new Point(192, 232);
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
            this.spnUpdateDelay.Size = new Size(64, 20);
            this.spnUpdateDelay.TabIndex = 17;
            this.spnUpdateDelay.Value = new decimal(new int[] {
            250,
            0,
            0,
            0});
            // 
            // txtWindowName
            // 
            this.txtWindowName.Location = new Point(8, 296);
            this.txtWindowName.Name = "txtWindowName";
            this.txtWindowName.Size = new Size(248, 20);
            this.txtWindowName.TabIndex = 23;
            // 
            // lblWindowName
            // 
            this.lblWindowName.Location = new Point(8, 280);
            this.lblWindowName.Name = "lblWindowName";
            this.lblWindowName.Size = new Size(144, 16);
            this.lblWindowName.TabIndex = 22;
            this.lblWindowName.Text = "Window Title:";
            // 
            // lblOverridelevel
            // 
            this.lblOverridelevel.Location = new Point(8, 208);
            this.lblOverridelevel.Name = "lblOverridelevel";
            this.lblOverridelevel.Size = new Size(136, 16);
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
            this.gbServer.Location = new Point(8, 8);
            this.gbServer.Name = "gbServer";
            this.gbServer.Size = new Size(248, 168);
            this.gbServer.TabIndex = 0;
            this.gbServer.TabStop = false;
            this.gbServer.Text = "Server";
            // 
            // txtIPAddress5
            // 
            this.txtIPAddress5.Location = new Point(128, 112);
            this.txtIPAddress5.Name = "txtIPAddress5";
            this.txtIPAddress5.Size = new Size(112, 20);
            this.txtIPAddress5.TabIndex = 10;
            // 
            // lblIPAddress5
            // 
            this.lblIPAddress5.Location = new Point(8, 112);
            this.lblIPAddress5.Name = "lblIPAddress5";
            this.lblIPAddress5.Size = new Size(120, 16);
            this.lblIPAddress5.TabIndex = 9;
            this.lblIPAddress5.Text = "IP Address 5: (Ctrl + 5)";
            // 
            // txtIPAddress4
            // 
            this.txtIPAddress4.Location = new Point(128, 88);
            this.txtIPAddress4.Name = "txtIPAddress4";
            this.txtIPAddress4.Size = new Size(112, 20);
            this.txtIPAddress4.TabIndex = 8;
            // 
            // lblIPAddress4
            // 
            this.lblIPAddress4.Location = new Point(8, 88);
            this.lblIPAddress4.Name = "lblIPAddress4";
            this.lblIPAddress4.Size = new Size(120, 16);
            this.lblIPAddress4.TabIndex = 7;
            this.lblIPAddress4.Text = "IP Address 4: (Ctrl + 4)";
            // 
            // txtIPAddress3
            // 
            this.txtIPAddress3.Location = new Point(128, 64);
            this.txtIPAddress3.Name = "txtIPAddress3";
            this.txtIPAddress3.Size = new Size(112, 20);
            this.txtIPAddress3.TabIndex = 6;
            // 
            // lblIPAddress3
            // 
            this.lblIPAddress3.Location = new Point(8, 64);
            this.lblIPAddress3.Name = "lblIPAddress3";
            this.lblIPAddress3.Size = new Size(120, 16);
            this.lblIPAddress3.TabIndex = 5;
            this.lblIPAddress3.Text = "IP Address 3: (Ctrl + 3)";
            // 
            // txtIPAddress2
            // 
            this.txtIPAddress2.Location = new Point(128, 40);
            this.txtIPAddress2.Name = "txtIPAddress2";
            this.txtIPAddress2.Size = new Size(112, 20);
            this.txtIPAddress2.TabIndex = 4;
            // 
            // lblIPAddress2
            // 
            this.lblIPAddress2.Location = new Point(8, 40);
            this.lblIPAddress2.Name = "lblIPAddress2";
            this.lblIPAddress2.Size = new Size(120, 16);
            this.lblIPAddress2.TabIndex = 3;
            this.lblIPAddress2.Text = "IP Address 2: (Ctrl + 2)";
            // 
            // txtPortNo
            // 
            this.txtPortNo.Location = new Point(128, 136);
            this.txtPortNo.Name = "txtPortNo";
            this.txtPortNo.Size = new Size(112, 20);
            this.txtPortNo.TabIndex = 12;
            this.txtPortNo.Text = "5555";
            // 
            // txtIPAddress1
            // 
            this.txtIPAddress1.Location = new Point(128, 16);
            this.txtIPAddress1.Name = "txtIPAddress1";
            this.txtIPAddress1.Size = new Size(112, 20);
            this.txtIPAddress1.TabIndex = 2;
            this.txtIPAddress1.Text = "localhost";
            // 
            // lblIPAddress1
            // 
            this.lblIPAddress1.Location = new Point(8, 16);
            this.lblIPAddress1.Name = "lblIPAddress1";
            this.lblIPAddress1.Size = new Size(120, 16);
            this.lblIPAddress1.TabIndex = 1;
            this.lblIPAddress1.Text = "IP Address 1: (Ctrl + 1)";
            // 
            // lbltxtPortNo
            // 
            this.lbltxtPortNo.Location = new Point(8, 136);
            this.lbltxtPortNo.Name = "lbltxtPortNo";
            this.lbltxtPortNo.Size = new Size(120, 16);
            this.lbltxtPortNo.TabIndex = 11;
            this.lbltxtPortNo.Text = "Port:";
            // 
            // lblUpdateDelay
            // 
            this.lblUpdateDelay.Location = new Point(8, 232);
            this.lblUpdateDelay.Name = "lblUpdateDelay";
            this.lblUpdateDelay.Size = new Size(136, 16);
            this.lblUpdateDelay.TabIndex = 16;
            this.lblUpdateDelay.Text = "Update Delay (mS):";
            // 
            // chkSaveOnExit
            // 
            this.chkSaveOnExit.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkSaveOnExit.Checked = true;
            this.chkSaveOnExit.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSaveOnExit.Location = new Point(8, 176);
            this.chkSaveOnExit.Name = "chkSaveOnExit";
            this.chkSaveOnExit.Size = new Size(197, 24);
            this.chkSaveOnExit.TabIndex = 13;
            this.chkSaveOnExit.Text = "Save Preferences On Exit:";
            // 
            // tabOptions
            // 
            this.tabOptions.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom
            | AnchorStyles.Left
            | AnchorStyles.Right);
            this.tabOptions.Controls.Add(this.tabGeneral);
            this.tabOptions.Controls.Add(this.tabMap);
            this.tabOptions.Controls.Add(this.tabAlerts);
            this.tabOptions.Controls.Add(this.tabFolders);
            this.tabOptions.Controls.Add(this.tabColors);
            this.tabOptions.Controls.Add(this.tabPage1);
            this.tabOptions.Location = new Point(0, 3);
            this.tabOptions.Name = "tabOptions";
            this.tabOptions.SelectedIndex = 0;
            this.tabOptions.Size = new Size(274, 401);
            this.tabOptions.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new Size(264, 374);
            this.tabPage1.TabIndex = 5;
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
            this.cmdCancel.DialogResult = DialogResult.Cancel;
            this.cmdCancel.Location = new Point(176, 410);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new Size(85, 23);
            this.cmdCancel.TabIndex = 2;
            this.cmdCancel.Text = "Cancel";
            // 
            // frmOptions
            // 
            this.AcceptButton = this.cmdCommand;
            this.AutoScaleBaseSize = new Size(5, 13);
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.CancelButton = this.cmdCommand;
            this.ClientSize = new Size(273, 445);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.tabOptions);
            this.Controls.Add(this.cmdCommand);
            this.Icon = ((Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmOptions";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Options";
            this.TopMost = true;
            this.tabColors.ResumeLayout(false);
            ((ISupportInitialize)(this.picGridLabelColor)).EndInit();
            ((ISupportInitialize)(this.picPlayerBorder)).EndInit();
            ((ISupportInitialize)(this.picListBackgroundColor)).EndInit();
            ((ISupportInitialize)(this.picRangeCircleColor)).EndInit();
            ((ISupportInitialize)(this.picGridColor)).EndInit();
            ((ISupportInitialize)(this.picMapBackgroundColor)).EndInit();
            this.tabFolders.ResumeLayout(false);
            this.tabFolders.PerformLayout();
            this.tabAlerts.ResumeLayout(false);
            this.grpDanger.ResumeLayout(false);
            this.grpDanger.PerformLayout();
            this.grpAlert.ResumeLayout(false);
            this.grpAlert.PerformLayout();
            this.grpCaution.ResumeLayout(false);
            this.grpCaution.PerformLayout();
            this.grpHunt.ResumeLayout(false);
            this.grpHunt.PerformLayout();
            this.tabMap.ResumeLayout(false);
            this.tabMap.PerformLayout();
            ((ISupportInitialize)(this.FadedLines)).EndInit();
            ((ISupportInitialize)(this.pvpLevels)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((ISupportInitialize)(this.spnRangeCircle)).EndInit();
            ((ISupportInitialize)(this.numMinAlertLevel)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((ISupportInitialize)(this.spnSpawnSize)).EndInit();
            this.tabGeneral.ResumeLayout(false);
            this.tabGeneral.PerformLayout();
            ((ISupportInitialize)(this.spnLogLevel)).EndInit();
            ((ISupportInitialize)(this.spnOverrideLevel)).EndInit();
            ((ISupportInitialize)(this.spnUpdateDelay)).EndInit();
            this.gbServer.ResumeLayout(false);
            this.gbServer.PerformLayout();
            this.tabOptions.ResumeLayout(false);
            this.ResumeLayout(false);
		}

		#endregion

		private void cmdCommand_Click(object sender, EventArgs e)
        {
//            UpdateSMTPSettings();
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

                Settings.Instance.IPAddress1 = txtIPAddress1.Text;

                Settings.Instance.IPAddress2 = txtIPAddress2.Text;

                Settings.Instance.IPAddress3 = txtIPAddress3.Text;

                Settings.Instance.IPAddress4 = txtIPAddress4.Text;

                Settings.Instance.IPAddress5 = txtIPAddress5.Text;

                Settings.Instance.Port = int.Parse(txtPortNo.Text);

                Settings.Instance.LevelOverride = (int)spnOverrideLevel.Value;

                Settings.Instance.SaveOnExit = chkSaveOnExit.Checked;

                Settings.Instance.UpdateDelay = (int)spnUpdateDelay.Value;

                Settings.Instance.CorpseAlerts = chkCorpsesAlerts.Checked;

                Settings.Instance.BackColor = picMapBackgroundColor.BackColor;

                Settings.Instance.PCBorderColor = picPlayerBorder.BackColor;

            Settings.Instance.PrefixStars = chkPrefixAlerts.Checked;

            Settings.Instance.AffixStars = chkAffixAlerts.Checked;

            Settings.Instance.HuntPrefix = txtHuntPrefix.Text;

            Settings.Instance.MatchFullTextH = chkHuntMatchFull.Checked;  //hunt

            Settings.Instance.NoneOnHunt = optHuntNone.Checked;

            Settings.Instance.BeepOnHunt = optHuntBeep.Checked;

            Settings.Instance.TalkOnHunt = optHuntSpeak.Checked;

            Settings.Instance.PlayOnHunt = optHuntPlay.Checked;

            Settings.Instance.HuntAudioFile = txtHuntAudioFile.Text;

            Settings.Instance.CautionPrefix = txtCautionPrefix.Text;

            Settings.Instance.MatchFullTextC = chkCautionMatchFull.Checked;  //Caution

            Settings.Instance.NoneOnCaution = optCautionNone.Checked;

            Settings.Instance.BeepOnCaution = optCautionBeep.Checked;

            Settings.Instance.TalkOnCaution = optCautionSpeak.Checked;

            Settings.Instance.PlayOnCaution = optCautionPlay.Checked;

            Settings.Instance.CautionAudioFile = txtCautionAudioFile.Text;

            Settings.Instance.DangerPrefix = txtDangerPrefix.Text;

            Settings.Instance.MatchFullTextD = chkDangerMatchFull.Checked;  //Caution

            Settings.Instance.NoneOnDanger = optDangerNone.Checked;

            Settings.Instance.BeepOnDanger = optDangerBeep.Checked;

            Settings.Instance.TalkOnDanger = optDangerSpeak.Checked;

            Settings.Instance.PlayOnDanger = optDangerPlay.Checked;

            Settings.Instance.DangerAudioFile = txtDangerAudioFile.Text;

            Settings.Instance.AlertPrefix = txtAlertPrefix.Text;

            Settings.Instance.MatchFullTextA = chkAlertMatchFull.Checked;  //Rare

            Settings.Instance.NoneOnAlert = optAlertNone.Checked;

            Settings.Instance.BeepOnAlert = optAlertBeep.Checked;

            Settings.Instance.TalkOnAlert = optAlertSpeak.Checked;

            Settings.Instance.PlayOnAlert = optAlertPlay.Checked;

            Settings.Instance.AlertAudioFile = txtAlertAudioFile.Text;

            Settings.Instance.RangeCircle = (int)spnRangeCircle.Value;

            Settings.Instance.DrawOptions = GetDrawOptions();

            Settings.Instance.ShowTargetInfo = chkShowTargetInfo.Checked;

            Settings.Instance.ShowZoneName = chkShowZoneName.Checked;

            Settings.Instance.ShowCharName = chkShowCharName.Checked;

            Settings.Instance.DrawFoV = chkDrawFoV.Checked;

            Settings.Instance.ColorRangeCircle = chkColorRangeCircle.Checked;

            Settings.Instance.AlertSound = cmbAlertSound.SelectedItem.ToString();

            Settings.Instance.HatchIndex = cmbHatch.SelectedItem.ToString();

            Settings.Instance.SpawnDrawSize = (int)spnSpawnSize.Value;

            Settings.Instance.FadedLines = (int)FadedLines.Value;

            Settings.Instance.PVPLevels = (int)pvpLevels.Value;

            Settings.Instance.MinAlertLevel = (int)numMinAlertLevel.Value;

            Settings.Instance.TitleBar = txtWindowName.Text;

            Settings.Instance.SearchString = txtSearchString.Text;

            Settings.Instance.MapDir = txtMapDir.Text;

            Settings.Instance.FilterDir = txtFilterDir.Text;

            Settings.Instance.CfgDir = txtCfgDir.Text;

            Settings.Instance.LogDir = txtLogDir.Text;

            Settings.Instance.TimerDir = txtTimerDir.Text;

            Settings.Instance.AutoSelectSpawnList = chkSelectSpawnList.Checked;

            Settings.Instance.OptionsWindowsLocation = Location;

            Settings.Instance.OptionsWindowsSize = Size;

            Settings.Instance.MaxLogLevel = (LogLevel)spnLogLevel.Value;

            if (Settings.Instance.CurrentIPAddress == 0 && txtIPAddress1.Text.Length > 0)
                Settings.Instance.CurrentIPAddress = 1;
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
				Settings.Instance.ListBackColor = colorOptionPicker.Color;

				picListBackgroundColor.BackColor = colorOptionPicker.Color;
			}
		}

		private void cmdGridColor_Click(object sender, EventArgs e)

		{
			colorOptionPicker.Color = picGridColor.BackColor;

			if(colorOptionPicker.ShowDialog() != DialogResult.Cancel)

			{
				Settings.Instance.GridColor = colorOptionPicker.Color;

				picGridColor.BackColor = colorOptionPicker.Color;
			}
		}

		private void cmdRangeCircleColor_Click(object sender, EventArgs e)

		{
			colorOptionPicker.Color = picRangeCircleColor.BackColor;

			if(colorOptionPicker.ShowDialog() != DialogResult.Cancel)

			{
				Settings.Instance.RangeCircleColor  = colorOptionPicker.Color;

				picRangeCircleColor.BackColor = colorOptionPicker.Color;
			}
		}

		private void cmdMapDirBrowse_Click(object sender, EventArgs e)

		{
			fldrBrowser.Description = "Map Directory";

            fldrBrowser.SelectedPath = Settings.Instance.MapDir;

			fldrBrowser.ShowDialog();

			if (fldrBrowser.SelectedPath.Trim() != "")
				txtMapDir.Text = fldrBrowser.SelectedPath;
		}

		private void cmdCfgDirBrowse_Click(object sender, EventArgs e)

		{
			fldrBrowser.Description = "Config Directory";

            fldrBrowser.SelectedPath = Settings.Instance.CfgDir;

			fldrBrowser.ShowDialog();

            if (fldrBrowser.SelectedPath.Trim() != "")
                txtCfgDir.Text = fldrBrowser.SelectedPath;
		}

		private void cmdFilterDirBrowse_Click(object sender, EventArgs e)

		{
            fldrBrowser.Description = "Filter Directory";

            fldrBrowser.SelectedPath = Settings.Instance.FilterDir;

			fldrBrowser.ShowDialog();

            if (fldrBrowser.SelectedPath.Trim() != "")
                txtFilterDir.Text = fldrBrowser.SelectedPath;
		}

		private void cmdLogDir_Click(object sender, EventArgs e)

		{
            fldrBrowser.Description = "Log Directory";

            fldrBrowser.SelectedPath = Settings.Instance.LogDir;

			fldrBrowser.ShowDialog();

            if (fldrBrowser.SelectedPath.Trim() != "")
                txtLogDir.Text = fldrBrowser.SelectedPath;
		}

        private void cmdSpawnTimers_Click(object sender, EventArgs e)

        {
            fldrBrowser.Description = "Timers Directory";

            fldrBrowser.SelectedPath = Settings.Instance.TimerDir;

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
            Settings.Instance.AlertSound = cmbAlertSound.SelectedItem.ToString();

            switch (Settings.Instance.AlertSound)

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
            Settings.Instance.HatchIndex = cmbHatch.SelectedItem.ToString();
        }

        private void cmdGridLabelColor_Click(object sender, EventArgs e)

        {
            colorOptionPicker.Color = picGridLabelColor.BackColor;

            if (colorOptionPicker.ShowDialog() != DialogResult.Cancel)

            {
                Settings.Instance.GridLabelColor = colorOptionPicker.Color;

                picGridLabelColor.BackColor = colorOptionPicker.Color;
            }
        }

        private void spnSpawnSize_ValueChanged(object sender, EventArgs e)

        {
            Settings.Instance.SpawnDrawSize = (int)spnSpawnSize.Value;
        }

        private void pvpLevels_ValueChanged(object sender, EventArgs e)

        {
            Settings.Instance.PVPLevels = (int)pvpLevels.Value;
        }

        //private void checkBoxSMTPUseNetworkCredentials_Click(object sender, EventArgs e)
        //{
        //    SmtpSettings.Instance.UseNetworkCredentials = !SmtpSettings.Instance.UseNetworkCredentials;

        //    checkBoxSMTPUseNetworkCredentials.Checked = SmtpSettings.Instance.UseNetworkCredentials;

        //    if (SmtpSettings.Instance.UseNetworkCredentials)
        //    {
        //        textSMTPPassword.Enabled = false;
        //        textSMTPUsername.Enabled = false;
        //        lblSMTPPassword.Enabled = false;
        //        lblSMTPUsername.Enabled = false;
        //    }
        //    else
        //    {
        //        textSMTPPassword.Enabled = true;
        //        textSMTPUsername.Enabled = true;
        //        lblSMTPPassword.Enabled = true;
        //        lblSMTPUsername.Enabled = true;
        //    }
        //}
        //private void UpdateSMTPSettings()
        //{
        //    SmtpSettings.Instance.UseSSL = checkBoxSMTPUseSecureAuthentication.Checked;
        //    if (textSMTPPassword.ToString().Length > 0 && textSMTPPassword.Text != "Password:::1") {
        //        SmtpSettings.Instance.SmtpPassword = textSMTPPassword.Text;
        //        //char[] pass = this.textSMTPPassword.Text.ToCharArray();
        //        //Settings.Instance.SmtpPassword.Clear();
        //        //foreach (char d in pass)
        //        //{
        //        //    Settings.Instance.SmtpPassword.AppendChar(d);
        //        //}
        //        //this.textSMTPPassword.Text = "Password:::1";
        //    }

        //    if (textSMTPPassword.ToString().Length == 0 && SmtpSettings.Instance.SmtpPassword.Length > 0)
        //        SmtpSettings.Instance.SmtpPassword = "";
        //        //Settings.Instance.SmtpPassword.Clear();
        //    SmtpSettings.Instance.SmtpUsername = textSMTPUsername.Text;
        //    SmtpSettings.Instance.SmtpDomain = textBoxSMTPDomain.Text;

        //    // make sure value for port is an int
        //    if (textSMTPPort.Text.Length > 0)
        //    {
        //        string Str = textSMTPPort.Text;
        //        bool isNum = int.TryParse(Str, out var Num);
        //        if (isNum && Num > 0)
        //        {
        //            SmtpSettings.Instance.SmtpPort = Num;
        //        }
        //        else
        //        {
        //            textSMTPPort.Text = SmtpSettings.Instance.SmtpPort.ToString();
        //        }
        //    }

        //    // Check if emails entered look like email addresses
        //    string emailPattern = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
        //    Regex re = new Regex(emailPattern);

        //    // From Email Address
        //    if (textSMTPFromEmail.Text.Length > 0)
        //        if (re.IsMatch(textSMTPFromEmail.Text))
        //            SmtpSettings.Instance.FromEmail = textSMTPFromEmail.Text;

        //    // To Email Address (can contain multiple addresses seperated by comma or semicolon
        //    if (textSMTPToEmail.Text.Length == 0)
        //        SmtpSettings.Instance.ToEmail = "";
        //    // split of To: email string with a , or ; delimiter, and verify
        //    if (textSMTPToEmail.Text.Length > 0)
        //    {
        //        string presplit = textSMTPToEmail.Text;
        //        string postsplit = "";
        //        string delim = ",;<>[] ";
        //        char[] delimarray = delim.ToCharArray();
        //        string[] split = null;
        //        split = presplit.Split(delimarray);
        //        foreach (string s in split)
        //        {
        //            if (re.IsMatch(s.Trim()))
        //            {
        //                postsplit = postsplit.Length > 0 ? postsplit + "; " + s.Trim() : s.Trim();
        //            }
        //        }
        //        SmtpSettings.Instance.ToEmail = postsplit.Length > 0 ? postsplit : "";
        //    }

        //    // CC Email Address (can contain multiple addresses seperated by comma or semicolon
        //    if (textSMTPCCEmail.Text.Length == 0)
        //        SmtpSettings.Instance.CCEmail = "";
        //    if (textSMTPCCEmail.Text.Length > 0)
        //    {
        //        string presplit = textSMTPCCEmail.Text;
        //        string postsplit = "";
        //        string delim = ",;<>[] ";
        //        char[] delimarray = delim.ToCharArray();
        //        string[] split = null;
        //        split = presplit.Split(delimarray);
        //        foreach (string s in split)
        //        {
        //            if (re.IsMatch(s.Trim()))
        //            {
        //                postsplit = postsplit.Length > 0 ? postsplit + "; " + s.Trim() : s.Trim();
        //            }
        //        }
        //        SmtpSettings.Instance.CCEmail = postsplit.Length > 0 ? postsplit : "";
        //    }

        //    textSMTPToEmail.Text = SmtpSettings.Instance.ToEmail;
        //    textSMTPFromEmail.Text = SmtpSettings.Instance.FromEmail;
        //    textSMTPCCEmail.Text = SmtpSettings.Instance.CCEmail;

        //    // Check if the SMTP Server Address looks like a host name
        //    string hostPattern = @"^((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
        //    Regex reh = new Regex(hostPattern);
        //    if (textSMTPAddress.Text.Length == 0)
        //        SmtpSettings.Instance.SmtpServer = "";
        //    if (textSMTPAddress.Text.Length > 0)
        //        if (reh.IsMatch(textSMTPAddress.Text))
        //            SmtpSettings.Instance.SmtpServer = textSMTPAddress.Text;
        //    textSMTPAddress.Text = SmtpSettings.Instance.SmtpServer;
        //}

        //private void btnTestEmail_Click(object sender, EventArgs e)
        //{
        //    UpdateSMTPSettings();
        //    // check email parameters all filled out
        //    string errmsg = "";
        //    if (textSMTPAddress.Text.Length == 0)
        //    {
        //        if (textSMTPPort.Text.Length == 0)
        //            errmsg += "Enter a valid SMTP Server Address and Port.\r\n";
        //        else
        //            errmsg += "Enter a valid SMTP Server Address.\r\n";
        //    }
        //    else if (textSMTPPort.Text.Length == 0)
        //    {
        //        errmsg += "Enter a valid SMTP Server Port.\r\n";
        //    }
        //    if (textSMTPFromEmail.Text.Length == 0)
        //    {
        //        if (textSMTPToEmail.Text.Length == 0)
        //            errmsg += "Valid From and To Email Addresses are required.\r\n";
        //        else
        //            errmsg += "Enter a valid From Email Address.\r\n";
        //    }
        //    else if (textSMTPToEmail.Text.Length == 0)
        //    {
        //        errmsg += "Enter a valid To Email Address.\r\n";
        //    }
        //    if (errmsg != string.Empty)
        //    {
        //        errmsg += "\r\nSending Test Email Aborted.";
        //        MessageBox.Show(errmsg, "Some Email Settings Missing.", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //    else
        //    {
        //        // Set up the message
        //        MailMessage mailMessage = new MailMessage
        //        {
        //            From = new MailAddress(SmtpSettings.Instance.FromEmail)
        //        };

        //        // To Email Address could contain multiple addresses
        //        string presplit = SmtpSettings.Instance.ToEmail;
        //        string delim = ",;";
        //        char[] delimarray = delim.ToCharArray();
        //        string[] split = null;
        //        split = presplit.Split(delimarray);
        //        foreach (string s in split)
        //        {
        //            if (s.Trim().Length > 0)
        //                mailMessage.To.Add(new MailAddress(s.Trim()));
        //        }

        //        // CC email addresses
        //        split = null;
        //        presplit = SmtpSettings.Instance.CCEmail;
        //        split = presplit.Split(delimarray);
        //        foreach (string s in split)
        //        {
        //            if (s.Trim().Length > 0)
        //                mailMessage.CC.Add(new MailAddress(s.Trim()));
        //        }

        //        mailMessage.Subject = "MySEQ Spawn Alert";
        //        mailMessage.Body = "This is a test MySEQ Email Alert.";

        //        SmtpClient smtpClient = new SmtpClient(SmtpSettings.Instance.SmtpServer, SmtpSettings.Instance.SmtpPort)
        //        {
        //            DeliveryMethod = SmtpDeliveryMethod.Network
        //        };

        //        if (SmtpSettings.Instance.UseNetworkCredentials)
        //        {
        //            smtpClient.UseDefaultCredentials = true;
        //        }
        //        else
        //        {
        //            smtpClient.UseDefaultCredentials = false;
        //            smtpClient.Credentials = SmtpSettings.Instance.SmtpDomain != string.Empty
        //                ? new System.Net.NetworkCredential(SmtpSettings.Instance.SmtpUsername, SmtpSettings.Instance.SmtpPassword, SmtpSettings.Instance.SmtpDomain)
        //                : new System.Net.NetworkCredential(SmtpSettings.Instance.SmtpUsername, SmtpSettings.Instance.SmtpPassword);
        //        }
        //        // using SSL to authenticate?
        //        smtpClient.EnableSsl = SmtpSettings.Instance.UseSSL;
        //        smtpClient.SendCompleted += new SendCompletedEventHandler(SmtpClient_OnCompleted);

        //        object UserState = mailMessage;
        //        // Send away ....
        //        try { smtpClient.SendAsync(mailMessage, UserState); }
        //        catch (Exception Ex) { MessageBox.Show("Send Mail Error: " + Ex.Message, "Mail Message Failed to Send.",MessageBoxButtons.OK,MessageBoxIcon.Error); }
        //    }
        //}
        //private void SmtpClient_OnCompleted(object sender, AsyncCompletedEventArgs e)
        //{
        //    //Get the Original MailMessage object
        //    MailMessage mail = (MailMessage)e.UserState;

        //    //write out the subject
        //    string subject = mail.Subject;

        //    if (e.Cancelled)
        //    {
        //        MessageBox.Show("Send canceled for [" + subject + "].", "Sending Mail Canceled", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //    if (e.Error != null)
        //    {
        //        MessageBox.Show("Error sending [" + subject +"\r\n" + e.Error.ToString(), "Error Sending Mail",MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //    else
        //    {
        //        MessageBox.Show("Message [" + subject + "] sent successfully.", "SMTP Configured Properly", MessageBoxButtons.OK);
        //    }
        //}

        //private void checkBoxSavePassword_Click(object sender, EventArgs e)
        //{
        //    SmtpSettings.Instance.SavePassword = !SmtpSettings.Instance.SavePassword;

        //    checkBoxSavePassword.Checked = SmtpSettings.Instance.SavePassword;
        //}

        //private void checkBoxSMTPUseSecureAuthentication_Click(object sender, EventArgs e)
        //{
        //    SmtpSettings.Instance.UseSSL = !SmtpSettings.Instance.UseSSL;

        //    if (SmtpSettings.Instance.UseSSL)
        //    {
        //        SmtpSettings.Instance.SmtpPort = 587;
        //        textSMTPPort.Text = SmtpSettings.Instance.SmtpPort.ToString();
        //    }
        //    else
        //    {
        //        SmtpSettings.Instance.SmtpPort = 25;
        //        textSMTPPort.Text = SmtpSettings.Instance.SmtpPort.ToString();
        //    }

        //    checkBoxSMTPUseSecureAuthentication.Checked = SmtpSettings.Instance.UseSSL;
        //}
	}
}

