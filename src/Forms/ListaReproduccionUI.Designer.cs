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
            this.SuspendLayout();
            // 
            // listViewCanciones
            // 
            this.listViewCanciones.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewCanciones.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnPlaying,
            this.columnName,
            this.columnDuration});
            this.listViewCanciones.FullRowSelect = true;
            this.listViewCanciones.HideSelection = false;
            this.listViewCanciones.Location = new System.Drawing.Point(0, 0);
            this.listViewCanciones.Name = "listViewCanciones";
            this.listViewCanciones.Size = new System.Drawing.Size(252, 254);
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
            // ListaReproduccionUI
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(340, 450);
            this.Controls.Add(this.listViewCanciones);
            this.Name = "ListaReproduccionUI";
            this.Text = "ListaReproduccionUI";
            this.SizeChanged += new System.EventHandler(this.ListaReproduccionUI_SizeChanged);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.ListaReproduccionUI_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.ListaReproduccionUI_DragEnter);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listViewCanciones;
        private System.Windows.Forms.ColumnHeader columnPlaying;
        private System.Windows.Forms.ColumnHeader columnName;
        private System.Windows.Forms.ColumnHeader columnDuration;
    }
}