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
    public class PublicarCircularDAL
    {

        //Procedimientos almacenados
        private const string _sp_GuardarDocumento = "paGuardarDocumento";

        public Reply<bool> PublicarCircular(Circulares ObjCircular, string Conexion)
        {
            Reply<bool> reply = new Reply<bool>();

            try
            {

                using (SqlConnection connection = new SqlConnection(Conexion))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(_sp_GuardarDocumento, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@Titulo", SqlDbType.NVarChar, 50) { Value = ObjCircular.NombreCircular });
                        command.Parameters.Add(new SqlParameter("@Categoria", SqlDbType.NVarChar, 50) { Value = "Circular" });
                        command.Parameters.Add(new SqlParameter("@Archivo", SqlDbType.VarBinary) { Value = ObjCircular.Archivo.ArchivoBytes });
                        command.Parameters.Add(new SqlParameter("@Fecha", SqlDbType.DateTime) { Value = ObjCircular.FechaEvento });
                        command.Parameters.Add(new SqlParameter("@IdUsuario", SqlDbType.Int) { Value = ObjCircular.IdUsuario });
                        command.Parameters.Add(new SqlParameter("@IdComite", SqlDbType.Int) { Value = ObjCircular.IdComite });



                        int RowAfectadas = command.ExecuteNonQuery();

                        if (RowAfectadas > 0)
                        {
                            reply.Ok = true;
                            reply.Message = "Circular guardada correctamente";
                        }
                        else
                        {
                            reply.Ok = false;
                            reply.Message = "Ha ocurrido un error al insertar la circular";
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
    }
}
