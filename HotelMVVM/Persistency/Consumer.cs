using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HotelMVVM.Persistency
{
    public class Consumer<T>
    {
        private string URI;

        public Consumer(string URI)
        {
            this.URI = URI;
        }

        private async Task<TB> GettingAsync<TB>(string URI)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage resp = await client.GetAsync(URI);
                if (resp.IsSuccessStatusCode)
                {
                    string result = await resp.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<TB>(result);
                }
                throw new HttpRequestException(resp.StatusCode.ToString());
            }
        }

        private async Task<bool> AlteringAsync(Func<Task<HttpResponseMessage>> clientResponse)
        {
            HttpResponseMessage resp = await clientResponse();
            if (resp.IsSuccessStatusCode)
            {
                string result = await resp.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<bool>(result);
            }

            throw new HttpRequestException(resp.StatusCode.ToString());
        }

        private StringContent EncodeContent(T item)
        {
            string jsonStr = JsonConvert.SerializeObject(item);
            StringContent content = new StringContent(jsonStr, Encoding.UTF8, "application/json");
            return content;
        }

        private string RouteIds(int[] ids)
        {
            string route = "";
            foreach (int id in ids)
            {
                route += "/" + id;
            }

            return route;
        }

        public async Task<List<T>> GetAsync()
        {
            return await GettingAsync<List<T>>(URI);
        }

        public async Task<T> GetOneAsync(int[] ids)
        {
            return await GettingAsync<T>(URI + RouteIds(ids));
        }

        public async Task<bool> PostAsync(T item)
        {
            StringContent content = EncodeContent(item);
            using (HttpClient client = new HttpClient())
            {
                return await AlteringAsync(() => client.PostAsync(URI, content));
            }
        }

        public async Task<bool> PutAsync(int[] ids, T item)
        {
            StringContent content = EncodeContent(item);
            using (HttpClient client = new HttpClient())
            {
                return await AlteringAsync(() => client.PutAsync(URI + RouteIds(ids), content));
            }
        }

        public async Task<bool> DeleteAsync(int[] ids)
        {
            using (HttpClient client = new HttpClient())
            {
                return await AlteringAsync(() => client.DeleteAsync(URI + RouteIds(ids)));
            }
        }
    }
}
