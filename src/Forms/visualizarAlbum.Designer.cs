namespace Cassiopeia
{
    partial class visualizarAlbum
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(visualizarAlbum));
            this.vistaCaratula = new System.Windows.Forms.PictureBox();
            this.clickDerechoCover = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copiarImagenStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.vistaCanciones = new System.Windows.Forms.ListView();
            this.num = new System.Windows.Forms.ColumnHeader();
            this.titulo = new System.Windows.Forms.ColumnHeader();
            this.duracion = new System.Windows.Forms.ColumnHeader();
            this.clickDerechoConfig = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.setBonusToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reproducirspotifyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reproducirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.verLyricsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fusionarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.defusionarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.labelInfoAlbum = new System.Windows.Forms.Label();
            this.okDoomerButton = new System.Windows.Forms.Button();
            this.editarButton = new System.Windows.Forms.Button();
            this.barraAbajo = new System.Windows.Forms.StatusStrip();
            this.duracionSeleccionada = new System.Windows.Forms.ToolStripStatusLabel();
            this.buttonAnotaciones = new System.Windows.Forms.Button();
            this.labelEstadoDisco = new System.Windows.Forms.Label();
            this.buttonPATH = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.vistaCaratula)).BeginInit();
            this.clickDerechoCover.SuspendLayout();
            this.clickDerechoConfig.SuspendLayout();
            this.barraAbajo.SuspendLayout();
            this.SuspendLayout();
            // 
            // vistaCaratula
            // 
            this.vistaCaratula.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.vistaCaratula.ContextMenuStrip = this.clickDerechoCover;
            this.vistaCaratula.Location = new System.Drawing.Point(516, 9);
            this.vistaCaratula.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.vistaCaratula.Name = "vistaCaratula";
            this.vistaCaratula.Size = new System.Drawing.Size(448, 448);
            this.vistaCaratula.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.vistaCaratula.TabIndex = 0;
            this.vistaCaratula.TabStop = false;
            // 
            // clickDerechoCover
            // 
            this.clickDerechoCover.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copiarImagenStrip});
            this.clickDerechoCover.Name = "clickDerechoAlbum";
            this.clickDerechoCover.Size = new System.Drawing.Size(172, 26);
            // 
            // copiarImagenStrip
            // 
            this.copiarImagenStrip.Name = "copiarImagenStrip";
            this.copiarImagenStrip.Size = new System.Drawing.Size(171, 22);
            this.copiarImagenStrip.Text = "copiarImagenStrip";
            this.copiarImagenStrip.Click += new System.EventHandler(this.copiar_Click);
            // 
            // vistaCanciones
            // 
            this.vistaCanciones.AllowDrop = true;
            this.vistaCanciones.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.num,
            this.titulo,
            this.duracion});
            this.vistaCanciones.ContextMenuStrip = this.clickDerechoConfig;
            this.vistaCanciones.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.vistaCanciones.FullRowSelect = true;
            this.vistaCanciones.HideSelection = false;
            this.vistaCanciones.Location = new System.Drawing.Point(13, 238);
            this.vistaCanciones.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.vistaCanciones.Name = "vistaCanciones";
            this.vistaCanciones.Size = new System.Drawing.Size(494, 214);
            this.vistaCanciones.TabIndex = 1;
            this.vistaCanciones.TabStop = false;
            this.vistaCanciones.UseCompatibleStateImageBehavior = false;
            this.vistaCanciones.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.ordenarColumnas);
            this.vistaCanciones.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.vistaCanciones_ItemDrag);
            this.vistaCanciones.SelectedIndexChanged += new System.EventHandler(this.vistaCanciones_SelectedIndexChanged_1);
            this.vistaCanciones.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.vistaCanciones_MouseDoubleClick);
            // 
            // num
            // 
            this.num.Width = 22;
            // 
            // titulo
            // 
            this.titulo.Width = 290;
            // 
            // duracion
            // 
            this.duracion.Width = 72;
            // 
            // clickDerechoConfig
            // 
            this.clickDerechoConfig.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setBonusToolStripMenuItem,
            this.reproducirspotifyToolStripMenuItem,
            this.reproducirToolStripMenuItem,
            this.verLyricsToolStripMenuItem,
            this.fusionarToolStripMenuItem,
            this.defusionarToolStripMenuItem});
            this.clickDerechoConfig.Name = "clickDerechoConfig";
            this.clickDerechoConfig.Size = new System.Drawing.Size(171, 136);
            this.clickDerechoConfig.Opening += new System.ComponentModel.CancelEventHandler(this.clickDerechoConfig_Opening);
            // 
            // setBonusToolStripMenuItem
            // 
            this.setBonusToolStripMenuItem.Name = "setBonusToolStripMenuItem";
            this.setBonusToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.setBonusToolStripMenuItem.Text = "setBonus";
            this.setBonusToolStripMenuItem.Click += new System.EventHandler(this.setBonusToolStripMenuItem_Click);
            // 
            // reproducirspotifyToolStripMenuItem
            // 
            this.reproducirspotifyToolStripMenuItem.Name = "reproducirspotifyToolStripMenuItem";
            this.reproducirspotifyToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.reproducirspotifyToolStripMenuItem.Text = "reproducir_spotify";
            this.reproducirspotifyToolStripMenuItem.Click += new System.EventHandler(this.reproducirspotifyToolStripMenuItem_Click);
            // 
            // reproducirToolStripMenuItem
            // 
            this.reproducirToolStripMenuItem.Name = "reproducirToolStripMenuItem";
            this.reproducirToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.reproducirToolStripMenuItem.Text = "reproducir";
            this.reproducirToolStripMenuItem.Click += new System.EventHandler(this.reproducirToolStripMenuItem_Click);
            // 
            // verLyricsToolStripMenuItem
            // 
            this.verLyricsToolStripMenuItem.Name = "verLyricsToolStripMenuItem";
            this.verLyricsToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.verLyricsToolStripMenuItem.Text = "verLyrics";
            this.verLyricsToolStripMenuItem.Click += new System.EventHandler(this.verLyricsToolStripMenuItem_Click);
            // 
            // fusionarToolStripMenuItem
            // 
            this.fusionarToolStripMenuItem.Name = "fusionarToolStripMenuItem";
            this.fusionarToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.fusionarToolStripMenuItem.Text = "fusionar";
            this.fusionarToolStripMenuItem.Click += new System.EventHandler(this.fusionarToolStripMenuItem_Click);
            // 
            // defusionarToolStripMenuItem
            // 
            this.defusionarToolStripMenuItem.Name = "defusionarToolStripMenuItem";
            this.defusionarToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.defusionarToolStripMenuItem.Text = "defusionar";
            this.defusionarToolStripMenuItem.Click += new System.EventHandler(this.defusionarToolStripMenuItem_Click);
            // 
            // labelInfoAlbum
            // 
            this.labelInfoAlbum.AutoSize = true;
            this.labelInfoAlbum.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelInfoAlbum.Location = new System.Drawing.Point(9, 9);
            this.labelInfoAlbum.Margin = new System.Windows.Forms.Padding(6);
            this.labelInfoAlbum.Name = "labelInfoAlbum";
            this.labelInfoAlbum.Size = new System.Drawing.Size(191, 171);
            this.labelInfoAlbum.TabIndex = 2;
            this.labelInfoAlbum.Text = "Artista: test\r\nTitulo: test\r\nAño: 2020\r\nDuración: 01:02:03 (00:04:05)\r\nGénero: Re" +
    "lleno\r\nFormato: Jewel Case\r\nAño publicación: 2020\r\nPaís: Unión Europea\r\nEstado e" +
    "xterior: M (Nuevo)\r\n";
            this.labelInfoAlbum.Click += new System.EventHandler(this.infoAlbum_Click);
            // 
            // okDoomerButton
            // 
            this.okDoomerButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.okDoomerButton.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.okDoomerButton.Location = new System.Drawing.Point(398, 469);
            this.okDoomerButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.okDoomerButton.Name = "okDoomerButton";
            this.okDoomerButton.Size = new System.Drawing.Size(206, 36);
            this.okDoomerButton.TabIndex = 3;
            this.okDoomerButton.Text = "button1";
            this.okDoomerButton.UseVisualStyleBackColor = true;
            this.okDoomerButton.Click += new System.EventHandler(this.okDoomerButton_Click);
            // 
            // editarButton
            // 
            this.editarButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.editarButton.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.editarButton.Location = new System.Drawing.Point(854, 469);
            this.editarButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.editarButton.Name = "editarButton";
            this.editarButton.Size = new System.Drawing.Size(111, 36);
            this.editarButton.TabIndex = 3;
            this.editarButton.Text = "editar";
            this.editarButton.UseVisualStyleBackColor = true;
            this.editarButton.Click += new System.EventHandler(this.editarButton_Click);
            // 
            // barraAbajo
            // 
            this.barraAbajo.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.duracionSeleccionada});
            this.barraAbajo.Location = new System.Drawing.Point(0, 517);
            this.barraAbajo.Name = "barraAbajo";
            this.barraAbajo.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.barraAbajo.Size = new System.Drawing.Size(972, 22);
            this.barraAbajo.TabIndex = 4;
            this.barraAbajo.Text = "statusStrip1";
            // 
            // duracionSeleccionada
            // 
            this.duracionSeleccionada.Name = "duracionSeleccionada";
            this.duracionSeleccionada.Size = new System.Drawing.Size(118, 17);
            this.duracionSeleccionada.Text = "toolStripStatusLabel1";
            // 
            // buttonAnotaciones
            // 
            this.buttonAnotaciones.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonAnotaciones.Location = new System.Drawing.Point(14, 469);
            this.buttonAnotaciones.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonAnotaciones.Name = "buttonAnotaciones";
            this.buttonAnotaciones.Size = new System.Drawing.Size(150, 36);
            this.buttonAnotaciones.TabIndex = 5;
            this.buttonAnotaciones.Text = "button1";
            this.buttonAnotaciones.UseVisualStyleBackColor = true;
            this.buttonAnotaciones.Click += new System.EventHandler(this.buttonAnotaciones_Click);
            // 
            // labelEstadoDisco
            // 
            this.labelEstadoDisco.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelEstadoDisco.AutoSize = true;
            this.labelEstadoDisco.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelEstadoDisco.Location = new System.Drawing.Point(9, 210);
            this.labelEstadoDisco.Margin = new System.Windows.Forms.Padding(6);
            this.labelEstadoDisco.Name = "labelEstadoDisco";
            this.labelEstadoDisco.Size = new System.Drawing.Size(223, 19);
            this.labelEstadoDisco.TabIndex = 6;
            this.labelEstadoDisco.Text = "Estado del disco X: VG+ (Excelente)";
            this.labelEstadoDisco.Click += new System.EventHandler(this.labelEstadoDisco_Click);
            // 
            // buttonPATH
            // 
            this.buttonPATH.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonPATH.Location = new System.Drawing.Point(172, 469);
            this.buttonPATH.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonPATH.Name = "buttonPATH";
            this.buttonPATH.Size = new System.Drawing.Size(153, 36);
            this.buttonPATH.TabIndex = 7;
            this.buttonPATH.Text = "calcularPATHS";
            this.buttonPATH.UseVisualStyleBackColor = true;
            this.buttonPATH.Click += new System.EventHandler(this.buttonPATH_Click);
            // 
            // visualizarAlbum
            // 
            this.AcceptButton = this.okDoomerButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(972, 539);
            this.Controls.Add(this.buttonPATH);
            this.Controls.Add(this.vistaCaratula);
            this.Controls.Add(this.labelEstadoDisco);
            this.Controls.Add(this.buttonAnotaciones);
            this.Controls.Add(this.barraAbajo);
            this.Controls.Add(this.editarButton);
            this.Controls.Add(this.okDoomerButton);
            this.Controls.Add(this.vistaCanciones);
            this.Controls.Add(this.labelInfoAlbum);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(988, 578);
            this.Name = "visualizarAlbum";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "visualizarAlbum";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.visualizarAlbum_KeyDown);
            this.Resize += new System.EventHandler(this.visualizarAlbum_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.vistaCaratula)).EndInit();
            this.clickDerechoCover.ResumeLayout(false);
            this.clickDerechoConfig.ResumeLayout(false);
            this.barraAbajo.ResumeLayout(false);
            this.barraAbajo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox vistaCaratula;
        private System.Windows.Forms.ListView vistaCanciones;
        private System.Windows.Forms.Label labelInfoAlbum;
        private System.Windows.Forms.Button okDoomerButton;
        private System.Windows.Forms.Button editarButton;
        private System.Windows.Forms.ColumnHeader num;
        private System.Windows.Forms.ColumnHeader titulo;
        private System.Windows.Forms.ColumnHeader duracion;
        private System.Windows.Forms.StatusStrip barraAbajo;
        private System.Windows.Forms.ToolStripStatusLabel duracionSeleccionada;
        private System.Windows.Forms.Button buttonAnotaciones;
        private System.Windows.Forms.Label labelEstadoDisco;
        private System.Windows.Forms.ContextMenuStrip clickDerechoConfig;
        private System.Windows.Forms.ToolStripMenuItem setBonusToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reproducirspotifyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reproducirToolStripMenuItem;
        private System.Windows.Forms.Button buttonPATH;
        private System.Windows.Forms.ToolStripMenuItem verLyricsToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip clickDerechoCover;
        private System.Windows.Forms.ToolStripMenuItem copiarImagenStrip;
        private System.Windows.Forms.ToolStripMenuItem fusionarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem defusionarToolStripMenuItem;
    }
}