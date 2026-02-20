using Entities_Circulares.Comites;
using Entities_Circulares.FileCirculares;
using Entities_Circulares.Reply;

namespace SistemaCirculares.Models
{
    public class ComitesModel: BaseAPI
    {


        public ComitesModel(IConfiguration config) : base(config)
        {






        }


        public Reply<List<Comites>> ObtenerComites()
        {

            Reply<List<Comites>> reply = new Reply<List<Comites>>();
            Comites ObjComites = new Comites { IdComite = 1 };

            try
            {
                var UrlAPI = string.Format("{0}/Comites/ObtenerComites", UrlApiCirculares);
                reply = PostAPI<Reply<List<Comites>>, Comites>(UrlAPI, ObjComites);



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
