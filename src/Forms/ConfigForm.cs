using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace aplicacion_musica.src.Forms
{
    enum ConfigActiva
    {
        Idioma,
        Portapapeles
    }
    public partial class ConfigForm : Form
    {
        RadioButton[] radioButtonsIdiomas;
        TextBox portapapelesConfig;
        Label vistaPreviaPortapapeles;
        AlbumData test = new AlbumData("Sabbath Bloddy Sabbath", "Black Sabbath", 1973);
        ConfigActiva config;
        public ConfigForm()
        {
            InitializeComponent();
        }

        private void ConfigForm_Load(object sender, EventArgs e)
        {
            label1.Show();
            
            PonerTextos();
            label1.Location = new Point(290 - (label1.Size.Width / 2), groupBoxRaiz.Size.Height / 2);
        }
        private void PonerTextos()
        {
            Text = Program.LocalTexts.GetString("configuracion");
            treeViewConfiguracion.Nodes[0].Text = Program.LocalTexts.GetString("idioma");
            label1.Text = Program.LocalTexts.GetString("seleccione_opcion");
            buttonAplicar.Text = Program.LocalTexts.GetString("aplicar");
            buttonOK.Text = Program.LocalTexts.GetString("aceptar");
            buttonCancelar.Text = Program.LocalTexts.GetString("cancelar");
            treeViewConfiguracion.Nodes[1].Text = Program.LocalTexts.GetString("cambiar_portapapeles");
        }
        private void CargarIdiomas()
        {
            config = ConfigActiva.Idioma;
            radioButtonsIdiomas = new RadioButton[Program.NumIdiomas];
            PictureBox[] pictureBoxesIdiomas = new PictureBox[Program.NumIdiomas];
            groupBoxRaiz.Text = Program.LocalTexts.GetString("cambiar_idioma");
            int y = 44;
            for (int i = 0; i < Program.NumIdiomas; i++)
            {
                radioButtonsIdiomas[i] = new RadioButton();
                radioButtonsIdiomas[i].Location = new Point(44, y);
                radioButtonsIdiomas[i].Text = Program.idiomas[i];
                if (radioButtonsIdiomas[i].Text == Config.Idioma)
                    radioButtonsIdiomas[i].Checked = true;
                radioButtonsIdiomas[i].Font = new Font("Segoe UI", 9);
                pictureBoxesIdiomas[i] = new PictureBox();
                pictureBoxesIdiomas[i].Location = new Point(6, y);
                pictureBoxesIdiomas[i].Size = new Size(32, 32);
                pictureBoxesIdiomas[i].SizeMode = PictureBoxSizeMode.StretchImage;
                CultureInfo nombreIdioma = new CultureInfo(Program.idiomas[i]);
                radioButtonsIdiomas[i].Text = nombreIdioma.NativeName;
                switch (Program.idiomas[i])
                {
                    case "es":
                        pictureBoxesIdiomas[i].Image = Properties.Resources.es;
                        break;
                    case "ca":
                        pictureBoxesIdiomas[i].Image = Properties.Resources.ca;
                        break;
                    case "en":
                        pictureBoxesIdiomas[i].Image = Properties.Resources.en;
                        break;
                    case "el":
                        pictureBoxesIdiomas[i].Image = Properties.Resources.el;
                        break;
                }
                groupBoxRaiz.Controls.Add(radioButtonsIdiomas[i]);
                groupBoxRaiz.Controls.Add(pictureBoxesIdiomas[i]);
                y += 35;
                radioButtonsIdiomas[i].Show();
                pictureBoxesIdiomas[i].Show();
            }
        }
        private void CargarPortapapeles()
        {
            config = ConfigActiva.Portapapeles;
            vistaPreviaPortapapeles = new Label();
            groupBoxRaiz.Text = Program.LocalTexts.GetString("cambiar_portapapeles");
            portapapelesConfig = new TextBox();
            portapapelesConfig.TextChanged += PortapapelesConfig_TextChanged;
            portapapelesConfig.Location = new Point(44, groupBoxRaiz.Size.Height / 2);
            Size size = portapapelesConfig.Size; size.Width = 500; portapapelesConfig.Size = size;
            portapapelesConfig.Font = new Font("Segoe UI", 9);
            portapapelesConfig.Text = Config.Portapapeles;
            vistaPreviaPortapapeles.Location = new Point(portapapelesConfig.Location.X, portapapelesConfig.Location.Y + 30);
            vistaPreviaPortapapeles.AutoSize = true;
            vistaPreviaPortapapeles.Font = portapapelesConfig.Font;
            string val = Config.Portapapeles.Replace("%artist%", test.Artist); //Es seguro.
            try
            {
                val = val.Replace("%artist%", test.Artist);
                val = val.Replace("%title%", test.Title);
                val = val.Replace("%year%", test.Year.ToString());
                val = val.Replace("%genre%", test.Genre.Name);
                val = val.Replace("%length%", test.Length.ToString());
                val = val.Replace("%length_seconds%", ((int)test.Length.TotalSeconds).ToString());
                vistaPreviaPortapapeles.Text = val;
            }
            catch (NullReferenceException)
            {
                vistaPreviaPortapapeles.Text = val;
            }
            groupBoxRaiz.Controls.Add(portapapelesConfig);
            groupBoxRaiz.Controls.Add(vistaPreviaPortapapeles);
        }

        private void PortapapelesConfig_TextChanged(object sender, EventArgs e)
        {
            string val = portapapelesConfig.Text.Replace("%artist%", test.Artist); //Es seguro.
            try
            {
                val = val.Replace("%artist%", test.Artist);
                val = val.Replace("%title%", test.Title);
                val = val.Replace("%year%", test.Year.ToString());
                val = val.Replace("%genre%", test.Genre.Name);
                val = val.Replace("%length%", test.Length.ToString());
                val = val.Replace("%length_seconds%", ((int)test.Length.TotalSeconds).ToString());
                vistaPreviaPortapapeles.Text = val;
            }
            catch (NullReferenceException)
            {
                vistaPreviaPortapapeles.Text = val;
            }
        }
        private void Limpiar()
        {
            label1.Show();
        }
        private void CargarPagina(string tag)
        {
            label1.Hide();
            groupBoxRaiz.Controls.Clear();

            switch (tag)
            {
                case "idioma":
                    CargarIdiomas();
                    break;
                case "portapapeles":
                    CargarPortapapeles();
                    break;
                default:
                    groupBoxRaiz.Controls.Add(label1);
                    label1.Show();
                    break;
            }
        }
        private void groupBoxIdioma_Enter(object sender, EventArgs e)
        {

        }

        private void treeViewConfiguracion_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            CargarPagina(e.Node.Tag.ToString());
        }
        private void Aplicar(ConfigActiva config)
        {
            switch (config)
            {
                case ConfigActiva.Idioma:
                    for (int i = 0; i < radioButtonsIdiomas.Length; i++)
                    {
                        if (radioButtonsIdiomas[i].Checked)
                            Program.ChangeLanguage(Program.idiomas[i]);
                    }
                    PonerTextos();
                    break;
                case ConfigActiva.Portapapeles:
                    Config.Portapapeles = portapapelesConfig.Text;
                    break;
                default:
                    break;
            }

        }
        private void buttonAplicar_Click(object sender, EventArgs e)
        {
            Aplicar(config);
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Aplicar(config);
            Close();
        }
    }
}
