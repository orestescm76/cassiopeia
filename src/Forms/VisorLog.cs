using System;
using System.Windows.Forms;

namespace Cassiopea.src.Forms
{
    public partial class VisorLog : Form
    {
        public VisorLog()
        {
            InitializeComponent();
        }

        private void VisorLog_Load(object sender, EventArgs e)
        {

        }
        public void AddText(String mensaje)
        {
            textBoxLog.AppendText(mensaje+Environment.NewLine);
        }

        private void VisorLog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.ApplicationExitCall)
                e.Cancel = true;
            else
                e.Cancel = false;
            Hide();
        }
    }
}
