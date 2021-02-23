namespace Cassiopea.src.Forms
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
            this.treeViewConfiguracion.Location = new System.Drawing.Point(12, 12);
            this.treeViewConfiguracion.Name = "treeViewConfiguracion";
            treeNode1.Name = "idioma";
            treeNode1.Tag = "idioma";
            treeNode1.Text = "idioma";
            treeNode2.Name = "portapapeles";
            treeNode2.Tag = "portapapeles";
            treeNode2.Text = "portapapeles";
            this.treeViewConfiguracion.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2});
            this.treeViewConfiguracion.Size = new System.Drawing.Size(246, 549);
            this.treeViewConfiguracion.TabIndex = 0;
            this.treeViewConfiguracion.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewConfiguracion_NodeMouseClick);
            // 
            // buttonAplicar
            // 
            this.buttonAplicar.Location = new System.Drawing.Point(780, 538);
            this.buttonAplicar.Name = "buttonAplicar";
            this.buttonAplicar.Size = new System.Drawing.Size(75, 23);
            this.buttonAplicar.TabIndex = 2;
            this.buttonAplicar.Text = "aplicar";
            this.buttonAplicar.UseVisualStyleBackColor = true;
            this.buttonAplicar.Click += new System.EventHandler(this.buttonAplicar_Click);
            // 
            // buttonCancelar
            // 
            this.buttonCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancelar.Location = new System.Drawing.Point(699, 538);
            this.buttonCancelar.Name = "buttonCancelar";
            this.buttonCancelar.Size = new System.Drawing.Size(75, 23);
            this.buttonCancelar.TabIndex = 3;
            this.buttonCancelar.Text = "cancelar";
            this.buttonCancelar.UseVisualStyleBackColor = true;
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(618, 538);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 4;
            this.buttonOK.Text = "ok";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // groupBoxRaiz
            // 
            this.groupBoxRaiz.Controls.Add(this.labelSelect);
            this.groupBoxRaiz.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBoxRaiz.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxRaiz.Location = new System.Drawing.Point(264, 12);
            this.groupBoxRaiz.Name = "groupBoxRaiz";
            this.groupBoxRaiz.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.groupBoxRaiz.Size = new System.Drawing.Size(591, 520);
            this.groupBoxRaiz.TabIndex = 5;
            this.groupBoxRaiz.TabStop = false;
            // 
            // labelSelect
            // 
            this.labelSelect.AutoSize = true;
            this.labelSelect.Location = new System.Drawing.Point(290, 240);
            this.labelSelect.Name = "labelSelect";
            this.labelSelect.Size = new System.Drawing.Size(94, 21);
            this.labelSelect.TabIndex = 0;
            this.labelSelect.Text = "seleccione.";
            this.labelSelect.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // ConfigForm
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancelar;
            this.ClientSize = new System.Drawing.Size(867, 573);
            this.Controls.Add(this.groupBoxRaiz);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancelar);
            this.Controls.Add(this.buttonAplicar);
            this.Controls.Add(this.treeViewConfiguracion);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "ConfigForm";
            this.Text = "Config";
            this.Load += new System.EventHandler(this.ConfigForm_Load);
            this.groupBoxRaiz.ResumeLayout(false);
            this.groupBoxRaiz.PerformLayout();
            this.ResumeLayout(false);

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