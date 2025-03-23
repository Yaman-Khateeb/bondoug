using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace DAL
{
    public class DataAccessSettings
    {
        public static string ConnectionString { get; private set; } = string.Empty;

        static DataAccessSettings()
        {
            try
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

                ConnectionString = config.GetConnectionString("DefaultConnection");

                if (string.IsNullOrEmpty(ConnectionString))
                {
                    throw new Exception("Connection string is missing or empty in appsettings.json!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading configuration: {ex.Message}");
                throw;
            }
        }
    }
}
