using System.Windows.Forms;

namespace Cassiopeia.src.Forms
{
    public partial class LoadBar : Form
    {
        int ElementosACargar;
        int count = 0;
        string Task;
        bool update = true;
        public LoadBar(int Elementos, string task, bool update = true)
        {
            InitializeComponent();
            ElementosACargar = Elementos;
            progressBar1.Maximum = ElementosACargar;
            label1.Text = task;
            Task = task;
            this.update = update;
        }
        public void Progreso()
        {
            progressBar1.PerformStep();
            progressBar1.Update();
            Application.DoEvents();
            count++;
            if(update)
                label1.Text = Task + "(" + count + "/" + ElementosACargar + ")";
        }
    }
}