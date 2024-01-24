using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Cliente
    {
        public int Id_Cliente { get; set; }
        public Venta Id_Venta { get; set; }
        public string Codigo_Cliente { get; set; }
        public string NombreCompleto_Cliente { get; set; }
        public string Correo_Cliente { get; set; }
        public string Telefono_Cliente { get; set; }
        public bool Estado_Cliente { get; set; }
        public string Fecha_Creacion_Cliente { get; set; }
    }
}
