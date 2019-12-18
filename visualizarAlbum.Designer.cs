namespace aplicacion_ipo
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
            this.infoAlbum = new System.Windows.Forms.Label();
            this.okDoomerButton = new System.Windows.Forms.Button();
            this.editarButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.vistaCaratula)).BeginInit();
            this.SuspendLayout();
            // 
            // vistaCaratula
            // 
            this.vistaCaratula.Location = new System.Drawing.Point(441, 6);
            this.vistaCaratula.Name = "vistaCaratula";
            this.vistaCaratula.Size = new System.Drawing.Size(300, 300);
            this.vistaCaratula.TabIndex = 0;
            this.vistaCaratula.TabStop = false;
            // 
            // vistaCanciones
            // 
            this.vistaCanciones.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vistaCanciones.HideSelection = false;
            this.vistaCanciones.Location = new System.Drawing.Point(11, 161);
            this.vistaCanciones.Name = "vistaCanciones";
            this.vistaCanciones.Size = new System.Drawing.Size(424, 145);
            this.vistaCanciones.TabIndex = 1;
            this.vistaCanciones.UseCompatibleStateImageBehavior = false;
            this.vistaCanciones.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.ordenarColumnas);
            // 
            // infoAlbum
            // 
            this.infoAlbum.AutoSize = true;
            this.infoAlbum.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.infoAlbum.Location = new System.Drawing.Point(8, 12);
            this.infoAlbum.Margin = new System.Windows.Forms.Padding(5);
            this.infoAlbum.Name = "infoAlbum";
            this.infoAlbum.Size = new System.Drawing.Size(43, 17);
            this.infoAlbum.TabIndex = 2;
            this.infoAlbum.Text = "label1";
            // 
            // okDoomerButton
            // 
            this.okDoomerButton.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.okDoomerButton.Location = new System.Drawing.Point(274, 325);
            this.okDoomerButton.Name = "okDoomerButton";
            this.okDoomerButton.Size = new System.Drawing.Size(237, 30);
            this.okDoomerButton.TabIndex = 3;
            this.okDoomerButton.Text = "button1";
            this.okDoomerButton.UseVisualStyleBackColor = true;
            this.okDoomerButton.Click += new System.EventHandler(this.okDoomerButton_Click);
            // 
            // editarButton
            // 
            this.editarButton.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.editarButton.Location = new System.Drawing.Point(657, 325);
            this.editarButton.Name = "editarButton";
            this.editarButton.Size = new System.Drawing.Size(84, 30);
            this.editarButton.TabIndex = 3;
            this.editarButton.Text = "editar";
            this.editarButton.UseVisualStyleBackColor = true;
            this.editarButton.Click += new System.EventHandler(this.editarButton_Click);
            // 
            // visualizarAlbum
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(753, 367);
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
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox vistaCaratula;
        private System.Windows.Forms.ListView vistaCanciones;
        private System.Windows.Forms.Label infoAlbum;
        private System.Windows.Forms.Button okDoomerButton;
        private System.Windows.Forms.Button editarButton;
    }
}