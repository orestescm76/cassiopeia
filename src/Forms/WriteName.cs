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
            Text = Kernel.LocalTexts.GetString("playlistName");
            buttonOk.Text = Kernel.LocalTexts.GetString("aceptar");
        }
        private void WriteName_Load(object sender, EventArgs e)
        {
            PonerTextos();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            if (textBoxName.Lines.Length != 0)
            {
                PlaylistName = textBoxName.Lines[0];
                DialogResult = DialogResult.OK;
                if (string.IsNullOrEmpty(PlaylistName))
                    DialogResult = DialogResult.Cancel;
            }
            else
                DialogResult = DialogResult.Cancel;
            Close();
            Dispose();

        }
    }
}
