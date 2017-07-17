using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Template.Engine.Helper;
using Template.Engine.Security;

namespace Template.Configurator
{
    public partial class FrmConfigurador : Form
    {
        public FrmConfigurador()
        {
            InitializeComponent();
        }

        private void btnLimpiarEncDes_Click(object Sender, EventArgs E)
        {
            textoEncriptarDesencriptar.Clear();
            textoOutput.Clear();
        }

        private void btnEncriptar_Click(object Sender, EventArgs E)
        {
            if (textoEncriptarDesencriptar.Text.Length == 0)
                MessageBox.Show("Debe ingresar un texto.", "¡Atención!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            else
            {
                textoOutput.Text = Coder.Encode(textoEncriptarDesencriptar.Text);
            }
        }

        private void btnDesencriptar_Click(object Sender, EventArgs E)
        {
            if (textoEncriptarDesencriptar.Text.Length == 0)
                MessageBox.Show("Debe ingresar un texto.", "¡Atención!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            else
            {
                textoOutput.Text = Coder.Decode(textoEncriptarDesencriptar.Text);
            }
        }

        private void btnLimpiarEmail_Click(object sender, EventArgs e)
        {
            textoDireccionEmisor.Clear();
            textoNombreEmisor.Clear();
            textoDireccionReceptor.Clear();
            textoNombreReceptor.Clear();
            textoAsunto.Clear();
            textoMensaje.Clear();
        }

        private void btnEnviarEmail_Click(object sender, EventArgs e)
        {
            try
            {
                bool Result = Email.EnviarEmail(textoNombreEmisor.Text, textoDireccionEmisor.Text, textoAsunto.Text, textoNombreReceptor.Text, new List<string>() { textoDireccionReceptor.Text }, textoMensaje.Text);

                MessageBox.Show("Correo Enviado");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetBaseException().Message);
            }
        }

        private void FrmConfigurador_Load(object sender, EventArgs e)
        {
            textoDireccionEmisor.Text = "carlospobletee@gmail.com";
            textoNombreEmisor.Text = "B2B textoNombreEmisor";
            textoDireccionReceptor.Text = "carlospobletee@gmail.com";
            textoNombreReceptor.Text = "GGD";
            textoAsunto.Text = "Asunto";
            textoMensaje.Text = "Mensaje";
        }
    }
}
