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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(principal));
            this.barraPrincipal = new System.Windows.Forms.MenuStrip();
            this.archivoMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.nuevoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.agregarAlbumToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.abrirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buscarEnSpotifyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.guardarcomo = new System.Windows.Forms.ToolStripMenuItem();
            this.salirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.adminMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.generarAlbumToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.verToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.digitalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cdToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.seleccionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.borrarseleccionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.opcionesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.acercaDeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.vistaAlbumes = new System.Windows.Forms.ListView();
            this.artista = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.titulo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.year = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.duracion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.genero = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.refrescarButton = new System.Windows.Forms.Button();
            this.borrarButton = new System.Windows.Forms.Button();
            this.barraAbajo = new System.Windows.Forms.StatusStrip();
            this.duracionSeleccionada = new System.Windows.Forms.ToolStripStatusLabel();
            this.clickDerechoMenuContexto = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.crearCDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copiarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.barraPrincipal.SuspendLayout();
            this.barraAbajo.SuspendLayout();
            this.clickDerechoMenuContexto.SuspendLayout();
            this.SuspendLayout();
            // 
            // barraPrincipal
            // 
            this.barraPrincipal.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.archivoMenuItem1,
            this.adminMenu,
            this.verToolStripMenuItem,
            this.seleccionToolStripMenuItem,
            this.opcionesToolStripMenuItem,
            this.acercaDeToolStripMenuItem,
            this.testToolStripMenuItem});
            this.barraPrincipal.Location = new System.Drawing.Point(0, 0);
            this.barraPrincipal.Name = "barraPrincipal";
            this.barraPrincipal.Size = new System.Drawing.Size(759, 24);
            this.barraPrincipal.TabIndex = 0;
            this.barraPrincipal.Text = "menuStrip2";
            // 
            // archivoMenuItem1
            // 
            this.archivoMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nuevoToolStripMenuItem,
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
            // nuevoToolStripMenuItem
            // 
            this.nuevoToolStripMenuItem.Name = "nuevoToolStripMenuItem";
            this.nuevoToolStripMenuItem.Size = new System.Drawing.Size(249, 22);
            this.nuevoToolStripMenuItem.Text = "nuevo";
            this.nuevoToolStripMenuItem.Click += new System.EventHandler(this.nuevoToolStripMenuItem_Click);
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
            // verToolStripMenuItem
            // 
            this.verToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.digitalToolStripMenuItem,
            this.cdToolStripMenuItem});
            this.verToolStripMenuItem.Name = "verToolStripMenuItem";
            this.verToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.verToolStripMenuItem.Text = "ver";
            // 
            // digitalToolStripMenuItem
            // 
            this.digitalToolStripMenuItem.Checked = true;
            this.digitalToolStripMenuItem.CheckOnClick = true;
            this.digitalToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.digitalToolStripMenuItem.Name = "digitalToolStripMenuItem";
            this.digitalToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.digitalToolStripMenuItem.Text = "digital";
            this.digitalToolStripMenuItem.Click += new System.EventHandler(this.digitalToolStripMenuItem_Click);
            // 
            // cdToolStripMenuItem
            // 
            this.cdToolStripMenuItem.CheckOnClick = true;
            this.cdToolStripMenuItem.Name = "cdToolStripMenuItem";
            this.cdToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.cdToolStripMenuItem.Text = "cd";
            this.cdToolStripMenuItem.Click += new System.EventHandler(this.cdToolStripMenuItem_Click);
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
            // testToolStripMenuItem
            // 
            this.testToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.testToolStripMenuItem1});
            this.testToolStripMenuItem.Name = "testToolStripMenuItem";
            this.testToolStripMenuItem.Size = new System.Drawing.Size(38, 20);
            this.testToolStripMenuItem.Text = "test";
            // 
            // testToolStripMenuItem1
            // 
            this.testToolStripMenuItem1.CheckOnClick = true;
            this.testToolStripMenuItem1.Name = "testToolStripMenuItem1";
            this.testToolStripMenuItem1.Size = new System.Drawing.Size(131, 22);
            this.testToolStripMenuItem1.Text = "testOscuro";
            this.testToolStripMenuItem1.Click += new System.EventHandler(this.testToolStripMenuItem1_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // vistaAlbumes
            // 
            this.vistaAlbumes.AllowColumnReorder = true;
            this.vistaAlbumes.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.vistaAlbumes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.artista,
            this.titulo,
            this.year,
            this.duracion,
            this.genero});
            this.vistaAlbumes.Cursor = System.Windows.Forms.Cursors.Default;
            this.vistaAlbumes.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vistaAlbumes.ForeColor = System.Drawing.SystemColors.WindowText;
            this.vistaAlbumes.FullRowSelect = true;
            this.vistaAlbumes.HideSelection = false;
            this.vistaAlbumes.Location = new System.Drawing.Point(0, 27);
            this.vistaAlbumes.Name = "vistaAlbumes";
            this.vistaAlbumes.ShowGroups = false;
            this.vistaAlbumes.Size = new System.Drawing.Size(759, 414);
            this.vistaAlbumes.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.vistaAlbumes.TabIndex = 1;
            this.vistaAlbumes.UseCompatibleStateImageBehavior = false;
            this.vistaAlbumes.View = System.Windows.Forms.View.Details;
            this.vistaAlbumes.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.ordenarColumnas);
            this.vistaAlbumes.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.vistaAlbumes_DrawColumnHeader);
            this.vistaAlbumes.ItemMouseHover += new System.Windows.Forms.ListViewItemMouseHoverEventHandler(this.vistaAlbumes_ItemMouseHover);
            this.vistaAlbumes.SelectedIndexChanged += new System.EventHandler(this.vistaAlbumes_SelectedIndexChanged_1);
            this.vistaAlbumes.KeyDown += new System.Windows.Forms.KeyEventHandler(this.vistaAlbumes_KeyDown);
            this.vistaAlbumes.MouseClick += new System.Windows.Forms.MouseEventHandler(this.vistaAlbumes_MouseClick);
            this.vistaAlbumes.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.vistaAlbumes_MouseDoubleClick);
            // 
            // artista
            // 
            this.artista.Width = 202;
            // 
            // titulo
            // 
            this.titulo.Width = 272;
            // 
            // year
            // 
            this.year.Width = 50;
            // 
            // duracion
            // 
            this.duracion.Width = 71;
            // 
            // genero
            // 
            this.genero.Width = 142;
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
            // barraAbajo
            // 
            this.barraAbajo.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.duracionSeleccionada});
            this.barraAbajo.Location = new System.Drawing.Point(0, 443);
            this.barraAbajo.Name = "barraAbajo";
            this.barraAbajo.Size = new System.Drawing.Size(759, 22);
            this.barraAbajo.TabIndex = 4;
            this.barraAbajo.Text = "statusStrip1";
            // 
            // duracionSeleccionada
            // 
            this.duracionSeleccionada.Name = "duracionSeleccionada";
            this.duracionSeleccionada.Size = new System.Drawing.Size(118, 17);
            this.duracionSeleccionada.Text = "toolStripStatusLabel1";
            // 
            // clickDerechoMenuContexto
            // 
            this.clickDerechoMenuContexto.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.crearCDToolStripMenuItem,
            this.copiarToolStripMenuItem});
            this.clickDerechoMenuContexto.Name = "contextMenuStrip1";
            this.clickDerechoMenuContexto.Size = new System.Drawing.Size(181, 70);
            this.clickDerechoMenuContexto.Opening += new System.ComponentModel.CancelEventHandler(this.clickDerechoMenuContexto_Opening);
            // 
            // crearCDToolStripMenuItem
            // 
            this.crearCDToolStripMenuItem.Name = "crearCDToolStripMenuItem";
            this.crearCDToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.crearCDToolStripMenuItem.Text = "cd";
            this.crearCDToolStripMenuItem.Click += new System.EventHandler(this.crearCDToolStripMenuItem_Click);
            // 
            // copiarToolStripMenuItem
            // 
            this.copiarToolStripMenuItem.Name = "copiarToolStripMenuItem";
            this.copiarToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.copiarToolStripMenuItem.Text = "copiar";
            this.copiarToolStripMenuItem.Click += new System.EventHandler(this.copiarToolStripMenuItem_Click);
            // 
            // principal
            // 
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(759, 465);
            this.Controls.Add(this.barraAbajo);
            this.Controls.Add(this.borrarButton);
            this.Controls.Add(this.refrescarButton);
            this.Controls.Add(this.vistaAlbumes);
            this.Controls.Add(this.barraPrincipal);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "principal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gestor de álbumes 0";
            this.barraPrincipal.ResumeLayout(false);
            this.barraPrincipal.PerformLayout();
            this.barraAbajo.ResumeLayout(false);
            this.barraAbajo.PerformLayout();
            this.clickDerechoMenuContexto.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip barraPrincipal;
        private System.Windows.Forms.ToolStripMenuItem archivoMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem salirToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem agregarAlbumToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem opcionesToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem abrirToolStripMenuItem;
        private System.Windows.Forms.ListView vistaAlbumes;
        private System.Windows.Forms.Button refrescarButton;
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
        private System.Windows.Forms.StatusStrip barraAbajo;
        private System.Windows.Forms.ToolStripStatusLabel duracionSeleccionada;
        private System.Windows.Forms.ToolStripMenuItem nuevoToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip clickDerechoMenuContexto;
        private System.Windows.Forms.ToolStripMenuItem crearCDToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem copiarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem verToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem digitalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cdToolStripMenuItem;
    }
}