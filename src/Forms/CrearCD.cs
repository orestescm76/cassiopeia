using System;
using System.Windows.Forms;

namespace aplicacion_musica
{
    public partial class CrearCD : Form
    {
        private AlbumData album;
        private DiscoCompacto cd;
        private short numDiscos;
        private short NDisco;
        private int NC;
        private bool edit = false;
        public CrearCD(ref AlbumData a)
        {
            InitializeComponent();
            album = a;
            Console.WriteLine("Creando sólo un CD, duración: " + album.Length);
            numericUpDownNumCanciones.Hide();
            labelNumCanciones.Hide();
            NDisco = 1;
            NC = a.NumberOfSongs;
            PonerTextos();
        }
        public CrearCD(ref AlbumData a, short nd)
        {
            InitializeComponent();
            Console.WriteLine("Creando primer CD");
            album = a;
            NDisco = 1;
            numDiscos = nd;
            PonerTextos();
        }
        public CrearCD(ref DiscoCompacto cdd, short n, bool edit = false)
        {
            InitializeComponent();
            NDisco = n;
            album = cdd.Album;
            cd = cdd;
            if(n > 1 && !edit)
            {
                labelFormato.Hide();
                comboBoxFormatoCD.Hide();
                labelAñoPublicacion.Hide();
                comboBoxEstadoExterior.Hide();
                labelEstadoExterior.Hide();
                labelAñoPublicacion.Hide();
                labelPaisPublicacion.Hide();
                textBoxPais.Hide();
                textBoxAño.Hide();
                numericUpDownNumCanciones.Maximum = album.NumberOfSongs - cdd.Discos[0].NumCanciones;
                numericUpDownNumCanciones.Value = numericUpDownNumCanciones.Maximum;
                Log.Instance.ImprimirMensaje("Creando otro CD con un máximo de "+numericUpDownNumCanciones.Maximum, TipoMensaje.Info);
            }
            else if(edit)
            {
                Log.Instance.ImprimirMensaje("Editando CD", TipoMensaje.Info);
                this.edit = true;
                comboBoxFormatoCD.SelectedItem = cdd.FormatoCD;
                comboBoxEstadoMedio.SelectedItem = cdd.Discos[n-1].EstadoDisco;
                comboBoxEstadoExterior.SelectedItem = cdd.EstadoExterior;
                numericUpDownNumCanciones.Value = cdd.Discos[n-1].NumCanciones;
                textBoxAño.Text = cd.Year.ToString();
                textBoxPais.Text = cd.Country;
            }
            PonerTextos();
        }
        private void PonerTextos()
        {
            labelEstadoExterior.Text = Program.LocalTexts.GetString("estado_exterior");
            labelEstadoMedio.Text = Program.LocalTexts.GetString("estado_medio");
            labelFormato.Text = Program.LocalTexts.GetString("formato");
            labelAñoPublicacion.Text = Program.LocalTexts.GetString("añoPublicacion");
            labelPaisPublicacion.Text = Program.LocalTexts.GetString("paisPublicacion");
            labelNumCanciones.Text = Program.LocalTexts.GetString("numcanciones");
            String[] eeT = new string[7];
            String[] fT = new string[4];
            for (int i = 0; i < eeT.Length; i++)
                eeT[i] = Program.LocalTexts.GetString(Enum.GetName(typeof(EstadoMedio), i));
            for (int i = 0; i < fT.Length; i++)
                fT[i] = Program.LocalTexts.GetString(Enum.GetName(typeof(FormatoCD), i));
            comboBoxEstadoMedio.Items.AddRange(eeT);
            comboBoxEstadoExterior.Items.AddRange(eeT);
            comboBoxFormatoCD.Items.AddRange(fT);
            comboBoxEstadoMedio.SelectedIndex = 0;
            comboBoxEstadoExterior.SelectedIndex = 0;
            comboBoxFormatoCD.SelectedIndex = 0;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            album.CanBeRemoved = false;

            EstadoMedio exterior = (EstadoMedio)Enum.Parse(typeof(EstadoMedio), comboBoxEstadoExterior.SelectedIndex.ToString());
            EstadoMedio medio = (EstadoMedio)Enum.Parse(typeof(EstadoMedio), comboBoxEstadoMedio.SelectedIndex.ToString());
            FormatoCD formato = (FormatoCD)Enum.Parse(typeof(FormatoCD), comboBoxFormatoCD.SelectedIndex.ToString());
            string s = album.Artist + "_" + album.Title;
            if(edit)
            {
                cd.FormatoCD = formato;
                cd.Discos[NDisco - 1].EstadoDisco = medio;
                cd.EstadoExterior = exterior;
                cd.Discos[NDisco - 1].NumCanciones=(short)numericUpDownNumCanciones.Value;
                cd.Year = Convert.ToInt16(textBoxAño.Text);
                cd.Country = textBoxPais.Text;
                visualizarAlbum nuevo = new visualizarAlbum(ref cd);
                Program.ReloadView();
                nuevo.Show();
            }
            else if (NC != album.NumberOfSongs)
            {
                if(NDisco > 1)
                {
                    Disco nuevo = new Disco(Convert.ToInt16(numericUpDownNumCanciones.Value), medio);
                    cd.Discos[NDisco-1] = nuevo;
                }
                else
                {
                    DiscoCompacto cd = new DiscoCompacto(s, Convert.ToInt16(numericUpDownNumCanciones.Value), medio, exterior, formato, numDiscos);
                    Program.Collection.AddCD(ref cd);
                }
            }
            else
            {
                DiscoCompacto cd = null;
                try
                {
                     cd = new DiscoCompacto(s, album.NumberOfSongs, medio, exterior, formato, Convert.ToInt16(textBoxAño.Text), textBoxPais.Text);

                }
                catch (Exception)
                {
                     cd = new DiscoCompacto(s, album.NumberOfSongs, medio, exterior, formato, 0, textBoxPais.Text);
                }
                Program.Collection.AddCD(ref cd);
                visualizarAlbum v = new visualizarAlbum(ref cd);
                v.Show();
            }
            Dispose();
        }
    }
}
