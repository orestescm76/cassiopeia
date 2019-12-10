using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace aplicacion_ipo
{
    public partial class agregarCancion : Form
    {
        public string t;
        public int min, sec;
        private int cual;
        Album album;
        public agregarCancion(ref Album a, int n)
        {
            InitializeComponent();
            album = a;
            cual = n;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            min = Convert.ToInt32(minTextBox.Text);
            sec = Convert.ToInt32(secsTextBox.Text);
            t = tituloTextBox.Text;
            Cancion c = new Cancion(t, new TimeSpan(0, min, sec));
            album.agregarCancion(c,cual);
            Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
