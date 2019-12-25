using System;
using System.Windows.Forms;

namespace aplicacion_ipo
{
    public partial class agregarCancion : Form
    {
        public string t;
        public int min, sec;
        private int cual;
        Album album;
        Cancion cancion;
        bool editar;
        public agregarCancion(ref Album a, int n)
        {
            InitializeComponent();
            album = a;
            cual = n;
            editar = false;
            ponerTextos();
        }
        public agregarCancion(ref Cancion c)
        {
            InitializeComponent();
            cancion = c;
            editar = true;
            tituloTextBox.Text = c.titulo;
            minTextBox.Text = c.duracion.Minutes.ToString();
            secsTextBox.Text = c.duracion.Seconds.ToString();
            ponerTextos();
        }
        private void ponerTextos()
        {
            int cualdeVerdad = cual + 1;

            if(editar)
            {
                Text = Programa.textosLocal[27] + " " + cancion.titulo;
                button1.Text = Programa.textosLocal[21];
            }

            else
            {
                Text = Programa.textosLocal[10] + " " + cualdeVerdad;
                button1.Text = Programa.textosLocal[9];
            }

            button2.Text = Programa.textosLocal[11];
            labelTituloCancion.Text = Programa.textosLocal[12];
            labelMinutosSegundos.Text = Programa.textosLocal[13];

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            try
            {
                min = Convert.ToInt32(minTextBox.Text);
                sec = Convert.ToInt32(secsTextBox.Text);
                t = tituloTextBox.Text;
                if(editar)
                {
                    cancion.titulo = t;
                    cancion.duracion = new TimeSpan(0, min, sec);
                    this.DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    Cancion c = new Cancion(t, new TimeSpan(0, min, sec));
                    album.agregarCancion(c, cual);
                    this.DialogResult = DialogResult.OK;
                    Close();
                }

            }
            catch (NullReferenceException)
            {
                MessageBox.Show(Programa.textosLocal[23]);
            }

            catch (FormatException)
            {
                MessageBox.Show(Programa.textosLocal[22]);
                //throw;
            }


        }

        private void agregarCancion_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void agregarCancion_Load(object sender, EventArgs e)
        {

        }
        private void agregarCancion_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                min = Convert.ToInt32(minTextBox.Text);
                sec = Convert.ToInt32(secsTextBox.Text);
                t = tituloTextBox.Text;
                Cancion c = new Cancion(t, new TimeSpan(0, min, sec));
                album.agregarCancion(c, cual);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK);
            }
            Close();
        }

        private void tituloTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.Tab))
            {
                minTextBox.Focus();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult =  DialogResult.Cancel;
            Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
