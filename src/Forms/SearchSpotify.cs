using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Cassiopeia.src.Forms
{
    public partial class SearchSpotify : Form
    {
        private enum SearchType
        {
            Text,
            URI,
            Unknown
        }
        public SearchSpotify()
        {
            InitializeComponent();
            Text = Kernel.LocalTexts.GetString("buscar_Spotify");
            labelBusqueda.Text = Kernel.LocalTexts.GetString("busqueda_Spotify");
            buttonOk.Text = Kernel.LocalTexts.GetString("buscar");
            labelAlternativa.Text = Kernel.LocalTexts.GetString("introduce_uri") + " (spotify:album:7pgQk5VJbjTzIKsU8fheig)";
            Icon = Properties.Resources.spotifyico;
        }
        private SearchType CheckQuery()
        {
            //Check which textbox has text.
            if (!string.IsNullOrEmpty(textBoxBusqueda.Text))
                return SearchType.Text;
            if (!string.IsNullOrEmpty(textBoxURISpotify.Text))
                return SearchType.URI;
            return SearchType.Unknown;
        }
        private void ProcessURI(string URI)
        {
            String[] uri = URI.Split(':');
            if (uri[1] != "album")
                throw new ArgumentException();
            Kernel.Spotify.InsertAlbumFromURI(uri[2]);
            Close();
            Dispose();
        }
        private DialogResult SearchAlbum(string query) //Invokes the form for the results. DialogResult determines if the user has completed the action.
        {
            try
            {
                List<SpotifyAPI.Web.SimpleAlbum> AlbumList = Kernel.Spotify.SearchAlbums(query, 20);
                SpotifyResults res = new SpotifyResults(ref AlbumList, false);
                res.ShowDialog();
                if (res.DialogResult == DialogResult.Cancel)
                    return DialogResult.Cancel;
                else return DialogResult.OK;

            }
            catch (SpotifyAPI.Web.APIException ex)
            {
                Log.Instance.PrintMessage(ex.Message, MessageType.Warning);
                return DialogResult.Cancel;
            }

        }
        private void buttonOk_Click(object sender, EventArgs e)
        {
            SearchType searchType = CheckQuery();
            switch (searchType)
            {
                case SearchType.Text:
                    DialogResult dialogResult = SearchAlbum(textBoxBusqueda.Text);
                    switch (dialogResult)
                    {
                        case DialogResult.OK:
                            Dispose();
                            break;
                        case DialogResult.Cancel: //User has cancelled the query.
                            break;
                        default:
                            break;
                    }
                    break;
                case SearchType.URI:
                    try
                    {
                        ProcessURI(textBoxURISpotify.Text);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show(Kernel.LocalTexts.GetString("error_uri") + " (spotify:album:2dpdDyEGEsdnOUUePgkT6E)");
                    }
                    break;
                case SearchType.Unknown:
                    MessageBox.Show(Kernel.LocalTexts.GetString("error_vacio2"), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                default:
                    break;
            }
        }
    }
}
