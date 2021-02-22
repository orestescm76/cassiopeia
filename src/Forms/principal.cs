using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections.Generic;
using aplicacion_musica.src.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace aplicacion_musica
{
    public enum TipoVista
    {
        Digital,
        CD,
        Vinilo,
    }
    public enum TipoGuardado
    {
        Digital, CD, Vinilo
    }
    public partial class principal : Form
    {
        private bool borrando;
        public static string BusquedaSpotify;
        private ListViewItemComparer lvwColumnSorter;
        public TipoVista TipoVista;
        Log Log;
        public principal()
        {
            Stopwatch crono = Stopwatch.StartNew();
            Log = Log.Instance;
            InitializeComponent();
            BusquedaSpotify = "";
            borrando = false;

            lvwColumnSorter = new ListViewItemComparer();
            vistaAlbumes.ListViewItemSorter = lvwColumnSorter;
            vistaAlbumes.MultiSelect = true;

            Application.ApplicationExit += new EventHandler(salidaAplicacion);

            vistaAlbumes.View = View.Details;
            PonerTextos();
            vistaAlbumes.FullRowSelect = true;
            duracionSeleccionada.AutoSize = true;
            barraAbajo.Visible = true;
            barraAbajo.Font = new Font("Segoe UI", 10);
            duracionSeleccionada.Text = Program.LocalTexts.GetString("dur_total") + ": 00:00:00";
            vistaAlbumes.DrawItem += (sender, e) => { e.DrawDefault = true; };
            vistaAlbumes.DrawSubItem += (sender, e) => { e.DrawDefault = true; };
            vistaAlbumes.OwnerDraw = true;
            crono.Stop();
            if (Program.SpotifyActivado)
                vincularToolStripMenuItem.Visible = false;
            cargarDiscosLegacyToolStripMenuItem.Visible = false;
            Log.ImprimirMensaje("Formulario principal creado", TipoMensaje.Correcto, crono);
        }
        public void Refrescar() { PonerTextos(); CargarVista(); }
        public void HayInternet(bool i)
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
            Log.Instance.ImprimirMensaje("Cargando vista" + TipoVista, TipoMensaje.Info);
            vistaAlbumes.Items.Clear();
            Stopwatch crono = Stopwatch.StartNew();
            switch (TipoVista)
            {
                case TipoVista.Digital:
                    ListViewItem[] items = new ListViewItem[Program.Collection.Albums.Count];
                    int i = 0;
                    foreach (AlbumData a in Program.Collection.Albums)
                    {
                        String[] datos = a.ToStringArray();
                        items[i] = new ListViewItem(datos);
                        i++;
                    }
                    vistaAlbumes.Items.AddRange(items);
                    break;
                case TipoVista.CD:
                    ListViewItem[] cds = new ListViewItem[Program.Collection.CDS.Count];
                    vistaAlbumes.Columns[5].Width = 0;
                    int j = 0;
                    foreach (CompactDisc cd in Program.Collection.CDS)
                    {
                        String[] datos = cd.toStringArray();
                        cds[j] = new ListViewItem(datos);
                        j++;
                    }
                    vistaAlbumes.Items.AddRange(cds);
                    break;
                case TipoVista.Vinilo:
                    break;
                default:
                    break;
            }

            crono.Stop();
            Log.Instance.ImprimirMensaje("Cargado", TipoMensaje.Correcto, crono);
        }
        private void PonerTextos()
        {
            Text = Program.LocalTexts.GetString("titulo_ventana_principal") + " " + Program.Version + " Codename " + Program.CodeName;
            archivoMenuItem1.Text = Program.LocalTexts.GetString("archivo");
            agregarAlbumToolStripMenuItem.Text = Program.LocalTexts.GetString("agregar_album");
            abrirToolStripMenuItem.Text = Program.LocalTexts.GetString("abrir_registros");
            salirToolStripMenuItem.Text = Program.LocalTexts.GetString("salir");
            vistaAlbumes.Columns[0].Text = Program.LocalTexts.GetString("artista");
            vistaAlbumes.Columns[1].Text = Program.LocalTexts.GetString("titulo");
            vistaAlbumes.Columns[2].Text = Program.LocalTexts.GetString("año");
            vistaAlbumes.Columns[3].Text = Program.LocalTexts.GetString("duracion");
            vistaAlbumes.Columns[4].Text = Program.LocalTexts.GetString("genero");
            buscarEnSpotifyToolStripMenuItem.Text = Program.LocalTexts.GetString("buscar_Spotify");
            guardarcomo.Text = Program.LocalTexts.GetString("guardar")+"...";
            seleccionToolStripMenuItem.Text = Program.LocalTexts.GetString("seleccion");
            adminMenu.Text = Program.LocalTexts.GetString("admin");
            generarAlbumToolStripMenuItem.Text = Program.LocalTexts.GetString("generar_azar");
            borrarseleccionToolStripMenuItem.Text = Program.LocalTexts.GetString("borrar_seleccion");
            acercaDeToolStripMenuItem.Text = Program.LocalTexts.GetString("acerca") + " " + Program.LocalTexts.GetString("titulo_ventana_principal");
            nuevoToolStripMenuItem.Text = Program.LocalTexts.GetString("nuevaBD");
            clickDerechoMenuContexto.Items[0].Text = Program.LocalTexts.GetString("crearCD");
            cargarDiscosLegacyToolStripMenuItem.Text = Program.LocalTexts.GetString("cargarDiscosLegacy");
            verToolStripMenuItem.Text = Program.LocalTexts.GetString("ver");
            digitalToolStripMenuItem.Text = Program.LocalTexts.GetString("digital");
            copiarToolStripMenuItem.Text = Program.LocalTexts.GetString("copiar");
            digitalToolStripMenuItem.Text = Program.LocalTexts.GetString("digital");
            vincularToolStripMenuItem.Text = Program.LocalTexts.GetString("vincular");
            spotifyToolStripMenuItem.Text = Program.LocalTexts.GetString("reproducirSpotify");
            reproductorToolStripMenuItem.Text = Program.LocalTexts.GetString("reproductor");
            abrirCDMenuItem.Text = Program.LocalTexts.GetString("abrirCD") + "...";
            verLyricsToolStripMenuItem.Text = Program.LocalTexts.GetString("verLyrics");
            tipografiaLyricsToolStripMenuItem.Text = Program.LocalTexts.GetString("tipografíaLyrics");
            verLogToolStripMenuItem.Text = Program.LocalTexts.GetString("verLog");
            nuevoAlbumDesdeCarpetaToolStripMenuItem.Text = Program.LocalTexts.GetString("nuevoAlbumDesdeCarpeta");
            configToolStripMenuItem.Text = Program.LocalTexts.GetString("configuracion");
            UpdateViewInfo();
        }
        private void UpdateViewInfo()
        {
            switch (TipoVista)
            {
                case TipoVista.Digital:
                    toolStripStatusLabelViewInfo.Text = Program.LocalTexts.GetString("digital");
                    break;
                case TipoVista.CD:
                    toolStripStatusLabelViewInfo.Text = "CD";
                    break;
                case TipoVista.Vinilo:
                    break;
                default:
                    break;
            }
        }
        private void OrdenarColumnas(object sender, ColumnClickEventArgs e)
        {
            Log.ImprimirMensaje("Ordenando columnas", TipoMensaje.Info);
            Stopwatch crono = Stopwatch.StartNew();
            if(e.Column == lvwColumnSorter.ColumnaAOrdenar) // Determine if clicked column is already the column that is being sorted.
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
                case TipoVista.Digital:
                    s = new string[Program.Collection.Albums.Count];
                    break;
                case TipoVista.CD:
                    s = new string[Program.Collection.CDS.Count];
                    break;
                case TipoVista.Vinilo:
                    break;
                default:
                    break;
            }
            for (int i = 0; i < s.Length; i++)
            {
                s[i] = vistaAlbumes.Items[i].SubItems[0].Text + "_" + vistaAlbumes.Items[i].SubItems[1].Text;
                AlbumData a = Program.Collection.GetAlbum(s[i]);
                nuevaLista.Add(a);
            }
            Program.Collection.ChangeList(ref nuevaLista);
            vistaAlbumes.Refresh();
            crono.Stop();
            Log.ImprimirMensaje("Ordenado", TipoMensaje.Correcto, crono);
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
        private void guardarDiscos(string nombre, TipoGuardado tipoGuardado)
        {
            if (tipoGuardado == TipoGuardado.Digital)
                Program.SaveAlbums(nombre, TipoGuardado.Digital);
            else
                Program.SaveAlbums(nombre, tipoGuardado, true);
        }
        private void salidaAplicacion(object sender, EventArgs e)
        {
            guardarDiscos("discos.csv", TipoGuardado.Digital);
            guardarDiscos("cd.json", TipoGuardado.CD);
            using (StreamWriter salida = new StreamWriter("idioma.cfg", false))
                salida.Write(Config.Idioma);
            Log.ImprimirMensaje("Apagando reproductor", TipoMensaje.Info);
            Reproductor.Instancia.Apagar();
            Reproductor.Instancia.Dispose();
        }

        private void vistaAlbumes_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Log.ImprimirMensaje("Iniciada busqueda del álbum linealmente...", TipoMensaje.Info);
            Stopwatch cronoTotal = Stopwatch.StartNew();
            switch(TipoVista)
            {
                case TipoVista.Digital:
                    foreach (ListViewItem item in vistaAlbumes.SelectedItems)
                    {
                        Stopwatch crono = Stopwatch.StartNew();
                        AlbumData a = Program.Collection.GetAlbum(item.Index);
                        crono.Stop();
                        Log.ImprimirMensajeTiempoCorto("Finalizado", TipoMensaje.Correcto, crono);
                        crono.Reset(); crono.Start();
                        visualizarAlbum vistazo = new visualizarAlbum(ref a);
                        vistazo.Show();
                        crono.Stop();
                        Log.ImprimirMensaje("Formulario creado y mostrado", TipoMensaje.Correcto, crono);
                    }
                    break;
                case TipoVista.CD:
                    foreach (ListViewItem cdViewItem in vistaAlbumes.SelectedItems)
                    {
                        Stopwatch crono = Stopwatch.StartNew();
                        string b = cdViewItem.SubItems[0].Text + '_' + cdViewItem.SubItems[1].Text;
                        CompactDisc cd;
                        Program.Collection.GetAlbum(b, out cd);
                        crono.Stop();
                        Log.ImprimirMensajeTiempoCorto("Finalizado", TipoMensaje.Correcto, crono);
                        crono.Reset(); crono.Start();
                        visualizarAlbum visCD = new visualizarAlbum(ref cd);
                        visCD.Show();
                        crono.Stop();
                        Log.ImprimirMensaje("Formulario creado y mostrado", TipoMensaje.Correcto, crono);
                    }
                    break;
            }
            cronoTotal.Stop();
            Log.ImprimirMensaje("Operación realizada",TipoMensaje.Correcto, cronoTotal);
        }

        private void vistaAlbumes_KeyDown(object sender, KeyEventArgs e)
        {
            if(vistaAlbumes.SelectedItems.Count == 1 && (e.KeyCode == Keys.C && e.Control))
            {
                string i;
                i = CopyAlbumToClipboard(vistaAlbumes.SelectedIndices[0]);
                Clipboard.SetText(i);
                Log.Instance.ImprimirMensaje("Copiado " + i + " al portapapeles", TipoMensaje.Info);
            }
            if (e.Control && e.KeyCode == Keys.A)
            {
                foreach (ListViewItem item in vistaAlbumes.Items)
                {
                    item.Selected = true;
                }
            }
            if(e.KeyCode == Keys.F5)
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
            if(e.KeyCode == Keys.Enter)
            {
                vistaAlbumes_MouseDoubleClick(null,null);
            }
            if(e.KeyCode == Keys.F11)
            {
                Reproductor.Instancia.Show();
            }
        }
        private void borrarAlbumesSeleccionados(TipoVista tipoVista)
        {

            Stopwatch crono = Stopwatch.StartNew();
            borrando = true;
            int cuantos = vistaAlbumes.SelectedItems.Count;
            ListViewItem[] itemsABorrar = new ListViewItem[cuantos];
            switch (tipoVista)
            {
                case TipoVista.Digital:
                    Console.WriteLine("Borrando " + vistaAlbumes.SelectedItems.Count + " álbumes");
                    for (int i = 0; i < cuantos; i++)
                    {
                        itemsABorrar[i] = vistaAlbumes.SelectedItems[i];
                    }
                    for (int i = 0; i < vistaAlbumes.SelectedIndices.Count; i++)
                    {
                        try
                        {
                            AlbumData a = Program.Collection.GetAlbum(vistaAlbumes.SelectedIndices[i]);
                            Program.Collection.RemoveAlbum(ref a);
                            for (int j = 0; j < cuantos; j++)
                            {
                                vistaAlbumes.Items.Remove(itemsABorrar[j]);
                            }
                        }
                        catch (InvalidOperationException)
                        {
                            MessageBox.Show(Program.LocalTexts.GetString("errorBorrado"));
                            continue;
                        }
                    }
                    break;
                case TipoVista.CD:
                    Console.WriteLine("Borrando " + vistaAlbumes.SelectedItems.Count + " CD");
                    for (int i = 0; i < cuantos; i++)
                    {
                        itemsABorrar[i] = vistaAlbumes.SelectedItems[i];
                    }
                    for (int i = 0; i < cuantos; i++)
                    {
                        CompactDisc cdaborrar = Program.Collection.GetCDById(vistaAlbumes.SelectedItems[i].SubItems[5].Text);
                        CompactDisc cdd = cdaborrar;
                        Program.Collection.DeleteCD(ref cdaborrar);
                        cdd.Album.CanBeRemoved = true;

                        foreach (CompactDisc cd in Program.Collection.CDS)
                        {
                            if (cd.Album == cdd.Album)
                                cd.Album.CanBeRemoved = false;
                        }
                    }
                    for (int i = 0; i < cuantos; i++)
                    {
                        vistaAlbumes.Items.Remove(itemsABorrar[i]);
                    }
                    break;
                case TipoVista.Vinilo:
                    break;
                default:
                    break;
            }
            borrando = false;
            duracionSeleccionada.Text = Program.LocalTexts.GetString("dur_total") + ": 00:00:00";
            vistaAlbumes.Refresh();
            crono.Stop();
            Console.WriteLine("Borrado completado en "+crono.ElapsedMilliseconds+"ms");
        }
        private void vistaAlbumes_ItemMouseHover(object sender, ListViewItemMouseHoverEventArgs e)
        {

        }

        private void vistaAlbumes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(!borrando)
            {
                TimeSpan seleccion = new TimeSpan();
                foreach (ListViewItem album in vistaAlbumes.SelectedItems)
                {
                    String a = album.SubItems[0].Text + "_" + album.SubItems[1].Text;
                    AlbumData ad = Program.Collection.GetAlbum(a);
                    seleccion += ad.Length;
                }
                duracionSeleccionada.Text = Program.LocalTexts.GetString("dur_total") + ": " + seleccion.ToString();
            }
        }
        private void masCortoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AlbumData a = Program.Collection.Albums.First();
            for (int i = 1; i < Program.Collection.Albums.Count; i++)
            {
                if (a.Length > Program.Collection.Albums[i].Length)
                    a = Program.Collection.Albums[i];
            }
            visualizarAlbum v = new visualizarAlbum(ref a);
            v.ShowDialog();
        }

        private void masLargoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AlbumData a = Program.Collection.Albums.First();
            for (int i = 1; i < Program.Collection.Albums.Count; i++)
            {
                if (a.Length < Program.Collection.Albums[i].Length)
                    a = Program.Collection.Albums[i];
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
            guardarComo.Filter = Program.LocalTexts.GetString("archivo") + ".csv(*.csv)|*.csv";
            guardarComo.InitialDirectory = Environment.CurrentDirectory;
            if(guardarComo.ShowDialog()==DialogResult.OK)
            {
                guardarDiscos(Path.GetFullPath(guardarComo.FileName), (TipoGuardado)TipoVista);
            }
        }

        private void generarAlbumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.ImprimirMensaje("Generando álbum al azar", TipoMensaje.Info);
            Stopwatch crono = Stopwatch.StartNew();
            if(vistaAlbumes.Items.Count == 0)
            {
                crono.Stop();
                Log.ImprimirMensaje("Cancelado por no haber álbumes", TipoMensaje.Advertencia);
                MessageBox.Show(Program.LocalTexts.GetString("error_noAlbumes"), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Random generador = new Random();
            switch (TipoVista)
            {
                case TipoVista.Digital:
                    int ganador = generador.Next(0, Program.Collection.Albums.Count);
                    AlbumData a = Program.Collection.Albums[ganador];
                    visualizarAlbum vistazo = new visualizarAlbum(ref a);
                    vistazo.Show();
                    break;
                case TipoVista.CD:
                    int ganadorCD = generador.Next(0, Program.Collection.CDS.Count);
                    CompactDisc cd = Program.Collection.CDS[ganadorCD];
                    visualizarAlbum vistazocd = new visualizarAlbum(ref cd);
                    vistazocd.Show();
                    break;
                case TipoVista.Vinilo:
                    break;
                default:
                    break;
            }
            crono.Stop();
            Log.ImprimirMensaje("Generado", TipoMensaje.Correcto, crono);
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
            DialogResult respuesta = MessageBox.Show(Program.LocalTexts.GetString("guardarBD"), "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (respuesta == DialogResult.Yes)
            {
                SaveFileDialog guardarComo = new SaveFileDialog();
                guardarComo.Filter = Program.LocalTexts.GetString("archivo") + ".json(*.json)|*.json";
                guardarComo.InitialDirectory = Environment.CurrentDirectory;
                if (guardarComo.ShowDialog() == DialogResult.OK)
                {
                    guardarDiscos(Path.GetFullPath(guardarComo.FileName), TipoGuardado.Digital);
                    guardarDiscos(Path.GetFullPath(guardarComo.FileName.Replace(".json", "") + "-CD.json"), TipoGuardado.CD);
                }
            }
            vistaAlbumes.Items.Clear();
            Program.Collection.Clear();
        }
        private void crearCDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string seleccion = vistaAlbumes.SelectedItems[0].SubItems[0].Text + "_" + vistaAlbumes.SelectedItems[0].SubItems[1].Text;
            AlbumData a = Program.Collection.GetAlbum(seleccion);

            if(a.Length.TotalMinutes < 80)
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
                    CompactDisc temp = Program.Collection.CDS.Last();
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
            if(!Program.ModoOscuro)
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
                foreach(ToolStripMenuItem menu in barraPrincipal.Items)
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
                    e.Graphics.FillRectangle(hBr,e.Bounds);
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
            AlbumData album = Program.Collection.GetAlbum(AlbumIndex);
            return album.ToClipboard();
        }
        private void copiarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string i = null;
            switch (TipoVista)
            {
                case TipoVista.Digital:
                    i = CopyAlbumToClipboard(vistaAlbumes.SelectedIndices[0]);
                    break;
                case TipoVista.CD:
                    break;
                case TipoVista.Vinilo:
                    break;
                default:
                    break;
            }

            Clipboard.SetText(i);
            Log.ImprimirMensaje("Copiado " + i + " al portapapeles", TipoMensaje.Info);
        }

        private void cdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TipoVista = TipoVista.CD;
            CargarVista();
            digitalToolStripMenuItem.Checked = false;
            UpdateViewInfo();
        }

        private void digitalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TipoVista = TipoVista.Digital;
            cdToolStripMenuItem.Checked = false;
            CargarVista();
            UpdateViewInfo();
        }

        private void cargarDiscosLegacyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.ImprimirMensaje("Abriendo desde fichero", TipoMensaje.Info);
            openFileDialog1.InitialDirectory = Environment.CurrentDirectory;
            openFileDialog1.Filter = Program.LocalTexts.GetString("archivo") + " .mdb (*.mdb)|*.mdb | "+Program.LocalTexts.GetString("archivo")+" .csv|*.csv";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fichero = openFileDialog1.FileName;
                Program.LoadCSVAlbums(fichero);
            }
            CargarVista();
        }

        private void digitalToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Log.ImprimirMensaje("Abriendo desde fichero", TipoMensaje.Info);
            openFileDialog1.InitialDirectory = Environment.CurrentDirectory;
            openFileDialog1.Filter = Program.LocalTexts.GetString("archivo") + " .csv (*.csv)|*.csv";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fichero = openFileDialog1.FileName;
                Program.LoadCSVAlbums(fichero);
            }
        }

        private void CargarCDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.ImprimirMensaje("Abriendo desde fichero", TipoMensaje.Info);
            openFileDialog1.InitialDirectory = Environment.CurrentDirectory;
            openFileDialog1.Filter = Program.LocalTexts.GetString("archivo") + " .json (*.json)|*.json";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fichero = openFileDialog1.FileName;
                Program.LoadCD(fichero);
            }
            CargarVista();
        }

        private void vincularToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool cancelado = false;
            DialogResult eleccion = MessageBox.Show(Program.LocalTexts.GetString("avisoSpotify"), "", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (eleccion == DialogResult.Yes)
            {
                Stopwatch espera = Stopwatch.StartNew();
                Program._spotify.Reiniciar();
                Program._spotify.SpotifyVinculado();
                while (!Program._spotify.cuentaLista)
                {
                    //deadlock, sincrono
                    if(espera.Elapsed.TotalSeconds >= 30)
                    {
                        cancelado = true;
                        break;
                    }
                }
                if(cancelado)
                {
                    Log.ImprimirMensaje("Se ha cancelado la vinculación por tiempo de espera.", TipoMensaje.Advertencia);
                    MessageBox.Show(Program.LocalTexts.GetString("errorVinculacion"));
                    return;
                }
                if (Program._spotify._spotify.GetPrivateProfile().Product != "premium")
                {
                    Log.ImprimirMensaje("El usuario no tiene premium, no podrá usar spotify desde el Gestor", TipoMensaje.Advertencia);
                    MessageBox.Show(Program.LocalTexts.GetString("noPremium"));
                    spotifyToolStripMenuItem.Enabled = false;
                    vincularToolStripMenuItem.Enabled = false;
                }
                Reproductor.Instancia.SpotifyEncendido();
            }
            else return;
        }
        private void spotifyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AlbumData a = Program.Collection.GetAlbum(vistaAlbumes.SelectedIndices[0]); //it fucking works! no es O(1)
            Log.ImprimirMensaje(a.ToString(), TipoMensaje.Info);
            if(string.IsNullOrEmpty(a.IdSpotify))
            {
                SpotifyAPI.Web.Models.SimpleAlbum album = Program._spotify.DevolverAlbum(a.GetSpotifySearchLabel());
                if (object.ReferenceEquals(a, null) || object.ReferenceEquals(album, null))
                {
                    Log.ImprimirMensaje("Album fue nulo", TipoMensaje.Error);
                }
                else
                {
                    SpotifyAPI.Web.Models.ErrorResponse err = Program._spotify.ReproducirAlbum(album.Id);
                    if (err != null && err.Error != null)
                    {
                        Log.ImprimirMensaje(err.Error.Message, TipoMensaje.Error, "spotifyToolStripMenuItem_Click()");
                        MessageBox.Show(err.Error.Message);
                    }
                    else
                        Log.ImprimirMensaje("Reproducción correcta", TipoMensaje.Correcto);
                }
            }
            else
            {
                SpotifyAPI.Web.Models.ErrorResponse err = Program._spotify.ReproducirAlbum(a.IdSpotify);
                if (err != null && err.Error != null)
                {
                    Log.ImprimirMensaje(err.Error.Message, TipoMensaje.Error, "spotifyToolStripMenuItem_Click()");
                    MessageBox.Show(err.Error.Message);
                }
                else
                    Log.ImprimirMensaje("Reproducción correcta", TipoMensaje.Correcto);
            }
        }

        private void guardarCSVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.SaveAlbums("discosCSV.csv", TipoGuardado.Digital);
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

        private void tipografiaLyricsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            Font fuente = null;
            fontDialog.Font = new Font(Config.TipografiaLyrics, 10);
            fontDialog.ShowDialog();
            fuente = fontDialog.Font;
            Config.TipografiaLyrics = fuente.FontFamily.Name;
        }

        private void verLyricsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AlbumData a = Program.Collection.GetAlbum(vistaAlbumes.SelectedIndices[0]);
            Song cancion = a.GetSong(0);
            VisorLyrics VL = new VisorLyrics(cancion);
            VL.Show();
        }

        private void verLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.MostrarLog();
        }

        private void nuevoAlbumDesdeCarpetaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AlbumData a = new AlbumData();
            CommonOpenFileDialog browserDialog = new CommonOpenFileDialog();
            browserDialog.InitialDirectory = Config.UltimoDirectorioAbierto;
            browserDialog.IsFolderPicker = true; //Selección de carpeta.
            //FolderBrowserDialog browserDialog = new FolderBrowserDialog();
            CommonFileDialogResult result = browserDialog.ShowDialog();
            if (result != CommonFileDialogResult.Cancel)
            {
                DirectoryInfo carpeta = new DirectoryInfo(browserDialog.FileName);
                Config.UltimoDirectorioAbierto = carpeta.FullName;
                BarraCarga bC = new BarraCarga(carpeta.GetFiles().Length);
                bC.Show();
                foreach (var filename in carpeta.GetFiles())
                {

                    switch (Path.GetExtension(filename.FullName))
                    {
                        case ".mp3":
                        case ".ogg":
                        case ".flac":
                            LectorMetadatos LM = new LectorMetadatos(filename.FullName);
                            if (a.NeedsMetadata())
                            {
                                a.Title = LM.Album;
                                a.Artist = LM.Artista;
                                a.Year = (short)LM.Año;
                                if (!(LM.Cover is null) && !File.Exists("cover.jpg"))
                                {
                                    Bitmap cover = new Bitmap(LM.Cover);
                                    cover.Save(carpeta.FullName + "\\cover.jpg", ImageFormat.Jpeg);
                                    a.CoverPath = carpeta.FullName + "\\cover.jpg";
                                }
                            }
                            Song c = new Song(LM.Titulo, (int)LM.Duracion.TotalMilliseconds, false);
                            c.SetAlbum(a);
                            c.Path = filename.FullName;
                            a.AddSong(c);
                            LM.Dispose();
                            break;
                        case ".jpg":
                            if (filename.Name == "folder.jpg" || filename.Name == "cover.jpg")
                                a.CoverPath = filename.FullName;
                            break;

                    }
                    bC.Progreso();
                }
                a.SoundFilesPath = carpeta.FullName;
                bC.Close();
                Program.Collection.AddAlbum(ref a);
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
                case TipoVista.Digital:
                    clickDerechoMenuContexto.Items[0].Visible = true;
                    break;
                case TipoVista.CD:
                    clickDerechoMenuContexto.Items[0].Visible = false;
                    break;
                case TipoVista.Vinilo:
                    break;
                default:
                    break;
            }
        }
    }
}
