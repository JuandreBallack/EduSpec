using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EduSpec.Code
{
    public class AgeImportDataTables
    {
        public static DataTable Age()
        {
            DataTable table = new DataTable();
            table.Columns.Add("ImportKey", typeof(int));
            table.Columns.Add("InstID", typeof(int));
            table.Columns.Add("AccountCode", typeof(string));
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("Days120", typeof(decimal));
            table.Columns.Add("Days90", typeof(decimal));
            table.Columns.Add("Days60", typeof(decimal));
            table.Columns.Add("Days30", typeof(decimal));
            table.Columns.Add("CurrentAmount", typeof(decimal));
            table.Columns.Add("TotalDue", typeof(decimal));
            table.Columns.Add("CategoryID", typeof(int));

            return table;
        }
        public static List<string> AgeFields()
        {
            List<string> list = new List<string>() {
                "ImportKey",
                "InstID",
                "AccountCode",
                "Name",
                "Days120",
                "Days90",
                "Days60",
                "Days30",
                "CurrentAmount",
                "TotalDue",
                "CategoryID"

            };
            return list;
        }

        public static DataTable AgeTransactions()
        {

            DataTable table = new DataTable();
            table.Columns.Add("ImportKey", typeof(int));
            table.Columns.Add("InstID", typeof(int));
            table.Columns.Add("AccountCode", typeof(string));
            table.Columns.Add("TrxDate", typeof(DateTime));
            table.Columns.Add("Reference", typeof(string));
            table.Columns.Add("Amount", typeof(decimal));

            return table;
        }
        public static List<string> AgeTransactionsFields()
        {
            List<string> list = new List<string>() { "ImportKey", "InstID", "AccountCode", "TrxDate", "Reference", "Amount" };
            return list;
        }
        public static DataTable PastelCategories()
        {
            DataTable table = new DataTable();
            table.Columns.Add("InstID", typeof(int));
            table.Columns.Add("CategoryID", typeof(int));
            table.Columns.Add("Description", typeof(string));

            return table;
        }
        public static DataTable AgeSageOneTransactions()
        {
            DataTable table = new DataTable();
            table.Columns.Add("ImportKey", typeof(int));
            table.Columns.Add("InstID", typeof(int));
            table.Columns.Add("Customer", typeof(string));
            table.Columns.Add("TrxDate", typeof(DateTime));
            table.Columns.Add("Reference", typeof(string));
            table.Columns.Add("Amount", typeof(decimal));
            return table;
        }
        public static List<string> AgeSageOneTransactionsFields()
        {
            List<string> list = new List<string>() { "ImportKey", "InstID", "Customer", "TrxDate", "Reference", "Amount" };
            return list;
        }        
        public static List<string> PastelCategoriesFields()
        {
            List<string> list = new List<string>() { "InstID","CategoryID", "Description" };
            return list;
        }
        public static void BulkCopyToSQLServer(string destinationTable, DataTable dataTable, List<string> fields)
        {

            using (SqlConnection connection = new SqlConnection(string.Format(@"Server = {0}; Database = {1}; User Id = {2};Password = {3};", SystemParameters.SystemParams().SQL_Server, SystemParameters.SystemParams().SQL_Database, SystemParameters.SystemParams().SQL_User, SystemParameters.SystemParams().SQL_Password)))
            {
                connection.Open();
                using (SqlBulkCopy copy = new SqlBulkCopy(connection))
                {
                    copy.DestinationTableName = destinationTable;
                    copy.BatchSize = 1000;
                    copy.BulkCopyTimeout = 240;
                    copy.NotifyAfter = 1000;

                    foreach (string field in fields)
                    {
                        copy.ColumnMappings.Add(field, field);
                    }

                    try
                    {
                        copy.WriteToServer(dataTable);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        copy.Close();
                    }

                }


            }
        }
    }
}