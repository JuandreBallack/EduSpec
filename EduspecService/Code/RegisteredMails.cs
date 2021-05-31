using EduspecService.Models;
using NLog;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using SearchResult;

namespace EduSpecService
{
    public class RegisteredMails
    {
        public class EmailSearch
        {
            public string username { get; set; }
            public string password { get; set; }
            public string subject_contains { get; set; }
            public string to_contains { get; set; }
            public string from_contains { get; set; }
        }
        public class CertificateRequest
        {
            public string username { get; set; }
            public string password { get; set; }
            public string token { get; set; }
            public string type { get; set; }
        }
        public class CertificateData
        {
            public string status { get; set; }
            public string type { get; set; }
            public string token { get; set; }
            public string mobile { get; set; }
            public string unique_id { get; set; }
            public string certificate_processed { get; set; }
            public string certificate_url { get; set; }
        }    
        public static string readHtmlPage(string url)
        {
            try
            {
                string Result;
                WebResponse objResponse;
                WebRequest objRequest = HttpWebRequest.Create(url);
                objResponse = objRequest.GetResponse();
                var sr = new StreamReader(objResponse.GetResponseStream());
                Result = sr.ReadToEnd();                
                sr.Close();
                return Result;
            }
            catch (Exception ex)
            {
                if (ex.Message == "(404)")
                    return string.Format("URL: {0} not found.", url);
                else
                    return ex.Message;
            }
        }
        public static async Task DownloadFileAsync(int RegisteredMailID, string CertificateUri, Boolean IsReadCertificate)
        {
            var uri = new Uri(CertificateUri);
            var client = new HttpClient();
            Logging.LogEntry(LogLevel.Info, string.Format("Get pdf content for FileID : {0}", RegisteredMailID));
            var result = await client.GetAsync(uri);
            Logging.LogEntry(LogLevel.Info, string.Format("Content downloaded Successfully."));

            if (result.IsSuccessStatusCode)
            {
                HttpContent contents = result.Content;
                string FileName;

                if (IsReadCertificate)
                {
                    FileName = "E" + RegisteredMailID + "-R.pdf";
                    Logging.LogEntry(LogLevel.Info, string.Format("Download read certificate : {0}", FileName));
                }
                else
                {
                    FileName = "E" + RegisteredMailID + "-S.pdf";
                    Logging.LogEntry(LogLevel.Info, string.Format("Download send certificate : {0}", FileName));
                }

                using (var file = File.Create(Path.Combine(SystemParameters.SystemParams().RegisteredMailsCertificatesFilePath, FileName)))
                {
                    Logging.LogEntry(LogLevel.Info, string.Format("Saving certificate to : {0}", Path.Combine(SystemParameters.SystemParams().RegisteredMailsCertificatesFilePath, FileName)));
                    var contentStream = await contents.ReadAsStreamAsync();
                    await contentStream.CopyToAsync(file);
                    Logging.LogEntry(LogLevel.Info, string.Format("Certificate {0} was saved.", FileName));
                    contentStream.Dispose();
                    file.Dispose();
                }

                using (var Context = new EduSpecDataDataContext())
                {
                    if (IsReadCertificate)
                    {
                        //Context.Set_EduSpecService_RegisteredEmailSendCertificates(RegisteredMailID, FileName);
                    }                        
                    else
                    {
                        Context.Set_EduSpecService_RegisteredEmailSendCertificates(RegisteredMailID, FileName);
                    }
                }
                
            }
        }
        public static async Task DownloadSentCertificates(int TimeOut)
        {
            using (var Context = new EduSpecDataDataContext() { CommandTimeout = TimeOut * 1000 })
            {               
                Logging.LogEntry(LogLevel.Info, string.Format("Get cetrificate list to download."));
                var CertificateList = Context.Get_EduSpecService_RegisteredEmailSendCertificates().ToList();               

                var client = new HttpClient();
                CertificateRequest certificateRequest = new CertificateRequest();
                CertificateData certificateData = new CertificateData();

                foreach (var Certificate in CertificateList)
                {
                    Logging.LogEntry(LogLevel.Info, string.Format("Set Uri for RegisteredMailID : {0}.", Certificate.RegisteredMailID));
                    var uri = new Uri(string.Format("https://regcoms.certcoms.com/api/v1/get-certificate"));
                    certificateRequest.username = "hennie@anova.co.za";
                    certificateRequest.password = "mm4e8dgk";
                    certificateRequest.type = "email";
                    certificateRequest.token = Certificate.Token;
                    try
                    {
                        var json = JsonConvert.SerializeObject(certificateRequest);
                        var content = new StringContent(json, Encoding.UTF8, "application/json");
                        var result = await client.PostAsync(uri,content);

                        if (result.IsSuccessStatusCode)
                        {
                            var ResultData = await result.Content.ReadAsStringAsync();
                            certificateData = JsonConvert.DeserializeObject<CertificateData>(ResultData);
                            Logging.LogEntry(LogLevel.Info, string.Format("Get Certificate content for FileID: {0}.", certificateData.token));
                           
                            await Task.Run(async () => await DownloadFileAsync(Certificate.RegisteredMailID, certificateData.certificate_url,false));
                            
                        }
                    }
                    catch (Exception error)
                    {
                        Logging.LogEntry(LogLevel.Error, string.Format("Error downloading Sertificate: {0} - message [{1}]", Certificate.RegisteredMailID, error.ToString()));
                        Context.Set_EduSpecService_RegisteredEmailError(Certificate.RegisteredMailID, error.Message);
                    }
                } 
            }
        }        
        public static async Task GetToken(int TimeOut)
        {
            using (var Context = new EduSpecDataDataContext() { CommandTimeout = TimeOut * 1000 })
            {
                Logging.LogEntry(LogLevel.Info, string.Format("Get cetrificate list to download."));
                var CertificateList = Context.Get_EduSpecService_RegisteredEmailSearchCertificates().ToList();
                                
                var client = new HttpClient();
                EmailSearch emailSearch = new EmailSearch();
                foreach (var search in CertificateList)
                {
                    emailSearch.username = "hennie@anova.co.za";
                    emailSearch.password = "mm4e8dgk";
                    emailSearch.subject_contains = search.Subject;
                    emailSearch.to_contains = search.MailTo;
                    emailSearch.from_contains = "";

                    Logging.LogEntry(LogLevel.Info, string.Format("Search for Delivered Certificate {0}.", search.RegisteredMailID));
                    var uri = new Uri(string.Format("https://regcoms.certcoms.com/api/v1/search-email"));
                    var json = JsonConvert.SerializeObject(emailSearch);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    try
                    {
                        var result = await client.PostAsync(uri, content);
                        if (result.IsSuccessStatusCode)
                        {   
                            var ResultData = await result.Content.ReadAsStringAsync();
                            RegEmailSearchResult emailSearchResult = new RegEmailSearchResult();
                            emailSearchResult = JsonConvert.DeserializeObject<RegEmailSearchResult>(ResultData);
                            if (emailSearchResult.ResultCount > 0)
                            {
                                Logging.LogEntry(LogLevel.Info, string.Format("Certificate content found for RegisteredMailID: {0}.", search.RegisteredMailID));
                                Context.Set_EduSpecService_RegisteredEmailTokenData(
                                    search.RegisteredMailID,
                                    emailSearchResult.Results[0].Subject,
                                    emailSearchResult.Results[0].EmailTo,
                                    Convert.ToBoolean(emailSearchResult.Results[0].CertificateProcessed),
                                    emailSearchResult.Results[0].Token,
                                    emailSearchResult.Results[0].Response,
                                    emailSearchResult.Results[0].DateSent,
                                    emailSearchResult.Results[0].DateDelivered,
                                    emailSearchResult.Results[0].Description
                                    );
                                Logging.LogEntry(LogLevel.Info, string.Format("Token found for RegisteredMailID: {0} - Token : {1}.", search.RegisteredMailID, emailSearchResult.Results[0].Token));
                            }
                            else
                            {
                                Logging.LogEntry(LogLevel.Info, string.Format("Certificate Token not found for RegisteredMailID: {0}.", search.RegisteredMailID));
                            }
                        }
                        else
                        {
                            Logging.LogEntry(LogLevel.Info, string.Format("Certificate not found for RegisteredMailID: {0}.", search.RegisteredMailID));
                        }
                    }
                    catch (Exception error)
                    {
                        Logging.LogEntry(LogLevel.Error, string.Format("Error downloading Sertificate: {0} - message [{1}]", search.RegisteredMailID, error.ToString()));
                        Context.Set_EduSpecService_RegisteredEmailError(search.RegisteredMailID, error.Message);
                    }
                }
                
            }
        }
    }
}