using CapaEntidad;
using CapaNegocio;
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
    public partial class frmNegocio : Form
    {
        public frmNegocio()
        {
            InitializeComponent();
        }

        public Image ByteaImagen(byte[] imagenabyte)
        {
            MemoryStream ms = new MemoryStream();
            ms.Write(imagenabyte, 0, imagenabyte.Length);
            Image image = new Bitmap(ms);
            return image;
        }

        private void frmNegocio_Load(object sender, EventArgs e)
        {
            bool obtenido = true;
            int idlogo = 1;
            idlogo = Convert.ToInt32(txtID.Text);
            byte[] byteimage = new Cn_Negocio().ObtenerLogo(out obtenido, idlogo);
            if(obtenido)          
                pbxLogo.Image = ByteaImagen(byteimage);
            Negocio datos = new Cn_Negocio().ObtenerDatos();
            txtNombre.Text = datos.Nombre;
            txtRUC.Text = datos.RUC;
            txtDireccion.Text = datos.Direccion;



        }

        private void btnSubir_Click(object sender, EventArgs e)
        {
            string mensaje = string.Empty;
            int idlogo = 1;
            idlogo = Convert.ToInt32(txtID.Text);
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = "files |*.jpg;*.jpeg;*.png";
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                byte[] byteimage = File.ReadAllBytes(ofd.FileName);
                bool respuesta = new Cn_Negocio().ActualizarLogo(byteimage, out mensaje,idlogo);
                if (respuesta)
                    pbxLogo.Image = ByteaImagen(byteimage);
                else 
                    MessageBox.Show(mensaje,"Mensaje",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            String mensaje = string.Empty;
            Negocio objNegocio = new Negocio()
            {
                Nombre = txtNombre.Text,
                RUC = txtRUC.Text,
                Direccion = txtDireccion.Text
            };
            bool respuesta = new Cn_Negocio().AlmacenarDatos(objNegocio ,out mensaje);
            if(respuesta)
            {
                MessageBox.Show("Los cambios fueron guardados correctamente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } 
            else
            {
                MessageBox.Show("No se pudo guardar los cambios", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        //Genera un data gried con los negocios a traves de id
    }
}
