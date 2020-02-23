namespace aplicacion_musica
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(visualizarAlbum));
            this.vistaCaratula = new System.Windows.Forms.PictureBox();
            this.vistaCanciones = new System.Windows.Forms.ListView();
            this.num = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.titulo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.duracion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.infoAlbum = new System.Windows.Forms.Label();
            this.okDoomerButton = new System.Windows.Forms.Button();
            this.editarButton = new System.Windows.Forms.Button();
            this.barraAbajo = new System.Windows.Forms.StatusStrip();
            this.duracionSeleccionada = new System.Windows.Forms.ToolStripStatusLabel();
            this.buttonAnotaciones = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.vistaCaratula)).BeginInit();
            this.barraAbajo.SuspendLayout();
            this.SuspendLayout();
            // 
            // vistaCaratula
            // 
            this.vistaCaratula.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.vistaCaratula.Location = new System.Drawing.Point(441, 11);
            this.vistaCaratula.Name = "vistaCaratula";
            this.vistaCaratula.Size = new System.Drawing.Size(360, 360);
            this.vistaCaratula.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.vistaCaratula.TabIndex = 0;
            this.vistaCaratula.TabStop = false;
            // 
            // vistaCanciones
            // 
            this.vistaCanciones.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.num,
            this.titulo,
            this.duracion});
            this.vistaCanciones.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vistaCanciones.FullRowSelect = true;
            this.vistaCanciones.HideSelection = false;
            this.vistaCanciones.Location = new System.Drawing.Point(11, 158);
            this.vistaCanciones.Name = "vistaCanciones";
            this.vistaCanciones.Size = new System.Drawing.Size(424, 213);
            this.vistaCanciones.TabIndex = 1;
            this.vistaCanciones.UseCompatibleStateImageBehavior = false;
            this.vistaCanciones.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.ordenarColumnas);
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
            // infoAlbum
            // 
            this.infoAlbum.AutoSize = true;
            this.infoAlbum.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.infoAlbum.Location = new System.Drawing.Point(7, 6);
            this.infoAlbum.Margin = new System.Windows.Forms.Padding(5);
            this.infoAlbum.Name = "infoAlbum";
            this.infoAlbum.Size = new System.Drawing.Size(50, 20);
            this.infoAlbum.TabIndex = 2;
            this.infoAlbum.Text = "label1";
            // 
            // okDoomerButton
            // 
            this.okDoomerButton.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.okDoomerButton.Location = new System.Drawing.Point(316, 376);
            this.okDoomerButton.Name = "okDoomerButton";
            this.okDoomerButton.Size = new System.Drawing.Size(177, 43);
            this.okDoomerButton.TabIndex = 3;
            this.okDoomerButton.Text = "button1";
            this.okDoomerButton.UseVisualStyleBackColor = true;
            this.okDoomerButton.Click += new System.EventHandler(this.okDoomerButton_Click);
            // 
            // editarButton
            // 
            this.editarButton.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.editarButton.Location = new System.Drawing.Point(700, 377);
            this.editarButton.Name = "editarButton";
            this.editarButton.Size = new System.Drawing.Size(95, 43);
            this.editarButton.TabIndex = 3;
            this.editarButton.Text = "editar";
            this.editarButton.UseVisualStyleBackColor = true;
            this.editarButton.Click += new System.EventHandler(this.editarButton_Click);
            // 
            // barraAbajo
            // 
            this.barraAbajo.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.duracionSeleccionada});
            this.barraAbajo.Location = new System.Drawing.Point(0, 423);
            this.barraAbajo.Name = "barraAbajo";
            this.barraAbajo.Size = new System.Drawing.Size(807, 22);
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
            this.buttonAnotaciones.Location = new System.Drawing.Point(12, 377);
            this.buttonAnotaciones.Name = "buttonAnotaciones";
            this.buttonAnotaciones.Size = new System.Drawing.Size(117, 43);
            this.buttonAnotaciones.TabIndex = 5;
            this.buttonAnotaciones.Text = "button1";
            this.buttonAnotaciones.UseVisualStyleBackColor = true;
            this.buttonAnotaciones.Click += new System.EventHandler(this.buttonAnotaciones_Click);
            // 
            // visualizarAlbum
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(807, 445);
            this.Controls.Add(this.buttonAnotaciones);
            this.Controls.Add(this.barraAbajo);
            this.Controls.Add(this.editarButton);
            this.Controls.Add(this.okDoomerButton);
            this.Controls.Add(this.infoAlbum);
            this.Controls.Add(this.vistaCanciones);
            this.Controls.Add(this.vistaCaratula);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "visualizarAlbum";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "visualizarAlbum";
            ((System.ComponentModel.ISupportInitialize)(this.vistaCaratula)).EndInit();
            this.barraAbajo.ResumeLayout(false);
            this.barraAbajo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox vistaCaratula;
        private System.Windows.Forms.ListView vistaCanciones;
        private System.Windows.Forms.Label infoAlbum;
        private System.Windows.Forms.Button okDoomerButton;
        private System.Windows.Forms.Button editarButton;
        private System.Windows.Forms.ColumnHeader num;
        private System.Windows.Forms.ColumnHeader titulo;
        private System.Windows.Forms.ColumnHeader duracion;
        private System.Windows.Forms.StatusStrip barraAbajo;
        private System.Windows.Forms.ToolStripStatusLabel duracionSeleccionada;
        private System.Windows.Forms.Button buttonAnotaciones;
    }
}