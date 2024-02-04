using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class Cn_Cliente
    {
        private CD_Clientes objcd_Cliente = new CD_Clientes();

        public List<Cliente> Listar()
        {
            return objcd_Cliente.Listar();
        }

        public int Registrar(Cliente obj, out string Mensaje)
        {
            Mensaje = String.Empty;

            if (obj.Documento == "")
            {
                Mensaje += "Es necesario ingresar un documento de identificacion\n";
            }
            if (obj.NombreCompleto == "")
            {
                Mensaje += "Es necesario ingresar un nombre para el cliente\n";
            }
            if (obj.Telefono == "")
            {
                Mensaje += "Es necesario ingresar una numero de telefono\n";
            }
            if (obj.Correo == "")
            {
                Mensaje += "Es necesario ingresar un correo para contactar al cliente \n";
            }
            if (obj.DireccionDomicilio == "")
            {
                Mensaje += "Es necesario ingresar un lugar de domicilio del cliente \n";
            }
            if (Mensaje != string.Empty)
            {
                return 0;
            }
            else
            {
                return objcd_Cliente.Registrar(obj, out Mensaje);
            }


        }

        public bool Editar(Cliente obj, out string Mensaje)
        {
            Mensaje = String.Empty;

            if (obj.Documento == "")
            {
                Mensaje += "Es necesario ingresar un documento de identificacion\n";
            }
            if (obj.NombreCompleto == "")
            {
                Mensaje += "Es necesario ingresar un nombre para el cliente\n";
            }
            if (obj.Telefono == "")
            {
                Mensaje += "Es necesario ingresar una numero de telefono\n";
            }
            if (obj.Correo == "")
            {
                Mensaje += "Es necesario ingresar un correo para contactar al cliente \n";
            }
            if (obj.DireccionDomicilio == "")
            {
                Mensaje += "Es necesario ingresar un lugar de domicilio del cliente\n";
            }
            if (Mensaje != string.Empty)
            {
                return false;
            }
            else
            {
                return objcd_Cliente.Editar(obj, out Mensaje);
            }


        }

        public bool Eliminar(Cliente obj, out string Mensaje)
        {
            return objcd_Cliente.Eliminar(obj, out Mensaje);
        }
    }
}
