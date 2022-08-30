namespace Cassiopeia.src.Forms
{
    partial class CreateSong
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreateSong));
            this.tituloTextBox = new System.Windows.Forms.TextBox();
            this.secsTextBox = new System.Windows.Forms.TextBox();
            this.minTextBox = new System.Windows.Forms.TextBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.labelColon = new System.Windows.Forms.Label();
            this.labelTituloCancion = new System.Windows.Forms.Label();
            this.labelMinutosSegundos = new System.Windows.Forms.Label();
            this.buttonCancelar = new System.Windows.Forms.Button();
            this.buttonLongSong = new System.Windows.Forms.Button();
            this.labelNumPartes = new System.Windows.Forms.Label();
            this.textBoxNumPartes = new System.Windows.Forms.TextBox();
            this.checkBoxBonus = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // tituloTextBox
            // 
            this.tituloTextBox.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.tituloTextBox.Location = new System.Drawing.Point(13, 69);
            this.tituloTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tituloTextBox.Name = "tituloTextBox";
            this.tituloTextBox.Size = new System.Drawing.Size(353, 25);
            this.tituloTextBox.TabIndex = 0;
            this.tituloTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tituloTextBox_KeyDown);
            // 
            // secsTextBox
            // 
            this.secsTextBox.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.secsTextBox.Location = new System.Drawing.Point(221, 140);
            this.secsTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.secsTextBox.Name = "secsTextBox";
            this.secsTextBox.Size = new System.Drawing.Size(145, 25);
            this.secsTextBox.TabIndex = 2;
            // 
            // minTextBox
            // 
            this.minTextBox.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.minTextBox.Location = new System.Drawing.Point(13, 140);
            this.minTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.minTextBox.Name = "minTextBox";
            this.minTextBox.Size = new System.Drawing.Size(144, 25);
            this.minTextBox.TabIndex = 1;
            // 
            // buttonOK
            // 
            this.buttonOK.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.buttonOK.Location = new System.Drawing.Point(14, 204);
            this.buttonOK.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(147, 37);
            this.buttonOK.TabIndex = 3;
            this.buttonOK.Text = "añadir9";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.button1_Click);
            // 
            // labelColon
            // 
            this.labelColon.AutoSize = true;
            this.labelColon.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.labelColon.Location = new System.Drawing.Point(179, 140);
            this.labelColon.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelColon.Name = "labelColon";
            this.labelColon.Size = new System.Drawing.Size(20, 29);
            this.labelColon.TabIndex = 4;
            this.labelColon.Text = ":";
            // 
            // labelTituloCancion
            // 
            this.labelTituloCancion.AutoSize = true;
            this.labelTituloCancion.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelTituloCancion.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.labelTituloCancion.Location = new System.Drawing.Point(9, 12);
            this.labelTituloCancion.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelTituloCancion.Name = "labelTituloCancion";
            this.labelTituloCancion.Padding = new System.Windows.Forms.Padding(6, 6, 18, 6);
            this.labelTituloCancion.Size = new System.Drawing.Size(133, 29);
            this.labelTituloCancion.TabIndex = 5;
            this.labelTituloCancion.Text = "introduceTitulo11";
            this.labelTituloCancion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelMinutosSegundos
            // 
            this.labelMinutosSegundos.AutoSize = true;
            this.labelMinutosSegundos.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelMinutosSegundos.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.labelMinutosSegundos.Location = new System.Drawing.Point(144, 101);
            this.labelMinutosSegundos.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelMinutosSegundos.Name = "labelMinutosSegundos";
            this.labelMinutosSegundos.Padding = new System.Windows.Forms.Padding(12, 9, 12, 0);
            this.labelMinutosSegundos.Size = new System.Drawing.Size(74, 22);
            this.labelMinutosSegundos.TabIndex = 5;
            this.labelMinutosSegundos.Text = "mm:ss12";
            this.labelMinutosSegundos.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonCancelar
            // 
            this.buttonCancelar.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.buttonCancelar.Location = new System.Drawing.Point(223, 204);
            this.buttonCancelar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonCancelar.Name = "buttonCancelar";
            this.buttonCancelar.Size = new System.Drawing.Size(146, 37);
            this.buttonCancelar.TabIndex = 6;
            this.buttonCancelar.Text = "cancelar10";
            this.buttonCancelar.UseVisualStyleBackColor = true;
            this.buttonCancelar.Click += new System.EventHandler(this.buttonCancelar_Click);
            // 
            // buttonLongSong
            // 
            this.buttonLongSong.AutoSize = true;
            this.buttonLongSong.Location = new System.Drawing.Point(221, 12);
            this.buttonLongSong.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonLongSong.Name = "buttonLongSong";
            this.buttonLongSong.Size = new System.Drawing.Size(145, 29);
            this.buttonLongSong.TabIndex = 7;
            this.buttonLongSong.Text = "eslarga?";
            this.buttonLongSong.UseVisualStyleBackColor = true;
            this.buttonLongSong.Click += new System.EventHandler(this.esLarga_Click);
            // 
            // labelNumPartes
            // 
            this.labelNumPartes.AutoSize = true;
            this.labelNumPartes.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelNumPartes.Location = new System.Drawing.Point(153, 110);
            this.labelNumPartes.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelNumPartes.Name = "labelNumPartes";
            this.labelNumPartes.Size = new System.Drawing.Size(65, 13);
            this.labelNumPartes.TabIndex = 8;
            this.labelNumPartes.Text = "num partes";
            // 
            // textBoxNumPartes
            // 
            this.textBoxNumPartes.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBoxNumPartes.Location = new System.Drawing.Point(133, 140);
            this.textBoxNumPartes.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.textBoxNumPartes.Name = "textBoxNumPartes";
            this.textBoxNumPartes.Size = new System.Drawing.Size(103, 25);
            this.textBoxNumPartes.TabIndex = 9;
            // 
            // checkBoxBonus
            // 
            this.checkBoxBonus.AutoSize = true;
            this.checkBoxBonus.Location = new System.Drawing.Point(14, 177);
            this.checkBoxBonus.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.checkBoxBonus.Name = "checkBoxBonus";
            this.checkBoxBonus.Size = new System.Drawing.Size(59, 19);
            this.checkBoxBonus.TabIndex = 10;
            this.checkBoxBonus.Text = "bonus";
            this.checkBoxBonus.UseVisualStyleBackColor = true;
            // 
            // CreateSong
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 253);
            this.Controls.Add(this.checkBoxBonus);
            this.Controls.Add(this.textBoxNumPartes);
            this.Controls.Add(this.labelNumPartes);
            this.Controls.Add(this.buttonLongSong);
            this.Controls.Add(this.buttonCancelar);
            this.Controls.Add(this.labelTituloCancion);
            this.Controls.Add(this.labelMinutosSegundos);
            this.Controls.Add(this.labelColon);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.minTextBox);
            this.Controls.Add(this.secsTextBox);
            this.Controls.Add(this.tituloTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.Name = "CreateSong";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "agregarCancion";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.agregarCancion_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tituloTextBox;
        private System.Windows.Forms.TextBox secsTextBox;
        private System.Windows.Forms.TextBox minTextBox;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Label labelColon;
        private System.Windows.Forms.Label labelTituloCancion;
        private System.Windows.Forms.Label labelMinutosSegundos;
        private System.Windows.Forms.Button buttonCancelar;
        private System.Windows.Forms.Button buttonLongSong;
        private System.Windows.Forms.Label labelNumPartes;
        private System.Windows.Forms.TextBox textBoxNumPartes;
        private System.Windows.Forms.CheckBox checkBoxBonus;
    }
}