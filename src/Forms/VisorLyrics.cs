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
        private Song cancion;
        private ToolTip ConsejoDeshacer;
        private Font Tipografia;
        public VisorLyrics(Song c)
        {
            InitializeComponent();
            Icon = Properties.Resources.letras;
            Tipografia = new Font(Config.TipografiaLyrics, 9);
            textBoxLyrics.Font = Tipografia;
            cancion = c;
            if (c.Lyrics == null)
                c.Lyrics = new string[0];
            textBoxLyrics.Lines = cancion.Lyrics;
            Text = c.ToString() + " (" + Tipografia.Size + ")";
            ConsejoDeshacer = new ToolTip();
            PonerTextos();
            textBoxLyrics.DeselectAll();
            if(object.ReferenceEquals(cancion.album, null))
            {
                buttonBack.Enabled = false;
                buttonNext.Enabled = false;
            }
            buttonBack.Enabled = !(cancion.Num == 1);
            buttonNext.Enabled = (cancion.Num != cancion.album.Songs.Count);
            textBoxLyrics.MouseWheel += new MouseEventHandler(textBoxLyrics_MouseWheel);
        }
        private void CambiarCancion(Song c)
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
            if (object.ReferenceEquals(cancion.album, null))
            {
                buttonBack.Enabled = false;
                buttonNext.Enabled = false;
            }
            buttonBack.Enabled = !(cancion.Num == 1);
            buttonNext.Enabled = (cancion.Num != cancion.album.Songs.Count);
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
                buttonEditar.Text = Programa.textosLocal.GetString("editar");
            }
            else
            {
                textBoxLyrics.ReadOnly = false;
                buttonEditar.Text = Programa.textosLocal.GetString("aceptar");

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
        private void textBoxLyrics_MouseWheel(object sender, MouseEventArgs e)
        {
            Font tipografiaNew = Tipografia;
            if(Control.ModifierKeys == Keys.Control)
            {
                if (e.Delta > 0)
                    tipografiaNew = new Font(Tipografia.FontFamily.Name, Tipografia.Size + 2);
                else
                {
                    tipografiaNew = new Font(Tipografia.FontFamily.Name, Tipografia.Size - 2);
                    if (tipografiaNew.Size <= 2)
                        tipografiaNew = Tipografia; //no se cambia
                }
            }
            textBoxLyrics.Font = Tipografia = tipografiaNew;
            Text = cancion.ToString() + " (" + Tipografia.Size + ")";
        }
    }
}
