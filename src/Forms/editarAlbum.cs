using System;
using System.Drawing;
using System.Windows.Forms;

namespace aplicacion_musica
{
    public partial class editarAlbum : Form
    {
        private AlbumData albumAEditar;
        private string[] generosTraducidos = new string[Programa.genres.Length-1];
        public editarAlbum(ref AlbumData a)
        {
            InitializeComponent();
            Console.WriteLine("Editando canción");
            albumAEditar = a;
            textBoxArtista.Text = albumAEditar.Artist;
            textBoxAño.Text = albumAEditar.Year.ToString();
            textBoxTitulo.Text = albumAEditar.Title;
            labelRuta.Text = albumAEditar.Cover;
            labelDirectorioActual.Text = albumAEditar.SoundFilesPath;
            textBoxURISpotify.Text = albumAEditar.IdSpotify;
            vistaCanciones.View = View.List;
            ponerTextos();
            cargarVista();

        }
        private void ponerTextos()
        {
            Text = Programa.textosLocal.GetString("editando") + " " + albumAEditar.Artist + " - " + albumAEditar.Title;
            labelArtista.Text = Programa.textosLocal.GetString("artista");
            labelTitulo.Text = Programa.textosLocal.GetString("titulo");
            labelAño.Text = Programa.textosLocal.GetString("año");
            labelGeneros.Text = Programa.textosLocal.GetString("genero");
            labelCaratula.Text = Programa.textosLocal.GetString("caratula");
            labelDirectorio.Text = Programa.textosLocal.GetString("directorio");
            labelURISpotify.Text = Programa.textosLocal.GetString("uriSpotify");
            botonOkDoomer.Text = Programa.textosLocal.GetString("hecho");
            botonCancelar.Text = Programa.textosLocal.GetString("cancelar");
            botonCaratula.Text = Programa.textosLocal.GetString("buscar");
            buttonAñadirCancion.Text = Programa.textosLocal.GetString("añadir_cancion");
            buttonDirectorio.Text = Programa.textosLocal.GetString("buscarDirectorio");
            labelDirectorioActual.Text = albumAEditar.SoundFilesPath;
            for (int i = 0; i < generosTraducidos.Length; i++)
            {
                generosTraducidos[i] = Programa.genres[i].Name;
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
            if(Config.Idioma == "el")
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
                items[i] = new ListViewItem(albumAEditar.Songs[i].titulo);
            }
            vistaCanciones.Items.AddRange(items);
        }

        private void botonOkDoomer_Click(object sender, EventArgs e)
        {
            try//si está vacío pues guarda vacío
            {
                Log.Instance.ImprimirMensaje("Intentando guardar", TipoMensaje.Info);
                albumAEditar.Artist = textBoxArtista.Text;
                albumAEditar.Title = textBoxTitulo.Text;
                albumAEditar.Year = Convert.ToInt16(textBoxAño.Text);
                string gn = comboBoxGeneros.SelectedItem.ToString();
                Genre g = Programa.genres[Programa.FindGeneroTraducido(gn)];
                albumAEditar.Genre = g;
                albumAEditar.Cover = labelRuta.Text;
                TimeSpan nuevaDuracion = new TimeSpan();
                albumAEditar.SoundFilesPath = labelDirectorioActual.Text;
                string[] uriSpotify = textBoxURISpotify.Text.Split(':');
                if(uriSpotify.Length == 3)
                    albumAEditar.IdSpotify = (uriSpotify[2]);
                else
                    albumAEditar.IdSpotify = (textBoxURISpotify.Text);
                foreach (Song c in albumAEditar.Songs)
                {
                    if(!c.Bonus)
                        nuevaDuracion += c.duracion;
                }
            }
            catch (NullReferenceException)
            {
                Log.Instance.ImprimirMensaje("Algún campo está vacío", TipoMensaje.Advertencia);
                MessageBox.Show(Programa.textosLocal.GetString("error_vacio1"));
            }

            catch (FormatException)
            {
                Log.Instance.ImprimirMensaje("Formato incorrecto, no se guardará nada.", TipoMensaje.Advertencia);
                MessageBox.Show(Programa.textosLocal.GetString("error_formato"));
                //throw;
            }
            catch (IndexOutOfRangeException)
            {
                Log.Instance.ImprimirMensaje("Formato incorrecto, no se guardará nada.", TipoMensaje.Advertencia);
                MessageBox.Show(Programa.textosLocal.GetString("error_formato"));
            }
            visualizarAlbum nuevo = new visualizarAlbum(ref albumAEditar);
            nuevo.Show();
            Programa.RefrescarVista();
            Close();
            Programa.RefrescarVista();
            Log.Instance.ImprimirMensaje("Guardado sin problema", TipoMensaje.Correcto);
        }

        private void botonCancelar_Click(object sender, EventArgs e)
        {
            visualizarAlbum nuevo = new visualizarAlbum(ref albumAEditar);
            nuevo.Show();
            Close();
        }

        private void botonCaratula_Click(object sender, EventArgs e)
        {
            OpenFileDialog abrirImagen = new OpenFileDialog();
            abrirImagen.Filter = Programa.textosLocal.GetString("archivo") + " .jpg, .png|*.jpg;*.png;*.jpeg";
            abrirImagen.InitialDirectory = albumAEditar.SoundFilesPath ?? Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            if (abrirImagen.ShowDialog() == DialogResult.OK)
            {
                string fichero = abrirImagen.FileName;
                labelRuta.Text = fichero;
            }
        }

        private void vistaCanciones_MouseDoubleClick(object sender, MouseEventArgs e) //editar cancion
        {
            Log.Instance.ImprimirMensaje("Editando canción", TipoMensaje.Info);
            String text = vistaCanciones.SelectedItems[0].Text;
            Song cancionAEditar = albumAEditar.DevolverCancion(text);
            agregarCancion editarCancion = new agregarCancion(ref cancionAEditar);
            editarCancion.ShowDialog();
            cargarVista();
            Log.Instance.ImprimirMensaje("Guardado correctamente", TipoMensaje.Correcto);
        }

        private void buttonAñadirCancion_Click(object sender, EventArgs e)
        {
            agregarCancion AC = new agregarCancion(ref albumAEditar, -2);
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
            if(e.KeyCode == Keys.Delete)
            {
                ListViewItem[] itemsborrar = new ListViewItem[vistaCanciones.SelectedItems.Count];
                int i = 0;
                foreach (ListViewItem item in vistaCanciones.SelectedItems)
                {
                    Song cancionABorrar = albumAEditar.DevolverCancion(item.Text);
                    albumAEditar.RemoveSong(cancionABorrar);
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
            dialogCarpetaAlbum.SelectedPath = Config.UltimoDirectorioAbierto;
            DialogResult dr = dialogCarpetaAlbum.ShowDialog();
            if (dr == DialogResult.OK)
            {
                labelDirectorioActual.Text = Config.UltimoDirectorioAbierto = dialogCarpetaAlbum.SelectedPath;
            }
        }
    }
}
