using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace aplicacion_musica.src.Forms
{
    public partial class ListaReproduccionUI : Form
    {
        private string Playing = "▶";
        private ListaReproduccion listaReproduccion;
        private int Puntero;
        public ListaReproduccionUI(ListaReproduccion lr)
        {
            InitializeComponent();
            listaReproduccion = lr;
            CargarVista();
            listViewCanciones.Size = Size;
            Puntero = 0;
            Text = lr.Nombre;
            PonerTextos();
        }
        private void PonerTextos()
        {
            nuevaToolStripMenuItem.Text = Programa.textosLocal.GetString("nuevaPlaylist");
            saveToolStripMenuItem.Text = Programa.textosLocal.GetString("guardar");
            addToolStripMenuItem.Text = Programa.textosLocal.GetString("añadir_cancion");
            listViewCanciones.Columns[0].Text = Programa.textosLocal.GetString("reproduciendo");
            listViewCanciones.Columns[1].Text = Programa.textosLocal.GetString("artista");
            listViewCanciones.Columns[2].Text = Programa.textosLocal.GetString("titulo");
            listViewCanciones.Columns[3].Text = Programa.textosLocal.GetString("duracion");
        }
        private void CargarVista()
        {
            listViewCanciones.Items.Clear();
            ListViewItem[] items = new ListViewItem[listaReproduccion.Canciones.Count];
            for (int i = 0; i < listaReproduccion.Canciones.Count; i++)
            {
                string[] data = new string[4];
                data[0] = "";
                //Coger los datos de la canción, si fuera necesario.
                if(string.IsNullOrEmpty(listaReproduccion.Canciones[i].Title))
                {
                    LectorMetadatos lectorMetadatos = new LectorMetadatos(listaReproduccion.Canciones[i].Path);
                    data[1] = lectorMetadatos.Artista;
                    data[2] = lectorMetadatos.Titulo;
                    data[3] = lectorMetadatos.Duracion.ToString();
                }
                else
                {
                    data[1] = listaReproduccion.Canciones[i].AlbumFrom.Artist;
                    data[2] = listaReproduccion.Canciones[i].Title;
                    data[3] = listaReproduccion.Canciones[i].Length.ToString();
                }
                items[i] = new ListViewItem(data);
            }
            listViewCanciones.Items.AddRange(items);
        }
        public void Refrescar()
        {
            CargarVista();
        }
        public void SetActivo(int punt)
        {
            if(punt != -1)
            {
                listViewCanciones.Items[Puntero].SubItems[0].Text = "";
                listViewCanciones.Items[punt].SubItems[0].Text = Playing;
                Puntero = punt;
            }
        }

        private void ListaReproduccionUI_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void ListaReproduccionUI_DragDrop(object sender, DragEventArgs e)
        {
            Song c = null;
            string[] canciones = null;
            if((c = (Song)e.Data.GetData(typeof(Song))) != null)
            {
                if(!string.IsNullOrEmpty(c.Path))
                {
                    listaReproduccion.AgregarCancion(c);
                }
            }
            else if ((canciones = (String[])e.Data.GetData(DataFormats.FileDrop)) != null) //El usuario arrastra desde el explorador.
            {
                foreach (string cancion in canciones)
                {
                    Song clr = new Song(cancion);
                    listaReproduccion.AgregarCancion(clr);
                    
                }
            }
            Refrescar();
            
            Reproductor.Instancia.ActivarPorLista();
        }

        private void listViewCanciones_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Reproductor.Instancia.ReproducirCancion(listViewCanciones.SelectedItems[0].Index);
        }

        private void ListaReproduccionUI_SizeChanged(object sender, EventArgs e)
        {
            listViewCanciones.Size = Size;

        }

        private void ListaReproduccionUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(e.CloseReason != CloseReason.ApplicationExitCall)
            {
                this.Hide();
                e.Cancel = true;
            }
        }

        private void ListaReproduccionUI_Load(object sender, EventArgs e)
        {

        }

        private void changeNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WriteName WriteNameForm = new WriteName();
            DialogResult Result = WriteNameForm.ShowDialog();
            if (Result == DialogResult.OK)
                listaReproduccion.Nombre = WriteNameForm.PlaylistName;
            Text = listaReproduccion.Nombre;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.CurrentDirectory;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {

            }

        }
    }
}
