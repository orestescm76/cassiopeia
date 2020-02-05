using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace aplicacion_musica
{
    public partial class acercaDe : Form
    {
        public acercaDe()
        {
            InitializeComponent();
            labelAcercaDe.Text = labelAcercaDe.Text.Replace("{ver}", Programa.version);
            Text = Programa.textosLocal[46] + " " + Programa.textosLocal[0] + " " + Programa.version;
        }
    }
}
