using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Text;
using System.Windows.Forms;

namespace GraphicsExamples
{
    public partial class GraphicsExamples : Form
    {
        private int temp = 0;

        public GraphicsExamples()
        {
            InitializeComponent();
            PopulateListBoxWithGraphicsResolution();
        }

        private void ChangePageScaleAndTranslateTransform(PaintEventArgs e)
        {
            // Create a rectangle.
            Rectangle rectangle1 = new Rectangle(20, 20, 50, 100);

            // Draw its outline.
            e.Graphics.DrawRectangle(Pens.SlateBlue, rectangle1);

            // Change the page scale.  
            e.Graphics.PageScale = 2.0F;

            // Call TranslateTransform to change the origin of the
            //  Graphics object.
            e.Graphics.TranslateTransform(10.0F, 10.0F);

            // Draw the rectangle again.
            e.Graphics.DrawRectangle(Pens.Tomato, rectangle1);

            // Set the page scale and origin back to their original values.
            e.Graphics.PageScale = 1.0F;
            e.Graphics.TranslateTransform(0.0F, 0.0F);

            SolidBrush transparentBrush = new SolidBrush(Color.FromArgb(50,
                Color.Yellow));

            // Create a new rectangle with the coordinates you expect
            // after setting PageScale and calling TranslateTransform:
            // x = 10 + (20 * 2)
            // y = 10 + (20 * 2)
            // Width = 50 * 2
            // Length = 100 * 2
            Rectangle newRectangle = new Rectangle(50, 50, 100, 200);

            // Fill in the rectangle with a semi-transparent color.
            e.Graphics.FillRectangle(transparentBrush, newRectangle);
        }

        private void SetAndFillClip(PaintEventArgs e)
        {

            // Set the Clip property to a new region.
            e.Graphics.Clip = new Region(new Rectangle(200, 10, 100, 200));

            // Fill the region.
            e.Graphics.FillRegion(Brushes.LightSalmon, e.Graphics.Clip);

            // Demonstrate the clip region by drawing a string
            // at the outer edge of the region.
            e.Graphics.DrawString("Outside of Clip", new Font("Arial",
                12.0F, FontStyle.Regular), Brushes.Black, 195.0F, 5.0F);

        }

        private void PopulateListBoxWithGraphicsResolution()
        {
            Graphics boxGraphics = listBox1.CreateGraphics();
            Graphics formGraphics = this.CreateGraphics();

            listBox1.Items.Add("ListBox horizontal resolution: "
                + boxGraphics.DpiX);
            listBox1.Items.Add("ListBox vertical resolution: "
                + boxGraphics.DpiY);

            boxGraphics.Dispose();
        }
        
        private void CreatePointsAndSizes(PaintEventArgs e)
        {

            // Create the starting point.
            Point startPoint = new Point(subtractButton.Size);

            // Use the addition operator to get the end point.
            Point endPoint = startPoint + new Size(140, 150);

            // Draw a line between the points.
            e.Graphics.DrawLine(SystemPens.Highlight, startPoint, endPoint);

            // Convert the starting point to a size and compare it to the
            // subtractButton size.  
            Size buttonSize = (Size)startPoint;
            if (buttonSize == subtractButton.Size)

            // If the sizes are equal, tell the user.
            {
                e.Graphics.DrawString("The sizes are equal.",
                    new Font(this.Font, FontStyle.Italic),
                    Brushes.Indigo, 10.0F, 65.0F);
            }

        }

        private void RenderOriginTest(PaintEventArgs e)
        {
            e.Graphics.Clear(Color.Pink);
            // Create and draw a rectangle.
            Rectangle rectangle1 = new Rectangle(temp, 20, 50, 100);
            e.Graphics.DrawRectangle(Pens.SlateBlue, rectangle1);
            
            temp++;
            if (temp > 100)
                temp = 0;
            
            e.Graphics.RenderingOrigin = new Point(temp, 0);
            
            e.Graphics.FillEllipse(new HatchBrush(HatchStyle.LargeCheckerBoard,Color.Firebrick), new Rectangle(50, 50, 500, 500));

            Thread.Sleep(200);
            this.Invalidate();
        }



        private void GraphicsExamples_Paint(object sender, PaintEventArgs e)
        {
            //ChangePageScaleAndTranslateTransform(e);
            //CreatePointsAndSizes(e);
            //SetAndFillClip(e);
            RenderOriginTest(e);
        }
    }
}