using System;
using System.Drawing;
using System.Windows.Forms;

namespace aplicacion_musica
{
    public partial class acercaDe : Form
    {
        public acercaDe()
        {
            InitializeComponent();
            Text = Programa.textosLocal.GetString("acerca") + " " + Programa.textosLocal.GetString("titulo_ventana_principal") + " " + Programa.version;
            PonerTextos();
        }
        private void PonerTextos()
        {
            labelAcercaDe.Text = "";
            string acercadeTexto = Programa.version + "Codename " + Programa.CodeName;
            acercadeTexto += Environment.NewLine;
            acercadeTexto += Programa.textosLocal.GetString("desarrolladoPor") + " Orestes Colomina Monsalve" + Environment.NewLine +
                Programa.textosLocal.GetString("contacto") + Environment.NewLine + Environment.NewLine + Programa.textosLocal.GetString("agradecimientosA") + Environment.NewLine +
                Programa.textosLocal.GetString("agradecimiento1") + Environment.NewLine + "https://github.com/JohnnyCrazy/SpotifyAPI-NET" + Environment.NewLine +
                Programa.textosLocal.GetString("agradecimiento3") + Environment.NewLine +
                Programa.textosLocal.GetString("agradecimiento4") + Environment.NewLine;
            switch (Config.Idioma)
            {
                case "ca":
                    acercadeTexto += Programa.textosLocal.GetString("agradecimientoTraduccion");
                    break;
                case "el":
                    labelAcercaDe.Location = new Point(2, 186);
                    Font but = labelAcercaDe.Font;
                    Font neo = new Font(but.FontFamily, 10);
                    labelAcercaDe.Font = neo;
                    break;
                default:
                    break;
            }
            labelAcercaDe.Text = acercadeTexto;
        }
    }
}
