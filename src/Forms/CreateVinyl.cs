using System;
using System.Windows.Forms;

namespace Cassiopeia
{
    public partial class CreateVinyl : Form
    {
        private AlbumData album;
        private VinylAlbum creatingVinyl = null;
        private VinylAlbum editingVinyl;
        private int numDisc;
        private int NC;
        private bool edit = false;
        TimeSpan maxLength;
        public CreateVinyl(ref AlbumData a)
        {
            InitializeComponent();
            album = a;
            Log.Instance.PrintMessage("Creating a CD. Album length: "+a.Length, MessageType.Info);
            numDisc = 1;
            NC = a.NumberOfSongs;
            if (a.Length.TotalMinutes >= 60)
                labelFrontLength.Text = a.Length.ToString(@"h\:mm\:ss");
            else
                labelFrontLength.Text = a.Length.ToString(@"mm\:ss");
            //Now we need to check if the album needs one or more CDS.
            //SetMaxLength();
            //PutTexts();
        }
        public CreateVinyl(ref VinylAlbum vinyl, int numDisc, bool edit = false)
        {
            InitializeComponent();
            this.numDisc = numDisc;
            album = vinyl.AlbumData;
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
                comboBoxEstadoMedio.SelectedItem = vinyl.discList[numDisc-1].MediaCondition;
                comboBoxEstadoExterior.SelectedItem = vinyl.EstadoExterior;
                numericUpDownNumSongsFront.Value = vinyl.discList[numDisc-1].NumberOfSongs;
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
            //Get max number of songs for first CD.
            int numSongs = 0;
            if (creatingVinyl is not null)
                numSongs = editingVinyl.TotalSongs;
            for (int i = numSongs; i < album.Songs.Count; i++)
            {
                maxLength += album.Songs[i].Length;
                if (maxLength.TotalMinutes > 79.5)
                {
                    maxLength -= album.Songs[i].Length;
                    break;
                }
                numSongs++;
            }
            if(creatingVinyl is not null)
            {
                numericUpDownNumSongsFront.Maximum = numSongs-editingVinyl.TotalSongs;
                numericUpDownNumSongsFront.Value = numSongs- editingVinyl.TotalSongs;
            }
            else
            {
                numericUpDownNumSongsFront.Maximum = numSongs;
                numericUpDownNumSongsFront.Value = numSongs;
            }

        }
        private void CreateNewCD(int nsFront, int nsBack)
        {
            MediaCondition exterior = (MediaCondition)Enum.Parse(typeof(MediaCondition), comboBoxEstadoExterior.SelectedIndex.ToString());
            MediaCondition medio = (MediaCondition)Enum.Parse(typeof(MediaCondition), comboBoxEstadoMedio.SelectedIndex.ToString());
            string s = album.Artist + "_" + album.Title;

            //Creating CD
            try
            {
                //creatingVinyl = new VinylDisc(s, numberSongs, medio, exterior, Convert.ToInt16(textBoxAño.Text), textBoxPais.Text);
                creatingVinyl = new VinylAlbum(s, nsFront, nsBack, exterior, medio, Convert.ToInt16(textBoxAño.Text), textBoxPais.Text);
            }
            catch (Exception)
            {
                //msgbox please enter a valid year...
                MessageBox.Show("enter a good year ffs");
                throw;
            }
            //Kernel.Collection.AddCD(ref creatingVinyl);

        }
        private void buttonOK_Click(object sender, EventArgs e)
        {
            NC = (int)this.numericUpDownNumSongsFront.Value;
            album.CanBeRemoved = false;

            MediaCondition exterior = (MediaCondition)Enum.Parse(typeof(MediaCondition), comboBoxEstadoExterior.SelectedIndex.ToString());
            MediaCondition medio = (MediaCondition)Enum.Parse(typeof(MediaCondition), comboBoxEstadoMedio.SelectedIndex.ToString());
            string s = album.Artist + "_" + album.Title;

            if (edit) //Editing an existing CD
            {
                editingVinyl.discList[numDisc - 1].MediaCondition = medio;
                editingVinyl.EstadoExterior = exterior;
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
                    AnotherCD();
                else
                    Dispose();
            }
            else
            {
                //We create the CD
                try
                {
                    //CreateNewCD(NC);
                }
                catch (Exception)
                {
                    return;
                }
                //The user might want a smaller CD, excluding the bonus songs. Or we need another disc 
                if (NC != album.NumberOfSongs)
                {
                    AnotherCD();
                }
            }          
        }
        private void AnotherCD()
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
                CreateVinyl newCD = new CreateVinyl(ref creatingVinyl, numDisc + 1);
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
                labelFrontLength.Text = len.ToString(@"h\:mm\:ss");
            else
                labelFrontLength.Text = len.ToString(@"mm\:ss");
        }

        private void CreateVinyl_Load(object sender, EventArgs e)
        {

        }
    }
}
