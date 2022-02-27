using Cassiopeia.src.Classes;
using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace Cassiopeia.src.Forms
{
    public partial class SpotifyResults : Form
    {
        private ListViewItemComparer lvwColumnSorter;
        private List<SimpleAlbum> listaBusqueda = new List<SimpleAlbum>();
        bool EditarID = false;
        AlbumData AlbumAEditar;
        public SpotifyResults(ref List<SimpleAlbum> l, bool edit, AlbumData album = null)
        {
            InitializeComponent();
            EditarID = edit;
            AlbumAEditar = album;
            Text = Kernel.LocalTexts.GetString("resultado_busqueda");
            labelAyuda.Text = Kernel.LocalTexts.GetString("ayudaAñadir");
            labelResultado.Text = Kernel.LocalTexts.GetString("seHanEncontrado") + l.Count + " " + Kernel.LocalTexts.GetString("resultados");
            listaBusqueda = l;
            listViewResultadoBusqueda.Columns[1].Text = Kernel.LocalTexts.GetString("artista");
            listViewResultadoBusqueda.Columns[2].Text = Kernel.LocalTexts.GetString("titulo");
            listViewResultadoBusqueda.Columns[3].Text = Kernel.LocalTexts.GetString("año");
            listViewResultadoBusqueda.Columns[4].Text = Kernel.LocalTexts.GetString("numcanciones");
            buttonCancelar.Text = Kernel.LocalTexts.GetString("cancelar");
            buttonOK.Text = Kernel.LocalTexts.GetString("añadir");
            int n = 1;
            foreach (SimpleAlbum a in listaBusqueda)
            {
                String[] parseFecha = a.ReleaseDate.Split('-');
                String[] datos = { n.ToString(), a.Artists[0].Name, a.Name, parseFecha[0], a.TotalTracks.ToString() };
                n++;
                ListViewItem i = new ListViewItem(datos);
                listViewResultadoBusqueda.Items.Add(i);
            }
            lvwColumnSorter = new ListViewItemComparer();
            listViewResultadoBusqueda.ListViewItemSorter = lvwColumnSorter;
            listViewResultadoBusqueda.View = View.Details;
            Icon = Properties.Resources.spotifyico;
        }

        private void buttonCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Dispose();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (!EditarID)
            {
                Log.Instance.PrintMessage("Trying to add " + listViewResultadoBusqueda.SelectedItems.Count +
                    " albums", MessageType.Info);
                Stopwatch crono = Stopwatch.StartNew();
                for (int i = 0; i < listViewResultadoBusqueda.SelectedItems.Count; i++)
                {
                    int cual = Convert.ToInt32(listViewResultadoBusqueda.SelectedItems[i].SubItems[0].Text);//la imagen tiene url
                    Kernel.Spotify.ProcessAlbum(listaBusqueda[cual - 1]);
                }
                DialogResult = DialogResult.OK; //quiza molaria una pantallita de carga
                crono.Stop();
                Log.Instance.PrintMessage("Added " + listViewResultadoBusqueda.SelectedItems.Count + " albums", MessageType.Correct, crono, TimeType.Milliseconds);
                
                Kernel.ReloadView();
            }
            else
            {
                int IndexAlbum = Convert.ToInt32(listViewResultadoBusqueda.SelectedItems[0].SubItems[0].Text);
                SimpleAlbum temp = listaBusqueda[IndexAlbum];
                AlbumAEditar.IdSpotify = temp.Id;
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
