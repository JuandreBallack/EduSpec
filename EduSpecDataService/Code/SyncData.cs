using EduSpec;
using EduSpecDataService.Models;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EduSpecDataService.Code
{
    class SyncData
    {
        private static int InsID = Convert.ToInt32(ConfigurationManager.AppSettings["InsID"]);

        #region Learners

        public static async Task SyncLearnersAsync(SysParams sysParams)
        {
            Logging.LogEntry(LogLevel.Info, "Using Client Database - " + sysParams.SASAMS_FilePath);
            List<Learner> LearnersList = new List<Learner>();
            using (DataTable dt = new DataTable())
            {
                using (OleDbCommand cmd = new OleDbCommand(AccessCommands.SASAMS_Learner(), Connection(sysParams)))
                using (OleDbDataAdapter adapter = new OleDbDataAdapter(cmd))
                {
                    cmd.Connection.Open();
                    adapter.SelectCommand.CommandTimeout = 240;
                    adapter.Fill(dt);
                    Logging.LogEntry(LogLevel.Info, string.Format("Found {0} Learners.", dt.Rows.Count));                    
                    adapter.Dispose();
                    cmd.Connection.Close();
                    cmd.Dispose();
                }
                
                LearnersList = (from DataRow dr in dt.Rows
                               select new Learner()
                               {
                                   InstID = Convert.ToInt32(InsID),
                                   ID = Convert.ToInt32(dr["ID"]),
                                   LearnerID = dr["LearnerID"].ToString(),
                                   AccessionNo = dr["AccessionNo"].ToString(),
                                   EntryDate = dr["EntryDate"].ToString(),
                                   DateLeft = dr["DateLeft"].ToString(),
                                   Title = dr["Title"].ToString(),
                                   Initials = dr["Initials"].ToString(),
                                   FullName = dr["FullName"].ToString(),
                                   Surname = dr["Surname"].ToString(),
                                   BirthDate = dr["BirthDate"].ToString(),
                                   IDNumber = dr["IDNumber"].ToString(),
                                   Gender = dr["Gender"].ToString(),
                                   Language = dr["LangDesc"].ToString(),
                                   TelHome = dr["TelHome"].ToString(),
                                   TelCell = dr["TelCell"].ToString(),
                                   Email = dr["Email"].ToString(),
                                   GradeID = Convert.ToInt32(dr["GradeID"]),
                                   ClassID = Convert.ToInt32(dr["ClassID"]),
                                   ClassName = dr["ClassName"].ToString(),
                                   Race = dr["Race"].ToString(),
                                   LuritsNumber = Convert.ToDouble(dr["LuritsNumber"]),
                                   Status = dr["Status"].ToString()
                               }).ToList();
                
            }
            Logging.LogEntry(LogLevel.Info, "Importing learners...Done");

            try
            {
                await Task.Run(() => PostLearners(LearnersList, "Learners").Wait());
            }
             catch (Exception ex)
            {
                //if (ex.InnerException != null)
                //Context.Set_Items_EcommSyncItemModified_Error(Product.Ecomm_ProductsID, ex.InnerException.Message, IserUtils.CurrentUser().UserID);
                //else
                //Context.Set_Items_EcommSyncItemModified_Error(Product.Ecomm_ProductsID, ex.Message, IserUtils.CurrentUser().UserID);
            }            
        }
        public static async Task SyncParentsAsync(SysParams sysParams)
        {
            Logging.LogEntry(LogLevel.Info, "Using Client Database - " + sysParams.SASAMS_FilePath);
            
            List<Parent> ParentsList = new List<Parent>();
            using (DataTable dt = new DataTable())
            {
                using (OleDbCommand cmd = new OleDbCommand(AccessCommands.SASAMS_Parents(), Connection(sysParams)))
                using (OleDbDataAdapter adapter = new OleDbDataAdapter(cmd))
                {
                    cmd.Connection.Open();
                    adapter.SelectCommand.CommandTimeout = 240;
                    adapter.Fill(dt);
                    Logging.LogEntry(LogLevel.Info, string.Format("Found {0} Parents.", dt.Rows.Count));
                    adapter.Dispose();
                    cmd.Connection.Close();
                    cmd.Dispose();
                }

                ParentsList = (from DataRow dr in dt.Rows
                                select new Parent()
                                {
                                    InstID = Convert.ToInt32(InsID),
                                    ParentID = Convert.ToInt32(dr["ParentID"]),
                                    Title = dr["Title"].ToString(),
                                    Initials = dr["Initials"].ToString(),
                                    FullName = dr["FullName"].ToString(),
                                    Surname = dr["Surname"].ToString(),
                                    IDNumber = dr["IDNumber"].ToString(),
                                    Employer = dr["Employer"].ToString(),
                                    Occupation = dr["Occupation"].ToString(),
                                    StreetAddress1 = dr["StreetAddress1"].ToString(),
                                    StreetAddress2 = dr["StreetAddress2"].ToString(),
                                    StreetAddress3 = dr["StreetAddress3"].ToString(),
                                    StreetCode = dr["StreetCode"].ToString(),
                                    PostalAddress1 = dr["PostalAddress1"].ToString(),
                                    PostalAddress2 = dr["PostalAddress2"].ToString(),
                                    PostalAddress3 = dr["PostalAddress3"].ToString(),
                                    PostalCode = dr["PostalCode"].ToString(),
                                    TelHome = dr["TelHome"].ToString(),
                                    TelWork = dr["TelWork"].ToString(),
                                    TelCell = dr["TelCell"].ToString(),
                                    TelFax = dr["TelFax"].ToString(),
                                    EMail = dr["EMail"].ToString(),
                                    Gender = dr["Gender"].ToString(),
                                    Homelanguage = dr["Homelanguage"].ToString(),
                                    SpouseFullName = dr["SpouseFullName"].ToString(),
                                    SpouseSurname = dr["SpouseSurname"].ToString(),
                                    SpouseIDNumber = dr["SpouseIDNumber"].ToString(),
                                    SpouseOccupation = dr["SpouseOccupation"].ToString(),
                                    SpouseWorkTel = dr["SpouseWorkTel"].ToString(),
                                    SpouseGender = dr["SpouseGender"].ToString(),
                                    SpouseCell = dr["SpouseCell"].ToString(),
                                    SpouseEmail = dr["SpouseEmail"].ToString(),
                                    MaritalStatus = dr["MaritalStatus"].ToString(),
                                    Relationship = dr["Relationship"].ToString(),
                                    Status = dr["Status"].ToString()
                                }).ToList();

            }            

            try
            {
                await Task.Run(() => PostParents(ParentsList, "Parents").Wait());
            }
            catch (Exception ex)
            {
                //if (ex.InnerException != null)
                //Context.Set_Items_EcommSyncItemModified_Error(Product.Ecomm_ProductsID, ex.InnerException.Message, IserUtils.CurrentUser().UserID);
                //else
                //Context.Set_Items_EcommSyncItemModified_Error(Product.Ecomm_ProductsID, ex.Message, IserUtils.CurrentUser().UserID);
            }
        }
        public static async Task SyncTransactionsAsync(SysParams sysParams)
        {
            Logging.LogEntry(LogLevel.Info, "Using Client Database - " + sysParams.SASAMS_FilePath);

            List<Transactions> TransactionList = new List<Transactions>();
            using (DataTable dt = new DataTable())
            {
                using (OleDbCommand cmd = new OleDbCommand(AccessCommands.SASAMS_Transactions(), Connection(sysParams)))
                using (OleDbDataAdapter adapter = new OleDbDataAdapter(cmd))
                {
                    cmd.Connection.Open();
                    adapter.SelectCommand.CommandTimeout = 240;
                    adapter.Fill(dt);
                    Logging.LogEntry(LogLevel.Info, string.Format("Found {0} Transactions.", dt.Rows.Count));
                    adapter.Dispose();
                    cmd.Connection.Close();
                    cmd.Dispose();
                }

                TransactionList = (from DataRow dr in dt.Rows
                               select new Transactions()
                               {
                                   InstID =sysParams.InstID,
                                   SubAccNumber = Convert.ToInt32(dr["SubAccNumber"]),
                                   Description = dr["Operation"].ToString(),
                                   Date = Convert.ToDateTime(dr["Date"]),
                                   Amount = Convert.ToDecimal(dr["Amount"])
                               }).ToList();

            }

            try
            {
                await Task.Run(() => PostTransactions(TransactionList, "Transactions").Wait());
            }
            catch (Exception ex)
            {
                //if (ex.InnerException != null)
                //Context.Set_Items_EcommSyncItemModified_Error(Product.Ecomm_ProductsID, ex.InnerException.Message, IserUtils.CurrentUser().UserID);
                //else
                //Context.Set_Items_EcommSyncItemModified_Error(Product.Ecomm_ProductsID, ex.Message, IserUtils.CurrentUser().UserID);
            }
        }


        static public HttpClient Authenticate()
        {
            HttpClient client;
            string username = StringCipher.Decrypt(ConfigurationManager.AppSettings["RestUS"], "RestService");
            string password = StringCipher.Decrypt(ConfigurationManager.AppSettings["RestPW"], "RestService");
            var authData = string.Format("{0}:{1}", username, password);
            var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));

            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);

            return client;
        }

        #endregion

        #region Parent_Child

        public static async Task SyncParent_ChildAsync(SysParams sysParams)
        {
            Logging.LogEntry(LogLevel.Info, "Using Client Database - " + sysParams.SASAMS_FilePath);
            
            List<Parent_Child> Parent_ChildList = new List<Parent_Child>();
            using (DataTable dt = new DataTable())
            {
                using (OleDbCommand cmd = new OleDbCommand(AccessCommands.SASAMS_Parent_Child(), Connection(sysParams)))
                using (OleDbDataAdapter adapter = new OleDbDataAdapter(cmd))
                {
                    cmd.Connection.Open();
                    adapter.SelectCommand.CommandTimeout = 240;
                    adapter.Fill(dt);
                    Logging.LogEntry(LogLevel.Info, string.Format("Found {0} Learners.", dt.Rows.Count));
                    adapter.Dispose();
                    cmd.Connection.Close();
                    cmd.Dispose();
                }

                Parent_ChildList = (from DataRow dr in dt.Rows
                                select new Parent_Child()
                                {
                                    InstID = Convert.ToInt32(InsID),
                                    ParentID = Convert.ToInt32(dr["ParentID"]),
                                    ChildID = Convert.ToInt32(dr["ChildID"]),
                                    LearnerID = dr["LearnerID"].ToString(),
                                    Status = dr["Status"].ToString(),
                                    FamilyCode =  dr["FamilyCode"].ToString()
                                }).ToList();

            }
            Logging.LogEntry(LogLevel.Info, "Importing learners...Done");
            try
            {
                await Task.Run(() => PostParent_Child(Parent_ChildList, "Parent_Child").Wait());
            }
            catch (Exception ex)
            {
                //if (ex.InnerException != null)
                //Context.Set_Items_EcommSyncItemModified_Error(Product.Ecomm_ProductsID, ex.InnerException.Message, IserUtils.CurrentUser().UserID);
                //else
                //Context.Set_Items_EcommSyncItemModified_Error(Product.Ecomm_ProductsID, ex.Message, IserUtils.CurrentUser().UserID);
            }
        }
        static public async Task<HttpResponseMessage> PostLearners(List<Learner> LearnerList, string UrlKey)
        {
            HttpClient client = new HttpClient();
            //using (HttpClient client = Authenticate())
            //{
            var uri = new Uri(string.Format("http://rest.eduspec.co.za/api/{0}/{1}", UrlKey, InsID));

            var json = JsonConvert.SerializeObject(LearnerList);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(uri, content);
            //}

            if (response.IsSuccessStatusCode)
            {
                Logging.LogEntry(LogLevel.Info, "Post learners Successful");
            }
            else
            {
                Logging.LogEntry(LogLevel.Error, "Post Parents failed : " + response.Headers.Warning.ToString());
            }
            return response;
        }
        static public async Task<HttpResponseMessage> PostParents(List<Parent> ParentList, string UrlKey)
        {
            HttpClient client = new HttpClient();
            //using (HttpClient client = Authenticate())
            //{
            var uri = new Uri(string.Format("http://rest.eduspec.co.za/api/{0}/{1}", UrlKey, InsID));

            var json = JsonConvert.SerializeObject(ParentList.Skip(0).Take(1000));
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(uri, content);

            //}
            if (response.IsSuccessStatusCode)
            {
                Logging.LogEntry(LogLevel.Info, "Post Parents Successful");
            }
            else
            {
                Logging.LogEntry(LogLevel.Error, "Post Parents failed : " + response.Headers.Warning.ToString());
            }
            return response;
        }
        static public async Task<HttpResponseMessage> PostTransactions(List<Transactions> TransactionList, string UrlKey)
        {
            HttpClient client = new HttpClient();
            //using (HttpClient client = Authenticate())
            //{
            Logging.LogEntry(LogLevel.Info, string.Format("Clearing SASAMS_Sync_Transactions for InsID : {0}", InsID));
            var uri = new Uri(string.Format("http://rest.eduspec.co.za/api/{0}/ClearTransactionTable/{1}", UrlKey, InsID));
            HttpResponseMessage response = await client.PutAsync(uri, null);
            Logging.LogEntry(LogLevel.Info, string.Format("SASAMS_Sync_Transactions cleared"));

            uri = new Uri(string.Format("http://rest.eduspec.co.za/api/{0}/{1}", UrlKey, InsID));
            for (int i = 0; i < TransactionList.Count; i = i + 10000)
            {
                var json = JsonConvert.SerializeObject(TransactionList.Skip(i).Take(10000));
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                response = await client.PostAsync(uri, content);
                
                if (response.IsSuccessStatusCode)
                {
                    Logging.LogEntry(LogLevel.Info, string.Format("Posting Transactions {0} to {1} of {2} Successful", i + 1, i + 10000 < TransactionList.Count ? i + 10000 : TransactionList.Count, TransactionList.Count));
                }
                else
                {
                    Logging.LogEntry(LogLevel.Error, "Posting Transactions failed : " + response.Headers.Warning.ToString());
                }
            }
            //}
            return response;
        }
        static public async Task<HttpResponseMessage> PostParent_Child(List<Parent_Child> Parent_ChildList,string UrlKey)
        {
            HttpResponseMessage response = null;

            HttpClient client = new HttpClient();
            //using (HttpClient client = Authenticate())
            //{
            var uri = new Uri(string.Format("http://rest.eduspec.co.za/api/{0}/{1}", UrlKey, InsID));
            var json = JsonConvert.SerializeObject(Parent_ChildList);
            var content = new StringContent(json, Encoding.UTF8, "application/json");            
            response = await client.PostAsync(uri, content);
            //}
            if (response.IsSuccessStatusCode)
            {
                Logging.LogEntry(LogLevel.Info, "Post Parent_Child Successful");
            }
            else
            {
                Logging.LogEntry(LogLevel.Error, "Post Parent_Child failed : " + response.Headers.Warning.ToString());
            }
            return response;
        }
        #endregion

        #region Sys Params

        static public async Task<SysParams> GetSysParamsAsync(int InstID, string UrlKey)
        {
            SysParams SysParams = new SysParams();
            HttpClient client = new HttpClient();
            //using (HttpClient client = Authenticate())
            //{
            var uri = new Uri(string.Format("http://rest.eduspec.co.za/api/{0}/{1}", UrlKey, InstID));
                var response = await client.GetAsync(uri);
                var content = await response.Content.ReadAsStringAsync();
                SysParams = JsonConvert.DeserializeObject<SysParams>(content);
            //}

            return SysParams;
        }
        #endregion

        #region Methods

        private static OleDbConnection Connection(SysParams sysParams)
        {
            OleDbConnection Conn = new OleDbConnection(string.Format(@"Provider={0};Data Source={1};Jet OLEDB:Database Password={2}", sysParams.AccessDatabaseProvider, sysParams.SASAMS_FilePath, sysParams.SASAMS_Password));
            return Conn;
        }
        #endregion
    }
}
