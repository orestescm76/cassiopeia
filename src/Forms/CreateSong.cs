﻿using Cassiopeia.src.Classes;
using System;
using System.Windows.Forms;

namespace Cassiopeia.src.Forms
{
    public partial class CreateSong : Form
    {
        public string title;
        public int min, sec, np;
        private int cual;
        AlbumData album;
        Song cancion;
        LongSong cancionlarga;
        bool editar;
        bool larga;
        bool bonus;
        ToolTip ConsejoEsLarga;
        ToolTip ConsejoEsBonus;
        public CreateSong(ref AlbumData a, int n) //caso normal
        {
            Log.Instance.PrintMessage("Creating song", MessageType.Info);
            InitializeComponent();
            album = a;
            cual = n;
            editar = false;
            textBoxNumPartes.Hide();
            labelNumPartes.Hide();
            cancionlarga = null;
            np = 0;
            ConsejoEsLarga = new ToolTip();
            ConsejoEsLarga.SetToolTip(buttonLongSong, Kernel.GetText("ayuda_larga"));
            ConsejoEsBonus = new ToolTip();
            ConsejoEsBonus.SetToolTip(checkBoxBonus, Kernel.GetText("esBonusAyuda"));
            ponerTextos();
            labelMinutosSegundos.Location = new System.Drawing.Point((Width / 2) - (labelMinutosSegundos.Width / 2), labelMinutosSegundos.Location.Y);
        }
        public CreateSong(ref Song c) //editar
        {
            Log.Instance.PrintMessage("Editing song " + c.Title, MessageType.Info);
            InitializeComponent();
            cual = -1;
            buttonLongSong.Hide();
            cancion = c;
            editar = true;
            cancionlarga = null;
            if (c is LongSong)
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
            if (c is LongSong)
            {
                minTextBox.Enabled = false;
                secsTextBox.Enabled = false;
            }
            buttonLongSong.Hide();
            labelNumPartes.Hide();
            textBoxNumPartes.Hide();
            ConsejoEsBonus = new ToolTip();
            ConsejoEsBonus.SetToolTip(checkBoxBonus, Kernel.GetText("esBonusAyuda"));
            if (c.IsBonus)
                checkBoxBonus.Checked = true;
            np = 0;
            ponerTextos();
        }
        public CreateSong(ref AlbumData a, int n, bool l) //crear canción larga
        {
            Log.Instance.PrintMessage("Creating multipart song", MessageType.Info);
            InitializeComponent();
            larga = l;
            album = a;
            cual = n;
            editar = false;
            cancionlarga = null;
            ponerTextos();
            labelNumPartes.Location = new System.Drawing.Point((Width / 2) - (labelNumPartes.Width / 2), labelNumPartes.Location.Y);
            textBoxNumPartes.Location = new System.Drawing.Point((Width / 2) - (textBoxNumPartes.Width / 2), textBoxNumPartes.Location.Y);
            labelColon.Hide();
            labelMinutosSegundos.Hide();
            secsTextBox.Hide();
            minTextBox.Hide();
            buttonLongSong.Hide();
            checkBoxBonus.Hide();
        }

        public CreateSong(ref LongSong l, int n, ref AlbumData a) //crear parte de canción larga
        {
            Log.Instance.PrintMessage("Creating part of a multipart song", MessageType.Info);
            InitializeComponent();
            cancionlarga = l;
            cual = n;
            editar = false;
            larga = true;
            album = a;
            textBoxNumPartes.Hide();
            labelNumPartes.Hide();
            buttonLongSong.Hide();
            np = 0;
            checkBoxBonus.Hide();
            ponerTextos();
        }

        private void ponerTextos()
        {
            int cualdeVerdad = cual;
            if (cual == -2)
                cualdeVerdad = album.NumberOfSongs;

            if (editar)
            {
                Text = Kernel.GetText("editando") + " " + cancion.Title;
                buttonOK.Text = Kernel.GetText("hecho");
            }
            else
            {
                Text = Kernel.GetText("añadir_cancion") + " " + (cualdeVerdad + 1);
                buttonOK.Text = Kernel.GetText("hecho");
            }

            if (cancionlarga != null)
            {
                Text = Kernel.GetText("añadir_cancion") + " " + Utils.ConvertToRomanNumeral(cual);
            }

            buttonCancelar.Text = Kernel.GetText("cancelar");
            labelTituloCancion.Text = Kernel.GetText("introduce_cancion");
            labelMinutosSegundos.Text = Kernel.GetText("min:sec");
            buttonLongSong.Text = Kernel.GetText("esLarga");
            labelNumPartes.Text = Kernel.GetText("numPartes");
            checkBoxBonus.Text = Kernel.GetText("esBonus");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (!larga && cancionlarga is null) //caso normal
                {
                    min = Convert.ToInt32(minTextBox.Text);
                    sec = Convert.ToInt32(secsTextBox.Text);
                    title = tituloTextBox.Text;
                    bonus = checkBoxBonus.Checked;
                    if (editar) //si edita
                    {
                        cancion.Title = title;
                        cancion.Length = new TimeSpan(0, min, sec);
                        cancion.IsBonus = bonus;
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                    else
                    {
                        Song c = new Song(title, new TimeSpan(0, min, sec), ref album, bonus);
                        album.AddSong(c);
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                }
                else if (larga && cancionlarga is null) //caso de que creemos una cancion larga, sin partes
                {
                    title = tituloTextBox.Text;
                    min = sec = 0;
                    np = Convert.ToInt32(textBoxNumPartes.Text);
                    LongSong longSong = new LongSong(title, album);

                    album.AddSong(longSong);
                    for (int i = 0; i < np; i++)
                    {
                        CreateSong addParte = new CreateSong(ref longSong, i + 1, ref album);
                        addParte.ShowDialog();
                        if (addParte.DialogResult == DialogResult.Cancel)
                            break;
                        else
                            DialogResult = DialogResult.OK;
                    }
                }
                else if (cancionlarga is not null && larga == true)//parte de una cancion normal
                {
                    title = tituloTextBox.Text;
                    min = Convert.ToInt32(minTextBox.Text);
                    sec = Convert.ToInt32(secsTextBox.Text);
                    TimeSpan dur = new TimeSpan(0, min, sec);
                    np = 0;
                    Song p = new Song(title, dur, ref album);
                    cancionlarga.AddPart(p);
                    DialogResult = DialogResult.OK;
                }
                Dispose();
            }
            catch (NullReferenceException ex)
            {
                Log.Instance.PrintMessage(ex.Message, MessageType.Error);

                MessageBox.Show(Kernel.GetText("error_vacio1"));

            }

            catch (FormatException ex)
            {
                Log.Instance.PrintMessage(ex.Message, MessageType.Error);
                MessageBox.Show(Kernel.GetText("error_formato"));
                //throw;
            }


        }
        private void agregarCancion_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                min = Convert.ToInt32(minTextBox.Text);
                sec = Convert.ToInt32(secsTextBox.Text);
                title = tituloTextBox.Text;
                Song c = new Song(title, new TimeSpan(0, min, sec), ref album);
                album.AddSong(c, cual);
                Log.Instance.PrintMessage(title + " añadido correctamente", MessageType.Correct);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK);
                Log.Instance.PrintMessage(ex.Message, MessageType.Error);

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

        private void buttonCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Log.Instance.PrintMessage("Cancelled", MessageType.Info);
            Close();
        }

        private void esLarga_Click(object sender, EventArgs e)
        {
            CreateSong larga = new CreateSong(ref album, cual, true);
            Close();
            DialogResult = larga.ShowDialog();
        }
    }
}
