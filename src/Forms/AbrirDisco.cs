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
        private void PonerTextos()
        {
            Text = Program.LocalTexts.GetString("abrirCD");
            buttonPlay.Text = Program.LocalTexts.GetString("reproducir");
            buttonRip.Text = "Rip";
        }
        private void AbrirDisco_Load(object sender, EventArgs e)
        {
            PonerTextos();
            Log.Instance.PrintMessage("Detectando disqueteras", MessageType.Info);
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
                Log.Instance.PrintMessage("No hay disqueteras", MessageType.Warning);
                MessageBox.Show("No tienes disqueteras");
                Close();
                Dispose();
            }
            else
                Log.Instance.PrintMessage("Se han detectado " + listViewDiscos.Items.Count + " disqueteras", MessageType.Correct);

        }

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            if(listViewDiscos.SelectedItems.Count == 1)
            {
                Reproductor.Instancia.PlayCD(listViewDiscos.SelectedItems[0].Text[0]);
                Close();
                Dispose();
            }
        }
    }
}
