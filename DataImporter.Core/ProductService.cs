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
    public class ProductService : IProductService, IDisposable
    {
        private string _connectionString;
        private SqlConnection _connection;

        private bool isDisposed = false;

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

                //use stored procedure for increased performance, retained companyID in query although it's redundant to be compatible with the requirements
                string SQLQ = $"exec sps_Products {companyID},{feedID}";
                DataTable dt = SQLServerDB.DatatableFromSQL(SQLQ, _connection);

                //compile list from datatable
                List<Product> productList = new List<Product>();

                if (dt.Rows.Count!=0)
                {
                    productList = (from DataRow dr in dt.Rows
                                   select new Product()
                                   {
                                       UniqueID = Convert.ToInt32(dr["UniqueID"]),
                                       ProductName = dr["ProductName"].ToString(),
                                       ProductBrand = dr["ProductBrand"].ToString(),
                                       ProductDescription = dr["ProductDescription"].ToString()
                                   }).ToList();
                }
                
                return productList;
            }
            catch (Exception ex)
            {
                ErrorsAndLog.HandleGenericError(ex);
                return null;
            }
        }

        public void Dispose()
        {
            {
                if (this.isDisposed) return;

                SQLServerDB.CloseConnection(_connection);
                isDisposed = true;
            }
        }
    }
}
