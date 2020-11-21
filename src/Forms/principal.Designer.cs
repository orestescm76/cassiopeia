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
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.agregarAlbumToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nuevoAlbumDesdeCarpetaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.abrirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CargarCDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.digitalToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.abrirCDMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cargarDiscosLegacyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.buscarEnSpotifyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vincularToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.guardarcomo = new System.Windows.Forms.ToolStripMenuItem();
            this.guardarCSVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.salirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.adminMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.generarAlbumToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tipografiaLyricsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.verLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.verToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.digitalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cdToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.seleccionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.borrarseleccionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reproductorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.columnID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.barraAbajo = new System.Windows.Forms.StatusStrip();
            this.duracionSeleccionada = new System.Windows.Forms.ToolStripStatusLabel();
            this.clickDerechoMenuContexto = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.crearCDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copiarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.spotifyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.verLyricsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.process1 = new System.Diagnostics.Process();
            this.configToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.reproductorToolStripMenuItem,
            this.acercaDeToolStripMenuItem,
            this.testToolStripMenuItem});
            this.barraPrincipal.Location = new System.Drawing.Point(0, 0);
            this.barraPrincipal.Name = "barraPrincipal";
            this.barraPrincipal.Size = new System.Drawing.Size(842, 24);
            this.barraPrincipal.TabIndex = 0;
            this.barraPrincipal.Text = "menuStrip2";
            // 
            // archivoMenuItem1
            // 
            this.archivoMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nuevoToolStripMenuItem,
            this.toolStripSeparator1,
            this.agregarAlbumToolStripMenuItem,
            this.nuevoAlbumDesdeCarpetaToolStripMenuItem,
            this.abrirToolStripMenuItem,
            this.abrirCDMenuItem,
            this.cargarDiscosLegacyToolStripMenuItem,
            this.toolStripSeparator2,
            this.buscarEnSpotifyToolStripMenuItem,
            this.vincularToolStripMenuItem,
            this.toolStripSeparator3,
            this.guardarcomo,
            this.guardarCSVToolStripMenuItem,
            this.toolStripSeparator4,
            this.salirToolStripMenuItem});
            this.archivoMenuItem1.Name = "archivoMenuItem1";
            this.archivoMenuItem1.Size = new System.Drawing.Size(64, 20);
            this.archivoMenuItem1.Text = "archivo1";
            // 
            // nuevoToolStripMenuItem
            // 
            this.nuevoToolStripMenuItem.Name = "nuevoToolStripMenuItem";
            this.nuevoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.N)));
            this.nuevoToolStripMenuItem.Size = new System.Drawing.Size(289, 22);
            this.nuevoToolStripMenuItem.Text = "nuevo";
            this.nuevoToolStripMenuItem.Click += new System.EventHandler(this.nuevoToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(286, 6);
            // 
            // agregarAlbumToolStripMenuItem
            // 
            this.agregarAlbumToolStripMenuItem.Name = "agregarAlbumToolStripMenuItem";
            this.agregarAlbumToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.agregarAlbumToolStripMenuItem.Size = new System.Drawing.Size(289, 22);
            this.agregarAlbumToolStripMenuItem.Text = "agregarAlbum3";
            this.agregarAlbumToolStripMenuItem.Click += new System.EventHandler(this.agregarAlbumToolStripMenuItem_Click);
            // 
            // nuevoAlbumDesdeCarpetaToolStripMenuItem
            // 
            this.nuevoAlbumDesdeCarpetaToolStripMenuItem.Name = "nuevoAlbumDesdeCarpetaToolStripMenuItem";
            this.nuevoAlbumDesdeCarpetaToolStripMenuItem.Size = new System.Drawing.Size(289, 22);
            this.nuevoAlbumDesdeCarpetaToolStripMenuItem.Text = "nuevoAlbumDesdeCarpeta";
            this.nuevoAlbumDesdeCarpetaToolStripMenuItem.Click += new System.EventHandler(this.nuevoAlbumDesdeCarpetaToolStripMenuItem_Click);
            // 
            // abrirToolStripMenuItem
            // 
            this.abrirToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CargarCDToolStripMenuItem,
            this.digitalToolStripMenuItem1});
            this.abrirToolStripMenuItem.Name = "abrirToolStripMenuItem";
            this.abrirToolStripMenuItem.Size = new System.Drawing.Size(289, 22);
            this.abrirToolStripMenuItem.Text = "abrirDesdeFichero14";
            // 
            // CargarCDToolStripMenuItem
            // 
            this.CargarCDToolStripMenuItem.Name = "CargarCDToolStripMenuItem";
            this.CargarCDToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.CargarCDToolStripMenuItem.Text = "CD";
            this.CargarCDToolStripMenuItem.Click += new System.EventHandler(this.CargarCDToolStripMenuItem_Click);
            // 
            // digitalToolStripMenuItem1
            // 
            this.digitalToolStripMenuItem1.Name = "digitalToolStripMenuItem1";
            this.digitalToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.digitalToolStripMenuItem1.Size = new System.Drawing.Size(151, 22);
            this.digitalToolStripMenuItem1.Text = "Digital";
            this.digitalToolStripMenuItem1.Click += new System.EventHandler(this.digitalToolStripMenuItem1_Click);
            // 
            // abrirCDMenuItem
            // 
            this.abrirCDMenuItem.Name = "abrirCDMenuItem";
            this.abrirCDMenuItem.Size = new System.Drawing.Size(289, 22);
            this.abrirCDMenuItem.Text = "abrirCD";
            this.abrirCDMenuItem.Click += new System.EventHandler(this.abrirCDMenuItem_Click);
            // 
            // cargarDiscosLegacyToolStripMenuItem
            // 
            this.cargarDiscosLegacyToolStripMenuItem.Name = "cargarDiscosLegacyToolStripMenuItem";
            this.cargarDiscosLegacyToolStripMenuItem.Size = new System.Drawing.Size(289, 22);
            this.cargarDiscosLegacyToolStripMenuItem.Text = "cargarDiscosLegacy";
            this.cargarDiscosLegacyToolStripMenuItem.Click += new System.EventHandler(this.cargarDiscosLegacyToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(286, 6);
            // 
            // buscarEnSpotifyToolStripMenuItem
            // 
            this.buscarEnSpotifyToolStripMenuItem.Name = "buscarEnSpotifyToolStripMenuItem";
            this.buscarEnSpotifyToolStripMenuItem.Size = new System.Drawing.Size(289, 22);
            this.buscarEnSpotifyToolStripMenuItem.Text = "Buscar en Spotify";
            this.buscarEnSpotifyToolStripMenuItem.Click += new System.EventHandler(this.buscarEnSpotifyToolStripMenuItem_Click);
            // 
            // vincularToolStripMenuItem
            // 
            this.vincularToolStripMenuItem.Name = "vincularToolStripMenuItem";
            this.vincularToolStripMenuItem.Size = new System.Drawing.Size(289, 22);
            this.vincularToolStripMenuItem.Text = "vincular";
            this.vincularToolStripMenuItem.Click += new System.EventHandler(this.vincularToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(286, 6);
            // 
            // guardarcomo
            // 
            this.guardarcomo.Name = "guardarcomo";
            this.guardarcomo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.guardarcomo.Size = new System.Drawing.Size(289, 22);
            this.guardarcomo.Text = "guardarComoToolStripMenuItem";
            this.guardarcomo.Click += new System.EventHandler(this.guardarcomo_Click);
            // 
            // guardarCSVToolStripMenuItem
            // 
            this.guardarCSVToolStripMenuItem.Name = "guardarCSVToolStripMenuItem";
            this.guardarCSVToolStripMenuItem.Size = new System.Drawing.Size(289, 22);
            this.guardarCSVToolStripMenuItem.Text = "guardarCSV";
            this.guardarCSVToolStripMenuItem.Visible = false;
            this.guardarCSVToolStripMenuItem.Click += new System.EventHandler(this.guardarCSVToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(286, 6);
            // 
            // salirToolStripMenuItem
            // 
            this.salirToolStripMenuItem.Name = "salirToolStripMenuItem";
            this.salirToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.salirToolStripMenuItem.Size = new System.Drawing.Size(289, 22);
            this.salirToolStripMenuItem.Text = "salirult";
            this.salirToolStripMenuItem.Click += new System.EventHandler(this.salirToolStripMenuItem_Click);
            // 
            // adminMenu
            // 
            this.adminMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.generarAlbumToolStripMenuItem,
            this.tipografiaLyricsToolStripMenuItem,
            this.verLogToolStripMenuItem,
            this.configToolStripMenuItem});
            this.adminMenu.Name = "adminMenu";
            this.adminMenu.Size = new System.Drawing.Size(79, 20);
            this.adminMenu.Text = "administrar";
            // 
            // generarAlbumToolStripMenuItem
            // 
            this.generarAlbumToolStripMenuItem.Name = "generarAlbumToolStripMenuItem";
            this.generarAlbumToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.generarAlbumToolStripMenuItem.Text = "generarAlbum";
            this.generarAlbumToolStripMenuItem.Click += new System.EventHandler(this.generarAlbumToolStripMenuItem_Click);
            // 
            // tipografiaLyricsToolStripMenuItem
            // 
            this.tipografiaLyricsToolStripMenuItem.Name = "tipografiaLyricsToolStripMenuItem";
            this.tipografiaLyricsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.tipografiaLyricsToolStripMenuItem.Text = "tipografiaLyrics";
            this.tipografiaLyricsToolStripMenuItem.Click += new System.EventHandler(this.tipografiaLyricsToolStripMenuItem_Click);
            // 
            // verLogToolStripMenuItem
            // 
            this.verLogToolStripMenuItem.Name = "verLogToolStripMenuItem";
            this.verLogToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.verLogToolStripMenuItem.Text = "verLog";
            this.verLogToolStripMenuItem.Click += new System.EventHandler(this.verLogToolStripMenuItem_Click);
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
            this.digitalToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D1)));
            this.digitalToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.digitalToolStripMenuItem.Text = "digital";
            this.digitalToolStripMenuItem.Click += new System.EventHandler(this.digitalToolStripMenuItem_Click);
            // 
            // cdToolStripMenuItem
            // 
            this.cdToolStripMenuItem.CheckOnClick = true;
            this.cdToolStripMenuItem.Name = "cdToolStripMenuItem";
            this.cdToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D2)));
            this.cdToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.cdToolStripMenuItem.Text = "CD";
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
            this.borrarseleccionToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.borrarseleccionToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.borrarseleccionToolStripMenuItem.Text = "borrar_seleccion";
            this.borrarseleccionToolStripMenuItem.Click += new System.EventHandler(this.borrarseleccionToolStripMenuItem_Click);
            // 
            // reproductorToolStripMenuItem
            // 
            this.reproductorToolStripMenuItem.Name = "reproductorToolStripMenuItem";
            this.reproductorToolStripMenuItem.Size = new System.Drawing.Size(85, 20);
            this.reproductorToolStripMenuItem.Text = "Reproductor";
            this.reproductorToolStripMenuItem.Click += new System.EventHandler(this.reproductorToolStripMenuItem_Click);
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
            this.testToolStripMenuItem.Visible = false;
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
            this.vistaAlbumes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.vistaAlbumes.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.vistaAlbumes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.vistaAlbumes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.artista,
            this.titulo,
            this.year,
            this.duracion,
            this.genero,
            this.columnID});
            this.vistaAlbumes.Cursor = System.Windows.Forms.Cursors.Default;
            this.vistaAlbumes.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vistaAlbumes.ForeColor = System.Drawing.SystemColors.WindowText;
            this.vistaAlbumes.FullRowSelect = true;
            this.vistaAlbumes.HideSelection = false;
            this.vistaAlbumes.Location = new System.Drawing.Point(0, 24);
            this.vistaAlbumes.Margin = new System.Windows.Forms.Padding(3, 3, 3, 12);
            this.vistaAlbumes.MinimumSize = new System.Drawing.Size(842, 512);
            this.vistaAlbumes.Name = "vistaAlbumes";
            this.vistaAlbumes.ShowGroups = false;
            this.vistaAlbumes.Size = new System.Drawing.Size(842, 512);
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
            // columnID
            // 
            this.columnID.Text = "id";
            this.columnID.Width = 0;
            // 
            // barraAbajo
            // 
            this.barraAbajo.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.duracionSeleccionada});
            this.barraAbajo.Location = new System.Drawing.Point(0, 536);
            this.barraAbajo.Name = "barraAbajo";
            this.barraAbajo.Size = new System.Drawing.Size(842, 22);
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
            this.copiarToolStripMenuItem,
            this.spotifyToolStripMenuItem,
            this.verLyricsToolStripMenuItem});
            this.clickDerechoMenuContexto.Name = "contextMenuStrip1";
            this.clickDerechoMenuContexto.Size = new System.Drawing.Size(120, 92);
            this.clickDerechoMenuContexto.Opening += new System.ComponentModel.CancelEventHandler(this.clickDerechoMenuContexto_Opening);
            // 
            // crearCDToolStripMenuItem
            // 
            this.crearCDToolStripMenuItem.Name = "crearCDToolStripMenuItem";
            this.crearCDToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.crearCDToolStripMenuItem.Text = "cd";
            this.crearCDToolStripMenuItem.Click += new System.EventHandler(this.crearCDToolStripMenuItem_Click);
            // 
            // copiarToolStripMenuItem
            // 
            this.copiarToolStripMenuItem.Name = "copiarToolStripMenuItem";
            this.copiarToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.copiarToolStripMenuItem.Text = "copiar";
            this.copiarToolStripMenuItem.Click += new System.EventHandler(this.copiarToolStripMenuItem_Click);
            // 
            // spotifyToolStripMenuItem
            // 
            this.spotifyToolStripMenuItem.Enabled = false;
            this.spotifyToolStripMenuItem.Name = "spotifyToolStripMenuItem";
            this.spotifyToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.spotifyToolStripMenuItem.Text = "Spotify";
            this.spotifyToolStripMenuItem.Click += new System.EventHandler(this.spotifyToolStripMenuItem_Click);
            // 
            // verLyricsToolStripMenuItem
            // 
            this.verLyricsToolStripMenuItem.Name = "verLyricsToolStripMenuItem";
            this.verLyricsToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.verLyricsToolStripMenuItem.Text = "verLyrics";
            this.verLyricsToolStripMenuItem.Click += new System.EventHandler(this.verLyricsToolStripMenuItem_Click);
            // 
            // process1
            // 
            this.process1.StartInfo.Domain = "";
            this.process1.StartInfo.LoadUserProfile = false;
            this.process1.StartInfo.Password = null;
            this.process1.StartInfo.StandardErrorEncoding = null;
            this.process1.StartInfo.StandardOutputEncoding = null;
            this.process1.StartInfo.UserName = "";
            this.process1.SynchronizingObject = this;
            // 
            // configToolStripMenuItem
            // 
            this.configToolStripMenuItem.Name = "configToolStripMenuItem";
            this.configToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.configToolStripMenuItem.Text = "config";
            this.configToolStripMenuItem.Click += new System.EventHandler(this.configToolStripMenuItem_Click);
            // 
            // principal
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(842, 558);
            this.Controls.Add(this.barraAbajo);
            this.Controls.Add(this.vistaAlbumes);
            this.Controls.Add(this.barraPrincipal);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.barraPrincipal;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(858, 597);
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
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem abrirToolStripMenuItem;
        private System.Windows.Forms.ListView vistaAlbumes;
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
        private System.Windows.Forms.ColumnHeader columnID;
        private System.Windows.Forms.ToolStripMenuItem cargarDiscosLegacyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CargarCDToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem digitalToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem vincularToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem spotifyToolStripMenuItem;
        private System.Diagnostics.Process process1;
        private System.Windows.Forms.ToolStripMenuItem guardarCSVToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reproductorToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem abrirCDMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem tipografiaLyricsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem verLyricsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem verLogToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nuevoAlbumDesdeCarpetaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem configToolStripMenuItem;
    }
}