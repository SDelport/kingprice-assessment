using Newtonsoft.Json;
using System.ComponentModel;

namespace King_Price_Assessment.Services
{
    //Simulates Requests to API
    public class ApiService
    {
        private static readonly HttpClient client = new HttpClient();

        public static string BaseUrl { get; set; }

        public async Task<T> GetJsonAsync<T>(string url)
        {
            var response = await client.GetAsync($"{BaseUrl}/{url}");

            var responseString = await response.Content.ReadAsStringAsync();

            var responseObject = JsonConvert.DeserializeObject<T>(responseString);

            if (responseObject == null)
                throw new Exception("No Response from Server");

            return responseObject;
        }

        public async Task<string> GetAsync(string url)
        {
            var response = await client.GetAsync($"{BaseUrl}/{url}");

            var responseString = await response.Content.ReadAsStringAsync();

            return responseString;
        }

        public async Task<T> PostAsync<T>(string url, object request)
        {
            var response = await client.PostAsJsonAsync($"{BaseUrl}/{url}", request);

            var responseString = await response.Content.ReadAsStringAsync();

            var responseObject = JsonConvert.DeserializeObject<T>(responseString);

            if (responseObject == null)
                throw new Exception("No Response from Server");

            return responseObject;
        }

        public async Task<T> PutAsync<T>(string url, object request)
        {
            var response = await client.PutAsJsonAsync($"{BaseUrl}/{url}", request);

            var responseString = await response.Content.ReadAsStringAsync();

            var responseObject = JsonConvert.DeserializeObject<T>(responseString);

            if (responseObject == null)
                throw new Exception("No Response from Server");

            return responseObject;
        }

        public async Task DeleteAsync(string url)
        {
            var response = await client.DeleteAsync($"{BaseUrl}/{url}");

            var responseString = await response.Content.ReadAsStringAsync();
        }
    }
}
