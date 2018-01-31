using System;
using System.Collections.Generic;
using System.Net.Http;
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

        public static async Task<ApiResults> GetUserRecordsAsync(string path)
        {
            ApiResults records = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                string resp = await response.Content.ReadAsStringAsync();
                Console.WriteLine(resp);
                var des = JsonConvert.DeserializeObject(resp);
                records = JsonConvert.DeserializeObject<ApiResults>(resp);
                Console.WriteLine(records);
            }
            return records;
        } 
    }
}