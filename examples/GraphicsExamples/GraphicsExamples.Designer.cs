namespace GraphicsExamples
{
    partial class GraphicsExamples
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
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.subtractButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(286, 336);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(239, 95);
            this.listBox1.TabIndex = 0;
            // 
            // subtractButton
            // 
            this.subtractButton.Location = new System.Drawing.Point(163, 131);
            this.subtractButton.Name = "subtractButton";
            this.subtractButton.Size = new System.Drawing.Size(100, 102);
            this.subtractButton.TabIndex = 1;
            this.subtractButton.Text = "button1";
            this.subtractButton.UseVisualStyleBackColor = true;
            // 
            // GraphicsExamples
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(527, 432);
            this.Controls.Add(this.subtractButton);
            this.Controls.Add(this.listBox1);
            this.Name = "GraphicsExamples";
            this.Text = "Form1";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.GraphicsExamples_Paint);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button subtractButton;
    }
}

