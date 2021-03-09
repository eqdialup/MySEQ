using System;

using System.Drawing;

using System.Windows.Forms;

using WeifenLuo.WinFormsUI.Docking;

namespace myseq
{
    public class MapPane : DockContent {
        public MapCon mapCon;

        public NumericUpDown offsetx;

        public NumericUpDown offsety;

        public NumericUpDown scale;

        public NumericUpDown filterzneg;

        public NumericUpDown filterzpos;

        public Button cmdCommand;

        public TextBox txtLookup;

        private Button cmdLookup;

        private Label lblScale;

        private Label lblOffsetY;

        private Label lblOffsetX;

        private Label lblZNeg;

        private Label lblZPos;

        private Label lblLookup;

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
                components?.Dispose();

            base.Dispose(disposing);
        }

        protected override string GetPersistString()

        {
            return "MapWindow";
        }

        private void cmdLookup_Click(object sender, EventArgs e)

        {
                txtLookup.Text = "";

                txtLookup.Focus();

                Lookup("");
        }

        private void txtLookup_KeyPress(object sender, KeyPressEventArgs e)

        {
            if (e.KeyChar == (char)Keys.Enter) {
                Lookup(txtLookup.Text);

                txtLookup.Focus();

                e.Handled = true;
            }
        }

        private void Lookup(string name)

        {
            eq.MarkLookups(name);

            mapCon.Invalidate();
        }

        #region Component Designer generated code

        private void InitializeComponent() {
            offsetx = new NumericUpDown();
            offsety = new NumericUpDown();
            scale = new NumericUpDown();
            lblScale = new Label();
            lblOffsetY = new Label();
            lblOffsetX = new Label();
            cmdCommand = new Button();
            filterzneg = new NumericUpDown();
            lblZNeg = new Label();
            filterzpos = new NumericUpDown();
            lblZPos = new Label();
            cmdLookup = new Button();
            txtLookup = new TextBox();
            lblLookup = new Label();
            mapCon = new MapCon();
            ((System.ComponentModel.ISupportInitialize)offsetx).BeginInit();
            ((System.ComponentModel.ISupportInitialize)offsety).BeginInit();
            ((System.ComponentModel.ISupportInitialize)scale).BeginInit();
            ((System.ComponentModel.ISupportInitialize)filterzneg).BeginInit();
            ((System.ComponentModel.ISupportInitialize)filterzpos).BeginInit();
            SuspendLayout();
            // 
            // offsetx
            // 
            offsetx.Location = new Point(114, 448);
            offsetx.Name = "offsetx";
            offsetx.Size = new Size(58, 20);
            offsetx.TabIndex = 3;
            offsetx.Visible = false;
            offsetx.ValueChanged += new EventHandler(offsetx_ValueChanged);
            // 
            // offsety
            // 
            offsety.Location = new Point(217, 448);
            offsety.Name = "offsety";
            offsety.Size = new Size(56, 20);
            offsety.TabIndex = 5;
            offsety.Visible = false;
            offsety.ValueChanged += new EventHandler(offsety_ValueChanged);
            // 
            // scale
            // 
            scale.Location = new Point(313, 447);
            scale.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            scale.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            scale.Name = "scale";
            scale.Size = new Size(48, 20);
            scale.TabIndex = 7;
            scale.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            scale.Visible = false;
            scale.ValueChanged += new EventHandler(scale_ValueChanged);
            // 
            // lblScale
            // 
            lblScale.ForeColor = SystemColors.WindowText;
            lblScale.Location = new Point(279, 450);
            lblScale.Name = "lblScale";
            lblScale.Size = new Size(40, 16);
            lblScale.TabIndex = 6;
            lblScale.Text = "Scale";
            lblScale.Visible = false;
            // 
            // lblOffsetY
            // 
            lblOffsetY.ForeColor = SystemColors.WindowText;
            lblOffsetY.Location = new Point(174, 451);
            lblOffsetY.Name = "lblOffsetY";
            lblOffsetY.Size = new Size(48, 16);
            lblOffsetY.TabIndex = 4;
            lblOffsetY.Text = "Offset Y";
            lblOffsetY.Visible = false;
            // 
            // lblOffsetX
            // 
            lblOffsetX.ForeColor = SystemColors.WindowText;
            lblOffsetX.Location = new Point(69, 452);
            lblOffsetX.Name = "lblOffsetX";
            lblOffsetX.Size = new Size(48, 16);
            lblOffsetX.TabIndex = 2;
            lblOffsetX.Text = "Offset X";
            lblOffsetX.Visible = false;
            // 
            // cmdCommand
            // 
            cmdCommand.Location = new Point(8, 448);
            cmdCommand.Name = "cmdCommand";
            cmdCommand.Size = new Size(55, 23);
            cmdCommand.TabIndex = 1;
            cmdCommand.Text = "GO";
            cmdCommand.Visible = false;
            cmdCommand.Click += new EventHandler(cmdCommand_Click);
            // 
            // filterzneg
            // 
            filterzneg.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            filterzneg.Location = new Point(413, 447);
            filterzneg.Maximum = new decimal(new int[] {
            3500,
            0,
            0,
            0});
            filterzneg.Name = "filterzneg";
            filterzneg.Size = new Size(56, 20);
            filterzneg.TabIndex = 9;
            filterzneg.Visible = false;
            filterzneg.ValueChanged += new EventHandler(filterzneg_ValueChanged);
            // 
            // lblZNeg
            // 
            lblZNeg.ForeColor = SystemColors.WindowText;
            lblZNeg.Location = new Point(367, 450);
            lblZNeg.Name = "lblZNeg";
            lblZNeg.Size = new Size(40, 16);
            lblZNeg.TabIndex = 8;
            lblZNeg.Text = "Z Neg";
            lblZNeg.Visible = false;
            // 
            // filterzpos
            // 
            filterzpos.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            filterzpos.Location = new Point(510, 448);
            filterzpos.Maximum = new decimal(new int[] {
            3500,
            0,
            0,
            0});
            filterzpos.Name = "filterzpos";
            filterzpos.Size = new Size(56, 20);
            filterzpos.TabIndex = 11;
            filterzpos.Visible = false;
            filterzpos.ValueChanged += new EventHandler(filterzpos_ValueChanged);
            // 
            // lblZPos
            // 
            lblZPos.ForeColor = SystemColors.WindowText;
            lblZPos.Location = new Point(475, 450);
            lblZPos.Name = "lblZPos";
            lblZPos.Size = new Size(40, 16);
            lblZPos.TabIndex = 10;
            lblZPos.Text = "Z Pos";
            lblZPos.Visible = false;
            // 
            // cmdLookup
            // 
            cmdLookup.Location = new Point(687, 447);
            cmdLookup.Name = "cmdLookup";
            cmdLookup.Size = new Size(48, 22);
            cmdLookup.TabIndex = 4;
            cmdLookup.Text = "Reset";
            cmdLookup.Visible = false;
            cmdLookup.Click += new EventHandler(cmdLookup_Click);
            // 
            // txtLookup
            // 
            txtLookup.Location = new Point(615, 448);
            txtLookup.Name = "txtLookup";
            txtLookup.Size = new Size(56, 20);
            txtLookup.TabIndex = 5;
            txtLookup.Visible = false;
            txtLookup.KeyPress += new KeyPressEventHandler(txtLookup_KeyPress);
            // 
            // lblLookup
            // 
            lblLookup.Location = new Point(572, 450);
            lblLookup.Name = "lblLookup";
            lblLookup.Size = new Size(47, 23);
            lblLookup.TabIndex = 12;
            lblLookup.Text = "Lookup";
            lblLookup.Visible = false;
            // 
            // mapCon
            // 
            mapCon.AutoScroll = true;
            mapCon.BackColor = SystemColors.ControlLightLight;
            mapCon.Location = new Point(0, 0);
            mapCon.Name = "mapCon";
            mapCon.Size = new Size(384, 264);
            mapCon.TabIndex = 0;
            mapCon.UpdateSteps = 5;
            mapCon.UpdateTicks = 1;
            mapCon.MouseEnter += new EventHandler(mapCon_MouseEnter);
            // 
            // MapPane
            // 
            AutoValidate = AutoValidate.EnablePreventFocusChange;
            BackColor = SystemColors.Control;
            ClientSize = new Size(760, 441);
            Controls.Add(cmdLookup);
            Controls.Add(cmdCommand);
            Controls.Add(offsetx);
            Controls.Add(offsety);
            Controls.Add(scale);
            Controls.Add(filterzneg);
            Controls.Add(filterzpos);
            Controls.Add(txtLookup);
            Controls.Add(lblLookup);
            Controls.Add(lblZPos);
            Controls.Add(lblZNeg);
            Controls.Add(lblScale);
            Controls.Add(lblOffsetY);
            Controls.Add(lblOffsetX);
            Controls.Add(mapCon);
            this.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Name = "MapPane";
            Size = new Size(776, 480);
            Resize += new EventHandler(MapPane_Resize);
            ((System.ComponentModel.ISupportInitialize)offsetx).EndInit();
            ((System.ComponentModel.ISupportInitialize)offsety).EndInit();
            ((System.ComponentModel.ISupportInitialize)scale).EndInit();
            ((System.ComponentModel.ISupportInitialize)filterzneg).EndInit();
            ((System.ComponentModel.ISupportInitialize)filterzpos).EndInit();
            ResumeLayout(false);
            PerformLayout();
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

        private void offsetx_ValueChanged(object sender, EventArgs e) {
            mapCon.m_panOffsetX = -(int)offsetx.Value;

            mapCon.ReAdjust();

            mapCon.Invalidate();
        }

        private void offsety_ValueChanged(object sender, EventArgs e) {
            mapCon.m_panOffsetY = -(int)offsety.Value;

            mapCon.ReAdjust();

            mapCon.Invalidate();
        }

        private void scale_ValueChanged(object sender, EventArgs e) {
            mapCon.scale = (float)scale.Value/100.0f;

            f1.toolStripScale.Text = $"{scale.Value / 100:0%}";

            mapCon.ReAdjust();

            mapCon.Invalidate();
        }

        private void cmdCommand_Click(object sender, EventArgs e) {
            if (f1 == null) return;

            f1.cmdCommand_Click(sender, e);
        }

        private void filterzpos_ValueChanged(object sender, EventArgs e) {
            mapCon.filterpos = (int)filterzpos.Value;
            if (f1 != null)
                f1.toolStripZPos.Text = $"{filterzpos.Value:0.0}";
        }

        private void filterzneg_ValueChanged(object sender, EventArgs e) {
            mapCon.filterneg = (int)filterzneg.Value;

            if (f1 != null)
                f1.toolStripZNeg.Text = $"{filterzneg.Value:0.0}";
        }

        private void mapCon_MouseEnter(object sender, EventArgs e) {
            // focus for docking panel changes, to autohide panels that may be visible
            if ((f1.SpawnList.DockState == DockState.DockLeftAutoHide ||
                 f1.SpawnList.DockState == DockState.DockRightAutoHide ||
                 f1.SpawnList.DockState == DockState.DockTopAutoHide ||
                 f1.SpawnList.DockState == DockState.DockBottomAutoHide) && f1.SpawnList.ContainsFocus)
            {
                mapCon.Focus();
            }

            if ((f1.SpawnTimerList.DockState == DockState.DockLeftAutoHide ||
                 f1.SpawnTimerList.DockState == DockState.DockRightAutoHide ||
                 f1.SpawnTimerList.DockState == DockState.DockTopAutoHide ||
                 f1.SpawnTimerList.DockState == DockState.DockBottomAutoHide) && f1.SpawnTimerList.ContainsFocus)
            {
                mapCon.Focus();
            }

            if ((f1.GroundItemList.DockState == DockState.DockLeftAutoHide ||
                 f1.GroundItemList.DockState == DockState.DockRightAutoHide ||
                 f1.GroundItemList.DockState == DockState.DockTopAutoHide ||
                 f1.GroundItemList.DockState == DockState.DockBottomAutoHide) && f1.GroundItemList.ContainsFocus)
            {
                mapCon.Focus();
            }
        }
    }
}
