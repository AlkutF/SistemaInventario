using CapaEntidad;
using CapaNegocio;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class frmDetalleVenta : Form
    {
        public frmDetalleVenta()
        {
            InitializeComponent();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            Venta oVenta = new CapaNegocio.Cn_Venta().ObtenerVenta(txtBusqueda.Text);

            if(oVenta.idVenta != 0)
            {
                txtNumeroDocumento.Text = oVenta.NumeroDocumento;
                txtFecha.Text = oVenta.FechaRegistro;
                txtTipoDocumento.Text = oVenta.TipoDocumento;
                txtUsuario.Text = oVenta.oUsuario.NombreCompleto;

                txtDocCliente.Text = oVenta.DocumentoCliente;
                txtNombreCliente.Text = oVenta.NombreCliente;
                txtMontoTotal.Text = oVenta.MontoTotal.ToString("0.00");
                txtMontoPago.Text = oVenta.MontoPago.ToString("0.00");
                txtMontoCambio.Text = oVenta.MontoCambio.ToString("0.00");


                dataGridView1.Rows.Clear();
                foreach(Detalle_Venta dv in oVenta.oDetalle_Ventas)
                {
                    dataGridView1.Rows.Add(new object[]
                    {
                        dv.oProducto.Nombre, dv.PrecioVenta, dv.Cantidad, dv.MontoTotal
                    });
                }

               
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtNumeroDocumento.Text = "";
            txtFecha.Text = "";
            txtTipoDocumento.Text = "";
            txtUsuario.Text = "";

            txtDocCliente.Text = "";
            txtNombreCliente.Text = "";
            txtMontoTotal.Text = "0.00";
            txtMontoPago.Text = "0.00";
            txtMontoCambio.Text = "0.00";
        }

        private void frmDetalleVenta_Load(object sender, EventArgs e)
        {
            txtBusqueda.Select();
        }

        private void btnDescargarPDF_Click(object sender, EventArgs e)
        {
            if (txtTipoDocumento.Text == "")
            {
                MessageBox.Show("No se encontrarion resultados", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string Texto_Html = Properties.Resources.PlantillaVenta.ToString();
            Negocio odatos = new Cn_Negocio().ObtenerDatos();
            Texto_Html = Texto_Html.Replace("@nombrenegocio", odatos.Nombre.ToUpper());
            Texto_Html = Texto_Html.Replace("@docnegocio", odatos.RUC);
            Texto_Html = Texto_Html.Replace("@direcnegocio", odatos.Direccion);

            Texto_Html = Texto_Html.Replace("@tipodocumento", txtTipoDocumento.Text.ToUpper());
            Texto_Html = Texto_Html.Replace("@numerodocumento", txtNumeroDocumento.Text);

            Texto_Html = Texto_Html.Replace("@doccliente", txtDocCliente.Text);
            Texto_Html = Texto_Html.Replace("@nombrecliente", txtNombreCliente.Text);
            Texto_Html = Texto_Html.Replace("@fecharegistro", txtFecha.Text);
            Texto_Html = Texto_Html.Replace("@usuarioregistro", txtUsuario.Text);

            string filas = string.Empty;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                filas += "<tr>";
                filas += "<td>" + row.Cells["Producto"].Value + "</td>";
                filas += "<td>" + row.Cells["Precio"].Value + "</td>";
                filas += "<td>" + row.Cells["Cantidad"].Value + "</td>";
                filas += "<td>" + row.Cells["SubTotal"].Value + "</td>";
                filas += "</tr>";
            }
            Texto_Html = Texto_Html.Replace("@filas", filas);
            Texto_Html = Texto_Html.Replace("@montototal", txtMontoTotal.Text);
            Texto_Html = Texto_Html.Replace("@pagocon", txtMontoPago.Text);
            Texto_Html = Texto_Html.Replace("@cambio", txtMontoCambio.Text);

            SaveFileDialog savefile = new SaveFileDialog();
            savefile.FileName = string.Format("Reporte_Venta{0}.pdf", txtNumeroDocumento.Text);
            savefile.Filter = "Pdf  Files|*.pdf";

            if (savefile.ShowDialog() == DialogResult.OK)
            {
                using (FileStream stream = new FileStream(savefile.FileName, FileMode.Create))
                {
                    Document pdfDoc = new Document(PageSize.A4, 25, 25, 25, 25);

                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    bool obtenido = true;
                    byte[] byteImage = new Cn_Negocio().ObtenerLogo(out obtenido);
                    if (obtenido)
                    {
                        iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(byteImage);
                        img.ScaleToFit(60, 60);
                        img.Alignment = iTextSharp.text.Image.UNDERLYING;
                        img.SetAbsolutePosition(pdfDoc.Left, pdfDoc.GetTop(51));
                        pdfDoc.Add(img);
                    }

                    using (StringReader sr = new StringReader(Texto_Html))
                    {
                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                    }

                    pdfDoc.Close();
                    stream.Close();
                    MessageBox.Show("Pdf Generado", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}
