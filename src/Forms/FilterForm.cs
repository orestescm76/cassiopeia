using Cassiopeia.src.Classes;
using System;
using System.Windows.Forms;

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
            Text = Kernel.GetText("filter");
            labelWriteFilter.Text = Kernel.GetText("write_filter");
            labelArtist.Text = Kernel.GetText("artista");
            labelTitle.Text = Kernel.GetText("titulo");
            labelSongTitle.Text = Kernel.GetText("song_title");
            buttonOk.Text = Kernel.GetText("aceptar");
            buttonReset.Text = Kernel.GetText("reset");
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
