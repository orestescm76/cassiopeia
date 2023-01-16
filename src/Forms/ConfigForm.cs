using Cassiopeia.src.Classes;
using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace Cassiopeia.src.Forms
{
    public enum ActiveConfig
    {
        Language,
        Clipboard,
        History,
        Colors,
        TextFont,
        Stream,
        Twitter
    }
    public partial class ConfigForm : Form
    {
        RadioButton[] radioButtonsIdiomas;
        //Common to clipboard and historial config..
        TextBox stringConfig;
        Label labelStringConfigPreview;
        Label labelStringConfigHelp;

        CheckBox checkBoxHistoryConfigCheckBox;
        CheckBox streamEnabledConfigCheckBox;

        AlbumData AlbumCopyPreview = new AlbumData("Sabbath Bloddy Sabbath", "Black Sabbath", 1973); //Only used if the collection is empty.
        Song SongPreview;

        Button btColorBonus;
        Button btColorLongSong;

        ToolTip ttbtColorBonus;
        ToolTip ttbtColorLongSong;

        //For text config
        Button btTextLyrics;
        Button btTextView;

        ToolTip ttbtTextLyrics;
        ToolTip ttbtTextView;

        Random random = new Random();

        public ActiveConfig ActiveConfig { get; set; }
        public ConfigForm()
        {
            InitializeComponent();
            SongPreview = new()
            {
                Title = "A National Acrobat",
                Length = TimeSpan.FromMilliseconds(375000),
                IsBonus = false,
                AlbumFrom = AlbumCopyPreview
            };
        }
        public ConfigForm(bool loadMetadata)
        {
            InitializeComponent();
            SongPreview = new()
            {
                Title = "A National Acrobat",
                Length = TimeSpan.FromMilliseconds(375000),
                IsBonus = false,
                AlbumFrom = AlbumCopyPreview
            };
            if (loadMetadata)
            {
                treeViewConfiguracion.Visible = false;
                LoadStreamConfig();
            }
        }
        private void ConfigForm_Load(object sender, EventArgs e)
        {
            //labelSelect.Show();

            PonerTextos();
            labelSelect.Location = new Point(groupBoxRaiz.Size.Width / 2 - (labelSelect.Size.Width / 2), groupBoxRaiz.Size.Height / 2);
            Icon = Properties.Resources.settings;
            treeViewConfiguracion.ExpandAll();
            labelSelect.Hide();
            SongPreview.SetAlbum(AlbumCopyPreview);
            //LoadLanguageConfig();

        }
        private void PonerTextos()
        {
            Text = Kernel.GetText("configuracion");
            labelSelect.Text = Kernel.GetText("seleccione_opcion");
            buttonAplicar.Text = Kernel.GetText("aplicar");
            buttonOK.Text = Kernel.GetText("aceptar");
            buttonCancelar.Text = Kernel.GetText("cancelar");
            treeViewConfiguracion.Nodes[0].Text = Kernel.GetText("idioma");
            treeViewConfiguracion.Nodes[1].Text = Kernel.GetText("cambiar_portapapeles");
            treeViewConfiguracion.Nodes[2].Text = Kernel.GetText("cambiar_historial");
            treeViewConfiguracion.Nodes[3].Text = Kernel.GetText("cambiar_stream");
            treeViewConfiguracion.Nodes[4].Text = Kernel.GetText("cambiarTwitter");
            treeViewConfiguracion.Nodes[5].Text = Kernel.GetText("configView");
            treeViewConfiguracion.Nodes[5].Nodes[0].Text = Kernel.GetText("tipografíaLyrics");
            treeViewConfiguracion.Nodes[5].Nodes[1].Text = Kernel.GetText("colorsHighlight");
        }
        private void LoadLanguageConfig()
        {
            ActiveConfig = ActiveConfig.Language;
            radioButtonsIdiomas = new RadioButton[Kernel.NumLanguages];
            PictureBox[] pictureBoxesIdiomas = new PictureBox[Kernel.NumLanguages];
            groupBoxRaiz.Text = Kernel.GetText("cambiar_idioma");
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
            ActiveConfig = ActiveConfig.Clipboard;
            labelStringConfigPreview = new Label();
            labelStringConfigHelp = new Label();
            groupBoxRaiz.Text = Kernel.GetText("cambiar_portapapeles");
            stringConfig = new TextBox();
            stringConfig.TextChanged += StringConfig_TextChanged;
            stringConfig.Location = new Point(35, groupBoxRaiz.Height / 4);
            Size size = stringConfig.Size; size.Width = 420; stringConfig.Size = size;
            stringConfig.Font = new Font("Segoe UI", 9);
            stringConfig.Text = Config.Clipboard;
            labelStringConfigPreview.Location = new Point(stringConfig.Location.X, stringConfig.Location.Y + 30);
            labelStringConfigPreview.AutoSize = true;
            labelStringConfigPreview.Font = stringConfig.Font;
            labelStringConfigHelp.Font = stringConfig.Font;
            labelStringConfigHelp.AutoSize = true;
            labelStringConfigHelp.Location = new Point(labelStringConfigPreview.Location.X, labelStringConfigPreview.Location.Y + 50);
            SetLabelHelp(ActiveConfig);

            string Preview = "";
            AlbumData album = Utils.GetRandomAlbum();
            if (album is null)
                Preview = Config.Clipboard.Replace("%artist%", AlbumCopyPreview.Artist);
            else
                AlbumCopyPreview = album;
            stringConfig.Text = Config.Clipboard;
            StringConfig_TextChanged(null, null);
            groupBoxRaiz.Controls.Add(stringConfig);
            groupBoxRaiz.Controls.Add(labelStringConfigPreview);
            groupBoxRaiz.Controls.Add(labelStringConfigHelp);
            SetLeftAnchor();
        }

        private void LoadHistorialConfig()
        {
            ActiveConfig = ActiveConfig.History;
            labelStringConfigPreview = new Label();
            labelStringConfigHelp = new Label();
            groupBoxRaiz.Text = Kernel.GetText("cambiar_stream");
            stringConfig = new TextBox();
            stringConfig.TextChanged += StringConfig_TextChanged;
            stringConfig.Location = new Point(35, groupBoxRaiz.Height / 4);

            checkBoxHistoryConfigCheckBox = new CheckBox();
            checkBoxHistoryConfigCheckBox.Click += checkBoxHistorialConfig_Click;
            checkBoxHistoryConfigCheckBox.Location = new Point(44, stringConfig.Location.Y - 25);
            checkBoxHistoryConfigCheckBox.Checked = Config.HistoryEnabled;
            checkBoxHistoryConfigCheckBox.Text = Kernel.GetText("enable_historial");
            checkBoxHistoryConfigCheckBox.AutoSize = true;
            checkBoxHistoryConfigCheckBox.Font = new Font("Segoe UI", 9);

            Size size = stringConfig.Size;
            size.Width = 420; stringConfig.Size = size;
            stringConfig.Font = new Font("Segoe UI", 9);
            stringConfig.Text = Config.History;
            labelStringConfigPreview.Location = new Point(stringConfig.Location.X, stringConfig.Location.Y + 30);
            labelStringConfigPreview.AutoSize = true;
            labelStringConfigPreview.Font = stringConfig.Font;
            labelStringConfigHelp.Font = stringConfig.Font;
            labelStringConfigHelp.AutoSize = true;
            labelStringConfigHelp.Location = new Point(labelStringConfigPreview.Location.X, labelStringConfigPreview.Location.Y + 50);
            SetLabelHelp(ActiveConfig);

            string Preview = Config.History;

            AlbumData album = Utils.GetRandomAlbum();
            if (album is not null)
                SongPreview = Utils.GetRandomSong(album);

            StringConfig_TextChanged(null, null);
            groupBoxRaiz.Controls.Add(stringConfig);
            groupBoxRaiz.Controls.Add(labelStringConfigPreview);
            groupBoxRaiz.Controls.Add(labelStringConfigHelp);

            groupBoxRaiz.Controls.Add(checkBoxHistoryConfigCheckBox);
            SetLeftAnchor();

        }

        private void LoadStreamConfig()
        {
            ActiveConfig = ActiveConfig.Stream;

            labelStringConfigPreview = new Label();
            labelStringConfigPreview.AutoSize = true;

            groupBoxRaiz.Text = Kernel.GetText("cambiar_stream");
            stringConfig = new TextBox();
            stringConfig.TextChanged += StringConfig_TextChanged;
            stringConfig.Location = new Point(35, groupBoxRaiz.Height / 4);
            Size size = stringConfig.Size;
            size.Width = 420; stringConfig.Size = size;
            stringConfig.Font = new Font("Segoe UI", 9);
            stringConfig.Text = Config.StreamString;


            labelStringConfigPreview.Font = stringConfig.Font;
            labelStringConfigPreview.Location = new Point(stringConfig.Location.X, stringConfig.Location.Y + 30);

            streamEnabledConfigCheckBox = new CheckBox();
            streamEnabledConfigCheckBox.Location = new Point(44, stringConfig.Location.Y - 25);
            streamEnabledConfigCheckBox.Checked = Config.StreamEnabled;
            streamEnabledConfigCheckBox.Text = Kernel.GetText("enable_stream_log");
            streamEnabledConfigCheckBox.AutoSize = true;
            streamEnabledConfigCheckBox.Font = new Font("Segoe UI", 9);

            labelStringConfigHelp = new Label();
            labelStringConfigHelp.Font = stringConfig.Font;
            labelStringConfigHelp.AutoSize = true;
            labelStringConfigHelp.Location = new Point(labelStringConfigPreview.Location.X, labelStringConfigPreview.Location.Y + 50);
            SetLabelHelp(ActiveConfig);

            string Preview = Config.History;

            AlbumData album = Utils.GetRandomAlbum();
            if (album is not null)
                SongPreview = Utils.GetRandomSong(album);

            StringConfig_TextChanged(null, null);

            groupBoxRaiz.Controls.Add(stringConfig);
            groupBoxRaiz.Controls.Add(labelStringConfigPreview);
            groupBoxRaiz.Controls.Add(labelStringConfigHelp);
            groupBoxRaiz.Controls.Add(streamEnabledConfigCheckBox);
            SetLeftAnchor();
        }
        private void LoadTwitterConfig()
        {
            ActiveConfig = ActiveConfig.Stream;

            labelStringConfigPreview = new Label();
            labelStringConfigPreview.AutoSize = true;

            groupBoxRaiz.Text = Kernel.GetText("twitter_share");
            stringConfig = new TextBox();
            stringConfig.TextChanged += StringConfig_TextChanged;
            stringConfig.Location = new Point(35, groupBoxRaiz.Height / 4);
            Size size = stringConfig.Size;
            size.Width = 420; stringConfig.Size = size;
            stringConfig.Font = new Font("Segoe UI", 9);
            stringConfig.Text = Config.StreamString;


            labelStringConfigPreview.Font = stringConfig.Font;
            labelStringConfigPreview.Location = new Point(stringConfig.Location.X, stringConfig.Location.Y + 30);

            labelStringConfigHelp = new Label();
            labelStringConfigHelp.Font = stringConfig.Font;
            labelStringConfigHelp.AutoSize = true;
            labelStringConfigHelp.Location = new Point(labelStringConfigPreview.Location.X, labelStringConfigPreview.Location.Y + 50);
            SetLabelHelp(ActiveConfig);

            string Preview = Config.History;

            AlbumData album = Utils.GetRandomAlbum();
            if (album is not null)
                SongPreview = Utils.GetRandomSong(album);

            StringConfig_TextChanged(null, null);

            groupBoxRaiz.Controls.Add(stringConfig);
            groupBoxRaiz.Controls.Add(labelStringConfigPreview);
            groupBoxRaiz.Controls.Add(labelStringConfigHelp);
            groupBoxRaiz.Controls.Add(streamEnabledConfigCheckBox);
            SetLeftAnchor();
        }
        private void LoadColorConfig()
        {
            ActiveConfig = ActiveConfig.Colors;
            groupBoxRaiz.Text = Kernel.GetText("colorsHighlight");
            //Create stuff
            btColorBonus = new Button();
            btColorLongSong = new Button();
            ttbtColorBonus = new ToolTip();
            ttbtColorLongSong = new ToolTip();

            ttbtColorLongSong.SetToolTip(btColorLongSong, Kernel.GetText("helpColorLongSong"));
            ttbtColorBonus.SetToolTip(btColorBonus, Kernel.GetText("helpColorBonus"));
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
            btColorBonus.Text = Kernel.GetText("bonus");

            btColorLongSong.BackColor = Config.ColorLongSong;
            btColorLongSong.Text = Kernel.GetText("longSong");

            //Config internal tags
            btColorBonus.Tag = "bonus";
            btColorLongSong.Tag = "longsong";

            btColorBonus.AutoSize = false;
            btColorLongSong.AutoSize = false;
            btColorBonus.Anchor = AnchorStyles.None;
            btColorLongSong.Anchor = AnchorStyles.None;

            groupBoxRaiz.Controls.Add(btColorBonus);
            groupBoxRaiz.Controls.Add(btColorLongSong);
        }

        private void LoadTextConfig()
        {
            groupBoxRaiz.Text = Kernel.GetText("tipografíaLyrics");
            ActiveConfig = ActiveConfig.TextFont;
            //Create stuff
            btTextLyrics = new Button();
            btTextView = new Button();
            ttbtTextLyrics = new ToolTip();
            ttbtTextView = new ToolTip();

            ttbtTextView.SetToolTip(btTextView, Kernel.GetText("helpView"));
            ttbtTextLyrics.SetToolTip(btTextLyrics, Kernel.GetText("helpLyrics"));
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

            btTextLyrics.Text = Kernel.GetText("lyrics");

            btTextView.Text = Kernel.GetText("configView");

            //Config internal tags
            btTextLyrics.Tag = "lyrics";
            btTextView.Tag = "view";
            btTextLyrics.Anchor = AnchorStyles.None;
            btTextView.Anchor = AnchorStyles.None;

            groupBoxRaiz.Controls.Add(btTextLyrics);
            groupBoxRaiz.Controls.Add(btTextView);
        }
        private void SetLabelHelp(ActiveConfig config)
        {
            switch (config)
            {
                case ActiveConfig.Language:
                    break;
                case ActiveConfig.Clipboard:
                    labelStringConfigHelp.Text = Kernel.GetText("label_clipboard");
                    break;
                case ActiveConfig.History:
                    labelStringConfigHelp.Text = Kernel.GetText("label_historial");
                    break;
                case ActiveConfig.Stream:
                    labelStringConfigHelp.Text = Kernel.GetText("label_stream");
                    break;
                case ActiveConfig.Twitter:
                    labelStringConfigHelp.Text = Kernel.GetText("label_twitter");
                    break;
                default:
                    break;
            }
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
                    groupBoxRaiz.Text = Kernel.GetText("cambiar_idioma");
                    break;
                case ActiveConfig.Clipboard:
                    Config.Clipboard = stringConfig.Text;
                    break;
                case ActiveConfig.History:
                    Config.History = stringConfig.Text;
                    Config.HistoryEnabled = checkBoxHistoryConfigCheckBox.Checked;
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
                case ActiveConfig.Stream:
                    Config.StreamEnabled = streamEnabledConfigCheckBox.Checked;
                    Config.StreamString = stringConfig.Text;
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
                case "historial":
                    LoadHistorialConfig();
                    break;
                case "stream":
                    LoadStreamConfig();
                    break;
                case "twitter":
                    LoadTwitterConfig();
                    break;
                default:
                    groupBoxRaiz.Controls.Add(labelSelect);
                    labelSelect.Show();
                    groupBoxRaiz.Text = "";
                    break;
            }
        }
        private void SetLeftAnchor()
        {
            foreach (Control item in groupBoxRaiz.Controls)
            {
                item.Anchor = AnchorStyles.Left;
            }
        }
        private void StringConfig_TextChanged(object sender, EventArgs e)
        {
            string Preview = stringConfig.Text;
            int num = random.Next(1, 101);
            try
            {
                switch (ActiveConfig)
                {
                    case ActiveConfig.Clipboard:
                        Preview = Preview.Replace("%artist%", AlbumCopyPreview.Artist);
                        Preview = Preview.Replace("%title%", AlbumCopyPreview.Title);
                        Preview = Preview.Replace("%year%", AlbumCopyPreview.Year.ToString());
                        Preview = Preview.Replace("%genre%", AlbumCopyPreview.Genre.Name);
                        Preview = Preview.Replace("%length%", AlbumCopyPreview.Length.ToString());
                        Preview = Preview.Replace("%length_seconds%", ((int)AlbumCopyPreview.Length.TotalSeconds).ToString());
                        Preview = Preview.Replace("%length_min%", AlbumCopyPreview.Length.TotalMinutes.ToString());
                        Preview = Preview.Replace("%totaltracks%", AlbumCopyPreview.NumberOfSongs.ToString());
                        Preview = Preview.Replace("%path%", AlbumCopyPreview.SoundFilesPath);
                        labelStringConfigPreview.Text = Preview;
                        break;
                    case ActiveConfig.History:
                        Preview = Preview.Replace("%track_num%", num.ToString());
                        Preview = Preview.Replace("%artist%", SongPreview.AlbumFrom.Artist);
                        Preview = Preview.Replace("%title%", SongPreview.Title);
                        Preview = Preview.Replace("%length%", SongPreview.Length.ToString());
                        Preview = Preview.Replace("%date%", DateTime.Now.Date.ToString("d"));
                        Preview = Preview.Replace("%time%", DateTime.Now.ToString("HH:mm"));
                        break;
                    case ActiveConfig.Stream:
                        Preview = Preview.Replace("%track_num%", num.ToString());
                        Preview = Preview.Replace("%artist%", SongPreview.AlbumFrom.Artist);
                        Preview = Preview.Replace("%album%", SongPreview.AlbumFrom.Title);
                        Preview = Preview.Replace("%title%", SongPreview.Title);
                        Preview = Preview.Replace("%length%", SongPreview.Length.ToString(@"mm\:ss"));
                        Preview = Preview.Replace("%pos%", TimeSpan.FromSeconds(random.Next((int)SongPreview.Length.TotalSeconds)).ToString(@"mm\:ss"));
                        Preview = Preview.Replace("%date%", DateTime.Now.Date.ToString("d"));
                        Preview = Preview.Replace("%time%", DateTime.Now.ToString("HH:mm"));
                        Preview = Preview.Replace("%year%", SongPreview.AlbumFrom.Year.ToString());
                        Preview = Preview.Replace("\\n", Environment.NewLine);
                        break;
                    case ActiveConfig.Twitter:
                        Preview = Preview.Replace("%track_num%", num.ToString());
                        Preview = Preview.Replace("%artist%", SongPreview.AlbumFrom.Artist);
                        Preview = Preview.Replace("%album%", SongPreview.AlbumFrom.Title);
                        Preview = Preview.Replace("%title%", SongPreview.Title);
                        Preview = Preview.Replace("%length%", SongPreview.Length.ToString(@"mm\:ss"));
                        Preview = Preview.Replace("%pos%", TimeSpan.FromSeconds(random.Next((int)SongPreview.Length.TotalSeconds)).ToString(@"mm\:ss"));
                        Preview = Preview.Replace("%date%", DateTime.Now.Date.ToString("d"));
                        Preview = Preview.Replace("%time%", DateTime.Now.ToString("HH:mm"));
                        Preview = Preview.Replace("%year%", SongPreview.AlbumFrom.Year.ToString());
                        Preview = Preview.Replace("%link%", "https://open.spotify.com/track/3elIkBzbWu7K5SSAJz5q5x");
                        Preview = Preview.Replace("\\n", Environment.NewLine);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {
                labelStringConfigPreview.Text = Preview;
            }
            finally
            {
                labelStringConfigPreview.Text = Preview;
            }
        }
        //Events
        private void treeViewConfiguracion_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            CargarPagina(e.Node.Tag.ToString());
        }

        private void buttonAplicar_Click(object sender, EventArgs e)
        {
            Aplicar(ActiveConfig);
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Aplicar(ActiveConfig);
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

        private void checkBoxHistorialConfig_Click(object sender, EventArgs e)
        {

        }
        private void treeViewConfiguracion_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void groupBoxRaiz_Resize(object sender, EventArgs e)
        {
            labelSelect.Location = new Point(groupBoxRaiz.Size.Width / 2 - (labelSelect.Size.Width / 2), groupBoxRaiz.Size.Height / 2);
        }

        private void ConfigForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                CargarPagina(treeViewConfiguracion.SelectedNode.Tag.ToString());
        }

        private void treeViewConfiguracion_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                CargarPagina(treeViewConfiguracion.SelectedNode.Tag.ToString());
        }
    }
}
