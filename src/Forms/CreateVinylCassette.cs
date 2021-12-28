using System;
using System.Windows.Forms;
using Cassiopeia.src.Classes;

namespace Cassiopeia.src.Forms
{
    public partial class CreateVinylCassette : Form
    {
        private AlbumData album;
        private VinylAlbum creatingVinyl = null;
        private VinylAlbum editingVinyl;
        private int numDisc;
        private int NCA, NCB;
        private bool edit = false;
        public CreateVinylCassette(ref AlbumData a, bool vinyl = true)
        {
            InitializeComponent();
            album = a;
            Log.Instance.PrintMessage("Creating a Vinyl. Album length: "+a.Length, MessageType.Info);
            numDisc = 1;
            //Now we need to check if the album needs one or more Vinyl.
            SetMaxLength();
            PutTexts();
        }
        public CreateVinylCassette(ref VinylAlbum vinyl, int numDisc, bool edit = false)
        {
            InitializeComponent();
            this.numDisc = numDisc;
            album = vinyl.Album;
            editingVinyl = vinyl;
            creatingVinyl = vinyl;
            //If we're NOT editing
            if(numDisc > 1 && !edit)
            {
                labelAñoPublicacion.Hide();
                comboBoxEstadoExterior.Hide();
                labelEstadoExterior.Hide();
                labelAñoPublicacion.Hide();
                labelPaisPublicacion.Hide();
                textBoxPais.Hide();
                textBoxAño.Hide();
            }
            else if(edit)
            {
                Log.Instance.PrintMessage("Editando CD", MessageType.Info);
                creatingVinyl = null;
                this.edit = true;
                comboBoxEstadoMedio.SelectedItem = vinyl.DiscList[numDisc-1].MediaCondition;
                comboBoxEstadoExterior.SelectedItem = vinyl.SleeveCondition;
                numericUpDownNumSongsFront.Value = vinyl.DiscList[numDisc-1].NumberOfSongs;
                textBoxAño.Text = editingVinyl.Year.ToString();
                textBoxPais.Text = editingVinyl.Country;
            }
            //SetMaxLength();
            //PutTexts();
        }
        
        private void PutTexts()
        {
            if(creatingVinyl is not null || numDisc == 1)
                Text = Kernel.LocalTexts.GetString("creando") + " CD " + numDisc;
            else
                Text = Kernel.LocalTexts.GetString("editando") + " CD " + numDisc;
            labelEstadoExterior.Text = Kernel.LocalTexts.GetString("estado_exterior");
            labelEstadoMedio.Text = Kernel.LocalTexts.GetString("estado_medio");
            labelAñoPublicacion.Text = Kernel.LocalTexts.GetString("añoPublicacion");
            labelPaisPublicacion.Text = Kernel.LocalTexts.GetString("paisPublicacion");
            labelNumSongsFront.Text = Kernel.LocalTexts.GetString("numcanciones");
            String[] eeT = new string[7];
            String[] fT = new string[4];
            for (int i = 0; i < eeT.Length; i++)
                eeT[i] = Kernel.LocalTexts.GetString(Enum.GetName(typeof(MediaCondition), i));
            for (int i = 0; i < fT.Length; i++)
                fT[i] = Kernel.LocalTexts.GetString(Enum.GetName(typeof(SleeveType), i));
            comboBoxEstadoMedio.Items.AddRange(eeT);
            comboBoxEstadoExterior.Items.AddRange(eeT);
            comboBoxEstadoMedio.SelectedIndex = 0;
            comboBoxEstadoExterior.SelectedIndex = 0;
        }
        private void SetMaxLength()
        {
            //Get max number of songs for first vinyl.
            int numSongsA = 0, numSongsB = 0, numSongs = 0;
            if (creatingVinyl is not null)
                numSongs = editingVinyl.TotalSongs;
            //Calc numsongs for both sides.
            //Side A
            //TimeSpan a = TimeSpan.Zero;
            //for (int i = 0; i < album.Songs.Count; i++)
            //{
            //    a += album.Songs[i].Length;
            //    if(a.TotalMinutes > 35)
            //    {
            //        a -= album.Songs[i].Length;
            //        break;
            //    }
            //    numSongsA++;
            //}
            //TimeSpan b = TimeSpan.Zero;
            //for (int i = numSongsA; i < album.Songs.Count; i++)
            //{
            //    b += album.Songs[i].Length;
            //    if (b.TotalMinutes > 35)
            //    {
            //        b -= album.Songs[i].Length;
            //        break;
            //    }
            //    numSongsB++;
            //}
            numericUpDownNumSongsFront.Maximum = album.Songs.Count;
            numericUpDownNumSongsBack.Maximum = album.Songs.Count;
            
            //if (creatingVinyl is not null)
            //{
            //    numericUpDownNumSongsFront.Maximum = numSongs-editingVinyl.TotalSongs;
            //    numericUpDownNumSongsFront.Value = numSongsA;
            //    numericUpDownNumSongsBack.Value = numSongsB;
            //}
            //else
            //{

            //    //numericUpDownNumSongsFront.Maximum = numSongs;
            //    //numericUpDownNumSongsFront.Value = numSongs;
            //}

        }
        private void CreateNewVinyl(int nsFront, int nsBack)
        {
            MediaCondition exterior = (MediaCondition)Enum.Parse(typeof(MediaCondition), comboBoxEstadoExterior.SelectedIndex.ToString());
            MediaCondition medio = (MediaCondition)Enum.Parse(typeof(MediaCondition), comboBoxEstadoMedio.SelectedIndex.ToString());
            string s = album.Artist + "_" + album.Title;

            //Creating Vinyl
            try
            {
                creatingVinyl = new VinylAlbum(album, nsFront, nsBack, exterior, medio, Convert.ToInt16(textBoxAño.Text), textBoxPais.Text);
            }
            catch (Exception)
            {
                //msgbox please enter a valid year...
                MessageBox.Show("enter a good year ffs");
                throw;
            }
            Kernel.Collection.AddVinyl(ref creatingVinyl);
            Log.Instance.PrintMessage("Vinyl added OK", MessageType.Correct);
            Kernel.SetSaveMark();
        }
        private void buttonOK_Click(object sender, EventArgs e)
        {
            NCA = (int)this.numericUpDownNumSongsFront.Value;
            NCB = (int)this.numericUpDownNumSongsBack.Value;
            album.CanBeRemoved = false;

            MediaCondition exterior = (MediaCondition)Enum.Parse(typeof(MediaCondition), comboBoxEstadoExterior.SelectedIndex.ToString());
            MediaCondition medio = (MediaCondition)Enum.Parse(typeof(MediaCondition), comboBoxEstadoMedio.SelectedIndex.ToString());

            if (edit) //Editing an existing Vinyl
            {
                editingVinyl.DiscList[numDisc - 1].MediaCondition = medio;
                editingVinyl.SleeveCondition = exterior;
                //editingVinyl.discList[numDisc - 1].NumberOfSongs=(int)this.numericUpDownNumSongsFront.Value;
                editingVinyl.Year = Convert.ToInt16(textBoxAño.Text);
                editingVinyl.Country = textBoxPais.Text;
                //AlbumViewer nuevo = new AlbumViewer(ref editingVinyl);
                Kernel.ReloadView();
                //nuevo.Show();
                Close();
                Dispose();
                return;
            }
            if (editingVinyl is not null)
            {
                //We are creating another disc.
                Disc disc = new Disc(Convert.ToInt16(this.numericUpDownNumSongsFront.Value), medio);
                //editingVinyl.Discos.Add(disc);
                if (editingVinyl.TotalSongs != album.NumberOfSongs)
                    AnotherDisc();
                else
                    Dispose();
            }
            else
            {
                //We create the Vinyl
                try
                {
                    CreateNewVinyl(NCA, NCB);
                }
                catch (Exception)
                {
                    return;
                }
                //The user might want a smaller CD, excluding the bonus songs. Or we need another disc 
                if (NCA + NCB != album.NumberOfSongs)
                {
                    AnotherDisc();
                }
            }
            Close();
        }
        private void AnotherDisc()
        {
            //Another CD?
            DialogResult res = MessageBox.Show("Another CD" + Environment.NewLine + "Quedan " + (album.Songs.Count - creatingVinyl.TotalSongs) + " canciones", "", MessageBoxButtons.YesNo);
            if (res == DialogResult.No)
            {
                //AlbumViewer v = new AlbumViewer(ref creatingVinyl);
                //v.Show();
                Close();
                Dispose();
            }

            else
            {
                CreateVinylCassette newCD = new CreateVinylCassette(ref creatingVinyl, numDisc + 1);
                //We're done here
                newCD.ShowDialog();
                Dispose();
            }
        }
        private void numericUpDownNumCanciones_ValueChanged(object sender, EventArgs e)
        {
            TimeSpan len = TimeSpan.Zero;
            int numSongs = 0;
            if (creatingVinyl is not null)
                numSongs = creatingVinyl.TotalSongs;

            for (int i = numSongs; i < numericUpDownNumSongsFront.Value+numSongs; i++)
            {
                len += album.Songs[i].Length;
            }
            if (len.TotalMinutes >= 60)
                labelFrontLength.Text = "A: "+ len.ToString(@"h\:mm\:ss");
            else
                labelFrontLength.Text = "A: " + len.ToString(@"mm\:ss");
            numericUpDownNumSongsFront.Maximum = album.Songs.Count - numericUpDownNumSongsBack.Value;
            numericUpDownNumSongsBack.Maximum = album.Songs.Count - numericUpDownNumSongsFront.Value;
        }

        private void CreateVinyl_Load(object sender, EventArgs e)
        {

        }

        private void numericUpDownNumSongsBack_ValueChanged(object sender, EventArgs e)
        {
            TimeSpan len = TimeSpan.Zero;
            int numSongs = 0;
            if (creatingVinyl is not null)
                numSongs = creatingVinyl.TotalSongs;
            numSongs += (int)numericUpDownNumSongsFront.Value;
            for (int i = numSongs; i < numericUpDownNumSongsBack.Value + numSongs; i++)
            {
                len += album.Songs[i].Length;
            }
            if (len.TotalMinutes >= 60)
                labelBackLength.Text = "B: " + len.ToString(@"h\:mm\:ss");
            else
                labelBackLength.Text = "B: " + len.ToString(@"mm\:ss");
            numericUpDownNumSongsFront.Maximum = album.Songs.Count - numericUpDownNumSongsBack.Value;
            numericUpDownNumSongsBack.Maximum = album.Songs.Count - numericUpDownNumSongsFront.Value;
        }
    }
}