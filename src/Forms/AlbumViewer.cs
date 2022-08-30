using Cassiopeia.src.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Cassiopeia.src.Forms
{
    public partial class AlbumViewer : Form
    {
        private enum AlbumInfo
        {
            Artist = 0,
            Title = 1,
            Year = 2,
            Length = 3,
            Genre = 4,
            Type = 5,
            Location = 6,
            Format = 6,
            PublishYear = 7,
            PublishCountry = 8,
            CoverWear = 9,
            MediaWear = 10
        }
        private AlbumData albumToVisualize;
        private byte numDisco;
        private CompactDisc ViewCD;
        private VinylAlbum ViewVinyl;
        private CassetteTape ViewTape;
        private ListViewItemComparer lvwColumnSorter;
        private string[] labelData;
        //private int margin = 578 - 448;
        public AlbumViewer(ref AlbumData a)
        {
            InitializeComponent();
            numDisco = 0;
            albumToVisualize = a;
            ViewCD = null;

            //We are visualising a digital album
            labelEstadoDisco.Hide();
            
            if (albumToVisualize is not null && string.IsNullOrEmpty(albumToVisualize.SoundFilesPath))
            {
                buttonAnotaciones.Enabled = false;
                buttonPATH.Enabled = false;
            }
            vistaCanciones.Font = Config.FontView;
            SetTexts();
            LoadView();
        }
        public AlbumViewer(ref CompactDisc cd)
        {
            InitializeComponent();
            ViewCD = cd;
            buttonPATH.Hide();
            albumToVisualize = cd.Album;
            SetViewAlbumCover();
            numDisco = 1;
            labelInfoAlbum.Text = Kernel.GetText("artista") + ": " + cd.Album.Artist + Environment.NewLine +
                Kernel.GetText("titulo") + ": " + cd.Album.Title + Environment.NewLine +
                Kernel.GetText("año") + ": " + cd.Album.Year + Environment.NewLine +
                Kernel.GetText("duracion") + ": " + cd.Album.Length.ToString() + Environment.NewLine +
                Kernel.GetText("genero") + ": " + cd.Album.Genre.Name + Environment.NewLine +
                Kernel.GetText("formato") + ": " + Kernel.GetText(cd.SleeveType.ToString()) + Environment.NewLine +
                Kernel.GetText("añoPublicacion") + ": " + cd.Year + Environment.NewLine +
                Kernel.GetText("paisPublicacion") + ":" + cd.Country + Environment.NewLine +
                Kernel.GetText("estado_exterior") + ": " + Kernel.GetText(cd.SleeveCondition.ToString()) + Environment.NewLine;
            labelEstadoDisco.Text = Kernel.GetText("estado_medio") + " " + numDisco + ": " + Kernel.GetText(cd.Discos[0].MediaCondition.ToString()) + Environment.NewLine;

            lvwColumnSorter = new ListViewItemComparer();
            vistaCanciones.ListViewItemSorter = lvwColumnSorter;
            vistaCanciones.View = View.Details;
            vistaCanciones.MultiSelect = true;
            duracionSeleccionada.AutoSize = true;
            barraAbajo.Font = new Font("Segoe UI", 9);

            SetTexts();
            LoadView();
        }
        public AlbumViewer(ref VinylAlbum vinyl)
        {
            InitializeComponent();
            ViewVinyl = vinyl;
            buttonPATH.Hide();
            albumToVisualize = vinyl.Album;
            SetViewAlbumCover();
            numDisco = 1;
            labelInfoAlbum.Text = Kernel.GetText("artista") + ": " + vinyl.Album.Artist + Environment.NewLine +
                Kernel.GetText("titulo") + ": " + vinyl.Album.Title + Environment.NewLine +
                Kernel.GetText("año") + ": " + vinyl.Album.Year + Environment.NewLine +
                Kernel.GetText("duracion") + ": " + vinyl.Album.Length.ToString() + Environment.NewLine +
                Kernel.GetText("genero") + ": " + vinyl.Album.Genre.Name + Environment.NewLine +
                Kernel.GetText("añoPublicacion") + ": " + vinyl.Year + Environment.NewLine +
                Kernel.GetText("paisPublicacion") + ":" + vinyl.Country + Environment.NewLine +
                Kernel.GetText("estado_exterior") + ": " + Kernel.GetText(vinyl.SleeveCondition.ToString()) + Environment.NewLine;
            labelEstadoDisco.Text = Kernel.GetText("estado_medio") + " " + numDisco + ": " + Kernel.GetText(vinyl.DiscList[0].MediaCondition.ToString()) + Environment.NewLine;

            lvwColumnSorter = new ListViewItemComparer();
            vistaCanciones.ListViewItemSorter = lvwColumnSorter;
            vistaCanciones.View = View.Details;
            vistaCanciones.MultiSelect = true;
            duracionSeleccionada.AutoSize = true;
            barraAbajo.Font = new Font("Segoe UI", 9);

            SetTexts();
            LoadView();
        }
        public AlbumViewer(ref CassetteTape tape)
        {
            InitializeComponent();
            ViewTape = tape;
            buttonPATH.Hide();
            albumToVisualize = tape.Album;
            SetViewAlbumCover();
            numDisco = 1;
            labelInfoAlbum.Text = Kernel.GetText("artista") + ": " + tape.Album.Artist + Environment.NewLine +
                Kernel.GetText("titulo") + ": " + tape.Album.Title + Environment.NewLine +
                Kernel.GetText("año") + ": " + tape.Album.Year + Environment.NewLine +
                Kernel.GetText("duracion") + ": " + tape.Album.Length.ToString() + Environment.NewLine +
                Kernel.GetText("genero") + ": " + tape.Album.Genre.Name + Environment.NewLine +
                Kernel.GetText("añoPublicacion") + ": " + tape.Year + Environment.NewLine +
                Kernel.GetText("paisPublicacion") + ":" + tape.Country + Environment.NewLine +
                Kernel.GetText("estado_exterior") + ": " + Kernel.GetText(tape.SleeveCondition.ToString()) + Environment.NewLine;
            labelEstadoDisco.Text = Kernel.GetText("estado_medio") + " " + numDisco + ": " + Kernel.GetText(tape.MediaCondition.ToString()) + Environment.NewLine;

            lvwColumnSorter = new ListViewItemComparer();
            vistaCanciones.ListViewItemSorter = lvwColumnSorter;
            vistaCanciones.View = View.Details;
            vistaCanciones.MultiSelect = true;
            duracionSeleccionada.AutoSize = true;
            barraAbajo.Font = new Font("Segoe UI", 9);

            SetTexts();
            LoadView();
        }
        private void SetViewAlbumCover()
        {
            //Set the cover
            try
            {
                if (!string.IsNullOrEmpty(albumToVisualize.CoverPath))
                {
                    vistaCaratula.Image = Image.FromFile(albumToVisualize.CoverPath);
                    vistaCaratula.SizeMode = PictureBoxSizeMode.StretchImage;
                }
                else
                    vistaCaratula.Image = Properties.Resources.albumdesconocido;
            }
            catch (FileNotFoundException)
            {
                Log.Instance.PrintMessage(albumToVisualize.Artist + " " + albumToVisualize.Title + " has broken cover path...", MessageType.Warning);
                albumToVisualize.CoverPath = string.Empty;
                vistaCaratula.Image = Properties.Resources.albumdesconocido;
            }
        }

        private void CreateSongView()
        {
            Song c;
            ListViewItem[] items;
            //If it is a normal album
            if (ViewCD is null && ViewVinyl is null && ViewTape is null)
            {
                items = new ListViewItem[albumToVisualize.Songs.Count];
                for (int i = 0; i < albumToVisualize.Songs.Count; i++)
                {
                    String[] datos = new string[3];
                    c = albumToVisualize.Songs[i];
                    datos[0] = (i + 1).ToString();
                    c.ToStringArray().CopyTo(datos, 1);
                    items[i] = new ListViewItem(datos);

                    if (c is LongSong)
                    {
                        items[i].BackColor = Config.ColorLongSong;
                    }
                    if (c.IsBonus)
                    {
                        items[i].BackColor = Config.ColorBonus;
                    }
                }
            }
            else if (ViewVinyl is not null)
            {
                vistaCanciones.ShowGroups = true;
                items = new ListViewItem[ViewVinyl.TotalSongs];
                char side = ViewVinyl.DiscList[0].FrontSide;
                int songNum = 0; //Song number in array
                int sideNum = 0; //id of group

                vistaCanciones.Columns[0].Width = -2;
                for (int d = 0; d < ViewVinyl.DiscList.Count; d++)
                {
                    vistaCanciones.Groups.Add(new ListViewGroup(Kernel.GetText("lado") + " " + side));
                    int punt = songNum;
                    int numSongSide = 1;
                    for (int i = punt; i < ViewVinyl.DiscList[d].NumSongsFront + punt; i++)
                    {
                        string[] data = new string[3];
                        data[0] = side + numSongSide.ToString();
                        c = albumToVisualize.Songs[i];
                        c.ToStringArray().CopyTo(data, 1);
                        items[i] = new ListViewItem(data);
                        items[i].Group = vistaCanciones.Groups[sideNum];
                        if (c is LongSong)
                        {
                            items[i].BackColor = Config.ColorLongSong;
                        }
                        if (c.IsBonus)
                        {
                            items[i].BackColor = Config.ColorBonus;
                        }
                        numSongSide++;
                        songNum++;
                    }
                    side++;
                    sideNum++;
                    vistaCanciones.Groups.Add(new ListViewGroup(Kernel.GetText("lado") + " " + side));
                    punt = songNum;
                    numSongSide = 1;
                    for (int i = punt; i < ViewVinyl.DiscList[d].NumSongsBack + punt; i++)
                    {
                        string[] data = new string[3];
                        data[0] = side + numSongSide.ToString();
                        c = albumToVisualize.Songs[i];
                        c.ToStringArray().CopyTo(data, 1);
                        items[i] = new ListViewItem(data);
                        items[i].Group = vistaCanciones.Groups[sideNum];
                        if (c is LongSong)
                        {
                            items[i].BackColor = Config.ColorLongSong;
                        }
                        if (c.IsBonus)
                        {
                            items[i].BackColor = Config.ColorBonus;
                        }
                        songNum++;
                        numSongSide++;
                    }
                    side++;
                    sideNum++;
                }
            }
            else if(ViewTape is not null)
            {
                vistaCanciones.ShowGroups = true;
                items = new ListViewItem[ViewTape.TotalSongs];
                char side = 'A';
                int songNum = 0; //Song number in array
                int sideNum = 0; //id of group

                vistaCanciones.Columns[0].Width = -2;
                vistaCanciones.Groups.Add(new ListViewGroup(Kernel.GetText("lado") + " A"));
                int numSongSide = 1;
                for (int i = 0; i < ViewTape.NumSongsFront; i++)
                {
                    string[] data = new string[3];
                    data[0] = side + numSongSide.ToString();
                    c = albumToVisualize.Songs[i];
                    c.ToStringArray().CopyTo(data, 1);
                    items[i] = new ListViewItem(data);
                    items[i].Group = vistaCanciones.Groups[sideNum];
                    if (c is LongSong)
                    {
                        items[i].BackColor = Config.ColorLongSong;
                    }
                    if (c.IsBonus)
                    {
                        items[i].BackColor = Config.ColorBonus;
                    }
                    numSongSide++;
                    songNum++;
                }
                numSongSide = 1;
                side = 'B';
                vistaCanciones.Groups.Add(new ListViewGroup(Kernel.GetText("lado") + " B"));
                sideNum++;
                for (int i = ViewTape.NumSongsFront; i < ViewTape.NumSongsFront + ViewTape.NumSongsBack; i++)
                {
                    string[] data = new string[3];
                    data[0] = side + numSongSide.ToString();
                    c = albumToVisualize.Songs[i];
                    c.ToStringArray().CopyTo(data, 1);
                    items[i] = new ListViewItem(data);
                    items[i].Group = vistaCanciones.Groups[sideNum];
                    if (c is LongSong)
                    {
                        items[i].BackColor = Config.ColorLongSong;
                    }
                    if (c.IsBonus)
                    {
                        items[i].BackColor = Config.ColorBonus;
                    }
                    numSongSide++;
                    songNum++;
                }
            }
            else //do CD routine
            {
                vistaCanciones.ShowGroups = true;
                items = new ListViewItem[ViewCD.TotalSongs];
                int songNum = 0;
                for (int d = 0; d < ViewCD.Discos.Count; d++)
                {
                    vistaCanciones.Groups.Add(new ListViewGroup("CD " + Convert.ToString(d + 1)));
                    for (int j = 0; j < ViewCD.Discos[d].NumberOfSongs; j++)
                    {
                        String[] datos = new string[3];
                        datos[0] = (j + 1).ToString();
                        c = albumToVisualize.Songs[songNum];
                        c.ToStringArray().CopyTo(datos, 1);
                        items[songNum] = new ListViewItem(datos);
                        items[songNum].Group = vistaCanciones.Groups[d];
                        if (c is LongSong)
                        {
                            items[songNum].BackColor = Config.ColorLongSong;
                        }
                        if (c.IsBonus)
                        {
                            items[songNum].BackColor = Config.ColorBonus;
                        }
                        songNum++;
                    }
                }

            }
            vistaCanciones.Items.AddRange(items);
        }
        private void SetTexts()
        {
            Text = Kernel.GetText("visualizando") + " " + albumToVisualize.Artist + " - " + albumToVisualize.Title;
            vistaCanciones.Columns[0].Text = "#";
            vistaCanciones.Columns[1].Text = Kernel.GetText("titulo");
            vistaCanciones.Columns[2].Text = Kernel.GetText("duracion");
            okDoomerButton.Text = Kernel.GetText("hecho");
            editarButton.Text = Kernel.GetText("editar");
            duracionSeleccionada.Text = Kernel.GetText("dur_total") + ": 00:00:00";
            if (ViewCD is not null || ViewVinyl is not null || ViewTape is not null)
                buttonAnotaciones.Text = Kernel.GetText("editar_anotaciones");
            else
                buttonAnotaciones.Text = Kernel.GetText("reproducir");
            setBonusToolStripMenuItem.Text = Kernel.GetText("setBonus");
            reproducirToolStripMenuItem.Text = Kernel.GetText("reproducir");
            reproducirspotifyToolStripMenuItem.Text = Kernel.GetText("reproducirSpotify");
            buttonPATH.Text = Kernel.GetText("calcularPATHS");
            if (Config.Language == "el") //Greek needs a little adjustment on the UI
            {
                Font but = buttonPATH.Font;
                Font neo = new Font(but.FontFamily, 7);
                buttonPATH.Font = neo;
            }
            verLyricsToolStripMenuItem.Text = Kernel.GetText("verLyrics");
            fusionarToolStripMenuItem.Text = Kernel.GetText("fusionarCancionPartes");
            defusionarToolStripMenuItem.Text = Kernel.GetText("defusionarCancionPartes");
            copiarImagenStrip.Text = Kernel.GetText("copiarImagen");
        }
        private void ReloadView()
        {
            SetTexts();
            vistaCanciones.Items.Clear();
            int i = 0;
            foreach (Song c in albumToVisualize.Songs)
            {
                String[] datos = new string[3];
                datos[0] = (i + 1).ToString();
                c.ToStringArray().CopyTo(datos, 1);
                ListViewItem item = new ListViewItem(datos);

                if (c is LongSong)
                {
                    item.BackColor = Config.ColorLongSong;
                }
                if (c.IsBonus)
                {
                    item.BackColor = Config.ColorBonus;
                }
                vistaCanciones.Items.Add(item);
                i++;
            }

        }
        private void LoadView()
        {
            //Config the view
            vistaCanciones.Items.Clear();
            lvwColumnSorter = new ListViewItemComparer();
            vistaCanciones.ListViewItemSorter = lvwColumnSorter;
            vistaCanciones.View = View.Details;
            vistaCanciones.MultiSelect = true;

            SetViewAlbumCover();

            //Disable everything player related if there is something wrong
            if (string.IsNullOrEmpty(albumToVisualize.IdSpotify) || Kernel.Spotify is null || !Kernel.Spotify.AccountReady)
                reproducirspotifyToolStripMenuItem.Enabled = false;
            if (string.IsNullOrEmpty(albumToVisualize.SoundFilesPath))
                reproducirToolStripMenuItem.Enabled = false;

            CreateSongView();

            //Set the labels
            labelData = new string[11];
            //for (int i = 0; i < 10; i++)
            //{
            //    labelData = null;
            //}
            labelInfoAlbum.Text = "";


            TimeSpan durBonus = albumToVisualize.BonusLength;
            labelData[(int)AlbumInfo.Artist] = Kernel.GetText("artista") + ": " + albumToVisualize.Artist + Environment.NewLine;
            labelData[(int)AlbumInfo.Title] = Kernel.GetText("titulo") + ": " + albumToVisualize.Title + Environment.NewLine;
            labelData[(int)AlbumInfo.Year] = Kernel.GetText("año") + ": " + albumToVisualize.Year + Environment.NewLine;

            if (durBonus.TotalMilliseconds != 0)
                labelData[(int)AlbumInfo.Length] = Kernel.GetText("duracion") + ": " + albumToVisualize.Length.ToString() + " (" + durBonus.ToString() + ")" + Environment.NewLine;
            else
                labelData[(int)AlbumInfo.Length] = Kernel.GetText("duracion") + ": " + albumToVisualize.Length.ToString() + Environment.NewLine;

            labelData[(int)AlbumInfo.Type] = Kernel.GetText("tipoAlbum") + ": ";
            switch (albumToVisualize.Type)
            {
                case AlbumType.Studio:
                    labelData[(int)AlbumInfo.Type] += Kernel.GetText("estudio") + Environment.NewLine;
                    break;
                case AlbumType.Live:
                    labelData[(int)AlbumInfo.Type] += Kernel.GetText("live") + Environment.NewLine;
                    break;
                case AlbumType.Compilation:
                    labelData[(int)AlbumInfo.Type] += Kernel.GetText("compilacion") + Environment.NewLine;
                    break;
                case AlbumType.EP:
                    labelData[(int)AlbumInfo.Type] += "EP" + Environment.NewLine;
                    break;
                case AlbumType.Single:
                    labelData[(int)AlbumInfo.Type] += Kernel.GetText("sencillo") + Environment.NewLine;
                    break;
                default:
                    break;
            }
            labelData[(int)AlbumInfo.Genre] = Kernel.GetText("genero") + ": " + albumToVisualize.Genre.Name + Environment.NewLine;
            if (string.IsNullOrEmpty(albumToVisualize.SoundFilesPath))
                labelData[(int)AlbumInfo.Location] = "";
            else
                labelData[(int)AlbumInfo.Location] = Kernel.GetText("localizacion") + ": " + albumToVisualize.SoundFilesPath + Environment.NewLine;
            if (ViewCD is not null)
            {
                labelData[(int)AlbumInfo.Format] = Kernel.GetText(ViewCD.SleeveType.ToString()) + Environment.NewLine;
                labelData[(int)AlbumInfo.PublishYear] = Kernel.GetText("añoPublicacion") + ": " + ViewCD.Year + Environment.NewLine;
                labelData[(int)AlbumInfo.PublishCountry] = Kernel.GetText("paisPublicacion") + ": " + ViewCD.Country + Environment.NewLine;
                labelData[(int)AlbumInfo.CoverWear] = Kernel.GetText("estado_exterior") + ": " + Kernel.GetText(ViewCD.SleeveCondition.ToString()) + Environment.NewLine;
                labelData[(int)AlbumInfo.Length] = Kernel.GetText("duracion") + ": " + ViewCD.Length.ToString() + Environment.NewLine;
            }
            else if (ViewVinyl is not null)
            {
                labelData[(int)AlbumInfo.PublishYear] = Kernel.GetText("añoPublicacion") + ": " + ViewVinyl.Year + Environment.NewLine;
                labelData[(int)AlbumInfo.PublishCountry] = Kernel.GetText("paisPublicacion") + ": " + ViewVinyl.Country + Environment.NewLine;
                labelData[(int)AlbumInfo.CoverWear] = Kernel.GetText("estado_exterior") + ": " + Kernel.GetText(ViewVinyl.SleeveCondition.ToString()) + Environment.NewLine;
                labelData[(int)AlbumInfo.Length] = Kernel.GetText("duracion") + ": " + ViewVinyl.Length.ToString() + Environment.NewLine;
                
            }
            else if (ViewTape is not null)
            {
                labelData[(int)AlbumInfo.PublishYear] = Kernel.GetText("añoPublicacion") + ": " + ViewTape.Year + Environment.NewLine;
                labelData[(int)AlbumInfo.PublishCountry] = Kernel.GetText("paisPublicacion") + ": " + ViewTape.Country + Environment.NewLine;
                labelData[(int)AlbumInfo.CoverWear] = Kernel.GetText("estado_exterior") + ": " + Kernel.GetText(ViewTape.SleeveCondition.ToString()) + Environment.NewLine;
                labelData[(int)AlbumInfo.Length] = Kernel.GetText("duracion") + ": " + ViewTape.Length.ToString() + Environment.NewLine;
            }
            foreach (string data in labelData)
            {
                if (data is not null)
                    labelInfoAlbum.Text += data;
            }
            Controls.Add(barraAbajo);
            duracionSeleccionada.AutoSize = true;
            barraAbajo.Font = new Font("Segoe UI", 10);

        }
        #region Events
        private void ordenarColumnas(object sender, ColumnClickEventArgs e)
        {
            if (e.Column == lvwColumnSorter.ColumnaAOrdenar) // Determine if clicked column is already the column that is being sorted.
            {
                if (lvwColumnSorter.Orden == SortOrder.Ascending)
                    lvwColumnSorter.Orden = SortOrder.Descending;
                else lvwColumnSorter.Orden = SortOrder.Ascending;

            }
            else if (e.Column != 2 && e.Column != 3)//si la columna es  la del año o la de la duracion, que lo ponga de mayor a menor.
            {
                lvwColumnSorter.ColumnaAOrdenar = e.Column;
                lvwColumnSorter.Orden = SortOrder.Ascending;

            }
            else
            {
                lvwColumnSorter.ColumnaAOrdenar = e.Column; // Set the column number that is to be sorted; default to ascending.
                lvwColumnSorter.Orden = SortOrder.Descending;
            }
            vistaCanciones.Sort();
            vistaCanciones.Refresh();
        }

        private void okDoomerButton_Click(object sender, EventArgs e)
        {
            Dispose();
        }
        private void editarButton_Click(object sender, EventArgs e)
        {
            if (ViewCD is null)
            {
                EditAlbum editor = new EditAlbum(ref albumToVisualize);
                editor.Show();
            }
            else
            {
                CreateCD editor = new CreateCD(ref ViewCD, numDisco, true);
                editor.ShowDialog();
            }
            Close();
        }
        private void vistaCanciones_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            TimeSpan seleccion = new TimeSpan();
            foreach (ListViewItem cancion in vistaCanciones.SelectedItems)
            {
                if (ViewCD is not null && ViewCD.Discos.Count > 1)
                {
                    Song can = albumToVisualize.GetSong(cancion.SubItems[1].Text);
                    seleccion += can.Length;
                }
                else if (ViewVinyl is not null || ViewTape is not null)
                {
                    Song can = albumToVisualize.GetSong(cancion.SubItems[1].Text);
                    seleccion += can.Length;
                }
                else
                {
                    int c = Convert.ToInt32(cancion.SubItems[0].Text); c--;
                    Song can = albumToVisualize.GetSong(c);
                    seleccion += can.Length;
                }
            }
            duracionSeleccionada.Text = Kernel.GetText("dur_total") + ": " + seleccion.ToString();
        }

        private void vistaCanciones_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int n = Convert.ToInt32(vistaCanciones.SelectedItems[0].SubItems[0].Text);
            Song c = albumToVisualize.GetSong(n - 1);
            if (c is LongSong cl)
            {
                string infoDetallada = "";
                for (int i = 0; i < cl.Parts.Count; i++)
                {
                    infoDetallada += Utils.ConvertToRomanNumeral(i + 1) + ". ";
                    infoDetallada += cl.Parts[i].Title + " - " + cl.Parts[i].Length;
                    infoDetallada += Environment.NewLine;
                }
                MessageBox.Show(infoDetallada);
            }
        }

        private void buttonAnotaciones_Click(object sender, EventArgs e)
        {
            if (ViewCD is not null || ViewVinyl is not null || ViewTape is not null)
            {
                Anotaciones anoForm = new Anotaciones(ViewCD);
                anoForm.ShowDialog();
            }
            else
            {
                Player.Instancia.CreatePlaylist(albumToVisualize.ToString());
                foreach (Song cancion in albumToVisualize.Songs)
                {
                    Player.Instancia.Playlist.AddSong(cancion);
                }
                Player.Instancia.PlayList();
            }
        }

        private void labelEstadoDisco_Click(object sender, EventArgs e)
        {
            if(ViewCD is not null && ViewCD.Discos.Count != 1)
            {
                switch (numDisco)
                {
                    case 1:
                        numDisco = 2;
                        labelEstadoDisco.Text = Kernel.GetText("estado_medio") + " " + numDisco + ": " + Kernel.GetText(ViewCD.Discos[numDisco - 1].MediaCondition.ToString()) + Environment.NewLine;
                        break;
                    case 2:
                        numDisco = 1;
                        labelEstadoDisco.Text = Kernel.GetText("estado_medio") + " " + numDisco + ": " + Kernel.GetText(ViewCD.Discos[numDisco - 1].MediaCondition.ToString()) + Environment.NewLine;
                        break;
                    default:
                        break;
                }
            }
            else if(ViewVinyl is not null && ViewVinyl.DiscList.Count != 1)
            {
                switch (numDisco)
                {
                    case 1:
                        numDisco = 2;
                        labelEstadoDisco.Text = Kernel.GetText("estado_medio") + " " + numDisco + ": " + Kernel.GetText(ViewVinyl.DiscList[numDisco - 1].MediaCondition.ToString()) + Environment.NewLine;
                        break;
                    case 2:
                        numDisco = 1;
                        labelEstadoDisco.Text = Kernel.GetText("estado_medio") + " " + numDisco + ": " + Kernel.GetText(ViewVinyl.DiscList[numDisco - 1].MediaCondition.ToString()) + Environment.NewLine;
                        break;
                    default:
                        break;
                }
            }
        }

        private void visualizarAlbum_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                foreach (ListViewItem item in vistaCanciones.Items)
                {
                    item.Selected = true;
                }
            }
        }

        private void setBonusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in vistaCanciones.SelectedItems)
            {
                Song c = albumToVisualize.Songs[Convert.ToInt32(item.SubItems[0].Text) - 1];
                c.IsBonus = !c.IsBonus;
            }
            LoadView();
            Kernel.SetSaveMark();
            Kernel.MainForm.ReloadView();
        }

        private void infoAlbum_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(albumToVisualize.SoundFilesPath))
            {
                Process explorador = new Process();
                explorador.StartInfo.UseShellExecute = true;
                explorador.StartInfo.FileName = "explorer.exe";
                explorador.StartInfo.Arguments = albumToVisualize.SoundFilesPath;
                explorador.Start();
                Log.Instance.PrintMessage("Opened explorer.exe with PID: " + explorador.Id, MessageType.Info);
            }
        }

        private void reproducirspotifyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Song selected = albumToVisualize.GetSong(vistaCanciones.SelectedIndices[0]);
            if (selected is not LongSong)
            {
                try
                {
                    Kernel.Spotify.PlaySongFromAlbum(albumToVisualize.IdSpotify, vistaCanciones.SelectedItems[0].Index);
                }
                catch (SpotifyAPI.Web.APIException ex)
                {
                    Log.Instance.PrintMessage(ex.Message, MessageType.Warning);
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                try
                {
                    LongSong ls = (LongSong)selected;
                    Kernel.Spotify.PlaySong(albumToVisualize.IdSpotify, ls);
                }
                catch (SpotifyAPI.Web.APIException ex)
                {
                    Log.Instance.PrintMessage(ex.Message, MessageType.Warning);
                    MessageBox.Show(ex.Message);
                }
            }

        }
        private void reproducirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Song cancionAReproducir = albumToVisualize.GetSong(vistaCanciones.SelectedItems[0].Index);
            if (cancionAReproducir is LongSong)
            {
                LongSong cl = (LongSong)cancionAReproducir;
                Player.Instancia.PlaySong(cl);
            }

            else
                Player.Instancia.PlaySong(cancionAReproducir);
        }

        private void vistaCanciones_ItemDrag(object sender, ItemDragEventArgs e)
        {
            Song cancion = albumToVisualize.GetSong(vistaCanciones.SelectedItems[0].Index);
            vistaCanciones.DoDragDrop(cancion, DragDropEffects.Copy);
        }
        private void vistaCanciones_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
            Song droppedData = (Song)e.Data.GetData(typeof(Song));
        }
        private void buttonPATH_Click(object sender, EventArgs e)
        {
            Kernel.SetPathsForAlbum(albumToVisualize);
        }

        private void verLyricsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Song cancion = albumToVisualize.GetSong(vistaCanciones.SelectedItems[0].Index);
            LyricsViewer VL = new LyricsViewer(cancion);
            VL.Show();
        }

        private void copiar_Click(object sender, EventArgs e)
        {
            Clipboard.SetImage(vistaCaratula.Image);
            Log.Instance.PrintMessage("Enviada imagen al portapapeles", MessageType.Correct);
        }
        private void clickDerechoConfig_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            defusionarToolStripMenuItem.Visible = true;
            fusionarToolStripMenuItem.Visible = true;
            int i = vistaCanciones.SelectedItems[0].Index;
            Song seleccion = albumToVisualize.GetSong(i);
            if (vistaCanciones.SelectedItems.Count > 1)
                defusionarToolStripMenuItem.Visible = false;
            if (!(seleccion is LongSong))
                defusionarToolStripMenuItem.Visible = false;
            else
                fusionarToolStripMenuItem.Visible = false;
        }
        private void fusionarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (vistaCanciones.SelectedItems.Count == 1)
            {
                MessageBox.Show(Kernel.GetText("error_fusionsingular"));
                return;
            }
            int num = vistaCanciones.SelectedItems[0].Index;
            List<string> cancionesABorrar = new List<string>();
            LongSong cl = new LongSong();
            cl.SetAlbum(albumToVisualize);
            cl.Title = albumToVisualize.GetSong(num).Title;

            foreach (ListViewItem cancionItem in vistaCanciones.SelectedItems)
            {
                cl.AddPart(albumToVisualize.GetSong(cancionItem.Index));
                cancionesABorrar.Add(cancionItem.SubItems[1].Text);
            }

            foreach (string songTitle in cancionesABorrar)
                albumToVisualize.RemoveSong(songTitle);

            albumToVisualize.AddSong(cl, num); //IT works...
            //This is an edit.
            Kernel.SetSaveMark();
            ReloadView();
        }

        private void defusionarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListViewItem item = vistaCanciones.SelectedItems[0];

            int num = item.Index;
            if (!(albumToVisualize.Songs[num] is LongSong))
            {
                MessageBox.Show(Kernel.GetText("error_defusion"));
                return;
            }

            LongSong longSong = (LongSong)albumToVisualize.GetSong(num);
            foreach (Song parte in longSong.Parts)
            {
                albumToVisualize.AddSong(parte, num);
                num++;
            }

            longSong.Title = "---"; //This is for safe defusing

            albumToVisualize.RemoveSong(longSong.Title);
            //This is an edit.
            Kernel.SetSaveMark();
            ReloadView();
        }

        private void visualizarAlbum_Resize(object sender, EventArgs e)
        {
            //Control controlSender = (Control)sender;
            //Size newImageSize = new Size(controlSender.Height - margin, controlSender.Height - margin);
            //if (newImageSize.Width+vistaCaratula.Location.X > controlSender.Width)
            //{
            //    if(controlSender.Width - newImageSize.Width < vistaCanciones.Width + vistaCanciones.Location.X)
            //        vistaCaratula.Location = new Point(vistaCanciones.Width + vistaCanciones.Location.X, vistaCaratula.Location.Y);
            //    else
            //        vistaCaratula.Location = new Point(controlSender.Width - newImageSize.Width, vistaCaratula.Location.Y);
            //}
            //vistaCaratula.Size = newImageSize;
        }
        #endregion
    }
}