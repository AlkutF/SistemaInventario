using CapaEntidad;
using CapaNegocio;
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
    public partial class frmVentas : Form
    {
        private Usuario _Usuario;
        public frmVentas(Usuario oUsuario = null)
        {
            _Usuario = oUsuario;
            InitializeComponent();
        }

        private void frmVentas_Load(object sender, EventArgs e)
        {
            cboTipoDocumento.Items.Add(new OpcionCombo() { Valor = "Metodo Pago 1", Texto = "Metodo Pago 1", });
            cboTipoDocumento.Items.Add(new OpcionCombo() { Valor = "Metodo Pago 2", Texto = "Metodo Pago 2", });
            cboTipoDocumento.DisplayMember = "Texto";
            cboTipoDocumento.ValueMember = "Valor";
            cboTipoDocumento.SelectedIndex = 0;
            txtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtIdProducto.Text = "0";

            txtPagocon.Text = "";
            txtCambio.Text = "";
            txtTotalPagar.Text = "";
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            using (var modalC = new mdCliente())
            {
                var result = modalC.ShowDialog();
                if (result == DialogResult.OK)
                {
                    txtNumeroDocumentoCliente.Text = modalC._Cliente.Documento;
                    txtNombreCliente.Text = modalC._Cliente.NombreCompleto;
                    txtCodProducto.Select();
                }
                else
                {
                    txtNumeroDocumentoCliente.Select();
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
                    txtPrecio.Text = modalPr._Producto.PrecioVenta.ToString("0.00");
                    txtStock.Text=modalPr._Producto.Stock.ToString();
                    txtCantidad.Select();
                }
                else
                {
                    txtCodProducto.Select();
                }
            }
        }

        private void txtCodProducto_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                Producto oProducto = new Cn_Productos().Listar().Where(p => p.Codigo == txtCodProducto.Text && p.Estado == true).FirstOrDefault();
                if (oProducto != null)
                {
                    txtIdProducto.Text = oProducto.IdProducto.ToString();
                    txtProducto.Text = oProducto.Nombre;
                    txtPrecio.Text = oProducto.PrecioVenta.ToString("0.00");
                    txtStock.Text= oProducto.Stock.ToString();
                    txtCantidad.Select();
                }
                else
                {
                    txtIdProducto.Text = "0";
                    txtProducto.Text = "";
                    txtPrecio.Text="";
                    txtStock.Text = "";
                    txtCantidad.Value = 1;
                }
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            decimal precio = 0;
            bool proucto_real = false;
            if (int.Parse(txtIdProducto.Text) == 0)
            {
                MessageBox.Show("Debe seleccionar un producto", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (!decimal.TryParse(txtPrecio.Text, out precio))
            {
                MessageBox.Show("Formato incorrecto solo agregar valores numericos", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtPrecio.Select();
                return;
            }
            if(Convert.ToInt32(txtStock.Text) < Convert.ToInt32(txtCantidad.Value.ToString()))
            {
                MessageBox.Show("La cantidad vendida no puede ser mayor al stock existente ", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            foreach (DataGridViewRow fila in dataGridView1.Rows)
            {
                if (fila.Cells["IdProducto"].Value != null && fila.Cells["IdProducto"].Value.ToString() == txtIdProducto.Text)
                {
                    proucto_real = true;
                    break;
                }
            }

            if (!proucto_real)
            {
                bool respuesta = new Cn_Venta().RestarStock(
                    Convert.ToInt32(txtIdProducto.Text) ,
                    Convert.ToInt32(txtCantidad.Value.ToString())
                    );
                if (respuesta)
                {
                    dataGridView1.Rows.Add(new object[]
                {
                txtIdProducto.Text,
                txtProducto.Text,
                precio.ToString("N2"),
                txtCantidad.Value.ToString(),
                (txtCantidad.Value * precio).ToString("N2"),
                });
                    Calcular_Total();
                    LimpiarProducto();
                    txtCodProducto.Select();
                }
                
            }

        }
        private void Calcular_Total()
        {
            decimal total = 0;
            if (dataGridView1.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    total += Convert.ToDecimal(row.Cells["SubTotal"].Value.ToString());
                }
                txtTotalPagar.Text = total.ToString("N2");
            }
        }

        private void LimpiarProducto()
        {
            txtIdProducto.Text = "0";
            txtCodProducto.Text = "";
            txtProducto.Text = "";
            txtPrecio.Text = "";
            txtStock.Text = "";
            txtCantidad.Value = 1;

        }

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if (e.ColumnIndex == 5)
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "btnEliminar")
            {
                int indice = e.RowIndex;

                if (indice >= 0)
                {
                    bool respuesta = new Cn_Venta().SumarStock(
                        Convert.ToInt32(dataGridView1.Rows[indice].Cells["IdProducto"].Value.ToString()),
                        Convert.ToInt32(dataGridView1.Rows[indice].Cells["Cantidad"].Value.ToString()));
                    if (respuesta)
                    {
                        dataGridView1.Rows.RemoveAt(indice);
                        Calcular_Total();
                        LimpiarProducto();
                    }
                        
                        
                    
                }
            }
        }

        private void txtPrecio_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || e.KeyChar == ',' || e.KeyChar == '\b')
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }

            if (e.KeyChar == '.' && txtPrecio.Text.Contains('.'))
            {
                e.Handled = true;
            }
        }

        private void calcularcambio()
        {
            if(txtTotalPagar.Text.Trim() == "")
            {
                MessageBox.Show("No se ha vendido ningun producto ", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            decimal pagacon;
            decimal total = Convert.ToDecimal(txtTotalPagar.Text);
            if(txtPagocon.Text.Trim() == "")
            {
                txtPagocon.Text = "0";
            }
            if(decimal.TryParse(txtPagocon.Text.Trim(),out pagacon))
            {
                if(pagacon < total)
                {          
                    decimal cambio = pagacon - total;
                    txtCambio.Text = cambio.ToString("0.00");
                    decimal cambiopositivo = -cambio;
                    MessageBox.Show("No se ha pagado el precio por completo faltan "+ cambiopositivo+" $ \n , por favor tenerlo en cuenta", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    decimal cambio = pagacon- total;
                    txtCambio.Text=cambio.ToString("0.00");
                }
            }
        }

        private void txtPagocon_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyData == Keys.Enter)
            {
                calcularcambio();
            }
        }

        private void btnFinalizar_Click(object sender, EventArgs e)
        {
            if(txtNumeroDocumentoCliente.Text == "")
            {
                MessageBox.Show("Debe seleccionar un cliente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (txtNombreCliente.Text == "")
            {
                MessageBox.Show("Debe ingresar un nombre para el cliente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (txtPagocon.Text == "")
            {
                MessageBox.Show("Debe ingresar un monto de pago", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (dataGridView1.Rows.Count < 1)
            {
                MessageBox.Show("Debe ingresar productos en la venta", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            DataTable detalle_venta = new DataTable();
            detalle_venta.Columns.Add("IdProducto", typeof(int));
            detalle_venta.Columns.Add("PrecioVenta", typeof(decimal));
            detalle_venta.Columns.Add("Cantidad", typeof(int));
            detalle_venta.Columns.Add("SubTotal", typeof(decimal));
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                detalle_venta.Rows.Add(new object[]
                {
                    Convert.ToInt32(row.Cells["IdProducto"].Value.ToString()),
                    row.Cells["Precio"].Value.ToString(),
                    row.Cells["Cantidad"].Value.ToString(),
                    row.Cells["SubTotal"].Value.ToString()
                });
            }
            int idCorrelativo = new Cn_Venta().ObtenerCorrelativo();
            string numerodocumento = string.Format("{0:00000}", idCorrelativo);
            calcularcambio();

            Venta oVenta = new Venta()
            {
                oUsuario = new Usuario() { IdUsuario = _Usuario.IdUsuario },
                TipoDocumento = ((OpcionCombo)cboTipoDocumento.SelectedItem).Texto,
                NumeroDocumento = numerodocumento,
                DocumentoCliente =txtNumeroDocumentoCliente.Text,
                NombreCliente = txtNombreCliente.Text,
                MontoPago = Convert.ToDecimal(txtPagocon.Text),
                MontoCambio = Convert.ToDecimal(txtCambio.Text),
                MontoTotal = Convert.ToDecimal(txtTotalPagar.Text),

            };

            string mensaje = string.Empty;
            bool respuesta = new Cn_Venta().Registrar(oVenta, detalle_venta, out mensaje);
            if (respuesta)
            {
                var result = MessageBox.Show("Su numero de venta generado fue :\n" + numerodocumento + "\n\n¿Desea copiar al portapales?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (result == DialogResult.Yes)
                {
                    Clipboard.SetText(numerodocumento);
                    txtNumeroDocumentoCliente.Text = "";
                    txtNombreCliente.Text = "";
                    dataGridView1.Rows.Clear();
                    Calcular_Total();
                    txtPagocon.Text="";
                    txtCambio.Text = "";
                }
                else
                {
                    MessageBox.Show(mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }
    }
}
