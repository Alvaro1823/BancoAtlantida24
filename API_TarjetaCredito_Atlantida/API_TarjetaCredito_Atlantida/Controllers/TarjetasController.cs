using API_TarjetaCredito_Atlantida.models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace API_TarjetaCredito_Atlantida.Controllers
{
    [Route("api/Tarjetas")]
    [ApiController]
    public class TarjetasController : ControllerBase
    {
        public readonly string con;

        public TarjetasController(IConfiguration configuracion)
        {
            con = configuracion.GetConnectionString("conexion");
        }
        [HttpGet]
        [Route("Clientes")]
        public dynamic GetClientes()
        {
            List<Cliente> clientes = new();
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("ObtenerClientes", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Cliente cli = new Cliente();
                            cli.IDCliente = Convert.ToInt32(reader["IDcliente"]);
                            cli.Nombre = reader["Nombre"].ToString();
                            cli.Apellido = reader["Apellido"].ToString();
                            cli.Dui = reader["Dui"].ToString();
                            cli.FechaNacimiento = Convert.ToDateTime(reader["fechanNacimiento"]);

                            clientes.Add(cli);
                        }
                    }
                }
            }
            return clientes;
        }

        [HttpGet("cliente/{idCliente:int}")]
        public dynamic GetClientesPorId(int idCliente)
        {
            List<Cliente> clientes = new();
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("ObtenerClientePorID", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdCliente", idCliente);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Cliente cli = new Cliente();
                            cli.IDCliente = Convert.ToInt32(reader["IDcliente"]);
                            cli.Nombre = reader["Nombre"].ToString();
                            cli.Apellido = reader["Apellido"].ToString();
                            cli.Dui = reader["Dui"].ToString();
                            cli.FechaNacimiento = Convert.ToDateTime(reader["fechanNacimiento"]);

                            clientes.Add(cli);
                        }
                    }
                }
            }
            return clientes;
        }

        [HttpGet("listar/{idCliente:int}")]
        //[Route("TarCliente")]
        public dynamic GetTarjetasByCliente(int idCliente)
        {
            List<Tarjetas> tarjetas = new();
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("ListarTarjetasPorCliente", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdCliente", idCliente);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Tarjetas tar = new Tarjetas();
                            tar.NumeroTarjeta = reader["NumeroTarjeta"].ToString();
                            tar.IDCliente = Convert.ToInt32(reader["IDcliente"]);
                            tar.NombreTarjeta = reader["NombreTarjeta"].ToString();
                            tar.LimiteCredito = Convert.ToDecimal(reader["LimiteCredito"]);
                            tar.SaldoActual = Convert.ToDecimal(reader["SaldoActual"]);
                            tar.SaldoDisponible = Convert.ToDecimal(reader["SaldoDisponible"]);
                            tar.PIC = Convert.ToInt32(reader["PIC"]); ;
                            tar.PCSM = Convert.ToInt32(reader["PCSM"]); ;


                            tarjetas.Add(tar);
                        }
                    }
                }
            }
            return tarjetas;
        }

        [HttpGet("tarjeta/{numTarjeta}")]
        public dynamic GetTarjeta(String numTarjeta)
        {

            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("TarjetaCliente", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@numTarjeta", numTarjeta);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Tarjetas tar = new Tarjetas();
                            tar.NumeroTarjeta = reader["NumeroTarjeta"].ToString();
                            tar.IDCliente = Convert.ToInt32(reader["IDcliente"]);
                            tar.NombreTarjeta = reader["NombreTarjeta"].ToString();
                            tar.LimiteCredito = Convert.ToDecimal(reader["LimiteCredito"]);
                            tar.SaldoActual = Convert.ToDecimal(reader["SaldoActual"]);
                            tar.SaldoDisponible = Convert.ToDecimal(reader["SaldoDisponible"]);
                            tar.PIC = Convert.ToInt32(reader["PIC"]); ;
                            tar.PCSM = Convert.ToInt32(reader["PCSM"]); ;


                            return tar;
                        }
                    }
                }
            }
            return null;
        }

        [HttpPost]
        [Route("ingresar")]
        public void Post([FromBody] Tarjetas t)
        {
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("InsertarTarjetaCredito", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NumeroTarjeta", t.NumeroTarjeta);
                    cmd.Parameters.AddWithValue("@NombreTarjeta", t.NombreTarjeta);
                    cmd.Parameters.AddWithValue("@IDCliente", t.IDCliente);
                    cmd.Parameters.AddWithValue("@LimiteCredito", t.LimiteCredito);
                    cmd.Parameters.AddWithValue("@SaldoActual", t.SaldoActual);
                    cmd.Parameters.AddWithValue("@SaldoDisponible", t.SaldoDisponible);
                    cmd.Parameters.AddWithValue("@PIC", t.PIC);
                    cmd.Parameters.AddWithValue("@PCSM", t.PCSM);
                    cmd.ExecuteNonQuery();
                }
            }
        }



        [HttpPut("Tarjetas")]

        public void Put([FromBody] int id, String NumeroTarjeta, Decimal Monto)
        {
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("ActualizarSaldo", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NumeroTarjeta", NumeroTarjeta);
                    cmd.Parameters.AddWithValue("@MontoTransaccion", Monto);                                  
                    cmd.ExecuteNonQuery();
                }
            }
        }

        [HttpPost("Transacciones/ingresos")]
        public void Post([FromBody] TransaccionesTarjetas p)
        {
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("InsertarTransaccion", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NumeroAutorizacion", p.NumeroAutorizacion);
                    cmd.Parameters.AddWithValue("@NumeroTarjeta", p.NumeroTarjeta);
                    cmd.Parameters.AddWithValue("@TipoTransaccion", p.TipoTransaccion);
                    cmd.Parameters.AddWithValue("@Monto", p.Monto);
                    cmd.Parameters.AddWithValue("@FechaTransaccion", p.FechaTransaccion);
                    cmd.Parameters.AddWithValue("@Descripcion", p.Descripcion);
                    cmd.ExecuteNonQuery();
                }
            }
        }



        [HttpPost("listarTransacciones")]
        public dynamic GetTransacciones([FromBody] String id, int mes, String tipoTransaccion, String numTarjeta)
        {
            List<TransaccionesTarjetas> transacciones = new();
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("ListarTransaccionesPorTarjeta", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NumeroTarjeta", numTarjeta);
                    cmd.Parameters.AddWithValue("@mes", mes);
                    cmd.Parameters.AddWithValue("@tipoTransccion", tipoTransaccion);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TransaccionesTarjetas transs = new TransaccionesTarjetas();

                            transs.NumeroAutorizacion = Convert.ToInt32(reader["NumeroAutorizacion"]);
                            transs.NumeroTarjeta = reader["NumeroTarjeta"].ToString();
                            transs.TipoTransaccion = reader["TipoTransaccion"].ToString();
                            transs.Monto = Convert.ToDecimal(reader["Monto"]);
                            transs.FechaTransaccion = Convert.ToDateTime(reader["FechaTransaccion"]);
                            transs.Descripcion = reader["Descripcion"].ToString();
                            transacciones.Add(transs);
                        }
                    }
                }
            }
            return transacciones;
        }

        [HttpPost("montoTrasaccion")]
        public dynamic GetMotoTotalTransaccion([FromBody] String id, int mes, String tipoTransaccion, String numTarjeta) 
        {

            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("MontoTotalTransacciones", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NumeroTarjeta", numTarjeta);
                    cmd.Parameters.AddWithValue("@mes", mes);
                    cmd.Parameters.AddWithValue("tipoTransccion", tipoTransaccion);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            return Convert.ToDecimal(reader["MontoTotalTransacciones"]);
                        }
                    }
                }
            }
            return null;
        }



    }
}
