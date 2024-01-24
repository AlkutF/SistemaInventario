using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;
using CapaEntidad;
using System.Runtime.Remoting.Messaging;

namespace CapaDatos
{
    public class CD_Usuario
    {
        public List<Usuario> Listar()
        {
            List<Usuario> lista = new List<Usuario>(); 
            using(SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    string query = "Select IdUsuario,Codigo_Usuario,Nombre_Completo_Usuario,Correo_Usuario,Clave_Usuario,Estado_Usuario from Usuario";
                    SqlCommand ocmd = new SqlCommand(query, oconexion);
                    ocmd.CommandType = CommandType.Text;

                    oconexion.Open();
                    
                    using(SqlDataReader dr = ocmd.ExecuteReader())
                    {
                        while (dr.Read()){
                            lista.Add(new Usuario()
                            {
                                IdUsuario = Convert.ToInt32(dr["IdUsuario"].ToString()),
                                Codigo_Usuario = dr["Codigo_Usuario"].ToString(),
                                Nombre_Completo_Usuario = dr["Nombre_Completo_Usuario"].ToString(),
                                Correo_Usuario = dr["Correo_Usuario"].ToString(),
                                Clave_Usuario = dr["Clave_Usuario"].ToString(),
                                Estado_Usuario = Convert.ToBoolean(dr["Estado_Usuario"].ToString())
                            });
                         
                        }
                    }
                }catch (Exception ex)
                {
                    lista = new List<Usuario>();
                    lista = new List<Usuario>();
                }
            }
            return lista;
        }   
    }
}
