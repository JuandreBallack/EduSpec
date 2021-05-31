using DevExpress.XtraReports.UI;
using EduspecService.Models;
using NLog;
using System;
using System.Linq;
using System.Web.Mvc;

namespace EduSpecService.Controllers
{
    //[SessionExpire]
    public class ReportsController : Controller
    {
        //private static XtraReport reportBulkFirst;

        public static XtraReport rptBulkAgeLetter()
        {
            using (var Context = new EduSpecDataDataContext())
            {
                Logging.LogEntry(LogLevel.Info, "Beginning save process of letters...");


                var TrackingLetterList = Context.Get_EduspecService_BulkLetterTracking_List().ToList();
                var QueueLetterList = Context.Get_EduspecService_BulkLetterQueue_List().ToList();
                int LetterTrackingKey = Convert.ToInt32(QueueLetterList.First().LetterTrackingKey);

                int count = 0;

                //Bulk letter---------------------------------------------------------------------------------------------------------------------------------

                foreach (var letter in TrackingLetterList)
                {
                    try
                    {

                        XtraReport reportBulk = new XtraReport();
                        reportBulk.LoadLayout(System.Windows.Forms.Application.StartupPath + "\\GenericLetter.repx");
                        reportBulk.DataSource = new
                        {
                            RPT_GenericLetter = Context.RPT_Bulk_Letters(letter.LetterTrackingKey),
                            RPT_Letterhead = Context.RPT_Letterhead(letter.InstID, letter.IsPrintLetterhead)
                        };
                        reportBulk.CreateDocument();

                        string resultFilePathBulk = String.Format("{0}{1}", SystemParameters.SystemParams().PrintoutsFilePath, String.Format(letter.FileName));

                        reportBulk.ExportToPdf(resultFilePathBulk);

                        Context.Set_EduSpecService_BulkLettersPrinted(letter.LetterTrackingKey);

                    }
                    catch (Exception error)
                    {
                        Logging.LogEntry(LogLevel.Info, string.Format("Printing letter failed - message [{0}]", error.ToString()));
                    }
                }
                

                //Single letters---------------------------------------------------------------------------------------------------------------------------------

                foreach (var letter in QueueLetterList)
                {
                    try
                    {
                       
                        XtraReport report = new XtraReport();
                        report.LoadLayout(System.Windows.Forms.Application.StartupPath + "\\GenericLetter.repx");
                        report.DataSource = new
                        {
                            RPT_GenericLetter = Context.RPT_Age_Letters(letter.InstID, Context.fn_Get_ActiveYearID(), letter.StatusID, letter.FamilyID, true, letter.LetterID, letter.LetterCorrespondenceID),
                            RPT_Letterhead = Context.RPT_Letterhead(letter.InstID, letter.IsPrintLetterhead)
                        };
                        report.CreateDocument();

                        var FileName = Context.Set_EduspecService_DebtManagement_ActionHistory(
                             letter.InstID,
                             letter.FamilyID,
                             letter.StatusID,
                             2, //LegalActionActions
                             letter.UserID,
                             null,
                             null,
                             false,
                             null,
                             null,
                             null,
                             letter.LetterID,
                             null,
                             null).First().FileName;

                        string resultFilePath = String.Format("{0}{1}", SystemParameters.SystemParams().PrintoutsFilePath, String.Format(FileName));

                        report.ExportToPdf(resultFilePath);

                        Context.Set_EduSpecService_LetterSaved(letter.LetterCorrespondenceID, LetterTrackingKey, true, FileName);

                        count = count + 1;

                        Logging.LogEntry(LogLevel.Info, string.Format("Bulk letter [{0}] with File name: [{1}] added to Legal history", count.ToString(), FileName));

                    }
                    catch (Exception error)
                    {
                        Logging.LogEntry(LogLevel.Info, string.Format("Printing letter failed - message [{0}]", error.ToString()));
                        Context.Set_EduSpecService_LetterSaved(letter.LetterCorrespondenceID, LetterTrackingKey, false, error.ToString());
                    }
                }
                

                return null;
            }
        }

        public static XtraReport GenerateAvailableDocuments()
        {
            using (var Context = new EduSpecDataDataContext())
            {
                var DocumentList = Context.Get_EduspecService_DocumentsToGenerate().ToList();
                foreach (var Data in DocumentList)
                {
                    try
                    {

                        string InvoiceFileName = null;
                        string StatementFileName = null;
                        
                        if(Data.DocumentTypeID == 1)
                        {
                            InvoiceFileName = string.Format("{0}{1}.{2}", "INV", Data.DocumentID, "pdf");
                            StatementFileName = string.Format("{0}{1}_{2}.{3}", "STM", Data.CorrespondenceID, Data.InstID, "pdf");
                        }
                        else if (Data.DocumentTypeID == 2)
                        {
                            StatementFileName = string.Format("{0}{1}_{2}.{3}", "STM", Data.CorrespondenceID, Data.InstID, "pdf");
                        }

                        Logging.LogEntry(LogLevel.Info, String.Format("Generate Document - InstID: {0}, CorrespondenceID: {1}, DocumentDescription: {2}", Data.InstID, Data.CorrespondenceID, Data.DocumentDesc ));

                        XtraReport report = new XtraReport();
                        Logging.LogEntry(LogLevel.Info, "Set document layout...");
                        if(Data.DocumentTypeID == 1)
                        {
                            //INVOICE--------------------------------------------------------------------------------------------------------------------------------
                            report.LoadLayout(System.Windows.Forms.Application.StartupPath + "\\Invoice.repx");
                            Logging.LogEntry(LogLevel.Info, "Opening details dataset...");
                            report.DataSource = new
                            {
                                RPT_InvoiceHeader = Context.RPT_InvoiceHeader(Data.InstID, Data.DocumentID),
                                RPT_Invoice = Context.RPT_Invoice(Data.DocumentID)
                            };
                            Logging.LogEntry(LogLevel.Info, "Generating Invoice.");
                            report.CreateDocument();
                            Logging.LogEntry(LogLevel.Info, String.Format("Invoice generated : {0}", InvoiceFileName));
                            string resultFilePath = String.Format("{0}{1}", SystemParameters.SystemParams().InstitutionInvoiceFilePath, String.Format(InvoiceFileName));
                            Logging.LogEntry(LogLevel.Info, String.Format("Save Invoice to : {0}", resultFilePath));
                            report.ExportToPdf(resultFilePath);
                            Logging.LogEntry(LogLevel.Info, String.Format("Invoice {0} saved.", InvoiceFileName));

                            //STATEMENT------------------------------------------------------------------------------------------------------------------------------
                            //report.LoadLayout(System.Windows.Forms.Application.StartupPath + "\\Statement.repx");
                            //Logging.LogEntry(LogLevel.Info, "Opening details dataset...");
                            //report.DataSource = new
                            //{
                            //    RPT_Statement = Context.RPT_Statement(Data.InstID),
                            //    RPT_StatementHeader = Context.RPT_StatementHeader(Data.InstID),
                            //    RPT_StatementFooter = Context.RPT_StatementFooter(Data.InstID)
                            //};
                            //Logging.LogEntry(LogLevel.Info, "Generating document.");
                            //report.CreateDocument();
                            //string StatementresultFilePath = String.Format("{0}{1}", SystemParameters.SystemParams().InstitutionDocumentsFilePath, String.Format(StatementFileName));
                            //report.ExportToPdf(StatementresultFilePath);

                        }
                        else if(Data.DocumentTypeID == 2)
                        {
                            report.LoadLayout(System.Windows.Forms.Application.StartupPath + "\\Statement.repx");
                            Logging.LogEntry(LogLevel.Info, "Opening details dataset...");
                            report.DataSource = new
                            {
                                RPT_StatementHeader = Context.RPT_StatementHeader(Data.InstID),
                                RPT_Statement = Context.RPT_Statement(Data.InstID),
                                RPT_StatementFooter = Context.RPT_StatementFooter(Data.InstID)
                            };
                            Logging.LogEntry(LogLevel.Info, "Generating document.");
                            report.CreateDocument();
                            string resultFilePath = String.Format("{0}{1}", SystemParameters.SystemParams().InstitutionDocumentsFilePath, String.Format(StatementFileName));
                            report.ExportToPdf(resultFilePath);
                        }
                        
                        Logging.LogEntry(LogLevel.Info, String.Format("Executing Set_EduspecService_DocumentGenerated {0}, {1}, {2}", Data.CorrespondenceID, StatementFileName, Data.DocumentDesc));
                        if(Data.DocumentTypeID == 1)
                        {
                            Context.Set_EduspecService_DocumentGenerated(
                            Data.CorrespondenceID,
                            //InvoiceFileName + "|" + StatementFileName,
                            InvoiceFileName,
                            Data.DocumentDesc);
                        }
                        else if(Data.DocumentTypeID == 2)
                        {
                            Context.Set_EduspecService_DocumentGenerated(
                            Data.CorrespondenceID,
                            StatementFileName,
                            Data.DocumentDesc);
                        }
                        
                        Logging.LogEntry(LogLevel.Info, "Set_EduspecService_DocumentGenerated Executed Successfully");
                        Logging.LogEntry(LogLevel.Info, "Document generated.");

                    }
                    catch (Exception error)
                    {
                        Logging.LogEntry(LogLevel.Info, string.Format("Document generating failed - message [{0}]", error.ToString()));
                    }

                }

                return null;

            }

        }

        public static XtraReport rptBulkEmailLetterAttachement()
        {
            using (var Context = new EduSpecDataDataContext())
            {
                Logging.LogEntry(LogLevel.Info, "Beginning save process of letters...");

                var EmailLetterList = Context.Get_EduspecService_BulkEmailLetterAttachment_List().ToList();

                int count = 0;
                
                foreach (var letter in EmailLetterList)
                {
                    try
                    {

                        XtraReport report = new XtraReport();
                        report.LoadLayout(System.Windows.Forms.Application.StartupPath + "\\GenericLetter.repx");
                        report.DataSource = new
                        {
                            RPT_GenericLetter = Context.RPT_Age_Letters(letter.InstID, Context.fn_Get_ActiveYearID(), letter.StatusID, letter.FamilyID, false, letter.InstLetterID, 0),
                            RPT_Letterhead = Context.RPT_Letterhead(letter.InstID, letter.IsPrintLetterHead)
                        };
                        report.CreateDocument();

                        string resultFilePath = null;

                        //if (letter.IsAge == false)
                        //{
                            resultFilePath = String.Format("{0}{1}", SystemParameters.SystemParams().EmailAttachementsFilePath, String.Format(letter.FileName));
                        //}
                        //else
                        //{
                        //    resultFilePath = String.Format("{0}{1}", SystemParameters.SystemParams().EmailAttachementsFilePath, String.Format(letter.FileName));
                        //}

                        report.ExportToPdf(resultFilePath);

                        Context.Set_EduspecService_EmailAttachementSaved_Update(letter.EmailCorrespondenceID);

                        count = count + 1;

                        Logging.LogEntry(LogLevel.Info, string.Format("Email letter [{0}] with File name: [{1}] added to Legal history", count.ToString(), letter.FileName));

                    }
                    catch (Exception error)
                    {
                        Logging.LogEntry(LogLevel.Info, string.Format("Printing letter failed - message [{0}]", error.ToString()));
                    }
                }


                return null;
            }
        }

        public static XtraReport rptUpdatedEmailLetterAttachement()
        {
            using (var Context = new EduSpecDataDataContext())
            {
                Logging.LogEntry(LogLevel.Info, "Beginning save process of letters (email updated)...");

                var EmailLetterList = Context.Get_EduspecService_UpdatedEmailLetterAttachment_List().ToList();

                int count = 0;

                foreach (var letter in EmailLetterList)
                {
                    try
                    {

                        XtraReport report = new XtraReport();
                        report.LoadLayout(System.Windows.Forms.Application.StartupPath + "\\GenericLetter.repx");
                        report.DataSource = new
                        {
                            RPT_GenericLetter = Context.RPT_Age_Letters(letter.InstID, Context.fn_Get_ActiveYearID(), letter.StatusID, letter.FamilyID, false, letter.InstLetterID, 0),
                            RPT_Letterhead = Context.RPT_Letterhead(letter.InstID, letter.IsPrintLetterHead)
                        };
                        report.CreateDocument();

                        string resultFilePath = String.Format("{0}{1}", SystemParameters.SystemParams().EmailAttachementsFilePath, String.Format(letter.FileName));

                        report.ExportToPdf(resultFilePath);

                        Context.Set_EduspecService_UpdatedEmailGenerated(letter.EmailCorrespondenceID);

                        count = count + 1;

                        Logging.LogEntry(LogLevel.Info, string.Format("Email letter [{0}] with File name: [{1}] saved to Email Attachements", count.ToString(), letter.FileName));

                    }
                    catch (Exception error)
                    {
                        Logging.LogEntry(LogLevel.Info, string.Format("Saving of letter failed - message [{0}]", error.ToString()));
                    }
                }


                return null;
            }
        }

        public static XtraReport rptManualEmailLetterAttachement()
        {
            using (var Context = new EduSpecDataDataContext())
            {
                Logging.LogEntry(LogLevel.Info, "Beginning save process of manual email letters ...");

                var EmailLetterList = Context.Get_EduspecService_ManualEmailLetterAttachment_List().ToList();

                int count = 0;

                foreach (var letter in EmailLetterList)
                {
                    try
                    {

                        XtraReport report = new XtraReport();                        
                        report.LoadLayout(System.Windows.Forms.Application.StartupPath + "\\GenericLetter.repx");                        
                        report.DataSource = new
                        {  
                            RPT_GenericLetter = Context.RPT_Age_Letters(letter.InstID, Context.fn_Get_ActiveYearID(), letter.StatusID, letter.FamilyID, false, letter.InstLetterID, 0),
                            RPT_Letterhead = Context.RPT_Letterhead(letter.InstID, letter.IsPrintLetterHead)
                        };
                        report.CreateDocument();

                        string resultFilePath = String.Format("{0}{1}", SystemParameters.SystemParams().EmailAttachementsFilePath, String.Format(letter.FileName));

                        report.ExportToPdf(resultFilePath);

                        Context.Set_EduspecService_ManualEmailLetterGenerated(letter.EmailCorrespondenceID);

                        count = count + 1;

                        Logging.LogEntry(LogLevel.Info, string.Format("Email letter [{0}] with File name: [{1}] saved in Email Attachements", count.ToString(), letter.FileName));

                    }
                    catch (Exception error)
                    {
                        Logging.LogEntry(LogLevel.Info, string.Format("Saving of letter failed - message [{0}]", error.ToString()));
                    }
                }


                return null;
            }
        }

        public static XtraReport rptParentAccountHistoryLegal()
        {
            using (var Context = new EduSpecDataDataContext())
            {
                var ReportList = Context.Get_EduSpecService_LegalProcessReportsPrepare().ToList();
                foreach (var item in ReportList)
                {
                    try
                    {
                        Logging.LogEntry(LogLevel.Info, String.Format("Set Parent Account History report parameters : InstID = {0}; FamilyID = {1}", item.InstID.ToString(),item.FamilyID.ToString()));
                        XtraReport report = new XtraReport();
                        report.LoadLayout(System.Windows.Forms.Application.StartupPath + "\\ParentAccountHistory.repx");
                        report.Parameters["FamilyID"].Value = item.FamilyID;
                        report.Parameters["InstID"].Value = item.InstID;
                        //report.DataSource = new
                        //{
                        //    RPT_InstitutionDetails = Context.RPT_InstitutionDetails(item.InstID),
                        //    RPT_ParentAccountHistory_AgeFamilyActionHistory = Context.RPT_ParentAccountHistory_AgeFamilyActionHistory(item.FamilyID),
                        //    RPT_ParentAccountHistory_ParentInfo = Context.RPT_ParentAccountHistory_ParentInfo(item.FamilyID),
                        //    RPT_ParentAccountHistory_LearnerInfo = Context.RPT_ParentAccountHistory_LearnerInfo(item.FamilyID),
                        //    RPT_ParentAccountHistory_Statement = Context.RPT_ParentAccountHistory_Statement(item.FamilyID)
                        //};

                        Logging.LogEntry(LogLevel.Info, "Generate Parent Account History report");
                        report.CreateDocument();
                        Logging.LogEntry(LogLevel.Info, "Report Generated.");

                        string resultFilePath = String.Format("{0}{1}", SystemParameters.SystemParams().EmailAttachementsFilePath, item.FileName);

                        Context.Set_EduspecService_LegalProcessReportGenerated(item.LegalProcessID, true, null);

                        Logging.LogEntry(LogLevel.Info, String.Format("Save Report to file : {0}.", resultFilePath));
                        report.ExportToPdf(resultFilePath);
                        Logging.LogEntry(LogLevel.Info, "Report Saved.");

                    }
                    catch (Exception error)
                    {
                        Logging.LogEntry(LogLevel.Info, string.Format("Generating Legal Process report failed - message [{0}]", error.ToString()));
                        Context.Set_EduspecService_LegalProcessReportGenerated(item.LegalProcessID, false, null);
                    }

                }

                return null;

            }
        }


    }         
}