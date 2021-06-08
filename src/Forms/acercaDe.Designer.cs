namespace Cassiopeia
{
    partial class acercaDe
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(acercaDe));
            this.labelAcercaDe = new System.Windows.Forms.Label();
            this.pictureBoxBanner = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBanner)).BeginInit();
            this.SuspendLayout();
            // 
            // labelAcercaDe
            // 
            this.labelAcercaDe.AutoSize = true;
            this.labelAcercaDe.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelAcercaDe.Location = new System.Drawing.Point(0, 192);
            this.labelAcercaDe.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelAcercaDe.Name = "labelAcercaDe";
            this.labelAcercaDe.Size = new System.Drawing.Size(393, 189);
            this.labelAcercaDe.TabIndex = 0;
            this.labelAcercaDe.Text = resources.GetString("labelAcercaDe.Text");
            this.labelAcercaDe.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBoxBanner
            // 
            this.pictureBoxBanner.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxBanner.ErrorImage = null;
            this.pictureBoxBanner.Image = global::Cassiopeia.Properties.Resources.banner1_7;
            this.pictureBoxBanner.InitialImage = null;
            this.pictureBoxBanner.Location = new System.Drawing.Point(0, -2);
            this.pictureBoxBanner.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pictureBoxBanner.Name = "pictureBoxBanner";
            this.pictureBoxBanner.Size = new System.Drawing.Size(549, 190);
            this.pictureBoxBanner.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxBanner.TabIndex = 1;
            this.pictureBoxBanner.TabStop = false;
            // 
            // acercaDe
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(547, 458);
            this.Controls.Add(this.pictureBoxBanner);
            this.Controls.Add(this.labelAcercaDe);
            this.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "acercaDe";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "acercaDe";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.acercaDe_KeyDown);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.acercaDe_PreviewKeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBanner)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelAcercaDe;
        private System.Windows.Forms.PictureBox pictureBoxBanner;
    }
}