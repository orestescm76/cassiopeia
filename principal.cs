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
    public partial class principal : Form
    {
        public principal()
        {
            InitializeComponent();
            ponerTextos();

        }
        private void ponerTextos()
        {
            Text = Programa.textosLocal[0];
            archivoMenuItem1.Text = Programa.textosLocal[1];
            opcionesToolStripMenuItem.Text = Programa.textosLocal[2];

            salirToolStripMenuItem.Text = Programa.textosLocal[3];
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void principal_Load(object sender, EventArgs e)
        {
            foreach (var idioma in Programa.codigosIdiomas)
            {
                ToolStripItem subIdioma = new ToolStripMenuItem(idioma);
                subIdioma.Click += new EventHandler(SubIdioma_Click);
                opcionesToolStripMenuItem.DropDownItems.Add(subIdioma);
            }
        }

        private void SubIdioma_Click(object sender, EventArgs e)
        {
            var menu = sender as ToolStripMenuItem;
            string codIdioma = menu.Text;
            Programa.cambiarIdioma(codIdioma);
            ponerTextos();
        }

        private void opciones2ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
