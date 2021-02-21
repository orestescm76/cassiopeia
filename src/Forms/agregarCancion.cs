using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace aplicacion_musica
{
    public partial class agregarCancion : Form
    {
        public string t;
        public int min, sec, np;
        private int cual;
        AlbumData album;
        Song cancion;
        CancionLarga cancionlarga;
        bool editar;
        bool larga;
        bool bonus;
        ToolTip ConsejoEsLarga;
        ToolTip ConsejoEsBonus;
        public agregarCancion(ref AlbumData a, int n) //caso normal
        {
            Log.Instance.ImprimirMensaje("Creando canción", TipoMensaje.Info);
            Stopwatch crono = Stopwatch.StartNew();
            InitializeComponent();
            album = a;
            cual = n;
            editar = false;
            textBoxNumPartes.Hide();
            labelNumPartes.Hide();
            cancionlarga = null;
            np = 0;
            ConsejoEsLarga = new ToolTip();
            ConsejoEsLarga.SetToolTip(esLarga, Program.LocalTexts.GetString("ayuda_larga"));
            ConsejoEsBonus = new ToolTip();
            ConsejoEsBonus.SetToolTip(checkBoxBonus, Program.LocalTexts.GetString("esBonusAyuda"));
            ponerTextos();
            crono.Stop();
            Log.Instance.ImprimirMensaje("Cargado", TipoMensaje.Correcto, crono);
        }
        public agregarCancion(ref Song c) //editar
        {
            Log.Instance.ImprimirMensaje("Editando canción", TipoMensaje.Info);
            Stopwatch crono = Stopwatch.StartNew();
            InitializeComponent();
            cual = -1;
            esLarga.Hide();
            cancion = c;
            editar = true;
            cancionlarga = null;
            if(c is CancionLarga)
            {
                minTextBox.Enabled = false;
                secsTextBox.Enabled = false;
                checkBoxBonus.Enabled = false;
            }
            tituloTextBox.Text = c.Title;
            if (c.Length.TotalMinutes >= 60)
                minTextBox.Text = ((int)c.Length.TotalMinutes).ToString();
            else
                minTextBox.Text = c.Length.Minutes.ToString();
            secsTextBox.Text = c.Length.Seconds.ToString();
            if (c is CancionLarga)
            {
                minTextBox.Enabled = false;
                secsTextBox.Enabled = false;
            }
            esLarga.Hide();
            labelNumPartes.Hide();
            textBoxNumPartes.Hide();
            ConsejoEsBonus = new ToolTip();
            ConsejoEsBonus.SetToolTip(checkBoxBonus, Program.LocalTexts.GetString("esBonusAyuda"));
            if (c.IsBonus)
                checkBoxBonus.Checked = true;
            np = 0;
            ponerTextos();
            crono.Stop();
            Log.Instance.ImprimirMensaje("Cargado", TipoMensaje.Correcto, crono);
        }
        public agregarCancion(ref AlbumData a, int n, bool l) //crear canción larga
        {
            Log.Instance.ImprimirMensaje("Creando canción con partes", TipoMensaje.Info);
            Stopwatch crono = Stopwatch.StartNew();
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
            crono.Stop();
            Log.Instance.ImprimirMensaje("Cargado", TipoMensaje.Correcto, crono);
        }
        public agregarCancion(ref CancionLarga l, int n, ref AlbumData a) //crear parte de canción larga
        {
            Log.Instance.ImprimirMensaje("Creando parte de canción larga", TipoMensaje.Info);
            Stopwatch crono = Stopwatch.StartNew();
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
            crono.Stop();
            Log.Instance.ImprimirMensaje("Cargado", TipoMensaje.Correcto, crono);
        }
        private void ponerTextos()
        {
            int cualdeVerdad = cual;
            if (cual == -2)
                cualdeVerdad = album.NumberOfSongs;
            if(editar)
            {
                Text = Program.LocalTexts.GetString("editando") + " " + cancion.titulo;
                buttonOK.Text = Program.LocalTexts.GetString("hecho");
            }

            else
            {
                Text = Program.LocalTexts.GetString("añadir_cancion") + " " + (cualdeVerdad+1);
                buttonOK.Text = Program.LocalTexts.GetString("hecho");
            }
            if(cancionlarga != null)
            {
                Text = Program.LocalTexts.GetString("añadir_cancion") + " " + cancionlarga.GetNumeroRomano(cual);
            }
            buttonCancelar.Text = Program.LocalTexts.GetString("cancelar");
            labelTituloCancion.Text = Program.LocalTexts.GetString("introduce_cancion");
            labelMinutosSegundos.Text = Program.LocalTexts.GetString("min:sec");
            esLarga.Text = Program.LocalTexts.GetString("esLarga");
            labelNumPartes.Text = Program.LocalTexts.GetString("num_partes");
            checkBoxBonus.Text = Program.LocalTexts.GetString("esBonus");
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
                        cancion.Title = t;
                        cancion.Length = new TimeSpan(0, min, sec);
                        cancion.IsBonus = bonus;
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                    else
                    {
                        Song c = new Song(t, new TimeSpan(0, min, sec), ref album, bonus);
                        album.AddSong(c);
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                }
                else if(larga && cancionlarga == null) //caso de que creemos una cancion larga, sin partes
                {
                    t = tituloTextBox.Text;
                    min = sec = 0;
                    np = Convert.ToInt32(textBoxNumPartes.Text);
                    CancionLarga longSong = new CancionLarga(t, ref album);

                    album.AddSong(longSong);
                    for (int i = 0; i < np; i++)
                    {
                        agregarCancion addParte = new agregarCancion(ref longSong, i + 1, ref album);
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
                    Song p = new Song(t, dur, ref album);
                    cancionlarga.addParte(p);
                    DialogResult = DialogResult.OK;
                }
                Dispose();
            }
            catch (NullReferenceException ex)
            {
                Log.Instance.ImprimirMensaje(ex.Message, TipoMensaje.Error);

                MessageBox.Show(Program.LocalTexts.GetString("error_vacio1"));

            }

            catch (FormatException ex)
            {
                Log.Instance.ImprimirMensaje(ex.Message, TipoMensaje.Error);
                MessageBox.Show(Program.LocalTexts.GetString("error_formato"));
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
                Song c = new Song(t, new TimeSpan(0, min, sec), ref album);
                album.AddSong(c, cual);
                Log.Instance.ImprimirMensaje(t + " añadido correctamente", TipoMensaje.Correcto);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK);
                Log.Instance.ImprimirMensaje(ex.Message, TipoMensaje.Error);

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
            DialogResult =  DialogResult.Cancel;
            Log.Instance.ImprimirMensaje("Cancelado", TipoMensaje.Info);
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
