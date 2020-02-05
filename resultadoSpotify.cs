using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SpotifyAPI.Web.Models;

namespace aplicacion_musica
{
    public partial class resultadoSpotify : Form
    {
        private List<SimpleAlbum> listaBusqueda;
        public resultadoSpotify(List<SimpleAlbum> l)
        {
            InitializeComponent();
            Text = Programa.textosLocal[44];
            labelAyuda.Text = Programa.textosLocal[41];
            labelResultado.Text = Programa.textosLocal[39] + listaBusqueda.Count + " " + Programa.textosLocal[40];
            listaBusqueda = l;
            listViewResultadoBusqueda.Columns[1].Text = Programa.textosLocal[5];
            listViewResultadoBusqueda.Columns[2].Text = Programa.textosLocal[6];
            listViewResultadoBusqueda.Columns[3].Text = Programa.textosLocal[17];
            listViewResultadoBusqueda.Columns[4].Text = Programa.textosLocal[7];
            buttonCancelar.Text = Programa.textosLocal[11];
            buttonOK.Text = Programa.textosLocal[9];
            int n = 1;
            foreach(SimpleAlbum a in listaBusqueda)
            {
                String[] parseFecha = a.ReleaseDate.Split('-');
                String[] datos = { n.ToString(), a.Artists[0].Name, a.Name, parseFecha[0], a.TotalTracks.ToString()};
                n++;
                ListViewItem i = new ListViewItem(datos);
                listViewResultadoBusqueda.Items.Add(i);
            }
        }

        private void buttonCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Dispose();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            int cual = listViewResultadoBusqueda.Items.IndexOf(listViewResultadoBusqueda.SelectedItems[0]);//la imagen tiene url
            SimpleAlbum temp = listaBusqueda[cual];
            Programa._spotify.procesarAlbum(temp);
            DialogResult= DialogResult.OK;
            Dispose();
        }
    }
}
