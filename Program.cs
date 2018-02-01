using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RewrapExample
{
    class Program
    {
        static void Main(string[] args)
        {
            RunAsync().GetAwaiter().GetResult();
        }

        static async Task RunAsync() {
            string baseUrl = "https://randomuser.me";
            WebHelper.client.BaseAddress = new Uri(baseUrl);
            WebHelper.client.DefaultRequestHeaders.Accept.Clear();
            WebHelper.client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json")
            );    
            //Console.WriteLine($"Connecting to ")
            string query = "/api/?results=10&nat=us";
            // retrieve data for DB
            await DBHelper.CreateDBAsync();
            await DBHelper.CreateTablesAsync();
            WebHelper.ApiResults apiResults = await WebHelper.GetUserRecordsAsync(baseUrl+query);
            Console.WriteLine(apiResults.Records);
            foreach (var record in apiResults.Records) {
                Console.WriteLine(record.Name.First);
                await DBHelper.InsertRecordAsyc(record);
            }
        }
    }
}
