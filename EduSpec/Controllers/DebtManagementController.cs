using DevExpress.Web;
using DevExpress.Web.Mvc;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using EduSpec.Code;
using EduSpec.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using WebMatrix.WebData;
using DataTable = System.Data.DataTable;

namespace EduSpec.Controllers
{
    [SessionExpire]
    public class DebtManagementController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public static string AgeAnalysisFilePath = SystemParameters.SystemParams().AgeAnalysisFilePath;
        public static string[] Dateformats = { "dd/MM/yyyy", "dd/MM/yy" };

        private class PastelCategory
        {
            public int AutoID { get; set; }
            public int CategoryID { get; set; }
            public string Description { get; set; }
            public bool IsImport { get; set; }
        }
        public class DropdownList
        {
            public int ID { get; set; }
            public string FamilyCode { get; set; }
            public string Surname { get; set; }
            public string AdmissionNo { get; set; }
            public string Learner { get; set; }
        }

        #region Age Analysis

        public ViewResult AgeAnalysis()
        {
            HttpContext.Session["BulkSMSTypeID"] = 2;
            HttpContext.Session["BulkEmailTypeID"] = 2;
            HttpContext.Session["EmailKeyValueType"] = 2;
            ViewData["ViewProperties"] = ViewProperties.viewProperties("Debt Management - Age Analysis", WebSecurity.CurrentUserId);
            return View("AgeAnalysis");
        }

        public PartialViewResult AgeAnalysisPartial(int? YearID)
        {
            using (var Context = new EduSpecDataContext())
            {
                
                ViewData["ViewProperties"] = ViewProperties.viewProperties("Debt Management - Age Analysis", WebSecurity.CurrentUserId);
                return PartialView("AgeAnalysisPartial", Context.Get_DebtManagement_AgeAnalysis_View(YearID ?? Context.fn_Get_YearID(DateTime.Now), Convert.ToInt32(HttpContext.Session["InstID"])).ToList());
            }
        }

        [HttpPost]
        public ActionResult updateAgeAnalysis(Get_DebtManagement_AgeAnalysis_ViewResult AgeResult)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_DebtManagement_AgeAnalysis_Update(
                        Convert.ToInt32(HttpContext.Session["InstID"]),
                        AgeResult.FamilyID,
                        AgeResult.YearID,
                        AgeResult.PriorOutstanding,
                        AgeResult.LegalStatusID,
                        AgeResult.LegalReferenceInternal,
                        AgeResult.LegalReferenceExternal,
                        AgeResult.LegalAmount,
                        AgeResult.LegalDate,
                        AgeResult.IsDebtReview
                        );
            }
            return AgeAnalysisPartial(AgeResult.YearID);
        }



        public PartialViewResult DebtManagementHistoryPartial(int? FamilyID)
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["DebtManagementHistoryViewProperties"] = ViewProperties.viewProperties("Debt Management - Action History", WebSecurity.CurrentUserId);
                return PartialView("DebtManagementHistoryPartial", Context.Get_DebtManagement_ActionHistory_View(FamilyID).ToList());
            }
        }

        public PartialViewResult DebtManagementNote()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("Debt Management Settings", WebSecurity.CurrentUserId);
                return PartialView("DebtManagementNote");
            }
        }

        public PartialViewResult DebtManagementNotePartial(int? FamilyID, int? YearID)
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("Debt Management - Family Statement", WebSecurity.CurrentUserId);
                return PartialView("DebtManagementNotePartial", Context.Get_DebtManagement_LegalNote(FamilyID, YearID ?? Context.fn_Get_YearID(DateTime.Now)).FirstOrDefault());
            }
        }
        public PartialViewResult DebtManagementFamilyStatementPartial(int? FamilyID)
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["DebtManagementFamilyStatementViewProperties"] = ViewProperties.viewProperties("Debt Management - Family Statement", WebSecurity.CurrentUserId);
                return PartialView("DebtManagementFamilyStatementPartial", Context.Get_DebtManagement_FamilyStatement_View(FamilyID).ToList());
            }
        }

        public ActionResult CallbacksEmailFileUpload()
        {
            try
            {
                UploadControlExtension.GetUploadedFiles("ucLegalProcessFileCallbacks", UploadControlHelper.EmailFileUploadValidationSettings, UploadControlHelper.ucEmailFileCallbacks_FileUploadComplete);
            }
            catch (Exception e)
            {
                string x = e.Message;
            }

            return null;
        }

        [HttpPost]
        public ActionResult SetVariableFileUpload(string key, string value)
        {
            if (key != "")
            {
                Session[key] = value;
            }

            return this.Json(new { success = true });
        }


        public ActionResult getLetter(string returnUrl)
        {
            return Redirect(returnUrl);
        }

        public ActionResult ImportAgeAnalysis()
        {
            string FileName = UploadControlExtension.GetUploadedFiles("ucImportAge", UploadControlHelper.UploadControlValidationSettings, UploadControlHelper.AgeAnalysisImportFileFromExcel).FirstOrDefault().FileName;
            return null;
        }

        [HttpPost]
        public JsonResult importData(string FileName, int AgeMonth, int AgeImportType, bool GetPastelCategories)
        {
            if (GetPastelCategories)
            {
                ImportPastelCategories(FileName, Convert.ToInt32(HttpContext.Session["InstID"]));
            }
            else
            {
                switch (AgeImportType)
                {
                    case 1:
                        {
                            AgeImportPastelPartner(FileName, AgeMonth, Convert.ToInt32(HttpContext.Session["InstID"]));
                        }
                        break;
                    case 2:
                        {
                            AgeImportPastelPartnerWithTransactions(FileName, AgeMonth, Convert.ToInt32(HttpContext.Session["InstID"]));
                        }
                        break;
                    case 5:
                        {
                            AgeImportSagesOneWithTransactions(FileName, AgeMonth, Convert.ToInt32(HttpContext.Session["InstID"]));
                        }
                        break;
                }
            }
            
            return Json(new { Success = true });
        }

        private static void AgeImportPastelPartner(string FileName, int AgeMonth, int InstID)
        {
            DataTable AgeTable = AgeImportDataTables.Age();
            string resultFilePath = FileName;

            using (var Context = new EduSpecDataContext())
            {                
                int? ImportKey;
                using (StreamReader reader = new StreamReader(resultFilePath))
                {
                    foreach (var worksheet in Excel.Workbook.Worksheets(resultFilePath))
                    {
                        if (worksheet.Rows.Count() > 0)
                        {
                            ImportKey = Context.Set_DebtManagement_AgeAnalysis_ImportTracking(InstID, AgeMonth, WebSecurity.CurrentUserId).FirstOrDefault().ImportKey;

                            foreach (var row in worksheet.Rows)
                            {
                                if (row.Cells[8] != null)
                                {
                                    if (row.Cells[0].Text.Equals("Totals :"))
                                        break;

                                    if (row.Cells[0].Text != "" && row.Cells[8].IsAmount == true)
                                        Context.Set_DebtManagement_AgeAnalysis_ImportFromExcel(
                                            ImportKey,
                                            InstID,
                                            row.Cells[0].Text,
                                            Convert.ToDecimal(row.Cells[3].Value),
                                            Convert.ToDecimal(row.Cells[4].Value),
                                            Convert.ToDecimal(row.Cells[5].Value),
                                            Convert.ToDecimal(row.Cells[6].Value),
                                            Convert.ToDecimal(row.Cells[7].Value),
                                            Convert.ToDecimal(row.Cells[8].Value)
                                            );
                                }
                            }
                        }

                    }
                }
            }
        }

        public static decimal ZeroIfEmpty(string value)
        {
            return Convert.ToDecimal(string.IsNullOrEmpty(value.Replace(".", ",")) ? "0" : value);
        }

        private static void AgeImportPastelPartnerWithTransactions(string FileName, int AgeMonth, int InstID)
        {
            string resultFilePath = FileName;
            int ImportKey;
            string Customer = "";
            string AccountName = "";
            
            using (var Context = new EduSpecDataContext())
            {
                DataTable AgeTable = AgeImportDataTables.Age();
                DataTable AgeTrxTable = AgeImportDataTables.AgeTransactions();
                DataTable CategoriesTable = AgeImportDataTables.PastelCategories();

                List<PastelCategory> ExistingCategories = new List<PastelCategory>();
                PastelCategory PastelCategory = new PastelCategory();

                foreach (var Cat in Context.Get_DebtManagement_PastelCategories(InstID).ToList())
                {
                    ExistingCategories.Add(new PastelCategory { 
                        AutoID = Cat.AutoID, 
                        CategoryID = Cat.CategoryID, 
                        Description = Cat.Description.Trim(), 
                        IsImport = Cat.IsImport 
                    });
                }

                if (System.IO.File.Exists(resultFilePath))
                {
                    ImportKey = Context.Set_DebtManagement_AgeAnalysis_ImportTracking(InstID, AgeMonth, WebSecurity.CurrentUserId).FirstOrDefault().ImportKey ?? 0;

                    using (StreamReader file = new StreamReader(resultFilePath))
                    {
                        int counter = 0;
                        string line;
                        char delimiter = ',';

                        while ((line = file.ReadLine()) != null)
                        {
                            if (line.Trim() != "" && counter > 2)
                            {
                                if (line.Trim('"').StartsWith("Name") && line.IndexOf(delimiter) == -1)
                                {
                                    delimiter = Convert.ToChar(line.Substring(6, 1));
                                }

                                if (line.Trim('"').StartsWith("Category"))
                                {
                                    PastelCategory = SetPastelCategories(CategoriesTable, line, ExistingCategories, InstID);
                                }
                                
                                List<string> list = line.Split(delimiter).ToList();

                                if (list[0].Trim() != "" && counter > 4 && PastelCategory.IsImport)
                                {
                                    if (Customer == "")
                                    {
                                        if (line.Trim('"').StartsWith("Customer"))
                                        {
                                            string CustomerString = list[0].Substring(list[0].IndexOf(":") + 1).Trim();

                                            Customer = CustomerString.Substring(0, CustomerString.IndexOf("-")).Trim();
                                            AccountName = CustomerString.Substring(CustomerString.IndexOf("-") + 1).Replace("\"", "").Trim();
                                        }
                                    }
                                    else
                                    {
                                        DateTime dDate;
                                        if (list[0] != "")
                                        {

                                            if (DateTime.TryParseExact(list[0].Replace("\"", "").Trim(), Dateformats,
                                                CultureInfo.InvariantCulture,
                                                DateTimeStyles.None,
                                                out dDate))
                                            {
                                                AgeTrxTable.Rows.Add(
                                                    ImportKey,
                                                    InstID,
                                                    Customer,
                                                    dDate,
                                                    list[1].Replace("\"", ""),
                                                    ZeroIfEmpty(list[2]) + ZeroIfEmpty(list[3]) + ZeroIfEmpty(list[4]) + ZeroIfEmpty(list[5]) + ZeroIfEmpty(list[6])
                                                );
                                            }

                                        }

                                        if (list[0].Substring(1, 6) == "Totals")
                                        {

                                            AgeTable.Rows.Add(
                                                ImportKey,
                                                InstID,
                                                Customer,
                                                AccountName,
                                                ZeroIfEmpty(list[3]),
                                                ZeroIfEmpty(list[4]),
                                                ZeroIfEmpty(list[5]),
                                                ZeroIfEmpty(list[6]),
                                                ZeroIfEmpty(list[7]),
                                                ZeroIfEmpty(list[8]),
                                                PastelCategory.CategoryID
                                            );

                                            Customer = "";
                                        }
                                    }
                                }

                            }
                            counter++;
                        }
                        file.Close();

                        AgeImportDataTables.BulkCopyToSQLServer("AgeAnalysis_Import", AgeTable, AgeImportDataTables.AgeFields());
                        AgeImportDataTables.BulkCopyToSQLServer("AgeAnalysisTransactions_Import", AgeTrxTable, AgeImportDataTables.AgeTransactionsFields());
                        AgeImportDataTables.BulkCopyToSQLServer("AgeAnalysis_PastelCategories", CategoriesTable, AgeImportDataTables.PastelCategoriesFields());

                        Context.Set_DebtManagement_AgeAnalysis(ImportKey, InstID);

                        AgeTable.Dispose();
                        AgeTrxTable.Dispose();
                    }
                }
            }
        }
        private static void AgeImportSagesOneWithTransactions(string FileName, int AgeMonth, int InstID)
        {
            string resultFilePath = FileName;
            int ImportKey;
            string Customer = "";
            bool CanStartImport = false;
            //string AccountName = "";

            using (var Context = new EduSpecDataContext())
            {
                DataTable AgeTable = AgeImportDataTables.Age();
                DataTable AgeTrxTable = AgeImportDataTables.AgeSageOneTransactions();
                DataTable CategoriesTable = AgeImportDataTables.PastelCategories();

                List<PastelCategory> ExistingCategories = new List<PastelCategory>();
                PastelCategory PastelCategory = new PastelCategory();

                //foreach (var Cat in Context.Get_DebtManagement_PastelCategories(InstID).ToList())
                //{
                //    ExistingCategories.Add(new PastelCategory
                //    {
                //        AutoID = Cat.AutoID,
                //        CategoryID = Cat.CategoryID,
                //        Description = Cat.Description.Trim(),
                //        IsImport = Cat.IsImport
                //    });
                //}

                if (System.IO.File.Exists(resultFilePath))
                {
                    ImportKey = Context.Set_DebtManagement_AgeAnalysis_ImportTracking(InstID, AgeMonth, WebSecurity.CurrentUserId).FirstOrDefault().ImportKey ?? 0;
                    using (StreamReader file = new StreamReader(resultFilePath))
                    {
                        foreach (var worksheet in Excel.Workbook.Worksheets(resultFilePath))
                        {
                            if (worksheet.Rows.Count() > 0)
                            {                                

                                foreach (var row in worksheet.Rows)
                                {
                                    if (row.Cells[0].Text.Equals("Customer") && CanStartImport == false)
                                    {
                                        CanStartImport = true;
                                    }                                    

                                    if (row.Cells[8] != null && CanStartImport && row.Cells[0].Text.Equals("Customer") == false)
                                    {
                                        if (row.Cells[0].Text.Equals("Grand Total"))
                                            break;

                                        if (row.Cells[0].Text.StartsWith("Total") == false && Customer == "")
                                        {
                                            Customer = row.Cells[0].Text.Trim();                                            
                                        }

                                        if (row.Cells[0].Text.StartsWith("Total"))
                                        {
                                            Customer = "";
                                        }

                                        DateTime dDate;
                                        if (row.Cells[0].Text != "")
                                        {

                                            if (DateTime.TryParseExact(row.Cells[0].Text.Replace("\"", "").Trim(), Dateformats,
                                                CultureInfo.InvariantCulture,
                                                DateTimeStyles.None,
                                                out dDate))
                                            {
                                                AgeTrxTable.Rows.Add(
                                                    ImportKey,
                                                    InstID,
                                                    Customer,
                                                    dDate,
                                                    row.Cells[1].Text.Trim() + " " + row.Cells[2].Text.Trim(),
                                                    ZeroIfEmpty(row.Cells[3].Value) + ZeroIfEmpty(row.Cells[4].Value) + ZeroIfEmpty(row.Cells[5].Value) + ZeroIfEmpty(row.Cells[6].Value) + ZeroIfEmpty(row.Cells[7].Value)
                                                );
                                            }

                                        }

                                        //if (row.Cells[0].Text != "" && row.Cells[8].IsAmount == true)
                                        //    x = Convert.ToDecimal(row.Cells[3].Value);
                                            //Context.Set_DebtManagement_AgeAnalysis_ImportFromExcel(
                                            //    ImportKey,
                                            //    InstID,
                                            //    row.Cells[0].Text,
                                            //    Convert.ToDecimal(row.Cells[3].Value),
                                            //    Convert.ToDecimal(row.Cells[4].Value),
                                            //    Convert.ToDecimal(row.Cells[5].Value),
                                            //    Convert.ToDecimal(row.Cells[6].Value),
                                            //    Convert.ToDecimal(row.Cells[7].Value),
                                            //    Convert.ToDecimal(row.Cells[8].Value)
                                            //    );
                                    }
                                }
                            }

                        }                    
                
                        file.Close();

                        //AgeImportDataTables.BulkCopyToSQLServer("AgeAnalysis_Import", AgeTable, AgeImportDataTables.AgeFields());
                        AgeImportDataTables.BulkCopyToSQLServer("AgeAnalysisTransactions_SageOneImport", AgeTrxTable, AgeImportDataTables.AgeSageOneTransactionsFields());
                        //AgeImportDataTables.BulkCopyToSQLServer("AgeAnalysis_PastelCategories", CategoriesTable, AgeImportDataTables.PastelCategoriesFields());

                        Context.Set_DebtManagement_AgeAnalysis(ImportKey, InstID);

                        AgeTable.Dispose();
                        AgeTrxTable.Dispose();
                    }
                }
            }
        }

        private static void ImportPastelCategories(string FileName, int InstID)
        {
            string line;

            using (var Context = new EduSpecDataContext())
            {
                DataTable CategoriesTable = AgeImportDataTables.PastelCategories();
                List<PastelCategory> ExistingCategories = new List<PastelCategory>();

                foreach(var Cat in Context.Get_DebtManagement_PastelCategories(InstID).ToList())
                {
                    ExistingCategories.Add(new PastelCategory {
                        AutoID = Cat.AutoID, 
                        CategoryID = Cat.CategoryID, 
                        Description = Cat.Description.Trim(),
                        IsImport = Cat.IsImport
                    });                                            
                }

                if (System.IO.File.Exists(FileName))
                {
                    using (StreamReader file = new StreamReader(FileName))
                    {
                        while ((line = file.ReadLine()) != null)
                        {
                            if (line.Trim('"').StartsWith("Category"))
                            {
                                SetPastelCategories(CategoriesTable, line, ExistingCategories, InstID);
                            }
                        }
                    }
                    AgeImportDataTables.BulkCopyToSQLServer("AgeAnalysis_PastelCategories", CategoriesTable, AgeImportDataTables.PastelCategoriesFields());
                }
            }
        }

        private static PastelCategory SetPastelCategories (DataTable dataTable, string line, List<PastelCategory> existingCategories, int InstID)
        {
            int categoryID;
            string description;
            PastelCategory PastelCategory = new PastelCategory();

            int StartIndex = line.IndexOf(":") + 1;
            int Count = (line.IndexOf("-") - 1) - StartIndex;
            categoryID = Convert.ToInt32(line.Substring(StartIndex, Count).Trim());
            description = line.Substring(line.IndexOf("-") + 1).Trim('"').Trim();

            PastelCategory = existingCategories.Find(x => x.CategoryID == categoryID);
            if (existingCategories.Count > 0 && PastelCategory != null)
            {
                if (PastelCategory.Description != description)
                {
                    using (var Context = new EduSpecDataContext())
                    {
                        Context.Set_DebtManagement_PastelCategoryDescription_Update(PastelCategory.AutoID, description);
                    }
                }
            }
            else
            {
                dataTable.Rows.Add(
                InstID,
                categoryID,
                description
                );

                PastelCategory =  new PastelCategory { CategoryID = categoryID, Description = description,  IsImport =  true };
            }
            return PastelCategory;
        }

        public ActionResult FamiliesPickListPartial()
        {
            return PartialView();
        }
        public static List<DropdownList> GetFamilyList(ListEditItemsRequestedByFilterConditionEventArgs Args)
        {
            using (var Context = new EduSpecDataContext())
            {
                return Context.ExecuteQuery<DropdownList>(
                    String.Format("EXEC Get_DebtManagement_Families_PickList {0},'{1}'",
                        Convert.ToInt32(System.Web.HttpContext.Current.Session["InstID"]),
                        Args.Filter)).ToList();
            }
        }
        public static List<DropdownList> GetFamilyListByFamilyID(ListEditItemRequestedByValueEventArgs Args)
        {
            return GetFamilyListValue(Args.Value);
        }

        public static List<DropdownList> GetFamilyListValue(object Value)
        {
            if (Value != null && Value is Int32)
            {
                int FamilyID = (int)Value;
                using (var Context = new EduSpecDataContext())
                {
                    var toReturn = Context.ExecuteQuery<DropdownList>(String.Format("EXEC Get_DebtManagement_FamiliesByFamilyID_PickList {0}", FamilyID)).ToList();
                    return null;
                }
            }
            else
                return null;
        }

        [HttpPost]
        public JsonResult SetContactReasonID(int ContactReasonID)
        {
            HttpContext.Session["ContactReasonID"] = ContactReasonID;

            return Json(new { error = "None" });
        }

        [HttpPost]
        public JsonResult sendSMS(string cellnumber, string message, string FamilyID, string AgeStatusID)
        {
            SMS.Send(message, cellnumber);

            using (var Context = new EduSpecDataContext())
            {
                Context.Set_DebtManagement_ActionHistory(
                    Convert.ToInt32(HttpContext.Session["InstID"]),
                    Convert.ToInt32(FamilyID),
                    Convert.ToInt32(AgeStatusID),
                    1,
                    WebSecurity.CurrentUserId,
                    message,
                    cellnumber,
                    true,
                    null,
                    null,
                    null,
                    0,
                    null,
                    null,
                    0
                    );
            }
            return Json(new { error = "None" });
        }

        [HttpPost]
        public JsonResult SetLetterQueue(int AgeStatusID, int LetterID, int FamilyID, bool IsPrintLetterhead, bool IsBulkLetter)
        {
            try
            {
                using (var Context = new EduSpecDataContext())
                {

                    MemoryStream stream = new MemoryStream();
                    XtraReport report = ReportsController.rptAgeLetters(Convert.ToInt32(HttpContext.Session["InstID"]), Context.fn_Get_ActiveYearID().Value, IsPrintLetterhead, AgeStatusID, FamilyID, LetterID, IsBulkLetter);
                    if (report != null)
                        report.DisplayName = "Age Analysis";

                    return Json(new { Success = true });
                }
            }
            catch (Exception e)
            {
                return Json(new { Success = false, error = e.Message });
            }

        }

        public ActionResult printAgeAnalysisReport(int FamilyID, bool IsPrintLetterHead, int AgeStatusID, int LetterID, bool IsBulkLetter)
        {
            try
            {
                using (var Context = new EduSpecDataContext())
                {
                    var redirectUrl = new UrlHelper(Request.RequestContext).Action("printAgeLetters", "DebtManagement", new { FamilyID, IsPrintLetterHead, AgeStatusID, LetterID, IsBulkLetter });
                    return Json(new { Url = redirectUrl });
                }
            }
            catch (Exception e)
            {
                return Json(new { Success = false, error = e.Message });
            }
        }

        [HttpPost]
        public ActionResult AddContactDetailsNote(
            int FamilyID,
            int AgeStatusID,
            int ContactTypeID,
            string ContactName,
            int ContactReasonID,
            decimal Amount,
            DateTime? ArrangementDate,
            int NumberOfMonthsPaying,
            string Note,
            int YearID,
            string FileName
            )
        {
            using (var Context = new EduSpecDataContext())
            {
                var NewFileName = Context.Set_DebtManagement_ManualContactDetails_Add(
                                    WebSecurity.CurrentUserId,
                                    YearID,
                                    FamilyID,
                                    Convert.ToInt32(HttpContext.Session["InstID"]),
                                    AgeStatusID,
                                    ContactTypeID,
                                    ContactName,
                                    ContactReasonID,
                                    Amount,
                                    ArrangementDate,
                                    NumberOfMonthsPaying,
                                    Note,
                                    FileName
                                   ).First().FileName;

                if (NewFileName != null)
                {
                    string Path = SystemParameters.SystemParams().AgeAnalysisFilePath;
                    System.IO.File.Move(Path + FileName, Path + NewFileName);
                }
                
                return Json(new { Success = true });

            }

        }

        public ActionResult CallbacksDebtManagementNoteCustomFileUpload()
        {
            try
            {
                UploadControlExtension.GetUploadedFiles("ucNoteFile", UploadControlHelper.NoteFileUploadValidationSettings, UploadControlHelper.ucNoteFileCallbacks_FileUploadComplete);
            }
            catch (Exception e)
            {
                string x = e.Message;
            }

            return null;
        }

        //public JsonResult ChangeFileName()
        //{
        //    using (var Context = new EduSpecDataContext())
        //    {
        //        string OldFile = System.Configuration.ConfigurationManager.AppSettings["EcommImagesFilePath"] + Convert.ToString(HttpContext.Session["OldNoteAttachmentFileName"]);
        //        string NewFile = System.Configuration.ConfigurationManager.AppSettings["EcommImagesFilePath"] + Convert.ToString(HttpContext.Session["NewNoteAttachmentFileName"]);
        //        try
        //        {
        //            var existFile = (s as ASPxFileManager).SelectedFolder.GetFiles().FirstOrDefault(f => f.Name.Equals(e.FileName, StringComparison.InvariantCultureIgnoreCase));
        //            if (existFile != null)
        //                File .Delete(Path.Combine(Server.MapPath("~"), existFile.RelativeName)); // A path depends on the RootFolder in the file manager
                    
        //            UploadedFile newFile;
        //            using (var ufTemp = new UploadedFile(OldFile))
        //            {
        //                Image = new Bitmap(bmpTemp);
        //                Image.Save(NewFilePath);
        //            }
        //            if (File.Exists(OldFile))
        //            {
        //                File.Delete(OldFilePath);
        //            }

        //            return Json(new { result = true, error = string.Empty, NewFileName });
        //        }
        //        catch (Exception ex)
        //        {
        //            return Json(new { result = true, error = ex.Message, NewFileName });
        //        }
        //    }
        //}

        [HttpPost]
        public JsonResult SetLegalProcess(string AgeList, int DebtCollectorID)
        {
            using (var Context = new EduSpecDataContext() { CommandTimeout = 5 * 60 })
            {
                Context.Set_DebtManagement_SetLegalProcess_Update(AgeList, DebtCollectorID, Convert.ToInt32(HttpContext.Session["InstID"]), WebSecurity.CurrentUserId);
            }
            return Json(new { result = "Success" });
        }

        public JsonResult PrintParentAccountHistoryReport(int FamilyID)
        {
            using (var Context = new EduSpecDataContext())
            {
                var redirectUrl = new UrlHelper(Request.RequestContext).Action("PrintParentAccountHistory", "DebtManagement", new { FamilyID });
                return Json(new { Url = redirectUrl });
            }
        }

        public PartialViewResult DebtManagementGenerateRegisteredEmails()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["EmailSection41LettersViewProperties"] = ViewProperties.viewProperties("Debt Management - Email section 41 letters", WebSecurity.CurrentUserId);
                return PartialView("DebtManagementGenerateRegisteredEmails", Context.Get_DebtManagement_ParentsForRegisteredEmails_View(Convert.ToInt32(HttpContext.Session["InstID"])).ToList());
            }
        }

        [HttpPost]
        public ActionResult updateParentsForRegisteredEmails(Get_DebtManagement_ParentsForRegisteredEmails_ViewResult Parent)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_DebtManagement_ParentsForRegisteredEmails_Update(
                    Parent.ParentID,
                    Parent.Email
                        );
            }
            return DebtManagementGenerateRegisteredEmails();
        }

        [HttpPost]
        public JsonResult SetSection41Letters(string ParentIDList, int InstEmailID, int InstLetterID, string EmailSubject)
        {
            using (var Context = new EduSpecDataContext() { CommandTimeout = 5 * 60 })
            {
                Context.Set_DebtManagement_SetRegisteredEmail_Add(ParentIDList, InstEmailID, InstLetterID, EmailSubject, Convert.ToInt32(HttpContext.Session["InstID"]), WebSecurity.CurrentUserId);
            }
            return Json(new { result = "Success" });
        }


        #endregion

        #region Age Analysis Import

        public ViewResult AgeAnalysisImport()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("Legal - Age Analysis Import Tracking", WebSecurity.CurrentUserId);
                return View("AgeAnalysisImport");
            }
        }

        public PartialViewResult AgeAnalysisImportPartial()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("Legal - Age Analysis Import Tracking", WebSecurity.CurrentUserId);
                return PartialView("AgeAnalysisImportPartial", Context.Get_DebtManagement_AgeAnalysis_ImportTracking_View(Convert.ToInt32(HttpContext.Session["InstID"])).ToList());
            }
        }

        public PartialViewResult AgeAnalysisImportDetailPartial(int? ImportKey)
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["AgeAnalysisImportDetailViewProperties"] = ViewProperties.viewProperties("Legal - Age Analysis Import Detail", WebSecurity.CurrentUserId);
                return PartialView("AgeAnalysisImportDetailPartial", Context.Get_DebtManagement_AgeAnalysis_Import_View(ImportKey).ToList());
            }
        }

        [HttpPost]
        public ActionResult LinkEduSpecFamily(Get_DebtManagement_AgeAnalysis_Import_ViewResult AgeResult)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_DebtManagement_LinkEduSpecFamily(
                    AgeResult.ImportID,
                    AgeResult.ImportKey,
                    Convert.ToInt32(HttpContext.Session["InstID"]),
                    AgeResult.FamilyID,
                    AgeResult.AccountCode
                    );
            }
            return AgeAnalysisImportDetailPartial(AgeResult.ImportKey);
        }

        public PartialViewResult DebtManagementPastelCategoriesPartial()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["DebtManagementPastelCategoriesViewProperties"] = ViewProperties.viewProperties("Debt Management - Pastel Categories", WebSecurity.CurrentUserId);
                return PartialView("DebtManagementPastelCategoriesPartial", Context.Get_DebtManagement_PastelCategories_View(Convert.ToInt32(HttpContext.Session["InstID"])).ToList());
            }            
        }

        public ActionResult BatchEditingUpdateModel(MVCxGridViewBatchUpdateValues<Get_DebtManagement_PastelCategories_ViewResult> updateValues)
        {
            using (var Context = new EduSpecDataContext())
            {
                foreach (var product in updateValues.Update)
                {
                    if (updateValues.IsValid(product))
                        Context.Set_DebtManagement_PastelCategories(product.AutoID, Convert.ToInt32(HttpContext.Session["InstID"]), product.IsImport);
                }
            }
            return DebtManagementPastelCategoriesPartial();
        }

        [HttpPost]
        public JsonResult SetAgeAnalysisFromImport(int ImportKey)
        {
            using (var Context = new EduSpecDataContext() { CommandTimeout = 5 * 60 })
            {
                Context.Set_DebtManagement_AgeAnalysisFromImport(ImportKey, Convert.ToInt32(HttpContext.Session["InstID"]));
            }
            return Json(new { Success = true });
        }

        [HttpPost]
        public JsonResult SetAgeAnalysisImportDelete(int ImportKey)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_DebtManagement_AgeAnalysis_ImportTracking_Delete(Convert.ToInt32(HttpContext.Session["InstID"]), ImportKey, WebSecurity.CurrentUserId);
            }
            return Json(new { Success = true });
        }

        #endregion

        #region Registered Emails

        public ViewResult RegisteredEmails()
        {
            ViewData["ViewProperties"] = ViewProperties.viewProperties("Debt Management - Registered Emails", WebSecurity.CurrentUserId);
            return View("RegisteredEmails");
        }
        public PartialViewResult RegisteredEmailsPartial(int? YearID)
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("Debt Management - Registered Emails", WebSecurity.CurrentUserId);
                return PartialView("RegisteredEmailsPartial", Context.Get_DebtManagement_RegisteredMails_View(Convert.ToInt32(HttpContext.Session["InstID"]),YearID ?? Context.fn_Get_YearID(DateTime.Now),1).ToList());
            }
        }
        public ActionResult getCertificate(string returnUrl)
        {
            return Redirect(returnUrl);
        }
        #endregion

        #region Follow Ups

        public ViewResult FollowUps()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("Legal - Follow Ups", WebSecurity.CurrentUserId);
                return View("FollowUps");
            }
        }

        public PartialViewResult FollowUpsPartial()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("Legal - Follow Ups", WebSecurity.CurrentUserId);
                return PartialView("FollowUpsPartial", Context.Get_DebtManagement_FollowUps_View(Convert.ToInt32(HttpContext.Session["InstID"])).ToList());
            }
        }

        [HttpPost]
        public ActionResult FollowUpsUpdate(Get_DebtManagement_FollowUps_ViewResult FollowUps)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_DebtManagement_FollowUps_Update(
                        FollowUps.ArrangementID,
                        FollowUps.FollowUpsID,
                        FollowUps.ContactReasonID,
                        FollowUps.ResolveFollowUpReasonID,
                        FollowUps.ResolveReasonNote,
                        WebSecurity.CurrentUserId
                    );

                return FollowUpsPartial();
            }
        }

        #endregion

        #region Settings 

        public ViewResult DebtManagementSettings()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("Debt Management Settings", WebSecurity.CurrentUserId);
                return View("DebtManagementSettings");
            }
        }

        public PartialViewResult DebtManagementSettingsGeneral()
        {
            using (var Context = new EduSpecDataContext())
            {
                return PartialView("DebtManagementSettingsGeneral", Context.Get_DebtManagementSettings_General_View(Convert.ToInt32(HttpContext.Session["InstID"])).FirstOrDefault());
            }
        }

        public PartialViewResult DebtManagementSettingsLetters()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["LettersViewProperties"] = ViewProperties.viewProperties("Debt Management Settings - Letters", WebSecurity.CurrentUserId);
                return PartialView("DebtManagementSettingsLetters", Context.Get_DebtManagementSettings_Letters_View(Convert.ToInt32(HttpContext.Session["InstID"])).ToList());
            }
        }

        public PartialViewResult LetterEditor_ENG(int? LetterID)
        {
            using (var Context = new EduSpecDataContext())
            {
                return PartialView("LetterEditor_ENG", Context.Get_DebtManagementSettings_LetterToEdit_View(LetterID).FirstOrDefault());
            }
        }

        public PartialViewResult LetterEditor_AFR(int? LetterID)
        {
            using (var Context = new EduSpecDataContext())
            {
                return PartialView("LetterEditor_AFR", Context.Get_DebtManagementSettings_LetterToEdit_View(LetterID).FirstOrDefault());
            }
        }

        [HttpPost]
        public ActionResult SetDebtManagementSettingsGeneralSettings(Get_DebtManagementSettings_General_ViewResult GeneralSettings)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_DebtManagementSettings_GeneralSettings_Update(
                    Convert.ToInt32(HttpContext.Session["InstID"]),
                    GeneralSettings.BillingCycleTypeID,
                    GeneralSettings.AgeImportTypeID
                    );
                return DebtManagementSettings();
            }
        }

        [HttpPost]
        public ActionResult LettersUpdate(Get_DebtManagementSettings_Letters_ViewResult Letters)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_DebtManagementSettings_LettersEdit_Update(
                    Letters.InstLetterID,
                    Letters.Body_ENG,
                    Letters.Body_AFR,
                    Letters.IsPrintLetterhead
                    );
                return DebtManagementSettingsLetters();
            }
        }

		[HttpPost]
        public ActionResult LettersAdd(Get_DebtManagementSettings_Letters_ViewResult Letters)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_DebtManagementSettings_LettersEdit_Add(
                    Convert.ToInt32(HttpContext.Session["InstID"]),
                    Letters.Description,
                    Letters.Body_ENG,
                    Letters.Body_AFR,
                    Letters.IsPrintLetterhead
                    );
                return DebtManagementSettingsLetters();
            }
        }
																				 
        public PartialViewResult DebtManagementSettingsSMSMessages()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["SMSViewProperties"] = ViewProperties.viewProperties("Debt Management - Debt Management Settings - SMSMessages", WebSecurity.CurrentUserId);
                return PartialView("DebtManagementSettingsSMSMessages", Context.Get_DebtManagementSettings_SMSMessage_View(Convert.ToInt32(HttpContext.Session["InstID"])).ToList());
            }
        }

        public PartialViewResult MessageEditorLegal_ENG(int InstSMSID)
        {
            using (var Context = new EduSpecDataContext())
            {
                return PartialView("MessageEditorLegal_ENG", Context.Get_DebtManagementSettings_SMSMessageToEdit_View(InstSMSID).FirstOrDefault());
            }
        }

        public PartialViewResult MessageEditorLegal_AFR(int InstSMSID)
        {
            using (var Context = new EduSpecDataContext())
            {
                return PartialView("MessageEditorLegal_AFR", Context.Get_DebtManagementSettings_SMSMessageToEdit_View(InstSMSID).FirstOrDefault());
            }
        }

        [HttpPost]
        public ActionResult SMSMessageUpdate(Get_DebtManagementSettings_SMSMessage_ViewResult SMS)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_DebtManagementSettings_SMSMessage_Update(
                    SMS.InstSMSID,
                    SMS.Description,
                    SMS.Message_ENG,
                    SMS.Message_AFR,
                    SMS.IsActive,
                    WebSecurity.CurrentUserId
                    );
                return DebtManagementSettingsSMSMessages();
            }
        }

        [HttpPost]
        public ActionResult SMSMessageAdd(Get_DebtManagementSettings_SMSMessage_ViewResult SMS)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_DebtManagementSettings_SMSMessage_Add(
                    Convert.ToInt32(HttpContext.Session["InstID"]),
                    SMS.Description,
                    SMS.Message_ENG,
                    SMS.Message_AFR,
                    WebSecurity.CurrentUserId,
                    true
                    );
                return DebtManagementSettingsSMSMessages();
            }
        }

        public PartialViewResult DebtManagementSettingsEmails()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["EmailsViewProperties"] = ViewProperties.viewProperties("Debt Management - Debt Management Settings - Emails", WebSecurity.CurrentUserId);
                return PartialView("DebtManagementSettingsEmails", Context.Get_DebtManagementSettings_Emails_View(Convert.ToInt32(HttpContext.Session["InstID"])).ToList());
            }
        }

        public PartialViewResult EmailEditor_ENG(int? InstEmailID)
        {
            using (var Context = new EduSpecDataContext())
            {
                return PartialView("EmailEditor_ENG", Context.Get_DebtManagementSettings_EmailToEdit_View(InstEmailID).FirstOrDefault());
            }
        }

        public PartialViewResult EmailEditor_AFR(int? InstEmailID)
        {
            using (var Context = new EduSpecDataContext())
            {
                return PartialView("EmailEditor_AFR", Context.Get_DebtManagementSettings_EmailToEdit_View(InstEmailID).FirstOrDefault());
            }
        }

        [HttpPost]
        public ActionResult EmailsUpdate(Get_DebtManagementSettings_Emails_ViewResult Email)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_DebtManagementSettings_Email_Update(
                    Email.InstEmailID,
                    Email.EmailBody_ENG,
                    Email.EmailBody_AFR,
                    Email.Description,
                    Email.IsActive,
                    WebSecurity.CurrentUserId
                    );
                return DebtManagementSettingsEmails();
            }
        }

        [HttpPost]
        public ActionResult EmailsAdd(Get_DebtManagementSettings_Emails_ViewResult Email)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_DebtManagementSettings_Email_Add(
                    Convert.ToInt32(HttpContext.Session["InstID"]),
                    2, //Legal
                    Email.Description,
                    Email.EmailBody_ENG,
                    Email.EmailBody_AFR,
                    true
                    );
                return DebtManagementSettingsEmails();
            }
        }

        public PartialViewResult DebtManagementSettingsDebtCollectors()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["DebtCollectorsViewProperties"] = ViewProperties.viewProperties("Debt Management - Debt Management Settings - DebtCollectors", WebSecurity.CurrentUserId);
                return PartialView("DebtManagementSettingsDebtCollectors", Context.Get_DebtManagementSettings_DebtCollectors_View(Convert.ToInt32(HttpContext.Session["InstID"])).ToList());
            }
        }
        [AllowAnonymous]
        public ActionResult DebtCollectorsPicklist()
        {
            return PartialView("DebtCollectorsPicklist");
        }
        [HttpPost]
        public ActionResult DebtCollectorAdd(Get_DebtManagementSettings_DebtCollectors_ViewResult Debt)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_DebtManagementSettings_DebtCollectors_Add(
                    Debt.DebtCollectorID,
                    Debt.CollectorName,
                    Debt.PhysAddress1,
                    Debt.PhysAddress2,
                    Debt.PhysAddress3,
                    Debt.PhysProvinceID,
                    Debt.PhysCode,
                    Debt.PosAddress1,
                    Debt.PosAddress2,
                    Debt.PosAddress3,
                    Debt.PosProvinceID,
                    Debt.PosCode,
                    Debt.Telephone,
                    Debt.Cell,
                    Debt.Fax,
                    Debt.Email,
                    Debt.ContactName,
                    Debt.ContactCell,
                    Debt.ContactEmail,
                    Convert.ToInt32(HttpContext.Session["InstID"])
                    );
                return DebtManagementSettingsDebtCollectors();
            }
        }
        [HttpPost]
        public ActionResult DebtCollectorUpdate(Get_DebtManagementSettings_DebtCollectors_ViewResult Debt)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_DebtManagementSettings_DebtCollectors_Update(
                    Debt.DebtCollectorID,
                    Debt.CollectorName,
                    Debt.PhysAddress1,
                    Debt.PhysAddress2,
                    Debt.PhysAddress3,
                    Debt.PhysProvinceID,
                    Debt.PhysCode,
                    Debt.PosAddress1,
                    Debt.PosAddress2,
                    Debt.PosAddress3,
                    Debt.PosProvinceID,
                    Debt.PosCode,
                    Debt.Telephone,
                    Debt.Cell,
                    Debt.Fax,
                    Debt.Email,
                    Debt.ContactName,
                    Debt.ContactCell,
                    Debt.ContactEmail,
                    Convert.ToInt32(HttpContext.Session["InstID"]),
                    Debt.IsCustom
                    );
                return DebtManagementSettingsDebtCollectors();
            }
        }
        public PartialViewResult StartLegalAction(int? IntegrationStatus)
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["LegalActionViewProperties"] = ViewProperties.viewProperties("Debt Management - Start Legal Action", WebSecurity.CurrentUserId);
                return PartialView("StartLegalAction", Context.Get_DebtManagement_StartLegalProcess_View(10, Convert.ToInt32(HttpContext.Session["InstID"])).ToList());
            }
        }
        [HttpPost]
        public JsonResult DeleteDebtCollector(int DebtCollectorID)
        {

            using (var Context = new EduSpecDataContext())
            {
                Context.Set_DebtManagementSettings_DebtCollector_Delete(
                    Convert.ToInt32(HttpContext.Session["InstID"]),
                    DebtCollectorID
                    );
            }
            return Json(new { Success = true });
        }

        #endregion

        #region Print Letters

        public ActionResult printAgeLetters(int FamilyID, bool IsPrintLetterHead, int AgeStatusID, int LetterID, bool IsBulkLetter)
        {
            using (var Context = new EduSpecDataContext())
            {

                MemoryStream stream = new MemoryStream();
                XtraReport report = ReportsController.rptAgeLetters(Convert.ToInt32(HttpContext.Session["InstID"]), Context.fn_Get_ActiveYearID().Value, IsPrintLetterHead, AgeStatusID, FamilyID, LetterID, IsBulkLetter);
                report.DisplayName = "Age Analysis";
                PdfExportOptions options = new PdfExportOptions() { ShowPrintDialogOnOpen = true };
                //report.ExportToPdf(string.Format("{0}{1}", AgeAnalysisFilePath, string.Format("{0}", FileName)));
                report.ExportToPdf(stream, options);
                stream.Seek(0, SeekOrigin.Begin);
                return new FileStreamResult(stream, "application/pdf");

            }
        }

        #endregion

        #region Reports
        public PartialViewResult reportParentAccountHistory(int FamilyID)
        {
            XtraReport report = new ParentAccountHistory();
            report.Parameters["FamilyID"].Value = FamilyID;
            report.Parameters["InstID"].Value = HttpContext.Session["InstID"];
            report.CreateDocument();
            return PartialView("reportParentAccountHistory", report);
        }
        //
        public PartialViewResult reportDebtManagementSummary(int YearID)
        {
            XtraReport report = new DebtManagementSummary();
            report.Parameters["Year"].Value = YearID;
            report.Parameters["Status"].Value = -2;
            report.Parameters["InstID"].Value = HttpContext.Session["InstID"];
            report.CreateDocument();
            return PartialView("reportDebtManagementSummary", report);
        }
        public ActionResult PrintParentAccountHistory(int FamilyID)
        {
            using (var Context = new EduSpecDataContext())
            {
                MemoryStream stream = new MemoryStream();
                XtraReport report = ReportsController.rptParentAccountHistory(FamilyID, Convert.ToInt32(HttpContext.Session["InstID"]), Convert.ToInt32(HttpContext.Session["cbYearSelectedIndex"]));
                report.DisplayName = "Parent account history";
                PdfExportOptions options = new PdfExportOptions() { ShowPrintDialogOnOpen = false };
                report.ExportToPdf(stream, options);
                stream.Seek(0, SeekOrigin.Begin);
                return new FileStreamResult(stream, "application/pdf");
            }
        }

        public PartialViewResult reportPaymentArrangements(int YearID)
        {
            XtraReport report = new PaymentArrangements();
            report.Parameters["Year"].Value = YearID;
            report.Parameters["InstID"].Value = HttpContext.Session["InstID"];
            report.CreateDocument();
            return PartialView("reportPaymentArrangements", report);
        }

        public PartialViewResult reportPaymentArrangementsFamily(int YearID, int FamilyID)
        {
            XtraReport report = new PaymentArrangementsFamily();
            report.Parameters["Year"].Value = YearID;
            report.Parameters["FamilyID"].Value = FamilyID;
            report.Parameters["InstID"].Value = HttpContext.Session["InstID"];
            report.CreateDocument();
            return PartialView("reportPaymentArrangementsFamily", report);
        }
        public PartialViewResult reportRegisteredMail(int YearID)
        {
            XtraReport report = new RegisteredEmails();
            report.Parameters["Year"].Value = YearID;
            report.Parameters["InstID"].Value = HttpContext.Session["InstID"];
            report.Parameters["DeliveryType"].Value = 1;
            report.CreateDocument();
            return PartialView("reportRegisteredMail", report);
        }

        //public ActionResult printAgeAnalysisSummaryReport(int AgeStatusID, int YearID)
        //{
        //    using (var Context = new EduSpecDataContext())
        //    {
        //        MemoryStream stream = new MemoryStream();
        //        XtraReport report = ReportsController.RptPrintAgeSummaryReport(Convert.ToInt32(HttpContext.Session["InstID"]), YearID, AgeStatusID);
        //        report.DisplayName = "Age analysis status summary";
        //        PdfExportOptions options = new PdfExportOptions() { ShowPrintDialogOnOpen = false };
        //        report.ExportToPdf(stream, options);
        //        stream.Seek(0, SeekOrigin.Begin);
        //        return new FileStreamResult(stream, "application/pdf");
        //    }
        //}

        #endregion

    }
}