using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Forms;
using CapaNegocio;
using CapaEntidad;

namespace CapaPresentacion
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void iconButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {

            List<Usuario> TEST = new Cn_Usuario().Listar();


            Usuario ousuario = new Cn_Usuario().Listar().Where(u => u.Documento == txt_Documento.Text && u.Clave == txt_Clave.Text).FirstOrDefault();
            if(ousuario != null)
            {
                MessageBox.Show("Bienvenido " + ousuario.NombreCompleto);
                Inicio form = new Inicio(ousuario);
                form.Show();
                this.Hide();

                form.FormClosing += frmClosing;
            }
            else
            {
                MessageBox.Show("Usuario o contraseña incorrectos", "Mensaje" ,MessageBoxButtons.OK, MessageBoxIcon.Exclamation); ;
            }
        }

        private void frmClosing(object sender, FormClosingEventArgs e)
        {
            txt_Documento.Text = "";
            txt_Clave.Text = "";
            this.Show();
        }
    }
}
