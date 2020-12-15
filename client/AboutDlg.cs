using System;

using System.Drawing;

using System.Collections;

using System.ComponentModel;

using System.Windows.Forms;



using Structures;



namespace myseq {

	public class AboutDlg : System.Windows.Forms.Form {

		private System.Windows.Forms.Label lblVersion;

		private System.Windows.Forms.Label lblInfo1;

		private System.Windows.Forms.Label lblInfo2;

        private System.Windows.Forms.Button cmdControl;

		private System.Windows.Forms.PictureBox picLogo;

        private Label label1;

        private LinkLabel linkLabel1;

        private LinkLabel linkLabel2;
        private Label label2;
        private System.ComponentModel.Container components = null;



		public AboutDlg() {

			InitializeComponent();



			lblVersion.Text = Application.ProductVersion.ToString() + " eqDialup's Personal Build";



		}



		protected override void Dispose(bool disposing) {

			if (disposing)

				if (components != null)

					components.Dispose();



			base.Dispose(disposing);

		}



		#region Windows Form Designer generated code

		private void InitializeComponent()

		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutDlg));
            this.lblInfo1 = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lblInfo2 = new System.Windows.Forms.Label();
            this.cmdControl = new System.Windows.Forms.Button();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // lblInfo1
            // 
            this.lblInfo1.Location = new System.Drawing.Point(97, 24);
            this.lblInfo1.Name = "lblInfo1";
            this.lblInfo1.Size = new System.Drawing.Size(140, 16);
            this.lblInfo1.TabIndex = 0;
            this.lblInfo1.Text = "MySEQ Open (2020)";
            this.lblInfo1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblInfo1.Click += new System.EventHandler(this.lblInfo1_Click);
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
            this.cmdControl.Click += new System.EventHandler(this.cmdControl_Click);
            // 
            // picLogo
            // 
            this.picLogo.Image = ((System.Drawing.Image)(resources.GetObject("picLogo.Image")));
            this.picLogo.Location = new System.Drawing.Point(12, 12);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(64, 64);
            this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
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
            this.linkLabel1.Click += new System.EventHandler(this.linkLabel1_Click);
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
            this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(97, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(127, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Based on EqMule 2.4.1.0";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // AboutDlg
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(363, 237);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.linkLabel2);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.picLogo);
            this.Controls.Add(this.cmdControl);
            this.Controls.Add(this.lblInfo2);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.lblInfo1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutDlg";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About MySEQ Open";
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion



		private void cmdControl_Click(object sender, System.EventArgs e) {

			Close();

		}



        private void linkLabel1_Click(object sender, EventArgs e)

        {

            System.Diagnostics.Process.Start("http://www.showeq.net/forums/forum.php");

        }



        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)

        {

            System.Diagnostics.Process.Start("http://dockpanelsuite.sourceforge.net");

        }



        private void lblInfo1_Click(object sender, EventArgs e)

        {



        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }

}

