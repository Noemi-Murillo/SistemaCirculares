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

        private const string _spObtenerUsuariosGestionComite = "ObtenerUsuariosGestionComite";


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
