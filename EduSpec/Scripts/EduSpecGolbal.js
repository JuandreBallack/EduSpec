function FindApplication(ApplicationNumber){

    $.ajax({
        type: 'POST',
        url: '/Exemptions/FindApplication',
        dataType: 'json',
        async: false,
        data: {
            ApplicationNumber: ApplicationNumber
        },
        success: function (result) {
            if (result.ExemptionID > 0) {
                cbpSubmitApplicationInfo.PerformCallback();
                if (result.Screen === 'Process') {
                    cbpApplicationCalculation.PerformCallback({ ExemptionID: result.ExemptionID, IsCalculation: false });
                    IncomeParent1.SetFocus();
                }
            }
            else
                pcWarning.Show();
        }
    });
       
}

function onBtnWarningClick(Result) {
    if (Result === "mrOk") {
        cbpSubmitApplicationInfo.PerformCallback();
    }
}

function onKeyPress(s, e) {

    if (e.htmlEvent.keyCode === 17 || e.htmlEvent.keyCode === 74 || e.htmlEvent.keyCode === 13) {
        ASPxClientUtils.PreventEventAndBubble(e.htmlEvent);
    }

    if (tbApplicationNumber.GetText().length >= 10)
        tbApplicationNumber.SetText(tbApplicationNumber.GetText().substring(0, 10));

    if (e.htmlEvent.keyCode === 13)
        FindApplication(tbApplicationNumber.GetText());
}

function ChangePassword(NewPassword, ConfirmPassword, userID) { 

    var str = userID;
    var res = userID.substring(5, userID.length);

    $.ajax({
        type: 'POST',
        url: '/Account/ChangePassword',
        dataType: 'json',
        async: false,
        data: {
            userID: userID.substring(5, userID.length),
            NewPassword: NewPassword,
            ConfirmPassword: ConfirmPassword
        },
        success: function (Data) {
            if (Data.result) {
                pcConfirmPaswordChange.Show();
                //pcChangePassword.Hide();
                //alert("Password was Changed.");
            }
        }
    });

}

function SetProcessButtons(s, e) {
    GridView.SetHeight(120);
    if (s.pageRowCount > 0) {
        SettingsMenuButton(TopMenu.GetItemByName("Btn_Submit"), true, "ConfirmSmall.png");
        SettingsMenuButton(TopMenu.GetItemByName("Btn_Calculate"), true, "CalculatorSmall.png");
        SettingsMenuButton(TopMenu.GetItemByName("Btn_Save"), true, "SaveSmall.png");
        SettingsMenuButton(TopMenu.GetItemByName("Btn_Process"), true, "SubmitSmall.png");
        SettingsMenuButton(TopMenu.GetItemByName("Btn_Finalise"), true, "FinaliseSmall.png");
    }
    else {
        SettingsMenuButton(TopMenu.GetItemByName("Btn_Submit"), false, "ConfirmSmallGrey.png");
        SettingsMenuButton(TopMenu.GetItemByName("Btn_Calculate"), false, "CalculatorSmallGrey.png");
        SettingsMenuButton(TopMenu.GetItemByName("Btn_Save"), false, "SaveSmallGrey.png");
        SettingsMenuButton(TopMenu.GetItemByName("Btn_Process"), false, "SubmitSmallGrey.png");
        SettingsMenuButton(TopMenu.GetItemByName("Btn_Finalise"), false, "FinaliseSmallGrey.png");
    }
}

function SetDateEditFirstDayNextMonth(s, e) {
    var current;
    var now = new Date();
    if (now.getMonth() === 11) {
        current = new Date(now.getFullYear() + 1, 0, 1);
    } else {
        current = new Date(now.getFullYear(), now.getMonth() + 1, 1);
    }    
    s.SetValue(current);
}

function SetDateEditFirstDayDecember(s, e) {
    var now = new Date();
    var current = new Date(now.getFullYear(), 11, 1);
    s.SetValue(current);
}

