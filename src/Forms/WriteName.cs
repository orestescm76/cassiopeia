using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace aplicacion_musica.src.Forms
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
        }
    }
}
