using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comparitter.Database
{
    public static class GenericDataAccess
    {
        #region Public Methods

        public static SqlCommand CreateCommand(string connectionString)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(connectionString);
            return cmd;
        }

        public static int ExecuteNonQuery(SqlCommand cmd)
        {
            int affectedRows = -1;
            try
            {
                cmd.Connection.Open();
                affectedRows = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //TODO: Log
                throw;
            }
            finally
            {
                cmd.Connection.Close();
            }
            return affectedRows;
        }

        public static DataTable ExecuteSelectCommand(SqlCommand cmd)
        {
            DataTable dt = new DataTable(); ;
            try
            {
                cmd.Connection.Open();
                DbDataReader reader = cmd.ExecuteReader();
                dt.Load(reader);
                reader.Close();
            }
            catch (Exception ex)
            {
                //TODO: Log
                throw;
            }
            finally
            {
                cmd.Connection.Close();
            }
            return dt;
        }

        public static string ExecuteScalar(SqlCommand cmd)
        {
            string value = "";
            try
            {
                cmd.Connection.Open();
                object result = cmd.ExecuteScalar();
                value = (result != null) ? result.ToString() : "";
            }
            catch (Exception ex)
            {
                //TODO: Log
                throw;
            }
            finally
            {
                cmd.Connection.Close();
            }
            return value;
        }

        public static bool BulkWrite(string connectionString, DataTable data, string tableName, List<SqlBulkCopyColumnMapping> columnMappings = null, int timeout = 30)
        {
            bool success = false;
            SqlBulkCopy bulkWrite = new SqlBulkCopy(connectionString);
            bulkWrite.DestinationTableName = tableName;
            bulkWrite.BulkCopyTimeout = timeout;
            if (columnMappings != null)
            {
                foreach (SqlBulkCopyColumnMapping map in columnMappings)
                {
                    bulkWrite.ColumnMappings.Add(map);
                }
            }
            try
            {
                bulkWrite.WriteToServer(data);
                success = true;
            }
            catch (Exception ex)
            {
                //TODO: Log
                throw;
            }
            return success;
        }

        #endregion Public Methods
    }
}