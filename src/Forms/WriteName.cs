using System;
using System.Windows.Forms;

namespace Cassiopeia.src.Forms
{
    public partial class WriteName : Form
    {
        public string PlaylistName { get; private set; } //Name of the playlist.
        public WriteName()
        {
            InitializeComponent();
        }
        private void PonerTextos()
        {
            Text = Program.LocalTexts.GetString("playlistName");
            buttonOk.Text = Program.LocalTexts.GetString("aceptar");
        }
        private void WriteName_Load(object sender, EventArgs e)
        {
            PonerTextos();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            PlaylistName = textBoxName.Lines[0];
            DialogResult = DialogResult.OK;
            if (string.IsNullOrEmpty(PlaylistName))
                DialogResult = DialogResult.Cancel;
            Close();
            Dispose();
        }
    }
}
