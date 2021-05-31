using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;
using EduSpec.Models;
using DevExpress.Web;
using System.IO;
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting;

namespace EduSpec.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        
        public class DropdownList
        {
            public int MasterInstID { get; set; }
            public string Institution { get; set; }
            public string Province { get; set; }
        }
        
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ActiveUser")]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                bool a = Roles.IsUserInRole(model.UserName, "ActiveUser");
                if (!Roles.IsUserInRole(model.UserName, "ActiveUser") && WebSecurity.UserExists(model.UserName))
                {
                    ModelState.AddModelError("CustomError", "This user account has been disabled. Please contact your system administrator.");
                    return View(model);
                }
                else if (WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe.Value))
                {
                    using (var Context = new EduSpecDataContext())
                    {
                        int UserTypeID = Context.fn_GetUserTypeIDFromUserName(model.UserName).Value;
                        int InstID = Context.fn_GetInstIDFromUserName(model.UserName).Value;

                        if (UserTypeID == 1 || UserTypeID == 2)
                        {

                            switch (Context.fn_GetInstitutionStatus(InstID).Value)
                            {
                                case 1:
                                    {
                                        WebSecurity.Logout();
                                        return RedirectToAction("LoginInformation", new
                                        {
                                            Title = "EduSpec Online registration",
                                            Message = "Your request for registration has been submitted and your institution will be registered once a validation process has been done. An agent will contact you shortly."
                                        });
                                    };
                                //break;
                                case 2:
                                    {
                                        WebSecurity.Logout();
                                        return RedirectToAction("LoginInformation", new
                                        {
                                            Title = "EduSpec Online registration",
                                            Message = "Your institution is currently pending activation and your institution will be activated once a validation process has been done. An agent will contact you shortly."
                                        });
                                    };
                                //break;
                                case 3:
                                case 6:
                                    {
                                        HttpContext.Session.Add("Username", model.UserName);
                                        HttpContext.Session.Add("InstID", InstID.ToString());
                                        HttpContext.Session.Add("InstitutionName", Context.fn_GetInstitutionName(InstID));
                                        HttpContext.Session.Add("ProcessScreen", "");
                                        HttpContext.Session.Add("IsEvaluation", Context.fn_IsEvaluationMode(InstID));
                                        HttpContext.Session.Add("CurrentYearID", Context.fn_Get_YearID(DateTime.Now));
                                        HttpContext.Session.Add("QuotationFileName", Context.fn_Get_ActiveQuotationFileName(InstID));
                                        HttpContext.Session.Add("IsInstQuotationActive", Context.fn_Get_IsInstQuotationActive(InstID));
                                        HttpContext.Session.Add("ImpersonateInstUserID", -1);
                                        HttpContext.Session.Add("ImpersonateInstUserName", "");
                                        HttpContext.Session.Add("ImpersonateInstID", -1);
                                        HttpContext.Session.Add("FamilyGuid", "");
                                        Context.Set_Accounts_UserLogin_Add(InstID, model.UserName);
                                        if (InstID == 1)
                                            return RedirectToAction("Institutions", "SystemAdmin");
                                        else
                                            return RedirectToAction("Exemptions", "Exemptions");
                                    }
                                case 4:
                                    {
                                        WebSecurity.Logout();
                                        return RedirectToAction("LoginInformation", new
                                        {
                                            Title = "EduSpec Online registration",
                                            Message = "Your institution is currently suspended. Please contact EduSpec for further information or enquires."
                                        });
                                    };
                                case 5:
                                    {
                                        WebSecurity.Logout();
                                        return RedirectToAction("LoginInformation", new
                                        {
                                            Title = "EduSpec Online registration",
                                            Message = "Your institution is currently not active. Please contact EduSpec for further information or enquires."
                                        });
                                    };
                            }
                        }
                        else if (UserTypeID == 3)
                        {
                            HttpContext.Session.Add("Username", model.UserName);
                            HttpContext.Session.Add("ImpersonateInstUserID", -1);
                            HttpContext.Session.Add("ImpersonateInstUserName", "");
                            HttpContext.Session.Add("CurrentYearID", Context.fn_Get_YearID(DateTime.Now));
                            HttpContext.Session.Add("DebtCollectorID", Context.fn_DebtCollectorIDFromUserName(model.UserName));
                            Context.Set_Accounts_UserLogin_Add(InstID, model.UserName);
                            if (InstID == 1)
                                return RedirectToAction("Institutions", "SystemAdmin");
                            else if (InstID == -1)
                                return RedirectToAction("AgeAnalysis", "DebtCollector");
                            else
                                return RedirectToAction("Exemptions", "Exemptions");
                        }

                    }
                }
                ModelState.AddModelError("CustomError", "User name or password provided is incorrect.");
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult ImpersonateInstitution(int InstID)
        {
            using (var Context = new EduSpecDataContext())
            {
                var InstitutionUser = Context.Get_Admin_InstitutionImpersonateUser(InstID).FirstOrDefault();
                HttpContext.Session["InstID"] = InstID;
                HttpContext.Session["InstitutionName"] = InstitutionUser.InstName;
                HttpContext.Session.Add("ImpersonateInstUserID", InstitutionUser.UserId);
                HttpContext.Session.Add("ImpersonateInstUserName", InstitutionUser.UserName);
                HttpContext.Session.Add("IsEvaluation", Context.fn_IsEvaluationMode(InstID));
                HttpContext.Session.Add("IsInstQuotationActive", Context.fn_Get_IsInstQuotationActive(InstID));
                HttpContext.Session.Add("QuotationFileName", Context.fn_Get_ActiveQuotationFileName(InstID));
                var redirectUrl = new UrlHelper(Request.RequestContext).Action("Exemptions", "Exemptions");
                return Json(new { Url = redirectUrl });
            }
        }

        public ActionResult EndImpersonateInstitution()
        {
            using (var Context = new EduSpecDataContext())
            {
                HttpContext.Session["ImpersonateInstUserID"] = -1;
                HttpContext.Session["ImpersonateInstID"] = "";
                HttpContext.Session.Add("IsEvaluation", Context.fn_IsEvaluationMode(1));
                HttpContext.Session.Add("IsInstQuotationActive", Context.fn_Get_IsInstQuotationActive(1));
                return RedirectToAction("InstitutionImpersonate", "SystemAdmin");
            }
        }

        [AllowAnonymous]
        public ActionResult LoginInformation(string Title, string Message)
        {
            ViewBag.Title = Title;
            ViewBag.Message = Message;

            return View();
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            using (var Context = new EduSpecDataContext())
            {
                return View("Register", Context.Get_Account_NewInstitutions_View().FirstOrDefault());
            }

        }

        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public JsonResult Register(Get_Account_NewInstitutions_ViewResult NewInstitute)
        {
            if (WebSecurity.UserExists(NewInstitute.Email) != true)
            {
                try
                {
                    using (var Context = new EduSpecDataContext())
                    {
                        int InstID = Context.Set_Registration_NewInstitution_Add(
                            NewInstitute.MasterInstID,
                            NewInstitute.EMISNo,
                            NewInstitute.QuintileID,
                            NewInstitute.PhaseID,
                            NewInstitute.PhysAddress1,
                            NewInstitute.PhysAddress2,
                            NewInstitute.PhysAddress3,
                            NewInstitute.PhysProvinceID,
                            NewInstitute.PhysCode,
                            NewInstitute.PosAddress1,
                            NewInstitute.PosAddress2,
                            NewInstitute.PosAddress3,
                            NewInstitute.PosProvinceID,
                            NewInstitute.PosCode,
                            NewInstitute.Telephone,
                            NewInstitute.Cell,
                            NewInstitute.Fax,
                            NewInstitute.InstEmail,
                            NewInstitute.IsRequestEvaluation,
                            NewInstitute.CorrespondenceLanguageID,
                            NewInstitute.GradeFromID,
                            NewInstitute.GradeToID,
                            NewInstitute.IntegrationTypeID).FirstOrDefault().InstID;

                        if (InstID != -1)
                        {
                            WebSecurity.CreateUserAndAccount(NewInstitute.Email, NewInstitute.Password, new
                            {
                                NewInstitute.FullName,
                                NewInstitute.Surname,
                                NewInstitute.Email,
                                InstID,
                                AccessLevelID = 3,
                                DepartmentID = 1,
                                IsActive = 1,
                                IsInstitution = true,
                                IsImpersonationUser = true
                            });

                            Roles.AddUserToRole(NewInstitute.Email, "ActiveUser");
                        }
                    }
                    var redirectUrl = new UrlHelper(Request.RequestContext).Action("LoginInformation", new
                    {
                        Title = "EduSpec Online registration",
                        Message = "Your request for registration has been submitted and your institution will be registered once a validation process has been done. An agent will contact you shortly."
                    });
                    return Json(new { Success = true, Url = redirectUrl });
                }
                catch (MembershipCreateUserException e)
                {
                    return Json(new { Success = false, Message = MembershipStatus.ErrorCodeToString(e.StatusCode), Url = "" });
                }
            }
            return Json(new { Success = false, Message = "The user already exist.", Url = "" });

        }

        public ActionResult LogOff()
        {
            WebSecurity.Logout();
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public ActionResult CheckUsername(string userName)
        {
            var status = WebSecurity.UserExists(userName);
            return Json(new
            { result = status });
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model, int userID, string NewPassword, string ConfirmPassword)
        {
            if (ModelState.IsValid)
            {
                bool changePasswordSucceeded;
                String Token;
                try
                {
                    var provider = (SimpleMembershipProvider)Membership.Provider;
                    var username = provider.GetUserNameFromId(userID);

                    Token = WebSecurity.GeneratePasswordResetToken(username);
                    changePasswordSucceeded = WebSecurity.ResetPassword(Token, NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }
                return Json(new
                { result = changePasswordSucceeded });
            }
            return Json(new
            { result = false });
        }

        #region

        [AllowAnonymous]
        public ActionResult MasterInstitutionPickList()
        {
            return PartialView("MasterInstitutionPickList");
        }

        [AllowAnonymous]
        public static List<DropdownList> getMasterInstitutions(ListEditItemsRequestedByFilterConditionEventArgs Args)
        {
            using (var Context = new EduSpecDataContext())
            {
                return Context.ExecuteQuery<DropdownList>(String.Format("EXEC Get_MasterInstitution_PickList '{0}'", Args.Filter)).ToList();
            }
        }

        public static List<DropdownList> getMasterInstitutionByMasterInstID(ListEditItemRequestedByValueEventArgs Args)
        {
            return getMasterInstitutionsByValue(Args.Value);
        }

        public static List<DropdownList> getMasterInstitutionsByValue(object Value)
        {
            if (Value != null && Value is Int32)
            {

                int MasterInstID = (int)Value;
                using (var Context = new EduSpecDataContext())
                {
                    var toReturn = Context.ExecuteQuery<DropdownList>(String.Format("EXEC Get_MasterInstitutionByMasterInstID_PickList {0}", MasterInstID)).ToList();
                    return toReturn;
                }
            }
            else
                return null;
        }

        #endregion

        public static void setActivUserRole(string UserName, bool IsActive)
        {
            if (IsActive && !Roles.IsUserInRole(UserName, "ActiveUser"))
                Roles.AddUserToRole(UserName, "ActiveUser");

            if (!IsActive && Roles.IsUserInRole(UserName, "ActiveUser"))
                Roles.RemoveUserFromRole(UserName, "ActiveUser");
        }

        //[HttpPost]
        //public JsonResult Set_GenerateQuotation(int ItemID, int PackageID)
        //{
        //    using (var Context = new EduSpecDataContext())
        //    {
        //        try
        //        {
        //           int? QuotationID = Context.Set_Quotation_Add(
        //                ItemID,
        //                Convert.ToInt32(HttpContext.Session["InstID"]),
        //                PackageID).FirstOrDefault().QuotationID;
        //           var redirectUrl = new UrlHelper(Request.RequestContext).Action("printQuotation", "Account", new { QuotationID });
        //            return Json(new { Success = true, error = "", Url = redirectUrl });

        //        }
        //        catch (Exception e)
        //        {
        //            return Json(new { Success = false, error = e.Message, Url = "" });
        //        }

        //    }

        //}
        public ActionResult printQuotation(int QuotationID)
        {
            using (var Context = new EduSpecDataContext())
            {
                HttpContext.Session.Add("IsInstQuotationActive", Context.fn_Get_IsInstQuotationActive(Convert.ToInt32(HttpContext.Session["InstID"])));
                HttpContext.Session.Add("QuotationFileName", Context.fn_Get_ActiveQuotationFileName(Convert.ToInt32(HttpContext.Session["InstID"])));
                MemoryStream stream = new MemoryStream();
                XtraReport report = ReportsController.rptQuotation(Convert.ToInt32(HttpContext.Session["InstID"]), QuotationID);
                report.DisplayName = "Quotation";
                PdfExportOptions options = new PdfExportOptions() { ShowPrintDialogOnOpen = true };
                report.ExportToPdf(stream, options);
                stream.Seek(0, SeekOrigin.Begin);
                return new FileStreamResult(stream, "application/pdf");
            }
        }


        public int InstID()
        {
            return Convert.ToInt32(HttpContext.Session["InstID"]);
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
    }
}
