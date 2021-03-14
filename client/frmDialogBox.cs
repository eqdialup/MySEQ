using System;

using System.Windows.Forms;

namespace myseq

{
    public partial class frmDialogBox : Form

    {
        public frmDialogBox()

        {
            InitializeComponent();
        }

        private void Button2_Click(object sender, EventArgs e)

        {
            Close();
        }

        private void Button1_Click(object sender, EventArgs e)

        {
            Close();
        }
    }
}