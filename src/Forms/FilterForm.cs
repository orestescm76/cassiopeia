using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cassiopeia.src.Forms
{
    public partial class FilterForm : Form
    {
        public FilterForm()
        {
            InitializeComponent();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            //apply filters
            Kernel.MainForm.ApplyFilter(textBoxArtist.Text);
            Close();
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            Kernel.MainForm.ResetFilter();
        }
    }
}
