using System;
using System.Windows.Forms;
using System.Diagnostics;
using Cassiopeia.src.Classes;

namespace Cassiopeia.src.Forms
{
    public partial class CreateAlbum : Form
    {
        private string caratula = "";
        private String[] genresToSelect = new string[Kernel.Genres.Length-1];
        public CreateAlbum()
        {
            InitializeComponent();
            ponerTextos();
            Log.Instance.PrintMessage("Creating album manually", MessageType.Info);
        }
        private void ponerTextos()
        {
            Text = Kernel.LocalTexts.GetString("agregar_album");
            labelArtista.Text = Kernel.LocalTexts.GetString("artista");
            labelTitulo.Text = Kernel.LocalTexts.GetString("titulo");
            labelAño.Text = Kernel.LocalTexts.GetString("año");
            labelNumCanciones.Text = Kernel.LocalTexts.GetString("numcanciones");
            labelGenero.Text = Kernel.LocalTexts.GetString("genero");
            buttonAdd.Text = Kernel.LocalTexts.GetString("añadir");
            addCaratula.Text = Kernel.LocalTexts.GetString("addcaratula");
            labelCaratula.Text = Kernel.LocalTexts.GetString("caratula");
            labelAlbumType.Text = Kernel.LocalTexts.GetString("tipoAlbum");
            for (int i = 0; i < Kernel.Genres.Length-1; i++)
            {
                genresToSelect[i] = Kernel.Genres[i].Name;
            }
            Array.Sort(genresToSelect);
            comboBoxGenres.Items.AddRange(genresToSelect);

            string[] types = new string[5];
            types[(int)AlbumType.Studio] = Kernel.LocalTexts.GetString("estudio");
            types[(int)AlbumType.Live] = Kernel.LocalTexts.GetString("live");
            types[(int)AlbumType.Compilation] = Kernel.LocalTexts.GetString("compilacion");
            types[(int)AlbumType.EP] = Kernel.LocalTexts.GetString("EP");
            types[(int)AlbumType.Single] = Kernel.LocalTexts.GetString("sencillo");
            comboBoxAlbumType.Items.AddRange(types);
            comboBoxAlbumType.SelectedIndex = 0;
        }
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            Log.Instance.PrintMessage("Looking for album cover", MessageType.Info);
            OpenFileDialog abrirImagen = new OpenFileDialog();
            abrirImagen.Filter = Kernel.LocalTexts.GetString("archivo") + " .jpg, .png|*.jpg;*.png;*.jpeg";
            abrirImagen.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            if (abrirImagen.ShowDialog() == DialogResult.OK)
            {
                string fichero = abrirImagen.FileName;
                caratula = fichero;
                ruta.Text = fichero;
            }
            Log.Instance.PrintMessage("Image " + ruta + " loaded", MessageType.Correct);
        }

        private void add_Click(object sender, EventArgs e)
        {

        }
    }
}
