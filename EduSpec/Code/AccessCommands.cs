using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EduSpec.Code
{
    public class AccessCommands
    {
        public static string SASAMS_Import(int InstID)
        {
            return
                "SELECT " + InstID + ", NULL, " +
                " IIF(IsNull(pc.FamilyCode) = 0, pc.FamilyCode, '') AS FamilyCode, NULL, NULL, NULL, NULL, NULL," +
                " l.ID, " +
                " l.LearnerID, " +
                " pi.ParentID, " +
                " 0, l.AccessionNo, " +
                " l.LuritsNumber, " +
                " l.TheDate AS EntryDate, " +
                " IIF(IsNull(l.DateLeft) = 0, l.DateLeft, '') AS DateLeft, " +
                " l.Title AS LearnerTitle, " +
                " l.Initials AS LearnerInitials, " +
                " l.FName + ' ' + SecondName AS LearnerName, " +
                " l.SName AS LearnerSurname, " +
                " l.BirthDate AS LearnerBirthDate, " +
                " l.IDNo AS LearnerIDNumber, " +
                " l.Gender AS LearnerGender, " +
                " il.LangDesc AS LearnerLanguage, " +
                " IIF(IsNull(l.Tel1Code) = 0, l.Tel1Code, '') + IIF(IsNull(l.Tel1) = 0, l.Tel1, '') AS LearnerTelHome, " +
                " IIF (IsNull(l.Tel2Code) = 0, l.Tel2Code, '') +IIF(IsNull(l.Tel2) = 0, l.Tel2, '') AS LearnerTelCell, " +
                " IIF (IsNull(l.DateLeft) = 0 AND l.DateLeft <> '',99,l.Grade) AS Grade, " +
                " IIF (IsNull(l.DateLeft) = 0 AND l.DateLeft <> '','Left School',lc.ClassName) AS ClassName, " +
                " l.Race AS LearnerNationality, " +
                " l.Email AS LearnerEmail, " +
                " pi.Relship AS ParentRelation, " +
                " pi.Title AS ParentTitle, " +
                " pi.Initials AS ParentInitials, " +
                " pi.FName AS ParentName, " +
                " pi.SName AS ParentSurname, " +
                " pi.IDNumber AS ParentIDNumber, " +
                " pi.Gender AS ParentGender, " +
                " pi.Employer AS ParentEmployer, " +
                " pi.Occupation AS ParentOccupation, " +
                " IIF(IsNull(pi.Tel1Code) = 0, pi.Tel1Code, '') + IIF(IsNull(pi.Tel1) = 0, pi.Tel1, '') AS ParentTelHome, " +
                " IIF (IsNull(pi.Tel2Code) = 0, pi.Tel2Code, '') +IIF(IsNull(pi.Tel2) = 0, pi.Tel2, '') AS ParentTelWork, " +
                " IIF (IsNull(pi.Tel3Code) = 0, pi.Tel3Code, '') +IIF(IsNull(pi.Tel3) = 0, pi.Tel3, '') AS ParentTelCell, " +
                " pi.EMail AS ParentEmail, " +
                " pi.StreetAddress1, " +
                " pi.StreetAddress2, " +
                " pi.StreetAddress3, " +
                " pi.StreetCode, " +
                " pi.PostalAddress1, " +
                " pi.PostalAddress2, " +
                " pi.PostalAddress3, " +
                " pi.PostalCode, " +
                " '' AS SpouceTitle, " +
                " '' AS SpouceInitials, " +
                " pi.SpouseFname AS SpouceName, " +
                " pi.Spouse AS SpouceSurname, " +
                " pi.SpouseID AS SpouceIDNumber, " +
                " pi.SpouseGender, " +
                " '' AS SpouceEmployer, " +
                " pi.SpouseOccupation, " +
                " pi.SpouseWorkTel AS SpouceTelWork, " +
                " pi.SpouseCell AS SpouceTelCell, " +
                " pi.SpouseEmail, " +
                " il.LangDesc AS HomeLanguage, " +
                " pi.Maritalstatus, " +
                " pi.Status " +
                "FROM((((Learner_Info l " +
                "LEFT JOIN InstructionLanguages il " +
                "ON(l.InstructionLanguage = il.LangID)) " +
                "LEFT JOIN Classes lc " +
                "ON(l.Class = lc.ClassId)) " +
                "INNER JOIN Parent_Child pc " +
                "ON(l.ID = pc.ChildId)) " +
                "INNER JOIN Parent_Info pi " +
                "ON(pc.ParentId = pi.ParentID))";
        }
        public static string SASAMS_LearnerClasses(int InstID)
        {
            return "SELECT " + InstID + ", ClassId, Grade, ClassName FROM Classes";
        }
        public static string SASAMS_Parent_Child(int InstID)
        {
            return
                "SELECT DISTINCT " +
                InstID + ",NULL, " +
                " ParentId, " +
                " ChildId, " +
                " Learnerid, " +
                " Status, " +
                " FamilyCode, " +
                " PastelCustomerAccountDescription, " +
                " IIF(IsNull(PastelCustomerCategoryCode) = 0, PastelCustomerCategoryCode, -1) " +
                "FROM Parent_Child";
        }
        public static string SASAMS_Learner(int InstID)
        {
            return
                "SELECT DISTINCT " +
                InstID + ", " +
                "l.ID, " +
                "l.LearnerID, " +
                "l.AccessionNo, " +
                "l.TheDate, " +
                "IIF(IsNull(l.DateLeft) = 0, l.DateLeft, ''), " +
                "l.Title, " +
                "l.Initials, " +
                "l.FName, " +
                "l.SecondName, " +
                "l.SName, " + "" +
                "l.BirthDate, " +
                "l.IDNo, " +
                "l.Gender, " +
                "tl.LangDesc AS HomeLanguage, " +
                "IIF(IsNull(l.HomeLanguage) = 0, l.HomeLanguage, -1), " +
                "l.Tel1Code, " +
                "l.Tel1, " +
                "l.Tel2Code, " +
                "l.Tel2, " +
                "l.Tel3Code, " +
                "l.Tel3, " +
                "l.Email, " +
                "l.Grade, " +
                "l.Class, " +
                "cl.ClassName, " +
                "l.Race, " +
                "l.Status " +
                "FROM ((Learner_Info l " +
                "LEFT JOIN InstructionLanguages tl " +
                "ON (tl.LangID = l.HomeLanguage)) " +
                "LEFT JOIN Classes cl " +
                "ON (l.Class = cl.ClassId))";
        }

        public static string SASAMS_Parents(int InstID)
        {
            return
                "SELECT DISTINCT " +
                InstID + ",null,null, " +
                "	CLng(p.ParentID) AS ParentID, " +
                "	p.Title, " +
                "	p.Initials, " +
                "	p.FName, " +
                "	p.SName, " +
                "	p.IDNumber, " +
                "	p.Relship, " +
                "	p.Employer, " +
                "	p.Occupation, " +
                "	p.StreetAddress1, " +
                "	p.StreetAddress2, " +
                "	p.StreetAddress3, " +
                "	p.StreetCode, " +
                "	p.PostalAddress1, " +
                "	p.PostalAddress2, " +
                "	p.PostalAddress3, " +
                "	p.PostalCode, " +
                "	p.Tel1Code, " +
                "	p.Tel1, " +
                "	p.Tel2Code, " +
                "	p.Tel2, " +
                "	p.Tel3Code, " +
                "	p.Tel3, " +
                "	p.EMail, " +
                "	p.FaxCode, " +
                "	p.FaxNo, " +
                "	p.Gender, " +
                "	p.Race, " +
                "	p.Homelanguage, " +
                "	p.SpouseFname, " +
                "	p.Spouse, " +
                "	p.SpouseOccupation, " +
                "	p.SpouseID, " +
                "	p.SpouseWorkTel, " +
                "	p.SpouseGender, " +
                "	p.SpouseCell, " +
                "	p.SpouseEmail,  " +
                "	p.maritalstatus  " +
                "FROM (Parent_Info p " +
                "INNER JOIN Parent_Child pc " +
                "ON(p.ParentID = pc.ParentId))";
        }
    }
}