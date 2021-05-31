function OnModelValidation(e, s, b) {
    if (e.value === null)
        e.isValid = false;
}

function OnPasswordValidation ( s, e ) {

    if (e.isValid)
        validateRequiredField(e.value, e);

    if (e.isValid)
        validatePassword(e.value, e);

}

function OnValidateUserName(s, e) {

    if (e.isValid)
        validateRequiredField(e.value, e);

    if (e.isValid)
        validateUserNameLength(e.value, e);

    if (e.isValid)
        validateUserName(e.value, e);
}


function OnValidateAdmissionNo(s, e, LearnerID) {

    if (e.isValid)
        validateAdmissionNo(e.value, e, LearnerID);

    if (e.isValid)
        validateRequiredField(e.value, e);

}

function OnValidateFamilyCode(s, e, FamilyID) {

    if (e.isValid)
        validateFamilyCode(e.value, e, FamilyID);

    if (e.isValid)
        validateRequiredField(e.value, e);
}
function OnValidateExternalAccNo(s, e, FamilyID) {

    if (e.isValid)
        validateExternalAccNo(e.value, e, FamilyID);
}

function OnRequiredFieldValidation (s, e) {
    if (e.isValid)
        validateRequiredField(e.value, e);
}

function OnRequiredFieldEmailValidation(s, e) {

    if (e.isValid)
        validateRequiredField(e.value, e);

    if (e.isValid)
        validateEmail(e.value, e);
}

function OnComparePasswords(s, e) {

    var Password1 = e.value;
    var Password2 = NewPassword.GetValue();

    if (e.isValid & !e.value === null)
        validatePasswordMatch(e, Password1, Password2);
}

function validateEmail(email,Editor) {
    var re = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/i;
    if (!re.test(email)) {
        Editor.isValid = false;
        Editor.errorText = "Email is invalid.";
        return false;
    }
    else {
        return true;
    }
    
}

function validateRequiredField(FieldValue,Editor) {
    if (FieldValue === null) {
        Editor.isValid = false;
        Editor.errorText = "Required.";

        return false;
    }
    else {
        return true;
    }
}

var LeftSchool = false;
function validateGradeAndClassField(FieldValue, Editor) {
    if (FieldValue === null) {
        Editor.isValid = false;
        Editor.errorText = "Required.";

        return false;
    }
    else {

        if (FieldValue == 99) {
            LeftSchool = true;
        }
        else {
            LeftSchool = false;
        }

        return true;
    }
}

var IsTerminated = false;
function validateTerminationSelectionField(FieldValue, Editor) {

    if (FieldValue == null) {
        IsTerminated = false;
    }
    else {
        IsTerminated = true;
    }

    return true;
    
}

function validateTerminationAndLeftSchoolField() {
    if ((IsTerminated === true) && (LeftSchool == false)) {
        pcErrorLeftSchoolNotSelected.Show();
        return false;
    }
    else if ((IsTerminated === false) && (LeftSchool == true)) {
        pcErrorTerminationNotSelected.Show();
        return false;
    }
    
    else {
        return true;
    }
    
}


//function validateTerminationField(FieldValue, Editor) {
//    if ((LeftSchool == true) && (FieldValue == null)) {
//        Editor.isValid = false;
//        Editor.errorText = "Required.";
//        return false;
//    }
//    else {
//        return true;
//    }
//}

function validateUserName(UserName, Editor) {

    $.ajax({
        type: 'POST',
        url: '/Account/CheckUsername',
        dataType: 'json',
        async: false,
        data: { userName: UserName },
        error: function () { alert("error"); },
        success: function (Data) {
            if (Data.result === true) {
                Editor.isValid = false;
                Editor.errorText = "User Already Exits.";
                return false;
            }
            else {
                return true;
            }
        }
    });
}

function validateAdmissionNo(AdmissionNo, Editor, LearnerID) {

    $.ajax({
        type: 'POST',
        url: '/Shared/ValidateAdmissionNo',
        dataType: 'json',
        async: false,
        data: {
            AdmissionNo: AdmissionNo,
            LearnerID: LearnerID
        },
        error: function () { alert("error"); },
        success: function (Data) {
            if (Data.result === true) {
                Editor.isValid = false;
                Editor.errorText = "Admission number already Exits.";
                return false;
            }
            else {
                return true;
            }
        }
    });
}

function validateFamilyCode(FamilyCode, Editor, FamilyID) {
    $.ajax({
        type: 'POST',
        url: '/Shared/ValidateFamilyCode',
        dataType: 'json',
        async: false,
        data: {
            FamilyID: FamilyID,
            FamilyCode: FamilyCode
        },
        error: function () { alert("error"); },
        success: function (Data) {
            if (Data.result === true) {
                Editor.isValid = false;
                Editor.errorText = "Family Code already Exits.";
                return false;
            }
            else {
                return true;
            }
        }
    });
}

function validateExternalAccNo(ExternalAccNo, Editor, FamilyID) {
    $.ajax({
        type: 'POST',
        url: '/Shared/ValidateExternalAccNo',
        dataType: 'json',
        async: false,
        data: {
            FamilyID: FamilyID,
            ExternalAccNo: ExternalAccNo
        },
        error: function () { alert("error"); },
        success: function (Data) {
            if (Data.result === true) {
                Editor.isValid = false;
                Editor.errorText = "Account number already exits.";
                return false;
            }
            else {
                return true;
            }
        }
    });
}

function validateUserNameLength(UserName, Editor) {

    var name = String(UserName);
    if (name.length > 56) {
        Editor.isValid = false;
        Editor.errorText = "Must be under 56 characters.";
        return false;
    }
    else
        return true;
}

function validatePassword(Password, Editor) {

    var pword = String(Password);
    if (pword.length < 5 || pword.length > 30) {
        Editor.isValid = false;
        Editor.errorText = "Password must be betreen 5 and 30 characters.";
        return false;
    }
    else
        return true;
}

function validatePasswordMatch(Editor, Password1, Password2) {

    if (Password1.localeCompare(Password2) === 0) {
        return true;
    }
    else {
        Editor.isValid = false;
        Editor.errorText = "The Passwords do not match.";
        return false;
    }
}

function validateQuantity(Quantity, Editor) {

    if (parseInt(Quantity) < 1) {
        Editor.isValid = false;
        Editor.errorText = "Must greater than 0.";
        return false;
    }
    else { return true; }
}