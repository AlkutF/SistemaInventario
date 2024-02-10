using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaPresentacion
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-ES");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Esta linea da el inicio del aplicativo , si no es para pruebas mantener el valor en login
            Application.Run(new Login());
        }
    }
}
