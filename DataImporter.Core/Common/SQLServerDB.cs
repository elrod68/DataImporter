using DataImporter.Core.Common;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DataImporter.Core.Common
{
    static class SQLServerDB
    {

        public static DataTable DatatableFromSQL(string SQLQ, SqlConnection sqlCon)
        {
            try
            {
                DataTable dt = new DataTable();

                using (var cmd = new SqlCommand(SQLQ, sqlCon))
                using (var da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
                return dt;

            }
            catch (Exception ex)
            {
                ErrorsAndLog.HandleGenericError(ex);
                return null;
            }
        }

        public static void CloseConnection(SqlConnection con)
        {
            try
            {
                if (con != null)
                {
                    con.Close();
                    con.Dispose();
                    con = null;
                }
            }
            catch (Exception ex)
            {
                ErrorsAndLog.HandleGenericError(ex);
                throw;
            }
        }

        public static void CleanDataTable(DataTable dt)
        {
            try
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
            }
            catch (Exception ex)
            {
                ErrorsAndLog.HandleGenericError(ex);
                throw;
            }
        }

        public static Boolean ExecuteSQL(string SQLQ, SqlConnection sqlCon)
        {
            try
            {
                using (var cmd = new SqlCommand(SQLQ, sqlCon))
                {
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                ErrorsAndLog.HandleGenericError(ex);
                return false;
            }
        }

        public static async Task<Boolean> ExecuteSQLAsync(string SQLQ, SqlConnection sqlCon)
        {
            try
            {
                using (var cmd = new SqlCommand(SQLQ, sqlCon))
                {
                    await cmd.ExecuteNonQueryAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                ErrorsAndLog.HandleGenericError(ex);
                return false;
            }
        }
    }
}
