    function SystemAdminMenuItemClick(s, e) {

    if (e.item.name === "Btn_Add") {

        $.ajax({
            type: 'POST',
            url: '/SystemAdmin/InstitutionTakeonAdd',
            dataType : 'json',
            async : false,
            data : {
                MasterInstID: s.GetRowKey(s.GetFocusedRowIndex())
            }
        });
        s.PerformCallback();
    }

    if (e.item.name === "Btn_SetYearEnd") {

        $.ajax({
            type: 'POST',
            url: '/SystemAdmin/SetYearEnd',
            dataType: 'json',
            async: false,
            data: {
         
            }
        });
    }

}

function ApplyMenuItemClick(s, e) {

    if (e.item.name === "Btn_Print") {
        LoadingPanel.Show();
        pcExemptionApplication.PerformCallback();
        //$.ajax({
        //    type: 'POST',
        //    url: '/Exemptions/PrintRequestToApplyReport',
        //    dataType: 'json',
        //    async: false,
        //    data: {
        //        FamilyID: s.GetRowKey(GridView.GetFocusedRowIndex())                
        //    },
        //    success: function (response) {
        //        window.open(response.Url, '_Blank');
        //    }
        //});
    }

    if (e.item.name === "Btn_Apply") {
        ShowApplicationForm(GridView.GetRowKey(GridView.GetFocusedRowIndex()), 1);
    }
}

function ExemptionsMenuItemClick(s, e) {

    if (e.item.name === "Btn_Refresh") {
        GridView.PerformCallback({ YearID: cbYear.GetValue() });
    }

    if (e.item.name === "Btn_PrintExemptionReport") {
        LoadingPanel.Show();
        pcExemptionSummary.PerformCallback();
    }

    if (e.item.name === "Btn_PrintExemptionReportGrouped") {
        LoadingPanel.Show();
        pcExemptionCompensationGrouped.PerformCallback();
    }

    if (e.item.name === "Btn_PrintApplicationForCompensasion") {
        LoadingPanel.Show();
        pcExemptionCompensation.PerformCallback();
    }

    if (e.item.name === "Btn_PrintLearnerClassList") {
        $.ajax({
            type: 'POST',
            url: '/Exemptions/PrintLearnerClassListReport',
            dataType: 'json',
            async: false,
            data: {
                
            },
            success: function (response) {
                window.open(response.Url, '_Blank');
            }
        });
    }

    if (e.item.name === "Btn_PrintExemptionProvisionalList") {
        LoadingPanel.Show();
        pcExemptionProvisionalList.PerformCallback();        
    }

    if (e.item.name === "Btn_PrintFinalTableForExemptions") {
        $.ajax({
            type: 'POST',
            url: '/Exemptions/PrintFinalTableForExemptionsReport',
            dataType: 'json',
            async: false,
            data: {
                YearID: cbYear.GetValue()
            },
            success: function (response) {
                window.open(response.Url, '_Blank');
            }
        });
    }

    if (e.item.name === "Btn_PrintApplicationRegister") {
        LoadingPanel.Show();
        pcExemptionRegister.PerformCallback();        
    }

    if (e.item.name === "Btn_SettingsSendSMS") {  
        ShowSMSForm(GridView.GetRowKey(GridView.GetFocusedRowIndex()), 1, false, null);
    }

    if (e.item.name === "Btn_SettingsSendBulkSMS") {
        ShowSMSForm(null, 1, true, 1);
    }

    if (e.item.name === "Btn_CorrespondenceMethodEdit") {
        pcCorrespondenceMethod.Show();
    }

    if (e.item.name === "Btn_AccountInfo") {
        ShowFamilyInfoForm(false, true, true, GridView.GetRowKey(GridView.GetFocusedRowIndex()));
    }

    if (e.item.name === "Btn_SingleEmail") {
        //cbpSingleEmailExemption.PerformCallback();
        //pcSingleEmailExemptions.Show();
        ShowEmailForm(GridView.GetRowKey(GridView.GetFocusedRowIndex()), 1, false, null);      
    }

    if (e.item.name === "Btn_BulkEmail") {
        ShowEmailForm(null, 1, true, 1); 
    }

    if (e.item.name === "Btn_Re-CreateApplicationForm") {
        ShowApplicationForm(GridView.GetRowKey(GridView.GetFocusedRowIndex()), 2);
    }

    if (e.item.name === "Btn_PrintSMSMessageHistory") {
        LoadingPanel.Show();
        pcPrintSMSMessageHistoryReport.PerformCallback()
    } 

    if (e.item.name === "Btn_PrintEmailHistory") {
        LoadingPanel.Show();
        pcPrintEmailHistoryReport.PerformCallback()
    }

    if (e.item.name === "Btn_DeleteApplication") {
        pcDeleteApplication.Show();
    }

    if (e.item.name === "Btn_PrintApplicationForCompensasion_Freestate") {
        LoadingPanel.Show();
        pcExemptionCompensationFreestate.PerformCallback();
    }

    if (e.item.name === "Btn_PrintExemptionSingleLetter") {
        GridView.GetRowValues(GridView.GetFocusedRowIndex(), "ExemptionID;ExemptStatusID;FamilyID", SetPrintLetterValues);
    }

    if (e.item.name === "Btn_PrintCovidForm") {
        LoadingPanel.Show();
        pcPrintCOVID19form.PerformCallback()
    }
}

function AdminManageInstitutionMenuItemClick(s, e) {
    if (e.item.name === "Btn_Save")
        pcConfirmSave.Show();
}

function ExemptionsSubmitApplicationMenuItemClick(s, e) {

    if (e.item.name === "Btn_Submit")
        SaveApplication(false);       

    if (e.item.name === "Btn_Save") {
        SaveApplication(true);        
    }
}

function SettingsMenuButton(item, enabled, image) {
    if (item) {
        item.SetEnabled(enabled);
        item.SetImageUrl("/Content/MenuButtons/" + image);
    }
}


function ExemptionsProcessApplicationMenuItemClick(s, e) {
    var FromPayment;
    var ToPayment;

    if (e.item.name === "Btn_Calculate") {
        
        if (PaymentFromDate.GetDate() !== null)
            FromPayment = PaymentFromDate.GetDate().toISOString();
        if (PaymentFromDate.GetDate() !== null)
            ToPayment = PaymentToDate.GetDate().toISOString();

        $.ajax({
            type: 'POST',
            url: '/Exemptions/Set_ApplicationCalculation',
            dataType: 'json',
            async: false,
            data: {
                ExemptionID: ExemptionID.GetText(),
                IncomeParent1: IncomeParent1.GetValue(),
                IncomeParent2: IncomeParent2.GetValue(),
                IsJointApplicationReceived: IsJointApplicationReceived.GetValue(),
                Offer: Offer.GetValue(),
                Monetary: Monetary.GetValue(),
                LearnersInOtherSchools: LearnersInOtherSchools.GetValue(),
                PaymentFromDate: FromPayment,
                PaymentToDate: ToPayment,
                Note: Note.GetText(),
                AmountPaidToDate: AmountPaidToDate.GetValue(),
                EffectiveFromMonth: 0,
                IsCovid19: IsCovid19.GetValue()
            },
            success: function (response) {                
                cbpApplicationCalculation.PerformCallback({ExemptionID: ExemptionID.GetText()});
            }
        });
    }

    if (e.item.name === "Btn_Process") {

        if (PaymentFromDate.GetDate() !== null)
            FromPayment = PaymentFromDate.GetDate().toISOString();
        if (PaymentFromDate.GetDate() !== null)
            ToPayment = PaymentToDate.GetDate().toISOString();


        $.ajax({
            type: 'POST',
            url: '/Exemptions/Set_ApplicationProcessed',
            dataType: 'json',
            async: false,
            data: {
                ExemptionID: ExemptionID.GetText(),
                IncomeParent1: IncomeParent1.GetValue(),
                IncomeParent2: IncomeParent2.GetValue(),
                IsJointApplicationReceived: IsJointApplicationReceived.GetValue(),
                Offer: Offer.GetValue(),
                Monetary: Monetary.GetValue(),
                LearnersInOtherSchools: LearnersInOtherSchools.GetValue(),
                PaymentFromDate: FromPayment,
                PaymentToDate: ToPayment,
                Note: Note.GetText(),
                AmountPaidToDate: AmountPaidToDate.GetValue(),
                EffectiveFromMonth: 0,
                IsCovid19: IsCovid19.GetValue()
            },
            success: function (response) {
                LoadingPanel.Show();
                pcExemptionCalculation.PerformCallback({ ExemptionID: ExemptionID.GetText(), FileName: response.FileName });
            }
        });
    }

}

function ExemptionsFinaliseSetExemptionReasonMenuItemClick(s, e) {
    if (e.item.name === "Btn_Finalise") {

        if (FinaliseReasonTypeID !== 0) {
            cbpFinaliseApplicationReason.PerformCallback();
            pcExemptionReason.Show();
        }
        else
        {

            var CanContinue = true;
            var DateNow = new Date();
            var resultDate;
            
            switch (ExemptionTypeID) {
                case 5:
                case 7:
                case 9:
                case 14:
                    ExemptedAmount = 0;
                    break;
                case 6:
                    ExemptedAmount = tbExemtionAmount.GetValue();
                    break;
                case 8:
                    ExemptedAmount = tbOfferedAmount.GetValue();
                    break;
                case 10:
                    ExemptedAmount = 0;
                    break;
                case 15:
                    ExemptedAmount = tbConditionalExemtionAmount.GetValue();
                    break;
                case 12:
                    if (deInterviewDate.GetDate() === null)
                    {
                        ParentInterviewDate = null;
                        ParentInterviewStartTime = null;
                        ParentInterviewEndTime = null;
                    }
                    else
                    {
                        if (deInterviewDate.GetDate() !== null)
                        {
                            if (deInterviewDate.GetDate().toISOString() > new Date().toISOString()) {
                                ParentInterviewDate = deInterviewDate.GetDate().toISOString();
                                if (teInterviewStartTime.GetDate() !== null)
                                    ParentInterviewStartTime = teInterviewStartTime.GetDate().toLocaleTimeString();
                                if (teInterviewEndTime.GetDate() !== null)
                                    ParentInterviewEndTime = teInterviewEndTime.GetDate().toLocaleTimeString();
                            }
                            else
                            {
                                pcChooseValidDate.Show();
                                CanContinue = false;
                            }

                        }
                        else
                        {
                            pcChooseValidDate.Show();
                            CanContinue = false;
                        }

                    }

                }

                if (CanContinue === true)
                $.ajax({
                    type: 'POST',
                    url: '/Exemptions/Set_ApplicationFinalise',
                    dataType: 'json',
                    async: false,
                    data: {
                        ExemptionID: ExemptionID.GetText(),
                        ExemptStatusID: ExemptionTypeID,
                        AmountExempted: ExemptedAmount,
                        ParentInterviewDate: ParentInterviewDate,
                        ParentInterviewStartTime: ParentInterviewStartTime,
                        ParentInterviewEndTime: ParentInterviewEndTime
                    },
                    success: function (result) {
                        if (result.Success === true) {
                            if (result.PrintOnFinalise === true) {
                                window.open(result.Url, '_Blank');
                            }
                            tbApplicationNumber.SetText('');
                            pcConfirmFinalize.Show();
                        }
                        else
                            pcWarning.Show();
                    }
                });

        }
        
    }
}

function ApplicationPacksMenuItemClick(s, e) {
   
    if (e.item.name === "Btn_Submit") {
        pcPurchasePackage.Show();
        
    }
}

function PacksPendingActivationMenuItemClick(s, e) {

    if (e.item.name === "Btn_Submit") {
        s.GetRowValues(s.GetFocusedRowIndex(), "Name;Package", onConfirmActivation);

    }
}

function PacksPendingActivationRefreshMenuItemClick(s, e) {

    if (e.item.name === "Btn_Refresh") {
        s.PerformCallback({ InstID: Convert.ToInt32(HttpContext.Session["InstID"]) });

    }
}

function InstitutionLearnersMenuItemClick(s, e) {
    if (e.item.name === "Btn_MoveLearner")
        OnBtn_MoveLearnerClick();

}

function AgeAnalysisMenuItemClick(s, e) {

    if (e.item.name === "Btn_Refresh") {
        GridView.PerformCallback();
    }
    
    if (e.item.name === "Btn_PrintSingleLetter") {
        GridView.GetRowValues(GridView.GetFocusedRowIndex(), "FamilyID;AgeStatusID", SetPrintLetterValues);
    }

    if (e.item.name === "Btn_PrintBulkLetter") {
        pcPrintBulkLetters.Show();
    }

    if (e.item.name === "Btn_PrintParentAccountHistory") {
        LoadingPanel.Show();
        pcParentAccountHistory.PerformCallback();        
    }

    if (e.item.name === "Btn_PrintAgeSummaryReport") {
        LoadingPanel.Show();
        pcDebtManagementSummary.PerformCallback();        
    }

    if (e.item.name === "Btn_SettingsSendSMS") {
        ShowSMSForm(GridView.GetRowKey(GridView.GetFocusedRowIndex()), 2, false, null);        
    }

    if (e.item.name === "Btn_SettingsSendSMSBulk") {
        ShowSMSForm(null, 2, true, 2);
    }

    if (e.item.name === "Btn_SingleEmail") {
        ShowEmailForm(GridView.GetRowKey(GridView.GetFocusedRowIndex()), 2, false, null);        
    }

    if (e.item.name === "Btn_BulkEmail") {
        ShowEmailForm(null, 2, true, 2);
    }

    if (e.item.name === "Btn_Note") {
        IsAddNewFamily = false;
        IsUseExemptionID = false;        
        cbpDebtManagemenNote.PerformCallback();
        pcDebtManagemenNote.Show();        
    }

    if (e.item.name === "Btn_AccountInfo") {
        ShowFamilyInfoForm(false, false, true, GridView.GetRowKey(GridView.GetFocusedRowIndex()));
    }

    if (e.item.name === "Btn_PrintPaymentArrangements") {
        LoadingPanel.Show();
        pcPaymentArrangements.PerformCallback();
    }

    if (e.item.name === "Btn_PrintPaymentArrangementsFamily") {
        LoadingPanel.Show();
        pcPaymentArrangementsFamily.PerformCallback();
    }

    if (e.item.name === "Btn_StartLegal") {
        pcLegalProcess.Show();
    }    

    if (e.item.name === "Btn_PrintSMSMessageHistory") {
        LoadingPanel.Show();
        pcPrintSMSMessageHistoryReport.PerformCallback();
    } 

    if (e.item.name === "Btn_PrintEmailHistory") {
        LoadingPanel.Show();
        pcPrintEmailHistoryReport.PerformCallback();
    }

    if (e.item.name === "Btn_GenerateRegisteredEmails") {
        pcRegisteredEmails.Show();
    } 

    if (e.item.name === "Btn_PrintRegisteredEmailPeport") {
        LoadingPanel.Show();
        pcRegisteredEmailsReport.PerformCallback();
    }

}

function AdminSchoolFeesMenuItemClick(s, e) {

    if (e.item.name === "Btn_Refresh") {
        GridView.PerformCallback();
    }
}

function LegalProcessSettingsMenuItemClick(s, e) {

    if (e.item.name === "Btn_Refresh") {
        SMSGridView.PerformCallback();
        LettersGridView.PerformCallback();
    }   
}

function AdminSMSBundleMenuItemClick(s, e) {

    if (e.item.name === "Btn_Refresh") {
        GridView.PerformCallback();
    }

    if (e.item.name === "Btn_Submit") {
        pcSMSBundles.Show();
    }
}

function BundlesPendingActivationMenuItemClick(s, e) {

    if (e.item.name === "Btn_Refresh") {
        GridView.PerformCallback();
    }

    if (e.item.name === "Btn_Submit") {
        s.GetRowValues(s.GetFocusedRowIndex(), "Name;Bundle", onConfirmActivation);
    }
}

function MarketingMenuItemClick(s, e) {

    if (e.item.name === "Btn_Refresh") {
        GridView.PerformCallback();
    }

    if (e.item.name === "Btn_Info") {
        pcInstitutionContacts.Show();
        ContactsGridView.PerformCallback({ InstID: GridView.GetRowKey(GridView.GetFocusedRowIndex())});

    }

    if (e.item.name === "Btn_AddNote") {
        pcAddNote.Show();
    }
}

function IntegrationMenuItemClick(s, e) {

    if (e.item.name === "Btn_ImportData") {
        pcUploadData.Show();
    }

    if (e.item.name === "Btn_UpdateLearners") {
        LoadingPanel.Show();
        s.GetSelectedFieldValues("IntLearnerID", UpdateLearners);
    }    

    if (e.item.name === "Btn_UpdateParents") {
        LoadingPanel.Show();
        s.GetSelectedFieldValues("IntParentID", UpdateParents);
    } 

}

function InstitutionFamilyMenuItemClick(s, e) {

    if (e.item.name === "Btn_Refresh") {
        GridView.PerformCallback();
    }

    if (e.item.name === "Btn_EditFamily") {
        ShowFamilyEditForm(true, true, false, GridView.GetRowKey(GridView.GetFocusedRowIndex()));
    }

    if (e.item.name === "Btn_AddFamily") {
        ShowFamilyEditForm(true, false, false, GridView.GetRowKey(GridView.GetFocusedRowIndex()));
    }

    if (e.item.name === "Btn_AccountInfo") {
        ShowFamilyInfoForm(false, false, false, GridView.GetRowKey(GridView.GetFocusedRowIndex()));
    }
        
    if (e.item.name === "Btn_FamilyDetail") {
        LoadingPanel.Show();
        pcFamilyDetail.PerformCallback();                
    }

    if (e.item.name === "Btn_CombineFamily") {
        pcCombineFamilies.Show();
    }
 }

function AgeAnalysisImportMenuItemClick(s, e) {

    if (e.item.name === "Btn_Import") {
        pcImportAgeAnalysis.Show();
    }

    if (e.item.name === "Btn_UpdateAge") {
        LoadingPanel.SetText("Processing Age Analysis...</br> This can take a few minutes.");
        LoadingPanel.Show();
        UpdateAgeAnalisis();
    }

    if (e.item.name === "Btn_DeleteAgeImport") {
        pcDeleteAge.Show();
    }
}

function BulkCorrespondenceBulkSMSMenuItemClick(s, e) {

    if (e.item.name === "Btn_SendMessages") {

        var str = GridView.GetRowKey(GridView.GetFocusedRowIndex());
        var res = str.split("|");

        $.ajax({
            type: 'POST',
            url: '/BulkCorrespondence/SendBulkSMS',
            dataType: 'json',
            async: false,
            data: {
                TrackingKey: res[0]
            },
            success: function (result) {
                if (result.Success === true) {
                    pcBulkSMSSending.Show();
                    GridView.PerformCallback();
                }
            }
        });
    }
    
    if (e.item.name === "Btn_DeleteSMS") {
        pcDeleteSMS.Show();        
    }
}

function BulkCorrespondenceBulkEmailMenuItemClick(s, e) {
    
    if (e.item.name === "Btn_SendEmails") {

        var str = GridView.GetRowKey(GridView.GetFocusedRowIndex());
        var res = str.split("|");

        $.ajax({
            type: 'POST',
            url: '/BulkCorrespondence/SendBulkEmails',
            dataType: 'json',
            async: false,
            data: {
                TrackingKey: res[0]
            },
            success: function (result) {
                if (result.Success === true) {
                    pcBulkEmailSending.Show();
                    GridView.PerformCallback();
                }
            }
        });
    }

    if (e.item.name === "Btn_DeleteEmail") {

        pcDeleteEmail.Show();
        
    }

}

function CorrespondencesSMSRepliesMenuItemClick(s, e) {

    if (e.item.name === "Btn_SMSReplyRead") {
        $.ajax({
            type: 'POST',
            url: '/BulkCorrespondence/SMSRepliesRead',
            dataType: 'json',
            async: false,
            data: {
                ReplyID: GridView.GetRowKey(GridView.GetFocusedRowIndex())
            },
            success: function (result) {
                if (result.Success === true) {
                    GridView.PerformCallback();
                }
            }
        });
    }
}

function LoggedCasesMenuItemClick(s, e) {

    if (e.item.name === "Btn_AddNewCase") {
        cbpSupportCaseActions.PerformCallback();
        pcAddNewCase.Show();
    }

    if (e.item.name === "Btn_EditSupportCase") {
        cbpEditSupportCase.PerformCallback();
        pcEditSupportCase.Show();
    }
}

function ClientLedgerMenuItemClick(s, e) {

    if (e.item.name === "Btn_AddNewLedger") {
        pcAddNewLedger.Show();
    }

    if (e.item.name === "Btn_GenerateInvoice") {
        pcAddNewInvoice.Show();
    }
    
}

function InstitutionExternalAccNumbersMenuItemClick(s, e) {

    if (e.item.name === "Btn_Save") {
        GridView.UpdateEdit();
        $.ajax({
            type: 'POST',
            url: '/Institution/UpdateExternalAccNumbers',
            dataType: 'json',
            async: false
        });
        GridView.PerformCallback();
    }

}

function SystemAdminInstitutionMenuItemClick(s, e) {
    if (e.item.name === "Btn_AnnualLicenceFees")
        $.ajax({
            type: 'POST',
            url: '/SystemAdmin/Set_AnnualLicenceFees',
            dataType: 'json',
            async: false,
            data: {

            },
            success: function (result) {
                if (result.Success === true) {
                    pcAnnualLicenceFeesSet.Show();
                }
            }
        });

}

//function BulkSMSTrackingHistoryMenuItemClick(s, e) {
//    if (e.item.name === "Btn_EditActinHistorySMSNumbers")
//        $.ajax({
//            type: 'POST',
//            url: '/SystemAdmin/Set_AnnualLicenceFees',
//            dataType: 'json',
//            async: false,
//            data: {

//            },
//            success: function (result) {
//                if (result.Success === true) {
//                    pcAnnualLicenceFeesSet.Show();
//                }
//            }
//        });

//}

function BulkCorrespondencesBulkLetterMenuItemClick(s, e) {
    if (e.item.name === "Btn_DeleteLetter") {

        pcDeleteLetter.Show();
    }

}

//function DebtManagementFollowUpsMenuItemClick(s, e) {

//    if (e.item.name === "Btn_EditReloveFollowUp") {
//        GridView.GetRowValues(GridView.GetFocusedRowIndex(), "ContactReasonID", SetContactReasonID);
        
//    }
//}

function DebtManagementSettingsDebtCollectorMenuItemClick(s, e) {

    if (e.item.name === "Btn_DeleteDebtCollector") {
        pcDeleteDebtCollector.Show();
    }
}

function SystemAdminNewInvoiceMenuItemClick(s, e) {

    if (e.item.name === "Btn_DeleteInvoice") {
        pcDeleteInvoice.Show();
    }

    if (e.item.name === "Btn_CreateInvoice")
        $.ajax({
            type: 'POST',
            url: '/SystemAdmin/Set_AnnualLicenceFees',
            dataType: 'json',
            async: false,
            data: {

            },
            success: function (result) {
                if (result.Success === true) {
                    pcAnnualLicenceFeesSet.Show();
                }
            }
        });
}

function InstitutionAdHocFeesMenuItemClick(s, e) {

    if (e.item.name === "Btn_AddAdHocFees") {
        pcAdHocFees.Show();
    }

    if (e.item.name === "Btn_PrintAdHocFees") {
        LoadingPanel.Show();
        pcReportAdHocFees.PerformCallback();
    }
}

function AdminFinanceMenuItemClick(s, e) {

    if (e.item.name === "Btn_GenerateQuotation") {
        pcQuotation.Show();
    }

    if (e.item.name === "Btn_AcceptQuotation") {
        pcAcceptQuotation.Show();
    }
    
}

function PastelCategoriesMenuItemClick(s, e) {
    if (e.item.name === "Btn_GenerateQuotation") {
        GetPastelCategories = true;
        ucImportAge.Upload();
    }
}

function BulkCorrespondenceListMenuItemClick(s, e) {

    if (e.item.name === "Btn_AddCorrespondenceList") {
        pcEditCorrespondenceItem.Show();
    }

    if (e.item.name === "Btn_QueueListMessage") {
        cbpCommunicationSendSMS.PerformCallback();
        s.GetSelectedFieldValues("LearnerID", SetCorrespondenceListSMS);
    }

    if (e.item.name === "Btn_QueueListEmails") {
        cbpCommunicationSendEmail.PerformCallback();
        s.GetSelectedFieldValues("LearnerID", SetCorrespondenceListEmails);
    }

    if (e.item.name === "Btn_CreateCustomList") {
        s.GetSelectedFieldValues("LearnerID", SetCustomCorrespondenceList);
    }

    if (e.item.name === "Btn_RemoveLearnersFromList") {
        s.GetSelectedFieldValues("LearnerID", SetRemoveLearnersList);
    }

    if (e.item.name === "Btn_AddToContactList") {
        s.GetSelectedFieldValues("LearnerID", SetAddToContactList);
    }

    if (e.item.name === "Btn_SaveLearnerEmailAndCell") {
        GridView.UpdateEdit();
        $.ajax({
            type: 'POST',
            url: '/BulkCorrespondence/UpdateLearnerCellNumberAndEmail',
            dataType: 'json',
            async: false
        });
        GridView.PerformCallback();
    }
}

function CommunicationHistoryMenuItemClick(s, e) {

    if (e.item.name === "Btn_PrintCommunicationHistory") {
        LoadingPanel.Show();
        pcCommunicationHistory.PerformCallback();  
    }

    if (e.item.name === "Btn_PrintCommunicationHistoryFamily") {
        LoadingPanel.Show();
        pcCommunicationHistoryFamily.PerformCallback();
    }
}

function DebtCollectorMenuItemClick(s, e) {

    if (e.item.name === "Btn_PrintParentAccountHistory") {
        LoadingPanel.Show();
        pcParentAccountHistory.PerformCallback();
    }

    if (e.item.name === "Btn_PrintSMSMessageHistory") {
        LoadingPanel.Show();
        pcPrintSMSMessageHistoryReport.PerformCallback()
    } 

    if (e.item.name === "Btn_PrintEmailHistoryHistory") {
        LoadingPanel.Show();
        pcPrintEmailHistoryReport.PerformCallback();
    }
}