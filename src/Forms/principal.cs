using System;
using System.Drawing;
using System.ComponentModel;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections.Generic;
using System.Globalization;
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
            duracionSeleccionada.Text = Programa.textosLocal.GetString("dur_total") + ": 00:00:00";
            vistaAlbumes.DrawItem += (sender, e) => { e.DrawDefault = true; };
            vistaAlbumes.DrawSubItem += (sender, e) => { e.DrawDefault = true; };
            vistaAlbumes.OwnerDraw = true;
            crono.Stop();
            if (Programa.SpotifyActivado)
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
                    ListViewItem[] items = new ListViewItem[Programa.miColeccion.albumes.Count];
                    int i = 0;
                    foreach (AlbumData a in Programa.miColeccion.albumes)
                    {
                        String[] datos = a.ToStringArray();
                        items[i] = new ListViewItem(datos);
                        i++;
                    }
                    vistaAlbumes.Items.AddRange(items);
                    break;
                case TipoVista.CD:
                    ListViewItem[] cds = new ListViewItem[Programa.miColeccion.cds.Count];
                    vistaAlbumes.Columns[5].Width = 0;
                    int j = 0;
                    foreach (DiscoCompacto cd in Programa.miColeccion.cds)
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
            Text = Programa.textosLocal.GetString("titulo_ventana_principal") + " " + Programa.version + " Codename " + Programa.CodeName;
            archivoMenuItem1.Text = Programa.textosLocal.GetString("archivo");
            agregarAlbumToolStripMenuItem.Text = Programa.textosLocal.GetString("agregar_album");
            abrirToolStripMenuItem.Text = Programa.textosLocal.GetString("abrir_registros");
            salirToolStripMenuItem.Text = Programa.textosLocal.GetString("salir");
            vistaAlbumes.Columns[0].Text = Programa.textosLocal.GetString("artista");
            vistaAlbumes.Columns[1].Text = Programa.textosLocal.GetString("titulo");
            vistaAlbumes.Columns[2].Text = Programa.textosLocal.GetString("año");
            vistaAlbumes.Columns[3].Text = Programa.textosLocal.GetString("duracion");
            vistaAlbumes.Columns[4].Text = Programa.textosLocal.GetString("genero");
            buscarEnSpotifyToolStripMenuItem.Text = Programa.textosLocal.GetString("buscar_Spotify");
            guardarcomo.Text = Programa.textosLocal.GetString("guardar")+"...";
            seleccionToolStripMenuItem.Text = Programa.textosLocal.GetString("seleccion");
            adminMenu.Text = Programa.textosLocal.GetString("admin");
            generarAlbumToolStripMenuItem.Text = Programa.textosLocal.GetString("generar_azar");
            borrarseleccionToolStripMenuItem.Text = Programa.textosLocal.GetString("borrar_seleccion");
            acercaDeToolStripMenuItem.Text = Programa.textosLocal.GetString("acerca") + " " + Programa.textosLocal.GetString("titulo_ventana_principal");
            nuevoToolStripMenuItem.Text = Programa.textosLocal.GetString("nuevaBD");
            clickDerechoMenuContexto.Items[0].Text = Programa.textosLocal.GetString("crearCD");
            cargarDiscosLegacyToolStripMenuItem.Text = Programa.textosLocal.GetString("cargarDiscosLegacy");
            verToolStripMenuItem.Text = Programa.textosLocal.GetString("ver");
            digitalToolStripMenuItem.Text = Programa.textosLocal.GetString("digital");
            copiarToolStripMenuItem.Text = Programa.textosLocal.GetString("copiar");
            digitalToolStripMenuItem.Text = Programa.textosLocal.GetString("digital");
            vincularToolStripMenuItem.Text = Programa.textosLocal.GetString("vincular");
            spotifyToolStripMenuItem.Text = Programa.textosLocal.GetString("reproducirSpotify");
            reproductorToolStripMenuItem.Text = Programa.textosLocal.GetString("reproductor");
            abrirCDMenuItem.Text = Programa.textosLocal.GetString("abrirCD") + "...";
            verLyricsToolStripMenuItem.Text = Programa.textosLocal.GetString("verLyrics");
            tipografiaLyricsToolStripMenuItem.Text = Programa.textosLocal.GetString("tipografíaLyrics");
            verLogToolStripMenuItem.Text = Programa.textosLocal.GetString("verLog");
            nuevoAlbumDesdeCarpetaToolStripMenuItem.Text = Programa.textosLocal.GetString("nuevoAlbumDesdeCarpeta");
            configToolStripMenuItem.Text = Programa.textosLocal.GetString("configuracion");
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
                    s = new string[Programa.miColeccion.albumes.Count];
                    break;
                case TipoVista.CD:
                    s = new string[Programa.miColeccion.cds.Count];
                    break;
                case TipoVista.Vinilo:
                    break;
                default:
                    break;
            }
            for (int i = 0; i < s.Length; i++)
            {
                s[i] = vistaAlbumes.Items[i].SubItems[0].Text + "_" + vistaAlbumes.Items[i].SubItems[1].Text;
                AlbumData a = Programa.miColeccion.devolverAlbum(s[i]);
                nuevaLista.Add(a);
            }
            Programa.miColeccion.cambiarLista(ref nuevaLista);
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
                Programa.GuardarDiscos(nombre, TipoGuardado.Digital);
            else
                Programa.GuardarDiscos(nombre, tipoGuardado, true);
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
                        AlbumData a = Programa.miColeccion.devolverAlbum(item.Index);
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
                        DiscoCompacto cd;
                        Programa.miColeccion.devolverAlbum(b, out cd);
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
                //arista - titulo. (año) (hh:mm:ss)
                string i = vistaAlbumes.SelectedItems[0].SubItems[0].Text + " - " + vistaAlbumes.SelectedItems[0].SubItems[1].Text + ". ("
                    + vistaAlbumes.SelectedItems[0].SubItems[2].Text + ") (" + vistaAlbumes.SelectedItems[0].SubItems[3].Text + ") (" + vistaAlbumes.SelectedItems[0].SubItems[4].Text + ")";
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
                            AlbumData a = Programa.miColeccion.devolverAlbum(vistaAlbumes.SelectedIndices[i]);
                            Programa.miColeccion.quitarAlbum(ref a);
                            for (int j = 0; j < cuantos; j++)
                            {
                                vistaAlbumes.Items.Remove(itemsABorrar[j]);
                            }
                        }
                        catch (InvalidOperationException)
                        {
                            MessageBox.Show(Programa.textosLocal.GetString("errorBorrado"));
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
                        DiscoCompacto cdaborrar = Programa.miColeccion.getCDById(vistaAlbumes.SelectedItems[i].SubItems[5].Text);
                        DiscoCompacto cdd = cdaborrar;
                        Programa.miColeccion.BorrarCD(ref cdaborrar);
                        cdd.Album.CanBeRemoved = true;

                        foreach (DiscoCompacto cd in Programa.miColeccion.cds)
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
            duracionSeleccionada.Text = Programa.textosLocal.GetString("dur_total") + ": 00:00:00";
            vistaAlbumes.Refresh();
            crono.Stop();
            Console.WriteLine("Borrado completado en "+crono.ElapsedMilliseconds+"ms");
        }
        private void vistaAlbumes_ItemMouseHover(object sender, ListViewItemMouseHoverEventArgs e)
        {

        }

        private void vistaAlbumes_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if(!borrando)
            {
                TimeSpan seleccion = new TimeSpan();
                foreach (ListViewItem album in vistaAlbumes.SelectedItems)
                {
                    String a = album.SubItems[0].Text + "_" + album.SubItems[1].Text;
                    AlbumData ad = Programa.miColeccion.devolverAlbum(a);
                    seleccion += ad.Length;
                }
                duracionSeleccionada.Text = Programa.textosLocal.GetString("dur_total") + ": " + seleccion.ToString();
            }
        }
        private void masCortoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AlbumData a = Programa.miColeccion.albumes.First();
            for (int i = 1; i < Programa.miColeccion.albumes.Count; i++)
            {
                if (a.Length > Programa.miColeccion.albumes[i].Length)
                    a = Programa.miColeccion.albumes[i];
            }
            visualizarAlbum v = new visualizarAlbum(ref a);
            v.ShowDialog();
        }

        private void masLargoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AlbumData a = Programa.miColeccion.albumes.First();
            for (int i = 1; i < Programa.miColeccion.albumes.Count; i++)
            {
                if (a.Length < Programa.miColeccion.albumes[i].Length)
                    a = Programa.miColeccion.albumes[i];
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
            guardarComo.Filter = Programa.textosLocal.GetString("archivo") + ".csv(*.csv)|*.csv";
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
                MessageBox.Show(Programa.textosLocal.GetString("error_noAlbumes"), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Random generador = new Random();
            switch (TipoVista)
            {
                case TipoVista.Digital:
                    int ganador = generador.Next(0, Programa.miColeccion.albumes.Count);
                    AlbumData a = Programa.miColeccion.albumes[ganador];
                    visualizarAlbum vistazo = new visualizarAlbum(ref a);
                    vistazo.Show();
                    break;
                case TipoVista.CD:
                    int ganadorCD = generador.Next(0, Programa.miColeccion.cds.Count);
                    DiscoCompacto cd = Programa.miColeccion.cds[ganadorCD];
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
            try
            {
                busquedaSpotify b = new busquedaSpotify();
                if(b.ShowDialog() == DialogResult.No)
                    Programa._spotify.buscarAlbum(BusquedaSpotify);
                b.Dispose();
            }
            catch (NullReferenceException)
            {
                MessageBox.Show(Programa.textosLocal.GetString("error_vacio2"));
            }
        }

        private void acercaDeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            acercaDe form = new acercaDe();
            form.Show();
        }
        private void nuevoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult respuesta = MessageBox.Show(Programa.textosLocal.GetString("guardarBD"), "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (respuesta == DialogResult.Yes)
            {
                SaveFileDialog guardarComo = new SaveFileDialog();
                guardarComo.Filter = Programa.textosLocal.GetString("archivo") + ".json(*.json)|*.json";
                guardarComo.InitialDirectory = Environment.CurrentDirectory;
                if (guardarComo.ShowDialog() == DialogResult.OK)
                {
                    guardarDiscos(Path.GetFullPath(guardarComo.FileName), TipoGuardado.Digital);
                    guardarDiscos(Path.GetFullPath(guardarComo.FileName.Replace(".json", "") + "-CD.json"), TipoGuardado.CD);
                }
            }
            vistaAlbumes.Items.Clear();
            Programa.miColeccion.BorrarTodo();
        }

        private void vistaAlbumes_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
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
                clickDerechoMenuContexto.Show(vistaAlbumes, e.Location);

            }
        }
        private void crearCDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string seleccion = vistaAlbumes.SelectedItems[0].SubItems[0].Text + "_" + vistaAlbumes.SelectedItems[0].SubItems[1].Text;
            AlbumData a = Programa.miColeccion.devolverAlbum(seleccion);

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
                    DiscoCompacto temp = Programa.miColeccion.cds.Last();
                    CrearCD formCD = new CrearCD(ref temp, i);
                    formCD.ShowDialog();
                }
            }
        }

        private void clickDerechoMenuContexto_Opening(object sender, CancelEventArgs e)
        {

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
            if(!Programa.ModoOscuro)
            {
                Programa.ModoOscuro = true;
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
                Programa.ModoOscuro = false;
                testToolStripMenuItem1.Checked = Programa.ModoOscuro;
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
            if (Programa.ModoOscuro)
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

        private void copiarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string i = null;
            switch (TipoVista)
            {
                case TipoVista.Digital:
                    AlbumData a = Programa.miColeccion.devolverAlbum(vistaAlbumes.SelectedIndices[0]);
                    i = a.GetPortapapeles();
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
            
        }

        private void digitalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TipoVista = TipoVista.Digital;
            cdToolStripMenuItem.Checked = false;
            CargarVista();
        }

        private void cargarDiscosLegacyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.ImprimirMensaje("Abriendo desde fichero", TipoMensaje.Info);
            openFileDialog1.InitialDirectory = Environment.CurrentDirectory;
            openFileDialog1.Filter = Programa.textosLocal.GetString("archivo") + " .mdb (*.mdb)|*.mdb | "+Programa.textosLocal.GetString("archivo")+" .csv|*.csv";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fichero = openFileDialog1.FileName;
                Programa.CargarAlbumesCSV(fichero);
            }
            CargarVista();
        }

        private void digitalToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Log.ImprimirMensaje("Abriendo desde fichero", TipoMensaje.Info);
            openFileDialog1.InitialDirectory = Environment.CurrentDirectory;
            openFileDialog1.Filter = Programa.textosLocal.GetString("archivo") + " .csv (*.csv)|*.csv";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fichero = openFileDialog1.FileName;
                Programa.CargarAlbumesCSV(fichero);
            }
        }

        private void CargarCDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.ImprimirMensaje("Abriendo desde fichero", TipoMensaje.Info);
            openFileDialog1.InitialDirectory = Environment.CurrentDirectory;
            openFileDialog1.Filter = Programa.textosLocal.GetString("archivo") + " .json (*.json)|*.json";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fichero = openFileDialog1.FileName;
                Programa.CargarCDS(fichero);
            }
            CargarVista();
        }

        private void vincularToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool cancelado = false;
            DialogResult eleccion = MessageBox.Show(Programa.textosLocal.GetString("avisoSpotify"), "", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (eleccion == DialogResult.Yes)
            {
                Stopwatch espera = Stopwatch.StartNew();
                Programa._spotify.Reiniciar();
                Programa._spotify.SpotifyVinculado();
                while (!Programa._spotify.cuentaLista)
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
                    MessageBox.Show(Programa.textosLocal.GetString("errorVinculacion"));
                    return;
                }
                if (Programa._spotify._spotify.GetPrivateProfile().Product != "premium")
                {
                    Log.ImprimirMensaje("El usuario no tiene premium, no podrá usar spotify desde el Gestor", TipoMensaje.Advertencia);
                    MessageBox.Show(Programa.textosLocal.GetString("noPremium"));
                    spotifyToolStripMenuItem.Enabled = false;
                    vincularToolStripMenuItem.Enabled = false;
                }
                Reproductor.Instancia.SpotifyEncendido();
            }
            else return;
        }
        private void spotifyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AlbumData a = Programa.miColeccion.devolverAlbum(vistaAlbumes.SelectedIndices[0]); //it fucking works! no es O(1)
            Log.ImprimirMensaje(a.ToString(), TipoMensaje.Info);
            if(string.IsNullOrEmpty(a.IdSpotify))
            {
                SpotifyAPI.Web.Models.SimpleAlbum album = Programa._spotify.DevolverAlbum(a.GetTerminoBusqueda());
                if (object.ReferenceEquals(a, null) || object.ReferenceEquals(album, null))
                {
                    Log.ImprimirMensaje("Album fue nulo", TipoMensaje.Error);
                }
                else
                {
                    SpotifyAPI.Web.Models.ErrorResponse err = Programa._spotify.ReproducirAlbum(album.Id);
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
                SpotifyAPI.Web.Models.ErrorResponse err = Programa._spotify.ReproducirAlbum(a.IdSpotify);
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
            Programa.GuardarDiscos("discosCSV.csv", TipoGuardado.Digital);
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
            AlbumData a = Programa.miColeccion.devolverAlbum(vistaAlbumes.SelectedIndices[0]);
            Song cancion = a.getCancion(0);
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
                foreach (var cancion in carpeta.GetFiles())
                {
                    LectorMetadatos LM = new LectorMetadatos(cancion.FullName);
                    switch (Path.GetExtension(cancion.FullName))
                    {
                        case ".mp3":

                            a.Title = LM.Album;
                            a.Artist = LM.Artista;
                            a.Year = (short)LM.Año;
                            if (LM.Cover != null)
                            {
                                Image cover = LM.Cover;
                                cover.Save(carpeta.FullName + "\\cover.jpg");
                                a.Cover = carpeta.FullName + "\\cover.jpg";
                            }
                            break;
                        case ".flac":
                        case ".ogg":
                            a.Title = LM.Album;
                            a.Artist = LM.Artista;
                            Song c = new Song(LM.Titulo, (int)Reproductor.Instancia.getDuracionFromFile(cancion.FullName).TotalMilliseconds, false);
                            c.PATH = cancion.FullName;
                            c.Num = LM.Pista;
                            a.Year = (short)LM.Año;
                            c.SetAlbum(a);
                            a.AddSong(c);
                            break;
                        case ".jpg":
                            if (cancion.Name == "folder.jpg" || cancion.Name == "cover.jpg")
                                a.Cover = cancion.FullName;
                            break;
                    }
                    bC.Progreso();
                }
                a.SoundFilesPath = carpeta.FullName;
                bC.Close();
                Programa.miColeccion.agregarAlbum(ref a);
                Refrescar();
            }
        }

        private void configToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConfigForm configForm = new ConfigForm();
            configForm.ShowDialog();
        }
    }
}
