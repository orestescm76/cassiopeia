using System;
using System.Windows.Forms;

namespace aplicacion_musica
{
    public partial class CrearCD : Form
    {
        private AlbumData album;
        private CompactDisc cd;
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
        public CrearCD(ref CompactDisc cdd, short n, bool edit = false)
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
                numericUpDownNumCanciones.Maximum = album.NumberOfSongs - cdd.Discos[0].NumberOfSongs;
                numericUpDownNumCanciones.Value = numericUpDownNumCanciones.Maximum;
                Log.Instance.PrintMessage("Creando otro CD con un máximo de "+numericUpDownNumCanciones.Maximum, MessageType.Info);
            }
            else if(edit)
            {
                Log.Instance.PrintMessage("Editando CD", MessageType.Info);
                this.edit = true;
                comboBoxFormatoCD.SelectedItem = cdd.SleeveType;
                comboBoxEstadoMedio.SelectedItem = cdd.Discos[n-1].MediaCondition;
                comboBoxEstadoExterior.SelectedItem = cdd.EstadoExterior;
                numericUpDownNumCanciones.Value = cdd.Discos[n-1].NumberOfSongs;
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
                eeT[i] = Program.LocalTexts.GetString(Enum.GetName(typeof(MediaCondition), i));
            for (int i = 0; i < fT.Length; i++)
                fT[i] = Program.LocalTexts.GetString(Enum.GetName(typeof(SleeveType), i));
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

            MediaCondition exterior = (MediaCondition)Enum.Parse(typeof(MediaCondition), comboBoxEstadoExterior.SelectedIndex.ToString());
            MediaCondition medio = (MediaCondition)Enum.Parse(typeof(MediaCondition), comboBoxEstadoMedio.SelectedIndex.ToString());
            SleeveType formato = (SleeveType)Enum.Parse(typeof(SleeveType), comboBoxFormatoCD.SelectedIndex.ToString());
            string s = album.Artist + "_" + album.Title;
            if(edit)
            {
                cd.SleeveType = formato;
                cd.Discos[NDisco - 1].MediaCondition = medio;
                cd.EstadoExterior = exterior;
                cd.Discos[NDisco - 1].NumberOfSongs=(short)numericUpDownNumCanciones.Value;
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
                    Disc nuevo = new Disc(Convert.ToInt16(numericUpDownNumCanciones.Value), medio);
                    cd.Discos[NDisco-1] = nuevo;
                }
                else
                {
                    CompactDisc cd = new CompactDisc(s, Convert.ToInt16(numericUpDownNumCanciones.Value), medio, exterior, formato, numDiscos);
                    Program.Collection.AddCD(ref cd);
                }
            }
            else
            {
                CompactDisc cd = null;
                try
                {
                     cd = new CompactDisc(s, album.NumberOfSongs, medio, exterior, formato, Convert.ToInt16(textBoxAño.Text), textBoxPais.Text);

                }
                catch (Exception)
                {
                     cd = new CompactDisc(s, album.NumberOfSongs, medio, exterior, formato, 0, textBoxPais.Text);
                }
                Program.Collection.AddCD(ref cd);
                visualizarAlbum v = new visualizarAlbum(ref cd);
                v.Show();
            }
            Dispose();
        }
    }
}
