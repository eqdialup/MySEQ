using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace myseq
{
    public class MapPane : DockContent
    {
        public MapCon mapCon;
        private MainForm f1;

        #region Designer components
        public NumericUpDown offsetx;

        public NumericUpDown offsety;
        public static NumericUpDown scale;
        public NumericUpDown filterzneg;

        public NumericUpDown filterzpos;

        private readonly System.ComponentModel.Container components;
        # endregion Designer components

        public MapPane()
        {
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

        public void SetComponents(MainForm f1)
        {
            this.f1 = f1;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
            }
            base.Dispose(disposing);
        }

        protected override string GetPersistString() => "MapWindow";

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.offsetx = new System.Windows.Forms.NumericUpDown();
            this.offsety = new System.Windows.Forms.NumericUpDown();
            scale = new System.Windows.Forms.NumericUpDown();
            this.filterzneg = new System.Windows.Forms.NumericUpDown();
            this.filterzpos = new System.Windows.Forms.NumericUpDown();
            this.mapCon = new myseq.MapCon();
            ((System.ComponentModel.ISupportInitialize)(this.offsetx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.offsety)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(scale)).BeginInit();
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
            this.offsetx.ValueChanged += new System.EventHandler(this.Offsetx_ValueChanged);
            //
            // offsety
            //
            this.offsety.Location = new System.Drawing.Point(217, 448);
            this.offsety.Name = "offsety";
            this.offsety.Size = new System.Drawing.Size(56, 20);
            this.offsety.TabIndex = 5;
            this.offsety.Visible = false;
            this.offsety.ValueChanged += new System.EventHandler(this.Offsety_ValueChanged);
            //
            // scale
            //
            scale.Location = new System.Drawing.Point(313, 447);
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
            scale.Size = new System.Drawing.Size(48, 20);
            scale.TabIndex = 7;
            scale.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            scale.Visible = false;
            scale.ValueChanged += new System.EventHandler(this.Scale_ValueChanged);

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
            this.filterzneg.ValueChanged += new System.EventHandler(this.Filterzneg_ValueChanged);
            ////
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
            this.filterzpos.ValueChanged += new System.EventHandler(this.Filterzpos_ValueChanged);
            //
            // mapCon
            //
            this.mapCon.AutoScroll = true;
            this.mapCon.BackColor = SystemColors.ControlLightLight;
            this.mapCon.Location = new System.Drawing.Point(0, 0);
            this.mapCon.Name = "mapCon";
            this.mapCon.PanOffsetX = 0F;
            this.mapCon.PanOffsetY = 0F;
            this.mapCon.Ratio = 1F;
            this.mapCon.Size = new System.Drawing.Size(361, 300);
            this.mapCon.TabIndex = 0;
            this.mapCon.UpdateSteps = 5;
            this.mapCon.UpdateTicks = 1;
            this.mapCon.MouseEnter += new System.EventHandler(this.MapCon_MouseEnter);
            //
            // MapPane
            //
            this.AutoValidate = AutoValidate.EnablePreventFocusChange;
            this.BackColor = SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(762, 488);
            this.Controls.Add(this.offsetx);
            this.Controls.Add(this.offsety);
            this.Controls.Add(scale);
            this.Controls.Add(this.filterzneg);
            this.Controls.Add(this.filterzpos);
            this.Controls.Add(this.mapCon);
            this.Name = "MapPane";
            this.Resize += new System.EventHandler(this.MapPane_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.offsetx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.offsety)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(scale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filterzneg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filterzpos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion Component Designer generated code

        public void MapReset()
        {
            scale.Value = 100M;
            mapCon.SetScale_1();
            offsetx.Value = 0;
            offsety.Value = 0;
            filterzneg.Value = 75;
            filterzpos.Value = 75;
            mapCon.ClearPan();
        }

        private void MapPane_Resize(object sender, EventArgs e)
        {
            Size s = mapCon.Size;
            Size t = Size;
            s.Width = t.Width;
            s.Height = t.Height;
            mapCon.Size = s;
            var top = Size.Height - 25; // Top of controls
            offsetx.Top = top;
            offsety.Top = top;
            scale.Top = top;
            filterzneg.Top = top;
            filterzpos.Top = top;

            mapCon.OnResize();
            mapCon.Invalidate();
        }

        public void ZoomIn()
        {
            AdjustZoom(isZoomingIn: true);
        }

        public void ZoomOut()
        {
            AdjustZoom(isZoomingIn: false);
        }

        // Consolidated logic for zooming in/out
        private void AdjustZoom(bool isZoomingIn)
        {
            // Get the zoom increment based on the current scale value
            int step = GetZoomIncrement((int)scale.Value, isZoomingIn);

            // Calculate new value, ensuring it remains within bounds
            var newValue = isZoomingIn
                ? Math.Min(scale.Value + step, scale.Maximum)
                : Math.Max(scale.Value - step, scale.Minimum);

            // Set new scale value
            if (newValue >= scale.Minimum && newValue <= scale.Maximum)
            {
                scale.Value = newValue;
            }
        }

        // Helper method to determine zoom increment/decrement
        private int GetZoomIncrement(int currentValue, bool isZoomingIn)
        {
            if (currentValue < 100) return 10;
            if (currentValue < 200) return 25;
            if (currentValue < 300) return 25;
            if (currentValue < 500) return 50;
            return 100;
        }

        private void Offsetx_ValueChanged(object sender, EventArgs e)
        {
            mapCon.Offset_X_ValueChanged(offsetx);
        }

        private void Offsety_ValueChanged(object sender, EventArgs e)
        {
            mapCon.Offset_Y_ValueChanged(offsety);
        }

        private void Scale_ValueChanged(object sender, EventArgs e)
        {
            f1.toolStripScale.Text = $"{scale.Value / 100:0%}";
            mapCon.scale = (float)scale.Value / 100.0f;

            mapCon.ReAdjust();
            mapCon.Invalidate();
        }

        private void Filterzpos_ValueChanged(object sender, EventArgs e)
        {
            mapCon.SetFilterPos((int)filterzpos.Value);
            if (f1 != null)
            {
                f1.toolStripZPos.Text = $"{filterzpos.Value:0.0}";
            }
        }

        private void Filterzneg_ValueChanged(object sender, EventArgs e)
        {
            mapCon.SetFilterNeg((int)filterzneg.Value);

            if (f1 != null)
            {
                f1.toolStripZNeg.Text = $"{filterzneg.Value:0.0}";
            }
        }

        private void MapCon_MouseEnter(object sender, EventArgs e)
        {
            f1.FocusMouse();
        }

        #region KeyPress

        public void MapCon_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Dictionary to map key presses to actions
            var keyInstructions = new Dictionary<char, Action>
    {
        {'1', () => MoveOffset(-50, 50)},
        {'2', () => MoveOffset(0, 50)},
        {'3', () => MoveOffset(50, 50)},
        {'4', () => MoveOffset(-50, 0)},
        {'5', ResetOffset},
        {'6', () => MoveOffset(50, 0)},
        {'7', () => MoveOffset(-50, -50)},
        {'8', () => MoveOffset(0, -50)},
        {'9', () => MoveOffset(50, -50)},
        {'+', IncreaseScale},
        {'-', DecreaseScale},
        {'c', ResetOffset}
    };

            char keyChar = char.ToLower(e.KeyChar);

            if (keyInstructions.TryGetValue(keyChar, out var action))
            {
                action();
                mapCon.ReAdjust();
            }
        }

        private void MoveOffset(int xChange, int yChange)
        {
            offsetx.Value += xChange;
            offsety.Value += yChange;
        }

        private void ResetOffset()
        {
            offsetx.Value = 0;
            offsety.Value = 0;
        }

        private const float ScaleIncrement = 0.2f;
        private const float MinScale = 0.1f;

        private void AdjustScale(float increment)
        {
            float newScale = mapCon.scale + increment;

            // Ensure the new scale doesn't go below the minimum threshold
            if (newScale >= MinScale)
            {
                mapCon.scale = newScale;
                scale.Value = (decimal)(mapCon.scale * 100);
                Invalidate();
            }
        }

        private void IncreaseScale()
        {
            AdjustScale(ScaleIncrement);
        }

        private void DecreaseScale()
        {
            AdjustScale(-ScaleIncrement);
        }

        #endregion KeyPress
    }
}