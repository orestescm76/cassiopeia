using System;
using System.Windows.Forms;

namespace Cassiopeia.src.Forms
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
            Text = lr.Name;
            PutTexts();
            toolStripStatusLabelInfo.Width = statusStrip.Size.Width - 200;
            Icon = Properties.Resources.playlist;
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
            if (Playlist.Length.TotalMinutes < 60)
                toolStripStatusLabelDuration.Text = Playlist.Length.ToString(@"mm\:ss");
            else
                toolStripStatusLabelDuration.Text = Playlist.Length.ToString(@"hh\:mm\:ss");
        }
        private Song[] GetSelectedSongs()
        {
            Song[] selectedSongs = new Song[listViewSongs.SelectedItems.Count];
            for (int i = 0; i < listViewSongs.SelectedIndices.Count; i++)
            {
                selectedSongs[i] = Playlist.GetSong(listViewSongs.SelectedIndices[i]);
            }
            return selectedSongs;
        }
        private void LoadView()
        {
            listViewSongs.Items.Clear();
            ListViewItem[] items = new ListViewItem[Playlist.Songs.Count];
            for (int i = 0; i < Playlist.Songs.Count; i++)
            {
                string[] data = new string[4];
                data[0] = "";
                //We can be playing a CD!
                if(Reproductor.Instancia.ModoCD)
                {
                    data[1] = "";
                    data[2] = "";
                    data[3] = Playlist.Songs[i].Length.ToString();
                    items[i] = new ListViewItem(data);
                    continue; //don't check anything else
                }
                //Pick song data if it's mandatory
                if(string.IsNullOrEmpty(Playlist.Songs[i].Title))
                {
                    MetadataSong lectorMetadatos = new MetadataSong(Playlist.Songs[i].Path);
                    data[1] = lectorMetadatos.Artist;
                    data[2] = lectorMetadatos.Title;
                    data[3] = lectorMetadatos.Length.ToString(@"mm\:ss");
                    Playlist.Songs[i].Length = lectorMetadatos.Length;
                }
                else
                {
                    data[1] = Playlist.Songs[i].AlbumFrom.Artist;
                    data[2] = Playlist.Songs[i].Title;
                    data[3] = Playlist.Songs[i].Length.ToString();
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
                listViewSongs.Items[punt].SubItems[0].Text = Playing;
                Pointer = punt;
                listViewSongs.Items[Pointer].SubItems[0].Text = "";
                
            }
            else
            {
                foreach (ListViewItem item in listViewSongs.Items)
                {
                    item.SubItems[0].Text = "";
                }
            }
        }
        private void SavePlaylist()
        {
            Log.Instance.PrintMessage("Guardando playlist", MessageType.Info);
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.CurrentDirectory;
            saveFileDialog.Filter = ".plf|*.plf";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Playlist.Save(saveFileDialog.FileName);
                Log.Instance.PrintMessage("Guardado correctamente", MessageType.Correct);
            }
        }
        //I don't care if the user has multiple songs selected, i play the first one.
        private void PlaySelectedSong()
        {
            SetActiveSong(listViewSongs.SelectedItems[0].Index);
            //Are we playing a CD?
            if (Reproductor.Instancia.ModoCD)
                Reproductor.Instancia.ReproducirCancion(Pointer);

            else
                Reproductor.Instancia.ReproducirCancion(Playlist[Pointer]);
            
            Reproductor.Instancia.ListaReproduccionPuntero = Pointer;
        }
        //Gets the selected songs and removes them from the playlist
        private void RemoveSongs()
        {
            Log.Instance.PrintMessage("Borrando canciones de la playlist", MessageType.Info);
            Song[] selectedSongs = GetSelectedSongs();
            for (int i = 0; i < selectedSongs.Length; i++)
            {
                Playlist.RemoveSong(selectedSongs[i]);
                listViewSongs.SelectedItems[i].Remove();
            }
            Log.Instance.PrintMessage("Borrado correcto", MessageType.Correct);
        }
        private void SetPlayVisibility(bool value)
        {
            toolStripMenuItemPlay.Visible = value;
            toolStripSeparator.Visible = value;
        }
        public void Stop()
        {
            Pointer = -1;
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
                    Playlist.AddSong(c);
                }
            }
            else if ((canciones = (String[])e.Data.GetData(DataFormats.FileDrop)) != null) //El usuario arrastra desde el explorador.
            {
                foreach (string songPath in canciones)
                {
                    Song clr = new Song();
                    clr.Path = songPath;
                    Playlist.AddSong(clr);
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
                Playlist.Name = WriteNameForm.PlaylistName;
            Text = Playlist.Name;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SavePlaylist();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.PrintMessage("Abriendo playlist", MessageType.Info);
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.CurrentDirectory;
            openFileDialog.Filter = ".plf|*.plf";
            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Playlist.Load(openFileDialog.FileName);
                    Log.Instance.PrintMessage("Abierto correctamente", MessageType.Correct);
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
            openFileDialog.InitialDirectory = Config.LastOpenedDirectory;
            openFileDialog.Filter = ".mp3, .ogg, .flac|*.mp3; *.ogg; *.flac";
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (var songFile in openFileDialog.FileNames)
                {
                    Song song = new Song();
                    song.Path = songFile;
                    Playlist.AddSong(song);
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
