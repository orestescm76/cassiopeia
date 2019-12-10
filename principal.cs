using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Windows.Forms;

namespace aplicacion_ipo
{
    public partial class principal : Form
    {
        public principal()
        {
            InitializeComponent();
            ponerTextos();
            Application.ApplicationExit += new EventHandler(this.salidaAplicacion);
        }
        private void ponerTextos()
        {
            Text = Programa.textosLocal[0];
            archivoMenuItem1.Text = Programa.textosLocal[1];
            opcionesToolStripMenuItem.Text = Programa.textosLocal[2];
            agregarAlbumToolStripMenuItem.Text = Programa.textosLocal[3];
            salirToolStripMenuItem.Text = Programa.textosLocal.Last();
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void principal_Load(object sender, EventArgs e)
        {
            foreach (var idioma in Programa.codigosIdiomas)
            {
                ToolStripItem subIdioma = new ToolStripMenuItem(idioma);
                subIdioma.Click += new EventHandler(SubIdioma_Click);
                opcionesToolStripMenuItem.DropDownItems.Add(subIdioma);
            }
        }

        private void SubIdioma_Click(object sender, EventArgs e)
        {
            var menu = sender as ToolStripMenuItem;
            string codIdioma = menu.Text;
            Programa.cambiarIdioma(codIdioma);
            ponerTextos();
        }

        private void opciones2ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void agregarAlbumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            agregarAlbum agregarAlbum = new agregarAlbum();
            agregarAlbum.Show();
        }
        private void salidaAplicacion(object sender, EventArgs e)
        {
            using(StreamWriter salida = new StreamWriter("discos.mdb", false, System.Text.Encoding.UTF8))
            {
                foreach (Album a in Programa.miColeccion.albumes)
                {
                    salida.WriteLine(a.nombre + ";" + a.artista + ";" + a.year+";"+a.numCanciones+";"+a.genero+";"+a.caratula);
                    for (int i = 0; i < a.numCanciones; i++)
                    {
                        salida.WriteLine(a.canciones[i].titulo + ";" + a.canciones[i].duracion.Minutes + ";" + a.canciones[i].duracion.Seconds);
                    }
                    salida.WriteLine();
                }
            }

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
           
        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = Environment.CurrentDirectory;
            openFileDialog1.Filter = Programa.textosLocal[1]+ " .mdb (*.mdb)|*.mdb";
            if(openFileDialog1.ShowDialog()== DialogResult.OK)
            {
                string fichero = openFileDialog1.FileName;
                Programa.miColeccion.cargarAlbumes(fichero);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Album primero = Programa.miColeccion.albumes.First();
            label1.Text = primero.artista + " - " + primero.nombre + " - " + primero.year + " - " + primero.duracion.ToString(); //todo, meter genero en un numero y a partir de ahi traducir en cual sea, mas facil mas guay
        }
    }
}
