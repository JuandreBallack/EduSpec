using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduSpecDataService.Code
{
    class AccessCommands
    {

        public static string SASAMS_Learner()
        {
            return
                "SELECT DISTINCT " +
                "	l.ID, " +
                "	l.LearnerID, " +
                "	l.AccessionNo, " +
                "	l.TheDate AS EntryDate, " +
                "	IIF(IsNull(l.DateLeft),'',l.DateLeft) AS DateLeft, " +
                "	IIF(IsNull(l.Title),'',l.Title) AS Title, " +
                "	IIF(IsNull(l.Initials),'',l.Initials) AS Initials, " +
                "	l.FName + ' ' + IIF(IsNull(l.SecondName),'',l.SecondName) + ' ' + IIF(IsNull(l.ThirdName),'',ThirdName) AS FullName, " +
                "	l.SName AS Surname, " +
                "	l.BirthDate, " +
                "	l.IDNo AS IDNumber, " +
                "	l.Gender, " +
                "	tl.LangDesc, " +
                "	IIF(IsNull(l.Tel1Code),'',l.Tel1Code) + IIF(IsNull(l.Tel1),'',l.Tel1) AS TelHome, " +
                "	IIF(IsNull(l.Tel2Code),'',l.Tel2Code) + IIF(IsNull(l.Tel2),'',l.Tel2) AS TelCell, " +
                "	IIF(IsNull(l.Email),'',l.Email) AS Email, " +
                "	l.Grade AS GradeID, " +
                "	IIF(IsNull(l.Class),0,l.Class) AS ClassID, " +
                "	IIF(IsNull(cl.ClassName),'',cl.ClassName) AS ClassName, " +
                "	l.Race, " +
                "	IIF(IsNull(l.LuritsNumber),0,l.LuritsNumber) AS LuritsNumber, " +
                "	l.Status " +
                "FROM ((Learner_Info l " +
                "LEFT JOIN InstructionLanguages tl " +
                "	ON (tl.LangID = l.InstructionLanguage)) " +
                "LEFT JOIN Classes cl " +
                "	ON (l.Class = cl.ClassId)) " +
                "ORDER BY l.ID ";
        }

        public static string SASAMS_Parents()
        {
            return
                "SELECT " +
                "	ParentID, " +
                "	Title, " +
                "	Initials, " +
                "	FName AS FullName, " +
                "	SName AS Surname, " +
                "	IDNumber, " +
                "	Employer, " +
                "	Occupation, " +
                "	StreetAddress1, " +
                "	StreetAddress2, " +
                "	StreetAddress3, " +
                "	StreetCode, " +
                "	PostalAddress1, " +
                "	PostalAddress2, " +
                "	PostalAddress3, " +
                "	PostalCode, " +
                "	IIF(IsNull(Tel1Code),'',Tel1Code) + IIF(IsNull(Tel1),'',Tel1) AS TelHome, " +
                "	IIF(IsNull(Tel2Code),'',Tel2Code) + IIF(IsNull(Tel2),'',Tel2) AS TelWork, " +
                "	IIF(IsNull(Tel3Code),'',Tel3Code) + IIF(IsNull(Tel3),'',Tel3) AS TelCell, " +
                "	IIF(IsNull(FaxCode),'',FaxCode) + IIF(IsNull(FaxNo),'',FaxNo) AS TelFax, " +
                "	EMail, " +
                "	Gender, " +
                "	Homelanguage, " +
                "	SpouseFname AS SpouseFullName, " +
                "	Spouse AS SpouseSurname, " +
                "	SpouseID AS SpouseIDNumber, " +
                "	SpouseOccupation, " +
                "	SpouseWorkTel, " +
                "	SpouseGender, " +
                "	SpouseCell, " +
                "	SpouseEmail, " +
                "	MaritalStatus, " +
                "	Relship AS Relationship, " +
                "	Status " +
                "FROM Parent_Info " +
                "ORDER BY ParentID ";
        }
    
        public static string SASAMS_Parent_Child()
        {
            return
                "SELECT " +
                "  ParentId, " +
                "  ChildId, " +
                "  Learnerid, " +
                "  Status, " +
                "  FamilyCode " +                
                "FROM Parent_Child " +
                "ORDER BY ChildId";
        }

        public static string Sync_AddNewLearners()
        {
            return
                "INSERT INTO Learner_Info_Sync (InstID, ID, LearnerID, AccessionNo, EntryDate, DateLeft, Title, Initials, FullName, Surname, BirthDate, IDNumber, Gender, Language, TelHome, TelCell, Email, GradeID, ClassID, ClassName, Race, LuritsNumber, Status, LearnerStatus) " +
                "SELECT @InstID, ID, LearnerID, AccessionNo, EntryDate, DateLeft, Title, Initials, FullName, Surname, BirthDate, IDNumber, Gender, LangDesc, TelHome, TelCell, Email, GradeID, ClassID, ClassName, Race, LuritsNumber, Status, @LearnerStatus FROM Learner_Info WHERE ID NOT IN (SELECT ID FROM Learner_Info_Sync)";
        }
        public static string SASAMS_Transactions()
        {
            return
                "SELECT " +
                "SubAccNumber, " +
                "Date, " +
                "Operation, " +
                "Switch(DebitAmount <= 0, CreditAmount * -1,DebitAmount > 0, DebitAmount) AS Amount " +
                "FROM GLTrans " + 
                "WHERE AccNumber = 700 AND SubAccNumber > 0";
        }
    }    
}
