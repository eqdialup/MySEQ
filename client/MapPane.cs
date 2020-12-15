using System;

using System.Data;

using System.Drawing;

using System.Collections;

using System.Windows.Forms;

using System.ComponentModel;

using WeifenLuo.WinFormsUI.Docking;

using System.Text.RegularExpressions;


namespace myseq {

    public class MapPane : DockContent {

        public myseq.MapCon mapCon;

        public System.Windows.Forms.NumericUpDown offsetx;

        public System.Windows.Forms.NumericUpDown offsety;

        public System.Windows.Forms.NumericUpDown scale;

        public System.Windows.Forms.NumericUpDown filterzneg;

        public System.Windows.Forms.NumericUpDown filterzpos;

        public System.Windows.Forms.Button cmdCommand;

        public  System.Windows.Forms.TextBox txtLookup;

        private System.Windows.Forms.Button cmdLookup;      

        private System.Windows.Forms.Label lblScale;

        private System.Windows.Forms.Label lblOffsetY;

        private System.Windows.Forms.Label lblOffsetX;

        private System.Windows.Forms.Label lblZNeg;

        private System.Windows.Forms.Label lblZPos;

        private System.Windows.Forms.Label lblLookup;

        private System.ComponentModel.Container components = null;

        private frmMain f1 = null; // Caution: may be null

        private EQData eq = null;

        public MapPane() {

            InitializeComponent();

            

            // Create the quick lookup text field and buttn

            

            offsetx.Minimum = -100000;

            offsetx.Maximum = 100000;

            offsety.Minimum = -100000;

            offsety.Maximum = 100000;

            

            scale.Maximum = 10000;

            scale.Value = 100;

            scale.Minimum = 10;



            scale.Increment = 10;

            offsetx.Increment = 20;

            offsety.Increment = 20;



            filterzneg.Increment = 5;

            filterzpos.Increment = 5;

            filterzpos.Minimum = 0;

            filterzneg.Minimum = 0;

            filterzpos.Maximum = 3500;

            filterzneg.Maximum = 3500;

            filterzneg.Value = 75;

            filterzpos.Value = 75;

        }



        public void SetComponents(frmMain f1,EQData eq)

        {

            this.f1 = f1;

            this.eq = eq;

        }



        protected override void Dispose(bool disposing)
        
        {

            if (disposing)  

                if (components != null)  

                    components.Dispose();

            base.Dispose(disposing);

        }

        protected override string GetPersistString()
        
        {

            return "MapWindow";

        }

        private void cmdLookup_Click(object sender, System.EventArgs e)
        
        {

                txtLookup.Text = "";

                txtLookup.Focus();

                Lookup("");

        }



        private void txtLookup_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        
        {

            if (e.KeyChar == (char)Keys.Enter) {

                Lookup(txtLookup.Text);

                txtLookup.Focus();                

                e.Handled = true;

            }

        }

        

        private void Lookup(String name)

        {   

            eq.MarkLookups(name);

            mapCon.Invalidate();

        }

        #region Component Designer generated code

        private void InitializeComponent() {
            this.offsetx = new System.Windows.Forms.NumericUpDown();
            this.offsety = new System.Windows.Forms.NumericUpDown();
            this.scale = new System.Windows.Forms.NumericUpDown();
            this.lblScale = new System.Windows.Forms.Label();
            this.lblOffsetY = new System.Windows.Forms.Label();
            this.lblOffsetX = new System.Windows.Forms.Label();
            this.cmdCommand = new System.Windows.Forms.Button();
            this.filterzneg = new System.Windows.Forms.NumericUpDown();
            this.lblZNeg = new System.Windows.Forms.Label();
            this.filterzpos = new System.Windows.Forms.NumericUpDown();
            this.lblZPos = new System.Windows.Forms.Label();
            this.cmdLookup = new System.Windows.Forms.Button();
            this.txtLookup = new System.Windows.Forms.TextBox();
            this.lblLookup = new System.Windows.Forms.Label();
            this.mapCon = new myseq.MapCon();
            ((System.ComponentModel.ISupportInitialize)(this.offsetx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.offsety)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.scale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filterzneg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filterzpos)).BeginInit();
            this.SuspendLayout();
            // 
            // offsetx
            // 
            this.offsetx.Location = new System.Drawing.Point(114, 448);
            this.offsetx.Name = "offsetx";
            this.offsetx.Size = new System.Drawing.Size(58, 20);
            this.offsetx.TabIndex = 3;
            this.offsetx.Visible = false;
            this.offsetx.ValueChanged += new System.EventHandler(this.offsetx_ValueChanged);
            // 
            // offsety
            // 
            this.offsety.Location = new System.Drawing.Point(217, 448);
            this.offsety.Name = "offsety";
            this.offsety.Size = new System.Drawing.Size(56, 20);
            this.offsety.TabIndex = 5;
            this.offsety.Visible = false;
            this.offsety.ValueChanged += new System.EventHandler(this.offsety_ValueChanged);
            // 
            // scale
            // 
            this.scale.Location = new System.Drawing.Point(313, 447);
            this.scale.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.scale.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.scale.Name = "scale";
            this.scale.Size = new System.Drawing.Size(48, 20);
            this.scale.TabIndex = 7;
            this.scale.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.scale.Visible = false;
            this.scale.ValueChanged += new System.EventHandler(this.scale_ValueChanged);
            // 
            // lblScale
            // 
            this.lblScale.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblScale.Location = new System.Drawing.Point(279, 450);
            this.lblScale.Name = "lblScale";
            this.lblScale.Size = new System.Drawing.Size(40, 16);
            this.lblScale.TabIndex = 6;
            this.lblScale.Text = "Scale";
            this.lblScale.Visible = false;
            // 
            // lblOffsetY
            // 
            this.lblOffsetY.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblOffsetY.Location = new System.Drawing.Point(174, 451);
            this.lblOffsetY.Name = "lblOffsetY";
            this.lblOffsetY.Size = new System.Drawing.Size(48, 16);
            this.lblOffsetY.TabIndex = 4;
            this.lblOffsetY.Text = "Offset Y";
            this.lblOffsetY.Visible = false;
            // 
            // lblOffsetX
            // 
            this.lblOffsetX.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblOffsetX.Location = new System.Drawing.Point(69, 452);
            this.lblOffsetX.Name = "lblOffsetX";
            this.lblOffsetX.Size = new System.Drawing.Size(48, 16);
            this.lblOffsetX.TabIndex = 2;
            this.lblOffsetX.Text = "Offset X";
            this.lblOffsetX.Visible = false;
            // 
            // cmdCommand
            // 
            this.cmdCommand.Location = new System.Drawing.Point(8, 448);
            this.cmdCommand.Name = "cmdCommand";
            this.cmdCommand.Size = new System.Drawing.Size(55, 23);
            this.cmdCommand.TabIndex = 1;
            this.cmdCommand.Text = "GO";
            this.cmdCommand.Visible = false;
            this.cmdCommand.Click += new System.EventHandler(this.cmdCommand_Click);
            // 
            // filterzneg
            // 
            this.filterzneg.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.filterzneg.Location = new System.Drawing.Point(413, 447);
            this.filterzneg.Maximum = new decimal(new int[] {
            3500,
            0,
            0,
            0});
            this.filterzneg.Name = "filterzneg";
            this.filterzneg.Size = new System.Drawing.Size(56, 20);
            this.filterzneg.TabIndex = 9;
            this.filterzneg.Visible = false;
            this.filterzneg.ValueChanged += new System.EventHandler(this.filterzneg_ValueChanged);
            // 
            // lblZNeg
            // 
            this.lblZNeg.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblZNeg.Location = new System.Drawing.Point(367, 450);
            this.lblZNeg.Name = "lblZNeg";
            this.lblZNeg.Size = new System.Drawing.Size(40, 16);
            this.lblZNeg.TabIndex = 8;
            this.lblZNeg.Text = "Z Neg";
            this.lblZNeg.Visible = false;
            // 
            // filterzpos
            // 
            this.filterzpos.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.filterzpos.Location = new System.Drawing.Point(510, 448);
            this.filterzpos.Maximum = new decimal(new int[] {
            3500,
            0,
            0,
            0});
            this.filterzpos.Name = "filterzpos";
            this.filterzpos.Size = new System.Drawing.Size(56, 20);
            this.filterzpos.TabIndex = 11;
            this.filterzpos.Visible = false;
            this.filterzpos.ValueChanged += new System.EventHandler(this.filterzpos_ValueChanged);
            // 
            // lblZPos
            // 
            this.lblZPos.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblZPos.Location = new System.Drawing.Point(475, 450);
            this.lblZPos.Name = "lblZPos";
            this.lblZPos.Size = new System.Drawing.Size(40, 16);
            this.lblZPos.TabIndex = 10;
            this.lblZPos.Text = "Z Pos";
            this.lblZPos.Visible = false;
            // 
            // cmdLookup
            // 
            this.cmdLookup.Location = new System.Drawing.Point(687, 447);
            this.cmdLookup.Name = "cmdLookup";
            this.cmdLookup.Size = new System.Drawing.Size(48, 22);
            this.cmdLookup.TabIndex = 4;
            this.cmdLookup.Text = "Reset";
            this.cmdLookup.Visible = false;
            this.cmdLookup.Click += new System.EventHandler(this.cmdLookup_Click);
            // 
            // txtLookup
            // 
            this.txtLookup.Location = new System.Drawing.Point(615, 448);
            this.txtLookup.Name = "txtLookup";
            this.txtLookup.Size = new System.Drawing.Size(56, 20);
            this.txtLookup.TabIndex = 5;
            this.txtLookup.Visible = false;
            this.txtLookup.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtLookup_KeyPress);
            // 
            // lblLookup
            // 
            this.lblLookup.Location = new System.Drawing.Point(572, 450);
            this.lblLookup.Name = "lblLookup";
            this.lblLookup.Size = new System.Drawing.Size(47, 23);
            this.lblLookup.TabIndex = 12;
            this.lblLookup.Text = "Lookup";
            this.lblLookup.Visible = false;
            // 
            // mapCon
            // 
            this.mapCon.AutoScroll = true;
            this.mapCon.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.mapCon.Location = new System.Drawing.Point(0, 0);
            this.mapCon.Name = "mapCon";
            this.mapCon.Size = new System.Drawing.Size(384, 264);
            this.mapCon.TabIndex = 0;
            this.mapCon.MouseEnter += new System.EventHandler(this.mapCon_MouseEnter);
            // 
            // MapPane
            // 
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.cmdLookup);
            this.Controls.Add(this.cmdCommand);
            this.Controls.Add(this.offsetx);
            this.Controls.Add(this.offsety);
            this.Controls.Add(this.scale);
            this.Controls.Add(this.filterzneg);
            this.Controls.Add(this.filterzpos);
            this.Controls.Add(this.txtLookup);
            this.Controls.Add(this.lblLookup);
            this.Controls.Add(this.lblZPos);
            this.Controls.Add(this.lblZNeg);
            this.Controls.Add(this.lblScale);
            this.Controls.Add(this.lblOffsetY);
            this.Controls.Add(this.lblOffsetX);
            this.Controls.Add(this.mapCon);
            this.Name = "MapPane";
            this.Size = new System.Drawing.Size(776, 480);
            this.Resize += new System.EventHandler(this.MapPane_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.offsetx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.offsety)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.scale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filterzneg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filterzpos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion



        private void MapPane_Resize(object sender, EventArgs e) {

            Size s = mapCon.Size;

            Size t = Size;



            s.Width = t.Width;

            // old toolbar adjust s.Height = t.Height - 30; // Allow for controls
            s.Height = t.Height;

            mapCon.Size = s;



            int top = Size.Height - 25; // Top of controls



            cmdCommand.Top = top;



            offsetx.Top = top;

            lblOffsetX.Top = top;



            offsety.Top = top;

            lblOffsetY.Top = top;



            scale.Top = top;

            lblScale.Top = top;



            filterzneg.Top = top;

            lblZNeg.Top = top;



            filterzpos.Top = top;

            lblZPos.Top = top;



            lblLookup.Top = top;

            txtLookup.Top = top;

            cmdLookup.Top = top;



            mapCon.onResize();

            mapCon.Invalidate();

        }



        private void offsetx_ValueChanged(object sender, System.EventArgs e) {

            mapCon.m_panOffsetX = -(int)offsetx.Value;

            mapCon.ReAdjust();

            mapCon.Invalidate();

        }



        private void offsety_ValueChanged(object sender, System.EventArgs e) {

            mapCon.m_panOffsetY = -(int)offsety.Value;

            mapCon.ReAdjust();

            mapCon.Invalidate();

        }



        private void scale_ValueChanged(object sender, System.EventArgs e) {

            mapCon.scale = (float)(scale.Value)/100.0f;

            f1.toolStripScale.Text = String.Format("{0:0.0%}", scale.Value/100);

            mapCon.ReAdjust();

            mapCon.Invalidate();

        }



        private void cmdCommand_Click(object sender, System.EventArgs e) {

            if (f1 == null) return;



            f1.cmdCommand_Click(sender, e);

        }



        private void filterzpos_ValueChanged(object sender, System.EventArgs e) {

            mapCon.filterpos = (int)filterzpos.Value;
            if (f1 != null)
                f1.toolStripZPos.Text = String.Format("{0:0.0}", filterzpos.Value);

        }



        private void filterzneg_ValueChanged(object sender, System.EventArgs e) {

            mapCon.filterneg = (int)filterzneg.Value;

            if (f1 != null)
                f1.toolStripZNeg.Text = String.Format("{0:0.0}", filterzneg.Value);

        }



        private void mapCon_MouseEnter(object sender, System.EventArgs e) {

            // focus for docking panel changes, to autohide panels that may be visible
            if ((f1.SpawnList.DockState == WeifenLuo.WinFormsUI.Docking.DockState.DockLeftAutoHide ||
                 f1.SpawnList.DockState == WeifenLuo.WinFormsUI.Docking.DockState.DockRightAutoHide ||
                 f1.SpawnList.DockState == WeifenLuo.WinFormsUI.Docking.DockState.DockTopAutoHide ||
                 f1.SpawnList.DockState == WeifenLuo.WinFormsUI.Docking.DockState.DockBottomAutoHide) && f1.SpawnList.ContainsFocus)
                mapCon.Focus();
            if ((f1.SpawnTimerList.DockState == WeifenLuo.WinFormsUI.Docking.DockState.DockLeftAutoHide ||
                 f1.SpawnTimerList.DockState == WeifenLuo.WinFormsUI.Docking.DockState.DockRightAutoHide ||
                 f1.SpawnTimerList.DockState == WeifenLuo.WinFormsUI.Docking.DockState.DockTopAutoHide ||
                 f1.SpawnTimerList.DockState == WeifenLuo.WinFormsUI.Docking.DockState.DockBottomAutoHide) && f1.SpawnTimerList.ContainsFocus)
                mapCon.Focus();
            if ((f1.GroundItemList.DockState == WeifenLuo.WinFormsUI.Docking.DockState.DockLeftAutoHide ||
                 f1.GroundItemList.DockState == WeifenLuo.WinFormsUI.Docking.DockState.DockRightAutoHide ||
                 f1.GroundItemList.DockState == WeifenLuo.WinFormsUI.Docking.DockState.DockTopAutoHide ||
                 f1.GroundItemList.DockState == WeifenLuo.WinFormsUI.Docking.DockState.DockBottomAutoHide) && f1.GroundItemList.ContainsFocus)
                mapCon.Focus();
        }

    }

}

