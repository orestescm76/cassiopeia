namespace aplicacion_ipo
{
    partial class agregarAlbum
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
            this.artistaTextBox = new System.Windows.Forms.TextBox();
            this.tituloTextBox = new System.Windows.Forms.TextBox();
            this.yearTextBox = new System.Windows.Forms.TextBox();
            this.numCancionesTextBox = new System.Windows.Forms.TextBox();
            this.add = new System.Windows.Forms.Button();
            this.labelArtista = new System.Windows.Forms.Label();
            this.labelAño = new System.Windows.Forms.Label();
            this.labelTitulo = new System.Windows.Forms.Label();
            this.labelNumCanciones = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.labelGenero = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // artistaTextBox
            // 
            this.artistaTextBox.Location = new System.Drawing.Point(143, 55);
            this.artistaTextBox.Name = "artistaTextBox";
            this.artistaTextBox.Size = new System.Drawing.Size(100, 20);
            this.artistaTextBox.TabIndex = 0;
            // 
            // tituloTextBox
            // 
            this.tituloTextBox.Location = new System.Drawing.Point(143, 81);
            this.tituloTextBox.Name = "tituloTextBox";
            this.tituloTextBox.Size = new System.Drawing.Size(100, 20);
            this.tituloTextBox.TabIndex = 1;
            // 
            // yearTextBox
            // 
            this.yearTextBox.Location = new System.Drawing.Point(143, 107);
            this.yearTextBox.Name = "yearTextBox";
            this.yearTextBox.Size = new System.Drawing.Size(100, 20);
            this.yearTextBox.TabIndex = 2;
            // 
            // numCancionesTextBox
            // 
            this.numCancionesTextBox.Location = new System.Drawing.Point(143, 136);
            this.numCancionesTextBox.Name = "numCancionesTextBox";
            this.numCancionesTextBox.Size = new System.Drawing.Size(100, 20);
            this.numCancionesTextBox.TabIndex = 3;
            this.numCancionesTextBox.TextChanged += new System.EventHandler(this.numCancionesTextBox_TextChanged);
            // 
            // add
            // 
            this.add.Location = new System.Drawing.Point(78, 215);
            this.add.Name = "add";
            this.add.Size = new System.Drawing.Size(102, 35);
            this.add.TabIndex = 4;
            this.add.Text = "añadir9";
            this.add.UseVisualStyleBackColor = true;
            this.add.Click += new System.EventHandler(this.add_Click);
            // 
            // labelArtista
            // 
            this.labelArtista.AutoSize = true;
            this.labelArtista.Location = new System.Drawing.Point(41, 58);
            this.labelArtista.Name = "labelArtista";
            this.labelArtista.Size = new System.Drawing.Size(41, 13);
            this.labelArtista.TabIndex = 5;
            this.labelArtista.Text = "artista4";
            this.labelArtista.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelAño
            // 
            this.labelAño.AutoSize = true;
            this.labelAño.Location = new System.Drawing.Point(41, 110);
            this.labelAño.Name = "labelAño";
            this.labelAño.Size = new System.Drawing.Size(31, 13);
            this.labelAño.TabIndex = 6;
            this.labelAño.Text = "año6";
            this.labelAño.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelTitulo
            // 
            this.labelTitulo.AutoSize = true;
            this.labelTitulo.Location = new System.Drawing.Point(41, 84);
            this.labelTitulo.Name = "labelTitulo";
            this.labelTitulo.Size = new System.Drawing.Size(35, 13);
            this.labelTitulo.TabIndex = 7;
            this.labelTitulo.Text = "titulo5";
            this.labelTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelNumCanciones
            // 
            this.labelNumCanciones.AutoSize = true;
            this.labelNumCanciones.Location = new System.Drawing.Point(21, 139);
            this.labelNumCanciones.Name = "labelNumCanciones";
            this.labelNumCanciones.Size = new System.Drawing.Size(83, 13);
            this.labelNumCanciones.TabIndex = 8;
            this.labelNumCanciones.Text = "numCanciones7";
            this.labelNumCanciones.Click += new System.EventHandler(this.labelNumCanciones_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(143, 162);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(100, 21);
            this.comboBox1.TabIndex = 9;
            // 
            // labelGenero
            // 
            this.labelGenero.AutoSize = true;
            this.labelGenero.Location = new System.Drawing.Point(36, 165);
            this.labelGenero.Name = "labelGenero";
            this.labelGenero.Size = new System.Drawing.Size(46, 13);
            this.labelGenero.TabIndex = 10;
            this.labelGenero.Text = "genero8";
            // 
            // agregarAlbum
            // 
            this.ClientSize = new System.Drawing.Size(255, 262);
            this.Controls.Add(this.labelGenero);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.labelNumCanciones);
            this.Controls.Add(this.labelTitulo);
            this.Controls.Add(this.labelAño);
            this.Controls.Add(this.labelArtista);
            this.Controls.Add(this.add);
            this.Controls.Add(this.numCancionesTextBox);
            this.Controls.Add(this.yearTextBox);
            this.Controls.Add(this.tituloTextBox);
            this.Controls.Add(this.artistaTextBox);
            this.Name = "agregarAlbum";
            this.Text = "agregarAlbum4";
            this.Load += new System.EventHandler(this.agregarAlbum_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox artistaTextBox;
        private System.Windows.Forms.TextBox tituloTextBox;
        private System.Windows.Forms.TextBox yearTextBox;
        private System.Windows.Forms.TextBox numCancionesTextBox;
        private System.Windows.Forms.Button add;
        private System.Windows.Forms.Label labelArtista;
        private System.Windows.Forms.Label labelAño;
        private System.Windows.Forms.Label labelTitulo;
        private System.Windows.Forms.Label labelNumCanciones;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label labelGenero;
    }
}