using Utils_Circulares.RestClient;

namespace SistemaCirculares.Models
{
    public class BaseAPI
    {
        public static T PostAPI<T, Y>(string url, Y entrada)
        {
            RestClient<T, Y> cliente = new RestClient<T, Y>();
            return cliente.PostAPI(url, entrada);
        }
    }
}
