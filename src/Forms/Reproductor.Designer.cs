namespace aplicacion_musica
{
    partial class Reproductor
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
            this.components = new System.ComponentModel.Container();
            this.pictureBoxCaratula = new System.Windows.Forms.PictureBox();
            this.trackBarPosicion = new System.Windows.Forms.TrackBar();
            this.buttonReproducirPausar = new System.Windows.Forms.Button();
            this.timerCancion = new System.Windows.Forms.Timer(this.components);
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.button2 = new System.Windows.Forms.Button();
            this.labelPosicion = new System.Windows.Forms.Label();
            this.labelDuracion = new System.Windows.Forms.Label();
            this.barraAbajoDatos = new System.Windows.Forms.StatusStrip();
            this.labelDatosCancion = new System.Windows.Forms.ToolStripStatusLabel();
            this.trackBarVolumen = new System.Windows.Forms.TrackBar();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCaratula)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarPosicion)).BeginInit();
            this.barraAbajoDatos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarVolumen)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxCaratula
            // 
            this.pictureBoxCaratula.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxCaratula.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBoxCaratula.Name = "pictureBoxCaratula";
            this.pictureBoxCaratula.Size = new System.Drawing.Size(300, 300);
            this.pictureBoxCaratula.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxCaratula.TabIndex = 0;
            this.pictureBoxCaratula.TabStop = false;
            // 
            // trackBarPosicion
            // 
            this.trackBarPosicion.Location = new System.Drawing.Point(0, 303);
            this.trackBarPosicion.Maximum = 500;
            this.trackBarPosicion.Name = "trackBarPosicion";
            this.trackBarPosicion.Size = new System.Drawing.Size(300, 45);
            this.trackBarPosicion.TabIndex = 1;
            this.trackBarPosicion.TickFrequency = 0;
            this.trackBarPosicion.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBarPosicion.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            this.trackBarPosicion.MouseDown += new System.Windows.Forms.MouseEventHandler(this.trackBarPosicion_MouseDown);
            this.trackBarPosicion.MouseUp += new System.Windows.Forms.MouseEventHandler(this.trackBarPosicion_MouseUp);
            // 
            // buttonReproducirPausar
            // 
            this.buttonReproducirPausar.Font = new System.Drawing.Font("Calibri", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.buttonReproducirPausar.Location = new System.Drawing.Point(116, 354);
            this.buttonReproducirPausar.Name = "buttonReproducirPausar";
            this.buttonReproducirPausar.Size = new System.Drawing.Size(50, 50);
            this.buttonReproducirPausar.TabIndex = 2;
            this.buttonReproducirPausar.Text = "▶";
            this.buttonReproducirPausar.UseVisualStyleBackColor = true;
            this.buttonReproducirPausar.Click += new System.EventHandler(this.buttonReproducirPausar_Click);
            // 
            // timerCancion
            // 
            this.timerCancion.Enabled = true;
            this.timerCancion.Interval = 50;
            this.timerCancion.Tick += new System.EventHandler(this.timerCancion_Tick);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(5, 398);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "abrir";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // labelPosicion
            // 
            this.labelPosicion.AutoSize = true;
            this.labelPosicion.Location = new System.Drawing.Point(2, 347);
            this.labelPosicion.Name = "labelPosicion";
            this.labelPosicion.Size = new System.Drawing.Size(28, 13);
            this.labelPosicion.TabIndex = 4;
            this.labelPosicion.Text = "0:00";
            // 
            // labelDuracion
            // 
            this.labelDuracion.AutoSize = true;
            this.labelDuracion.Location = new System.Drawing.Point(267, 347);
            this.labelDuracion.Name = "labelDuracion";
            this.labelDuracion.Size = new System.Drawing.Size(10, 13);
            this.labelDuracion.TabIndex = 5;
            this.labelDuracion.Text = "-";
            this.labelDuracion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelDuracion.Click += new System.EventHandler(this.labelDuracion_Click);
            // 
            // barraAbajoDatos
            // 
            this.barraAbajoDatos.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.labelDatosCancion});
            this.barraAbajoDatos.Location = new System.Drawing.Point(0, 453);
            this.barraAbajoDatos.Name = "barraAbajoDatos";
            this.barraAbajoDatos.Size = new System.Drawing.Size(300, 22);
            this.barraAbajoDatos.TabIndex = 6;
            this.barraAbajoDatos.Text = "statusStrip1";
            // 
            // labelDatosCancion
            // 
            this.labelDatosCancion.Name = "labelDatosCancion";
            this.labelDatosCancion.Size = new System.Drawing.Size(0, 17);
            // 
            // trackBarVolumen
            // 
            this.trackBarVolumen.Location = new System.Drawing.Point(184, 405);
            this.trackBarVolumen.Maximum = 100;
            this.trackBarVolumen.Name = "trackBarVolumen";
            this.trackBarVolumen.Size = new System.Drawing.Size(116, 45);
            this.trackBarVolumen.TabIndex = 7;
            this.trackBarVolumen.TickFrequency = 10;
            this.trackBarVolumen.Scroll += new System.EventHandler(this.trackBarVolumen_Scroll);
            this.trackBarVolumen.MouseDown += new System.Windows.Forms.MouseEventHandler(this.trackBarVolumen_MouseDown);
            this.trackBarVolumen.MouseHover += new System.EventHandler(this.trackBarVolumen_MouseHover);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(5, 427);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Reproductor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(300, 475);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.trackBarVolumen);
            this.Controls.Add(this.barraAbajoDatos);
            this.Controls.Add(this.labelDuracion);
            this.Controls.Add(this.labelPosicion);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.buttonReproducirPausar);
            this.Controls.Add(this.trackBarPosicion);
            this.Controls.Add(this.pictureBoxCaratula);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Reproductor";
            this.Text = "Reproductor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Reproductor_FormClosing);
            this.Load += new System.EventHandler(this.Reproductor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCaratula)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarPosicion)).EndInit();
            this.barraAbajoDatos.ResumeLayout(false);
            this.barraAbajoDatos.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarVolumen)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxCaratula;
        private System.Windows.Forms.TrackBar trackBarPosicion;
        private System.Windows.Forms.Button buttonReproducirPausar;
        private System.Windows.Forms.Timer timerCancion;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label labelPosicion;
        private System.Windows.Forms.Label labelDuracion;
        private System.Windows.Forms.StatusStrip barraAbajoDatos;
        private System.Windows.Forms.ToolStripStatusLabel labelDatosCancion;
        private System.Windows.Forms.TrackBar trackBarVolumen;
        private System.Windows.Forms.Button button1;
    }
}