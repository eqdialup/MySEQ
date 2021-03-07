using System;

using System.Windows.Forms;

namespace myseq
{
    public class AboutDlg : Form
    {
		private Label lblVersion;

		private Label lblInfo1;

		private Label lblInfo2;

        private Button cmdControl;

		private PictureBox picLogo;

        private Label label1;

        private LinkLabel linkLabel1;

        private LinkLabel linkLabel2;
//        private Label label2;
        private System.ComponentModel.Container components;

		public AboutDlg() {
			InitializeComponent();

			lblVersion.Text = Application.ProductVersion + " Lenny's Build";
		}

		protected override void Dispose(bool disposing) {
			if (disposing)
				components?.Dispose();

			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		private void InitializeComponent()

		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutDlg));
            this.lblInfo1 = new Label();
            this.lblVersion = new Label();
            this.lblInfo2 = new Label();
            this.cmdControl = new Button();
            this.picLogo = new PictureBox();
            this.label1 = new Label();
            this.linkLabel1 = new LinkLabel();
            this.linkLabel2 = new LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // lblInfo1
            // 
            this.lblInfo1.Location = new System.Drawing.Point(97, 24);
            this.lblInfo1.Name = "lblInfo1";
            this.lblInfo1.Size = new System.Drawing.Size(140, 16);
            this.lblInfo1.TabIndex = 0;
            this.lblInfo1.Text = "MySEQ Open (2021)";
            this.lblInfo1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblVersion
            // 
            this.lblVersion.Location = new System.Drawing.Point(62, 40);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(194, 16);
            this.lblVersion.TabIndex = 1;
            this.lblVersion.Text = "Version 2.XXX";
            this.lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblInfo2
            // 
            this.lblInfo2.Location = new System.Drawing.Point(38, 83);
            this.lblInfo2.Name = "lblInfo2";
            this.lblInfo2.Size = new System.Drawing.Size(233, 16);
            this.lblInfo2.TabIndex = 2;
            this.lblInfo2.Text = "Uses the DockPanel Suite, available at:";
            this.lblInfo2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmdControl
            // 
            this.cmdControl.Location = new System.Drawing.Point(120, 164);
            this.cmdControl.Name = "cmdControl";
            this.cmdControl.Size = new System.Drawing.Size(75, 23);
            this.cmdControl.TabIndex = 4;
            this.cmdControl.Text = "OK";
            this.cmdControl.Click += new EventHandler(this.CmdControl_Click);
            // 
            // picLogo
            // 
            this.picLogo.Image = ((System.Drawing.Image)(resources.GetObject("picLogo.Image")));
            this.picLogo.Location = new System.Drawing.Point(12, 12);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(64, 64);
            this.picLogo.SizeMode = PictureBoxSizeMode.StretchImage;
            this.picLogo.TabIndex = 6;
            this.picLogo.TabStop = false;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(71, 123);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(173, 16);
            this.label1.TabIndex = 7;
            this.label1.Text = "MySEQ Open Forums at";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(97, 139);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(120, 13);
            this.linkLabel1.TabIndex = 8;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "http://www.showeq.net";
            this.linkLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.linkLabel1.Click += new EventHandler(this.LinkLabel1_Click);
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.Location = new System.Drawing.Point(62, 99);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(187, 13);
            this.linkLabel2.TabIndex = 9;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "http://dockpanelsuite.sourceforge.net";
            this.linkLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.linkLabel2.LinkClicked += new LinkLabelLinkClickedEventHandler(this.LinkLabel2_LinkClicked);
            // 
            // AboutDlg
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(297, 202);
            this.Controls.Add(this.linkLabel2);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.picLogo);
            this.Controls.Add(this.cmdControl);
            this.Controls.Add(this.lblInfo2);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.lblInfo1);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutDlg";
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "About MySEQ Open";
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private void CmdControl_Click(object sender, EventArgs e) {
			Close();
		}

        private void LinkLabel1_Click(object sender, EventArgs e)

        {
            System.Diagnostics.Process.Start("http://www.showeq.net/forums/forum.php");
        }

        private void LinkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)

        {
            System.Diagnostics.Process.Start("http://dockpanelsuite.sourceforge.net");
        }
    }
}

