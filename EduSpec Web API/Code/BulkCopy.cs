using EduSpec;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace EduSpecWebAPI.Code
{
    public class BulkCopy
    {
        public static DataTable SASAMSTransactions()
        {
            DataTable table = new DataTable();
            table.Columns.Add("InstID", typeof(int));
            table.Columns.Add("LearnerID", typeof(int));
            table.Columns.Add("DateDone", typeof(DateTime));
            table.Columns.Add("Description", typeof(string));
            table.Columns.Add("Amount", typeof(decimal));

            return table;
        }
        public static List<string> SASAMSTransactionsFields()
        {
            List<string> list = new List<string>() {
                "InstID",
                "LearnerID",
                "DateDone",
                "Description",
                "Amount"
            };
            return list;
        }
        public static void BulkCopyToSQLServer(string destinationTable, DataTable dataTable, List<string> fields)
        {

            using (SqlConnection connection = new SqlConnection(string.Format(@"Server = {0}; Database = {1}; User Id = {2};Password = {3};", StringCipher.Decrypt(ConfigurationManager.AppSettings["SQL_Server"], "WebAPI@12"), StringCipher.Decrypt(ConfigurationManager.AppSettings["SQL_Database"], "WebAPI@12"), StringCipher.Decrypt(ConfigurationManager.AppSettings["SQL_User"], "WebAPI@12"), StringCipher.Decrypt(ConfigurationManager.AppSettings["SQL_Password"], "WebAPI@12"))))
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
