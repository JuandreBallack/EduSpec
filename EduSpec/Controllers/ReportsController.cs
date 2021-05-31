using DevExpress.XtraReports.UI;
using EduSpec.Models;
using EduSpec.Reports;
using NLog;
using System;
using System.Linq;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace EduSpec.Controllers
{
    [SessionExpire]
    public class ReportsController : Controller
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        public static XtraReport rptExemptionApplication(int InstID, int YearID, int ExemptionID, bool IsPrintreport, bool IsPrintLetterHead, int LanguageID, int MaritalStatusID, bool IsSingleApplication, int? Wareabouts, int? Contact, int? Unwilling, bool isAttachCovidForm, string FileName)//, out string Filename)
        {
            try
            {
                using (var Context = new EduSpecDataContext())
                {
                    MappedDiagnosticsLogicalContext.Set("Institution", Context.fn_GetInstitutionName(InstID));
                    var PrintAffidavit = false;
                    //string Filename = string.Format("{0}{1}.pdf", SystemParameters.SystemParams().ApplicationsFilePath, string.Format("{0}", Context.fn_Get_ApplicationNumber(ExemptionID)));

                    logger.Info(String.Format("EXEC RPT_ExemptionApplication {0},{1},{2}", InstID.ToString(), YearID.ToString(), ExemptionID.ToString()));
                    var Application = Context.RPT_ExemptionApplication(InstID, YearID, ExemptionID).FirstOrDefault();
                    var Letterhead = Context.RPT_Letterhead(InstID, IsPrintLetterHead).FirstOrDefault();
                    logger.Info("Institution = " + Letterhead.InstitutionName + "| InstitutionID = " + InstID.ToString());

                    XtraReport report = new Application_CoverPage();                    
                    report.Parameters["YearID"].Value = YearID;
                    report.Parameters["InstID"].Value = InstID;
                    report.Parameters["ExemptionID"].Value = ExemptionID;
                    report.Parameters["IsPrintLetterHead"].Value = IsPrintLetterHead;
                    logger.Info("Create cover page.");
                    report.CreateDocument();
                    logger.Info("Cover page created.");

                    logger.Info("Print only cover page = " + IsPrintreport.ToString());
                    if (IsPrintreport == true)
                    {
                        return report;
                    }
                    else
                    {
                        if (Wareabouts != 2 || Contact != 4 || Unwilling != 6)
                        {
                            PrintAffidavit = true;
                        }
                        else
                        {
                            PrintAffidavit = false;
                        }

                        switch (MaritalStatusID)
                        {
                            case 2: //Single (Contact with Biological Spouse)----------------------------------------------------------------------------------------
                                {
                                    if (IsSingleApplication == true)
                                    {
                                        if (LanguageID == 3)
                                        {
                                            XtraReport reportPages = new ApplicationForm_P2()
                                            {
                                                DataSource = new
                                                {
                                                    RPT_ApplicationForm_P2 = Context.RPT_ApplicationForm_P2(InstID),
                                                }
                                            };
                                            CreateReport(report, reportPages, 2, logger);
                                            reportPages = new ApplicationForm_P3_Single();
                                            CreateReport(report, reportPages, 3, logger);
                                            reportPages = new ApplicationForm_P4_Single();
                                            CreateReport(report, reportPages, 4, logger);
                                            reportPages = new Application_P5_Single();
                                            CreateReport(report, reportPages, 5, logger);
                                            reportPages = new Application_P6_Single();
                                            CreateReport(report, reportPages, 6, logger);
                                            reportPages = new ApplicationBankFormParent();
                                            CreateReport(report, reportPages, 7, logger);

                                            if (PrintAffidavit == true)
                                            {
                                                XtraReport reportAffidavit = new AffidavitNoContactWithSpouce_ENG()
                                                {
                                                    DataSource = new
                                                    {
                                                        RPT_Exemption_Affidavit = Context.RPT_Exemption_Affidavit(InstID, ExemptionID)
                                                    }
                                                };
                                                CreateReport(report, reportAffidavit, 8, logger);
                                            }

                                            if (isAttachCovidForm == true)
                                            {
                                                reportPages = new ApplicationCovidForm_ENG();
                                                CreateReport(report, reportPages, 9, logger);
                                            }

                                            report.PrintingSystem.ContinuousPageNumbering = true;
                                        }
                                        else
                                        {
                                            XtraReport reportPages = new ApplicationForm_P2_AFR()
                                            {
                                                DataSource = new
                                                {
                                                    RPT_ApplicationForm_P2 = Context.RPT_ApplicationForm_P2(InstID),
                                                }
                                            };
                                            CreateReport(report, reportPages, 2, logger);
                                            reportPages = new ApplicationForm_P3_Single_AFR();
                                            CreateReport(report, reportPages, 3, logger);
                                            reportPages = new ApplicationForm_P4_Single_AFR();
                                            CreateReport(report, reportPages, 4, logger);
                                            reportPages = new ApplicationForm_P5_Single_AFR();
                                            CreateReport(report, reportPages, 5, logger);
                                            reportPages = new ApplicationForm_P6_Single_AFR();
                                            CreateReport(report, reportPages, 6, logger);
                                            reportPages = new ApplicationBankFormParent_AFR();
                                            CreateReport(report, reportPages, 7, logger);

                                            if (PrintAffidavit == true)
                                            {
                                                XtraReport reportAffidavit = new AffidavitNoContactWithSpouce_AFR()
                                                {
                                                    DataSource = new
                                                    {
                                                        RPT_Exemption_Affidavit = Context.RPT_Exemption_Affidavit(InstID, ExemptionID)
                                                    }
                                                };
                                                CreateReport(report, reportAffidavit, 8, logger);
                                            }

                                            if (isAttachCovidForm == true)
                                            {
                                                reportPages = new ApplicationCovidForm_AFR();
                                                CreateReport(report, reportPages, 9, logger);
                                            }

                                            report.PrintingSystem.ContinuousPageNumbering = true;
                                        }
                                    }
                                    else
                                    {
                                        if (LanguageID == 3)
                                        {
                                            //English-----------------------------------------------------------------------------------------------------------------------------------------
                                            XtraReport reportPages = new Reports.ApplicationForm_P2()
                                            {
                                                DataSource = new
                                                {
                                                    RPT_ApplicationForm_P2 = Context.RPT_ApplicationForm_P2(InstID),
                                                }
                                            };
                                            CreateReport(report, reportPages, 2, logger);
                                            reportPages = new Reports.ApplicationForm_P3();
                                            CreateReport(report, reportPages, 3, logger);
                                            reportPages = new Reports.ApplicationForm_P4();
                                            CreateReport(report, reportPages, 4, logger);
                                            reportPages = new Reports.ApplicationForm_P5();
                                            CreateReport(report, reportPages, 5, logger);
                                            reportPages = new Reports.ApplicationForm_P6();
                                            CreateReport(report, reportPages, 6, logger);
                                            reportPages = new Reports.ApplicationForm_P7();
                                            CreateReport(report, reportPages, 7, logger);
                                            reportPages = new Reports.ApplicationBankFormParent();
                                            CreateReport(report, reportPages, 8, logger);
                                            reportPages = new Reports.ApplicationBankFormSpouse();
                                            CreateReport(report, reportPages, 9, logger);

                                            if (PrintAffidavit == true)
                                            {
                                                XtraReport reportAffidavit = new AffidavitNoContactWithSpouce_ENG()
                                                {
                                                    DataSource = new
                                                    {
                                                        RPT_Exemption_Affidavit = Context.RPT_Exemption_Affidavit(InstID, ExemptionID)
                                                    }
                                                };
                                                CreateReport(report, reportAffidavit, 10, logger);
                                            }

                                            if (isAttachCovidForm == true)
                                            {
                                                reportPages = new ApplicationCovidForm_ENG();
                                                CreateReport(report, reportPages, 11, logger);
                                            }

                                            report.PrintingSystem.ContinuousPageNumbering = true;
                                        }
                                        else
                                        {
                                            //Afrikaans-----------------------------------------------------------------------------------------------------------------------------------------
                                            XtraReport reportPages = new ApplicationForm_P2_AFR()
                                            {
                                                DataSource = new
                                                {
                                                    RPT_ApplicationForm_P2 = Context.RPT_ApplicationForm_P2(InstID),
                                                }
                                            };
                                            CreateReport(report, reportPages, 2, logger);
                                            reportPages = new ApplicationForm_P3_AFR();
                                            CreateReport(report, reportPages, 3, logger);
                                            reportPages = new ApplicationForm_P4_AFR();
                                            CreateReport(report, reportPages, 4, logger);
                                            reportPages = new ApplicationForm_P5_AFR();
                                            CreateReport(report, reportPages, 5, logger);
                                            reportPages = new ApplicationForm_P6_AFR();
                                            CreateReport(report, reportPages, 6, logger);
                                            reportPages = new ApplicationForm_P7_AFR();
                                            CreateReport(report, reportPages, 7, logger);
                                            reportPages = new ApplicationBankFormParent_AFR();
                                            CreateReport(report, reportPages, 8, logger);
                                            reportPages = new AppicationBankFormSpouce_AFR();
                                            CreateReport(report, reportPages, 9, logger);

                                            if (PrintAffidavit == true)
                                            {
                                                XtraReport reportAffidavit = new AffidavitNoContactWithSpouce_AFR()
                                                {
                                                    DataSource = new
                                                    {
                                                        RPT_Exemption_Affidavit = Context.RPT_Exemption_Affidavit(InstID, ExemptionID)
                                                    }
                                                };
                                                CreateReport(report, reportAffidavit, 10, logger);
                                            }

                                            if (isAttachCovidForm == true)
                                            {
                                                reportPages = new ApplicationCovidForm_AFR();
                                                CreateReport(report, reportPages, 11, logger);
                                            }

                                            report.PrintingSystem.ContinuousPageNumbering = true;
                                        }
                                    }
                                }
                                break;
                            case 4: //Divorced (Contact with Biological Spouse)
                                {
                                    if (IsSingleApplication == true)
                                    {
                                        if (LanguageID == 3)
                                        {
                                            XtraReport reportPages = new ApplicationForm_P2()
                                            {
                                                DataSource = new
                                                {
                                                    RPT_ApplicationForm_P2 = Context.RPT_ApplicationForm_P2(InstID),
                                                }
                                            };
                                            CreateReport(report, reportPages, 2, logger);
                                            reportPages = new ApplicationForm_P3_Single();
                                            CreateReport(report, reportPages, 3, logger);
                                            reportPages = new ApplicationForm_P4_Single();
                                            CreateReport(report, reportPages, 4, logger);
                                            reportPages = new Application_P5_Single();
                                            CreateReport(report, reportPages, 5, logger);
                                            reportPages = new Application_P6_Single();
                                            CreateReport(report, reportPages, 6, logger);
                                            reportPages = new ApplicationBankFormParent();
                                            CreateReport(report, reportPages, 7, logger);

                                            if (PrintAffidavit == true)
                                            {
                                                XtraReport reportAffidavit = new AffidavitNoContactWithSpouce_ENG()
                                                {
                                                    DataSource = new
                                                    {
                                                        RPT_Exemption_Affidavit = Context.RPT_Exemption_Affidavit(InstID, ExemptionID)
                                                    }
                                                };
                                                CreateReport(report, reportAffidavit, 8, logger);
                                            }

                                            if (isAttachCovidForm == true)
                                            {
                                                reportPages = new ApplicationCovidForm_ENG();
                                                CreateReport(report, reportPages, 9, logger);
                                            }

                                            report.PrintingSystem.ContinuousPageNumbering = true;
                                        }
                                        else
                                        {
                                            XtraReport reportPages = new ApplicationForm_P2_AFR()
                                            {
                                                DataSource = new
                                                {
                                                    RPT_ApplicationForm_P2 = Context.RPT_ApplicationForm_P2(InstID),
                                                }
                                            };
                                            CreateReport(report, reportPages, 2, logger);
                                            reportPages = new ApplicationForm_P3_Single_AFR();
                                            CreateReport(report, reportPages, 3, logger);
                                            reportPages = new ApplicationForm_P4_Single_AFR();
                                            CreateReport(report, reportPages, 4, logger);
                                            reportPages = new ApplicationForm_P5_Single_AFR();
                                            CreateReport(report, reportPages, 5, logger);
                                            reportPages = new ApplicationForm_P6_Single_AFR();
                                            CreateReport(report, reportPages, 6, logger);
                                            reportPages = new ApplicationBankFormParent_AFR();
                                            CreateReport(report, reportPages, 7, logger);

                                            if (PrintAffidavit == true)
                                            {
                                                XtraReport reportAffidavit = new AffidavitNoContactWithSpouce_AFR()
                                                {
                                                    DataSource = new
                                                    {
                                                        RPT_Exemption_Affidavit = Context.RPT_Exemption_Affidavit(InstID, ExemptionID)
                                                    }
                                                };
                                                CreateReport(report, reportAffidavit, 8, logger);
                                            }

                                            if (isAttachCovidForm == true)
                                            {
                                                reportPages = new ApplicationCovidForm_AFR();
                                                CreateReport(report, reportPages, 9, logger);
                                            }

                                            report.PrintingSystem.ContinuousPageNumbering = true;
                                        }
                                    }
                                    else
                                    {
                                        if (LanguageID == 3)
                                        {
                                            //English-----------------------------------------------------------------------------------------------------------------------------------------
                                            XtraReport reportPages = new ApplicationForm_P2()
                                            {
                                                DataSource = new
                                                {
                                                    RPT_ApplicationForm_P2 = Context.RPT_ApplicationForm_P2(InstID),
                                                }
                                            };
                                            CreateReport(report, reportPages, 2, logger);
                                            reportPages = new Reports.ApplicationForm_P3();
                                            CreateReport(report, reportPages, 3, logger);
                                            reportPages = new Reports.ApplicationForm_P4();
                                            CreateReport(report, reportPages, 4, logger);
                                            reportPages = new Reports.ApplicationForm_P5();
                                            CreateReport(report, reportPages, 5, logger);
                                            reportPages = new Reports.ApplicationForm_P6();
                                            CreateReport(report, reportPages, 6, logger);
                                            reportPages = new Reports.ApplicationForm_P7();
                                            CreateReport(report, reportPages, 7, logger);
                                            reportPages = new Reports.ApplicationBankFormParent();
                                            CreateReport(report, reportPages, 8, logger);
                                            reportPages = new Reports.ApplicationBankFormSpouse();
                                            CreateReport(report, reportPages, 9, logger);

                                            if (PrintAffidavit == true)
                                            {
                                                XtraReport reportAffidavit = new AffidavitNoContactWithSpouce_ENG()
                                                {
                                                    DataSource = new
                                                    {
                                                        RPT_Exemption_Affidavit = Context.RPT_Exemption_Affidavit(InstID, ExemptionID)
                                                    }
                                                };
                                                CreateReport(report, reportAffidavit, 10, logger);
                                            }

                                            if (isAttachCovidForm == true)
                                            {
                                                reportPages = new ApplicationCovidForm_ENG();
                                                CreateReport(report, reportPages, 11, logger);
                                            }

                                            report.PrintingSystem.ContinuousPageNumbering = true;
                                        }
                                        else
                                        {
                                            //Afrikaans-----------------------------------------------------------------------------------------------------------------------------------------
                                            XtraReport reportPages = new ApplicationForm_P2_AFR()
                                            {
                                                DataSource = new
                                                {
                                                    RPT_ApplicationForm_P2 = Context.RPT_ApplicationForm_P2(InstID),
                                                }
                                            };
                                            CreateReport(report, reportPages, 2, logger);
                                            reportPages = new ApplicationForm_P3_AFR();
                                            CreateReport(report, reportPages, 3, logger);
                                            reportPages = new ApplicationForm_P4_AFR();
                                            CreateReport(report, reportPages, 4, logger);
                                            reportPages = new ApplicationForm_P5_AFR();
                                            CreateReport(report, reportPages, 5, logger);
                                            reportPages = new ApplicationForm_P6_AFR();
                                            CreateReport(report, reportPages, 6, logger);
                                            reportPages = new ApplicationForm_P7_AFR();
                                            CreateReport(report, reportPages, 7, logger);
                                            reportPages = new ApplicationBankFormParent_AFR();
                                            CreateReport(report, reportPages, 8, logger);
                                            reportPages = new AppicationBankFormSpouce_AFR();
                                            CreateReport(report, reportPages, 9, logger);

                                            if (PrintAffidavit == true)
                                            {
                                                XtraReport reportAffidavit = new AffidavitNoContactWithSpouce_AFR()
                                                {
                                                    DataSource = new
                                                    {
                                                        RPT_Exemption_Affidavit = Context.RPT_Exemption_Affidavit(InstID, ExemptionID)
                                                    }
                                                };
                                                CreateReport(report, reportAffidavit, 10, logger);
                                            }

                                            if (isAttachCovidForm == true)
                                            {
                                                reportPages = new ApplicationCovidForm_AFR();
                                                CreateReport(report, reportPages, 11, logger);
                                            }

                                            report.PrintingSystem.ContinuousPageNumbering = true;
                                        }
                                    }
                                }
                                break;
                            case 5: //Widow
                                {
                                    if (LanguageID == 3)
                                    {
                                        XtraReport reportPages = new ApplicationForm_P2()
                                        {
                                            DataSource = new
                                            {
                                                RPT_ApplicationForm_P2 = Context.RPT_ApplicationForm_P2(InstID),
                                            }
                                        };
                                        CreateReport(report, reportPages, 2, logger);
                                        reportPages = new ApplicationForm_P3_Single();
                                        CreateReport(report, reportPages, 3, logger);
                                        reportPages = new ApplicationForm_P4_Single();
                                        CreateReport(report, reportPages, 4, logger);
                                        reportPages = new Application_P5_Single();
                                        CreateReport(report, reportPages, 5, logger);
                                        reportPages = new Application_P6_Single();
                                        CreateReport(report, reportPages, 6, logger);
                                        reportPages = new ApplicationBankFormParent();
                                        CreateReport(report, reportPages, 7, logger);

                                        if (isAttachCovidForm == true)
                                        {
                                            reportPages = new ApplicationCovidForm_ENG();
                                            CreateReport(report, reportPages, 8, logger);
                                        }

                                        report.PrintingSystem.ContinuousPageNumbering = true;
                                    }
                                    else
                                    {
                                        XtraReport reportPages = new ApplicationForm_P2_AFR()
                                        {
                                            DataSource = new
                                            {
                                                RPT_ApplicationForm_P2 = Context.RPT_ApplicationForm_P2(InstID),
                                            }
                                        };
                                        CreateReport(report, reportPages, 2, logger);
                                        reportPages = new ApplicationForm_P3_Single_AFR();
                                        CreateReport(report, reportPages, 3, logger);
                                        reportPages = new ApplicationForm_P4_Single_AFR();
                                        CreateReport(report, reportPages, 4, logger);
                                        reportPages = new ApplicationForm_P5_Single_AFR();
                                        CreateReport(report, reportPages, 5, logger);
                                        reportPages = new ApplicationForm_P6_Single_AFR();
                                        CreateReport(report, reportPages, 6, logger);
                                        reportPages = new ApplicationBankFormParent_AFR();
                                        CreateReport(report, reportPages, 7, logger);

                                        if (isAttachCovidForm == true)
                                        {
                                            reportPages = new ApplicationCovidForm_AFR();
                                            CreateReport(report, reportPages, 8, logger);
                                        }

                                        report.PrintingSystem.ContinuousPageNumbering = true;
                                    }
                                }
                                break;
                            case 6: //Single (No Contact with Biological Spouse)
                                {
                                    if (LanguageID == 3)
                                    {
                                        XtraReport reportPages = new ApplicationForm_P2()
                                        {
                                            DataSource = new
                                            {
                                                RPT_ApplicationForm_P2 = Context.RPT_ApplicationForm_P2(InstID),
                                            }
                                        };
                                        CreateReport(report, reportPages, 2, logger);
                                        reportPages = new ApplicationForm_P3_Single();
                                        CreateReport(report, reportPages, 3, logger);
                                        reportPages = new ApplicationForm_P4_Single();
                                        CreateReport(report, reportPages, 4, logger);
                                        reportPages = new Application_P5_Single();
                                        CreateReport(report, reportPages, 5, logger);
                                        reportPages = new Application_P6_Single();
                                        CreateReport(report, reportPages, 6, logger);
                                        reportPages = new ApplicationBankFormParent();
                                        CreateReport(report, reportPages, 7, logger);

                                        if (PrintAffidavit == true)
                                        {
                                            XtraReport reportAffidavit = new AffidavitNoContactWithSpouce_ENG()
                                            {
                                                DataSource = new
                                                {
                                                    RPT_Exemption_Affidavit = Context.RPT_Exemption_Affidavit(InstID, ExemptionID)
                                                }
                                            };
                                            CreateReport(report, reportAffidavit, 8, logger);
                                        }

                                        if (isAttachCovidForm == true)
                                        {
                                            reportPages = new ApplicationCovidForm_ENG();
                                            CreateReport(report, reportPages, 9, logger);
                                        }

                                        report.PrintingSystem.ContinuousPageNumbering = true;
                                    }
                                    else
                                    {
                                        XtraReport reportPages = new ApplicationForm_P2_AFR()
                                        {
                                            DataSource = new
                                            {
                                                RPT_ApplicationForm_P2 = Context.RPT_ApplicationForm_P2(InstID),
                                            }
                                        };
                                        CreateReport(report, reportPages, 2, logger);
                                        reportPages = new ApplicationForm_P3_Single_AFR();
                                        CreateReport(report, reportPages, 3, logger);
                                        reportPages = new ApplicationForm_P4_Single_AFR();
                                        CreateReport(report, reportPages, 4, logger);
                                        reportPages = new ApplicationForm_P5_Single_AFR();
                                        CreateReport(report, reportPages, 5, logger);
                                        reportPages = new ApplicationForm_P6_Single_AFR();
                                        CreateReport(report, reportPages, 6, logger);
                                        reportPages = new ApplicationBankFormParent_AFR();
                                        CreateReport(report, reportPages, 7, logger);

                                        if (PrintAffidavit == true)
                                        {
                                            XtraReport reportAffidavit = new AffidavitNoContactWithSpouce_AFR()
                                            {
                                                DataSource = new
                                                {
                                                    RPT_Exemption_Affidavit = Context.RPT_Exemption_Affidavit(InstID, ExemptionID)
                                                }
                                            };
                                            CreateReport(report, reportAffidavit, 8, logger);
                                        }

                                        if (isAttachCovidForm == true)
                                        {
                                            reportPages = new ApplicationCovidForm_AFR();
                                            CreateReport(report, reportPages, 9, logger);
                                        }

                                        report.PrintingSystem.ContinuousPageNumbering = true;
                                    }
                                }
                                break;
                            case 7: //Divorced (No Contact with Biological Spouse)
                                {
                                    if (LanguageID == 3)
                                    {
                                        XtraReport reportPages = new ApplicationForm_P2()
                                        {
                                            DataSource = new
                                            {
                                                RPT_ApplicationForm_P2 = Context.RPT_ApplicationForm_P2(InstID),
                                            }
                                        };
                                        CreateReport(report, reportPages, 2, logger);
                                        reportPages = new ApplicationForm_P3_Single();
                                        CreateReport(report, reportPages, 3, logger);
                                        reportPages = new ApplicationForm_P4_Single();
                                        CreateReport(report, reportPages, 4, logger);
                                        reportPages = new Application_P5_Single();
                                        CreateReport(report, reportPages, 5, logger);
                                        reportPages = new Application_P6_Single();
                                        CreateReport(report, reportPages, 6, logger);
                                        reportPages = new ApplicationBankFormParent();
                                        CreateReport(report, reportPages, 7, logger);

                                        if (PrintAffidavit == true)
                                        {
                                            XtraReport reportAffidavit = new AffidavitNoContactWithSpouce_ENG()
                                            {
                                                DataSource = new
                                                {
                                                    RPT_Exemption_Affidavit = Context.RPT_Exemption_Affidavit(InstID, ExemptionID)
                                                }
                                            };
                                            CreateReport(report, reportAffidavit, 8, logger);
                                        }

                                        if (isAttachCovidForm == true)
                                        {
                                            reportPages = new ApplicationCovidForm_ENG();
                                            CreateReport(report, reportPages, 9, logger);
                                        }

                                        report.PrintingSystem.ContinuousPageNumbering = true;
                                    }
                                    else
                                    {
                                        XtraReport reportPages = new ApplicationForm_P2_AFR()
                                        {
                                            DataSource = new
                                            {
                                                RPT_ApplicationForm_P2 = Context.RPT_ApplicationForm_P2(InstID),
                                            }
                                        };
                                        CreateReport(report, reportPages, 2, logger);
                                        reportPages = new ApplicationForm_P3_Single_AFR();
                                        CreateReport(report, reportPages, 3, logger);
                                        reportPages = new ApplicationForm_P4_Single_AFR();
                                        CreateReport(report, reportPages, 4, logger);
                                        reportPages = new ApplicationForm_P5_Single_AFR();
                                        CreateReport(report, reportPages, 5, logger);
                                        reportPages = new ApplicationForm_P6_Single_AFR();
                                        CreateReport(report, reportPages, 6, logger);
                                        reportPages = new ApplicationBankFormParent_AFR();
                                        CreateReport(report, reportPages, 7, logger);

                                        if (PrintAffidavit == true)
                                        {
                                            XtraReport reportAffidavit = new AffidavitNoContactWithSpouce_AFR()
                                            {
                                                DataSource = new
                                                {
                                                    RPT_Exemption_Affidavit = Context.RPT_Exemption_Affidavit(InstID, ExemptionID)
                                                }
                                            };
                                            CreateReport(report, reportAffidavit, 8, logger);
                                        }

                                        if (isAttachCovidForm == true)
                                        {
                                            reportPages = new ApplicationCovidForm_AFR();
                                            CreateReport(report, reportPages, 9, logger);
                                        }

                                        report.PrintingSystem.ContinuousPageNumbering = true;
                                    }
                                }
                                break;
                            case 3: //Married                            
                                {
                                    if (LanguageID == 3)
                                    {
                                        //English-----------------------------------------------------------------------------------------------------------------------------------------
                                        XtraReport reportPages = new Reports.ApplicationForm_P2()
                                        {
                                            DataSource = new
                                            {
                                                RPT_ApplicationForm_P2 = Context.RPT_ApplicationForm_P2(InstID),
                                            }
                                        };
                                        CreateReport(report, reportPages, 2, logger);
                                        reportPages = new Reports.ApplicationForm_P3();
                                        CreateReport(report, reportPages, 3, logger);
                                        reportPages = new Reports.ApplicationForm_P4();
                                        CreateReport(report, reportPages, 4, logger);
                                        reportPages = new Reports.ApplicationForm_P5();
                                        CreateReport(report, reportPages, 5, logger);
                                        reportPages = new Reports.ApplicationForm_P6();
                                        CreateReport(report, reportPages, 6, logger);
                                        reportPages = new Reports.ApplicationForm_P7();
                                        CreateReport(report, reportPages, 7, logger);
                                        reportPages = new Reports.ApplicationBankFormParent();
                                        CreateReport(report, reportPages, 8, logger);
                                        reportPages = new Reports.ApplicationBankFormSpouse();
                                        CreateReport(report, reportPages, 9, logger);

                                        if (isAttachCovidForm == true)
                                        {
                                            reportPages = new ApplicationCovidForm_ENG();
                                            CreateReport(report, reportPages, 10, logger);
                                        }

                                        report.PrintingSystem.ContinuousPageNumbering = true;
                                    }
                                    else
                                    {
                                        //Afrikaans-----------------------------------------------------------------------------------------------------------------------------------------
                                        XtraReport reportPages = new ApplicationForm_P2_AFR()
                                        {
                                            DataSource = new
                                            {
                                                RPT_ApplicationForm_P2 = Context.RPT_ApplicationForm_P2(InstID),
                                            }
                                        };
                                        CreateReport(report, reportPages, 2, logger);
                                        reportPages = new ApplicationForm_P3_AFR();
                                        CreateReport(report, reportPages, 3, logger);
                                        reportPages = new ApplicationForm_P4_AFR();
                                        CreateReport(report, reportPages, 4, logger);
                                        reportPages = new ApplicationForm_P5_AFR();
                                        CreateReport(report, reportPages, 5, logger);
                                        reportPages = new ApplicationForm_P6_AFR();
                                        CreateReport(report, reportPages, 6, logger);
                                        reportPages = new ApplicationForm_P7_AFR();
                                        CreateReport(report, reportPages, 7, logger);
                                        reportPages = new ApplicationBankFormParent_AFR();
                                        CreateReport(report, reportPages, 8, logger);
                                        reportPages = new AppicationBankFormSpouce_AFR();
                                        CreateReport(report, reportPages, 9, logger);

                                        if (isAttachCovidForm == true)
                                        {
                                            reportPages = new ApplicationCovidForm_AFR();
                                            CreateReport(report, reportPages, 10, logger);
                                        }

                                        report.PrintingSystem.ContinuousPageNumbering = true;
                                    }
                                }
                                break;
                            case 8: //Foreigners----------------------------------------------------------------------------------------
                                {
                                    if (IsSingleApplication == true)
                                    {
                                        if (LanguageID == 3)
                                        {
                                            XtraReport reportPages = new ApplicationForm_P2()
                                            {
                                                DataSource = new
                                                {
                                                    RPT_ApplicationForm_P2 = Context.RPT_ApplicationForm_P2(InstID),
                                                }
                                            };
                                            CreateReport(report, reportPages, 2, logger);
                                            reportPages = new ApplicationForm_P3_Single();
                                            CreateReport(report, reportPages, 3, logger);
                                            reportPages = new ApplicationForm_P4_Single();
                                            CreateReport(report, reportPages, 4, logger);
                                            reportPages = new Application_P5_Single();
                                            CreateReport(report, reportPages, 5, logger);
                                            reportPages = new Application_P6_Single();
                                            CreateReport(report, reportPages, 6, logger);
                                            reportPages = new ApplicationBankFormParent();
                                            CreateReport(report, reportPages, 7, logger);

                                            if (PrintAffidavit == true)
                                            {
                                                XtraReport reportAffidavit = new AffidavitNoContactWithSpouce_ENG()
                                                {
                                                    DataSource = new
                                                    {
                                                        RPT_Exemption_Affidavit = Context.RPT_Exemption_Affidavit(InstID, ExemptionID)
                                                    }
                                                };
                                                CreateReport(report, reportAffidavit, 8, logger);
                                            }

                                            if (isAttachCovidForm == true)
                                            {
                                                reportPages = new ApplicationCovidForm_ENG();
                                                CreateReport(report, reportPages, 9, logger);
                                            }

                                            report.PrintingSystem.ContinuousPageNumbering = true;
                                        }
                                        else
                                        {
                                            XtraReport reportPages = new ApplicationForm_P2_AFR()
                                            {
                                                DataSource = new
                                                {
                                                    RPT_ApplicationForm_P2 = Context.RPT_ApplicationForm_P2(InstID),
                                                }
                                            };
                                            CreateReport(report, reportPages, 2, logger);
                                            reportPages = new ApplicationForm_P3_Single_AFR();
                                            CreateReport(report, reportPages, 3, logger);
                                            reportPages = new ApplicationForm_P4_Single_AFR();
                                            CreateReport(report, reportPages, 4, logger);
                                            reportPages = new ApplicationForm_P5_Single_AFR();
                                            CreateReport(report, reportPages, 5, logger);
                                            reportPages = new ApplicationForm_P6_Single_AFR();
                                            CreateReport(report, reportPages, 6, logger);
                                            reportPages = new ApplicationBankFormParent_AFR();
                                            CreateReport(report, reportPages, 7, logger);

                                            if (PrintAffidavit == true)
                                            {
                                                XtraReport reportAffidavit = new AffidavitNoContactWithSpouce_AFR()
                                                {
                                                    DataSource = new
                                                    {
                                                        RPT_Exemption_Affidavit = Context.RPT_Exemption_Affidavit(InstID, ExemptionID)
                                                    }
                                                };
                                                CreateReport(report, reportAffidavit, 8, logger);
                                            }

                                            if (isAttachCovidForm == true)
                                            {
                                                reportPages = new ApplicationCovidForm_AFR();
                                                CreateReport(report, reportPages, 9, logger);
                                            }

                                            report.PrintingSystem.ContinuousPageNumbering = true;
                                        }
                                    }
                                    else
                                    {
                                        if (LanguageID == 3)
                                        {
                                            //English-----------------------------------------------------------------------------------------------------------------------------------------
                                            XtraReport reportPages = new Reports.ApplicationForm_P2()
                                            {
                                                DataSource = new
                                                {
                                                    RPT_ApplicationForm_P2 = Context.RPT_ApplicationForm_P2(InstID),
                                                }
                                            };
                                            CreateReport(report, reportPages, 2, logger);
                                            reportPages = new Reports.ApplicationForm_P3();
                                            CreateReport(report, reportPages, 3, logger);
                                            reportPages = new Reports.ApplicationForm_P4();
                                            CreateReport(report, reportPages, 4, logger);
                                            reportPages = new Reports.ApplicationForm_P5();
                                            CreateReport(report, reportPages, 5, logger);
                                            reportPages = new Reports.ApplicationForm_P6();
                                            CreateReport(report, reportPages, 6, logger);
                                            reportPages = new Reports.ApplicationForm_P7();
                                            CreateReport(report, reportPages, 7, logger);
                                            reportPages = new Reports.ApplicationBankFormParent();
                                            CreateReport(report, reportPages, 8, logger);
                                            reportPages = new Reports.ApplicationBankFormSpouse();
                                            CreateReport(report, reportPages, 9, logger);

                                            if (PrintAffidavit == true)
                                            {
                                                XtraReport reportAffidavit = new AffidavitNoContactWithSpouce_ENG()
                                                {
                                                    DataSource = new
                                                    {
                                                        RPT_Exemption_Affidavit = Context.RPT_Exemption_Affidavit(InstID, ExemptionID)
                                                    }
                                                };
                                                CreateReport(report, reportAffidavit, 10, logger);
                                            }

                                            if (isAttachCovidForm == true)
                                            {
                                                reportPages = new ApplicationCovidForm_ENG();
                                                CreateReport(report, reportPages, 11, logger);
                                            }

                                            report.PrintingSystem.ContinuousPageNumbering = true;
                                        }
                                        else
                                        {
                                            //Afrikaans-----------------------------------------------------------------------------------------------------------------------------------------
                                            XtraReport reportPages = new ApplicationForm_P2_AFR()
                                            {
                                                DataSource = new
                                                {
                                                    RPT_ApplicationForm_P2 = Context.RPT_ApplicationForm_P2(InstID),
                                                }
                                            };
                                            CreateReport(report, reportPages, 2, logger);
                                            reportPages = new ApplicationForm_P3_AFR();
                                            CreateReport(report, reportPages, 3, logger);
                                            reportPages = new ApplicationForm_P4_AFR();
                                            CreateReport(report, reportPages, 4, logger);
                                            reportPages = new ApplicationForm_P5_AFR();
                                            CreateReport(report, reportPages, 5, logger);
                                            reportPages = new ApplicationForm_P6_AFR();
                                            CreateReport(report, reportPages, 6, logger);
                                            reportPages = new ApplicationForm_P7_AFR();
                                            CreateReport(report, reportPages, 7, logger);
                                            reportPages = new ApplicationBankFormParent_AFR();
                                            CreateReport(report, reportPages, 8, logger);
                                            reportPages = new AppicationBankFormSpouce_AFR();
                                            CreateReport(report, reportPages, 9, logger);

                                            if (PrintAffidavit == true)
                                            {
                                                XtraReport reportAffidavit = new AffidavitNoContactWithSpouce_AFR()
                                                {
                                                    DataSource = new
                                                    {
                                                        RPT_Exemption_Affidavit = Context.RPT_Exemption_Affidavit(InstID, ExemptionID)
                                                    }
                                                };
                                                CreateReport(report, reportAffidavit, 10, logger);
                                            }

                                            if (isAttachCovidForm == true)
                                            {
                                                reportPages = new ApplicationCovidForm_AFR();
                                                CreateReport(report, reportPages, 11, logger);
                                            }

                                            report.PrintingSystem.ContinuousPageNumbering = true;
                                        }
                                    }
                                }
                                break;
                            case 9: //Foster Care----------------------------------------------------------------------------------------
                                {
                                    if (LanguageID == 3)
                                    {
                                        XtraReport reportPages = new ApplicationForm_P2()
                                        {
                                            DataSource = new
                                            {
                                                RPT_ApplicationForm_P2 = Context.RPT_ApplicationForm_P2(InstID),
                                            }
                                        };
                                        CreateReport(report, reportPages, 2, logger);
                                        reportPages = new ApplicationForm_P3_FosterCare();
                                        CreateReport(report, reportPages, 3, logger);

                                        if (PrintAffidavit == true)
                                        {
                                            XtraReport reportAffidavit = new AffidavitNoContactWithSpouce_ENG()
                                            {
                                                DataSource = new
                                                {
                                                    RPT_Exemption_Affidavit = Context.RPT_Exemption_Affidavit(InstID, ExemptionID)
                                                }
                                            };
                                            CreateReport(report, reportAffidavit, 8, logger);
                                        }

                                        report.PrintingSystem.ContinuousPageNumbering = true;
                                    }
                                    else
                                    {
                                        XtraReport reportPages = new ApplicationForm_P2_AFR()
                                        {
                                            DataSource = new
                                            {
                                                RPT_ApplicationForm_P2 = Context.RPT_ApplicationForm_P2(InstID),
                                            }
                                        };
                                        CreateReport(report, reportPages, 2, logger);
                                        reportPages = new ApplicationForm_P3_FosterCare_AFR();
                                        CreateReport(report, reportPages, 3, logger);

                                        if (PrintAffidavit == true)
                                        {
                                            XtraReport reportAffidavit = new AffidavitNoContactWithSpouce_AFR()
                                            {
                                                DataSource = new
                                                {
                                                    RPT_Exemption_Affidavit = Context.RPT_Exemption_Affidavit(InstID, ExemptionID)
                                                }
                                            };
                                            CreateReport(report, reportAffidavit, 8, logger);
                                        }

                                        report.PrintingSystem.ContinuousPageNumbering = true;
                                    }
                                }
                                break;
                        };
                        logger.Info("Set filename and location");

                        logger.Info("Filename and location = " + FileName);
                        try
                        {
                            logger.Info("Save filename to location");
                            //report.ExportToPdf(Filename);
                            //logger.Info("File saved.");
                        }
                        catch (Exception e)
                        {
                            logger.Error(e.Message);
                        }
                        return report;
                    }
                }
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                //Filename = "";
                return null;
            }
        }
        private static void CreateReport(XtraReport report, XtraReport reportPages, int PageNumber, Logger logger)
        {
            logger.Info("Create application page " + PageNumber + ".");
            reportPages.CreateDocument();
            report.Pages.AddRange(reportPages.Pages);
            logger.Info("Application page " + PageNumber + " created.");
        }
        public static XtraReport rptExemptions(int InstID, int YearID, string ReportFilter)
        {
            using (var Context = new EduSpecDataContext())
            {

                XtraReport report = new ExemptionSummary()
                {
                    DataSource = new { RPT_ExemptionSummary = Context.RPT_ExemptionSummary(InstID, YearID, ReportFilter) }
                };
                report.CreateDocument();

                return report;
            }
        }

        public static XtraReport rptExemptionsSummaryGrouped(int InstID, int YearID, string ReportFilter)
        {
            using (var Context = new EduSpecDataContext())
            {

                XtraReport report = new ExemptionsSummaryGrouped()
                {
                    DataSource = new { RPT_ExemptionSummaryGroupedAlphabetically = Context.RPT_ExemptionSummaryGroupedAlphabetically(InstID, YearID, ReportFilter) }
                };
                report.CreateDocument();

                return report;
            }
        }

        public static XtraReport rptFamilyDetail(int FamilyID, int InstID)
        {
            using (var Context = new EduSpecDataContext())
            {

                XtraReport report = new FamilyDetail()
                {
                    DataSource = new { RPT_FamilyDetails = Context.RPT_FamilyDetails(FamilyID, InstID) }
                };
                report.CreateDocument();

                return report;
            }
        }

        //public static XtraReport rptExemptionLetters(int InstID, int YearID, bool IsPrintLetterHead)
        //{
        //    using (var Context = new EduSpecDataContext())
        //    {

        //        XtraReport report = new ExemptionLetters()
        //        {
        //            DataSource = new 
        //            { 
        //                RPT_Exemptions_Letters = Context.RPT_Exemptions_Letters(InstID, YearID, -1, -1),
        //                RPT_Letterhead = Context.RPT_Letterhead(InstID, IsPrintLetterHead)
        //            }
        //        };
        //        report.CreateDocument();

        //        return report;
        //    }
        //}

        public static XtraReport rptExemptionCompensasion(int YearID, int InstID)
        {
            using (var Context = new EduSpecDataContext())
            {

                XtraReport report = new ExemptionCompensation()
                {
                    DataSource = new
                    {
                        RPT_Exemption_Compensation_Detail = Context.RPT_Exemption_Compensation_Detail(YearID, InstID),
                        RPT_Exemption_Compensation_Header = Context.RPT_Exemption_Compensation_Header(YearID, WebSecurity.CurrentUserId)
                    }
                };
                report.CreateDocument();

                return report;
            }
        }

        //public static XtraReport rptExemptionCompensasion_Freestate(int YearID, int InstID)
        //{
        //    using (var Context = new EduSpecDataContext())
        //    {

        //        XtraReport report = new ExemptionCompensation_Freesate()
        //        {
        //            DataSource = new
        //            {
        //                RPT_Exemption_Compensation_Freestate = Context.RPT_Exemption_Compensation_Freestate(YearID, InstID)
        //            }
        //        };
        //        report.CreateDocument();

        //        return report;
        //    }
        //}

        public static XtraReport rptLearnerClassList(int InstID)
        {
            using (var Context = new EduSpecDataContext())
            {

                XtraReport report = new LearnersClassList()
                {
                    DataSource = new
                    {
                        RPT_LearnersClassList = Context.RPT_LearnersClassList(InstID)
                    }
                };
                report.CreateDocument();

                return report;
            }
        }

        public static XtraReport rptExemptionProvisionalList(int InstID, int YearID)
        {
            using (var Context = new EduSpecDataContext())
            {

                XtraReport report = new ExemptionProvisionalList()
                {
                    DataSource = new
                    {
                        RPT_ExemptionProvisionalReportDetail = Context.RPT_ExemptionProvisionalReportDetail(InstID, YearID),
                        RPT_ExemptionProvisionalReport_Header = Context.RPT_ExemptionProvisionalReport_Header(InstID, YearID)
                    }
                };
                report.CreateDocument();

                return report;
            }
        }

        public static XtraReport rptFinalTableForExemptions(int YearID, int InstID)
        {
            using (var Context = new EduSpecDataContext())
            {

                XtraReport report = new FinalTableExemptions()
                {
                    DataSource = new
                    {
                        RPT_Exemption_FinalTableForExemptions = Context.RPT_Exemption_FinalTableForExemptions(YearID, InstID)
                    }
                };
                report.CreateDocument();

                return report;
            }
        }

        public static XtraReport rptApplicationRegister(int InstID, int Qty)
        {
            using (var Context = new EduSpecDataContext())
            {

                XtraReport report = new FormRegister()
                {
                    DataSource = new
                    {
                        RPT_ExemptionFormRegister = Context.RPT_ExemptionFormRegister(InstID, Qty)
                    }
                };
                report.CreateDocument();

                return report;
            }
        }

        public static XtraReport rptAgeLetters(int InstID, int YearID, bool IsPrintLetterHead, int AgeStatusID, int FamilyID, int LetterID, bool IsBulkLetter)
        {
            using (var Context = new EduSpecDataContext())
            {

                if (IsBulkLetter == true)
                {
                    string FileName = Context.fn_Get_BulkFileName(LetterID, InstID);
                    Context.Set_BulkLetterQueue_Add(InstID, WebSecurity.CurrentUserId, AgeStatusID, LetterID, IsPrintLetterHead, FileName);
                }
                else
                {
                    XtraReport report = new GenericLetter()
                    {
                        DataSource = new
                        {
                            RPT_GenericLetter = Context.RPT_Age_Letters(InstID, YearID, AgeStatusID, FamilyID, false, LetterID, null),
                            RPT_Letterhead = Context.RPT_Letterhead(InstID, IsPrintLetterHead)
                        }
                    };
                    report.CreateDocument();

                    var FileName = Context.Set_DebtManagement_ActionHistory(
                                    InstID,
                                    FamilyID,
                                    AgeStatusID,
                                    2, //LegalActionActions
                                    WebSecurity.CurrentUserId,
                                    null,
                                    null,
                                    false,
                                    null,
                                    null,
                                    null,
                                    LetterID,
                                    null,
                                    null,
                                    0
                                    ).First().FileName;
                    string resultFilePath = String.Format("{0}{1}", SystemParameters.SystemParams().AgeAnalysisFilePath, String.Format(FileName));
                    report.ExportToPdf(resultFilePath);
                    return report;
                }
                return null;
            }
        }

        public static XtraReport rptExemptionSingleLetter(int InstID, int ExemptionID, int FamilyID, bool IsPrintLetterHead, int ExemptionStatusID, int LetterID)
        {
            //ScriptPermissionManager.GlobalInstance = new ScriptPermissionManager(ExecutionMode.Unrestricted);
            using (var Context = new EduSpecDataContext())
            {
                
                XtraReport report = new GenericLetter()
                {
                    DataSource = new
                    {
                        RPT_GenericLetter = Context.RPT_Exemption_Letters(InstID, ExemptionID, FamilyID, LetterID),
                        RPT_Letterhead = Context.RPT_Letterhead(InstID, IsPrintLetterHead)
                    }
                };
                report.CreateDocument();

                var FileName = Context.Set_Exemptions_SingleLetterFile_Add(
                                    InstID,
														
                                    ExemptionID,
						  
                                    ExemptionStatusID,
                                    LetterID,
                                    WebSecurity.CurrentUserId
                                    ).First().FileName;
						
						
					   
					 
                string resultFilePath = String.Format("{0}{1}", SystemParameters.SystemParams().PrintoutsFilePath, String.Format(FileName));
                report.ExportToPdf(resultFilePath);
                return report;
                
                //return null;
            }
        }

        public static XtraReport RptExemptionCalculation(int ExemptionID)
        {
            using (var Context = new EduSpecDataContext())
            {
                XtraReport report = new ExemptionCalculation();
                report.Parameters["ExemptionID"].Value = ExemptionID;
                report.CreateDocument();

                XtraReport reportPages = new FamilyExemptionHistory();
                reportPages.Parameters["ExemptionID"].Value = ExemptionID;                
                reportPages.CreateDocument();
                report.Pages.AddRange(reportPages.Pages);

                return report;
            }
        }
        public static XtraReport rptGenericLetter(int InstID, int ExemptionID, int FamilyID, bool IsPrintLetterHead)
        {
            using (var Context = new EduSpecDataContext())
            {
                XtraReport report = new Reports.GenericLetter()
                {
                    DataSource = new
                    {
                        RPT_GenericLetter = Context.RPT_GenericLetter(InstID, ExemptionID, FamilyID),
                        RPT_Letterhead = Context.RPT_Letterhead(InstID, IsPrintLetterHead)
                    }
                };
                report.CreateDocument();

                XtraReport reportPages = new GenericLetterAcceptance()
                {
                    DataSource = new
                    {
                        RPT_GenericLetter = Context.RPT_GenericLetter(InstID, ExemptionID, FamilyID),
                        RPT_Letterhead = Context.RPT_Letterhead(InstID, IsPrintLetterHead),
                        RPT_GenericLetterFooter = Context.RPT_GenericLetterFooter(InstID, ExemptionID)
                    }
                };
                reportPages.CreateDocument();
                report.Pages.AddRange(reportPages.Pages);
                return report;
            }
        }
        public static XtraReport rptQuotation(int InstID, int Quotation)
        {
            using (var Context = new EduSpecDataContext())
            {

                XtraReport report = new Quotation
                {
                    DataSource = new
                    {
                        RPT_Quotation = Context.RPT_Quotation(Quotation),
                        RPT_QuotationHeader = Context.RPT_QuotationHeader(InstID)
                    }
                };
                report.CreateDocument();

                string resultFilePath = String.Format("{0}{1}", SystemParameters.SystemParams().QuotationFilePath, String.Format("Q{0}", Quotation + ".pdf"));

                report.ExportToPdf(resultFilePath);

                return report;
            }
        }
        public static XtraReport rptInvoice(int InstID, int InvoiceID)
        {
            using (var Context = new EduSpecDataContext())
            {

                XtraReport report = new Invoice()
                {
                    DataSource = new
                    {
                        RPT_Invoice = Context.RPT_Invoice(InvoiceID),
                        RPT_InvoiceHeader = Context.RPT_InvoiceHeader(InstID, InvoiceID)
                    }
                };
                report.CreateDocument();

                string resultFilePath = String.Format("{0}{1}", SystemParameters.SystemParams().InstitutionInvoiceFilePath, String.Format("INV{0}", InvoiceID + ".pdf"));

                report.ExportToPdf(resultFilePath);

                return report;

            }
        }
        public static XtraReport RptAffidavit(int InstID, int ExemptionID, int LanguageID)
        {
            using (var Context = new EduSpecDataContext())
            {

                if (LanguageID == 2) //Afrikaans
                {
                    XtraReport report = new AffidavitNoContactWithSpouce_AFR()
                    {
                        DataSource = new
                        {
                            RPT_Exemption_Affidavit = Context.RPT_Exemption_Affidavit(InstID, ExemptionID)
                        }
                    };
                    report.CreateDocument();

                    string resultFilePath = String.Format("{0}{1}", SystemParameters.SystemParams().PrintoutsFilePath, String.Format("AFFIDAVIT_{0}", ExemptionID + ".pdf"));

                    report.ExportToPdf(resultFilePath);

                    return report;
                }
                else //English
                {

                    XtraReport report = new AffidavitNoContactWithSpouce_ENG()
                    {
                        DataSource = new
                        {
                            RPT_Exemption_Affidavit = Context.RPT_Exemption_Affidavit(InstID, ExemptionID)
                        }
                    };
                    report.CreateDocument();

                    string resultFilePath = String.Format("{0}{1}", SystemParameters.SystemParams().PrintoutsFilePath, String.Format("AFFIDAVIT_{0}", ExemptionID + ".pdf"));

                    report.ExportToPdf(resultFilePath);

                    return report;
                }
                
            }
        }
        public static XtraReport RptAffidavitRefuses(int InstID, int ExemptionID)
        {
            using (var Context = new EduSpecDataContext())
            {
                XtraReport report = new AffidavitSpouseRefuseToParticipate()
                {
                    DataSource = new
                    {
                        RPT_Exemption_Affidavit = Context.RPT_Exemption_Affidavit(InstID, ExemptionID)
                    }
                };
                report.CreateDocument();

                string resultFilePath = String.Format("{0}{1}", SystemParameters.SystemParams().PrintoutsFilePath, String.Format("AFFIDAVIT_{0}", ExemptionID + ".pdf"));

                report.ExportToPdf(resultFilePath);

                return report;

            }
        }
        public static XtraReport RptPrintRequestToApply(int InstID, int FamilyID, int LetterID, bool IsPrintLetterHead)
        {
            using (var Context = new EduSpecDataContext())
            {

                XtraReport report = new Reports.GenericLetter()
                {
                    DataSource = new
                    {
                        RPT_GenericLetter = Context.RPT_PrintRequestToApplyLetter(InstID, LetterID, FamilyID),
                        RPT_Letterhead = Context.RPT_Letterhead(InstID, IsPrintLetterHead)
                    }
                };
                report.CreateDocument();
                return report;

            }
        }
        public static XtraReport rptParentAccountHistory(int FamilyID, int InstID, int YearID)
        {
            using (var Context = new EduSpecDataContext())
            {

                XtraReport report = new ParentAccountHistory()
                {
                    DataSource = new
                    {
                        RPT_ParentAccountHistory_ParentInfo = Context.RPT_ParentAccountHistory_ParentInfo(FamilyID, YearID),
                        RPT_ParentAccountHistory_AgeFamilyActionHistory = Context.RPT_ParentAccountHistory_AgeFamilyActionHistory(FamilyID),
                        RPT_InstitutionDetails = Context.RPT_InstitutionDetails(InstID)
                    }
                    
                };                
                report.CreateDocument();

                return report;
            }
        }

        //public static XtraReport RptPrintAgeSummaryReport(int InstID, int YearID, int AgeStatusID)
        //{
        //    using (var Context = new EduSpecDataContext())
        //    {

        //        XtraReport report = new DebtManagementSummary()
        //        {
        //            DataSource = new
        //            {
        //                RPT_DebtManagement_SummaryAccordingToStatus = Context.RPT_DebtManagement_SummaryAccordingToStatus(InstID, YearID, AgeStatusID)
        //            }
        //        };
        //        report.CreateDocument();
        //        return report;

        //    }
        //}

        internal static XtraReport rptInvoice(int InvoiceID)
        {
            throw new NotImplementedException();
        }
        internal static XtraReport rptExemptionLetters(int p, int YearID)
        {
            throw new NotImplementedException();
        }
    }
}