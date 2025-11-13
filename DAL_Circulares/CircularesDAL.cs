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

namespace DAL_Circulares
{
    public class CircularesDAL
    {

        //Procedimientos almacenados
        private const string _spObtenerCirculares = "spObtenerCirculares";
        private const string _spObtenerArchivoCircular = "spObtenerArchivoCircular";


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
