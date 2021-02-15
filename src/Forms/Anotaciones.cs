using System;
using Newtonsoft.Json;
using System.Windows.Forms;

namespace aplicacion_musica
{
    public partial class Anotaciones : Form
    {
        DiscoCompacto cd;
        public Anotaciones(ref DiscoCompacto cd)
        {
            InitializeComponent();
            this.cd = cd;
            textBox1.Lines = cd.Anotaciones;
            buttonOk.Text = Program.LocalTexts.GetString("hecho");
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            cd.Anotaciones = textBox1.Lines;
            Log.Instance.ImprimirMensaje("Guardado " + cd.Anotaciones.Length + " bytes", TipoMensaje.Correcto);
            Dispose();
        }
    }
}
