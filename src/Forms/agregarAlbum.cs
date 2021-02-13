using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace aplicacion_musica
{
    public partial class agregarAlbum : Form
    {
        private string caratula = "";
        private String[] genresToSelect = new string[Programa.genres.Length-1];
        public agregarAlbum()
        {
            InitializeComponent();
            ponerTextos();
            Log.Instance.ImprimirMensaje("Creando álbum", TipoMensaje.Info);
        }
        private void ponerTextos()
        {
            Text = Programa.textosLocal.GetString("agregar_album");
            labelArtista.Text = Programa.textosLocal.GetString("artista");
            labelTitulo.Text = Programa.textosLocal.GetString("titulo");
            labelAño.Text = Programa.textosLocal.GetString("año");
            labelNumCanciones.Text = Programa.textosLocal.GetString("numcanciones");
            labelGenero.Text = Programa.textosLocal.GetString("genero");
            add.Text = Programa.textosLocal.GetString("añadir");
            addCaratula.Text = Programa.textosLocal.GetString("addcaratula");
            labelCaratula.Text = Programa.textosLocal.GetString("caratula");
            for (int i = 0; i < Programa.genres.Length-1; i++)
            {
                genresToSelect[i] = Programa.genres[i].Name;
            }
            Array.Sort(genresToSelect);
            comboBox1.Items.AddRange(genresToSelect);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Log.Instance.ImprimirMensaje("Buscando carátula", TipoMensaje.Info);
            OpenFileDialog abrirImagen = new OpenFileDialog();
            abrirImagen.Filter = Programa.textosLocal.GetString("archivo") + " .jpg, .png|*.jpg;*.png;*.jpeg";
            abrirImagen.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            if (abrirImagen.ShowDialog() == DialogResult.OK)
            {
                string fichero = abrirImagen.FileName;
                caratula = fichero;
                ruta.Text = fichero;
            }
            Log.Instance.ImprimirMensaje("Imagen " + ruta + " cargada", TipoMensaje.Correcto);
        }

        private void add_Click(object sender, EventArgs e)
        {
            string titulo, artista;
            bool cancelado = false;
            short year, nC;
            try
            {
                titulo = tituloTextBox.Text;
                artista = artistaTextBox.Text;
                int gn = comboBox1.SelectedIndex;
                string gent = comboBox1.SelectedItem.ToString();
                year = Convert.ToInt16(yearTextBox.Text);
                nC = Convert.ToInt16(numCancionesTextBox.Text);
                Genre g = Programa.genres[Programa.FindGeneroTraducido(gent)];
                Album a = null;
                if(caratula == "")
                    a = new Album(g, titulo, artista, year, "");
                else
                    a = new Album(g, titulo, artista, year, caratula);
                Programa.miColeccion.agregarAlbum(ref a);
                DialogResult cancelar = DialogResult.OK;
                for (int i = 0; i < nC; i++)
                {
                    agregarCancion agregarCancion = new agregarCancion(ref a,i);
                    Hide();
                    cancelar = agregarCancion.ShowDialog();
                    if (cancelar == DialogResult.Cancel)
                    {
                        Log.Instance.ImprimirMensaje("Cancelado el proceso de añadir álbum", TipoMensaje.Advertencia);
                        Programa.miColeccion.quitarAlbum(ref a);
                        Close();
                        cancelado = true;
                        break;
                    }
                    else if (cancelar == DialogResult.None)
                        continue;
                }
                if(!cancelado)
                    Log.Instance.ImprimirMensaje(artista + " - " + titulo + " agregado correctamente", TipoMensaje.Correcto);
                Programa.RefrescarVista();
                Close();
            }
            catch (NullReferenceException ex)
            {
                Log.Instance.ImprimirMensaje(ex.Message, TipoMensaje.Error);
                MessageBox.Show(Programa.textosLocal.GetString("error_vacio1"));
            }

            catch (FormatException ex)
            {
                Log.Instance.ImprimirMensaje(ex.Message, TipoMensaje.Error);
                MessageBox.Show(Programa.textosLocal.GetString("error_formato"));
                //throw;
            }

        }
    }
}
