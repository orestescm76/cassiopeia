using Cassiopeia.src.Classes;
using System;
using System.Diagnostics;
using System.IO;
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
            fileToolStripMenuItem.Text = Kernel.LocalTexts.GetString("archivo");
            newToolStripMenuItem.Text = Kernel.LocalTexts.GetString("nuevaPlaylist");
            saveToolStripMenuItem.Text = Kernel.LocalTexts.GetString("guardar");
            openToolStripMenuItem.Text = Kernel.LocalTexts.GetString("abrir");
            addSongToolStripMenuItem.Text = Kernel.LocalTexts.GetString("añadir_cancion");
            changeNameToolStripMenuItem.Text = Kernel.LocalTexts.GetString("cambiarNombrePL");
            addSongToolStripMenuItem.Text = Kernel.LocalTexts.GetString("añadir_cancion");
            listViewSongs.Columns[0].Text = Kernel.LocalTexts.GetString("reproduciendo");
            listViewSongs.Columns[1].Text = Kernel.LocalTexts.GetString("artista");
            listViewSongs.Columns[2].Text = Kernel.LocalTexts.GetString("titulo");
            listViewSongs.Columns[3].Text = Kernel.LocalTexts.GetString("duracion");
            toolStripStatusLabelTracksSelected.Text = "0 " + Kernel.LocalTexts.GetString("canciones");
            toolStripStatusLabelDuration.Text = "0:00";
            toolStripMenuItemPlay.Text = Kernel.LocalTexts.GetString("reproducir");
            toolStripMenuItemRemove.Text = Kernel.LocalTexts.GetString("suprimir");
            toolStripMenuItemOpenFolder.Text = Kernel.LocalTexts.GetString("openFolder");
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
        private string GetSongTime(TimeSpan time)
        {
            if (time.TotalMinutes >= 60)
                return time.ToString(@"hh\:mm\:ss");
            else
                return time.ToString(@"mm\:ss");
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
                if (Player.Instancia.ModoCD)
                {
                    data[1] = "";
                    data[2] = "";
                    data[3] = Playlist.Songs[i].Length.ToString();
                    items[i] = new ListViewItem(data);
                    continue; //don't check anything else
                }
                //Pick song data if it's mandatory
                if (string.IsNullOrEmpty(Playlist.Songs[i].Title))
                {
                    MetadataSong lectorMetadatos = new MetadataSong(Playlist.Songs[i].Path);
                    data[1] = lectorMetadatos.Artist;
                    data[2] = lectorMetadatos.Title;
                    data[3] = GetSongTime(lectorMetadatos.Length);
                    Playlist.Songs[i].Length = lectorMetadatos.Length;
                }
                else
                {
                    data[1] = Playlist.Songs[i].AlbumFrom.Artist;
                    data[2] = Playlist.Songs[i].Title;
                    data[3] = GetSongTime(Playlist.Songs[i].Length);
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
            if (punt != -1)
            {
                //Get the last one and set playing tick
                int aux = Pointer;
                listViewSongs.Items[punt].SubItems[0].Text = Playing;
                Pointer = punt;
                //If we changed song, clean it
                if (aux != Pointer)
                    listViewSongs.Items[aux].SubItems[0].Text = "";
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
            Log.Instance.PrintMessage("Saving playlist", MessageType.Info);
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.CurrentDirectory;
            saveFileDialog.Filter = ".plf|*.plf";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Playlist.Save(saveFileDialog.FileName);
                Log.Instance.PrintMessage("Saved", MessageType.Correct);
            }
        }
        //I don't care if the user has multiple songs selected, i play the first one.
        private void PlaySelectedSong()
        {
            SetActiveSong(listViewSongs.SelectedItems[0].Index);
            //Are we playing a CD?
            if (Player.Instancia.ModoCD)
                Player.Instancia.PlaySong(Pointer);
            else
                Player.Instancia.PlaySong(Playlist[Pointer]);

            Player.Instancia.ListaReproduccionPuntero = Pointer;
        }
        //Gets the selected songs and removes them from the playlist
        private void RemoveSongs()
        {
            Log.Instance.PrintMessage("Deleting songs off the playlist", MessageType.Info);
            Song[] selectedSongs = GetSelectedSongs();
            listViewSongs.SelectedItems.Clear();
            for (int i = 0; i < selectedSongs.Length; i++)
            {
                Playlist.RemoveSong(selectedSongs[i]);
                listViewSongs.SelectedItems[i].Remove();
            }
            RefreshView();
            Log.Instance.PrintMessage("Deleted succesfully!", MessageType.Correct);
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

        #region Events

        private void PlaylistIU_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void PlaylistIU_DragDrop(object sender, DragEventArgs e)
        {
            Log.Instance.PrintMessage("Detected Drag & Drop", MessageType.Info);
            Song c = null;
            string[] canciones = null;
            if ((c = (Song)e.Data.GetData(typeof(Song))) != null)
            {
                if (!string.IsNullOrEmpty(c.Path))
                {
                    Log.Instance.PrintMessage("Adding " + c.Path, MessageType.Info);
                    Playlist.AddSong(c);
                }
            }
            else if ((canciones = (String[])e.Data.GetData(DataFormats.FileDrop)) != null) //El usuario arrastra desde el explorador.
            {
                foreach (string songPath in canciones)
                {
                    Log.Instance.PrintMessage("Adding " + songPath, MessageType.Info);
                    Song clr = new Song();
                    clr.Path = songPath;
                    Playlist.AddSong(clr);
                }
            }
            else
            {
                Log.Instance.PrintMessage("Can't process the data. Wrong input?", MessageType.Warning);
            }
            RefreshView();

            Player.Instancia.ActivarPorLista();
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
            if (e.CloseReason != CloseReason.ApplicationExitCall)
            {
                Hide();
                e.Cancel = true;
            }
        }

        private void changeNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WriteName WriteNameForm = new WriteName();
            WriteNameForm.StartPosition = FormStartPosition.CenterParent;
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
            Log.Instance.PrintMessage("Opening playlist", MessageType.Info);
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.CurrentDirectory;
            openFileDialog.Filter = ".plf|*.plf";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Playlist.Load(openFileDialog.FileName);
                    Log.Instance.PrintMessage("Opened", MessageType.Correct);
                }
                catch (Exception)
                {
                    Log.Instance.PrintMessage("Playlist file is invalid", MessageType.Error);
                    MessageBox.Show(Kernel.LocalTexts.GetString("errrorLR"), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                toolStripStatusLabelTracksSelected.Text = listViewSongs.SelectedItems.Count + " " + Kernel.LocalTexts.GetString("canciones");
                TimeSpan seleccion = new TimeSpan();
                foreach (ListViewItem songItem in listViewSongs.SelectedItems)
                {
                    Song song = Playlist[songItem.Index];
                    seleccion += song.Length;
                }
                if (seleccion.TotalMinutes < 60)
                    toolStripStatusLabelDuration.Text = seleccion.ToString(@"mm\:ss");
                else
                    toolStripStatusLabelDuration.Text = seleccion.ToString(@"hh\:mm\:ss");
            }
            else
            {
                UpdateTime();
                toolStripStatusLabelTracksSelected.Text = "0 " + Kernel.LocalTexts.GetString("canciones");
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Creates a new playlist and sends it to the Player.
            DialogResult result = MessageBox.Show(Kernel.LocalTexts.GetString("guardarPL"), "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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
            if (result != DialogResult.Yes) //for some reason, the user cancels at the save dialog...
            {
                Player.Instancia.CreatePlaylist(Kernel.LocalTexts.GetString("nuevaPlaylist"));
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
            {
                SetPlayVisibility(false);
                toolStripMenuItemOpenFolder.Enabled = false;
            }

            else
            {
                SetPlayVisibility(true);
                toolStripMenuItemOpenFolder.Enabled = true;
            }
            if (listViewSongs.SelectedItems.Count == 0)
                e.Cancel = true;
        }

        private void toolStripMenuItemPlay_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabelInfo.Text = Kernel.LocalTexts.GetString("infoReproducir");
        }

        private void toolStripMenuItemRemove_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabelInfo.Text = Kernel.LocalTexts.GetString("infoDelete");
        }

        private void toolStripMenuItemOpenFolder_Click(object sender, EventArgs e)
        {
            int indexSong = listViewSongs.SelectedItems[0].Index;
            DirectoryInfo dir = new DirectoryInfo(Playlist.GetSong(indexSong).Path);
            dir = dir.Parent;
            Process explorer = new Process();
            explorer.StartInfo.FileName = "explorer.exe";
            explorer.StartInfo.UseShellExecute = true;
            explorer.StartInfo.Arguments = dir.FullName;
            explorer.Start();
        }

        private void toolStripMenuItemOpenFolder_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabelInfo.Text = Kernel.LocalTexts.GetString("infoOpenFolder");
        }
        private void listViewSongs_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                RemoveSongs();
            }
        }
        #endregion
    }
}
