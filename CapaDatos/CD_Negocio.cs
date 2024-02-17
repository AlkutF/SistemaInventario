using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Negocio
    {
        public Negocio ObtenerDatos()
        {
            Negocio objnegocio = new Negocio();
            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
                {
                    conexion.Open();
                    string query = "Select IdNegocio ,Nombre ,RUC ,Direccion from Negocio where IdNegocio=1";
                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.CommandType = CommandType.Text;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            objnegocio = new Negocio()
                            {
                                IdNegocio = int.Parse(dr["IdNegocio"].ToString()),
                                Nombre = dr["Nombre"].ToString(),
                                RUC = dr["RUC"].ToString(),
                                Direccion = dr["Direccion"].ToString()
                            };
                        }
                    }
                }
            }
            catch
            {
                objnegocio = new Negocio();
            }

            return objnegocio;
        }



        public bool AlmacenarDatos(Negocio objeto, out string Mensaje)
        {
            Mensaje = string.Empty;
            bool respuesta = true;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
                {
                    conexion.Open();

                    StringBuilder query = new StringBuilder();
                    query.AppendLine("update Negocio set Nombre = @Nombre,");
                    query.AppendLine("RUC=@RUC,");
                    query.AppendLine("Direccion=@Direccion");
                    query.AppendLine("Where IdNegocio=1;");
                    SqlCommand cmd = new SqlCommand(query.ToString(), conexion);
                    cmd.Parameters.AddWithValue("@Nombre", objeto.Nombre);
                    cmd.Parameters.AddWithValue("@RUC", objeto.RUC);
                    cmd.Parameters.AddWithValue("@Direccion", objeto.Direccion);
                    cmd.CommandType = CommandType.Text;

                    if (cmd.ExecuteNonQuery() < 1)
                    {
                        Mensaje = "No se pudo guardar los datos";
                        respuesta = false;
                    }

                }
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
                respuesta = false;
            }

            return respuesta;
        }

        public byte[] ObtenerLogo(out bool obtenido ,int idnegocio)
        {
            obtenido = true;
            byte[] LogoBytes = new byte[0];
            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
                {
                    conexion.Open();
                    string query = "Select Logo from Negocio Where IdNegocio=@IdNegocio";
                    SqlCommand cmd = new SqlCommand(query.ToString(), conexion);
                    cmd.Parameters.AddWithValue("@IdNegocio", idnegocio);
                    cmd.CommandType = CommandType.Text;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            LogoBytes = (byte[])dr["Logo"];
                        }
                    }
                }
            }
            catch
            {
                obtenido = false;
                LogoBytes = new byte[0];
            }
            return LogoBytes;
        }

        public bool ActualizarLogo(byte[] image, out string Mensaje , int idnegocio)
        {
            Mensaje = string.Empty;
            bool respuesta = true;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
                {
                    conexion.Open();

                    StringBuilder query = new StringBuilder();
                    query.AppendLine("update Negocio set Logo = @imagen");
                    query.AppendLine("Where IdNegocio = @IdNegocio;");
                    SqlCommand cmd = new SqlCommand(query.ToString(), conexion);
                    cmd.Parameters.AddWithValue("@imagen", image);
                    cmd.Parameters.AddWithValue("@IdNegocio", idnegocio);
                    cmd.CommandType = CommandType.Text;

                    if (cmd.ExecuteNonQuery() < 1)
                    {
                        Mensaje = "No se pudo recuperar el logo";
                        respuesta = false;
                    }

                }
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
                respuesta = false;
            }

            return respuesta;
        }
    }
}
