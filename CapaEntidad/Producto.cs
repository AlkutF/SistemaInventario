using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Producto
    {
        public int IdProducto { get; set; }
        public Categoria Categoria { get; set; }
        public string Codigo_Producto { get; set; }
        public string Nombre_Producto { get; set; }
        public string Descripcion_Producto { get; set; }
        public string Stock_Producto { get; set; }
        public string Precio_Compra_Producto { get; set; }
        public string Precio_Venta_Producto { get; set; }
        public bool Estado_Producto { get; set; }
        public string Fecha_Vencimiento_Producto { get; set; }
        public string Fecha_Registro_Producto { get; set; }
        public byte[] Imagen_Producto { get; set; }
    }
}
