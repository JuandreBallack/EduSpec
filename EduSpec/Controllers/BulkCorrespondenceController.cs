using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using WebMatrix.WebData;
using EduSpec.Models;
using System.Collections.Generic;
using DevExpress.Web.Mvc;
using DevExpress.XtraReports.UI;
using EduSpec.Reports;
using EduSpec.Code;
using System.Data;

namespace EduSpec.Controllers
{
    [SessionExpire]
    public class BulkCorrespondenceController : Controller
    {
        public static class RemoveHtml
        {
            private static Regex oClearHtmlScript = new Regex(@"<(.|\n)*?>", RegexOptions.Compiled);

            public static string RemoveAllHTMLTags(string sHtml)
            {
                if (string.IsNullOrEmpty(sHtml))
                    return string.Empty;

                return oClearHtmlScript.Replace(sHtml, string.Empty);
            }
        }

        #region Bulk Letters

        public ActionResult BulkLetterCorrespondence()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("BulkCorrespondences - Bulk Letter Correspondence", WebSecurity.CurrentUserId);
                return View("BulkLetterCorrespondence");
            }
        }

        public PartialViewResult BulkLetterCorrespondencePartial(int? YearID)
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("BulkCorrespondences - Bulk Letter Correspondence", WebSecurity.CurrentUserId);
                return PartialView("BulkLetterCorrespondencePartial", Context.Get_BulkCorrespondences_BulkLetterTracking_View(YearID ?? Context.fn_Get_YearID(DateTime.Now), Convert.ToInt32(HttpContext.Session["InstID"])).ToList());
            }
        }

        public PartialViewResult BulkLetterTrackingHistoryPartial(int? LetterTrackingKey)
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["BulkLetterHistoryViewProperties"] = ViewProperties.viewProperties("BulkCorrespondences - Bulk Letter Tracking History", WebSecurity.CurrentUserId);
                return PartialView("BulkLetterTrackingHistoryPartial", Context.Get_BulkCorrespondences_BulkLetterTrackingHistory_View(Convert.ToInt32(HttpContext.Session["InstID"]), LetterTrackingKey).ToList());
            }
        }

        [HttpPost]
        public JsonResult DeleteBulkLetter(int LetterTrackingKey)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_BulkCorrespondences_DeleteLetters(
                    LetterTrackingKey
                    );
            }
            return Json(new { Success = true });
        }

        public ActionResult getInstitutionBulkLetters(string returnUrl)
        {
            return Redirect(returnUrl);
        }

        public ActionResult getInstitutionBulkLettersHistory(string returnUrl)
        {
            return Redirect(returnUrl);
        }

        public ActionResult getBulkEmailLetters(string returnUrl)
        {
            return Redirect(returnUrl);
        }

        #endregion

        #region Bulk SMS

        public ActionResult BulkSMSTracking()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("BulkCorrespondences - Bulk SMS Tracking", WebSecurity.CurrentUserId);
                HttpContext.Session.Add("SMSBundleBalanceMessage", "");
                return View("BulkSMSTracking");
            }
        }

        public PartialViewResult BulkSMSTrackingPartial(int? YearID)
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("BulkCorrespondences - Bulk SMS Tracking", WebSecurity.CurrentUserId, string.Format("{0}", Context.fn_Get_YearDescription(YearID ?? Context.fn_Get_YearID(DateTime.Now))));
                return PartialView("BulkSMSTrackingPartial", Context.Get_BulkCorrespondences_BulkSMSTracking_View(YearID ?? Context.fn_Get_YearID(DateTime.Now), Convert.ToInt32(HttpContext.Session["InstID"])).ToList());
            }
        }

        public PartialViewResult BulkSMSTrackingHistoryPartial(int? SMSTrackingKey)
        {
            using (var Context = new EduSpecDataContext())
            {
                HttpContext.Session.Add("SMSTrackingKey", SMSTrackingKey);
                string _selectedIDs = Request.Params["selectedIDs"];
                ViewData["BulkSMSHistoryViewProperties"] = ViewProperties.viewProperties("BulkCorrespondences - Bulk SMS Tracking History", WebSecurity.CurrentUserId);
                return PartialView("BulkSMSTrackingHistoryPartial", Context.Get_BulkCorrespondences_BulkSMSTrackingHistory_View(Convert.ToInt32(HttpContext.Session["InstID"]), SMSTrackingKey).ToList());
            }
        }

        [HttpPost]
        public ActionResult BulkSMSCellnumberUpdate(Get_BulkCorrespondences_BulkSMSTrackingHistory_ViewResult SMSHistory)
        {
            using (var Context = new EduSpecDataContext())
            {
                if (SMSHistory.PrimaryCellNumber == null)
                {
                    SMSHistory.PrimaryCellNumber = "";
                }
                else if (SMSHistory.SecondaryCellNumber == null)
                {
                    SMSHistory.SecondaryCellNumber = "";
                }

                Context.Set_BulkCorrespondences_BulkSMSParentsCellNumbers_Update(
                        SMSHistory.SMSCorrespondenceID,
                        SMSHistory.PrimaryCellNumber,
                        SMSHistory.SecondaryCellNumber
                    );

                return BulkSMSTrackingHistoryPartial(Convert.ToInt32(HttpContext.Session["SMSTrackingKey"]));
            }
        }

        [HttpPost]
        public ActionResult DeleteSMSQueueRow(Get_BulkCorrespondences_BulkSMSTrackingHistory_ViewResult SMSHistory)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_BulkCorrespondences_DeleteSMSQueueRow(
                    SMSHistory.SMSCorrespondenceID
                    );
            }
            return BulkSMSTrackingHistoryPartial(Convert.ToInt32(HttpContext.Session["SMSTrackingKey"]));
        }

        [HttpPost]
        public JsonResult SendBulkSMS(int TrackingKey)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_Enquiries_SendBulkSMS(
                    TrackingKey,
                    true
                    );
            }
            return Json(new { Success = true });
        }

        [HttpPost]
        public JsonResult DeleteBulkSMS(int SMSTrackingKey)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_BulkCorrespondences_DeleteSMS(
                    SMSTrackingKey
                    );
            }
            return Json(new { Success = true });
        }

        #endregion

        #region Bulk Emails

        public ActionResult BulkEmailCorrespondence()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("BulkCorrespondences - Bulk Email Correspondence", WebSecurity.CurrentUserId);
                return View("BulkEmailCorrespondence");
            }
        }

        public PartialViewResult BulkEmailCorrespondencePartial(int? YearID)
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("BulkCorrespondences - Bulk Email Correspondence", WebSecurity.CurrentUserId);
                return PartialView("BulkEmailCorrespondencePartial", Context.Get_BulkCorrespondences_BulkEmailTracking_View(YearID ?? Context.fn_Get_YearID(DateTime.Now), Convert.ToInt32(HttpContext.Session["InstID"])).ToList());
            }
        }

        public PartialViewResult BulkEmailTrackingHistoryPartial(int? EmailTrackingKey)
        {
            using (var Context = new EduSpecDataContext())
            {
                HttpContext.Session.Add("EmailTrackingKey", EmailTrackingKey);
                ViewData["BulkEmailHistoryViewProperties"] = ViewProperties.viewProperties("BulkCorrespondences - Bulk Email Tracking History", WebSecurity.CurrentUserId);
                return PartialView("BulkEmailTrackingHistoryPartial", Context.Get_BulkCorrespondences_BulkEmailTrackingHistory_View(Convert.ToInt32(HttpContext.Session["InstID"]), EmailTrackingKey).ToList());
            }
        }

        [HttpPost]
        public ActionResult BulkEmailUpdate(Get_BulkCorrespondences_BulkEmailTrackingHistory_ViewResult EmailHistory)
        {
            using (var Context = new EduSpecDataContext())
            {
                if (EmailHistory.Recipient == null)
                {
                    EmailHistory.Recipient = "";
                }
                else if (EmailHistory.Cc == null)
                {
                    EmailHistory.Cc = "";
                }

                Context.Set_BulkCorrespondences_BulkEmailAddresses_Update(
                        EmailHistory.EmailCorrespondenceID,
                        EmailHistory.Recipient,
                        EmailHistory.Cc
                    );

                return BulkEmailTrackingHistoryPartial(Convert.ToInt32(HttpContext.Session["EmailTrackingKey"]));
            }
        }

        [HttpPost]
        public JsonResult SendBulkEmails(int TrackingKey)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_BulkCorrespondences_SendBulkEmails(
                    TrackingKey,
                    true
                    );

                var BulkEmailTrackingInfo = Context.Get_BulkCorrespondences_BulkEmailTrackingInfo(TrackingKey).FirstOrDefault();

                if (BulkEmailTrackingInfo.IsLegalProcess == true)
                {
                    switch(BulkEmailTrackingInfo.ExcelExportTypeID)
                    {
                        case 2:
                            CreateProsperitasExelFile(BulkEmailTrackingInfo);
                            break;                        
                    }
                }
            }   
            return Json(new { Success = true });
        }
        public void CreateProsperitasExelFile(Get_BulkCorrespondences_BulkEmailTrackingInfoResult BulkEmailTrackingInfo)
        {
            using (var Context = new EduSpecDataContext())
            {                
                List<Get_BulkCorrespondences_ProsperitasList_ViewResult> d = Context.Get_BulkCorrespondences_ProsperitasList_View(BulkEmailTrackingInfo.EmailTrackingKey).ToList();
                ExcelFile file = new ExcelFile();
                file.NewFile();
                file.AddHeader(ExcelHeaders.ProsperitasHeader);
                using (DataTable dt = new DataTable())
                {
                    foreach (var Header in ExcelHeaders.ProsperitasHeader)
                    {
                        dt.Columns.Add(Header);
                    }

                    for (int i = 0; i < d.Count; i++)
                    {
                        dt.Rows.Add(
                            d[i].Account_Number,
                            d[i].Debtor_1_Title,
                            d[i].Debtor_1_Initials,
                            d[i].Debtor_1_FullName,
                            d[i].Debtor_1_Surname,
                            d[i].Debtor_1_IDNumber,
                            d[i].Debtor_1_Passport_Number,
                            d[i].Debtor_1_TelHome,
                            d[i].Debtor_1_TelWork,
                            d[i].Debtor_1_TelCell,
                            d[i].Debtor_1_PostalAddress1,
                            d[i].Debtor_1_PostalAddress2,
                            d[i].Debtor_1_PostalAddress3,
                            d[i].Debtor_1_PostalCode,
                            d[i].Debtor_1_Email,
                            d[i].Debtor_2_Title,
                            d[i].Debtor_2_Initials,
                            d[i].Debtor_2_FullName,
                            d[i].Debtor_2_Surname,
                            d[i].Debtor_2_IDNumber,
                            d[i].Debtor_2_Passport_Number,
                            d[i].Debtor_2_TelHome,
                            d[i].Debtor_2_TelWork,
                            d[i].Debtor_2_TelCell,
                            d[i].Debtor_2_PostalAddress1,
                            d[i].Debtor_2_PostalAddress2,
                            d[i].Debtor_2_PostalAddress3,
                            d[i].Debtor_2_PostalCode,
                            d[i].Debtor_2_Email,
                            d[i].Learner,
                            d[i].Capital_Amount,
                            d[i].Date);
                    }
                    file.AddRows(dt);

                }
                file.SaveAs(String.Format("{0}{1}", SystemParameters.SystemParams().EmailAttachementsFilePath, BulkEmailTrackingInfo.ExcelFileName + ".xlsx"));
                file.Close();

                Context.Set_BulkCorrespondences_SendDebtCollectorExcelFile(
                    BulkEmailTrackingInfo.InstID,
                    BulkEmailTrackingInfo.EmailTrackingKey,
                    BulkEmailTrackingInfo.ContactEmail,
                    BulkEmailTrackingInfo.ContactName,
                    BulkEmailTrackingInfo.InstitutionName,
                    WebSecurity.CurrentUserId,
                    BulkEmailTrackingInfo.ExcelFileName
                    );
            }
        }

        [HttpPost]
        public JsonResult DeleteBulkEmail(int EmailTrackingKey)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_BulkCorrespondences_DeleteEmail(
                    EmailTrackingKey
                    );
            }
            return Json(new { Success = true });
        }

        [HttpPost]
        public ActionResult DeleteEmailQueueRow(Get_BulkCorrespondences_BulkEmailTrackingHistory_ViewResult EmailHistory)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_BulkCorrespondences_DeleteEmailQueueRow(
                    EmailHistory.EmailCorrespondenceID
                    );
            }
            return BulkEmailTrackingHistoryPartial(Convert.ToInt32(HttpContext.Session["EmailTrackingKey"]));
        }

        #endregion

        #region SMS Replies

        public ActionResult SMSReplies()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("Enquiries - SMS Replies", WebSecurity.CurrentUserId);
                return View("SMSReplies");
            }
        }

        public PartialViewResult SMSRepliesPartial()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("Enquiries - SMS Replies", WebSecurity.CurrentUserId);
                return PartialView("SMSRepliesPartial", Context.Get_Enquiries_SMsReplies_View(Convert.ToInt32(HttpContext.Session["InstID"])).ToList());
            }
        }

        [HttpPost]
        public JsonResult SMSRepliesRead(int ReplyID)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_Correspondences_SMSReplies_Update(
                    ReplyID,
                    true
                    );
            }
            return Json(new { Success = true });
        }

        #endregion

        #region Communication

        public ActionResult CorrespondenceList()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("BulkCorrespondences - Correspondence List", WebSecurity.CurrentUserId);
                return View("CorrespondenceList");
            }
        }

        public PartialViewResult CorrespondenceListPartial(int? ListID)
        {
            using (var Context = new EduSpecDataContext())
            {
                if (ListID != null)
                {
                    HttpContext.Session["ListID"] = ListID;
                }
                else
                {
                    HttpContext.Session["ListID"] = -1;
                }
                HttpContext.Session["IsVisible"] = false;
                ViewData["ListViewProperties"] = ViewProperties.viewProperties("BulkCorrespondences - Correspondence List", WebSecurity.CurrentUserId);
                return PartialView("CorrespondenceListPartial", Context.Get_BulkCorrespondences_CorrespondenceList_View(Convert.ToInt32(HttpContext.Session["InstID"]), Convert.ToInt32(HttpContext.Session["ListID"])).ToList());
            }
        }


        public PartialViewResult CommunicationSendSMSPartial()
        {
            using (var Context = new EduSpecDataContext())
            {
                return PartialView("CommunicationSendSMSPartial");
            }
        }

        public PartialViewResult CommunicationSendEmailPartial()
        {
            using (var Context = new EduSpecDataContext())
            {
                return PartialView("CommunicationSendEmailPartial");
            }
        }

        public PartialViewResult CorrespondenceListItems(int? ItemID)
        {
            using (var Context = new EduSpecDataContext())
            {
                return PartialView("CorrespondenceListItems", Context.Get_BulkCorrespondence_CorrespondenceListValues(ItemID).FirstOrDefault());
            }
        }

        public PartialViewResult CorrespondenceListComboBoxItems()
        {
            using (var Context = new EduSpecDataContext())
            {
                return PartialView("CorrespondenceListComboBoxItems");
            }
        }

        public PartialViewResult LearnerClassPickList(int? GradeID, int? ClassID)
        {
            ViewData["GradeID"] = GradeID ?? -1;
            ViewData["ClassID"] = ClassID ?? -1;
            return PartialView("LearnerClassPickList", ClassID);
        }

        public static List<GridUtils.DropdownList> GetInstitutionLearnerClass(int GradeID, int InstID)
        {
            using (var Context = new EduSpecDataContext())
            {
                return Context.ExecuteQuery<GridUtils.DropdownList>(String.Format("EXEC Get_BulkCorrespondenceLearnersClass_PickList {0},{1}", GradeID.ToString(), InstID.ToString())).ToList();
            }

        }

        public PartialViewResult CorrespondencePicklist(int? ListID, int? GradeID)
        {
            ViewData["ListID"] = ListID ?? -1;
            ViewData["GradeID"] = GradeID ?? -1;
            return PartialView("CorrespondencePicklist", GradeID);
        }

        public static List<GridUtils.DropdownList> GetListParameters(int ListID)
        {
            using (var Context = new EduSpecDataContext())
            {
                return Context.ExecuteQuery<GridUtils.DropdownList>(String.Format("EXEC Get_CorrespondenceListGrade_Picklist {0}", ListID.ToString())).ToList();
            }

        }

        public PartialViewResult CorrespondenceSMsSetupPartial()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["SMSViewProperties"] = ViewProperties.viewProperties("BulkCorrespondence - Family correspondence - SMS setup", WebSecurity.CurrentUserId);
                return PartialView("CorrespondenceSMsSetupPartial", Context.Get_BulkCorrespondence_FamilyCorrespondence_SMSMessages_View(Convert.ToInt32(HttpContext.Session["InstID"])).ToList());
            }
        }

        public PartialViewResult CorrespondenceEmailSetupPartial()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["EmailsViewProperties"] = ViewProperties.viewProperties("BulkCorrespondence - Family correspondence - Email setup", WebSecurity.CurrentUserId);
                return PartialView("CorrespondenceEmailSetupPartial", Context.Get_BulkCorrespondence_FamilyCorrespondence_EmailMessages_View(Convert.ToInt32(HttpContext.Session["InstID"])).ToList());
            }
        }

        public PartialViewResult CorrespondenceLetterSetupPartial()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["LettersViewProperties"] = ViewProperties.viewProperties("BulkCorrespondence - General correspondence - Letter setup", WebSecurity.CurrentUserId);
                return PartialView("CorrespondenceLetterSetupPartial", Context.Get_BulkCorrespondence_GeneralCorrespondence_Letters_View(Convert.ToInt32(HttpContext.Session["InstID"])).ToList());
            }
        }

        [HttpPost]
        public ActionResult LettersUpdate(Get_BulkCorrespondence_GeneralCorrespondence_Letters_ViewResult Letters)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_BulkCorrespondence_GeneralCorrespondence_Letters_Update(
                    Letters.InstLetterID,
                    Letters.Description,
                    Letters.Body_ENG,
                    Letters.Body_AFR,
                    Letters.IsPrintCoverPage,
                    Letters.IsPrintLetterHead
                    );
                return CorrespondenceLetterSetupPartial();
            }
        }

        [HttpPost]
        public ActionResult LettersAdd(Get_BulkCorrespondence_GeneralCorrespondence_Letters_ViewResult Letters)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_BulkCorrespondence_GeneralCorrespondence_Letters_Add(
                    Convert.ToInt32(HttpContext.Session["InstID"]),
                    Letters.Description,
                    Letters.Body_ENG,
                    Letters.Body_AFR,
                    Letters.IsPrintCoverPage,
                    Letters.IsPrintLetterHead
                    );
                return CorrespondenceLetterSetupPartial();
            }
        }

        public PartialViewResult LetterEditor_ENG(int? LetterID)
        {
            using (var Context = new EduSpecDataContext())
            {
                return PartialView("LetterEditor_ENG", Context.Get_BulkCorrespondence_GeneralCorrespondence_LetterToEdit_View(LetterID).FirstOrDefault());
            }
        }

        public PartialViewResult LetterEditor_AFR(int? LetterID)
        {
            using (var Context = new EduSpecDataContext())
            {
                return PartialView("LetterEditor_AFR", Context.Get_BulkCorrespondence_GeneralCorrespondence_LetterToEdit_View(LetterID).FirstOrDefault());
            }
        }

        public PartialViewResult MessageEditorFamilyCorrespondence_ENG(int? InstSMSID)
        {
            using (var Context = new EduSpecDataContext())
            {
                return PartialView("MessageEditorExemptions_ENG", Context.Get_FamilyCorrespondence_SMSMessageToEdit(InstSMSID).FirstOrDefault());
            }
        }

        public PartialViewResult MessageEditorFamilyCorrespondence_AFR(int? InstSMSID)
        {
            using (var Context = new EduSpecDataContext())
            {
                return PartialView("MessageEditorExemptions_AFR", Context.Get_FamilyCorrespondence_SMSMessageToEdit(InstSMSID).FirstOrDefault());
            }
        }

        public PartialViewResult EmailEditorFamilyCorrespondence_ENG(int? InstEmailID)
        {
            using (var Context = new EduSpecDataContext())
            {
                return PartialView("EmailEditorExemptions_ENG", Context.Get_FamilyCorrespondence_EmailMessageToEdit_View(InstEmailID).FirstOrDefault());
            }
        }

        public PartialViewResult EmailEditorFamilyCorrespondence_AFR(int? InstEmailID)
        {
            using (var Context = new EduSpecDataContext())
            {
                return PartialView("EmailEditorExemptions_AFR", Context.Get_FamilyCorrespondence_EmailMessageToEdit_View(InstEmailID).FirstOrDefault());
            }
        }

        [HttpPost]
        public ActionResult SMSMessageUpdate(Get_BulkCorrespondence_FamilyCorrespondence_SMSMessages_ViewResult SMS)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_FamilyCorrespondence_SMSMessage_Update(
                    SMS.InstSMSID,
                    SMS.Description,
                    SMS.Message_ENG,
                    SMS.Message_AFR,
                    SMS.IsActive,
                    WebSecurity.CurrentUserId
                    );
                return CorrespondenceSMsSetupPartial();
            }
        }

        [HttpPost]
        public ActionResult SMSMessageAdd(Get_BulkCorrespondence_FamilyCorrespondence_SMSMessages_ViewResult SMS)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_FamilyCorrespondence_SMSMessage_Add(
                    Convert.ToInt32(HttpContext.Session["InstID"]),
                    SMS.Description,
                    SMS.Message_ENG,
                    SMS.Message_AFR,
                    WebSecurity.CurrentUserId,
                    true
                    );
                return CorrespondenceSMsSetupPartial();
            }
        }

        [HttpPost]
        public ActionResult EmailMessageUpdate(Get_BulkCorrespondence_FamilyCorrespondence_EmailMessages_ViewResult Email)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_FamilyCorrespondence_EmailMessage_Update(
                    Email.InstEmailID,
                    Email.Description,
                    Email.EmailBody_ENG,
                    Email.EmailBody_AFR,
                    WebSecurity.CurrentUserId
                    );
                return CorrespondenceEmailSetupPartial();
            }
        }

        [HttpPost]
        public ActionResult EmailMessageAdd(Get_BulkCorrespondence_FamilyCorrespondence_EmailMessages_ViewResult Email)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_FamilyCorrespondence_EmailMessage_Add(
                    Convert.ToInt32(HttpContext.Session["InstID"]),
                    Email.Description,
                    Email.EmailBody_ENG,
                    Email.EmailBody_AFR,
                    WebSecurity.CurrentUserId,
                    true
                    );
                return CorrespondenceEmailSetupPartial();
            }
        }

        [HttpPost]
        public JsonResult AddCustomList(string CustomListDescription, string CorrespondenceListCustom)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_BulkCorrespondence_CustomCorrespondenceList_Add(
                    Convert.ToInt32(HttpContext.Session["InstID"]),
                    CustomListDescription,
                    CorrespondenceListCustom
                    );
            }
            return Json(new { Success = true });
        }

        [HttpPost]
        public JsonResult LinkToCustomList(int CorrespondenceListID, string AddToContactList)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_BulkCorrespondence_CustomCorrespondenceList_Update(
                    CorrespondenceListID,
                    Convert.ToInt32(HttpContext.Session["InstID"]),
                    AddToContactList
                    );
            }
            return Json(new { Success = true });
        }

        [HttpPost]
        public JsonResult RemoveLearnersFromList(string RemoveLearnersList)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_BulkCorrespondence_CorrespondenceListRemoveLearners(
                    Convert.ToInt32(HttpContext.Session["ListID"]),
                    Convert.ToInt32(HttpContext.Session["InstID"]),
                    RemoveLearnersList
                    );
            }
            return Json(new { Success = true });
        }

        [HttpPost]
        public JsonResult AddNewCorrespondenceList(string ListDescription)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_BulkCorrespondence_NewCorrespondenceList(
                    Convert.ToInt32(HttpContext.Session["InstID"]),
                    ListDescription
                    );
            }
            return Json(new { Success = true });
        }

        [HttpPost]
        public JsonResult UpdateCorrespondenceItem(int ListID, int GradeID, int? ClassID, int? GenderID)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_BulkCorrespondence_CorrespondenceItem_Update(
                   ListID,
                   GradeID,
                   ClassID,
                   GenderID
                    );
            }
            return Json(new { Success = true });
        }

        [HttpPost]
        public JsonResult CorrespondenceListIDChange(int? ListID)
        {
            using (var Context = new EduSpecDataContext())
            {
                HttpContext.Session["ListID"] = ListID;
            }
            return Json(new { Success = true });
        }

        [HttpPost]
        public JsonResult CorrespondenceListIDInit(int? ListID)
        {
            using (var Context = new EduSpecDataContext())
            {
                HttpContext.Session["ListID"] = ListID;
            }
            return Json(new { Success = true });
        }

        [HttpPost]
        public JsonResult QueueSMSList(string CorrespondenceList, int? MessageID, string ManualMessage, int FamilyContactTypeID, bool SendSingleParent)
        {
            using (var Context = new EduSpecDataContext() { CommandTimeout = 5 * 60 })
            {

                Context.Set_BulkCorrespondence_QueueListMessages(
                    Convert.ToInt32(HttpContext.Session["InstID"]),
                    WebSecurity.CurrentUserId, 
                    CorrespondenceList,
                    Convert.ToInt32(HttpContext.Session["ListID"]),
                    1,//SMS
                    MessageID,
                    ManualMessage,
                    null,
                    null,
                    null,
                    FamilyContactTypeID,
                    SendSingleParent,
                    null,
                    null
                    );
            }
            return Json(new { result = "Success" });
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult QueueEmailList(string CorrespondenceList, int? MessageID, string ManualMessage, string EmailSubject, int EmailTypeID, int FamilyContactTypeID, bool SendSingleParent, int? AttachEmailFileType, int? InstLetter)
        {
            using (var Context = new EduSpecDataContext() { CommandTimeout = 5 * 60 })
            {
                string FileName;
                if(EmailTypeID == 1)
                {
                    FileName = Convert.ToString(HttpContext.Session["CustomEmailAttachmentFileName"]);
                }
                else
                    FileName = Convert.ToString(HttpContext.Session["ManualEmailAttachmentFileName"]);

                Context.Set_BulkCorrespondence_QueueListMessages(
                    Convert.ToInt32(HttpContext.Session["InstID"]),
                    WebSecurity.CurrentUserId,
                    CorrespondenceList,
                    Convert.ToInt32(HttpContext.Session["ListID"]),
                    2,//Email
                    MessageID,
                    ManualMessage,
                    EmailSubject,
                    EmailTypeID,
                    FileName,
                    FamilyContactTypeID,
                    SendSingleParent,
                    AttachEmailFileType,
                    InstLetter
                    );
            }
            return Json(new { result = "Success" });
        }

        public ActionResult CallbacksCorrespondenceEmailCustomFileUpload()
        {
            try
            {
                HttpContext.Session.Add("CustomEmailAttachmentFileName", null);
                UploadControlExtension.GetUploadedFiles("ucCorrespondenceEmailCustomUploadData", UploadControlHelper.EmailFileUploadValidationSettings, UploadControlHelper.ucCustomEmailFileCallbacks_FileUploadComplete);
            }
            catch (Exception e)
            {
                string x = e.Message;
            }

            return null;
        }

        public ActionResult CallbacksCorrespondenceEmailManualFileUpload()
        {
            try
            {
                HttpContext.Session.Add("ManualEmailAttachmentFileName", null);
                UploadControlExtension.GetUploadedFiles("ucCorrespondenceEmailManualUploadData", UploadControlHelper.EmailFileUploadValidationSettings, UploadControlHelper.ucManualEmailFileCallbacks_FileUploadComplete);
            }
            catch (Exception e)
            {
                string x = e.Message;
            }

            return null;
        }

        [HttpPost]
        public ActionResult UpdateLearnerCellNumberAndEmail(MVCxGridViewBatchUpdateValues<Get_BulkCorrespondences_CorrespondenceList_ViewResult, int> updateValues)
        {
            using (var Context = new EduSpecDataContext())
            {
                foreach (var Item in updateValues.Update)
                {
                    if (updateValues.IsValid(Item))
                    {

                        Context.Set_BulkCorrespondence_LearnerCellAndEmail_Update(
                        Item.LearnerID,
                        Item.CellNo,
                        Item.Email
                        );
                    }
                }
            }

            return CorrespondenceListPartial(Convert.ToInt32(HttpContext.Session["ListID"]));

        }

        public PartialViewResult CommunicationFamiliesPartial()
        {
            using (var Context = new EduSpecDataContext())
            {

                ViewData["FamiliesPartialViewProperties"] = ViewProperties.viewProperties("BulkCorrespondence - Communication Families", WebSecurity.CurrentUserId);
                return PartialView("CommunicationFamiliesPartial", Context.Get_BulkCorrespondence_CommunicationFamilies_View(Convert.ToInt32(HttpContext.Session["InstID"]), Context.fn_Get_YearID(DateTime.Now)).ToList());
            }
        }

        public PartialViewResult CommunicationFamilyActionHistoryPartial(int? FamilyID)
        {
            using (var Context = new EduSpecDataContext())
            {

                ViewData["FamilyActionHistoryViewProperties"] = ViewProperties.viewProperties("BulkCorrespondence - Communication Family History", WebSecurity.CurrentUserId);
                return PartialView("CommunicationFamilyActionHistoryPartial", Context.Get_BulkCorrespondence_CommunicationActionHistory_View(FamilyID).ToList());
            }
        }

        public PartialViewResult LinkNewCustomListPartial()
        {
            using (var Context = new EduSpecDataContext())
            {
                return PartialView("LinkNewCustomListPartial");
            }
        }

        public ActionResult getDocument(string returnUrl)
        {
            return Redirect(returnUrl);
        }

        #endregion

        #region Communication History

        public ActionResult CommunicationHistory()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ViewProperties"] = ViewProperties.viewProperties("BulkCorrespondence - CommunicationHistory", WebSecurity.CurrentUserId);
                return View("CommunicationHistory");
            }
        }

        public PartialViewResult CommunicationHistoryPartial()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["HistoryViewProperties"] = ViewProperties.viewProperties("BulkCorrespondence - CommunicationHistory", WebSecurity.CurrentUserId);
                return PartialView("CommunicationHistoryPartial", Context.Get_BulkCorrespondence_CommunicationHistory_View(Convert.ToInt32(HttpContext.Session["InstID"]), Context.fn_Get_YearID(DateTime.Now)).ToList());
            }
        }

        public PartialViewResult CommunicationHistoryDetailPartial(int? FamilyID)
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["HistoryDetailViewProperties"] = ViewProperties.viewProperties("BulkCorrespondence - CommunicationHistory Detail", WebSecurity.CurrentUserId);
                return PartialView("CommunicationHistoryDetailPartial", Context.Get_BulkCorrespondence_CommunicationHistoryDetail_View(FamilyID, Context.fn_Get_YearID(DateTime.Now)).ToList());
            }
        }

        public PartialViewResult reportCommunicationHistory()
        {
            DateTime DateFrom = DateTime.Now.AddDays(-20);
            DateTime DateTo = DateTime.Now;

            XtraReport report = new CorrespondenceHistory();
            report.Parameters["DateFrom"].Value = DateFrom;
            report.Parameters["DateTo"].Value = DateTo;
            report.Parameters["Form"].Value = -1;
            report.Parameters["FormType"].Value = -1;
            report.Parameters["InstID"].Value = HttpContext.Session["InstID"];
            report.CreateDocument();
            return PartialView("reportCommunicationHistory", report);
        }

        public PartialViewResult reportCommunicationHistoryFamily(int FamilyID)
        {
            DateTime DateFrom = DateTime.Now.AddDays(-20);
            DateTime DateTo = DateTime.Now;

            XtraReport report = new CorrespondenceHistoryFamily();
            report.Parameters["DateFrom"].Value = DateFrom;
            report.Parameters["DateTo"].Value = DateTo;
            report.Parameters["Form"].Value = -1;
            report.Parameters["FormType"].Value = -1;
            report.Parameters["InstID"].Value = HttpContext.Session["InstID"];
            report.Parameters["FamilyID"].Value = FamilyID;
            report.CreateDocument();
            return PartialView("reportCommunicationHistoryFamily", report);
        }
        #endregion
    }
}