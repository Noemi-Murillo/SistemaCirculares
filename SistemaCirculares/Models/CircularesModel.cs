using Entities_Circulares.Eventos;
using Entities_Circulares.FileCirculares;
using Entities_Circulares.GestionUsuarios;
using Entities_Circulares.Reply;

namespace SistemaCirculares.Models
{
    public class CircularesModel : BaseAPI
    {

        public CircularesModel(IConfiguration config) : base(config)
        {

        }

        public Reply<bool> PublicarCircular(Circulares ObjCirculares)
        {

            Reply<bool> reply = new Reply<bool>();


            try
            {
                var UrlAPI = string.Format("{0}/Circular/PublicarCircular", UrlApiCirculares);
                reply = PostAPI<Reply<bool>, Circulares>(UrlAPI, ObjCirculares);



            }
            catch (Exception ex)
            {

                reply.Ok = false;
                reply.Message = "Error, " + ex;
            }

            return reply;



        }


        public Reply<List<Circulares>> ObtenerCirculares()
        {

            Reply<List<Circulares>> reply = new Reply<List<Circulares>>();
            Circulares ObjCirculares = new Circulares { IdCircular = 1 };

            try
            {
                var UrlAPI = string.Format("{0}/Circular/ObtenerCirculares", UrlApiCirculares);
                reply = PostAPI<Reply<List<Circulares>>, Circulares>(UrlAPI, ObjCirculares);



            }
            catch (Exception ex)
            {

                reply.Ok = false;
                reply.Message = "Error, " + ex;
            }

            return reply;



        }

        public Reply<List<Eventos>> ObtenerEventosCalendario()
        {

            Reply<List<Eventos>> reply = new Reply<List<Eventos>>();
            Eventos ObjCirculares = new Eventos { IdCircular = 1 };

            try
            {
                var UrlAPI = string.Format("{0}/Circular/ObtenerEventosCalendario", UrlApiCirculares);
                reply = PostAPI<Reply<List<Eventos>>, Eventos>(UrlAPI, ObjCirculares);



            }
            catch (Exception ex)
            {

                reply.Ok = false;
                reply.Message = "Error, " + ex;
            }

            return reply;



        }

        public Reply<Circulares> ObtenerCircularPorId(int IdCircular)
        {

            Reply<Circulares> reply = new Reply<Circulares>();

            try
            {
                var UrlAPI = string.Format("{0}/Circular/ObtenerCircularesPorId", UrlApiCirculares);
                reply = PostAPI<Reply<Circulares>, int>(UrlAPI, IdCircular);


            }
            catch (Exception ex)
            {

                reply.Ok = false;
                reply.Message = "Error, " + ex;
            }

            return reply;



        }

    }
}
