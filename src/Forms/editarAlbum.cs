using System;
using System.Windows.Forms;

namespace aplicacion_musica
{
    public partial class editarAlbum : Form
    {
        private Album albumAEditar;
        private string[] generosTraducidos = new string[Programa.generos.Length-1];
        public editarAlbum(ref Album a)
        {
            InitializeComponent();
            Console.WriteLine("Editando canción");
            albumAEditar = a;
            textBoxArtista.Text = albumAEditar.artista;
            textBoxAño.Text = albumAEditar.year.ToString();
            textBoxTitulo.Text = albumAEditar.nombre;
            labelRuta.Text = albumAEditar.caratula;
            vistaCanciones.View = View.List;
            ponerTextos();
            cargarVista();

        }
        private void ponerTextos()
        {
            Text = Programa.textosLocal.GetString("editando") + " " + albumAEditar.artista + " - " + albumAEditar.nombre;
            labelArtista.Text = Programa.textosLocal.GetString("artista");
            labelTitulo.Text = Programa.textosLocal.GetString("titulo");
            labelAño.Text = Programa.textosLocal.GetString("año");
            labelGeneros.Text = Programa.textosLocal.GetString("genero");
            labelCaratula.Text = Programa.textosLocal.GetString("caratula");
            botonOkDoomer.Text = Programa.textosLocal.GetString("hecho");
            botonCancelar.Text = Programa.textosLocal.GetString("cancelar");
            botonCaratula.Text = Programa.textosLocal.GetString("buscar");
            buttonAñadirCancion.Text = Programa.textosLocal.GetString("añadir_cancion");
            labelDirectorioActual.Text = albumAEditar.DirectorioSonido;
            for (int i = 0; i < generosTraducidos.Length; i++)
            {
                generosTraducidos[i] = Programa.generos[i].traducido;
            }
            Array.Sort(generosTraducidos);
            comboBoxGeneros.Items.AddRange(generosTraducidos);
            int index = 0;
            for (int i = 0; i < generosTraducidos.Length; i++)
            {
                if (albumAEditar.genero.traducido == generosTraducidos[i])
                    index = i;
            }
            comboBoxGeneros.SelectedIndex = index;
        }
        private void cargarVista()
        {
            ListViewItem[] items = new ListViewItem[albumAEditar.numCanciones];
            for (int i = 0; i < items.Length; i++)
            {
                items[i] = new ListViewItem(albumAEditar.canciones[i].titulo);
            }
            vistaCanciones.Items.AddRange(items);
        }

        private void botonOkDoomer_Click(object sender, EventArgs e)
        {
            try//si está vacío pues guarda vacío
            {
                Log.Instance.ImprimirMensaje("Intentando guardar", TipoMensaje.Info);
                albumAEditar.artista = textBoxArtista.Text;
                albumAEditar.nombre = textBoxTitulo.Text;
                albumAEditar.year = Convert.ToInt16(textBoxAño.Text);
                string gn = comboBoxGeneros.SelectedItem.ToString();
                Genero g = Programa.generos[Programa.findGeneroTraducido(gn)];
                albumAEditar.genero = g;
                albumAEditar.caratula = labelRuta.Text;
                TimeSpan nuevaDuracion = new TimeSpan();
                albumAEditar.DirectorioSonido = labelDirectorioActual.Text;
                foreach (Cancion c in albumAEditar.canciones)
                {
                    if(!c.Bonus)
                        nuevaDuracion += c.duracion;
                }
                albumAEditar.duracion = nuevaDuracion;
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
            visualizarAlbum nuevo = new visualizarAlbum(ref albumAEditar);
            cargarVista();
            nuevo.Show();
            Close();
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
            abrirImagen.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
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
            Cancion cancionAEditar = albumAEditar.DevolverCancion(text);
            agregarCancion editarCancion = new agregarCancion(ref cancionAEditar);
            editarCancion.ShowDialog();
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
                    Cancion cancionABorrar = albumAEditar.DevolverCancion(item.Text);
                    albumAEditar.BorrarCancion(cancionABorrar);
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
            dialogCarpetaAlbum.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            DialogResult dr = dialogCarpetaAlbum.ShowDialog();
            if (dr == DialogResult.OK)
            {
                labelDirectorioActual.Text = dialogCarpetaAlbum.SelectedPath;
            }
        }
    }
}
