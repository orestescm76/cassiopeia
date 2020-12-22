using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace aplicacion_musica.src.Forms
{
    public partial class ConfigForm : Form
    {

        public ConfigForm()
        {
            InitializeComponent();
        }

        private void ConfigForm_Load(object sender, EventArgs e)
        {
            label1.Show();
            
            PonerTextos();
            label1.Location = new Point(290 - (label1.Size.Width / 2), groupBoxRaiz.Size.Height / 2);
        }
        private void PonerTextos()
        {
            Text = Programa.textosLocal.GetString("configuracion");
            treeViewConfiguracion.Nodes[0].Text = Programa.textosLocal.GetString("idioma");
            label1.Text = Programa.textosLocal.GetString("seleccione_opcion");
            buttonAplicar.Text = Programa.textosLocal.GetString("aplicar");
            buttonOK.Text = Programa.textosLocal.GetString("aceptar");
            buttonCancelar.Text = Programa.textosLocal.GetString("cancelar");
            CargarPagina();
        }
        private void CargarIdiomas()
        {
            RadioButton[] radioButtonsIdiomas = new RadioButton[Programa.NumIdiomas];
            PictureBox[] pictureBoxesIdiomas = new PictureBox[Programa.NumIdiomas];
            groupBoxRaiz.Text = Programa.textosLocal.GetString("cambiar_idioma");
            int y = 44;
            for (int i = 0; i < Programa.NumIdiomas; i++)
            {
                radioButtonsIdiomas[i] = new RadioButton();
                radioButtonsIdiomas[i].Location = new Point(44, y);
                radioButtonsIdiomas[i].Text = Programa.idiomas[i];
                if (radioButtonsIdiomas[i].Text == Config.Idioma)
                    radioButtonsIdiomas[i].Checked = true;
                radioButtonsIdiomas[i].Font = new Font("Segoe UI", 9);
                pictureBoxesIdiomas[i] = new PictureBox();
                pictureBoxesIdiomas[i].Location = new Point(6, y);
                pictureBoxesIdiomas[i].Size = new Size(32, 32);
                pictureBoxesIdiomas[i].SizeMode = PictureBoxSizeMode.StretchImage;
                CultureInfo nombreIdioma = new CultureInfo(Programa.idiomas[i]);
                radioButtonsIdiomas[i].Text = nombreIdioma.NativeName;
                switch (Programa.idiomas[i])
                {
                    case "es":
                        pictureBoxesIdiomas[i].Image = Properties.Resources.es;
                        break;
                    case "ca":
                        pictureBoxesIdiomas[i].Image = Properties.Resources.ca;
                        break;
                    case "en":
                        pictureBoxesIdiomas[i].Image = Properties.Resources.en;
                        break;
                    case "el":
                        pictureBoxesIdiomas[i].Image = Properties.Resources.el;
                        break;
                }
                groupBoxRaiz.Controls.Add(radioButtonsIdiomas[i]);
                groupBoxRaiz.Controls.Add(pictureBoxesIdiomas[i]);
                y += 35;
                radioButtonsIdiomas[i].Show();
                pictureBoxesIdiomas[i].Show();
            }
        }
        private void Limpiar()
        {
            label1.Show();
        }
        private void CargarPagina()
        {
            label1.Hide();
            groupBoxRaiz.Controls.Clear();
            string seleccion = string.Empty;
            try
            {
                seleccion = treeViewConfiguracion.SelectedNode.Tag.ToString();
            }
            catch (NullReferenceException)
            {
                System.Diagnostics.Debug.WriteLine("fuck off");
            }

            switch (seleccion)
            {
                case "idioma":
                    CargarIdiomas();
                    break;
                default:
                    groupBoxRaiz.Controls.Add(label1);
                    label1.Show();
                    break;
            }

        }
        private void groupBoxIdioma_Enter(object sender, EventArgs e)
        {

        }

        private void treeViewConfiguracion_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            CargarPagina();
        }
        private void Aplicar()
        {
            
            PonerTextos();
        }
        private void buttonAplicar_Click(object sender, EventArgs e)
        {
            Aplicar();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Aplicar();
            Close();
        }
    }
}
