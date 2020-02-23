using System;
using System.Globalization;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace aplicacion_musica
{
    /// <summary>
    /// Formulario responsable de la creacion del cd
    /// </summary>
    public partial class CrearCD : Form
    {
        private Album album;
        private DiscoCompacto cd;
        private short numDiscos;
        private short NDisco;
        private short NC;
        /// <summary>
        /// Constructor para crear un sólo CD, para álbumes con menos de 80 minutos de duración
        /// </summary>
        /// <param name="a"></param>
        public CrearCD(ref Album a)
        {
            InitializeComponent();
            album = a;
            numericUpDownNumCanciones.Hide();
            labelNumCanciones.Hide();
            NDisco = 1;
            NC = a.numCanciones;
            PonerTextos();
        }
        public CrearCD(ref Album a, short nd)
        {
            InitializeComponent();
            album = a;
            NDisco = 1;
            numDiscos = nd;
            PonerTextos();
        }
        /// <summary>
        /// Constructor para crear un segundo, tercer CD
        /// </summary>
        /// <param name="a">Álbun al que pertenece</param>
        /// <param name="nc">Número de canciones del segundo CD</param>
        /// <param name="n">Número del CD, segundo...</param>
        public CrearCD(ref DiscoCompacto cdd, short n)
        {
            InitializeComponent();
            NDisco = n;
            album = cdd.Album;
            cd = cdd;
            if(n > 1)
            {
                labelFormato.Hide();
                comboBoxFormatoCD.Hide();
                labelAñoPublicacion.Hide();
                comboBoxEstadoExterior.Hide();
                labelEstadoExterior.Hide();
                labelAñoPublicacion.Hide();
                labelPaisPublicacion.Hide();
                textBox1.Hide();
                textBox2.Hide();
            }
            PonerTextos();
        }
        private void PonerTextos()
        {
            labelEstadoExterior.Text = Programa.textosLocal.GetString("estado_exterior");
            labelEstadoMedio.Text = Programa.textosLocal.GetString("estado_medio");
            labelFormato.Text = Programa.textosLocal.GetString("formato");
            labelAñoPublicacion.Text = Programa.textosLocal.GetString("añoPublicacion");
            labelPaisPublicacion.Text = Programa.textosLocal.GetString("paisPublicacion");
            String[] eeT = new string[7];
            String[] fT = new string[4];
            for (int i = 0; i < eeT.Length; i++)
                eeT[i] = Programa.textosLocal.GetString(Enum.GetName(typeof(EstadoMedio), i));
            for (int i = 0; i < fT.Length; i++)
                fT[i] = Programa.textosLocal.GetString(Enum.GetName(typeof(FormatoCD), i));
            comboBoxEstadoMedio.Items.AddRange(eeT);
            comboBoxEstadoExterior.Items.AddRange(eeT);
            comboBoxFormatoCD.Items.AddRange(fT);
            comboBoxEstadoMedio.SelectedIndex = 0;
            comboBoxEstadoExterior.SelectedIndex = 0;
            comboBoxFormatoCD.SelectedIndex = 0;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            EstadoMedio exterior = (EstadoMedio)Enum.Parse(typeof(EstadoMedio), comboBoxEstadoExterior.SelectedIndex.ToString());
            EstadoMedio medio = (EstadoMedio)Enum.Parse(typeof(EstadoMedio), comboBoxEstadoMedio.SelectedIndex.ToString());
            FormatoCD formato = (FormatoCD)Enum.Parse(typeof(FormatoCD), comboBoxFormatoCD.SelectedIndex.ToString());
            string s = album.artista + "_" + album.nombre;
            if (NC != album.numCanciones)
            {
                if(NDisco > 1)
                {
                    Disco nuevo = new Disco(Convert.ToInt16(numericUpDownNumCanciones.Value), medio);
                    cd.Discos[NDisco-1] = nuevo;
                }
                else
                {
                    DiscoCompacto cd = new DiscoCompacto(s, Convert.ToInt16(numericUpDownNumCanciones.Value), medio, exterior, formato, numDiscos);
                    Programa.miColeccion.AgregarCD(ref cd);
                }
            }
            else
            {
                DiscoCompacto cd = new DiscoCompacto(s, album.numCanciones, medio, exterior, formato);
                Programa.miColeccion.AgregarCD(ref cd);
                visualizarAlbum v = new visualizarAlbum(ref cd);
                v.Show();
            }
            Dispose();
        }
    }
}
