using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public  class Cn_Negocio
    {
        private CD_Negocio objcd_Negocio = new CD_Negocio();

        public Negocio ObtenerDatos()
        {
            return objcd_Negocio.ObtenerDatos();
        }

        public bool AlmacenarDatos(Negocio obj, out string Mensaje)
        {
            Mensaje = String.Empty;

            if (obj.RUC == "")
            {
                Mensaje += "Es necesario ingresar un RUC para el negocio\n";
            }
            if (obj.Direccion == "")
            {
                Mensaje += "Es necesario ingresar la direccion del negocio\n";
            }
            if (obj.Nombre == "")
            {
                Mensaje += "Es necesario ingresar un nombre del negocion";
            }
            if (Mensaje != string.Empty)
            {
                return false;
            }
            else
            {
                return objcd_Negocio.AlmacenarDatos(obj, out Mensaje);
            }


        }
        public byte[] ObtenerLogo(out bool obtenido , int id)
        {
            if(id != 0)
            {
                return objcd_Negocio.ObtenerLogo(out obtenido, id);
            }
            else
            {
                return objcd_Negocio.ObtenerLogo(out obtenido, 1);
            }
            
        }

        public bool ActualizarLogo(byte [] imagen,out string mensaje, int id)
        {
            if(id != 0)
            {
                return objcd_Negocio.ActualizarLogo(imagen, out mensaje, id);
            }
            else
            {
                return objcd_Negocio.ActualizarLogo(imagen, out mensaje, 1);
            }
           
        }

    }
}
