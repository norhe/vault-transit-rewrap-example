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
        static VaultClient client = null;
        static void Main(string[] args)
        {
            //RunAsync().GetAwaiter().GetResult();
            // Get our env vars

            string vaultUri = Environment.GetEnvironmentVariable("VAULT_ADDR");
            string token = Environment.GetEnvironmentVariable("VAULT_TOKEN");
            string transitKeyName = Environment.GetEnvironmentVariable("VAULT_TRANSIT_KEY");
            string shouldSeed = Environment.GetEnvironmentVariable("SHOULD_SEED_USERS");
            string numRecords = Environment.GetEnvironmentVariable("NUMBER_SEED_USERS");

            if (null == client)
            {
                client = new VaultClient(vaultUri, token, transitKeyName);
            }

            if (null != shouldSeed) {
                SeedDB(numRecords).GetAwaiter().GetResult();
                Console.WriteLine("Seeded the database...");
            }

            RunAsync().GetAwaiter().GetResult();
        }

        static async Task InitDBAsync()
        {
            await DBHelper.CreateDBAsync();
            await DBHelper.CreateTablesAsync();

        }

        // Download records from the randomuser api, and encrypt some 
        // fields so we can rewrap them later
        static async Task SeedDB(string numRecords)
        {
            WebHelper.ApiResults apiResults = await WebHelper.GetUserRecordsAsync(numRecords);
            var tasks = new List<Task>();
            foreach (var record in apiResults.Records) {
                Console.WriteLine(record.Name.First);
                ICollection<Task> encryptValues = new List<Task>();
                record.DOB = await client.EncryptValue(record.DOB);
                record.Phone = await client.EncryptValue(record.Phone);
                record.Email = await client.EncryptValue(record.Email);
                tasks.Add(DBHelper.InsertRecordAsyc(record));
            }
            await Task.WhenAll(tasks);
            
        }
        static async Task RunAsync() {
            
            int v = await client.GetLatestTransitKeyVersion();
            Console.WriteLine($"Version: {v}");
        }
    }
}
