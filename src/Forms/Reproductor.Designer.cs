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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Reproductor));
            this.trackBarPosicion = new System.Windows.Forms.TrackBar();
            this.buttonReproducirPausar = new System.Windows.Forms.Button();
            this.timerCancion = new System.Windows.Forms.Timer(this.components);
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.button2 = new System.Windows.Forms.Button();
            this.labelPosicion = new System.Windows.Forms.Label();
            this.labelDuracion = new System.Windows.Forms.Label();
            this.barraAbajoDatos = new System.Windows.Forms.StatusStrip();
            this.labelDatosCancion = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelCorreoUsuario = new System.Windows.Forms.ToolStripStatusLabel();
            this.trackBarVolumen = new System.Windows.Forms.TrackBar();
            this.buttonSpotify = new System.Windows.Forms.Button();
            this.timerSpotify = new System.Windows.Forms.Timer(this.components);
            this.labelVolumen = new System.Windows.Forms.Label();
            this.labelPorcentaje = new System.Windows.Forms.Label();
            this.checkBoxAleatorio = new System.Windows.Forms.CheckBox();
            this.buttonSaltarAdelante = new System.Windows.Forms.Button();
            this.buttonSaltarAtras = new System.Windows.Forms.Button();
            this.pictureBoxCaratula = new System.Windows.Forms.PictureBox();
            this.timerMetadatos = new System.Windows.Forms.Timer(this.components);
            this.buttonAgregar = new System.Windows.Forms.Button();
            this.iconoCerrar = new System.Windows.Forms.NotifyIcon(this.components);
            this.checkBoxFoobar = new System.Windows.Forms.CheckBox();
            this.timerFoobar = new System.Windows.Forms.Timer(this.components);
            this.buttonTwit = new System.Windows.Forms.Button();
            this.buttoncrearLR = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarPosicion)).BeginInit();
            this.barraAbajoDatos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarVolumen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCaratula)).BeginInit();
            this.SuspendLayout();
            // 
            // trackBarPosicion
            // 
            this.trackBarPosicion.Location = new System.Drawing.Point(-8, 368);
            this.trackBarPosicion.Maximum = 500;
            this.trackBarPosicion.Name = "trackBarPosicion";
            this.trackBarPosicion.Size = new System.Drawing.Size(384, 45);
            this.trackBarPosicion.TabIndex = 1;
            this.trackBarPosicion.TabStop = false;
            this.trackBarPosicion.TickFrequency = 0;
            this.trackBarPosicion.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBarPosicion.Scroll += new System.EventHandler(this.trackBarPosicion_Scroll);
            this.trackBarPosicion.ValueChanged += new System.EventHandler(this.trackBarPosicion_ValueChanged);
            this.trackBarPosicion.MouseDown += new System.Windows.Forms.MouseEventHandler(this.trackBarPosicion_MouseDown);
            this.trackBarPosicion.MouseUp += new System.Windows.Forms.MouseEventHandler(this.trackBarPosicion_MouseUp);
            // 
            // buttonReproducirPausar
            // 
            this.buttonReproducirPausar.Font = new System.Drawing.Font("Calibri", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.buttonReproducirPausar.Location = new System.Drawing.Point(157, 408);
            this.buttonReproducirPausar.Name = "buttonReproducirPausar";
            this.buttonReproducirPausar.Size = new System.Drawing.Size(43, 42);
            this.buttonReproducirPausar.TabIndex = 2;
            this.buttonReproducirPausar.Text = "▶";
            this.buttonReproducirPausar.UseVisualStyleBackColor = true;
            this.buttonReproducirPausar.Click += new System.EventHandler(this.buttonReproducirPausar_Click);
            // 
            // timerCancion
            // 
            this.timerCancion.Enabled = true;
            this.timerCancion.Tick += new System.EventHandler(this.timerCancion_Tick);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(0, 515);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(102, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "abrir";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // labelPosicion
            // 
            this.labelPosicion.AutoSize = true;
            this.labelPosicion.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPosicion.Location = new System.Drawing.Point(8, 422);
            this.labelPosicion.Name = "labelPosicion";
            this.labelPosicion.Size = new System.Drawing.Size(32, 17);
            this.labelPosicion.TabIndex = 4;
            this.labelPosicion.Text = "0:00";
            this.labelPosicion.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // labelDuracion
            // 
            this.labelDuracion.AutoSize = true;
            this.labelDuracion.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDuracion.Location = new System.Drawing.Point(310, 422);
            this.labelDuracion.Name = "labelDuracion";
            this.labelDuracion.Size = new System.Drawing.Size(43, 17);
            this.labelDuracion.TabIndex = 5;
            this.labelDuracion.Text = "XX:XX";
            this.labelDuracion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelDuracion.Click += new System.EventHandler(this.labelDuracion_Click);
            // 
            // barraAbajoDatos
            // 
            this.barraAbajoDatos.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.labelDatosCancion,
            this.toolStripStatusLabelCorreoUsuario});
            this.barraAbajoDatos.Location = new System.Drawing.Point(0, 570);
            this.barraAbajoDatos.Name = "barraAbajoDatos";
            this.barraAbajoDatos.Size = new System.Drawing.Size(365, 22);
            this.barraAbajoDatos.TabIndex = 6;
            this.barraAbajoDatos.Text = "statusStrip1";
            // 
            // labelDatosCancion
            // 
            this.labelDatosCancion.Name = "labelDatosCancion";
            this.labelDatosCancion.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripStatusLabelCorreoUsuario
            // 
            this.toolStripStatusLabelCorreoUsuario.Name = "toolStripStatusLabelCorreoUsuario";
            this.toolStripStatusLabelCorreoUsuario.Size = new System.Drawing.Size(0, 17);
            // 
            // trackBarVolumen
            // 
            this.trackBarVolumen.Location = new System.Drawing.Point(222, 493);
            this.trackBarVolumen.Maximum = 100;
            this.trackBarVolumen.Name = "trackBarVolumen";
            this.trackBarVolumen.Size = new System.Drawing.Size(143, 45);
            this.trackBarVolumen.TabIndex = 7;
            this.trackBarVolumen.TabStop = false;
            this.trackBarVolumen.TickFrequency = 10;
            this.trackBarVolumen.Scroll += new System.EventHandler(this.trackBarVolumen_Scroll);
            this.trackBarVolumen.ValueChanged += new System.EventHandler(this.trackBarVolumen_ValueChanged);
            this.trackBarVolumen.MouseDown += new System.Windows.Forms.MouseEventHandler(this.trackBarVolumen_MouseDown);
            this.trackBarVolumen.MouseHover += new System.EventHandler(this.trackBarVolumen_MouseHover);
            // 
            // buttonSpotify
            // 
            this.buttonSpotify.Location = new System.Drawing.Point(0, 544);
            this.buttonSpotify.Name = "buttonSpotify";
            this.buttonSpotify.Size = new System.Drawing.Size(102, 23);
            this.buttonSpotify.TabIndex = 8;
            this.buttonSpotify.TabStop = false;
            this.buttonSpotify.Text = "cambiar a Spotify";
            this.buttonSpotify.UseVisualStyleBackColor = true;
            this.buttonSpotify.Click += new System.EventHandler(this.buttonSpotify_Click);
            // 
            // timerSpotify
            // 
            this.timerSpotify.Interval = 250;
            this.timerSpotify.Tick += new System.EventHandler(this.timerSpotify_Tick);
            // 
            // labelVolumen
            // 
            this.labelVolumen.AutoSize = true;
            this.labelVolumen.Location = new System.Drawing.Point(332, 525);
            this.labelVolumen.Name = "labelVolumen";
            this.labelVolumen.Size = new System.Drawing.Size(33, 13);
            this.labelVolumen.TabIndex = 9;
            this.labelVolumen.Text = "100%";
            // 
            // labelPorcentaje
            // 
            this.labelPorcentaje.AutoSize = true;
            this.labelPorcentaje.Location = new System.Drawing.Point(8, 454);
            this.labelPorcentaje.Name = "labelPorcentaje";
            this.labelPorcentaje.Size = new System.Drawing.Size(21, 13);
            this.labelPorcentaje.TabIndex = 10;
            this.labelPorcentaje.Text = "0%";
            // 
            // checkBoxAleatorio
            // 
            this.checkBoxAleatorio.AutoSize = true;
            this.checkBoxAleatorio.Font = new System.Drawing.Font("Segoe UI Emoji", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxAleatorio.Location = new System.Drawing.Point(11, 484);
            this.checkBoxAleatorio.Name = "checkBoxAleatorio";
            this.checkBoxAleatorio.Size = new System.Drawing.Size(51, 25);
            this.checkBoxAleatorio.TabIndex = 11;
            this.checkBoxAleatorio.Text = "🔀️";
            this.checkBoxAleatorio.UseVisualStyleBackColor = true;
            this.checkBoxAleatorio.CheckedChanged += new System.EventHandler(this.checkBoxAleatorio_CheckedChanged);
            // 
            // buttonSaltarAdelante
            // 
            this.buttonSaltarAdelante.Font = new System.Drawing.Font("Webdings", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.buttonSaltarAdelante.Location = new System.Drawing.Point(256, 408);
            this.buttonSaltarAdelante.Name = "buttonSaltarAdelante";
            this.buttonSaltarAdelante.Size = new System.Drawing.Size(43, 42);
            this.buttonSaltarAdelante.TabIndex = 12;
            this.buttonSaltarAdelante.Text = ":";
            this.buttonSaltarAdelante.UseVisualStyleBackColor = true;
            this.buttonSaltarAdelante.Click += new System.EventHandler(this.buttonSaltarAdelante_Click);
            // 
            // buttonSaltarAtras
            // 
            this.buttonSaltarAtras.Font = new System.Drawing.Font("Webdings", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.buttonSaltarAtras.Location = new System.Drawing.Point(59, 408);
            this.buttonSaltarAtras.Name = "buttonSaltarAtras";
            this.buttonSaltarAtras.Size = new System.Drawing.Size(43, 42);
            this.buttonSaltarAtras.TabIndex = 13;
            this.buttonSaltarAtras.Text = "9";
            this.buttonSaltarAtras.UseVisualStyleBackColor = true;
            this.buttonSaltarAtras.Click += new System.EventHandler(this.buttonSaltarAtras_Click);
            // 
            // pictureBoxCaratula
            // 
            this.pictureBoxCaratula.Image = global::aplicacion_musica.Properties.Resources.albumdesconocido;
            this.pictureBoxCaratula.InitialImage = global::aplicacion_musica.Properties.Resources.albumdesconocido;
            this.pictureBoxCaratula.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxCaratula.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBoxCaratula.Name = "pictureBoxCaratula";
            this.pictureBoxCaratula.Size = new System.Drawing.Size(365, 365);
            this.pictureBoxCaratula.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxCaratula.TabIndex = 0;
            this.pictureBoxCaratula.TabStop = false;
            // 
            // timerMetadatos
            // 
            this.timerMetadatos.Interval = 750;
            this.timerMetadatos.Tick += new System.EventHandler(this.timerMetadatos_Tick);
            // 
            // buttonAgregar
            // 
            this.buttonAgregar.Location = new System.Drawing.Point(108, 544);
            this.buttonAgregar.Name = "buttonAgregar";
            this.buttonAgregar.Size = new System.Drawing.Size(115, 23);
            this.buttonAgregar.TabIndex = 14;
            this.buttonAgregar.TabStop = false;
            this.buttonAgregar.Text = "Agregar a la BBDD";
            this.buttonAgregar.UseVisualStyleBackColor = true;
            this.buttonAgregar.Click += new System.EventHandler(this.buttonAgregar_Click);
            // 
            // iconoCerrar
            // 
            this.iconoCerrar.Icon = ((System.Drawing.Icon)(resources.GetObject("iconoCerrar.Icon")));
            this.iconoCerrar.Text = "notifyIcon1";
            this.iconoCerrar.Visible = true;
            this.iconoCerrar.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.iconoCerrar_doubleClick);
            // 
            // checkBoxFoobar
            // 
            this.checkBoxFoobar.AutoSize = true;
            this.checkBoxFoobar.Font = new System.Drawing.Font("Segoe UI Emoji", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxFoobar.Location = new System.Drawing.Point(290, 462);
            this.checkBoxFoobar.Name = "checkBoxFoobar";
            this.checkBoxFoobar.Size = new System.Drawing.Size(75, 25);
            this.checkBoxFoobar.TabIndex = 15;
            this.checkBoxFoobar.Text = "foobar";
            this.checkBoxFoobar.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.checkBoxFoobar.UseVisualStyleBackColor = true;
            this.checkBoxFoobar.Visible = false;
            this.checkBoxFoobar.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // timerFoobar
            // 
            this.timerFoobar.Interval = 2000;
            this.timerFoobar.Tick += new System.EventHandler(this.timerFoobar_Tick);
            // 
            // buttonTwit
            // 
            this.buttonTwit.AllowDrop = true;
            this.buttonTwit.Location = new System.Drawing.Point(0, 515);
            this.buttonTwit.Name = "buttonTwit";
            this.buttonTwit.Size = new System.Drawing.Size(102, 23);
            this.buttonTwit.TabIndex = 16;
            this.buttonTwit.Text = "twittear";
            this.buttonTwit.UseVisualStyleBackColor = true;
            this.buttonTwit.Click += new System.EventHandler(this.buttonTwit_Click);
            // 
            // buttoncrearLR
            // 
            this.buttoncrearLR.AllowDrop = true;
            this.buttoncrearLR.Location = new System.Drawing.Point(229, 544);
            this.buttoncrearLR.Name = "buttoncrearLR";
            this.buttoncrearLR.Size = new System.Drawing.Size(129, 23);
            this.buttoncrearLR.TabIndex = 17;
            this.buttoncrearLR.Text = "crear LR";
            this.buttoncrearLR.UseVisualStyleBackColor = true;
            this.buttoncrearLR.Click += new System.EventHandler(this.button1_Click);
            // 
            // Reproductor
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(365, 592);
            this.Controls.Add(this.buttoncrearLR);
            this.Controls.Add(this.buttonTwit);
            this.Controls.Add(this.checkBoxFoobar);
            this.Controls.Add(this.buttonAgregar);
            this.Controls.Add(this.buttonSaltarAtras);
            this.Controls.Add(this.buttonSaltarAdelante);
            this.Controls.Add(this.checkBoxAleatorio);
            this.Controls.Add(this.labelPorcentaje);
            this.Controls.Add(this.labelVolumen);
            this.Controls.Add(this.buttonSpotify);
            this.Controls.Add(this.trackBarVolumen);
            this.Controls.Add(this.barraAbajoDatos);
            this.Controls.Add(this.labelDuracion);
            this.Controls.Add(this.labelPosicion);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.buttonReproducirPausar);
            this.Controls.Add(this.trackBarPosicion);
            this.Controls.Add(this.pictureBoxCaratula);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.Name = "Reproductor";
            this.Text = "x";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Reproductor_FormClosing);
            this.Load += new System.EventHandler(this.Reproductor_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Reproductor_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Reproductor_DragEnter);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Reproductor_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarPosicion)).EndInit();
            this.barraAbajoDatos.ResumeLayout(false);
            this.barraAbajoDatos.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarVolumen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCaratula)).EndInit();
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
        private System.Windows.Forms.Button buttonSpotify;
        private System.Windows.Forms.Timer timerSpotify;
        private System.Windows.Forms.Label labelVolumen;
        private System.Windows.Forms.Label labelPorcentaje;
        private System.Windows.Forms.CheckBox checkBoxAleatorio;
        private System.Windows.Forms.Button buttonSaltarAdelante;
        private System.Windows.Forms.Button buttonSaltarAtras;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelCorreoUsuario;
        private System.Windows.Forms.Timer timerMetadatos;
        private System.Windows.Forms.Button buttonAgregar;
        private System.Windows.Forms.NotifyIcon iconoCerrar;
        private System.Windows.Forms.CheckBox checkBoxFoobar;
        private System.Windows.Forms.Timer timerFoobar;
        private System.Windows.Forms.Button buttonTwit;
        private System.Windows.Forms.Button buttoncrearLR;
    }
}