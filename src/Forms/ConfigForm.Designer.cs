namespace Cassiopeia.src.Forms
{
    partial class ConfigForm
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("idioma");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("portapapeles");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("historial");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("stream");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("text");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("colors");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("view", new System.Windows.Forms.TreeNode[] {
            treeNode5,
            treeNode6});
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigForm));
            this.treeViewConfiguracion = new System.Windows.Forms.TreeView();
            this.buttonAplicar = new System.Windows.Forms.Button();
            this.buttonCancelar = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.groupBoxRaiz = new System.Windows.Forms.GroupBox();
            this.labelSelect = new System.Windows.Forms.Label();
            this.groupBoxRaiz.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeViewConfiguracion
            // 
            this.treeViewConfiguracion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.treeViewConfiguracion.Location = new System.Drawing.Point(14, 12);
            this.treeViewConfiguracion.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.treeViewConfiguracion.Name = "treeViewConfiguracion";
            treeNode1.Name = "language";
            treeNode1.Tag = "language";
            treeNode1.Text = "idioma";
            treeNode2.Name = "clipboard";
            treeNode2.Tag = "clipboard";
            treeNode2.Text = "portapapeles";
            treeNode3.Name = "historial";
            treeNode3.Tag = "historial";
            treeNode3.Text = "historial";
            treeNode4.Name = "stream";
            treeNode4.Tag = "stream";
            treeNode4.Text = "stream";
            treeNode5.Name = "text";
            treeNode5.Tag = "text";
            treeNode5.Text = "text";
            treeNode6.Name = "colors";
            treeNode6.Tag = "colors";
            treeNode6.Text = "colors";
            treeNode7.Name = "visual";
            treeNode7.Tag = "view";
            treeNode7.Text = "view";
            this.treeViewConfiguracion.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4,
            treeNode7});
            this.treeViewConfiguracion.Size = new System.Drawing.Size(220, 353);
            this.treeViewConfiguracion.TabIndex = 0;
            this.treeViewConfiguracion.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewConfiguracion_AfterSelect);
            this.treeViewConfiguracion.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewConfiguracion_NodeMouseClick);
            this.treeViewConfiguracion.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treeViewConfiguracion_KeyDown);
            // 
            // buttonAplicar
            // 
            this.buttonAplicar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAplicar.Location = new System.Drawing.Point(627, 371);
            this.buttonAplicar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonAplicar.Name = "buttonAplicar";
            this.buttonAplicar.Size = new System.Drawing.Size(88, 27);
            this.buttonAplicar.TabIndex = 2;
            this.buttonAplicar.Text = "aplicar";
            this.buttonAplicar.UseVisualStyleBackColor = true;
            this.buttonAplicar.Click += new System.EventHandler(this.buttonAplicar_Click);
            // 
            // buttonCancelar
            // 
            this.buttonCancelar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancelar.Location = new System.Drawing.Point(531, 371);
            this.buttonCancelar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonCancelar.Name = "buttonCancelar";
            this.buttonCancelar.Size = new System.Drawing.Size(88, 27);
            this.buttonCancelar.TabIndex = 3;
            this.buttonCancelar.Text = "cancelar";
            this.buttonCancelar.UseVisualStyleBackColor = true;
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.Location = new System.Drawing.Point(435, 371);
            this.buttonOK.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(88, 27);
            this.buttonOK.TabIndex = 4;
            this.buttonOK.Text = "ok";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // groupBoxRaiz
            // 
            this.groupBoxRaiz.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxRaiz.AutoSize = true;
            this.groupBoxRaiz.Controls.Add(this.labelSelect);
            this.groupBoxRaiz.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBoxRaiz.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.groupBoxRaiz.Location = new System.Drawing.Point(242, 12);
            this.groupBoxRaiz.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBoxRaiz.Name = "groupBoxRaiz";
            this.groupBoxRaiz.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBoxRaiz.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.groupBoxRaiz.Size = new System.Drawing.Size(473, 353);
            this.groupBoxRaiz.TabIndex = 5;
            this.groupBoxRaiz.TabStop = false;
            this.groupBoxRaiz.Resize += new System.EventHandler(this.groupBoxRaiz_Resize);
            // 
            // labelSelect
            // 
            this.labelSelect.AutoSize = true;
            this.labelSelect.Location = new System.Drawing.Point(187, 124);
            this.labelSelect.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelSelect.Name = "labelSelect";
            this.labelSelect.Size = new System.Drawing.Size(94, 21);
            this.labelSelect.TabIndex = 0;
            this.labelSelect.Text = "seleccione.";
            this.labelSelect.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // ConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancelar;
            this.ClientSize = new System.Drawing.Size(728, 406);
            this.Controls.Add(this.groupBoxRaiz);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancelar);
            this.Controls.Add(this.buttonAplicar);
            this.Controls.Add(this.treeViewConfiguracion);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MinimumSize = new System.Drawing.Size(744, 445);
            this.Name = "ConfigForm";
            this.Text = "Config";
            this.Load += new System.EventHandler(this.ConfigForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ConfigForm_KeyDown);
            this.groupBoxRaiz.ResumeLayout(false);
            this.groupBoxRaiz.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeViewConfiguracion;
        private System.Windows.Forms.Button buttonAplicar;
        private System.Windows.Forms.Button buttonCancelar;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.GroupBox groupBoxRaiz;
        private System.Windows.Forms.Label labelSelect;
    }
}