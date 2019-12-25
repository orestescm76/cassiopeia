using System;
using System.Drawing;
using System.ComponentModel;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

namespace aplicacion_ipo
{
    public partial class principal : Form
    {
        StatusBar barraAbajo;
        StatusBarPanel duracionSeleccionada;
        private ListViewItemComparer lvwColumnSorter;
        public principal()
        {
            InitializeComponent();

            foreach (var idioma in Programa.codigosIdiomas)
            {
                ToolStripItem subIdioma = new ToolStripMenuItem(idioma);
                subIdioma.Click += new EventHandler(SubIdioma_Click);
                opcionesToolStripMenuItem.DropDownItems.Add(subIdioma);
            }
            lvwColumnSorter = new ListViewItemComparer();
            vistaAlbumes.ListViewItemSorter = lvwColumnSorter;
            vistaAlbumes.MultiSelect = true;

            Application.ApplicationExit += new EventHandler(salidaAplicacion);
            //String[] columnas = { "Artista", "Título", "Número de canciones", "Duración" };

            vistaAlbumes.View = View.Details;
            //Debug.WriteLine(item.SubItems.Count);
            vistaAlbumes.Columns.Add(Programa.textosLocal[4], -2, HorizontalAlignment.Left);
            vistaAlbumes.Columns.Add(Programa.textosLocal[5], -2, HorizontalAlignment.Left);
            vistaAlbumes.Columns.Add(Programa.textosLocal[6], -2, HorizontalAlignment.Left);
            vistaAlbumes.Columns.Add(Programa.textosLocal[17], -2, HorizontalAlignment.Left);
            vistaAlbumes.Columns.Add(Programa.textosLocal[8], -2, HorizontalAlignment.Left);
            ponerTextos();
            if (Programa.miColeccion.albumes.Count != 0)
                cargarVista();
            vistaAlbumes.FullRowSelect = true;
            Debug.WriteLine(vistaAlbumes.Columns.Count);
            barraAbajo = new StatusBar();
            duracionSeleccionada = new StatusBarPanel();
            duracionSeleccionada.AutoSize = StatusBarPanelAutoSize.Spring;
            barraAbajo.Panels.Add(duracionSeleccionada);
            barraAbajo.Visible = true;
            barraAbajo.ShowPanels = true;
            barraAbajo.Font = new Font("Segoe UI", 10);
            duracionSeleccionada.Text = Programa.textosLocal[29] + ": 00:00:00";
            Controls.Add(barraAbajo);
        }
        private void cargarVista()
        {
            ListViewItem[] items = new ListViewItem[Programa.miColeccion.albumes.Count];
            int i = 0;
            vistaAlbumes.Items.Clear();
            foreach (Album a in Programa.miColeccion.albumes)
            {
                String[] datos = a.toStringArray();
                items[i] = new ListViewItem(datos);
                i++;
            }
            vistaAlbumes.Items.AddRange(items);
        }
        private void ponerTextos()
        {
            
            try
            {
                Text = Programa.textosLocal[0] + " " + Programa.version;
                archivoMenuItem1.Text = Programa.textosLocal[1];
                opcionesToolStripMenuItem.Text = Programa.textosLocal[2];
                agregarAlbumToolStripMenuItem.Text = Programa.textosLocal[3];
                abrirToolStripMenuItem.Text = Programa.textosLocal[14];
                salirToolStripMenuItem.Text = Programa.textosLocal.Last();
                vistaAlbumes.Columns[0].Text = Programa.textosLocal[4];
                vistaAlbumes.Columns[1].Text = Programa.textosLocal[5];
                vistaAlbumes.Columns[2].Text = Programa.textosLocal[6];
                vistaAlbumes.Columns[3].Text = Programa.textosLocal[17];
                vistaAlbumes.Columns[4].Text = Programa.textosLocal[8];
                refrescarButton.Text = Programa.textosLocal[18];
                borrarButton.Text = Programa.textosLocal[28];
                banderaImageBox.ImageLocation = Programa.imagenesLocal.First();
                Debug.WriteLine(Programa.imagenesLocal.First());
                banderaImageBox.SizeMode = PictureBoxSizeMode.StretchImage;
                cargarVista();
            }
            catch(IndexOutOfRangeException)
            {

                MessageBox.Show(Programa.ErrorIdioma, "", MessageBoxButtons.OK);
            }
        }
        private void ordenarColumnas(object sender, ColumnClickEventArgs e)
        {
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
            vistaAlbumes.Refresh();
        }
        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void principal_Load(object sender, EventArgs e)
        {

        }

        private void SubIdioma_Click(object sender, EventArgs e)
        {
            var menu = sender as ToolStripMenuItem;
            string codIdioma = menu.Text;
            Programa.cambiarIdioma(codIdioma);
            ponerTextos();

        }
        private void agregarAlbumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            agregarAlbum agregarAlbum = new agregarAlbum();
            agregarAlbum.Show();
            cargarVista();
            vistaAlbumes.Refresh();
        }
        private void salidaAplicacion(object sender, EventArgs e)
        {
            using(StreamWriter salida = new StreamWriter("discos.mdb", false, System.Text.Encoding.UTF8))
            {
                foreach (Album a in Programa.miColeccion.albumes)
                {
                    if(!(a.canciones[0] == null)) //no puede ser un album con 0 canciones
                    {
                        salida.WriteLine(a.nombre + ";" + a.artista + ";" + a.year + ";" + a.numCanciones + ";" + a.genero.Id + ";" + a.caratula);
                        for (int i = 0; i < a.numCanciones; i++)
                        {
                            if(a.canciones[i].duracion.Hours >= 1)
                            {
                                int minutos = a.canciones[i].duracion.Minutes + 60 * a.canciones[i].duracion.Hours;
                                salida.WriteLine(a.canciones[i].titulo + ";" + minutos + ";" + a.canciones[i].duracion.Seconds);
                            }
                            else
                                salida.WriteLine(a.canciones[i].titulo + ";" + a.canciones[i].duracion.Minutes + ";" + a.canciones[i].duracion.Seconds);
                        }
                    }

                    salida.WriteLine();
                }
            }
            using(StreamWriter salida = new StreamWriter("idioma.cfg"))
            {
                salida.Write(Programa.idioma);
            }

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
           
        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = Environment.CurrentDirectory;
            openFileDialog1.Filter = Programa.textosLocal[1]+ " .mdb (*.mdb)|*.mdb";
            if(openFileDialog1.ShowDialog()== DialogResult.OK)
            {
                string fichero = openFileDialog1.FileName;
                Programa.miColeccion.cargarAlbumes(fichero);
            }
        }

        private void vistaAlbumes_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void button1_Click(object sender, EventArgs e)
        {

        }
        private void dobleClickItem(object sender, EventArgs e)
        {

        }

        private void vistaAlbumes_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            string s = vistaAlbumes.SelectedItems[0].SubItems[0].Text + ',' + vistaAlbumes.SelectedItems[0].SubItems[1].Text;
            Album a = Programa.miColeccion.devolverAlbum(s);
            visualizarAlbum vistazo = new visualizarAlbum(ref a);
            vistazo.ShowDialog();
            //MessageBox.Show(vistaAlbumes.SelectedItems[0].SubItems[1].Text,"",MessageBoxButtons.OK);
        }

        private void vistaAlbumes_KeyDown(object sender, KeyEventArgs e)
        {
            if(vistaAlbumes.SelectedItems.Count != 0 && e.KeyCode == Keys.Delete)
            {
                borrarAlbumesSeleccionados();
            }
        }

        private void refrescarButton_Click(object sender, EventArgs e)
        {
            cargarVista();
            //vistaAlbumes.Refresh();
        }

        private void borrarButton_Click(object sender, EventArgs e)
        {
            if(vistaAlbumes.SelectedItems.Count != 0)
                borrarAlbumesSeleccionados();
        }
        private void borrarAlbumesSeleccionados()
        {
            string[] s = new string[Programa.miColeccion.albumes.Count];
            int cuantos = vistaAlbumes.SelectedItems.Count;
            ListViewItem[] itemsABorrar = new ListViewItem[cuantos];
            for (int i = 0; i < cuantos; i++)
            {
                s[i] = vistaAlbumes.SelectedItems[i].SubItems[0].Text + "," + vistaAlbumes.SelectedItems[i].SubItems[1].Text;
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
            vistaAlbumes.Refresh();
        }
        private void vistaAlbumes_ItemMouseHover(object sender, ListViewItemMouseHoverEventArgs e)
        {

        }

        private void vistaAlbumes_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            TimeSpan seleccion = new TimeSpan();
            foreach (ListViewItem album in vistaAlbumes.SelectedItems)
            {
                String a = album.SubItems[0].Text + "," + album.SubItems[1].Text;
                Album ad = Programa.miColeccion.devolverAlbum(a);
                seleccion += ad.duracion;
            }
            duracionSeleccionada.Text = Programa.textosLocal[29] + ": "+ seleccion.ToString();
        }
    }
}
