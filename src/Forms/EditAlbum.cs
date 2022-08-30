using Cassiopeia.src.Classes;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Cassiopeia.src.Forms
{
    public partial class EditAlbum : Form
    {
        private AlbumData albumAEditar;
        bool fromMainView = false;
        public EditAlbum(ref AlbumData a, bool fromMain = false)
        {
            InitializeComponent();
            fromMainView = fromMain;
            Log.Instance.PrintMessage("Editing album " + a.Artist + " - " + a.Title, MessageType.Info);
            albumAEditar = a;
            textBoxArtista.Text = albumAEditar.Artist;
            textBoxAño.Text = albumAEditar.Year.ToString();
            textBoxTitulo.Text = albumAEditar.Title;
            labelRuta.Text = albumAEditar.CoverPath;
            labelDirectorioActual.Text = albumAEditar.SoundFilesPath;
            textBoxURISpotify.Text = albumAEditar.IdSpotify;
            vistaCanciones.View = View.List;
            ponerTextos();
            cargarVista();

        }
        private void ponerTextos()
        {
            Text = Kernel.GetText("editando") + " " + albumAEditar.Artist + " - " + albumAEditar.Title;
            labelArtista.Text = Kernel.GetText("artista");
            labelTitulo.Text = Kernel.GetText("titulo");
            labelAño.Text = Kernel.GetText("año");
            labelGeneros.Text = Kernel.GetText("genero");
            labelCaratula.Text = Kernel.GetText("caratula");
            labelDirectorio.Text = Kernel.GetText("directorio");
            labelURISpotify.Text = Kernel.GetText("uriSpotify");
            labelAlbumType.Text = Kernel.GetText("tipoAlbum");
            botonOkDoomer.Text = Kernel.GetText("hecho");
            botonCancelar.Text = Kernel.GetText("cancelar");
            botonCaratula.Text = Kernel.GetText("buscar");
            buttonAñadirCancion.Text = Kernel.GetText("añadir_cancion");
            buttonDirectorio.Text = Kernel.GetText("buscarDirectorio");
            labelDirectorioActual.Text = albumAEditar.SoundFilesPath;
            string[] generosTraducidos = new string[Kernel.Genres.Length - 1];
            for (int i = 0; i < generosTraducidos.Length; i++)
            {
                generosTraducidos[i] = Kernel.Genres[i].Name;
            }
            Array.Sort(generosTraducidos);
            comboBoxGeneros.Items.AddRange(generosTraducidos);
            int index = 0;
            for (int i = 0; i < generosTraducidos.Length; i++)
            {
                if (albumAEditar.Genre.Name == generosTraducidos[i])
                    index = i;
            }
            comboBoxGeneros.SelectedIndex = index;

            string[] types = new string[5];
            types[(int)AlbumType.Studio] = Kernel.GetText("estudio");
            types[(int)AlbumType.Live] = Kernel.GetText("live");
            types[(int)AlbumType.Compilation] = Kernel.GetText("compilacion");
            types[(int)AlbumType.EP] = Kernel.GetText("EP");
            types[(int)AlbumType.Single] = Kernel.GetText("sencillo");
            comboBoxAlbumType.Items.AddRange(types);
            comboBoxAlbumType.SelectedIndex = (int)albumAEditar.Type;

            if (Config.Language == "el")
            {
                Font but = buttonAñadirCancion.Font;
                Font neo = new Font(but.FontFamily, 7);
                buttonAñadirCancion.Font = neo;
            }
        }
        private void cargarVista()
        {
            vistaCanciones.Items.Clear();
            ListViewItem[] items = new ListViewItem[albumAEditar.NumberOfSongs];
            for (int i = 0; i < items.Length; i++)
            {
                items[i] = new ListViewItem(albumAEditar.Songs[i].Title);
            }
            vistaCanciones.Items.AddRange(items);
        }

        private void botonOkDoomer_Click(object sender, EventArgs e)
        {
            try//si está vacío pues guarda vacío
            {
                Log.Instance.PrintMessage("Trying to save", MessageType.Info);
                //Check if the title has been modified
                
                if(albumAEditar.Title != textBoxTitulo.Text)
                {
                    //Remove the album first from the collection
                    Kernel.Collection.Albums.Remove(albumAEditar.Key);
                    albumAEditar.Title = textBoxTitulo.Text;
                    Kernel.Collection.Albums.Add(albumAEditar.Key, albumAEditar);
                }
                albumAEditar.Artist = textBoxArtista.Text;
                albumAEditar.Year = Convert.ToInt16(textBoxAño.Text);
                string gn = comboBoxGeneros.SelectedItem.ToString();
                Genre g = Kernel.Genres[Kernel.FindTranslatedGenre(gn)];
                albumAEditar.Genre = g;
                albumAEditar.Type = (AlbumType)comboBoxAlbumType.SelectedIndex;
                albumAEditar.CoverPath = labelRuta.Text;
                TimeSpan nuevaDuracion = new TimeSpan();
                albumAEditar.SoundFilesPath = labelDirectorioActual.Text;
                string[] uriSpotify = textBoxURISpotify.Text.Split(':');
                if (uriSpotify.Length == 3)
                    albumAEditar.IdSpotify = (uriSpotify[2]);
                else
                    albumAEditar.IdSpotify = (textBoxURISpotify.Text);
                foreach (Song c in albumAEditar.Songs)
                {
                    if (!c.IsBonus)
                        nuevaDuracion += c.Length;
                }
            }
            catch (NullReferenceException)
            {
                Log.Instance.PrintMessage("Can't save the edit", MessageType.Warning);
                MessageBox.Show(Kernel.GetText("error_vacio1"));
            }

            catch (FormatException)
            {
                Log.Instance.PrintMessage("Wrong input, won't change anything", MessageType.Warning);
                MessageBox.Show(Kernel.GetText("error_formato"));
                //throw;
            }
            catch (IndexOutOfRangeException)
            {
                Log.Instance.PrintMessage("Wrong input, won't change anything", MessageType.Warning);
                MessageBox.Show(Kernel.GetText("error_formato"));
            }
            if (!fromMainView)
            {
                AlbumViewer nuevo = new AlbumViewer(ref albumAEditar);
                nuevo.Show();
            }
            Kernel.ReloadView();
            Close();
            Log.Instance.PrintMessage("Saved correctly", MessageType.Correct);
            Kernel.SetSaveMark();
        }

        private void botonCancelar_Click(object sender, EventArgs e)
        {
            if (!fromMainView)
            {
                AlbumViewer nuevo = new AlbumViewer(ref albumAEditar);
                nuevo.Show();
            }
            Close();
        }

        private void botonCaratula_Click(object sender, EventArgs e)
        {
            OpenFileDialog abrirImagen = new OpenFileDialog();
            abrirImagen.Filter = Kernel.GetText("archivo") + " .jpg, .png|*.jpg;*.png;*.jpeg";
            abrirImagen.InitialDirectory = albumAEditar.SoundFilesPath ?? Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            if (abrirImagen.ShowDialog() == DialogResult.OK)
            {
                string fichero = abrirImagen.FileName;
                labelRuta.Text = fichero;
            }
        }

        private void vistaCanciones_MouseDoubleClick(object sender, MouseEventArgs e) //editar cancion
        {
            Log.Instance.PrintMessage("Editing song", MessageType.Info);
            String text = vistaCanciones.SelectedItems[0].Text;
            Song cancionAEditar = albumAEditar.GetSong(text);
            CreateSong editarCancion = new CreateSong(ref cancionAEditar);
            editarCancion.ShowDialog();
            cargarVista();
            Log.Instance.PrintMessage("Saved!", MessageType.Correct);
        }

        private void buttonAñadirCancion_Click(object sender, EventArgs e)
        {
            Log.Instance.PrintMessage("Trying to add a song", MessageType.Info);
            CreateSong AC = new CreateSong(ref albumAEditar, -2);
            AC.ShowDialog();
            borrarVista();
            cargarVista();
        }
        private void borrarVista()
        {
            for (int i = 0; i < vistaCanciones.Items.Count; i++)
            {
                vistaCanciones.Clear();
            }
        }
        private void vistaCanciones_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                ListViewItem[] itemsborrar = new ListViewItem[vistaCanciones.SelectedItems.Count];
                int i = 0;
                foreach (ListViewItem item in vistaCanciones.SelectedItems)
                {
                    albumAEditar.RemoveSong(item.Text);
                    itemsborrar[i] = item;
                    i++;
                }
                foreach (var item in itemsborrar)
                {
                    vistaCanciones.Items.Remove(item);
                }
            }
        }

        private void buttonDirectorio_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialogCarpetaAlbum = new FolderBrowserDialog();
            dialogCarpetaAlbum.SelectedPath = Config.LastOpenedDirectory;
            DialogResult dr = dialogCarpetaAlbum.ShowDialog();
            if (dr == DialogResult.OK)
            {
                labelDirectorioActual.Text = Config.LastOpenedDirectory = dialogCarpetaAlbum.SelectedPath;
            }
        }
    }
}
