using System;
using System.Linq;
using System.Web.Mvc;
using EduSpec.Models;
using WebMatrix.WebData;
using System.IO;
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting;
using NLog;
using DevExpress.Web.Mvc;
using EduSpec.Code;
using EduSpec.Reports;

namespace EduSpec.Controllers
{
    [SessionExpire]
    public class ExemptionsController : Controller
    {
        public static string PrintoutsFilePath = SystemParameters.SystemParams().PrintoutsFilePath;

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();        

        #region Families

        public ViewResult Apply()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("Exemptions - Apply", WebSecurity.CurrentUserId);
                HttpContext.Session.Add("ApplicationPackBalance", Context.fn_GetInstitutionApplicationPackBalance(Convert.ToInt32(HttpContext.Session["InstID"])));
                ViewBag.ApplicationBalance = Context.fn_GetApplicationPackBalance(Convert.ToInt32(HttpContext.Session["InstID"]));
                //var x = Context.Get_Exemptions_Apply_View(Convert.ToInt32(HttpContext.Session["InstID"]), YearID ?? Context.fn_Get_YearID(DateTime.Now)).ToList();
                return View("Apply");
            }
        }               
        public PartialViewResult ApplyPartial(int? YearID)
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("Exemptions - Apply", WebSecurity.CurrentUserId);
                //var x = Context.Get_Exemptions_Apply_View(Convert.ToInt32(HttpContext.Session["InstID"])).ToList();
                HttpContext.Session.Add("cbYearSelectedIndex", YearID);
                return PartialView("ApplyPartial", Context.Get_Exemptions_Apply_View(Convert.ToInt32(HttpContext.Session["InstID"]), YearID ?? Context.fn_Get_YearID(DateTime.Now)).ToList());
            }
        }
        public PartialViewResult LearnersPartial(int? FamilyID)
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["LearnersViewProperties"] = ViewProperties.viewProperties("Exemptions - Learners", WebSecurity.CurrentUserId);
                return PartialView("LearnersPartial", Context.Get_Exemptions_Learners_View(FamilyID).ToList());
            }
        }
        public ActionResult getApplication(string returnUrl)
        {
            return Redirect(returnUrl);
        }
        //public PartialViewResult ApplicantInfo(int? FamilyID)
        //{
        //    using (var Context = new EduSpecDataContext())
        //    {
        //        return PartialView("ApplicantInfo", Context.Get_Exemptions_ApplicantToApply_View(FamilyID).FirstOrDefault());
        //    }
        //}
        public JsonResult ApplyForExemption(Get_Exemptions_Apply_ViewResult Application, int? Wareabouts, int? Contact, int? Unwilling, bool? isAttachCovidForm)
        {
            using (var Context = new EduSpecDataContext())
            {
                int InstID = Convert.ToInt32(HttpContext.Session["InstID"]);

                if (isAttachCovidForm == null)
                    isAttachCovidForm = false;

                MappedDiagnosticsLogicalContext.Set("Institution", Context.fn_GetInstitutionName(InstID));
                logger.Info("******** New Application ********");
                logger.Info("Application for family ID : " + Application.FamilyID.ToString());
                try
                {
                    logger.Info(String.Format("Executing - EXEC Set_Exemptions_ApplyForExemption {0},{1},{2}", InstID.ToString(), Application.FamilyID.ToString(), WebSecurity.CurrentUserId.ToString()));
                    var application = Context.Set_Exemptions_ApplyForExemption(
                                            InstID, 
                                            Application.FamilyID, 
                                            WebSecurity.CurrentUserId,
                                            //Application.ExternalAccNo,
                                            Application.ApplMaritalStatusID, 
                                            Application.IsSingleApplication,
                                            Wareabouts,
                                            Contact,
                                            Unwilling,
                                            Convert.ToInt32(HttpContext.Session["cbYearSelectedIndex"]),
                                            isAttachCovidForm
                                            ).First();
                    logger.Info(String.Format("Successful executing - EXEC Set_Exemptions_ApplyForExemption {0},{1},{2}", InstID.ToString(), Application.FamilyID.ToString(), WebSecurity.CurrentUserId.ToString()));
                    logger.Info("Generate application form and getteing url.");
                    var redirectUrl = new UrlHelper(Request.RequestContext).Action(
                        "PrintApplication", 
                        "Exemptions", 
                        new {
                            application.ExemptionID,
                            application.IsPrintCoverPage,
                            application.IsPrintLetterHead,
                            application.LanguageID,
                            application.ApplMaritalStatusID,
                            application.IsSingleApplication,
                            Wareabouts,
                            Contact,
                            Unwilling,
                            isAttachCovidForm,
                            application.FileName
                        });
                    logger.Info("Return application url : " + redirectUrl);

                    return Json(new { Success = true, ExceptionMessage = "", Url = redirectUrl });
                }
                catch(Exception e)
                {
                    logger.Error(e.Message);
                    return Json(new { Success = false, ExceptionMessage = e.Message, Url = "" });
                }
                
                
            }
        }public ActionResult ApplicationBalanceInformation(string Title, string Message)
        {
            ViewBag.Title = Title;
            ViewBag.Message = Message;

            return View();
        }

        #endregion

        #region Exemptions

        public ViewResult Exemptions()
        {
            using (var Context = new EduSpecDataContext())
            {
                HttpContext.Session["BulkSMSTypeID"] = 1;
                HttpContext.Session["BulkEmailTypeID"] = 1;
                HttpContext.Session["EmailKeyValueType"] = 1;
                ViewData[ "ViewProperties"] = ViewProperties.viewProperties("Exemptions - Exemptions", WebSecurity.CurrentUserId);
                ViewBag.SMSBundleBalance = Context.fn_Get_InstitutionSMSBundleBalance(Convert.ToInt32(HttpContext.Session["InstID"]));
                HttpContext.Session.Add("EmailAttachmentFileName", null);
                HttpContext.Session.Add("SMSBundleBalanceMessage", Context.fn_Get_InstitutionSMSBundleBalanceMessage(Convert.ToInt32(HttpContext.Session["InstID"])));
                return View("Exemptions");
            }
        }

        public PartialViewResult ExemptionsPartial(int? YearID)
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("Exemptions - Exemptions", WebSecurity.CurrentUserId, string.Format("{0}", Context.fn_Get_YearDescription(YearID ?? Context.fn_Get_YearID(DateTime.Now))));
                return PartialView("ExemptionsPartial", Context.Get_Exemptions_Summary_View(YearID ?? Context.fn_Get_YearID(DateTime.Now), Convert.ToInt32(HttpContext.Session["InstID"])).ToList());
            }
        }
                
        public PartialViewResult ExemptionsApplicationHistoryPartial(int? ExemptionID)
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ApplicationHistoryViewProperties"] = ViewProperties.viewProperties("Exemptions - Application History", WebSecurity.CurrentUserId);
                return PartialView("ExemptionsApplicationHistoryPartial", Context.Get_Exemptions_ApplicationHistory_View(ExemptionID).ToList());
            }
        }

        public ActionResult CallbacksSingleEmailFileUpload()
        {
            try
            {
                UploadControlExtension.GetUploadedFiles("ucUploadData", UploadControlHelper.EmailFileUploadValidationSettings, UploadControlHelper.ucEmailFileCallbacks_FileUploadComplete);
            }
            catch (Exception e)
            {
                string x = e.Message;
            }

            return null;
        }

        public JsonResult printExemptionsReport(int YearID, string ReportFilter)
        {
            using (var Context = new EduSpecDataContext())
            {
                int InstID = Convert.ToInt32(HttpContext.Session["InstID"]);

                var redirectUrl = new UrlHelper(Request.RequestContext).Action("printExemptions", "Exemptions", new {InstID, YearID, ReportFilter });
                return Json(new { Url = redirectUrl });
            }
        }

        public JsonResult printExemptionsReportGrouped(int YearID, string ReportFilter)
        {
            using (var Context = new EduSpecDataContext())
            {
                int InstID = Convert.ToInt32(HttpContext.Session["InstID"]);

                var redirectUrl = new UrlHelper(Request.RequestContext).Action("printExemptionsSummaryGrouped", "Exemptions", new { InstID, YearID, ReportFilter });
                return Json(new { Url = redirectUrl });
            }
        }       

        [HttpPost]
        public JsonResult SendSingleSMS(string cellnumber, string message, string ExemptionID, string ExemptionStatusID, string FileName)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_ExemptionApplicationHistory(
                    Convert.ToInt32(ExemptionID),
                    Convert.ToInt32(HttpContext.Session["InstID"]),
                    Convert.ToInt32(ExemptionStatusID),
                    2,
                    WebSecurity.CurrentUserId,
                    ref FileName,
                    message,
                    cellnumber,
                    null
                    );
            }
            SMS.Send(message, cellnumber);
            return Json(new { error = "None" });
        }

        [HttpPost]
        public JsonResult SetCorrespondenceMethod(int ExemptionID, int CorrespondenceMethodID, string Message)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_Exemptions_CorrespondenceMethod_Update(
                        ExemptionID,
                        CorrespondenceMethodID,
                        Message,
                        WebSecurity.CurrentUserId
                    );
                return Json(new { Success = true });
            }
           // return Json(new { Success = true});
        }

        public ActionResult printExemptionSingleLetterReport(int ExemptionID, int FamilyID, bool IsPrintLetterHead, int ExemptionStatusID, int LetterID)
        {
            try
            {
                using (var Context = new EduSpecDataContext())
                {
                    var redirectUrl = new UrlHelper(Request.RequestContext).Action("printExemptionSingleLetter", "Exemptions", new { ExemptionID, FamilyID, IsPrintLetterHead, ExemptionStatusID, LetterID });
                    return Json(new { Url = redirectUrl });
                }
            }
            catch (Exception e)
            {
                return Json(new { Success = false, error = e.Message });
            }
        }

        public ActionResult printExemptionSingleLetter(int ExemptionID, int FamilyID, bool IsPrintLetterHead, int ExemptionStatusID, int LetterID)
        {
            using (var Context = new EduSpecDataContext())
            {
                MemoryStream stream = new MemoryStream();
                XtraReport report = ReportsController.rptExemptionSingleLetter(Convert.ToInt32(HttpContext.Session["InstID"]), ExemptionID, FamilyID, IsPrintLetterHead, ExemptionStatusID, LetterID);
                report.DisplayName = "Exemption Summary";
                PdfExportOptions options = new PdfExportOptions() { ShowPrintDialogOnOpen = true };
                //report.ExportToPdf(string.Format("{0}{1}", AgeAnalysisFilePath, string.Format("{0}", FileName)));
                report.ExportToPdf(stream, options);
                stream.Seek(0, SeekOrigin.Begin);
                return new FileStreamResult(stream, "application/pdf");
            }
        }

        public JsonResult PrintApplicationForCompensasionReport(int YearID)
        {
            using (var Context = new EduSpecDataContext())
            {
                int InstID = Convert.ToInt32(HttpContext.Session["InstID"]);
                var redirectUrl = new UrlHelper(Request.RequestContext).Action("PrintApplicationForCompensasion", "Exemptions", new { YearID, InstID });
                return Json(new { Url = redirectUrl });
            }
        }

        public JsonResult PrintLearnerClassListReport()
        {
            using (var Context = new EduSpecDataContext())
            {
                int InstID = Convert.ToInt32(HttpContext.Session["InstID"]);
                var redirectUrl = new UrlHelper(Request.RequestContext).Action("PrintLearnerClassList", "Exemptions", new { InstID });
                return Json(new { Url = redirectUrl });
            }
        }

        public JsonResult PrintExemptionProvisionalListReport(int YearID)
        {
            using (var Context = new EduSpecDataContext())
            {
                int InstID = Convert.ToInt32(HttpContext.Session["InstID"]);
                var redirectUrl = new UrlHelper(Request.RequestContext).Action("PrintExemptionProvisionalList", "Exemptions", new { InstID, YearID });
                return Json(new { Url = redirectUrl });
            }
        }

        public JsonResult PrintFinalTableForExemptionsReport(int YearID)
        {
            using (var Context = new EduSpecDataContext())
            {
                int InstID = Convert.ToInt32(HttpContext.Session["InstID"]);
                var redirectUrl = new UrlHelper(Request.RequestContext).Action("PrintFinalTableForExemptions", "Exemptions", new { InstID, YearID });
                return Json(new { Url = redirectUrl });
            }
        }

        public JsonResult PrintAffidavitReport(int ExemptionID, int LanguageID)
        {
            using (var Context = new EduSpecDataContext())
            {
                int InstID = Convert.ToInt32(HttpContext.Session["InstID"]);
                var redirectUrl = new UrlHelper(Request.RequestContext).Action("PrintAffidavit", "Exemptions", new { ExemptionID, LanguageID });
                return Json(new { Url = redirectUrl });
            }
        }

        [HttpPost]
        public JsonResult SetAffidavitSpouseQuestions(int ExemptionID, int KnowWareabouts, int HaveContact, int UnwillingToApply)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_Exemptions_AffidavitSpouseQuestions_Update(
                        ExemptionID,
                        KnowWareabouts,
                        HaveContact,
                        UnwillingToApply
                    );
                return Json(new { Success = true });
            }
        }

        [HttpPost]
        public JsonResult DeletApplication(int ExemptionID)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_Exemption_Delete(
                        ExemptionID,
                        Convert.ToInt32(HttpContext.Session["InstID"]),
                        WebSecurity.CurrentUserId
                    );
                return Json(new { Success = true });
            }
        }

        public JsonResult PrintAffidavitRefusesReport(int ExemptionID)
        {
            using (var Context = new EduSpecDataContext())
            {
                int InstID = Convert.ToInt32(HttpContext.Session["InstID"]);
                var redirectUrl = new UrlHelper(Request.RequestContext).Action("PrintAffidavitRefuses", "Exemptions", new { ExemptionID });
                return Json(new { Url = redirectUrl });
            }
        }
        public JsonResult PrintRequestToApplyReport(int FamilyID)
        {
            using (var Context = new EduSpecDataContext())
            {
                int InstID = Convert.ToInt32(HttpContext.Session["InstID"]);
                var redirectUrl = new UrlHelper(Request.RequestContext).Action("PrintRequestToApply", "Exemptions", new { FamilyID });
                return Json(new { Url = redirectUrl });
            }
        }
        public JsonResult printApplicationRegisterReport(int Qty)
        {
            using (var Context = new EduSpecDataContext())
            {
                int InstID = Convert.ToInt32(HttpContext.Session["InstID"]);
                var redirectUrl = new UrlHelper(Request.RequestContext).Action("printApplicationRegister", "Exemptions", new {InstID, Qty });
                return Json(new { Url = redirectUrl });
            }
        }


        #endregion

        #region Submit Application

        public ViewResult SubmitApplication()
        {
            using (var Context = new EduSpecDataContext())
            {
                HttpContext.Session["ProcessScreen"] = "Submit";
                ViewData["ViewProperties"] = ViewProperties.viewProperties("Exemptions - Submit Application", WebSecurity.CurrentUserId);
                return View("SubmitApplication");
            }
        }

        public PartialViewResult SubmitApplicationInfo(string ApplicationNumber)
        {
            using (var Context = new EduSpecDataContext())
            {
                HttpContext.Session["ApplicationQuestionAnswers"] = "0";
                ViewData["ViewProperties"] = ViewProperties.viewProperties("Exemptions - Submit Application", WebSecurity.CurrentUserId);
                return PartialView("SubmitApplicationInfo", Context.Get_Exemptions_ApplicationInformation(Convert.ToInt32(HttpContext.Session["InstID"]), ApplicationNumber).FirstOrDefault());
            }
        }

        public PartialViewResult AffidavitQuestions(string ApplicationNumber)
        {
            using (var Context = new EduSpecDataContext())
            {
                //ViewData["ViewProperties"] = ViewProperties.viewProperties("Exemptions - Submit Application", WebSecurity.CurrentUserId);
                return PartialView("AffidavitQuestions", Context.Get_Exemptions_AffidavitQuestions(Convert.ToInt32(HttpContext.Session["InstID"]), ApplicationNumber).FirstOrDefault());
            }
        }

        [HttpPost]
        public JsonResult Set_SaveSubmitApplication(Get_Exemptions_ApplicationInformationResult Application, bool IsSaveApplication)
        {
            using (var Context = new EduSpecDataContext())
            {
                try
                {
                    Context.Set_Exemptions_SaveApplication(
                        Application.ExemptionID,
                        Application.YearID,
                        Convert.ToInt32(HttpContext.Session["InstID"]),
                        Application.FamilyID,
                        Application.Note,
                        Application.ApplParentTypeID,
                        Application.ParentIdentityDocument,
                        Application.SpouseIdentityDocument,
                        Application.GuardianFosterIdentityDocument,
                        Application.ProofofGuardianFosterCare,
                        Application.LearnerBirthCertificate,
                        Application.ApplMaritalStatusID,
                        Application.ParentIncomeTypeID,
                        Application.SpouseIncomeTypeID,
                        Application.ParentPayslip,
                        Application.SpousePaySlip,
                        Application.DeathCertificate,
                        Application.SpouseContactDetails,
                        Application.AffidavitNoContact,
                        Application.ParentBankStatements,
                        Application.SpouseBankStatements,
                        Application.ParentRetrenchedProof,
                        Application.SpouseRetrenchedProof,
                        Application.ParentUnemploymentProof,
                        Application.SpouseUnemploymentProof,
                        Application.ParentWelfareGrand,
                        Application.SpouseWelfareGrand,
                        Application.ParentSARS,
                        Application.SpouseSARS,
                        Application.DivorceOrder,
                        Application.LearnerBirthCertificateOtherSchools,
                        Application.MarriageCertificate,
                        Application.VISA,
                        Application.IsIncomplete,
                        Application.RequireRentBond,
                        Application.RequireMunicipalAccount,
                        Application.RequireFurnitureAccounts,
                        Application.RequireVehicleFinance,
                        Application.RequireCreditCardAccount,
                        Application.RequireClothingAccount,
                        Application.RequireRevolvingCreditAccount,
                        Application.RequireTelephoneAccount,
                        Application.RequireCellPhoneAccount,
                        Application.DivorceOrder,                        
                        WebSecurity.CurrentUserId
                        );

                    if (IsSaveApplication == false)
                    {
                        var x = Context.Set_Exemptions_SubmitApplication(
                            Application.ExemptionID,
                            Convert.ToInt32(HttpContext.Session["InstID"]),
                            WebSecurity.CurrentUserId).First();

                        var redirectUrl = new UrlHelper(Request.RequestContext).Action("printGenericLetter", "Exemptions", new { Application.ExemptionID, x.IsPrintLetterHead, x.FileName});
                        return Json(new { Success = true, error = "", Url = redirectUrl });
                    }
                    else
                    {
                        return Json(new { Success = true, error = "", Url = ""});
                    }
                    
                    
                }
                catch (Exception e)
                {
                    return Json(new { Success = false, error = e.Message, Url = "" });
                }

            }

        }
        
        #endregion

        #region Process Application

        public ActionResult ProcessApplication()
        {
            using (var Context = new EduSpecDataContext())
            {
                HttpContext.Session["ProcessScreen"] = "Process";
                ViewData["ViewProperties"] = ViewProperties.viewProperties("Exemptions - Process Application", WebSecurity.CurrentUserId);
                return View("ProcessApplication");
            }
        }

        public PartialViewResult ProcessApplicationInfo(string ApplicationNumber)
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("Exemptions - Submit Application", WebSecurity.CurrentUserId);
                return PartialView("ProcessApplicationInfo", Context.Get_Exemptions_ApplicationInformation(Convert.ToInt32(HttpContext.Session["InstID"]), ApplicationNumber).FirstOrDefault());
            }
        }

        public PartialViewResult CalculationPartial(int? ExemptionID)
        {
            using (var Context = new EduSpecDataContext())
            {
                return PartialView("CalculationPartial", Context.Get_Exemptions_Calculation(ExemptionID).FirstOrDefault());                
            }            
        }

        [HttpPost]
        public JsonResult Set_ApplicationCalculation(
            int ExemptionID, 
            decimal IncomeParent1, 
            decimal IncomeParent2,
            bool IsJointApplicationReceived,
            decimal Offer, 
            decimal Monetary, 
            int LearnersInOtherSchools, 
            DateTime? PaymentFromDate,
            DateTime? PaymentToDate,
            string Note,
            decimal AmountPaidToDate,
            int EffectiveFromMonth,
            bool IsCovid19)
        {
            using (var Context = new EduSpecDataContext())
            {
                var Calc = Context.Get_Exemptions_CalculateExemption(
                    Convert.ToInt32(HttpContext.Session["InstID"]),
                    IncomeParent1,
                    IncomeParent2,
                    IsJointApplicationReceived,
                    Monetary,
                    LearnersInOtherSchools,
                    ExemptionID,
                    Offer,
                    EffectiveFromMonth,
                    AmountPaidToDate,
                    IsCovid19,
                    PaymentFromDate,
                    PaymentToDate).FirstOrDefault();

                Context.Set_Exemptions_SaveExemptionCalculation(
                    ExemptionID,
                    IncomeParent1,
                    IncomeParent2,
                    IsJointApplicationReceived,
                    Calc.CombinedGrossSalary,
                    Calc.Offer,
                    Calc.PaymentAmount,
                    Monetary,
                    LearnersInOtherSchools,
                    Calc.E,
                    Calc.EActual,
                    Calc.ExceptionPercentage,
                    Calc.AmountExempted,
                    Calc.AmountPayable,
                    Calc.ExemptionTypeID,
                    PaymentFromDate,
                    PaymentToDate,
                    Calc.PaymentCount,
                    Calc.AmountPaidToDate,
                    Calc.EffectiveFromMonth,
                    Calc.IsCovid19,
                    Note);
                return Json(new { Success = true});
            }            
        } 

        [HttpPost]
        public JsonResult Set_ApplicationProcessed(
            int ExemptionID, 
            decimal IncomeParent1, 
            decimal IncomeParent2,
            bool IsJointApplicationReceived,
            decimal Offer, 
            decimal Monetary, 
            int LearnersInOtherSchools,
            DateTime? PaymentFromDate,
            DateTime? PaymentToDate,
            string Note,
            decimal AmountPaidToDate,
            int EffectiveFromMonth,
            bool IsCovid19
            )
        {
            using (var Context = new EduSpecDataContext())
            {
                try
                {
                    var Calc = Context.Get_Exemptions_CalculateExemption(
                        Convert.ToInt32(HttpContext.Session["InstID"]),
                        IncomeParent1,
                        IncomeParent2,
                        IsJointApplicationReceived,
                        Monetary,
                        LearnersInOtherSchools,
                        ExemptionID,
                        Offer,
                        EffectiveFromMonth,
                        AmountPaidToDate,
                        IsCovid19,
                        PaymentFromDate,
                        PaymentToDate).FirstOrDefault();

                    Context.Set_Exemptions_SaveExemptionCalculation(
                       ExemptionID,
                       IncomeParent1,
                       IncomeParent2,
                       IsJointApplicationReceived,
                       Calc.CombinedGrossSalary,
                       Calc.Offer,
                       Calc.PaymentAmount,
                       Monetary,
                       LearnersInOtherSchools,
                       Calc.E,
                       Calc.EActual,
                       Calc.ExceptionPercentage,
                       Calc.AmountExempted,
                       Calc.AmountPayable,
                       Calc.ExemptionTypeID,
                       PaymentFromDate,
                       PaymentToDate,
                       Calc.PaymentCount,
                       Calc.AmountPaidToDate,
                       Calc.EffectiveFromMonth,
                       Calc.IsCovid19,
                       Note);

                    var y = Context.Set_Exemptions_ApplicationProcessed(
                        Convert.ToInt32(HttpContext.Session["InstID"]),
                        ExemptionID,
                        IncomeParent1,
                        IncomeParent2,
                        IsJointApplicationReceived,
                        Calc.CombinedGrossSalary,
                        Offer,
                        Calc.PaymentAmount,
                        Monetary,
                        LearnersInOtherSchools,
                        Calc.E,
                        Calc.EActual,
                        Calc.ExceptionPercentage,
                        Calc.AmountExempted,
                        Calc.AmountPayable,
                        AmountPaidToDate,
                        EffectiveFromMonth,
                        WebSecurity.CurrentUserId,
                        PaymentFromDate,
                        PaymentToDate,
                        Calc.PaymentCount,
                        Note,
                        IsCovid19,
                        Calc.AmountNotExempted).First();

                    return Json(new { Success = false, y.FileName });
                }
                catch (Exception e)
                {
                    return Json(new { Success = false, error = e.Message });
                }
            }
        } 

        #endregion

        #region Finalise Application

        public ViewResult FinaliseApplication()
        {
            using (var Context = new EduSpecDataContext())
            {
                HttpContext.Session["ProcessScreen"] = "Finalise";
                ViewData["ViewProperties"] = ViewProperties.viewProperties("Exemptions - Finalise Application", WebSecurity.CurrentUserId);
                return View("FinaliseApplication");
            }
        }

        public PartialViewResult FinaliseApplicationInfo(string ApplicationNumber)
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("Exemptions - Finalise Application", WebSecurity.CurrentUserId);
                var x = Context.Get_Exemptions_ApplicationInformation(Convert.ToInt32(HttpContext.Session["InstID"]), ApplicationNumber).FirstOrDefault();
                HttpContext.Session["ExemptionID"] = x.ExemptionID;
                return PartialView("FinaliseApplicationInfo", x);
            }
        }

        //public PartialViewResult FinaliseApplicationReason(int ExemptionTypeID, int FinaliseReasonTypeID)
        //{
        //    using (var Context = new EduSpecDataContext())
        //    {
        //        HttpContext.Session["ExemptionType"] = ExemptionTypeID;
        //        ViewData["ExemptionFinaliseReasonViewProperties"] = ViewProperties.viewProperties("Shared - Exemption finalise reason", WebSecurity.CurrentUserId);

        //        if(FinaliseReasonTypeID == 1)
        //        {
        //            return PartialView("FinaliseApplicationReason", Context.Get_Exemptions_FinaliseApplicationReason(Convert.ToInt32(HttpContext.Session["InstID"]), Convert.ToString(HttpContext.Session["ApplicationNumber"]), FinaliseReasonTypeID).FirstOrDefault());
        //        }
        //        else
        //        {
        //            return PartialView("FinaliseApplicationReason", Context.Get_Exemptions_FinaliseApplicationReason(Convert.ToInt32(HttpContext.Session["InstID"]), Convert.ToString(HttpContext.Session["ApplicationNumber"]), FinaliseReasonTypeID).FirstOrDefault());
        //        }
                
        //    }
        //}

        public PartialViewResult ExemptionLearners(int? ExceptionID)
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ExemptionLearnersViewProperties"] = ViewProperties.viewProperties("Exemptions - Exemption Learners", WebSecurity.CurrentUserId);
                return PartialView("ExemptionLearners", Context.Get_ExemptionLearners_View(ExceptionID).ToList());
            }
        }

        [HttpPost]
        public JsonResult Set_ApplicationFinalise(int ExemptStatusID, decimal AmountExempted, DateTime? ParentInterviewDate, DateTime? ParentInterviewStartTime, DateTime? ParentInterviewEndTime)
        {
            using (var Context = new EduSpecDataContext())
            {
                try
                {
                    TimeSpan? StartTime = null;
                    TimeSpan? EndTime = null;

                    var ExemptionID = Convert.ToInt32(HttpContext.Session["ExemptionID"]);
                    var CheckInterviewStartTime = ParentInterviewStartTime.HasValue;
                    var CheckInterviewEndTime = ParentInterviewEndTime.HasValue;

                    if (CheckInterviewStartTime != false)
                        StartTime = ParentInterviewStartTime.Value.TimeOfDay;
                
                    if (CheckInterviewEndTime != false)
                        EndTime = ParentInterviewEndTime.Value.TimeOfDay; 
                    
                    var x = Context.Set_Exemptions_ApplicationFinalise(
                            Convert.ToInt32(HttpContext.Session["InstID"]),
                            ExemptionID,
                            ExemptStatusID,
                            AmountExempted,
                            WebSecurity.CurrentUserId,
                            ParentInterviewDate,
                            StartTime,
                            EndTime
                        ).First();

                    if (x.PrintOnFinalise == true)
                    {
                        Context.Set_ExemptionLetterCorresponded(ExemptionID);
                    }


                    var redirectUrl = new UrlHelper(Request.RequestContext).Action("printGenericLetter", "Exemptions", new { ExemptionID, x.IsPrintLetterHead, x.FileName });
                    return Json(new { Success = true, error = "", Url = redirectUrl, x.PrintOnFinalise });

                }
                catch (Exception e)
                {
                    return Json(new { Success = false, error = e.Message,  });
                }
            }
        } 

        #endregion

        #region Settings

        public ViewResult ExemptionSettings()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("Exemptions - Exemption Settings", WebSecurity.CurrentUserId);
                HttpContext.Session.Add("ManagementTypeLeftID", 1);
                HttpContext.Session.Add("ManagementTypeRightID", 3);
                return View("ExemptionSettings");
            }
        }

        public PartialViewResult ExemptionSettingsRequiredDocuments()
        {
            using (var Context = new EduSpecDataContext())
            {
                return PartialView("ExemptionSettingsRequiredDocuments", Context.Get_ExemptionSettings_RequiredDocuments(Convert.ToInt32(HttpContext.Session["InstID"])).FirstOrDefault());
            }
        }

        [HttpPost]
        public ActionResult setExemptionSettingsRequiredDocuments(Get_ExemptionSettings_RequiredDocumentsResult DocumentSettings)
        {
            using (var Context = new EduSpecDataContext())
            {

                string EmployedRequiredDocs = string.Join(",",CheckBoxListExtension.GetSelectedValues<string>("EmployedRequiredDocs").ToList());
                string EmployedandWelfareGrandDocs = string.Join(",", CheckBoxListExtension.GetSelectedValues<string>("EmployedandWelfareGrandDocs").ToList());
                string SelfEmployedRequiredDocs = string.Join(",", CheckBoxListExtension.GetSelectedValues<string>("SelfEmployedRequiredDocs").ToList());
                string UnemployedRequiredDocs = string.Join(",", CheckBoxListExtension.GetSelectedValues<string>("UnemployedRequiredDocs").ToList());
                string WelfareGrandRequiredDocs = string.Join(",", CheckBoxListExtension.GetSelectedValues<string>("WelfareGrandRequiredDocs").ToList());
                string RetrenchedRequiredDocs = string.Join(",", CheckBoxListExtension.GetSelectedValues<string>("RetrenchedRequiredDocs").ToList());
                string SingleContactRequiredDocs = string.Join(",", CheckBoxListExtension.GetSelectedValues<string>("SingleContactRequiredDocs").ToList());
                string SingleNoContactRequiredDocs = string.Join(",", CheckBoxListExtension.GetSelectedValues<string>("SingleNoContactRequiredDocs").ToList());
                string MarriedRequiredDocs = string.Join(",", CheckBoxListExtension.GetSelectedValues<string>("MarriedRequiredDocs").ToList());
                string DivorcedContactRequiredDocs = string.Join(",", CheckBoxListExtension.GetSelectedValues<string>("DivorcedContactRequiredDocs").ToList());
                string DivorcedNoContactRequiredDocs = string.Join(",", CheckBoxListExtension.GetSelectedValues<string>("DivorcedNoContactRequiredDocs").ToList());
                string WidowRequiredDocs = string.Join(",", CheckBoxListExtension.GetSelectedValues<string>("WidowRequiredDocs").ToList());
                string ForeignersRequiredDocs = string.Join(",", CheckBoxListExtension.GetSelectedValues<string>("ForeignersRequiredDocs").ToList());
                string ExpenceRequiredDocs = string.Join(",", CheckBoxListExtension.GetSelectedValues<string>("ExpenceRequiredDocs").ToList());
                string LearnersRequiredDocs = string.Join(",", CheckBoxListExtension.GetSelectedValues<string>("LearnersRequiredDocs").ToList());
                string GuardianFosterRequiredDocs = string.Join(",", CheckBoxListExtension.GetSelectedValues<string>("GuardianFosterRequiredDocs").ToList());

                Context.Set_ExemptionSettings_RequiredDocuments(
                    Convert.ToInt32(HttpContext.Session["InstID"]),
                    DocumentSettings.ParentIDDescription_ENG,
                    DocumentSettings.ParentIDDescription_AFR,
                    DocumentSettings.SpouseIDDescription_ENG,
                    DocumentSettings.SpouseIDDescription_AFR,
                    DocumentSettings.GuardianFosterIDDescription_ENG,
                    DocumentSettings.GuardianFosterIDDescription_AFR,
                    DocumentSettings.ParentPayslipDescription_ENG,
                    DocumentSettings.ParentPayslipDescription_AFR,
                    DocumentSettings.SpousePayslipDescription_ENG,
                    DocumentSettings.SpousePayslipDescription_AFR,
                    DocumentSettings.DeathCertificateDescription_ENG,
                    DocumentSettings.DeathCertificateDescription_AFR,
                    DocumentSettings.ParentBankStratementsDescription_ENG,
                    DocumentSettings.ParentBankStratementsDescription_AFR,
                    DocumentSettings.SpouseBankStatementsDescription_ENG,
                    DocumentSettings.SpouseBankStatementsDescription_AFR,
                    DocumentSettings.ParentRetrenchedProofDescription_ENG,
                    DocumentSettings.ParentRetrenchedProofDescription_AFR,
                    DocumentSettings.SpouseRetrenchedProofDescription_ENG,
                    DocumentSettings.SpouseRetrenchedProofDescription_AFR,
                    DocumentSettings.ParentUnemploymentProofDescription_ENG,
                    DocumentSettings.ParentUnemploymentProofDescription_AFR,
                    DocumentSettings.SpouseUnemploymentProofDescription_ENG,
                    DocumentSettings.SpouseUnemploymentProofDescription_AFR,
                    DocumentSettings.ParentWelfareGrandProofDescription_ENG,
                    DocumentSettings.ParentWelfareGrandProofDescription_AFR,
                    DocumentSettings.SpouseWelfareGrandDescription_ENG,
                    DocumentSettings.SpouseWelfareGrandDescription_AFR,
                    DocumentSettings.ParentSARSDescription_ENG,
                    DocumentSettings.ParentSARSDescription_AFR,
                    DocumentSettings.SpouseSARSDescription_ENG,
                    DocumentSettings.SpouseSARSDescription_AFR,
                    DocumentSettings.RentBondDescription_ENG,
                    DocumentSettings.RentBondDescription_AFR,
                    DocumentSettings.MunicipalAccountDescription_ENG,
                    DocumentSettings.MunicipalAccountDescription_AFR,
                    DocumentSettings.FurnitureAccountsDescription_ENG,
                    DocumentSettings.FurnitureAccountsDescription_AFR,
                    DocumentSettings.VehicleFinanceDescription_ENG,
                    DocumentSettings.VehicleFinanceDescription_AFR,
                    DocumentSettings.CreditCardAccountDescription_ENG,
                    DocumentSettings.CreditCardAccountDescription_AFR,
                    DocumentSettings.ClothingAccountDescription_ENG,
                    DocumentSettings.ClothingAccountDescription_AFR,
                    DocumentSettings.RevolvingCreditAccountDescription_ENG,
                    DocumentSettings.RevolvingCreditAccountDescription_AFR,
                    DocumentSettings.TelephoneAccountDescription_ENG,
                    DocumentSettings.TelephoneAccountDescription_AFR,
                    DocumentSettings.CellPhoneAccountDescription_ENG,
                    DocumentSettings.CellPhoneAccountDescription_AFR,
                    DocumentSettings.ProofofGuardianFosterCareDescription_ENG,
                    DocumentSettings.ProofofGuardianFosterCareDescription_AFR,
                    DocumentSettings.LearnerBirthCertificateDescription_ENG,
                    DocumentSettings.LearnerBirthCertificateDescription_AFR,
                    DocumentSettings.LearnerBirthCertificateOtherSchoolsDescription_AFR,
                    DocumentSettings.LearnerBirthCertificateOtherSchoolsDescription_ENG,
                    DocumentSettings.DivorceOrderDecription_ENG,
                    DocumentSettings.DivorceOrderDecription_AFR,
                    DocumentSettings.SpouseContactDescription_ENG,
                    DocumentSettings.SpouseContactDescription_AFR,
                    DocumentSettings.AffidavitNoContactDescription_ENG,
                    DocumentSettings.AffidavitNoContactDescription_AFR,
                    DocumentSettings.MarriageCertificateDescription_ENG,
                    DocumentSettings.MarriageCertificateDescription_AFR,
                    DocumentSettings.VISADescription_ENG,
                    DocumentSettings.VISADescription_AFR,
                    EmployedRequiredDocs,
                    EmployedandWelfareGrandDocs,
                    SelfEmployedRequiredDocs,
                    UnemployedRequiredDocs,
                    WelfareGrandRequiredDocs,
                    RetrenchedRequiredDocs,
                    SingleContactRequiredDocs,
                    SingleNoContactRequiredDocs,
                    MarriedRequiredDocs,
                    DivorcedContactRequiredDocs,
                    DivorcedNoContactRequiredDocs,
                    WidowRequiredDocs,
                    ForeignersRequiredDocs,
                    ExpenceRequiredDocs,
                    LearnersRequiredDocs,
                    GuardianFosterRequiredDocs
                    );
                
                return ExemptionSettings();
            }
        }

        public PartialViewResult ExemptionSettingsGeneralSettings()
        {
            using (var Context = new EduSpecDataContext())
            {
                return PartialView("ExemptionSettingsGeneralSettings", Context.Get_ExemptionSettings_GeneralSettings(Convert.ToInt32(HttpContext.Session["InstID"])).FirstOrDefault());
            }
        }

        [HttpPost]
        public ActionResult SetExemptionSettingsGeneralSettings(Get_ExemptionSettings_GeneralSettingsResult GeneralSettings)
        {
            using (var Context = new EduSpecDataContext())
            {   
                Context.Set_ExemptionSettings_GeneralSettings_Update(
                    Convert.ToInt32(HttpContext.Session["InstID"]),
                    GeneralSettings.PrincipalTitleID,
                    GeneralSettings.Principal,
                    GeneralSettings.GoverningBodyTitleID,
                    GeneralSettings.GoverningBody,
                    GeneralSettings.GoverningFinanceTitleID,
                    GeneralSettings.GoverningFinance,
                    GeneralSettings.FinancialSecretaryTitleID,
                    GeneralSettings.FinancialSecretary,
                    GeneralSettings.BursarTitleID,
                    GeneralSettings.Bursar,
                    RadioButtonListExtension.GetValue<Int32>("CalculationLetterLeftPerson"),
                    RadioButtonListExtension.GetValue<Int32>("CalculationLetterRightPerson")
                    );
                return ExemptionSettings();
            }
        }

        public PartialViewResult ExemptionSettingsAnnualParam()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["AnnualParamViewProperties"] = ViewProperties.viewProperties("Exemptions - Exemption Settings - Annual Parameters", WebSecurity.CurrentUserId);
                return PartialView("ExemptionSettingsAnnualParam", Context.Get_ExemptionSettings_AnnualParameters_View(Convert.ToInt32(HttpContext.Session["InstID"])).ToList());
            }
        }

        [HttpPost]
        public ActionResult AnnualParametersUpdate(Get_ExemptionSettings_AnnualParameters_ViewResult AnnualParam)
        {
            using (var Context = new EduSpecDataContext())
            {                
                Context.Set_ExemptionSettings_AnnualParameters_Update(
                    Convert.ToInt32(HttpContext.Session["InstID"]),
                    AnnualParam.YearID,
                    AnnualParam.AllocationPerLearner,
                    AnnualParam.LearnerEnrolement,
                    AnnualParam.NoOfSchoolDays,
                    AnnualParam.TotalAllocation,
                    AnnualParam.ApplicationReturnDate,
                    AnnualParam.ApplicationResultDate,
                    AnnualParam.ResolutionLetterDate,
                    AnnualParam.BudgetMeetingDate
                    );
                return ExemptionSettingsAnnualParam();
            }
        }
        
        
        public PartialViewResult ExemptionSettingsLetters()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["LettersViewProperties"] = ViewProperties.viewProperties("Exemptions - Exemption Settings - Letters", WebSecurity.CurrentUserId);
                return PartialView("ExemptionSettingsLetters", Context.Get_ExemptionSettings_Letters_View(Convert.ToInt32(HttpContext.Session["InstID"])).ToList());
            }
        }

        public PartialViewResult LetterEditor_ENG(int? LetterID)
        {
            using (var Context = new EduSpecDataContext())
            {
                return PartialView("LetterEditor_ENG", Context.Get_ExemptionSettings_LetterToEdit_View(LetterID).FirstOrDefault());
            }
        }

        public PartialViewResult LetterEditor_AFR(int? LetterID)
        {
            using (var Context = new EduSpecDataContext())
            {
                return PartialView("LetterEditor_AFR", Context.Get_ExemptionSettings_LetterToEdit_View(LetterID).FirstOrDefault());
            }
        }

        [HttpPost]
        public ActionResult LettersUpdate(Get_ExemptionSettings_Letters_ViewResult Letters)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_ExemptionSettings_Letters_Update(
                    Letters.InstLetterID,
                    Letters.Body_ENG,
                    Letters.Body_AFR,
                    Letters.IsPrintCoverPage,
                    Letters.IsPrintLetterHead                    
                    );
                return ExemptionSettingsLetters();
            }
        }

[HttpPost]
        public ActionResult LettersAdd(Get_ExemptionSettings_Letters_ViewResult Letters)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_ExemptionSettings_Letters_Add(
                    Convert.ToInt32(HttpContext.Session["InstID"]),
                    Letters.Description,
                    Letters.CategoryID,
                    Letters.Body_ENG,
                    Letters.Body_AFR,
                    Letters.IsPrintCoverPage,
                    Letters.IsPrintLetterHead
                    );
                return ExemptionSettingsLetters();
            }
        }
        public PartialViewResult ExemptionSettingsSMS()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["SMSViewProperties"] = ViewProperties.viewProperties("Exemptions - Exemption Settings - SMS", WebSecurity.CurrentUserId);
                return PartialView("ExemptionSettingsSMS", Context.Get_ExemptionSettings_SMSMessages_View(Convert.ToInt32(HttpContext.Session["InstID"])).ToList());
            }
        }

        public PartialViewResult ExemptionSettingsEmails()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["EmailsViewProperties"] = ViewProperties.viewProperties("Exemptions - Exemption Settings - Emails", WebSecurity.CurrentUserId);
                return PartialView("ExemptionSettingsEmails", Context.Get_ExemptionSettings_EmailMessages_View(Convert.ToInt32(HttpContext.Session["InstID"])).ToList());
            }
        }


        public PartialViewResult MessageEditorExemptions_ENG(int? InstSMSID)
        {
            using (var Context = new EduSpecDataContext())
            {
                return PartialView("MessageEditorExemptions_ENG", Context.Get_ExemptionSettings_SMSMessageToEdit_View(InstSMSID).FirstOrDefault());
            }
        }

        public PartialViewResult MessageEditorExemptions_AFR(int? InstSMSID)
        {
            using (var Context = new EduSpecDataContext())
            {
                return PartialView("MessageEditorExemptions_AFR", Context.Get_ExemptionSettings_SMSMessageToEdit_View(InstSMSID).FirstOrDefault());
            }
        }

        [HttpPost]
        public ActionResult SMSMessageUpdate(Get_ExemptionSettings_SMSMessages_ViewResult SMS)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_ExemptionSettings_SMSMessage_Update(
                    SMS.InstSMSID,
                    SMS.Description,
                    SMS.Message_ENG,
                    SMS.Message_AFR,
                    SMS.IsActive,
                    WebSecurity.CurrentUserId
                    );
                return ExemptionSettingsSMS();
            }
        }

        [HttpPost]
        public ActionResult SMSMessageAdd(Get_ExemptionSettings_SMSMessages_ViewResult SMS)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_ExemptionSettings_SMSMessage_Add(
                    Convert.ToInt32(HttpContext.Session["InstID"]),
                    SMS.Description,
                    SMS.Message_ENG,
                    SMS.Message_AFR,
                    WebSecurity.CurrentUserId,
                    true
                    );
                return ExemptionSettingsSMS();
            }
        }

        public PartialViewResult EmailEditorExemptions_ENG(int? InstEmailID)
        {
            using (var Context = new EduSpecDataContext())
            {
                return PartialView("EmailEditorExemptions_ENG", Context.Get_ExemptionSettings_EmailMessageToEdit_View(InstEmailID).FirstOrDefault());
            }
        }

        public PartialViewResult EmailEditorExemptions_AFR(int? InstEmailID)
        {
            using (var Context = new EduSpecDataContext())
            {
                return PartialView("EmailEditorExemptions_AFR", Context.Get_ExemptionSettings_EmailMessageToEdit_View(InstEmailID).FirstOrDefault());
            }
        }

        [HttpPost]
        public ActionResult EmailMessageUpdate(Get_ExemptionSettings_EmailMessages_ViewResult Email)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_ExemptionSettings_EmailMessage_Update(
                    Email.InstEmailID,
                    Email.Description,
                    Email.EmailBody_ENG,
                    Email.EmailBody_AFR,
                    WebSecurity.CurrentUserId
                    );
                return ExemptionSettingsEmails();
            }
        }

        [HttpPost]
        public ActionResult EmailMessageAdd(Get_ExemptionSettings_EmailMessages_ViewResult Email)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_ExemptionSettings_EmailMessage_Add(
                    Convert.ToInt32(HttpContext.Session["InstID"]),
                    Email.Description,
                    Email.EmailBody_ENG,
                    Email.EmailBody_AFR,
                    WebSecurity.CurrentUserId,
                    true
                    );
                return ExemptionSettingsEmails();
            }
        }



        #endregion

        #region Application

        public JsonResult FindApplication(string ApplicationNumber)
        {
            using (var Context = new EduSpecDataContext())
            {
                HttpContext.Session["ApplicationNumber"] = ApplicationNumber;
                int ExemptionID = Context.fn_GetExemptionID(Convert.ToInt32(HttpContext.Session["InstID"]), ApplicationNumber).Value;
                return Json(new { ExemptionID, Screen = HttpContext.Session["ProcessScreen"] });
            }

                                                                    
        }

        #endregion

        #region Reports

        public PartialViewResult ReportExemptionCalculation(int ExemptionID, string FileName)
        {
            XtraReport report = new ExemptionCalculation();
            report = ReportsController.RptExemptionCalculation(ExemptionID);
            report.DisplayName = "Exemption Calculation";
            PdfExportOptions options = new PdfExportOptions() { ShowPrintDialogOnOpen = false };
            report.ExportToPdf(String.Format("{0}{1}", SystemParameters.SystemParams().PrintoutsFilePath, String.Format("{0}", FileName)),options);

            return PartialView("ReportExemptionCalculation", report);
        }
        public  PartialViewResult reportCompensation(int YearID)
        {
            XtraReport report = new ExemptionCompensation();
            report.Parameters["Year"].Value = YearID;
            report.Parameters["UserID"].Value = WebSecurity.CurrentUserId;
            report.Parameters["InstID"].Value = HttpContext.Session["InstID"];
            report.CreateDocument();
            return PartialView("reportCompensation", report);
        }

        public PartialViewResult reportCompensation_FreeState(int YearID)
        {
            XtraReport report = new ExemptionCompensation_FreeSate();
            report.Parameters["Year"].Value = YearID;
            report.Parameters["InstID"].Value = HttpContext.Session["InstID"];
            report.CreateDocument();
            return PartialView("reportCompensation_FreeState", report);
        }

        public PartialViewResult reportExemptionSummary(int YearID)
        {
            XtraReport report = new ExemptionSummary();
            report.Parameters["Year"].Value = YearID;
            report.Parameters["ExemptionStatus"].Value = -2; //-2 = ALL
            report.Parameters["InstID"].Value = HttpContext.Session["InstID"];
            report.CreateDocument();
            return PartialView("reportExemptionSummary", report);
        }

        public PartialViewResult reportExemptionSummaryGrouped(int YearID)
        {
            XtraReport report = new ExemptionsSummaryGrouped();
            report.Parameters["Year"].Value = YearID;
            report.Parameters["InstID"].Value = HttpContext.Session["InstID"];
            report.CreateDocument();
            return PartialView("reportExemptionSummaryGrouped", report);
        }

        public PartialViewResult reportExemptionProvisionalList(int YearID)
        {
            XtraReport report = new ExemptionProvisionalList();
            report.Parameters["YearID"].Value = YearID;
            report.Parameters["InstID"].Value = HttpContext.Session["InstID"];
            report.CreateDocument();
            return PartialView("reportExemptionProvisionalList", report);
        }

        public PartialViewResult reportExemptionRegister(int YearID)
        {
            XtraReport report = new FormRegister();
            report.Parameters["YearID"].Value = YearID;
            report.Parameters["InstID"].Value = HttpContext.Session["InstID"];
            report.Parameters["FromNumber"].Value = 1;
            report.Parameters["Quantity"].Value = 100;
            report.CreateDocument();
            return PartialView("reportExemptionRegister", report);
        }
        public PartialViewResult reportExemptionApplication()
        {
            XtraReport report = new Application_CoverPage();
            report.Parameters["YearID"].Value = 11;
            report.Parameters["InstID"].Value = HttpContext.Session["InstID"];
            report.Parameters["ExemptionID"].Value = 16586;
            report.Parameters["IsPrintLetterHead"].Value = true;
            report.CreateDocument();
            return PartialView("reportExemptionApplication", report);
        }

        public PartialViewResult reportCOVID19()
        {
            XtraReport report = new ApplicationCovidForm_ENG();
            report.CreateDocument();
            return PartialView("reportCOVID19", report);
        }

        public PartialViewResult reportCOVID19AFR()
        {
            XtraReport report = new ApplicationCovidForm_AFR();
            report.CreateDocument();
            return PartialView("reportCOVID19AFR", report);
        }

        #endregion

        #region Print Reports

        public ActionResult printExemptions(int YearID, string ReportFilter)
        {
            using (var Context = new EduSpecDataContext())
            {
                MemoryStream stream = new MemoryStream();
                XtraReport report = ReportsController.rptExemptions(Convert.ToInt32(HttpContext.Session["InstID"]), YearID, ReportFilter);
                report.DisplayName = "Exemptions";
                PdfExportOptions options = new PdfExportOptions() { ShowPrintDialogOnOpen = false };
                report.ExportToPdf(stream, options);
                stream.Seek(0, SeekOrigin.Begin);
                return new FileStreamResult(stream, "application/pdf");
            }
        }

        public ActionResult printExemptionsSummaryGrouped(int YearID, string ReportFilter)
        {

            using (var Context = new EduSpecDataContext())
            {
                MemoryStream stream = new MemoryStream();
                XtraReport report = ReportsController.rptExemptionsSummaryGrouped(Convert.ToInt32(HttpContext.Session["InstID"]), YearID, ReportFilter);
                report.DisplayName = "Exemptions";
                PdfExportOptions options = new PdfExportOptions() { ShowPrintDialogOnOpen = false };
                report.ExportToPdf(stream, options);
                stream.Seek(0, SeekOrigin.Begin);
                return new FileStreamResult(stream, "application/pdf");
            }
        }
        
        //public ActionResult PrintApplicationForCompensasion(int YearID, int InstID)
        //{
        //    using (var Context = new EduSpecDataContext())
        //    {
        //        MemoryStream stream = new MemoryStream();
        //        XtraReport report = ReportsController.rptExemptionCompensasion(YearID, InstID);
        //        report.DisplayName = "ExemptionS";
        //        PdfExportOptions options = new PdfExportOptions() { ShowPrintDialogOnOpen = false };
        //        report.ExportToPdf(stream, options);
        //        stream.Seek(0, SeekOrigin.Begin);
        //        return new FileStreamResult(stream, "application/pdf");
        //    }
        //}
        public ActionResult PrintLearnerClassList(int InstID)
        {
            using (var Context = new EduSpecDataContext())
            {
                MemoryStream stream = new MemoryStream();
                XtraReport report = ReportsController.rptLearnerClassList(InstID);
                report.DisplayName = "Class list";
                PdfExportOptions options = new PdfExportOptions() { ShowPrintDialogOnOpen = false };
                report.ExportToPdf(stream, options);
                stream.Seek(0, SeekOrigin.Begin);
                return new FileStreamResult(stream, "application/pdf");
            }
        }
        public ActionResult PrintExemptionProvisionalList(int InstID,int YearID)
        {
            using (var Context = new EduSpecDataContext())
            {
                MemoryStream stream = new MemoryStream();
                XtraReport report = ReportsController.rptExemptionProvisionalList(InstID, YearID);
                report.DisplayName = "Provisional list";
                PdfExportOptions options = new PdfExportOptions() { ShowPrintDialogOnOpen = false };
                report.ExportToPdf(stream, options);
                stream.Seek(0, SeekOrigin.Begin);
                return new FileStreamResult(stream, "application/pdf");
            }
        }

        public ActionResult PrintFinalTableForExemptions(int InstID, int YearID)
        {
            using (var Context = new EduSpecDataContext())
            {
                MemoryStream stream = new MemoryStream();
                XtraReport report = ReportsController.rptFinalTableForExemptions(YearID,InstID);
                report.DisplayName = "Final table for exemptions";
                PdfExportOptions options = new PdfExportOptions() { ShowPrintDialogOnOpen = false };
                report.ExportToPdf(stream, options);
                stream.Seek(0, SeekOrigin.Begin);
                return new FileStreamResult(stream, "application/pdf");
            }
        }

        public ActionResult printApplicationRegister(int InstID, int Qty)
        {
            using (var Context = new EduSpecDataContext())
            {
                MemoryStream stream = new MemoryStream();
                XtraReport report = ReportsController.rptApplicationRegister(InstID, Qty);
                report.DisplayName = "Application Register";
                PdfExportOptions options = new PdfExportOptions() { ShowPrintDialogOnOpen = false };
                report.ExportToPdf(stream, options);
                stream.Seek(0, SeekOrigin.Begin);
                return new FileStreamResult(stream, "application/pdf");
            }
        }        
        public ActionResult printGenericLetter(int ExemptionID, bool IsPrintLetterHead, string FileName)
        {
            using (var Context = new EduSpecDataContext())
            {
                MemoryStream stream = new MemoryStream();
                XtraReport report = ReportsController.rptGenericLetter(Convert.ToInt32(HttpContext.Session["InstID"]), ExemptionID, 0, IsPrintLetterHead);
                report.DisplayName = "Exemptions";
                PdfExportOptions options = new PdfExportOptions() { ShowPrintDialogOnOpen = false };

                report.ExportToPdf(string.Format("{0}{1}", SystemParameters.SystemParams().PrintoutsFilePath, string.Format("{0}", FileName)));

                report.ExportToPdf(stream, options);
                stream.Seek(0, SeekOrigin.Begin);
                return new FileStreamResult(stream, "application/pdf");
            }
        }
        public ActionResult PrintAffidavit(int ExemptionID, int LanguageID)
        {
            using (var Context = new EduSpecDataContext())
            {
                MemoryStream stream = new MemoryStream();
                XtraReport report = ReportsController.RptAffidavit(Convert.ToInt32(HttpContext.Session["InstID"]), ExemptionID, LanguageID);
                report.DisplayName = "Exemptions";
                PdfExportOptions options = new PdfExportOptions() { ShowPrintDialogOnOpen = false };

                logger.Info("Set filename and location");
                string Filename = string.Format("{0}{1}.pdf", SystemParameters.SystemParams().ApplicationsFilePath, string.Format("{0}", Context.fn_Get_ApplicationNumber(ExemptionID)));
                logger.Info("Filename and location = " + Filename);
                try
                {
                    logger.Info("Save filename to location");
                    report.ExportToPdf(Filename);
                    logger.Info("File seved.");
                }
                catch(Exception e)
                {
                    logger.Error(e.Message);
                }
                finally
                {
                    logger.Info("Export file to stream.");
                    report.ExportToPdf(stream, options);
                    stream.Seek(0, SeekOrigin.Begin);                    
                }
                return new FileStreamResult(stream, "application/pdf");
            }
        }
        public ActionResult PrintAffidavitRefuses(int ExemptionID)
        {
            using (var Context = new EduSpecDataContext())
            {
                MemoryStream stream = new MemoryStream();
                XtraReport report = ReportsController.RptAffidavitRefuses(Convert.ToInt32(HttpContext.Session["InstID"]), ExemptionID);
                report.DisplayName = "Exemptions";
                PdfExportOptions options = new PdfExportOptions() { ShowPrintDialogOnOpen = false };

                report.ExportToPdf(stream, options);
                stream.Seek(0, SeekOrigin.Begin);
                return new FileStreamResult(stream, "application/pdf");
            }
        }
        public ActionResult PrintRequestToApply(int FamilyID)
        {
            using (var Context = new EduSpecDataContext())
            {
                MemoryStream stream = new MemoryStream();
                var x = Context.Get_RequestToApplyLetterValues(Convert.ToInt32(HttpContext.Session["InstID"]), FamilyID).FirstOrDefault();
                XtraReport report = ReportsController.RptPrintRequestToApply(Convert.ToInt32(HttpContext.Session["InstID"]), FamilyID, x.InstLetterID, x.IsPrintLetterHead);
                report.DisplayName = "Exemptions";
                PdfExportOptions options = new PdfExportOptions() { ShowPrintDialogOnOpen = false };

                report.ExportToPdf(stream, options);
                stream.Seek(0, SeekOrigin.Begin);
                return new FileStreamResult(stream, "application/pdf");
            }
        }
        public ActionResult PrintApplication(int ExemptionID, bool IsPrintCoverPage, bool IsPrintLetterHead, int LanguageID, int ApplMaritalStatusID, bool IsSingleApplication, int? Wareabouts, int? Contact, int? Unwilling, bool isAttachCovidForm, string FileName)
        {
            using (var Context = new EduSpecDataContext())
            {
                MemoryStream stream = new MemoryStream();
                try
                {
                    logger.Info("Get report.");
                    XtraReport report = ReportsController.rptExemptionApplication(
                        Convert.ToInt32(HttpContext.Session["InstID"]),
                        Context.fn_Get_ActiveYearID().Value,
                        ExemptionID,
                        IsPrintCoverPage,
                        IsPrintLetterHead,
                        LanguageID,
                        ApplMaritalStatusID,
                        IsSingleApplication,
                        Wareabouts,
                        Contact,
                        Unwilling,
                        isAttachCovidForm,
                        FileName
                        );

                    report.DisplayName = "Aplication For Exemption " + ExemptionID;
                    
                    logger.Info("Set filename and location");
                    string NewFilename = string.Format("{0}{1}", SystemParameters.SystemParams().ApplicationsFilePath, FileName);
                    logger.Info("Filename and location = " + NewFilename);
                    try
                    {
                        logger.Info("Save filename to location");
                        report.ExportToPdf(NewFilename);
                        logger.Info("File seved.");
                    }
                    catch (Exception e)
                    {
                        logger.Error(e.Message);
                    }
                    finally
                    {
                        PdfExportOptions options = new PdfExportOptions() { ShowPrintDialogOnOpen = true };
                        logger.Info("Export file to stream.");
                        report.ExportToPdf(stream, options);
                        stream.Seek(0, SeekOrigin.Begin);
                    }

                    return new FileStreamResult(stream, "application/pdf");
                }
                catch (Exception e)
                {
                    logger.Error(e.Message);
                    return null;
                }
            }
        }
        #endregion
    }
}
