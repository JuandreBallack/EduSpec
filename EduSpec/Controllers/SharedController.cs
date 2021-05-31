using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using EduSpec.Models;
using WebMatrix.WebData;
using EduSpec.Code;
using NLog;
using System.IO;
using DevExpress.XtraReports.UI;
using NLog.Mvc;
using DevExpress.XtraPrinting;

namespace EduSpec.Controllers
{
    [SessionExpire]

    public class SharedController : Controller
    {

        #region Family
        public PartialViewResult FamilyInfoPartial(int? FamilyInfoFamilyID, bool? IsUseExemptionID)
        {
            using (var Context = new EduSpecDataContext())
            {
                return PartialView("FamilyInfoPartial", Context.Get_Shared_AccountInfo(FamilyInfoFamilyID, IsUseExemptionID).FirstOrDefault());
            }
        }
        public PartialViewResult FamilyManagementPartial(int? EditFamilyID, bool? IsAddNewFamily, bool? IsUseExemptionID)
        {
            if (IsAddNewFamily == true && HttpContext.Session["FamilyGuid"].ToString() == "")
            {
                HttpContext.Session.Add("FamilyGuid", Guid.NewGuid());
            }

            using (var Context = new EduSpecDataContext())
            {
                return PartialView("FamilyManagementPartial", Context.Get_Shared_EditAccountInfo(EditFamilyID, IsUseExemptionID).FirstOrDefault());
            }
        }
        public PartialViewResult LearnerManagementPartial(int? EditFamilyID, bool? IsAddNewFamily, bool? IsUseExemptionID)
        {
            using (var Context = new EduSpecDataContext())
            {
                var x = Context.Get_Shared_EditAccountLearners_View(EditFamilyID, IsUseExemptionID, null).ToList();
                ViewData["LearnersMagagementViewProperties"] = ViewProperties.viewProperties("Shared - Manage Learners", WebSecurity.CurrentUserId);
                Guid TempGuid;
                if (HttpContext.Session["FamilyGuid"].ToString() != "")
                {
                    TempGuid = Guid.Parse(HttpContext.Session["FamilyGuid"].ToString());
                    return PartialView("LearnerManagementPartial", Context.Get_Shared_EditAccountLearners_View(EditFamilyID, IsUseExemptionID, TempGuid).ToList());
                }
                else
                    return PartialView("LearnerManagementPartial", Context.Get_Shared_EditAccountLearners_View(EditFamilyID, IsUseExemptionID, null).ToList());
            }
        }

        [HttpPost]
        public JsonResult UpdateFamily(Get_Shared_EditAccountInfoResult Detail)
        {
            using (var Context = new EduSpecDataContext())
            {
                try
                {
                    Context.Set_Shared_Family_Update(
                        Convert.ToInt32(HttpContext.Session["InstID"]),
                        Detail.Edit_FamilyID,
                        Detail.Edit_FamilyCode,
                        Detail.Edit_ExternalAccNo,
                        Detail.Edit_LanguageID,
                        Detail.Edit_BillingCycleTypeID,
                        Detail.Edit_MaritalStatusID,
                        Detail.Edit_PrimaryParentParentID,
                        Detail.Edit_PrimaryParentRelationID,
                        Detail.Edit_PrimaryParentTitleID,
                        Detail.Edit_PrimaryParentInitials,
                        Detail.Edit_PrimaryParentFullName,
                        Detail.Edit_PrimaryParentSurname,
                        Detail.Edit_PrimaryParentIDNumber,
                        Detail.Edit_PrimaryParentOccupation,
                        Detail.Edit_PrimaryParentEmployer,
                        Detail.Edit_PrimaryParentTelHome,
                        Detail.Edit_PrimaryParentTelWork,
                        Detail.Edit_PrimaryParentTelFax,
                        Detail.Edit_PrimaryParentTelCell,
                        Detail.Edit_PrimaryParentTelAlternative,
                        Detail.Edit_PrimaryParentEmail,
                        Detail.Edit_PrimaryParentEmailWork,
                        Detail.Edit_PrimaryParentStreetAddress1,
                        Detail.Edit_PrimaryParentStreetAddress2,
                        Detail.Edit_PrimaryParentStreetAddress3,
                        Detail.Edit_PrimaryParentStreetCode,
                        Detail.Edit_PrimaryParentPostalAddress1,
                        Detail.Edit_PrimaryParentPostalAddress2,
                        Detail.Edit_PrimaryParentPostalAddress3,
                        Detail.Edit_PrimaryParentPostalCode,
                        Detail.Edit_PrimaryParentGenderID,
                        Detail.Edit_SecondaryParentParentID,
                        Detail.Edit_SecondaryParentRelationID,
                        Detail.Edit_SecondaryParentTitleID,
                        Detail.Edit_SecondaryParentInitials,
                        Detail.Edit_SecondaryParentFullName,
                        Detail.Edit_SecondaryParentSurname,
                        Detail.Edit_SecondaryParentIDNumber,
                        Detail.Edit_SecondaryParentOccupation,
                        Detail.Edit_SecondaryParentEmployer,
                        Detail.Edit_SecondaryParentTelHome,
                        Detail.Edit_SecondaryParentTelWork,
                        Detail.Edit_SecondaryParentTelFax,
                        Detail.Edit_SecondaryParentTelCell,
                        Detail.Edit_SecondaryParentTelAlternative,
                        Detail.Edit_SecondaryParentEmail,
                        Detail.Edit_SecondaryParentEmailWork,
                        Detail.Edit_SecondaryParentStreetAddress1,
                        Detail.Edit_SecondaryParentStreetAddress2,
                        Detail.Edit_SecondaryParentStreetAddress3,
                        Detail.Edit_SecondaryParentStreetCode,
                        Detail.Edit_SecondaryParentPostalAddress1,
                        Detail.Edit_SecondaryParentPostalAddress2,
                        Detail.Edit_SecondaryParentPostalAddress3,
                        Detail.Edit_SecondaryParentPostalCode,
                        Detail.Edit_SecondaryParentGenderID,
                        Detail.Edit_AlternativeParentParentID,
                        Detail.Edit_AlternativeParentRelationID,
                        Detail.Edit_AlternativeParentTitleID,
                        Detail.Edit_AlternativeParentInitials,
                        Detail.Edit_AlternativeParentFullName,
                        Detail.Edit_AlternativeParentSurname,
                        Detail.Edit_AlternativeParentIDNumber,
                        Detail.Edit_AlternativeParentOccupation,
                        Detail.Edit_AlternativeParentEmployer,
                        Detail.Edit_AlternativeParentTelHome,
                        Detail.Edit_AlternativeParentTelWork,
                        Detail.Edit_AlternativeParentTelFax,
                        Detail.Edit_AlternativeParentTelCell,
                        Detail.Edit_AlternativeParentTelAlternative,
                        Detail.Edit_AlternativeParentEmail,
                        Detail.Edit_AlternativeParentEmailWork,
                        Detail.Edit_AlternativeParentStreetAddress1,
                        Detail.Edit_AlternativeParentStreetAddress2,
                        Detail.Edit_AlternativeParentStreetAddress3,
                        Detail.Edit_AlternativeParentStreetCode,
                        Detail.Edit_AlternativeParentPostalAddress1,
                        Detail.Edit_AlternativeParentPostalAddress2,
                        Detail.Edit_AlternativeParentPostalAddress3,
                        Detail.Edit_AlternativeParentPostalCode,
                        Detail.Edit_AlternativeParentGenderID,
                        UserUtils.CurrentUser().UserID
                        );
                    return Json(new { Success = true, error = "" });

                }
                catch (Exception e)
                {
                    return Json(new { Success = false, error = e.Message });
                }

            }

        }

        [HttpPost]
        public JsonResult AddNewFamily(Get_Shared_EditAccountInfoResult Family, bool? IsAddNewFamily)
        {
            using (var Context = new EduSpecDataContext())
            {
                try
                {
                    int? FamilyID;

                    FamilyID = Context.Set_Shared_NewFamily_Add(
                                Convert.ToInt32(HttpContext.Session["InstID"]),
                                Family.Edit_FamilyCode,
                                Family.Edit_ExternalAccNo,
                                Family.Edit_MaritalStatusID,
                                Family.Edit_LanguageID,
                                Family.Edit_BillingCycleTypeID,
                                Family.Edit_FamilySurname,
                                Family.Edit_PrimaryParentRelationID,
                                Family.Edit_PrimaryParentTitleID,
                                Family.Edit_PrimaryParentInitials,
                                Family.Edit_PrimaryParentFullName,
                                Family.Edit_PrimaryParentSurname,
                                Family.Edit_PrimaryParentIDNumber,
                                Family.Edit_PrimaryParentOccupation,
                                Family.Edit_PrimaryParentEmployer,
                                Family.Edit_PrimaryParentTelHome,
                                Family.Edit_PrimaryParentTelWork,
                                Family.Edit_PrimaryParentTelFax,
                                Family.Edit_PrimaryParentTelCell,
                                Family.Edit_PrimaryParentTelAlternative,
                                Family.Edit_PrimaryParentEmail,
                                Family.Edit_PrimaryParentEmailWork,
                                Family.Edit_PrimaryParentStreetAddress1,
                                Family.Edit_PrimaryParentStreetAddress2,
                                Family.Edit_PrimaryParentStreetAddress3,
                                Family.Edit_PrimaryParentStreetCode,
                                Family.Edit_PrimaryParentPostalAddress1,
                                Family.Edit_PrimaryParentPostalAddress2,
                                Family.Edit_PrimaryParentPostalAddress3,
                                Family.Edit_PrimaryParentPostalCode,
                                Family.Edit_PrimaryParentGenderID,
                                Family.Edit_SecondaryParentRelationID,
                                Family.Edit_SecondaryParentTitleID,
                                Family.Edit_SecondaryParentInitials,
                                Family.Edit_SecondaryParentFullName,
                                Family.Edit_SecondaryParentSurname,
                                Family.Edit_SecondaryParentIDNumber,
                                Family.Edit_SecondaryParentOccupation,
                                Family.Edit_SecondaryParentEmployer,
                                Family.Edit_SecondaryParentTelHome,
                                Family.Edit_SecondaryParentTelWork,
                                Family.Edit_SecondaryParentTelFax,
                                Family.Edit_SecondaryParentTelCell,
                                Family.Edit_SecondaryParentTelAlternative,
                                Family.Edit_SecondaryParentEmail,
                                Family.Edit_SecondaryParentEmailWork,
                                Family.Edit_SecondaryParentStreetAddress1,
                                Family.Edit_SecondaryParentStreetAddress2,
                                Family.Edit_SecondaryParentStreetAddress3,
                                Family.Edit_SecondaryParentStreetCode,
                                Family.Edit_SecondaryParentPostalAddress1,
                                Family.Edit_SecondaryParentPostalAddress2,
                                Family.Edit_SecondaryParentPostalAddress3,
                                Family.Edit_SecondaryParentPostalCode,
                                Family.Edit_SecondaryParentGenderID,
                                Family.Edit_AlternativeParentRelationID,
                                Family.Edit_AlternativeParentTitleID,
                                Family.Edit_AlternativeParentInitials,
                                Family.Edit_AlternativeParentFullName,
                                Family.Edit_AlternativeParentSurname,
                                Family.Edit_AlternativeParentIDNumber,
                                Family.Edit_AlternativeParentOccupation,
                                Family.Edit_AlternativeParentEmployer,
                                Family.Edit_AlternativeParentTelHome,
                                Family.Edit_AlternativeParentTelWork,
                                Family.Edit_AlternativeParentTelFax,
                                Family.Edit_AlternativeParentTelCell,
                                Family.Edit_AlternativeParentTelAlternative,
                                Family.Edit_AlternativeParentEmail,
                                Family.Edit_AlternativeParentEmailWork,
                                Family.Edit_AlternativeParentStreetAddress1,
                                Family.Edit_AlternativeParentStreetAddress2,
                                Family.Edit_AlternativeParentStreetAddress3,
                                Family.Edit_AlternativeParentStreetCode,
                                Family.Edit_AlternativeParentPostalAddress1,
                                Family.Edit_AlternativeParentPostalAddress2,
                                Family.Edit_AlternativeParentPostalAddress3,
                                Family.Edit_AlternativeParentPostalCode,
                                Family.Edit_AlternativeParentGenderID,
                                Guid.Parse(HttpContext.Session["FamilyGuid"].ToString()),
                                UserUtils.CurrentUser().UserID).FirstOrDefault().FamilyID;

                    HttpContext.Session["FamilyGuid"] = "";
                    HttpContext.Session.Add("NewFamilyID", FamilyID.ToString());
                    return Json(new { Success = true, error = "", FamilyID });
                }
                catch (Exception e)
                {
                    return Json(new { Success = false, error = e.Message, FamilyID = "" });
                }

            }

        }

        [HttpPost]
        public ActionResult InstitutionLearnerAdd(Get_Shared_EditAccountLearners_ViewResult Learner, int? EditFormFamilyID, bool? IsAddNewFamily, bool? IsUseExemptionID)
        {
            using (var Context = new EduSpecDataContext())
            {
                Guid FamilyGuid;
                string s = HttpContext.Session["FamilyGuid"].ToString();
                if (s != "")
                {
                    FamilyGuid = Guid.Parse(HttpContext.Session["FamilyGuid"].ToString());
                }
                else
                    FamilyGuid = Guid.Parse("00000000-0000-0000-0000-000000000000");

                Context.Set_Shared_NewLearner_Add(
                        EditFormFamilyID,
                        Convert.ToInt32(HttpContext.Session["InstID"]),
                        Learner.Edit_AdmissionNo,
                        Learner.Edit_EntryDate,
                        Learner.Edit_TerminationDate,
                        Learner.Edit_ClassID,
                        Learner.Edit_GradeID,
                        Learner.Edit_NationalityID,
                        Learner.Edit_GenderID,
                        Learner.Edit_TitleID,
                        Learner.Edit_Initials,
                        Learner.Edit_FirstName,
                        Learner.Edit_Surname,
                        Learner.Edit_IDNo,
                        Learner.Edit_DateOfBirth,
                        Learner.Edit_LanguageID,
                        Learner.Edit_TelNo,
                        Learner.Edit_CellNo,
                        Learner.Edit_Email,
                        IsUseExemptionID,
                        FamilyGuid,
                        WebSecurity.CurrentUserId
                    );

                return LearnerManagementPartial(EditFormFamilyID, IsAddNewFamily, IsUseExemptionID);
            }
        }

        [HttpPost]
        public ActionResult InstitutionLearnerUpdate(Get_Shared_EditAccountLearners_ViewResult Learner, bool? IsAddNewFamily, bool? IsUseExemptionID)
        {
            using (var Context = new EduSpecDataContext())
            {
                Guid FamilyGuid;
                string s = HttpContext.Session["FamilyGuid"].ToString();
                if (s != "")
                {
                    FamilyGuid = Guid.Parse(HttpContext.Session["FamilyGuid"].ToString());
                }
                else
                    FamilyGuid = Guid.Parse("00000000-0000-0000-0000-000000000000");

                Context.Set_Shared_Learner_Update(
                        Learner.Edit_LearnerFamilyID,
                        Convert.ToInt32(HttpContext.Session["InstID"]),
                        Learner.Edit_LearnerID,
                        Learner.Edit_AdmissionNo,
                        Learner.Edit_ClassID,
                        Learner.Edit_GradeID,
                        Learner.Edit_NationalityID,
                        Learner.Edit_GenderID,
                        Learner.Edit_TitleID,
                        Learner.Edit_Initials,
                        Learner.Edit_FirstName,
                        Learner.Edit_Surname,
                        Learner.Edit_EntryDate,
                        Learner.Edit_TerminationDate,
                        Learner.Edit_IDNo,
                        Learner.Edit_DateOfBirth,
                        Learner.Edit_LanguageID,
                        Learner.Edit_TelNo,
                        Learner.Edit_CellNo,
                        Learner.Edit_Email,
                        FamilyGuid,
                        WebSecurity.CurrentUserId
                    );

                return LearnerManagementPartial(Learner.Edit_LearnerFamilyID, IsAddNewFamily, IsUseExemptionID);
            }
        }

        [HttpPost]
        public JsonResult SwitchPrimaryAndSecondaryParent(int FamilyID)
        {
            using (var Context = new EduSpecDataContext())
            {
                try
                {
                    Context.Set_Shared_SwitchPrimarySecondaryPerents
                    (
                       FamilyID, 
                       Convert.ToInt32(HttpContext.Session["InstID"]),
                       WebSecurity.CurrentUserId
                     );
                    return Json(new { Success = true, error = "" });
                }
                catch (Exception e)
                {
                    return Json(new { Success = false, error = e.Message });
                }
            }
        }

        [HttpPost]
        public JsonResult SwithWithAlternativeParent(int FamilyID, int ActiveTabIndex)
        {
            using (var Context = new EduSpecDataContext())
            {
                try
                {
                    Context.Set_Shared_SwithWithAlternativeParent
                    (
                        FamilyID, 
                        Convert.ToInt32(HttpContext.Session["InstID"]), ActiveTabIndex,
                        WebSecurity.CurrentUserId
                    );
                    return Json(new { Success = true, error = "" });
                }
                catch (Exception e)
                {
                    return Json(new { Success = false, error = e.Message });
                }
            }
        }
        #endregion

        #region Validations

        [HttpPost]
        public JsonResult ValidateAdmissionNo(string AdmissionNo, int LearnerID)
        {
            using (var Context = new EduSpecDataContext())
            {
                bool result = Context.fn_ValidateAdmissionNo(Convert.ToInt32(HttpContext.Session["InstID"]), AdmissionNo, LearnerID).Value;
                return Json(new { result });
            }
        }

        [HttpPost]
        public JsonResult ValidateFamilyCode(string FamilyCode, int? FamilyID)
        {
            using (var Context = new EduSpecDataContext())
            {
                bool result = Context.fn_ValidateFamilyCode(Convert.ToInt32(HttpContext.Session["InstID"]), FamilyID ?? -1, FamilyCode).Value;
                return Json(new { result });
            }
        }

        [HttpPost]
        public JsonResult ValidateExternalAccNo(string ExternalAccNo, int? FamilyID)
        {
            using (var Context = new EduSpecDataContext())
            {
                bool result = Context.fn_ValidateAccountCode(Convert.ToInt32(HttpContext.Session["InstID"]), FamilyID ?? -1, ExternalAccNo).Value;
                return Json(new { result });
            }
        }

        public PartialViewResult LearnerClassPickList(int? GradeID, int? ClassID)
        {
            ViewData["GradeID"] = GradeID ?? -1;
            return PartialView("LearnerClassPickList",ClassID);
        }

        public static List<GridUtils.DropdownList> GetInstitutionLearnerClass(int GradeID, int InstID)
        {
            using (var Context = new EduSpecDataContext())
            {
                return Context.ExecuteQuery<GridUtils.DropdownList>(String.Format("EXEC Get_LearnersClass_PickList {0},{1}", GradeID.ToString(), InstID.ToString())).ToList();
            }

        }
        #endregion

        #region Info Panel

        public PartialViewResult InfoPanel()
        {
            using (var Context = new EduSpecDataContext())
            {
                return PartialView("InfoPanel", Context.Get_Shared_InfoPanelInfo(Convert.ToInt32(HttpContext.Session["InstID"])).FirstOrDefault());
            }
        }
        #endregion

        #region SMS's

        public PartialViewResult SendSMSSingle (int SMSKeyValue, int SMSKeyValueType)
        {
            using (var Context = new EduSpecDataContext())
            {
                return PartialView("SendSMSSingle", Context.Get_Shared_SingleSMSInfo(SMSKeyValue, SMSKeyValueType, Convert.ToInt32(HttpContext.Session["InstID"])).FirstOrDefault());
            }
        }

        public PartialViewResult SendSMSBulk(int BulkSMSTypeID)
        {
            using (var Context = new EduSpecDataContext())
            {
                return PartialView("SendSMSBulk", Context.Get_Shared_BulkSMSInfo(Convert.ToInt32(HttpContext.Session["InstID"]), BulkSMSTypeID).FirstOrDefault());
            }
        }

        [HttpPost]
        public JsonResult QueueBulkSMS(int BulkSMSTypeID, int StatusID, int SMSMessageID, int CorrespondenceMethodID, bool? IsExcludeZeroBalances, bool? IsExcludeNegativeBalances, int? FamilyStatusID)
        {
            using (var Context = new EduSpecDataContext())
            {
                if(IsExcludeZeroBalances == null)
                {
                    IsExcludeZeroBalances = false;
                }

                if (IsExcludeNegativeBalances == null)
                {
                    IsExcludeNegativeBalances = false;
                }

                if (FamilyStatusID == null)
                {
                    FamilyStatusID = -1;
                }

                Context.Set_BulkSMSQueue_Add(
                    Convert.ToInt32(HttpContext.Session["InstID"]),
                    StatusID,
                    SMSMessageID,
                    BulkSMSTypeID,
                    WebSecurity.CurrentUserId,
                    CorrespondenceMethodID,
                    1,
                    IsExcludeZeroBalances,
                    IsExcludeNegativeBalances,
                    FamilyStatusID
                    );
                return Json(new { Message = "The messages is being queued. Please go to the Bulk SMS page under \"Bulk Corespondences\" to complete the sending process." });
            }
        }

        [HttpPost]
        public JsonResult SendSingleSMS(string cellnumber, string message, int KeyValue, int KeyValueType)
        {
            
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_Shared_SingleSMSQueue(
                    Convert.ToInt32(HttpContext.Session["InstID"]),
                    cellnumber,
                    message,
                    KeyValue,
                    KeyValueType,
                    WebSecurity.CurrentUserId
                    );
            }
                
            return Json(new { error = "None" });
        }

        [HttpPost]
        public JsonResult SendSingleCustomSMS(string cellnumber, int messageID, int KeyValue, int KeyValueType)
        {
            using (var Context = new EduSpecDataContext())
            {
                string message = Context.fn_Get_SingleSMSMessage(messageID, KeyValueType, KeyValue);
                
                Context.Set_Shared_SingleSMSQueue(
                    Convert.ToInt32(HttpContext.Session["InstID"]),
                    cellnumber,
                    message,
                    KeyValue,
                    KeyValueType,
                    WebSecurity.CurrentUserId
                    );
            }

            return Json(new { error = "None" });
        }

        #endregion

        #region Emails

        public PartialViewResult SendEmailSingle(int? EmailKeyValue, int? EmailKeyValueType)
        {
            using (var Context = new EduSpecDataContext())
            {
                HttpContext.Session.Add("SingleEmailAttachmentFileName", null);
                HttpContext.Session.Add("BulkEmailAttachmentFileName", null);
                //HttpContext.Session.Add("BulkEmailTypeID", null);
                //HttpContext.Session.Add("EmailKeyValue", EmailKeyValue);
                //HttpContext.Session.Add("EmailKeyValueType", EmailKeyValueType);
                //HttpContext.Session.Add("SingleEmailKeyValueType", EmailKeyValueType);
                return PartialView("SendEmailSingle", Context.Get_Shared_SingleEmailInfo(EmailKeyValue, EmailKeyValueType, Convert.ToInt32(HttpContext.Session["InstID"])).FirstOrDefault());
            }
        }

        public PartialViewResult SendEmailBulk(int BulkEmailTypeID)
        {
            using (var Context = new EduSpecDataContext())
            {
                return PartialView("SendEmailBulk", Context.Get_Shared_BulkEmailInfo(Convert.ToInt32(HttpContext.Session["InstID"]), BulkEmailTypeID).FirstOrDefault());
            }
        }

        public ActionResult CallbacksSingleEmailFileUpload()
        {
            try
            {
                UploadControlExtension.GetUploadedFiles("ucSingleEmailUploadData", UploadControlHelper.EmailFileUploadValidationSettings, UploadControlHelper.ucEmailFileCallbacks_FileUploadComplete);
            }
            catch (Exception e)
            {
                string x = e.Message;
            }

            return null;
        }

        public ActionResult CallbacksSingleEmailCustomFileUpload()
        {
            try
            {
                UploadControlExtension.GetUploadedFiles("ucSingleEmailCustomUploadData", UploadControlHelper.EmailFileUploadValidationSettings, UploadControlHelper.ucEmailFileCallbacks_FileUploadComplete);
            }
            catch (Exception e)
            {
                string x = e.Message;
            }

            return null;
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SendSingleEmail(string Recipient, string CcEmail, string Subject, string MessageEmail, int KeyValue, int KeyValueType)
        {
            
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_Shared_EmailSend(
                    1,//Manual email
                    Convert.ToInt32(HttpContext.Session["InstID"]),
                    Recipient,
                    CcEmail,
                    Subject,
                    MessageEmail,
                    null,
                    KeyValue,
                    KeyValueType,
                    WebSecurity.CurrentUserId,
                    Convert.ToString(HttpContext.Session["SingleEmailAttachmentFileName"])
                    );
            }
            return Json(new { error = "None" });
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SendSingleEmailCustom(string Recipient, string CcEmail, string Subject, int MessageEmail, int KeyValue, int KeyValueType)
        {

            using (var Context = new EduSpecDataContext())
            {
                Context.Set_Shared_EmailSend(
                    2,//Custom email
                    Convert.ToInt32(HttpContext.Session["InstID"]),
                    Recipient,
                    CcEmail,
                    Subject,
                    null,
                    MessageEmail,
                    KeyValue,
                    KeyValueType,
                    WebSecurity.CurrentUserId,
                    Convert.ToString(HttpContext.Session["SingleEmailAttachmentFileName"])
                    );
            }
            return Json(new { error = "None" });
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SetBulkEmailTypeID(int KeyValueType)
        {

            using (var Context = new EduSpecDataContext())
            {

                HttpContext.Session["BulkEmailTypeID"] = KeyValueType;

            }
            return Json(new { error = "None" });
        }

        public ActionResult CallbacksBulkEmailFileUpload()
        {
            try
            {
                UploadControlExtension.GetUploadedFiles("ucBulkEmailUploadData", UploadControlHelper.EmailFileUploadValidationSettings, UploadControlHelper.ucBulkEmailFileCallbacks_FileUploadComplete);
            }
            catch (Exception e)
            {
                string x = e.Message;
            }

            return null;
        }

        [HttpPost]
        public ActionResult SendBulkEmail(int StatusID, int EmailMessageID, string EmailSubject, int EmailTypeID, int? AttachEmailFileType, int? BulkLetterID)
        {
            using (var Context = new EduSpecDataContext())
            {
                if(AttachEmailFileType == null)
                {
                    AttachEmailFileType = null;
                };

                Context.Set_EmailBulk_Add(
                    Convert.ToInt32(HttpContext.Session["InstID"]),
                    StatusID,
                    EmailMessageID,
                    EmailSubject,
                    WebSecurity.CurrentUserId,
                    EmailTypeID,
                    AttachEmailFileType,
                    BulkLetterID,
                    Convert.ToString(HttpContext.Session["BulkEmailAttachmentFileName"])
                    );
                return Json(new { Success = true });
            }
        }

        #endregion

        #region Document Viewer

        public PartialViewResult DocumentViewerPartial()
        {
            XtraReport report = ReportsController.rptExemptionCompensasion(10, 2);
            return PartialView("DocumentViewer", report);
            //using (var Context = new EduSpecDataContext())
            //{
            //    //int OrderID = Convert.ToInt32(HttpContext.Session["FocusedOrderID"]);

            //    //if (Context.fn_IsMultiBranchOrder(OrderID).Value == true)
            //    //    ViewData["Report"] = getMultiBranchOrder();
            //    //else
            //    //    ViewData["Report"] = getSingleBranshOrder();
            //    return PartialView("DocumentViewerPartial");
            //}
        }

        #endregion

        #region Generate apply for application 

        public PartialViewResult ApplicantToApplyOrReprint(int ApplicationKeyValue, int ApplicationKeyValueType)
        {
            using (var Context = new EduSpecDataContext())
            {
                return PartialView("ApplicantToApplyOrReprint", Context.Get_Shared_ApplicantToApplyOrReprint(ApplicationKeyValue, ApplicationKeyValueType).FirstOrDefault());
            }
        }

        public JsonResult ReCreateApplication(int ExemptionID, int LanguageID, bool IsSingleApplication, int ApplMaritalStatusID, int? Wareabouts, int? Contact, int? Unwilling, bool? isAttachCovidForm)
        {
            using (var Context = new EduSpecDataContext())
            {
                int InstID = Convert.ToInt32(HttpContext.Session["InstID"]);
                //
                if (isAttachCovidForm == null)
                {
                    isAttachCovidForm = false;
                }

                try
                {
                  var  application = Context.Set_Shared_ReCreateApplication(
                                        ExemptionID,
                                        WebSecurity.CurrentUserId,
                                        LanguageID,
                                        ApplMaritalStatusID,
                                        IsSingleApplication,
                                        Wareabouts,
                                        Contact,
                                        Unwilling
                                        ).FirstOrDefault();

                    
                    var redirectUrl = new UrlHelper(Request.RequestContext).Action(
                        "PrintApplication",
                        "Shared",
                        new
                        {
                            ExemptionID,
                            application.IsPrintCoverPage,
                            application.IsPrintLetterHead,
                            LanguageID,
                            ApplMaritalStatusID,
                            IsSingleApplication,
                            Wareabouts,
                            Contact,
                            Unwilling,
                            isAttachCovidForm,
                            application.FileName
                        });


                    return Json(new { Success = true, ExceptionMessage = "", Url = redirectUrl });
                }
                catch (Exception e)
                {
                    return Json(new { Success = false, ExceptionMessage = e.Message, Url = "" });
                }

            }
        }

        public ActionResult PrintApplication(int ExemptionID, bool IsPrintCoverPage, bool IsPrintLetterHead, int LanguageID, int ApplMaritalStatusID, bool IsSingleApplication, int? Wareabouts, int? Contact, int? Unwilling, bool isAttachCovidForm, string FileName)
        {
            using (var Context = new EduSpecDataContext())
            {
                MemoryStream stream = new MemoryStream();
                try
                {
                    XtraReport report = ReportsController.rptExemptionApplication(
                        Convert.ToInt32(HttpContext.Session["InstID"]),
                        Context.fn_Get_ActiveYearID().Value,
                        ExemptionID,
                        IsPrintCoverPage,
                        IsPrintLetterHead,
                        LanguageID,
                        ApplMaritalStatusID,
                        IsSingleApplication,
                        Wareabouts,
                        Contact,
                        Unwilling,
                        isAttachCovidForm,
                        FileName);
                    
                    report.DisplayName = "Aplication For Exemption " + ExemptionID;
                    
                    string NewFilename = string.Format("{0}{1}", SystemParameters.SystemParams().ApplicationsFilePath, FileName);
                    
                    //
                    try
                    {
                        report.ExportToPdf(NewFilename);
                    }
                    catch (Exception e)
                    {
                        string message = e.Message;
                    }
                    finally
                    {
                        PdfExportOptions options = new PdfExportOptions() { ShowPrintDialogOnOpen = true };
                        report.ExportToPdf(stream, options);
                        stream.Seek(0, SeekOrigin.Begin);
                    }

                    return new FileStreamResult(stream, "application/pdf");
                }
                catch (Exception e)
                {
                    string message = e.Message;
                    return null;
                }
            }
        }

        #endregion

        #region Finalise reasons

        public PartialViewResult FinaliseApplicationReason(int? ExemptionTypeID, int? FinaliseReasonTypeID)
        {
            using (var Context = new EduSpecDataContext())
            {
                HttpContext.Session["ExemptionType"] = ExemptionTypeID;
                HttpContext.Session["FinaliseReasonTypeID"] = FinaliseReasonTypeID;
                return PartialView("FinaliseApplicationReason");
                
            }
        }

        public PartialViewResult ExemptionFinaliseReason()
        {
            using (var Context = new EduSpecDataContext())
            {
                ViewData["ExemptionFinaliseReasonViewProperties"] = ViewProperties.viewProperties("Shared - Exemption finalise reason", WebSecurity.CurrentUserId);
                
                return PartialView("ExemptionFinaliseReason", Context.Get_Shared_FinaliseApplicationReason_View(Convert.ToInt32(HttpContext.Session["InstID"]), Convert.ToString(HttpContext.Session["ApplicationNumber"]), Convert.ToInt32(HttpContext.Session["FinaliseReasonTypeID"])).ToList());
             }
        }

        [HttpPost]
        public ActionResult FinaliseReasonAdd(Get_Shared_FinaliseApplicationReason_ViewResult Reason, int? ExemptionTypeID, int? FinaliseReasonTypeID)
        {
            using (var Context = new EduSpecDataContext())
            {
                Context.Set_Exemptions_FinaliseReason_Add(
                        Convert.ToInt32(HttpContext.Session["InstID"]),
                        WebSecurity.CurrentUserId,
                        Convert.ToInt32(HttpContext.Session["FinaliseReasonTypeID"]),
                        Convert.ToInt32(HttpContext.Session["ExemptionID"]),
                        Reason.Description
                    );
                return ExemptionFinaliseReason();
            }
        }

        #endregion

        #region Reports

        public PartialViewResult reportSMSMessageHistory()
        {
            DateTime DateFrom = DateTime.Now.AddDays(-20);
            DateTime DateTo = DateTime.Now;

            XtraReport report = new SMSHistory();
            report.Parameters["InstID"].Value = HttpContext.Session["InstID"];
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
            report.Parameters["InstID"].Value = HttpContext.Session["InstID"];
            report.Parameters["DateFrom"].Value = DateFrom;
            report.Parameters["DateTo"].Value = DateTo;
            report.Parameters["BulkEmailTypeID"].Value = HttpContext.Session["BulkEmailTypeID"];
            report.CreateDocument();
            return PartialView("reportEmailHistory", report);
        }

        #endregion
    }
}