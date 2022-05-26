using Cassiopeia.src.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cassiopeia.src.Forms
{
    public enum ViewType
    {
        Digital,
        CD,
        Vinyl,
        Cassette_Tape
    }
    public enum SaveType
    {
        Digital, CD, Vinyl, Cassette_Tape
    }
    public partial class MainForm : Form
    {
        private bool deleting;
        private ListViewItemComparer lvwColumnSorter;
        public ViewType ViewType;
        private delegate void SafeCallBringFront();
        Log Log = Log.Instance;
        Size margins;
        private AlbumData selectedAlbum = null;
        private bool filtered;
        private bool selecting = false;
        public MainForm()
        {
            InitializeComponent();
            //Load Spotify
            this.Load += async (sender, args) => await Task.Run(() => Kernel.InitSpotify());
            deleting = false;
            filtered = false;
            lvwColumnSorter = new ListViewItemComparer();
            vistaAlbumes.ListViewItemSorter = lvwColumnSorter;
            vistaAlbumes.MultiSelect = true;

            vistaAlbumes.View = View.Details;
            SetTexts();
            vistaAlbumes.FullRowSelect = true;
            duracionSeleccionada.AutoSize = true;
            barraAbajo.Font = new Font("Segoe UI", 10);
            duracionSeleccionada.Text = Kernel.LocalTexts.GetString("dur_total") + ": 00:00:00";
            duracionSeleccionada.Visible = false;
            //vistaAlbumes.DrawItem += (sender, e) => { e.DrawDefault = true; };
            //vistaAlbumes.DrawSubItem += (sender, e) => { e.DrawDefault = true; };
            //vistaAlbumes.OwnerDraw = true;
            if (Config.LinkedWithSpotify)
                linkSpotifyStripMenuItem.Visible = false;
            else
                importSpotifyStripMenuItem.Enabled = false;
            spotifyStripMenuItem.Enabled = false;
            cargarDiscosLegacyToolStripMenuItem.Visible = false;
            vistaAlbumes.Font = Config.FontView;
            margins = Size - vistaAlbumes.Size;
            //From designer. Default has sidebar enabled.

            Size = Config.MainFormSize;
            vistaAlbumes.Size = Size - margins;
            panelSidebar.Height = vistaAlbumes.Height;
            //If sidebar is enabled
            if (Config.MainFormViewSidebar)
            {
                showSidebarToolStripMenuItem.Checked = true;
                panelSidebar.Visible = true;
            }
            else
            {
                showSidebarToolStripMenuItem.Checked = false;
                panelSidebar.Visible = false;
                vistaAlbumes.Width += panelSidebar.Width;
            }
            //DarkMode();
            Log.PrintMessage("Main form created", MessageType.Correct);
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public void ReloadView()
        {
            vistaAlbumes.Font = Config.FontView;
            SetTexts();
            LoadView();
        }
        public void EnableInternet(bool i)
        {
            spotifyStripMenuItem.Enabled = i;
        }
        public void RemoveLink()
        {
            linkSpotifyStripMenuItem.Visible = false;
        }
        public void ActivarReproduccionSpotify()
        {
            playSpotifyAlbumToolStripMenuItem.Enabled = true;
        }
        private void LoadView()
        {
            Log.Instance.PrintMessage("Loading view" + ViewType, MessageType.Info);
            if (filtered)
            {
                LoadFilteredView(Kernel.Collection.FilteredAlbums);
                return;
            }
            vistaAlbumes.BeginUpdate();
            vistaAlbumes.Items.Clear();
            Stopwatch crono = Stopwatch.StartNew();
            switch (ViewType)
            {
                case ViewType.Digital:
                    ListViewItem[] items = new ListViewItem[Kernel.Collection.Albums.Count];
                    int i = 0;
                    foreach (var pair in Kernel.Collection.Albums)
                    {
                        string[] datos = pair.Value.ToStringArray();
                        items[i] = new ListViewItem(datos);
                        i++;
                    }
                    //for (int i = 0; i < Kernel.Collection.Albums.Count; i++)
                    //{
                    //    String[] datos = Kernel.Collection.Albums[i].ToStringArray();
                    //    items[i] = new ListViewItem(datos);
                    //}
                    vistaAlbumes.Items.AddRange(items);
                    labelGeneralInfo.Text = Kernel.LocalTexts.GetString("numOf") + ": " + Kernel.LocalTexts.GetString("albumes").ToLower() +" " + Kernel.Collection.Albums.Count + Environment.NewLine +
                    "Total duration: " + Kernel.Collection.GetTotalTime(Kernel.Collection.Albums.Select(pair => pair.Value).ToList());
                    labelGeneralInfo.Location = new Point((panelSidebar.Width - labelGeneralInfo.Width) / 2, labelGeneralInfo.Location.Y);
                    break;
                case ViewType.CD:
                    ListViewPhysicalAlbum[] cds = new ListViewPhysicalAlbum[Kernel.Collection.CDS.Count];
                    int j = 0;
                    foreach (CompactDisc cd in Kernel.Collection.CDS)
                    {
                        String[] datos = cd.ToStringArray();
                        cds[j] = new ListViewPhysicalAlbum(datos);
                        cds[j].ID = cd.Id;
                        j++;
                    }

                    vistaAlbumes.Items.AddRange(cds);

                    labelGeneralInfo.Text = Kernel.LocalTexts.GetString("numOf") + " CDS: " + Kernel.Collection.CDS.Count + Environment.NewLine +
                    Kernel.LocalTexts.GetString("dur_total") + ": " + Kernel.Collection.GetTotalTime(Kernel.Collection.CDS);
                    labelGeneralInfo.Location = new Point((panelSidebar.Width - labelGeneralInfo.Width) / 2, labelGeneralInfo.Location.Y);
                    break;
                case ViewType.Vinyl:
                    ListViewPhysicalAlbum[] vinyls = new ListViewPhysicalAlbum[Kernel.Collection.Vinyls.Count];
                    int k = 0;
                    foreach (VinylAlbum v in Kernel.Collection.Vinyls)
                    {
                        String[] datos = v.ToStringArray();
                        vinyls[k] = new ListViewPhysicalAlbum(datos);
                        vinyls[k].ID = v.Id;
                        k++;
                    }
                    vistaAlbumes.Items.AddRange(vinyls);
                    labelGeneralInfo.Text = Kernel.LocalTexts.GetString("numOf") + " " + Kernel.LocalTexts.GetString("vinyls") + ": " + Kernel.Collection.Vinyls.Count + Environment.NewLine + 
                        Kernel.LocalTexts.GetString("dur_total") + ": " + Kernel.Collection.GetTotalTime(Kernel.Collection.Vinyls);
                    labelGeneralInfo.Location = new Point((panelSidebar.Width - labelGeneralInfo.Width) / 2, labelGeneralInfo.Location.Y);
                    break;
                default:
                    break;
            }
            vistaAlbumes.EndUpdate();
            crono.Stop();
            Log.Instance.PrintMessage("Loaded", MessageType.Correct, crono, TimeType.Milliseconds);
        }
        private void SetTexts()
        {
#if DEBUG
            Text = Kernel.LocalTexts.GetString("titulo_ventana_principal") + " " + Kernel.Version + " Codename " + Kernel.Codename + " DEBUG";
#else
            Text = Kernel.LocalTexts.GetString("titulo_ventana_principal");
#endif
            archivoMenuItem1.Text = Kernel.LocalTexts.GetString("archivo");

            agregarAlbumToolStripMenuItem.Text = Kernel.LocalTexts.GetString("agregar_album");
            toolStripButtonNewAlbum.ToolTipText = Kernel.LocalTexts.GetString("agregar_album");

            nuevoToolStripMenuItem.Text = Kernel.LocalTexts.GetString("nuevaBD");
            toolStripButtonNewDatabase.ToolTipText = Kernel.LocalTexts.GetString("nuevaBD");

            toolStripButtonSaveDatabase.ToolTipText = Kernel.LocalTexts.GetString("save");

            abrirToolStripMenuItem.Text = Kernel.LocalTexts.GetString("abrir_registros");
            toolStripButtonOpenDatabase.ToolTipText = abrirToolStripMenuItem.Text;

            salirToolStripMenuItem.Text = Kernel.LocalTexts.GetString("salir");
            vistaAlbumes.Columns[0].Text = Kernel.LocalTexts.GetString("artista");
            vistaAlbumes.Columns[1].Text = Kernel.LocalTexts.GetString("titulo");
            vistaAlbumes.Columns[2].Text = Kernel.LocalTexts.GetString("año");
            vistaAlbumes.Columns[3].Text = Kernel.LocalTexts.GetString("duracion");
            vistaAlbumes.Columns[4].Text = Kernel.LocalTexts.GetString("genero");
            searchSpotifyStripMenuItem.Text = Kernel.LocalTexts.GetString("buscar_Spotify");
            guardarcomo.Text = Kernel.LocalTexts.GetString("saveAs") + "...";
            seleccionToolStripMenuItem.Text = Kernel.LocalTexts.GetString("seleccion");
            adminMenu.Text = Kernel.LocalTexts.GetString("admin");
            generarAlbumToolStripMenuItem.Text = Kernel.LocalTexts.GetString("generar_azar");
            borrarseleccionToolStripMenuItem.Text = Kernel.LocalTexts.GetString("borrar_seleccion");
            acercaDeToolStripMenuItem.Text = Kernel.LocalTexts.GetString("acerca") + " " + Kernel.LocalTexts.GetString("titulo_ventana_principal");
            clickDerechoMenuContexto.Items[0].Text = Kernel.LocalTexts.GetString("crearCD");
            cargarDiscosLegacyToolStripMenuItem.Text = Kernel.LocalTexts.GetString("cargarDiscosLegacy");
            verToolStripMenuItem.Text = Kernel.LocalTexts.GetString("ver");
            digitalToolStripMenuItem.Text = Kernel.LocalTexts.GetString("digital");
            copiarToolStripMenuItem.Text = Kernel.LocalTexts.GetString("copiar");
            linkSpotifyStripMenuItem.Text = Kernel.LocalTexts.GetString("vincular");
            playSpotifyAlbumToolStripMenuItem.Text = Kernel.LocalTexts.GetString("reproducirSpotify");
            reproductorToolStripMenuItem.Text = Kernel.LocalTexts.GetString("reproductor");
            abrirCDMenuItem.Text = Kernel.LocalTexts.GetString("abrirCD") + "...";
            verLyricsToolStripMenuItem.Text = Kernel.LocalTexts.GetString("verLyrics");
            verLogToolStripMenuItem.Text = Kernel.LocalTexts.GetString("verLog");
            nuevoAlbumDesdeCarpetaToolStripMenuItem.Text = Kernel.LocalTexts.GetString("nuevoAlbumDesdeCarpeta");
            configToolStripMenuItem.Text = Kernel.LocalTexts.GetString("configuracion");
            importSpotifyStripMenuItem.Text = Kernel.LocalTexts.GetString("importSpotify");
            sidebarCopyImageToolStripMenuItem.Text = Kernel.LocalTexts.GetString("copiarImagen");
            showSidebarToolStripMenuItem.Text = Kernel.LocalTexts.GetString("showPanel");

            filterToolStripMenuItem.Text = Kernel.LocalTexts.GetString("filter");
            toolStripButtonFilter.Text = filterToolStripMenuItem.Text;

            toolStripTextBoxSearch.ToolTipText = Kernel.LocalTexts.GetString("write_filter");

            vinylToolStripMenuItem.Text = Kernel.LocalTexts.GetString("vinyl");
            createVinylToolStripMenuItem.Text = Kernel.LocalTexts.GetString("createVinyl");
            UpdateViewInfo();
        }
        private void UpdateViewInfo()
        {
            switch (ViewType)
            {
                case ViewType.Digital:
                    toolStripStatusLabelViewInfo.Text = Kernel.LocalTexts.GetString("digital");
                    break;
                case ViewType.CD:
                    toolStripStatusLabelViewInfo.Text = "CD";
                    break;
                case ViewType.Vinyl:
                    toolStripStatusLabelViewInfo.Text = Kernel.LocalTexts.GetString("vinyl");
                    break;
                default:
                    break;
            }
        }

        private void DeleteSelectedAlbums(ViewType tipoVista)
        {
            Stopwatch crono = Stopwatch.StartNew();
            vistaAlbumes.BeginUpdate();
            deleting = true;
            switch (tipoVista)
            {
                case ViewType.Digital:
                    Console.WriteLine("Deleting " + vistaAlbumes.SelectedItems.Count + " albums");
                    try
                    {

                        while (vistaAlbumes.SelectedIndices.Count != 0)
                        {
                            int i = vistaAlbumes.SelectedIndices[0];
                            AlbumData a = Kernel.Collection.GetAlbum(i, filtered);
                            Kernel.Collection.RemoveAlbum(ref a);
                            //vistaAlbumes.Items.Remove(vistaAlbumes.Items[i]);
                            vistaAlbumes.Items.RemoveAt(i);
                        }
                    }
                    catch (InvalidOperationException ex)
                    {
                        MessageBox.Show(Kernel.LocalTexts.GetString("errorBorrado") + Environment.NewLine + ex.Message);
                    }
                    break;
                case ViewType.CD:
                    Console.WriteLine("Deleting " + vistaAlbumes.SelectedItems.Count + " CD");
                    try
                    {
                        while (vistaAlbumes.SelectedIndices.Count != 0)
                        {
                            ListViewPhysicalAlbum item = (ListViewPhysicalAlbum)vistaAlbumes.Items[vistaAlbumes.SelectedIndices[0]];
                            int i = vistaAlbumes.SelectedIndices[0];
                            CompactDisc cd = Kernel.Collection.GetCDById(item.ID);
                            Kernel.Collection.DeleteCD(cd.Id);
                            vistaAlbumes.Items.Remove(vistaAlbumes.Items[i]);
                        }
                    }
                    catch (InvalidOperationException ex)
                    {
                        MessageBox.Show(Kernel.LocalTexts.GetString("errorBorrado") + Environment.NewLine + ex.Message);
                    }
                    //for (int i = 0; i < cuantos; i++)
                    //{
                    //    //itemsABorrar[i] = vistaAlbumes.SelectedItems[i];
                    //}
                    //for (int i = 0; i < cuantos; i++)
                    //{
                    //    CompactDisc cdaborrar = Kernel.Collection.GetCDById(vistaAlbumes.SelectedItems[i].SubItems[5].Text);
                    //    CompactDisc cdd = cdaborrar;
                    //    Kernel.Collection.DeleteCD(ref cdaborrar);
                    //    cdd.AlbumData.CanBeRemoved = true;

                    //    foreach (CompactDisc cd in Kernel.Collection.CDS)
                    //    {
                    //        if (cd.AlbumData == cdd.AlbumData)
                    //            cd.AlbumData.CanBeRemoved = false;
                    //    }
                    //}
                    //for (int i = 0; i < cuantos; i++)
                    //{
                    //    //vistaAlbumes.Items.Remove(itemsABorrar[i]);
                    //}
                    break;
                case ViewType.Vinyl:
                    Console.WriteLine("Deleting " + vistaAlbumes.SelectedItems.Count + " Vinyls");
                    try
                    {
                        while (vistaAlbumes.SelectedIndices.Count != 0)
                        {
                            ListViewPhysicalAlbum item = (ListViewPhysicalAlbum)vistaAlbumes.Items[vistaAlbumes.SelectedIndices[0]];
                            int i = vistaAlbumes.SelectedIndices[0];
                            VinylAlbum v = Kernel.Collection.GetVinylByID(item.ID);
                            Kernel.Collection.DeleteVinyl(v.Id);
                            vistaAlbumes.Items.Remove(vistaAlbumes.Items[i]);
                        }
                    }
                    catch (InvalidOperationException ex)
                    {
                        MessageBox.Show(Kernel.LocalTexts.GetString("errorBorrado") + Environment.NewLine + ex.Message);
                    }
                    break;
                default:
                    break;
            }
            deleting = false;
            vistaAlbumes.EndUpdate();
            duracionSeleccionada.Text = Kernel.LocalTexts.GetString("dur_total") + ": 00:00:00";
            crono.Stop();
            if(!filtered)
                LoadView();
            //Unload the sidebar because we deleted the album.
            UpdateSidebar((AlbumData)null);
            //TODO: DELETE THE JPG FILE.
            Log.Instance.PrintMessage("Deletion completed.", MessageType.Correct, crono, TimeType.Milliseconds);
        }
       
        private void guardarDiscos(string nombre, SaveType tipoGuardado)
        {
            if (tipoGuardado == SaveType.Digital)
                Kernel.SaveAlbums(nombre, SaveType.Digital);
            else
                Kernel.SaveAlbums(nombre, tipoGuardado, true);
        }
        private void UpdateSidebar(AlbumData a)
        {
            int width = panelSidebar.Width - 20;
            if (a is not null)
            {   
                try
                {
                    if (!string.IsNullOrEmpty(a.CoverPath))
                    {
                        //Doing this will allow me to replace album cover and not locking the file
                        Image cover;
                        using (var temp = new Bitmap(a.CoverPath))
                            cover = new Bitmap(temp);
                        pictureBoxSidebarCover.Image = cover;
                        //free mem
                        GC.Collect();
                    }
                }
                catch (Exception PataPumParriba)
                {
                    Log.Instance.PrintMessage("Couldn't set the album cover on the sidebar", MessageType.Warning);
                    Log.Instance.PrintMessage("This file cannot be found: "+PataPumParriba.Message, MessageType.Warning);
                    pictureBoxSidebarCover.Image = Properties.Resources.albumdesconocido;
                }


                labelInfoAlbum.Location = new Point(0, labelInfoAlbum.Location.Y);
                labelInfoAlbum.Text = a.Artist + Environment.NewLine +
                                      a.Title + "(" + a.Year + ")" + Environment.NewLine +
                                       a.Length + Environment.NewLine;
                labelInfoAlbum.AutoSize = true;
                if (labelInfoAlbum.Width > width)
                {
                    int height = labelInfoAlbum.Height;
                    labelInfoAlbum.AutoSize = false;
                    labelInfoAlbum.Size = new Size(width, height);
                }
            }
            else
            {
                labelInfoAlbum.Text = "";
                pictureBoxSidebarCover.Image = Properties.Resources.albumdesconocido;
            }
            labelInfoAlbum.Location = new Point((panelSidebar.Width - labelInfoAlbum.Width) / 2, pictureBoxSidebarCover.Height + 20);
            pictureBoxSidebarCover.Location = new Point((panelSidebar.Width - pictureBoxSidebarCover.Width) / 2, 3);
        }
        private void UpdateSidebar(CompactDisc cd)
        {
            if (cd is not null)
            {
                if (pictureBoxSidebarCover.Image != Properties.Resources.albumdesconocido)
                    pictureBoxSidebarCover.Image = null;
                if (!string.IsNullOrEmpty(cd.Album.CoverPath))
                    pictureBoxSidebarCover.Image = Image.FromFile(cd.Album.CoverPath);
                else
                    pictureBoxSidebarCover.Image = Properties.Resources.albumdesconocido;
                ////Doing this will allow me to replace album cover and not locking the file
                //Image cover;
                //using (var temp = new Bitmap(a.CoverPath))
                //    cover = new Bitmap(temp);
                //pictureBoxSidebarCover.Image = cover;
                labelInfoAlbum.Location = new Point(0, labelInfoAlbum.Location.Y);
                labelInfoAlbum.Text = cd.Album.Artist + Environment.NewLine +
                                      cd.Album.Title + "(" + cd.Album.Year + ")" + Environment.NewLine +
                                       cd.Length + Environment.NewLine +
                                       Kernel.LocalTexts.GetString("estado_exterior") + ": " + Kernel.LocalTexts.GetString(cd.SleeveCondition.ToString()) + Environment.NewLine +
                                        "Number of discs: " + cd.Discos.Count;
            }
            else
            {
                labelInfoAlbum.Text = "";
                pictureBoxSidebarCover.Image = Properties.Resources.albumdesconocido;
            }
            labelInfoAlbum.Location = new Point((panelSidebar.Width - labelInfoAlbum.Width) / 2, pictureBoxSidebarCover.Height + 20);
            pictureBoxSidebarCover.Location = new Point((panelSidebar.Width - pictureBoxSidebarCover.Width) / 2, 3);
        }
        private void ShowSidebar()
        {
            panelSidebar.Visible = true;
            Width += panelSidebar.Width;
            Config.MainFormViewSidebar = true;
        }
        private void HideSidebar()
        {
            panelSidebar.Visible = false;
            if (WindowState != FormWindowState.Maximized)
                Width -= panelSidebar.Width - 20;
            else
                vistaAlbumes.Width += panelSidebar.Width;
            
            Config.MainFormViewSidebar = false;
        }
        public void SetSaveMark()
        {
            toolStripButtonSaveDatabase.Image = Properties.Resources.diskette_mark;
        }
        private void OpenFile()
        {
            Log.PrintMessage("Opening from file", MessageType.Info);
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.CurrentDirectory;
            switch (ViewType)
            {
                case ViewType.Digital:
                    openFileDialog.Filter = Kernel.LocalTexts.GetString("archivo") + " .mdb (*.mdb)|*.mdb | " + Kernel.LocalTexts.GetString("archivo") + " .csv|*.csv";
                    break;
                case ViewType.CD:
                    openFileDialog.Filter = Kernel.LocalTexts.GetString("archivo") + " .json (*.json)|*.json";
                    break;
                case ViewType.Vinyl:
                    break;
                case ViewType.Cassette_Tape:
                    break;
                default:
                    break;
            }
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                switch (ViewType)
                {
                    case ViewType.Digital:
                        Kernel.LoadCSVAlbums(openFileDialog.FileName);
                        break;
                    case ViewType.CD:
                        Kernel.LoadCD(openFileDialog.FileName);
                        break;
                    case ViewType.Vinyl:
                        break;
                    case ViewType.Cassette_Tape:
                        break;
                    default:
                        break;
                }
            }
            Kernel.SetSaveMark();
            LoadView();
        }
        private void EditSelectedAlbum()
        {
            //maybe implement rest of views.
            //var selected = vistaAlbumes.SelectedItems[0];
            //string query = selected.SubItems[0].Text + Kernel.SearchSeparator + selected.SubItems[1].Text;
            //AlbumData a = Kernel.Collection.GetAlbum(query);
            AlbumData a = GetSelectedAlbumFromView();
            EditAlbum editAlbumForm = new EditAlbum(ref a, true);
            editAlbumForm.Show();
        }
        private void ShowSelectedAlbum()
        {
            switch (ViewType)
            {
                case ViewType.Digital:
                    foreach (ListViewItem item in vistaAlbumes.SelectedItems)
                    {
                        AlbumData albumToShow = GetSelectedAlbumFromView();
                        AlbumViewer vistazo = new AlbumViewer(ref albumToShow);
                        vistazo.Show();
                    }
                    break;
                case ViewType.CD:
                    foreach (ListViewPhysicalAlbum cdViewItem in vistaAlbumes.SelectedItems)
                    {
                        string b = cdViewItem.SubItems[0].Text + Kernel.SearchSeparator + cdViewItem.SubItems[1].Text;
                        CompactDisc cd;
                        Kernel.Collection.GetAlbum(b, out cd);
                        AlbumViewer visCD = new AlbumViewer(ref cd);
                        visCD.Show();
                    }
                    break;
                case ViewType.Vinyl:
                    foreach (ListViewPhysicalAlbum vinylListViewItem in vistaAlbumes.SelectedItems)
                    {
                        VinylAlbum v = Kernel.Collection.GetVinylByID(vinylListViewItem.ID);
                        AlbumViewer visCD = new AlbumViewer(ref v);
                        visCD.Show();
                    }
                    break;
            }
        }
        private void ManageSongIcons()
        {
            if (vistaAlbumes.SelectedItems.Count == 1)
            {
                //Add the buttons.
                if (!toolStripMain.Items.ContainsKey("separator_song"))
                {
                    ToolStripSeparator toolStripSeparator = new();
                    toolStripSeparator.Name = "separator_song";
                    toolStripMain.Items.Add(toolStripSeparator);
                }
                if (!toolStripMain.Items.ContainsKey("view"))
                {
                    ToolStripButton view = new ToolStripButton(Properties.Resources.view);
                    view.ToolTipText = Kernel.LocalTexts.GetString("ver");
                    view.Click += (object sender, EventArgs e) => ShowSelectedAlbum();
                    view.Name = "view";
                    toolStripMain.Items.Add(view);
                }
                if (!toolStripMain.Items.ContainsKey("edit"))
                {
                    ToolStripButton edit = new ToolStripButton(Properties.Resources.editing);
                    edit.ToolTipText = Kernel.LocalTexts.GetString("editar");
                    edit.Click += (object sender, EventArgs e) => EditSelectedAlbum();
                    edit.Name = "edit";
                    toolStripMain.Items.Add(edit);
                }
                if (!toolStripMain.Items.ContainsKey("delete"))
                {
                    ToolStripButton delete = new ToolStripButton(Properties.Resources.delete);
                    delete.ToolTipText = Kernel.LocalTexts.GetString("borrar_seleccion");
                    delete.Click += (object sender, EventArgs e) => DeleteSelectedAlbums(ViewType);
                    delete.Name = "delete";
                    toolStripMain.Items.Add(delete);
                }
                if (!toolStripMain.Items.ContainsKey("lyrics"))
                {
                    ToolStripButton lyrics = new ToolStripButton(Properties.Resources.lyrics);
                    lyrics.ToolTipText = Kernel.LocalTexts.GetString("verLyrics");
                    lyrics.Click += (object sender, EventArgs e) => OpenLyricsForSelectedAlbum(sender, e);
                    lyrics.Name = "lyrics";
                    toolStripMain.Items.Add(lyrics);
                }
                if (!toolStripMain.Items.ContainsKey("addCD"))
                {
                    ToolStripButton addCD = new(Properties.Resources.addcd);
                    addCD.ToolTipText = Kernel.LocalTexts.GetString("crearCD");
                    addCD.Click += (object sender, EventArgs e) => CreateCDFromSelectionAndAdd();
                    addCD.Name = "addCD";
                    toolStripMain.Items.Add(addCD);
                }
                if (!toolStripMain.Items.ContainsKey("addvinyl"))
                {
                    ToolStripButton addvinyl = new(Properties.Resources.vinyl);
                    addvinyl.ToolTipText = Kernel.LocalTexts.GetString("createVinyl");
                    addvinyl.Click += (object sender, EventArgs e) => CreateVinylRecordFromSelectionAndAdd();
                    addvinyl.Name = "addvinyl";
                    toolStripMain.Items.Add(addvinyl);
                }
            }
            //Remove buttons for actions which requires only one album
            else if (vistaAlbumes.SelectedItems.Count > 1)
            {
                toolStripMain.Items.RemoveByKey("view");
                toolStripMain.Items.RemoveByKey("edit");
                toolStripMain.Items.RemoveByKey("lyrics");
                toolStripMain.Items.RemoveByKey("addCD");
                toolStripMain.Items.RemoveByKey("addvinyl");

            }
            else
            {
                toolStripMain.Items.RemoveByKey("separator_song");
                toolStripMain.Items.RemoveByKey("delete");
                toolStripMain.Items.RemoveByKey("view");
                toolStripMain.Items.RemoveByKey("edit");
                toolStripMain.Items.RemoveByKey("addCD");
                toolStripMain.Items.RemoveByKey("addvinyl");
                toolStripMain.Items.RemoveByKey("lyrics");
            }
        }
        public void ApplyFilter(Filter filter)
        {
            filtered = true;
            Log.PrintMessage("Applying filter", MessageType.Info);
            Stopwatch stopwatch = Stopwatch.StartNew();
            IEnumerable<AlbumData> query = null;
            HashSet<AlbumData> albumContainsSong = new();
            switch (ViewType)
            {
                case ViewType.Digital:
                    query = Kernel.Collection.Albums.Values;
                    break;
                case ViewType.CD:
                    query = Kernel.Collection.GetCDAlbums();
                    break;
                case ViewType.Vinyl:
                    query = Kernel.Collection.GetVinylAlbums();
                    break;
                case ViewType.Cassette_Tape:
                    break;
                default:
                    break;
            }
            //Get albums if there is a song title
            if (!string.IsNullOrEmpty(filter.ContainsSongTitle))
            {

                albumContainsSong = Utils.GetAlbumsWithSongTitle(query.ToList(), filter.ContainsSongTitle);
            }
            //from album in Kernel.Collection.Albums select album;
            if (albumContainsSong.Count == 0) //this means we found an album with the searched song title
            {
                if (!string.IsNullOrEmpty(filter.Artist))
                    query = from album in query where album.Artist.ToLower().Contains(filter.Artist) select album;
                if (!string.IsNullOrEmpty(filter.Title))
                    query = from album in query where album.Title.ToLower().Contains(filter.Title) select album;
            }
            else
            {
                query = albumContainsSong;
                if (!string.IsNullOrEmpty(filter.Artist))
                    query = from album in albumContainsSong where album.Artist.ToLower().Contains(filter.Artist) select album;
                if (!string.IsNullOrEmpty(filter.Title))
                    query = from album in albumContainsSong where album.Title.ToLower().Contains(filter.Title) select album;
            }
            stopwatch.Stop();
            Log.PrintMessage("", MessageType.Correct, stopwatch, TimeType.Milliseconds);
            List<AlbumData> list = query.ToList();
            LoadFilteredView(list);
        }
        public void ApplySearchFilter(Filter filter)
        {
            filtered = true;
            IEnumerable<AlbumData> query = null;
            HashSet<AlbumData> albumContainsSong = new();
            //Get albums if there is a song title
            switch (ViewType)
            {
                case ViewType.Digital:
                    query = Kernel.Collection.Albums.Values;
                    break;
                case ViewType.CD:
                    query = Kernel.Collection.GetCDAlbums();
                    break;
                case ViewType.Vinyl:
                    query = Kernel.Collection.GetVinylAlbums();
                    break;
                case ViewType.Cassette_Tape:
                    break;
                default:
                    break;
            }
            if (!string.IsNullOrEmpty(filter.ContainsSongTitle))
            {
                albumContainsSong = Utils.GetAlbumsWithSongTitle(query.ToList(), filter.ContainsSongTitle);
            }
            //Now, filter all the albums by artist and title.
            query = from album in query where album.ID.ToLower().Contains(filter.Artist) || album.ID.ToLower().Contains(filter.Title) select album;

            List<AlbumData> list = query.ToList();
            list.AddRange(albumContainsSong);
            //filter duplicates
            list = list.GroupBy(album => album.ID).Select(g => g.FirstOrDefault()).ToList();
            LoadFilteredView(list);
        }
        public void ResetFilter()
        {
            filtered = false;
            //Delete everything in the view and reload the database, it's quick
            LoadView();
        }
        private void LoadFilteredView(List<AlbumData> albums)
        {
            Kernel.Collection.FilteredAlbums = albums;
            vistaAlbumes.BeginUpdate();
            vistaAlbumes.Items.Clear();
            //Determine view type
            switch (ViewType)
            {
                case ViewType.Digital:
                    ListViewItem[] items = new ListViewItem[albums.Count];
                    int i = 0;
                    foreach (AlbumData a in albums)
                    {
                        String[] datos = a.ToStringArray();
                        items[i] = new ListViewItem(datos);
                        i++;
                    }
                    vistaAlbumes.Items.AddRange(items);
                    labelGeneralInfo.Text = "Num albums: " + albums.Count + Environment.NewLine +
                                            "Total duration: " + Kernel.Collection.GetTotalTime(albums);
                    labelGeneralInfo.Location = new Point((panelSidebar.Width - labelGeneralInfo.Width) / 2, labelGeneralInfo.Location.Y);
                    break;
                case ViewType.CD:
                    //Iterate for each CD and if it's the same album, add it.
                    TimeSpan time = TimeSpan.Zero;
                    foreach (var album in albums)
                    {
                        foreach (var cd in Kernel.Collection.CDS)
                        {
                            if(cd.Album.ID == album.ID)
                            {
                                string[] data = cd.ToStringArray();
                                ListViewPhysicalAlbum item = new(data);
                                item.ID = cd.Id;
                                time = time.Add(cd.Length);
                                vistaAlbumes.Items.Add(item);
                            }
                        }
                    }
                    labelGeneralInfo.Text = "Num CDS: " + vistaAlbumes.Items.Count + Environment.NewLine +
                                            "Total duration: " + time.ToString();
                    labelGeneralInfo.Location = new Point((panelSidebar.Width - labelGeneralInfo.Width) / 2, labelGeneralInfo.Location.Y);
                    break;
                case ViewType.Vinyl:
                    time = TimeSpan.Zero;
                    foreach (var album in albums)
                    {
                        foreach (var vinyl in Kernel.Collection.Vinyls)
                        {
                            if(album.ID == vinyl.Album.ID)
                            {
                                string[] data = vinyl.ToStringArray();
                                ListViewPhysicalAlbum item = new(data);
                                item.ID = vinyl.Id;
                                time = time.Add(vinyl.Length);
                                vistaAlbumes.Items.Add(item);
                            }
                        }
                    }

                    labelGeneralInfo.Text = "Num Vinyls: " + vistaAlbumes.Items.Count + Environment.NewLine +
                                            "Total duration: " + time.ToString();
                    labelGeneralInfo.Location = new Point((panelSidebar.Width - labelGeneralInfo.Width) / 2, labelGeneralInfo.Location.Y);
                    break;
                case ViewType.Cassette_Tape:
                    break;
                default:
                    break;
            }



            vistaAlbumes.EndUpdate();

        }

        private AlbumData GetSelectedAlbumFromView()
        {
            ListViewPhysicalAlbum physicalItem = null;
            
            switch (ViewType)
            {
                case ViewType.Digital:
                    var item = vistaAlbumes.SelectedItems[0];
                    string query = item.SubItems[0].Text + Kernel.SearchSeparator + item.SubItems[1].Text;
                    selectedAlbum = Kernel.Collection.GetAlbum(query);
                    return Kernel.Collection.GetAlbum(query);
                case ViewType.CD:
                    item = (ListViewPhysicalAlbum)vistaAlbumes.SelectedItems[0];
                    return Kernel.Collection.GetCDById(physicalItem.ID).Album;
                case ViewType.Vinyl:
                    item = (ListViewPhysicalAlbum)vistaAlbumes.SelectedItems[0];
                    return Kernel.Collection.GetVinylByID(physicalItem.ID).Album;
                case ViewType.Cassette_Tape:
                default:
                    return null;
            }
        }
        
        //private void DarkMode()
        //{
        //    // styles for dark mode
        //    var backgroundColor = Color.FromArgb(12, 12, 12);
        //    var foregroundColor = Color.White;
        //    var panelColor = Color.FromArgb(28, 28, 28);
        //    var buttonColor = Color.FromArgb(44, 44, 44);
        //    var actionButtonColor = Color.DodgerBlue;
        //    var buttonBorderSize = 0;
        //    var buttonFlatStyle = FlatStyle.Flat;
        //    var textboxBorderStyle = BorderStyle.None;
        //    foreach (ToolStripMenuItem item in barraPrincipal.Items)
        //    {
        //        item.BackColor = backgroundColor;
        //        item.ForeColor = foregroundColor;
        //    }
        //    foreach (Control control in Controls)
        //    {
        //        switch (control.GetType().ToString())
        //        {
        //            default:
        //                control.BackColor = backgroundColor;
        //                control.ForeColor = foregroundColor;
        //                break;
        //        }
        //        this.BackColor = backgroundColor;
        //        this.ForeColor = foregroundColor;

        //    }
        //}
        
        private void CreateCDFromSelectionAndAdd()
        {
            //string seleccion = vistaAlbumes.SelectedItems[0].SubItems[0].Text + Kernel.SearchSeparator + vistaAlbumes.SelectedItems[0].SubItems[1].Text;
            //AlbumData a = Kernel.Collection.GetAlbum(seleccion);
            AlbumData a = GetSelectedAlbumFromView();
            CreateCD formCD = new CreateCD(ref a);
            formCD.Show();
        }
        
        private void CreateVinylRecordFromSelectionAndAdd()
        {
            //string seleccion = vistaAlbumes.SelectedItems[0].SubItems[0].Text + Kernel.SearchSeparator + vistaAlbumes.SelectedItems[0].SubItems[1].Text;
            //AlbumData a = Kernel.Collection.GetAlbum(seleccion);
            AlbumData a = GetSelectedAlbumFromView();
            CreateVinylCassette formV = new(ref a);
            formV.Show();
        }
        #region Events
        private void OrdenarColumnas(object sender, ColumnClickEventArgs e)
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
            vistaAlbumes.Sort();
            Dictionary<string, AlbumData> nuevaLista = new();
            string[] s = null;
            if (!filtered)
            {
                switch (ViewType)
                {
                    case ViewType.Digital:
                        s = new string[Kernel.Collection.Albums.Count];
                        break;
                    case ViewType.CD:
                        s = new string[Kernel.Collection.CDS.Count];
                        break;
                    case ViewType.Vinyl:
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (ViewType)
                {
                    case ViewType.Digital:
                        s = new string[Kernel.Collection.FilteredAlbums.Count];
                        break;
                    case ViewType.CD:
                        break;
                    case ViewType.Vinyl:
                        break;
                    default:
                        break;
                }
            }
            for (int i = 0; i < s.Length; i++)
            {
                s[i] = vistaAlbumes.Items[i].SubItems[0].Text + Kernel.SearchSeparator + vistaAlbumes.Items[i].SubItems[1].Text;
                AlbumData a = Kernel.Collection.GetAlbum(s[i]);
                nuevaLista.Add(vistaAlbumes.Items[i].SubItems[0].Text + Kernel.SearchSeparator + vistaAlbumes.Items[i].SubItems[1].Text, a);
            }
            if (!filtered)
                Kernel.Collection.ChangeAlbums(ref nuevaLista);
            else
                Kernel.Collection.FilteredAlbums = nuevaLista.Values.ToList();
            vistaAlbumes.Refresh();
        }
        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void CreateNewAlbum(object sender, EventArgs e)
        {
            CreateAlbum agregarAlbum = new CreateAlbum();
            agregarAlbum.Show();
            LoadView();
        }


        private void vistaAlbumes_KeyDown(object sender, KeyEventArgs e)
        {
            if (vistaAlbumes.SelectedItems.Count == 1 && (e.KeyCode == Keys.C && e.Control))
            {
                string i;
                i = CopyAlbumToClipboard(vistaAlbumes.SelectedIndices[0]);
                Clipboard.SetText(i);
                Log.Instance.PrintMessage("Copied " + i + " to the clipboard", MessageType.Info);
            }
            if (e.Control && e.KeyCode == Keys.A)
            {
                selecting = true;
                for (int i = 0; i < vistaAlbumes.Items.Count; i++)
                {
                    vistaAlbumes.Items[i].Selected = true;
                }
                selecting = false;
            }
            if (e.KeyCode == Keys.F5)
            {
                LoadView();
            }
            if (e.KeyCode == Keys.Escape)
            {
                selecting = true;
                for (int i = 0; i < vistaAlbumes.Items.Count; i++)
                {
                    vistaAlbumes.Items[i].Selected = false;
                }
                selecting = false;
            }
            if (e.KeyCode == Keys.Enter)
            {
                ShowSelectedAlbum();
            }
            if (e.KeyCode == Keys.F11)
            {
                Player.Instancia.Show();
            }
            if (e.Control && e.KeyCode == Keys.L && vistaAlbumes.SelectedItems.Count == 1)
                OpenLyricsForSelectedAlbum(null, null);
        }

        private void vistaAlbumes_ItemMouseHover(object sender, ListViewItemMouseHoverEventArgs e)
        {

        }

        private void vistaAlbumes_SelectedIndexChanged(object sender, EventArgs e)
        {
            ManageSongIcons();
            selectedAlbum = null;
            if (selecting)
                return;
            if (!deleting)
            {
                if (panelSidebar.Visible)
                {
                    if (vistaAlbumes.SelectedItems.Count == 0) //set the sidebar with no cover
                        UpdateSidebar(selectedAlbum);
                    else if (vistaAlbumes.SelectedItems.Count == 1)
                        UpdateSidebar(GetSelectedAlbumFromView());
                }
                //PENDING FIX
                //TimeSpan seleccion = new TimeSpan();

                //foreach (ListViewItem selItem in vistaAlbumes.SelectedItems)
                //{
                //    string search = selItem.SubItems[0].Text + Kernel.SearchSeparator + selItem.SubItems[1].Text;
                //    seleccion += Kernel.Collection.Albums[search].Length;
                //    //if (!filtered)
                //    //    seleccion += Kernel.Collection.Albums[selItem.Index].Length;
                //    //else
                //    //    seleccion += Kernel.Collection.FilteredAlbums[selItem.Index].Length;
                //}
                //duracionSeleccionada.Text = Kernel.LocalTexts.GetString("dur_total") + ": " + seleccion.ToString();
            }
        }
        private void borrarseleccionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteSelectedAlbums(ViewType);
        }
        private void guardarcomo_Click(object sender, EventArgs e)
        {
            SaveFileDialog guardarComo = new SaveFileDialog();
            guardarComo.Filter = Kernel.LocalTexts.GetString("archivo") + ".csv(*.csv)|*.csv";
            guardarComo.InitialDirectory = Environment.CurrentDirectory;
            if (guardarComo.ShowDialog() == DialogResult.OK)
            {
                guardarDiscos(Path.GetFullPath(guardarComo.FileName), (SaveType)ViewType);
            }
        }

        private void generarAlbumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.PrintMessage("Visualizando álbum al azar", MessageType.Info);
            if (vistaAlbumes.Items.Count == 0)
            {
                Log.PrintMessage("Cancelado por no haber álbumes", MessageType.Warning);
                MessageBox.Show(Kernel.LocalTexts.GetString("error_noAlbumes"), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Random generador = new Random();
            switch (ViewType)
            {
                case ViewType.Digital:
                    int ganador = generador.Next(0, Kernel.Collection.Albums.Count);
                    //AlbumData a = Kernel.Collection.Albums[ganador];
                    //AlbumViewer vistazo = new AlbumViewer(ref a);
                    //vistazo.Show();
                    break;
                case ViewType.CD:
                    int ganadorCD = generador.Next(0, Kernel.Collection.CDS.Count);
                    CompactDisc cd = Kernel.Collection.CDS[ganadorCD];
                    AlbumViewer vistazocd = new AlbumViewer(ref cd);
                    vistazocd.Show();
                    break;
                case ViewType.Vinyl:
                    break;
                default:
                    break;
            }
        }

        private void acercaDeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About form = new About();
            form.ShowDialog();
            //fixes memory leak
            form.Dispose();
        }
        private void NewDatabase(object sender, EventArgs e)
        {
            DialogResult respuesta = MessageBox.Show(Kernel.LocalTexts.GetString("guardarBD"), "", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            switch (respuesta)
            {
                case DialogResult.Cancel:
                    break;
                case DialogResult.Yes:
                    SaveFileDialog guardarComo = new SaveFileDialog();
                    guardarComo.InitialDirectory = Environment.CurrentDirectory;
                    switch (ViewType)
                    {
                        case ViewType.Digital:
                            guardarComo.Filter = Kernel.LocalTexts.GetString("archivo") + ".csv(*.csv)|*.csv";
                            if (guardarComo.ShowDialog() == DialogResult.OK)
                            {
                                guardarDiscos(Path.GetFullPath(guardarComo.FileName), SaveType.Digital);
                            }
                            Kernel.Collection.Albums.Clear();
                            break;
                        case ViewType.CD:
                            guardarComo.Filter = Kernel.LocalTexts.GetString("archivo") + ".json(*.json)|*.json";
                            if (guardarComo.ShowDialog() == DialogResult.OK)
                            {
                                guardarDiscos(Path.GetFullPath(guardarComo.FileName.Replace(".json", "") + "-CD.json"), SaveType.CD);
                            }
                            Kernel.Collection.CDS.Clear();
                            break;
                        case ViewType.Vinyl:
                            break;
                        case ViewType.Cassette_Tape:
                            break;
                        default:
                            break;

                    }
                    vistaAlbumes.Items.Clear();
                    break;
                case DialogResult.No:
                    vistaAlbumes.Items.Clear();
                    Kernel.Collection.Clear();
                    break;
                default:
                    break;
            }
        }
        private void crearCDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateCDFromSelectionAndAdd();
        }
        private void colorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog DC = new ColorDialog();
            ThreadExceptionDialog td = new ThreadExceptionDialog(new InsufficientMemoryException());
            td.ShowDialog();
            DC.ShowDialog();
            vistaAlbumes.SelectedItems[0].BackColor = DC.Color;
        }

        private void testToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (!Program.ModoOscuro)
            {
                Program.ModoOscuro = true;
                testToolStripMenuItem1.Checked = true;
                Color fondoOscuro = Color.FromArgb(30, 30, 30);
                Color menuOscuro = Color.FromArgb(45, 45, 45);
                BackColor = fondoOscuro;
                vistaAlbumes.BackColor = fondoOscuro;
                vistaAlbumes.ForeColor = Color.White;
                barraPrincipal.BackColor = menuOscuro;
                barraPrincipal.ForeColor = Color.White;
                barraAbajo.BackColor = menuOscuro;
                barraAbajo.ForeColor = Color.White;
                foreach (ToolStripMenuItem menu in barraPrincipal.Items)
                {
                    foreach (ToolStripMenuItem item in menu.DropDownItems)
                    {
                        item.BackColor = menuOscuro;
                        item.ForeColor = Color.White;
                    }
                }
            }
            else
            {
                Program.ModoOscuro = false;
                testToolStripMenuItem1.Checked = Program.ModoOscuro;
                BackColor = SystemColors.ControlLightLight;
                ForeColor = SystemColors.WindowText;
                vistaAlbumes.BackColor = SystemColors.ControlLightLight;
                vistaAlbumes.ForeColor = SystemColors.WindowText;
                barraPrincipal.BackColor = SystemColors.ControlLightLight;
                barraPrincipal.ForeColor = SystemColors.WindowText;
                barraAbajo.BackColor = SystemColors.ControlLightLight;
                barraAbajo.ForeColor = SystemColors.WindowText;
                foreach (ToolStripMenuItem menu in barraPrincipal.Items)
                {
                    foreach (ToolStripMenuItem item in menu.DropDownItems)
                    {
                        item.BackColor = SystemColors.ControlLightLight;
                        item.ForeColor = Color.Black;
                    }
                }
            }
        }

        private void vistaAlbumes_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            if (Program.ModoOscuro)
            {
                e.DrawDefault = false;
                using (Brush hBr = new SolidBrush(Color.FromArgb(30, 30, 30)))
                {
                    e.Graphics.FillRectangle(hBr, e.Bounds);
                    e.Graphics.DrawLine(SystemPens.ControlLightLight, e.Bounds.X, e.Bounds.Y, e.Bounds.X, e.Bounds.Bottom);
                }
                using (Font f = new Font("Segoe UI", 9.5f, FontStyle.Regular))
                {
                    TextRenderer.DrawText(e.Graphics, e.Header.Text, f, e.Bounds, Color.White, TextFormatFlags.Left);
                }

            }
            else
                e.DrawDefault = true;
            //vistaAlbumes.OwnerDraw = false;
            return;
        }
        private string CopyAlbumToClipboard(int AlbumIndex)
        {
            AlbumData album = Kernel.Collection.GetAlbum(AlbumIndex, filtered);
            return album.ToClipboard();
        }
        private void copiarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string i = null;
            switch (ViewType)
            {
                case ViewType.Digital:
                    i = CopyAlbumToClipboard(vistaAlbumes.SelectedIndices[0]);
                    break;
                case ViewType.CD:
                    break;
                case ViewType.Vinyl:
                    break;
                default:
                    break;
            }

            Clipboard.SetText(i);
            Log.PrintMessage("Copied " + i + " to the clipboard", MessageType.Info);
        }

        private void viewCDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ViewType = ViewType.CD;
            LoadView();
            digitalToolStripMenuItem.Checked = false;
            UpdateViewInfo();
        }

        private void digitalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ViewType = ViewType.Digital;
            vierCDToolStripMenuItem.Checked = false;
            LoadView();
            UpdateViewInfo();
        }

        private void cargarDiscosLegacyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.PrintMessage("Opening from file", MessageType.Info);
            openFileDialog1.InitialDirectory = Environment.CurrentDirectory;
            openFileDialog1.Filter = Kernel.LocalTexts.GetString("archivo") + " .mdb (*.mdb)|*.mdb | " + Kernel.LocalTexts.GetString("archivo") + " .csv|*.csv";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fichero = openFileDialog1.FileName;
                Kernel.LoadCSVAlbums(fichero);
            }
            LoadView();
        }

        private void digitalToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Log.PrintMessage("Opening from file", MessageType.Info);
            openFileDialog1.InitialDirectory = Environment.CurrentDirectory;
            openFileDialog1.Filter = Kernel.LocalTexts.GetString("archivo") + " .csv (*.csv)|*.csv";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fichero = openFileDialog1.FileName;
                Kernel.LoadCSVAlbums(fichero);
            }
        }

        private void CargarCDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.PrintMessage("Opening from file", MessageType.Info);
            openFileDialog1.InitialDirectory = Environment.CurrentDirectory;
            openFileDialog1.Filter = Kernel.LocalTexts.GetString("archivo") + " .json (*.json)|*.json";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fichero = openFileDialog1.FileName;
                Kernel.LoadCD(fichero);
            }
            LoadView();
        }
        private void playSpotifyAlbumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AlbumData a = Kernel.Collection.GetAlbum(vistaAlbumes.SelectedIndices[0], filtered); //it fucking works! no es O(1)
            Log.PrintMessage("Trying to play " + a.ToString(), MessageType.Info);
            if (string.IsNullOrEmpty(a.IdSpotify))
            {
                try
                {
                    Log.PrintMessage("Fetching Spotify URI", MessageType.Info);
                    SpotifyAPI.Web.SimpleAlbum album = Kernel.Spotify.ReturnAlbum(a.GetSpotifySearchLabel());
                    Kernel.Spotify.PlayAlbum(album.Id);
                }
                catch (SpotifyAPI.Web.APIException ex)
                {
                    Log.PrintMessage(ex.Message, MessageType.Warning);
                }
            }
            else
            {
                try
                {
                    Kernel.Spotify.PlayAlbum(a.IdSpotify);
                }
                catch (SpotifyAPI.Web.APIException ex)
                {
                    Log.PrintMessage(ex.Message, MessageType.Warning);
                }
            }
        }

        private void SaveActualDatabase(object sender, EventArgs e)
        {
            switch (ViewType)
            {
                case ViewType.Digital:
                    Kernel.SaveAlbums("discos.csv", SaveType.Digital, false);
                    break;
                case ViewType.CD:
                    Kernel.SaveAlbums("cd.json", SaveType.CD);
                    break;
                case ViewType.Vinyl:
                    break;
                case ViewType.Cassette_Tape:
                    break;
                default:
                    break;
            }
            toolStripButtonSaveDatabase.Image = Properties.Resources.diskette;
        }

        private void reproductorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Player.Instancia.Show();
        }

        private void abrirCDMenuItem_Click(object sender, EventArgs e)
        {
            OpenDisc AD = new OpenDisc();
            AD.ShowDialog();
        }

        private void OpenLyricsForSelectedAlbum(object sender, EventArgs e)
        {
            AlbumData a = GetSelectedAlbumFromView();
            Song cancion = a.GetSong(0);
            LyricsViewer VL = new LyricsViewer(cancion);
            VL.Show();
        }

        private void verLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.ShowLog();
        }

        private void nuevoAlbumDesdeCarpetaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog browserDialog = new FolderBrowserDialog();
            DialogResult result = browserDialog.ShowDialog();
            if(result != DialogResult.Cancel)
                Kernel.CreateAndAddAlbumFromFolder(browserDialog.SelectedPath);
        }

        private void configToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConfigForm configForm = new ConfigForm();
            configForm.ShowDialog();
        }

        private void clickDerechoMenuContexto_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            switch (ViewType)
            {
                case ViewType.Digital:
                    clickDerechoMenuContexto.Items.Find("createVinylToolStripMenuItem", false)[0].Visible = true;
                    clickDerechoMenuContexto.Items.Find("crearCDToolStripMenuItem", false)[0].Visible = true;
                    break;
                case ViewType.Vinyl:
                case ViewType.CD:
                    clickDerechoMenuContexto.Items.Find("createVinylToolStripMenuItem", false)[0].Visible = false;
                    clickDerechoMenuContexto.Items.Find("crearCDToolStripMenuItem", false)[0].Visible = false;
                    break;
                default:
                    break;
            }
            if(vistaAlbumes.SelectedItems.Count == 0)
            {
                foreach (ToolStripItem item in clickDerechoMenuContexto.Items)
                {
                    item.Enabled = false;
                }
            }
        }
        private void MainForm_Resize(object sender, EventArgs e)
        {
            vistaAlbumes.Size = Size - margins;
            if (!panelSidebar.Visible)
                vistaAlbumes.Width = Width - 20;
            else
            {
                panelSidebar.Height = vistaAlbumes.Height;
            }
        }

        private void searchSpotifyStripMenuItem_Click(object sender, EventArgs e)
        {
            SearchSpotify busquedaSpotifyForm = new SearchSpotify();
            busquedaSpotifyForm.ShowDialog();
        }

        private async void linkSpotifyStripMenuItem_Click(object sender, EventArgs e)
        {
            bool cancelado = false;
            DialogResult eleccion = MessageBox.Show(Kernel.LocalTexts.GetString("avisoSpotify"), "", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (eleccion == DialogResult.Yes)
            {
                Stopwatch espera = Stopwatch.StartNew();
                Log.Instance.PrintMessage("Linking with Spotify", MessageType.Info);
                await Kernel.Spotify.InitStreamMode();
                while (!Kernel.Spotify.IsSpotifyReady())
                {
                    if (espera.Elapsed.TotalSeconds >= 20)
                    {
                        cancelado = true;
                        break;
                    }
                    await System.Threading.Tasks.Task.Delay(50);
                }
                if (cancelado)
                {
                    Log.PrintMessage("Linking cancelled.", MessageType.Warning);
                    MessageBox.Show(Kernel.LocalTexts.GetString("errorVinculacion"));
                    Kernel.InternetAvaliable(true);
                    return;
                }
                try
                {
                    if (!Kernel.Spotify.UserIsPremium())
                    {
                        Log.PrintMessage("User is not premium", MessageType.Warning);
                        MessageBox.Show(Kernel.LocalTexts.GetString("noPremium"));
                    }
                    else
                        Log.PrintMessage("User is premium", MessageType.Info);
                    linkSpotifyStripMenuItem.Visible = false;
                    importSpotifyStripMenuItem.Enabled = true;
                    Player.Instancia.SpotifyEncendido();
                }
                catch (SpotifyAPI.Web.APIException ex)
                {
                    Log.PrintMessage(ex.Message, MessageType.Warning);
                    linkSpotifyStripMenuItem.Visible = true;

                    Kernel.InternetAvaliable(false);
                    throw;
                }
                MessageBox.Show(Kernel.LocalTexts.GetString("linked_ok"), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void importSpotifyStripMenuItem_Click(object sender, EventArgs e)
        {
            Kernel.Spotify.GetUserAlbums();
        }
        private void panelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //The user clicked when it was checked.
            if (!showSidebarToolStripMenuItem.Checked)
            {
                //HIDE
                HideSidebar();
            }
            else
            {
                //SHOW
                ShowSidebar();
            }

        }
        private void copiarImagenStrip_Click(object sender, EventArgs e)
        {
            Clipboard.SetImage(pictureBoxSidebarCover.Image);
            Log.Instance.PrintMessage("Sent cover to clipboard. Size: " + pictureBoxSidebarCover.Image.Size, MessageType.Correct);
        }

        private void contextMenuSidebarCover_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            sidebarCopyImageToolStripMenuItem.Enabled = true;
            if (selectedAlbum is null)
                sidebarCopyImageToolStripMenuItem.Enabled = false;

        }

        private void OpenDatabase(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void vistaAlbumes_Click(object sender, EventArgs e)
        {
            selectedAlbum = null;
            if (vistaAlbumes.SelectedItems.Count == 1)
            {
                selectedAlbum = Kernel.Collection.GetAlbum(vistaAlbumes.SelectedIndices[0], filtered);
                UpdateSidebar(GetSelectedAlbumFromView());
            }
            else if (vistaAlbumes.SelectedItems.Count == 0)
                UpdateSidebar(GetSelectedAlbumFromView());
        }
        private void vistaAlbumes_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ShowSelectedAlbum();
        }
        private void OpenFilterWindow(object sender, EventArgs e)
        {
            FilterForm filterForm = new();
            filterForm.Show();
        }
        private void toolStripTextBoxSearch_Click(object sender, EventArgs e)
        {
            toolStripTextBoxSearch.Text = String.Empty;
            ResetFilter();
        }

        private void toolStripTextBoxSearch_TextChanged(object sender, EventArgs e)
        {
            Filter f = new();
            f.Title = toolStripTextBoxSearch.Text;
            f.Artist = toolStripTextBoxSearch.Text;
            f.ContainsSongTitle = toolStripTextBoxSearch.Text;
            ApplySearchFilter(f);
        }

        private void vinylToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            CreateVinylRecordFromSelectionAndAdd();
        }

        private void vinylToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ViewType = ViewType.Vinyl;
            LoadView();
            digitalToolStripMenuItem.Checked = false;
            UpdateViewInfo();
        }
        #endregion
    }
}