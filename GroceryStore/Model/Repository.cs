using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryStore.Model
{
    public class Repository
    {
        private const string DB_NAME = "database.db";


        public List<Customer> GetCustomers()
        {
            List<Customer> result = new();
            using (var conn = InitConnection())
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM CUSTOMER";
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(readCustomer(reader));
                    }
                }
            }
            return result;
        }

        private static Customer readCustomer(SqliteDataReader reader)
        {
            return new Customer
            {
                Id = reader.GetInt32(0),
                FirstName = reader.GetString(1),
                LastName = reader.GetString(2),
                Age = reader.GetInt32(3)
            };
        }

        /// <summary>
        /// Saves the customer into a database and returns the saved instance, if nothing is saved due to a problem
        /// it returns null
        /// </summary>
        /// <param name="c">Customer to save</param>
        /// <returns>Saved customer</returns>
        public Customer Save(Customer c)
        {
            Customer result = null;
            using (var conn = InitConnection())
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                    INSERT INTO CUSTOMER(ID, FIRST_NAME, LAST_NAME, AGE)
                    VALUES($Id, $FirstName, $LastName, $Age)
                ";
                var param = cmd.Parameters;
                param.AddWithValue("$Id", c.Id);
                param.AddWithValue("$FirstName", c.FirstName);
                param.AddWithValue("$LastName", c.LastName);
                param.AddWithValue("$Age", c.Age);
                if (cmd.ExecuteNonQuery() > 0)
                    result = c;
            }
            return result;
        }

        private SqliteConnection InitConnection()
        {
            var connectionStr = $"Data Source={DB_NAME}";
            InitDatabase(connectionStr);
            return new SqliteConnection(connectionStr);
        }

        /// <summary>
        /// Initialize the database if required
        /// </summary>
        /// <param name="connectionStr"></param>
        private static void InitDatabase(string connectionStr)
        {
            using (var conn = new SqliteConnection(connectionStr))
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT EXISTS(SELECT 1 FROM sqlite_master WHERE type='table' AND name='CUSTOMER')";
                using (var reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    if (reader.GetInt32(0) == 0) // if value is 0 the table doesn't exist, if value is 1 table exists
                    {
                        reader.Close();
                        var createCommads = new List<string> {
                            "CREATE TABLE CUSTOMER(ID INTEGER PRIMARY KEY, FIRST_NAME VARCHAR, LAST_NAME VARCHAR, AGE INT)"                            
                        };
                        foreach (var str in createCommads)
                        {
                            cmd.CommandText = str;
                            if (cmd.ExecuteNonQuery() != 0)
                            { // successful = 0
                                throw new SystemException($"Couldn't create table for command '{str}'");
                            }
                        }
                        Debug.WriteLine("Database initialized");
                    }
                    else
                    {
                        Debug.WriteLine("Database already initialized, skipping tables creation");
                    }
                }
            }
        }

        /// <summary>
        /// Deletes the existing database if it exists.
        /// </summary>
        public void DeleteDatabase()
        {
            if (File.Exists(DB_NAME))
            {
                File.Delete(DB_NAME);
            }
        }
    }
}
