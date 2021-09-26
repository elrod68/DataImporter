using DataImporter.Core.Abstractions;
using System;
using System.Threading.Tasks;
using DataImporter.Core.Common;
using System.Data;
using System.Linq;
using System.IO;
using CsvHelper;
using System.Data.SqlClient;

namespace DataImporter.Core
{
    public class DataImporterService : IDataImporterService
    {

        private string _connectionString;
        private string _basePath;
        private SqlConnection _connection;

        public DataImporterService(string connectionString, string basePath)
        {
            _connectionString = connectionString;
            _basePath = basePath;

            //open connection
            _connection = new SqlConnection(connectionString);
            _connection.Open();
        }
        private Boolean ConfiguredOK()
        {
            if ((_basePath != "") && (_connectionString != "") && (_connection != null)) return true;
            else return false;
        }

        private async Task<bool> deleteAll()
        {
            try
            {
                //check for correct initialization
                if (!ConfiguredOK()) return false;

                string SQLQ = "delete from FeedData delete from Products";
                await SQLServerDB.ExecuteSQLAsync(SQLQ, _connection);

                return true;
            }
            catch (Exception ex)
            {
                ErrorsAndLog.HandleGenericError(ex);
                return false;
            }
        }

        public async Task<ImportResult> importAll()
        {
            try
            {
                //check for correct initialization
                if (!ConfiguredOK()) return ImportResult.DeleteFailed;

                if (deleteAll().Result == true)
                {
                    string SQLQ = "select * from feeds";
                    DataTable feeds = SQLServerDB.DatatableFromSQL(SQLQ, _connection);

                    var companies = feeds.AsEnumerable()
                                    .GroupBy(r => new { Col1 = r["CompanyID"] })
                                    .Select(g => g.OrderBy(r => r["CompanyID"]).First());

                    foreach (var company in companies)
                    {
                        int companyID = company.Field<int>("CompanyID");
                        Console.WriteLine($"CompanyID={companyID}");
                        var companyFeeds = feeds.Select($"companyID={companyID}");
                        foreach (var feedRow in companyFeeds)
                        {
                            int feedID = feedRow.Field<int>("FeedID");
                            Console.WriteLine($"Feed ID {feedID}");
                            Boolean resImport= await ImportFeed(companyID, feedID);
                            if (!resImport) return ImportResult.ImportFailed;
                        }
                    }

                    return ImportResult.ImportOK;
                }
                else return ImportResult.DeleteFailed;
            }
            catch (Exception ex)
            {
                ErrorsAndLog.HandleGenericError(ex);
                return ImportResult.ImportFailed;
            }
        }

        private async Task<Boolean> ImportFeed(int companyID, int feedID)
        {
            const string rawFeedTable = "FeedData";
            const string destFeedTable = "Products";
            try
            {
                string filePath = _basePath + $"Company_{companyID}" + @"\Feed_" + feedID + @"\Data.csv";
                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")))
                {
                    // Do any configuration to `CsvReader` before creating CsvDataReader.
                    using (var dr = new CsvDataReader(csv))
                    {
                        var bcp = new SqlBulkCopy(_connectionString)
                        {
                            DestinationTableName = rawFeedTable
                        };

                        bcp.ColumnMappings.Add("Unique Id", "UniqueID");
                        bcp.ColumnMappings.Add("Name", "ProductName");
                        bcp.ColumnMappings.Add("Brand", "ProductBrand");
                        bcp.ColumnMappings.Add("Description", "ProductDescription");

                        await bcp.WriteToServerAsync(dr);

                        var SQLQ = $"INSERT INTO {destFeedTable} SELECT {feedID},UniqueID,ProductName,ProductBrand,ProductDescription FROM FeedData DELETE FROM {rawFeedTable}";
                        await SQLServerDB.ExecuteSQLAsync(SQLQ, _connection);
                    }
                }

                return true;
            }

            catch (Exception ex)
            {
                ErrorsAndLog.HandleGenericError(ex);
                return false;
            }
        }
    }
}
