using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;

namespace CapaDatos
{
    public class CD_Venta
    {
        public int ObtenerCorrelativo()
        {
            int idCorrelativo = 0;
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("Select count (*) +1 from VENTA");
                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    idCorrelativo = Convert.ToInt32(cmd.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    idCorrelativo = 0;
                }
            }
            return idCorrelativo;
        }

        public bool restarStock(int idProducto, int cantidad)
        {
            bool respuesta = true;
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("Update PRODUCTO set stock = stock - @cantidad where IdProducto = @IdProducto");
                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.Parameters.AddWithValue("@cantidad", cantidad);
                    cmd.Parameters.AddWithValue("@IdProducto", idProducto);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    respuesta = cmd.ExecuteNonQuery() > 0 ? true : false;

                }
                catch (Exception ex)
                {
                    respuesta = false;
                }
            }

            return respuesta;
        }

        public bool SumarStock(int idProducto, int cantidad)
        {
            bool respuesta = true;
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("Update PRODUCTO set stock = stock + @cantidad where IdProducto = @IdProducto");
                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.Parameters.AddWithValue("@cantidad", cantidad);
                    cmd.Parameters.AddWithValue("@IdProducto", idProducto);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    respuesta = cmd.ExecuteNonQuery() > 0 ? true : false;

                }
                catch (Exception ex)
                {
                    respuesta = false;
                }
            }

            return respuesta;
        }

        public bool Registrar(Venta obj, DataTable DetalleVenta, out string Mensaje)
        {
            bool respuesta = false;
            Mensaje = String.Empty;
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("USP_Registrar_Venta", oconexion);
                    cmd.Parameters.AddWithValue("IdUsuario", obj.oUsuario.IdUsuario);
                    cmd.Parameters.AddWithValue("TipoDocumento", obj.TipoDocumento);
                    cmd.Parameters.AddWithValue("NumeroDocumento", obj.NumeroDocumento);
                    cmd.Parameters.AddWithValue("DocumentoCliente", obj.DocumentoCliente);
                    cmd.Parameters.AddWithValue("NombreCliente", obj.NombreCliente);
                    cmd.Parameters.AddWithValue("MontoPago", obj.MontoPago);
                    cmd.Parameters.AddWithValue("MontoCambio", obj.MontoCambio);
                    cmd.Parameters.AddWithValue("MontoTotal", obj.MontoTotal);
                    cmd.Parameters.AddWithValue("DetalleVenta", DetalleVenta);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
                catch (Exception ex)
                {
                    respuesta = false;
                    Mensaje = ex.Message;
                }
            }
            return respuesta;
        }

        public Venta ObtenerVenta(string numero)
        {
            Venta objVenta = new Venta();
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    oconexion.Open();
                    StringBuilder query = new StringBuilder();

                    query.AppendLine("Select v.IdVenta , u.NombreCompleto ,");
                    query.AppendLine("v.DocumentoCliente , v.NombreCliente , v.TipoDocumento,");
                    query.AppendLine("v.NumeroDocumento , v.MontoPago,v.MontoCambio , v.MontoTotal , CONVERT(char(10),v.FechaRegistro,103)[FechaRegistro]");
                    query.AppendLine("from Venta v");
                    query.AppendLine("Inner Join Usuario u on v.IdUsuario =u.IdUsuario");
                    query.AppendLine("where v.NumeroDocumento= @numero");
                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.Parameters.AddWithValue("@numero", numero);
                    cmd.CommandType = System.Data.CommandType.Text;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            objVenta = new Venta()
                            {
                                idVenta = int.Parse(dr["IdVenta"].ToString()),
                                oUsuario = new Usuario() { NombreCompleto = dr["NombreCompleto"].ToString() },
                                DocumentoCliente = dr["DocumentoCliente"].ToString(),
                                NombreCliente = dr["NombreCliente"].ToString(),
                                TipoDocumento = dr["TipoDocumento"].ToString(),
                                NumeroDocumento = dr["NumeroDocumento"].ToString(),
                                MontoPago = Convert.ToDecimal(dr["MontoPago"].ToString()),
                                MontoCambio = Convert.ToDecimal(dr["MontoCambio"].ToString()),
                                MontoTotal = Convert.ToDecimal(dr["MontoTotal"].ToString()),
                                FechaRegistro = dr["FechaRegistro"].ToString()
                            };
                        }
                    }


                }
                catch (Exception ex)
                {
                    objVenta = new Venta();
                }
                return objVenta;
            }
        }
        public List<Detalle_Venta>ObtenerDetalleVenta(int IdVenta)
        {
            List<Detalle_Venta> oLista = new List<Detalle_Venta>();
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
                {
                    oconexion.Open();
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("Select p.Nombre , dv.PrecioVenta,dv.Cantidad,dv.Subtotal from DETALLE_VENTA dv ");
                    query.AppendLine("inner join PRODUCTO p on dv.IdProducto = p.IdProducto");
                    query.AppendLine("where dv.IdVenta = @Idventa");
                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.Parameters.AddWithValue("@Idventa", IdVenta);
                    cmd.CommandType = CommandType.Text;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            oLista.Add(new Detalle_Venta()
                            {
                                oProducto = new Producto() { Nombre = dr["Nombre"].ToString() },
                                PrecioVenta = Convert.ToDecimal(dr["PrecioVenta"].ToString()),
                                Cantidad = Convert.ToInt32(dr["Cantidad"].ToString()),
                                //Si hay un error va a ser aqui puedo apostarlo
                                MontoTotal = Convert.ToDecimal(dr["Subtotal"].ToString()),
                            });

                        }
                    }
                }
            }
            catch
            {
                oLista = new List<Detalle_Venta>();
            }
            return oLista;
        }

    }
    }

