var ApplicationKeyValue;
var ApplicationKeyValueType; //1 = NewAppliaction; 2 = ReprintApplication


function ShowApplicationForm(KeyValue, KeyValueType) {
    ApplicationKeyValue = KeyValue;
    ApplicationKeyValueType = KeyValueType;

    ShowExemptionApplicationForm();
    
}

function ShowExemptionApplicationForm() {
    cbpApplicantToApplyOrReprintInfo.PerformCallback({ ApplicationKeyValue: ApplicationKeyValue, ApplicationKeyValueType: ApplicationKeyValueType });
    pcGenerateApplication.Show();
}


function ApplyForExemption() {
    var FamilyID = GridView.GetRowKey(GridView.GetFocusedRowIndex());
    $.ajax({
        type: 'POST',
        url: '/Exemptions/ApplyForExemption',
        dataType: 'json',
        async: false,
        data: {
            FamilyID: GridView.GetRowKey(GridView.GetFocusedRowIndex()),
            ExternalAccNo: ApplicantExternalAccNo.GetValue(),
            ApplMaritalStatusID: ApplicantMaritalStatusID.GetValue(),
            IsSingleApplication: ApplicantIsSingleApplication.GetValue(),
            Wareabouts: rbWareabouts.GetValue(),
            Contact: rbContact.GetValue(),
            Unwilling: rbUnwilling.GetValue(),
            isAttachCovidForm: cbIsAttachCovidForm.GetValue()
        },
        success: function (response) {
            if (response.Success === true) {
                if (response.Url !== "") {
                    window.open(response.Url, '_Blank');
                }
            }
            else {
                pcErrorlbMessage.SetText(response.ExceptionMessage);
                pcError.Show();
            }
        }
    });

    IsApplyForExemptions = false;
    GridView.PerformCallback();
    SetApplyButton(GridView);

}

function ReCreateApplication() {

    //var IsAttachCovidForm;
    //if (cbIsAttachCovidForm.GetValue() === null) {
    //    IsAttachCovidForm = false;
    //}
    //else {
    //    IsAttachCovidForm = true;
    //}

    $.ajax({
        type: 'POST',
        url: '/Shared/ReCreateApplication',
        dataType: 'json',
        async: false,
        data: {
            ExemptionID: GridView.GetRowKey(GridView.GetFocusedRowIndex()),
            LanguageID: LanguageID.GetValue(),
            IsSingleApplication: ApplicantIsSingleApplication.GetValue(),
            ApplMaritalStatusID: ApplicantMaritalStatusID.GetValue(),
            Wareabouts: rbWareabouts.GetValue(),
            Contact: rbContact.GetValue(),
            Unwilling: rbUnwilling.GetValue(),
            isAttachCovidForm: cbIsAttachCovidForm.GetValue(),
        },
        success: function (response) {
            if (response.Success === true) {
                if (response.Url !== "") {
                    window.open(response.Url, '_Blank');
                }
            }
            else {
                pcErrorlbMessage.SetText(response.ExceptionMessage);
                pcError.Show();
            }
        }
    });
    
    GridView.PerformCallback();

}

function MaritalStatusIDChange(s, e) {
    var x = s.GetValue();
    var ReqDocstr = "";
    if (x === 1)
        btnApplyForExemption.SetEnabled(false);
    else
        btnApplyForExemption.SetEnabled(true);

    switch (x) {
        case 2:
            ReqDocstr = SingleContactRequiredDocsApply.GetValue();
            break;
        case 4:
            ReqDocstr = DivorcedContactRequiredDocsApply.GetValue();
            break;
        case 6:
            ReqDocstr = SingleNoContactRequiredDocsApply.GetValue();
            break;
        case 7:
            ReqDocstr = DivorcedNoContactRequiredDocsApply.GetValue();
    }

    if (x === 2 || x === 4 || x === 6 || x === 7) {
        var Affidavitstr = "6";
        if (ReqDocstr.match(new RegExp("(?:^|,)" + Affidavitstr + "(?:,|$)"))) {
            lWareabouts.SetVisible(true);
            rbWareabouts.SetVisible(true);
            lContactWithSpouse.SetVisible(true);
            rbContact.SetVisible(true);
            lSpouseUnwilling.SetVisible(true);
            rbUnwilling.SetVisible(true);
        }
        else {
            lWareabouts.SetVisible(false);
            rbWareabouts.SetVisible(false);
            rbWareabouts.SetValue(2);
            lContactWithSpouse.SetVisible(false);
            rbContact.SetVisible(false);
            rbContact.SetValue(4);
            lSpouseUnwilling.SetVisible(false);
            rbUnwilling.SetVisible(false);
            rbUnwilling.SetValue(6);
        }
    }
    else {
        lWareabouts.SetVisible(false);
        rbWareabouts.SetVisible(false);
        rbWareabouts.SetValue(2);
        lContactWithSpouse.SetVisible(false);
        rbContact.SetVisible(false);
        rbContact.SetValue(4);
        lSpouseUnwilling.SetVisible(false);
        rbUnwilling.SetVisible(false);
        rbUnwilling.SetValue(6);
    }
    
    if (ApplicationKeyValueType !== 2)
    {
        var str = "2,5,6,7";
        if (str.match(new RegExp("(?:^|,)" + x + "(?:,|$)"))) {
            ApplicantIsSingleApplication.SetChecked(true);
        }
        else {
            ApplicantIsSingleApplication.SetChecked(false);
        }
    }
   
}

