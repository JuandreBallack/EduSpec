
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using EduSpec.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;


namespace EduSpec.Controllers
{
    [SessionExpire]
    public class SystemAdminController : Controller
    {

        #region InstitutionTakeOn
        public ViewResult InstitutionTakeOn()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("System Admin - Institution Take On", WebSecurity.CurrentUserId);
                return View("InstitutionTakeOn");
            }
        }

        public ActionResult InstitutionTakeOnPartial()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("System Admin - Institution Take On", WebSecurity.CurrentUserId);
                return PartialView("InstitutionTakeOnPartial", Context.Get_SystemAdmin_InstitutionsTakeOn_View().ToList());
            }
        }

        [HttpPost]
        public ActionResult InstitutionTakeonAdd(int MasterInstID)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_SystemAdmin_InstitutionTakeOn_Add(MasterInstID, WebSecurity.CurrentUserId);
                return null;
            }
        }

        [HttpPost]
        public ActionResult InstitutionTakeOnUpdate(Get_SystemAdmin_InstitutionsTakeOn_ViewResult TakeOn)
        {
         
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_SystemAdmin_InstitutionsTakeOn_Update(
                    TakeOn.MasterInstID,
                    TakeOn.Institution,
                    TakeOn.ProvinceID,
                    TakeOn.Telephone,
                    TakeOn.Cell
                    );
                return InstitutionTakeOnPartial();
            }
        }

        #endregion

        #region Institutions
        public ViewResult Institutions()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("System Admin - Institutions", WebSecurity.CurrentUserId);
                return View("Institutions");
            }
        }
          
        public ActionResult InstitutionsPartial()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("System Admin - Institutions", WebSecurity.CurrentUserId);
                return PartialView("InstitutionsPartial", Context.Get_SystemAdmin_Institutions_View().ToList());
            }
        }

        [HttpPost]
        public ActionResult InstitutionUpdate(Get_SystemAdmin_Institutions_ViewResult Inst)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_SystemAdmin_Institutions_Update(
                    Inst.InstID,
                    Inst.SystemTypeID,
                    Inst.Account,
                    Inst.IsRequireExemptionsLicenceFee,
                    Inst.IsRequireDebtManagementLicenceFee
                    );
                return InstitutionsPartial();
            }
        }

        [HttpPost]
        public JsonResult Set_AnnualLicenceFees()
        {
            using (var Context = new EduSpecDataContext())
            {
                try
                {
                    Context.Set_Annual_LicenceFees();
                    return Json(new { Success = true, error = "" });

                }
                catch (Exception e)
                {
                    return Json(new { Success = false, error = e.Message, Url = "" });
                }

            }

        }

        #endregion

        #region InstitutionsActivation
        public ViewResult InstitutionsActivation()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("System Admin - Institutions Activation", WebSecurity.CurrentUserId);
                return View("InstitutionsActivation");
            }
        }

        public ActionResult InstitutionsActivationPartial()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("System Admin - Institutions Activation", WebSecurity.CurrentUserId);
                return PartialView("InstitutionsActivationPartial", Context.Get_SystemAdmin_ActivateInstitution_View().ToList());
            }
        }

        [HttpPost]
        public ActionResult InstitutionActivationUpdate(Get_SystemAdmin_ActivateInstitution_ViewResult Institution)
        {
            if (ModelState.IsValid)
            {
                if (!WebSecurity.UserExists(Institution.UserName))
                {
                    try
                    {
                           
                        WebSecurity.CreateUserAndAccount(Institution.UserName, "Password1", new
                        { Institution.FullName, Institution.Surname, Email = Institution.UserName, Institution.InstID, AccessLevelID = 3, DepartmentID = 1, IsActive = 1, PrintOnFinalise = true, IsInstitution = true});
                        Roles.AddUserToRole(Institution.UserName, "ActiveUser");
                        using (var Context = new EduSpecDataContext())
                        {
                            int InvoiceID = Context.Set_SystemAdmin_ActivateInstitution_Update(
                                                Institution.InstID,
                                                Institution.IsWebEnabled,
                                                Institution.IsActive,
                                                WebSecurity.CurrentUserId,
                                                Institution.InstitutionStatusID).FirstOrDefault().InvoiceID ?? -1;
                            if ((InvoiceID > -1) && (Institution.InstitutionStatusID == 3))
                            {
                                XtraReport report = ReportsController.rptInvoice(Institution.InstID, InvoiceID);
                            }

                            return InstitutionsActivationPartial();
                        }



                    }
                    catch (MembershipCreateUserException e)
                    {
                        ModelState.AddModelError(string.Empty, MembershipStatus.ErrorCodeToString(e.StatusCode));
                    }
                }
                else
                {
                    using (var Context = new EduSpecDataContext())
                    {
                        int InvoiceID = Context.Set_SystemAdmin_ActivateInstitution_Update(
                                            Institution.InstID,
                                            Institution.IsWebEnabled,
                                            Institution.IsActive,
                                            WebSecurity.CurrentUserId,
                                            Institution.InstitutionStatusID).FirstOrDefault().InvoiceID ?? -1;
                        if ((InvoiceID > -1) && (Institution.InstitutionStatusID == 3))
                        {
                            XtraReport report = ReportsController.rptInvoice(Institution.InstID, InvoiceID);
                        }

                        return InstitutionsActivationPartial();
                    }
                }
            }
            return null;            
        }

        [HttpPost]
        public ActionResult SetYearEnd()
        {
            using (var Context = new EduSpecDataContext())
            {
                var InvoiceIDList = Context.Set_YearEnd().ToList();

                foreach (var Invoice in InvoiceIDList)
                {

                     XtraReport report = ReportsController.rptInvoice(Convert.ToInt32(Invoice.InstID), Convert.ToInt32(Invoice.InvoiceID));

                }
            }
            return null;
        }

        public ActionResult printInvoice(int InstID, int InvoiceID)
        {
            using (var Context = new EduSpecDataContext())
            {
                MemoryStream stream = new MemoryStream();
                XtraReport report = ReportsController.rptInvoice(Convert.ToInt32(HttpContext.Session["InstID"]), InvoiceID);
                report.DisplayName = "Invoice";
                PdfExportOptions options = new PdfExportOptions() { ShowPrintDialogOnOpen = true };
                report.ExportToPdf(stream, options);
                stream.Seek(0, SeekOrigin.Begin);
                return new FileStreamResult(stream, "application/pdf");
            }
        }

        #endregion

        #region PacksPendingActivation
        public ViewResult PacksPendingActivation()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("System Admin - Packs Pending Activation", WebSecurity.CurrentUserId);
                return View("PacksPendingActivation");
            }
        }

        public PartialViewResult PacksPendingActivationPartial()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("System Admin - Packs Pending Activation", WebSecurity.CurrentUserId);
                return PartialView("PacksPendingActivationPartial", Context.Get_SystemAdmin_PacksPendingActivation_View().ToList());
            }
        }

        [HttpPost]
        public ActionResult ActivateApplicationPack(int ApplicationPackID)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_ApplicationPack_Activation_Add(ApplicationPackID);
            }
            return PacksPendingActivationPartial();
        }

        #endregion

        #region Bundles Pending Activation

        public ViewResult BundlesPendingActivation()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("System Admin - Bundles Pending Activation", WebSecurity.CurrentUserId);
                return View("BundlesPendingActivation");
            }
        }

        public PartialViewResult BundlesPendingActivationPartial()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("System Admin - Bundles Pending Activation", WebSecurity.CurrentUserId);
                return PartialView("BundlesPendingActivationPartial", Context.Get_SystemAdmin_BundlesPendingActivation_View().ToList());
            }
        }

        [HttpPost]
        public ActionResult ActivateSMSBundle(int SMSBundleID)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_SMSBundle_Activation_Add(SMSBundleID);
            }
            return BundlesPendingActivationPartial();
        }

        #endregion

        #region Impersonate Institution

        public ViewResult InstitutionImpersonate()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("SystemAdmin - Institution Impersonate", WebSecurity.CurrentUserId);
                return View("InstitutionImpersonate");
            }

        }

        public PartialViewResult InstitutionImpersonatePartial()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("SystemAdmin - Institution Impersonate", WebSecurity.CurrentUserId);
                return PartialView("InstitutionImpersonatePartial", Context.Get_Admin_InstitutionImpersonate_View().ToList());
            }
        }

        #endregion

        #region Client ledger

        public ActionResult ClientLedger()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("System Admin - Client ledger", WebSecurity.CurrentUserId);
                return View("ClientLedger");
            }
        }

        public PartialViewResult ClientLedgerPartial(int? YearID)
        {
            using (var Context = new EduSpecDataContext())
            {
                
                ViewData["ViewProperties"] = ViewProperties.viewProperties("System Admin - Client ledger", WebSecurity.CurrentUserId);
                return PartialView("ClientLedgerPartial", Context.Get_SystemAdmin_ClientLedger_View(YearID ?? Context.fn_Get_YearID(DateTime.Now)).ToList());
            }
        }

        public PartialViewResult AddNewInvoicePartial(int? LedgerTypeID)
        {
            using (var Context = new EduSpecDataContext())
            {
                HttpContext.Session.Add("LedgerTypeID", LedgerTypeID);
                //ViewBag.LedgerTypeID = LedgerTypeID;
                return PartialView("AddNewInvoicePartial");
            }
        }

        public PartialViewResult LedgerTypePickList(int? LedgerTypeID)
        {
            //ViewBag.LedgerTypeID = LedgerTypeID;
            HttpContext.Session.Add("LedgerTypeID", LedgerTypeID);
            return PartialView("LedgerTypePickList");
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult NewInvoiceAdd(int InstID)
        {

            using (var Context = new EduSpecDataContext())
            {
                int? InvoiceID = Context.Set_SystemAdmin_ClientLedger_NewInvoice_Add(
                                    InstID
                                    ).FirstOrDefault().InvoiceID;
                var redirectUrl = new UrlHelper(Request.RequestContext).Action("NewInvoice", "SystemAdmin", new { InvoiceID = InvoiceID });
                return Json(new { Url = redirectUrl });
            }
        }

        public ActionResult NewInvoice(int InvoiceID)
        {
            using (var Context = new EduSpecDataContext())
            {
                HttpContext.Session.Add("NewInvoiceID", InvoiceID);
                ViewData["NewInvoiceLinesViewProperties"] = ViewProperties.viewProperties("System Admin - New Invoice Lines", WebSecurity.CurrentUserId);
                return View("NewInvoice");
            }
        }

        public PartialViewResult NewInvoiceLinesPartial(int? InvoiceID)
        {
            using (var Context = new EduSpecDataContext())
            {

                ViewData["NewInvoiceLinesViewProperties"] = ViewProperties.viewProperties("System Admin - New Invoice Lines", WebSecurity.CurrentUserId);
                return PartialView("NewInvoiceLinesPartial", Context.Get_SystemAdmin_ClientLedger_NewInvoiceLines_View(InvoiceID).ToList());
            }
        }

        [HttpPost]
        //public ActionResult AddInvoiceItem(int ItemID, int? TotalHours)
        public ActionResult AddInvoiceItem(Get_SystemAdmin_ClientLedger_NewInvoiceLines_ViewResult Item)
        {

            using (var Context = new EduSpecDataContext())
            {
                if (Item.Quantity == 0)
                {
                    Item.Quantity = 1;
                }

                Context.Set_SystemAdmin_InvoiceItem_Add(
                    Item.ItemID,
                    Convert.ToInt32(HttpContext.Session["NewInvoiceID"]),
                    Item.Quantity
                    );

                return NewInvoiceLinesPartial(Convert.ToInt32(HttpContext.Session["NewInvoiceID"]));
            }
        }

        [HttpPost]
        //public ActionResult AddInvoiceItem(int ItemID, int? TotalHours)
        public ActionResult DeleteRowInvoiceItem(Get_SystemAdmin_ClientLedger_NewInvoiceLines_ViewResult Item)
        {

            using (var Context = new EduSpecDataContext())
            {
               
                Context.Set_SystemAdmin_InvoiceItem_Delete(
                    Convert.ToInt32(HttpContext.Session["NewInvoiceID"]),
                    Item.InvoiceLinesID
                    );

                return NewInvoiceLinesPartial(Convert.ToInt32(HttpContext.Session["NewInvoiceID"]));
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult DeleteNewInvoice(int InvoiceLinesID)
        {

            using (var Context = new EduSpecDataContext())
            {
                Context.Set_SystemAdmin_Invoice_Delete(
                    Convert.ToInt32(HttpContext.Session["NewInvoiceID"]),
                    InvoiceLinesID
                );
                var redirectUrl = new UrlHelper(Request.RequestContext).Action("ClientLedger", "SystemAdmin");
                return Json(new { Url = redirectUrl });
            }
        }


        [HttpPost]
        public ActionResult ClientLedgerAdd(int InstID, DateTime DateDone, int LedgerTypeID, int LedgerTrxTypeID, decimal Amount)
        {

            using (var Context = new EduSpecDataContext())
            {
                Context.Set_SystemAdmin_ClientLedger_Add(
                    InstID,
                    DateDone,
                    LedgerTypeID,
                    LedgerTrxTypeID,
                    Amount
                    );
                return Json(new { Success = true, });
            }
        }

        [HttpPost]
        public ActionResult GenerateInvoice(int ItemID, int LedgerTypeID, int InstID, bool EmailInvoice)
        {
            using (var Context = new EduSpecDataContext())
            {

                int? InvoiceID = Context.Set_SystemAdmin_ClientLedger_GenerateInvoice(
                                    ItemID,
                                    InstID,
                                    LedgerTypeID,
                                    EmailInvoice
                                    ).FirstOrDefault().InvoiceID;
                var redirectUrl = new UrlHelper(Request.RequestContext).Action("PrintClientLedgerInvoice", "SystemAdmin", new { InvoiceID = InvoiceID, InstID });
                return Json(new { Url = redirectUrl });
            }
        }

        public ActionResult PrintClientLedgerInvoice(int InvoiceID, int InstID)
        {
            using (var Context = new EduSpecDataContext())
            {
                MemoryStream stream = new MemoryStream();
                XtraReport report = ReportsController.rptInvoice(InstID, InvoiceID);
                report.DisplayName = "Invoice";
                PdfExportOptions options = new PdfExportOptions() { ShowPrintDialogOnOpen = true };
                report.ExportToPdf(stream, options);
                stream.Seek(0, SeekOrigin.Begin);
                return new FileStreamResult(stream, "application/pdf");
            }
        }

        public ActionResult GenerateNewInvoice()
        {
            using (var Context = new EduSpecDataContext())
            {
                MemoryStream stream = new MemoryStream();
                XtraReport report = ReportsController.rptInvoice(Convert.ToInt32(HttpContext.Session["InstID"]), Convert.ToInt32(HttpContext.Session["InvoiceID"]));
                report.DisplayName = "Invoice";
                PdfExportOptions options = new PdfExportOptions() { ShowPrintDialogOnOpen = true };
                report.ExportToPdf(stream, options);
                stream.Seek(0, SeekOrigin.Begin);
                return new FileStreamResult(stream, "application/pdf");
            }
        }

        #endregion

        #region User Add

        public ViewResult UserAdd()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("System Admin - User Add", WebSecurity.CurrentUserId);
                return View("UserAdd");
            }
        }

        public ActionResult UserAddPartial()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("System Admin - User Add", WebSecurity.CurrentUserId);
                return PartialView("UserAddPartial", Context.Get_SystemAdmin_UserAdd_View().ToList());
            }
        }
        //
        [HttpPost] 
        public ActionResult AddNewUser(Get_SystemAdmin_UserAdd_ViewResult User)
        {
            using (var Context = new EduSpecDataContext())
            {
                WebSecurity.CreateUserAndAccount(User.Email, User.Password, new
                {
                    FullName = User.FullName,
                    Surname = User.Surname,
                    Email = User.Email,
                    InstID = -1,
                    AccessLevelID = User.AccessLevelID,
                    DepartmentID = User.DepartmentID,
                    DebtCollectorID = User.DebtCollectorID,
                    IsActive = User.IsActive,
                    CanAddNewApplication = 0,
                    CanSubmitApplication = 0,
                    CanProcessApplication = 0,
                    CanFinaliseApplication = 0,
                    CanManageSettings = 0,
                    PrintOnFinalise = 0,
                    UserTypeID = 3,
                    IsInstitution = false
                });
                Roles.AddUserToRole(User.Email, "ActiveUser");

                return UserAddPartial();
            }
        }

        [HttpPost]
        public ActionResult UserUpdate(Get_SystemAdmin_UserAdd_ViewResult _User)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_SystemAdmin_User_Update(
                    _User.UserId,
                    _User.FullName,
                    _User.Surname,
                    _User.Email,
                    _User.AccessLevelID,
                    _User.DepartmentID,
                    _User.DebtCollectorID,
                    _User.IsActive,
                    WebSecurity.CurrentUserId
                    );

                AccountController.setActivUserRole(_User.UserName, _User.IsActive);

                return UserAddPartial();
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

    }
}
