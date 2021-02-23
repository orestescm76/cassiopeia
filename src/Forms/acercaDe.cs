using System;
using System.Drawing;
using System.Windows.Forms;

namespace Cassiopea
{
    public partial class acercaDe : Form
    {
        private bool bannerAntiguo = false;
        public acercaDe()
        {
            InitializeComponent();
            Text = Program.LocalTexts.GetString("acerca") + " " + Program.LocalTexts.GetString("titulo_ventana_principal") + " " + Program.Version;
            PonerTextos();
        }
        private void PonerTextos()
        {
            labelAcercaDe.Text = "";
            labelAcercaDe.AutoSize = true;
            string acercadeTexto = Program.Version + " Codename " + Program.CodeName;
            acercadeTexto += Environment.NewLine;
            acercadeTexto += Program.LocalTexts.GetString("desarrolladoPor") + " Orestes Colomina Monsalve" + Environment.NewLine +
                Program.LocalTexts.GetString("contacto") + Environment.NewLine + Environment.NewLine + Program.LocalTexts.GetString("agradecimientosA") + Environment.NewLine +
                Program.LocalTexts.GetString("agradecimiento1") + Environment.NewLine + "https://github.com/JohnnyCrazy/SpotifyAPI-NET" + Environment.NewLine +
                Program.LocalTexts.GetString("agradecimiento3") + Environment.NewLine +
                Program.LocalTexts.GetString("agradecimiento4") + Environment.NewLine;

            switch (Config.Idioma)
            {
                case "ca":
                    acercadeTexto += Program.LocalTexts.GetString("agradecimientoTraduccion");
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
            int posX = Width - labelAcercaDe.Size.Width;
            labelAcercaDe.Location = new Point(posX / 2, pictureBoxBanner.Size.Height + 1);
        }
        private void cambiarBanner()
        {
            if (!bannerAntiguo)
                pictureBoxBanner.Image = Properties.Resources.banner;
            else
                pictureBoxBanner.Image = Properties.Resources.banner1_5;
        }
        private void acercaDe_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
            {
                Close();
                Dispose();
            }
        }

        private void acercaDe_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if ((e.Control & e.Alt) && e.KeyCode == Keys.F9)
            {
                bannerAntiguo = !bannerAntiguo;
                cambiarBanner();
            }
        }
    }
}
