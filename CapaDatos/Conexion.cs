using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace CapaDatos
{   //La conexion depende del App.config , si cambiamos de pc hay que cambiar el App.config el connectionString
    public class Conexion
    {
        public static string cadena = ConfigurationManager.ConnectionStrings["Cadena_Conexion"].ToString();
    }
}
