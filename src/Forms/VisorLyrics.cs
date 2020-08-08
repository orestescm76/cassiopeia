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
        private ToolTip ConsejoDeshacer;
        public VisorLyrics(Cancion c)
        {
            InitializeComponent();
            cancion = c;
            if (c.Lyrics == null)
                c.Lyrics = new string[0];
            textBoxLyrics.Lines = cancion.Lyrics;
            Text = c.ToString();
            ConsejoDeshacer = new ToolTip();
            PonerTextos();
            textBoxLyrics.DeselectAll();
            if(cancion.album == null)
            {
                buttonBack.Enabled = false;
                buttonNext.Enabled = false;
            }
            buttonBack.Enabled = !(cancion.Num == 1);
            buttonNext.Enabled = (cancion.Num != cancion.album.canciones.Count);
        }
        private void CambiarCancion(Cancion c)
        {
            cancion = c;
            Recargar();
        }
        private void Recargar()
        {
            if (cancion.Lyrics == null)
                cancion.Lyrics = new string[0];
            textBoxLyrics.Lines = cancion.Lyrics;
            Text = cancion.ToString();
            textBoxLyrics.DeselectAll();
            if (cancion.album == null)
            {
                buttonBack.Enabled = false;
                buttonNext.Enabled = false;
            }
            buttonBack.Enabled = !(cancion.Num == 1);
            buttonNext.Enabled = (cancion.Num != cancion.album.canciones.Count);
        }
        private void PonerTextos()
        {
            buttonBuscar.Text = Programa.textosLocal.GetString("buscar");
            buttonEditar.Text = Programa.textosLocal.GetString("editar");
            buttonCerrar.Text = Programa.textosLocal.GetString("cerrar");
            buttonLimpiar.Text = Programa.textosLocal.GetString("limpiar");
            buttonDeshacer.Text = Programa.textosLocal.GetString("deshacer");
            ConsejoDeshacer.SetToolTip(buttonDeshacer, Programa.textosLocal.GetString("consejoDeshacer"));
            buttonBack.Text = Programa.textosLocal.GetString("anterior");
            buttonNext.Text = Programa.textosLocal.GetString("siguiente");
        }
        private void Guardar()
        {
            cancion.Lyrics = textBoxLyrics.Lines;
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

        private void buttonCerrar_Click(object sender, EventArgs e)
        {
            cancion.Lyrics = textBoxLyrics.Lines;
            this.Close();
        }

        private void buttonLimpiar_Click(object sender, EventArgs e)
        {
            textBoxLyrics.Clear();
        }

        private void buttonDeshacer_Click(object sender, EventArgs e)
        {
            textBoxLyrics.Lines = cancion.Lyrics;
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            Guardar();
            CambiarCancion(cancion.album.getCancion(cancion.Num));
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            Guardar();
            CambiarCancion(cancion.album.getCancion(cancion.Num-2));
        }
    }
}
