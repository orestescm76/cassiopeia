using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace aplicacion_musica
{
    public partial class ListaReproduccionUI : Form
    {
        private string Playing = "▶";
        private ListaReproduccion listaReproduccion;
        private int Puntero;
        public ListaReproduccionUI(ListaReproduccion lr)
        {
            InitializeComponent();
            listaReproduccion = lr;
            CargarVista();
            listViewCanciones.Size = Size;
            Puntero = 0;
            SetActivo(Puntero);
            Text = lr.Nombre;
        }
        private void CargarVista()
        {
            for (int i = 0; i < listaReproduccion.Canciones.Count; i++)
            {
                string[] data = new string[3];
                data[0] = "";
                listaReproduccion.Canciones[i].ToStringArray().CopyTo(data, 1);
                ListViewItem item = new ListViewItem(data);
                listViewCanciones.Items.Add(item);
            }
        }
        public void Refrescar()
        {
            listViewCanciones.Clear();
            CargarVista();
        }
        public void SetActivo(int punt)
        {
            listViewCanciones.Items[Puntero].SubItems[0].Text = "";
            listViewCanciones.Items[punt].SubItems[0].Text = Playing;
            Puntero = punt;
        }

        private void ListaReproduccionUI_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void ListaReproduccionUI_DragDrop(object sender, DragEventArgs e)
        {
            Cancion c = null;
            if((c = (Cancion)e.Data.GetData(typeof(Cancion))) != null)
            {
                listaReproduccion.AgregarCancion(c);
                Refrescar();
            }
        }

        private void listViewCanciones_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Reproductor.Instancia.ReproducirCancion(listViewCanciones.SelectedItems[0].Index);
        }

        private void ListaReproduccionUI_SizeChanged(object sender, EventArgs e)
        {
            listViewCanciones.Size = Size;

        }
    }
}
