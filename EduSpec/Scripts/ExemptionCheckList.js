function setParentType(s, e) {

    switch (s.GetSelectedIndex()) {
        case 0: //Parent
            {
                $("#ApplicationMaritalStatus").show();
                $("#ApplicationProofOfIncome").show();
                $("#ApplicationExpenditure").show();
                $("#ApplicationProofOfGuardianFosterCare").hide();                
            }
            break;
        case 1: //Guardian/Foster care
            {
                $("#ApplicationMaritalStatus").hide();
                $("#ApplicationProofOfIncome").hide();
                $("#ApplicationExpenditure").hide();
                $("#ApplicationProofOfGuardianFosterCare").show();
            }
            break;        
    }
    
}

function setParentSpouseIDType(s, e, SingleContactRequiredDocs, SingleNoContactRequiredDocs, MarriedRequiredDocs, DivorcedContactRequiredDocs, DivorcedNoContactRequiredDocs, WidowRequiredDocs, ForeignersRequiredDocs)
{
    var SingleContact = SingleContactRequiredDocs.split(",");
    var SingleNoContact = SingleNoContactRequiredDocs.split(",");
    var Married = MarriedRequiredDocs.split(",");
    var DivorcedContact = DivorcedContactRequiredDocs.split(",");
    var DivorcedNoContact = DivorcedNoContactRequiredDocs.split(",");
    var Widow = WidowRequiredDocs.split(",");
    var Foreigners = ForeignersRequiredDocs.split(",");

    switch (s.GetSelectedIndex()) {
        case 0: //Single (Contact with Biological Spouse)
            {
                setMaritalStatusRequirementsCheckboxes(
                    (SingleContact[0] === 'true') ? true : false,
                    (SingleContact[1] === 'true') ? true : false,
                    (SingleContact[2] === 'true') ? true : false,
                    (SingleContact[3] === 'true') ? true : false,
                    (SingleContact[4] === 'true') ? true : false,
                    (SingleContact[5] === 'true') ? true : false,
                    (SingleContact[6] === 'true') ? true : false,
                    (SingleContact[7] === 'true') ? true : false
                );
                $("#SpouseProofOfIncome").show();
                btnPrintAffidavitRefuses.SetVisible(true);
            }
            break;
        case 1: //Single (No Contact with Biological Spouse)
            {
                setMaritalStatusRequirementsCheckboxes(
                    (SingleNoContact[0] === 'true') ? true : false,
                    (SingleNoContact[1] === 'true') ? true : false,
                    (SingleNoContact[2] === 'true') ? true : false,
                    (SingleNoContact[3] === 'true') ? true : false,
                    (SingleNoContact[4] === 'true') ? true : false,
                    (SingleNoContact[5] === 'true') ? true : false,
                    (SingleNoContact[6] === 'true') ? true : false,
                    (SingleNoContact[7] === 'true') ? true : false
                );
                $("#SpouseProofOfIncome").hide();
                btnPrintAffidavitRefuses.SetVisible(false);
                setClearSpouseIncomeDocs();
            }
            break;
        case 2: //Married
            {
                setMaritalStatusRequirementsCheckboxes(
                    (Married[0] === 'true') ? true : false,
                    (Married[1] === 'true') ? true : false,
                    (Married[2] === 'true') ? true : false,
                    (Married[3] === 'true') ? true : false,
                    (Married[4] === 'true') ? true : false,
                    (Married[5] === 'true') ? true : false,
                    (Married[6] === 'true') ? true : false,
                    (Married[7] === 'true') ? true : false
                ); 
                $("#SpouseProofOfIncome").show();
                btnPrintAffidavitRefuses.SetVisible(true);
            }
            break;
        case 3: //Divorced (Contact with Biological Spouse)
            {
                setMaritalStatusRequirementsCheckboxes(
                    (DivorcedContact[0] === 'true') ? true : false,
                    (DivorcedContact[1] === 'true') ? true : false,
                    (DivorcedContact[2] === 'true') ? true : false,
                    (DivorcedContact[3] === 'true') ? true : false,
                    (DivorcedContact[4] === 'true') ? true : false,
                    (DivorcedContact[5] === 'true') ? true : false,
                    (DivorcedContact[6] === 'true') ? true : false,
                    (DivorcedContact[7] === 'true') ? true : false
                ); 
                $("#SpouseProofOfIncome").show();
                btnPrintAffidavitRefuses.SetVisible(true);
            }
            break;
        case 4: //Divorced (No Contact with Biological Spouse)            
            {
                setMaritalStatusRequirementsCheckboxes(
                    (DivorcedNoContact[0] === 'true') ? true : false,
                    (DivorcedNoContact[1] === 'true') ? true : false,
                    (DivorcedNoContact[2] === 'true') ? true : false,
                    (DivorcedNoContact[3] === 'true') ? true : false,
                    (DivorcedNoContact[4] === 'true') ? true : false,
                    (DivorcedNoContact[5] === 'true') ? true : false,
                    (DivorcedNoContact[6] === 'true') ? true : false,
                    (DivorcedNoContact[7] === 'true') ? true : false
                ); 
                $("#SpouseProofOfIncome").hide();
                setClearSpouseIncomeDocs();
                btnPrintAffidavitRefuses.SetVisible(false);
            }
            break;
        case 5: //Widow/er
            {
                setMaritalStatusRequirementsCheckboxes(
                    (Widow[0] === 'true') ? true : false,
                    (Widow[1] === 'true') ? true : false,
                    (Widow[2] === 'true') ? true : false,
                    (Widow[3] === 'true') ? true : false,
                    (Widow[4] === 'true') ? true : false,
                    (Widow[5] === 'true') ? true : false,
                    (Widow[6] === 'true') ? true : false,
                    (Widow[7] === 'true') ? true : false
                );
                $("#SpouseProofOfIncome").hide();
                setClearSpouseIncomeDocs();
                btnPrintAffidavitRefuses.SetVisible(false);
            }
            break;
        case 6: //Foreigners
            {
                setMaritalStatusRequirementsCheckboxes(
                    (Foreigners[0] === 'true') ? true : false,
                    (Foreigners[1] === 'true') ? true : false,
                    (Foreigners[2] === 'true') ? true : false,
                    (Foreigners[3] === 'true') ? true : false,
                    (Foreigners[4] === 'true') ? true : false,
                    (Foreigners[5] === 'true') ? true : false,
                    (Foreigners[6] === 'true') ? true : false,
                    (Foreigners[7] === 'true') ? true : false
                );
                $("#SpouseProofOfIncome").show();
                btnPrintAffidavitRefuses.SetVisible(true);
            }
            break;
    }
}

function setMaritalStatusRequirementsCheckboxes(ParentID, SpouseID, Divorce, DeathCert, SpouseContact, Affidavit, Marriage, _VISA) {
    
    ParentIdentityDocument.SetVisible(ParentID);
    if (ParentID === false)
        ParentIdentityDocument.SetValue(false);

    SpouseIdentityDocument.SetVisible(SpouseID);
    if (SpouseID === false)
        SpouseIdentityDocument.SetValue(false);

    DivorceOrder.SetVisible(Divorce);
    if (Divorce === false)
        DivorceOrder.SetValue(false);

    DeathCertificate.SetVisible(DeathCert);
    if (DeathCert === false)
        DeathCertificate.SetValue(false);

    SpouseContactDetails.SetVisible(SpouseContact);
    if (SpouseContact === false)
        SpouseContactDetails.SetValue(false);

    AffidavitNoContact.SetVisible(Affidavit);
    btnPrintAffidavit.SetVisible(Affidavit);
    if (Affidavit === false) {
        AffidavitNoContact.SetValue(false);
        btnPrintAffidavit.SetVisible(false);
    }
        
    MarriageCertificate.SetVisible(Marriage)
    if (Marriage === false)
        MarriageCertificate.SetValue(false)

    VISA.SetVisible(_VISA)
    if (_VISA === false)
        VISA.SetValue(false)

}

function setIncomeRequirementsCheckboxes(Parent1, Payslip, Unemploy, BankStetements, Retrench, Sars, Welfare) {
    if (Parent1 === true) {
        ParentPayslip.SetVisible(Payslip);
        if (Payslip === false)
            ParentPayslip.SetValue(false);

        ParentBankStatements.SetVisible(BankStetements);
        if (BankStetements === false)
            ParentBankStatements.SetValue(false);

        ParentUnemploymentProof.SetVisible(Unemploy);
        if (Unemploy === false)
            ParentUnemploymentProof.SetValue(false);

        ParentRetrenchedProof.SetVisible(Retrench);
        if (Retrench === false)
            ParentRetrenchedProof.SetValue(false);

        ParentWelfareGrand.SetVisible(Welfare);
        if (Welfare === false)
            ParentWelfareGrand.SetValue(false);

        ParentSARS.SetVisible(Sars);
        if (Sars === false)
            ParentSARS.SetValue(false);
    }
    else {
        SpousePaySlip.SetVisible(Payslip);
        if (Payslip === false)
            SpousePaySlip.SetValue(false);

        SpouseBankStatements.SetVisible(BankStetements);
        if (BankStetements === false)
            SpouseBankStatements.SetValue(false);

        SpouseUnemploymentProof.SetVisible(Unemploy);
        if (Unemploy === false)
            SpouseUnemploymentProof.SetValue(false);

        SpouseRetrenchedProof.SetVisible(Retrench);
        if (Retrench === false)
            SpouseRetrenchedProof.SetValue(false);

        SpouseWelfareGrand.SetVisible(Welfare);
        if (Welfare === false)
            SpouseWelfareGrand.SetValue(false);

        SpouseSARS.SetVisible(Sars);
        if (Sars === false)
            SpouseSARS.SetValue(false);
    }
}

function setClearSpouseIncomeDocs(){
    SpousePaySlip.SetValue(false);
    SpouseBankStatements.SetValue(false);
    SpouseUnemploymentProof.SetValue(false);
    SpouseRetrenchedProof.SetValue(false);
    SpouseWelfareGrand.SetValue(false);
    SpouseSARS.SetValue(false);
}

function setIncomeRequirements(s, e, EmployedRequiredDocs, EmployedandWelfareGrandDocs, SelfEmployedRequiredDocs, UnemployedRequiredDocs, WelfareGrandRequiredDocs, RetrenchedRequiredDocs) {

    var Parent1;
    var Employed = EmployedRequiredDocs.split(",");
    var EmployedAndGrand = EmployedandWelfareGrandDocs.split(",");
    var SelfEmployed = SelfEmployedRequiredDocs.split(",");
    var Unemployed = UnemployedRequiredDocs.split(",");
    var WelfareGrand = WelfareGrandRequiredDocs.split(",");
    var Retrenched = RetrenchedRequiredDocs.split(",");

    if (s.name === 'ParentIncomeTypeID')
        Parent1 = true;
    else
        Parent1 = false;

    switch (s.GetSelectedIndex()) {
        case 0: //Employed
            {
                setIncomeRequirementsCheckboxes(
                    Parent1,
                    (Employed[0] === 'true') ? true : false,
                    (Employed[1] === 'true') ? true : false,
                    (Employed[2] === 'true') ? true : false,
                    (Employed[3] === 'true') ? true : false,
                    (Employed[4] === 'true') ? true : false,
                    (Employed[5] === 'true') ? true : false
                );
            }
            break;
        case 1: //Emploayed and Welfare Grand
            {
                setIncomeRequirementsCheckboxes(
                    Parent1,
                    (EmployedAndGrand[0] === 'true') ? true : false,
                    (EmployedAndGrand[1] === 'true') ? true : false,
                    (EmployedAndGrand[2] === 'true') ? true : false,
                    (EmployedAndGrand[3] === 'true') ? true : false,
                    (EmployedAndGrand[4] === 'true') ? true : false,
                    (EmployedAndGrand[5] === 'true') ? true : false
                );                
            }
            break;
        case 2: //Self Employed
            {
                setIncomeRequirementsCheckboxes(
                    Parent1,
                    (SelfEmployed[0] === 'true') ? true : false,
                    (SelfEmployed[1] === 'true') ? true : false,
                    (SelfEmployed[2] === 'true') ? true : false,
                    (SelfEmployed[3] === 'true') ? true : false,
                    (SelfEmployed[4] === 'true') ? true : false,
                    (SelfEmployed[5] === 'true') ? true : false
                );
            }
            break;
        case 3: //Unemployed
            {
                setIncomeRequirementsCheckboxes(
                    Parent1,
                    (Unemployed[0] === 'true') ? true : false,
                    (Unemployed[1] === 'true') ? true : false,
                    (Unemployed[2] === 'true') ? true : false,
                    (Unemployed[3] === 'true') ? true : false,
                    (Unemployed[4] === 'true') ? true : false,
                    (Unemployed[5] === 'true') ? true : false
                );
            }
            break;
        case 4: //Welfare Grand
            {
                setIncomeRequirementsCheckboxes(
                    Parent1,
                    (WelfareGrand[0] === 'true') ? true : false,
                    (WelfareGrand[1] === 'true') ? true : false,
                    (WelfareGrand[2] === 'true') ? true : false,
                    (WelfareGrand[3] === 'true') ? true : false,
                    (WelfareGrand[4] === 'true') ? true : false,
                    (WelfareGrand[5] === 'true') ? true : false
                );
            }
            break;
        case 5: //Retrenched
            {
                setIncomeRequirementsCheckboxes(
                    Parent1,
                    (Retrenched[0] === 'true') ? true : false,
                    (Retrenched[1] === 'true') ? true : false,
                    (Retrenched[2] === 'true') ? true : false,
                    (Retrenched[3] === 'true') ? true : false,
                    (Retrenched[4] === 'true') ? true : false,
                    (Retrenched[5] === 'true') ? true : false
                );
            }
            break;
    }
}

function SetExpenseCheckboxesEnabled(s, e) {

    var IsIncomplete = s.GetValue();

    RequireRentBond.SetEnabled(IsIncomplete);
    RequireMunicipalAccount.SetEnabled(IsIncomplete);
    RequireFurnitureAccounts.SetEnabled(IsIncomplete);
    RequireVehicleFinance.SetEnabled(IsIncomplete);
    RequireCreditCardAccount.SetEnabled(IsIncomplete);
    RequireClothingAccount.SetEnabled(IsIncomplete);
    RequireRevolvingCreditAccount.SetEnabled(IsIncomplete);
    RequireTelephoneAccount.SetEnabled(IsIncomplete);
    RequireCellPhoneAccount.SetEnabled(IsIncomplete);
}