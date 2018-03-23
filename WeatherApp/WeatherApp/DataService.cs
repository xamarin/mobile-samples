using System.Net.Http;
using System.Threading.Tasks;
// using Newtonsoft.Json;


using System.Runtime.Serialization.Json;
using System.IO;

namespace WeatherApp
{
    public class DataService
    {
        public static async Task<dynamic> getDataFromService(string queryString)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(queryString);

            dynamic data = null;
            if (response != null)
            {
                string json = response.Content.ReadAsStringAsync().Result;

                var x = new DataContractJsonSerializer(typeof(object));

           //     StringReader reader = new StringReader(json);

                MemoryStream stream = new MemoryStream();
                StreamWriter writer = new StreamWriter(stream);
                writer.Write(json);
                writer.Flush();
                stream.Position = 0;
                
                data = x.ReadObject(stream);


          //      data = JsonConvert.DeserializeObject(json);
            }

            return data;
        }
    }
}
