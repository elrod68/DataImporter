using DataImporter.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataImporter.Core.Abstractions
{
  public interface IProductService
  {
        public List<Product> GetProducts(int companyID, int feedID);
  }
}
