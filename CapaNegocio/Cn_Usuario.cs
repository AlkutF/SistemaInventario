using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class Cn_Usuario
    {
        private CD_Usuario objcd_Usuario = new CD_Usuario();

        public List<Usuario> Listar()
        {
            return objcd_Usuario.Listar();
        }

        public int Registrar(Usuario obj ,out string Mensaje)
        {
            Mensaje = String.Empty;

            if (obj.Documento == "")
            {
                Mensaje += "Es necesario ingresar un documento de identificacion\n";
            }
            if (obj.NombreCompleto == "")
            {
                Mensaje += "Es necesario ingresar un nombre completo\n";
            }
            if (obj.Clave == "")
            {
                Mensaje += "Es necesario ingresar una clave para su documento\n";
            }
           if(Mensaje != string.Empty)
            {
                return 0;
            }else
            {
                return objcd_Usuario.Registrar(obj, out Mensaje);
            }

           
        }

        public bool Editar(Usuario obj, out string Mensaje)
        {
            Mensaje = String.Empty;

            if (obj.Documento == "")
            {
                Mensaje += "Es necesario ingresar un documento de identificacion\n";
            }
            if (obj.NombreCompleto == "")
            {
                Mensaje += "Es necesario ingresar un nombre completo\n";
            }
            if (obj.Clave == "")
            {
                Mensaje += "Es necesario ingresar una clave para su documento\n";
            }
            if (Mensaje != null)
            {
                return false;
            }
            else
            {
                return objcd_Usuario.Editar(obj, out Mensaje);
            }

            
        }

        public bool Eliminar(Usuario obj, out string Mensaje)
        {
            return objcd_Usuario.Eliminar(obj, out Mensaje);
        }
    }
}

