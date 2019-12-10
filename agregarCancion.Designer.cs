namespace aplicacion_ipo
{
    partial class agregarCancion
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
            this.tituloTextBox = new System.Windows.Forms.TextBox();
            this.secsTextBox = new System.Windows.Forms.TextBox();
            this.minTextBox = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tituloTextBox
            // 
            this.tituloTextBox.Location = new System.Drawing.Point(47, 35);
            this.tituloTextBox.Name = "tituloTextBox";
            this.tituloTextBox.Size = new System.Drawing.Size(142, 20);
            this.tituloTextBox.TabIndex = 0;
            this.tituloTextBox.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // secsTextBox
            // 
            this.secsTextBox.Location = new System.Drawing.Point(129, 75);
            this.secsTextBox.Name = "secsTextBox";
            this.secsTextBox.Size = new System.Drawing.Size(60, 20);
            this.secsTextBox.TabIndex = 1;
            // 
            // minTextBox
            // 
            this.minTextBox.Location = new System.Drawing.Point(47, 75);
            this.minTextBox.Name = "minTextBox";
            this.minTextBox.Size = new System.Drawing.Size(59, 20);
            this.minTextBox.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(78, 119);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // agregarCancion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(252, 154);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.minTextBox);
            this.Controls.Add(this.secsTextBox);
            this.Controls.Add(this.tituloTextBox);
            this.Name = "agregarCancion";
            this.Text = "agregarCancion";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tituloTextBox;
        private System.Windows.Forms.TextBox secsTextBox;
        private System.Windows.Forms.TextBox minTextBox;
        private System.Windows.Forms.Button button1;
    }
}