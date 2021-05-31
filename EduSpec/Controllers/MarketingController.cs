using EduSpec.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using WebMatrix.WebData;


namespace EduSpec.Controllers
{
    [SessionExpire]
    public class MarketingController : Controller
    {
        #region Institutions
        public ActionResult Institutions()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("Marketing - Institutions", WebSecurity.CurrentUserId);
                return View("Institutions", Context.Get_Marketing_Institutions_View().ToList());
            }
        }

        public PartialViewResult InstitutionsPartial()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("Marketing - Institutions", WebSecurity.CurrentUserId);
                return PartialView("InstitutionsPartial", Context.Get_Marketing_Institutions_View().ToList());
            }
        }

        public PartialViewResult InstitutionHistoryPartial(int? InstID)
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["HistoryViewProperties"] = ViewProperties.viewProperties("Marketing - Institution History", WebSecurity.CurrentUserId);
                return PartialView("InstitutionHistoryPartial", Context.Get_Marketing_InstitutionHistory_View(InstID).ToList());
            }
        }

        public PartialViewResult InstitutionContactsPartial(int? InstID)
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ContactInfoViewProperties"] = ViewProperties.viewProperties("Marketing - Institution Contacts", WebSecurity.CurrentUserId);
                return PartialView("InstitutionContactsPartial", Context.Get_Marketing_InstitutionContacts_View(InstID).ToList());
            }
        }

        [HttpPost]
        public JsonResult NewNoteAdd(DateTime Date, string NewNote, int InstID)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_Marketing_NewNote_Add(
                    WebSecurity.CurrentUserId,
                    InstID,
                    NewNote,
                    Date
                    );
            }
            return Json(new { Success = true });
        }

        [HttpPost]
        public ActionResult InstitutionContactUpdate(Get_Marketing_InstitutionContacts_ViewResult Contact)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_Marketing_InstitutionContact_Update(
                    Contact.InstContactID,
                    Contact.InstID,
                    Contact.TitleID,
                    Contact.Name,
                    Contact.Surname,
                    Contact.Cell,
                    Contact.Email,
                    Contact.JobDescription
                    );

                return InstitutionContactsPartial(Contact.InstID);
            }
        }

        

        [HttpPost]
        public ActionResult InstitutionContactAdd(Get_Marketing_InstitutionContacts_ViewResult Contact)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_Marketing_InstitutionContact_Add(
                    Contact.InstID,
                    Contact.TitleID,
                    Contact.Name,
                    Contact.Surname,
                    Contact.Cell,
                    Contact.Email,
                    Contact.JobDescription
                    );

                return InstitutionContactsPartial(Contact.InstID);
            }
        }

        [HttpPost]
        public ActionResult InstitutionUpdate(Get_Marketing_Institutions_ViewResult Inst)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_Marketing_Institution_Update(
                    Inst.InstID,
                    Inst.InstitutionStatusID,
                    Inst.Name,
                    Inst.Email,
                    Inst.Telephone,
                    Inst.Cell
                    );

                return InstitutionsPartial();
            }
        }

        #endregion

        #region Institution

        public ViewResult Scheduler()
        {
            return View();
        }

        public PartialViewResult SchedularPartial()
        {
            return PartialView();
        }

        #endregion
    }
}