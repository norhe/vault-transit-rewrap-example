using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace RewrapExample
{

    class DBHelper 
    {
        public static async Task CreateTablesAsync()
        {
            using (var db = new AppDb())
            {
                await db.Connection.OpenAsync();
                using (var cmd = db.Connection.CreateCommand())
                {
                    string command = "CREATE TABLE IF NOT EXISTS `user_data`(" +
                        "`user_id` INT(11) NOT NULL AUTO_INCREMENT, " +
                        "`user_name` VARCHAR(256) NOT NULL," +
                        "`first_name` VARCHAR(256) NULL, " +
                        "`last_name` VARCHAR(256) NULL, " +
                        "`address` VARCHAR(256) NOT NULL, " +
                        "`city` VARCHAR(256) NOT NULL," +
                        "`state` VARCHAR(256) NOT NULL," +
                        "`postcode` VARCHAR(256) NOT NULL," +
                        "`email` VARCHAR(256) NOT NULL," +
                        "`dob` VARCHAR(256) NULL," +
                        "PRIMARY KEY (user_id) " +
                        ") engine=InnoDB;";
                    cmd.CommandText = command;


                    await cmd.ExecuteNonQueryAsync();
                    Console.WriteLine("Create (if not exist) user_data table");
                }
            }
        }

        public static async Task CreateDBAsync()
        {
            using (var db = new AppDb())
            {
                await db.Connection.OpenAsync();
                using (var cmd = db.Connection.CreateCommand())
                {
                    string command = "CREATE DATABASE IF NOT EXISTS my_app";
                    cmd.CommandText = command;


                    await cmd.ExecuteNonQueryAsync();
                    Console.WriteLine("Created (if not exist) my_app DB");
                }
            }
        }

        public static async Task InsertRecordAsyc(Record r)
        {
            using (var db = new AppDb())
            {
                await db.Connection.OpenAsync();
                using (var cmd = db.Connection.CreateCommand())
                {
                    
                    string command = "INSERT INTO `user_data` " + 
                    "(`user_name`, `first_name`, `last_name`, `address`, " +
                    "`city`, `state`, `postcode`, `email`, `dob`) " +
                    $"VALUES (\"{r.Login.Username}\", \"{r.Name.First}\", \"{r.Name.Last}\", " +
                    $"\"{r.Location.Street}\", \"{r.Location.City}\", \"{r.Location.State}\", " +
                    $"\"{r.Location.Postcode}\", \"{r.Email}\", \"{r.DOB}\");";
                    
                    Console.WriteLine(command);
                    cmd.CommandText = command;

                    var rowsAffected = await cmd.ExecuteNonQueryAsync();
                    Console.WriteLine($"Created {rowsAffected} rows");
                }
            }
        }
    }
}
