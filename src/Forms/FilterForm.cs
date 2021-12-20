using System;
using System.Windows.Forms;
using Cassiopeia.src.Classes;

namespace Cassiopeia.src.Forms
{
    public partial class FilterForm : Form
    {
        public FilterForm()
        {
            InitializeComponent();
            PutTexts();
        }

        private void PutTexts()
        {
            Text = Kernel.LocalTexts.GetString("filter");
            labelWriteFilter.Text = Kernel.LocalTexts.GetString("write_filter");
            labelArtist.Text = Kernel.LocalTexts.GetString("artista");
            labelTitle.Text = Kernel.LocalTexts.GetString("titulo");
            labelSongTitle.Text = Kernel.LocalTexts.GetString("song_title");
            buttonOk.Text = Kernel.LocalTexts.GetString("aceptar");
            buttonReset.Text = Kernel.LocalTexts.GetString("reset");
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            //apply filters
            Filter filter = new();
            filter.Artist = textBoxArtist.Text.ToLower();
            filter.Title = textBoxAlbumName.Text.ToLower();
            filter.ContainsSongTitle = textBoxSongTitle.Text.ToLower();
            Kernel.MainForm.ApplyFilter(filter);
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            Kernel.MainForm.ResetFilter();
        }
    }
}
