namespace Cassiopeia.src.Forms
{
    partial class FilterForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FilterForm));
            this.textBoxArtist = new System.Windows.Forms.TextBox();
            this.labelWriteFilter = new System.Windows.Forms.Label();
            this.buttonOk = new System.Windows.Forms.Button();
            this.labelArtist = new System.Windows.Forms.Label();
            this.buttonReset = new System.Windows.Forms.Button();
            this.labelTitle = new System.Windows.Forms.Label();
            this.textBoxAlbumName = new System.Windows.Forms.TextBox();
            this.labelSongTitle = new System.Windows.Forms.Label();
            this.textBoxSongTitle = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBoxArtist
            // 
            this.textBoxArtist.Location = new System.Drawing.Point(217, 51);
            this.textBoxArtist.Name = "textBoxArtist";
            this.textBoxArtist.Size = new System.Drawing.Size(196, 23);
            this.textBoxArtist.TabIndex = 0;
            // 
            // labelWriteFilter
            // 
            this.labelWriteFilter.AutoSize = true;
            this.labelWriteFilter.Location = new System.Drawing.Point(12, 9);
            this.labelWriteFilter.Name = "labelWriteFilter";
            this.labelWriteFilter.Size = new System.Drawing.Size(127, 15);
            this.labelWriteFilter.TabIndex = 1;
            this.labelWriteFilter.Text = "escribe algo para filtrar";
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(338, 191);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 3;
            this.buttonOk.Text = "Ok";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // labelArtist
            // 
            this.labelArtist.AutoSize = true;
            this.labelArtist.Location = new System.Drawing.Point(12, 54);
            this.labelArtist.Name = "labelArtist";
            this.labelArtist.Size = new System.Drawing.Size(39, 15);
            this.labelArtist.TabIndex = 4;
            this.labelArtist.Text = "artista";
            // 
            // buttonReset
            // 
            this.buttonReset.Location = new System.Drawing.Point(9, 191);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(75, 23);
            this.buttonReset.TabIndex = 5;
            this.buttonReset.Text = "reset";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Location = new System.Drawing.Point(12, 83);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(72, 15);
            this.labelTitle.TabIndex = 7;
            this.labelTitle.Text = "titulo album";
            // 
            // textBoxAlbumName
            // 
            this.textBoxAlbumName.Location = new System.Drawing.Point(217, 80);
            this.textBoxAlbumName.Name = "textBoxAlbumName";
            this.textBoxAlbumName.Size = new System.Drawing.Size(196, 23);
            this.textBoxAlbumName.TabIndex = 6;
            // 
            // labelSongTitle
            // 
            this.labelSongTitle.AutoSize = true;
            this.labelSongTitle.Location = new System.Drawing.Point(12, 112);
            this.labelSongTitle.Name = "labelSongTitle";
            this.labelSongTitle.Size = new System.Drawing.Size(136, 15);
            this.labelSongTitle.TabIndex = 9;
            this.labelSongTitle.Text = "titulo cancion(prioridad)";
            // 
            // textBoxSongTitle
            // 
            this.textBoxSongTitle.Location = new System.Drawing.Point(217, 109);
            this.textBoxSongTitle.Name = "textBoxSongTitle";
            this.textBoxSongTitle.Size = new System.Drawing.Size(196, 23);
            this.textBoxSongTitle.TabIndex = 8;
            // 
            // FilterForm
            // 
            this.AcceptButton = this.buttonOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(425, 226);
            this.Controls.Add(this.labelSongTitle);
            this.Controls.Add(this.textBoxSongTitle);
            this.Controls.Add(this.labelTitle);
            this.Controls.Add(this.textBoxAlbumName);
            this.Controls.Add(this.buttonReset);
            this.Controls.Add(this.labelArtist);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.labelWriteFilter);
            this.Controls.Add(this.textBoxArtist);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FilterForm";
            this.Text = "FilterForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxArtist;
        private System.Windows.Forms.Label labelWriteFilter;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Label labelArtist;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.TextBox textBoxAlbumName;
        private System.Windows.Forms.Label labelSongTitle;
        private System.Windows.Forms.TextBox textBoxSongTitle;
    }
}