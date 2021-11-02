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
        Label labelClipboardPreview;
        Label labelClipboardHelp;
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
            Text = Kernel.LocalTexts.GetString("configuracion");
            labelSelect.Text = Kernel.LocalTexts.GetString("seleccione_opcion");
            buttonAplicar.Text = Kernel.LocalTexts.GetString("aplicar");
            buttonOK.Text = Kernel.LocalTexts.GetString("aceptar");
            buttonCancelar.Text = Kernel.LocalTexts.GetString("cancelar");
            treeViewConfiguracion.Nodes[0].Text = Kernel.LocalTexts.GetString("idioma");
            treeViewConfiguracion.Nodes[1].Text = Kernel.LocalTexts.GetString("cambiar_portapapeles");
            treeViewConfiguracion.Nodes[2].Text = Kernel.LocalTexts.GetString("configView");
            treeViewConfiguracion.Nodes[2].Nodes[0].Text = Kernel.LocalTexts.GetString("tipografíaLyrics");
            treeViewConfiguracion.Nodes[2].Nodes[1].Text = Kernel.LocalTexts.GetString("colorsHighlight");
        }
        private void LoadLanguageConfig()
        {
            config = ActiveConfig.Language;
            radioButtonsIdiomas = new RadioButton[Kernel.NumLanguages];
            PictureBox[] pictureBoxesIdiomas = new PictureBox[Kernel.NumLanguages];
            groupBoxRaiz.Text = Kernel.LocalTexts.GetString("cambiar_idioma");
            int y = 44;
            for (int i = 0; i < Kernel.NumLanguages; i++)
            {
                radioButtonsIdiomas[i] = new RadioButton();
                radioButtonsIdiomas[i].Location = new Point(44, y);
                radioButtonsIdiomas[i].Text = Kernel.Languages[i];
                if (radioButtonsIdiomas[i].Text == Config.Language)
                    radioButtonsIdiomas[i].Checked = true;
                radioButtonsIdiomas[i].Font = new Font("Segoe UI", 9);
                pictureBoxesIdiomas[i] = new PictureBox();
                pictureBoxesIdiomas[i].Location = new Point(6, y);
                pictureBoxesIdiomas[i].Size = new Size(32, 32);
                pictureBoxesIdiomas[i].SizeMode = PictureBoxSizeMode.StretchImage;
                CultureInfo nombreIdioma = new CultureInfo(Kernel.Languages[i]);
                radioButtonsIdiomas[i].Text = nombreIdioma.NativeName;
                pictureBoxesIdiomas[i].Image = Config.GetIconoBandera(Kernel.Languages[i]);

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
            labelClipboardPreview = new Label();
            labelClipboardHelp = new Label();
            groupBoxRaiz.Text = Kernel.LocalTexts.GetString("cambiar_portapapeles");
            portapapelesConfig = new TextBox();
            portapapelesConfig.TextChanged += PortapapelesConfig_TextChanged;
            portapapelesConfig.Location = new Point(44, groupBoxRaiz.Size.Height / 2);
            Size size = portapapelesConfig.Size; size.Width = 500; portapapelesConfig.Size = size;
            portapapelesConfig.Font = new Font("Segoe UI", 9);
            portapapelesConfig.Text = Config.Clipboard;
            labelClipboardPreview.Location = new Point(portapapelesConfig.Location.X, portapapelesConfig.Location.Y + 30);
            labelClipboardPreview.AutoSize = true;
            labelClipboardPreview.Font = portapapelesConfig.Font;
            labelClipboardHelp.Font = portapapelesConfig.Font;
            labelClipboardHelp.AutoSize = true;
            labelClipboardHelp.Location = new Point(labelClipboardPreview.Location.X, labelClipboardPreview.Location.Y + 50);
            labelClipboardHelp.Text = @"%artist% - Album artist
%title% - Album title
%year% - Album release year
%genre% - Album genre
%length_min% - Album duration in minutes
%length_seconds% - Album length in seconds, formatted as an integer
%length% - Length of the album, formatted as [HH:]MM:SS. 
%totaltracks% - Total number of tracks
%path% - Path of local files, if avaliable";
            string Preview = "";
            if (Kernel.Collection.Albums.Count == 0)
                Preview = Config.Clipboard.Replace("%artist%", AlbumCopyPreview.Artist); //Es seguro.
            else
            {
                //Select a random album from the collection.
                Random random = new Random();
                AlbumData AlbumRef = Kernel.Collection.Albums[random.Next(Kernel.Collection.Albums.Count)];
                AlbumCopyPreview = AlbumRef;
                Preview = Config.Clipboard.Replace("%artist%", AlbumCopyPreview.Artist);
            }
            portapapelesConfig.Text = Config.Clipboard;
            PortapapelesConfig_TextChanged(null, null);
            groupBoxRaiz.Controls.Add(portapapelesConfig);
            groupBoxRaiz.Controls.Add(labelClipboardPreview);
            groupBoxRaiz.Controls.Add(labelClipboardHelp);
        }

        private void LoadColorConfig()
        {
            config = ActiveConfig.Colors;
            groupBoxRaiz.Text = Kernel.LocalTexts.GetString("colorsHighlight");
            //Create stuff
            btColorBonus = new Button();
            btColorLongSong = new Button();
            ttbtColorBonus = new ToolTip();
            ttbtColorLongSong = new ToolTip();

            ttbtColorLongSong.SetToolTip(btColorLongSong, Kernel.LocalTexts.GetString("helpColorLongSong"));
            ttbtColorBonus.SetToolTip(btColorBonus, Kernel.LocalTexts.GetString("helpColorBonus"));
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
            btColorBonus.Text = Kernel.LocalTexts.GetString("bonus");

            btColorLongSong.BackColor = Config.ColorLongSong;
            btColorLongSong.Text = Kernel.LocalTexts.GetString("longSong");

            //Config internal tags
            btColorBonus.Tag = "bonus";
            btColorLongSong.Tag = "longsong";

            groupBoxRaiz.Controls.Add(btColorBonus);
            groupBoxRaiz.Controls.Add(btColorLongSong);
        }

        private void LoadTextConfig()
        {
            groupBoxRaiz.Text = Kernel.LocalTexts.GetString("tipografíaLyrics");
            config = ActiveConfig.TextFont;
            //Create stuff
            btTextLyrics = new Button();
            btTextView = new Button();
            ttbtTextLyrics = new ToolTip();
            ttbtTextView = new ToolTip();

            ttbtTextView.SetToolTip(btTextView, Kernel.LocalTexts.GetString("helpView"));
            ttbtTextLyrics.SetToolTip(btTextLyrics, Kernel.LocalTexts.GetString("helpLyrics"));
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

            btTextLyrics.Text = Kernel.LocalTexts.GetString("lyrics");

            btTextView.Text = Kernel.LocalTexts.GetString("configView");

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
                            Kernel.ChangeLanguage(Kernel.Languages[i]);
                    }
                    PonerTextos();
                    groupBoxRaiz.Text = Kernel.LocalTexts.GetString("cambiar_idioma");
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
                    Kernel.ReloadView();
                    break;
                default:
                    break;
            }
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
                Preview = Preview.Replace("%length_min%", AlbumCopyPreview.Length.TotalMinutes.ToString());
                Preview = Preview.Replace("%totaltracks%", AlbumCopyPreview.NumberOfSongs.ToString());
                Preview = Preview.Replace("%path%", AlbumCopyPreview.SoundFilesPath);
                labelClipboardPreview.Text = Preview;
            }
            catch (NullReferenceException)
            {
                labelClipboardPreview.Text = Preview;
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
