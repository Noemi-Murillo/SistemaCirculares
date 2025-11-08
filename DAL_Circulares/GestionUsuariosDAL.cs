using Entities_Circulares.Comites;
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
    public class GestionUsuariosDAL
    {
        //Procedimientos almacenados
        private const string _spObtenerUsuariosGestionComite = "ObtenerUsuariosGestionComite";
        private const string _sp_CrearNuevoCodigo = "sp_CrearNuevoCodigo";
        private const string _spObtenerComites = "spObtenerComites";




        public Reply<List<Usuario>> ObtenerUsuariosGestionComite(string Conexion)
        {

            Reply<List<Usuario>> reply = new Reply<List<Usuario>>();
            List<Usuario> ListaUsuarios = new List<Usuario>();

            try
            {

                using (SqlConnection connection = new SqlConnection(Conexion))
                {

                    connection.Open();

                    using (SqlCommand command = new SqlCommand(_spObtenerUsuariosGestionComite, connection))
                    {

                        command.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            while (reader.Read())
                            {


                                Usuario ObjUsuarios = new Usuario
                                {
                                    Id = (int)reader["Id"],
                                    Nombre = (string)reader["Nombre"],
                                    IdComite = (int)reader["IdComite"],
                                    NombreComite = (string)reader["NombreComite"],
                                    EsCordinador = (bool)reader["EsCoordinador"],
                                    Activo = (bool)reader["Activo"]

                                };

                                ListaUsuarios.Add(ObjUsuarios);


                            }

                            reply.Ok = true;
                            reply.Result = ListaUsuarios;


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


        public Reply<string> GenerarCodigoRegistro(Usuario ObjUsuario, string Conexion)
        {
            Reply<string> reply = new Reply<string>();

            try
            {

                using (SqlConnection connection = new SqlConnection(Conexion))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(_sp_CrearNuevoCodigo, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@pCodigo", SqlDbType.NVarChar, 10) { Value = ObjUsuario.CodigoRegistro });
                        command.Parameters.Add(new SqlParameter("@pFechaExpiracion", SqlDbType.DateTime) { Value = ObjUsuario.FechaExpiracion });

                        int RowAfectadas = command.ExecuteNonQuery();

                        if (RowAfectadas > 0)
                        {
                            reply.Ok = true;
                            reply.Message = "Código generado correctamente";
                        }
                        else
                        {
                            reply.Ok = false;
                            reply.Message = "Ha ocurrido un error al insertar los datos";
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                reply.Ok = false;
                reply.Message = string.Format("Error en capa Data.Dapper, en la clase GestionUsuariosDAL: ", ex.Message);

            }

            return reply;
        }


        public Reply<List<Comites>> ObtenerComites(int Parametro, string Conexion)
        {

            Reply<List<Comites>> reply = new Reply<List<Comites>>();
            List<Comites> ListaComites = new List<Comites>();

            try
            {

                using (SqlConnection connection = new SqlConnection(Conexion))
                {

                    connection.Open();

                    using (SqlCommand command = new SqlCommand(_spObtenerComites, connection))
                    {

                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@pParametro", SqlDbType.Int) { Value = Parametro });

                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            while (reader.Read())
                            {


                                Comites ObjComite = new Comites
                                {
                                    IdComite = (int)reader["IdComite"],
                                    NombreComite = (string)reader["Nombre"],
                                    Activo = (bool)reader["Activo"]


                                };

                                ListaComites.Add(ObjComite);


                            }

                            reply.Ok = true;
                            reply.Result = ListaComites;


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


        public Reply<List<Usuario>> GuardarUsuarios(List<Usuario> ObjUsuario, string Conexion)
        {

            Reply<List<Usuario>> reply = new Reply<List<Usuario>>();
            List<Usuario> ListaUsuarios = new List<Usuario>();

            try
            {

                using (SqlConnection connection = new SqlConnection(Conexion))
                {

                    connection.Open();

                    using (SqlCommand command = new SqlCommand(_spObtenerUsuariosGestionComite, connection))
                    {

                        command.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            while (reader.Read())
                            {


                                Usuario ObjUsuarios = new Usuario
                                {
                                    Id = (int)reader["Id"],
                                    Nombre = (string)reader["Nombre"],
                                    IdComite = (int)reader["IdComite"],
                                    NombreComite = (string)reader["NombreComite"],
                                    EsCordinador = (bool)reader["EsCoordinador"],
                                    Activo = (bool)reader["Activo"]

                                };

                                ListaUsuarios.Add(ObjUsuarios);


                            }

                            reply.Ok = true;
                            reply.Result = ListaUsuarios;


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
