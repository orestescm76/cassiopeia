using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections.Generic;
using Cassiopeia.src.Classes;

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
        Digital, CD, Vinilo
    }
    public partial class MainForm : Form
    {
        private bool borrando;
        private ListViewItemComparer lvwColumnSorter;
        public ViewType ViewType;
        private delegate void SafeCallBringFront();
        Log Log = Log.Instance;
        Size margins;
        AlbumData selectedAlbum = null;
        public MainForm()
        {
            InitializeComponent();
            borrando = false;

            lvwColumnSorter = new ListViewItemComparer();
            vistaAlbumes.ListViewItemSorter = lvwColumnSorter;
            vistaAlbumes.MultiSelect = true;

            vistaAlbumes.View = View.Details;
            PonerTextos();
            vistaAlbumes.FullRowSelect = true;
            duracionSeleccionada.AutoSize = true;
            barraAbajo.Font = new Font("Segoe UI", 10);
            duracionSeleccionada.Text = Kernel.LocalTexts.GetString("dur_total") + ": 00:00:00";
            vistaAlbumes.DrawItem += (sender, e) => { e.DrawDefault = true; };
            vistaAlbumes.DrawSubItem += (sender, e) => { e.DrawDefault = true; };
            vistaAlbumes.OwnerDraw = true;
            if (Config.LinkedWithSpotify)
                linkSpotifyStripMenuItem.Visible = false;
            else
                importSpotifyStripMenuItem.Enabled = false;
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
            Log.PrintMessage("Main form created", MessageType.Correct);
        }
        public void Refrescar()
        {
            vistaAlbumes.Font = Config.FontView;
            PonerTextos();
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
            vistaAlbumes.Items.Clear();
            Stopwatch crono = Stopwatch.StartNew();
            switch (ViewType)
            {
                case ViewType.Digital:
                    ListViewItem[] items = new ListViewItem[Kernel.Collection.Albums.Count];
                    int i = 0;
                    foreach (AlbumData a in Kernel.Collection.Albums)
                    {
                        String[] datos = a.ToStringArray();
                        items[i] = new ListViewItem(datos);
                        i++;
                    }
                    vistaAlbumes.Items.AddRange(items);
                    labelGeneralInfo.Text = "Num albums: " + Kernel.Collection.Albums.Count + Environment.NewLine +
                    "Total duration: " + Kernel.Collection.GetTotalTime(Kernel.Collection.Albums);
                    labelGeneralInfo.Location = new Point((panelSidebar.Width - labelGeneralInfo.Width) / 2, labelGeneralInfo.Location.Y);
                    break;
                case ViewType.CD:
                    src.Classes.ListViewPhysicalAlbum[] cds = new src.Classes.ListViewPhysicalAlbum[Kernel.Collection.CDS.Count];
                    int j = 0;
                    foreach (CompactDisc cd in Kernel.Collection.CDS)
                    {
                        String[] datos = cd.ToStringArray();
                        cds[j] = new src.Classes.ListViewPhysicalAlbum(datos);
                        cds[j].ID = cd.Id;
                        j++;
                    }
                    vistaAlbumes.Items.AddRange(cds);
                    labelGeneralInfo.Text = "Num CDS: " + Kernel.Collection.CDS.Count + Environment.NewLine +
                    "Total duration: " + Kernel.Collection.GetTotalTime(Kernel.Collection.CDS);
                    labelGeneralInfo.Location = new Point((panelSidebar.Width - labelGeneralInfo.Width) / 2, labelGeneralInfo.Location.Y);
                    break;
                case ViewType.Vinyl:
                    break;
                default:
                    break;
            }

            crono.Stop();
            Log.Instance.PrintMessage("Loaded", MessageType.Correct, crono, TimeType.Milliseconds);
        }
        private void PonerTextos()
        {
#if DEBUG
            Text = Kernel.LocalTexts.GetString("titulo_ventana_principal") + " " + Kernel.Version + " Codename " + Kernel.CodeName + " DEBUG";
#else
            Text = Kernel.LocalTexts.GetString("titulo_ventana_principal");
#endif
            archivoMenuItem1.Text = Kernel.LocalTexts.GetString("archivo");

            agregarAlbumToolStripMenuItem.Text = Kernel.LocalTexts.GetString("agregar_album");
            toolStripButtonNewAlbum.ToolTipText = Kernel.LocalTexts.GetString("agregar_album");

            nuevoToolStripMenuItem.Text = Kernel.LocalTexts.GetString("nuevaBD");
            toolStripButtonNewDatabase.ToolTipText = Kernel.LocalTexts.GetString("nuevaBD");

            toolStripButtonSaveDatabase.ToolTipText = "Guardar";

            abrirToolStripMenuItem.Text = Kernel.LocalTexts.GetString("abrir_registros");
            salirToolStripMenuItem.Text = Kernel.LocalTexts.GetString("salir");
            vistaAlbumes.Columns[0].Text = Kernel.LocalTexts.GetString("artista");
            vistaAlbumes.Columns[1].Text = Kernel.LocalTexts.GetString("titulo");
            vistaAlbumes.Columns[2].Text = Kernel.LocalTexts.GetString("año");
            vistaAlbumes.Columns[3].Text = Kernel.LocalTexts.GetString("duracion");
            vistaAlbumes.Columns[4].Text = Kernel.LocalTexts.GetString("genero");
            searchSpotifyStripMenuItem.Text = Kernel.LocalTexts.GetString("buscar_Spotify");
            guardarcomo.Text = Kernel.LocalTexts.GetString("guardar") + "...";
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
                    break;
                default:
                    break;
            }
        }

        private void DeleteSelectedAlbums(ViewType tipoVista)
        {
            Stopwatch crono = Stopwatch.StartNew();
            vistaAlbumes.BeginUpdate();
            borrando = true;
            switch (tipoVista)
            {
                case ViewType.Digital:
                    Console.WriteLine("Deleting " + vistaAlbumes.SelectedItems.Count + " albums");
                    try
                    {
                        
                        while (vistaAlbumes.SelectedIndices.Count != 0)
                        {
                            int i = vistaAlbumes.SelectedIndices[0];
                            AlbumData a = Kernel.Collection.GetAlbum(i);
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
                    break;
                default:
                    break;
            }
            borrando = false;
            vistaAlbumes.EndUpdate();
            duracionSeleccionada.Text = Kernel.LocalTexts.GetString("dur_total") + ": 00:00:00";
            crono.Stop();
            LoadView();
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
            if(a is not null)
            {
                if (pictureBoxSidebarCover.Image != Properties.Resources.albumdesconocido)
                    pictureBoxSidebarCover.Image = null;
                if (!string.IsNullOrEmpty(a.CoverPath))
                    pictureBoxSidebarCover.Image = Image.FromFile(a.CoverPath);
                else
                    pictureBoxSidebarCover.Image = Properties.Resources.albumdesconocido;
                ////Doing this will allow me to replace album cover and not locking the file
                //Image cover;
                //using (var temp = new Bitmap(a.CoverPath))
                //    cover = new Bitmap(temp);
                //pictureBoxSidebarCover.Image = cover;
                labelInfoAlbum.Location = new Point(0, labelInfoAlbum.Location.Y);
                labelInfoAlbum.Text = a.Artist + Environment.NewLine +
                                      a.Title + "(" + a.Year + ")" + Environment.NewLine +
                                       a.Length + Environment.NewLine;    
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
                if(!string.IsNullOrEmpty(cd.Album.CoverPath))
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
                                       cd.Length + Environment.NewLine+
                                       Kernel.LocalTexts.GetString("estado_exterior") + ": " + Kernel.LocalTexts.GetString(cd.EstadoExterior.ToString()) + Environment.NewLine+
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
            Width -= panelSidebar.Width-20;
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
                string fichero = openFileDialog.FileName;
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
            LoadView();
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
            LinkedList<AlbumData> nuevaLista = new LinkedList<AlbumData>();
            string[] s = null;
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
            for (int i = 0; i < s.Length; i++)
            {
                s[i] = vistaAlbumes.Items[i].SubItems[0].Text + "/**/" + vistaAlbumes.Items[i].SubItems[1].Text;
                AlbumData a = Kernel.Collection.GetAlbum(s[i]);
                nuevaLista.AddLast(a);
            }
            Kernel.Collection.ChangeList(ref nuevaLista);
            vistaAlbumes.Refresh();
        }
        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void CreateNewAlbum(object sender, EventArgs e)
        {
            CreateAlbum agregarAlbum = new CreateAlbum();
            agregarAlbum.Show();
            LoadView();
        }


        private void vistaAlbumes_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Stopwatch cronoTotal = Stopwatch.StartNew();
            switch (ViewType)
            {
                case ViewType.Digital:
                    foreach (ListViewItem item in vistaAlbumes.SelectedItems)
                    {
                        AlbumData a = Kernel.Collection.GetAlbum(item.Index);
                        AlbumViewer vistazo = new AlbumViewer(ref a);
                        vistazo.Show();
                    }
                    break;
                case ViewType.CD:
                    foreach (ListViewItem cdViewItem in vistaAlbumes.SelectedItems)
                    {
                        string b = cdViewItem.SubItems[0].Text + "/**/" + cdViewItem.SubItems[1].Text;
                        CompactDisc cd;
                        Kernel.Collection.GetAlbum(b, out cd);
                        AlbumViewer visCD = new AlbumViewer(ref cd);
                        visCD.Show();
                    }
                    break;
            }
            cronoTotal.Stop();
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
                Log.Instance.PrintMessage("Begin selection", MessageType.Info);
                Stopwatch crono = Stopwatch.StartNew();
                vistaAlbumes.BeginUpdate();
                for (int i = 0; i < vistaAlbumes.Items.Count; i++)
                {
                    vistaAlbumes.Items[i].Selected = true;
                }
                //foreach (ListViewItem item in vistaAlbumes.Items)
                //{
                    
                //}
                vistaAlbumes.EndUpdate();
                //vistaAlbumes.Refresh();
                crono.Stop();
                Log.Instance.PrintMessage("Done", MessageType.Info, crono, TimeType.Milliseconds);
            }
            if (e.KeyCode == Keys.F5)
            {
                LoadView();
            }
            if (e.KeyCode == Keys.Escape)
            {
                foreach (ListViewItem item in vistaAlbumes.Items)
                {
                    item.Selected = false;
                }
            }
            if (e.KeyCode == Keys.Enter)
            {
                vistaAlbumes_MouseDoubleClick(null, null);
            }
            if (e.KeyCode == Keys.F11)
            {
                Player.Instancia.Show();
            }
        }

        private void vistaAlbumes_ItemMouseHover(object sender, ListViewItemMouseHoverEventArgs e)
        {

        }

        private void vistaAlbumes_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedAlbum = null;
            if (!borrando)
            {
                if (vistaAlbumes.SelectedItems.Count == 1)
                {
                    selectedAlbum = Kernel.Collection.GetAlbum(vistaAlbumes.SelectedIndices[0]);
                    UpdateSidebar(selectedAlbum);
                }
                else if (vistaAlbumes.SelectedItems.Count == 0)
                    UpdateSidebar(selectedAlbum);
                    
                //if (vistaAlbumes.SelectedIndices.Count >= 1)
                //{
                //    selectedAlbum = Kernel.Collection.GetAlbum(vistaAlbumes.SelectedIndices[0]);
                //    if(vistaAlbumes.SelectedItems[0] is ListViewPhysicalAlbum)
                //    {
                //        ListViewPhysicalAlbum listViewPhysicalAlbum = (ListViewPhysicalAlbum)vistaAlbumes.SelectedItems[0];
                //        UpdateSidebar(Kernel.Collection.GetCDById(listViewPhysicalAlbum.ID));
                //    }
                //    else
                //        UpdateSidebar(selectedAlbum);
                //}
                //else
                //    UpdateSidebar(selectedAlbum);
                TimeSpan seleccion = new TimeSpan();
                foreach (ListViewItem album in vistaAlbumes.SelectedItems)
                {
                    String a = album.SubItems[0].Text + "/**/" + album.SubItems[1].Text;
                    AlbumData ad = Kernel.Collection.GetAlbum(a);
                    seleccion += ad.Length;
                }
                duracionSeleccionada.Text = Kernel.LocalTexts.GetString("dur_total") + ": " + seleccion.ToString();
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
            form.Show();
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
            string seleccion = vistaAlbumes.SelectedItems[0].SubItems[0].Text + "/**/" + vistaAlbumes.SelectedItems[0].SubItems[1].Text;
            AlbumData a = Kernel.Collection.GetAlbum(seleccion);
            CreateCD formCD = new CreateCD(ref a);
            formCD.Show();
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
            AlbumData album = Kernel.Collection.GetAlbum(AlbumIndex);
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
            AlbumData a = Kernel.Collection.GetAlbum(vistaAlbumes.SelectedIndices[0]); //it fucking works! no es O(1)
            Log.PrintMessage("Trying to play "+a.ToString(), MessageType.Info);
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
                    Kernel.SaveAlbums("discosCSV.csv", SaveType.Digital, false);
                    break;
                case ViewType.CD:
                    Kernel.SaveAlbums("discosCD.json", SaveType.CD);
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

        private void verLyricsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AlbumData a = Kernel.Collection.GetAlbum(vistaAlbumes.SelectedIndices[0]);
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
            Log.PrintMessage("Creating an album from a directory.", MessageType.Info);

            AlbumData a = new AlbumData();
            FolderBrowserDialog browserDialog = new FolderBrowserDialog();
            DialogResult result = browserDialog.ShowDialog();
            //To avoid a random song order, i create an array to store the songs. 150 should be big enough.
            Song[] tempStorage = new Song[150];
            int numSongs = 0; //to keep track of how many songs i've addded.
            if (result != DialogResult.Cancel)
            {
                Stopwatch crono = Stopwatch.StartNew();
                DirectoryInfo carpeta = new DirectoryInfo(browserDialog.SelectedPath);
                Config.LastOpenedDirectory = carpeta.FullName;
                foreach (var filename in carpeta.GetFiles())
                {

                    switch (Path.GetExtension(filename.FullName))
                    {
                        case ".mp3":
                        case ".ogg":
                        case ".flac":
                            MetadataSong LM = new MetadataSong(filename.FullName);
                            if (a.NeedsMetadata())
                            {
                                a.Title = LM.AlbumFrom;
                                a.Artist = LM.Artist;
                                a.Year = (short)LM.Year;
                                if (!(LM.Cover is null) && !File.Exists("cover.jpg"))
                                {
                                    Bitmap cover = new Bitmap(LM.Cover);
                                    cover.Save(carpeta.FullName + "\\cover.jpg", ImageFormat.Jpeg);
                                    a.CoverPath = carpeta.FullName + "\\cover.jpg";
                                }
                            }
                            Song c = new Song(LM.Title, (int)LM.Length.TotalMilliseconds, false);
                            if (LM.TrackNumber != 0) //A music file with no track number? Can happen. Instead, do the normal process.
                            {
                                tempStorage[LM.TrackNumber - 1] = c;
                                numSongs++;
                            }
                            else
                                a.AddSong(c);
                            c.SetAlbum(a);
                            c.Path = filename.FullName;
                            LM.Dispose();
                            break;
                        case ".jpg":
                            if (filename.Name == "folder.jpg" || filename.Name == "cover.jpg")
                                a.CoverPath = filename.FullName;
                            break;
                    }
                }
                if (numSongs != 0) //The counter has been updated and songs had a track number.
                {
                    //This list goes to the album.
                    List<Song> songList = new List<Song>();
                    for (int i = 0; i < numSongs; i++)
                    {
                        //Copy the correct song order.
                        songList.Add(tempStorage[i]);
                    }
                    a.Songs = songList;
                }
                a.SoundFilesPath = carpeta.FullName;
                Kernel.Collection.AddAlbum(ref a);
                crono.Stop();
                Log.PrintMessage("Operation completed", MessageType.Correct, crono, TimeType.Milliseconds);
                Refrescar();
            }
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
                    clickDerechoMenuContexto.Items[0].Visible = true;
                    break;
                case ViewType.CD:
                    clickDerechoMenuContexto.Items[0].Visible = false;
                    break;
                case ViewType.Vinyl:
                    break;
                default:
                    break;
            }
        }
        private void MainForm_Resize(object sender, EventArgs e)
        {
            vistaAlbumes.Size = Size - margins;
            if(!panelSidebar.Visible)
                vistaAlbumes.Width = Width-20;
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
                Log.Instance.PrintMessage("Reiniciando Spotify", MessageType.Info);
                await Kernel.Spotify.LinkSpotify();
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
                    Log.PrintMessage("Se ha cancelado la vinculación por tiempo de espera.", MessageType.Warning);
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
                MessageBox.Show("Linked ok");
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
        #endregion
    }
}
