using System;
using System.Drawing;
using System.Windows.Forms;

namespace aplicacion_ipo
{
    public partial class visualizarAlbum : Form
    {
        private Album albumAVisualizar;
        private ListViewItemComparer lvwColumnSorter;
        public visualizarAlbum(ref Album a)
        {
            InitializeComponent();
            albumAVisualizar = a;
            ponerTextos();
            infoAlbum.Text = Programa.textosLocal[4] + ": " + a.artista + Environment.NewLine +
                Programa.textosLocal[5] + ": " + a.nombre + Environment.NewLine +
                Programa.textosLocal[6] + ": " + a.year + Environment.NewLine +
                Programa.textosLocal[17] + ": " + a.duracion.ToString() + Environment.NewLine +
                Programa.textosLocal[8] + ": " + a.genero.traducido + Environment.NewLine;
            if (a.caratula != "")
            {
                Image caratula = Image.FromFile(a.caratula);
                vistaCaratula.Image = caratula;
                vistaCaratula.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            lvwColumnSorter = new ListViewItemComparer();
            vistaCanciones.ListViewItemSorter = lvwColumnSorter;
            vistaCanciones.View = View.Details;
            vistaCanciones.Columns.Add("#", -2, HorizontalAlignment.Left);
            vistaCanciones.Columns.Add(Programa.textosLocal[5], 280, HorizontalAlignment.Left);
            vistaCanciones.Columns.Add(Programa.textosLocal[17], -2, HorizontalAlignment.Left);
            cargarVista();

        }
        private void ponerTextos()
        {
            Text = Programa.textosLocal[19] + " " + albumAVisualizar.artista + " - " + albumAVisualizar.nombre;
            okDoomerButton.Text = Programa.textosLocal[21];
            editarButton.Text = Programa.textosLocal[20];
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
    }
}
