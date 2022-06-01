using Cassiopeia.src.Classes;
using HtmlAgilityPack;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Cassiopeia.src.Forms
{
    public partial class LyricsViewer : Form
    {
        private Song Song;
        private ToolTip ConsejoDeshacer;
        private Font Tipografia;
        public LyricsViewer(Song c)
        {
            InitializeComponent();
            Icon = Properties.Resources.letras;
            Tipografia = Config.FontLyrics;
            textBoxLyrics.Font = Tipografia;
            Song = c;
            if (c.Lyrics == null)
                c.Lyrics = new string[0];
            textBoxLyrics.Lines = Song.Lyrics;
            Text = c.ToString() + " (" + Tipografia.Size + ")";
            ConsejoDeshacer = new ToolTip();
            PonerTextos();
            textBoxLyrics.DeselectAll();
            if (Song.AlbumFrom is null)
            {
                buttonBack.Enabled = false;
                buttonNext.Enabled = false;
            }
            buttonBack.Enabled = !(Song.IndexInAlbum == 1);
            buttonNext.Enabled = (Song.IndexInAlbum != Song.AlbumFrom.Songs.Count);
            textBoxLyrics.MouseWheel += new MouseEventHandler(textBoxLyrics_MouseWheel);
        }
        private void CambiarCancion(Song c)
        {
            Song = c;
            Recargar();
        }
        private void Recargar()
        {
            if (Song.Lyrics == null)
                Song.Lyrics = new string[0];
            textBoxLyrics.Lines = Song.Lyrics;
            Text = Song.ToString();
            textBoxLyrics.DeselectAll();
            if (Song.AlbumFrom is null)
            {
                buttonBack.Enabled = false;
                buttonNext.Enabled = false;
            }
            buttonBack.Enabled = !(Song.IndexInAlbum == 1);
            buttonNext.Enabled = (Song.IndexInAlbum != Song.AlbumFrom.Songs.Count);
        }
        private void PonerTextos()
        {
            buttonBuscar.Text = Kernel.LocalTexts.GetString("buscar");
            buttonEditar.Text = Kernel.LocalTexts.GetString("editar");
            buttonCerrar.Text = Kernel.LocalTexts.GetString("cerrar");
            buttonLimpiar.Text = Kernel.LocalTexts.GetString("limpiar");
            buttonDeshacer.Text = Kernel.LocalTexts.GetString("deshacer");
            ConsejoDeshacer.SetToolTip(buttonDeshacer, Kernel.LocalTexts.GetString("consejoDeshacer"));
            buttonBack.Text = Kernel.LocalTexts.GetString("anterior");
            buttonNext.Text = Kernel.LocalTexts.GetString("siguiente");
        }
        private void Guardar()
        {
            Song.Lyrics = textBoxLyrics.Lines;
            Kernel.SetSaveMark();
            Log.Instance.PrintMessage("Lyrics saved!", MessageType.Correct);
        }
        #region Events
        private void buttonEditar_Click(object sender, EventArgs e)
        {
            if (!textBoxLyrics.ReadOnly)
            {
                textBoxLyrics.ReadOnly = true;
                buttonEditar.Text = Kernel.LocalTexts.GetString("editar");
            }
            else
            {
                textBoxLyrics.ReadOnly = false;
                buttonEditar.Text = Kernel.LocalTexts.GetString("aceptar");

            }
        }

        private void buttonCerrar_Click(object sender, EventArgs e)
        {
            Guardar();
            Close();
        }

        private void buttonLimpiar_Click(object sender, EventArgs e)
        {
            textBoxLyrics.Clear();
        }

        private void buttonDeshacer_Click(object sender, EventArgs e)
        {
            textBoxLyrics.Lines = Song.Lyrics;
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            Guardar();
            CambiarCancion(Song.AlbumFrom.GetSong(Song.IndexInAlbum));
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            Guardar();
            CambiarCancion(Song.AlbumFrom.GetSong(Song.IndexInAlbum - 2));
        }
        private void textBoxLyrics_MouseWheel(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                Font tipografiaNew = Tipografia;
                if (e.Delta > 0)
                    tipografiaNew = new Font(Tipografia.FontFamily.Name, Tipografia.Size + 2);
                else
                    tipografiaNew = new Font(Tipografia.FontFamily.Name, Tipografia.Size - 2);
                if (tipografiaNew.Size <= 2 || tipografiaNew.Size >= 75)
                    tipografiaNew = Tipografia; //no se cambia
                textBoxLyrics.Font = Tipografia = tipografiaNew;
                Text = Song.ToString() + " (" + Tipografia.Size + ")";
            }
        }
        private void buttonBuscar_Click(object sender, EventArgs e)
        {
            /*
             * 
             * @lyrics_service
            def _musixmatch(song):
                service_name = "Musixmatch"

                def extract_mxm_props(soup_page):
                    scripts = soup_page.find_all("script")
                    for script in scripts:
                        if script and script.contents and "__mxmProps" in script.contents[0]:
                            return script.contents[0]

                search_url = "https://www.musixmatch.com/search/%s-%s" % (
                    song.artist.replace(' ', '-'), song.name.replace(' ', '-'))
                header = {"User-Agent": "curl/7.9.8 (i686-pc-linux-gnu) libcurl 7.9.8 (OpenSSL 0.9.6b) (ipv6 enabled)"}
                search_results = requests.get(search_url, headers=header, proxies=Config.PROXY)
                soup = BeautifulSoup(search_results.text, 'html.parser')
                props = extract_mxm_props(soup)
                if props:
                    page = re.findall('"track_share_url":"([^"]*)', props)
                    if page:
                        url = codecs.decode(page[0], 'unicode-escape')
                        lyrics_page = requests.get(url, headers=header, proxies=Config.PROXY)
                        soup = BeautifulSoup(lyrics_page.text, 'html.parser')
                        props = extract_mxm_props(soup)
                        if '"body":"' in props:
                            lyrics = props.split('"body":"')[1].split('","language"')[0]
                            lyrics = lyrics.replace("\\n", "\n")
                            lyrics = lyrics.replace("\\", "")
                            album = soup.find(class_="mxm-track-footer__album")
                            if album:
                                song.album = album.find(class_="mui-cell__title").getText()
                            if lyrics.strip():
                                return lyrics, lyrics_page.url, service_name
             */
            /* GENIUS CODE. NOT SO CLEAN
             *  var link = "https://genius.com/%a-%t-lyrics".Replace("%a", Song.AlbumFrom.Artist.Replace(' ', '-')).Replace("%t", Song.Title.ToLower().Replace(' ', '-'));
                HtmlWeb web = new();
                var htmlDoc = web.Load(link);
                var lyrics = htmlDoc.DocumentNode.SelectNodes("//div[@data-lyrics-container='true']");
                Console.WriteLine(lyrics[0].InnerHtml);
                textBoxLyrics.Text = lyrics[0].InnerHtml.Replace("<br>", Environment.NewLine);
            */
            try
            {

                //do we have proper lyrics?
                string lyrics = "";
                if(!Utils.GetLyrics(Song.AlbumFrom.Artist, Song.Title, out lyrics))
                    MessageBox.Show(Kernel.LocalTexts.GetString("lyricsNotAvailable"), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else
                    textBoxLyrics.Text = lyrics;
            }
            catch (Exception ex)
            {
                Console.WriteLine("fuck you, " + ex.Message);
            }
        }
        #endregion

    }
}
