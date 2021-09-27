using DataImporter.Core.Models;
using System.Collections.Generic;

namespace DataImporter.Core.Abstractions
{
  public interface IProductService
  {
        public List<Product> GetProducts(int companyID, int feedID);
  }
}
