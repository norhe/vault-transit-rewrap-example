using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RewrapExample
{
    class WebHelper
    {
        public static HttpClient client = new HttpClient();
        
        public class ApiResults
        {
            [JsonProperty("results")]
            public IEnumerable<Record> Records { get; set; }
        }

        public static async Task<ApiResults> GetUserRecordsAsync()
        {
            string baseUrl = "https://randomuser.me";
            WebHelper.client.BaseAddress = new Uri(baseUrl);
            WebHelper.client.DefaultRequestHeaders.Accept.Clear();
            WebHelper.client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json")
            );    
            //Console.WriteLine($"Connecting to ")
            string query = "/api/?results=10&nat=us";
            
            ApiResults records = null;
            HttpResponseMessage response = await client.GetAsync(baseUrl + query);
            if (response.IsSuccessStatusCode)
            {
                string resp = await response.Content.ReadAsStringAsync();
                records = JsonConvert.DeserializeObject<ApiResults>(resp);
            }
            return records;
        } 
    }
}