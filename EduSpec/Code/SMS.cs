using System;
using System.IO;
using System.Net;

namespace EduSpec
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

        public static string Send(string Message, string CellNumber)
        {
            string MyString = "http://www.winsms.net/api/batchmessage.asp?User=";
            MyString = string.Format("{0}hennie@anova.co.za&Password=Anova@12&Delivery=No", MyString);
            MyString = string.Format("{0}&Message={1}&Numbers={2};", MyString,Message,CellNumber);            
            return readHtmlPage(MyString);
        }
    }
}