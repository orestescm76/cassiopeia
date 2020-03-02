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
        bool bonus;
        ToolTip ConsejoEsLarga;
        ToolTip ConsejoEsBonus;
        public agregarCancion(ref Album a, int n) //caso normal
        {
            Console.WriteLine("Creando canción");
            InitializeComponent();
            album = a;
            cual = n;
            editar = false;
            textBoxNumPartes.Hide();
            labelNumPartes.Hide();
            cancionlarga = null;
            np = 0;
            ConsejoEsLarga = new ToolTip();
            ConsejoEsLarga.SetToolTip(esLarga, Programa.textosLocal.GetString("ayuda_larga"));
            ConsejoEsBonus = new ToolTip();
            ConsejoEsBonus.SetToolTip(checkBoxBonus, Programa.textosLocal.GetString("esBonusAyuda"));
            ponerTextos();
        }
        public agregarCancion(ref Cancion c) //editar
        {
            InitializeComponent();
            cual = -1;
            esLarga.Hide();
            cancion = c;
            editar = true;
            cancionlarga = null;
            tituloTextBox.Text = c.titulo;
            minTextBox.Text = c.duracion.Minutes.ToString();
            secsTextBox.Text = c.duracion.Seconds.ToString();
            esLarga.Hide();
            labelNumPartes.Hide();
            textBoxNumPartes.Hide();
            ConsejoEsBonus = new ToolTip();
            ConsejoEsBonus.SetToolTip(checkBoxBonus, Programa.textosLocal.GetString("esBonusAyuda"));
            if (c.Bonus)
                checkBoxBonus.Checked = true;
            np = 0;
            ponerTextos();
        }
        public agregarCancion(ref Album a, int n, bool l) //crear canción larga
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
            checkBoxBonus.Hide();
        }
        public agregarCancion(ref CancionLarga l, int n, ref Album a) //crear parte de canción larga
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
            checkBoxBonus.Hide();
            ponerTextos();
        }
        private void ponerTextos()
        {
            int cualdeVerdad = cual + 1;
            if (cual == 0)
                cualdeVerdad = album.numCanciones+1;
            if(editar)
            {
                Text = Programa.textosLocal.GetString("editando") + " " + cancion.titulo;
                buttonOK.Text = Programa.textosLocal.GetString("hecho");
            }

            else
            {
                Text = Programa.textosLocal.GetString("añadir_cancion") + " " + cualdeVerdad;
                buttonOK.Text = Programa.textosLocal.GetString("hecho");
            }
            if(cancionlarga != null)
            {
                Text = Programa.textosLocal.GetString("añadir_cancion") + " " + cancionlarga.GetNumeroRomano(cual);
            }
            buttonCancelar.Text = Programa.textosLocal.GetString("cancelar");
            labelTituloCancion.Text = Programa.textosLocal.GetString("introduce_cancion");
            labelMinutosSegundos.Text = Programa.textosLocal.GetString("min:sec");
            esLarga.Text = Programa.textosLocal.GetString("esLarga");
            labelNumPartes.Text = Programa.textosLocal.GetString("num_partes");
            checkBoxBonus.Text = Programa.textosLocal.GetString("esBonus");
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
                    bonus = checkBoxBonus.Checked;
                    if (editar) //si edita
                    {
                        cancion.titulo = t;
                        cancion.duracion = new TimeSpan(0, min, sec);
                        cancion.Bonus = bonus;
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                    else
                    {
                        Cancion c = new Cancion(t, new TimeSpan(0, min, sec), ref album, bonus);
                        if (cual != 0)
                            album.agregarCancion(c, cual);
                        else
                            album.agregarCancion(c);
                        DialogResult = DialogResult.OK;
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
                        else
                            DialogResult = DialogResult.OK;
                    }
                }
                else if(cancionlarga != null && larga == true)//parte de una cancion normal
                {
                    t = tituloTextBox.Text;
                    min = Convert.ToInt32(minTextBox.Text);
                    sec = Convert.ToInt32(secsTextBox.Text);
                    TimeSpan dur = new TimeSpan(0, min, sec);
                    np = 0;
                    Cancion p = new Cancion(t, dur, ref album);
                    cancionlarga.addParte(ref p);
                    DialogResult = DialogResult.OK;
                    album.duracion += dur;
                }
                Dispose();
            }
            catch (NullReferenceException)
            {
                MessageBox.Show(Programa.textosLocal.GetString("error_vacio1"));
            }

            catch (FormatException)
            {
                MessageBox.Show(Programa.textosLocal.GetString("error_formato"));
                //throw;
            }


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

        private void esLarga_Click(object sender, EventArgs e)
        {
            agregarCancion larga = new agregarCancion(ref album, cual, true);
            DialogResult = DialogResult.OK;
            larga.ShowDialog();
        }
    }
}
