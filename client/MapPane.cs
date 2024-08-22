using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace myseq
{
    public class MapPane : DockContent
    {
        public MapCon mapCon;// {get; set; }
        private MainForm f1; // Caution: may be null

        #region Designer components
        public NumericUpDown offsetx;// {get; set; }

        public NumericUpDown offsety;// {get; set; }
        public static NumericUpDown scale;
        public NumericUpDown filterzneg;// {get; set; }

        public NumericUpDown filterzpos;// {get; set; }

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
            mapCon.scale = 1.0f;
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
            mapCon?.Invalidate();
        }

        public void ZoomIn()
        {
            var current_val = scale.Value;
            if (current_val < 100)
            {
                current_val += 10;
            }
            else if (current_val < 200)
            {
                current_val += 25;
            }
            else if (current_val < 300)
            {
                current_val += 25;
            }
            else if (current_val < 500)
            {
                current_val += 50;
            }
            else
            {
                current_val += 100;
            }

            scale.Value = current_val;
        }

        public void ZoomOut()
        {
            var current_val = (int)scale.Value;
            switch (current_val)
            {
                case int n when n <= 100:
                    current_val = 10;
                    break;

                case int n when n <= 200:
                    current_val = 100;
                    break;

                case int n when n <= 300:
                    current_val = 200;
                    break;

                case int n when n <= 400:
                    current_val = 300;
                    break;

                case int n when n <= 500:
                    current_val = 500;
                    break;

                default:
                    current_val -= 100;
                    break;
            }

            if (current_val >= scale.Minimum && current_val <= scale.Maximum)
            {
                scale.Value = current_val;
            }
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

        public bool MapPaneScale(decimal Num)
        {
            scale.Value = Num;
            return true;
        }

        #region KeyPress

        public void MapCon_KeyPress(object sender, KeyPressEventArgs e)
        {
            Dictionary<char, Action> KeyInstructions = new Dictionary<char, Action>()
                {
                    {'1', OneKey },
                    {'2', TwoKey },
                    {'3', ThreeKey},
                    {'4', FourKey},
                    {'5', Cor5Key},
                    {'c', Cor5Key},
                    {'6', SixKey},
                    {'7', SevenKey},
                    {'8', EightKey},
                    {'9', NineKey},
                    {'+', PlusKey},
                    {'-', MinusKey}
                };
            if (!KeyInstructions.ContainsKey(e.KeyChar)) return;
            KeyInstructions[char.ToLower(e.KeyChar)]();
            mapCon.ReAdjust();
        }

        private void OneKey()
        {
            offsety.Value += 50;
            offsetx.Value -= 50;
        }

        private void TwoKey()
        {
            offsety.Value += 50;
        }

        private void ThreeKey()
        {
            offsety.Value += 50;
            offsetx.Value += 50;
        }

        private void SixKey()
        {
            offsetx.Value += 50;
        }

        private void NineKey()
        {
            offsety.Value -= 50;

            offsetx.Value += 50;
        }

        private void EightKey()
        {
            offsety.Value -= 50;
        }

        private void SevenKey()
        {
            offsetx.Value -= 50;

            offsety.Value -= 50;
        }

        private void FourKey()
        {
            offsetx.Value -= 50;
        }

        private void Cor5Key()
        {
            offsetx.Value = 0;

            offsety.Value = 0;
        }

        private void MinusKey()
        {
            if (mapCon.scale - 0.2 >= 0.1)
            {
                mapCon.scale -= 0.2f;

                scale.Value = (decimal)(mapCon.scale * 100);
            }

            Invalidate();
        }

        private void PlusKey()
        {
            mapCon.scale += 0.2f;

            scale.Value = (decimal)(mapCon.scale * 100);

            Invalidate();
        }

        #endregion KeyPress
    }
}