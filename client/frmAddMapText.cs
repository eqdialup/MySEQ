using myseq.Properties;
using Structures;
using System;
using System.Windows.Forms;

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
            if (Settings.Default.SelectedAddMapText != txtColr)
            {
                Settings.Default.SelectedAddMapText = txtColr;
            }
            Close();
        }

        private void BtnAddTxtCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void PicMapTextColor1_Click(object sender, EventArgs e)
        {
            selectedColor = picMapTextColor1.BackColor;
            updateColorBoxes();
        }

        private void picMapTextColor2_Click(object sender, EventArgs e)
        {
            selectedColor = picMapTextColor2.BackColor;
            updateColorBoxes();
        }

        private void picMapTextColor3_Click(object sender, EventArgs e)
        {
            selectedColor = picMapTextColor3.BackColor;
            updateColorBoxes();
        }

        private void picMapTextColor4_Click(object sender, EventArgs e)
        {
            selectedColor = picMapTextColor4.BackColor;
            updateColorBoxes();
        }

        private void picMapTextColor5_Click(object sender, EventArgs e)
        {
            selectedColor = picMapTextColor5.BackColor;
            updateColorBoxes();
        }

        private void picMapTextColor6_Click(object sender, EventArgs e)
        {
            selectedColor = picMapTextColor6.BackColor;
            updateColorBoxes();
        }

        private void picMapTextColor7_Click(object sender, EventArgs e)
        {
            selectedColor = picMapTextColor7.BackColor;
            updateColorBoxes();
        }

        private void picMapTextColor8_Click(object sender, EventArgs e)
        {
            selectedColor = picMapTextColor8.BackColor;
            updateColorBoxes();
        }

        private void picMapTextColor9_Click(object sender, EventArgs e)
        {
            selectedColor = picMapTextColor9.BackColor;
            updateColorBoxes();
        }

        private void picMapTextColor10_Click(object sender, EventArgs e)
        {
            selectedColor = picMapTextColor10.BackColor;
            updateColorBoxes();
        }

        private void picMapTextColor11_Click(object sender, EventArgs e)
        {
            selectedColor = picMapTextColor11.BackColor;
            updateColorBoxes();
        }

        private void picMapTextColor12_Click(object sender, EventArgs e)
        {
            selectedColor = picMapTextColor12.BackColor;
            updateColorBoxes();
        }

        private void picMapTextColor13_Click(object sender, EventArgs e)
        {
            selectedColor = picMapTextColor13.BackColor;
            updateColorBoxes();
        }

        private void picMapTextColor14_Click(object sender, EventArgs e)
        {
            selectedColor = picMapTextColor14.BackColor;
            updateColorBoxes();
        }

        private void picMapTextColor15_Click(object sender, EventArgs e)
        {
            selectedColor = picMapTextColor15.BackColor;
            updateColorBoxes();
        }

        private void picMapTextColor16_Click(object sender, EventArgs e)
        {
            selectedColor = picMapTextColor16.BackColor;
            updateColorBoxes();
        }

        private void picMapTextColor17_Click(object sender, EventArgs e)
        {
            selectedColor = picMapTextColor17.BackColor;
            updateColorBoxes();
        }

        private void picMapTextColor18_Click(object sender, EventArgs e)
        {
            selectedColor = picMapTextColor18.BackColor;
            updateColorBoxes();
        }

        private void picMapTextColor19_Click(object sender, EventArgs e)
        {
            selectedColor = picMapTextColor19.BackColor;
            updateColorBoxes();
        }

        private void picMapTextColor20_Click(object sender, EventArgs e)
        {
            selectedColor = picMapTextColor20.BackColor;
            updateColorBoxes();
        }

        private void picMapTextColor21_Click(object sender, EventArgs e)
        {
            selectedColor = picMapTextColor21.BackColor;
            updateColorBoxes();
        }

        private void picMapTextColor22_Click(object sender, EventArgs e)
        {
            selectedColor = picMapTextColor22.BackColor;
            updateColorBoxes();
        }

        private void picMapTextColor23_Click(object sender, EventArgs e)
        {
            selectedColor = picMapTextColor23.BackColor;
            updateColorBoxes();
        }

        private void picMapTextColor24_Click(object sender, EventArgs e)
        {
            selectedColor = picMapTextColor24.BackColor;
            updateColorBoxes();
        }

        private void picMapTextColor25_Click(object sender, EventArgs e)
        {
            selectedColor = picMapTextColor25.BackColor;
            updateColorBoxes();
        }

        private void picMapTextColor26_Click(object sender, EventArgs e)
        {
            selectedColor = picMapTextColor26.BackColor;
            updateColorBoxes();
        }

        private void picMapTextColor27_Click(object sender, EventArgs e)
        {
            selectedColor = picMapTextColor27.BackColor;
            updateColorBoxes();
        }

        private void picMapTextColor28_Click(object sender, EventArgs e)
        {
            selectedColor = picMapTextColor28.BackColor;
            updateColorBoxes();
        }

        private void picMapTextColor29_Click(object sender, EventArgs e)
        {
            selectedColor = picMapTextColor29.BackColor;
            updateColorBoxes();
        }

        private void picMapTextColor30_Click(object sender, EventArgs e)
        {
            selectedColor = picMapTextColor30.BackColor;
            updateColorBoxes();
        }

        private void picMapTextColor31_Click(object sender, EventArgs e)
        {
            selectedColor = picMapTextColor31.BackColor;
            updateColorBoxes();
        }

        private void picMapTextColor32_Click(object sender, EventArgs e)
        {
            selectedColor = picMapTextColor32.BackColor;
            updateColorBoxes();
        }

        private void picMapTextColor33_Click(object sender, EventArgs e)
        {
            selectedColor = picMapTextColor33.BackColor;
            updateColorBoxes();
        }

        private void picMapTextColor34_Click(object sender, EventArgs e)
        {
            selectedColor = picMapTextColor34.BackColor;
            updateColorBoxes();
        }

        private void picMapTextColor35_Click(object sender, EventArgs e)
        {
            selectedColor = picMapTextColor35.BackColor;
            updateColorBoxes();
        }

        private void picMapTextColor36_Click(object sender, EventArgs e)
        {
            selectedColor = picMapTextColor36.BackColor;
            updateColorBoxes();
        }

        private void picMapTextColor37_Click(object sender, EventArgs e)
        {
            selectedColor = picMapTextColor37.BackColor;
            updateColorBoxes();
        }

        private void picMapTextColor38_Click(object sender, EventArgs e)
        {
            selectedColor = picMapTextColor38.BackColor;
            updateColorBoxes();
        }

        private void picMapTextColor39_Click(object sender, EventArgs e)
        {
            selectedColor = picMapTextColor39.BackColor;
            updateColorBoxes();
        }

        private void picMapTextColor40_Click(object sender, EventArgs e)
        {
            selectedColor = picMapTextColor40.BackColor;
            updateColorBoxes();
        }

        private void picMapTextColor1_SizeChanged(object sender, EventArgs e)
        {
            picMapTextColor1.BorderStyle = picMapTextColor1.Size.Height == 13 ? BorderStyle.None : BorderStyle.Fixed3D;
        }

        private void picMapTextColor2_SizeChanged(object sender, EventArgs e)
        {
            picMapTextColor2.BorderStyle = picMapTextColor2.Size.Height == 13 ? BorderStyle.None : BorderStyle.Fixed3D;
        }

        private void picMapTextColor3_SizeChanged(object sender, EventArgs e)
        {
            picMapTextColor3.BorderStyle = picMapTextColor3.Size.Height == 13 ? BorderStyle.None : BorderStyle.Fixed3D;
        }

        private void picMapTextColor4_SizeChanged(object sender, EventArgs e)
        {
            picMapTextColor4.BorderStyle = picMapTextColor4.Size.Height == 13 ? BorderStyle.None : BorderStyle.Fixed3D;
        }

        private void picMapTextColor5_SizeChanged(object sender, EventArgs e)
        {
            picMapTextColor5.BorderStyle = picMapTextColor5.Size.Height == 13 ? BorderStyle.None : BorderStyle.Fixed3D;
        }

        private void picMapTextColor6_SizeChanged(object sender, EventArgs e)
        {
            picMapTextColor6.BorderStyle = picMapTextColor6.Size.Height == 13 ? BorderStyle.None : BorderStyle.Fixed3D;
        }

        private void picMapTextColor7_SizeChanged(object sender, EventArgs e)
        {
            picMapTextColor7.BorderStyle = picMapTextColor7.Size.Height == 13 ? BorderStyle.None : BorderStyle.Fixed3D;
        }

        private void picMapTextColor8_SizeChanged(object sender, EventArgs e)
        {
            picMapTextColor8.BorderStyle = picMapTextColor8.Size.Height == 13 ? BorderStyle.None : BorderStyle.Fixed3D;
        }

        private void picMapTextColor9_SizeChanged(object sender, EventArgs e)
        {
            picMapTextColor9.BorderStyle = picMapTextColor9.Size.Height == 13 ? BorderStyle.None : BorderStyle.Fixed3D;
        }

        private void picMapTextColor10_SizeChanged(object sender, EventArgs e)
        {
            picMapTextColor10.BorderStyle = picMapTextColor10.Size.Height == 13 ? BorderStyle.None : BorderStyle.Fixed3D;
        }

        private void picMapTextColor11_SizeChanged(object sender, EventArgs e)
        {
            picMapTextColor11.BorderStyle = picMapTextColor11.Size.Height == 13 ? BorderStyle.None : BorderStyle.Fixed3D;
        }

        private void picMapTextColor12_SizeChanged(object sender, EventArgs e)
        {
            picMapTextColor12.BorderStyle = picMapTextColor12.Size.Height == 13 ? BorderStyle.None : BorderStyle.Fixed3D;
        }

        private void picMapTextColor13_SizeChanged(object sender, EventArgs e)
        {
            picMapTextColor13.BorderStyle = picMapTextColor13.Size.Height == 13 ? BorderStyle.None : BorderStyle.Fixed3D;
        }

        private void picMapTextColor14_SizeChanged(object sender, EventArgs e)
        {
            picMapTextColor14.BorderStyle = picMapTextColor14.Size.Height == 13 ? BorderStyle.None : BorderStyle.Fixed3D;
        }

        private void picMapTextColor15_SizeChanged(object sender, EventArgs e)
        {
            picMapTextColor15.BorderStyle = picMapTextColor15.Size.Height == 13 ? BorderStyle.None : BorderStyle.Fixed3D;
        }

        private void picMapTextColor16_SizeChanged(object sender, EventArgs e)
        {
            picMapTextColor16.BorderStyle = picMapTextColor16.Size.Height == 13 ? BorderStyle.None : BorderStyle.Fixed3D;
        }

        private void picMapTextColor17_SizeChanged(object sender, EventArgs e)
        {
            picMapTextColor17.BorderStyle = picMapTextColor17.Size.Height == 13 ? BorderStyle.None : BorderStyle.Fixed3D;
        }

        private void picMapTextColor18_SizeChanged(object sender, EventArgs e)
        {
            picMapTextColor18.BorderStyle = picMapTextColor18.Size.Height == 13 ? BorderStyle.None : BorderStyle.Fixed3D;
        }

        private void picMapTextColor19_SizeChanged(object sender, EventArgs e)
        {
            picMapTextColor19.BorderStyle = picMapTextColor19.Size.Height == 13 ? BorderStyle.None : BorderStyle.Fixed3D;
        }

        private void picMapTextColor20_SizeChanged(object sender, EventArgs e)
        {
            picMapTextColor20.BorderStyle = picMapTextColor20.Size.Height == 13 ? BorderStyle.None : BorderStyle.Fixed3D;
        }

        private void picMapTextColor21_SizeChanged(object sender, EventArgs e)
        {
            picMapTextColor21.BorderStyle = picMapTextColor21.Size.Height == 13 ? BorderStyle.None : BorderStyle.Fixed3D;
        }

        private void picMapTextColor22_SizeChanged(object sender, EventArgs e)
        {
            picMapTextColor22.BorderStyle = picMapTextColor22.Size.Height == 13 ? BorderStyle.None : BorderStyle.Fixed3D;
        }

        private void picMapTextColor23_SizeChanged(object sender, EventArgs e)
        {
            picMapTextColor23.BorderStyle = picMapTextColor23.Size.Height == 13 ? BorderStyle.None : BorderStyle.Fixed3D;
        }

        private void picMapTextColor24_SizeChanged(object sender, EventArgs e)
        {
            picMapTextColor24.BorderStyle = picMapTextColor24.Size.Height == 13 ? BorderStyle.None : BorderStyle.Fixed3D;
        }

        private void picMapTextColor25_SizeChanged(object sender, EventArgs e)
        {
            picMapTextColor25.BorderStyle = picMapTextColor25.Size.Height == 13 ? BorderStyle.None : BorderStyle.Fixed3D;
        }

        private void picMapTextColor26_SizeChanged(object sender, EventArgs e)
        {
            picMapTextColor26.BorderStyle = picMapTextColor26.Size.Height == 13 ? BorderStyle.None : BorderStyle.Fixed3D;
        }

        private void picMapTextColor27_SizeChanged(object sender, EventArgs e)
        {
            picMapTextColor27.BorderStyle = picMapTextColor27.Size.Height == 13 ? BorderStyle.None : BorderStyle.Fixed3D;
        }

        private void picMapTextColor28_SizeChanged(object sender, EventArgs e)
        {
            picMapTextColor28.BorderStyle = picMapTextColor28.Size.Height == 13 ? BorderStyle.None : BorderStyle.Fixed3D;
        }

        private void picMapTextColor29_SizeChanged(object sender, EventArgs e)
        {
            picMapTextColor29.BorderStyle = picMapTextColor29.Size.Height == 13 ? BorderStyle.None : BorderStyle.Fixed3D;
        }

        private void picMapTextColor30_SizeChanged(object sender, EventArgs e)
        {
            picMapTextColor30.BorderStyle = picMapTextColor30.Size.Height == 13 ? BorderStyle.None : BorderStyle.Fixed3D;
        }

        private void picMapTextColor31_SizeChanged(object sender, EventArgs e)
        {
            picMapTextColor31.BorderStyle = picMapTextColor31.Size.Height == 13 ? BorderStyle.None : BorderStyle.Fixed3D;
        }

        private void picMapTextColor32_SizeChanged(object sender, EventArgs e)
        {
            picMapTextColor32.BorderStyle = picMapTextColor32.Size.Height == 13 ? BorderStyle.None : BorderStyle.Fixed3D;
        }

        private void picMapTextColor33_SizeChanged(object sender, EventArgs e)
        {
            picMapTextColor33.BorderStyle = picMapTextColor33.Size.Height == 13 ? BorderStyle.None : BorderStyle.Fixed3D;
        }

        private void picMapTextColor34_SizeChanged(object sender, EventArgs e)
        {
            picMapTextColor34.BorderStyle = picMapTextColor34.Size.Height == 13 ? BorderStyle.None : BorderStyle.Fixed3D;
        }

        private void picMapTextColor35_SizeChanged(object sender, EventArgs e)
        {
            picMapTextColor35.BorderStyle = picMapTextColor35.Size.Height == 13 ? BorderStyle.None : BorderStyle.Fixed3D;
        }

        private void picMapTextColor36_SizeChanged(object sender, EventArgs e)
        {
            picMapTextColor36.BorderStyle = picMapTextColor36.Size.Height == 13 ? BorderStyle.None : BorderStyle.Fixed3D;
        }

        private void picMapTextColor37_SizeChanged(object sender, EventArgs e)
        {
            picMapTextColor37.BorderStyle = picMapTextColor37.Size.Height == 13 ? BorderStyle.None : BorderStyle.Fixed3D;
        }

        private void picMapTextColor38_SizeChanged(object sender, EventArgs e)
        {
            picMapTextColor38.BorderStyle = picMapTextColor38.Size.Height == 13 ? BorderStyle.None : BorderStyle.Fixed3D;
        }

        private void picMapTextColor39_SizeChanged(object sender, EventArgs e)
        {
            picMapTextColor39.BorderStyle = picMapTextColor39.Size.Height == 13 ? BorderStyle.None : BorderStyle.Fixed3D;
        }

        private void picMapTextColor40_SizeChanged(object sender, EventArgs e)
        {
            picMapTextColor40.BorderStyle = picMapTextColor40.Size.Height == 13 ? BorderStyle.None : BorderStyle.Fixed3D;
        }

        public void updateColorBoxes()
        {
            // update size based on selected color
            // size update will trigger boarder update.
            MapTextToAdd.ForeColor = selectedColor;

            if(selectedColor == picMapTextColor1.BackColor) {
                if (picMapTextColor1.Size.Height != 14)
                    picMapTextColor1.Size = new System.Drawing.Size(14, 14);
            } else {
                if (picMapTextColor1.Size.Height != 13)
                    picMapTextColor1.Size = new System.Drawing.Size(13, 13);
            }
            if(selectedColor == picMapTextColor2.BackColor) {
                if (picMapTextColor2.Size.Height != 14)
                    picMapTextColor2.Size = new System.Drawing.Size(14, 14);
            } else {
                if (picMapTextColor2.Size.Height != 13)
                    picMapTextColor2.Size = new System.Drawing.Size(13, 13);
            }
            if(selectedColor == picMapTextColor3.BackColor) {
                if (picMapTextColor3.Size.Height != 14)
                    picMapTextColor3.Size = new System.Drawing.Size(14, 14);
            } else {
                if (picMapTextColor3.Size.Height != 13)
                    picMapTextColor3.Size = new System.Drawing.Size(13, 13);
            }
            if(selectedColor == picMapTextColor4.BackColor) {
                if (picMapTextColor4.Size.Height != 14)
                    picMapTextColor4.Size = new System.Drawing.Size(14, 14);
            } else {
                if (picMapTextColor4.Size.Height != 13)
                    picMapTextColor4.Size = new System.Drawing.Size(13, 13);
            }
            if (selectedColor == picMapTextColor5.BackColor)
            {
                if (picMapTextColor5.Size.Height != 14)
                    picMapTextColor5.Size = new System.Drawing.Size(14, 14);
            }
            else
            {
                if (picMapTextColor5.Size.Height != 13)
                    picMapTextColor5.Size = new System.Drawing.Size(13, 13);
            }
            if (selectedColor == picMapTextColor6.BackColor)
            {
                if (picMapTextColor6.Size.Height != 14)
                    picMapTextColor6.Size = new System.Drawing.Size(14, 14);
            }
            else
            {
                if (picMapTextColor6.Size.Height != 13)
                    picMapTextColor6.Size = new System.Drawing.Size(13, 13);
            }
            if (selectedColor == picMapTextColor7.BackColor)
            {
                if (picMapTextColor7.Size.Height != 14)
                    picMapTextColor7.Size = new System.Drawing.Size(14, 14);
            }
            else
            {
                if (picMapTextColor7.Size.Height != 13)
                    picMapTextColor7.Size = new System.Drawing.Size(13, 13);
            }
            if (selectedColor == picMapTextColor8.BackColor)
            {
                if (picMapTextColor8.Size.Height != 14)
                    picMapTextColor8.Size = new System.Drawing.Size(14, 14);
            }
            else
            {
                if (picMapTextColor8.Size.Height != 13)
                    picMapTextColor8.Size = new System.Drawing.Size(13, 13);
            }
            if (selectedColor == picMapTextColor9.BackColor)
            {
                if (picMapTextColor9.Size.Height != 14)
                    picMapTextColor9.Size = new System.Drawing.Size(14, 14);
            }
            else
            {
                if (picMapTextColor9.Size.Height != 13)
                    picMapTextColor9.Size = new System.Drawing.Size(13, 13);
            }
            if (selectedColor == picMapTextColor10.BackColor)
            {
                if (picMapTextColor10.Size.Height != 14)
                    picMapTextColor10.Size = new System.Drawing.Size(14, 14);
            }
            else
            {
                if (picMapTextColor10.Size.Height != 13)
                    picMapTextColor10.Size = new System.Drawing.Size(13, 13);
            }
            if (selectedColor == picMapTextColor11.BackColor)
            {
                if (picMapTextColor11.Size.Height != 14)
                    picMapTextColor11.Size = new System.Drawing.Size(14, 14);
            }
            else
            {
                if (picMapTextColor11.Size.Height != 13)
                    picMapTextColor11.Size = new System.Drawing.Size(13, 13);
            }
            if (selectedColor == picMapTextColor12.BackColor)
            {
                if (picMapTextColor12.Size.Height != 14)
                    picMapTextColor12.Size = new System.Drawing.Size(14, 14);
            }
            else
            {
                if (picMapTextColor12.Size.Height != 13)
                    picMapTextColor12.Size = new System.Drawing.Size(13, 13);
            }
            if (selectedColor == picMapTextColor13.BackColor)
            {
                if (picMapTextColor13.Size.Height != 14)
                    picMapTextColor13.Size = new System.Drawing.Size(14, 14);
            }
            else
            {
                if (picMapTextColor13.Size.Height != 13)
                    picMapTextColor13.Size = new System.Drawing.Size(13, 13);
            }
            if (selectedColor == picMapTextColor14.BackColor)
            {
                if (picMapTextColor14.Size.Height != 14)
                    picMapTextColor14.Size = new System.Drawing.Size(14, 14);
            }
            else
            {
                if (picMapTextColor14.Size.Height != 13)
                    picMapTextColor14.Size = new System.Drawing.Size(13, 13);
            }
            if (selectedColor == picMapTextColor15.BackColor)
            {
                if (picMapTextColor15.Size.Height != 14)
                    picMapTextColor15.Size = new System.Drawing.Size(14, 14);
            }
            else
            {
                if (picMapTextColor15.Size.Height != 13)
                    picMapTextColor15.Size = new System.Drawing.Size(13, 13);
            }
            if (selectedColor == picMapTextColor16.BackColor)
            {
                if (picMapTextColor16.Size.Height != 14)
                    picMapTextColor16.Size = new System.Drawing.Size(14, 14);
            }
            else
            {
                if (picMapTextColor16.Size.Height != 13)
                    picMapTextColor16.Size = new System.Drawing.Size(13, 13);
            }
            if (selectedColor == picMapTextColor17.BackColor)
            {
                if (picMapTextColor17.Size.Height != 14)
                    picMapTextColor17.Size = new System.Drawing.Size(14, 14);
            }
            else
            {
                if (picMapTextColor17.Size.Height != 13)
                    picMapTextColor17.Size = new System.Drawing.Size(13, 13);
            }
            if (selectedColor == picMapTextColor18.BackColor)
            {
                if (picMapTextColor18.Size.Height != 14)
                    picMapTextColor18.Size = new System.Drawing.Size(14, 14);
            }
            else
            {
                if (picMapTextColor18.Size.Height != 13)
                    picMapTextColor18.Size = new System.Drawing.Size(13, 13);
            }
            if (selectedColor == picMapTextColor19.BackColor)
            {
                if (picMapTextColor19.Size.Height != 14)
                    picMapTextColor19.Size = new System.Drawing.Size(14, 14);
            }
            else
            {
                if (picMapTextColor19.Size.Height != 13)
                    picMapTextColor19.Size = new System.Drawing.Size(13, 13);
            }
            if (selectedColor == picMapTextColor20.BackColor)
            {
                if (picMapTextColor20.Size.Height != 14)
                    picMapTextColor20.Size = new System.Drawing.Size(14, 14);
            }
            else
            {
                if (picMapTextColor20.Size.Height != 13)
                    picMapTextColor20.Size = new System.Drawing.Size(13, 13);
            }
            if (selectedColor == picMapTextColor21.BackColor)
            {
                if (picMapTextColor21.Size.Height != 14)
                    picMapTextColor21.Size = new System.Drawing.Size(14, 14);
            }
            else
            {
                if (picMapTextColor21.Size.Height != 13)
                    picMapTextColor21.Size = new System.Drawing.Size(13, 13);
            }
            if (selectedColor == picMapTextColor22.BackColor)
            {
                if (picMapTextColor22.Size.Height != 14)
                    picMapTextColor22.Size = new System.Drawing.Size(14, 14);
            }
            else
            {
                if (picMapTextColor22.Size.Height != 13)
                    picMapTextColor22.Size = new System.Drawing.Size(13, 13);
            }
            if (selectedColor == picMapTextColor23.BackColor)
            {
                if (picMapTextColor23.Size.Height != 14)
                    picMapTextColor23.Size = new System.Drawing.Size(14, 14);
            }
            else
            {
                if (picMapTextColor23.Size.Height != 13)
                    picMapTextColor23.Size = new System.Drawing.Size(13, 13);
            }
            if (selectedColor == picMapTextColor24.BackColor)
            {
                if (picMapTextColor24.Size.Height != 14)
                    picMapTextColor24.Size = new System.Drawing.Size(14, 14);
            }
            else
            {
                if (picMapTextColor24.Size.Height != 13)
                    picMapTextColor24.Size = new System.Drawing.Size(13, 13);
            }
            if (selectedColor == picMapTextColor25.BackColor)
            {
                if (picMapTextColor25.Size.Height != 14)
                    picMapTextColor25.Size = new System.Drawing.Size(14, 14);
            }
            else
            {
                if (picMapTextColor25.Size.Height != 13)
                    picMapTextColor25.Size = new System.Drawing.Size(13, 13);
            }
            if (selectedColor == picMapTextColor26.BackColor)
            {
                if (picMapTextColor26.Size.Height != 14)
                    picMapTextColor26.Size = new System.Drawing.Size(14, 14);
            }
            else
            {
                if (picMapTextColor26.Size.Height != 13)
                    picMapTextColor26.Size = new System.Drawing.Size(13, 13);
            }
            if (selectedColor == picMapTextColor27.BackColor)
            {
                if (picMapTextColor27.Size.Height != 14)
                    picMapTextColor27.Size = new System.Drawing.Size(14, 14);
            }
            else
            {
                if (picMapTextColor27.Size.Height != 13)
                    picMapTextColor27.Size = new System.Drawing.Size(13, 13);
            }
            if (selectedColor == picMapTextColor28.BackColor)
            {
                if (picMapTextColor28.Size.Height != 14)
                    picMapTextColor28.Size = new System.Drawing.Size(14, 14);
            }
            else
            {
                if (picMapTextColor28.Size.Height != 13)
                    picMapTextColor28.Size = new System.Drawing.Size(13, 13);
            }
            if (selectedColor == picMapTextColor29.BackColor)
            {
                if (picMapTextColor29.Size.Height != 14)
                    picMapTextColor29.Size = new System.Drawing.Size(14, 14);
            }
            else
            {
                if (picMapTextColor29.Size.Height != 13)
                    picMapTextColor29.Size = new System.Drawing.Size(13, 13);
            }
            if (selectedColor == picMapTextColor30.BackColor)
            {
                if (picMapTextColor30.Size.Height != 14)
                    picMapTextColor30.Size = new System.Drawing.Size(14, 14);
            }
            else
            {
                if (picMapTextColor30.Size.Height != 13)
                    picMapTextColor30.Size = new System.Drawing.Size(13, 13);
            }
            if (selectedColor == picMapTextColor31.BackColor)
            {
                if (picMapTextColor31.Size.Height != 14)
                    picMapTextColor31.Size = new System.Drawing.Size(14, 14);
            }
            else
            {
                if (picMapTextColor31.Size.Height != 13)
                    picMapTextColor31.Size = new System.Drawing.Size(13, 13);
            }
            if (selectedColor == picMapTextColor32.BackColor)
            {
                if (picMapTextColor32.Size.Height != 14)
                    picMapTextColor32.Size = new System.Drawing.Size(14, 14);
            }
            else
            {
                if (picMapTextColor32.Size.Height != 13)
                    picMapTextColor32.Size = new System.Drawing.Size(13, 13);
            }
            if (selectedColor == picMapTextColor33.BackColor)
            {
                if (picMapTextColor33.Size.Height != 14)
                    picMapTextColor33.Size = new System.Drawing.Size(14, 14);
            }
            else
            {
                if (picMapTextColor33.Size.Height != 13)
                    picMapTextColor33.Size = new System.Drawing.Size(13, 13);
            }
            if (selectedColor == picMapTextColor34.BackColor)
            {
                if (picMapTextColor34.Size.Height != 14)
                    picMapTextColor34.Size = new System.Drawing.Size(14, 14);
            }
            else
            {
                if (picMapTextColor34.Size.Height != 13)
                    picMapTextColor34.Size = new System.Drawing.Size(13, 13);
            }
            if (selectedColor == picMapTextColor35.BackColor)
            {
                if (picMapTextColor35.Size.Height != 14)
                    picMapTextColor35.Size = new System.Drawing.Size(14, 14);
            }
            else
            {
                if (picMapTextColor35.Size.Height != 13)
                    picMapTextColor35.Size = new System.Drawing.Size(13, 13);
            }
            if (selectedColor == picMapTextColor36.BackColor)
            {
                if (picMapTextColor36.Size.Height != 14)
                    picMapTextColor36.Size = new System.Drawing.Size(14, 14);
            }
            else
            {
                if (picMapTextColor36.Size.Height != 13)
                    picMapTextColor36.Size = new System.Drawing.Size(13, 13);
            }
            if (selectedColor == picMapTextColor37.BackColor)
            {
                if (picMapTextColor37.Size.Height != 14)
                    picMapTextColor37.Size = new System.Drawing.Size(14, 14);
            }
            else
            {
                if (picMapTextColor37.Size.Height != 13)
                    picMapTextColor37.Size = new System.Drawing.Size(13, 13);
            }
            if (selectedColor == picMapTextColor38.BackColor)
            {
                if (picMapTextColor38.Size.Height != 14)
                    picMapTextColor38.Size = new System.Drawing.Size(14, 14);
            }
            else
            {
                if (picMapTextColor38.Size.Height != 13)
                    picMapTextColor38.Size = new System.Drawing.Size(13, 13);
            }
            if (selectedColor == picMapTextColor39.BackColor)
            {
                if (picMapTextColor39.Size.Height != 14)
                    picMapTextColor39.Size = new System.Drawing.Size(14, 14);
            }
            else
            {
                if (picMapTextColor39.Size.Height != 13)
                    picMapTextColor39.Size = new System.Drawing.Size(13, 13);
            }
            if (selectedColor == picMapTextColor40.BackColor)
            {
                if (picMapTextColor40.Size.Height != 14)
                    picMapTextColor40.Size = new System.Drawing.Size(14, 14);
            }
            else
            {
                if (picMapTextColor40.Size.Height != 13)
                    picMapTextColor40.Size = new System.Drawing.Size(13, 13);
            }
        }
    }
}
