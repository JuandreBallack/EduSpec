using System;

namespace EduSpecDataService.Models
{
    public class Learner
    {
        public Int32 InstID { get; set; }
        public Int32 ID { get; set; }
        public string LearnerID { get; set; }
        public string AccessionNo { get; set; }
        public string EntryDate { get; set; }
        public string DateLeft { get; set; }
        public string Title { get; set; }
        public string Initials { get; set; }
        public string FullName { get; set; }
        public string Surname { get; set; }
        public string BirthDate { get; set; }
        public string IDNumber { get; set; }
        public string Gender { get; set; }
        public string Language { get; set; }
        public string TelHome { get; set; }
        public string TelCell { get; set; }
        public string Email { get; set; }
        public Int32 GradeID { get; set; }
        public Int32 ClassID { get; set; }
        public string ClassName { get; set; }
        public string Race { get; set; }
        public double LuritsNumber { get; set; }
        public string Status { get; set; }
    }
    public class Parent
    {
        public Int32 InstID { get; set; }
        public Int32 ParentID { get; set; }
        public string Title { get; set; }
        public string Initials { get; set; }
        public string FullName { get; set; }
        public string Surname { get; set; }
        public string IDNumber { get; set; }
        public string Employer { get; set; }
        public string Occupation { get; set; }
        public string StreetAddress1 { get; set; }
        public string StreetAddress2 { get; set; }
        public string StreetAddress3 { get; set; }
        public string StreetCode { get; set; }
        public string PostalAddress1 { get; set; }
        public string PostalAddress2 { get; set; }
        public string PostalAddress3 { get; set; }
        public string PostalCode { get; set; }
        public string TelHome { get; set; }
        public string TelWork { get; set; }
        public string TelCell { get; set; }
        public string TelFax { get; set; }
        public string EMail { get; set; }
        public string Gender { get; set; }
        public string Homelanguage { get; set; }
        public string SpouseFullName { get; set; }
        public string SpouseSurname { get; set; }
        public string SpouseIDNumber { get; set; }
        public string SpouseOccupation { get; set; }
        public string SpouseWorkTel { get; set; }
        public string SpouseGender { get; set; }
        public string SpouseCell { get; set; }
        public string SpouseEmail { get; set; }
        public string MaritalStatus { get; set; }
        public string Relationship { get; set; }
        public string Status { get; set; }
    }
    public class Parent_Child
    {
        public Int32 InstID { get; set; }
        public Int32 ParentID { get; set; }
        public Int32 ChildID { get; set; }
        public string LearnerID { get; set; }
        public string Status { get; set; }
        public string FamilyCode { get; set; }
    }
    public class SysParams
    {
        public Int32 InstID { get; set; }
        public string SASAMS_FilePath { get; set; }
        public string SASAMS_Password { get; set; }
        public string AccessDatabaseProvider { get; set; }
        public bool IsSASAMSFinance { get; set; }
    }

    public class Transactions
    {
        public Int32 InstID { get; set; }
        public Int32 SubAccNumber { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
    }
}
