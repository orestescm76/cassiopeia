namespace aplicacion_ipo
{
    partial class agregarCancion
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(agregarCancion));
            this.tituloTextBox = new System.Windows.Forms.TextBox();
            this.secsTextBox = new System.Windows.Forms.TextBox();
            this.minTextBox = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.labelTituloCancion = new System.Windows.Forms.Label();
            this.labelMinutosSegundos = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tituloTextBox
            // 
            this.tituloTextBox.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tituloTextBox.Location = new System.Drawing.Point(7, 48);
            this.tituloTextBox.Name = "tituloTextBox";
            this.tituloTextBox.Size = new System.Drawing.Size(228, 25);
            this.tituloTextBox.TabIndex = 0;
            this.tituloTextBox.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            this.tituloTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tituloTextBox_KeyDown);
            // 
            // secsTextBox
            // 
            this.secsTextBox.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.secsTextBox.Location = new System.Drawing.Point(138, 103);
            this.secsTextBox.Name = "secsTextBox";
            this.secsTextBox.Size = new System.Drawing.Size(97, 25);
            this.secsTextBox.TabIndex = 2;
            // 
            // minTextBox
            // 
            this.minTextBox.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.minTextBox.Location = new System.Drawing.Point(7, 103);
            this.minTextBox.Name = "minTextBox";
            this.minTextBox.Size = new System.Drawing.Size(97, 25);
            this.minTextBox.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(7, 145);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(108, 32);
            this.button1.TabIndex = 3;
            this.button1.Text = "añadir9";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(117, 96);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(20, 29);
            this.label2.TabIndex = 4;
            this.label2.Text = ":";
            // 
            // labelTituloCancion
            // 
            this.labelTituloCancion.AutoSize = true;
            this.labelTituloCancion.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTituloCancion.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.labelTituloCancion.Location = new System.Drawing.Point(9, 13);
            this.labelTituloCancion.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.labelTituloCancion.Name = "labelTituloCancion";
            this.labelTituloCancion.Padding = new System.Windows.Forms.Padding(5, 5, 15, 5);
            this.labelTituloCancion.Size = new System.Drawing.Size(129, 27);
            this.labelTituloCancion.TabIndex = 5;
            this.labelTituloCancion.Text = "introduceTitulo11";
            this.labelTituloCancion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelMinutosSegundos
            // 
            this.labelMinutosSegundos.AutoSize = true;
            this.labelMinutosSegundos.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMinutosSegundos.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.labelMinutosSegundos.Location = new System.Drawing.Point(85, 75);
            this.labelMinutosSegundos.Name = "labelMinutosSegundos";
            this.labelMinutosSegundos.Padding = new System.Windows.Forms.Padding(10, 8, 10, 0);
            this.labelMinutosSegundos.Size = new System.Drawing.Size(70, 21);
            this.labelMinutosSegundos.TabIndex = 5;
            this.labelMinutosSegundos.Text = "mm:ss12";
            this.labelMinutosSegundos.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(127, 145);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(108, 32);
            this.button2.TabIndex = 6;
            this.button2.Text = "cancelar10";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // agregarCancion
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(252, 196);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.labelTituloCancion);
            this.Controls.Add(this.labelMinutosSegundos);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.minTextBox);
            this.Controls.Add(this.secsTextBox);
            this.Controls.Add(this.tituloTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "agregarCancion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "agregarCancion";
            this.Load += new System.EventHandler(this.agregarCancion_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.agregarCancion_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tituloTextBox;
        private System.Windows.Forms.TextBox secsTextBox;
        private System.Windows.Forms.TextBox minTextBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelTituloCancion;
        private System.Windows.Forms.Label labelMinutosSegundos;
        private System.Windows.Forms.Button button2;
    }
}