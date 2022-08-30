namespace Cassiopeia.src.Forms
{
    partial class About
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(About));
            this.labelAcercaDe = new System.Windows.Forms.Label();
            this.pictureBoxBanner = new System.Windows.Forms.PictureBox();
            this.labelBTC = new System.Windows.Forms.Label();
            this.pictureBoxBTC = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBanner)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBTC)).BeginInit();
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
            this.pictureBoxBanner.Image = global::Cassiopeia.Properties.Resources.banner_thalassa;
            this.pictureBoxBanner.InitialImage = null;
            this.pictureBoxBanner.Location = new System.Drawing.Point(0, -2);
            this.pictureBoxBanner.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pictureBoxBanner.Name = "pictureBoxBanner";
            this.pictureBoxBanner.Size = new System.Drawing.Size(549, 190);
            this.pictureBoxBanner.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxBanner.TabIndex = 1;
            this.pictureBoxBanner.TabStop = false;
            // 
            // labelBTC
            // 
            this.labelBTC.AutoSize = true;
            this.labelBTC.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelBTC.Location = new System.Drawing.Point(111, 415);
            this.labelBTC.Name = "labelBTC";
            this.labelBTC.Size = new System.Drawing.Size(358, 25);
            this.labelBTC.TabIndex = 2;
            this.labelBTC.Text = "19xFr3GVwEThYsekRvoN8xaTuipNxKUrSv";
            // 
            // pictureBoxBTC
            // 
            this.pictureBoxBTC.Image = global::Cassiopeia.Properties.Resources.btc;
            this.pictureBoxBTC.InitialImage = global::Cassiopeia.Properties.Resources.btc;
            this.pictureBoxBTC.Location = new System.Drawing.Point(28, 399);
            this.pictureBoxBTC.Name = "pictureBoxBTC";
            this.pictureBoxBTC.Size = new System.Drawing.Size(59, 56);
            this.pictureBoxBTC.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxBTC.TabIndex = 3;
            this.pictureBoxBTC.TabStop = false;
            // 
            // About
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(547, 467);
            this.Controls.Add(this.pictureBoxBTC);
            this.Controls.Add(this.labelBTC);
            this.Controls.Add(this.pictureBoxBanner);
            this.Controls.Add(this.labelAcercaDe);
            this.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "About";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "acercaDe";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.acercaDe_KeyDown);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.acercaDe_PreviewKeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBanner)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBTC)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelAcercaDe;
        private System.Windows.Forms.PictureBox pictureBoxBanner;
        private System.Windows.Forms.Label labelBTC;
        private System.Windows.Forms.PictureBox pictureBoxBTC;
    }
}