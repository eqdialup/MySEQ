using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using myseq.Properties;

namespace myseq
{
    public partial class FrmAddMapText : Form
    {
        public FrmAddMapText()
        {
            InitializeComponent();
            txtColr = Settings.Default.SelectedAddMapText;
            txtBkg = Settings.Default.BackColor;
        }

        private void BtnAddTxtOk_Click(object sender, EventArgs e)
        {
            txtColr = Settings.Default.SelectedAddMapText;
            Close();
        }

        private void BtnAddTxtCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() ==DialogResult.OK)
            {
                selectedColor = colorDialog1.Color;
                Settings.Default.SelectedAddMapText = colorDialog1.Color;
                pictureBox1.BackColor = colorDialog1.Color;
            }
        }
        public void SOEMapTextAdd(string new_text, MainForm f1)
        {
            IFormatProvider NumFormat = new CultureInfo("en-US");
            var soe_maptext = string.Format(NumFormat, "P {0:f3}, {1:f3}, {2}, {3}, {4}, {5}, {6}, {7}", f1.alertX * -1, f1.alertY * -1, f1.alertZ, txtColr.R, txtColr.G, txtColr.B, txtSize, new_text);

            MapText work = new MapText(soe_maptext);

            work.draw_color = work.color;
            work.draw_pen = new Pen(work.color);
            f1.map.AddMapText(work);

            if (DialogResult.Yes == MessageBox.Show($"Do you want to write the label to {mapName}?" +
                Environment.NewLine + Environment.NewLine + soe_maptext, "Write label to map",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
            {
                try
                {
                    File.AppendAllText(f1.mapnameWithLabels, soe_maptext);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Access Violation {ex}", "Add text to Map Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                f1.map.DeleteMapText(work);
            }
        }
    }
}