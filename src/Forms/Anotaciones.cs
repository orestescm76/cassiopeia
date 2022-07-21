﻿using Cassiopeia.src.Classes;
using System;
using System.Windows.Forms;

namespace Cassiopeia.src.Forms
{
    public partial class Anotaciones : Form
    {
        CompactDisc cd;
        public Anotaciones(ref CompactDisc cd)
        {
            InitializeComponent();
            this.cd = cd;
            textBox1.Lines = cd.Anotaciones;
            buttonOk.Text = Kernel.GetText("hecho");
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            cd.Anotaciones = textBox1.Lines;
            Log.Instance.PrintMessage("Guardado " + cd.Anotaciones.Length + " bytes", MessageType.Correct);
            Kernel.SetSaveMark();
            Dispose();
        }
    }
}
