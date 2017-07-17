using System;
using System.Windows.Forms;
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
            textoEncriptarDesencriptar.Text = "";
        }

        private void btnEncriptar_Click(object Sender, EventArgs E)
        {
            if (textoEncriptarDesencriptar.Text.Length == 0)
                MessageBox.Show("Debe ingresar un texto.", "¡Atención!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            else
            {
                textoEncriptarDesencriptar.Text = Coder.Encode(textoEncriptarDesencriptar.Text);
            }
        }

        private void btnDesencriptar_Click(object Sender, EventArgs E)
        {
            if (textoEncriptarDesencriptar.Text.Length == 0)
                MessageBox.Show("Debe ingresar un texto.", "¡Atención!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            else
            {
                textoEncriptarDesencriptar.Text = Coder.Decode(textoEncriptarDesencriptar.Text);
            }
        }
    }
}
