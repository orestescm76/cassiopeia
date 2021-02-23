namespace Cassiopea.src.Forms
{
    partial class SpotifyResults
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SpotifyResults));
            this.labelResultado = new System.Windows.Forms.Label();
            this.labelAyuda = new System.Windows.Forms.Label();
            this.buttonCancelar = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.listViewResultadoBusqueda = new System.Windows.Forms.ListView();
            this.columnaNum = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnaArtista = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnaNombre = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnaYear = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnaNumSongs = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // labelResultado
            // 
            this.labelResultado.AutoSize = true;
            this.labelResultado.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelResultado.Location = new System.Drawing.Point(12, 9);
            this.labelResultado.Name = "labelResultado";
            this.labelResultado.Size = new System.Drawing.Size(50, 20);
            this.labelResultado.TabIndex = 1;
            this.labelResultado.Text = "label1";
            // 
            // labelAyuda
            // 
            this.labelAyuda.AutoSize = true;
            this.labelAyuda.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAyuda.Location = new System.Drawing.Point(12, 49);
            this.labelAyuda.Name = "labelAyuda";
            this.labelAyuda.Size = new System.Drawing.Size(298, 20);
            this.labelAyuda.TabIndex = 2;
            this.labelAyuda.Text = "selecciona los álbumes que quieres agregar";
            // 
            // buttonCancelar
            // 
            this.buttonCancelar.Location = new System.Drawing.Point(12, 296);
            this.buttonCancelar.Name = "buttonCancelar";
            this.buttonCancelar.Size = new System.Drawing.Size(75, 26);
            this.buttonCancelar.TabIndex = 3;
            this.buttonCancelar.Text = "cancelar";
            this.buttonCancelar.UseVisualStyleBackColor = true;
            this.buttonCancelar.Click += new System.EventHandler(this.buttonCancelar_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(480, 296);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(83, 26);
            this.buttonOK.TabIndex = 4;
            this.buttonOK.Text = "agregar";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // listViewResultadoBusqueda
            // 
            this.listViewResultadoBusqueda.AllowColumnReorder = true;
            this.listViewResultadoBusqueda.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnaNum,
            this.columnaArtista,
            this.columnaNombre,
            this.columnaYear,
            this.columnaNumSongs});
            this.listViewResultadoBusqueda.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listViewResultadoBusqueda.FullRowSelect = true;
            this.listViewResultadoBusqueda.HideSelection = false;
            this.listViewResultadoBusqueda.Location = new System.Drawing.Point(16, 73);
            this.listViewResultadoBusqueda.Name = "listViewResultadoBusqueda";
            this.listViewResultadoBusqueda.Size = new System.Drawing.Size(547, 217);
            this.listViewResultadoBusqueda.TabIndex = 5;
            this.listViewResultadoBusqueda.UseCompatibleStateImageBehavior = false;
            this.listViewResultadoBusqueda.View = System.Windows.Forms.View.Details;
            this.listViewResultadoBusqueda.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listViewResultadoBusqueda_ColumnClick);
            // 
            // columnaNum
            // 
            this.columnaNum.Text = "#";
            this.columnaNum.Width = 24;
            // 
            // columnaArtista
            // 
            this.columnaArtista.Text = "artista";
            this.columnaArtista.Width = 135;
            // 
            // columnaNombre
            // 
            this.columnaNombre.Text = "nombre";
            this.columnaNombre.Width = 135;
            // 
            // columnaYear
            // 
            this.columnaYear.Text = "año";
            this.columnaYear.Width = 48;
            // 
            // columnaNumSongs
            // 
            this.columnaNumSongs.Text = "num canciones";
            this.columnaNumSongs.Width = 72;
            // 
            // resultadoSpotify
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(575, 334);
            this.Controls.Add(this.listViewResultadoBusqueda);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancelar);
            this.Controls.Add(this.labelAyuda);
            this.Controls.Add(this.labelResultado);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "resultadoSpotify";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "resultadoSpotify";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label labelResultado;
        private System.Windows.Forms.Label labelAyuda;
        private System.Windows.Forms.Button buttonCancelar;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.ListView listViewResultadoBusqueda;
        private System.Windows.Forms.ColumnHeader columnaNum;
        private System.Windows.Forms.ColumnHeader columnaArtista;
        private System.Windows.Forms.ColumnHeader columnaNombre;
        private System.Windows.Forms.ColumnHeader columnaYear;
        private System.Windows.Forms.ColumnHeader columnaNumSongs;
    }
}