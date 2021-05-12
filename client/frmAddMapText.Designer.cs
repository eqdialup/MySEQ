namespace myseq
{
    partial class FrmAddMapText
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.radioBtnSmall = new System.Windows.Forms.RadioButton();
            this.radioBtnMedium = new System.Windows.Forms.RadioButton();
            this.radioBtnLarge = new System.Windows.Forms.RadioButton();
            this.FontSizeGroupBox = new System.Windows.Forms.GroupBox();
            this.MapTextToAdd = new System.Windows.Forms.TextBox();
            this.BtnAddTxtOk = new System.Windows.Forms.Button();
            this.BtnAddTxtCancel = new System.Windows.Forms.Button();
            this.lblMapTextToAdd = new System.Windows.Forms.Label();
            this.ColorGroupBox = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.FontSizeGroupBox.SuspendLayout();
            this.ColorGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // radioBtnSmall
            // 
            this.radioBtnSmall.AutoSize = true;
            this.radioBtnSmall.Location = new System.Drawing.Point(10, 16);
            this.radioBtnSmall.Name = "radioBtnSmall";
            this.radioBtnSmall.Size = new System.Drawing.Size(50, 17);
            this.radioBtnSmall.Text = "Small";
            this.radioBtnSmall.UseVisualStyleBackColor = true;
            // 
            // radioBtnMedium
            // 
            this.radioBtnMedium.AutoSize = true;
            this.radioBtnMedium.Checked = true;
            this.radioBtnMedium.Location = new System.Drawing.Point(10, 33);
            this.radioBtnMedium.Name = "radioBtnMedium";
            this.radioBtnMedium.Size = new System.Drawing.Size(62, 17);
            this.radioBtnMedium.Text = "Medium";
            this.radioBtnMedium.UseVisualStyleBackColor = true;
            // 
            // radioBtnLarge
            // 
            this.radioBtnLarge.AutoSize = true;
            this.radioBtnLarge.Location = new System.Drawing.Point(10, 50);
            this.radioBtnLarge.Name = "radioBtnLarge";
            this.radioBtnLarge.Size = new System.Drawing.Size(52, 17);
            this.radioBtnLarge.Text = "Large";
            this.radioBtnLarge.UseVisualStyleBackColor = true;
            // 
            // FontSizeGroupBox
            // 
            this.FontSizeGroupBox.Controls.Add(this.radioBtnLarge);
            this.FontSizeGroupBox.Controls.Add(this.radioBtnMedium);
            this.FontSizeGroupBox.Controls.Add(this.radioBtnSmall);
            this.FontSizeGroupBox.Location = new System.Drawing.Point(126, 0);
            this.FontSizeGroupBox.Name = "FontSizeGroupBox";
            this.FontSizeGroupBox.Size = new System.Drawing.Size(86, 77);
            this.FontSizeGroupBox.Text = "Font Size";
            // 
            // MapTextToAdd
            // 
            this.MapTextToAdd.BackColor = System.Drawing.Color.LightGray;
            this.MapTextToAdd.ForeColor = global::myseq.Properties.Settings.Default.SelectedAddMapText;
            this.MapTextToAdd.Location = new System.Drawing.Point(19, 21);
            this.MapTextToAdd.Name = "MapTextToAdd";
            this.MapTextToAdd.Size = new System.Drawing.Size(205, 20);
            // 
            // BtnAddTxtOk
            // 
            this.BtnAddTxtOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.BtnAddTxtOk.Location = new System.Drawing.Point(32, 154);
            this.BtnAddTxtOk.Name = "BtnAddTxtOk";
            this.BtnAddTxtOk.Size = new System.Drawing.Size(82, 22);
            this.BtnAddTxtOk.Text = "Ok";
            this.BtnAddTxtOk.UseVisualStyleBackColor = true;
            this.BtnAddTxtOk.Click += new System.EventHandler(this.BtnAddTxtOk_Click);
            // 
            // BtnAddTxtCancel
            // 
            this.BtnAddTxtCancel.Location = new System.Drawing.Point(143, 154);
            this.BtnAddTxtCancel.Name = "BtnAddTxtCancel";
            this.BtnAddTxtCancel.Size = new System.Drawing.Size(81, 22);
            this.BtnAddTxtCancel.Text = "Cancel";
            this.BtnAddTxtCancel.UseVisualStyleBackColor = true;
            this.BtnAddTxtCancel.Click += new System.EventHandler(this.BtnAddTxtCancel_Click);
            // 
            // lblMapTextToAdd
            // 
            this.lblMapTextToAdd.AutoSize = true;
            this.lblMapTextToAdd.Location = new System.Drawing.Point(16, 4);
            this.lblMapTextToAdd.Name = "lblMapTextToAdd";
            this.lblMapTextToAdd.Size = new System.Drawing.Size(81, 13);
            this.lblMapTextToAdd.Text = "Map Text Label";
            // 
            // ColorGroupBox
            // 
            this.ColorGroupBox.Controls.Add(this.label1);
            this.ColorGroupBox.Controls.Add(this.pictureBox1);
            this.ColorGroupBox.Controls.Add(this.button1);
            this.ColorGroupBox.Controls.Add(this.FontSizeGroupBox);
            this.ColorGroupBox.Location = new System.Drawing.Point(12, 47);
            this.ColorGroupBox.Name = "ColorGroupBox";
            this.ColorGroupBox.Size = new System.Drawing.Size(212, 101);
            this.ColorGroupBox.Text = "Color";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 13);
            this.label1.Text = "Chosen color";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = global::myseq.Properties.Settings.Default.SelectedAddMapText;
            this.pictureBox1.Location = new System.Drawing.Point(20, 33);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 15);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(20, 54);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(102, 23);
            this.button1.Text = "Pick Text Color";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // FrmAddMapText
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(235, 182);
            this.Controls.Add(this.ColorGroupBox);
            this.Controls.Add(this.lblMapTextToAdd);
            this.Controls.Add(this.BtnAddTxtCancel);
            this.Controls.Add(this.BtnAddTxtOk);
            this.Controls.Add(this.MapTextToAdd);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "AddMapText";
            this.Text = "";
            this.TopMost = true;
            this.FontSizeGroupBox.ResumeLayout(false);
            this.FontSizeGroupBox.PerformLayout();
            this.ColorGroupBox.ResumeLayout(false);
            this.ColorGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.RadioButton radioBtnSmall;
        private System.Windows.Forms.RadioButton radioBtnMedium;
        private System.Windows.Forms.RadioButton radioBtnLarge;
        private System.Windows.Forms.GroupBox FontSizeGroupBox;
        private System.Windows.Forms.TextBox MapTextToAdd;
        private System.Windows.Forms.Button BtnAddTxtOk;
        private System.Windows.Forms.Button BtnAddTxtCancel;
        private System.Windows.Forms.Label lblMapTextToAdd;
        private System.Windows.Forms.GroupBox ColorGroupBox;

        private System.Drawing.Color selectedColor;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;

        public string txtAdd { get { return MapTextToAdd.Text; } set { MapTextToAdd.Text = value; } }
        public string mapName { get { return this.Text; } set { this.Text = value; } }
        public int txtSize
        {
            get
            {
                if (radioBtnSmall.Checked)
                    return 1;
                if (radioBtnLarge.Checked)
                    return 3;
                return 2;
            }
        }

        public System.Drawing.Color txtColr { get { return selectedColor; } set { selectedColor = value; } }
        public System.Drawing.Color txtBkg { get { return MapTextToAdd.BackColor; } set { MapTextToAdd.BackColor = value; } }
    }
}