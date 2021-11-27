using System;
using System.Windows.Forms;
using Cassiopeia.src.Classes;

namespace Cassiopeia.src.Forms
{
    public partial class CreateAlbum : Form
    {
        private string caratula = "";
        private String[] genresToSelect = new string[Kernel.Genres.Length-1];
        public CreateAlbum()
        {
            InitializeComponent();
            ponerTextos();
            Log.Instance.PrintMessage("Creating album manually", MessageType.Info);
        }
        private void ponerTextos()
        {
            Text = Kernel.LocalTexts.GetString("agregar_album");
            labelArtista.Text = Kernel.LocalTexts.GetString("artista");
            labelTitulo.Text = Kernel.LocalTexts.GetString("titulo");
            labelAño.Text = Kernel.LocalTexts.GetString("año");
            labelNumCanciones.Text = Kernel.LocalTexts.GetString("numcanciones");
            labelGenero.Text = Kernel.LocalTexts.GetString("genero");
            buttonAddAlbum.Text = Kernel.LocalTexts.GetString("añadir");
            addCaratula.Text = Kernel.LocalTexts.GetString("addcaratula");
            labelCaratula.Text = Kernel.LocalTexts.GetString("caratula");
            labelAlbumType.Text = Kernel.LocalTexts.GetString("tipoAlbum");
            for (int i = 0; i < Kernel.Genres.Length-1; i++)
            {
                genresToSelect[i] = Kernel.Genres[i].Name;
            }
            Array.Sort(genresToSelect);
            comboBoxGenres.Items.AddRange(genresToSelect);

            string[] types = new string[5];
            types[(int)AlbumType.Studio] = Kernel.LocalTexts.GetString("estudio");
            types[(int)AlbumType.Live] = Kernel.LocalTexts.GetString("live");
            types[(int)AlbumType.Compilation] = Kernel.LocalTexts.GetString("compilacion");
            types[(int)AlbumType.EP] = Kernel.LocalTexts.GetString("EP");
            types[(int)AlbumType.Single] = Kernel.LocalTexts.GetString("sencillo");
            comboBoxAlbumType.Items.AddRange(types);
            comboBoxAlbumType.SelectedIndex = 0;
        }
        private void buttonAddCover_Click(object sender, EventArgs e)
        {
            Log.Instance.PrintMessage("Looking for album cover", MessageType.Info);
            OpenFileDialog abrirImagen = new OpenFileDialog();
            abrirImagen.Filter = Kernel.LocalTexts.GetString("archivo") + " .jpg, .png|*.jpg;*.png;*.jpeg";
            abrirImagen.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            if (abrirImagen.ShowDialog() == DialogResult.OK)
            {
                string fichero = abrirImagen.FileName;
                caratula = fichero;
                ruta.Text = fichero;
            }
            Log.Instance.PrintMessage("Image " + ruta + " loaded", MessageType.Correct);
        }
        private void buttonAddAlbum_Click(object sender, EventArgs e)
        {
            string title, artist;
            bool cancelado = false;
            short year, nC;
            try
            {
                title = tituloTextBox.Text;
                artist = artistaTextBox.Text;
                int gn = comboBoxGenres.SelectedIndex;
                string gent = comboBoxGenres.SelectedItem.ToString();
                year = Convert.ToInt16(yearTextBox.Text);
                nC = Convert.ToInt16(numCancionesTextBox.Text);
                Genre g = Kernel.Genres[Kernel.FindTranslatedGenre(gent)];
                AlbumData a = null;
                if (caratula == "")
                    a = new AlbumData(g, title, artist, year, "");
                else
                    a = new AlbumData(g, title, artist, year, caratula);
                Kernel.Collection.AddAlbum(ref a);
                DialogResult cancelar = DialogResult.OK;
                for (int i = 0; i < nC; i++)
                {
                    CreateSong agregarCancion = new CreateSong(ref a, i);
                    Hide();
                    cancelar = agregarCancion.ShowDialog();
                    if (cancelar == DialogResult.Cancel)
                    {
                        Log.Instance.PrintMessage("Canceled", MessageType.Warning);
                        Kernel.Collection.RemoveAlbum(ref a);
                        Close();
                        cancelado = true;
                        break;
                    }
                    else if (cancelar == DialogResult.None)
                        continue;
                }
                if (!cancelado)
                    Log.Instance.PrintMessage(artist + " - " + title + " added OK", MessageType.Correct);
                Kernel.ReloadView();
                Close();
            }
            catch (NullReferenceException ex)
            {
                Log.Instance.PrintMessage(ex.Message, MessageType.Error);
                MessageBox.Show(Kernel.LocalTexts.GetString("error_vacio1"));
            }

            catch (FormatException ex)
            {
                Log.Instance.PrintMessage(ex.Message, MessageType.Error);
                MessageBox.Show(Kernel.LocalTexts.GetString("error_formato"));
                //throw;
            }
        }
    }
}
