using System;
using System.Windows.Forms;

namespace aplicacion_musica.src.Forms
{
    public partial class PlaylistIU : Form
    {
        public Playlist ListaReproduccion { get; set; }
        private string Playing = "▶";
        private int Puntero;
        public PlaylistIU(Playlist lr)
        {
            InitializeComponent();
            ListaReproduccion = lr;
            LoadView();
            listViewSongs.Size = Size;
            Puntero = 0;
            Text = lr.Nombre;
            PutTexts();
            toolStripStatusLabelInfo.Width = statusStrip.Size.Width - 200;
        }
        private void PutTexts()
        {
            fileToolStripMenuItem.Text = Program.LocalTexts.GetString("archivo");
            newToolStripMenuItem.Text = Program.LocalTexts.GetString("nuevaPlaylist");
            saveToolStripMenuItem.Text = Program.LocalTexts.GetString("guardar");
            openToolStripMenuItem.Text = Program.LocalTexts.GetString("abrir");
            addSongToolStripMenuItem.Text = Program.LocalTexts.GetString("añadir_cancion");
            changeNameToolStripMenuItem.Text = Program.LocalTexts.GetString("cambiarNombrePL");
            addSongToolStripMenuItem.Text = Program.LocalTexts.GetString("añadir_cancion");
            listViewSongs.Columns[0].Text = Program.LocalTexts.GetString("reproduciendo");
            listViewSongs.Columns[1].Text = Program.LocalTexts.GetString("artista");
            listViewSongs.Columns[2].Text = Program.LocalTexts.GetString("titulo");
            listViewSongs.Columns[3].Text = Program.LocalTexts.GetString("duracion");
            toolStripStatusLabelTracksSelected.Text = "0 " + Program.LocalTexts.GetString("canciones");
            toolStripStatusLabelDuration.Text = "0:00";
        }
        public void UpdateTime()
        {
            if (ListaReproduccion.Duration.TotalMinutes < 60)
                toolStripStatusLabelDuration.Text = Program.LocalTexts.GetString("duracion") + ": " + ListaReproduccion.Duration.ToString(@"mm\:ss");
            else
                toolStripStatusLabelDuration.Text = Program.LocalTexts.GetString("duracion") + ": " + ListaReproduccion.Duration.ToString(@"hh\:mm\:ss");
        }
        private void LoadView()
        {
            listViewSongs.Items.Clear();
            ListViewItem[] items = new ListViewItem[ListaReproduccion.Canciones.Count];
            for (int i = 0; i < ListaReproduccion.Canciones.Count; i++)
            {
                string[] data = new string[4];
                data[0] = "";
                //Pick song data if it's mandatory
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
            listViewSongs.Items.AddRange(items);
            UpdateTime();
        }
        public void RefreshView()
        {
            LoadView();
        }
        public void SetActiveSong(int punt)
        {
            if(punt != -1)
            {
                listViewSongs.Items[Puntero].SubItems[0].Text = "";
                listViewSongs.Items[punt].SubItems[0].Text = Playing;
                Puntero = punt;
            }
        }
        private void SavePlaylist()
        {
            Log.Instance.ImprimirMensaje("Guardando playlist", TipoMensaje.Info);
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.CurrentDirectory;
            saveFileDialog.Filter = ".plf|*.plf";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                ListaReproduccion.Guardar(saveFileDialog.FileName);
                Log.Instance.ImprimirMensaje("Guardado correctamente", TipoMensaje.Correcto);
            }
        }
        private void PlaylistIU_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void PlaylistIU_DragDrop(object sender, DragEventArgs e)
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
            RefreshView();
            
            Reproductor.Instancia.ActivarPorLista();
        }

        private void listViewSongs_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Reproductor.Instancia.ReproducirCancion(listViewSongs.SelectedItems[0].Index);
        }

        private void PlaylistIU_SizeChanged(object sender, EventArgs e)
        {
            listViewSongs.Size = Size;
            toolStripStatusLabelInfo.Width = statusStrip.Size.Width - 200;
        }

        private void PlaylistIU_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(e.CloseReason != CloseReason.ApplicationExitCall)
            {
                Hide();
                e.Cancel = true;
            }
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
            SavePlaylist();
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
            RefreshView();
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
                RefreshView();
            }
        }
        //Called everytime the user selects a song in the UI.
        private void listViewSongs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewSongs.SelectedItems.Count != 0)
            {
                TimeSpan seleccion = new TimeSpan();
                foreach (ListViewItem songItem in listViewSongs.SelectedItems)
                {
                    Song song = ListaReproduccion[songItem.Index];
                    seleccion += song.Length;
                }
                if(seleccion.TotalMinutes < 60)
                    toolStripStatusLabelDuration.Text = Program.LocalTexts.GetString("duracion") + ": " + seleccion.ToString(@"mm\:ss");
                else
                    toolStripStatusLabelDuration.Text = Program.LocalTexts.GetString("duracion") + ": " + seleccion.ToString(@"hh\:mm\:ss");
                toolStripStatusLabelTracksSelected.Text = listViewSongs.SelectedItems.Count + " " + Program.LocalTexts.GetString("canciones");
            }
            else
            {
                UpdateTime();
                toolStripStatusLabelTracksSelected.Text = "0 " + Program.LocalTexts.GetString("canciones");
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Creates a new playlist and sends it to the Player.
            DialogResult result = MessageBox.Show(Program.LocalTexts.GetString("guardarPL"), "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            switch (result)
            {
                case DialogResult.Yes:
                    SavePlaylist();
                    break;
                case DialogResult.No:
                    break;
                default:
                    break;
            }
            if(result != DialogResult.Yes) //for some reason, the user cancels at the save dialog...
            {
                Reproductor.Instancia.CreatePlaylist(Program.LocalTexts.GetString("nuevaPlaylist"));
                Puntero = 0;
                RefreshView();
            }
        }
    }
}
