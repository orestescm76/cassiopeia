namespace Cassiopeia.src.Forms
{
    partial class LyricsViewer
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
            this.textBoxLyrics = new System.Windows.Forms.TextBox();
            this.buttonEditar = new System.Windows.Forms.Button();
            this.buttonLimpiar = new System.Windows.Forms.Button();
            this.buttonBuscar = new System.Windows.Forms.Button();
            this.buttonCerrar = new System.Windows.Forms.Button();
            this.buttonDeshacer = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonBack = new System.Windows.Forms.Button();
            this.buttonNext = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxLyrics
            // 
            this.textBoxLyrics.AcceptsReturn = true;
            this.textBoxLyrics.AllowDrop = true;
            this.textBoxLyrics.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxLyrics.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textBoxLyrics.Location = new System.Drawing.Point(14, 14);
            this.textBoxLyrics.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.textBoxLyrics.Multiline = true;
            this.textBoxLyrics.Name = "textBoxLyrics";
            this.textBoxLyrics.ReadOnly = true;
            this.textBoxLyrics.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxLyrics.Size = new System.Drawing.Size(495, 416);
            this.textBoxLyrics.TabIndex = 0;
            // 
            // buttonEditar
            // 
            this.buttonEditar.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.buttonEditar.Location = new System.Drawing.Point(221, 3);
            this.buttonEditar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonEditar.Name = "buttonEditar";
            this.buttonEditar.Size = new System.Drawing.Size(88, 39);
            this.buttonEditar.TabIndex = 1;
            this.buttonEditar.Text = "buttonEditar";
            this.buttonEditar.UseVisualStyleBackColor = true;
            this.buttonEditar.Click += new System.EventHandler(this.buttonEditar_Click);
            // 
            // buttonLimpiar
            // 
            this.buttonLimpiar.Location = new System.Drawing.Point(4, 3);
            this.buttonLimpiar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonLimpiar.Name = "buttonLimpiar";
            this.buttonLimpiar.Size = new System.Drawing.Size(88, 39);
            this.buttonLimpiar.TabIndex = 2;
            this.buttonLimpiar.Text = "buttonLimpiar";
            this.buttonLimpiar.UseVisualStyleBackColor = true;
            this.buttonLimpiar.Click += new System.EventHandler(this.buttonLimpiar_Click);
            // 
            // buttonBuscar
            // 
            this.buttonBuscar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonBuscar.Location = new System.Drawing.Point(433, 3);
            this.buttonBuscar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonBuscar.Name = "buttonBuscar";
            this.buttonBuscar.Size = new System.Drawing.Size(88, 39);
            this.buttonBuscar.TabIndex = 3;
            this.buttonBuscar.Text = "buttonBuscar";
            this.buttonBuscar.UseVisualStyleBackColor = true;
            this.buttonBuscar.Click += new System.EventHandler(this.buttonBuscar_Click);
            // 
            // buttonCerrar
            // 
            this.buttonCerrar.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonCerrar.Location = new System.Drawing.Point(221, 60);
            this.buttonCerrar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonCerrar.Name = "buttonCerrar";
            this.buttonCerrar.Size = new System.Drawing.Size(88, 39);
            this.buttonCerrar.TabIndex = 4;
            this.buttonCerrar.Text = "buttonCerrar";
            this.buttonCerrar.UseVisualStyleBackColor = true;
            this.buttonCerrar.Click += new System.EventHandler(this.buttonCerrar_Click);
            // 
            // buttonDeshacer
            // 
            this.buttonDeshacer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonDeshacer.Location = new System.Drawing.Point(4, 60);
            this.buttonDeshacer.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonDeshacer.Name = "buttonDeshacer";
            this.buttonDeshacer.Size = new System.Drawing.Size(88, 39);
            this.buttonDeshacer.TabIndex = 5;
            this.buttonDeshacer.Text = "buttonDeshacer";
            this.buttonDeshacer.UseVisualStyleBackColor = true;
            this.buttonDeshacer.Click += new System.EventHandler(this.buttonDeshacer_Click);
            // 
            // panel1
            // 
            this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel1.Controls.Add(this.buttonBack);
            this.panel1.Controls.Add(this.buttonNext);
            this.panel1.Controls.Add(this.buttonCerrar);
            this.panel1.Controls.Add(this.buttonDeshacer);
            this.panel1.Controls.Add(this.buttonBuscar);
            this.panel1.Controls.Add(this.buttonEditar);
            this.panel1.Controls.Add(this.buttonLimpiar);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 436);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(524, 103);
            this.panel1.TabIndex = 6;
            // 
            // buttonBack
            // 
            this.buttonBack.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.buttonBack.Location = new System.Drawing.Point(125, 3);
            this.buttonBack.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.Size = new System.Drawing.Size(88, 39);
            this.buttonBack.TabIndex = 7;
            this.buttonBack.Text = "buttonPrevious";
            this.buttonBack.UseVisualStyleBackColor = true;
            this.buttonBack.Click += new System.EventHandler(this.buttonBack_Click);
            // 
            // buttonNext
            // 
            this.buttonNext.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.buttonNext.Location = new System.Drawing.Point(317, 3);
            this.buttonNext.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(88, 39);
            this.buttonNext.TabIndex = 6;
            this.buttonNext.Text = "buttonNext";
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
            // 
            // LyricsViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 539);
            this.Controls.Add(this.textBoxLyrics);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MinimumSize = new System.Drawing.Size(540, 578);
            this.Name = "LyricsViewer";
            this.Text = "VisorLyrics";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxLyrics;
        private System.Windows.Forms.Button buttonEditar;
        private System.Windows.Forms.Button buttonLimpiar;
        private System.Windows.Forms.Button buttonBuscar;
        private System.Windows.Forms.Button buttonCerrar;
        private System.Windows.Forms.Button buttonDeshacer;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonBack;
        private System.Windows.Forms.Button buttonNext;
    }
}