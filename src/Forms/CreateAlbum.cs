using Cassiopeia.src.Classes;
using System;
using System.Windows.Forms;

namespace Cassiopeia.src.Forms
{
    public partial class CreateAlbum : Form
    {
        private string caratula = "";
        private String[] genresToSelect = new string[Kernel.Genres.Length - 1];
        public CreateAlbum()
        {
            InitializeComponent();
            ponerTextos();
            Log.Instance.PrintMessage("Creating album manually", MessageType.Info);
        }
        private void ponerTextos()
        {
            Text = Kernel.GetText("agregar_album");
            labelArtista.Text = Kernel.GetText("artista");
            labelTitulo.Text = Kernel.GetText("titulo");
            labelAño.Text = Kernel.GetText("año");
            labelNumCanciones.Text = Kernel.GetText("numcanciones");
            labelGenero.Text = Kernel.GetText("genero");
            buttonAddAlbum.Text = Kernel.GetText("añadir");
            addCaratula.Text = Kernel.GetText("addcaratula");
            labelCaratula.Text = Kernel.GetText("caratula");
            labelAlbumType.Text = Kernel.GetText("tipoAlbum");
            labelCoverPATH.Text = "";
            for (int i = 0; i < Kernel.Genres.Length - 1; i++)
            {
                genresToSelect[i] = Kernel.Genres[i].Name;
            }
            Array.Sort(genresToSelect);
            comboBoxGenres.Items.AddRange(genresToSelect);

            string[] types = new string[5];
            types[(int)AlbumType.Studio] = Kernel.GetText("estudio");
            types[(int)AlbumType.Live] = Kernel.GetText("live");
            types[(int)AlbumType.Compilation] = Kernel.GetText("compilacion");
            types[(int)AlbumType.EP] = Kernel.GetText("EP");
            types[(int)AlbumType.Single] = Kernel.GetText("sencillo");
            comboBoxAlbumType.Items.AddRange(types);
            comboBoxAlbumType.SelectedIndex = 0;
        }
        private void buttonAddCover_Click(object sender, EventArgs e)
        {
            Log.Instance.PrintMessage("Looking for album cover", MessageType.Info);
            OpenFileDialog abrirImagen = new OpenFileDialog();
            abrirImagen.Filter = Kernel.GetText("archivo") + " .jpg, .png|*.jpg;*.png;*.jpeg";
            abrirImagen.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            if (abrirImagen.ShowDialog() == DialogResult.OK)
            {
                string fichero = abrirImagen.FileName;
                caratula = fichero;
                labelCoverPATH.Text = fichero;
            }
            Log.Instance.PrintMessage("Image " + labelCoverPATH + " loaded", MessageType.Correct);
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
                MessageBox.Show(Kernel.GetText("error_vacio1"));
            }

            catch (FormatException ex)
            {
                Log.Instance.PrintMessage(ex.Message, MessageType.Error);
                MessageBox.Show(Kernel.GetText("error_formato"));
                //throw;
            }
        }
    }
}
