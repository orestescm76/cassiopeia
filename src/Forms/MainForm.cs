using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections.Generic;
using Cassiopeia.src.Forms;
namespace Cassiopeia
{
    public enum ViewType
    {
        Digital,
        CD,
        Vinilo,
    }
    public enum SaveType
    {
        Digital, CD, Vinilo
    }
    public partial class MainForm : Form
    {
        private bool borrando;
        public static string BusquedaSpotify;
        private ListViewItemComparer lvwColumnSorter;
        public ViewType TipoVista;
        private delegate void SafeCallBringFront();
        Log Log = Log.Instance;
        public MainForm()
        {
            InitializeComponent();
            BusquedaSpotify = "";
            borrando = false;

            lvwColumnSorter = new ListViewItemComparer();
            vistaAlbumes.ListViewItemSorter = lvwColumnSorter;
            vistaAlbumes.MultiSelect = true;

            vistaAlbumes.View = View.Details;
            PonerTextos();
            vistaAlbumes.FullRowSelect = true;
            duracionSeleccionada.AutoSize = true;
            barraAbajo.Visible = true;
            barraAbajo.Font = new Font("Segoe UI", 10);
            duracionSeleccionada.Text = Kernel.LocalTexts.GetString("dur_total") + ": 00:00:00";
            vistaAlbumes.DrawItem += (sender, e) => { e.DrawDefault = true; };
            vistaAlbumes.DrawSubItem += (sender, e) => { e.DrawDefault = true; };
            vistaAlbumes.OwnerDraw = true;
            if (Config.LinkedWithSpotify)
                vincularToolStripMenuItem.Visible = false;
            cargarDiscosLegacyToolStripMenuItem.Visible = false;
            vistaAlbumes.Font = Config.FontView;
            Log.PrintMessage("Main form created", MessageType.Correct);
        }
        public void Refrescar()
        {
            vistaAlbumes.Font = Config.FontView;
            PonerTextos();
            CargarVista();
        }
        public void EnableInternet(bool i)
        {
            buscarEnSpotifyToolStripMenuItem.Enabled = i;
            vincularToolStripMenuItem.Enabled = i;
        }
        public void DesactivarVinculacion()
        {
            vincularToolStripMenuItem.Visible = false;
        }
        public void ActivarReproduccionSpotify()
        {
            spotifyToolStripMenuItem.Enabled = true;
        }
        private void CargarVista()
        {
            Log.Instance.PrintMessage("Loading view" + TipoVista, MessageType.Info);
            vistaAlbumes.Items.Clear();
            Stopwatch crono = Stopwatch.StartNew();
            switch (TipoVista)
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
                    break;
                case ViewType.CD:
                    ListViewItem[] cds = new ListViewItem[Kernel.Collection.CDS.Count];
                    vistaAlbumes.Columns[5].Width = 0;
                    int j = 0;
                    foreach (CompactDisc cd in Kernel.Collection.CDS)
                    {
                        String[] datos = cd.ToStringArray();
                        cds[j] = new ListViewItem(datos);
                        j++;
                    }
                    vistaAlbumes.Items.AddRange(cds);
                    break;
                case ViewType.Vinilo:
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
            Text = Kernel.LocalTexts.GetString("titulo_ventana_principal") + " " + Kernel.Version + " Codename " + Kernel.CodeName;
#else
            Text = Kernel.LocalTexts.GetString("titulo_ventana_principal");
#endif
            archivoMenuItem1.Text = Kernel.LocalTexts.GetString("archivo");
            agregarAlbumToolStripMenuItem.Text = Kernel.LocalTexts.GetString("agregar_album");
            abrirToolStripMenuItem.Text = Kernel.LocalTexts.GetString("abrir_registros");
            salirToolStripMenuItem.Text = Kernel.LocalTexts.GetString("salir");
            vistaAlbumes.Columns[0].Text = Kernel.LocalTexts.GetString("artista");
            vistaAlbumes.Columns[1].Text = Kernel.LocalTexts.GetString("titulo");
            vistaAlbumes.Columns[2].Text = Kernel.LocalTexts.GetString("año");
            vistaAlbumes.Columns[3].Text = Kernel.LocalTexts.GetString("duracion");
            vistaAlbumes.Columns[4].Text = Kernel.LocalTexts.GetString("genero");
            buscarEnSpotifyToolStripMenuItem.Text = Kernel.LocalTexts.GetString("buscar_Spotify");
            guardarcomo.Text = Kernel.LocalTexts.GetString("guardar") + "...";
            seleccionToolStripMenuItem.Text = Kernel.LocalTexts.GetString("seleccion");
            adminMenu.Text = Kernel.LocalTexts.GetString("admin");
            generarAlbumToolStripMenuItem.Text = Kernel.LocalTexts.GetString("generar_azar");
            borrarseleccionToolStripMenuItem.Text = Kernel.LocalTexts.GetString("borrar_seleccion");
            acercaDeToolStripMenuItem.Text = Kernel.LocalTexts.GetString("acerca") + " " + Kernel.LocalTexts.GetString("titulo_ventana_principal");
            nuevoToolStripMenuItem.Text = Kernel.LocalTexts.GetString("nuevaBD");
            clickDerechoMenuContexto.Items[0].Text = Kernel.LocalTexts.GetString("crearCD");
            cargarDiscosLegacyToolStripMenuItem.Text = Kernel.LocalTexts.GetString("cargarDiscosLegacy");
            verToolStripMenuItem.Text = Kernel.LocalTexts.GetString("ver");
            digitalToolStripMenuItem.Text = Kernel.LocalTexts.GetString("digital");
            copiarToolStripMenuItem.Text = Kernel.LocalTexts.GetString("copiar");
            digitalToolStripMenuItem.Text = Kernel.LocalTexts.GetString("digital");
            vincularToolStripMenuItem.Text = Kernel.LocalTexts.GetString("vincular");
            spotifyToolStripMenuItem.Text = Kernel.LocalTexts.GetString("reproducirSpotify");
            reproductorToolStripMenuItem.Text = Kernel.LocalTexts.GetString("reproductor");
            abrirCDMenuItem.Text = Kernel.LocalTexts.GetString("abrirCD") + "...";
            verLyricsToolStripMenuItem.Text = Kernel.LocalTexts.GetString("verLyrics");
            verLogToolStripMenuItem.Text = Kernel.LocalTexts.GetString("verLog");
            nuevoAlbumDesdeCarpetaToolStripMenuItem.Text = Kernel.LocalTexts.GetString("nuevoAlbumDesdeCarpeta");
            configToolStripMenuItem.Text = Kernel.LocalTexts.GetString("configuracion");
            UpdateViewInfo();
        }
        private void UpdateViewInfo()
        {
            switch (TipoVista)
            {
                case ViewType.Digital:
                    toolStripStatusLabelViewInfo.Text = Kernel.LocalTexts.GetString("digital");
                    break;
                case ViewType.CD:
                    toolStripStatusLabelViewInfo.Text = "CD";
                    break;
                case ViewType.Vinilo:
                    break;
                default:
                    break;
            }
        }

        private void borrarAlbumesSeleccionados(ViewType tipoVista)
        {

            Stopwatch crono = Stopwatch.StartNew();
            borrando = true;
            int cuantos = vistaAlbumes.SelectedItems.Count;
            ListViewItem[] itemsABorrar = new ListViewItem[cuantos];
            switch (tipoVista)
            {
                case ViewType.Digital:
                    Console.WriteLine("Deleting " + vistaAlbumes.SelectedItems.Count + " albums");
                    for (int i = 0; i < cuantos; i++)
                    {
                        itemsABorrar[i] = vistaAlbumes.SelectedItems[i];
                    }
                    for (int i = 0; i < vistaAlbumes.SelectedIndices.Count; i++)
                    {
                        try
                        {
                            AlbumData a = Kernel.Collection.GetAlbum(vistaAlbumes.SelectedIndices[i]);
                            Kernel.Collection.RemoveAlbum(ref a);
                            for (int j = 0; j < cuantos; j++)
                            {
                                vistaAlbumes.Items.Remove(itemsABorrar[j]);
                            }
                        }
                        catch (InvalidOperationException)
                        {
                            MessageBox.Show(Kernel.LocalTexts.GetString("errorBorrado"));
                            continue;
                        }
                    }
                    break;
                case ViewType.CD:
                    Console.WriteLine("Deleting " + vistaAlbumes.SelectedItems.Count + " CD");
                    for (int i = 0; i < cuantos; i++)
                    {
                        itemsABorrar[i] = vistaAlbumes.SelectedItems[i];
                    }
                    for (int i = 0; i < cuantos; i++)
                    {
                        CompactDisc cdaborrar = Kernel.Collection.GetCDById(vistaAlbumes.SelectedItems[i].SubItems[5].Text);
                        CompactDisc cdd = cdaborrar;
                        Kernel.Collection.DeleteCD(ref cdaborrar);
                        cdd.AlbumData.CanBeRemoved = true;

                        foreach (CompactDisc cd in Kernel.Collection.CDS)
                        {
                            if (cd.AlbumData == cdd.AlbumData)
                                cd.AlbumData.CanBeRemoved = false;
                        }
                    }
                    for (int i = 0; i < cuantos; i++)
                    {
                        vistaAlbumes.Items.Remove(itemsABorrar[i]);
                    }
                    break;
                case ViewType.Vinilo:
                    break;
                default:
                    break;
            }
            borrando = false;
            duracionSeleccionada.Text = Kernel.LocalTexts.GetString("dur_total") + ": 00:00:00";
            vistaAlbumes.Refresh();
            crono.Stop();
            Log.Instance.PrintMessage("Deletion completed.", MessageType.Correct, crono, TimeType.Milliseconds);
        }

        private void guardarDiscos(string nombre, SaveType tipoGuardado)
        {
            if (tipoGuardado == SaveType.Digital)
                Kernel.SaveAlbums(nombre, SaveType.Digital);
            else
                Kernel.SaveAlbums(nombre, tipoGuardado, true);
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
            List<AlbumData> nuevaLista = new List<AlbumData>();
            string[] s = null;
            switch (TipoVista)
            {
                case ViewType.Digital:
                    s = new string[Kernel.Collection.Albums.Count];
                    break;
                case ViewType.CD:
                    s = new string[Kernel.Collection.CDS.Count];
                    break;
                case ViewType.Vinilo:
                    break;
                default:
                    break;
            }
            for (int i = 0; i < s.Length; i++)
            {
                s[i] = vistaAlbumes.Items[i].SubItems[0].Text + "_" + vistaAlbumes.Items[i].SubItems[1].Text;
                AlbumData a = Kernel.Collection.GetAlbum(s[i]);
                nuevaLista.Add(a);
            }
            Kernel.Collection.ChangeList(ref nuevaLista);
            vistaAlbumes.Refresh();
        }
        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void agregarAlbumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            agregarAlbum agregarAlbum = new agregarAlbum();
            agregarAlbum.Show();
            CargarVista();
        }


        private void vistaAlbumes_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Stopwatch cronoTotal = Stopwatch.StartNew();
            switch (TipoVista)
            {
                case ViewType.Digital:
                    foreach (ListViewItem item in vistaAlbumes.SelectedItems)
                    {
                        AlbumData a = Kernel.Collection.GetAlbum(item.Index);
                        visualizarAlbum vistazo = new visualizarAlbum(ref a);
                        vistazo.Show();
                    }
                    break;
                case ViewType.CD:
                    foreach (ListViewItem cdViewItem in vistaAlbumes.SelectedItems)
                    {
                        string b = cdViewItem.SubItems[0].Text + '_' + cdViewItem.SubItems[1].Text;
                        CompactDisc cd;
                        Kernel.Collection.GetAlbum(b, out cd);
                        visualizarAlbum visCD = new visualizarAlbum(ref cd);
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
                foreach (ListViewItem item in vistaAlbumes.Items)
                {
                    item.Selected = true;
                }
            }
            if (e.KeyCode == Keys.F5)
            {
                CargarVista();
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
                Reproductor.Instancia.Show();
            }
        }

        private void vistaAlbumes_ItemMouseHover(object sender, ListViewItemMouseHoverEventArgs e)
        {

        }

        private void vistaAlbumes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!borrando)
            {
                TimeSpan seleccion = new TimeSpan();
                foreach (ListViewItem album in vistaAlbumes.SelectedItems)
                {
                    String a = album.SubItems[0].Text + "_" + album.SubItems[1].Text;
                    AlbumData ad = Kernel.Collection.GetAlbum(a);
                    seleccion += ad.Length;
                }
                duracionSeleccionada.Text = Kernel.LocalTexts.GetString("dur_total") + ": " + seleccion.ToString();
            }
        }
        private void masCortoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AlbumData a = Kernel.Collection.Albums.First();
            for (int i = 1; i < Kernel.Collection.Albums.Count; i++)
            {
                if (a.Length > Kernel.Collection.Albums[i].Length)
                    a = Kernel.Collection.Albums[i];
            }
            visualizarAlbum v = new visualizarAlbum(ref a);
            v.ShowDialog();
        }

        private void masLargoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AlbumData a = Kernel.Collection.Albums.First();
            for (int i = 1; i < Kernel.Collection.Albums.Count; i++)
            {
                if (a.Length < Kernel.Collection.Albums[i].Length)
                    a = Kernel.Collection.Albums[i];
            }
            visualizarAlbum v = new visualizarAlbum(ref a);
            v.ShowDialog();
        }

        private void borrarseleccionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            borrarAlbumesSeleccionados(TipoVista);
        }
        private void guardarcomo_Click(object sender, EventArgs e)
        {
            SaveFileDialog guardarComo = new SaveFileDialog();
            guardarComo.Filter = Kernel.LocalTexts.GetString("archivo") + ".csv(*.csv)|*.csv";
            guardarComo.InitialDirectory = Environment.CurrentDirectory;
            if (guardarComo.ShowDialog() == DialogResult.OK)
            {
                guardarDiscos(Path.GetFullPath(guardarComo.FileName), (SaveType)TipoVista);
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
            switch (TipoVista)
            {
                case ViewType.Digital:
                    int ganador = generador.Next(0, Kernel.Collection.Albums.Count);
                    AlbumData a = Kernel.Collection.Albums[ganador];
                    visualizarAlbum vistazo = new visualizarAlbum(ref a);
                    vistazo.Show();
                    break;
                case ViewType.CD:
                    int ganadorCD = generador.Next(0, Kernel.Collection.CDS.Count);
                    CompactDisc cd = Kernel.Collection.CDS[ganadorCD];
                    visualizarAlbum vistazocd = new visualizarAlbum(ref cd);
                    vistazocd.Show();
                    break;
                case ViewType.Vinilo:
                    break;
                default:
                    break;
            }
        }

        private void buscarEnSpotifyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SearchSpotify busquedaSpotifyForm = new SearchSpotify();
            busquedaSpotifyForm.ShowDialog();
        }

        private void acercaDeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            acercaDe form = new acercaDe();
            form.Show();
        }
        private void nuevoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult respuesta = MessageBox.Show(Kernel.LocalTexts.GetString("guardarBD"), "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (respuesta == DialogResult.Yes)
            {
                SaveFileDialog guardarComo = new SaveFileDialog();
                guardarComo.Filter = Kernel.LocalTexts.GetString("archivo") + ".json(*.json)|*.json";
                guardarComo.InitialDirectory = Environment.CurrentDirectory;
                if (guardarComo.ShowDialog() == DialogResult.OK)
                {
                    guardarDiscos(Path.GetFullPath(guardarComo.FileName), SaveType.Digital);
                    guardarDiscos(Path.GetFullPath(guardarComo.FileName.Replace(".json", "") + "-CD.json"), SaveType.CD);
                }
            }
            vistaAlbumes.Items.Clear();
            Kernel.Collection.Clear();
        }
        private void crearCDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string seleccion = vistaAlbumes.SelectedItems[0].SubItems[0].Text + "_" + vistaAlbumes.SelectedItems[0].SubItems[1].Text;
            AlbumData a = Kernel.Collection.GetAlbum(seleccion);

            if (a.Length.TotalMinutes < 80)
            {
                CrearCD formCD = new CrearCD(ref a);
                formCD.Show();
            }
            else
            {
                short numDiscos = (short)Math.Ceiling((a.Length.TotalMinutes / 80));
                CrearCD fCD = new CrearCD(ref a, numDiscos);
                fCD.ShowDialog();
                for (short i = 2; i <= numDiscos; i++)
                {
                    CompactDisc temp = Kernel.Collection.CDS.Last();
                    CrearCD formCD = new CrearCD(ref temp, i);
                    formCD.ShowDialog();
                }
            }
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
            switch (TipoVista)
            {
                case ViewType.Digital:
                    i = CopyAlbumToClipboard(vistaAlbumes.SelectedIndices[0]);
                    break;
                case ViewType.CD:
                    break;
                case ViewType.Vinilo:
                    break;
                default:
                    break;
            }

            Clipboard.SetText(i);
            Log.PrintMessage("Copied " + i + " to the clipboard", MessageType.Info);
        }

        private void cdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TipoVista = ViewType.CD;
            CargarVista();
            digitalToolStripMenuItem.Checked = false;
            UpdateViewInfo();
        }

        private void digitalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TipoVista = ViewType.Digital;
            cdToolStripMenuItem.Checked = false;
            CargarVista();
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
            CargarVista();
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
            CargarVista();
        }

        private void vincularToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool cancelado = false;
            DialogResult eleccion = MessageBox.Show(Kernel.LocalTexts.GetString("avisoSpotify"), "", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (eleccion == DialogResult.Yes)
            {
                Stopwatch espera = Stopwatch.StartNew();
                Kernel.Spotify.Restart();
                Kernel.Spotify.SpotifyVinculado();
                while (!Kernel.Spotify.cuentaLista)
                {
                    //deadlock, sincrono
                    if (espera.Elapsed.TotalSeconds >= 30)
                    {
                        cancelado = true;
                        break;
                    }
                }
                if (cancelado)
                {
                    Log.PrintMessage("Se ha cancelado la vinculación por tiempo de espera.", MessageType.Warning);
                    MessageBox.Show(Kernel.LocalTexts.GetString("errorVinculacion"));
                    return;
                }
                if (Kernel.Spotify._spotify.GetPrivateProfile().Product != "premium")
                {
                    Log.PrintMessage("El usuario no tiene premium, no podrá usar spotify desde el Gestor", MessageType.Warning);
                    MessageBox.Show(Kernel.LocalTexts.GetString("noPremium"));
                    spotifyToolStripMenuItem.Enabled = false;
                    vincularToolStripMenuItem.Enabled = false;
                }
                Reproductor.Instancia.SpotifyEncendido();
            }
            else return;
        }
        private void spotifyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AlbumData a = Kernel.Collection.GetAlbum(vistaAlbumes.SelectedIndices[0]); //it fucking works! no es O(1)
            Log.PrintMessage(a.ToString(), MessageType.Info);
            if (string.IsNullOrEmpty(a.IdSpotify))
            {
                SpotifyAPI.Web.Models.SimpleAlbum album = Kernel.Spotify.DevolverAlbum(a.GetSpotifySearchLabel());
                if (object.ReferenceEquals(a, null) || object.ReferenceEquals(album, null))
                {
                    Log.PrintMessage("Album was null..., spotifyToolStripMenuItem_Click()", MessageType.Error);
                }
                else
                {
                    SpotifyAPI.Web.Models.ErrorResponse err = Kernel.Spotify.PlayAlbum(album.Id);
                    if (err is not null && err.Error is not null)
                    {
                        Log.PrintMessage(err.Error.Message, MessageType.Error, "spotifyToolStripMenuItem_Click()");
                        MessageBox.Show(err.Error.Message);
                    }
                    else
                        Log.PrintMessage("Playing OK", MessageType.Correct);
                }
            }
            else
            {
                SpotifyAPI.Web.Models.ErrorResponse err = Kernel.Spotify.PlayAlbum(a.IdSpotify);
                if (err != null && err.Error != null)
                {
                    Log.PrintMessage(err.Error.Message, MessageType.Error, "spotifyToolStripMenuItem_Click()");
                    MessageBox.Show(err.Error.Message);
                }
                else
                    Log.PrintMessage("Playing OK", MessageType.Correct);
            }
        }

        private void guardarCSVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Kernel.SaveAlbums("discosCSV.csv", SaveType.Digital);
            MessageBox.Show("Done!");
        }

        private void reproductorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Reproductor.Instancia.Show();
        }

        private void abrirCDMenuItem_Click(object sender, EventArgs e)
        {
            AbrirDisco AD = new AbrirDisco();
            AD.ShowDialog();
        }

        private void verLyricsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AlbumData a = Kernel.Collection.GetAlbum(vistaAlbumes.SelectedIndices[0]);
            Song cancion = a.GetSong(0);
            VisorLyrics VL = new VisorLyrics(cancion);
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
                BarraCarga bC = new BarraCarga(carpeta.GetFiles().Length);
                bC.Show();
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
                    bC.Progreso();
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
                bC.Close();
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
            switch (TipoVista)
            {
                case ViewType.Digital:
                    clickDerechoMenuContexto.Items[0].Visible = true;
                    break;
                case ViewType.CD:
                    clickDerechoMenuContexto.Items[0].Visible = false;
                    break;
                case ViewType.Vinilo:
                    break;
                default:
                    break;
            }
        }
        #endregion
    }
}
