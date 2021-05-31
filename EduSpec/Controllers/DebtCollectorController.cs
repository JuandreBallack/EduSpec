using System;
using System.Linq;
using System.Web.Mvc;
using WebMatrix.WebData;
using EduSpec.Models;
using DevExpress.XtraReports.UI;

namespace EduSpec.Controllers
{
    public class DebtCollectorController : Controller
    {

        #region Age Analysis
        public ViewResult AgeAnalysis()
        {
            using (var Context = new EduSpecDataContext())
            {
                HttpContext.Session.Add("InstListID", Context.Get_DebtCollector_InstID(WebSecurity.CurrentUserId));
                ViewData["ViewProperties"] = ViewProperties.viewProperties("Debt Collector - Age Analysis", WebSecurity.CurrentUserId);
                return View("AgeAnalysis");
            }
        }

        public PartialViewResult AgeAnalysisPartial(int? YearID, int? InstID)
        {
            using (var Context = new EduSpecDataContext())
            {
                HttpContext.Session["InstListID"] = InstID;
                HttpContext.Session["cbYearSelectedIndex"] = YearID;
                HttpContext.Session["BulkSMSTypeID"] = 2;
                HttpContext.Session["BulkEmailTypeID"] = 2;
                ViewData["ViewProperties"] = ViewProperties.viewProperties("Debt Collector - Age Analysis", WebSecurity.CurrentUserId);
                return PartialView("AgeAnalysisPartial", Context.Get_DebtCollector_AgeAnalysis_View(YearID ?? Context.fn_Get_YearID(DateTime.Now), InstID ?? -1).ToList());
            }
        }

        public PartialViewResult AgeAnalysisHistoryPartial(int? FamilyID)
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["AgeAnalysisHistoryViewProperties"] = ViewProperties.viewProperties("Debt Collector - Age Action History", WebSecurity.CurrentUserId);
                return PartialView("AgeAnalysisHistoryPartial", Context.Get_DebtCollector_AgeActionHistory_View(FamilyID).ToList());
            }
        }

        public ActionResult getLetter(string returnUrl)
        {
            return Redirect(returnUrl);
        }

        public PartialViewResult reportParentAccountHistory(int FamilyID)
        {
            XtraReport report = new ParentAccountHistory();
            report.Parameters["FamilyID"].Value = FamilyID;
            report.Parameters["InstID"].Value = HttpContext.Session["InstListID"];
            report.Parameters["YearID"].Value = HttpContext.Session["cbYearSelectedIndex"];
            report.CreateDocument();
            return PartialView("reportParentAccountHistory", report);
        }

        public PartialViewResult reportSMSMessageHistory()
        {
            DateTime DateFrom = DateTime.Now.AddDays(-20);
            DateTime DateTo = DateTime.Now;

            XtraReport report = new SMSHistory();
            report.Parameters["InstID"].Value = HttpContext.Session["InstListID"];
            report.Parameters["DateFrom"].Value = DateFrom;
            report.Parameters["DateTo"].Value = DateTo;
            report.Parameters["BulkSMSTypeID"].Value = HttpContext.Session["BulkSMSTypeID"];
            report.CreateDocument();
            return PartialView("reportSMSMessageHistory", report);
        }

        public PartialViewResult reportEmailHistory()
        {
            DateTime DateFrom = DateTime.Now.AddDays(-20);
            DateTime DateTo = DateTime.Now;

            XtraReport report = new EmailHistory();
            report.Parameters["InstID"].Value = HttpContext.Session["InstListID"];
            report.Parameters["DateFrom"].Value = DateFrom;
            report.Parameters["DateTo"].Value = DateTo;
            report.Parameters["BulkEmailTypeID"].Value = HttpContext.Session["BulkEmailTypeID"];
            report.CreateDocument();
            return PartialView("reportEmailHistory", report);
        }

        #endregion
    }
}