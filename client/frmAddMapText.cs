using System;
using System.Windows.Forms;
using myseq.Properties;

namespace myseq
{
    public partial class FrmAddMapText : Form
    {
        public FrmAddMapText()
        {
            InitializeComponent();
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
            if (colorDialog1.ShowDialog() != DialogResult.Cancel && colorDialog1.Color != selectedColor)
            {
                selectedColor = colorDialog1.Color;
                Settings.Default.SelectedAddMapText = colorDialog1.Color;
                pictureBox1.BackColor = colorDialog1.Color;
            }
        }
    }
}
