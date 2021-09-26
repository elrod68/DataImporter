using DataImporter.Core;
using DataImporter.Core.Abstractions;
using DataImporter.Core.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataImporter.ConsoleApp
{
    class Program
    {
        public static string basePath=@"D:\temp\TestData\";

        static async Task Main(string[] args)
        {

            var builder = new ConfigurationBuilder()
              .AddJsonFile($"appsettings.json", true, true);

            var config = builder.Build();
            string connectionString = config.GetConnectionString("MainDB");
            Console.WriteLine(connectionString);
            Console.WriteLine(basePath);

            DataImporterService di = new DataImporterService(connectionString, basePath);
            ImportResult res=  await di.importAll();

            ProductService pr = new ProductService(connectionString);
            List<Product> products = pr.GetProducts(100, 102);
            foreach (Product p in products)
            {
                Console.WriteLine(p.UniqueID + " " + p.ProductName);
            }

        }
    }
}
