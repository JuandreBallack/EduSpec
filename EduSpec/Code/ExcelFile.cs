using System.Collections;
using System.Collections.Generic;
using Microsoft.Office.Interop.Excel;
using _excel = Microsoft.Office.Interop.Excel;
using EduSpec.Models;
using System.Data;

namespace EduSpec.Code
{
    public class ExcelHeaders
    {
        public static string[] ProsperitasHeader =
        {
            "Account Number",
            "Debtor 1 Title",
            "Debtor 1 Initials",
            "Debtor 1 FullName",
            "Debtor 1 Surname",
            "Debtor 1 IDNumber",
            "Debtor 1 Passport Number",
            "Debtor 1 TelHome",
            "Debtor 1 TelWork",
            "Debtor 1 TelCell",
            "Debtor 1 PostalAddress1",
            "Debtor 1 PostalAddress2",
            "Debtor 1 PostalAddress3",
            "Debtor 1 PostalCode",
            "Debtor 1 Email",
            "Debtor 2 Title",
            "Debtor 2 Initials",
            "Debtor 2 FullName",
            "Debtor 2 Surname",
            "Debtor 2 IDNumber",
            "Debtor 2 Passport Number",
            "Debtor 2 TelHome",
            "Debtor 2 TelWork",
            "Debtor 2 TelCell",
            "Debtor 2 PostalAddress1",
            "Debtor 2 PostalAddress2",
            "Debtor 2 PostalAddress3",
            "Debtor 2 PostalCode",
            "Debtor 2 Email",
            "Learner",
            "Capital Amount",
            "Date"
        };

        public static string[] BentleyHeader =
        {
            "Client",
            "Matter Number",            
            "Capital",
            "Interest From Date",
            "Instruction Received Date",
            "Account Number",
            "Interest Rate",
            "Debtor Title",
            "Debtor Full Names",
            "Debtor Initials",
            "Debtor Surname",
            "Debtor Identity Number",
            "Debtor Home Address Line 1",
            "Debtor Home Address Line 2",
            "Debtor Home Address Line 3",
            "Debtor Home Address Line 4",
            "Debtor Home Address Line 5 (Postal Code)",
            "Name of Debtors Employer",
            "Debtors Employee Number",
            "Debtor Work Phone Number",
            "Debtor Work Address Line 1",
            "Debtor Work Address Line 2",
            "Debtor Work Address Line 3",
            "Debtor Work Address Line 4",
            "Debtor Work Address Line 5 (Postal Code)",
            "Debtor Postal Address Line 1",
            "Debtor Postal Address Line 2",
            "Debtor Postal Address Line 3",
            "Debtor Postal Address Line 4",
            "Debtor Postal Address Line 5 (Postal Code)",
            "Debtor Landline Number",
            "Debtor Alternate Number",
            "Debtor Fax Number",
            "Debtor Mobile Number",
            "Debtor e-Mail Address",
            "Additional Residential Addresses1",
            "Additional Residential Addresses2",
            "Additional Residential Addresses3",
            "Additional Residential Addresses4",
            "Additional Residential Addresses5",
            "Additional Debtor telephone number 1",
            "Additional Debtor telephone number 2",
            "Additional Debtor telephone number 3",
            "Additional Debtor telephone number 4",
            "Additional Debtor telephone number 5",
            "Parents Status (Married/ Divorced)",
            "Credit Listing (Yes/No)",
            "Can go to legal (Yes/No/Confirm)",
            "Contingency Commission Precent",
            "Number of Judgments",
            "Income Band",
            "LegalSuite Reference",
            "Childs Name",
            "School Fee year",
            "Handover Year & Debt Amount",
            "Debt Year & Total School Fee’s",
            "Additional Comments",
            "Combined fee for years of handover (all children)",
            "Co-Def Title",
            "Co-Def Name",
            "Co-Def Surname",
            "Co-Def ID Number",
            "Co-Def Phone",
            "Co-Def Mobile",
            "Co-Def Email",
            "Co-Def Employer Name",
            "Co-Def Work No",
            "Co-Def Occupation",
            "Co-Def Address Line 1",
            "Co-Def Street Address",
            "Co-Def Suburb",
            "Co-Def Postal Code",
            "2nd Defendant PostalLine1",
            "2nd Defendant PostalLine2",
            "2nd Defendant PostalLine3",
            "2nd Defendant PostalCode",
            "2nd Defendant Party Type",
            "Party Type",
            "Matter Description",
            "EmployeeID",
            "StageGroupID",
            "MatterTypeID",
            "DebtorFeeSheetID",
            "ClientFeeSheetID",
            "DebtorCollCommOption",
            "DebtorCollCommPercent",
            "CostCentreID",
            "DesktopExtraScreenID1",
            "DesktopExtraScreenID2",
            "DesktopExtraScreenID3",
            "Occupation (Debtor)",
            "2nd Defendant Occupation",
            "2nd Dep Employer Name",
            "Desktop Extra Field 6 - Persal No",
            "2nd Dep Work Tel Number",
            "2nd Dep Work Email",
            "Additional Comments"
        };

    }
    public class ExcelFile
    {
        // Create an excel application object, workbook oject and worksheet object
        _Application excel = new _excel.Application();
        Workbook workbook;
        Worksheet worksheet;

        // Method creates a new Excel file by creating a new Excel workbook with a single worksheet
        public void NewFile()
        {
            this.workbook = excel.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
            this.worksheet = this.workbook.Worksheets[1];
        }

        // Method adds a new worksheet to the existing workbook 
        public void NewSheet()
        {
            Worksheet newSheet = excel.Worksheets.Add(After: this.worksheet);
        }

        // Method saves workbook at a specified path
        public void SaveAs(string path)
        {
            workbook.SaveAs(path);
        }

        // Method closes Excel file
        public void Close()
        {
            workbook.Close();
        }

        public void AddRows(System.Data.DataTable str)
        {
            int rowIndex = 1;
            int colIndex;
            foreach (DataRow row in str.Rows)
            {
                rowIndex++;
                colIndex = 0;
                foreach (DataColumn col in str.Columns)
                {
                    colIndex++; excel.Cells[rowIndex, colIndex] = row[col.ColumnName].ToString();
                }                
            }
        }
        public void AddHeader(string[] header)
        {
            int colIndex = 1;
            foreach (var h in header)
            {
                worksheet.Cells[1, colIndex] = h;
                colIndex = colIndex + 1;
            }
        }
    }
}