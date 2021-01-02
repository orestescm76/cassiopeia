using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using SpotifyAPI.Web.Models;

namespace aplicacion_musica
{
    public partial class resultadoSpotify : Form
    {
        private ListViewItemComparer lvwColumnSorter;
        private List<SimpleAlbum> listaBusqueda = new List<SimpleAlbum>();
        bool EditarID = false;
        Album AlbumAEditar;
        public resultadoSpotify(ref List<SimpleAlbum> l, bool edit, Album album = null)
        {
            InitializeComponent();
            EditarID = edit;
            AlbumAEditar = album;
            Text = Programa.textosLocal.GetString("resultado_busqueda");
            labelAyuda.Text = Programa.textosLocal.GetString("ayudaAñadir");
            labelResultado.Text = Programa.textosLocal.GetString("seHanEncontrado") + l.Count + " " + Programa.textosLocal.GetString("resultados");
            listaBusqueda = l;
            listViewResultadoBusqueda.Columns[1].Text = Programa.textosLocal.GetString("artista");
            listViewResultadoBusqueda.Columns[2].Text = Programa.textosLocal.GetString("titulo");
            listViewResultadoBusqueda.Columns[3].Text = Programa.textosLocal.GetString("año");
            listViewResultadoBusqueda.Columns[4].Text = Programa.textosLocal.GetString("numcanciones");
            buttonCancelar.Text = Programa.textosLocal.GetString("cancelar");
            buttonOK.Text = Programa.textosLocal.GetString("añadir");
            int n = 1;
            foreach(SimpleAlbum a in listaBusqueda)
            {
                String[] parseFecha = a.ReleaseDate.Split('-');
                String[] datos = { n.ToString(), a.Artists[0].Name, a.Name, parseFecha[0], a.TotalTracks.ToString()};
                n++;
                ListViewItem i = new ListViewItem(datos);
                listViewResultadoBusqueda.Items.Add(i);
            }
            lvwColumnSorter = new ListViewItemComparer();
            listViewResultadoBusqueda.ListViewItemSorter = lvwColumnSorter;
            listViewResultadoBusqueda.View = View.Details;
        }

        private void buttonCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Dispose();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if(!EditarID)
            {
                Log.Instance.ImprimirMensaje("Intentando añadir " + listViewResultadoBusqueda.SelectedItems.Count +
                    " álbumes", TipoMensaje.Info);
                Stopwatch crono = Stopwatch.StartNew();
                for (int i = 0; i < listViewResultadoBusqueda.SelectedItems.Count; i++)
                {
                    int cual = Convert.ToInt32(listViewResultadoBusqueda.SelectedItems[i].SubItems[0].Text);//la imagen tiene url
                    Programa._spotify.procesarAlbum(listaBusqueda[cual - 1]);
                }
                DialogResult = DialogResult.OK; //quiza molaria una pantallita de carga
                crono.Stop();
                Console.WriteLine("Agregdos " + listViewResultadoBusqueda.SelectedItems.Count + " álbumes correctamente en " + crono.ElapsedMilliseconds + "ms");
                Programa.refrescarVista();
            }
            else
            {
                int IndexAlbum = Convert.ToInt32(listViewResultadoBusqueda.SelectedItems[0].SubItems[0].Text);
                SimpleAlbum temp = listaBusqueda[IndexAlbum];
                AlbumAEditar.SetSpotifyID(temp.Id);
            }
            Dispose();
        }

        private void listViewResultadoBusqueda_ColumnClick(object sender, ColumnClickEventArgs e)
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
            listViewResultadoBusqueda.Sort();
            listViewResultadoBusqueda.Refresh();
        }
    }
}
