﻿using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.Utilidades;
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
    public partial class frmDetalleCompra : Form
    {
        public frmDetalleCompra()
        {
            InitializeComponent();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            Compra oCompra = new Cn_Compra().ObtenerCompra(txtBusqueda.Text);

            if(oCompra.IdCompra != 0)
            {
                txtNumeroDocumento.Text = oCompra.NumeroDocumento;

                txtFecha.Text = oCompra.FechaRegistro;
                txtTipoDocumento.Text= oCompra.TipoDocumento;
                txtUsuario.Text = oCompra.oUsuario.NombreCompleto;
                txtDocProveedor.Text = oCompra.oProveedor.Documento;
                txtNombreProveedor.Text = oCompra.oProveedor.RazonSocial;
                dataGridView1.Rows.Clear();
                foreach (Detalle_Compra dc in oCompra.oDetalle_Compra)
                {
                    dataGridView1.Rows.Add(new object[]
                    {
                        dc.oProducto.Nombre,dc.PrecioCompra,dc.Cantidad,dc.MontoTotal
                    });
                }

                    txtMontoTotal.Text = oCompra.MontoTotal.ToString("0.00");
                }
            
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtFecha.Text = "";
            txtTipoDocumento.Text = "";
            txtUsuario.Text = "";
            txtDocProveedor.Text = "";
            txtNombreProveedor.Text = "";
            dataGridView1.Rows.Clear();
            txtMontoTotal.Text = "0.00";

        }

        private void btnDescargarPDF_Click(object sender, EventArgs e)
        {
            if (txtTipoDocumento.Text == "")
            {
                MessageBox.Show("No se encontrarion resultados", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string Texto_Html = Properties.Resources.PlantillaCompra.ToString();
            Negocio odatos = new Cn_Negocio().ObtenerDatos();
            Texto_Html = Texto_Html.Replace("@nombrenegocio", odatos.Nombre.ToUpper());
            Texto_Html = Texto_Html.Replace("@docnegocio", odatos.RUC);
            Texto_Html = Texto_Html.Replace("@direcnegocio", odatos.Direccion);

            Texto_Html = Texto_Html.Replace("@tipodocumento",txtTipoDocumento.Text.ToUpper());
            Texto_Html = Texto_Html.Replace("@numerodocumento", txtNumeroDocumento.Text);

            Texto_Html = Texto_Html.Replace("@docproveedor", txtDocProveedor.Text);
            Texto_Html = Texto_Html.Replace("@nombreproveedor", txtNombreProveedor.Text);
            Texto_Html = Texto_Html.Replace("@fecharegistro", txtFecha.Text);
            Texto_Html = Texto_Html.Replace("@usuarioregistro", txtUsuario.Text);

            string filas = string.Empty;
            foreach(DataGridViewRow row in dataGridView1.Rows)
            {
                filas += "<tr>";
                filas += "<td>" + row.Cells["Producto"].Value + "</td>";
                filas += "<td>" + row.Cells["PrecioCompra"].Value + "</td>";
                filas += "<td>" + row.Cells["Cantidad"].Value + "</td>";
                filas += "<td>" + row.Cells["SubTotal"].Value + "</td>";
                filas += "</tr>";
            }
            Texto_Html = Texto_Html.Replace("@filas", filas);
            Texto_Html = Texto_Html.Replace("@montototal", txtMontoTotal.Text);

            SaveFileDialog savefile = new SaveFileDialog();
            savefile.FileName = string.Format("Reporte_Compra_{0}.pdf", txtNumeroDocumento.Text);
            savefile.Filter = "Pdf  Files|*.pdf";

            if(savefile.ShowDialog() == DialogResult.OK)
            {
                using (FileStream stream = new FileStream(savefile.FileName, FileMode.Create))
                {
                    Document pdfDoc = new Document(PageSize.A4,25,25,25,25);

                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    bool obtenido = true;
                    int id = 1;
                    byte[] byteImage = new Cn_Negocio().ObtenerLogo(out obtenido ,id);
                    if(obtenido) {
                        iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(byteImage);
                        img.ScaleToFit(60, 60);
                        img.Alignment = iTextSharp.text.Image.UNDERLYING;
                        img.SetAbsolutePosition(pdfDoc.Left, pdfDoc.GetTop(51));
                        pdfDoc.Add(img);
                    }

                    using(StringReader sr = new StringReader(Texto_Html))
                    {
                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                    }

                    pdfDoc.Close();
                    stream.Close(); 
                    MessageBox.Show("Pdf Generado","Mensaje",MessageBoxButtons.OK,MessageBoxIcon.Information);
                }   
            }    

        }

        private void frmDetalleCompra_Load(object sender, EventArgs e)
        {
           
        }
    }
}
