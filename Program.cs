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
            
            // retrieve data for DB
            await DBHelper.CreateDBAsync();
            await DBHelper.CreateTablesAsync();
            WebHelper.ApiResults apiResults = await WebHelper.GetUserRecordsAsync(baseUrl + query);
            Console.WriteLine(apiResults.Records);
            foreach (var record in apiResults.Records) {
                Console.WriteLine(record.Name.First);
                await DBHelper.InsertRecordAsyc(record);
            }
        }
    }
}
