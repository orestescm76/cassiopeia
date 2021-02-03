namespace aplicacion_musica
{
    partial class ListaReproduccionUI
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
            this.listViewCanciones = new System.Windows.Forms.ListView();
            this.columnPlaying = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnDuration = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnArtista = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.nuevaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listViewCanciones
            // 
            this.listViewCanciones.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewCanciones.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnPlaying,
            this.columnArtista,
            this.columnName,
            this.columnDuration});
            this.listViewCanciones.FullRowSelect = true;
            this.listViewCanciones.HideSelection = false;
            this.listViewCanciones.Location = new System.Drawing.Point(1, 27);
            this.listViewCanciones.Name = "listViewCanciones";
            this.listViewCanciones.Size = new System.Drawing.Size(487, 371);
            this.listViewCanciones.TabIndex = 0;
            this.listViewCanciones.UseCompatibleStateImageBehavior = false;
            this.listViewCanciones.View = System.Windows.Forms.View.Details;
            this.listViewCanciones.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listViewCanciones_MouseDoubleClick);
            // 
            // columnPlaying
            // 
            this.columnPlaying.Text = "Playing";
            this.columnPlaying.Width = 82;
            // 
            // columnName
            // 
            this.columnName.Text = "Titulo";
            this.columnName.Width = 82;
            // 
            // columnDuration
            // 
            this.columnDuration.Text = "Duracion";
            this.columnDuration.Width = 84;
            // 
            // columnArtista
            // 
            this.columnArtista.Text = "artista";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nuevaToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.addToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(488, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // nuevaToolStripMenuItem
            // 
            this.nuevaToolStripMenuItem.Name = "nuevaToolStripMenuItem";
            this.nuevaToolStripMenuItem.Size = new System.Drawing.Size(51, 20);
            this.nuevaToolStripMenuItem.Text = "nueva";
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.addToolStripMenuItem.Text = "add";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(42, 20);
            this.saveToolStripMenuItem.Text = "save";
            // 
            // ListaReproduccionUI
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(488, 396);
            this.Controls.Add(this.listViewCanciones);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ListaReproduccionUI";
            this.Text = "ListaReproduccionUI";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ListaReproduccionUI_FormClosing);
            this.Load += new System.EventHandler(this.ListaReproduccionUI_Load);
            this.SizeChanged += new System.EventHandler(this.ListaReproduccionUI_SizeChanged);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.ListaReproduccionUI_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.ListaReproduccionUI_DragEnter);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listViewCanciones;
        private System.Windows.Forms.ColumnHeader columnPlaying;
        private System.Windows.Forms.ColumnHeader columnName;
        private System.Windows.Forms.ColumnHeader columnDuration;
        private System.Windows.Forms.ColumnHeader columnArtista;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem nuevaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
    }
}