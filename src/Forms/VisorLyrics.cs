using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace aplicacion_musica.src.Forms
{
    public partial class VisorLyrics : Form
    {
        private Cancion cancion;
        public VisorLyrics()
        {
            InitializeComponent();
            textBoxLyrics.Lines = cancion.Lyrics;
        }

        private void buttonEditar_Click(object sender, EventArgs e)
        {
            if(!textBoxLyrics.ReadOnly)
            {
                textBoxLyrics.ReadOnly = true;
                //boton.texto = editar
            }
            else
            {
                textBoxLyrics.ReadOnly = false;
                //boton.texto = OK

            }
        }
    }
}
