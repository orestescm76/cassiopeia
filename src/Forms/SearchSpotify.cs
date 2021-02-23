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
            Text = Program.LocalTexts.GetString("buscar_Spotify");
            labelBusqueda.Text = Program.LocalTexts.GetString("busqueda_Spotify");
            buttonOk.Text = Program.LocalTexts.GetString("buscar");
            labelAlternativa.Text = Program.LocalTexts.GetString("introduce_uri") + " (spotify:album:7pgQk5VJbjTzIKsU8fheig)";
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
            Program._spotify.InsertarAlbumFromURI(uri[2]);
            Dispose();
        }
        private DialogResult SearchAlbum(string query) //Invokes the form for the results. DialogResult determines if the user has completed the action.
        {
            List<SpotifyAPI.Web.Models.SimpleAlbum> AlbumList = Program._spotify.SearchAlbums(query);
            if (!(AlbumList is null))
            {
                SpotifyResults res = new SpotifyResults(ref AlbumList, false);
                res.ShowDialog();
                if (res.DialogResult == DialogResult.Cancel)
                    return DialogResult.Cancel;
                else return DialogResult.OK;
            }
            else return DialogResult.Cancel;
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
                    catch (ArgumentException)
                    {
                        MessageBox.Show(Program.LocalTexts.GetString("error_uri") + " (spotify:album:2dpdDyEGEsdnOUUePgkT6E)");
                    }
                    break;
                case SearchType.Unknown:
                    MessageBox.Show(Program.LocalTexts.GetString("error_vacio2"), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                default:
                    break;
            }
        }
    }
}
