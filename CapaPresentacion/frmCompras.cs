using CapaEntidad;
using CapaPresentacion.Modales;
using CapaPresentacion.Utilidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class frmCompras : Form
    {
        private Usuario _Usuario;
        public frmCompras(Usuario oUsuario = null)
        {
            _Usuario= oUsuario;
            InitializeComponent();
        }

        private void frmCompras_Load(object sender, EventArgs e)
        {
            //Esto aumenta o disminuye los metodos de pago
            cboTipoDocumento.Items.Add(new OpcionCombo() { Valor = "Metodo Pago 1", Texto = "Metodo Pago 1", });
            cboTipoDocumento.Items.Add(new OpcionCombo() { Valor = "Metodo Pago 2", Texto = "Metodo Pago 2", });
            cboTipoDocumento.DisplayMember = "Texto";
            cboTipoDocumento.ValueMember = "Valor";
            cboTipoDocumento.SelectedIndex = 0;
            txtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtIdProveedor.Text = "0";
            txtIdProducto.Text = "0";
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            using (var modalP = new mdProveedor())
            {
                var result = modalP.ShowDialog();
                if(result == DialogResult.OK)
                {
                    txtIdProveedor.Text = modalP._Proveedor.IdProveedor.ToString();
                    txtNumeroDocumentoProveedor.Text = modalP._Proveedor.Documento;
                    txtRazonSocial.Text = modalP._Proveedor.RazonSocial;
                }
                else { 
                txtNumeroDocumentoProveedor.Select();
                }
            }

        }

        private void btnBuscarProducto_Click(object sender, EventArgs e)
        {
            using (var modalPr = new mdProducto())
            {
                var result = modalPr.ShowDialog();
                if (result == DialogResult.OK)
                {
                    txtIdProveedor.Text = modalPr._Producto.IdProducto.ToString();
                    txtCodProducto.Text = modalPr._Producto.Codigo;
                    txtProducto.Text = modalPr._Producto.Nombre;
                    txtPrecioCompra.Select();
                }
                else
                {
                    txtCodProducto.Select();
                }
            }
        }
    }
}
