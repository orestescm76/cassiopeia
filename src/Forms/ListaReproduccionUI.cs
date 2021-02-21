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
        public ListaReproduccion ListaReproduccion { get; set; }
        private string Playing = "▶";
        private int Puntero;
        public ListaReproduccionUI(ListaReproduccion lr)
        {
            InitializeComponent();
            ListaReproduccion = lr;
            CargarVista();
            listViewCanciones.Size = Size;
            Puntero = 0;
            Text = lr.Nombre;
            PonerTextos();
        }
        private void PonerTextos()
        {
            fileToolStripMenuItem.Text = Program.LocalTexts.GetString("archivo");
            nuevaToolStripMenuItem.Text = Program.LocalTexts.GetString("nuevaPlaylist");
            saveToolStripMenuItem.Text = Program.LocalTexts.GetString("guardar");
            openToolStripMenuItem.Text = Program.LocalTexts.GetString("abrir");
            addSongToolStripMenuItem.Text = Program.LocalTexts.GetString("añadir_cancion");
            changeNameToolStripMenuItem.Text = Program.LocalTexts.GetString("cambiarNombrePL");
            addSongToolStripMenuItem.Text = Program.LocalTexts.GetString("añadir_cancion");
            listViewCanciones.Columns[0].Text = Program.LocalTexts.GetString("reproduciendo");
            listViewCanciones.Columns[1].Text = Program.LocalTexts.GetString("artista");
            listViewCanciones.Columns[2].Text = Program.LocalTexts.GetString("titulo");
            listViewCanciones.Columns[3].Text = Program.LocalTexts.GetString("duracion");
        }
        public void UpdateTime()
        {
            if (ListaReproduccion.Duration.TotalMinutes < 60)
                toolStripStatusLabelDuration.Text = Program.LocalTexts.GetString("duracion") + ": " + ListaReproduccion.Duration.ToString(@"mm\:ss");
            else
                toolStripStatusLabelDuration.Text = Program.LocalTexts.GetString("duracion") + ": " + ListaReproduccion.Duration.ToString(@"hh\:mm\:ss");
        }
        private void CargarVista()
        {
            listViewCanciones.Items.Clear();
            ListViewItem[] items = new ListViewItem[ListaReproduccion.Canciones.Count];
            for (int i = 0; i < ListaReproduccion.Canciones.Count; i++)
            {
                string[] data = new string[4];
                data[0] = "";
                //Coger los datos de la canción, si fuera necesario.
                if(string.IsNullOrEmpty(ListaReproduccion.Canciones[i].Title))
                {
                    LectorMetadatos lectorMetadatos = new LectorMetadatos(ListaReproduccion.Canciones[i].Path);
                    data[1] = lectorMetadatos.Artista;
                    data[2] = lectorMetadatos.Titulo;
                    data[3] = lectorMetadatos.Duracion.ToString(@"mm\:ss");
                    ListaReproduccion.Canciones[i].Length = lectorMetadatos.Duracion;
                }
                else
                {
                    data[1] = ListaReproduccion.Canciones[i].AlbumFrom.Artist;
                    data[2] = ListaReproduccion.Canciones[i].Title;
                    data[3] = ListaReproduccion.Canciones[i].Length.ToString();
                }
                items[i] = new ListViewItem(data);
            }
            listViewCanciones.Items.AddRange(items);
            UpdateTime();
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
                    ListaReproduccion.AgregarCancion(c);
                }
            }
            else if ((canciones = (String[])e.Data.GetData(DataFormats.FileDrop)) != null) //El usuario arrastra desde el explorador.
            {
                foreach (string songPath in canciones)
                {
                    Song clr = new Song();
                    clr.Path = songPath;
                    ListaReproduccion.AgregarCancion(clr);
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
                Hide();
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
                ListaReproduccion.Nombre = WriteNameForm.PlaylistName;
            Text = ListaReproduccion.Nombre;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.ImprimirMensaje("Guardando playlist",TipoMensaje.Info);
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.CurrentDirectory;
            saveFileDialog.Filter = ".plf|*.plf";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                ListaReproduccion.Guardar(saveFileDialog.FileName);
                Log.Instance.ImprimirMensaje("Guardado correctamente", TipoMensaje.Correcto);
            }
            
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.ImprimirMensaje("Abriendo playlist", TipoMensaje.Info);
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.CurrentDirectory;
            openFileDialog.Filter = ".plf|*.plf";
            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ListaReproduccion.Cargar(openFileDialog.FileName);
                    Log.Instance.ImprimirMensaje("Abierto correctamente", TipoMensaje.Correcto);
                }
                catch (Exception)
                {
                    MessageBox.Show(Program.LocalTexts.GetString("errrorLR"), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            Refrescar();
        }

        private void addSongToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Config.UltimoDirectorioAbierto;
            openFileDialog.Filter = ".mp3, .ogg, .flac|*.mp3; *.ogg; *.flac";
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (var songFile in openFileDialog.FileNames)
                {
                    Song song = new Song();
                    song.Path = songFile;
                    ListaReproduccion.AgregarCancion(song);
                }
                Refrescar();
            }
        }
        //Called everytime the user selects a song in the UI.
        private void listViewCanciones_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewCanciones.SelectedItems.Count != 0)
            {
                TimeSpan seleccion = new TimeSpan();
                foreach (ListViewItem songItem in listViewCanciones.SelectedItems)
                {
                    Song song = ListaReproduccion[songItem.Index];
                    seleccion += song.Length;
                }
                if(seleccion.TotalMinutes < 60)
                    toolStripStatusLabelDuration.Text = Program.LocalTexts.GetString("duracion") + ": " + seleccion.ToString(@"mm\:ss");
                else
                    toolStripStatusLabelDuration.Text = Program.LocalTexts.GetString("duracion") + ": " + seleccion.ToString(@"hh\:mm\:ss");
            }
            else
                UpdateTime();
        }
    }
}
