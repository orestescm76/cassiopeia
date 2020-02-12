using System;
using System.Windows.Forms;

namespace aplicacion_musica
{
    public partial class agregarAlbum : Form
    {
        private string titulo;
        private string caratula = "";
        private int min, sec;
        private String[] generosTraducidos = new string[Programa.generos.Length-1];
        public agregarAlbum()
        {
            InitializeComponent();
            ponerTextos();

        }
        private void ponerTextos()
        {
            Text = Programa.textosLocal.GetString("agregar_album");
            labelArtista.Text = Programa.textosLocal.GetString("artista");
            labelTitulo.Text = Programa.textosLocal.GetString("titulo");
            labelAño.Text = Programa.textosLocal.GetString("año");
            labelNumCanciones.Text = Programa.textosLocal.GetString("num_canciones");
            labelGenero.Text = Programa.textosLocal.GetString("genero");
            add.Text = Programa.textosLocal.GetString("añadir");
            addCaratula.Text = Programa.textosLocal.GetString("addcaratula");
            labelCaratula.Text = Programa.textosLocal.GetString("caratula");
                //TODO rediseñar sistema generos, ponerlo como ultimos string.
            for (int i = 0; i < Programa.generos.Length-1; i++)
            {
                generosTraducidos[i] = Programa.generos[i].traducido;
            }
            Array.Sort(generosTraducidos);
            comboBox1.Items.AddRange(generosTraducidos);
            
        }
        private void agregarAlbum_Load(object sender, EventArgs e)
        {

        }

        private void numCancionesTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog abrirImagen = new OpenFileDialog();
            abrirImagen.Filter = Programa.textosLocal.GetString("archivo") + " .jpg, .png|*.jpg;*.png;*.jpeg";
            abrirImagen.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            if (abrirImagen.ShowDialog() == DialogResult.OK)
            {
                string fichero = abrirImagen.FileName;
                caratula = fichero;
                ruta.Text = fichero;
            }
        }

        private void add_Click(object sender, EventArgs e)
        {
            string titulo, artista;
            short year, nC;


            try
            {
                titulo = tituloTextBox.Text;
                artista = artistaTextBox.Text;
                int gn = comboBox1.SelectedIndex;
                string gent = comboBox1.SelectedItem.ToString();
                year = Convert.ToInt16(yearTextBox.Text);
                nC = Convert.ToInt16(numCancionesTextBox.Text);
                Genero g = Programa.generos[Programa.findGeneroTraducido(gent)];
                Album a = null;
                if(caratula == "")
                    a = new Album(g, titulo, artista, year, nC, "");
                else
                    a = new Album(g, titulo, artista, year, nC, caratula);
                Programa.miColeccion.agregarAlbum(ref a);
                DialogResult cancelar = DialogResult.OK;
                for (int i = 0; i < nC; i++)
                {

                    agregarCancion agregarCancion = new agregarCancion(ref a,i);
                    Hide();
                    cancelar = agregarCancion.ShowDialog();
                    if (cancelar == DialogResult.Cancel)
                    {
                        Programa.miColeccion.quitarAlbum(ref a);
                        Close();
                        break;
                    }
                    else if (cancelar == DialogResult.None)
                        continue;
                }

                Close();
            }
            catch (NullReferenceException)
            {
                MessageBox.Show(Programa.textosLocal.GetString("error_vacio1"));
            }

            catch (FormatException)
            {
                MessageBox.Show(Programa.textosLocal.GetString("error_formato"));
                //throw;
            }

        }
    }
}
