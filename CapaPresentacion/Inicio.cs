using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaEntidad;
using FontAwesome.Sharp;
using CapaNegocio;
using System.Diagnostics.Eventing.Reader;

namespace CapaPresentacion
{
    public partial class Inicio : Form
    {

        private static Usuario usuarioActual;
        private static IconMenuItem MenuActivo=null;
        private static Form FormularioActivo = null;
        
        public Inicio(Usuario objusuario) { 

                usuarioActual = objusuario;
                InitializeComponent();
        }

        private void Inicio_Load(object sender, EventArgs e)
        {
            List<Permiso> ListaPermisos = new Cn_Permiso().Listar(usuarioActual.IdUsuario);

            foreach (IconMenuItem iconMenu in Menu.Items)
            {
                bool encontrado = ListaPermisos.Any(m => m.NombreMenu == iconMenu.Name);

                if(encontrado == false)
                {
                    iconMenu.Visible = false;
                }
            }
            LblUsuario.Text = usuarioActual.NombreCompleto;
        }

        private void AbrirFormulario(IconMenuItem menu , Form formulario)
        {
            if (MenuActivo !=null)
            {
                MenuActivo.BackColor = Color.White;
            }
            menu.BackColor = Color.Silver;
            MenuActivo = menu;

            if(FormularioActivo != null)
            {
                FormularioActivo.Close();
            }
            //Esta area se encarga del estilo en general del formulario
            FormularioActivo = formulario;
            formulario.TopLevel = false;
            formulario.FormBorderStyle = FormBorderStyle.None;
            formulario.Dock= DockStyle.Fill;
            formulario.BackColor = Color.Gray;
            Contenedor.Controls.Add(formulario);
            formulario.Show();
        }

        private void MenuUsuario_Click(object sender, EventArgs e)
        {
            AbrirFormulario((IconMenuItem)sender, new FrmUsuarios());
        }

        private void SubMenuCategoria_Click(object sender, EventArgs e)
        {
            AbrirFormulario(MenuMantenedor, new frmCategoria());
        }

        private void SubMenuProducto_Click(object sender, EventArgs e)
        {
            AbrirFormulario(MenuMantenedor, new frmProducto());
        }

        private void submenuRegistarVenta_Click(object sender, EventArgs e)
        {
            AbrirFormulario(MenuVentas, new frmVentas());
        }

        private void submenuVerDetalleVenta_Click(object sender, EventArgs e)
        {
            AbrirFormulario(MenuVentas, new frmDetalleVenta());
        }

        private void submenuRegistarCompra_Click(object sender, EventArgs e)
        {
            AbrirFormulario(MenuCompras, new frmCompras(usuarioActual));
            
        }

        private void submenuVerDetalleCompra_Click(object sender, EventArgs e)
        {
            AbrirFormulario(MenuCompras, new frmDetalleCompra());
        }

        private void MenuClientes_Click(object sender, EventArgs e)
        {
            AbrirFormulario((IconMenuItem)sender, new frmClientes());
        }

        private void MenuProveedores_Click(object sender, EventArgs e)
        {
            AbrirFormulario((IconMenuItem)sender, new frmProveedores());
        }

        private void MenuReportes_Click(object sender, EventArgs e)
        {
            AbrirFormulario((IconMenuItem)sender, new frmReportes());
        }

        private void SubMenuNegocio_Click(object sender, EventArgs e)
        {
            AbrirFormulario(MenuMantenedor, new frmNegocio());
        }

        private void MenuCompras_Click(object sender, EventArgs e)
        {

        }
    }
}
