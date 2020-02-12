using System;
using System.Drawing;
using System.Windows.Forms;

namespace aplicacion_musica
{
    public partial class visualizarAlbum : Form
    {
        private StatusBar barraAbajo;
        private StatusBarPanel duracionSeleccionada;
        private Album albumAVisualizar;
        private ListViewItemComparer lvwColumnSorter;
        public visualizarAlbum(ref Album a)
        {
            InitializeComponent();
            albumAVisualizar = a;

            infoAlbum.Text = Programa.textosLocal.GetString("artista") + ": " + a.artista + Environment.NewLine +
                Programa.textosLocal.GetString("titulo") + ": " + a.nombre + Environment.NewLine +
                Programa.textosLocal.GetString("año") + ": " + a.year + Environment.NewLine +
                Programa.textosLocal.GetString("duracion") + ": " + a.duracion.ToString() + Environment.NewLine +
                Programa.textosLocal.GetString("genero") + ": " + a.genero.traducido + Environment.NewLine;
            if (a.caratula != "")
            {
                Image caratula = Image.FromFile(a.caratula);
                vistaCaratula.Image = caratula;
                vistaCaratula.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            lvwColumnSorter = new ListViewItemComparer();
            vistaCanciones.ListViewItemSorter = lvwColumnSorter;
            vistaCanciones.View = View.Details;
            vistaCanciones.MultiSelect = true;
            barraAbajo = new StatusBar();
            duracionSeleccionada = new StatusBarPanel();
            duracionSeleccionada.AutoSize = StatusBarPanelAutoSize.Spring;
            barraAbajo.Panels.Add(duracionSeleccionada);
            barraAbajo.Visible = true;
            barraAbajo.ShowPanels = true;
            barraAbajo.Font = new Font("Segoe UI", 10);
            Controls.Add(barraAbajo);
            ponerTextos();
            cargarVista();

        }
        private void ponerTextos()
        {
            Text = Programa.textosLocal.GetString("visualizando") + " " + albumAVisualizar.artista + " - " + albumAVisualizar.nombre;
            vistaCanciones.Columns[0].Text = "#";
            vistaCanciones.Columns[1].Text = Programa.textosLocal.GetString("titulo");
            vistaCanciones.Columns[2].Text = Programa.textosLocal.GetString("duracion");
            okDoomerButton.Text = Programa.textosLocal.GetString("hecho");
            editarButton.Text = Programa.textosLocal.GetString("editar");
            duracionSeleccionada.Text = Programa.textosLocal.GetString("dur_total") + ": 00:00:00";
        }
        private void cargarVista()
        {
            ListViewItem[] items = new ListViewItem[albumAVisualizar.canciones.Length];
            int i = 0;
            foreach (Cancion c in albumAVisualizar.canciones)
            {

                String[] datos = new string[3];
                datos[0] = (i + 1).ToString();
                c.toStringArray().CopyTo(datos,1);
                items[i] = new ListViewItem(datos);
                if ((CancionLarga)c)
                {
                    items[i].BackColor = Color.LightSalmon;
                }
                i++;
            }
            vistaCanciones.Items.AddRange(items);
        }
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
            Close();
        }

        private void editarButton_Click(object sender, EventArgs e)
        {
            editarAlbum editor = new editarAlbum(ref albumAVisualizar);
            editor.Show();
            Close();
        }
        private void vistaCanciones_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            TimeSpan seleccion = new TimeSpan();
            foreach (ListViewItem cancion in vistaCanciones.SelectedItems)
            {

                int c = Convert.ToInt32(cancion.SubItems[0].Text); c--;
                Cancion can = albumAVisualizar.getCancion(c);
                seleccion += can.duracion;
            }
            duracionSeleccionada.Text = Programa.textosLocal.GetString("dur_total") + ": " + seleccion.ToString();
        }

        private void vistaCanciones_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int n = Convert.ToInt32(vistaCanciones.SelectedItems[0].SubItems[0].Text);
            Cancion c = albumAVisualizar.getCancion(n-1);
            if((CancionLarga)c)
            {
                string infoDetallada = "";
                for (int i = 0; i < cl.Partes.Count; i++)
                {
                    infoDetallada += cl.GetNumeroRomano(i + 1) + ". ";
                    infoDetallada += cl.Partes[i].titulo + " - " + cl.Partes[i].duracion;
                    infoDetallada += Environment.NewLine;
                }
                MessageBox.Show(infoDetallada);
            }
        }
    }
}
