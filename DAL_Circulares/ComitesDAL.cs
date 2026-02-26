using Entities_Circulares.Comites;
using Entities_Circulares.FileCirculares;
using Entities_Circulares.GestionUsuarios;
using Entities_Circulares.Reply;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_Circulares
{
    public class ComitesDAL
    {

        //Procedimientos almacenados

        private const string _spObtenerComites = "spObtenerComites";
        private const string _spCrearComite = "spObtenerComites";
        private const string _spEliminarComitee = "spEliminarComite";

        




        public Reply<List<Comites>> ObtenerComites(string Conexion)
        {

            Reply<List<Comites>> reply = new Reply<List<Comites>>();
            List<Comites> ListaComite = new List<Comites>();

            try
            {

                using (SqlConnection connection = new SqlConnection(Conexion))
                {

                    connection.Open();

                    using (SqlCommand command = new SqlCommand(_spObtenerComites, connection))
                    {

                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@pParametro", SqlDbType.Int) { Value = 1 });


                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            while (reader.Read())
                            {


                                Comites ObjComite = new Comites
                                {
                                    IdComite = (int)reader["IdComite"],
                                    NombreComite = (string)reader["Nombre"]
                              

                                };

                                ListaComite.Add(ObjComite);


                            }

                            reply.Ok = true;
                            reply.Result = ListaComite;


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


        public Reply<bool> CrearComite(Comites ObjComite, string Conexion)
        {
            Reply<bool> reply = new Reply<bool>();

            try
            {

                using (SqlConnection connection = new SqlConnection(Conexion))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(_spCrearComite, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@pNombreComite", SqlDbType.NVarChar, 50) { Value = ObjComite.NombreComite });

                        int RowAfectadas = command.ExecuteNonQuery();

                        if (RowAfectadas > 0)
                        {
                            reply.Ok = true;
                            reply.Message = "Comité guardado correctamente";
                        }
                        else
                        {
                            reply.Ok = false;
                            reply.Message = "Ha ocurrido un error al insertar el comité";
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                reply.Ok = false;
                reply.Message = string.Format("Error en capa Data.Dapper, en la clase ComitesDAL: ", ex.Message);

            }

            return reply;
        }


        public Reply<bool> EliminarComite(Comites ObjComite, string Conexion)
        {
            Reply<bool> reply = new Reply<bool>();

            try
            {

                using (SqlConnection connection = new SqlConnection(Conexion))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(_spEliminarComitee, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@pParametro", SqlDbType.Int) { Value = ObjComite.IdComite });

                        int RowAfectadas = command.ExecuteNonQuery();

                        if (RowAfectadas > 0)
                        {
                            reply.Ok = true;
                            reply.Message = "Comité eliminado correctamente";
                        }
                        else
                        {
                            reply.Ok = false;
                            reply.Message = "Ha ocurrido un error al eliminar el comité";
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                reply.Ok = false;
                reply.Message = string.Format("Error en capa Data.Dapper, en la clase ComitesDAL: ", ex.Message);

            }

            return reply;
        }

    }
}
