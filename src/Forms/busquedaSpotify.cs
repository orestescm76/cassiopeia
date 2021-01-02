using System;
using System.Windows.Forms;

namespace aplicacion_musica
{
    public partial class busquedaSpotify : Form
    {
        public busquedaSpotify()
        {
            InitializeComponent();
            Text = Programa.textosLocal.GetString("buscar_Spotify");
            labelBusqueda.Text = Programa.textosLocal.GetString("busqueda_Spotify");
            buscarButton.Text = Programa.textosLocal.GetString("buscar");
            labelAlternativa.Text = Programa.textosLocal.GetString("introduce_uri") + " (spotify:album:7pgQk5VJbjTzIKsU8fheig)";
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
                    Programa._spotify.insertarAlbumFromURI(uri[2]);
                    DialogResult = DialogResult.Yes;
                    Dispose();
                }
            }
            catch (IndexOutOfRangeException)
            {
                MessageBox.Show(Programa.textosLocal.GetString("error_uri") + " (spotify:album:7pgQk5VJbjTzIKsU8fheig)");
            }
            catch(ArgumentException)
            {
                MessageBox.Show(Programa.textosLocal.GetString("error_uri") + " (spotify:album:7pgQk5VJbjTzIKsU8fheig)");
            }
        }
    }
}
