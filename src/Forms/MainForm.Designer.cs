namespace Cassiopeia.src.Forms
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
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
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.guardarcomo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.salirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.adminMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.generarAlbumToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.verLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.filterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.verToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.digitalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vierCDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vinylToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showSidebarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.spotifyStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchSpotifyStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.linkSpotifyStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importSpotifyStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.seleccionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.borrarseleccionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reproductorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.acercaDeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.vistaAlbumes = new System.Windows.Forms.ListView();
            this.artista = new System.Windows.Forms.ColumnHeader();
            this.titulo = new System.Windows.Forms.ColumnHeader();
            this.year = new System.Windows.Forms.ColumnHeader();
            this.duracion = new System.Windows.Forms.ColumnHeader();
            this.genero = new System.Windows.Forms.ColumnHeader();
            this.barraAbajo = new System.Windows.Forms.StatusStrip();
            this.duracionSeleccionada = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelViewInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.clickDerechoMenuContexto = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.crearCDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copiarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playSpotifyAlbumToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.verLyricsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createVinylToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelSidebar = new System.Windows.Forms.Panel();
            this.labelGeneralInfo = new System.Windows.Forms.Label();
            this.labelInfoAlbum = new System.Windows.Forms.Label();
            this.pictureBoxSidebarCover = new System.Windows.Forms.PictureBox();
            this.contextMenuSidebarCover = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.sidebarCopyImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMain = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonNewAlbum = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonNewDatabase = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonOpenDatabase = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSaveDatabase = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonFilter = new System.Windows.Forms.ToolStripButton();
            this.toolStripTextBoxSearch = new System.Windows.Forms.ToolStripTextBox();
            this.barraPrincipal.SuspendLayout();
            this.barraAbajo.SuspendLayout();
            this.clickDerechoMenuContexto.SuspendLayout();
            this.panelSidebar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSidebarCover)).BeginInit();
            this.contextMenuSidebarCover.SuspendLayout();
            this.toolStripMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // barraPrincipal
            // 
            this.barraPrincipal.GripMargin = new System.Windows.Forms.Padding(2);
            this.barraPrincipal.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.archivoMenuItem1,
            this.adminMenu,
            this.verToolStripMenuItem,
            this.spotifyStripMenuItem,
            this.seleccionToolStripMenuItem,
            this.reproductorToolStripMenuItem,
            this.acercaDeToolStripMenuItem,
            this.testToolStripMenuItem});
            this.barraPrincipal.Location = new System.Drawing.Point(0, 0);
            this.barraPrincipal.Name = "barraPrincipal";
            this.barraPrincipal.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.barraPrincipal.Size = new System.Drawing.Size(1131, 24);
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
            this.toolStripSeparator3,
            this.guardarcomo,
            this.toolStripSeparator4,
            this.salirToolStripMenuItem});
            this.archivoMenuItem1.Name = "archivoMenuItem1";
            this.archivoMenuItem1.Size = new System.Drawing.Size(64, 20);
            this.archivoMenuItem1.Text = "archivo1";
            this.archivoMenuItem1.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            // 
            // nuevoToolStripMenuItem
            // 
            this.nuevoToolStripMenuItem.Name = "nuevoToolStripMenuItem";
            this.nuevoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.N)));
            this.nuevoToolStripMenuItem.Size = new System.Drawing.Size(289, 22);
            this.nuevoToolStripMenuItem.Text = "nuevo";
            this.nuevoToolStripMenuItem.Click += new System.EventHandler(this.NewDatabase);
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
            this.agregarAlbumToolStripMenuItem.Click += new System.EventHandler(this.CreateNewAlbum);
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
            this.verLogToolStripMenuItem,
            this.configToolStripMenuItem,
            this.filterToolStripMenuItem});
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
            // verLogToolStripMenuItem
            // 
            this.verLogToolStripMenuItem.Name = "verLogToolStripMenuItem";
            this.verLogToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.verLogToolStripMenuItem.Text = "verLog";
            this.verLogToolStripMenuItem.Click += new System.EventHandler(this.verLogToolStripMenuItem_Click);
            // 
            // configToolStripMenuItem
            // 
            this.configToolStripMenuItem.Name = "configToolStripMenuItem";
            this.configToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.configToolStripMenuItem.Text = "config";
            this.configToolStripMenuItem.Click += new System.EventHandler(this.configToolStripMenuItem_Click);
            // 
            // filterToolStripMenuItem
            // 
            this.filterToolStripMenuItem.Name = "filterToolStripMenuItem";
            this.filterToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.filterToolStripMenuItem.Text = "filter";
            this.filterToolStripMenuItem.Click += new System.EventHandler(this.OpenFilterWindow);
            // 
            // verToolStripMenuItem
            // 
            this.verToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewModeToolStripMenuItem,
            this.showSidebarToolStripMenuItem});
            this.verToolStripMenuItem.Name = "verToolStripMenuItem";
            this.verToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.verToolStripMenuItem.Text = "ver";
            // 
            // viewModeToolStripMenuItem
            // 
            this.viewModeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.digitalToolStripMenuItem,
            this.vierCDToolStripMenuItem,
            this.vinylToolStripMenuItem});
            this.viewModeToolStripMenuItem.Name = "viewModeToolStripMenuItem";
            this.viewModeToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.viewModeToolStripMenuItem.Text = "ViewMode";
            // 
            // digitalToolStripMenuItem
            // 
            this.digitalToolStripMenuItem.Checked = true;
            this.digitalToolStripMenuItem.CheckOnClick = true;
            this.digitalToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.digitalToolStripMenuItem.Name = "digitalToolStripMenuItem";
            this.digitalToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D1)));
            this.digitalToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.digitalToolStripMenuItem.Tag = "digital";
            this.digitalToolStripMenuItem.Text = "digital";
            this.digitalToolStripMenuItem.Click += new System.EventHandler(this.digitalToolStripMenuItem_Click);
            // 
            // vierCDToolStripMenuItem
            // 
            this.vierCDToolStripMenuItem.CheckOnClick = true;
            this.vierCDToolStripMenuItem.Name = "vierCDToolStripMenuItem";
            this.vierCDToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D2)));
            this.vierCDToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.vierCDToolStripMenuItem.Tag = "cd";
            this.vierCDToolStripMenuItem.Text = "CD";
            this.vierCDToolStripMenuItem.Click += new System.EventHandler(this.viewCDToolStripMenuItem_Click);
            // 
            // vinylToolStripMenuItem
            // 
            this.vinylToolStripMenuItem.Name = "vinylToolStripMenuItem";
            this.vinylToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D3)));
            this.vinylToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.vinylToolStripMenuItem.Text = "vinyl";
            this.vinylToolStripMenuItem.Click += new System.EventHandler(this.vinylToolStripMenuItem_Click);
            // 
            // showSidebarToolStripMenuItem
            // 
            this.showSidebarToolStripMenuItem.Checked = true;
            this.showSidebarToolStripMenuItem.CheckOnClick = true;
            this.showSidebarToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showSidebarToolStripMenuItem.Name = "showSidebarToolStripMenuItem";
            this.showSidebarToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.showSidebarToolStripMenuItem.Text = "showPanel";
            this.showSidebarToolStripMenuItem.Click += new System.EventHandler(this.panelToolStripMenuItem_Click);
            // 
            // spotifyStripMenuItem
            // 
            this.spotifyStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.searchSpotifyStripMenuItem,
            this.linkSpotifyStripMenuItem,
            this.importSpotifyStripMenuItem});
            this.spotifyStripMenuItem.Name = "spotifyStripMenuItem";
            this.spotifyStripMenuItem.Size = new System.Drawing.Size(56, 20);
            this.spotifyStripMenuItem.Text = "Spotify";
            // 
            // searchSpotifyStripMenuItem
            // 
            this.searchSpotifyStripMenuItem.Name = "searchSpotifyStripMenuItem";
            this.searchSpotifyStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.searchSpotifyStripMenuItem.Text = "searchSpotifyStripMenuItem";
            this.searchSpotifyStripMenuItem.Click += new System.EventHandler(this.searchSpotifyStripMenuItem_Click);
            // 
            // linkSpotifyStripMenuItem
            // 
            this.linkSpotifyStripMenuItem.Name = "linkSpotifyStripMenuItem";
            this.linkSpotifyStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.linkSpotifyStripMenuItem.Text = "linkSpotifyStripMenuItem";
            this.linkSpotifyStripMenuItem.Click += new System.EventHandler(this.linkSpotifyStripMenuItem_Click);
            // 
            // importSpotifyStripMenuItem
            // 
            this.importSpotifyStripMenuItem.Name = "importSpotifyStripMenuItem";
            this.importSpotifyStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.importSpotifyStripMenuItem.Text = "importSpotifyStripMenuItem";
            this.importSpotifyStripMenuItem.Click += new System.EventHandler(this.importSpotifyStripMenuItem_Click);
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
            this.borrarseleccionToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
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
            this.vistaAlbumes.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.vistaAlbumes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.vistaAlbumes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.artista,
            this.titulo,
            this.year,
            this.duracion,
            this.genero});
            this.vistaAlbumes.Cursor = System.Windows.Forms.Cursors.Default;
            this.vistaAlbumes.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.vistaAlbumes.ForeColor = System.Drawing.SystemColors.WindowText;
            this.vistaAlbumes.FullRowSelect = true;
            this.vistaAlbumes.HideSelection = false;
            this.vistaAlbumes.Location = new System.Drawing.Point(0, 49);
            this.vistaAlbumes.Name = "vistaAlbumes";
            this.vistaAlbumes.ShowGroups = false;
            this.vistaAlbumes.Size = new System.Drawing.Size(824, 429);
            this.vistaAlbumes.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.vistaAlbumes.TabIndex = 1;
            this.vistaAlbumes.UseCompatibleStateImageBehavior = false;
            this.vistaAlbumes.View = System.Windows.Forms.View.Details;
            this.vistaAlbumes.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.OrdenarColumnas);
            this.vistaAlbumes.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.vistaAlbumes_DrawColumnHeader);
            this.vistaAlbumes.ItemMouseHover += new System.Windows.Forms.ListViewItemMouseHoverEventHandler(this.vistaAlbumes_ItemMouseHover);
            this.vistaAlbumes.SelectedIndexChanged += new System.EventHandler(this.vistaAlbumes_SelectedIndexChanged);
            this.vistaAlbumes.Click += new System.EventHandler(this.vistaAlbumes_Click);
            this.vistaAlbumes.KeyDown += new System.Windows.Forms.KeyEventHandler(this.vistaAlbumes_KeyDown);
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
            // barraAbajo
            // 
            this.barraAbajo.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.duracionSeleccionada,
            this.toolStripStatusLabelViewInfo});
            this.barraAbajo.Location = new System.Drawing.Point(0, 481);
            this.barraAbajo.Name = "barraAbajo";
            this.barraAbajo.Size = new System.Drawing.Size(1131, 22);
            this.barraAbajo.TabIndex = 4;
            this.barraAbajo.Text = "statusStrip1";
            // 
            // duracionSeleccionada
            // 
            this.duracionSeleccionada.Name = "duracionSeleccionada";
            this.duracionSeleccionada.Size = new System.Drawing.Size(183, 17);
            this.duracionSeleccionada.Text = "toolStripStatusLabelTotalDuration";
            // 
            // toolStripStatusLabelViewInfo
            // 
            this.toolStripStatusLabelViewInfo.Name = "toolStripStatusLabelViewInfo";
            this.toolStripStatusLabelViewInfo.Size = new System.Drawing.Size(168, 17);
            this.toolStripStatusLabelViewInfo.Text = "toolStripStatusLabelViewMode";
            // 
            // clickDerechoMenuContexto
            // 
            this.clickDerechoMenuContexto.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.crearCDToolStripMenuItem,
            this.copiarToolStripMenuItem,
            this.playSpotifyAlbumToolStripMenuItem,
            this.verLyricsToolStripMenuItem,
            this.createVinylToolStripMenuItem});
            this.clickDerechoMenuContexto.Name = "contextMenuStrip1";
            this.clickDerechoMenuContexto.Size = new System.Drawing.Size(181, 136);
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
            // playSpotifyAlbumToolStripMenuItem
            // 
            this.playSpotifyAlbumToolStripMenuItem.Enabled = false;
            this.playSpotifyAlbumToolStripMenuItem.Name = "playSpotifyAlbumToolStripMenuItem";
            this.playSpotifyAlbumToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.playSpotifyAlbumToolStripMenuItem.Text = "Spotify";
            this.playSpotifyAlbumToolStripMenuItem.Click += new System.EventHandler(this.playSpotifyAlbumToolStripMenuItem_Click);
            // 
            // verLyricsToolStripMenuItem
            // 
            this.verLyricsToolStripMenuItem.Name = "verLyricsToolStripMenuItem";
            this.verLyricsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.verLyricsToolStripMenuItem.Text = "verLyrics";
            this.verLyricsToolStripMenuItem.Click += new System.EventHandler(this.verLyricsToolStripMenuItem_Click);
            // 
            // createVinylToolStripMenuItem
            // 
            this.createVinylToolStripMenuItem.Name = "createVinylToolStripMenuItem";
            this.createVinylToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.createVinylToolStripMenuItem.Text = "vinyl";
            this.createVinylToolStripMenuItem.Click += new System.EventHandler(this.vinylToolStripMenuItem1_Click);
            // 
            // panelSidebar
            // 
            this.panelSidebar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelSidebar.AutoSize = true;
            this.panelSidebar.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelSidebar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelSidebar.Controls.Add(this.labelGeneralInfo);
            this.panelSidebar.Controls.Add(this.labelInfoAlbum);
            this.panelSidebar.Controls.Add(this.pictureBoxSidebarCover);
            this.panelSidebar.Location = new System.Drawing.Point(830, 49);
            this.panelSidebar.Name = "panelSidebar";
            this.panelSidebar.Size = new System.Drawing.Size(292, 429);
            this.panelSidebar.TabIndex = 5;
            // 
            // labelGeneralInfo
            // 
            this.labelGeneralInfo.AllowDrop = true;
            this.labelGeneralInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelGeneralInfo.AutoSize = true;
            this.labelGeneralInfo.Location = new System.Drawing.Point(18, 382);
            this.labelGeneralInfo.Name = "labelGeneralInfo";
            this.labelGeneralInfo.Size = new System.Drawing.Size(38, 15);
            this.labelGeneralInfo.TabIndex = 2;
            this.labelGeneralInfo.Text = "label1";
            // 
            // labelInfoAlbum
            // 
            this.labelInfoAlbum.AutoSize = true;
            this.labelInfoAlbum.Location = new System.Drawing.Point(18, 256);
            this.labelInfoAlbum.Name = "labelInfoAlbum";
            this.labelInfoAlbum.Size = new System.Drawing.Size(0, 15);
            this.labelInfoAlbum.TabIndex = 1;
            this.labelInfoAlbum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBoxSidebarCover
            // 
            this.pictureBoxSidebarCover.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBoxSidebarCover.ContextMenuStrip = this.contextMenuSidebarCover;
            this.pictureBoxSidebarCover.Image = global::Cassiopeia.Properties.Resources.albumdesconocido;
            this.pictureBoxSidebarCover.Location = new System.Drawing.Point(30, 3);
            this.pictureBoxSidebarCover.Name = "pictureBoxSidebarCover";
            this.pictureBoxSidebarCover.Size = new System.Drawing.Size(235, 235);
            this.pictureBoxSidebarCover.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxSidebarCover.TabIndex = 0;
            this.pictureBoxSidebarCover.TabStop = false;
            // 
            // contextMenuSidebarCover
            // 
            this.contextMenuSidebarCover.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sidebarCopyImageToolStripMenuItem});
            this.contextMenuSidebarCover.Name = "clickDerechoAlbum";
            this.contextMenuSidebarCover.Size = new System.Drawing.Size(172, 26);
            this.contextMenuSidebarCover.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuSidebarCover_Opening);
            // 
            // sidebarCopyImageToolStripMenuItem
            // 
            this.sidebarCopyImageToolStripMenuItem.Name = "sidebarCopyImageToolStripMenuItem";
            this.sidebarCopyImageToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.sidebarCopyImageToolStripMenuItem.Text = "copiarImagenStrip";
            this.sidebarCopyImageToolStripMenuItem.Click += new System.EventHandler(this.copiarImagenStrip_Click);
            // 
            // toolStripMain
            // 
            this.toolStripMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonNewAlbum,
            this.toolStripButtonNewDatabase,
            this.toolStripButtonOpenDatabase,
            this.toolStripButtonSaveDatabase,
            this.toolStripButtonFilter,
            this.toolStripTextBoxSearch});
            this.toolStripMain.Location = new System.Drawing.Point(0, 24);
            this.toolStripMain.Name = "toolStripMain";
            this.toolStripMain.Padding = new System.Windows.Forms.Padding(4, 0, 2, 0);
            this.toolStripMain.Size = new System.Drawing.Size(1131, 25);
            this.toolStripMain.TabIndex = 6;
            this.toolStripMain.Text = "toolStrip1";
            // 
            // toolStripButtonNewAlbum
            // 
            this.toolStripButtonNewAlbum.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonNewAlbum.Image = global::Cassiopeia.Properties.Resources._new;
            this.toolStripButtonNewAlbum.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonNewAlbum.Name = "toolStripButtonNewAlbum";
            this.toolStripButtonNewAlbum.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.toolStripButtonNewAlbum.Size = new System.Drawing.Size(30, 22);
            this.toolStripButtonNewAlbum.Text = "toolStripButtonNewAlbum";
            this.toolStripButtonNewAlbum.Click += new System.EventHandler(this.CreateNewAlbum);
            // 
            // toolStripButtonNewDatabase
            // 
            this.toolStripButtonNewDatabase.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonNewDatabase.Image = global::Cassiopeia.Properties.Resources.newDB;
            this.toolStripButtonNewDatabase.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonNewDatabase.Name = "toolStripButtonNewDatabase";
            this.toolStripButtonNewDatabase.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonNewDatabase.Text = "toolStripButtonNewDatabase";
            this.toolStripButtonNewDatabase.Click += new System.EventHandler(this.NewDatabase);
            // 
            // toolStripButtonOpenDatabase
            // 
            this.toolStripButtonOpenDatabase.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonOpenDatabase.Image = global::Cassiopeia.Properties.Resources.open;
            this.toolStripButtonOpenDatabase.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonOpenDatabase.Name = "toolStripButtonOpenDatabase";
            this.toolStripButtonOpenDatabase.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonOpenDatabase.Text = "toolStripButton1";
            this.toolStripButtonOpenDatabase.Click += new System.EventHandler(this.OpenDatabase);
            // 
            // toolStripButtonSaveDatabase
            // 
            this.toolStripButtonSaveDatabase.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSaveDatabase.Image = global::Cassiopeia.Properties.Resources.diskette;
            this.toolStripButtonSaveDatabase.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSaveDatabase.Name = "toolStripButtonSaveDatabase";
            this.toolStripButtonSaveDatabase.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonSaveDatabase.Text = "toolStripButton1";
            this.toolStripButtonSaveDatabase.Click += new System.EventHandler(this.SaveActualDatabase);
            // 
            // toolStripButtonFilter
            // 
            this.toolStripButtonFilter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonFilter.Image = global::Cassiopeia.Properties.Resources.filter;
            this.toolStripButtonFilter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonFilter.Name = "toolStripButtonFilter";
            this.toolStripButtonFilter.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonFilter.Text = "filter";
            this.toolStripButtonFilter.Click += new System.EventHandler(this.OpenFilterWindow);
            // 
            // toolStripTextBoxSearch
            // 
            this.toolStripTextBoxSearch.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripTextBoxSearch.Margin = new System.Windows.Forms.Padding(1, 0, 8, 0);
            this.toolStripTextBoxSearch.Name = "toolStripTextBoxSearch";
            this.toolStripTextBoxSearch.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.toolStripTextBoxSearch.Size = new System.Drawing.Size(150, 25);
            this.toolStripTextBoxSearch.ToolTipText = "write";
            this.toolStripTextBoxSearch.Click += new System.EventHandler(this.toolStripTextBoxSearch_Click);
            this.toolStripTextBoxSearch.TextChanged += new System.EventHandler(this.toolStripTextBoxSearch_TextChanged);
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1131, 503);
            this.ContextMenuStrip = this.clickDerechoMenuContexto;
            this.Controls.Add(this.toolStripMain);
            this.Controls.Add(this.panelSidebar);
            this.Controls.Add(this.barraAbajo);
            this.Controls.Add(this.vistaAlbumes);
            this.Controls.Add(this.barraPrincipal);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.barraPrincipal;
            this.MinimumSize = new System.Drawing.Size(771, 440);
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Gestor de álbumes 0";
            this.Click += new System.EventHandler(this.OpenFilterWindow);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.barraPrincipal.ResumeLayout(false);
            this.barraPrincipal.PerformLayout();
            this.barraAbajo.ResumeLayout(false);
            this.barraAbajo.PerformLayout();
            this.clickDerechoMenuContexto.ResumeLayout(false);
            this.panelSidebar.ResumeLayout(false);
            this.panelSidebar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSidebarCover)).EndInit();
            this.contextMenuSidebarCover.ResumeLayout(false);
            this.toolStripMain.ResumeLayout(false);
            this.toolStripMain.PerformLayout();
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
        private System.Windows.Forms.ToolStripMenuItem cargarDiscosLegacyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CargarCDToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem digitalToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem playSpotifyAlbumToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reproductorToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem abrirCDMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem verLyricsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem verLogToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nuevoAlbumDesdeCarpetaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem configToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelViewInfo;
        private System.Windows.Forms.ToolStripMenuItem spotifyStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem searchSpotifyStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem linkSpotifyStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importSpotifyStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showSidebarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewModeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem digitalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem vierCDToolStripMenuItem;
        private System.Windows.Forms.Panel panelSidebar;
        private System.Windows.Forms.PictureBox pictureBoxSidebarCover;
        private System.Windows.Forms.Label labelInfoAlbum;
        private System.Windows.Forms.Label labelGeneralInfo;
        private System.Windows.Forms.ContextMenuStrip contextMenuSidebarCover;
        private System.Windows.Forms.ToolStripMenuItem sidebarCopyImageToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStripMain;
        private System.Windows.Forms.ToolStripButton toolStripButtonNewAlbum;
        private System.Windows.Forms.ToolStripButton toolStripButtonNewDatabase;
        private System.Windows.Forms.ToolStripButton toolStripButtonSaveDatabase;
        private System.Windows.Forms.ToolStripButton toolStripButtonOpenDatabase;
        private System.Windows.Forms.ToolStripMenuItem filterToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButtonFilter;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBoxSearch;
        private System.Windows.Forms.ToolStripMenuItem vinylToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createVinylToolStripMenuItem;
    }
}