using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace aplicacion_musica
{
    public partial class busquedaSpotify : Form
    {
        public busquedaSpotify()
        {
            InitializeComponent();
            Text = Programa.textosLocal[37];
            labelBusqueda.Text = Programa.textosLocal[38];
            buscarButton.Text = Programa.textosLocal[25];
            labelAlternativa.Text = Programa.textosLocal[47] + " (spotify:album:7pgQk5VJbjTzIKsU8fheig)";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBoxURISpotify.Text is "" && textBox1.Text != "")
                {
                    textBoxURISpotify.Text = "";
                    principal.BusquedaSpotify = textBox1.Text;
                    DialogResult = DialogResult.No;
                    Dispose();
                }
                else if (textBox1.Text is "" && textBoxURISpotify.Text is "")
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
                MessageBox.Show(Programa.textosLocal[48] + " (spotify:album:7pgQk5VJbjTzIKsU8fheig)");
            }
            catch(ArgumentException)
            {
                MessageBox.Show(Programa.textosLocal[48] + " (spotify:album:7pgQk5VJbjTzIKsU8fheig)");

            }
        }
    }
}
