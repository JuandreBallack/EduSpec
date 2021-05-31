using System;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Text;
using EduSpec;
using JRO;
using NLog;


namespace EduSpecDataService.Code
{
    class ImportData
    {
        private static string AccessUser = StringCipher.Decrypt(ConfigurationManager.AppSettings["AccessUser"], "EduSpecDataSync");
        private static string AccessPassword = StringCipher.Decrypt(ConfigurationManager.AppSettings["AccessPassword"], "EduSpecDataSync");
        private static string ClientDatabaseFilePath = ConfigurationManager.AppSettings["ClientDatabaseFilePath"];
        private static string SyncDatabaseFilePath = ConfigurationManager.AppSettings["SyncDatabaseFilePath"];
        private static string AccessDatabaseProvider = ConfigurationManager.AppSettings["AccessDatabaseProvider"];
        
               
        public static void ImportLearners()
        {
            Logging.LogEntry(LogLevel.Info, "Using Sync Database - " + SyncDatabaseFilePath);
            Logging.LogEntry(LogLevel.Info, "Clear Learners Table.");
            using (OleDbCommand cmd = new OleDbCommand("DELETE FROM Learner_Info", Connection(SyncDatabaseFilePath)))
            {
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                cmd.Dispose();
            }
            Logging.LogEntry(LogLevel.Info, "Clear Learners Table...Done");

            Logging.LogEntry(LogLevel.Info, "Using Client Database - " + ClientDatabaseFilePath);
            Logging.LogEntry(LogLevel.Info, "Importing learners.");
            using (DataTable dt = new DataTable())
            {
                using (OleDbCommand cmd = new OleDbCommand(AccessCommands.SASAMS_Learner(), Connection(ClientDatabaseFilePath)))
                using (OleDbDataAdapter adapter = new OleDbDataAdapter(cmd))
                {
                    cmd.Connection.Open();
                    adapter.SelectCommand.CommandTimeout = 240;
                    adapter.Fill(dt);
                    Logging.LogEntry(LogLevel.Info, string.Format("Found {0} Learners.", dt.Rows.Count));                        
                    InsertIntoAccess(dt, "Learner_Info");
                    adapter.Dispose();
                    cmd.Connection.Close();
                    cmd.Dispose();
                }
            }
            Logging.LogEntry(LogLevel.Info, "Importing learners...Done");

        }
        public static void ImportParents()
        {
            Logging.LogEntry(LogLevel.Info, "Using Sync Database - " + SyncDatabaseFilePath);
            Logging.LogEntry(LogLevel.Info, "Clear Parents Table.");
            using (OleDbCommand cmd = new OleDbCommand("DELETE FROM Parent_Info", Connection(SyncDatabaseFilePath)))
            {
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                cmd.Dispose();
            }
            Logging.LogEntry(LogLevel.Info, "Clear Parents Table...Done");

            Logging.LogEntry(LogLevel.Info, "Using Client Database - " + ClientDatabaseFilePath);
            Logging.LogEntry(LogLevel.Info, "Importing Parents.");
            using (DataTable dt = new DataTable())
            {
                using (OleDbCommand cmd = new OleDbCommand(AccessCommands.SASAMS_Parents(), Connection(ClientDatabaseFilePath)))
                using (OleDbDataAdapter adapter = new OleDbDataAdapter(cmd))
                {
                    cmd.Connection.Open();
                    adapter.SelectCommand.CommandTimeout = 240;
                    adapter.Fill(dt);
                    Logging.LogEntry(LogLevel.Info, string.Format("Found {0} Parents.", dt.Rows.Count));
                    InsertIntoAccess(dt, "Parent_Info");
                    adapter.Dispose();
                    cmd.Connection.Close();
                    cmd.Dispose();
                }
            }
            Logging.LogEntry(LogLevel.Info, "Importing Parents...Done");
        }
        public static void ImportParent_Child()
        {
            Logging.LogEntry(LogLevel.Info, "Using Sync Database - " + SyncDatabaseFilePath);
            Logging.LogEntry(LogLevel.Info, "Clear Parent_Child Table.");
            ExecuteNonQuery("DELETE FROM Parent_Child", Connection(SyncDatabaseFilePath));
            //using (OleDbCommand cmd = new OleDbCommand("DELETE FROM Parent_Child", Connection(SyncDatabaseFilePath)))
            //{
            //    cmd.Connection.Open();
            //    cmd.ExecuteNonQuery();
            //    cmd.Connection.Close();
            //    cmd.Dispose();
            //}
            Logging.LogEntry(LogLevel.Info, "Clear Parent_Child Table...Done");

            Logging.LogEntry(LogLevel.Info, "Using Client Database - " + ClientDatabaseFilePath);
            Logging.LogEntry(LogLevel.Info, "Importing Parent_Child.");
            using (DataTable dt = new DataTable())
            {
                using (OleDbCommand cmd = new OleDbCommand(AccessCommands.SASAMS_Parent_Child(), Connection(ClientDatabaseFilePath)))
                using (OleDbDataAdapter adapter = new OleDbDataAdapter(cmd))
                {
                    cmd.Connection.Open();
                    adapter.SelectCommand.CommandTimeout = 240;
                    adapter.Fill(dt);
                    Logging.LogEntry(LogLevel.Info, string.Format("Found {0} Learners.", dt.Rows.Count));
                    InsertIntoAccess(dt, "Parent_Child");
                    adapter.Dispose();
                    cmd.Connection.Close();
                    cmd.Dispose();
                }
            }
            Logging.LogEntry(LogLevel.Info, "Importing Parent_Child...Done");
        }
        

        public static void SyncNewLearners()
        {
            Logging.LogEntry(LogLevel.Info, "Using Sync Database - " + SyncDatabaseFilePath);
            Logging.LogEntry(LogLevel.Info, "Add new Learners to Sync Table.");
            //ExecuteNonQuery(AccessCommands.Sync_AddNewLearners(), Connection(SyncDatabaseFilePath));
            using (OleDbCommand cmd = new OleDbCommand(AccessCommands.Sync_AddNewLearners(), Connection(SyncDatabaseFilePath)))
            {
                try
                {
                    cmd.Parameters.AddWithValue("@InstID", 10);
                    cmd.Parameters.AddWithValue("@LearnerStatus", 1);
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                    cmd.Dispose();
                }
                catch (Exception error)
                {
                    Logging.LogEntry(LogLevel.Info, error.Message);
                    cmd.Connection.Close();
                    cmd.Dispose();
                }
            }
            Logging.LogEntry(LogLevel.Info, "Add new Learners to Sync Table...Done");
            
        }

        #region Methods

        public static void InsertIntoAccess(DataTable dtbl, string TableName)
        {
            StringBuilder names = new StringBuilder();
            StringBuilder values = new StringBuilder();

            using (OleDbCommand cmd = new OleDbCommand("", Connection(SyncDatabaseFilePath)))
            {
                foreach (DataColumn column in dtbl.Columns)
                {

                    if (column.Ordinal < dtbl.Columns.Count - 1)
                    {
                        names.Append(column.ColumnName + ",");
                        values.Append("@" + column.ColumnName + ",");
                    }
                    else
                    {
                        names.Append(column.ColumnName);
                        values.Append("@" + column.ColumnName);
                    }
                }
                cmd.CommandText = "INSERT INTO " + TableName + " (" + names.ToString() + ") VALUES (" + values.ToString() + ")";
                cmd.Connection.Open();
                foreach (DataRow row in dtbl.Rows)
                {
                    foreach (DataColumn column in dtbl.Columns)
                    {
                        cmd.Parameters.Add("@" + column.ColumnName, ParamType(column)).Value = row[column.Ordinal];
                    }
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                }
                names.Clear();
                values.Clear();
                cmd.Connection.Close();
            }
        }
        public static void ExecuteNonQuery(string command, OleDbConnection connection)
        {
            using (OleDbCommand cmd = new OleDbCommand(command, connection))
            {
                try
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                    cmd.Dispose();
                }
                catch (Exception error)
                {
                    Logging.LogEntry(LogLevel.Info, error.Message);
                    cmd.Connection.Close();
                    cmd.Dispose();
                }
            }

        }

        private static OleDbType ParamType(DataColumn column)
        {
            OleDbType type = new OleDbType();
            switch (Type.GetTypeCode(column.DataType))
            {
                case TypeCode.Int16:
                case TypeCode.Int32:
                    {
                        type = OleDbType.Integer;
                    }
                    break;
                case TypeCode.String:
                    {
                        type = OleDbType.VarChar;
                    }
                    break;
                case TypeCode.Double:
                    {
                        type = OleDbType.Double;
                    }
                    break;
                case TypeCode.Boolean:
                    {
                        type = OleDbType.Boolean;
                    }
                    break;
                default:
                    type = OleDbType.VarChar;
                    break;
            }
            return type;
        }
        private static OleDbConnection Connection(string DatabaseFilePath)
        {
            OleDbConnection Conn = new OleDbConnection(string.Format(@"Provider={0};Data Source={1};Jet OLEDB:Database Password={2}", AccessDatabaseProvider, DatabaseFilePath, AccessPassword));
            return Conn;
        }

        public static void CompactAccessDB()
        {
            object[] oParams;

            //create an inctance of a Jet Replication Object
            object objJRO =
              Activator.CreateInstance(Type.GetTypeFromProgID("JRO.JetEngine"));

            //filling Parameters array
            //cnahge "Jet OLEDB:Engine Type=5" to an appropriate value
            // or leave it as is if you db is JET4X format (access 2000,2002)
            //(yes, jetengine5 is for JET4X, no misprint here)
            oParams = new object[] {
                string.Format(@"Provider={0};Data Source={1};Jet OLEDB:Database Password={2}", AccessDatabaseProvider, SyncDatabaseFilePath, AccessPassword),
                string.Format(@"Provider={0};Data Source={1};Jet OLEDB:Database Password={2}", AccessDatabaseProvider,@"D:\EduSpec\Access Data Base\Temp.mdb", AccessPassword) };

            //invoke a CompactDatabase method of a JRO object
            //pass Parameters array
            objJRO.GetType().InvokeMember("CompactDatabase",
                System.Reflection.BindingFlags.InvokeMethod,
                null,
                objJRO,
                oParams);

            //database is compacted now
            //to a new file C:\\tempdb.mdw
            //let's copy it over an old one and delete it

            System.IO.File.Delete(SyncDatabaseFilePath);
            System.IO.File.Move(@"D:\EduSpec\Access Data Base\Temp.mdb", SyncDatabaseFilePath);

            //clean up (just in case)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(objJRO);
            objJRO = null;
        }

        public const string AccessOleDbConnectionStringFormat =
  "Data Source={0};Provider=Microsoft.Jet.OLEDB.4.0;";

        /// <summary>
        /// Compacts an Access database using Microsoft JET COM
        /// interop.
        /// </summary>
        /// <param name="fileName">
        /// The filename of the Access database to compact. This
        /// filename will be mapped to the appropriate path on the
        /// web server, so use a tilde (~) to specify the web site
        /// root folder. For example, "~/Downloads/Export.mdb".
        /// The ASP.NET worker process must have been granted
        /// permission to read and write this file, as well as to
        /// create files in the folder in which this file resides.
        /// In addition, Microsoft JET 4.0 or later must be
        /// present on the server.
        /// </param>
        /// <returns>
        /// True if the compact was successful. False can indicate
        /// several possible problems including: unable to create
        /// JET COM object, unable to find source file, unable to
        /// create new compacted file, or unable to delete
        /// original file.
        /// </returns>
        public static bool CompactJetDatabase()
        {
            // I use this function as part of an AJAX page, so rather
            // than throwing exceptions if errors are encountered, I
            // simply return false and allow the page to handle the
            // failure generically.
            try
            {
                // Find the database on the web server
                string oldFileName = SyncDatabaseFilePath;
                 

                // JET will not compact the database in place, so we
                // need to create a temporary filename to use
                string newFileName = @"D:\EduSpec\Access Data Base\" + Guid.NewGuid().ToString("N") + ".mdb";

                // Obtain a reference to the JET engine
                JetEngine engine = new JetEngine();

                // Compact the database (saves the compacted version to
                // newFileName)
                engine.CompactDatabase(
                    string.Format(@"Provider={0};Data Source={1};Jet OLEDB:Database Password={2}", AccessDatabaseProvider, SyncDatabaseFilePath, AccessPassword)                 ,
                 String.Format(
                  AccessOleDbConnectionStringFormat, newFileName));

                // Delete the original database
                File.Delete(oldFileName);

                // Move (rename) the temporary compacted database to
                // the original filename
                File.Move(newFileName, oldFileName);

                // The operation was successful
                return true;
            }
            catch (Exception error)
            {
                Logging.LogEntry(LogLevel.Info, error.Message);
                return false;
            }
        }
        #endregion
    }
}
