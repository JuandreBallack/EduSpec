using DevExpress.Web.Mvc;
using EduSpec.Code;
using EduSpec.Models;
using NLog;
using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace EduSpec.Controllers
{
    [SessionExpire]
    public class IntegrationController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        #region 
        public ActionResult Integration()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("Exemptions - Families", WebSecurity.CurrentUserId);
                return View("Integration");
            }
        }
        public PartialViewResult Partial()
        {
            using (var Context = new EduSpecDataContext())
            {
                return PartialView("Partial");
            }
        }

        public PartialViewResult Parents(int? IntegrationStatus)
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ParentsViewProperties"] = ViewProperties.viewProperties("Integration - Parents", WebSecurity.CurrentUserId);
                return PartialView("Parents", Context.Get_Integration_Parents_View(Convert.ToInt32(HttpContext.Session["InstID"]), IntegrationStatus).ToList());
            }
        }

        public PartialViewResult Learners(int? IntegrationStatus)
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["LearnersViewProperties"] = ViewProperties.viewProperties("Integration - Learners", WebSecurity.CurrentUserId);
                return PartialView("Learners", Context.Get_Integration_Learners_View(Convert.ToInt32(HttpContext.Session["InstID"]), IntegrationStatus).ToList());
            }
        }
        public PartialViewResult LinkClasses()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["LinkClassesViewProperties"] = ViewProperties.viewProperties("Integration - Classes", WebSecurity.CurrentUserId);
                return PartialView("LinkClasses", Context.Get_Integration_Link_LearnerClasses_View(Convert.ToInt32(HttpContext.Session["InstID"])).ToList());
            }
        }
        [HttpPost]
        public PartialViewResult updateLinkClasses(Get_Integration_Link_LearnerClasses_ViewResult result)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_Integration_Link_LearnerClasses_Update(result.AutoIndex, result.ClassID);
                return LinkClasses();
            }
        }

        public PartialViewResult LinkLanguages()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["LinkLanguagesViewProperties"] = ViewProperties.viewProperties("Integration - Language", WebSecurity.CurrentUserId);
                return PartialView("LinkLanguages", Context.Get_Integration_Link_Language_View(Convert.ToInt32(HttpContext.Session["InstID"])).ToList());
            }
        }
        [HttpPost]
        public PartialViewResult updateLinkLanguages(Get_Integration_Link_Language_ViewResult result)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_Integration_Link_Language_Update(result.AutoIndex, result.LanguageID);
                return LinkLanguages();
            }
        }
        public PartialViewResult LinkMaritalSatus()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["LinkMaritalSatusViewProperties"] = ViewProperties.viewProperties("Integration - Marital Status", WebSecurity.CurrentUserId);
                return PartialView("LinkMaritalSatus", Context.Get_Integration_Link_MaritalStatus_View(Convert.ToInt32(HttpContext.Session["InstID"])).ToList());
            }
        }
        [HttpPost]
        public PartialViewResult updateLinkMaritalSatus(Get_Integration_Link_MaritalStatus_ViewResult result)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_Integration_Link_MaritalStatus_Update(result.AutoIndex, result.MaritalStatusID);
                return LinkMaritalSatus();
            }
        }
        public PartialViewResult LinkTitles()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["LinkTitlesViewProperties"] = ViewProperties.viewProperties("Integration - Titles", WebSecurity.CurrentUserId);
                return PartialView("LinkTitles", Context.Get_Integration_Link_Titles_View(Convert.ToInt32(HttpContext.Session["InstID"])).ToList());
            }
        }
        [HttpPost]
        public PartialViewResult updateLinkTitles(Get_Integration_Link_Titles_ViewResult result)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_Integration_Link_Titles_Update(result.AutoIndex, result.TitleID);
                return LinkTitles();
            }
        }
        public PartialViewResult LinkNationalities()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["LinkNationalitiesViewProperties"] = ViewProperties.viewProperties("Integration - Nationalities", WebSecurity.CurrentUserId);
                return PartialView("LinkNationalities", Context.Get_Integration_Link_Nationalities_View(Convert.ToInt32(HttpContext.Session["InstID"])).ToList());
            }
        }
        [HttpPost]
        public PartialViewResult updateLinkNationalities(Get_Integration_Link_Nationalities_ViewResult result)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_Integration_Link_Nationalities_Update(result.AutoIndex, result.NationalityID);
                return LinkNationalities();
            }
        }
        public PartialViewResult LinkParentRelation()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["LinkParentRelationViewProperties"] = ViewProperties.viewProperties("Integration - ParentRelation", WebSecurity.CurrentUserId);
                return PartialView("LinkParentRelation", Context.Get_Integration_Link_ParentRelation_View(Convert.ToInt32(HttpContext.Session["InstID"])).ToList());
            }
        }
        [HttpPost]
        public PartialViewResult updateLinkParentRelation(Get_Integration_Link_ParentRelation_ViewResult result)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_Integration_Link_ParentRelation_Update(result.AutoIndex, result.ParentRelationID);
                return LinkParentRelation();
            }
        }
        public ActionResult CallbacksFileUpload()
        {
            string FileName = UploadControlExtension.GetUploadedFiles("ucUploadData", UploadControlHelper.DataImportValidationSettings, UploadControlHelper.Integration_ImportSASAMS).FirstOrDefault().FileName;
            //string resultFilePath = String.Format("{0}{1}", SystemParameters.SystemParams().InstitutionDataUploadFilePath, FileName);

            return null;
        }
                
        [HttpPost]
        public JsonResult ProcessLearnerInformation(string LearnersList, int IntegrationStatusID)
        {
            using (var Context = new EduSpecDataContext() { CommandTimeout = 5 * 60 })
            {                
                if (IntegrationStatusID == 1)
                {
                    Context.Set_Integration_Learners_Add(Convert.ToInt32(HttpContext.Session["InstID"]), LearnersList, IntegrationStatusID);
                }
                else
                {
                    Context.Set_Integration_Learners_Update(Convert.ToInt32(HttpContext.Session["InstID"]), LearnersList, IntegrationStatusID);
                }                
            }
            return Json(new { result = "Success" });
        }

        [HttpPost]
        public JsonResult ProcessParentsInformation(string ParentsList, int IntegrationStatusID)
        {
            using (var Context = new EduSpecDataContext() { CommandTimeout = 5 * 60 })
            {
                Context.Set_Integration_Parents_Update(Convert.ToInt32(HttpContext.Session["InstID"]), ParentsList, IntegrationStatusID);
            }
            return Json(new { result = "Success" });
        }

        [HttpPost]
        public JsonResult importData(string resultFilePath)
        {   
            try
            {
                using (var Context = new EduSpecDataContext())
                {
                    MappedDiagnosticsLogicalContext.Set("Institution", Context.fn_GetInstitutionName(Convert.ToInt32(HttpContext.Session["InstID"])));

                    logger.Info("Using Database - " + resultFilePath);

                    logger.Info("Clear Import Tables.");
                    Context.Integration_SASAMS_ClearImportTables(Convert.ToInt32(HttpContext.Session["InstID"]));
                    logger.Info("Clear Import Tables .. Done");

                    logger.Info("Import Pearents.");
                    BulkCopyAccessToSQLServer(AccessCommands.SASAMS_Import(Convert.ToInt32(HttpContext.Session["InstID"])), "Integration_SASAMS_Import", resultFilePath);
                    logger.Info("Import Pearents .. Done.");

                    logger.Info("Exec Integration_SASAMS_Import_Link " + HttpContext.Session["InstID"]);
                    Context.Integration_SASAMS_Import_Link(Convert.ToInt32(HttpContext.Session["InstID"]));
                    logger.Info("Exec Integration_SASAMS_Import_Link " + HttpContext.Session["InstID"]);

                    //logger.Info("Import Classes.");
                    //BulkCopyAccessToSQLServer(AccessCommands.SASAMS_LearnerClasses(Convert.ToInt32(HttpContext.Session["InstID"])), "Integration_SASAMS_LearnerClasses_Info_Sync", resultFilePath);
                    //logger.Info("Import Classes .. Done.");

                    //logger.Info("Import Parent Child.");
                    //BulkCopyAccessToSQLServer(AccessCommands.SASAMS_Parent_Child(Convert.ToInt32(HttpContext.Session["InstID"])), "Integration_SASAMS_Parent_Child", resultFilePath);
                    //logger.Info("Import Parent Child .. Done.");

                    //logger.Info("Import Learners.");
                    //BulkCopyAccessToSQLServer(AccessCommands.SASAMS_Learner(Convert.ToInt32(HttpContext.Session["InstID"])), "Integration_SASAMS_Learner_Info_Sync", resultFilePath);
                    //logger.Info("Import Learners .. Done.");

                    //logger.Info("Import Pearents.");
                    //BulkCopyAccessToSQLServer(AccessCommands.SASAMS_Parents(Convert.ToInt32(HttpContext.Session["InstID"])), "Integration_SASAMS_Parent_Info_Sync", resultFilePath);
                    //logger.Info("Import Pearents .. Done.");


                    //logger.Info("Exec Integration_SASAMS_LearnerClasses_Sync " + HttpContext.Session["InstID"]);
                    //Context.Integration_SASAMS_LearnerClasses_Sync(Convert.ToInt32(HttpContext.Session["InstID"]));
                    //logger.Info("Exec Integration_SASAMS_LearnerClasses_Update " + HttpContext.Session["InstID"]);
                    //Context.Integration_SASAMS_LearnerClasses_Update(Convert.ToInt32(HttpContext.Session["InstID"]));
                    logger.Info("Exec Integration_SASAMS_AddMissingLists " + HttpContext.Session["InstID"]);
                    Context.Integration_SASAMS_AddMissingLists(Convert.ToInt32(HttpContext.Session["InstID"]));
                    logger.Info("Exec Integration_SASAMS_Parents_Sync " + HttpContext.Session["InstID"]);
                    //Context.Integration_SASAMS_Parents_Sync(Convert.ToInt32(HttpContext.Session["InstID"]));
                    //logger.Info("Exec Integration_SASAMS_Learners_Sync " + HttpContext.Session["InstID"]);
                    //Context.Integration_SASAMS_Learners_Sync(Convert.ToInt32(HttpContext.Session["InstID"]));

                }

                return Json(new { Success = true });
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return Json(new { Success = false });
            }
            
        }

        private static void BulkCopyAccessToSQLServer(string sql, string destinationTable, string fileName)
        {
            using (DataTable dt = new DataTable())
            {
                using (OleDbConnection conn = new OleDbConnection(string.Format(@"Provider={0};Data Source={1};Jet OLEDB:Database Password={2}", SystemParameters.SystemParams().AccessDatabaseProvider, fileName, SystemParameters.SystemParams().SASAMS_Password)))
                using (OleDbCommand cmd = new OleDbCommand(sql, conn))
                using (OleDbDataAdapter adapter = new OleDbDataAdapter(cmd))
                {
                    cmd.Connection.Open();
                    adapter.SelectCommand.CommandTimeout = 240;
                    adapter.Fill(dt);
                    adapter.Dispose();
                }
                using (SqlConnection conn2 = new SqlConnection(string.Format(@"Server = {0}; Database = {1}; User Id = {2};Password = {3};",SystemParameters.SystemParams().SQL_Server,SystemParameters.SystemParams().SQL_Database, SystemParameters.SystemParams().SQL_User, SystemParameters.SystemParams().SQL_Password)))
                {
                    conn2.Open();
                    using (SqlBulkCopy copy = new SqlBulkCopy(conn2))
                    {
                        copy.DestinationTableName = destinationTable;
                        copy.BatchSize = 1000;
                        copy.BulkCopyTimeout = 240;
                        copy.WriteToServer(dt);
                        copy.NotifyAfter = 1000;
                    }
                }
            }
        }

        #endregion

    }
}