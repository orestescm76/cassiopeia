namespace aplicacion_musica.src.Forms
{
    partial class VisorLyrics
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
            this.textBoxLyrics = new System.Windows.Forms.TextBox();
            this.buttonEditar = new System.Windows.Forms.Button();
            this.buttonLimpiar = new System.Windows.Forms.Button();
            this.buttonBuscar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBoxLyrics
            // 
            this.textBoxLyrics.AcceptsReturn = true;
            this.textBoxLyrics.AllowDrop = true;
            this.textBoxLyrics.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textBoxLyrics.Location = new System.Drawing.Point(12, 12);
            this.textBoxLyrics.Multiline = true;
            this.textBoxLyrics.Name = "textBoxLyrics";
            this.textBoxLyrics.ReadOnly = true;
            this.textBoxLyrics.Size = new System.Drawing.Size(398, 526);
            this.textBoxLyrics.TabIndex = 0;
            // 
            // buttonEditar
            // 
            this.buttonEditar.Location = new System.Drawing.Point(170, 544);
            this.buttonEditar.Name = "buttonEditar";
            this.buttonEditar.Size = new System.Drawing.Size(75, 34);
            this.buttonEditar.TabIndex = 1;
            this.buttonEditar.Text = "buttonEditar";
            this.buttonEditar.UseVisualStyleBackColor = true;
            this.buttonEditar.Click += new System.EventHandler(this.buttonEditar_Click);
            // 
            // buttonLimpiar
            // 
            this.buttonLimpiar.Location = new System.Drawing.Point(12, 544);
            this.buttonLimpiar.Name = "buttonLimpiar";
            this.buttonLimpiar.Size = new System.Drawing.Size(75, 34);
            this.buttonLimpiar.TabIndex = 2;
            this.buttonLimpiar.Text = "buttonLimpiar";
            this.buttonLimpiar.UseVisualStyleBackColor = true;
            // 
            // buttonBuscar
            // 
            this.buttonBuscar.Location = new System.Drawing.Point(335, 544);
            this.buttonBuscar.Name = "buttonBuscar";
            this.buttonBuscar.Size = new System.Drawing.Size(75, 34);
            this.buttonBuscar.TabIndex = 3;
            this.buttonBuscar.Text = "button3";
            this.buttonBuscar.UseVisualStyleBackColor = true;
            // 
            // VisorLyrics
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(422, 645);
            this.Controls.Add(this.buttonBuscar);
            this.Controls.Add(this.buttonLimpiar);
            this.Controls.Add(this.buttonEditar);
            this.Controls.Add(this.textBoxLyrics);
            this.Name = "VisorLyrics";
            this.Text = "VisorLyrics";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxLyrics;
        private System.Windows.Forms.Button buttonEditar;
        private System.Windows.Forms.Button buttonLimpiar;
        private System.Windows.Forms.Button buttonBuscar;
    }
}