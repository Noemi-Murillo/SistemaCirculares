using Microsoft.Data.SqlClient;
using System.Data;
using Entities_Circulares.Reply;
using Microsoft.IdentityModel.Protocols;
using Entities_Circulares.UserRegistration;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DAL_Circulares
{
    public class InicioDAL
    {


        #region Procedimientos Almacenados
        //Procedimientos almacenados
        private const string spIniciarSesion = "sp_IniciarSesion";

        #endregion

        #region Métodos Iniciar Sesión


        public Reply<UserRegistration> LogIn(UserRegistration ObjUsuario, string Conexion)
        {

            Reply<UserRegistration> reply = new Reply<UserRegistration>();

            try
            {
                using (SqlConnection connection = new SqlConnection(Conexion))
                {

                    connection.Open();

                    using (SqlCommand command = new SqlCommand(spIniciarSesion, connection))
                    {

                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@pCorreo", SqlDbType.NVarChar, 50) { Value = ObjUsuario.Email });
                        //command.Parameters.Add(new SqlParameter("@pContrasena", SqlDbType.NVarChar, 50) { Value = ObjUsuario.Password });

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                reply.Ok = (bool)reader["Exito"];

                                if (reply.Ok)
                                {

                                    UserRegistration ObjUsuarioObtenido = new UserRegistration
                                    {
                                        Nombre = (string)reader["NombreCompleto"],
                                        Password = (string)reader["HashContrasena"]
                                    };

                                    reply.Result = ObjUsuarioObtenido;


                                }
                                else
                                {
                                    UserRegistration ObjUsuarioObtenido = new UserRegistration
                                    {
                                        Password = (string)reader["HashContrasena"],
                                        
                                    };
                                    reply.Ok = false;
                                    reply.Message = "El usuario o la contraseña no son correctos";
                                    reply.Result = ObjUsuarioObtenido;


                                }

                            }
                            else
                            {

                                reply.Ok = false;
                                reply.Message = "El usuario o la contraseña no son correctos";
                            }


                        }
                    }
                }

            }
            catch (Exception ex)
            {
                reply.Ok = false;
                reply.Message = $"Ha ocurrido un error en la función de LogIn en DAL {ex.Message}";
            }

            return reply;

        }












        #endregion

        //public Reply<eGastosDependenciaCanastasBasicas> GuardarGastoDependenciaCanastasBasicas(eGastosDependenciaCanastasBasicas gastosDependenciaCanastasBasicas)
        //{
        //    Reply<eGastosDependenciaCanastasBasicas> reply = new Reply<eGastosDependenciaCanastasBasicas>();

        //    try
        //    {
        //        string StringConn = ConfigurationManager.ConnectionStrings[strConnection].ConnectionString;

        //        using (SqlConnection connection = new SqlConnection(StringConn))
        //        {
        //            connection.Open();

        //            using (SqlCommand command = new SqlCommand(sp_paGuardarGastoDependenciaCanastasBasicas, connection))
        //            {
        //                command.CommandType = CommandType.StoredProcedure;
        //                command.Parameters.Add(new SqlParameter("@pIdUsuario", SqlDbType.Int) { Value = gastosDependenciaCanastasBasicas.idUsuario });
        //                command.Parameters.Add(new SqlParameter("@pDescripcionGastoDependencia", SqlDbType.NVarChar, 200) { Value = gastosDependenciaCanastasBasicas.DescripcionGasto });
        //                command.Parameters.Add(new SqlParameter("@pMontoGastoDependenciaSevera", SqlDbType.Decimal) { Value = gastosDependenciaCanastasBasicas.DependenciaSevera });
        //                command.Parameters.Add(new SqlParameter("@pMontoGastoDependenciaModerada", SqlDbType.Decimal) { Value = gastosDependenciaCanastasBasicas.DependenciaModerada });
        //                command.Parameters.Add(new SqlParameter("@pMontoGastoDependenciaLeve", SqlDbType.Decimal) { Value = gastosDependenciaCanastasBasicas.DependenciaLeve });
        //                command.Parameters.Add(new SqlParameter("@pEstado", SqlDbType.Bit) { Value = gastosDependenciaCanastasBasicas.Estado });
        //                command.Parameters.Add(new SqlParameter("@pPeriodicidad", SqlDbType.DateTime) { Value = gastosDependenciaCanastasBasicas.FechaCalculada });
        //                command.Parameters.Add(new SqlParameter("@pIdPeriodicidad", SqlDbType.Int) { Value = gastosDependenciaCanastasBasicas.IdPeriodicidad });



        //                int RowAfectadas = command.ExecuteNonQuery();

        //                if (RowAfectadas > 0)
        //                {
        //                    reply.blnIndicadorTransaccion = true;
        //                }
        //                else
        //                {
        //                    reply.blnIndicadorTransaccion = false;
        //                    reply.strMensajeRespuesta = "Ha ocurrido un error al insertar los datos";
        //                }

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        reply.blnIndicadorTransaccion = false;
        //        reply.strMensajeRespuesta = string.Format("Error en capa Data.Dapper, en la clase Canastas Básicas: ", ex.Message);

        //    }

        //    return reply;
        //}

        //public Reply<List<eGastosDependenciaCanastasBasicas>> ObtenerGastosDependenciasCanastasBasicas()
        //{

        //    Reply<List<eGastosDependenciaCanastasBasicas>> reply = new Reply<List<eGastosDependenciaCanastasBasicas>>();
        //    List<eGastosDependenciaCanastasBasicas> ListaGastosDependencia = new List<eGastosDependenciaCanastasBasicas>();

        //    try
        //    {
        //        string StringConn = ConfigurationManager.ConnectionStrings[strConnection].ConnectionString;

        //        using (SqlConnection connection = new SqlConnection(StringConn))
        //        {

        //            connection.Open();

        //            using (SqlCommand command = new SqlCommand(sp_paObtenerGastosDependenciaCanastasBasicas, connection))
        //            {

        //                command.CommandType = CommandType.StoredProcedure;

        //                using (SqlDataReader reader = command.ExecuteReader())
        //                {

        //                    while (reader.Read())
        //                    {


        //                        eGastosDependenciaCanastasBasicas ObjGastosDependencia = new eGastosDependenciaCanastasBasicas
        //                        {
        //                            IdGastoDependencia = (int)reader["IdGastoDependencia"],
        //                            DescripcionGasto = (string)reader["DescripcionGasto"],
        //                            DependenciaSevera = (decimal)reader["DependenciaSevera"],
        //                            DependenciaModerada = (decimal)reader["DependenciaModerada"],
        //                            DependenciaLeve = (decimal)reader["DependenciaLeve"],
        //                            Estado = (bool)reader["Estado"],
        //                            IdPeriodicidad = Convert.ToString(reader["IdPeriodicidad"]),
        //                            FechaPeriodicidad = Convert.ToDateTime(reader["Periodicidad"]),


        //                        };

        //                        ListaGastosDependencia.Add(ObjGastosDependencia);


        //                    }

        //                    reply.blnIndicadorTransaccion = true;
        //                    reply.ValorRetorno = ListaGastosDependencia;


        //                }
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        reply.blnIndicadorTransaccion = false;
        //        reply.strMensajeRespuesta = ex.Message;
        //    }

        //    return reply;

        //}

    }
}
