using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string Codigo_Usuario { get; set; }
        public string Nombre_Completo_Usuario { get; set; }
        public string Correo_Usuario { get; set; }
        public string Clave_Usuario { get; set; }
        public Rol Rol { get; set; }
        public bool Estado_Usuario { get; set; }
        public string Fecha_Creacion_Usuario { get; set; }

    }
