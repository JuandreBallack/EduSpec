using System.Linq;
using EduSpec.Models;

namespace EduSpec
{
    public static class SystemParameters
    {
        public class SysParams
        {
            public decimal? VAT { get; set; }
            public string VATNumber { get; set; }
            public string InstitutionInvoiceFilePath { get; set; }
            public string ApplicationsFilePath { get; set; }
            public string PrintoutsFilePath { get; set; }
            public string AgeAnalysisFilePath { get; set; }
            public string InstitutionDataUploadFilePath { get; set; }
            public string QuotationFilePath { get; set; }
            public string EmailUploadFilePath { get; set; }
            public string SupportUploadFilePath { get; set; }
            public string EmailAttachementsFilePath { get; set; }
            public string AccessDatabaseProvider { get; set; }
            public string SASAMS_Password { get; set; }
            public string SQL_Server { get; set; }
            public string SQL_Database { get; set; }
            public string SQL_User { get; set; }
            public string SQL_Password { get; set; }
        }

        public static SysParams SystemParams()
        {
            var Context = new EduSpecDataContext();
            var param = new fn_Get_SysParamsResult();
            param = Context.fn_Get_SysParams().FirstOrDefault();

            return new SysParams()
            {
                VAT = param.VAT,
                VATNumber = param.VATNumber,
                InstitutionInvoiceFilePath = param.InstitutionInvoiceFilePath,
                ApplicationsFilePath = param.ApplicationsFilePath,
                PrintoutsFilePath = param.PrintoutsFilePath,
                AgeAnalysisFilePath = param.AgeAnalysisFilePath,
                InstitutionDataUploadFilePath = param.InstitutionDataUploadFilePath,
                QuotationFilePath = param.QuotationFilePath,
                EmailUploadFilePath = param.EmailUploadFilePath,
                SupportUploadFilePath = param.SupportUploadFilePath,
                EmailAttachementsFilePath = param.EmailAttachements,
                AccessDatabaseProvider = param.AccessDatabaseProvider,
                SASAMS_Password = param.SASAMS_Password,
                SQL_Server = param.SQL_Server,
                SQL_Database = param.SQL_Database,
                SQL_User = param.SQL_User,
                SQL_Password = param.SQL_Password
            };            
        }
    }
}