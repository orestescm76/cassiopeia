using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace aplicacion_musica
{
    public partial class agregarAlbum : Form
    {
        private string caratula = "";
        private String[] genresToSelect = new string[Program.Genres.Length-1];
        public agregarAlbum()
        {
            InitializeComponent();
            ponerTextos();
            Log.Instance.ImprimirMensaje("Creando álbum", TipoMensaje.Info);
        }
        private void ponerTextos()
        {
            Text = Program.LocalTexts.GetString("agregar_album");
            labelArtista.Text = Program.LocalTexts.GetString("artista");
            labelTitulo.Text = Program.LocalTexts.GetString("titulo");
            labelAño.Text = Program.LocalTexts.GetString("año");
            labelNumCanciones.Text = Program.LocalTexts.GetString("numcanciones");
            labelGenero.Text = Program.LocalTexts.GetString("genero");
            add.Text = Program.LocalTexts.GetString("añadir");
            addCaratula.Text = Program.LocalTexts.GetString("addcaratula");
            labelCaratula.Text = Program.LocalTexts.GetString("caratula");
            for (int i = 0; i < Program.Genres.Length-1; i++)
            {
                genresToSelect[i] = Program.Genres[i].Name;
            }
            Array.Sort(genresToSelect);
            comboBox1.Items.AddRange(genresToSelect);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Log.Instance.ImprimirMensaje("Buscando carátula", TipoMensaje.Info);
            OpenFileDialog abrirImagen = new OpenFileDialog();
            abrirImagen.Filter = Program.LocalTexts.GetString("archivo") + " .jpg, .png|*.jpg;*.png;*.jpeg";
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
                Genre g = Program.Genres[Program.FindTranslatedGenre(gent)];
                AlbumData a = null;
                if(caratula == "")
                    a = new AlbumData(g, titulo, artista, year, "");
                else
                    a = new AlbumData(g, titulo, artista, year, caratula);
                Program.Collection.AddAlbum(ref a);
                DialogResult cancelar = DialogResult.OK;
                for (int i = 0; i < nC; i++)
                {
                    agregarCancion agregarCancion = new agregarCancion(ref a,i);
                    Hide();
                    cancelar = agregarCancion.ShowDialog();
                    if (cancelar == DialogResult.Cancel)
                    {
                        Log.Instance.ImprimirMensaje("Cancelado el proceso de añadir álbum", TipoMensaje.Advertencia);
                        Program.Collection.RemoveAlbum(ref a);
                        Close();
                        cancelado = true;
                        break;
                    }
                    else if (cancelar == DialogResult.None)
                        continue;
                }
                if(!cancelado)
                    Log.Instance.ImprimirMensaje(artista + " - " + titulo + " agregado correctamente", TipoMensaje.Correcto);
                Program.ReloadView();
                Close();
            }
            catch (NullReferenceException ex)
            {
                Log.Instance.ImprimirMensaje(ex.Message, TipoMensaje.Error);
                MessageBox.Show(Program.LocalTexts.GetString("error_vacio1"));
            }

            catch (FormatException ex)
            {
                Log.Instance.ImprimirMensaje(ex.Message, TipoMensaje.Error);
                MessageBox.Show(Program.LocalTexts.GetString("error_formato"));
                //throw;
            }

        }
    }
}
