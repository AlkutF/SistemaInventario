using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class Cn_Productos
    {
        private CD_Productos objcd_Productos = new CD_Productos();

        public List<Producto> Listar()
        {
            return objcd_Productos.Listar();
        }

        public int Registrar(Producto obj, out string Mensaje)
        {
            Mensaje = String.Empty;

            if (obj.Codigo == "")
            {
                Mensaje += "Es necesario ingresar un codigo para el producto\n";
            }
            if (obj.Nombre == "")
            {
                Mensaje += "Es necesario ingresar un nombre de producto\n";
            }
            if (obj.Descripcion == "")
            {
                Mensaje += "Es necesario ingresar una descripcion\n";
            }
            if (Mensaje != string.Empty)
            {
                return 0;
            }
            else
            {
                return objcd_Productos.Registrar(obj, out Mensaje);
            }


        }

        public bool Editar(Producto obj, out string Mensaje)
        {
            Mensaje = String.Empty;

            if (obj.Codigo == "")
            {
                Mensaje += "Es necesario ingresar un codigo para el producto\n";
            }
            if (obj.Nombre == "")
            {
                Mensaje += "Es necesario ingresar un nombre de producto\n";
            }
            if (obj.Descripcion == "")
            {
                Mensaje += "Es necesario ingresar una descripcion\n";
            }
            if (Mensaje != string.Empty)
            {
                return false;
            }
            else
            {
                return objcd_Productos.Editar(obj, out Mensaje);
            }


        }

        public bool Eliminar(Producto obj, out string Mensaje)
        {
            return objcd_Productos.Eliminar(obj, out Mensaje);
        }
    }
}
