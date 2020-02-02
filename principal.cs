using System;
using System.Drawing;
using System.ComponentModel;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections.Generic;

namespace aplicacion_musica
{
    public partial class principal : Form
    {
        private bool borrando;
        StatusBar barraAbajo;
        StatusBarPanel duracionSeleccionada;
        private ListViewItemComparer lvwColumnSorter;
        public principal()
        {
            InitializeComponent();
            borrando = false;
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

            vistaAlbumes.View = View.Details;
            //Debug.WriteLine(item.SubItems.Count);
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
                guardarcomo.Text = Programa.textosLocal[31];
                seleccionToolStripMenuItem.Text = Programa.textosLocal[30];
                adminMenu.Text = Programa.textosLocal[32];
                generarAlbumToolStripMenuItem.Text = Programa.textosLocal[33];
                borrarseleccionToolStripMenuItem.Text = Programa.textosLocal[28];
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
            List<Album> nuevaLista = new List<Album>();
            string[] s = new string[Programa.miColeccion.albumes.Count];
            int cuantos = Programa.miColeccion.albumes.Count;
            for (int i = 0; i < cuantos; i++)
            {
                Debug.WriteLine(vistaAlbumes.Items.Count);
                s[i] = vistaAlbumes.Items[i].SubItems[0].Text + "_" + vistaAlbumes.Items[i].SubItems[1].Text;
                Album a = Programa.miColeccion.devolverAlbum(s[i]);
                nuevaLista.Add(a);
            }
            Programa.miColeccion.cambiarLista(ref nuevaLista);
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
        private void guardarDiscos(string nombre)
        {
            using (StreamWriter salida = new StreamWriter(nombre, false, System.Text.Encoding.UTF8))
            {
                foreach (Album a in Programa.miColeccion.albumes)
                {
                    if (!(a.canciones[0] == null)) //no puede ser un album con 0 canciones
                    {
                        salida.WriteLine(a.nombre + ";" + a.artista + ";" + a.year + ";" + a.numCanciones + ";" + a.genero.Id + ";" + a.caratula);
                        for (int i = 0; i < a.numCanciones; i++)
                        {
                            if (a.canciones[i] is CancionLarga cl)
                            {
                                salida.WriteLine(cl.titulo + ";" + cl.Partes.Count);//no tiene duracion y son 2 datos a guardar...
                                foreach (Cancion parte in cl.Partes)
                                {
                                    salida.WriteLine(parte.titulo + ";" + parte.duracion.Minutes + ";" + parte.duracion.Seconds);
                                }

                            }
                            else if (a.canciones[i].duracion.Hours >= 1)
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
        }
        private void salidaAplicacion(object sender, EventArgs e)
        {
            guardarDiscos("discos.mdb");
            using(StreamWriter salida = new StreamWriter("idioma.cfg"))
            {
                if ((!File.Exists("idioma.cfg")))
                    File.Create("idioma.cfg");
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
            string s = vistaAlbumes.SelectedItems[0].SubItems[0].Text + '_' + vistaAlbumes.SelectedItems[0].SubItems[1].Text;
            Album a = Programa.miColeccion.devolverAlbum(s);
            visualizarAlbum vistazo = new visualizarAlbum(ref a);
            vistazo.ShowDialog();
            //MessageBox.Show(vistaAlbumes.SelectedItems[0].SubItems[1].Text,"",MessageBoxButtons.OK);
        }

        private void vistaAlbumes_KeyDown(object sender, KeyEventArgs e)
        {
            if(vistaAlbumes.SelectedItems.Count == 1 && (e.KeyCode == Keys.C && e.Control))
            {
                //black sabbath - paranoid. (1970) (00:42:29)
                string i = vistaAlbumes.SelectedItems[0].SubItems[0].Text + " - " + vistaAlbumes.SelectedItems[0].SubItems[1].Text + ". ("
                    + vistaAlbumes.SelectedItems[0].SubItems[2].Text + ") (" + vistaAlbumes.SelectedItems[0].SubItems[3].Text + ") (" + vistaAlbumes.SelectedItems[0].SubItems[4].Text + ")";
                Debug.WriteLine(i);
                Clipboard.SetText(i);
            }
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
            borrando = true;
            string[] s = new string[Programa.miColeccion.albumes.Count];
            int cuantos = vistaAlbumes.SelectedItems.Count;
            ListViewItem[] itemsABorrar = new ListViewItem[cuantos];
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
            borrando = false;
            duracionSeleccionada.Text = Programa.textosLocal[29] + ": 00:00:00";
            vistaAlbumes.Refresh();
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
                duracionSeleccionada.Text = Programa.textosLocal[29] + ": " + seleccion.ToString();
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
            borrarAlbumesSeleccionados();
        }
        private void guardarcomo_Click(object sender, EventArgs e)
        {
            SaveFileDialog guardarComo = new SaveFileDialog();
            guardarComo.Filter = Programa.textosLocal[1] + ".mdb(*.mdb)|*.mdb";
            guardarComo.InitialDirectory = Environment.CurrentDirectory;
            if(guardarComo.ShowDialog()==DialogResult.OK)
            {
                guardarDiscos(Path.GetFullPath(guardarComo.FileName));
            }
        }

        private void generarAlbumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Random generador = new Random();
            int ganador = generador.Next(0, Programa.miColeccion.albumes.Count);
            Album a = Programa.miColeccion.albumes[ganador];
            visualizarAlbum vistazo = new visualizarAlbum(ref a);
            vistazo.ShowDialog();
        }
    }
}
