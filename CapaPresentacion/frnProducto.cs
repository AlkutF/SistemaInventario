using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.Utilidades;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Resources.ResXFileRef;
using SaveFileDialog = System.Windows.Forms.SaveFileDialog;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Wordprocessing;

namespace CapaPresentacion
{
    public partial class frmProducto : Form
    {

        public frmProducto()
        {
            InitializeComponent();

        }

        private void frnProducto_Load(object sender, EventArgs e)
        {  
            cboEstado.Items.Add(new OpcionCombo() { Valor = 1, Texto = "Activo " });
            cboEstado.Items.Add(new OpcionCombo() { Valor = 0, Texto = "No activo" });
            cboEstado.DisplayMember = "Texto";
            cboEstado.ValueMember = "Valor";

            List<Categoria> ListaCategoria = new Cn_Categoria().Listar();

            foreach (Categoria item in ListaCategoria)
            {
                cboCategoria.Items.Add(new OpcionCombo() { Valor = item.IdCategoria, Texto = item.Descripcion });
            }
            cboCategoria.DisplayMember = "Texto";
            cboCategoria.ValueMember = "Valor";
            cboCategoria.SelectedIndex = 0;

            foreach (DataGridViewColumn columna in dgvData.Columns)
            {
                if (columna.Visible == true && columna.Name != "btnSelecionar")
                {
                    cboBusqueda.Items.Add(new OpcionCombo() { Valor = columna.Name, Texto = columna.HeaderText });
                }
            }
            cboBusqueda.DisplayMember = "Texto";
            cboBusqueda.ValueMember = "Valor";
            cboBusqueda.SelectedIndex = 0;

            //Intento de cargar todos los productos
            List<Producto> Lista = new Cn_Productos().Listar();
                
                  
            foreach (Producto item in Lista)
            {
                dgvData.Rows.Add(new object[] {
                    "",
                    item.IdProducto ,
                    item.Codigo ,
                    item.Nombre ,
                    item.Descripcion,
                    item.oCategoria.IdCategoria,
                    item.oCategoria.Descripcion,
                    item.Stock ,
                    item.PrecioCompra ,
                    item.PrecioVenta ,
                    item.Estado== true ?1 : 0,
                    item.Estado== true ?"Activo" : "No activo"
            }

                ); ;
            }
           
        }

        public Image ByteaImagen(byte[] imagenabyte)
        {
            MemoryStream ms = new MemoryStream();
            ms.Write(imagenabyte, 0, imagenabyte.Length);
            Image image = new Bitmap(ms);
            return image;
        }




        private void btnGuardar_Click(object sender, EventArgs e)
        {

            String Mensaje = string.Empty;

            Producto objProducto = new Producto()
            {
                IdProducto = Convert.ToInt32(txtId.Text),
                Codigo = txtCodigo.Text,
                Nombre = txtNombre.Text,
                Descripcion = txtDescripcion.Text,
                oCategoria = new Categoria() { IdCategoria = Convert.ToInt32(((OpcionCombo)cboCategoria.SelectedItem).Valor) },
                Estado = Convert.ToInt32(((OpcionCombo)cboEstado.SelectedItem).Valor) == 1 ? true : false,
                
            };

            if (objProducto.IdProducto == 0)
            {
                int idproductogenerado = new Cn_Productos().Registrar(objProducto, out Mensaje);

                if (idproductogenerado != 0)
                {
                    dgvData.Rows.Add(new object[] {
                        "",
                        idproductogenerado ,
                        txtCodigo.Text,
                        txtNombre.Text,
                        txtDescripcion.Text,
                        ((OpcionCombo)cboCategoria.SelectedItem).Valor.ToString(),
                        ((OpcionCombo)cboCategoria.SelectedItem).Texto.ToString(),
                        "0",
                        "0.00",
                        "0.00",
                        ((OpcionCombo)cboEstado.SelectedItem).Valor.ToString(),
                        ((OpcionCombo)cboEstado.SelectedItem).Texto.ToString(),

            }); ;
                    Limpiar();
                }
                else
                {
                    MessageBox.Show(Mensaje);
                }
            }
            else
            {
                bool resultado = new Cn_Productos().Editar(objProducto, out Mensaje);
                if (resultado)
                {
                    DataGridViewRow row = dgvData.Rows[Convert.ToInt32(txtIndice.Text)];
                    row.Cells["Id"].Value = txtId.Text;
                    row.Cells["Codigo"].Value = txtCodigo.Text;
                    row.Cells["Nombre"].Value = txtNombre.Text;
                    row.Cells["Descripcion"].Value = txtDescripcion.Text;
                    row.Cells["IdCategoria"].Value = ((OpcionCombo)cboCategoria.SelectedItem).Valor.ToString();
                    row.Cells["Categoria"].Value = ((OpcionCombo)cboCategoria.SelectedItem).Texto.ToString();
                    row.Cells["EstadoValor"].Value = ((OpcionCombo)cboEstado.SelectedItem).Valor.ToString();
                    row.Cells["Estado"].Value = ((OpcionCombo)cboEstado.SelectedItem).Texto.ToString();
                    Limpiar();
                    
                }
                else { MessageBox.Show(Mensaje); }
            }
        }
        private void Limpiar()
        {
            txtIndice.Text = "-1";
            txtId.Text = "0";
            txtCodigo.Text = "";
            txtNombre.Text = "";
            txtDescripcion.Text = "";
            cboCategoria.SelectedIndex = 0;
            cboEstado.SelectedIndex = 0;
            txtCodigo.Select();
            pbxLogo.Visible= false;
            btnSubir.Visible = false;

        }
        private void dgvData_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {

            if (e.RowIndex < 0)
                return;
            if (e.ColumnIndex == 0)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);
                var w = Properties.Resources.check_157977.Width;
                var h = Properties.Resources.check_157977.Height;
                var x = e.CellBounds.Left + (e.CellBounds.Width / 3);
                var y = e.CellBounds.Top + (e.CellBounds.Height / 3);

                e.Graphics.DrawImage(Properties.Resources.check_157977, new Rectangle(x, y, w, h));
                e.Handled = true;

            }
        }


        private void dgvData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvData.Columns[e.ColumnIndex].Name == "btnSelecionar")
            {
                int indice = e.RowIndex;
                if (indice >= 0)
                {
                    txtIndice.Text = indice.ToString();
                    txtId.Text = dgvData.Rows[indice].Cells["Id"].Value.ToString();
                    txtCodigo.Text = dgvData.Rows[indice].Cells["Codigo"].Value.ToString();
                    txtNombre.Text = dgvData.Rows[indice].Cells["Nombre"].Value.ToString();
                    txtDescripcion.Text = dgvData.Rows[indice].Cells["Descripcion"].Value.ToString();
                    foreach (OpcionCombo oc in cboCategoria.Items)
                    {
                        if (Convert.ToInt32(oc.Valor) == Convert.ToInt32(dgvData.Rows[indice].Cells["IdCategoria"].Value.ToString()))
                        {
                            int indice_combo = cboCategoria.Items.IndexOf(oc);
                            cboCategoria.SelectedIndex = indice_combo;
                            break;
                        }
                    }

                    foreach (OpcionCombo oc in cboEstado.Items)
                    {
                        if (Convert.ToInt32(oc.Valor) == Convert.ToInt32(dgvData.Rows[indice].Cells["EstadoValor"].Value.ToString()))
                        {
                            int indice_combo = cboEstado.Items.IndexOf(oc);
                            cboEstado.SelectedIndex = indice_combo;
                            break;
                        }
                    }
                    bool obtenido = true;
                    int id = Convert.ToInt32(txtId.Text);
                    byte[] byteimage = new Cn_Productos().ObtenerImagen(out obtenido, id);                
                    if (obtenido)
                    {
                        pbxLogo.Visible = true;
                        btnSubir.Visible = true;
                        pbxLogo.Image = ByteaImagen(byteimage);
                    }
                    else
                    {
                        btnSubir.Visible = true;
                    }
                }
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(txtId.Text) != 0)
            {
                if (MessageBox.Show("¿Desea Eliminar el producto", "Mensaje de confirmacion", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                    DialogResult.Yes)
                {
                    string mensaje = string.Empty;
                    Producto objProducto = new Producto()
                    {
                        IdProducto = Convert.ToInt32(txtId.Text),
                    };

                    bool respuesta = new Cn_Productos().Eliminar(objProducto, out mensaje);

                    if (respuesta)
                    {
                        dgvData.Rows.RemoveAt(Convert.ToInt32(txtIndice.Text));
                        Limpiar();
                    }
                    else
                    {
                        MessageBox.Show(mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            if(dgvData.Rows.Count < 1)
            {
                MessageBox.Show("No hay datos para generar el exel", "Mensaje",MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                DataTable dt = new DataTable();
                foreach(DataGridViewColumn Columna in dgvData.Columns)
                {
                    if (Columna.HeaderText != "" && Columna.Visible)
                        dt.Columns.Add(Columna.HeaderText, typeof(String));
                }
                foreach (DataGridViewRow row in dgvData.Rows)
                {
                    if (row.Visible)
                        dt.Rows.Add(new object[] {
                            row.Cells[2].Value.ToString(),
                            row.Cells[3].Value.ToString(),
                            row.Cells[4].Value.ToString(),
                            row.Cells[6].Value.ToString(),
                            row.Cells[7].Value.ToString(),
                            row.Cells[8].Value.ToString(),
                            row.Cells[9].Value.ToString(),
                            row.Cells[11].Value.ToString(),

                        });
                }
                SaveFileDialog savefile = new SaveFileDialog();
                savefile.FileName = string.Format("Reporte_Productos_{0}.xlss", DateTime.Now.ToString("ddMMyyyyHHmmss"));
                savefile.Filter = "Excel Files | *.xlsx";

                if(savefile.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        XLWorkbook wb = new XLWorkbook();
                        var hoja = wb.Worksheets.Add(dt, "Informe");
                        hoja.ColumnsUsed().AdjustToContents();
                        wb.SaveAs(savefile.FileName);
                        MessageBox.Show("Reporte Generado","Mensaje",MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch
                    {
                        MessageBox.Show("Error al generar el reporte", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            

        }


        private void btnBuscar_Click_1(object sender, EventArgs e)
        {
            string buscador = ((OpcionCombo)cboBusqueda.SelectedItem).Valor.ToString();

            if (dgvData.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvData.Rows)
                {
                    if (row.Cells[buscador].Value.ToString().Trim().ToUpper().Contains(txtBusqueda.Text.Trim().ToUpper()))
                    {
                        row.Visible = true;
                    }
                    else { row.Visible = false; }
                }

            }
        }

        private void btnLimpiar_Click_1(object sender, EventArgs e)
        {
            txtBusqueda.Text = "";
            foreach (DataGridViewRow row in dgvData.Rows)
            {
                row.Visible = true;
            }
        }

        private void btnSubir_Click(object sender, EventArgs e)
        {
            string mensaje = string.Empty;
            int id = Convert.ToInt32(txtId.Text);
            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
            ofd.FileName = "files |*.jpg;*.jpeg;*.png";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                byte[] byteimage = File.ReadAllBytes(ofd.FileName);
                bool respuesta = new Cn_Productos().ActualizarImagen(byteimage, out mensaje, id);
                if (respuesta)
                    pbxLogo.Image = ByteaImagen(byteimage);
                else
                    MessageBox.Show(mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
