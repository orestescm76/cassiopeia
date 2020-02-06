namespace aplicacion_musica
{
    partial class principal
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(principal));
            this.menuStrip2 = new System.Windows.Forms.MenuStrip();
            this.archivoMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.agregarAlbumToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.abrirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buscarEnSpotifyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.guardarcomo = new System.Windows.Forms.ToolStripMenuItem();
            this.salirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.seleccionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.borrarseleccionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.adminMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.generarAlbumToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.opcionesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.acercaDeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.vistaAlbumes = new System.Windows.Forms.ListView();
            this.artista = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.titulo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.year = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.duracion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.genero = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.refrescarButton = new System.Windows.Forms.Button();
            this.banderaImageBox = new System.Windows.Forms.PictureBox();
            this.borrarButton = new System.Windows.Forms.Button();
            this.menuStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.banderaImageBox)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip2
            // 
            this.menuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.archivoMenuItem1,
            this.seleccionToolStripMenuItem,
            this.adminMenu,
            this.opcionesToolStripMenuItem,
            this.acercaDeToolStripMenuItem});
            this.menuStrip2.Location = new System.Drawing.Point(0, 0);
            this.menuStrip2.Name = "menuStrip2";
            this.menuStrip2.Size = new System.Drawing.Size(759, 24);
            this.menuStrip2.TabIndex = 0;
            this.menuStrip2.Text = "menuStrip2";
            // 
            // archivoMenuItem1
            // 
            this.archivoMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.agregarAlbumToolStripMenuItem,
            this.abrirToolStripMenuItem,
            this.buscarEnSpotifyToolStripMenuItem,
            this.guardarcomo,
            this.salirToolStripMenuItem});
            this.archivoMenuItem1.Name = "archivoMenuItem1";
            this.archivoMenuItem1.Size = new System.Drawing.Size(64, 20);
            this.archivoMenuItem1.Text = "archivo1";
            this.archivoMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // agregarAlbumToolStripMenuItem
            // 
            this.agregarAlbumToolStripMenuItem.Name = "agregarAlbumToolStripMenuItem";
            this.agregarAlbumToolStripMenuItem.Size = new System.Drawing.Size(249, 22);
            this.agregarAlbumToolStripMenuItem.Text = "agregarAlbum3";
            this.agregarAlbumToolStripMenuItem.Click += new System.EventHandler(this.agregarAlbumToolStripMenuItem_Click);
            // 
            // abrirToolStripMenuItem
            // 
            this.abrirToolStripMenuItem.Name = "abrirToolStripMenuItem";
            this.abrirToolStripMenuItem.Size = new System.Drawing.Size(249, 22);
            this.abrirToolStripMenuItem.Text = "abrirDesdeFichero14";
            this.abrirToolStripMenuItem.Click += new System.EventHandler(this.abrirToolStripMenuItem_Click);
            // 
            // buscarEnSpotifyToolStripMenuItem
            // 
            this.buscarEnSpotifyToolStripMenuItem.Name = "buscarEnSpotifyToolStripMenuItem";
            this.buscarEnSpotifyToolStripMenuItem.Size = new System.Drawing.Size(249, 22);
            this.buscarEnSpotifyToolStripMenuItem.Text = "Buscar en Spotify";
            this.buscarEnSpotifyToolStripMenuItem.Click += new System.EventHandler(this.buscarEnSpotifyToolStripMenuItem_Click);
            // 
            // guardarcomo
            // 
            this.guardarcomo.Name = "guardarcomo";
            this.guardarcomo.Size = new System.Drawing.Size(249, 22);
            this.guardarcomo.Text = "guardarComoToolStripMenuItem";
            this.guardarcomo.Click += new System.EventHandler(this.guardarcomo_Click);
            // 
            // salirToolStripMenuItem
            // 
            this.salirToolStripMenuItem.Name = "salirToolStripMenuItem";
            this.salirToolStripMenuItem.Size = new System.Drawing.Size(249, 22);
            this.salirToolStripMenuItem.Text = "salirult";
            this.salirToolStripMenuItem.Click += new System.EventHandler(this.salirToolStripMenuItem_Click);
            // 
            // seleccionToolStripMenuItem
            // 
            this.seleccionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.borrarseleccionToolStripMenuItem});
            this.seleccionToolStripMenuItem.Name = "seleccionToolStripMenuItem";
            this.seleccionToolStripMenuItem.Size = new System.Drawing.Size(68, 20);
            this.seleccionToolStripMenuItem.Text = "seleccion";
            // 
            // borrarseleccionToolStripMenuItem
            // 
            this.borrarseleccionToolStripMenuItem.Name = "borrarseleccionToolStripMenuItem";
            this.borrarseleccionToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.borrarseleccionToolStripMenuItem.Text = "borrar_seleccion";
            this.borrarseleccionToolStripMenuItem.Click += new System.EventHandler(this.borrarseleccionToolStripMenuItem_Click);
            // 
            // adminMenu
            // 
            this.adminMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.generarAlbumToolStripMenuItem});
            this.adminMenu.Name = "adminMenu";
            this.adminMenu.Size = new System.Drawing.Size(79, 20);
            this.adminMenu.Text = "administrar";
            // 
            // generarAlbumToolStripMenuItem
            // 
            this.generarAlbumToolStripMenuItem.Name = "generarAlbumToolStripMenuItem";
            this.generarAlbumToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.generarAlbumToolStripMenuItem.Text = "generarAlbum";
            this.generarAlbumToolStripMenuItem.Click += new System.EventHandler(this.generarAlbumToolStripMenuItem_Click);
            // 
            // opcionesToolStripMenuItem
            // 
            this.opcionesToolStripMenuItem.Name = "opcionesToolStripMenuItem";
            this.opcionesToolStripMenuItem.Size = new System.Drawing.Size(110, 20);
            this.opcionesToolStripMenuItem.Text = "opcionesIdioma2";
            // 
            // acercaDeToolStripMenuItem
            // 
            this.acercaDeToolStripMenuItem.Name = "acercaDeToolStripMenuItem";
            this.acercaDeToolStripMenuItem.Size = new System.Drawing.Size(67, 20);
            this.acercaDeToolStripMenuItem.Text = "acercaDe";
            this.acercaDeToolStripMenuItem.Click += new System.EventHandler(this.acercaDeToolStripMenuItem_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // vistaAlbumes
            // 
            this.vistaAlbumes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.artista,
            this.titulo,
            this.year,
            this.duracion,
            this.genero});
            this.vistaAlbumes.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vistaAlbumes.FullRowSelect = true;
            this.vistaAlbumes.HideSelection = false;
            this.vistaAlbumes.Location = new System.Drawing.Point(12, 83);
            this.vistaAlbumes.Name = "vistaAlbumes";
            this.vistaAlbumes.ShowGroups = false;
            this.vistaAlbumes.Size = new System.Drawing.Size(735, 322);
            this.vistaAlbumes.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.vistaAlbumes.TabIndex = 1;
            this.vistaAlbumes.UseCompatibleStateImageBehavior = false;
            this.vistaAlbumes.View = System.Windows.Forms.View.Details;
            this.vistaAlbumes.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.ordenarColumnas);
            this.vistaAlbumes.ItemMouseHover += new System.Windows.Forms.ListViewItemMouseHoverEventHandler(this.vistaAlbumes_ItemMouseHover);
            this.vistaAlbumes.SelectedIndexChanged += new System.EventHandler(this.vistaAlbumes_SelectedIndexChanged_1);
            this.vistaAlbumes.KeyDown += new System.Windows.Forms.KeyEventHandler(this.vistaAlbumes_KeyDown);
            this.vistaAlbumes.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.vistaAlbumes_MouseDoubleClick);
            // 
            // artista
            // 
            this.artista.Width = 150;
            // 
            // titulo
            // 
            this.titulo.Width = 220;
            // 
            // year
            // 
            this.year.Width = 50;
            // 
            // duracion
            // 
            this.duracion.Width = 65;
            // 
            // genero
            // 
            this.genero.Width = 130;
            // 
            // refrescarButton
            // 
            this.refrescarButton.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.refrescarButton.Location = new System.Drawing.Point(669, 411);
            this.refrescarButton.Name = "refrescarButton";
            this.refrescarButton.Size = new System.Drawing.Size(78, 30);
            this.refrescarButton.TabIndex = 2;
            this.refrescarButton.Text = "refresh";
            this.refrescarButton.UseVisualStyleBackColor = true;
            this.refrescarButton.Click += new System.EventHandler(this.refrescarButton_Click);
            // 
            // banderaImageBox
            // 
            this.banderaImageBox.Location = new System.Drawing.Point(697, 27);
            this.banderaImageBox.Name = "banderaImageBox";
            this.banderaImageBox.Size = new System.Drawing.Size(50, 50);
            this.banderaImageBox.TabIndex = 3;
            this.banderaImageBox.TabStop = false;
            // 
            // borrarButton
            // 
            this.borrarButton.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.borrarButton.Location = new System.Drawing.Point(12, 411);
            this.borrarButton.Name = "borrarButton";
            this.borrarButton.Size = new System.Drawing.Size(147, 30);
            this.borrarButton.TabIndex = 2;
            this.borrarButton.Text = "borrar";
            this.borrarButton.UseVisualStyleBackColor = true;
            this.borrarButton.Click += new System.EventHandler(this.borrarButton_Click);
            // 
            // principal
            // 
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(759, 465);
            this.Controls.Add(this.banderaImageBox);
            this.Controls.Add(this.borrarButton);
            this.Controls.Add(this.refrescarButton);
            this.Controls.Add(this.vistaAlbumes);
            this.Controls.Add(this.menuStrip2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "principal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gestor de álbumes 0";
            this.Load += new System.EventHandler(this.principal_Load);
            this.menuStrip2.ResumeLayout(false);
            this.menuStrip2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.banderaImageBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBox1;
        private System.Windows.Forms.MenuStrip menuStrip2;
        private System.Windows.Forms.ToolStripMenuItem archivoMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem salirToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem agregarAlbumToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem opcionesToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem abrirToolStripMenuItem;
        private System.Windows.Forms.ListView vistaAlbumes;
        private System.Windows.Forms.Button refrescarButton;
        private System.Windows.Forms.PictureBox banderaImageBox;
        private System.Windows.Forms.Button borrarButton;
        private System.Windows.Forms.ToolStripMenuItem buscarEnSpotifyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem adminMenu;
        private System.Windows.Forms.ToolStripMenuItem generarAlbumToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem seleccionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem borrarseleccionToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader artista;
        private System.Windows.Forms.ColumnHeader titulo;
        private System.Windows.Forms.ColumnHeader year;
        private System.Windows.Forms.ColumnHeader duracion;
        private System.Windows.Forms.ColumnHeader genero;
        private System.Windows.Forms.ToolStripMenuItem guardarcomo;
        private System.Windows.Forms.ToolStripMenuItem acercaDeToolStripMenuItem;
    }
}