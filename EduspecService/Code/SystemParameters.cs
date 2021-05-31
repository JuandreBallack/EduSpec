using System.Configuration;
using System.Linq;
using EduspecService.Models;


namespace EduSpecService
{
    public static class SystemParameters
    {
        public class SysParams
        {
            public string AgeAnalysisFilePath { get; set; }
            public string InstitutionDocumentsFilePath { get; set; }
            public string SingleEmailExemptionsFilePath { get; set; }
            public string SingleEmailDebtManagementFilePath { get; set; }
            public string EmailAttachementsFilePath { get; set; }
            public string RegisteredMailsCertificatesFilePath { get; set; }
            public string PrintoutsFilePath { get; set; }
            public string InstitutionInvoiceFilePath { get; set; }
        }

        public static SysParams SystemParams()
        {
            var Context = new EduSpecDataDataContext();
            var param = new fn_Get_SysParamsResult();
            param = Context.fn_Get_SysParams().FirstOrDefault();

            return new SysParams()
            {
                EmailAttachementsFilePath = param.EmailAttachements,
                AgeAnalysisFilePath = param.AgeAnalysisFilePath,
                InstitutionDocumentsFilePath = param.InstitutionDocumentsFilePath,
                SingleEmailExemptionsFilePath = param.ApplicationsFilePath,
                PrintoutsFilePath = param.PrintoutsFilePath,
                RegisteredMailsCertificatesFilePath = param.RegisteredMailsCertificatesFilePath,
                InstitutionInvoiceFilePath = param.InstitutionInvoiceFilePath
            };
        }
    }
}