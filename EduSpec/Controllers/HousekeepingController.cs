using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EduSpec.Models;
using WebMatrix.WebData;

namespace EduSpec.Controllers
{
    [SessionExpire]
    public class HousekeepingController : Controller
    {
        public ActionResult AccessLevels()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("Housekeeping - Access Levels", WebSecurity.CurrentUserId);
                return View("AccessLevels");
            }
        }

        public PartialViewResult AccessLevelsPartial()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("Housekeeping - Access Levels", WebSecurity.CurrentUserId);
                return PartialView("AccessLevelsPartial", Context.Get_Housekeeping_AccessLevels_View().ToList());
            }
        }

        [HttpPost]
        public PartialViewResult AccessLevelsUpdate(Get_Housekeeping_AccessLevels_ViewResult AccessLevel)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_Housekeeping_AccessLevels_Update(
                    AccessLevel.AccessLevelID,
                    AccessLevel.Description,
                    AccessLevel.IsActive,
                    AccessLevel.IsSystemAdmin,
                    AccessLevel.IsAdmin,
                    AccessLevel.IsInstitution,
                    AccessLevel.IsExemptions,
                    AccessLevel.IsHousekeeping,
                    AccessLevel.IsLegalProcess,
                    AccessLevel.IsMarketing,
                    AccessLevel.IsIntegration,
                    AccessLevel.IsSupport,
                    WebSecurity.CurrentUserId);
                return AccessLevelsPartial();
            }
        }





        public ActionResult DepartmentAccess()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("Housekeeping - Departments Access", WebSecurity.CurrentUserId);
                return View("DepartmentAccess");
            }
        }

        public PartialViewResult DepartmentAccessPartial()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("Housekeeping - Departments Access", WebSecurity.CurrentUserId);
                return PartialView("DepartmentAccessPartial", Context.Get_Housekeeping_DepartmentAccess_View().ToList());
            }
        }

        public PartialViewResult DepartmentAccessSetupPartial(int? NavBarNodeID , Int16? AccessLevelID )
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewPropertiesDetail"] = ViewProperties.viewProperties("Housekeeping - Departments Access Setup", WebSecurity.CurrentUserId);
                return PartialView("DepartmentAccessSetupPartial", Context.Get_Housekeeping_DepartmentAccessSetup_View(NavBarNodeID, AccessLevelID).ToList());
            }
        }

        [HttpPost]
        public ActionResult UpdateDepartmentsAccess(Get_Housekeeping_DepartmentAccessSetup_ViewResult AccessLevel)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_Housekeeping_DepertmentAccess_Update(
                    AccessLevel.DepartmentID,
                    AccessLevel.AccessLevelID,
                    AccessLevel.NavBarNodeID,
                    AccessLevel.IsReadOnly,
                    AccessLevel.IsActive,
                    WebSecurity.CurrentUserId);
                return DepartmentAccessSetupPartial(AccessLevel.NavBarNodeID, AccessLevel.AccessLevelID);
            }
        }
    }
}
