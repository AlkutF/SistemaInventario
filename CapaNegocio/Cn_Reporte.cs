using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class Cn_Reporte
    {
        private CD_Reporte objReporte = new CD_Reporte();

        public List<Reporte_Compra> Compra(string fechainicio, string fechafin, int idproveedor)
        {
            return objReporte.Compra(fechainicio, fechafin,idproveedor);
        }
        public List<Reporte_Venta> Venta(string fechainicio, string fechafin)
        {
            return objReporte.Venta(fechainicio, fechafin);
        }
    }
}
