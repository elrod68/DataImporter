using DataImporter.Core.Abstractions;
using DataImporter.Core.Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace DataImporter.Core.Tests
{
    public class DITests
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

        [Theory]
        [InlineData(100, 102)]
        [InlineData(100, 105)]
        [InlineData(200, 201)]
        [InlineData(200, 102)]
        [InlineData(300, 305)]
        public void GetProductsTest(int companyID, int feedID)
        {
            ProductService svc = new ProductService(connectionString);
            List<Product> products = svc.GetProducts(companyID, feedID);
            Assert.True(products.Count==150);
        }
    }
}
