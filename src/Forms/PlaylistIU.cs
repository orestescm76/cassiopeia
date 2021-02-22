using System;
using System.Windows.Forms;

namespace aplicacion_musica.src.Forms
{
    public partial class PlaylistIU : Form
    {
        public Playlist Playlist { get; set; }
        private string Playing = "▶";
        private int Pointer;
        public PlaylistIU(Playlist lr)
        {
            InitializeComponent();
            Playlist = lr;
            LoadView();
            listViewSongs.Size = Size;
            Pointer = 0;
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
            toolStripMenuItemPlay.Text = Program.LocalTexts.GetString("reproducir");
            toolStripMenuItemRemove.Text = Program.LocalTexts.GetString("suprimir");
        }
        public void UpdateTime()
        {
            if (Playlist.Duration.TotalMinutes < 60)
                toolStripStatusLabelDuration.Text = Playlist.Duration.ToString(@"mm\:ss");
            else
                toolStripStatusLabelDuration.Text = Playlist.Duration.ToString(@"hh\:mm\:ss");
        }
        private Song[] GetSelectedSongs()
        {
            Song[] selectedSongs = new Song[listViewSongs.SelectedItems.Count];
            for (int i = 0; i < listViewSongs.SelectedIndices.Count; i++)
            {
                selectedSongs[i] = Playlist.GetCancion(listViewSongs.SelectedIndices[i]);
            }
            return selectedSongs;
        }
        private void LoadView()
        {
            listViewSongs.Items.Clear();
            ListViewItem[] items = new ListViewItem[Playlist.Canciones.Count];
            for (int i = 0; i < Playlist.Canciones.Count; i++)
            {
                string[] data = new string[4];
                data[0] = "";
                //Pick song data if it's mandatory
                if(string.IsNullOrEmpty(Playlist.Canciones[i].Title))
                {
                    LectorMetadatos lectorMetadatos = new LectorMetadatos(Playlist.Canciones[i].Path);
                    data[1] = lectorMetadatos.Artista;
                    data[2] = lectorMetadatos.Titulo;
                    data[3] = lectorMetadatos.Duracion.ToString(@"mm\:ss");
                    Playlist.Canciones[i].Length = lectorMetadatos.Duracion;
                }
                else
                {
                    data[1] = Playlist.Canciones[i].AlbumFrom.Artist;
                    data[2] = Playlist.Canciones[i].Title;
                    data[3] = Playlist.Canciones[i].Length.ToString();
                }
                items[i] = new ListViewItem(data);
            }
            listViewSongs.Items.AddRange(items);
            for (int i = 0; i < listViewSongs.Columns.Count; i++)
            {
                listViewSongs.Columns[i].Width = -2;
            }
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
                listViewSongs.Items[Pointer].SubItems[0].Text = "";
                listViewSongs.Items[punt].SubItems[0].Text = Playing;
                Pointer = punt;
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
                Playlist.Guardar(saveFileDialog.FileName);
                Log.Instance.ImprimirMensaje("Guardado correctamente", TipoMensaje.Correcto);
            }
        }
        //I don't care if the user has multiple songs selected, i play the first one.
        private void PlaySelectedSong()
        {
            SetActiveSong(listViewSongs.SelectedItems[0].Index);
            Reproductor.Instancia.ReproducirCancion(Playlist[Pointer]);
            Reproductor.Instancia.ListaReproduccionPuntero = Pointer;
        }
        //Gets the selected songs and removes them from the playlist
        private void RemoveSongs()
        {
            Log.Instance.ImprimirMensaje("Borrando canciones de la playlist", TipoMensaje.Info);
            Song[] selectedSongs = GetSelectedSongs();
            for (int i = 0; i < selectedSongs.Length; i++)
            {
                Playlist.DeleteSong(selectedSongs[i]);
                listViewSongs.SelectedItems[i].Remove();
            }
            Log.Instance.ImprimirMensaje("Borrado correcto", TipoMensaje.Correcto);
        }
        private void SetPlayVisibility(bool value)
        {
            toolStripMenuItemPlay.Visible = value;
            toolStripSeparator.Visible = value;
        }
        public void Stop()
        {
            Pointer = 0;
            SetActiveSong(Pointer);
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
                    Playlist.AgregarCancion(c);
                }
            }
            else if ((canciones = (String[])e.Data.GetData(DataFormats.FileDrop)) != null) //El usuario arrastra desde el explorador.
            {
                foreach (string songPath in canciones)
                {
                    Song clr = new Song();
                    clr.Path = songPath;
                    Playlist.AgregarCancion(clr);
                }
            }
            RefreshView();
            
            Reproductor.Instancia.ActivarPorLista();
        }

        private void listViewSongs_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            PlaySelectedSong();
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
                Playlist.Nombre = WriteNameForm.PlaylistName;
            Text = Playlist.Nombre;
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
                    Playlist.Cargar(openFileDialog.FileName);
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
                    Playlist.AgregarCancion(song);
                }
                RefreshView();
            }
        }
        //Called everytime the user selects a song in the UI.
        private void listViewSongs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewSongs.SelectedItems.Count != 0)
            {
                toolStripStatusLabelTracksSelected.Text = listViewSongs.SelectedItems.Count + " " + Program.LocalTexts.GetString("canciones");
                TimeSpan seleccion = new TimeSpan();
                foreach (ListViewItem songItem in listViewSongs.SelectedItems)
                {
                    Song song = Playlist[songItem.Index];
                    seleccion += song.Length;
                }
                if(seleccion.TotalMinutes < 60)
                    toolStripStatusLabelDuration.Text = seleccion.ToString(@"mm\:ss");
                else
                    toolStripStatusLabelDuration.Text = seleccion.ToString(@"hh\:mm\:ss");
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
                Pointer = 0;
                RefreshView();
            }
        }

        private void playToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PlaySelectedSong();
        }

        private void toolStripMenuItemRemove_Click(object sender, EventArgs e)
        {
            RemoveSongs();
        }

        private void PlaylistIU_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Delete:
                    RemoveSongs();
                    break;
                case Keys.Enter:
                    PlaySelectedSong();
                    break;
                default:
                    break;
            }
        }

        private void contextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (listViewSongs.SelectedItems.Count != 1)
                SetPlayVisibility(false);
            else
                SetPlayVisibility(true);
            if (listViewSongs.SelectedItems.Count == 0)
                e.Cancel = true;
        }

        private void toolStripMenuItemPlay_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabelInfo.Text = Program.LocalTexts.GetString("infoReproducir");
        }

        private void toolStripMenuItemRemove_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabelInfo.Text = Program.LocalTexts.GetString("infoBorrar");
        }

        private void contextMenuStrip_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {

        }
    }
}
