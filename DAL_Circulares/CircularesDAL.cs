using Entities_Circulares.GestionUsuarios;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities_Circulares.Reply;
using Entities_Circulares.FileCirculares;
using Entities_Circulares.Eventos;

namespace DAL_Circulares
{
    public class CircularesDAL
    {

        //Procedimientos almacenados
        private const string _spObtenerCirculares = "spObtenerCirculares";
        private const string _spObtenerArchivoCircular = "spObtenerArchivoCircular";
        private const string _spObtenerCorreosTodosUsuarios = "spObtenerAllCorreos";
        private const string _spObtenerEventos = "spObtenerEventos";
        private const string spObtenerCircularesParametrizado = "spObtenerCircularesParametrizado";
        private const string spEliminarCirculares = "spEliminarCirculares";




        public Reply<List<Circulares>> ObtenerCirculares(string Conexion)
        {

            Reply<List<Circulares>> reply = new Reply<List<Circulares>>();
            List<Circulares> ListaCirculares = new List<Circulares>();

            try
            {

                using (SqlConnection connection = new SqlConnection(Conexion))
                {

                    connection.Open();

                    using (SqlCommand command = new SqlCommand(_spObtenerCirculares, connection))
                    {

                        command.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            while (reader.Read())
                            {


                                Circulares ObjCircular = new Circulares
                                {
                                    IdCircular = (int)reader["IdCircular"],
                                    FechaCircular = (DateTime)reader["Fecha"],
                                    NombreCircular = (string)reader["Titulo"],
                                    ArchivoBytes = (byte[])reader["Archivo"],
                                    NombreComite = (string)reader["NombreComite"],

                                };

                                ListaCirculares.Add(ObjCircular);


                            }

                            reply.Ok = true;
                            reply.Result = ListaCirculares;


                        }
                    }
                }

            }
            catch (Exception ex)
            {
                reply.Ok = false;
                reply.Message = ex.Message;
            }

            return reply;



        }

        public Reply<List<Circulares>> ObtenerCircularesPorCantidad(string Conexion, int Cantidad)
        {

            Reply<List<Circulares>> reply = new Reply<List<Circulares>>();
            List<Circulares> ListaCirculares = new List<Circulares>();

            try
            {

                using (SqlConnection connection = new SqlConnection(Conexion))
                {

                    connection.Open();

                    using (SqlCommand command = new SqlCommand(spObtenerCircularesParametrizado, connection))
                    {

                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@pOpcion", SqlDbType.Int) { Value = Cantidad });

                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            while (reader.Read())
                            {


                                Circulares ObjCircular = new Circulares
                                {
                                    IdCircular = (int)reader["IdCircular"],
                                    FechaCircular = (DateTime)reader["Fecha"],
                                    NombreCircular = (string)reader["Titulo"],
                                    ArchivoBytes = (byte[])reader["Archivo"],
                                    NombreComite = (string)reader["NombreComite"],

                                };

                                ListaCirculares.Add(ObjCircular);


                            }

                            reply.Ok = true;
                            reply.Result = ListaCirculares;


                        }
                    }
                }

            }
            catch (Exception ex)
            {
                reply.Ok = false;
                reply.Message = ex.Message;
            }

            return reply;



        }


        public Reply<List<Eventos>> ObtenerEventosCalendario(string Conexion)
        {

            Reply<List<Eventos>> reply = new Reply<List<Eventos>>();
            List<Eventos> ListaEventos = new List<Eventos>();

            try
            {

                using (SqlConnection connection = new SqlConnection(Conexion))
                {

                    connection.Open();

                    using (SqlCommand command = new SqlCommand(_spObtenerEventos, connection))
                    {

                        command.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            while (reader.Read())
                            {


                                Eventos ObjEvento = new Eventos
                                {
                                    IdCircular = (int)reader["IdCircular"],
                                    Fecha = (DateTime)reader["Fecha"],
                                    Titulo = (string)reader["Titulo"],
                                    NombreComite = (string)reader["NombreComite"],

                                };

                                ListaEventos.Add(ObjEvento);


                            }

                            reply.Ok = true;
                            reply.Result = ListaEventos;


                        }
                    }
                }

            }
            catch (Exception ex)
            {
                reply.Ok = false;
                reply.Message = ex.Message;
            }

            return reply;

        }



        public Reply<List<Usuario>> ObtenerCorreos(string Conexion)
        {

            Reply<List<Usuario>> reply = new Reply<List<Usuario>>();
            List<Usuario> ListaCorreo = new List<Usuario>();

            try
            {

                using (SqlConnection connection = new SqlConnection(Conexion))
                {

                    connection.Open();

                    using (SqlCommand command = new SqlCommand(_spObtenerCorreosTodosUsuarios, connection))
                    {

                        command.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            while (reader.Read())
                            {


                                Usuario ObjUsuario = new Usuario
                                {
                                    Correo = (string)reader["Correo"],


                                };

                                ListaCorreo.Add(ObjUsuario);


                            }

                            reply.Ok = true;
                            reply.Result = ListaCorreo;


                        }
                    }
                }

            }
            catch (Exception ex)
            {
                reply.Ok = false;
                reply.Message = ex.Message;
            }

            return reply;

        }

        public Reply<Circulares> ObtenerCircularPorId(int IdCircular, string Conexion)
        {

            Reply<Circulares> reply = new Reply<Circulares>();

            try
            {

                using (SqlConnection connection = new SqlConnection(Conexion))
                {

                    connection.Open();

                    using (SqlCommand command = new SqlCommand(_spObtenerArchivoCircular, connection))
                    {

                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@pIdCircular", SqlDbType.Int) { Value = IdCircular });

                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            if (reader.Read())
                            {


                                Circulares ObjCircular = new Circulares
                                {
                                    IdCircular = (int)reader["IdCircular"],
                                    FechaCircular = (DateTime)reader["Fecha"],
                                    NombreCircular = (string)reader["Titulo"],
                                    ArchivoBytes = (byte[])reader["Archivo"],
                                    NombreComite = (string)reader["NombreComite"],

                                };


                                reply.Ok = true;
                                reply.Result = ObjCircular;

                            }



                        }
                    }
                }

            }
            catch (Exception ex)
            {
                reply.Ok = false;
                reply.Message = ex.Message;
            }

            return reply;

        }


    }
}
