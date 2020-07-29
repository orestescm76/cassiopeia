using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace aplicacion_musica.src.Forms
{
    public partial class AbrirDisco : Form
    {
        public AbrirDisco()
        {
            InitializeComponent();
        }

        private void AbrirDisco_Load(object sender, EventArgs e)
        {
            Log.Instance.ImprimirMensaje("Detectando disqueteras", TipoMensaje.Info);
            DriveInfo[] Discos = DriveInfo.GetDrives();
            foreach (var d in Discos)
            {
                if (d.DriveType == DriveType.CDRom)
                {
                    listViewDiscos.Items.Add(d.Name);
                }
            }
            if (listViewDiscos.Items.Count == 0)
            {
                Log.Instance.ImprimirMensaje("No hay disqueteras", TipoMensaje.Advertencia);
                MessageBox.Show("No tienes disqueteras");
                Close();
                Dispose();
            }
            else
                Log.Instance.ImprimirMensaje("Se han detectado " + listViewDiscos.Items.Count + " disqueteras", TipoMensaje.Correcto);

        }

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            if(listViewDiscos.SelectedItems.Count == 1)
            {
                Reproductor.Instancia.ReproducirCD(listViewDiscos.SelectedItems[0].Text[0]);
                Close();
                Dispose();
            }
        }
    }
}
