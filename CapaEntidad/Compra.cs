using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Compra
    {
        public int Id_Compra { get; set; }
        public Usuario Id_Usuario { get; set; }
        public Proveedor Id_Proveedor { get; set; }
        public string Tipo_Documento_Compra { get; set; }//Boleta o factura
        public string Codigo_Documento { get; set; }
        public decimal Monto_Total_Documento { get; set; }
        public List<Detalle_Compra> Detalle_Compra { get; set; }
        public string Fecha_Creacion_Compra { get; set; }
    }
}
