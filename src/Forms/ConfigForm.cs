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
        Language,
        Clipboard
    }
    public partial class ConfigForm : Form
    {
        RadioButton[] radioButtonsIdiomas;
        TextBox portapapelesConfig;
        Label vistaPreviaPortapapeles;
        AlbumData AlbumCopyPreview = new AlbumData("Sabbath Bloddy Sabbath", "Black Sabbath", 1973); //Only used if the collection is empty.
        ConfigActiva config;
        public ConfigForm()
        {
            InitializeComponent();
        }

        private void ConfigForm_Load(object sender, EventArgs e)
        {
            labelSelect.Show();
            
            PonerTextos();
            labelSelect.Location = new Point(290 - (labelSelect.Size.Width / 2), groupBoxRaiz.Size.Height / 2);
        }
        private void PonerTextos()
        {
            Text = Program.LocalTexts.GetString("configuracion");
            treeViewConfiguracion.Nodes[0].Text = Program.LocalTexts.GetString("idioma");
            labelSelect.Text = Program.LocalTexts.GetString("seleccione_opcion");
            buttonAplicar.Text = Program.LocalTexts.GetString("aplicar");
            buttonOK.Text = Program.LocalTexts.GetString("aceptar");
            buttonCancelar.Text = Program.LocalTexts.GetString("cancelar");
            treeViewConfiguracion.Nodes[1].Text = Program.LocalTexts.GetString("cambiar_portapapeles");
        }
        private void CargarIdiomas()
        {
            config = ConfigActiva.Language;
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
            config = ConfigActiva.Clipboard;
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
            string Preview = "";
            if (Program.Collection.Albums.Count == 0)
                Preview = Config.Portapapeles.Replace("%artist%", AlbumCopyPreview.Artist); //Es seguro.
            else
            {
                //Select a random album from the collection.
                Random random = new Random();
                AlbumData AlbumRef = Program.Collection.Albums[random.Next(Program.Collection.Albums.Count)];
                AlbumCopyPreview = AlbumRef;
                Preview = Config.Portapapeles.Replace("%artist%", AlbumCopyPreview.Artist);
            }
            try
            {
                Preview = Preview.Replace("%artist%", AlbumCopyPreview.Artist);
                Preview = Preview.Replace("%title%", AlbumCopyPreview.Title);
                Preview = Preview.Replace("%year%", AlbumCopyPreview.Year.ToString());
                Preview = Preview.Replace("%genre%", AlbumCopyPreview.Genre.Name);
                Preview = Preview.Replace("%length%", AlbumCopyPreview.Length.ToString());
                Preview = Preview.Replace("%length_seconds%", ((int)AlbumCopyPreview.Length.TotalSeconds).ToString());
                vistaPreviaPortapapeles.Text = Preview;
            }
            catch (NullReferenceException)
            {
                vistaPreviaPortapapeles.Text = Preview;
            }
            groupBoxRaiz.Controls.Add(portapapelesConfig);
            groupBoxRaiz.Controls.Add(vistaPreviaPortapapeles);
        }

        private void PortapapelesConfig_TextChanged(object sender, EventArgs e)
        {
            string Preview = portapapelesConfig.Text.Replace("%artist%", AlbumCopyPreview.Artist); //Es seguro.
            try
            {
                Preview = Preview.Replace("%artist%", AlbumCopyPreview.Artist);
                Preview = Preview.Replace("%title%", AlbumCopyPreview.Title);
                Preview = Preview.Replace("%year%", AlbumCopyPreview.Year.ToString());
                Preview = Preview.Replace("%genre%", AlbumCopyPreview.Genre.Name);
                Preview = Preview.Replace("%length%", AlbumCopyPreview.Length.ToString());
                Preview = Preview.Replace("%length_seconds%", ((int)AlbumCopyPreview.Length.TotalSeconds).ToString());
                vistaPreviaPortapapeles.Text = Preview;
            }
            catch (NullReferenceException)
            {
                vistaPreviaPortapapeles.Text = Preview;
            }
        }
        private void Limpiar()
        {
            labelSelect.Show();
        }
        private void CargarPagina(string tag)
        {
            labelSelect.Hide();
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
                    groupBoxRaiz.Controls.Add(labelSelect);
                    labelSelect.Show();
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
                case ConfigActiva.Language:
                    for (int i = 0; i < radioButtonsIdiomas.Length; i++)
                    {
                        if (radioButtonsIdiomas[i].Checked)
                            Program.ChangeLanguage(Program.idiomas[i]);
                    }
                    PonerTextos();
                    break;
                case ConfigActiva.Clipboard:
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
