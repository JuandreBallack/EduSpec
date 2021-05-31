using System;
using System.Linq;
using System.Net;
using System.IO;
using NLog;
using System.Configuration;
using EduspecService.Models;
using EduspecService;

namespace EduSpecService
{
    public class SMS
    {
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
                //clean up StreamReader
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

        public static void SendAvailableSMS()
        {
            using (var Context = new EduSpecDataDataContext() { CommandTimeout = 10 * 60 })
            { 

                var SMSList = Context.Get_EduSpecService_SMSsToSent().ToList();
                foreach (var DataRecord in SMSList)
                {
                    try
                    {
                        Logging.LogEntry(LogLevel.Info, "Sending SMS .......");
                        string MyString = "http://www.winsms.net/api/batchmessage.asp?User=";
                        MyString = string.Format("{0}hennie@anova.co.za&Password=Anova@12&Delivery=No", MyString);
                        MyString = string.Format("{0}&Message={1}&Numbers={2};", MyString, DataRecord.SMSMessage, DataRecord.CellNumber);
                        string SMS_OUTPUT = readHtmlPage(MyString);
                        Logging.LogEntry(LogLevel.Info, string.Format("EXEC Set_EduSpecService_SMSSent {0},{1},{2},{3}", DataRecord.SMSCorrespondenceID, DataRecord.SMSTrackingKey, true, SMS_OUTPUT));
                        Context.Set_EduSpecService_SMSSent(DataRecord.SMSCorrespondenceID,DataRecord.SMSTrackingKey,true,SMS_OUTPUT);
                        Logging.LogEntry(LogLevel.Info, "Sending SMS OK.");
                    }
                    catch (Exception error)
                    {
                        Logging.LogEntry(LogLevel.Info, string.Format("Sending SMS failed - message [{0}]", error.ToString()));
                        Context.Set_EduSpecService_SMSSent(DataRecord.SMSCorrespondenceID,DataRecord.SMSTrackingKey,false, null);
                    }
                }                
            }
        }

        public static void SendAvailableSingleSMS()
        {
            using (var Context = new EduSpecDataDataContext() { CommandTimeout = 10 * 60 })
            {



                var SMSList = Context.Get_EduSpecService_SingleSMSsToSend().ToList();
                foreach (var DataRecord in SMSList)
                {
                    try
                    {
                        Logging.LogEntry(LogLevel.Info, "Sending single SMS .......");
                        string MyString = "http://www.winsms.net/api/batchmessage.asp?User=";
                        MyString = string.Format("{0}hennie@anova.co.za&Password=Anova@12&Delivery=No", MyString);
                        MyString = string.Format("{0}&Message={1}&Numbers={2};", MyString, DataRecord.SMSMessage, DataRecord.CellNumber);
                        string SMS_OUTPUT = readHtmlPage(MyString);
                        Logging.LogEntry(LogLevel.Info, string.Format("EXEC Set_EduSpecService_SingleSMSSent {0},{1},{2},{3},{4},{5},{6}", DataRecord.SingleSMSQueueID, DataRecord.KeyValueType, DataRecord.InstID, true, DataRecord.CellNumber, DataRecord.SMSMessage, SMS_OUTPUT));
                        Context.Set_EduSpecService_SingleSMSSent(DataRecord.SingleSMSQueueID, DataRecord.KeyValueType, DataRecord.InstID, true, DataRecord.CellNumber, DataRecord.SMSMessage, SMS_OUTPUT);
                        Logging.LogEntry(LogLevel.Info, "Sending SMS OK.");
                    }
                    catch (Exception error)
                    {
                        Logging.LogEntry(LogLevel.Info, string.Format("Sending SMS failed - message [{0}]", error.ToString()));
                        Context.Set_EduSpecService_SingleSMSSent(DataRecord.SingleSMSQueueID, DataRecord.KeyValueType, DataRecord.InstID, false, DataRecord.CellNumber, DataRecord.SMSMessage, null);
                    }
                    //
                }
            }
        }

        public static void PrepareAvailableSMS()
        {
            using (var Context = new EduSpecDataDataContext() { CommandTimeout = 10 * 60 })
            {
                var SMSList = Context.Get_EduSpecService_SMSsToPrepare().ToList();
                foreach (var DataRecord in SMSList)
                {
                    try
                    {
                        Logging.LogEntry(LogLevel.Info, string.Format("Prepare SMS queue.",DataRecord.SMSTrackingKey.ToString()));
                        Logging.LogEntry(LogLevel.Info, string.Format("EXEC Set_EduSpecService_PrepareSMSQueue {0}", DataRecord.SMSTrackingKey.ToString()));
                        Context.Set_EduSpecService_PrepareSMSQueue(DataRecord.SMSTrackingKey,DataRecord.InstID);
                        Logging.LogEntry(LogLevel.Info, "SMS queue prepared.");
                    }
                    catch (Exception error)
                    {
                        Logging.LogEntry(LogLevel.Error, string.Format("Prepare SMS queue failed - message [{0}]", error.ToString()));
                    }
                }
            }
        }
        public static void GetRepliesSMS()
        {
            if (ConfigurationManager.AppSettings["SystemMode"] == "LIVE")
            {
                using (var Context = new EduSpecDataDataContext() { CommandTimeout = 10 * 60 })
                {
                    Logging.LogEntry(LogLevel.Info, "Check for SMS relpies.");
                    string replies = "https://www.winsms.co.za/api/HTTPGetReplies.ASP?User=";
                    replies = string.Format("{0}hennie@anova.co.za&Password=Anova@12&excludesc=true", replies);
                    Context.Set_EduSpecService_SMSReplies(readHtmlPage(replies));
                    Logging.LogEntry(LogLevel.Info, "Check for SMS relpies complete.");
                }
            }
        }
    }
}

