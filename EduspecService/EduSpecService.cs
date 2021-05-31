using System;
using System.Configuration;
using System.Data.SqlClient;
using System.ServiceProcess;
using DevExpress.XtraReports.UI;
using EduSpecService;
using EduSpecService.Controllers;
using NLog;
using EduspecService.Models;
using System.Threading.Tasks;
using static EduSpecService.SystemParameters;
using System.Linq;

namespace EduspecService
{
    public partial class EduSpecService : ServiceBase
    {
        double ProcessIntervals;
        public System.Timers.Timer timer;
        //bool CanContinue = true;
        public EduSpecService()
        {
            InitializeComponent();
        }
        public class Schedule
        {
            public string ScheduleName { get; set; }
            public int TimeOut { get; set; }
        }

        protected override void OnStart(string[] args)
        {
            Logging.LogEntry(LogLevel.Info, "Application Start.");
            SetUp();
            timer = new System.Timers.Timer() { Interval = ProcessIntervals };
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            Logging.LogEntry(LogLevel.Info, "Start timer");
            timer.Start();
        }

        public async void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            try
            {
                Logging.LogEntry(LogLevel.Info, "Get Sysparams.");
                SysParams sysParams = new SysParams();
                sysParams = SystemParams();

                timer.Stop();
                Logging.LogEntry(LogLevel.Info, "Timer Stoped.");
                Logging.LogEntry(LogLevel.Info, "Check schedule to run.");

                //string connection = ConfigurationManager.ConnectionStrings["EduSpecConnectionString"].ToString();

                using (var Context = new EduSpecDataDataContext())
                {
                    var Result = Context.Get_Services_ScheduleToRun("EduSpec Service").FirstOrDefault();
                    
                    Schedule Schedule = new Schedule()
                    {
                        ScheduleName = Result.ScheduleName,
                        TimeOut = Result.Timeout
                    };
                    Logging.LogEntry(LogLevel.Info, Schedule.ScheduleName);

                    if (Schedule.ScheduleName != "None")
                    {
                        if (Schedule.ScheduleName == "GET_CERTIFICATES_TOKEN")
                        {
                            Logging.LogEntry(LogLevel.Info, "*** Running " + Schedule.ScheduleName + " schedule***");
                            await Task.Run(async () => await RegisteredMails.GetToken(Schedule.TimeOut));
                            Logging.LogEntry(LogLevel.Info, "Registered Mail Certificates Tokens download complete.");                            
                            Context.Set_Services_ScheduleComplete("GET_CERTIFICATES_TOKEN");
                        }
                        
                        if (Schedule.ScheduleName == "DOWNLOAD_SEND_CERTIFICATES")
                        {
                            int SertificateCount = Context.fn_Get_Eduspecservice_SendCertificates().Value;
                            if (SertificateCount > 0)
                            {
                                Logging.LogEntry(LogLevel.Info, "*** Running " + Schedule.ScheduleName + " schedule***");
                                Logging.LogEntry(LogLevel.Info, string.Format("Found [{0}] Registered Mail send Certificates to download.", SertificateCount));
                                await Task.Run(async () => await RegisteredMails.DownloadSentCertificates(Schedule.TimeOut));
                                Logging.LogEntry(LogLevel.Info, "Registered Mail send Certificates download complete.");
                            }
                            else
                            {
                                Logging.LogEntry(LogLevel.Info, string.Format("Found [{0}] Registered Mail send Certificates to download.", SertificateCount));
                            }
                            Context.Set_Services_ScheduleComplete("DOWNLOAD_SEND_CERTIFICATES");
                        }

                        //if (Schedule.ScheduleName == "DOWNLOAD_READ_CERTIFICATES")
                        //{
                        //    int SertificateCount = Context.fn_Get_Eduspecservice_ReadCertificates().Value;
                        //    if (SertificateCount > 0)
                        //    {
                        //        Logging.LogEntry(LogLevel.Info, "*** Running " + Schedule.ScheduleName + " schedule***");
                        //        Logging.LogEntry(LogLevel.Info, string.Format("Found [{0}] Registered Mail send Certificates to download.", SertificateCount));
                        //        await Task.Run(async () => await RegisteredMails.DownloadReadCertificates(Schedule.TimeOut));
                        //        Logging.LogEntry(LogLevel.Info, "Registered Mail send Certificates download complete.");
                        //    }
                        //    else
                        //    {
                        //        Logging.LogEntry(LogLevel.Info, string.Format("Found [{0}] Registered Mail send Certificates to download.", SertificateCount));
                        //    }
                        //    Context.Set_Services_ScheduleComplete("DOWNLOAD_READ_CERTIFICATES");
                        //}

                        if (Schedule.ScheduleName == "SMS_AND_EMAIS")
                        {
                            //SMS----------------------------------------------------------------------------------------

                            int smsSentCount = Context.fn_Get_SMSToSendCount().Value;
                            if (smsSentCount > 0)
                            {
                                Logging.LogEntry(LogLevel.Info, "*** SMS's to sent ***");
                                Logging.LogEntry(LogLevel.Info, string.Format("Found [{0}] SMS's to send.", smsSentCount));
                                SMS.SendAvailableSMS();
                            }
                            else
                            {
                                Logging.LogEntry(LogLevel.Info, string.Format("Found [{0}] SMS's to send.", smsSentCount));
                            }
                            
                            //SMS to prepare--------------------------------------------------------------------------------

                            int smsPrepareCount = Context.fn_Get_SMSToPrepareCount().Value;
                            if (smsPrepareCount > 0)
                            {
                                Logging.LogEntry(LogLevel.Info, "*** SMS's to prepare ***");
                                Logging.LogEntry(LogLevel.Info, string.Format("Found [{0}] SMS's to prepare.", smsPrepareCount));
                                SMS.PrepareAvailableSMS();
                            }
                            else
                            {
                                Logging.LogEntry(LogLevel.Info, string.Format("Found [{0}] SMS's to prepare.", smsPrepareCount));
                            }
                            
                            //Single SMS----------------------------------------------------------------------------------------

                            int SingleSMSSendCount = Context.fn_Get_SingleSMSToSendCount().Value;
                            if (SingleSMSSendCount > 0)
                            {
                                Logging.LogEntry(LogLevel.Info, "*** Sending single SMS's ***");
                                Logging.LogEntry(LogLevel.Info, string.Format("Found [{0}] single SMS's to send.", SingleSMSSendCount));
                                SMS.SendAvailableSingleSMS();

                            }
                            else
                            {
                                Logging.LogEntry(LogLevel.Info, string.Format("Found [{0}] single SMS's to send.", SingleSMSSendCount));
                            }
                            
                            //Letters and Documents------------------------------------------------------------------------ -

                            int LettersPrintCount = Context.fn_Get_LettersToPrintCount().Value;
                            if (LettersPrintCount > 0)
                            {
                                Logging.LogEntry(LogLevel.Info, "*** Prepare letter to save ***");
                                Logging.LogEntry(LogLevel.Info, string.Format("Found [{0}] Letters to save.", LettersPrintCount));
                                XtraReport report = ReportsController.rptBulkAgeLetter();
                            }
                            else
                            {
                                Logging.LogEntry(LogLevel.Info, string.Format("Found [{0}] Letters to save.", LettersPrintCount));
                            }

                            int InstDocumentsPrintCount = Context.fn_Get_InstDocumentsToPrintCount().Value;
                            if (InstDocumentsPrintCount > 0)
                            {
                                Logging.LogEntry(LogLevel.Info, "*** Prepare Document to save ***");
                                Logging.LogEntry(LogLevel.Info, string.Format("Found [{0}] Documents to save.", InstDocumentsPrintCount));
                                XtraReport DocReport = ReportsController.GenerateAvailableDocuments();
                            }
                            else
                            {
                                Logging.LogEntry(LogLevel.Info, string.Format("Found [{0}] Documents to save.", InstDocumentsPrintCount));
                            }

                            int LegalProcessReportsCount = Context.fn_Get_LegalProcessReportPrepareCount().Value;
                            if (LegalProcessReportsCount > 0)
                            {
                                Logging.LogEntry(LogLevel.Info, "*** Running " + Schedule.ScheduleName + " schedule***");
                                Logging.LogEntry(LogLevel.Info, string.Format("Found [{0}] Legal Process reports to prepare.", LegalProcessReportsCount));
                                XtraReport DocReport = ReportsController.rptParentAccountHistoryLegal();
                            }
                            else
                            {
                                Logging.LogEntry(LogLevel.Info, string.Format("Found [{0}] Legal Process reports to prepare.", LegalProcessReportsCount));
                            }
                        
                            //Emails----------------------------------------------------------------------------------------

                            int emailSentCount = Context.fn_GetEmailsToSentCount().Value;
                            if (emailSentCount > 0)
                            {
                                Logging.LogEntry(LogLevel.Info, "*** Sending bulk Emails ***");
                                Logging.LogEntry(LogLevel.Info, string.Format("Found [{0}] Bulk Emails to send.", emailSentCount));
                                Mailer.MailFamilyEmails();
                            }
                            else
                            {
                                Logging.LogEntry(LogLevel.Info, string.Format("Found [{0}] Bulk Emails to send.", emailSentCount));
                            }

                            int InstemailSentCount = Context.fn_GetInstEmailsToSentCount().Value;
                            if (InstemailSentCount > 0)
                            {
                                Logging.LogEntry(LogLevel.Info, "*** Sending Emails to Institutions ***");
                                Logging.LogEntry(LogLevel.Info, string.Format("Found [{0}] Inst Single Emails to send.", InstemailSentCount));
                                Mailer.MailInstitutionEmails();
                            }
                            else
                            {
                                Logging.LogEntry(LogLevel.Info, string.Format("Found [{0}] Inst Single Emails to send.", InstemailSentCount));
                            }

                            int SingleEmailSentCount = Context.fn_GetSingleEmailsToSendCount().Value;
                            if (SingleEmailSentCount > 0)
                            {
                                Logging.LogEntry(LogLevel.Info, "*** Running Sending Single Emails ***");
                                Logging.LogEntry(LogLevel.Info, string.Format("Found [{0}] Single Emails to send.", SingleEmailSentCount));
                                Mailer.MailSingleEmails();
                            }
                            {
                                Logging.LogEntry(LogLevel.Info, string.Format("Found [{0}] Single Emails to send.", SingleEmailSentCount));
                            }
                        
                            //Emails to prepare-----------------------------------------------------------------------------

                            int emailsPrepareCount = Context.fn_Get_EmailToPrepareCount().Value;
                            int BulkLettersPrepareCount = Context.fn_Get_Eduspecservice_BulkEmailLettersPrepareCount().Value;
                            if (emailsPrepareCount > 0)
                            {
                                Logging.LogEntry(LogLevel.Info, "*** Preparing Emails ***");
                                Logging.LogEntry(LogLevel.Info, string.Format("Found [{0}] Emails to prepare.", emailsPrepareCount));
                                Mailer.PrepareAvailableEmails();
                                if (BulkLettersPrepareCount > 0)
                                {
                                    Logging.LogEntry(LogLevel.Info, string.Format("Found [{0}] Bulk Email Letters to prepare.", BulkLettersPrepareCount));
                                    XtraReport DocReport = ReportsController.rptBulkEmailLetterAttachement();
                                }
                                else
                                {
                                    Logging.LogEntry(LogLevel.Info, string.Format("Found [{0}] Bulk Email Letters to prepare.", BulkLettersPrepareCount));
                                }
                            }
                            else
                            {
                                Logging.LogEntry(LogLevel.Info, string.Format("Found [{0}] Emails to prepare.", emailsPrepareCount));
                            }

                            int UpdatedEmailLetterPrepareCount = Context.fn_Get_Eduspecservice_UpdatedEmailPrepareCount().Value;
                            if (UpdatedEmailLetterPrepareCount > 0)
                            {
                                Logging.LogEntry(LogLevel.Info, "*** Update email attachemts ***");
                                Logging.LogEntry(LogLevel.Info, string.Format("Found [{0}] Update Email Letter Attachemnt.", UpdatedEmailLetterPrepareCount));
                                XtraReport DocReport = ReportsController.rptUpdatedEmailLetterAttachement();
                            }
                            else
                            {
                                Logging.LogEntry(LogLevel.Info, string.Format("Found [{0}] Update Email Letter Attachemnt.", UpdatedEmailLetterPrepareCount));
                            }

                            int ManualEmailLetterPrepareCount = Context.fn_Get_Eduspecservice_ManualEmailLetterPrepareCount().Value;
                            if (ManualEmailLetterPrepareCount > 0)
                            {
                                Logging.LogEntry(LogLevel.Info, "*** Manual email attachemnts ***");
                                Logging.LogEntry(LogLevel.Info, string.Format("Found [{0}] Manual Email Letter Attachemnt.", ManualEmailLetterPrepareCount));
                                XtraReport DocReport = ReportsController.rptManualEmailLetterAttachement();
                            }
                            else
                            {
                                Logging.LogEntry(LogLevel.Info, string.Format("Found [{0}] Manual Email Letter Attachemnt.", ManualEmailLetterPrepareCount));
                            }
                            
                            //SMS Relpies-------------------------------------------------------------------------------------
                            Logging.LogEntry(LogLevel.Info, "*** Downoad SMS replies ***");
                            SMS.GetRepliesSMS();
                            Context.Set_Services_ScheduleComplete("SMS_AND_EMAIS");
                        }

                    }
                    else
                    {
                        Logging.LogEntry(LogLevel.Info, "No schedule to run.");
                    }
                    timer.Start();
                    Logging.LogEntry(LogLevel.Info, "Timer Started.");
                }
            }
            catch(Exception xe)
            {
                Logging.LogEntry(LogLevel.Info, "Error:" + xe.Message);
            }
        }
        protected override void OnStop()
        {
            Logging.LogEntry(LogLevel.Info, "Application Stop.");
        }

        private void SetUp()
        {
            try
            {
                Logging.LogEntry(LogLevel.Info, "Settings");
                ProcessIntervals = double.Parse(ConfigurationManager.AppSettings["ProcessInterval"]);
                Logging.LogEntry(LogLevel.Info, string.Format("Setting Process Interval [{0}]", ProcessIntervals));
            }
            catch (Exception xe)
            {
                Logging.LogEntry(LogLevel.Info, "Error:" + xe.Message);
            }
        }
    }
}
