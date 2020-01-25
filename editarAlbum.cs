using System;
using System.Windows.Forms;

namespace aplicacion_musica
{
    public partial class editarAlbum : Form
    {
        private Album albumAEditar;
        private string caratula;
        private string[] generosTraducidos = new string[Programa.generos.Length-1];
        public editarAlbum(ref Album a)
        {
            InitializeComponent();
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
            Text = Programa.textosLocal[27] + " " + albumAEditar.artista + " - " + albumAEditar.nombre;
            labelArtista.Text = Programa.textosLocal[4];
            labelTitulo.Text = Programa.textosLocal[5];
            labelAño.Text = Programa.textosLocal[6];
            labelGeneros.Text = Programa.textosLocal[8];
            labelCaratula.Text = Programa.textosLocal[26];
            botonOkDoomer.Text = Programa.textosLocal[21];
            botonCancelar.Text = Programa.textosLocal[11];
            botonCaratula.Text = Programa.textosLocal[25];
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
            foreach (Cancion cancion in albumAEditar.canciones)
            {
                vistaCanciones.Items.Add(cancion.titulo);
            }
        }

        private void botonOkDoomer_Click(object sender, EventArgs e)
        {
            try//si está vacío pues guarda vacío
            {
                albumAEditar.artista = textBoxArtista.Text;
                albumAEditar.nombre = textBoxTitulo.Text;
                albumAEditar.year = Convert.ToInt16(textBoxAño.Text);
                string gn = comboBoxGeneros.SelectedItem.ToString();
                Genero g = Programa.generos[Programa.findGeneroTraducido(gn)];
                albumAEditar.genero = g;
                albumAEditar.caratula = labelRuta.Text;
                TimeSpan nuevaDuracion = new TimeSpan();
                foreach (Cancion c in albumAEditar.canciones)
                {
                    nuevaDuracion += c.duracion;
                }
                albumAEditar.duracion = nuevaDuracion;
            }
            catch (NullReferenceException)
            {
                MessageBox.Show(Programa.textosLocal[23]);
            }

            catch (FormatException)
            {
                MessageBox.Show(Programa.textosLocal[22]);
                //throw;
            }
            visualizarAlbum nuevo = new visualizarAlbum(ref albumAEditar);
            nuevo.Show();
            Close();
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
            abrirImagen.Filter = Programa.textosLocal[1] + " .jpg, .png|*.jpg;*.png;*.jpeg";
            abrirImagen.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            if (abrirImagen.ShowDialog() == DialogResult.OK)
            {
                string fichero = abrirImagen.FileName;
                caratula = fichero;
                labelRuta.Text = fichero;
            }
        }

        private void vistaCanciones_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            agregarCancion editarCancion = new agregarCancion(ref albumAEditar.canciones[albumAEditar.buscarCancion(vistaCanciones.SelectedItems[0].Text)]);
            editarCancion.ShowDialog();
        }
    }
}
