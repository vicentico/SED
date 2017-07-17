using System.ComponentModel;
using System.Windows.Forms;

namespace Template.Configurator
{
    partial class FrmConfigurador
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            this.tabs = new System.Windows.Forms.TabControl();
            this.tabEncriptacion = new System.Windows.Forms.TabPage();
            this.btnDesencriptar = new System.Windows.Forms.Button();
            this.btnEncriptar = new System.Windows.Forms.Button();
            this.btnLimpiarEncDes = new System.Windows.Forms.Button();
            this.textoEncriptarDesencriptar = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabs.SuspendLayout();
            this.tabEncriptacion.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabs
            // 
            this.tabs.Controls.Add(this.tabEncriptacion);
            this.tabs.Location = new System.Drawing.Point(12, 12);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(864, 474);
            this.tabs.TabIndex = 0;
            // 
            // tabEncriptacion
            // 
            this.tabEncriptacion.Controls.Add(this.btnDesencriptar);
            this.tabEncriptacion.Controls.Add(this.btnEncriptar);
            this.tabEncriptacion.Controls.Add(this.btnLimpiarEncDes);
            this.tabEncriptacion.Controls.Add(this.textoEncriptarDesencriptar);
            this.tabEncriptacion.Controls.Add(this.label1);
            this.tabEncriptacion.Location = new System.Drawing.Point(4, 22);
            this.tabEncriptacion.Name = "tabEncriptacion";
            this.tabEncriptacion.Padding = new System.Windows.Forms.Padding(3);
            this.tabEncriptacion.Size = new System.Drawing.Size(856, 448);
            this.tabEncriptacion.TabIndex = 0;
            this.tabEncriptacion.Text = "Encriptación";
            this.tabEncriptacion.UseVisualStyleBackColor = true;
            // 
            // btnDesencriptar
            // 
            this.btnDesencriptar.Location = new System.Drawing.Point(174, 51);
            this.btnDesencriptar.Name = "btnDesencriptar";
            this.btnDesencriptar.Size = new System.Drawing.Size(75, 23);
            this.btnDesencriptar.TabIndex = 4;
            this.btnDesencriptar.Text = "&Desencriptar";
            this.btnDesencriptar.UseVisualStyleBackColor = true;
            this.btnDesencriptar.Click += new System.EventHandler(this.btnDesencriptar_Click);
            // 
            // btnEncriptar
            // 
            this.btnEncriptar.Location = new System.Drawing.Point(92, 51);
            this.btnEncriptar.Name = "btnEncriptar";
            this.btnEncriptar.Size = new System.Drawing.Size(75, 23);
            this.btnEncriptar.TabIndex = 3;
            this.btnEncriptar.Text = "&Encriptar";
            this.btnEncriptar.UseVisualStyleBackColor = true;
            this.btnEncriptar.Click += new System.EventHandler(this.btnEncriptar_Click);
            // 
            // btnLimpiarEncDes
            // 
            this.btnLimpiarEncDes.Location = new System.Drawing.Point(10, 51);
            this.btnLimpiarEncDes.Name = "btnLimpiarEncDes";
            this.btnLimpiarEncDes.Size = new System.Drawing.Size(75, 23);
            this.btnLimpiarEncDes.TabIndex = 2;
            this.btnLimpiarEncDes.Text = "&Limpiar";
            this.btnLimpiarEncDes.UseVisualStyleBackColor = true;
            this.btnLimpiarEncDes.Click += new System.EventHandler(this.btnLimpiarEncDes_Click);
            // 
            // textoEncriptarDesencriptar
            // 
            this.textoEncriptarDesencriptar.Location = new System.Drawing.Point(10, 24);
            this.textoEncriptarDesencriptar.Name = "textoEncriptarDesencriptar";
            this.textoEncriptarDesencriptar.Size = new System.Drawing.Size(270, 20);
            this.textoEncriptarDesencriptar.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(160, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Texto a Encriptar o Desencriptar";
            // 
            // frmConfigurador
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(888, 498);
            this.Controls.Add(this.tabs);
            this.Name = "FrmConfigurador";
            this.Text = "Configurador";
            this.tabs.ResumeLayout(false);
            this.tabEncriptacion.ResumeLayout(false);
            this.tabEncriptacion.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TabControl tabs;
        private TabPage tabEncriptacion;
        private Button btnDesencriptar;
        private Button btnEncriptar;
        private Button btnLimpiarEncDes;
        private TextBox textoEncriptarDesencriptar;
        private Label label1;
    }
}

