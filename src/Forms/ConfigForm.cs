using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace Cassiopeia.src.Forms
{
    enum ActiveConfig
    {
        Language,
        Clipboard,
        Colors,
        TextFont
    }
    public partial class ConfigForm : Form
    {
        RadioButton[] radioButtonsIdiomas;
        TextBox portapapelesConfig;
        Label vistaPreviaPortapapeles;
        AlbumData AlbumCopyPreview = new AlbumData("Sabbath Bloddy Sabbath", "Black Sabbath", 1973); //Only used if the collection is empty.

        Button btColorBonus;
        Button btColorLongSong;

        ToolTip ttbtColorBonus;
        ToolTip ttbtColorLongSong;
        
        //For text config
        Button btTextLyrics;
        Button btTextView;

        ToolTip ttbtTextLyrics;
        ToolTip ttbtTextView;

        ActiveConfig config;
        public ConfigForm()
        {
            InitializeComponent();
        }

        private void ConfigForm_Load(object sender, EventArgs e)
        {
            labelSelect.Show();
            
            PonerTextos();
            labelSelect.Location = new Point(290 - (labelSelect.Size.Width / 2), groupBoxRaiz.Size.Height / 2);
            Icon = Properties.Resources.settings;
            treeViewConfiguracion.ExpandAll();
        }
        private void PonerTextos()
        {
            Text = Program.LocalTexts.GetString("configuracion");
            labelSelect.Text = Program.LocalTexts.GetString("seleccione_opcion");
            buttonAplicar.Text = Program.LocalTexts.GetString("aplicar");
            buttonOK.Text = Program.LocalTexts.GetString("aceptar");
            buttonCancelar.Text = Program.LocalTexts.GetString("cancelar");
            treeViewConfiguracion.Nodes[0].Text = Program.LocalTexts.GetString("idioma");
            treeViewConfiguracion.Nodes[1].Text = Program.LocalTexts.GetString("cambiar_portapapeles");
            treeViewConfiguracion.Nodes[2].Text = Program.LocalTexts.GetString("configView");
            treeViewConfiguracion.Nodes[2].Nodes[0].Text = Program.LocalTexts.GetString("tipografíaLyrics");
            treeViewConfiguracion.Nodes[2].Nodes[1].Text = Program.LocalTexts.GetString("colorsHighlight");
        }
        private void LoadLanguageConfig()
        {
            config = ActiveConfig.Language;
            radioButtonsIdiomas = new RadioButton[Program.NumIdiomas];
            PictureBox[] pictureBoxesIdiomas = new PictureBox[Program.NumIdiomas];
            groupBoxRaiz.Text = Program.LocalTexts.GetString("cambiar_idioma");
            int y = 44;
            for (int i = 0; i < Program.NumIdiomas; i++)
            {
                radioButtonsIdiomas[i] = new RadioButton();
                radioButtonsIdiomas[i].Location = new Point(44, y);
                radioButtonsIdiomas[i].Text = Program.idiomas[i];
                if (radioButtonsIdiomas[i].Text == Config.Language)
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
        private void LoadClipboardConfig()
        {
            config = ActiveConfig.Clipboard;
            vistaPreviaPortapapeles = new Label();
            groupBoxRaiz.Text = Program.LocalTexts.GetString("cambiar_portapapeles");
            portapapelesConfig = new TextBox();
            portapapelesConfig.TextChanged += PortapapelesConfig_TextChanged;
            portapapelesConfig.Location = new Point(44, groupBoxRaiz.Size.Height / 2);
            Size size = portapapelesConfig.Size; size.Width = 500; portapapelesConfig.Size = size;
            portapapelesConfig.Font = new Font("Segoe UI", 9);
            portapapelesConfig.Text = Config.Clipboard;
            vistaPreviaPortapapeles.Location = new Point(portapapelesConfig.Location.X, portapapelesConfig.Location.Y + 30);
            vistaPreviaPortapapeles.AutoSize = true;
            vistaPreviaPortapapeles.Font = portapapelesConfig.Font;
            string Preview = "";
            if (Program.Collection.Albums.Count == 0)
                Preview = Config.Clipboard.Replace("%artist%", AlbumCopyPreview.Artist); //Es seguro.
            else
            {
                //Select a random album from the collection.
                Random random = new Random();
                AlbumData AlbumRef = Program.Collection.Albums[random.Next(Program.Collection.Albums.Count)];
                AlbumCopyPreview = AlbumRef;
                Preview = Config.Clipboard.Replace("%artist%", AlbumCopyPreview.Artist);
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

        private void LoadColorConfig()
        {
            config = ActiveConfig.Colors;
            groupBoxRaiz.Text = Program.LocalTexts.GetString("colorsHighlight");
            //Create stuff
            btColorBonus = new Button();
            btColorLongSong = new Button();
            ttbtColorBonus = new ToolTip();
            ttbtColorLongSong = new ToolTip();

            ttbtColorLongSong.SetToolTip(btColorLongSong, Program.LocalTexts.GetString("helpColorLongSong"));
            ttbtColorBonus.SetToolTip(btColorBonus, Program.LocalTexts.GetString("helpColorBonus"));
            //Event config
            btColorBonus.Click += buttonColor_Click;
            btColorLongSong.Click += buttonColor_Click;

            btColorBonus.Size = new Size(220, 60);
            btColorLongSong.Size = btColorBonus.Size;

            int x = (groupBoxRaiz.Width / 2) - 110;
            int y = groupBoxRaiz.Height / 2;

            btColorBonus.Location = new Point(x, y - 70);
            btColorLongSong.Location = new Point(x, y + 10);

            btColorBonus.BackColor = Config.ColorBonus;
            btColorBonus.Text = Program.LocalTexts.GetString("bonus");

            btColorLongSong.BackColor = Config.ColorLongSong;
            btColorLongSong.Text = Program.LocalTexts.GetString("longSong");

            //Config internal tags
            btColorBonus.Tag = "bonus";
            btColorLongSong.Tag = "longsong";

            groupBoxRaiz.Controls.Add(btColorBonus);
            groupBoxRaiz.Controls.Add(btColorLongSong);
        }

        private void LoadTextConfig()
        {
            groupBoxRaiz.Text = Program.LocalTexts.GetString("tipografíaLyrics");
            config = ActiveConfig.TextFont;
            //Create stuff
            btTextLyrics = new Button();
            btTextView = new Button();
            ttbtTextLyrics = new ToolTip();
            ttbtTextView = new ToolTip();

            ttbtTextView.SetToolTip(btTextView, Program.LocalTexts.GetString("helpView"));
            ttbtTextLyrics.SetToolTip(btTextLyrics, Program.LocalTexts.GetString("helpLyrics"));
            //Event config
            btTextLyrics.Click += buttonText_Click;
            btTextView.Click += buttonText_Click;

            btTextLyrics.Size = new Size(220, 60);
            btTextView.Size = btTextLyrics.Size;

            int x = (groupBoxRaiz.Width / 2) - 110;
            int y = groupBoxRaiz.Height / 2;

            btTextLyrics.Location = new Point(x, y - 70);
            btTextView.Location = new Point(x, y + 10);

            btTextLyrics.Font = Config.FontLyrics;
            btTextView.Font = Config.FontView;

            btTextLyrics.Text = Program.LocalTexts.GetString("lyrics");

            btTextView.Text = Program.LocalTexts.GetString("view");

            //Config internal tags
            btTextLyrics.Tag = "lyrics";
            btTextView.Tag = "view";

            groupBoxRaiz.Controls.Add(btTextLyrics);
            groupBoxRaiz.Controls.Add(btTextView);
        }
        private void Aplicar(ActiveConfig config)
        {
            switch (config)
            {
                case ActiveConfig.Language:
                    for (int i = 0; i < radioButtonsIdiomas.Length; i++)
                    {
                        if (radioButtonsIdiomas[i].Checked)
                            Program.ChangeLanguage(Program.idiomas[i]);
                    }
                    PonerTextos();
                    break;
                case ActiveConfig.Clipboard:
                    Config.Clipboard = portapapelesConfig.Text;
                    break;
                case ActiveConfig.Colors:
                    Config.ColorBonus = btColorBonus.BackColor;
                    Config.ColorLongSong = btColorLongSong.BackColor;
                    break;
                case ActiveConfig.TextFont:
                    Config.FontView = btTextView.Font;
                    Config.FontLyrics = btTextLyrics.Font;
                    Program.ReloadView();
                    break;
                default:
                    break;
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
                case "language":
                    LoadLanguageConfig();
                    break;
                case "clipboard":
                    LoadClipboardConfig();
                    break;
                case "colors":
                    LoadColorConfig();
                    break;
                case "text":
                    LoadTextConfig();
                    break;
                default:
                    groupBoxRaiz.Controls.Add(labelSelect);
                    labelSelect.Show();
                    groupBoxRaiz.Text = "";
                    break;
            }
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
        //Events
        private void groupBoxIdioma_Enter(object sender, EventArgs e)
        {

        }

        private void treeViewConfiguracion_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            CargarPagina(e.Node.Tag.ToString());
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
        private void buttonColor_Click(object sender, EventArgs e)
        {
            Button btSender = (Button)sender;
            Color newColor = btSender.BackColor;

            ColorDialog colorDialog = new ColorDialog();
            colorDialog.Color = btSender.BackColor;

            colorDialog.ShowDialog();
            newColor = colorDialog.Color;

            btSender.BackColor = newColor;
        }

        private void buttonText_Click(object sender, EventArgs e)
        {
            Button btSender = (Button)sender;
            
            FontDialog fontDialog = new FontDialog();
            fontDialog.Font = btSender.Font;
            fontDialog.ShowDialog();

            btSender.Font = fontDialog.Font;
        }
    }
}
