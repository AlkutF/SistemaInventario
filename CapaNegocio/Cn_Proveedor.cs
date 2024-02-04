using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class Cn_Proveedor
    {
        private CD_Proveedor objcd_Proveedor = new CD_Proveedor();

        public List<Proveedor> Listar()
        {
            return objcd_Proveedor.Listar();
        }

        public int Registrar(Proveedor obj, out string Mensaje)
        {
            Mensaje = String.Empty;

            if (obj.Documento == "")
            {
                Mensaje += "Es necesario ingresar un numero de documento para el proveedor\n";
            }
            if (obj.RazonSocial == "")
            {
                Mensaje += "Es necesario ingresar la razon social del proveedor\n";
            }
            if (obj.Correo == "")
            {
                Mensaje += "Es necesario ingresar un correo de contacto para el proveedor \n";
            }
            if (obj.Telefono == "")
            {
                Mensaje += "Es necesario ingresar un numero de telefono para contactar al proveedor \n";
            }
            if (Mensaje != string.Empty)
            {
                return 0;
            }
            else
            {
                return objcd_Proveedor.Registrar(obj, out Mensaje);
            }


        }

        public bool Editar(Proveedor obj, out string Mensaje)
        {
            Mensaje = String.Empty;

            if (obj.Documento == "")
            {
                Mensaje += "Es necesario ingresar un numero de documento para el proveedor\n";
            }
            if (obj.RazonSocial == "")
            {
                Mensaje += "Es necesario ingresar la razon social del proveedor\\n";
            }
            if (obj.Correo == "")
            {
                Mensaje += "Es necesario ingresar un correo de contacto para el proveedorn";
            }
            if (obj.Telefono == "")
            {
                Mensaje += "Es necesario ingresar un numero de telefono para contactar al proveedor\n";
            }
            if (Mensaje != string.Empty)
            {
                return false;
            }
            else
            {
                return objcd_Proveedor.Editar(obj, out Mensaje);
            }


        }

        public bool Eliminar(Proveedor obj, out string Mensaje)
        {
            return objcd_Proveedor.Eliminar(obj, out Mensaje);
        }
    }
}
