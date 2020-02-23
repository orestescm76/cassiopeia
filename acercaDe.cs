using System.Resources;
using System.Windows.Forms;

namespace aplicacion_musica
{
    public partial class acercaDe : Form
    {
        public acercaDe()
        {
            InitializeComponent();
            labelAcercaDe.Text = labelAcercaDe.Text.Replace("{ver}", Programa.version);
            Text = Programa.textosLocal.GetString("acerca") + " " + Programa.textosLocal.GetString("titulo_ventana_principal") + " " + Programa.version;
        }
    }
}
