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
        private ListaReproduccion listaReproduccion;
        public ListaReproduccionUI(ListaReproduccion lr)
        {
            InitializeComponent();
            listaReproduccion = lr;
            CargarVista();
        }
        private void CargarVista()
        {
            for (int i = 0; i < listaReproduccion.Canciones.Count; i++)
            {
                string[] data = new string[3];
                data[0] = (i + 1).ToString();
                listaReproduccion.Canciones[i].ToStringArray().CopyTo(data, 1);
                dataGridViewCanciones.Rows.Add(data);
            }
        }
        public void Refrescar()
        {
            dataGridViewCanciones.Rows.Clear();
            CargarVista();
        }
        public void SetActivo(int punt)
        {
            dataGridViewCanciones.CurrentCell = dataGridViewCanciones.Rows[punt].Cells[0];
        }
        private void dataGridViewCanciones_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

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
    }
}
