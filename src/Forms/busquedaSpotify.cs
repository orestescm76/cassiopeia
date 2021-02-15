using System;
using System.Windows.Forms;

namespace aplicacion_musica
{
    public partial class busquedaSpotify : Form
    {
        public busquedaSpotify()
        {
            InitializeComponent();
            Text = Program.LocalTexts.GetString("buscar_Spotify");
            labelBusqueda.Text = Program.LocalTexts.GetString("busqueda_Spotify");
            buscarButton.Text = Program.LocalTexts.GetString("buscar");
            labelAlternativa.Text = Program.LocalTexts.GetString("introduce_uri") + " (spotify:album:7pgQk5VJbjTzIKsU8fheig)";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBoxURISpotify.Text == "" && textBoxBusqueda.Text != "")
                {
                    principal.BusquedaSpotify = textBoxBusqueda.Text;
                    DialogResult = DialogResult.No;
                    Dispose();
                }
                else if (textBoxBusqueda.Text == "" && textBoxURISpotify.Text == "")
                {
                    throw new NullReferenceException();
                }
                else
                {
                    String[] uri = textBoxURISpotify.Text.Split(':');
                    if (uri[1] != "album")
                        throw new ArgumentException();
                    Program._spotify.InsertarAlbumFromURI(uri[2]);
                    DialogResult = DialogResult.Yes;
                    Dispose();
                }
            }
            catch (IndexOutOfRangeException)
            {
                MessageBox.Show(Program.LocalTexts.GetString("error_uri") + " (spotify:album:7pgQk5VJbjTzIKsU8fheig)");
            }
            catch(ArgumentException)
            {
                MessageBox.Show(Program.LocalTexts.GetString("error_uri") + " (spotify:album:7pgQk5VJbjTzIKsU8fheig)");
            }
        }
    }
}
