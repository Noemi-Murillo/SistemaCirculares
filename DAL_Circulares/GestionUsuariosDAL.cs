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
        private const string _spCrearEditarUsuarios = "spCrearEditarUsuarios";


        


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
            if (ObjUsuario == null || ObjUsuario.Count == 0)
            {
                reply.Message = "No hay usuarios para guardar.";
                return reply;
            }

            try
            {
                using (var connection = new SqlConnection(Conexion))
                {
                    connection.Open();
                    using (var tx = connection.BeginTransaction())
                    using (var command = new SqlCommand(_spCrearEditarUsuarios, connection, tx))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        // command.CommandTimeout = 60; // opcional

                        // Declaración de parámetros UNA sola vez (tipos y tamaños)
                        var pIdUsuario = command.Parameters.Add("@pIdUsuario", SqlDbType.Int);

                        var pNombre = command.Parameters.Add("@pNombre", SqlDbType.NVarChar, 50);
                        var pApellido1 = command.Parameters.Add("@pApellido1", SqlDbType.NVarChar, 50);
                        var pApellido2 = command.Parameters.Add("@pApellido2", SqlDbType.NVarChar, 50);
                        var pCorreo = command.Parameters.Add("@pCorreo", SqlDbType.NVarChar, 50);
                        var pContrasena = command.Parameters.Add("@pContrasena", SqlDbType.NVarChar, 250);

                        var pIdRol = command.Parameters.Add("@pIdRol", SqlDbType.Int);
                        var pActivo = command.Parameters.Add("@pActivo", SqlDbType.Bit);

                        // OJO: coincide exactamente con tu SP: @pEscordinador (con "r" antes de "dinador")
                        var pEscordinador = command.Parameters.Add("@pEscordinador", SqlDbType.Bit);

                        var pIdComite = command.Parameters.Add("@pIdComite", SqlDbType.Int);
                        foreach (var u in ObjUsuario)
                        {
                            // Int
                            pIdUsuario.Value = u.Id;

                            // NVARCHARs (usa DBNull.Value si null/ vacío)
                            pNombre.Value = string.IsNullOrWhiteSpace(u.Nombre) ? (object)DBNull.Value : u.Nombre.Trim();
                            pApellido1.Value = string.IsNullOrWhiteSpace(u.Apellido1) ? (object)DBNull.Value : u.Apellido1.Trim();
                            pApellido2.Value = string.IsNullOrWhiteSpace(u.Apellido2) ? (object)DBNull.Value : u.Apellido2.Trim();
                            pCorreo.Value = string.IsNullOrWhiteSpace(u.Correo) ? (object)DBNull.Value : u.Correo.Trim();
                            pContrasena.Value = string.IsNullOrWhiteSpace(u.Contrasena) ? (object)DBNull.Value : u.Contrasena; // si no la actualizas, deja null

                            // Ints (nullable)
                            pIdRol.Value = u.IdRol.HasValue ? (object)u.IdRol.Value : DBNull.Value;
                            pIdComite.Value = u.IdComite.HasValue ? (object)u.IdComite.Value : DBNull.Value;

                            // Bits
                            pActivo.Value = u.Activo;
                            pEscordinador.Value = u.EsCordinador;

                            // Ejecuta el SP para este usuario
                            command.ExecuteNonQuery();

                            // Opcional: acumular para devolver lo aplicado
                        }

                        tx.Commit();
                        reply.Ok = true;
                        reply.Message = "Los usuarios se han actualizado de manera correcta";
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
