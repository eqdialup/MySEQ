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
        public void AddMapText(string newText, MainForm f1)
        {
            IFormatProvider NumFormat = new CultureInfo("en-US");
            var soeMapText = string.Format(NumFormat, "P {0:f3}, {1:f3}, {2}, {3}, {4}, {5}, {6}, {7}", f1.alertX * -1, f1.alertY * -1, f1.alertZ, txtColr.R, txtColr.G, txtColr.B, txtSize, newText);

            var mapText = new MapText(soeMapText);

            mapText.draw_color = mapText.color;
            mapText.draw_pen = new Pen(mapText.color);
            f1.map.AddMapText(mapText);

           var result = MessageBox.Show($"Do you want to write the label to {f1.mapnameWithLabels}?" + Environment.NewLine + Environment.NewLine + soeMapText, "Write label to map", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

            if (result == DialogResult.Yes)
            {
                try
                {
                    File.AppendAllText(f1.mapnameWithLabels, soeMapText);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Access Violation {ex}", "Add text to Map Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                f1.map.DeleteMapText(mapText);
            }
        }
    }
}