using DataImporter.Core.Abstractions;
using DataImporter.Core.Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace DataImporter.Core.Tests
{
    public class UnitTest1
    {
        private string basePath = @"D:\temp\TestData\";
        private string connectionString = "Server=.;Database=IntReach;Trusted_Connection=True;MultipleActiveResultSets=true;";

        [Fact]
        public async void ImportAllTest()
        {
            DataImporterService svc = new DataImporterService(connectionString, basePath);
            ImportResult res = await svc.importAll();
            Assert.True(res == ImportResult.ImportOK);
        }

        [Fact]
        public void GetProductsTest()
        {
            ProductService svc = new ProductService(connectionString);
            List<Product> products = svc.GetProducts(100, 102);
            Assert.True(products.Count==150);
        }
    }
}
