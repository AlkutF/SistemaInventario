﻿using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class Cn_Permiso
    {
        private CD_Permiso objcd_Permiso = new CD_Permiso();

        public List<Permiso> Listar(int IdUsuario)
        {
            return objcd_Permiso.Listar(IdUsuario);
        }
    }
}
