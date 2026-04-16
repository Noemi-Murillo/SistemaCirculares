using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Utils_Circulares.RestClient
{
    public class RestClient<T, Y>
    {
        public T PostAPI(string url, Y input)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    String postBody = JsonConvert.SerializeObject(input);
                    HttpResponseMessage response = client.PostAsync(url, new StringContent(postBody, Encoding.UTF8, "application/json")).Result;
                    string output = response.Content.ReadAsStringAsync().Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return JsonConvert.DeserializeObject<T>(output);
                    }
                    else
                    {
                        throw (new API_Exception(output));
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        [Serializable]
        public sealed class API_Exception : Exception
        {
            public readonly string errorCode;

            public API_Exception(string message)
                : base(message)
            {
                this.errorCode = Guid.NewGuid().ToString("N");
            }
        }

    }
}
