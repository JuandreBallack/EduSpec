var SMSKeyValue;
var SMSKeyValueType; //1 = ExemptionsID; 2 = FamilyID; 3 = FollowUpsID; 
var IsBulkSMS;
var BulkSMSTypeID; //1 = Exemptions; 2 = DebtManagement;

function ShowSMSForm(KeyValue, KeyValueType, IsBulk, BulkSMSType) {
    SMSKeyValue = KeyValue;
    SMSKeyValueType = KeyValueType;
    IsBulkSMS = IsBulk;
    BulkSMSTypeID = BulkSMSType;

    if (IsBulk) {
        ShowBulkSMSForm();
    }
    else {
        ShowSingleSMSForm();
    }
}
 
function ShowSingleSMSForm() {
    cbpSendSingleSMS.PerformCallback({ SMSKeyValue: SMSKeyValue, SMSKeyValueType: SMSKeyValueType });
    pcSendSingleSMS.Show();
}

function ShowBulkSMSForm() {
    cbpSendBulhSMS.PerformCallback({ BulkSMSTypeID: BulkSMSTypeID });
    pcSendBulkSMS.Show();
}

function SendSingleSMS() {

    $.ajax({
        type: 'POST',
        url: '/Shared/SendSingleSMS',
        dataType: 'json',
        async: false,
        data: {
            cellnumber: ParentsCell.GetValue(),
            message: SMSMessage.GetValue(),
            KeyValue: SMSKeyValue,
            KeyValueType: SMSKeyValueType
        },
        success: function (result) {
            pcSendSingleSMS.Hide();
            if (SMSKeyValueType === 1)
                DetailGridView.PerformCallback({ ExemptionID: SMSKeyValue });
            if (SMSKeyValueType === 2)
                DetailGridView.PerformCallback({ FamilyID: SMSKeyValue });
            cbpInfoPanel.PerformCallback();
            pcSMSSent.Show();
        }
    });

}

function SendSingleCustomSMS() {

    $.ajax({
        type: 'POST',
        url: '/Shared/SendSingleCustomSMS',
        dataType: 'json',
        async: false,
        data: {
            cellnumber: ParentsCellCustomSMS.GetValue(),
            messageID: ComboBoxListProc_2.GetValue(),
            KeyValue: SMSKeyValue,
            KeyValueType: SMSKeyValueType
        },
        success: function (result) {
            pcSendSingleSMS.Hide();
            if (SMSKeyValueType === 1)
                DetailGridView.PerformCallback({ ExemptionID: SMSKeyValue });
            if (SMSKeyValueType === 2)
                DetailGridView.PerformCallback({ FamilyID: SMSKeyValue });
            cbpInfoPanel.PerformCallback();
            pcSMSSent.Show();
        }
    });

}

function SendBulkSMS() {

    var Validated = true;

    if (document.getElementById("ComboBoxListProc_1") && Validated)
        Validated = ComboBoxListProc_1.GetIsValid();

    if (document.getElementById("ComboBoxListProc_2") && Validated)
        Validated = ComboBoxListProc_2.GetIsValid();

    //if (document.getElementById("ComboBoxListProc_3") && Validated)
    //    Validated = ComboBoxListProc_3.GetIsValid();

    if (Validated && BulkSMSTypeID === 1) {
        $.ajax({
            type: 'POST',
            url: '/Shared/QueueBulkSMS',
            dataType: 'json',
            async: false,
            data: {
                BulkSMSTypeID: BulkSMSTypeID,
                StatusID: ComboBoxListProc_1.GetValue(),
                SMSMessageID: ComboBoxListProc_2.GetValue(),
                CorrespondenceMethodID: ComboBoxListProc_3.GetValue()
            },
            success: function (result) {
                pcSendBulkSMS.Hide();
                pcSMSBundlelbMessage.SetText(result.Message);
                pcSMSBundle.Show();
            }
        });
    }

    if (Validated && BulkSMSTypeID === 2) {
        $.ajax({
            type: 'POST',
            url: '/Shared/QueueBulkSMS',
            dataType: 'json',
            async: false,
            data: {
                BulkSMSTypeID: BulkSMSTypeID,
                StatusID: ComboBoxListProc_1.GetValue(),
                SMSMessageID: ComboBoxListProc_2.GetValue(),
                CorrespondenceMethodID: 0,
                IsExcludeZeroBalances: IsExcludeZeroBalances.GetValue(),
                IsExcludeNegativeBalances: IsExcludeNegativeBalances.GetValue()
            },
            success: function(result) {
                pcSendBulkSMS.Hide();
                pcSMSBundlelbMessage.SetText(result.Message);
                pcSMSBundle.Show();
            }
        });
    }
}