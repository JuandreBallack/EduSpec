using NLog;
using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using EduspecService.Models;
using OpenPop.Pop3;
using System.Collections.Generic;
using OpenPop.Mime;
using System.IO;

namespace EduSpecService
{
    class Mailer
    {
        private static Pop3Client Pop3Client()
        {
            var pop3client = new Pop3Client();
            Logging.LogEntry(LogLevel.Info, "Retrieving Invoices.");
            Logging.LogEntry(LogLevel.Info, "Connecting to mailbox.....");
            pop3client.Connect(
                StringCipher.Decrypt(ConfigurationManager.AppSettings["RegisteredMailHost"], "EduspecService"),
                Convert.ToInt32(StringCipher.Decrypt(ConfigurationManager.AppSettings["RegisteredMailPort"], "EduspecService")),
                false
                ); //UseSSL true or false
            pop3client.Authenticate(StringCipher.Decrypt(ConfigurationManager.AppSettings["RegisteredMailUserName"], "EduspecService"),
                StringCipher.Decrypt(ConfigurationManager.AppSettings["RegisteredMailPassword"], "EduspecService"));
            Logging.LogEntry(LogLevel.Info, "Connected to mailbox.....");
            return pop3client;
        }
        public static void PrepareAvailableEmails()
        {
            using (var Context = new EduSpecDataDataContext() { CommandTimeout = 10 * 60 })
            {
                var EmailList = Context.Get_EduSpecService_EmailsToPrepare().ToList();
                foreach (var DataRecord in EmailList)
                {
                    try
                    {
                        Logging.LogEntry(LogLevel.Info, string.Format("Prepare Email queue.", DataRecord.EmailTrackingKey.ToString()));
                        Logging.LogEntry(LogLevel.Info, string.Format("EXEC Set_EduSpecService_PrepareEmailQueue {0}", DataRecord.EmailTrackingKey.ToString()));
                        Context.Set_EduSpecService_PrepareEmailQueue(DataRecord.EmailTrackingKey, DataRecord.IsAge , DataRecord.AttachLetter, DataRecord.AttachFile);
                        Logging.LogEntry(LogLevel.Info, "Email queue prepared.");
                    }
                    catch (Exception error)
                    {
                        Logging.LogEntry(LogLevel.Error, string.Format("Prepare Email queue failed - message [{0}]", error.ToString()));
                    }
                }
            }
        }

        public static void MailFamilyEmails()
        {
            string SystemMode = ConfigurationManager.AppSettings["SystemMode"];
            Logging.LogEntry(LogLevel.Info, string.Format("Setting System Mode [{0}]", SystemMode));   
            using (var Context = new EduSpecDataDataContext())
            {
                var MailList = Context.Get_EduSpecService_FamiliesToMail().ToList();
                foreach (var _Data in MailList)
                {
                    try
                    {

                        Logging.LogEntry(LogLevel.Info,"Setting mail server settings.");
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

                        mailclient.UseDefaultCredentials = false;
                        mailclient.Credentials = new NetworkCredential(mailCred.UserName, mailCred.Password);
                        Logging.LogEntry(LogLevel.Info, string.Format("Mail Server [Host] : [{0}]", StringCipher.Decrypt(ConfigurationManager.AppSettings["Host"], "EduspecService")));
                        Logging.LogEntry(LogLevel.Info, string.Format("Mail Server [Port] : [{0}]", StringCipher.Decrypt(ConfigurationManager.AppSettings["Port"], "EduspecService")));
                        Logging.LogEntry(LogLevel.Info, string.Format("Mail Server [User] : [{0}]", StringCipher.Decrypt(ConfigurationManager.AppSettings["UserName"], "EduspecService")));

                        string FromEmail;
                        if (MailList.FirstOrDefault().IsSection41Letter == true)
                        {
                            FromEmail = "registeredemail@eduspec.co.za";
                        }
                        else
                        {
                            FromEmail = "no-reply@eduspec.co.za";                            
                        }

                        MailMessage mail = new MailMessage()
                        {
                            From = new MailAddress(FromEmail, _Data.Name),
                            Subject = _Data.EmailSubject,
                            Body = _Data.EmailBody,
                            IsBodyHtml = true
                        };

                        if (_Data.FileName != null && _Data.FileName != "")
                        {
                            if(_Data.IsAge == true && _Data.IsLegalProcess == false)
                            {
                                Logging.LogEntry(LogLevel.Info, string.Format("Adding file to message : {0}{1}", SystemParameters.SystemParams().EmailAttachementsFilePath, _Data.FileName));
                                mail.Attachments.Add(new Attachment(string.Format("{0}{1}", SystemParameters.SystemParams().EmailAttachementsFilePath, _Data.FileName), "application/pdf"));
                            }
                            else 
                            if (_Data.IsAge == true && _Data.IsLegalProcess == true)
                            {
                                var Attachements = Context.Get_EduSpecService_LegalProcessEmailAttachements(_Data.InstID, _Data.FamilyID).ToList();
                                Logging.LogEntry(LogLevel.Info, string.Format("Adding attachement : {0}", _Data.FileName));
                                mail.Attachments.Add(new Attachment(string.Format("{0}{1}", SystemParameters.SystemParams().EmailAttachementsFilePath, _Data.FileName), "application/pdf"));
                                foreach (var Attachement in Attachements)
                                {                                    
                                    if (Attachement.FilePath != "No File")
                                    {
                                        Logging.LogEntry(LogLevel.Info, Attachement.FilePath);
                                        mail.Attachments.Add(new Attachment(Attachement.FilePath, "application/pdf"));
                                    }                                        
                                }
                            }
                            else
                            {
                                Logging.LogEntry(LogLevel.Info, string.Format("Adding file to message : {0}{1}", SystemParameters.SystemParams().PrintoutsFilePath, _Data.FileName));
                                mail.Attachments.Add(new Attachment(string.Format("{0}{1}", SystemParameters.SystemParams().PrintoutsFilePath, _Data.FileName), "application/pdf"));
                            }
                            
                        };

                        if (SystemMode == "DEV" || SystemMode == "UAT")
                        {
                            string s = "juandre@anova.co.za;hennie@eduspec.co.za";
                            string[] toMails = s.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            Logging.LogEntry(LogLevel.Info, string.Format("Adding email to address : {0}", s));
                            foreach (string item in toMails)
                            {
                                if (item.Trim() != "")
                                {
                                    mail.To.Add(new MailAddress(item));
                                }

                            }                            
                        }

                        if (SystemMode == "LIVE")
                        {
                            string[] toMails = _Data.Recipient.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            Logging.LogEntry(LogLevel.Info, string.Format("Adding email to address : {0}",_Data.Recipient));
                            foreach (string item in toMails)
                            {
                                if (item.Trim() != "")
                                {
                                    mail.To.Add(new MailAddress(item));
                                }
                            }

                            string[] ccMails = _Data.Cc.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            Logging.LogEntry(LogLevel.Info, string.Format("Adding email cc address : {0}", _Data.Cc));
                            foreach (string item in ccMails)
                            {
                                if (item.Trim() != "")
                                {
                                    mail.CC.Add(new MailAddress(item));
                                }
                            }
                        }
                        Logging.LogEntry(LogLevel.Info, "Sending mail .......");
                        mailclient.Send(mail);
                        Context.Set_EduSpecService_EmailSent(_Data.EmailCorrespondenceID,_Data.EmailTrackingKey, true,null, _Data.InstID, _Data.FamilyID, _Data.StatusID, _Data.IsLegalProcess, _Data.UserID, _Data.EmailBody, _Data.Recipient, _Data.Cc, _Data.FileName, _Data.DebtCollectorID);
                        Logging.LogEntry(LogLevel.Info, "Sending mail OK.");
                    }
                    catch (Exception error)
                    {
                        Logging.LogEntry(LogLevel.Info, string.Format("Sending mail failed - message [{0}]", error.ToString()));
                        Context.Set_EduSpecService_EmailSent(_Data.EmailCorrespondenceID,_Data.EmailTrackingKey, false, error.ToString(), _Data.InstID, _Data.FamilyID, _Data.StatusID, _Data.IsLegalProcess, _Data.UserID, _Data.EmailBody, _Data.Recipient, _Data.Cc, _Data.FileName, _Data.DebtCollectorID);
                    }

                }
                
                //if(MailList.FirstOrDefault().IsSection41Letter == true)
                //{
                //    Context.Set_EduSpecService_Section41EmailInvoices_Add(Convert.ToInt32(MailList.FirstOrDefault().EmailTrackingKey), Convert.ToInt32(MailList.FirstOrDefault().InstID));
                //}
                
            }
        }


        public static void MailInstitutionEmails()
        {
            string SystemMode = ConfigurationManager.AppSettings["SystemMode"];
            Logging.LogEntry(LogLevel.Info, string.Format("Setting System Mode [{0}]", SystemMode));
            using (var Context = new EduSpecDataDataContext())
            {
                var MailList = Context.Get_EduspecService_InstitutionsToMail().ToList();
                foreach (var _Data in MailList)
                {
                    try
                    {
                        Logging.LogEntry(LogLevel.Info, "Setting mail server settings.");
                        SmtpClient mailclient = new SmtpClient()
                        {
                            Port = Convert.ToInt32(StringCipher.Decrypt(ConfigurationManager.AppSettings["Port"], "EduspecService")),
                            Host = StringCipher.Decrypt(ConfigurationManager.AppSettings["Host"], "EduspecService"),
                            DeliveryMethod = SmtpDeliveryMethod.Network,
                            UseDefaultCredentials = false,
                            Timeout = Convert.ToInt32(ConfigurationManager.AppSettings["MailTimeOut"]),
                            //ReadReceipt = true
                        };

                        NetworkCredential mailCred = new NetworkCredential()
                        {
                            UserName = StringCipher.Decrypt(ConfigurationManager.AppSettings["UserName"], "EduspecService"),
                            Password = StringCipher.Decrypt(ConfigurationManager.AppSettings["Password"], "EduspecService")
                        };

                        mailclient.Credentials = new NetworkCredential(mailCred.UserName, mailCred.Password);
                        Logging.LogEntry(LogLevel.Info, string.Format("Mail Server [Host] : [{0}]", StringCipher.Decrypt(ConfigurationManager.AppSettings["Host"], "EduspecService")));
                        Logging.LogEntry(LogLevel.Info, string.Format("Mail Server [Port] : [{0}]", StringCipher.Decrypt(ConfigurationManager.AppSettings["Port"], "EduspecService")));
                        Logging.LogEntry(LogLevel.Info, string.Format("Mail Server [User] : [{0}]", StringCipher.Decrypt(ConfigurationManager.AppSettings["UserName"], "EduspecService")));

                        MailMessage mail = new MailMessage()
                        {
                            From = new MailAddress("no-reply@eduspec.co.za", "EduSpec"),
                            Subject = _Data.EmailSubject,
                            Body = _Data.EmailBody,
                            IsBodyHtml = true

                        };

                        if (_Data.FileName != null && _Data.FileName != "")
                        {
                            
                            string[] Documents = _Data.FileName.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            Logging.LogEntry(LogLevel.Info, string.Format("Adding attachement : {0}", _Data.FileName));
                            foreach (string item in Documents)
                            {
                                if (item.Trim() != "")
                                {
                                    if (item.Substring(0,3) == "INV")
                                        mail.Attachments.Add(new Attachment(string.Format("{0}{1}", SystemParameters.SystemParams().InstitutionDocumentsFilePath, item).ToString(), "application/pdf"));
                                    if (item.Substring(0,3) == "STM")
                                        mail.Attachments.Add(new Attachment(string.Format("{0}{1}", SystemParameters.SystemParams().InstitutionDocumentsFilePath, item).ToString(), "application/pdf"));
                                }
                            }
                        };

                        if (SystemMode == "DEV")
                        {
                            string s = "info@eduspec.co.za";
                            string[] toMails = s.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            Logging.LogEntry(LogLevel.Info, string.Format("Adding email to address : {0}", s));
                            foreach (string item in toMails)
                            {
                                if (item.Trim() != "")
                                {
                                    mail.To.Add(new MailAddress(item));
                                }

                            }

                            string ccMails = "hennie@eduspec.co.za";
                            Logging.LogEntry(LogLevel.Info, string.Format("Adding email cc address : {0}", "hennie@eduspec.co.za"));
                            mail.CC.Add(new MailAddress(ccMails));
                        }

                        if (SystemMode == "LIVE")
                        {
                            string[] toMails = _Data.EmailRecipient.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            Logging.LogEntry(LogLevel.Info, string.Format("Adding email to address : {0}", _Data.EmailRecipient));
                            foreach (string item in toMails)
                            {
                                if (item.Trim() != "")
                                {
                                    mail.To.Add(new MailAddress(item));
                                }
                            }

                            string ccMails = "info@eduspec.co.za";
                            Logging.LogEntry(LogLevel.Info, string.Format("Adding email cc address : {0}", "info@eduspec.co.za"));
                            mail.CC.Add(new MailAddress(ccMails));

                        }

                        Logging.LogEntry(LogLevel.Info, "Sending mail .......");
                        mailclient.Send(mail);
                        Context.Set_EduspecService_DocumentSent(_Data.CorrespondenceID, true, null);
                        Logging.LogEntry(LogLevel.Info, "Sending mail OK.");
                    }
                    catch (Exception error)
                    {
                        Logging.LogEntry(LogLevel.Info, string.Format("Sending mail failed - message [{0}]", error.ToString()));
                        Context.Set_EduspecService_DocumentSent(_Data.CorrespondenceID, false, error.ToString());
                    }

                }
            }
        }

        public static void MailSingleEmails()
        {
            string SystemMode = ConfigurationManager.AppSettings["SystemMode"];
            Logging.LogEntry(LogLevel.Info, string.Format("Setting System Mode [{0}]", SystemMode));
            using (var Context = new EduSpecDataDataContext())
            {
                var MailList = Context.Get_EduspecService_SingleEmailsToMail().ToList();
                foreach (var _Data in MailList)
                {
                    try
                    {
                        Logging.LogEntry(LogLevel.Info, "Setting mail server settings.");
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
                        Logging.LogEntry(LogLevel.Info, string.Format("Mail Server [Host] : [{0}]", StringCipher.Decrypt(ConfigurationManager.AppSettings["Host"], "EduspecService")));
                        Logging.LogEntry(LogLevel.Info, string.Format("Mail Server [Port] : [{0}]", StringCipher.Decrypt(ConfigurationManager.AppSettings["Port"], "EduspecService")));
                        Logging.LogEntry(LogLevel.Info, string.Format("Mail Server [User] : [{0}]", StringCipher.Decrypt(ConfigurationManager.AppSettings["UserName"], "EduspecService")));

                        MailMessage mail = new MailMessage()
                        {
                            From = new MailAddress("no-reply@eduspec.co.za", _Data.InstName),
                            Subject = _Data.Subject,
                            Body = _Data.EmailMessage,
                            IsBodyHtml = true
                        };

                        
                        if (_Data.FileName != null && _Data.FileName != "")
                        {
                            if (_Data.KeyValueType == 1)//Exemption
                            {
                                Logging.LogEntry(LogLevel.Info, string.Format("Adding file to message : {0}{1}.pdf", SystemParameters.SystemParams().EmailAttachementsFilePath.ToString(), _Data.FileName));
                                mail.Attachments.Add(new Attachment(string.Format("{0}{1}", SystemParameters.SystemParams().EmailAttachementsFilePath, _Data.FileName).ToString(), "application/pdf"));
                            }
                            else if (_Data.KeyValueType == 3)//Legal Process
                            {
                                string[] Documents = _Data.FileName.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                Logging.LogEntry(LogLevel.Info, string.Format("Adding attachement : {0}", _Data.FileName));
                                foreach (string item in Documents)
                                {
                                    if (item.Trim() != "")
                                    {
                                        Logging.LogEntry(LogLevel.Info, string.Format("Adding file to message : {0}{1}.pdf", SystemParameters.SystemParams().AgeAnalysisFilePath.ToString(), _Data.FileName));
                                        mail.Attachments.Add(new Attachment(string.Format("{0}{1}", SystemParameters.SystemParams().AgeAnalysisFilePath, item).ToString(), "application/pdf"));
                                    }
                                }
                            }
                            else //Debt Management
                            {
                                Logging.LogEntry(LogLevel.Info, string.Format("Adding file to message : {0}{1}.pdf", SystemParameters.SystemParams().EmailAttachementsFilePath.ToString(), _Data.FileName));
                                mail.Attachments.Add(new Attachment(string.Format("{0}{1}", SystemParameters.SystemParams().EmailAttachementsFilePath, _Data.FileName).ToString(), "application/pdf"));
                            }


                        };
                        

                        if (SystemMode == "DEV")
                        {
                            mail.Headers.Add("Disposition-Notification-To", "juandre@anova.co.za");

                            string s = "juandre@anova.co.za; hennie@anova.co.za";
                            string[] toMails = s.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            Logging.LogEntry(LogLevel.Info, string.Format("Adding email to address : {0}", s));
                            foreach (string item in toMails)
                            {
                                if (item.Trim() != "")
                                {
                                    mail.To.Add(new MailAddress(item));
                                }

                            }

                            s = "juandre@anova.co.za; hennie@anova.co.za";
                            string[] ccMails = s.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            Logging.LogEntry(LogLevel.Info, string.Format("Adding email cc address : {0}", s));
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
                            string[] toMails = _Data.Recipient.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            Logging.LogEntry(LogLevel.Info, string.Format("Adding email to address : {0}", _Data.Recipient));
                            foreach (string item in toMails)
                            {
                                if (item.Trim() != "")
                                {
                                    mail.To.Add(new MailAddress(item));
                                }
                            }

                            string[] ccMails = _Data.Cc.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            Logging.LogEntry(LogLevel.Info, string.Format("Adding email cc address : {0}", _Data.Cc));
                            foreach (string item in ccMails)
                            {
                                if (item.Trim() != "")
                                {
                                    mail.CC.Add(new MailAddress(item));
                                }
                            }
                        }

                        Logging.LogEntry(LogLevel.Info, "Sending mail .......");
                        mailclient.Send(mail);
                        Context.Set_EduspecService_SingleEmailSent(_Data.SingleEmailQueueID, _Data.KeyValueType, true, null);
                        Logging.LogEntry(LogLevel.Info, "Sending mail OK.");
                    }
                    catch (Exception error)
                    {
                        Logging.LogEntry(LogLevel.Info, string.Format("Sending mail failed - message [{0}]", error.ToString()));
                        Context.Set_EduspecService_SingleEmailSent(_Data.SingleEmailQueueID, null, false, error.ToString());
                    }
                }
            }
        }
    }
}