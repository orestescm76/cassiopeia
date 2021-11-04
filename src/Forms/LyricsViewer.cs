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
    public partial class LyricsViewer : Form
    {
        private Song cancion;
        private ToolTip ConsejoDeshacer;
        private Font Tipografia;
        public LyricsViewer(Song c)
        {
            InitializeComponent();
            Icon = Properties.Resources.letras;
            Tipografia = Config.FontLyrics;
            textBoxLyrics.Font = Tipografia;
            cancion = c;
            if (c.Lyrics == null)
                c.Lyrics = new string[0];
            textBoxLyrics.Lines = cancion.Lyrics;
            Text = c.ToString() + " (" + Tipografia.Size + ")";
            ConsejoDeshacer = new ToolTip();
            PonerTextos();
            textBoxLyrics.DeselectAll();
            if(cancion.AlbumFrom is null)
            {
                buttonBack.Enabled = false;
                buttonNext.Enabled = false;
            }
            buttonBack.Enabled = !(cancion.IndexInAlbum == 1);
            buttonNext.Enabled = (cancion.IndexInAlbum != cancion.AlbumFrom.Songs.Count);
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
            if (object.ReferenceEquals(cancion.AlbumFrom, null))
            {
                buttonBack.Enabled = false;
                buttonNext.Enabled = false;
            }
            buttonBack.Enabled = !(cancion.IndexInAlbum == 1);
            buttonNext.Enabled = (cancion.IndexInAlbum != cancion.AlbumFrom.Songs.Count);
        }
        private void PonerTextos()
        {
            buttonBuscar.Text = Kernel.LocalTexts.GetString("buscar");
            buttonEditar.Text = Kernel.LocalTexts.GetString("editar");
            buttonCerrar.Text = Kernel.LocalTexts.GetString("cerrar");
            buttonLimpiar.Text = Kernel.LocalTexts.GetString("limpiar");
            buttonDeshacer.Text = Kernel.LocalTexts.GetString("deshacer");
            ConsejoDeshacer.SetToolTip(buttonDeshacer, Kernel.LocalTexts.GetString("consejoDeshacer"));
            buttonBack.Text = Kernel.LocalTexts.GetString("anterior");
            buttonNext.Text = Kernel.LocalTexts.GetString("siguiente");
        }
        private void Guardar()
        {
            cancion.Lyrics = textBoxLyrics.Lines;
            Log.Instance.PrintMessage("Lyrics saved!", MessageType.Correct);
        }
        #region Events
        private void buttonEditar_Click(object sender, EventArgs e)
        {
            if(!textBoxLyrics.ReadOnly)
            {
                textBoxLyrics.ReadOnly = true;
                buttonEditar.Text = Kernel.LocalTexts.GetString("editar");
            }
            else
            {
                textBoxLyrics.ReadOnly = false;
                buttonEditar.Text = Kernel.LocalTexts.GetString("aceptar");

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
            CambiarCancion(cancion.AlbumFrom.GetSong(cancion.IndexInAlbum));
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            Guardar();
            CambiarCancion(cancion.AlbumFrom.GetSong(cancion.IndexInAlbum-2));
        }
        private void textBoxLyrics_MouseWheel(object sender, MouseEventArgs e)
        {
            Font tipografiaNew = Tipografia;
            if(ModifierKeys == Keys.Control)
            {
                if (e.Delta > 0)
                    tipografiaNew = new Font(Tipografia.FontFamily.Name, Tipografia.Size + 2);
                else
                    tipografiaNew = new Font(Tipografia.FontFamily.Name, Tipografia.Size - 2);
            }
            if (tipografiaNew.Size <= 2 || tipografiaNew.Size >= 75)
                tipografiaNew = Tipografia; //no se cambia
            textBoxLyrics.Font = Tipografia = tipografiaNew;
            Text = cancion.ToString() + " (" + Tipografia.Size + ")";
            Log.Instance.PrintMessage("Changed font size to " + tipografiaNew.Size, MessageType.Info);
        }
        #endregion
    }
}
