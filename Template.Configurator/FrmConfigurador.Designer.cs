using System.ComponentModel;
using System.Windows.Forms;

namespace Template.Configurator
{
    partial class FrmConfigurador
    {
        private IContainer components = null;

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
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnEnviarEmail = new System.Windows.Forms.Button();
            this.btnLimpiarEmail = new System.Windows.Forms.Button();
            this.textoMensaje = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textoAsunto = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textoNombreReceptor = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textoDireccionReceptor = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textoNombreEmisor = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textoDireccionEmisor = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textoOutput = new System.Windows.Forms.TextBox();
            this.tabs.SuspendLayout();
            this.tabEncriptacion.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabs
            // 
            this.tabs.Controls.Add(this.tabEncriptacion);
            this.tabs.Controls.Add(this.tabPage1);
            this.tabs.Location = new System.Drawing.Point(12, 12);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(864, 474);
            this.tabs.TabIndex = 0;
            // 
            // tabEncriptacion
            // 
            this.tabEncriptacion.Controls.Add(this.textoOutput);
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
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnEnviarEmail);
            this.tabPage1.Controls.Add(this.btnLimpiarEmail);
            this.tabPage1.Controls.Add(this.textoMensaje);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.textoAsunto);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.textoNombreReceptor);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.textoDireccionReceptor);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.textoNombreEmisor);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.textoDireccionEmisor);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(856, 448);
            this.tabPage1.TabIndex = 1;
            this.tabPage1.Text = "Correo";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnEnviarEmail
            // 
            this.btnEnviarEmail.Location = new System.Drawing.Point(775, 48);
            this.btnEnviarEmail.Name = "btnEnviarEmail";
            this.btnEnviarEmail.Size = new System.Drawing.Size(75, 23);
            this.btnEnviarEmail.TabIndex = 13;
            this.btnEnviarEmail.Text = "&Enviar";
            this.btnEnviarEmail.UseVisualStyleBackColor = true;
            this.btnEnviarEmail.Click += new System.EventHandler(this.btnEnviarEmail_Click);
            // 
            // btnLimpiarEmail
            // 
            this.btnLimpiarEmail.Location = new System.Drawing.Point(775, 19);
            this.btnLimpiarEmail.Name = "btnLimpiarEmail";
            this.btnLimpiarEmail.Size = new System.Drawing.Size(75, 23);
            this.btnLimpiarEmail.TabIndex = 12;
            this.btnLimpiarEmail.Text = "&Limpiar";
            this.btnLimpiarEmail.UseVisualStyleBackColor = true;
            this.btnLimpiarEmail.Click += new System.EventHandler(this.btnLimpiarEmail_Click);
            // 
            // textoMensaje
            // 
            this.textoMensaje.Location = new System.Drawing.Point(9, 151);
            this.textoMensaje.Multiline = true;
            this.textoMensaje.Name = "textoMensaje";
            this.textoMensaje.Size = new System.Drawing.Size(512, 291);
            this.textoMensaje.TabIndex = 11;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 135);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(47, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "Mensaje";
            // 
            // textoAsunto
            // 
            this.textoAsunto.Location = new System.Drawing.Point(9, 107);
            this.textoAsunto.Name = "textoAsunto";
            this.textoAsunto.Size = new System.Drawing.Size(512, 20);
            this.textoAsunto.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 91);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(40, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "Asunto";
            // 
            // textoNombreReceptor
            // 
            this.textoNombreReceptor.Location = new System.Drawing.Point(268, 63);
            this.textoNombreReceptor.Name = "textoNombreReceptor";
            this.textoNombreReceptor.Size = new System.Drawing.Size(253, 20);
            this.textoNombreReceptor.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(265, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Nombre Receptor";
            // 
            // textoDireccionReceptor
            // 
            this.textoDireccionReceptor.Location = new System.Drawing.Point(9, 63);
            this.textoDireccionReceptor.Name = "textoDireccionReceptor";
            this.textoDireccionReceptor.Size = new System.Drawing.Size(253, 20);
            this.textoDireccionReceptor.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 47);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(99, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Dirección Receptor";
            // 
            // textoNombreEmisor
            // 
            this.textoNombreEmisor.Location = new System.Drawing.Point(268, 19);
            this.textoNombreEmisor.Name = "textoNombreEmisor";
            this.textoNombreEmisor.Size = new System.Drawing.Size(253, 20);
            this.textoNombreEmisor.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(265, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Nombre Emisor";
            // 
            // textoDireccionEmisor
            // 
            this.textoDireccionEmisor.Location = new System.Drawing.Point(9, 19);
            this.textoDireccionEmisor.Name = "textoDireccionEmisor";
            this.textoDireccionEmisor.Size = new System.Drawing.Size(253, 20);
            this.textoDireccionEmisor.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Dirección Emisor";
            // 
            // textoOutput
            // 
            this.textoOutput.Location = new System.Drawing.Point(286, 24);
            this.textoOutput.Name = "textoOutput";
            this.textoOutput.ReadOnly = true;
            this.textoOutput.Size = new System.Drawing.Size(270, 20);
            this.textoOutput.TabIndex = 5;
            // 
            // FrmConfigurador
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(888, 498);
            this.Controls.Add(this.tabs);
            this.Name = "FrmConfigurador";
            this.Text = "Configurador";
            this.Load += new System.EventHandler(this.FrmConfigurador_Load);
            this.tabs.ResumeLayout(false);
            this.tabEncriptacion.ResumeLayout(false);
            this.tabEncriptacion.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
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
        private TabPage tabPage1;
        private TextBox textoDireccionEmisor;
        private Label label2;
        private Button btnEnviarEmail;
        private Button btnLimpiarEmail;
        private TextBox textoMensaje;
        private Label label7;
        private TextBox textoAsunto;
        private Label label6;
        private TextBox textoNombreReceptor;
        private Label label4;
        private TextBox textoDireccionReceptor;
        private Label label5;
        private TextBox textoNombreEmisor;
        private Label label3;
        private TextBox textoOutput;
    }
}

