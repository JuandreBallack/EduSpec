using DevExpress.Web;
using DevExpress.Web.Mvc;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using EduSpec.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace EduSpec.Controllers
{
    [SessionExpire]
    public class InstitutionController : Controller
    {
        public class DropdownList
        {
            public int ID { get; set; }
            public string FamilyCode { get; set; }
            public string ExternalAccNo { get; set; }
            public string Surname { get; set; }
        }

        #region Learners

        public ViewResult Learners()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("Institution - Learners", WebSecurity.CurrentUserId);
                return View("Learners");
            }
        }

        public ActionResult LearnersPartial()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("Institution - Learners", WebSecurity.CurrentUserId);
                return PartialView("LearnersPartial", Context.Get_Institution_Learners_View(Convert.ToInt32(HttpContext.Session["InstID"])).ToList());
            }
        }

        [HttpPost]
        public ActionResult InstitutionLearnersUpdate(Get_Institution_Learners_ViewResult Learners)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_Institutions_Learner_Update(
                    Learners.FamilyID,
                    Convert.ToInt32(HttpContext.Session["InstID"]),
                    Learners.LearnerID,
                    Learners.ExternalAccNo,
                    Learners.AdmissionNo,
                    Learners.ClassID,
                    Learners.GradeID,
                    Learners.OverrideFee,
                    Learners.GenderID,
                    Learners.TitleID,
                    Learners.Initials,
                    Learners.FirstName,
                    Learners.Surname,
                    Learners.EntryDate,
                    Learners.TerminationDate,
                    Learners.IDNo,
                    Learners.DateOfBirth,
                    Learners.LanguageID,
                    Learners.TelNo,
                    Learners.CellNo,
                    Learners.Email,
                    WebSecurity.CurrentUserId
                    );

                return LearnersPartial();
            }
        }              

        public ActionResult LearnerNewFamilyPicklist(int? LearnerID)
        {
            HttpContext.Session.Add("LearnerID", LearnerID ?? -1);
            return PartialView();
        }
        public PartialViewResult LearnerClassPickList(int? GradeID, int? ClassID)
        {
            ViewData["GradeID"] = GradeID ?? -1;
            return PartialView("LearnerClassPickList", ClassID);
        }

        public static List<DropdownList> getNewFamilyList(ListEditItemsRequestedByFilterConditionEventArgs Args)
        {
            using (var Context = new EduSpecDataContext())
            {
                return Context.ExecuteQuery<DropdownList>(
                    String.Format("EXEC Get_Institution_LearnerNewFamily_PickList {0},{1},'{2}'", 
                        Convert.ToInt32(System.Web.HttpContext.Current.Session["InstID"]), 
                        Convert.ToInt32(System.Web.HttpContext.Current.Session["LearnerID"]), 
                        Args.Filter)).ToList();
            }
        }

        public static List<DropdownList> getNewFamilyListByFamilyID(ListEditItemRequestedByValueEventArgs Args)
        {
            return getNewFamilyListValue(Args.Value);
        }

        public static List<DropdownList> getNewFamilyListValue(object Value)
        {
            if (Value != null && Value is Int32)
            {
                int FamilyID = (int)Value;
                using (var Context = new EduSpecDataContext())
                {
                    var toReturn = Context.ExecuteQuery<DropdownList>(String.Format("EXEC Get_Institution_NewFamilyByFamilyID_PickList {0}", FamilyID)).ToList();
                    return null;
                }
            }
            else
                return null;
        }

        [HttpPost]
        public ActionResult MoveLearner(int LearnerID, int NewFamilyID, int OldFamilyID, bool IsDeactivateOldFamily)
        {
            using (var Context = new EduSpecDataContext())
            {
                try
                {
                    Context.Set_Institution_MoveLearner_Update(
                            Convert.ToInt32(HttpContext.Session["InstID"]),
                            LearnerID,
                            NewFamilyID,
                            OldFamilyID,
                            IsDeactivateOldFamily,
                            WebSecurity.CurrentUserId);
                    return Json(new { Success = true, error = "" });
                }
                catch (Exception e)
                {
                    return Json(new { Success = false, error = e.Message });
                }
            }
        }
               

        #endregion

        #region Family

        public ViewResult Family()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("Institution - Family", WebSecurity.CurrentUserId);
                return View("Family");
            }
        }

        public PartialViewResult FamilyPartial(int? FamilyVisibleTypeID)
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("Institution - Family", WebSecurity.CurrentUserId);
                return PartialView("FamilyPartial", Context.Get_Institution_Families_View(Convert.ToInt32(HttpContext.Session["InstID"]), FamilyVisibleTypeID ?? 2).ToList());
            }
        }
        public PartialViewResult FamilyLearnersPartial(int? FamilyID)
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["FamilyLearnersViewProperties"] = ViewProperties.viewProperties("Institution - Family Learners");
                return PartialView("FamilyLearnersPartial", Context.Get_Institution_FamilyLearners_View(FamilyID).ToList());
            }
        }

        public PartialViewResult CombineFamiliesPartial(int? FamilyID)
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["FamilyLearnersViewProperties"] = ViewProperties.viewProperties("Institution - Family Learners");
                return PartialView("FamilyLearnersPartial", Context.Get_Institution_FamilyLearners_View(FamilyID).ToList());
            }
        }

        public JsonResult printFamilyDetailReport(int FamilyID)
        {
            using (var Context = new EduSpecDataContext())
            {
                var redirectUrl = new UrlHelper(Request.RequestContext).Action("printFamilyDetail", "Institution", new { FamilyID });
                return Json(new { Url = redirectUrl });
            }
        }

        public ActionResult printFamilyDetail(int FamilyID)
        {
            using (var Context = new EduSpecDataContext())
            {
                MemoryStream stream = new MemoryStream();
                XtraReport report = ReportsController.rptFamilyDetail(FamilyID, Convert.ToInt32(HttpContext.Session["InstID"]));
                report.DisplayName = "Family detail";
                PdfExportOptions options = new PdfExportOptions() { ShowPrintDialogOnOpen = false };
                report.ExportToPdf(stream, options);
                stream.Seek(0, SeekOrigin.Begin);
                return new FileStreamResult(stream, "application/pdf");
            }
        }

        #endregion

        #region Classes

        public ViewResult Classes()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("Institution - Classes", WebSecurity.CurrentUserId);
                return View("Classes");
            }
        }

        public ActionResult ClassesPartial()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("Institution - Classes", WebSecurity.CurrentUserId);
                return PartialView("ClassesPartial", Context.Get_Institution_Classes_View(Convert.ToInt32(HttpContext.Session["InstID"])).ToList());
            }
        }

        [HttpPost]
        public ActionResult InstitutionClassUpdate(Get_Institution_Classes_ViewResult classes)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_Institutions_Classes_Update(
                        classes.InstID,
                        classes.ClassID,
                        classes.GradeID,
                        classes.Description_ENG,
                        classes.Description_AFR,
                        WebSecurity.CurrentUserId
                    );
                return ClassesPartial();
            }
        }

        [HttpPost]
        public ActionResult InstitutionClassAdd(Get_Institution_Classes_ViewResult classes)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_Institutions_Classes_Add(
                        Convert.ToInt32(HttpContext.Session["InstID"]),
                        classes.GradeID,
                        classes.Description_ENG,
                        classes.Description_AFR,
                        WebSecurity.CurrentUserId
                    );

                return ClassesPartial();
            }
        }

        [HttpPost]
        public ActionResult InstitutionClassDelete(Get_Institution_Classes_ViewResult classes)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_Institutions_Class_Delete(
                        Convert.ToInt32(HttpContext.Session["InstID"]),
                        classes.ClassID,
                        WebSecurity.CurrentUserId
                    );

                return ClassesPartial();
            }
        }
        #endregion

        #region External account numbers

        public ActionResult ExternalAccountNumbers()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("Institution - External account numbers", WebSecurity.CurrentUserId);
                return View("ExternalAccountNumbers");
            }
        }

        public PartialViewResult ExternalAccountNumbersPartial()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("Institution - External account numbers", WebSecurity.CurrentUserId);
                return PartialView("ExternalAccountNumbersPartial", Context.Get_Institution_ExternalAccountNumbers_View(Convert.ToInt32(HttpContext.Session["InstID"])).ToList());
            }
        }

        [HttpPost]
        public ActionResult UpdateExternalAccNumbers(MVCxGridViewBatchUpdateValues<Get_Institution_ExternalAccountNumbers_ViewResult, int> updateValues)
        {
            using (var Context = new EduSpecDataContext())
            {
                foreach (var Item in updateValues.Update)
                {
                    if (updateValues.IsValid(Item))
                    {

                        Context.Set_Institution_ExternalAccountNumbers_Update(
                        Item.FamilyID,
                        Item.ExternalAccNo
                        );
                    }
                }
            }

            return ExternalAccountNumbersPartial();

        }

        #endregion

        #region Ad Hoc fees

        public ViewResult AdHocFees()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("Institution - Ad Hoc fees Learners", WebSecurity.CurrentUserId);
                return View("AdHocFees");
            }
        }

        public PartialViewResult AdHocFeesPartial(int? YearID)
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("Institution - Ad Hoc fees Learners", WebSecurity.CurrentUserId);
                return PartialView("AdHocFeesPartial", Context.Get_Institution_AdHocFees_View(Convert.ToInt32(HttpContext.Session["InstID"]), YearID ?? Context.fn_Get_YearID(DateTime.Now)).ToList());
            }
        }

        public PartialViewResult LearnerAdHocFees()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["AdHocFeesViewProperties"] = ViewProperties.viewProperties("Institution - Ad Hoc Fees", WebSecurity.CurrentUserId);
                return PartialView("LearnerAdHocFees", Context.Get_Institution_LearnersAdHocFees_View(Convert.ToInt32(HttpContext.Session["InstID"])).ToList());
            }
        }

        [HttpPost]
        public JsonResult SetLearnerAdHocFees(string LearnerList, int AdHocFeesID)
        {
            using (var Context = new EduSpecDataContext() { CommandTimeout = 5 * 60 })
            {
                Context.Set_Institution_LearnerAdHocFees_Add(LearnerList, AdHocFeesID, Convert.ToInt32(HttpContext.Session["InstID"]), WebSecurity.CurrentUserId);
            }
            return Json(new { result = "Success" });
        }

        [HttpPost]
        public ActionResult SetLearnerAdHocFeesUpdate(Get_Institution_AdHocFees_ViewResult SchoolFeeSettings)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_Institution_LearnerAdHocFees_Update(
                    SchoolFeeSettings.LearnerAdHocFeesID,
                    SchoolFeeSettings.Amount,
                    SchoolFeeSettings.AmountPaid,
                    WebSecurity.CurrentUserId
                    );
                return AdHocFeesPartial(SchoolFeeSettings.YearID);
            }
        }

        [HttpPost]
        public ActionResult SetLearnerAdHocFeesDelete(Get_Institution_AdHocFees_ViewResult SchoolFeeSettings)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_Institution_LearnerAdHocFees_Delete(
                    SchoolFeeSettings.LearnerAdHocFeesID,
                    WebSecurity.CurrentUserId
                    );
                return AdHocFeesPartial(SchoolFeeSettings.YearID);
            }
        }

        #endregion

        #region Reports

        public PartialViewResult reportFamilyDetail(int FamilyID)
        {
            XtraReport report = new FamilyDetail();
            report.Parameters["FamilyID"].Value = FamilyID;
            report.Parameters["InstID"].Value = HttpContext.Session["InstID"];
            report.CreateDocument();
            return PartialView("reportFamilyDetail", report);
        }

        public PartialViewResult reportAdHocFees(int YearID)
        {
            XtraReport report = new AdHocFees();
            report.Parameters["Year"].Value = YearID;
            report.Parameters["InstID"].Value = HttpContext.Session["InstID"];
            report.Parameters["FeeDescription"].Value = -1; //-1 = ALL 
            report.CreateDocument();
            return PartialView("reportAdHocFees", report);
        }

        #endregion


    }
}
