namespace Cassiopeia
{
    partial class CrearCD
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Console.WriteLine("Creando CD");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CrearCD));
            this.comboBoxEstadoExterior = new System.Windows.Forms.ComboBox();
            this.comboBoxEstadoMedio = new System.Windows.Forms.ComboBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.comboBoxFormatoCD = new System.Windows.Forms.ComboBox();
            this.labelEstadoExterior = new System.Windows.Forms.Label();
            this.textBoxPais = new System.Windows.Forms.TextBox();
            this.labelEstadoMedio = new System.Windows.Forms.Label();
            this.labelFormato = new System.Windows.Forms.Label();
            this.labelPaisPublicacion = new System.Windows.Forms.Label();
            this.labelAñoPublicacion = new System.Windows.Forms.Label();
            this.textBoxAño = new System.Windows.Forms.TextBox();
            this.labelNumCanciones = new System.Windows.Forms.Label();
            this.numericUpDownNumCanciones = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownNumCanciones)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBoxEstadoExterior
            // 
            this.comboBoxEstadoExterior.FormattingEnabled = true;
            this.comboBoxEstadoExterior.Location = new System.Drawing.Point(120, 12);
            this.comboBoxEstadoExterior.Name = "comboBoxEstadoExterior";
            this.comboBoxEstadoExterior.Size = new System.Drawing.Size(121, 21);
            this.comboBoxEstadoExterior.TabIndex = 0;
            // 
            // comboBoxEstadoMedio
            // 
            this.comboBoxEstadoMedio.FormattingEnabled = true;
            this.comboBoxEstadoMedio.Location = new System.Drawing.Point(120, 58);
            this.comboBoxEstadoMedio.Name = "comboBoxEstadoMedio";
            this.comboBoxEstadoMedio.Size = new System.Drawing.Size(121, 21);
            this.comboBoxEstadoMedio.TabIndex = 1;
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(96, 234);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 2;
            this.buttonOK.Text = "Ok";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // comboBoxFormatoCD
            // 
            this.comboBoxFormatoCD.FormattingEnabled = true;
            this.comboBoxFormatoCD.Location = new System.Drawing.Point(120, 109);
            this.comboBoxFormatoCD.Name = "comboBoxFormatoCD";
            this.comboBoxFormatoCD.Size = new System.Drawing.Size(121, 21);
            this.comboBoxFormatoCD.TabIndex = 3;
            // 
            // labelEstadoExterior
            // 
            this.labelEstadoExterior.AutoSize = true;
            this.labelEstadoExterior.Location = new System.Drawing.Point(12, 15);
            this.labelEstadoExterior.Name = "labelEstadoExterior";
            this.labelEstadoExterior.Size = new System.Drawing.Size(35, 13);
            this.labelEstadoExterior.TabIndex = 4;
            this.labelEstadoExterior.Text = "label1";
            // 
            // textBoxPais
            // 
            this.textBoxPais.Location = new System.Drawing.Point(120, 145);
            this.textBoxPais.Name = "textBoxPais";
            this.textBoxPais.Size = new System.Drawing.Size(121, 20);
            this.textBoxPais.TabIndex = 5;
            // 
            // labelEstadoMedio
            // 
            this.labelEstadoMedio.AutoSize = true;
            this.labelEstadoMedio.Location = new System.Drawing.Point(12, 61);
            this.labelEstadoMedio.Name = "labelEstadoMedio";
            this.labelEstadoMedio.Size = new System.Drawing.Size(35, 13);
            this.labelEstadoMedio.TabIndex = 6;
            this.labelEstadoMedio.Text = "label2";
            // 
            // labelFormato
            // 
            this.labelFormato.AutoSize = true;
            this.labelFormato.Location = new System.Drawing.Point(12, 112);
            this.labelFormato.Name = "labelFormato";
            this.labelFormato.Size = new System.Drawing.Size(35, 13);
            this.labelFormato.TabIndex = 7;
            this.labelFormato.Text = "label3";
            // 
            // labelPaisPublicacion
            // 
            this.labelPaisPublicacion.AutoSize = true;
            this.labelPaisPublicacion.Location = new System.Drawing.Point(12, 148);
            this.labelPaisPublicacion.Name = "labelPaisPublicacion";
            this.labelPaisPublicacion.Size = new System.Drawing.Size(26, 13);
            this.labelPaisPublicacion.TabIndex = 8;
            this.labelPaisPublicacion.Text = "pais";
            // 
            // labelAñoPublicacion
            // 
            this.labelAñoPublicacion.AutoSize = true;
            this.labelAñoPublicacion.Location = new System.Drawing.Point(12, 183);
            this.labelAñoPublicacion.Name = "labelAñoPublicacion";
            this.labelAñoPublicacion.Size = new System.Drawing.Size(25, 13);
            this.labelAñoPublicacion.TabIndex = 10;
            this.labelAñoPublicacion.Text = "año";
            // 
            // textBoxAño
            // 
            this.textBoxAño.Location = new System.Drawing.Point(120, 180);
            this.textBoxAño.Name = "textBoxAño";
            this.textBoxAño.Size = new System.Drawing.Size(121, 20);
            this.textBoxAño.TabIndex = 9;
            // 
            // labelNumCanciones
            // 
            this.labelNumCanciones.AutoSize = true;
            this.labelNumCanciones.Location = new System.Drawing.Point(12, 216);
            this.labelNumCanciones.Name = "labelNumCanciones";
            this.labelNumCanciones.Size = new System.Drawing.Size(35, 13);
            this.labelNumCanciones.TabIndex = 11;
            this.labelNumCanciones.Text = "label4";
            // 
            // numericUpDownNumCanciones
            // 
            this.numericUpDownNumCanciones.Location = new System.Drawing.Point(120, 209);
            this.numericUpDownNumCanciones.Name = "numericUpDownNumCanciones";
            this.numericUpDownNumCanciones.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownNumCanciones.TabIndex = 12;
            // 
            // CrearCD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(253, 269);
            this.Controls.Add(this.numericUpDownNumCanciones);
            this.Controls.Add(this.labelNumCanciones);
            this.Controls.Add(this.labelAñoPublicacion);
            this.Controls.Add(this.textBoxAño);
            this.Controls.Add(this.labelPaisPublicacion);
            this.Controls.Add(this.labelFormato);
            this.Controls.Add(this.labelEstadoMedio);
            this.Controls.Add(this.textBoxPais);
            this.Controls.Add(this.labelEstadoExterior);
            this.Controls.Add(this.comboBoxFormatoCD);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.comboBoxEstadoMedio);
            this.Controls.Add(this.comboBoxEstadoExterior);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "CrearCD";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "CrearCD";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownNumCanciones)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxEstadoExterior;
        private System.Windows.Forms.ComboBox comboBoxEstadoMedio;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.ComboBox comboBoxFormatoCD;
        private System.Windows.Forms.Label labelEstadoExterior;
        private System.Windows.Forms.TextBox textBoxPais;
        private System.Windows.Forms.Label labelEstadoMedio;
        private System.Windows.Forms.Label labelFormato;
        private System.Windows.Forms.Label labelPaisPublicacion;
        private System.Windows.Forms.Label labelAñoPublicacion;
        private System.Windows.Forms.TextBox textBoxAño;
        private System.Windows.Forms.Label labelNumCanciones;
        private System.Windows.Forms.NumericUpDown numericUpDownNumCanciones;
    }
}