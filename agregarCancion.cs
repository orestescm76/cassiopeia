using System;
using System.Windows.Forms;

namespace aplicacion_musica
{
    public partial class agregarCancion : Form
    {
        public string t;
        public int min, sec, np;
        private int cual;
        Album album;
        Cancion cancion;
        CancionLarga cancionlarga;
        bool editar;
        bool larga;
        public agregarCancion(ref Album a, int n)
        {
            InitializeComponent();
            album = a;
            cual = n;
            editar = false;
            textBoxNumPartes.Hide();
            labelNumPartes.Hide();
            cancionlarga = null;
            np = 0;
            ponerTextos();
        }
        public agregarCancion(ref Cancion c)
        {
            InitializeComponent();
            esLarga.Hide();
            cancion = c;
            editar = true;
            cancionlarga = null;
            tituloTextBox.Text = c.titulo;
            minTextBox.Text = c.duracion.Minutes.ToString();
            secsTextBox.Text = c.duracion.Seconds.ToString();
            np = 0;
            ponerTextos();
        }
        public agregarCancion(ref Album a, int n, bool l)
        {
            InitializeComponent();
            larga = l;
            album = a;
            cual = n;
            editar = false;
            cancionlarga = null;
            ponerTextos();
            label2.Hide();
            labelMinutosSegundos.Hide();
            secsTextBox.Hide();
            minTextBox.Hide();
            esLarga.Hide();
        }
        public agregarCancion(ref CancionLarga l, int n, ref Album a)
        {
            InitializeComponent();
            cancionlarga = l;
            cual = n;
            editar = false;
            larga = true;
            album = a;
            textBoxNumPartes.Hide();
            labelNumPartes.Hide();
            esLarga.Hide();
            np = 0;
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
            if(cancionlarga != null)
            {
                Text = Programa.textosLocal[10] + " " + cancionlarga.GetNumeroRomano(cual);
            }
            button2.Text = Programa.textosLocal[11];
            labelTituloCancion.Text = Programa.textosLocal[12];
            labelMinutosSegundos.Text = Programa.textosLocal[13];

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            try
            {
                if(!larga && cancionlarga == null) //caso normal
                {
                    min = Convert.ToInt32(minTextBox.Text);
                    sec = Convert.ToInt32(secsTextBox.Text);
                    t = tituloTextBox.Text;
                    if (editar)
                    {
                        cancion.titulo = t;
                        cancion.duracion = new TimeSpan(0, min, sec);
                        this.DialogResult = DialogResult.OK;
                        Close();
                    }
                    else
                    {
                        Cancion c = new Cancion(t, new TimeSpan(0, min, sec), ref album);
                        album.agregarCancion(c, cual);
                        this.DialogResult = DialogResult.OK;
                        Close();
                    }
                }
                else if(larga && cancionlarga == null) //caso de que creemos una cancion larga, sin partes
                {
                    t = tituloTextBox.Text;
                    min = sec = 0;
                    np = Convert.ToInt32(textBoxNumPartes.Text);
                    CancionLarga cl = new CancionLarga(t, ref album);
                    album.agregarCancion(cl, cual);
                    for (int i = 0; i < np; i++)
                    {
                        agregarCancion addParte = new agregarCancion(ref cl, i + 1, ref album);
                        addParte.ShowDialog();
                        if (addParte.DialogResult == DialogResult.Cancel)
                            break;
                    }
                    album.RefrescarDuracion();
                }
                else //parte de una cancion normal
                {
                    t = tituloTextBox.Text;
                    min = Convert.ToInt32(minTextBox.Text);
                    sec = Convert.ToInt32(secsTextBox.Text);
                    np = 0;
                    Cancion p = new Cancion(t, new TimeSpan(0, min, sec), ref album);
                    cancionlarga.addParte(ref p);
                    DialogResult = DialogResult.OK;
                }
                Dispose();
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
                Cancion c = new Cancion(t, new TimeSpan(0, min, sec), ref album);
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

        private void minTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void esLarga_Click(object sender, EventArgs e)
        {
            agregarCancion larga = new agregarCancion(ref album, cual, true);
            larga.Show();
            Dispose();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
