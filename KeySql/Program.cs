using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using System.Data.SqlClient;
using System.Diagnostics;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using Microsoft.Azure.Management.ResourceManager.Fluent;

namespace KeySql
{
    class Program
    {
        //public static object SdkContext { get; private set; }

        static async Task Main(string[] args)
        {
            try
            {
                await RunSql().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
        }
        static async Task RunSql()
        {
            string keyVaultName = "kvsummer";
            var kvUri = "https://" + keyVaultName + ".vault.azure.net/";

          
            string userAssignedClientId = "32645928-a691-4338-b9b8-42536bc18be8";
            var credential = new DefaultAzureCredential(new DefaultAzureCredentialOptions { ManagedIdentityClientId = userAssignedClientId });

            var client = new SecretClient(new Uri(kvUri), credential);
            string secretName = "SQLPassword";
            try
            {
                //var secret = await client.GetSecretAsync(secretName);
                //string conn = "Server=tcp:summerspring.database.windows.net,1433;Initial Catalog=summer;Persist Security Info=False;User ID=summer;Password={your_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
                string conn = "Server=tcp:summerspring.database.windows.net,1433;Initial Catalog=summer;Persist Security Info=False;User ID=summer;Password=Howmany1#;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";


                //var conString = conn.Replace("{your_password}", secret.Value.Value);
                var conString = conn;
                using (SqlConnection connection = new SqlConnection(conString))
                {
                    connection.Open();
                    //
                    // SqlCommand should be created inside using.
                    // ... It receives the SQL statement.
                    // ... It receives the connection object.
                    // ... The SQL text works with a specific database.
                    //
                    
                    //String query = "INSERT INTO dbo.Product (Name,Category,Price) VALUES ( @name, @category, @price)";
                    
                    String query = "update dbo.Product set Name=@name,Category=@category,Price=@price where Id = 1";


                    using (SqlCommand command = new SqlCommand(
                        query,

                        connection))
                    {
                        //command.Parameters.AddWithValue("@id", 1);
                        command.Parameters.AddWithValue("@name", "Tomato Soup plus");
                        command.Parameters.AddWithValue("@category", "Groceries plus");
                        command.Parameters.AddWithValue("@price", 2 );

                        command.ExecuteNonQuery();

                        //
                        // Instance methods can be used on the SqlCommand.
                        // ... These read data.
                        //
                        //using (SqlDataReader reader = command.ExecuteReader())
                        //{
                        //    while (reader.Read())
                        //    {
                        //        for (int i = 0; i < reader.FieldCount; i++)
                        //        {
                        //            Console.WriteLine(reader.GetValue(i));
                        //        }
                        //        Console.WriteLine();
                        //    }
                        //}
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
