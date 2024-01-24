using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Detalle_Compra
    {
        public int Id_Detalle_Compra { get; set; }
        public Compra Id_Compra { get; set; }//Esto puede ser eliminado por que ya esta en la clase compra
        public Producto Id_Producto { get; set; }
        public decimal Precio_Compra_Detalle_Compra { get; set; }
        public decimal Precio_Venta_Detalle_Compra { get; set; }
        public int Cantidad_Detalle_Compra { get; set; }
        public int Montol_Total_Detalle_Compra { get; set; }
        public string Fecha_Creacion_Detalle_Compra { get; set; }
    }
