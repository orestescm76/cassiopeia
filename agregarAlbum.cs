using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace aplicacion_ipo
{
    public partial class agregarAlbum : Form
    {
        public string titulo;
        public int min, sec;
        private String[] generosTraducidos = new string[Programa.generos.Length];
        public agregarAlbum()
        {
            InitializeComponent();
            ponerTextos();

        }
        private void ponerTextos()
        {
            this.Text = Programa.textosLocal[3];
            labelArtista.Text = Programa.textosLocal[4];
            labelTitulo.Text = Programa.textosLocal[5];
            labelAño.Text = Programa.textosLocal[6];
            labelNumCanciones.Text = Programa.textosLocal[7];
            labelGenero.Text = Programa.textosLocal[8];
            add.Text = Programa.textosLocal[9];
            int c = 0;
            for (int i = 10; i < Programa.generos.Length+10; i++)
            {
                generosTraducidos[c] = Programa.textosLocal[i];
                c++;
            }
            comboBox1.Items.AddRange(generosTraducidos);

        }
        private void agregarAlbum_Load(object sender, EventArgs e)
        {

        }

        private void numCancionesTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void labelNumCanciones_Click(object sender, EventArgs e)
        {

        }

        private void add_Click(object sender, EventArgs e)
        {
            string titulo, artista;
            short year, nC;
            titulo = tituloTextBox.Text;
            artista = artistaTextBox.Text;
            int gn = comboBox1.SelectedIndex;
            string genero = Programa.generos[gn];
            try
            {
                year = Convert.ToInt16(yearTextBox.Text);
                nC = Convert.ToInt16(numCancionesTextBox.Text);
                Album a = new Album(titulo, artista, year, nC,genero, "");
                Programa.miColeccion.agregarAlbum(ref a);
                for (int i = 0; i < nC; i++)
                {
                    agregarCancion agregarCancion = new agregarCancion(ref a,i);
                    agregarCancion.ShowDialog();
                }
            }
            catch (FormatException ex)
            {
                MessageBox.Show(ex.ToString());
                //throw;
            }
            Close();
        }
    }
}
