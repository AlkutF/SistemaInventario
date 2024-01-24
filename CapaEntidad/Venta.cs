using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Venta
    {
        public int Id_Venta { get; set; }
        public Usuario Id_Usuario { get; set; }
        public Cliente Id_Cliente { get; set; }
        public string Tipo_Documento_Venta { get; set; }
        public string Codigo_Documento_Venta { get; set; }
        public decimal Monto_Pago_Venta { get; set; }
        public decimal Monto_Cambio_Venta { get; set; }
        public decimal Monto_Total_Venta { get; set; }
        public List<Detalle_Venta> Detalle_Venta { get; set; }
        public string Fecha_Registro_Venta { get; set; }


    }
}
