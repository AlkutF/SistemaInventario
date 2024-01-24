using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Proveedor
    {
        public int IdProveedor { get; set; }
        public string Codigo_Proveedor { get; set; }
        public string Razon_Social_Proveedor { get; set; }
        public string Correo_Proveedor { get; set; }
        public string Telefono_Proveedor { get; set; }
        public bool Estado_Proveedor { get; set; }
        public string Fecha_Creacion_Proveedor { get; set; }
    }
}
