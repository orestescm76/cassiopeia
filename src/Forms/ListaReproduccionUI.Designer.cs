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
            this.dataGridViewCanciones = new System.Windows.Forms.DataGridView();
            this.indexColumn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.nameColumn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.durationColumn = new System.Windows.Forms.DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCanciones)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewCanciones
            // 
            this.dataGridViewCanciones.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCanciones.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.indexColumn,
            this.nameColumn,
            this.durationColumn});
            this.dataGridViewCanciones.Location = new System.Drawing.Point(0, 2);
            this.dataGridViewCanciones.Name = "dataGridViewCanciones";
            this.dataGridViewCanciones.Size = new System.Drawing.Size(340, 450);
            this.dataGridViewCanciones.TabIndex = 0;
            this.dataGridViewCanciones.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewCanciones_CellContentClick);
            // 
            // indexColumn
            // 
            this.indexColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.indexColumn.Frozen = true;
            this.indexColumn.HeaderText = "#";
            this.indexColumn.Name = "indexColumn";
            this.indexColumn.ReadOnly = true;
            this.indexColumn.Width = 21;
            // 
            // nameColumn
            // 
            this.nameColumn.HeaderText = "name";
            this.nameColumn.Name = "nameColumn";
            this.nameColumn.ReadOnly = true;
            this.nameColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.nameColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // durationColumn
            // 
            this.durationColumn.HeaderText = "duracion";
            this.durationColumn.Name = "durationColumn";
            this.durationColumn.ReadOnly = true;
            // 
            // ListaReproduccionUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(340, 450);
            this.Controls.Add(this.dataGridViewCanciones);
            this.Name = "ListaReproduccionUI";
            this.Text = "ListaReproduccionUI";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCanciones)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewCanciones;
        private System.Windows.Forms.DataGridViewButtonColumn indexColumn;
        private System.Windows.Forms.DataGridViewButtonColumn nameColumn;
        private System.Windows.Forms.DataGridViewButtonColumn durationColumn;
    }
}