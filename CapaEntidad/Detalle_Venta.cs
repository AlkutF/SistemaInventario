using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Detalle_Venta
    {
        public int Id_Detalle_Venta { get; set; }
        public Venta Id_Venta { get; set; }//Eso puede ser eliminado por que ya esta en la clase venta
        public Producto Id_Producto { get; set; }
        public decimal Precio_Venta_Detalle_Venta { get; set; }
        public int Cantidad_Detalle_Venta { get; set; }
        public decimal Subtotal_Detalle_Venta { get; set; }
        public string Fecha_Registro_Detalle_Venta { get; set; }
    }
}
