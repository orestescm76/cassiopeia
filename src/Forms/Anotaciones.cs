using Cassiopeia.src.Classes;
using System;
using System.Windows.Forms;

namespace Cassiopeia.src.Forms
{
    public partial class Anotaciones : Form
    {
        IPhysicalAlbum media;
        public Anotaciones(IPhysicalAlbum media)
        {
            InitializeComponent();
            this.media = media;
            textBox1.Lines = media.Anotaciones;
            buttonOk.Text = Kernel.GetText("hecho");
        }
        private void buttonOk_Click(object sender, EventArgs e)
        {
            media.Anotaciones = textBox1.Lines;
            Log.Instance.PrintMessage("Guardado " + media.Anotaciones.Length + " bytes", MessageType.Correct);
            Kernel.SetSaveMark();
            Dispose();
        }
    }
}
