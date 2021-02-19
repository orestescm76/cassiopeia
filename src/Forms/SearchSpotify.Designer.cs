namespace aplicacion_musica.src.Forms
{
    partial class SearchSpotify
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchSpotify));
            this.textBoxBusqueda = new System.Windows.Forms.TextBox();
            this.buttonOk = new System.Windows.Forms.Button();
            this.labelBusqueda = new System.Windows.Forms.Label();
            this.textBoxURISpotify = new System.Windows.Forms.TextBox();
            this.labelAlternativa = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBoxBusqueda
            // 
            this.textBoxBusqueda.Location = new System.Drawing.Point(12, 39);
            this.textBoxBusqueda.Name = "textBoxBusqueda";
            this.textBoxBusqueda.Size = new System.Drawing.Size(330, 20);
            this.textBoxBusqueda.TabIndex = 0;
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(146, 125);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 0;
            this.buttonOk.Text = "Buscar";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // labelBusqueda
            // 
            this.labelBusqueda.AutoSize = true;
            this.labelBusqueda.Location = new System.Drawing.Point(13, 13);
            this.labelBusqueda.Name = "labelBusqueda";
            this.labelBusqueda.Size = new System.Drawing.Size(303, 13);
            this.labelBusqueda.TabIndex = 2;
            this.labelBusqueda.Text = "Introduce lo que vayas a buscar en Spotify (máx 20 resultados)";
            // 
            // textBoxURISpotify
            // 
            this.textBoxURISpotify.Location = new System.Drawing.Point(12, 99);
            this.textBoxURISpotify.Name = "textBoxURISpotify";
            this.textBoxURISpotify.Size = new System.Drawing.Size(330, 20);
            this.textBoxURISpotify.TabIndex = 3;
            // 
            // labelAlternativa
            // 
            this.labelAlternativa.AutoSize = true;
            this.labelAlternativa.Location = new System.Drawing.Point(12, 73);
            this.labelAlternativa.Name = "labelAlternativa";
            this.labelAlternativa.Size = new System.Drawing.Size(60, 13);
            this.labelAlternativa.TabIndex = 4;
            this.labelAlternativa.Text = "o introduce";
            // 
            // busquedaSpotify
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(368, 160);
            this.Controls.Add(this.labelAlternativa);
            this.Controls.Add(this.textBoxURISpotify);
            this.Controls.Add(this.labelBusqueda);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.textBoxBusqueda);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "busquedaSpotify";
            this.Text = "Búsqueda en Spotify";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxBusqueda;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Label labelBusqueda;
        private System.Windows.Forms.TextBox textBoxURISpotify;
        private System.Windows.Forms.Label labelAlternativa;
    }
}