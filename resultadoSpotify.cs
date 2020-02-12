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
        private List<SimpleAlbum> listaBusqueda = new List<SimpleAlbum>();
        public resultadoSpotify(ref List<SimpleAlbum> l)
        {
            InitializeComponent();
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
        }

        private void buttonCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Dispose();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listViewResultadoBusqueda.SelectedItems.Count; i++)
            {
                int cual = listViewResultadoBusqueda.Items.IndexOf(listViewResultadoBusqueda.SelectedItems[i]);//la imagen tiene url
                SimpleAlbum temp = listaBusqueda[cual];
                Programa._spotify.procesarAlbum(temp);
            }
            DialogResult = DialogResult.OK; //quiza molaria una pantallatita de carga
            Programa.refrescarVista();
            Dispose();
        }
    }
}
