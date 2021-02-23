using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cassiopea.src.Forms
{
    public partial class BarraCarga : Form
    {
        int ElementosACargar;
        public BarraCarga(int Elementos)
        {
            InitializeComponent();
            ElementosACargar = Elementos;
            progressBar1.Maximum = ElementosACargar;
        }
        public void Progreso()
        {
            progressBar1.PerformStep();
        }
    }
}