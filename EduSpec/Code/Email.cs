using EduSpec.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace EduSpec.Code
{
    public class Email
    {

        public static void sendSingleEmail(int InstID, string Recipient, string CcEmail, string Subject, string MessageEmail,string FileName)
        {
            string SystemMode = ConfigurationManager.AppSettings["SystemMode"];
            using (var Context = new EduSpecDataContext())
            {

                try
                {
                    SmtpClient mailclient = new SmtpClient()
                    {
                        Port = Convert.ToInt32(StringCipher.Decrypt(ConfigurationManager.AppSettings["Port"], "EduspecService")),
                        Host = StringCipher.Decrypt(ConfigurationManager.AppSettings["Host"], "EduspecService"),
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Timeout = Convert.ToInt32(ConfigurationManager.AppSettings["MailTimeOut"])
                    };

                    NetworkCredential mailCred = new NetworkCredential()
                    {
                        UserName = StringCipher.Decrypt(ConfigurationManager.AppSettings["UserName"], "EduspecService"),
                        Password = StringCipher.Decrypt(ConfigurationManager.AppSettings["Password"], "EduspecService")
                    };

                    mailclient.Credentials = new NetworkCredential(mailCred.UserName, mailCred.Password);

                    MailMessage mail = new MailMessage()
                    {
                        From = new MailAddress("no-reply@eduspec.co.za", Context.fn_GetInstitutionName(InstID)),
                        Subject = Subject,
                        Body = MessageEmail,
                        IsBodyHtml = true
                    };

                    if (FileName != null && FileName != "")
                    {
                        //Logging.LogEntry(LogLevel.Info, string.Format("Adding file to message : {0}{1}.pdf", ConfigurationManager.AppSettings["OrdersPath"].ToString(), FileName));
                        mail.Attachments.Add(new Attachment(string.Format("{0}{1}.pdf", SystemParameters.SystemParams().EmailUploadFilePath, FileName).ToString(), "application/pdf"));
                    };

                    if (SystemMode == "DEV")
                    {
                        string s = "juandre@anova.co.za";
                        string[] toMails = s.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        foreach (string item in toMails)
                        {
                            if (item.Trim() != "")
                            {
                                mail.To.Add(new MailAddress(item));
                            }

                        }

                        s = "juandre@anova.co.za";
                        string[] ccMails = s.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        foreach (string item in ccMails)
                        {
                            if (item.Trim() != "")
                            {

                                mail.CC.Add(new MailAddress(item));
                            }
                        }
                    }

                    if (SystemMode == "LIVE")
                    {
                        string[] toMails = Recipient.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        foreach (string item in toMails)
                        {
                            if (item.Trim() != "")
                            {
                                mail.To.Add(new MailAddress(item));
                            }
                        }

                        string[] ccMails = CcEmail.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        foreach (string item in ccMails)
                        {
                            if (item.Trim() != "")
                            {
                                mail.CC.Add(new MailAddress(item));
                            }
                        }
                    }
                    //var File = "";

                    mailclient.Send(mail);
                    //if (IsExemption == false)
                    //{
                    //    Context.Set_DebtManagement_ActionHistory(InstID, FamilyID, StatusID, 3, UserID, MessageEmail, null, false, Recipient + ';' + CcEmail, null, null, 0, null);
                    //}
                    //else
                    //{
                    //    Context.Set_ExemptionApplicationHistory(ExemptionID, InstID, StatusID, 6, UserID, ref File, MessageEmail, null, Recipient + ';' + CcEmail);
                    //}
                    //Logging.LogEntry(LogLevel.Info, "Sending mail OK.");
                }
                catch (Exception error)
                {
                    //Logging.LogEntry(LogLevel.Info, string.Format("Sending mail failed - message [{0}]", error.ToString()));
                    //Context.Set_DebtManagement_ActionHistory(_Data.CorrespondenceID, _Data.TrackingKey, false, error.ToString());
                    string x = error.Message;
                }

            }
        }

    }
}