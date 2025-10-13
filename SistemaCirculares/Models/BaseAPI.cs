using Utils_Circulares.RestClient;

namespace SistemaCirculares.Models
{
    public class BaseAPI
    {
        protected readonly string UrlApiCirculares;

        private readonly IConfiguration _config;

        public BaseAPI(IConfiguration config)
        {
            _config = config;
            UrlApiCirculares = _config["UrlAPICirculares"];

        }

        public static T PostAPI<T, Y>(string url, Y entrada)
        {
            RestClient<T, Y> cliente = new RestClient<T, Y>();
            return cliente.PostAPI(url, entrada);
        }
    }
}
