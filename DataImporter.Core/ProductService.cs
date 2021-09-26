using DataImporter.Core.Abstractions;
using DataImporter.Core.Common;
using DataImporter.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DataImporter.Core
{
    public class ProductService : IProductService
    {
        private string _connectionString;
        private SqlConnection _connection;

        public ProductService()
        { }

        public ProductService(string connectionString)
        {
            _connectionString = connectionString;

            //open connection
            _connection = new SqlConnection(connectionString);
            _connection.Open();
        }

        private Boolean ConfiguredOK()
        {
            if ((_connectionString != "") && (_connection != null)) return true;
            else return false;
        }

        public List<Product> GetProducts(int companyID, int feedID)
        {
            try
            {
                //check for correct initialization
                if (!ConfiguredOK()) return null;

                string SQLQ = $"select * from VW_ProductsGeneral WHERE companyID={companyID} AND feedID={feedID}";
                DataTable dt = SQLServerDB.DatatableFromSQL(SQLQ, _connection);

                List<Product> productList = new List<Product>();
                productList = (from DataRow dr in dt.Rows
                               select new Product()
                               {
                                   UniqueID = Convert.ToInt32(dr["UniqueID"]),
                                   ProductName = dr["ProductName"].ToString(),
                                   ProductBrand = dr["ProductBrand"].ToString(),
                                   ProductDescription = dr["ProductDescription"].ToString()
                               }).ToList();
                return productList;
            }
            catch (Exception ex)
            {
                ErrorsAndLog.HandleGenericError(ex);
                return null;
            }
        }
    }
}
