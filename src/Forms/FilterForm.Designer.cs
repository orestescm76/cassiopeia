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
            this.textBoxArtist = new System.Windows.Forms.TextBox();
            this.labelWriteFilter = new System.Windows.Forms.Label();
            this.buttonOk = new System.Windows.Forms.Button();
            this.labelArtist = new System.Windows.Forms.Label();
            this.buttonReset = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBoxArtist
            // 
            this.textBoxArtist.Location = new System.Drawing.Point(180, 43);
            this.textBoxArtist.Name = "textBoxArtist";
            this.textBoxArtist.Size = new System.Drawing.Size(233, 23);
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
            this.buttonOk.Location = new System.Drawing.Point(338, 148);
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
            this.labelArtist.Location = new System.Drawing.Point(12, 43);
            this.labelArtist.Name = "labelArtist";
            this.labelArtist.Size = new System.Drawing.Size(39, 15);
            this.labelArtist.TabIndex = 4;
            this.labelArtist.Text = "artista";
            // 
            // buttonReset
            // 
            this.buttonReset.Location = new System.Drawing.Point(12, 148);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(75, 23);
            this.buttonReset.TabIndex = 5;
            this.buttonReset.Text = "reset";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // FilterForm
            // 
            this.AcceptButton = this.buttonOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(425, 183);
            this.Controls.Add(this.buttonReset);
            this.Controls.Add(this.labelArtist);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.labelWriteFilter);
            this.Controls.Add(this.textBoxArtist);
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
    }
}