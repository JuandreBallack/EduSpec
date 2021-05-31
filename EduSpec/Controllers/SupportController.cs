using DevExpress.Web.Mvc;
using EduSpec.Code;
using EduSpec.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace EduSpec.Controllers
{
    public class SupportController : Controller
    {
        #region Logged Cases

        public ViewResult LoggedCases()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("Support - LoggedCases", WebSecurity.CurrentUserId);
                return View("LoggedCases");
            }
        }

        public PartialViewResult LoggedCasesUsersPartial()
        {
            if (Convert.ToInt32(HttpContext.Session["ImpersonateInstUserID"]) == -1)
            {
                using (var Context = new EduSpecDataContext())
                {
                    ViewData["ViewProperties"] = ViewProperties.viewProperties("Support - LoggedCases Users", WebSecurity.CurrentUserId);
                    return PartialView("LoggedCasesUsersPartial", Context.Get_Support_LoggedCases_Users_View(Convert.ToInt32(HttpContext.Session["InstID"]), WebSecurity.CurrentUserId).ToList());
                }
            }
            else
            {
                using (var Context = new EduSpecDataContext())
                {
                    ViewData["ViewProperties"] = ViewProperties.viewProperties("Support - LoggedCases Users", WebSecurity.CurrentUserId);
                    return PartialView("LoggedCasesUsersPartial", Context.Get_Support_LoggedCases_Users_View(Convert.ToInt32(HttpContext.Session["InstID"]), Convert.ToInt32(HttpContext.Session["ImpersonateInstUserID"])).ToList());
                }
            }
        }

        public PartialViewResult LoggedCasesPartial()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("Support - LoggedCases", WebSecurity.CurrentUserId);
                return PartialView("LoggedCasesPartial", Context.Get_Support_LoggedCases_View().ToList());
            }
        }

        public PartialViewResult LoggedCasesHistoryPartial(int? SupportID)
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["HistoryViewProperties"] = ViewProperties.viewProperties("Support - Logged cases history", WebSecurity.CurrentUserId);
                return PartialView("LoggedCasesHistoryPartial", Context.Get_Support_LoggedCasesHistory_View(SupportID).ToList());
            }
        }

        public PartialViewResult AddNewCasePartial()
        {
            using (var Context = new EduSpecDataContext())
            {
                HttpContext.Session.Add("SupportAttachmentFileName", null);
                ViewData["ViewProperties"] = ViewProperties.viewProperties("Support - AddNewCase", WebSecurity.CurrentUserId);
                return PartialView("AddNewCasePartial", Context.Get_Support_LoggedCases_View().FirstOrDefault());
            }
        }

        public PartialViewResult EditCasePartial(int? SupportID, int? SupportTypeID)
        {
            using (var Context = new EduSpecDataContext())
            {
                HttpContext.Session.Add("SupportID", SupportID);
                ViewBag.SupportTypeID = SupportTypeID;
                var x = Context.Get_Support_EditLoggedCase_View(SupportID).FirstOrDefault();
                if (x != null)
                {
                    ViewBag.SupportStatusID = x.StatusID;
                }
                return PartialView("EditCasePartial", Context.Get_Support_EditLoggedCase_View(SupportID).FirstOrDefault());
            }
        }

        [HttpPost]
        public ActionResult InstitutionSupportAdd(int SupportCategoryID, int SupportTypeID, string CaseTitle, string CaseDescription)
        {
            using (var Context = new EduSpecDataContext())
            {
                var x = Context.Set_Support_LoggedCases_Add(
                             Convert.ToInt32(HttpContext.Session["InstID"]),
                             SupportCategoryID,
                             SupportTypeID,
                             WebSecurity.CurrentUserId,
                             CaseTitle,
                             CaseDescription,
                             Convert.ToString(HttpContext.Session["SupportAttachmentFileName"])
                             ).FirstOrDefault();

                Email.sendSingleEmail(
                        Convert.ToInt32(HttpContext.Session["InstID"]),
                        x.ToEmail,
                        x.CcEmail,
                        x.Subject,
                        x.Body,
                        null
                        );

                return Json(new { Success = true, });
            }
        }

        [HttpPost]
        public ActionResult InstitutionSupportUpdate(int SupportTypeID, string CaseTitle, string CaseDescription, string HistoryComment, string Resolution, int? ResolutionCategoryID)
        {
            using (var Context = new EduSpecDataContext())
            {
                var x = Context.Set_Support_LoggedCases_Update(
                             Convert.ToInt32(HttpContext.Session["SupportID"]),
                             Convert.ToInt32(HttpContext.Session["InstID"]),
                             WebSecurity.CurrentUserId,
                             SupportTypeID,
                             CaseTitle,
                             CaseDescription,
                             HistoryComment,
                             Resolution,
                             ResolutionCategoryID,
                             null
                             ).First();

                if (x.UserId == 1)
                {
                    Email.sendSingleEmail(
                        Convert.ToInt32(HttpContext.Session["InstID"]),
                        x.ToEmail,
                        null,
                        x.Subject,
                        x.Body,
                        null);
                }
                return Json(new { Success = true, });
            }
        }

        public ActionResult getInstitutionSupportFiles(string returnUrl)
        {
            return Redirect(returnUrl);
        }

        public ActionResult CallbacksSupportFileUpload()
        {
            try
            {
                UploadControlExtension.GetUploadedFiles("ucUploadData", UploadControlHelper.SupportDocsUploadValidationSettings, UploadControlHelper.ucSupportFileCallbacks_FileUploadComplete);
            }
            catch (Exception e)
            {
                string x = e.Message;
            }

            return null;
        }

        #endregion

        #region Tutorial videos

        public ViewResult TutorialVideos()
        {
            ViewData["ViewProperties"] = ViewProperties.viewProperties("Support - Tutorial Videos", WebSecurity.CurrentUserId);
            return View("TutorialVideos");
        }

        public PartialViewResult TutorialVideosPartial()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("Support - Tutorial Videos", WebSecurity.CurrentUserId);
                return PartialView("TutorialVideosPartial", Context.Get_Support_TutorialVideos_View().ToList());
            }
        }

        public ActionResult getTutorialVideos(string returnUrl)
        {
            return Redirect(returnUrl);
        }

        #endregion

    }
}