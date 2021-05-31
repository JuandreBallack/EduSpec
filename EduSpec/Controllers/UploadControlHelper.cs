using CsvHelper.Configuration;
using DevExpress.Web;
using EduSpec.Models;
using NLog;
using System;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Windows.Forms;

namespace EduSpec.Controllers
{
    [SessionExpire]
    public class UploadControlHelper
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public static string FileName { get; set; }

        public static readonly UploadControlValidationSettings UploadControlValidationSettings = new UploadControlValidationSettings
        {
            AllowedFileExtensions = new string[] { ".xlsx", ".xls", ".txt" },
            MaxFileSize = 20971520
        };

        public static readonly UploadControlValidationSettings DataImportValidationSettings = new UploadControlValidationSettings
        {
            AllowedFileExtensions = new string[] { ".mdb" },
            MaxFileSize = 314572800
        };

        public static readonly UploadControlValidationSettings EmailFileUploadValidationSettings = new UploadControlValidationSettings
        {
            AllowedFileExtensions = new string[] { ".pdf", ".xlsx", ".xls", ".mdb", ".jpg", ".png", ".rar", ".zip", ".7z" },
            MaxFileSize = 20971520
        };

        public static readonly UploadControlValidationSettings SupportDocsUploadValidationSettings = new UploadControlValidationSettings
        {
            AllowedFileExtensions = new string[] { ".pdf", ".xlsx", ".xls", ".mdb", ".jpg", ".png", ".rar", ".zip", ".7z" },
            //AllowedFileExtensions = new string[] { ".mdb" },
            MaxFileSize = 314572800
        };

        public static readonly UploadControlValidationSettings NoteFileUploadValidationSettings = new UploadControlValidationSettings
        {
            AllowedFileExtensions = new string[] { ".jpg", ".jpeg", ".gif", ".png", ".pdf" },
            MaxFileSize = 314572800
        };

        public static void AgeAnalysisImportFileFromExcel(object sender, FileUploadCompleteEventArgs e)
        {
            if (e.UploadedFile.IsValid)
            {
                try
                {
                    string resultFilePath = String.Format("{0}{1}", SystemParameters.SystemParams().AgeAnalysisFilePath, e.UploadedFile.FileName);
                    logger.Info("Save age import file : " + resultFilePath);
                    var s = new UploadedFile();
                    s = e.UploadedFile;
                    s.SaveAs(resultFilePath);
                    logger.Info("File saved.");
                    IUrlResolutionService urlResolver = sender as IUrlResolutionService;
                    if (urlResolver != null)
                        e.CallbackData = urlResolver.ResolveClientUrl(resultFilePath);
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message);
                }
            }
        }

        public static void ucEmailFileCallbacks_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            try
            {
                if (e.UploadedFile.IsValid)
                {
                    using (var Context = new EduSpecDataContext())
                    {
                        logger.Info("Beginning process of single email file upload...");
                        string resultFilePath = null;
                        string fileExt = Path.GetExtension(e.UploadedFile.FileName);
                        string fileName = String.Format("{0}{1}", Context.fn_Get_EmailFileName(Convert.ToInt32(HttpContext.Current.Session["InstID"])), fileExt);

                        resultFilePath = String.Format("{0}{1}", SystemParameters.SystemParams().EmailAttachementsFilePath, fileName);
                        logger.Info(String.Format("Result file path for single email file : {0}", resultFilePath));

                        logger.Info(String.Format("Trying to save file for single email : {0}", fileName));

                        var file = new UploadedFile();
                        file = e.UploadedFile;
                        file.SaveAs(resultFilePath);
                        HttpContext.Current.Session["SingleEmailAttachmentFileName"] = fileName;

                        logger.Info(String.Format("Finished saving single email file: {0}", fileName));

                    }
                }
            }
            catch (Exception ex)
            {
                logger.Info("Error: Could not read file from disk. Original error: " + ex.Message);
                MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
            }
        }

        public static void ucBulkEmailFileCallbacks_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            try
            {
                if (e.UploadedFile.IsValid)
                {
                    using (var Context = new EduSpecDataContext())
                    {
                        logger.Info("Beginning process of bulk email file upload...");
                        string resultFilePath = null;
                        string fileExt = Path.GetExtension(e.UploadedFile.FileName);
                        string str = e.UploadedFile.FileName.ToString();
                        string fileName = String.Format("{0}{1}", str.Substring(0, str.IndexOf(".pdf") - 1) + ' ' + DateTime.Now.ToString("yyyy-MM-dd"), fileExt);

                        if (Convert.ToInt32(HttpContext.Current.Session["BulkEmailTypeID"]) == 1)
                        {
                            resultFilePath = String.Format("{0}{1}", SystemParameters.SystemParams().PrintoutsFilePath, fileName);
                        }
                        else
                        {
                            resultFilePath = String.Format("{0}{1}", SystemParameters.SystemParams().EmailAttachementsFilePath, fileName);
                        }

                        logger.Info(String.Format("Result file path for bulk email file : {0}", resultFilePath));

                        logger.Info(String.Format("Trying to save file for bulk email : {0}", fileName));

                        var file = new UploadedFile();
                        file = e.UploadedFile;
                        file.SaveAs(resultFilePath);
                        HttpContext.Current.Session["BulkEmailAttachmentFileName"] = fileName;

                        logger.Info(String.Format("Finished saving bulk email file: {0}", fileName));

                    }
                }
            }
            catch (Exception ex)
            {
                logger.Info("Error: Could not read file from disk. Original error: " + ex.Message);
                MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
            }
        }

        public static void ucSupportFileCallbacks_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            try
            {
                if (e.UploadedFile.IsValid)
                {
                    using (var Context = new EduSpecDataContext())
                    {

                        string fileExt = Path.GetExtension(e.UploadedFile.FileName);
                        string fileName = String.Format("{0}{1}", Context.fn_Get_SupportFileName(Convert.ToInt32(HttpContext.Current.Session["InstID"])), fileExt);
                        string resultFilePath = String.Format("{0}{1}", SystemParameters.SystemParams().SupportUploadFilePath, fileName);

                        logger.Info(String.Format("Trying to save support file: {0}", fileName));

                        var file = new UploadedFile();
                        file = e.UploadedFile;
                        file.SaveAs(resultFilePath);
                        HttpContext.Current.Session["SupportAttachmentFileName"] = fileName;

                        logger.Info(String.Format("Finished saving support file: {0}", fileName));
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Info("Error: Could not read file from disk. Original error: " + ex.Message);
                MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
            }
        }

        public static void ucCustomEmailFileCallbacks_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            try
            {
                if (e.UploadedFile.IsValid)
                {
                    using (var Context = new EduSpecDataContext())
                    {
                        logger.Info("Beginning process of correspondence(custom) email file upload...");
                        string resultFilePath = null;
                        string fileExt = Path.GetExtension(e.UploadedFile.FileName);
                        string fileName = String.Format("{0}{1}", Context.fn_Get_FamilyCorrespondenceEmailFileName(Convert.ToInt32(HttpContext.Current.Session["InstID"])), fileExt);

                        resultFilePath = String.Format("{0}{1}", SystemParameters.SystemParams().EmailAttachementsFilePath, fileName);
                        logger.Info(String.Format("Result file path for custom email file : {0}", resultFilePath));

                        logger.Info(String.Format("Trying to save file for custom email : {0}", fileName));

                        var file = new UploadedFile();
                        file = e.UploadedFile;
                        file.SaveAs(resultFilePath);
                        HttpContext.Current.Session["CustomEmailAttachmentFileName"] = fileName;

                        logger.Info(String.Format("Finished saving custom correspondence email file: {0}", fileName));

                    }
                }
            }
            catch (Exception ex)
            {
                logger.Info("Error: Could not read file from disk. Original error: " + ex.Message);
                MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
            }
        }

        public static void ucManualEmailFileCallbacks_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            try
            {
                if (e.UploadedFile.IsValid)
                {
                    using (var Context = new EduSpecDataContext())
                    {
                        logger.Info("Beginning process of correspondence(manual) email file upload...");
                        string resultFilePath = null;
                        string fileExt = Path.GetExtension(e.UploadedFile.FileName);
                        string fileName = String.Format("{0}{1}", Context.fn_Get_FamilyCorrespondenceEmailFileName(Convert.ToInt32(HttpContext.Current.Session["InstID"])), fileExt);

                        resultFilePath = String.Format("{0}{1}", SystemParameters.SystemParams().EmailAttachementsFilePath, fileName);
                        logger.Info(String.Format("Result file path for manual email file : {0}", resultFilePath));

                        logger.Info(String.Format("Trying to save file for manual email : {0}", fileName));

                        var file = new UploadedFile();
                        file = e.UploadedFile;
                        file.SaveAs(resultFilePath);
                        HttpContext.Current.Session["ManualEmailAttachmentFileName"] = fileName;

                        logger.Info(String.Format("Finished saving manual correspondence email file: {0}", fileName));

                    }
                }
            }
            catch (Exception ex)
            {
                logger.Info("Error: Could not read file from disk. Original error: " + ex.Message);
                MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
            }
        }

        public static void ucNoteFileCallbacks_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            try
            {
                if (e.UploadedFile.IsValid)
                {
                    using (var Context = new EduSpecDataContext())
                    {
                        logger.Info("Beginning process of new note file upload...");
                        string resultFilePath = null;
                        string fileName = (e.UploadedFile.FileName);
                        resultFilePath = String.Format("{0}{1}", SystemParameters.SystemParams().AgeAnalysisFilePath, fileName);
                        logger.Info(String.Format("Result file path for new note file : {0}", resultFilePath));
                        logger.Info(String.Format("Trying to save file for note : {0}", fileName));

                        var file = new UploadedFile();
                        file = e.UploadedFile;
                        file.SaveAs(resultFilePath);

                        logger.Info(String.Format("Finished saving new note file: {0}", fileName));
                        e.CallbackData = fileName;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Info("Error: Could not read file from disk. Original error: " + ex.Message);
                MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
            }
        }

        #region Data Import
        public class Classes
        {
            public int ClassID { get; set; }
            public int Grade { get; set; }
            public string ClassName { get; set; }
        }
        public sealed class ClassesMap : ClassMap<Classes>
        {
            public ClassesMap()
            {
                Map(m => m.ClassID).Index(0);
                Map(m => m.Grade).Index(1);
                Map(m => m.ClassName).Index(2);
            }
        }
        public class Parents
        {
            public int ParentID { get; set; }
            public string ExternalAccount { get; set; }
            public string FamilyCode { get; set; }
            public string Status { get; set; }
            public string MaritalStatus { get; set; }
            public string ParentTitle { get; set; }
            public string ParentInitials { get; set; }
            public string ParentFullName { get; set; }
            public string ParentSurname { get; set; }
            public string ParentIDNumber { get; set; }
            public string ParentRelation { get; set; }
            public string ParentEmployer { get; set; }
            public string ParentOccupation { get; set; }
            public string StreetAddress1 { get; set; }
            public string StreetAddress2 { get; set; }
            public string StreetAddress3 { get; set; }
            public string StreetCode { get; set; }
            public string PostalAddress1 { get; set; }
            public string PostalAddress2 { get; set; }
            public string PostalAddress3 { get; set; }
            public string PostalCode { get; set; }
            public string Tel1Code { get; set; }
            public string Tel1 { get; set; }
            public string Tel2Code { get; set; }
            public string Tel2 { get; set; }
            public string Tel3Code { get; set; }
            public string Tel3 { get; set; }
            public string EMail { get; set; }
            public string FaxCode { get; set; }
            public string FaxNo { get; set; }
            public string Gender { get; set; }
            public string Race { get; set; }
            public string HomeLanguage { get; set; }
            public string SpouseFullName { get; set; }
            public string SpouseSurname { get; set; }
            public string SpouseOccupation { get; set; }
            public string SpouseID { get; set; }
            public string SpouseWorkTel { get; set; }
            public string SpouseGender { get; set; }
            public string SpouseCell { get; set; }
            public string SpouseEmail { get; set; }
        }
        public sealed class ParentsMap : ClassMap<Parents>
        {
            public ParentsMap()
            {
                Map(m => m.ParentID).Index(0);
                Map(m => m.ExternalAccount).Index(1);
                Map(m => m.FamilyCode).Index(2);
                Map(m => m.Status).Index(3);
                Map(m => m.MaritalStatus).Index(4);
                Map(m => m.ParentTitle).Index(5);
                Map(m => m.ParentInitials).Index(6);
                Map(m => m.ParentFullName).Index(7);
                Map(m => m.ParentSurname).Index(8);
                Map(m => m.ParentIDNumber).Index(9);
                Map(m => m.ParentRelation).Index(10);
                Map(m => m.ParentEmployer).Index(11);
                Map(m => m.ParentOccupation).Index(12);
                Map(m => m.StreetAddress1).Index(13);
                Map(m => m.StreetAddress2).Index(14);
                Map(m => m.StreetAddress3).Index(15);
                Map(m => m.StreetCode).Index(16);
                Map(m => m.PostalAddress1).Index(17);
                Map(m => m.PostalAddress2).Index(18);
                Map(m => m.PostalAddress3).Index(19);
                Map(m => m.PostalCode).Index(20);
                Map(m => m.Tel1Code).Index(21);
                Map(m => m.Tel1).Index(22);
                Map(m => m.Tel2Code).Index(23);
                Map(m => m.Tel2).Index(24);
                Map(m => m.Tel3Code).Index(25);
                Map(m => m.Tel3).Index(26);
                Map(m => m.EMail).Index(27);
                Map(m => m.FaxCode).Index(28);
                Map(m => m.FaxNo).Index(29);
                Map(m => m.Gender).Index(30);
                Map(m => m.Race).Index(31);
                Map(m => m.HomeLanguage).Index(32);
                Map(m => m.SpouseFullName).Index(33);
                Map(m => m.SpouseSurname).Index(34);
                Map(m => m.SpouseOccupation).Index(35);
                Map(m => m.SpouseID).Index(36);
                Map(m => m.SpouseWorkTel).Index(37);
                Map(m => m.SpouseGender).Index(38);
                Map(m => m.SpouseCell).Index(39);
                Map(m => m.SpouseEmail).Index(40);
            }
        }
        public class Learners
        {
            public int LearnerID { get; set; }
            public int ParentID { get; set; }
            public string AccessionNo { get; set; }
            public string EntryDate { get; set; }
            public string DateLeft { get; set; }
            public string Title { get; set; }
            public string Initials { get; set; }
            public string FirstName { get; set; }
            public string SecondName { get; set; }
            public string Surname { get; set; }
            public string Birthday { get; set; }
            public string IDNo { get; set; }
            public string Gender { get; set; }
            public string Language { get; set; }
            public string LanguageID { get; set; }
            public string PreferredLanguageID { get; set; }
            public string Tel1Code { get; set; }
            public string Tel1 { get; set; }
            public string Tel2Code { get; set; }
            public string Tel2 { get; set; }
            public string Email { get; set; }
            public string Grade { get; set; }
            public string ClassID { get; set; }
            public string ClassName { get; set; }
            public string Race { get; set; }
            public string Status { get; set; }
            public string PositionFamily { get; set; }
        }
        public sealed class LearnersMap : ClassMap<Learners>
        {
            public LearnersMap()
            {
                Map(m => m.LearnerID).Index(0);
                Map(m => m.ParentID).Index(1);
                Map(m => m.AccessionNo).Index(2);
                Map(m => m.EntryDate).Index(3);
                Map(m => m.DateLeft).Index(4);
                Map(m => m.Title).Index(5);
                Map(m => m.Initials).Index(6);
                Map(m => m.FirstName).Index(7);
                Map(m => m.SecondName).Index(8);
                Map(m => m.Surname).Index(9);
                Map(m => m.Birthday).Index(10);
                Map(m => m.IDNo).Index(11);
                Map(m => m.Gender).Index(12);
                Map(m => m.Language).Index(13);
                Map(m => m.LanguageID).Index(14);
                Map(m => m.PreferredLanguageID).Index(15);
                Map(m => m.Tel1Code).Index(16);
                Map(m => m.Tel1).Index(17);
                Map(m => m.Tel2Code).Index(18);
                Map(m => m.Tel2).Index(19);
                Map(m => m.Email).Index(20);
                Map(m => m.Grade).Index(21);
                Map(m => m.ClassID).Index(22);
                Map(m => m.ClassName).Index(23);
                Map(m => m.Race).Index(24);
                Map(m => m.Status).Index(25);
                Map(m => m.PositionFamily).Index(26);
            }
        }
        public enum RecordType
        {
            None = 0,
            Class,
            Parent,
            Learner
        }

        public static void Integration_ImportSASAMS(object sender, FileUploadCompleteEventArgs e)
        {
            if (e.UploadedFile.IsValid)
            {
                //int InstID = Convert.ToInt32(HttpContext.Current.Session["InstID"]);
                string resultFilePath = String.Format("{0}{1}", SystemParameters.SystemParams().InstitutionDataUploadFilePath, e.UploadedFile.FileName);
                var s = new UploadedFile();
                s = e.UploadedFile;
                s.SaveAs(resultFilePath);
                IUrlResolutionService urlResolver = sender as IUrlResolutionService;
                if (urlResolver != null)
                    e.CallbackData = urlResolver.ResolveClientUrl(resultFilePath);
            }
        }
        #endregion
    }                                                                                   
}

