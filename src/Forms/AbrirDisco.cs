using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Cassiopeia.src.Forms
{
    public partial class AbrirDisco : Form
    {
        public AbrirDisco()
        {
            InitializeComponent();
        }
        private void PonerTextos()
        {
            Text = Kernel.LocalTexts.GetString("abrirCD");
            buttonPlay.Text = Kernel.LocalTexts.GetString("reproducir");
            buttonRip.Text = "Rip";
        }
        private void AbrirDisco_Load(object sender, EventArgs e)
        {
            PonerTextos();
            Log.Instance.PrintMessage("Detecting CD Drives", MessageType.Info);
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
                Log.Instance.PrintMessage("No CD Drives avaliable", MessageType.Warning);
                MessageBox.Show(Kernel.LocalTexts.GetString("noDisqueteras"));
                Close();
                Dispose();
            }
            else
                Log.Instance.PrintMessage("Found " + listViewDiscos.Items.Count + " CD Drives", MessageType.Correct);

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
