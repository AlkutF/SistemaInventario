using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.Modales;
using CapaPresentacion.Utilidades;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
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
                    txtIdProducto.Text = modalPr._Producto.IdProducto.ToString();
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

        private void txtCodProducto_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyData == Keys.Enter)
            {
                Producto oProducto = new Cn_Productos().Listar().Where(p => p.Codigo == txtCodProducto.Text && p.Estado == true).FirstOrDefault();
                if (oProducto != null)
                {
                    txtIdProducto.Text = oProducto.IdProducto.ToString();
                    txtProducto.Text=oProducto.Nombre;
                    txtPrecioCompra.Select();
                }
                else
                {
                    txtIdProducto.Text = "0";
                    txtProducto.Text = "";
                }
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            decimal preciocompra = 0;
            decimal precioventa = 0;
            bool proucto_real = false;
            if (int.Parse(txtIdProducto.Text) == 0)
            {
                MessageBox.Show("Debe seleccionar un producto para agregar", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (!decimal.TryParse(txtPrecioCompra.Text , out preciocompra))
            {
                MessageBox.Show("Formato incorrecto en el precio de la compra , solo agregar valores numericos", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtPrecioCompra.Select() ;  
                return;
            }
            if (!decimal.TryParse(txtPrecioVenta.Text, out precioventa))
            {
                MessageBox.Show("Formato incorrecto en el precio de la venta , solo agregar valores numericos", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtPrecioVenta.Select();
                return;
            }
            foreach(DataGridViewRow fila in dataGridView1.Rows)
            {
                if (fila.Cells["IdProducto"].Value != null && fila.Cells["IdProducto"].Value.ToString() == txtIdProducto.Text)
                {
                    proucto_real = true;
                    break;
                }
            }

            if (!proucto_real)
            {
               

                dataGridView1.Rows.Add(new object[]
                {
                txtIdProducto.Text,
                txtProducto.Text,
                preciocompra.ToString("N2"),
                precioventa.ToString("N2"),
                txtCantidad.Value.ToString(),
                (txtCantidad.Value * preciocompra).ToString("N2"),
                });
                Calcular_Total();
                LimpiarProducto();
                txtCodProducto.Select();
            }


        }

        private void LimpiarProducto()
        {
            txtIdProducto.Text = "0";
            txtCodProducto.Text = "";
            txtProducto.Text = "";
            txtPrecioCompra.Text = "";
            txtPrecioVenta.Text = "";
            txtCantidad.Value = 1;
            
        }

        private void dgvData_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {

            if (e.RowIndex < 0)
                return;
            if (e.ColumnIndex == 6)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);
                var w = Properties.Resources.delete.Width;
                var h = Properties.Resources.delete.Height;
                var x = e.CellBounds.Left + (e.CellBounds.Width / 3);
                var y = e.CellBounds.Top + (e.CellBounds.Height / 3);

                e.Graphics.DrawImage(Properties.Resources.delete, new Rectangle(x, y, w, h));
                e.Handled = true;

            }
        }

        private void Calcular_Total()
        {
            decimal total = 0;
            if(dataGridView1.Rows.Count >0)
            {
                foreach(DataGridViewRow row in dataGridView1.Rows)
                {
                    total += Convert.ToDecimal(row.Cells["SubTotal"].Value.ToString());
                }
                txtTotalPagar.Text = total.ToString("N2");
            }
        }
        private void txtPrecioCompra_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || e.KeyChar == ',' || e.KeyChar == '\b')
            {
                e.Handled = false; 
            }
            else
            {
                e.Handled = true; 
            }

            if (e.KeyChar == '.' && txtPrecioCompra.Text.Contains('.'))
            {
                e.Handled = true;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "btnEliminar")
            {
                int indice = e.RowIndex;

                if (indice >= 0)
                {
                    dataGridView1.Rows.RemoveAt(indice);
                    Calcular_Total();   
                }
            }
        }

        private void btnFinalizar_Click(object sender, EventArgs e)
        {
            if(Convert.ToInt32(txtIdProveedor.Text) == 0)
            {
                MessageBox.Show("Debe seleccionar el proveedor", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if(dataGridView1.Rows.Count <1 ) {
                MessageBox.Show("Debe ingresar productos en la compra", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            DataTable detalle_compra = new DataTable();
            detalle_compra.Columns.Add("IdProducto",typeof(int));
            detalle_compra.Columns.Add("PrecioCompra", typeof(decimal));
            detalle_compra.Columns.Add("PrecioVenta", typeof(decimal));
            detalle_compra.Columns.Add("Cantidad", typeof(int));
            detalle_compra.Columns.Add("SubTotal", typeof(decimal));
            foreach(DataGridViewRow row in dataGridView1.Rows)
            {
                detalle_compra.Rows.Add(new object[]
                {
                    Convert.ToInt32(row.Cells["IdProducto"].Value.ToString()),
                    row.Cells["PrecioCompra"].Value.ToString(),
                    row.Cells["PrecioVenta"].Value.ToString(),
                    row.Cells["Cantidad"].Value.ToString(),
                    row.Cells["SubTotal"].Value.ToString()
                });
            }

            int idCorrelativo = new Cn_Compra().ObtenerCorrelativo();
            string numerodocumento = string.Format("{0:00000}",idCorrelativo);

            Compra oCompra = new Compra()
            {
                oUsuario = new Usuario() { IdUsuario = _Usuario.IdUsuario },
                oProveedor = new Proveedor() { IdProveedor = Convert.ToInt32(txtIdProveedor.Text ) },
                TipoDocumento = ((OpcionCombo)cboTipoDocumento.SelectedItem).Texto,
                NumeroDocumento = numerodocumento,
                MontoTotal = Convert.ToDecimal(txtTotalPagar.Text),
            };
            string mensaje = string.Empty;
            bool respuesta = new Cn_Compra().Registrar(oCompra,detalle_compra,out mensaje);
            if (respuesta)
            {
                var result = MessageBox.Show("Su numero de compra generado fue :\n" + numerodocumento + "\n\n¿Desea copiar al portapales?","Mensaje",MessageBoxButtons.YesNo,MessageBoxIcon.Information);
                if(result == DialogResult.Yes)
                {
                    Clipboard.SetText(numerodocumento);
                    txtIdProveedor.Text = "0";
                    txtNumeroDocumentoProveedor.Text = "";
                    txtRazonSocial.Text = "";
                    dataGridView1.Rows.Clear();
                    Calcular_Total();
                }
                else
                {
                    MessageBox.Show(mensaje,"Mensaje",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                }

            }
        }
    }
}
