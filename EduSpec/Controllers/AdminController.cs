using EduSpec.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting;
using System.IO;
using DevExpress.Web.Mvc;

namespace EduSpec.Controllers
{
    [SessionExpire]
    public class AdminController : Controller
    {

        #region Manage Users

        public ViewResult InstitutionUsers()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("Admin - Institution Users", WebSecurity.CurrentUserId);
                var i = Convert.ToInt32(HttpContext.Session["InstID"]);
                return View("InstitutionUsers");
            }
        }

        public PartialViewResult InstitutionUsersPartial()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("Admin - Institution Users", WebSecurity.CurrentUserId);
                var i = Convert.ToInt32(HttpContext.Session["InstID"]);
                return PartialView("InstitutionUsersPartial", Context.Get_Admin_InstitutionUsers_View(i).ToList());
            }
        }

        [HttpPost]
        public ActionResult InstitutionUserUpdate(Get_Admin_InstitutionUsers_ViewResult _User)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_Admin_User_Update(
                    _User.UserId,
                    _User.FullName,
                    _User.Surname,
                    _User.Email,
                    _User.AccessLevelID,
                    _User.DepartmentID,
                    _User.IsActive,
                    _User.CanAddNewApplication,
                    _User.CanSubmitApplication,
                    _User.CanProcessApplication,
                    _User.CanFinaliseApplication,
                    _User.CanManageSettings,
                    WebSecurity.CurrentUserId,
                    _User.PrintOnFinalise                    
                    );

                AccountController.setActivUserRole(_User.UserName, _User.IsActive);

                return InstitutionUsersPartial();
            }            
        }

        [HttpPost]
        public ActionResult InstitutionUserAdd(Get_Admin_InstitutionUsers_ViewResult User)
        {
            using (var Context = new EduSpecDataContext())
            {
                WebSecurity.CreateUserAndAccount(User.Email, User.Password, new
                {
                    FullName = User.FullName,
                    Surname = User.Surname,
                    Email = User.Email,
                    InstID = Convert.ToInt32(HttpContext.Session["InstID"]),
                    AccessLevelID = User.AccessLevelID,
                    DepartmentID = User.DepartmentID,
                    IsActive = User.IsActive,
                    CanAddNewApplication = User.CanAddNewApplication,
                    CanSubmitApplication = User.CanSubmitApplication ,
                    CanProcessApplication = User.CanProcessApplication,
                    CanFinaliseApplication = User.CanFinaliseApplication,
                    CanManageSettings = User.CanManageSettings,
                    PrintOnFinalise = User.PrintOnFinalise,
                    IsInstitution = true
                });
                Roles.AddUserToRole(User.Email, "ActiveUser");

                return InstitutionUsersPartial();
            }
        }

        public PartialViewResult DepartmentPickList(int? AccessLevelID)
        {
            ViewData["AccessLevelID"] = AccessLevelID ?? -1;
            return PartialView();
        }

        public static List<EduSpec.GridUtils.DropdownList> getInstitutionDepartments(int AccessLevelID)
        {
            using (var Context = new EduSpecDataContext())
            {
                return Context.ExecuteQuery<EduSpec.GridUtils.DropdownList>(String.Format("EXEC Get_InstitutionDepartments_PickList '{0}'", AccessLevelID)).ToList();
            }
        }

        #endregion

        #region Manage Institution

        public ViewResult ManageInstitution()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("Admin - Manage Institution", WebSecurity.CurrentUserId);
                return View("ManageInstitution");
            }
        }

        public PartialViewResult ManageInstitutionPartial()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("Admin - Manage Institution", WebSecurity.CurrentUserId);
                return PartialView("ManageInstitutionPartial", GetEditableDealer(Convert.ToInt32(HttpContext.Session["InstID"])).FirstOrDefault());
            }
        }

        [HttpPost]
        public ActionResult updateInstitution(Get_Admin_ManageInstitution_ViewResult Institution)
        {
            if (ModelState.IsValid)
            {
                using (var Context = new EduSpecDataContext())
                {
                    Context.Set_Admin_Institution_Update(
                        Convert.ToInt32(HttpContext.Session["InstID"]),
                        WebSecurity.CurrentUserId,
                        Institution.Name,
                        Institution.EMISNo,
                        Institution.QuintileID,
                        Institution.PhaseID,
                        Institution.PhysAddress1,
                        Institution.PhysAddress2,
                        Institution.PhysAddress3,
                        Institution.PhysProvinceID,
                        Institution.PhysCode,
                        Institution.PosAddress1,
                        Institution.PosAddress2,
                        Institution.PosAddress3,
                        Institution.PosProvinceID,
                        Institution.PosCode,
                        Institution.Telephone,
                        Institution.Cell,
                        Institution.Fax,
                        Institution.Email,
                        Institution.CorrespondenceLanguageID,
                        Institution.GradeFromID,
                        Institution.GradeToID,
                        Institution.LESNFromYear,
                        Institution.LESNToYear,
                        Institution.IntegrationTypeID,
                        Institution.BankID,
                        Institution.BankAccount,
                        Institution.BankAccountTypeID,
                        Institution.BankBranchCode
                    );
                    return ManageInstitution();
                }
            }
            return null;
        }

        #endregion

        #region Application Packs

        public ViewResult ApplicationPacks()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("Admin - Application Packs", WebSecurity.CurrentUserId);
                return View("ApplicationPacks");
            }
        }

        public PartialViewResult ApplicationPacksPartial()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("Admin - Application Packs", WebSecurity.CurrentUserId);
                return PartialView("ApplicationPacksPartial", Context.Get_Admin_InstitutionApplicationPacks_View(Convert.ToInt32(HttpContext.Session["InstID"])).ToList());
            }
        }

        public PartialViewResult ApplicationPacksOrderPartial()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["OrderViewProperties"] = ViewProperties.viewProperties("Admin - Order Application Pack", WebSecurity.CurrentUserId);
                return PartialView("ApplicationPacksOrderPartial", Context.Get_Admin_ApplicationPacks_View(Convert.ToInt32(HttpContext.Session["InstID"])).ToList());
            }
        }

        [HttpPost]
        public ActionResult ApplicationPacksAddNewOrder(int PackID)
        {
            using (var Context = new EduSpecDataContext())
            {
                int? InvoiceID = Context.Set_ApplicationPack_Add(Convert.ToInt32(HttpContext.Session["InstID"]), PackID, 1).FirstOrDefault().InvoiceID;
                var redirectUrl = new UrlHelper(Request.RequestContext).Action("PrintApplicationPackInvoice", "Admin", new { InvoiceID = InvoiceID });
                return Json(new { Url = redirectUrl });
            }
        }

        public ActionResult PrintApplicationPackInvoice(int InvoiceID)
        {
            using (var Context = new EduSpecDataContext())
            { 
                MemoryStream stream = new MemoryStream();
                XtraReport report = ReportsController.rptInvoice(Convert.ToInt32(HttpContext.Session["InstID"]),
                    InvoiceID);
                report.DisplayName = "Invoice";
                PdfExportOptions options = new PdfExportOptions() { ShowPrintDialogOnOpen = true };
                report.ExportToPdf(stream, options);
                stream.Seek(0, SeekOrigin.Begin);
                return new FileStreamResult(stream, "application/pdf");
            }
        }

        public ActionResult getInstitutionApplicationPackInvoice(string returnUrl)
        {
            return Redirect(returnUrl);
        }
  

        #endregion

        #region SMS Bundles

        public ViewResult SMSBundles()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("Admin - SMS Bundles", WebSecurity.CurrentUserId);
                return View("SMSBundles");
            }
        }

        public PartialViewResult SMSBundlesPartial()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("Admin - SMS Bundles", WebSecurity.CurrentUserId);
                return PartialView("SMSBundlesPartial", Context.Get_Admin_InstitutionSMSBundles_View(Convert.ToInt32(HttpContext.Session["InstID"])).ToList());
            }
        }

        public PartialViewResult SMSBundlesOrderPartial()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["OrderViewProperties"] = ViewProperties.viewProperties("Admin - Order SMS Bundle", WebSecurity.CurrentUserId);
                return PartialView("SMSBundlesOrderPartial", Context.Get_Admin_SMSBundles_View().ToList());
            }
        }

        [HttpPost]
        public ActionResult SMSBundleAddNewOrder(int BundleID)
        {
            using (var Context = new EduSpecDataContext())
            {
                int? InvoiceID = Context.Set_Admin_SMSBundle_Add(Convert.ToInt32(HttpContext.Session["InstID"]), BundleID, 1).FirstOrDefault().InvoiceID;
                var redirectUrl = new UrlHelper(Request.RequestContext).Action("PrintSMSBundleInvoice", "Admin", new { InvoiceID = InvoiceID });
                return Json(new { Url = redirectUrl });
            }
        }

        public ActionResult PrintSMSBundleInvoice(int InvoiceID)
        {
            using (var Context = new EduSpecDataContext())
            {
                MemoryStream stream = new MemoryStream();
                XtraReport report = ReportsController.rptInvoice(Convert.ToInt32(HttpContext.Session["InstID"]),
                    InvoiceID);
                report.DisplayName = "Invoice";
                PdfExportOptions options = new PdfExportOptions() { ShowPrintDialogOnOpen = true };
                report.ExportToPdf(stream, options);
                stream.Seek(0, SeekOrigin.Begin);
                return new FileStreamResult(stream, "application/pdf");
            }
        }

        public ActionResult getInstitutionSMSBundleInvoice(string returnUrl)
        {
            return Redirect(returnUrl);
        }

        public ActionResult getInstitutionQuotation(string returnUrl)
        {
            return Redirect(returnUrl);
        }

        #endregion

        #region School fees

        public ViewResult SetupSchoolFees()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("Admin - School Fees", WebSecurity.CurrentUserId);
                return View("SetupSchoolFees");
            }
        }

        public PartialViewResult SetupSchoolFeesPartial()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("Admin - School Fees", WebSecurity.CurrentUserId);
                return PartialView("SetupSchoolFeesPartial", Context.Get_Admin_SchoolFees_View(Convert.ToInt32(HttpContext.Session["InstID"])).ToList());
            }
        }

        public PartialViewResult SetupSchoolFeesNextYearPartial()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["SchoolFeesNextYearViewProperties"] = ViewProperties.viewProperties("Admin - School Fees Next Year", WebSecurity.CurrentUserId);
                return PartialView("SetupSchoolFeesNextYearPartial", Context.Get_Admin_SchoolFeesNextYear_View(Convert.ToInt32(HttpContext.Session["InstID"])).ToList());
            }
        }
        public PartialViewResult SetupSchoolFeesBillingCyclePartial()
        {
            using (var Context = new EduSpecDataContext())
            {                
                return PartialView("SetupSchoolFeesBillingCyclePartial", Context.Get_Admin_InstitutionBillingCycle_View(Convert.ToInt32(HttpContext.Session["InstID"])).FirstOrDefault());
            }
        }
        public PartialViewResult SetupAdHocFeesPartial()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["AdHocFeesViewProperties"] = ViewProperties.viewProperties("Admin - AdHocFees", WebSecurity.CurrentUserId);
                return PartialView("SetupAdHocFeesPartial", Context.Get_Admin_AdHocFees_View(Convert.ToInt32(HttpContext.Session["InstID"])).ToList());
            }
        }

        [HttpPost]
        public ActionResult SchoolFeesUpdate(Get_Admin_SchoolFees_ViewResult SchoolFees)
        {
            using (var Context = new EduSpecDataContext() { CommandTimeout = 5 * 60 })
            {
                Context.Set_Admin_SchoolFees_Update(
                    SchoolFees.AutoID,
                    SchoolFees.SchoolFees1,
                    SchoolFees.SchoolFees2,
                    SchoolFees.SchoolFees3,
                    SchoolFees.IsIncludeInExemptions,
                    WebSecurity.CurrentUserId,
                    Convert.ToInt32(HttpContext.Session["InstID"])
                    );
                return SetupSchoolFeesPartial();
            }
        }

        [HttpPost]
        public ActionResult SchoolFeesNextYearUpdate(Get_Admin_SchoolFeesNextYear_ViewResult SchoolFees)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_Admin_SchoolFees_Update(
                    SchoolFees.AutoID,
                    SchoolFees.SchoolFees1,
                    SchoolFees.SchoolFees2,
                    SchoolFees.SchoolFees3,
                    SchoolFees.IsIncludeInExemptions,
                    WebSecurity.CurrentUserId,
                    Convert.ToInt32(HttpContext.Session["InstID"])
                    );
                return SetupSchoolFeesNextYearPartial();
            }
        }
        [HttpPost]
        public JsonResult SchoolFeesBillingCycleUpdate(Get_Admin_InstitutionBillingCycle_ViewResult Billing)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_Admin_InstitutionBillingCycle_Update(
                    Convert.ToInt32(HttpContext.Session["InstID"]),
                    Billing.BillingCycleTypeID,
                    Billing.IsBillingAnnualy,
                    Billing.IsBillngMonthly,
                    Billing.IsBillingQuarterly,
                    Billing.Quarter1InvoiceDate,
                    Billing.Quarter1InvoiceAmount,
                    Billing.Quarter2InvoiceDate,
                    Billing.Quarter2InvoiceAmount,
                    Billing.Quarter3InvoiceDate,
                    Billing.Quarter3InvoiceAmount,
                    Billing.Quarter4InvoiceDate,
                    Billing.Quarter4InvoiceAmount,
                    WebSecurity.CurrentUserId
                    );
                return Json(new { Success = true, });
            }
        }
        [HttpPost]
        public ActionResult AdHocFeesAdd(Get_Admin_AdHocFees_ViewResult AdHocFees)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_Admin_AdHocFees_Add(
                    Convert.ToInt32(HttpContext.Session["InstID"]),
                    AdHocFees.Description,
                    AdHocFees.AgeReference,
                    AdHocFees.IsCredit,
                    AdHocFees.IsPercentage,
                    AdHocFees.Value,
                    AdHocFees.IsApplyToSchoolFees,
                    AdHocFees.IsOnSchoolFeesAccount,
                    AdHocFees.MonthsPayableAdhocFees,
                    AdHocFees.EffectiveDate,
                    WebSecurity.CurrentUserId
                );
                return SetupAdHocFeesPartial();
            }
        }

        [HttpPost]
        public ActionResult AdHocFeesUpdate(Get_Admin_AdHocFees_ViewResult AdHocFees)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_Admin_AdHocFees_Update(
                    AdHocFees.AdHocFeeID,
                    AdHocFees.Description,
                    AdHocFees.AgeReference,
                    AdHocFees.IsCredit,
                    AdHocFees.IsPercentage,
                    AdHocFees.Value,
                    AdHocFees.IsApplyToSchoolFees,
                    AdHocFees.IsOnSchoolFeesAccount,
                    AdHocFees.MonthsPayableAdhocFees,
                    AdHocFees.EffectiveDate,
                    WebSecurity.CurrentUserId
                );
                return SetupAdHocFeesPartial();
            }
        }

        [HttpPost]
        public JsonResult SchoolFeesAllUpdate(int AutoID)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_Admin_SchoolFeesAll_Update(
                    AutoID,
                    Convert.ToInt32(HttpContext.Session["InstID"]),
                    WebSecurity.CurrentUserId

                    );
                return Json(new { Success = true, });
            }
        }

        [HttpPost]
        public JsonResult SchoolFeesNextYearAllUpdate(int AutoID)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_Admin_SchoolFeesAll_Update(
                    AutoID,
                    Convert.ToInt32(HttpContext.Session["InstID"]),
                    WebSecurity.CurrentUserId
                    );
                return Json(new { Success = true, });
            }
        }

        public PartialViewResult InstitutionSchoolFeePayments()
        {
            using (var Context = new EduSpecDataContext())
            {
                return PartialView("InstitutionSchoolFeePayments", Context.Get_Admin_InstitutionSchoolFeeSettings_View(Convert.ToInt32(HttpContext.Session["InstID"]), Convert.ToInt32(HttpContext.Session["CurrentYearID"])).FirstOrDefault());
            }
        }

        [HttpPost]
        public ActionResult setInstitutionSchoolFeePaymentsAdd(Get_Admin_InstitutionSchoolFeeSettings_ViewResult SchoolFeeSettings)
        {
            using (var Context = new EduSpecDataContext() { CommandTimeout = 5 * 60 })
            {
                Context.Set_Admin_SchoolFeeGeneralSettings_Add(
                    Convert.ToInt32(HttpContext.Session["InstID"]),
                    SchoolFeeSettings.InitialPayment,
                    SchoolFeeSettings.InitialPaymentDescription_ENG,
                    SchoolFeeSettings.InitialPaymentDescription_AFR,
                    SchoolFeeSettings.MonthsPayable,
                    SchoolFeeSettings.SchoolFeesStartMonth,
                    SchoolFeeSettings.IsInitialPaymentExcludedFromSchoolFees,
                    SchoolFeeSettings.EntryRuleID,
                    SchoolFeeSettings.TerminationRuleID,
                    SchoolFeeSettings.IsAddIntitialFeeToStartMonth,
                    SchoolFeeSettings.Rounding
                    );
                return SetupSchoolFees();
            }
        }

        #endregion

        #region Finance

        public ViewResult Finance()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("Admin - Finance", WebSecurity.CurrentUserId);
                return View("Finance");
            }
        }

        //public PartialViewResult FinancePartial()
        //{
        //    using (var Context = new EduSpecDataContext())
        //    {
        //        ViewData["ViewProperties"] = ViewProperties.viewProperties("Admin - Finance", WebSecurity.CurrentUserId);
        //        return PartialView("FinancePartial", Context.Get_Admin_InstitutionFinance_View(Convert.ToInt32(HttpContext.Session["InstID"])).ToList());
        //    }
        //}

        public PartialViewResult FinanceInvoicePartial()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["InvoiceViewProperties"] = ViewProperties.viewProperties("Admin - Finance Invoices", WebSecurity.CurrentUserId);
                return PartialView("FinanceInvoicePartial", Context.Get_Admin_InstitutionFinance_View(Convert.ToInt32(HttpContext.Session["InstID"])).ToList());
            }
        }

        public PartialViewResult FinanceQuotationPartial()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["QuotationViewProperties"] = ViewProperties.viewProperties("Admin - Finance Quotations", WebSecurity.CurrentUserId);
                return PartialView("FinanceQuotationPartial", Context.Get_Admin_FinanceQuotations_View(Convert.ToInt32(HttpContext.Session["InstID"])).ToList());
            }
        }

        [HttpPost]
        public JsonResult Set_GenerateQuotation(int ItemID, int PackageID)
        {
            using (var Context = new EduSpecDataContext())
            {
                try
                {
                    int? QuotationID = Context.Set_Quotation_Add(
                         ItemID,
                         Convert.ToInt32(HttpContext.Session["InstID"]),
                         PackageID).FirstOrDefault().QuotationID;
                    var redirectUrl = new UrlHelper(Request.RequestContext).Action("printQuotation", "Account", new { QuotationID });
                    return Json(new { Success = true, error = "", Url = redirectUrl });

                }
                catch (Exception e)
                {
                    return Json(new { Success = false, error = e.Message, Url = "" });
                }

            }

        }

        [HttpPost]
        public JsonResult Set_AcceptQuotation(int QuotationID)
        {
            using (var Context = new EduSpecDataContext())
            {
                try
                {
                    //var QuotationID = Context.fn_Get_InstitutionActiveQuotationID(Convert.ToInt32(HttpContext.Session["InstID"]));

                    int? InvoiceID = Context.Set_ApplicationPackFromQuotation_Add(
                                        Convert.ToInt32(HttpContext.Session["InstID"]),
                                        QuotationID).FirstOrDefault().InvoiceID;
                    var redirectUrl = new UrlHelper(Request.RequestContext).Action("PrintNewInvoice", "Admin", new { InvoiceID = InvoiceID });
                    return Json(new { Success = true, error = "", Url = redirectUrl });

                }
                catch (Exception e)
                {
                    return Json(new { Success = false, error = e.Message, Url = "" });
                }

            }

        }

        public ActionResult PrintNewInvoice(int InvoiceID)
        {
            using (var Context = new EduSpecDataContext())
            {
                MemoryStream stream = new MemoryStream();
                XtraReport report = ReportsController.rptInvoice(Convert.ToInt32(HttpContext.Session["InstID"]),
                    InvoiceID);
                report.DisplayName = "Invoice";
                PdfExportOptions options = new PdfExportOptions() { ShowPrintDialogOnOpen = true };
                report.ExportToPdf(stream, options);
                stream.Seek(0, SeekOrigin.Begin);
                return new FileStreamResult(stream, "application/pdf");
            }
        }

        public ActionResult getInstitutionSMSInvoices(string returnUrl)
        {
            return Redirect(returnUrl);
        }

        public ActionResult getInstitutionAplicationInvoices(string returnUrl)
        {
            return Redirect(returnUrl);
        }

        public ActionResult getInstitutionQuotations(string returnUrl)
        {
            return Redirect(returnUrl);
        }

        #endregion



        public class EditableInstitution
        {
            [Required(ErrorMessage = "Institution Name is required")]
            [StringLength(100, ErrorMessage = "Must be under 100 characters")]
            [Display(Name = "Institution Name")]
            public string Name { get; set; }

            [StringLength(30, ErrorMessage = "Must be under 30 characters")]
            [Display(Name = "EMIS No")]
            public string EMISNo { get; set; }

            [Display(Name = "Quintile")]
            public int QuintileID { get; set; }

            [Display(Name = "Phase")]
            public int PhaseID { get; set ; }

            [Display(Name = "CorrespondenceLanguageID")]
            public int CorrespondenceLanguageID { get; set; }

            [Display(Name = "GradeFromID")]
            public int GradeFromID { get; set; }

            [Display(Name = "LESNFromYear")]
            public int LESNFromYear { get; set; }

            [Display(Name = "LESNToYear")]
            public int LESNToYear { get; set; }

            [Display(Name = "GradeToID")]
            public int GradeToID { get; set; }

            [Display(Name = "IntegrationTypeID")]
            public int? IntegrationTypeID { get; set; }

            [StringLength(50, ErrorMessage = "Must be under 50 characters")]
            [Display(Name = "Physical Address")]
            public string PhysAddress1 { get; set; }

            public string PhysAddress2 { get; set; }

            public string PhysAddress3 { get; set; }

            public byte? PhysProvinceID { get; set; }

            public string PhysCode { get; set; }

            [StringLength(50, ErrorMessage = "Must be under 50 characters")]
            [Display(Name = "Postal Address")]
            public string PosAddress1 { get; set; }

            public string PosAddress2 { get; set; }

            public string PosAddress3 { get; set; }

            public byte? PosProvinceID { get; set; }

            public string PosCode { get; set; }

            [Display(Name = "Bank details")]
            public int? BankID { get; set; }

            public string BankAccount { get; set; }

            public int? BankAccountTypeID { get; set; }

            public string BankBranchCode { get; set; }

            [Display(Name = "Telephone Number")]
            public string Telephone { get; set; }

            [Display(Name = "Cell Number")]
            public string Cell { get; set; }

            [Display(Name = "Fax Number")]
            public string Fax { get; set; }

            [Display(Name = "Email Address")]
            public string Email { get; set; }
        }

        public static IList<EditableInstitution> GetEditableDealer(int InstID)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Get_Admin_ManageInstitution_View(InstID).ToList();
                var Institution = (
                                   from _Institution in Context.Get_Admin_ManageInstitution_View(InstID).ToList()
                                   select new EditableInstitution
                                   { Name = _Institution.Name,
                        EMISNo = _Institution.EMISNo,
                        QuintileID = _Institution.QuintileID,
                        PhaseID = _Institution.PhaseID,
                        PhysAddress1 = _Institution.PhysAddress1,
                        PhysAddress2 = _Institution.PhysAddress2,
                        PhysAddress3 = _Institution.PhysAddress3,
                        PhysProvinceID = _Institution.PhysProvinceID,
                        PhysCode = _Institution.PhysCode,
                        PosAddress1 = _Institution.PosAddress1,
                        PosAddress2 = _Institution.PosAddress2,
                        PosAddress3 = _Institution.PosAddress3,
                        PosProvinceID = _Institution.PosProvinceID,
                        PosCode = _Institution.PosCode,
                        Telephone = _Institution.Telephone,
                        Cell = _Institution.Cell,
                        Fax = _Institution.Fax,
                        Email = _Institution.Email,
                        CorrespondenceLanguageID = _Institution.CorrespondenceLanguageID,
                        GradeFromID = _Institution.GradeFromID,
                        GradeToID = _Institution.GradeToID,
                        LESNFromYear = _Institution.LESNFromYear,
                        LESNToYear = _Institution.LESNToYear,
                        IntegrationTypeID = _Institution.IntegrationTypeID,
                        BankID = _Institution.BankID,
                        BankAccount = _Institution.BankAccount,
                        BankAccountTypeID = _Institution.BankAccountTypeID,
                        BankBranchCode = _Institution.BankBranchCode
                                   }).ToList();
                return Institution;
            }
        }
    }
}
