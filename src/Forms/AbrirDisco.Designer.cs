namespace Cassiopea.src.Forms
{
    partial class AbrirDisco
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
            this.listViewDiscos = new System.Windows.Forms.ListView();
            this.buttonPlay = new System.Windows.Forms.Button();
            this.buttonRip = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listViewDiscos
            // 
            this.listViewDiscos.HideSelection = false;
            this.listViewDiscos.Location = new System.Drawing.Point(12, 29);
            this.listViewDiscos.MultiSelect = false;
            this.listViewDiscos.Name = "listViewDiscos";
            this.listViewDiscos.Size = new System.Drawing.Size(364, 162);
            this.listViewDiscos.TabIndex = 0;
            this.listViewDiscos.UseCompatibleStateImageBehavior = false;
            // 
            // buttonPlay
            // 
            this.buttonPlay.Location = new System.Drawing.Point(12, 197);
            this.buttonPlay.Name = "buttonPlay";
            this.buttonPlay.Size = new System.Drawing.Size(176, 23);
            this.buttonPlay.TabIndex = 1;
            this.buttonPlay.Text = "play";
            this.buttonPlay.UseVisualStyleBackColor = true;
            this.buttonPlay.Click += new System.EventHandler(this.buttonPlay_Click);
            // 
            // buttonRip
            // 
            this.buttonRip.Location = new System.Drawing.Point(211, 197);
            this.buttonRip.Name = "buttonRip";
            this.buttonRip.Size = new System.Drawing.Size(165, 23);
            this.buttonRip.TabIndex = 2;
            this.buttonRip.Text = "rip";
            this.buttonRip.UseVisualStyleBackColor = true;
            // 
            // AbrirDisco
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(388, 227);
            this.Controls.Add(this.buttonRip);
            this.Controls.Add(this.buttonPlay);
            this.Controls.Add(this.listViewDiscos);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "AbrirDisco";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AbrirDisco";
            this.Load += new System.EventHandler(this.AbrirDisco_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listViewDiscos;
        private System.Windows.Forms.Button buttonPlay;
        private System.Windows.Forms.Button buttonRip;
    }
}