using System;
using System.Drawing;
using System.ComponentModel;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;

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
    /// <summary>
    /// Formulario principal de la aplicación
    /// </summary>
    public partial class principal : Form
    {
        private bool borrando;
        public static string BusquedaSpotify;
        private ListViewItemComparer lvwColumnSorter;
        public TipoVista TipoVista;
        public principal()
        {
            Stopwatch crono = Stopwatch.StartNew();
            InitializeComponent();
            BusquedaSpotify = "";
            borrando = false;
            DirectoryInfo id = new DirectoryInfo("./idiomas");
            Programa.idiomas = new String[id.GetFiles().Length];
            int i = 0;
            foreach(var idioma in id.GetFiles())
            {
                CultureInfo nombreIdioma = new CultureInfo(idioma.Name.Replace(".resx",""));
                Programa.idiomas[i] = idioma.Name.Replace(".resx", "");
                ToolStripItem subIdioma = new ToolStripMenuItem(nombreIdioma.NativeName);
                subIdioma.Click += new EventHandler(SubIdioma_Click);
                opcionesToolStripMenuItem.Image = System.Drawing.Image.FromFile("./iconosBanderas/" + nombreIdioma.Name + ".png");
                subIdioma.Image = System.Drawing.Image.FromFile("./iconosBanderas/" + nombreIdioma.Name + ".png");
                opcionesToolStripMenuItem.DropDownItems.Add(subIdioma);
                i++;
            }

            lvwColumnSorter = new ListViewItemComparer();
            vistaAlbumes.ListViewItemSorter = lvwColumnSorter;
            vistaAlbumes.MultiSelect = true;

            Application.ApplicationExit += new EventHandler(salidaAplicacion);

            vistaAlbumes.View = View.Details;
            ponerTextos();
            vistaAlbumes.FullRowSelect = true;
            duracionSeleccionada.AutoSize = true;
            barraAbajo.Visible = true;
            barraAbajo.Font = new Font("Segoe UI", 10);
            duracionSeleccionada.Text = Programa.textosLocal.GetString("dur_total") + ": 00:00:00";
            vistaAlbumes.DrawItem += (sender, e) => { e.DrawDefault = true; };
            vistaAlbumes.DrawSubItem += (sender, e) => { e.DrawDefault = true; };
            vistaAlbumes.OwnerDraw = true;
            crono.Stop();
            Console.WriteLine("Formulario principal creado y cargado en "+crono.ElapsedMilliseconds+"ms");
        }
        public void Refrescar() { cargarVista(); }
        public void HayInternet(bool i)
        {
            buscarEnSpotifyToolStripMenuItem.Enabled = i;
        }
        private void cargarVista()
        {
            Console.WriteLine(nameof(cargarVista) + " - Cargando vista " + TipoVista);
            vistaAlbumes.Items.Clear();
            Stopwatch crono = Stopwatch.StartNew();
            switch (TipoVista)
            {
                case TipoVista.Digital:
                    ListViewItem[] items = new ListViewItem[Programa.miColeccion.albumes.Count];
                    int i = 0;
                    foreach (Album a in Programa.miColeccion.albumes)
                    {
                        String[] datos = a.toStringArray();
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
            Console.WriteLine(nameof(cargarVista) + "- Cargado en "+crono.ElapsedMilliseconds + " ms");
           
        }
        private void ponerTextos()
        {
            Text = Programa.textosLocal.GetString("titulo_ventana_principal") + " " + Programa.version;
            archivoMenuItem1.Text = Programa.textosLocal.GetString("archivo");
            opcionesToolStripMenuItem.Text = Programa.textosLocal.GetString("cambiar_idioma");
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
            opcionesToolStripMenuItem.Image = System.Drawing.Image.FromFile("./iconosBanderas/" + Programa.Idioma + ".png");
            digitalToolStripMenuItem.Text = Programa.textosLocal.GetString("digital");
        }
        private void ordenarColumnas(object sender, ColumnClickEventArgs e)
        {
            Console.WriteLine("Ordenando columnas");
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
            List<Album> nuevaLista = new List<Album>();
            string[] s = new string[Programa.miColeccion.albumes.Count];
            int cuantos = Programa.miColeccion.albumes.Count;
            for (int i = 0; i < cuantos; i++)
            {
                s[i] = vistaAlbumes.Items[i].SubItems[0].Text + "_" + vistaAlbumes.Items[i].SubItems[1].Text;
                Album a = Programa.miColeccion.devolverAlbum(s[i]);
                nuevaLista.Add(a);
            }
            Programa.miColeccion.cambiarLista(ref nuevaLista);
            vistaAlbumes.Refresh();
            crono.Stop();
            Console.WriteLine("Ordenado en "+crono.ElapsedMilliseconds+"ms");
        }
        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void SubIdioma_Click(object sender, EventArgs e)
        {

        }
        private void agregarAlbumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            agregarAlbum agregarAlbum = new agregarAlbum();
            agregarAlbum.Show();
            cargarVista();
        }
        private void guardarDiscos(string nombre, TipoGuardado tipoGuardado)
        {
            using (StreamWriter salida = new StreamWriter(nombre, false, System.Text.Encoding.UTF8))
            {
                Console.WriteLine(nameof(guardarDiscos) + " - Guardando la base de datos...");
                Console.WriteLine("Nombre del fichero: "+nombre);
                Stopwatch crono = Stopwatch.StartNew();
                switch (tipoGuardado)
                {
                    case TipoGuardado.Digital:
                        foreach (Album a in Programa.miColeccion.albumes)
                        {
                            salida.WriteLine(JsonConvert.SerializeObject(a));
                        }
                        break;
                    case TipoGuardado.CD:
                        
                        foreach (DiscoCompacto compacto in Programa.miColeccion.cds)
                        {
                            salida.WriteLine(JsonConvert.SerializeObject(compacto));
                        }
                        break;
                    default:
                        break;
                }

                crono.Stop();
                Console.WriteLine(nameof(guardarDiscos) + "- Guardado en " + crono.ElapsedMilliseconds + " ms");
                FileInfo fich = new FileInfo(nombre);
                Console.WriteLine("Tamaño: "+ fich.Length + " bytes");
            }
        }
        private void salidaAplicacion(object sender, EventArgs e)
        {
            guardarDiscos("discos.json", TipoGuardado.Digital);
            guardarDiscos("cd.json", TipoGuardado.CD);
            using (StreamWriter salida = new StreamWriter("idioma.cfg", false))
            {
                salida.Write(Programa.Idioma);
            }
        }

        private void vistaAlbumes_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Console.WriteLine("Iniciada busqueda del álbum linealmente...");
            Stopwatch cronoTotal = Stopwatch.StartNew();
            switch(TipoVista)
            {
                case TipoVista.Digital:
                    foreach (ListViewItem item in vistaAlbumes.SelectedItems)
                    {
                        Stopwatch crono = Stopwatch.StartNew();
                        string s = item.SubItems[0].Text + '_' + item.SubItems[1].Text;
                        Album a = Programa.miColeccion.devolverAlbum(s);
                        crono.Stop();
                        Console.WriteLine("Finalizado en " + crono.ElapsedTicks / 10 + " μs");
                        crono.Reset(); crono.Start();
                        visualizarAlbum vistazo = new visualizarAlbum(ref a);
                        vistazo.Show();
                        crono.Stop();
                        Console.WriteLine("Formulario creado y mostrado en " + crono.ElapsedMilliseconds + "ms");
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
                        Console.WriteLine("Finalizado en " + crono.ElapsedTicks / 10 + " μs");
                        crono.Reset(); crono.Start();
                        visualizarAlbum visCD = new visualizarAlbum(ref cd);
                        visCD.Show();
                        crono.Stop();
                        Console.WriteLine("Formulario creado y mostrado en " + crono.ElapsedMilliseconds + "ms");
                    }

                    break;
            }
            cronoTotal.Stop();
            Console.WriteLine("Operación realizada en "+cronoTotal.ElapsedMilliseconds+"ms");
            //MessageBox.Show(vistaAlbumes.SelectedItems[0].SubItems[1].Text,"",MessageBoxButtons.OK);
        }

        private void vistaAlbumes_KeyDown(object sender, KeyEventArgs e)
        {

            if(vistaAlbumes.SelectedItems.Count == 1 && (e.KeyCode == Keys.C && e.Control))
            {
                //arista - titulo. (año) (hh:mm:ss)
                Console.WriteLine("Se presionó Ctrl + C");
                string i = vistaAlbumes.SelectedItems[0].SubItems[0].Text + " - " + vistaAlbumes.SelectedItems[0].SubItems[1].Text + ". ("
                    + vistaAlbumes.SelectedItems[0].SubItems[2].Text + ") (" + vistaAlbumes.SelectedItems[0].SubItems[3].Text + ") (" + vistaAlbumes.SelectedItems[0].SubItems[4].Text + ")";
                Clipboard.SetText(i);
                Console.WriteLine("Copiado " + i + " al portapapeles");
            }
            if (e.Control && e.KeyCode == Keys.A)
            {
                Console.WriteLine("Se presionó Ctrl + A");
                foreach (ListViewItem item in vistaAlbumes.Items)
                {
                    item.Selected = true;
                }
            }
            if(e.KeyCode == Keys.F5)
            {
                Console.WriteLine("Se presionó F5");
                cargarVista();
            }
            if (e.KeyCode == Keys.Escape)
            {
                Console.WriteLine("Se presionó Esc");
                foreach (ListViewItem item in vistaAlbumes.Items)
                {
                    item.Selected = false;
                }
            }
            if(e.KeyCode == Keys.Enter)
            {
                vistaAlbumes_MouseDoubleClick(null,null);
            }
        }
        private void borrarAlbumesSeleccionados(TipoVista tipoVista)
        {
            Console.WriteLine("Borrando "+vistaAlbumes.SelectedItems.Count+" álbumes");
            Stopwatch crono = Stopwatch.StartNew();
            borrando = true;
            int cuantos = vistaAlbumes.SelectedItems.Count;
            ListViewItem[] itemsABorrar = new ListViewItem[cuantos];
            switch (tipoVista)
            {
                case TipoVista.Digital:
                    string[] s = new string[Programa.miColeccion.albumes.Count];

 
                    for (int i = 0; i < cuantos; i++)
                    {
                        s[i] = vistaAlbumes.SelectedItems[i].SubItems[0].Text + "_" + vistaAlbumes.SelectedItems[i].SubItems[1].Text;
                        //s[i] = vistaAlbumes.SelectedItems[i].SubItems[0].Text + ',' + vistaAlbumes.SelectedItems[i].SubItems[1].Text;
                        itemsABorrar[i] = vistaAlbumes.SelectedItems[i];
                        //vistaAlbumes.Items.Remove(vistaAlbumes.Items[vistaAlbumes.SelectedIndices[i]]);
                    }
                    for (int i = 0; i < vistaAlbumes.SelectedIndices.Count; i++)
                    {
                        Album a = Programa.miColeccion.devolverAlbum(s[i]);
                        Programa.miColeccion.quitarAlbum(ref a);
                    }
                    for (int i = 0; i < cuantos; i++)
                    {
                        vistaAlbumes.Items.Remove(itemsABorrar[i]);
                    }
                    break;
                case TipoVista.CD:
                    for (int i = 0; i < cuantos; i++)
                    {
                        itemsABorrar[i] = vistaAlbumes.SelectedItems[i];
                    }
                    for (int i = 0; i < cuantos; i++)
                    {
                        DiscoCompacto cdaborrar = Programa.miColeccion.getCDById(vistaAlbumes.SelectedItems[i].SubItems[5].Text);

                        Programa.miColeccion.BorrarCD(ref cdaborrar);

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
                    Album ad = Programa.miColeccion.devolverAlbum(a);
                    seleccion += ad.duracion;
                }
                duracionSeleccionada.Text = Programa.textosLocal.GetString("dur_total") + ": " + seleccion.ToString();
            }
        }
        private void masCortoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Album a = Programa.miColeccion.albumes.First();
            for (int i = 1; i < Programa.miColeccion.albumes.Count; i++)
            {
                if (a.duracion > Programa.miColeccion.albumes[i].duracion)
                    a = Programa.miColeccion.albumes[i];
            }
            visualizarAlbum v = new visualizarAlbum(ref a);
            v.ShowDialog();
        }

        private void masLargoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Album a = Programa.miColeccion.albumes.First();
            for (int i = 1; i < Programa.miColeccion.albumes.Count; i++)
            {
                if (a.duracion < Programa.miColeccion.albumes[i].duracion)
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
            guardarComo.Filter = Programa.textosLocal.GetString("archivo") + ".json(*.json)|*.json";
            guardarComo.InitialDirectory = Environment.CurrentDirectory;
            if(guardarComo.ShowDialog()==DialogResult.OK)
            {
                guardarDiscos(Path.GetFullPath(guardarComo.FileName), (TipoGuardado)TipoVista);
            }
        }

        private void generarAlbumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Generando álbum");
            Stopwatch crono = Stopwatch.StartNew();
            if(vistaAlbumes.Items.Count == 0)
            {
                crono.Stop();
                Console.WriteLine("Cancelado por no haber álbumes");
                MessageBox.Show(Programa.textosLocal.GetString("error_noAlbumes"), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Random generador = new Random();
            switch (TipoVista)
            {
                case TipoVista.Digital:
                    int ganador = generador.Next(0, Programa.miColeccion.albumes.Count);
                    Album a = Programa.miColeccion.albumes[ganador];
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
            Console.WriteLine("Generado en "+crono.ElapsedMilliseconds+"ms");
        }

        private void buscarEnSpotifyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                busquedaSpotify b = new busquedaSpotify();
                if(b.ShowDialog() == DialogResult.No)
                    Programa._spotify.buscarAlbum(BusquedaSpotify);
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

        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {

        }

        private void vistaAlbumes_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                if (TipoVista == TipoVista.CD)
                    clickDerechoMenuContexto.Items[0].Visible = false;
                clickDerechoMenuContexto.Show(vistaAlbumes,e.Location);


            }
        }
        private void crearCDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string seleccion = vistaAlbumes.SelectedItems[0].SubItems[0].Text + "_" + vistaAlbumes.SelectedItems[0].SubItems[1].Text;
            Album a = Programa.miColeccion.devolverAlbum(seleccion);

            if(a.duracion.TotalMinutes < 80)
            {
                CrearCD formCD = new CrearCD(ref a);
                formCD.Show();
            }
            else
            {

                short numDiscos = (short)Math.Ceiling((a.duracion.TotalMinutes / 80));
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
                    i = vistaAlbumes.SelectedItems[0].SubItems[0].Text + " - " + vistaAlbumes.SelectedItems[0].SubItems[1].Text + ". ("
                        + vistaAlbumes.SelectedItems[0].SubItems[2].Text + ") (" + vistaAlbumes.SelectedItems[0].SubItems[3].Text + ") (" + vistaAlbumes.SelectedItems[0].SubItems[4].Text + ")";
                    break;
                case TipoVista.CD:

                    break;
                case TipoVista.Vinilo:
                    break;
                default:
                    break;
            }

            Clipboard.SetText(i);
            Console.WriteLine("Copiado " + i + " al portapapeles");
        }

        private void cdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TipoVista = TipoVista.CD;
            cargarVista();
            digitalToolStripMenuItem.Checked = false;
            
        }

        private void digitalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TipoVista = TipoVista.Digital;
            cdToolStripMenuItem.Checked = false;
            cargarVista();
        }

        private void cargarDiscosLegacyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Abriendo desde fichero");
            openFileDialog1.InitialDirectory = Environment.CurrentDirectory;
            openFileDialog1.Filter = Programa.textosLocal.GetString("archivo") + " .mdb (*.mdb)|*.mdb";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fichero = openFileDialog1.FileName;
                Programa.cargarAlbumesLegacy(fichero);
            }
            cargarVista();
        }

        private void digitalToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Abriendo desde fichero");
            openFileDialog1.InitialDirectory = Environment.CurrentDirectory;
            openFileDialog1.Filter = Programa.textosLocal.GetString("archivo") + " .json (*.json)|*.json";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fichero = openFileDialog1.FileName;
                Programa.cargarAlbumes(fichero);
            }
            cargarVista();
        }

        private void CargarCDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Abriendo desde fichero");
            openFileDialog1.InitialDirectory = Environment.CurrentDirectory;
            openFileDialog1.Filter = Programa.textosLocal.GetString("archivo") + " .json (*.json)|*.json";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fichero = openFileDialog1.FileName;
                Programa.cargarCDS(fichero);
            }
            cargarVista();
        }
    }
}
