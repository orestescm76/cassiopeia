using System;
using System.Windows.Forms;

namespace Cassiopeia.src.Forms
{
    public partial class LogViewer : Form
    {
        private delegate void SafeCallAddText(string msg);
        public LogViewer()
        {
            InitializeComponent();
        }

        private void VisorLog_Load(object sender, EventArgs e)
        {

        }
        public void AddText(String mensaje)
        {
            //Visual Studio complains that sometime this call is unsafe...
            if (textBoxLog.InvokeRequired)
            {
                var call = new SafeCallAddText(AddText);
                textBoxLog.Invoke(call, new object[] { mensaje });
            }
            else
                textBoxLog.AppendText(mensaje + Environment.NewLine);
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
