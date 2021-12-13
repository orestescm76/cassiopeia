using System;
using System.Windows.Forms;
using Cassiopeia.src.Classes;

namespace Cassiopeia.src.Forms
{
    public partial class CreateCD : Form
    {
        private AlbumData album;
        private CompactDisc creatingCD = null;
        private CompactDisc editingCD;
        private int numDisc;
        private int NC;
        private bool edit = false;
        TimeSpan maxLength;
        public CreateCD(ref AlbumData a)
        {
            InitializeComponent();
            album = a;
            Log.Instance.PrintMessage("Creating a CD. Album length: "+a.Length, MessageType.Info);
            numDisc = 1;
            NC = a.NumberOfSongs;
            if (a.Length.TotalMinutes >= 60)
                labelCDDuration.Text = a.Length.ToString(@"h\:mm\:ss");
            else
                labelCDDuration.Text = a.Length.ToString(@"mm\:ss");
            //Now we need to check if the album needs one or more CDS.
            SetMaxLength();
            PutTexts();
        }
        public CreateCD(ref CompactDisc cdd, int numDisc, bool edit = false)
        {
            InitializeComponent();
            this.numDisc = numDisc;
            album = cdd.Album;
            editingCD = cdd;
            creatingCD = cdd;
            //If we're NOT editing
            if(numDisc > 1 && !edit)
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
            }
            else if(edit)
            {
                Log.Instance.PrintMessage("Editando CD", MessageType.Info);
                creatingCD = null;
                this.edit = true;
                comboBoxFormatoCD.SelectedItem = cdd.SleeveType;
                comboBoxEstadoMedio.SelectedItem = cdd.Discos[numDisc-1].MediaCondition;
                comboBoxEstadoExterior.SelectedItem = cdd.EstadoExterior;
                numericUpDownNumCanciones.Maximum = cdd.Discos[numDisc - 1].NumberOfSongs;
                numericUpDownNumCanciones.Value = cdd.Discos[numDisc - 1].NumberOfSongs;
                textBoxAño.Text = editingCD.Year.ToString();
                textBoxPais.Text = editingCD.Country;
            }
            SetMaxLength();
            PutTexts();
        }
        private void PutTexts()
        {
            if(creatingCD is not null || numDisc == 1)
                Text = Kernel.LocalTexts.GetString("creando") + " CD " + numDisc;
            else
                Text = Kernel.LocalTexts.GetString("editando") + " CD " + numDisc;
            labelEstadoExterior.Text = Kernel.LocalTexts.GetString("estado_exterior");
            labelEstadoMedio.Text = Kernel.LocalTexts.GetString("estado_medio");
            labelFormato.Text = Kernel.LocalTexts.GetString("formato");
            labelAñoPublicacion.Text = Kernel.LocalTexts.GetString("añoPublicacion");
            labelPaisPublicacion.Text = Kernel.LocalTexts.GetString("paisPublicacion");
            labelNumCanciones.Text = Kernel.LocalTexts.GetString("numcanciones");
            String[] eeT = new string[7];
            String[] fT = new string[4];
            for (int i = 0; i < eeT.Length; i++)
                eeT[i] = Kernel.LocalTexts.GetString(Enum.GetName(typeof(MediaCondition), i));
            for (int i = 0; i < fT.Length; i++)
                fT[i] = Kernel.LocalTexts.GetString(Enum.GetName(typeof(SleeveType), i));
            comboBoxEstadoMedio.Items.AddRange(eeT);
            comboBoxEstadoExterior.Items.AddRange(eeT);
            comboBoxFormatoCD.Items.AddRange(fT);
            comboBoxEstadoMedio.SelectedIndex = 0;
            comboBoxEstadoExterior.SelectedIndex = 0;
            comboBoxFormatoCD.SelectedIndex = 0;
        }
        private void SetMaxLength()
        {
            //Get max number of songs for first CD.
            int numSongs = 0;
            if (creatingCD is not null)
                numSongs = editingCD.TotalSongs;
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
            if(creatingCD is not null)
            {
                numericUpDownNumCanciones.Maximum = numSongs-editingCD.TotalSongs;
                numericUpDownNumCanciones.Value = numSongs- editingCD.TotalSongs;
            }
            else
            {
                numericUpDownNumCanciones.Maximum = numSongs;
                numericUpDownNumCanciones.Value = numSongs;
            }

        }
        private void CreateNewCD(int numberSongs)
        {
            MediaCondition exterior = (MediaCondition)Enum.Parse(typeof(MediaCondition), comboBoxEstadoExterior.SelectedIndex.ToString());
            MediaCondition medio = (MediaCondition)Enum.Parse(typeof(MediaCondition), comboBoxEstadoMedio.SelectedIndex.ToString());
            SleeveType formato = (SleeveType)Enum.Parse(typeof(SleeveType), comboBoxFormatoCD.SelectedIndex.ToString());

            //Creating CD
            try
            {
                creatingCD = new CompactDisc(album, numberSongs, medio, exterior, formato, Convert.ToInt16(textBoxAño.Text), textBoxPais.Text);

            }
            catch (Exception)
            {
                //msgbox please enter a valid year...
                MessageBox.Show("enter a good year ffs");
                throw;
            }
            Kernel.Collection.AddCD(ref creatingCD);
            Log.Instance.PrintMessage("CD added OK", MessageType.Correct);
            Kernel.SetSaveMark();

        }
        private void buttonOK_Click(object sender, EventArgs e)
        {
            NC = (int)numericUpDownNumCanciones.Value;
            album.CanBeRemoved = false;

            MediaCondition exterior = (MediaCondition)Enum.Parse(typeof(MediaCondition), comboBoxEstadoExterior.SelectedIndex.ToString());
            MediaCondition medio = (MediaCondition)Enum.Parse(typeof(MediaCondition), comboBoxEstadoMedio.SelectedIndex.ToString());
            SleeveType formato = (SleeveType)Enum.Parse(typeof(SleeveType), comboBoxFormatoCD.SelectedIndex.ToString());

            if (edit) //Editing an existing CD
            {
                editingCD.SleeveType = formato;
                editingCD.Discos[numDisc - 1].MediaCondition = medio;
                editingCD.EstadoExterior = exterior;
                editingCD.Discos[numDisc - 1].NumberOfSongs=(short)numericUpDownNumCanciones.Value;
                editingCD.Year = Convert.ToInt16(textBoxAño.Text);
                editingCD.Country = textBoxPais.Text;
                AlbumViewer nuevo = new AlbumViewer(ref editingCD);
                Kernel.ReloadView();
                nuevo.Show();
                Close();
                Dispose();
                return;
            }
            if (editingCD is not null)
            {
                //We are creating another disc.
                Disc disc = new Disc(Convert.ToInt16(numericUpDownNumCanciones.Value), medio);
                editingCD.Discos.Add(disc);
                if (editingCD.TotalSongs != album.NumberOfSongs)
                    AnotherCD();
                else
                    Dispose();
            }
            else
            {
                //We create the CD
                try
                {
                    CreateNewCD(NC);
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
            DialogResult res = MessageBox.Show("Another CD" + Environment.NewLine + "Quedan " + (album.Songs.Count - creatingCD.TotalSongs) + " canciones", "", MessageBoxButtons.YesNo);
            if (res == DialogResult.No)
            {
                AlbumViewer v = new AlbumViewer(ref creatingCD);
                v.Show();
                Close();
                Dispose();
            }

            else
            {
                CreateCD newCD = new CreateCD(ref creatingCD, numDisc + 1);
                //We're done here
                newCD.ShowDialog();
                Dispose();
            }
        }
        private void numericUpDownNumCanciones_ValueChanged(object sender, EventArgs e)
        {
            TimeSpan len = TimeSpan.Zero;
            int numSongs = 0;
            if (creatingCD is not null)
                numSongs = creatingCD.TotalSongs;

            for (int i = numSongs; i < numericUpDownNumCanciones.Value+numSongs; i++)
            {
                len += album.Songs[i].Length;
            }
            if (len.TotalMinutes >= 60)
                labelCDDuration.Text = len.ToString(@"h\:mm\:ss");
            else
                labelCDDuration.Text = len.ToString(@"mm\:ss");
        }
    }
}
