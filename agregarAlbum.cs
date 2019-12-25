using System;
using System.Windows.Forms;

namespace aplicacion_ipo
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
            Text = Programa.textosLocal[3];
            labelArtista.Text = Programa.textosLocal[4];
            labelTitulo.Text = Programa.textosLocal[5];
            labelAño.Text = Programa.textosLocal[6];
            labelNumCanciones.Text = Programa.textosLocal[7];
            labelGenero.Text = Programa.textosLocal[8];
            add.Text = Programa.textosLocal[9];
            addCaratula.Text = Programa.textosLocal[25];
            labelCaratula.Text = Programa.textosLocal[24];
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
            abrirImagen.Filter = Programa.textosLocal[1] + " .jpg, .png|*.jpg;*.png";
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
                    if (cancelar == DialogResult.Cancel)
                    {
                        Programa.miColeccion.quitarAlbum(ref a);
                        Close();
                        break;
                    }
                    agregarCancion agregarCancion = new agregarCancion(ref a,i);
                    cancelar = agregarCancion.ShowDialog();
                }

                Close();
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

        }
    }
}
