var emailKeyValue;
var emailKeyValueType; //1 = ExemptionsID 2 = FamilyID
var IsBulkEmail;
var BulkEmailTypeID; //1 = Exemptions; 2 = DebtManagement

function ShowEmailForm(KeyValue, KeyValueType, IsBulk, BulkEmailType) {
    emailKeyValue = KeyValue;
    emailKeyValueType = KeyValueType;
    IsBulkEmail = IsBulk;
    BulkEmailTypeID = BulkEmailType;
    
    if (IsBulk)
    {
        ShowBulkEmailForm();
    }
    else {
        ShowSingleEmailForm();
    }
}

function ShowSingleEmailForm() {
    cbpSendSingleEmail.PerformCallback({ emailKeyValue: emailKeyValue, emailKeyValueType: emailKeyValueType });
    pcSendSingleEmail.Show();
}

function ShowBulkEmailForm() {
    cbpSendBulkEmail.PerformCallback({ BulkEmailTypeID: BulkEmailTypeID });
    pcSendBulkEmails.Show();
}

function SendSingleEmail() {
    $.ajax({
        type: 'POST',
        url: '/Shared/SendSingleEmail',
        dataType: 'json',
        async: false,
        data: {
            Recipient: PrimaryEmail.GetValue(),
            CcEmail: SecondaryEmail.GetValue(),
            Subject: EmailSubject.GetValue(),
            MessageEmail: EmailBody.GetHtml(),
            KeyValue: emailKeyValue,
            KeyValueType: emailKeyValueType
        },
        success: function (response) {
            pcSendSingleEmail.Hide();
            pcEmailSent.Show();
            GridView.PerformCallback();

        }
    });
}

function SendSingleEmailCustom() {
    $.ajax({
        type: 'POST',
        url: '/Shared/SendSingleEmailCustom',
        dataType: 'json',
        async: false,
        data: {
            Recipient: PrimaryEmailCustom.GetValue(),
            CcEmail: SecondaryEmailCustom.GetValue(),
            Subject: EmailSubjectCustom.GetValue(),
            MessageEmail: ComboBoxListProc_2.GetValue(),
            KeyValue: emailKeyValue,
            KeyValueType: emailKeyValueType
        },
        success: function (response) {
            pcSendSingleEmail.Hide();
            pcEmailSent.Show();
            GridView.PerformCallback();

        }
    });
}

function SendBulkEmail() {

    
        $.ajax({
            type: 'POST',
            url: '/Shared/SendBulkEmail',
            dataType: 'json',
            async: false,
            data: {
                StatusID: ComboBoxListProc_1.GetValue(),
                EmailMessageID: ComboBoxListProc_2.GetValue(),
                EmailSubject: BulkEmailSubject.GetValue(),
                EmailTypeID: BulkEmailTypeID,
                AttachEmailFileType: AttachBulkEmailFile.GetValue(),
                BulkLetterID: ComboBoxListProc_3.GetValue()
            },
            success: function (result) {
                if (result.Success) {
                    pcBulkEmailSent.Show();
                    pcSendBulkEmails.Hide();
                }

            }
        });
        

}

function SetBulkEmailTypeID() {
    $.ajax({
        type: 'POST',
        url: '/Shared/SetBulkEmailTypeID',
        dataType: 'json',
        async: false,
        data: {
            KeyValueType: emailKeyValueType
        },
        success: function (response) {
            

        }
    });
}

function ImportFile(s, e) {
    if (ucSingleEmailUploadData.GetText() === '') {
        SendSingleEmail();
    }
    else {
        ucSingleEmailUploadData.Upload();
    }
}

function ImportBulkEmailFile(s, e) {
    if (ucBulkEmailUploadData.GetText() === '') {
        SendBulkEmail();
    }
    else {
        SetBulkEmailTypeID();
        ucBulkEmailUploadData.Upload();
    }
}

function ImportFileEmailCustom(s, e) {
    if (ucSingleEmailCustomUploadData.GetText() === '') {
        SendSingleEmailCustom();
    }
    else {
        ucSingleEmailCustomUploadData.Upload();
    }
}

function BulkEmailUploadFileTypeChange(s, e) {
    var x = s.GetValue();
    //if (emailKeyValueType === 2)
    //{
        if (x === 1) {
            lblBulkEmailLetters.SetVisible(true);
            ComboBoxListProc_3.SetVisible(true);
            lblBulkEmailUploadFile.SetVisible(false);
            ucBulkEmailUploadData.SetVisible(false);
        }
        else if (x === 2) {
            lblBulkEmailLetters.SetVisible(false);
            ComboBoxListProc_3.SetVisible(false);
            ucBulkEmailUploadData.SetVisible(true);
            lblBulkEmailUploadFile.SetVisible(true);
        }
        else
        {
            lblBulkEmailLetters.SetVisible(false);
            ComboBoxListProc_3.SetVisible(false);
            ucBulkEmailUploadData.SetVisible(false);
            lblBulkEmailUploadFile.SetVisible(false);
        }
    //}
    
}

