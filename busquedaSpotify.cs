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
    public partial class busquedaSpotify : Form
    {
        public busquedaSpotify()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            principal.BusquedaSpotify = textBox1.Text;
            this.Close();
        }
    }
}
