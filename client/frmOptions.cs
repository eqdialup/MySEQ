using System;

using System.Media;

using System.Drawing;

using System.Drawing.Drawing2D;

using System.Text.RegularExpressions;

using System.Collections;

using System.ComponentModel;

using System.IO;

using System.Windows.Forms;

using Structures;

using SpeechLib;

using System.Net;

using System.Net.Mail;



namespace myseq

{

	/// <summary>

	/// Summary description for frmOptions.

	/// </summary>

    public class frmOptions : System.Windows.Forms.Form

    {

        private System.Windows.Forms.ColorDialog colorOptionPicker; 

        private System.Windows.Forms.Button cmdCommand;

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

        private SpVoice speech = new SpVoice();

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
        private GroupBox groupBox3;
        private Label lblSMTPPort;
        private TextBox textSMTPPort;
        private Label lblSMTPUsername;
        private Label lblSMTPAddress;
        private TextBox textSMTPAddress;
        private TextBox textSMTPUsername;
        private Label lblSMTPPassword;
        private TextBox textSMTPToEmail;
        private Label lblToAddress;
        private TextBox textSMTPFromEmail;
        private Label lblFromAddress;
        private TextBox textSMTPPassword;
        private CheckBox checkBoxSMTPUseNetworkCredentials;
        private CheckBox checkBoxSMTPUseSecureAuthentication;
        private Button btnTestEmail;
        private CheckBox checkBoxSavePassword;
        private Label lblSMTPDomain;
        private TextBox textBoxSMTPDomain;
        private Label lblCCEmail;
        private TextBox textSMTPCCEmail;
        private Label lblFadedLines;
        public NumericUpDown FadedLines;
        public CheckBox chkShowCharName;
        private Button cmdCancel;
        



		/// <summary>

		/// Required designer variable.

		/// </summary>

		private System.ComponentModel.Container components = null;



		public frmOptions()  {

			//

			// Required for Windows Form Designer support

			//
			InitializeComponent();          

            foreach (string styleName in Enum.GetNames(typeof(HatchStyle)))

            {

                cmbHatch.Items.Add(styleName);

            }

            textSMTPAddress.Text = SMTPSettings.Instance.SmtpServer.ToString();
            textSMTPPort.Text = SMTPSettings.Instance.SmtpPort.ToString();
            textBoxSMTPDomain.Text = SMTPSettings.Instance.SmtpDomain.ToString();
            textSMTPUsername.Text = SMTPSettings.Instance.SmtpUsername.ToString();

            if (SMTPSettings.Instance.SmtpPassword.ToString().Length > 0)
                textSMTPPassword.Text = "Password:::1";
            else
                textSMTPPassword.Text = "";
            textSMTPToEmail.Text = SMTPSettings.Instance.ToEmail;
            textSMTPFromEmail.Text = SMTPSettings.Instance.FromEmail;
            textSMTPCCEmail.Text = SMTPSettings.Instance.CCEmail;
            checkBoxSMTPUseNetworkCredentials.Checked = SMTPSettings.Instance.UseNetworkCredentials;
            checkBoxSMTPUseSecureAuthentication.Checked = SMTPSettings.Instance.UseSSL;
            checkBoxSavePassword.Checked = SMTPSettings.Instance.SavePassword;

            if (SMTPSettings.Instance.UseNetworkCredentials)
            {
                textSMTPUsername.Enabled = false;
                textSMTPPassword.Enabled = false;
                lblSMTPPassword.Enabled = false;
                lblSMTPUsername.Enabled = false;
            }
            else
            {
                textSMTPUsername.Enabled = true;
                textSMTPPassword.Enabled = true;
                lblSMTPPassword.Enabled = true;
                lblSMTPUsername.Enabled = true;
            }
            cmbHatch.SelectedText = Settings.Instance.HatchIndex;

            cmbAlertSound.SelectedText = Settings.Instance.AlertSound;

		}



		/// <summary>

		/// Clean up any resources being used.

		/// </summary>

		protected override void Dispose( bool disposing )  {

			if( disposing )  {

				if(components != null)  {

					components.Dispose();

				}

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmOptions));
            this.cmdCommand = new System.Windows.Forms.Button();
            this.colorOptionPicker = new System.Windows.Forms.ColorDialog();
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
            this.tabFolders = new System.Windows.Forms.TabPage();
            this.cmdSpawnTimers = new System.Windows.Forms.Button();
            this.txtTimerDir = new System.Windows.Forms.TextBox();
            this.txtLogDir = new System.Windows.Forms.TextBox();
            this.txtFilterDir = new System.Windows.Forms.TextBox();
            this.txtCfgDir = new System.Windows.Forms.TextBox();
            this.txtMapDir = new System.Windows.Forms.TextBox();
            this.lblSpawnTimers = new System.Windows.Forms.Label();
            this.cmdLogDir = new System.Windows.Forms.Button();
            this.lblLogDir = new System.Windows.Forms.Label();
            this.cmdFilterDirBrowse = new System.Windows.Forms.Button();
            this.lblFilterDir = new System.Windows.Forms.Label();
            this.cmdCfgDirBrowse = new System.Windows.Forms.Button();
            this.lblCfgDir = new System.Windows.Forms.Label();
            this.cmdMapDirBrowse = new System.Windows.Forms.Button();
            this.lblMapDir = new System.Windows.Forms.Label();
            this.tabAlerts = new System.Windows.Forms.TabPage();
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
            this.chkAffixAlerts = new System.Windows.Forms.CheckBox();
            this.chkPrefixAlerts = new System.Windows.Forms.CheckBox();
            this.chkCorpsesAlerts = new System.Windows.Forms.CheckBox();
            this.tabMap = new System.Windows.Forms.TabPage();
            this.FadedLines = new System.Windows.Forms.NumericUpDown();
            this.lblFadedLines = new System.Windows.Forms.Label();
            this.lblPVPLevels = new System.Windows.Forms.Label();
            this.pvpLevels = new System.Windows.Forms.NumericUpDown();
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
            this.chkSelectSpawnList = new System.Windows.Forms.CheckBox();
            this.spnSpawnSize = new System.Windows.Forms.NumericUpDown();
            this.chkShowTargetInfo = new System.Windows.Forms.CheckBox();
            this.chkDrawFoV = new System.Windows.Forms.CheckBox();
            this.tabGeneral = new System.Windows.Forms.TabPage();
            this.chkShowCharName = new System.Windows.Forms.CheckBox();
            this.txtSearchString = new System.Windows.Forms.TextBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.spnLogLevel = new System.Windows.Forms.NumericUpDown();
            this.lblLogLevel = new System.Windows.Forms.Label();
            this.chkShowZoneName = new System.Windows.Forms.CheckBox();
            this.spnOverrideLevel = new System.Windows.Forms.NumericUpDown();
            this.spnUpdateDelay = new System.Windows.Forms.NumericUpDown();
            this.txtWindowName = new System.Windows.Forms.TextBox();
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
            this.chkSaveOnExit = new System.Windows.Forms.CheckBox();
            this.tabOptions = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.lblCCEmail = new System.Windows.Forms.Label();
            this.textSMTPCCEmail = new System.Windows.Forms.TextBox();
            this.btnTestEmail = new System.Windows.Forms.Button();
            this.textSMTPToEmail = new System.Windows.Forms.TextBox();
            this.lblToAddress = new System.Windows.Forms.Label();
            this.textSMTPFromEmail = new System.Windows.Forms.TextBox();
            this.lblFromAddress = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lblSMTPDomain = new System.Windows.Forms.Label();
            this.textBoxSMTPDomain = new System.Windows.Forms.TextBox();
            this.checkBoxSavePassword = new System.Windows.Forms.CheckBox();
            this.checkBoxSMTPUseNetworkCredentials = new System.Windows.Forms.CheckBox();
            this.checkBoxSMTPUseSecureAuthentication = new System.Windows.Forms.CheckBox();
            this.textSMTPPassword = new System.Windows.Forms.TextBox();
            this.lblSMTPPort = new System.Windows.Forms.Label();
            this.textSMTPPort = new System.Windows.Forms.TextBox();
            this.lblSMTPUsername = new System.Windows.Forms.Label();
            this.lblSMTPAddress = new System.Windows.Forms.Label();
            this.textSMTPAddress = new System.Windows.Forms.TextBox();
            this.textSMTPUsername = new System.Windows.Forms.TextBox();
            this.lblSMTPPassword = new System.Windows.Forms.Label();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.tabColors.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picGridLabelColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPlayerBorder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picListBackgroundColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picRangeCircleColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGridColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMapBackgroundColor)).BeginInit();
            this.tabFolders.SuspendLayout();
            this.tabAlerts.SuspendLayout();
            this.grpDanger.SuspendLayout();
            this.grpAlert.SuspendLayout();
            this.grpCaution.SuspendLayout();
            this.grpHunt.SuspendLayout();
            this.tabMap.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FadedLines)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pvpLevels)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spnRangeCircle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMinAlertLevel)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spnSpawnSize)).BeginInit();
            this.tabGeneral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spnLogLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spnOverrideLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spnUpdateDelay)).BeginInit();
            this.gbServer.SuspendLayout();
            this.tabOptions.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdCommand
            // 
            this.cmdCommand.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdCommand.Location = new System.Drawing.Point(122, 480);
            this.cmdCommand.Name = "cmdCommand";
            this.cmdCommand.Size = new System.Drawing.Size(102, 27);
            this.cmdCommand.TabIndex = 0;
            this.cmdCommand.Text = "&Save";
            this.cmdCommand.Click += new System.EventHandler(this.cmdCommand_Click);
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
            this.tabColors.Location = new System.Drawing.Point(4, 25);
            this.tabColors.Name = "tabColors";
            this.tabColors.Size = new System.Drawing.Size(318, 433);
            this.tabColors.TabIndex = 3;
            this.tabColors.Text = "Colors";
            // 
            // picGridLabelColor
            // 
            this.picGridLabelColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picGridLabelColor.Location = new System.Drawing.Point(182, 189);
            this.picGridLabelColor.Name = "picGridLabelColor";
            this.picGridLabelColor.Size = new System.Drawing.Size(125, 28);
            this.picGridLabelColor.TabIndex = 13;
            this.picGridLabelColor.TabStop = false;
            this.picGridLabelColor.Click += new System.EventHandler(this.cmdGridLabelColor_Click);
            // 
            // cmdGridLabelColor
            // 
            this.cmdGridLabelColor.Location = new System.Drawing.Point(10, 189);
            this.cmdGridLabelColor.Name = "cmdGridLabelColor";
            this.cmdGridLabelColor.Size = new System.Drawing.Size(163, 28);
            this.cmdGridLabelColor.TabIndex = 12;
            this.cmdGridLabelColor.Text = "Grid Label Color";
            this.cmdGridLabelColor.Click += new System.EventHandler(this.cmdGridLabelColor_Click);
            // 
            // picPlayerBorder
            // 
            this.picPlayerBorder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picPlayerBorder.Location = new System.Drawing.Point(182, 155);
            this.picPlayerBorder.Name = "picPlayerBorder";
            this.picPlayerBorder.Size = new System.Drawing.Size(125, 27);
            this.picPlayerBorder.TabIndex = 11;
            this.picPlayerBorder.TabStop = false;
            this.picPlayerBorder.Click += new System.EventHandler(this.butPlayerBorder_Click);
            // 
            // butPlayerBorder
            // 
            this.butPlayerBorder.Location = new System.Drawing.Point(10, 155);
            this.butPlayerBorder.Name = "butPlayerBorder";
            this.butPlayerBorder.Size = new System.Drawing.Size(163, 27);
            this.butPlayerBorder.TabIndex = 10;
            this.butPlayerBorder.Text = "PC Highlight Color";
            this.butPlayerBorder.Click += new System.EventHandler(this.butPlayerBorder_Click);
            // 
            // picListBackgroundColor
            // 
            this.picListBackgroundColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picListBackgroundColor.Location = new System.Drawing.Point(182, 120);
            this.picListBackgroundColor.Name = "picListBackgroundColor";
            this.picListBackgroundColor.Size = new System.Drawing.Size(125, 28);
            this.picListBackgroundColor.TabIndex = 7;
            this.picListBackgroundColor.TabStop = false;
            this.picListBackgroundColor.Click += new System.EventHandler(this.cmdListBackgroundColor_Click);
            // 
            // cmdListBackgroundColor
            // 
            this.cmdListBackgroundColor.Location = new System.Drawing.Point(10, 120);
            this.cmdListBackgroundColor.Name = "cmdListBackgroundColor";
            this.cmdListBackgroundColor.Size = new System.Drawing.Size(163, 28);
            this.cmdListBackgroundColor.TabIndex = 6;
            this.cmdListBackgroundColor.Text = "List Background";
            this.cmdListBackgroundColor.Click += new System.EventHandler(this.cmdListBackgroundColor_Click);
            // 
            // picRangeCircleColor
            // 
            this.picRangeCircleColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picRangeCircleColor.Location = new System.Drawing.Point(182, 83);
            this.picRangeCircleColor.Name = "picRangeCircleColor";
            this.picRangeCircleColor.Size = new System.Drawing.Size(125, 28);
            this.picRangeCircleColor.TabIndex = 5;
            this.picRangeCircleColor.TabStop = false;
            this.picRangeCircleColor.Click += new System.EventHandler(this.cmdRangeCircleColor_Click);
            // 
            // cmdRangeCircleColor
            // 
            this.cmdRangeCircleColor.Location = new System.Drawing.Point(10, 83);
            this.cmdRangeCircleColor.Name = "cmdRangeCircleColor";
            this.cmdRangeCircleColor.Size = new System.Drawing.Size(163, 28);
            this.cmdRangeCircleColor.TabIndex = 2;
            this.cmdRangeCircleColor.Text = "Range Circle";
            this.cmdRangeCircleColor.Click += new System.EventHandler(this.cmdRangeCircleColor_Click);
            // 
            // picGridColor
            // 
            this.picGridColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picGridColor.Location = new System.Drawing.Point(182, 46);
            this.picGridColor.Name = "picGridColor";
            this.picGridColor.Size = new System.Drawing.Size(125, 28);
            this.picGridColor.TabIndex = 3;
            this.picGridColor.TabStop = false;
            this.picGridColor.Click += new System.EventHandler(this.cmdGridColor_Click);
            // 
            // cmdGridColor
            // 
            this.cmdGridColor.Location = new System.Drawing.Point(10, 46);
            this.cmdGridColor.Name = "cmdGridColor";
            this.cmdGridColor.Size = new System.Drawing.Size(163, 28);
            this.cmdGridColor.TabIndex = 1;
            this.cmdGridColor.Text = "Grid";
            this.cmdGridColor.Click += new System.EventHandler(this.cmdGridColor_Click);
            // 
            // picMapBackgroundColor
            // 
            this.picMapBackgroundColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picMapBackgroundColor.Location = new System.Drawing.Point(182, 9);
            this.picMapBackgroundColor.Name = "picMapBackgroundColor";
            this.picMapBackgroundColor.Size = new System.Drawing.Size(125, 28);
            this.picMapBackgroundColor.TabIndex = 1;
            this.picMapBackgroundColor.TabStop = false;
            this.picMapBackgroundColor.Click += new System.EventHandler(this.cmdMapBackgroundColor_Click);
            // 
            // cmdMapBackgroundColor
            // 
            this.cmdMapBackgroundColor.Location = new System.Drawing.Point(10, 9);
            this.cmdMapBackgroundColor.Name = "cmdMapBackgroundColor";
            this.cmdMapBackgroundColor.Size = new System.Drawing.Size(163, 28);
            this.cmdMapBackgroundColor.TabIndex = 0;
            this.cmdMapBackgroundColor.Text = "Map Background";
            this.cmdMapBackgroundColor.Click += new System.EventHandler(this.cmdMapBackgroundColor_Click);
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
            this.tabFolders.Location = new System.Drawing.Point(4, 25);
            this.tabFolders.Name = "tabFolders";
            this.tabFolders.Size = new System.Drawing.Size(318, 433);
            this.tabFolders.TabIndex = 4;
            this.tabFolders.Text = "Folders";
            // 
            // cmdSpawnTimers
            // 
            this.cmdSpawnTimers.Location = new System.Drawing.Point(278, 212);
            this.cmdSpawnTimers.Name = "cmdSpawnTimers";
            this.cmdSpawnTimers.Size = new System.Drawing.Size(29, 27);
            this.cmdSpawnTimers.TabIndex = 40;
            this.cmdSpawnTimers.Text = "...";
            this.cmdSpawnTimers.Click += new System.EventHandler(this.cmdSpawnTimers_Click);
            // 
            // txtTimerDir
            // 
            this.txtTimerDir.BackColor = System.Drawing.Color.White;
            this.txtTimerDir.Location = new System.Drawing.Point(10, 212);
            this.txtTimerDir.Name = "txtTimerDir";
            this.txtTimerDir.Size = new System.Drawing.Size(259, 22);
            this.txtTimerDir.TabIndex = 39;
            // 
            // txtLogDir
            // 
            this.txtLogDir.BackColor = System.Drawing.Color.White;
            this.txtLogDir.Location = new System.Drawing.Point(10, 166);
            this.txtLogDir.Name = "txtLogDir";
            this.txtLogDir.Size = new System.Drawing.Size(259, 22);
            this.txtLogDir.TabIndex = 36;
            // 
            // txtFilterDir
            // 
            this.txtFilterDir.BackColor = System.Drawing.Color.White;
            this.txtFilterDir.Location = new System.Drawing.Point(10, 120);
            this.txtFilterDir.Name = "txtFilterDir";
            this.txtFilterDir.Size = new System.Drawing.Size(259, 22);
            this.txtFilterDir.TabIndex = 33;
            // 
            // txtCfgDir
            // 
            this.txtCfgDir.BackColor = System.Drawing.Color.White;
            this.txtCfgDir.Location = new System.Drawing.Point(10, 74);
            this.txtCfgDir.Name = "txtCfgDir";
            this.txtCfgDir.Size = new System.Drawing.Size(259, 22);
            this.txtCfgDir.TabIndex = 30;
            // 
            // txtMapDir
            // 
            this.txtMapDir.BackColor = System.Drawing.Color.White;
            this.txtMapDir.Location = new System.Drawing.Point(10, 28);
            this.txtMapDir.Name = "txtMapDir";
            this.txtMapDir.Size = new System.Drawing.Size(259, 22);
            this.txtMapDir.TabIndex = 27;
            // 
            // lblSpawnTimers
            // 
            this.lblSpawnTimers.Location = new System.Drawing.Point(10, 194);
            this.lblSpawnTimers.Name = "lblSpawnTimers";
            this.lblSpawnTimers.Size = new System.Drawing.Size(172, 18);
            this.lblSpawnTimers.TabIndex = 38;
            this.lblSpawnTimers.Text = "Spawn Timers";
            // 
            // cmdLogDir
            // 
            this.cmdLogDir.Location = new System.Drawing.Point(278, 166);
            this.cmdLogDir.Name = "cmdLogDir";
            this.cmdLogDir.Size = new System.Drawing.Size(29, 27);
            this.cmdLogDir.TabIndex = 37;
            this.cmdLogDir.Text = "...";
            this.cmdLogDir.Click += new System.EventHandler(this.cmdLogDir_Click);
            // 
            // lblLogDir
            // 
            this.lblLogDir.Location = new System.Drawing.Point(10, 148);
            this.lblLogDir.Name = "lblLogDir";
            this.lblLogDir.Size = new System.Drawing.Size(172, 18);
            this.lblLogDir.TabIndex = 35;
            this.lblLogDir.Text = "Log Folder";
            // 
            // cmdFilterDirBrowse
            // 
            this.cmdFilterDirBrowse.Location = new System.Drawing.Point(278, 120);
            this.cmdFilterDirBrowse.Name = "cmdFilterDirBrowse";
            this.cmdFilterDirBrowse.Size = new System.Drawing.Size(29, 27);
            this.cmdFilterDirBrowse.TabIndex = 34;
            this.cmdFilterDirBrowse.Text = "...";
            this.cmdFilterDirBrowse.Click += new System.EventHandler(this.cmdFilterDirBrowse_Click);
            // 
            // lblFilterDir
            // 
            this.lblFilterDir.Location = new System.Drawing.Point(10, 102);
            this.lblFilterDir.Name = "lblFilterDir";
            this.lblFilterDir.Size = new System.Drawing.Size(172, 18);
            this.lblFilterDir.TabIndex = 32;
            this.lblFilterDir.Text = "Filter Folder";
            // 
            // cmdCfgDirBrowse
            // 
            this.cmdCfgDirBrowse.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCfgDirBrowse.Location = new System.Drawing.Point(278, 74);
            this.cmdCfgDirBrowse.Name = "cmdCfgDirBrowse";
            this.cmdCfgDirBrowse.Size = new System.Drawing.Size(29, 26);
            this.cmdCfgDirBrowse.TabIndex = 31;
            this.cmdCfgDirBrowse.Text = "...";
            this.cmdCfgDirBrowse.Click += new System.EventHandler(this.cmdCfgDirBrowse_Click);
            // 
            // lblCfgDir
            // 
            this.lblCfgDir.Location = new System.Drawing.Point(10, 55);
            this.lblCfgDir.Name = "lblCfgDir";
            this.lblCfgDir.Size = new System.Drawing.Size(172, 19);
            this.lblCfgDir.TabIndex = 29;
            this.lblCfgDir.Text = "Config Folder";
            // 
            // cmdMapDirBrowse
            // 
            this.cmdMapDirBrowse.Location = new System.Drawing.Point(278, 28);
            this.cmdMapDirBrowse.Name = "cmdMapDirBrowse";
            this.cmdMapDirBrowse.Size = new System.Drawing.Size(29, 26);
            this.cmdMapDirBrowse.TabIndex = 28;
            this.cmdMapDirBrowse.Text = "...";
            this.cmdMapDirBrowse.Click += new System.EventHandler(this.cmdMapDirBrowse_Click);
            // 
            // lblMapDir
            // 
            this.lblMapDir.Location = new System.Drawing.Point(10, 9);
            this.lblMapDir.Name = "lblMapDir";
            this.lblMapDir.Size = new System.Drawing.Size(172, 19);
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
            this.tabAlerts.Location = new System.Drawing.Point(4, 25);
            this.tabAlerts.Name = "tabAlerts";
            this.tabAlerts.Size = new System.Drawing.Size(318, 433);
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
            this.grpDanger.Location = new System.Drawing.Point(5, 241);
            this.grpDanger.Name = "grpDanger";
            this.grpDanger.Size = new System.Drawing.Size(297, 90);
            this.grpDanger.TabIndex = 23;
            this.grpDanger.TabStop = false;
            this.grpDanger.Text = "Danger";
            // 
            // optDangerPlay
            // 
            this.optDangerPlay.Location = new System.Drawing.Point(10, 63);
            this.optDangerPlay.Name = "optDangerPlay";
            this.optDangerPlay.Size = new System.Drawing.Size(96, 19);
            this.optDangerPlay.TabIndex = 18;
            this.optDangerPlay.Text = "Play Wav";
            // 
            // optDangerSpeak
            // 
            this.optDangerSpeak.Location = new System.Drawing.Point(200, 40);
            this.optDangerSpeak.Name = "optDangerSpeak";
            this.optDangerSpeak.Size = new System.Drawing.Size(87, 19);
            this.optDangerSpeak.TabIndex = 17;
            this.optDangerSpeak.Text = "Speak";
            // 
            // optDangerBeep
            // 
            this.optDangerBeep.Location = new System.Drawing.Point(106, 40);
            this.optDangerBeep.Name = "optDangerBeep";
            this.optDangerBeep.Size = new System.Drawing.Size(86, 19);
            this.optDangerBeep.TabIndex = 16;
            this.optDangerBeep.Text = "Beep";
            // 
            // optDangerNone
            // 
            this.optDangerNone.Checked = true;
            this.optDangerNone.Location = new System.Drawing.Point(10, 40);
            this.optDangerNone.Name = "optDangerNone";
            this.optDangerNone.Size = new System.Drawing.Size(86, 19);
            this.optDangerNone.TabIndex = 15;
            this.optDangerNone.TabStop = true;
            this.optDangerNone.Text = "None";
            // 
            // txtDangerAudioFile
            // 
            this.txtDangerAudioFile.Location = new System.Drawing.Point(106, 63);
            this.txtDangerAudioFile.Name = "txtDangerAudioFile";
            this.txtDangerAudioFile.Size = new System.Drawing.Size(182, 22);
            this.txtDangerAudioFile.TabIndex = 19;
            // 
            // txtDangerPrefix
            // 
            this.txtDangerPrefix.Location = new System.Drawing.Point(106, 13);
            this.txtDangerPrefix.MaxLength = 5;
            this.txtDangerPrefix.Name = "txtDangerPrefix";
            this.txtDangerPrefix.Size = new System.Drawing.Size(38, 22);
            this.txtDangerPrefix.TabIndex = 13;
            this.txtDangerPrefix.Text = "[D]";
            // 
            // lblDangerPrefix
            // 
            this.lblDangerPrefix.Location = new System.Drawing.Point(10, 16);
            this.lblDangerPrefix.Name = "lblDangerPrefix";
            this.lblDangerPrefix.Size = new System.Drawing.Size(94, 19);
            this.lblDangerPrefix.TabIndex = 12;
            this.lblDangerPrefix.Text = "Prefix/Suffix:";
            // 
            // chkDangerMatchFull
            // 
            this.chkDangerMatchFull.Location = new System.Drawing.Point(163, 13);
            this.chkDangerMatchFull.Name = "chkDangerMatchFull";
            this.chkDangerMatchFull.Size = new System.Drawing.Size(125, 27);
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
            this.grpAlert.Location = new System.Drawing.Point(5, 338);
            this.grpAlert.Name = "grpAlert";
            this.grpAlert.Size = new System.Drawing.Size(297, 90);
            this.grpAlert.TabIndex = 20;
            this.grpAlert.TabStop = false;
            this.grpAlert.Text = "Rare";
            // 
            // optAlertPlay
            // 
            this.optAlertPlay.Location = new System.Drawing.Point(10, 63);
            this.optAlertPlay.Name = "optAlertPlay";
            this.optAlertPlay.Size = new System.Drawing.Size(96, 19);
            this.optAlertPlay.TabIndex = 27;
            this.optAlertPlay.Text = "Play Wav";
            // 
            // optAlertSpeak
            // 
            this.optAlertSpeak.Location = new System.Drawing.Point(202, 40);
            this.optAlertSpeak.Name = "optAlertSpeak";
            this.optAlertSpeak.Size = new System.Drawing.Size(86, 19);
            this.optAlertSpeak.TabIndex = 26;
            this.optAlertSpeak.Text = "Speak";
            // 
            // optAlertBeep
            // 
            this.optAlertBeep.Location = new System.Drawing.Point(106, 40);
            this.optAlertBeep.Name = "optAlertBeep";
            this.optAlertBeep.Size = new System.Drawing.Size(86, 19);
            this.optAlertBeep.TabIndex = 25;
            this.optAlertBeep.Text = "Beep";
            // 
            // optAlertNone
            // 
            this.optAlertNone.Checked = true;
            this.optAlertNone.Location = new System.Drawing.Point(10, 40);
            this.optAlertNone.Name = "optAlertNone";
            this.optAlertNone.Size = new System.Drawing.Size(86, 19);
            this.optAlertNone.TabIndex = 24;
            this.optAlertNone.TabStop = true;
            this.optAlertNone.Text = "None";
            // 
            // txtAlertAudioFile
            // 
            this.txtAlertAudioFile.Location = new System.Drawing.Point(106, 63);
            this.txtAlertAudioFile.Name = "txtAlertAudioFile";
            this.txtAlertAudioFile.Size = new System.Drawing.Size(182, 22);
            this.txtAlertAudioFile.TabIndex = 28;
            // 
            // txtAlertPrefix
            // 
            this.txtAlertPrefix.Location = new System.Drawing.Point(106, 13);
            this.txtAlertPrefix.MaxLength = 5;
            this.txtAlertPrefix.Name = "txtAlertPrefix";
            this.txtAlertPrefix.Size = new System.Drawing.Size(38, 22);
            this.txtAlertPrefix.TabIndex = 22;
            this.txtAlertPrefix.Text = "[R]";
            // 
            // lblAlertPrefix
            // 
            this.lblAlertPrefix.Location = new System.Drawing.Point(10, 16);
            this.lblAlertPrefix.Name = "lblAlertPrefix";
            this.lblAlertPrefix.Size = new System.Drawing.Size(94, 19);
            this.lblAlertPrefix.TabIndex = 21;
            this.lblAlertPrefix.Text = "Prefix/Suffix:";
            // 
            // chkAlertMatchFull
            // 
            this.chkAlertMatchFull.Location = new System.Drawing.Point(163, 13);
            this.chkAlertMatchFull.Name = "chkAlertMatchFull";
            this.chkAlertMatchFull.Size = new System.Drawing.Size(125, 27);
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
            this.grpCaution.Location = new System.Drawing.Point(4, 144);
            this.grpCaution.Name = "grpCaution";
            this.grpCaution.Size = new System.Drawing.Size(297, 90);
            this.grpCaution.TabIndex = 11;
            this.grpCaution.TabStop = false;
            this.grpCaution.Text = "Caution";
            // 
            // optCautionPlay
            // 
            this.optCautionPlay.Location = new System.Drawing.Point(10, 63);
            this.optCautionPlay.Name = "optCautionPlay";
            this.optCautionPlay.Size = new System.Drawing.Size(96, 19);
            this.optCautionPlay.TabIndex = 18;
            this.optCautionPlay.Text = "Play Wav";
            // 
            // optCautionSpeak
            // 
            this.optCautionSpeak.Location = new System.Drawing.Point(202, 44);
            this.optCautionSpeak.Name = "optCautionSpeak";
            this.optCautionSpeak.Size = new System.Drawing.Size(86, 18);
            this.optCautionSpeak.TabIndex = 17;
            this.optCautionSpeak.Text = "Speak";
            // 
            // optCautionBeep
            // 
            this.optCautionBeep.Location = new System.Drawing.Point(106, 40);
            this.optCautionBeep.Name = "optCautionBeep";
            this.optCautionBeep.Size = new System.Drawing.Size(86, 19);
            this.optCautionBeep.TabIndex = 16;
            this.optCautionBeep.Text = "Beep";
            // 
            // optCautionNone
            // 
            this.optCautionNone.Checked = true;
            this.optCautionNone.Location = new System.Drawing.Point(10, 40);
            this.optCautionNone.Name = "optCautionNone";
            this.optCautionNone.Size = new System.Drawing.Size(86, 19);
            this.optCautionNone.TabIndex = 15;
            this.optCautionNone.TabStop = true;
            this.optCautionNone.Text = "None";
            // 
            // txtCautionAudioFile
            // 
            this.txtCautionAudioFile.Location = new System.Drawing.Point(106, 63);
            this.txtCautionAudioFile.Name = "txtCautionAudioFile";
            this.txtCautionAudioFile.Size = new System.Drawing.Size(182, 22);
            this.txtCautionAudioFile.TabIndex = 19;
            // 
            // txtCautionPrefix
            // 
            this.txtCautionPrefix.Location = new System.Drawing.Point(106, 13);
            this.txtCautionPrefix.MaxLength = 5;
            this.txtCautionPrefix.Name = "txtCautionPrefix";
            this.txtCautionPrefix.Size = new System.Drawing.Size(38, 22);
            this.txtCautionPrefix.TabIndex = 13;
            this.txtCautionPrefix.Text = "[C]";
            // 
            // lblCautionPrefix
            // 
            this.lblCautionPrefix.Location = new System.Drawing.Point(10, 16);
            this.lblCautionPrefix.Name = "lblCautionPrefix";
            this.lblCautionPrefix.Size = new System.Drawing.Size(96, 19);
            this.lblCautionPrefix.TabIndex = 12;
            this.lblCautionPrefix.Text = "Prefix/Suffix:";
            // 
            // chkCautionMatchFull
            // 
            this.chkCautionMatchFull.Location = new System.Drawing.Point(163, 13);
            this.chkCautionMatchFull.Name = "chkCautionMatchFull";
            this.chkCautionMatchFull.Size = new System.Drawing.Size(125, 27);
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
            this.grpHunt.Location = new System.Drawing.Point(4, 47);
            this.grpHunt.Name = "grpHunt";
            this.grpHunt.Size = new System.Drawing.Size(297, 90);
            this.grpHunt.TabIndex = 2;
            this.grpHunt.TabStop = false;
            this.grpHunt.Text = "Hunt";
            // 
            // optHuntPlay
            // 
            this.optHuntPlay.Location = new System.Drawing.Point(10, 63);
            this.optHuntPlay.Name = "optHuntPlay";
            this.optHuntPlay.Size = new System.Drawing.Size(96, 19);
            this.optHuntPlay.TabIndex = 9;
            this.optHuntPlay.Text = "Play Wav";
            // 
            // optHuntSpeak
            // 
            this.optHuntSpeak.Location = new System.Drawing.Point(202, 40);
            this.optHuntSpeak.Name = "optHuntSpeak";
            this.optHuntSpeak.Size = new System.Drawing.Size(86, 19);
            this.optHuntSpeak.TabIndex = 8;
            this.optHuntSpeak.Text = "Speak";
            // 
            // optHuntBeep
            // 
            this.optHuntBeep.Location = new System.Drawing.Point(106, 40);
            this.optHuntBeep.Name = "optHuntBeep";
            this.optHuntBeep.Size = new System.Drawing.Size(86, 19);
            this.optHuntBeep.TabIndex = 7;
            this.optHuntBeep.Text = "Beep";
            // 
            // optHuntNone
            // 
            this.optHuntNone.Checked = true;
            this.optHuntNone.Location = new System.Drawing.Point(10, 40);
            this.optHuntNone.Name = "optHuntNone";
            this.optHuntNone.Size = new System.Drawing.Size(86, 19);
            this.optHuntNone.TabIndex = 6;
            this.optHuntNone.TabStop = true;
            this.optHuntNone.Text = "None";
            // 
            // txtHuntAudioFile
            // 
            this.txtHuntAudioFile.Location = new System.Drawing.Point(106, 63);
            this.txtHuntAudioFile.Name = "txtHuntAudioFile";
            this.txtHuntAudioFile.Size = new System.Drawing.Size(182, 22);
            this.txtHuntAudioFile.TabIndex = 10;
            // 
            // txtHuntPrefix
            // 
            this.txtHuntPrefix.Location = new System.Drawing.Point(106, 13);
            this.txtHuntPrefix.MaxLength = 5;
            this.txtHuntPrefix.Name = "txtHuntPrefix";
            this.txtHuntPrefix.Size = new System.Drawing.Size(38, 22);
            this.txtHuntPrefix.TabIndex = 4;
            this.txtHuntPrefix.Text = "[H]";
            // 
            // lblHuntPrefix
            // 
            this.lblHuntPrefix.Location = new System.Drawing.Point(10, 16);
            this.lblHuntPrefix.Name = "lblHuntPrefix";
            this.lblHuntPrefix.Size = new System.Drawing.Size(96, 19);
            this.lblHuntPrefix.TabIndex = 3;
            this.lblHuntPrefix.Text = "Prefix/Suffix:";
            // 
            // chkHuntMatchFull
            // 
            this.chkHuntMatchFull.Location = new System.Drawing.Point(163, 13);
            this.chkHuntMatchFull.Name = "chkHuntMatchFull";
            this.chkHuntMatchFull.Size = new System.Drawing.Size(125, 27);
            this.chkHuntMatchFull.TabIndex = 5;
            this.chkHuntMatchFull.Text = "Match Full Text";
            // 
            // chkAffixAlerts
            // 
            this.chkAffixAlerts.Checked = true;
            this.chkAffixAlerts.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAffixAlerts.Location = new System.Drawing.Point(5, 25);
            this.chkAffixAlerts.Name = "chkAffixAlerts";
            this.chkAffixAlerts.Size = new System.Drawing.Size(134, 28);
            this.chkAffixAlerts.TabIndex = 1;
            this.chkAffixAlerts.Text = "Attach Suffix Text";
            // 
            // chkPrefixAlerts
            // 
            this.chkPrefixAlerts.Checked = true;
            this.chkPrefixAlerts.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPrefixAlerts.Location = new System.Drawing.Point(5, 3);
            this.chkPrefixAlerts.Name = "chkPrefixAlerts";
            this.chkPrefixAlerts.Size = new System.Drawing.Size(144, 28);
            this.chkPrefixAlerts.TabIndex = 0;
            this.chkPrefixAlerts.Text = "Attach Prefix Text";
            // 
            // chkCorpsesAlerts
            // 
            this.chkCorpsesAlerts.Checked = true;
            this.chkCorpsesAlerts.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCorpsesAlerts.Location = new System.Drawing.Point(182, 3);
            this.chkCorpsesAlerts.Name = "chkCorpsesAlerts";
            this.chkCorpsesAlerts.Size = new System.Drawing.Size(120, 28);
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
            this.tabMap.Location = new System.Drawing.Point(4, 25);
            this.tabMap.Name = "tabMap";
            this.tabMap.Size = new System.Drawing.Size(318, 433);
            this.tabMap.TabIndex = 2;
            this.tabMap.Text = "Map";
            // 
            // FadedLines
            // 
            this.FadedLines.Location = new System.Drawing.Point(210, 114);
            this.FadedLines.Name = "FadedLines";
            this.FadedLines.Size = new System.Drawing.Size(77, 22);
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
            this.lblFadedLines.Location = new System.Drawing.Point(7, 114);
            this.lblFadedLines.Name = "lblFadedLines";
            this.lblFadedLines.Size = new System.Drawing.Size(184, 17);
            this.lblFadedLines.TabIndex = 60;
            this.lblFadedLines.Text = "Dynamic Alpha Faded Lines";
            // 
            // lblPVPLevels
            // 
            this.lblPVPLevels.Location = new System.Drawing.Point(7, 88);
            this.lblPVPLevels.Name = "lblPVPLevels";
            this.lblPVPLevels.Size = new System.Drawing.Size(171, 18);
            this.lblPVPLevels.TabIndex = 58;
            this.lblPVPLevels.Text = "PVP Level Range:";
            // 
            // pvpLevels
            // 
            this.pvpLevels.Location = new System.Drawing.Point(210, 85);
            this.pvpLevels.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.pvpLevels.Name = "pvpLevels";
            this.pvpLevels.Size = new System.Drawing.Size(77, 22);
            this.pvpLevels.TabIndex = 59;
            this.pvpLevels.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.pvpLevels.ValueChanged += new System.EventHandler(this.pvpLevels_ValueChanged);
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
            this.groupBox1.Location = new System.Drawing.Point(4, 137);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(309, 134);
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
            this.cmbAlertSound.Location = new System.Drawing.Point(125, 77);
            this.cmbAlertSound.Name = "cmbAlertSound";
            this.cmbAlertSound.Size = new System.Drawing.Size(159, 24);
            this.cmbAlertSound.TabIndex = 54;
            this.cmbAlertSound.SelectionChangeCommitted += new System.EventHandler(this.cmbAlertSound_SelectionChangeCommitted);
            // 
            // cmbHatch
            // 
            this.cmbHatch.FormattingEnabled = true;
            this.cmbHatch.Location = new System.Drawing.Point(125, 48);
            this.cmbHatch.Name = "cmbHatch";
            this.cmbHatch.Size = new System.Drawing.Size(159, 24);
            this.cmbHatch.TabIndex = 53;
            this.cmbHatch.Tag = "";
            this.cmbHatch.SelectionChangeCommitted += new System.EventHandler(this.cmbHatch_SelectionChangeCommitted);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(5, 81);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(123, 18);
            this.label4.TabIndex = 58;
            this.label4.Text = "Alert Sound";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(5, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(123, 18);
            this.label2.TabIndex = 57;
            this.label2.Text = "Hatch Pattern";
            // 
            // chkColorRangeCircle
            // 
            this.chkColorRangeCircle.Location = new System.Drawing.Point(8, 23);
            this.chkColorRangeCircle.Name = "chkColorRangeCircle";
            this.chkColorRangeCircle.Size = new System.Drawing.Size(130, 19);
            this.chkColorRangeCircle.TabIndex = 5;
            this.chkColorRangeCircle.Text = "Range Circle";
            // 
            // spnRangeCircle
            // 
            this.spnRangeCircle.Location = new System.Drawing.Point(208, 22);
            this.spnRangeCircle.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.spnRangeCircle.Name = "spnRangeCircle";
            this.spnRangeCircle.Size = new System.Drawing.Size(76, 22);
            this.spnRangeCircle.TabIndex = 7;
            this.spnRangeCircle.Value = new decimal(new int[] {
            150,
            0,
            0,
            0});
            // 
            // numMinAlertLevel
            // 
            this.numMinAlertLevel.Location = new System.Drawing.Point(206, 105);
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
            this.numMinAlertLevel.Size = new System.Drawing.Size(77, 22);
            this.numMinAlertLevel.TabIndex = 55;
            this.numMinAlertLevel.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(5, 107);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(170, 19);
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
            this.groupBox2.Location = new System.Drawing.Point(4, 278);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(309, 150);
            this.groupBox2.TabIndex = 52;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Map Drawing Settings";
            // 
            // chkMap
            // 
            this.chkMap.Location = new System.Drawing.Point(170, 61);
            this.chkMap.Name = "chkMap";
            this.chkMap.Size = new System.Drawing.Size(113, 23);
            this.chkMap.TabIndex = 30;
            this.chkMap.Text = "Draw Map";
            // 
            // chkPlayer
            // 
            this.chkPlayer.Location = new System.Drawing.Point(170, 81);
            this.chkPlayer.Name = "chkPlayer";
            this.chkPlayer.Size = new System.Drawing.Size(113, 23);
            this.chkPlayer.TabIndex = 50;
            this.chkPlayer.Text = "Draw Player";
            // 
            // chkSpawns
            // 
            this.chkSpawns.Location = new System.Drawing.Point(170, 42);
            this.chkSpawns.Name = "chkSpawns";
            this.chkSpawns.Size = new System.Drawing.Size(113, 23);
            this.chkSpawns.TabIndex = 43;
            this.chkSpawns.Text = "Draw Spawns";
            // 
            // chkAddjust
            // 
            this.chkAddjust.Location = new System.Drawing.Point(7, 22);
            this.chkAddjust.Name = "chkAddjust";
            this.chkAddjust.Size = new System.Drawing.Size(145, 23);
            this.chkAddjust.TabIndex = 29;
            this.chkAddjust.Text = "Readjust Map";
            // 
            // chkGround
            // 
            this.chkGround.Location = new System.Drawing.Point(170, 120);
            this.chkGround.Name = "chkGround";
            this.chkGround.Size = new System.Drawing.Size(134, 23);
            this.chkGround.TabIndex = 51;
            this.chkGround.Text = "Ground Spawns";
            // 
            // chkTrails
            // 
            this.chkTrails.Location = new System.Drawing.Point(7, 100);
            this.chkTrails.Name = "chkTrails";
            this.chkTrails.Size = new System.Drawing.Size(156, 23);
            this.chkTrails.TabIndex = 45;
            this.chkTrails.Text = "Spawn Trails";
            // 
            // chkHighlight
            // 
            this.chkHighlight.Location = new System.Drawing.Point(7, 120);
            this.chkHighlight.Name = "chkHighlight";
            this.chkHighlight.Size = new System.Drawing.Size(156, 23);
            this.chkHighlight.TabIndex = 49;
            this.chkHighlight.Text = "Highlight Merchants";
            // 
            // chkGrid
            // 
            this.chkGrid.Location = new System.Drawing.Point(7, 42);
            this.chkGrid.Name = "chkGrid";
            this.chkGrid.Size = new System.Drawing.Size(156, 23);
            this.chkGrid.TabIndex = 37;
            this.chkGrid.Text = "Show Gridlines";
            // 
            // chkTimers
            // 
            this.chkTimers.Location = new System.Drawing.Point(170, 22);
            this.chkTimers.Name = "chkTimers";
            this.chkTimers.Size = new System.Drawing.Size(140, 23);
            this.chkTimers.TabIndex = 47;
            this.chkTimers.Text = "Spawn Timers";
            // 
            // chkText
            // 
            this.chkText.Location = new System.Drawing.Point(7, 81);
            this.chkText.Name = "chkText";
            this.chkText.Size = new System.Drawing.Size(156, 23);
            this.chkText.TabIndex = 41;
            this.chkText.Text = "Show Zone Text";
            // 
            // chkDirection
            // 
            this.chkDirection.Location = new System.Drawing.Point(170, 100);
            this.chkDirection.Name = "chkDirection";
            this.chkDirection.Size = new System.Drawing.Size(131, 23);
            this.chkDirection.TabIndex = 46;
            this.chkDirection.Text = "Heading Lines";
            // 
            // chkLineToPoint
            // 
            this.chkLineToPoint.Location = new System.Drawing.Point(7, 61);
            this.chkLineToPoint.Name = "chkLineToPoint";
            this.chkLineToPoint.Size = new System.Drawing.Size(156, 23);
            this.chkLineToPoint.TabIndex = 42;
            this.chkLineToPoint.Text = "Draw Line to Point";
            // 
            // lblSpawnSize
            // 
            this.lblSpawnSize.Location = new System.Drawing.Point(7, 59);
            this.lblSpawnSize.Name = "lblSpawnSize";
            this.lblSpawnSize.Size = new System.Drawing.Size(171, 18);
            this.lblSpawnSize.TabIndex = 20;
            this.lblSpawnSize.Text = "Spawn Draw Size:";
            // 
            // chkSelectSpawnList
            // 
            this.chkSelectSpawnList.Location = new System.Drawing.Point(10, 37);
            this.chkSelectSpawnList.Name = "chkSelectSpawnList";
            this.chkSelectSpawnList.Size = new System.Drawing.Size(297, 18);
            this.chkSelectSpawnList.TabIndex = 4;
            this.chkSelectSpawnList.Text = "Auto Select Spawn in the Spawn List";
            // 
            // spnSpawnSize
            // 
            this.spnSpawnSize.Location = new System.Drawing.Point(210, 57);
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
            this.spnSpawnSize.Size = new System.Drawing.Size(77, 22);
            this.spnSpawnSize.TabIndex = 21;
            this.spnSpawnSize.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.spnSpawnSize.ValueChanged += new System.EventHandler(this.spnSpawnSize_ValueChanged);
            // 
            // chkShowTargetInfo
            // 
            this.chkShowTargetInfo.Location = new System.Drawing.Point(10, 14);
            this.chkShowTargetInfo.Name = "chkShowTargetInfo";
            this.chkShowTargetInfo.Size = new System.Drawing.Size(297, 18);
            this.chkShowTargetInfo.TabIndex = 3;
            this.chkShowTargetInfo.Text = "Show Target Information Window";
            // 
            // chkDrawFoV
            // 
            this.chkDrawFoV.Location = new System.Drawing.Point(10, -28);
            this.chkDrawFoV.Name = "chkDrawFoV";
            this.chkDrawFoV.Size = new System.Drawing.Size(297, 19);
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
            this.tabGeneral.Location = new System.Drawing.Point(4, 25);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Size = new System.Drawing.Size(318, 433);
            this.tabGeneral.TabIndex = 0;
            this.tabGeneral.Text = "General";
            // 
            // chkShowCharName
            // 
            this.chkShowCharName.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkShowCharName.Checked = true;
            this.chkShowCharName.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowCharName.Location = new System.Drawing.Point(161, 369);
            this.chkShowCharName.Name = "chkShowCharName";
            this.chkShowCharName.Size = new System.Drawing.Size(137, 28);
            this.chkShowCharName.TabIndex = 27;
            this.chkShowCharName.Text = "Show Char Name";
            // 
            // txtSearchString
            // 
            this.txtSearchString.Location = new System.Drawing.Point(73, 397);
            this.txtSearchString.Name = "txtSearchString";
            this.txtSearchString.Size = new System.Drawing.Size(234, 22);
            this.txtSearchString.TabIndex = 26;
            // 
            // lblSearch
            // 
            this.lblSearch.Location = new System.Drawing.Point(10, 400);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(56, 20);
            this.lblSearch.TabIndex = 25;
            this.lblSearch.Text = "Search:";
            // 
            // spnLogLevel
            // 
            this.spnLogLevel.Location = new System.Drawing.Point(230, 295);
            this.spnLogLevel.Name = "spnLogLevel";
            this.spnLogLevel.Size = new System.Drawing.Size(77, 22);
            this.spnLogLevel.TabIndex = 21;
            // 
            // lblLogLevel
            // 
            this.lblLogLevel.Location = new System.Drawing.Point(10, 295);
            this.lblLogLevel.Name = "lblLogLevel";
            this.lblLogLevel.Size = new System.Drawing.Size(163, 19);
            this.lblLogLevel.TabIndex = 20;
            this.lblLogLevel.Text = "Error Logging Level:";
            // 
            // chkShowZoneName
            // 
            this.chkShowZoneName.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkShowZoneName.Checked = true;
            this.chkShowZoneName.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowZoneName.Location = new System.Drawing.Point(10, 369);
            this.chkShowZoneName.Name = "chkShowZoneName";
            this.chkShowZoneName.Size = new System.Drawing.Size(136, 28);
            this.chkShowZoneName.TabIndex = 24;
            this.chkShowZoneName.Text = "Show Zone Name";
            // 
            // spnOverrideLevel
            // 
            this.spnOverrideLevel.Location = new System.Drawing.Point(230, 240);
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
            this.spnOverrideLevel.Size = new System.Drawing.Size(77, 22);
            this.spnOverrideLevel.TabIndex = 15;
            this.spnOverrideLevel.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // spnUpdateDelay
            // 
            this.spnUpdateDelay.Location = new System.Drawing.Point(230, 268);
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
            this.spnUpdateDelay.Size = new System.Drawing.Size(77, 22);
            this.spnUpdateDelay.TabIndex = 17;
            this.spnUpdateDelay.Value = new decimal(new int[] {
            250,
            0,
            0,
            0});
            // 
            // txtWindowName
            // 
            this.txtWindowName.Location = new System.Drawing.Point(10, 342);
            this.txtWindowName.Name = "txtWindowName";
            this.txtWindowName.Size = new System.Drawing.Size(297, 22);
            this.txtWindowName.TabIndex = 23;
            // 
            // lblWindowName
            // 
            this.lblWindowName.Location = new System.Drawing.Point(10, 323);
            this.lblWindowName.Name = "lblWindowName";
            this.lblWindowName.Size = new System.Drawing.Size(172, 19);
            this.lblWindowName.TabIndex = 22;
            this.lblWindowName.Text = "Window Title:";
            // 
            // lblOverridelevel
            // 
            this.lblOverridelevel.Location = new System.Drawing.Point(10, 240);
            this.lblOverridelevel.Name = "lblOverridelevel";
            this.lblOverridelevel.Size = new System.Drawing.Size(163, 18);
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
            this.gbServer.Location = new System.Drawing.Point(10, 9);
            this.gbServer.Name = "gbServer";
            this.gbServer.Size = new System.Drawing.Size(297, 194);
            this.gbServer.TabIndex = 0;
            this.gbServer.TabStop = false;
            this.gbServer.Text = "Server";
            // 
            // txtIPAddress5
            // 
            this.txtIPAddress5.Location = new System.Drawing.Point(154, 129);
            this.txtIPAddress5.Name = "txtIPAddress5";
            this.txtIPAddress5.Size = new System.Drawing.Size(134, 22);
            this.txtIPAddress5.TabIndex = 10;
            // 
            // lblIPAddress5
            // 
            this.lblIPAddress5.Location = new System.Drawing.Point(10, 129);
            this.lblIPAddress5.Name = "lblIPAddress5";
            this.lblIPAddress5.Size = new System.Drawing.Size(144, 19);
            this.lblIPAddress5.TabIndex = 9;
            this.lblIPAddress5.Text = "IP Address 5: (Ctrl + 5)";
            // 
            // txtIPAddress4
            // 
            this.txtIPAddress4.Location = new System.Drawing.Point(154, 102);
            this.txtIPAddress4.Name = "txtIPAddress4";
            this.txtIPAddress4.Size = new System.Drawing.Size(134, 22);
            this.txtIPAddress4.TabIndex = 8;
            // 
            // lblIPAddress4
            // 
            this.lblIPAddress4.Location = new System.Drawing.Point(10, 102);
            this.lblIPAddress4.Name = "lblIPAddress4";
            this.lblIPAddress4.Size = new System.Drawing.Size(144, 18);
            this.lblIPAddress4.TabIndex = 7;
            this.lblIPAddress4.Text = "IP Address 4: (Ctrl + 4)";
            // 
            // txtIPAddress3
            // 
            this.txtIPAddress3.Location = new System.Drawing.Point(154, 74);
            this.txtIPAddress3.Name = "txtIPAddress3";
            this.txtIPAddress3.Size = new System.Drawing.Size(134, 22);
            this.txtIPAddress3.TabIndex = 6;
            // 
            // lblIPAddress3
            // 
            this.lblIPAddress3.Location = new System.Drawing.Point(10, 74);
            this.lblIPAddress3.Name = "lblIPAddress3";
            this.lblIPAddress3.Size = new System.Drawing.Size(144, 18);
            this.lblIPAddress3.TabIndex = 5;
            this.lblIPAddress3.Text = "IP Address 3: (Ctrl + 3)";
            // 
            // txtIPAddress2
            // 
            this.txtIPAddress2.Location = new System.Drawing.Point(154, 46);
            this.txtIPAddress2.Name = "txtIPAddress2";
            this.txtIPAddress2.Size = new System.Drawing.Size(134, 22);
            this.txtIPAddress2.TabIndex = 4;
            // 
            // lblIPAddress2
            // 
            this.lblIPAddress2.Location = new System.Drawing.Point(10, 46);
            this.lblIPAddress2.Name = "lblIPAddress2";
            this.lblIPAddress2.Size = new System.Drawing.Size(144, 19);
            this.lblIPAddress2.TabIndex = 3;
            this.lblIPAddress2.Text = "IP Address 2: (Ctrl + 2)";
            // 
            // txtPortNo
            // 
            this.txtPortNo.Location = new System.Drawing.Point(154, 157);
            this.txtPortNo.Name = "txtPortNo";
            this.txtPortNo.Size = new System.Drawing.Size(134, 22);
            this.txtPortNo.TabIndex = 12;
            this.txtPortNo.Text = "5555";
            // 
            // txtIPAddress1
            // 
            this.txtIPAddress1.Location = new System.Drawing.Point(154, 18);
            this.txtIPAddress1.Name = "txtIPAddress1";
            this.txtIPAddress1.Size = new System.Drawing.Size(134, 22);
            this.txtIPAddress1.TabIndex = 2;
            this.txtIPAddress1.Text = "localhost";
            // 
            // lblIPAddress1
            // 
            this.lblIPAddress1.Location = new System.Drawing.Point(10, 18);
            this.lblIPAddress1.Name = "lblIPAddress1";
            this.lblIPAddress1.Size = new System.Drawing.Size(144, 19);
            this.lblIPAddress1.TabIndex = 1;
            this.lblIPAddress1.Text = "IP Address 1: (Ctrl + 1)";
            // 
            // lbltxtPortNo
            // 
            this.lbltxtPortNo.Location = new System.Drawing.Point(10, 157);
            this.lbltxtPortNo.Name = "lbltxtPortNo";
            this.lbltxtPortNo.Size = new System.Drawing.Size(144, 18);
            this.lbltxtPortNo.TabIndex = 11;
            this.lbltxtPortNo.Text = "Port:";
            // 
            // lblUpdateDelay
            // 
            this.lblUpdateDelay.Location = new System.Drawing.Point(10, 268);
            this.lblUpdateDelay.Name = "lblUpdateDelay";
            this.lblUpdateDelay.Size = new System.Drawing.Size(163, 18);
            this.lblUpdateDelay.TabIndex = 16;
            this.lblUpdateDelay.Text = "Update Delay (MS):";
            // 
            // chkSaveOnExit
            // 
            this.chkSaveOnExit.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkSaveOnExit.Checked = true;
            this.chkSaveOnExit.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSaveOnExit.Location = new System.Drawing.Point(10, 203);
            this.chkSaveOnExit.Name = "chkSaveOnExit";
            this.chkSaveOnExit.Size = new System.Drawing.Size(236, 28);
            this.chkSaveOnExit.TabIndex = 13;
            this.chkSaveOnExit.Text = "Save Preferences On Exit:";
            // 
            // tabOptions
            // 
            this.tabOptions.Controls.Add(this.tabGeneral);
            this.tabOptions.Controls.Add(this.tabMap);
            this.tabOptions.Controls.Add(this.tabAlerts);
            this.tabOptions.Controls.Add(this.tabFolders);
            this.tabOptions.Controls.Add(this.tabColors);
            this.tabOptions.Controls.Add(this.tabPage1);
            this.tabOptions.Location = new System.Drawing.Point(10, 9);
            this.tabOptions.Name = "tabOptions";
            this.tabOptions.SelectedIndex = 0;
            this.tabOptions.Size = new System.Drawing.Size(326, 462);
            this.tabOptions.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.lblCCEmail);
            this.tabPage1.Controls.Add(this.textSMTPCCEmail);
            this.tabPage1.Controls.Add(this.btnTestEmail);
            this.tabPage1.Controls.Add(this.textSMTPToEmail);
            this.tabPage1.Controls.Add(this.lblToAddress);
            this.tabPage1.Controls.Add(this.textSMTPFromEmail);
            this.tabPage1.Controls.Add(this.lblFromAddress);
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(318, 433);
            this.tabPage1.TabIndex = 5;
            this.tabPage1.Text = "SMTP";
            // 
            // lblCCEmail
            // 
            this.lblCCEmail.AutoSize = true;
            this.lblCCEmail.Location = new System.Drawing.Point(24, 324);
            this.lblCCEmail.Name = "lblCCEmail";
            this.lblCCEmail.Size = new System.Drawing.Size(30, 17);
            this.lblCCEmail.TabIndex = 12;
            this.lblCCEmail.Text = "CC:";
            // 
            // textSMTPCCEmail
            // 
            this.textSMTPCCEmail.Location = new System.Drawing.Point(60, 321);
            this.textSMTPCCEmail.Name = "textSMTPCCEmail";
            this.textSMTPCCEmail.Size = new System.Drawing.Size(242, 22);
            this.textSMTPCCEmail.TabIndex = 10;
            // 
            // btnTestEmail
            // 
            this.btnTestEmail.Location = new System.Drawing.Point(60, 370);
            this.btnTestEmail.Name = "btnTestEmail";
            this.btnTestEmail.Size = new System.Drawing.Size(185, 27);
            this.btnTestEmail.TabIndex = 11;
            this.btnTestEmail.Text = "Send Test Email";
            this.btnTestEmail.UseVisualStyleBackColor = true;
            this.btnTestEmail.Click += new System.EventHandler(this.btnTestEmail_Click);
            // 
            // textSMTPToEmail
            // 
            this.textSMTPToEmail.Location = new System.Drawing.Point(60, 291);
            this.textSMTPToEmail.Name = "textSMTPToEmail";
            this.textSMTPToEmail.Size = new System.Drawing.Size(242, 22);
            this.textSMTPToEmail.TabIndex = 9;
            // 
            // lblToAddress
            // 
            this.lblToAddress.AutoSize = true;
            this.lblToAddress.Location = new System.Drawing.Point(25, 294);
            this.lblToAddress.Name = "lblToAddress";
            this.lblToAddress.Size = new System.Drawing.Size(29, 17);
            this.lblToAddress.TabIndex = 4;
            this.lblToAddress.Text = "To:";
            // 
            // textSMTPFromEmail
            // 
            this.textSMTPFromEmail.Location = new System.Drawing.Point(60, 260);
            this.textSMTPFromEmail.Name = "textSMTPFromEmail";
            this.textSMTPFromEmail.Size = new System.Drawing.Size(242, 22);
            this.textSMTPFromEmail.TabIndex = 8;
            // 
            // lblFromAddress
            // 
            this.lblFromAddress.AutoSize = true;
            this.lblFromAddress.Location = new System.Drawing.Point(13, 263);
            this.lblFromAddress.Name = "lblFromAddress";
            this.lblFromAddress.Size = new System.Drawing.Size(44, 17);
            this.lblFromAddress.TabIndex = 2;
            this.lblFromAddress.Text = "From:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lblSMTPDomain);
            this.groupBox3.Controls.Add(this.textBoxSMTPDomain);
            this.groupBox3.Controls.Add(this.checkBoxSavePassword);
            this.groupBox3.Controls.Add(this.checkBoxSMTPUseNetworkCredentials);
            this.groupBox3.Controls.Add(this.checkBoxSMTPUseSecureAuthentication);
            this.groupBox3.Controls.Add(this.textSMTPPassword);
            this.groupBox3.Controls.Add(this.lblSMTPPort);
            this.groupBox3.Controls.Add(this.textSMTPPort);
            this.groupBox3.Controls.Add(this.lblSMTPUsername);
            this.groupBox3.Controls.Add(this.lblSMTPAddress);
            this.groupBox3.Controls.Add(this.textSMTPAddress);
            this.groupBox3.Controls.Add(this.textSMTPUsername);
            this.groupBox3.Controls.Add(this.lblSMTPPassword);
            this.groupBox3.Location = new System.Drawing.Point(4, 7);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(306, 246);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "SMTP Setup";
            // 
            // lblSMTPDomain
            // 
            this.lblSMTPDomain.AutoSize = true;
            this.lblSMTPDomain.Location = new System.Drawing.Point(6, 68);
            this.lblSMTPDomain.Name = "lblSMTPDomain";
            this.lblSMTPDomain.Size = new System.Drawing.Size(120, 17);
            this.lblSMTPDomain.TabIndex = 11;
            this.lblSMTPDomain.Text = "Domain (optional)";
            // 
            // textBoxSMTPDomain
            // 
            this.textBoxSMTPDomain.Location = new System.Drawing.Point(120, 65);
            this.textBoxSMTPDomain.Name = "textBoxSMTPDomain";
            this.textBoxSMTPDomain.Size = new System.Drawing.Size(179, 22);
            this.textBoxSMTPDomain.TabIndex = 2;
            // 
            // checkBoxSavePassword
            // 
            this.checkBoxSavePassword.AutoSize = true;
            this.checkBoxSavePassword.Location = new System.Drawing.Point(102, 211);
            this.checkBoxSavePassword.Name = "checkBoxSavePassword";
            this.checkBoxSavePassword.Size = new System.Drawing.Size(127, 21);
            this.checkBoxSavePassword.TabIndex = 7;
            this.checkBoxSavePassword.Text = "Save Password";
            this.checkBoxSavePassword.UseVisualStyleBackColor = true;
            this.checkBoxSavePassword.Click += new System.EventHandler(this.checkBoxSavePassword_Click);
            // 
            // checkBoxSMTPUseNetworkCredentials
            // 
            this.checkBoxSMTPUseNetworkCredentials.AutoSize = true;
            this.checkBoxSMTPUseNetworkCredentials.Location = new System.Drawing.Point(10, 121);
            this.checkBoxSMTPUseNetworkCredentials.Name = "checkBoxSMTPUseNetworkCredentials";
            this.checkBoxSMTPUseNetworkCredentials.Size = new System.Drawing.Size(185, 21);
            this.checkBoxSMTPUseNetworkCredentials.TabIndex = 4;
            this.checkBoxSMTPUseNetworkCredentials.Text = "Use Network Credentials";
            this.checkBoxSMTPUseNetworkCredentials.UseVisualStyleBackColor = true;
            this.checkBoxSMTPUseNetworkCredentials.Click += new System.EventHandler(this.checkBoxSMTPUseNetworkCredentials_Click);
            // 
            // checkBoxSMTPUseSecureAuthentication
            // 
            this.checkBoxSMTPUseSecureAuthentication.AutoSize = true;
            this.checkBoxSMTPUseSecureAuthentication.Location = new System.Drawing.Point(10, 95);
            this.checkBoxSMTPUseSecureAuthentication.Name = "checkBoxSMTPUseSecureAuthentication";
            this.checkBoxSMTPUseSecureAuthentication.Size = new System.Drawing.Size(198, 21);
            this.checkBoxSMTPUseSecureAuthentication.TabIndex = 3;
            this.checkBoxSMTPUseSecureAuthentication.Text = "Use Secure Authentication";
            this.checkBoxSMTPUseSecureAuthentication.UseVisualStyleBackColor = true;
            this.checkBoxSMTPUseSecureAuthentication.Click += new System.EventHandler(this.checkBoxSMTPUseSecureAuthentication_Click);
            // 
            // textSMTPPassword
            // 
            this.textSMTPPassword.Location = new System.Drawing.Point(102, 181);
            this.textSMTPPassword.Name = "textSMTPPassword";
            this.textSMTPPassword.PasswordChar = '*';
            this.textSMTPPassword.Size = new System.Drawing.Size(164, 22);
            this.textSMTPPassword.TabIndex = 6;
            this.textSMTPPassword.UseSystemPasswordChar = true;
            // 
            // lblSMTPPort
            // 
            this.lblSMTPPort.AutoSize = true;
            this.lblSMTPPort.Location = new System.Drawing.Point(245, 18);
            this.lblSMTPPort.Name = "lblSMTPPort";
            this.lblSMTPPort.Size = new System.Drawing.Size(38, 17);
            this.lblSMTPPort.TabIndex = 8;
            this.lblSMTPPort.Text = "Port:";
            // 
            // textSMTPPort
            // 
            this.textSMTPPort.Location = new System.Drawing.Point(248, 37);
            this.textSMTPPort.Name = "textSMTPPort";
            this.textSMTPPort.Size = new System.Drawing.Size(51, 22);
            this.textSMTPPort.TabIndex = 1;
            this.textSMTPPort.Text = "25";
            // 
            // lblSMTPUsername
            // 
            this.lblSMTPUsername.AutoSize = true;
            this.lblSMTPUsername.Location = new System.Drawing.Point(28, 151);
            this.lblSMTPUsername.Name = "lblSMTPUsername";
            this.lblSMTPUsername.Size = new System.Drawing.Size(77, 17);
            this.lblSMTPUsername.TabIndex = 6;
            this.lblSMTPUsername.Text = "Username:";
            // 
            // lblSMTPAddress
            // 
            this.lblSMTPAddress.AutoSize = true;
            this.lblSMTPAddress.Location = new System.Drawing.Point(6, 18);
            this.lblSMTPAddress.Name = "lblSMTPAddress";
            this.lblSMTPAddress.Size = new System.Drawing.Size(152, 17);
            this.lblSMTPAddress.TabIndex = 5;
            this.lblSMTPAddress.Text = "SMTP Server Address:";
            // 
            // textSMTPAddress
            // 
            this.textSMTPAddress.Location = new System.Drawing.Point(10, 37);
            this.textSMTPAddress.Name = "textSMTPAddress";
            this.textSMTPAddress.Size = new System.Drawing.Size(231, 22);
            this.textSMTPAddress.TabIndex = 0;
            // 
            // textSMTPUsername
            // 
            this.textSMTPUsername.Location = new System.Drawing.Point(102, 148);
            this.textSMTPUsername.Name = "textSMTPUsername";
            this.textSMTPUsername.Size = new System.Drawing.Size(164, 22);
            this.textSMTPUsername.TabIndex = 5;
            // 
            // lblSMTPPassword
            // 
            this.lblSMTPPassword.AutoSize = true;
            this.lblSMTPPassword.Location = new System.Drawing.Point(30, 185);
            this.lblSMTPPassword.Name = "lblSMTPPassword";
            this.lblSMTPPassword.Size = new System.Drawing.Size(73, 17);
            this.lblSMTPPassword.TabIndex = 2;
            this.lblSMTPPassword.Text = "Password:";
            // 
            // cmdCancel
            // 
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(230, 480);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(102, 27);
            this.cmdCancel.TabIndex = 2;
            this.cmdCancel.Text = "Cancel";
            // 
            // frmOptions
            // 
            this.AcceptButton = this.cmdCommand;
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CancelButton = this.cmdCommand;
            this.ClientSize = new System.Drawing.Size(347, 519);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.tabOptions);
            this.Controls.Add(this.cmdCommand);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmOptions";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.TopMost = true;
            this.tabColors.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picGridLabelColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPlayerBorder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picListBackgroundColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picRangeCircleColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGridColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMapBackgroundColor)).EndInit();
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
            ((System.ComponentModel.ISupportInitialize)(this.FadedLines)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pvpLevels)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spnRangeCircle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMinAlertLevel)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spnSpawnSize)).EndInit();
            this.tabGeneral.ResumeLayout(false);
            this.tabGeneral.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spnLogLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spnOverrideLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spnUpdateDelay)).EndInit();
            this.gbServer.ResumeLayout(false);
            this.gbServer.PerformLayout();
            this.tabOptions.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion



		private void cmdCommand_Click(object sender, System.EventArgs e) {

            UpdateSMTPSettings();

			bool done = true;

			if (!Directory.Exists(this.txtMapDir.Text))

				if (DialogResult.Yes == MessageBox.Show("Map directory doesn't exist.  Create it?", "Directory Not Found", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))

					Directory.CreateDirectory(this.txtMapDir.Text);

				else done = false;

			if (!Directory.Exists(this.txtFilterDir.Text))

				if (DialogResult.Yes == MessageBox.Show("Filter directory doesn't exist.  Create it?", "Directory Not Found", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))

					Directory.CreateDirectory(this.txtFilterDir.Text);

				else done = false;

			if (!Directory.Exists(this.txtCfgDir.Text))

				if (DialogResult.Yes == MessageBox.Show("Config directory doesn't exist.  Create it?", "Directory Not Found", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))

					Directory.CreateDirectory(this.txtCfgDir.Text);

				else done = false;

			if (!Directory.Exists(this.txtLogDir.Text))

				if (DialogResult.Yes == MessageBox.Show("Log directory doesn't exist.  Create it?", "Directory Not Found", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))

					Directory.CreateDirectory(this.txtLogDir.Text);

				else done = false;

			if (!Directory.Exists(this.txtTimerDir.Text))

				if (DialogResult.Yes == MessageBox.Show("Spawn Timer directory doesn't exist.  Create it?", "Directory Not Found", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))

					Directory.CreateDirectory(this.txtTimerDir.Text);

				else done = false;



			if (done) Hide();
            

		}





		private void cmdMapBackgroundColor_Click(object sender, System.EventArgs e)

		{

			colorOptionPicker.Color = picMapBackgroundColor.BackColor;

			if(colorOptionPicker.ShowDialog() != DialogResult.Cancel) 

			{

				picMapBackgroundColor.BackColor = colorOptionPicker.Color;

			}

		}



		private void cmdListBackgroundColor_Click(object sender, System.EventArgs e)

		{

			colorOptionPicker.Color = picListBackgroundColor.BackColor;

			if(colorOptionPicker.ShowDialog() != DialogResult.Cancel)

			{

				Settings.Instance.ListBackColor = colorOptionPicker.Color;

				picListBackgroundColor.BackColor = colorOptionPicker.Color;

			}

		}



		private void cmdGridColor_Click(object sender, System.EventArgs e)

		{

			colorOptionPicker.Color = picGridColor.BackColor;

			if(colorOptionPicker.ShowDialog() != DialogResult.Cancel) 

			{

				Settings.Instance.GridColor = colorOptionPicker.Color;

				picGridColor.BackColor = colorOptionPicker.Color;

			}

		}





		private void cmdRangeCircleColor_Click(object sender, System.EventArgs e)

		{

			colorOptionPicker.Color = picRangeCircleColor.BackColor;

			if(colorOptionPicker.ShowDialog() != DialogResult.Cancel) 

			{

				Settings.Instance.RangeCircleColor  = colorOptionPicker.Color;

				picRangeCircleColor.BackColor = colorOptionPicker.Color;

			}

		}



		private void cmdMapDirBrowse_Click(object sender, System.EventArgs e)

		{

			this.fldrBrowser.Description = "Map Directory";

            this.fldrBrowser.SelectedPath = Settings.Instance.MapDir;

			fldrBrowser.ShowDialog();

			if (fldrBrowser.SelectedPath.Trim() != "")

				this.txtMapDir.Text = fldrBrowser.SelectedPath;

		}



		private void cmdCfgDirBrowse_Click(object sender, System.EventArgs e)

		{

			this.fldrBrowser.Description = "Config Directory";

            this.fldrBrowser.SelectedPath = Settings.Instance.CfgDir;

			fldrBrowser.ShowDialog();

            if (fldrBrowser.SelectedPath.Trim() != "")

                this.txtCfgDir.Text = fldrBrowser.SelectedPath;

		}



		private void cmdFilterDirBrowse_Click(object sender, System.EventArgs e)

		{

            this.fldrBrowser.Description = "Filter Directory";

            this.fldrBrowser.SelectedPath = Settings.Instance.FilterDir;

			fldrBrowser.ShowDialog();

            if (fldrBrowser.SelectedPath.Trim() != "")

                this.txtFilterDir.Text = fldrBrowser.SelectedPath;

		}



		private void cmdLogDir_Click(object sender, System.EventArgs e)

		{

            this.fldrBrowser.Description = "Log Directory";

            this.fldrBrowser.SelectedPath = Settings.Instance.LogDir;

			fldrBrowser.ShowDialog();

            if (fldrBrowser.SelectedPath.Trim() != "")

                this.txtLogDir.Text = fldrBrowser.SelectedPath;

		}

        private void cmdSpawnTimers_Click(object sender, EventArgs e)

        {

            this.fldrBrowser.Description = "Timers Directory";

            this.fldrBrowser.SelectedPath = Settings.Instance.TimerDir;

            fldrBrowser.ShowDialog();

            if (fldrBrowser.SelectedPath.Trim() != "")

                this.txtTimerDir.Text = fldrBrowser.SelectedPath;

        }

		public DrawOptions GetDrawOptions()

		{

			DrawOptions DrawOpts = DrawOptions.DrawNone;



			if (chkMap.Checked) DrawOpts=DrawOpts|DrawOptions.DrawMap;

			if (chkAddjust.Checked) DrawOpts=DrawOpts|DrawOptions.Readjust;

			if (chkPlayer.Checked) DrawOpts=DrawOpts|DrawOptions.Player;

			if (chkLineToPoint.Checked) DrawOpts=DrawOpts|DrawOptions.SpotLine;

			if (chkSpawns.Checked) DrawOpts=DrawOpts|DrawOptions.Spawns;

			if (chkTrails.Checked) DrawOpts=DrawOpts|DrawOptions.SpawnTrails;

			if (chkGround.Checked) DrawOpts=DrawOpts|DrawOptions.GroundItems;

			if (chkTimers.Checked) DrawOpts=DrawOpts|DrawOptions.SpawnTimers;

			if (chkDirection.Checked) DrawOpts=DrawOpts|DrawOptions.DirectionLines;

			if (chkHighlight.Checked) DrawOpts=DrawOpts|DrawOptions.SpawnRings;

			if (chkGrid.Checked) DrawOpts=DrawOpts|DrawOptions.GridLines;

			if (chkText.Checked) DrawOpts=DrawOpts|DrawOptions.ZoneText;



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

		private void butPlayerBorder_Click(object sender, System.EventArgs e)

		{

			colorOptionPicker.Color = picPlayerBorder.BackColor;

			if(colorOptionPicker.ShowDialog() != DialogResult.Cancel) 

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

                    System.Media.SystemSounds.Asterisk.Play();

                    break;

                case "Beep":

                    System.Media.SystemSounds.Beep.Play();

                    break;

                case "Exclamation":

                    System.Media.SystemSounds.Exclamation.Play();

                    break;

                case "Hand":

                    System.Media.SystemSounds.Hand.Play();

                    break;

                case "Question":

                    System.Media.SystemSounds.Question.Play();

                    break;

                default:

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

            Settings.Instance.SpawnDrawSize = (int)this.spnSpawnSize.Value;

        }

        private void pvpLevels_ValueChanged(object sender, EventArgs e)

        {

            Settings.Instance.PVPLevels = (int)this.pvpLevels.Value;

        }

        private void checkBoxSMTPUseNetworkCredentials_Click(object sender, EventArgs e)
        {
            SMTPSettings.Instance.UseNetworkCredentials = !SMTPSettings.Instance.UseNetworkCredentials;

            checkBoxSMTPUseNetworkCredentials.Checked = SMTPSettings.Instance.UseNetworkCredentials;

            if (SMTPSettings.Instance.UseNetworkCredentials == true)
            {
                this.textSMTPPassword.Enabled = false;
                this.textSMTPUsername.Enabled = false;
                this.lblSMTPPassword.Enabled = false;
                this.lblSMTPUsername.Enabled = false;
            }
            else
            {
                this.textSMTPPassword.Enabled = true;
                this.textSMTPUsername.Enabled = true;
                this.lblSMTPPassword.Enabled = true;
                this.lblSMTPUsername.Enabled = true;
            }
        }
        private void UpdateSMTPSettings()
        {
            SMTPSettings.Instance.UseSSL = this.checkBoxSMTPUseSecureAuthentication.Checked;
            if (this.textSMTPPassword.ToString().Length > 0 && this.textSMTPPassword.Text.ToString() != "Password:::1") {
                SMTPSettings.Instance.SmtpPassword = this.textSMTPPassword.Text.ToString();
                //char[] pass = this.textSMTPPassword.Text.ToCharArray();
                //Settings.Instance.SmtpPassword.Clear();
                //foreach (char d in pass)
                //{
                //    Settings.Instance.SmtpPassword.AppendChar(d);
                //}
                //this.textSMTPPassword.Text = "Password:::1";
            }

            if (this.textSMTPPassword.ToString().Length == 0 && SMTPSettings.Instance.SmtpPassword.ToString().Length > 0)
                SMTPSettings.Instance.SmtpPassword = "";
                //Settings.Instance.SmtpPassword.Clear();
            SMTPSettings.Instance.SmtpUsername = this.textSMTPUsername.Text.ToString();
            SMTPSettings.Instance.SmtpDomain = this.textBoxSMTPDomain.Text.ToString();

            // make sure value for port is an int
            int Num;
            if (this.textSMTPPort.Text.ToString().Length > 0)
            {
                string Str = this.textSMTPPort.Text.ToString();
                bool isNum = int.TryParse(Str, out Num);
                if (isNum && Num > 0)
                {
                    SMTPSettings.Instance.SmtpPort = Num;
                }
                else
                {
                    this.textSMTPPort.Text = SMTPSettings.Instance.SmtpPort.ToString();
                }
            }

            // Check if emails entered look like email addresses
            string emailPattern = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(emailPattern);

            // From Email Address
            if (textSMTPFromEmail.Text.ToString().Length > 0)
                if (re.IsMatch(textSMTPFromEmail.Text.ToString()))
                    SMTPSettings.Instance.FromEmail = textSMTPFromEmail.Text.ToString();

            // To Email Address (can contain multiple addresses seperated by comma or semicolon
            if (textSMTPToEmail.Text.ToString().Length == 0)
                SMTPSettings.Instance.ToEmail = "";
            // split of To: email string with a , or ; delimiter, and verify
            if (textSMTPToEmail.Text.ToString().Length > 0)
            {
                string presplit = textSMTPToEmail.Text.ToString();
                string postsplit = "";
                string delim = ",;<>[] ";
                char[] delimarray = delim.ToCharArray();
                string[] split = null;
                split = presplit.Split(delimarray);
                foreach (string s in split)
                {
                    if (re.IsMatch(s.Trim()))
                    {
                        if (postsplit.Length > 0)
                            postsplit = postsplit + "; " + s.Trim();
                        else
                            postsplit = s.Trim();
                    }
                }
                if (postsplit.Length > 0)
                {
                    SMTPSettings.Instance.ToEmail = postsplit;
                }
                else
                {
                    SMTPSettings.Instance.ToEmail = "";
                }
            }

            // CC Email Address (can contain multiple addresses seperated by comma or semicolon
            if (textSMTPCCEmail.Text.ToString().Length == 0)
                SMTPSettings.Instance.CCEmail = "";
            if (textSMTPCCEmail.Text.ToString().Length > 0)
            {
                string presplit = textSMTPCCEmail.Text.ToString();
                string postsplit = "";
                string delim = ",;<>[] ";
                char[] delimarray = delim.ToCharArray();
                string[] split = null;
                split = presplit.Split(delimarray);
                foreach (string s in split)
                {
                    if (re.IsMatch(s.Trim()))
                    {
                        if (postsplit.Length > 0)
                            postsplit = postsplit + "; " + s.Trim();
                        else
                            postsplit = s.Trim();
                    }
                }
                if (postsplit.Length > 0)
                {
                    SMTPSettings.Instance.CCEmail = postsplit;
                }
                else
                {
                    SMTPSettings.Instance.CCEmail = "";
                }
            }

            textSMTPToEmail.Text = SMTPSettings.Instance.ToEmail.ToString();
            textSMTPFromEmail.Text = SMTPSettings.Instance.FromEmail.ToString();
            textSMTPCCEmail.Text = SMTPSettings.Instance.CCEmail.ToString();

            // Check if the SMTP Server Address looks like a host name
            string hostPattern = @"^((\[[0-9]{1,3}\.[0-9]{1,3}" +
                @"\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex reh = new Regex(hostPattern);
            if (textSMTPAddress.Text.ToString().Length == 0)
                SMTPSettings.Instance.SmtpServer = "";
            if (textSMTPAddress.Text.ToString().Length > 0)
                if (reh.IsMatch(textSMTPAddress.Text.ToString()))
                    SMTPSettings.Instance.SmtpServer = this.textSMTPAddress.Text.ToString();
            textSMTPAddress.Text = SMTPSettings.Instance.SmtpServer.ToString();

        }

        private void btnTestEmail_Click(object sender, EventArgs e)
        {
            UpdateSMTPSettings();
            // check email parameters all filled out
            string errmsg = "";
            if (textSMTPAddress.Text.ToString().Length == 0)
            {
                if (textSMTPPort.Text.ToString().Length == 0)
                    errmsg += "Enter a valid SMTP Server Address and Port.\r\n";
                else
                    errmsg += "Enter a valid SMTP Server Address.\r\n";
            }
            else if (textSMTPPort.Text.ToString().Length == 0)
            {
                errmsg += "Enter a valid SMTP Server Port.\r\n";
            }
            if (textSMTPFromEmail.Text.ToString().Length == 0)
            {
                if (textSMTPToEmail.Text.ToString().Length == 0)
                    errmsg += "Valid From and To Email Addresses are required.\r\n";
                else
                    errmsg += "Enter a valid From Email Address.\r\n";
            }
            else if (textSMTPToEmail.Text.ToString().Length == 0)
            {
                errmsg += "Enter a valid To Email Address.\r\n";
            }
            if (errmsg != string.Empty)
            {
                errmsg += "\r\nSending Test Email Aborted.";
                MessageBox.Show(errmsg, "Some Email Settings Missing.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {

                // Set up the message
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(SMTPSettings.Instance.FromEmail);

                // To Email Address could contain multiple addresses
                string presplit = SMTPSettings.Instance.ToEmail.ToString();
                string delim = ",;";
                char[] delimarray = delim.ToCharArray();
                string[] split = null;
                split = presplit.Split(delimarray);
                foreach (string s in split)
                {
                    if (s.Trim().Length > 0)
                        mailMessage.To.Add(new MailAddress(s.Trim()));
                }

                // CC email addresses
                split = null;
                presplit = SMTPSettings.Instance.CCEmail.ToString();
                split = presplit.Split(delimarray);
                foreach (string s in split)
                {
                    if (s.Trim().Length > 0)
                        mailMessage.CC.Add(new MailAddress(s.Trim()));
                }

                mailMessage.Subject = "MySEQ Spawn Alert";
                mailMessage.Body = "This is a test MySEQ Email Alert.";

                SmtpClient smtpClient = new SmtpClient(SMTPSettings.Instance.SmtpServer, SMTPSettings.Instance.SmtpPort);
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                if (SMTPSettings.Instance.UseNetworkCredentials)
                {
                    smtpClient.UseDefaultCredentials = true;
                }
                else
                {
                    smtpClient.UseDefaultCredentials = false;
                    if (SMTPSettings.Instance.SmtpDomain.ToString() != string.Empty)
                        smtpClient.Credentials = new System.Net.NetworkCredential(SMTPSettings.Instance.SmtpUsername, SMTPSettings.Instance.SmtpPassword.ToString(), SMTPSettings.Instance.SmtpDomain.ToString());
                    else
                        smtpClient.Credentials = new System.Net.NetworkCredential(SMTPSettings.Instance.SmtpUsername.ToString(), SMTPSettings.Instance.SmtpPassword.ToString());

                }
                // using SSL to authenticate?
                if (SMTPSettings.Instance.UseSSL)
                    smtpClient.EnableSsl = true;
                else
                    smtpClient.EnableSsl = false;
                smtpClient.SendCompleted += new SendCompletedEventHandler(SmtpClient_OnCompleted);



                object UserState = mailMessage;
                // Send away ....
                try { smtpClient.SendAsync(mailMessage, UserState); }

                catch (Exception Ex) { MessageBox.Show("Send Mail Error: " + Ex.Message, "Mail Message Failed to Send.",MessageBoxButtons.OK,MessageBoxIcon.Error); }
            }
        }
        private void SmtpClient_OnCompleted(object sender, AsyncCompletedEventArgs e)
        {
            //Get the Original MailMessage object
            MailMessage mail = (MailMessage)e.UserState;

            //write out the subject
            string subject = mail.Subject;

            if (e.Cancelled)
            {
                MessageBox.Show("Send canceled for [" + subject + "].", "Sending Mail Canceled", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (e.Error != null)
            {
                MessageBox.Show("Error sending [" + subject +"\r\n" + e.Error.ToString(), "Error Sending Mail",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Message [" + subject + "] sent successfully.", "SMTP Configured Properly", MessageBoxButtons.OK);
            }
        }

        private void checkBoxSavePassword_Click(object sender, EventArgs e)
        {
            SMTPSettings.Instance.SavePassword = !SMTPSettings.Instance.SavePassword;

            checkBoxSavePassword.Checked = SMTPSettings.Instance.SavePassword;
        }

        private void checkBoxSMTPUseSecureAuthentication_Click(object sender, EventArgs e)
        {
            SMTPSettings.Instance.UseSSL = !SMTPSettings.Instance.UseSSL;

            if (SMTPSettings.Instance.UseSSL)
            {
                SMTPSettings.Instance.SmtpPort = 587;
                textSMTPPort.Text = SMTPSettings.Instance.SmtpPort.ToString();
            }
            else
            {
                SMTPSettings.Instance.SmtpPort = 25;
                textSMTPPort.Text = SMTPSettings.Instance.SmtpPort.ToString();
            }

            checkBoxSMTPUseSecureAuthentication.Checked = SMTPSettings.Instance.UseSSL;
            
        }


	}

}

