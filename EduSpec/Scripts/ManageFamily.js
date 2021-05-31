var IsAddNewFamily;
var IsEditFromInfoForm;
var IsUseExemptionID;
var PrimaryParentParentID;
var SecondaryParentParentID;
var AlternativeParentParentID;
var IsEditFromButton = false;
var IsShowEditButton = true;
var FormFamilyID;

function onLearnerSaveBtnClick(grid) {
   
     grid.UpdateEdit();
}

function onFamilyManagementBeginCallback(s, e) {
    if (IsAddNewFamily === false) {
        e.customArgs['EditFamilyID'] = FormFamilyID;        
        e.customArgs['IsAddNewFamily'] = IsAddNewFamily;
        e.customArgs['IsUseExemptionID'] = IsUseExemptionID;
    }
    else {
        e.customArgs['EditFamilyID'] = null;
        e.customArgs['IsAddNewFamily'] = IsAddNewFamily;
        e.customArgs['IsUseExemptionID'] = IsUseExemptionID;
    }
}

function onLearnersManagementBeginCallback(s, e) {
    if (IsAddNewFamily === false) {
        e.customArgs['EditFamilyID'] = FormFamilyID;
        e.customArgs['EditFormFamilyID'] = FormFamilyID; 
        e.customArgs['IsAddNewFamily'] = IsAddNewFamily;
        e.customArgs['IsUseExemptionID'] = IsUseExemptionID;
    }
    else {
        e.customArgs['EditFamilyID'] = -1;
        e.customArgs['EditFormFamilyID'] = -1;
        e.customArgs['IsAddNewFamily'] = IsAddNewFamily;
        e.customArgs['IsUseExemptionID'] = IsUseExemptionID;
    }
}

function onbtnSaveFamily() {

    if (Edit_FamilyCode.GetIsValid() &&
        Edit_LanguageID.GetIsValid() &&
        Edit_MaritalStatusID.GetIsValid() &&
        Edit_PrimaryParentTitleID.GetIsValid() &&
        Edit_PrimaryParentInitials.GetIsValid() &&
        Edit_PrimaryParentFullName.GetIsValid() &&
        Edit_PrimaryParentSurname.GetIsValid() &&
        Edit_PrimaryParentIDNumber.GetIsValid() &&
        Edit_PrimaryParentRelationID.GetIsValid() &&
        Edit_PrimaryParentGenderID.GetIsValid() &&
        Edit_ExternalAccNo.GetIsValid()) {
        if (IsAddNewFamily === false) {
            UpdateFamily();
        }
        else {
            AddNewFamily();
        }
    }    
}

function UpdateFamily() {

    $.ajax({
        type: 'POST',
        url: '/Shared/UpdateFamily',
        dataType: 'json',
        async: false,
        data: {
            Edit_FamilyID: Edit_FamilyID.GetValue(),
            Edit_FamilyCode: Edit_FamilyCode.GetValue(),
            Edit_ExternalAccNo: Edit_ExternalAccNo.GetValue(),
            Edit_MaritalStatusID: Edit_MaritalStatusID.GetValue(),
            Edit_LanguageID: Edit_LanguageID.GetValue(),
            Edit_BillingCycleTypeID: Edit_BillingCycleTypeID.GetValue(),
            Edit_PrimaryParentParentID: Edit_PrimaryParentParentID.GetValue(),
            Edit_PrimaryParentRelationID: Edit_PrimaryParentRelationID.GetValue(),
            Edit_PrimaryParentTitleID: Edit_PrimaryParentTitleID.GetValue(),
            Edit_PrimaryParentInitials: Edit_PrimaryParentInitials.GetValue(),
            Edit_PrimaryParentFullName: Edit_PrimaryParentFullName.GetValue(),
            Edit_PrimaryParentSurname: Edit_PrimaryParentSurname.GetValue(),
            Edit_PrimaryParentIDNumber: Edit_PrimaryParentIDNumber.GetValue(),
            Edit_PrimaryParentTelHome: Edit_PrimaryParentTelHome.GetValue(),
            Edit_PrimaryParentTelWork: Edit_PrimaryParentTelWork.GetValue(),
            Edit_PrimaryParentTelCell: Edit_PrimaryParentTelCell.GetValue(),
            Edit_PrimaryParentTelFax: Edit_PrimaryParentTelFax.GetValue(),
            Edit_PrimaryParentTelAlternative: Edit_PrimaryParentTelAlternative.GetValue(),
            Edit_PrimaryParentEmployer: Edit_PrimaryParentEmployer.GetValue(),
            Edit_PrimaryParentOccupation: Edit_PrimaryParentOccupation.GetValue(),
            Edit_PrimaryParentEmail: Edit_PrimaryParentEmail.GetValue(),
            Edit_PrimaryParentEmailWork: Edit_PrimaryParentEmailWork.GetValue(),
            Edit_PrimaryParentStreetAddress1: Edit_PrimaryParentStreetAddress1.GetValue(),
            Edit_PrimaryParentStreetAddress2: Edit_PrimaryParentStreetAddress2.GetValue(),
            Edit_PrimaryParentStreetAddress3: Edit_PrimaryParentStreetAddress3.GetValue(),
            Edit_PrimaryParentStreetCode: Edit_PrimaryParentStreetCode.GetValue(),
            Edit_PrimaryParentPostalAddress1: Edit_PrimaryParentPostalAddress1.GetValue(),
            Edit_PrimaryParentPostalAddress2: Edit_PrimaryParentPostalAddress2.GetValue(),
            Edit_PrimaryParentPostalAddress3: Edit_PrimaryParentPostalAddress3.GetValue(),
            Edit_PrimaryParentPostalCode: Edit_PrimaryParentPostalCode.GetValue(),
            Edit_PrimaryParentGenderID: Edit_PrimaryParentGenderID.GetValue(),
            Edit_SecondaryParentParentID: Edit_SecondaryParentParentID.GetValue(),
            Edit_SecondaryParentRelationID: Edit_SecondaryParentRelationID.GetValue(),
            Edit_SecondaryParentTitleID: Edit_SecondaryParentTitleID.GetValue(),
            Edit_SecondaryParentInitials: Edit_SecondaryParentInitials.GetValue(),
            Edit_SecondaryParentFullName: Edit_SecondaryParentFullName.GetValue(),
            Edit_SecondaryParentSurname: Edit_SecondaryParentSurname.GetValue(),
            Edit_SecondaryParentIDNumber: Edit_SecondaryParentIDNumber.GetValue(),
            Edit_SecondaryParentTelHome: Edit_SecondaryParentTelHome.GetValue(),
            Edit_SecondaryParentTelWork: Edit_SecondaryParentTelWork.GetValue(),
            Edit_SecondaryParentTelCell: Edit_SecondaryParentTelCell.GetValue(),
            Edit_SecondaryParentTelAlternative: Edit_SecondaryParentTelAlternative.GetValue(),
            Edit_SecondaryParentTelFax: Edit_SecondaryParentTelFax.GetValue(),
            Edit_SecondaryParentEmployer: Edit_SecondaryParentEmployer.GetValue(),
            Edit_SecondaryParentOccupation: Edit_SecondaryParentOccupation.GetValue(),
            Edit_SecondaryParentEmail: Edit_SecondaryParentEmail.GetValue(),
            Edit_SecondaryParentEmailWork: Edit_SecondaryParentEmailWork.GetValue(),
            Edit_SecondaryParentStreetAddress1: Edit_SecondaryParentStreetAddress1.GetValue(),
            Edit_SecondaryParentStreetAddress2: Edit_SecondaryParentStreetAddress2.GetValue(),
            Edit_SecondaryParentStreetAddress3: Edit_SecondaryParentStreetAddress3.GetValue(),
            Edit_SecondaryParentStreetCode: Edit_SecondaryParentStreetCode.GetValue(),
            Edit_SecondaryParentPostalAddress1: Edit_SecondaryParentPostalAddress1.GetValue(),
            Edit_SecondaryParentPostalAddress2: Edit_SecondaryParentPostalAddress2.GetValue(),
            Edit_SecondaryParentPostalAddress3: Edit_SecondaryParentPostalAddress3.GetValue(),
            Edit_SecondaryParentPostalCode: Edit_SecondaryParentPostalCode.GetValue(),
            Edit_SecondaryParentGenderID: Edit_SecondaryParentGenderID.GetValue(),
            Edit_AlternativeParentParentID: Edit_AlternativeParentParentID.GetValue(),
            Edit_AlternativeParentRelationID: Edit_AlternativeParentRelationID.GetValue(),
            Edit_AlternativeParentTitleID: Edit_AlternativeParentTitleID.GetValue(),
            Edit_AlternativeParentInitials: Edit_AlternativeParentInitials.GetValue(),
            Edit_AlternativeParentFullName: Edit_AlternativeParentFullName.GetValue(),
            Edit_AlternativeParentSurname: Edit_AlternativeParentSurname.GetValue(),
            Edit_AlternativeParentIDNumber: Edit_AlternativeParentIDNumber.GetValue(),
            Edit_AlternativeParentTelHome: Edit_AlternativeParentTelHome.GetValue(),
            Edit_AlternativeParentTelWork: Edit_AlternativeParentTelWork.GetValue(),
            Edit_AlternativeParentTelCell: Edit_AlternativeParentTelCell.GetValue(),
            Edit_AlternativeParentTelFax: Edit_AlternativeParentTelFax.GetValue(),
            Edit_AlternativeParentTelAlternative: Edit_AlternativeParentTelAlternative.GetValue(),
            Edit_AlternativeParentEmployer: Edit_AlternativeParentEmployer.GetValue(),
            Edit_AlternativeParentOccupation: Edit_AlternativeParentOccupation.GetValue(),
            Edit_AlternativeParentEmail: Edit_AlternativeParentEmail.GetValue(),
            Edit_AlternativeParentEmailWork: Edit_AlternativeParentEmailWork.GetValue(),
            Edit_AlternativeParentStreetAddress1: Edit_AlternativeParentStreetAddress1.GetValue(),
            Edit_AlternativeParentStreetAddress2: Edit_AlternativeParentStreetAddress2.GetValue(),
            Edit_AlternativeParentStreetAddress3: Edit_AlternativeParentStreetAddress3.GetValue(),
            Edit_AlternativeParentStreetCode: Edit_AlternativeParentStreetCode.GetValue(),
            Edit_AlternativeParentPostalAddress1: Edit_AlternativeParentPostalAddress1.GetValue(),
            Edit_AlternativeParentPostalAddress2: Edit_AlternativeParentPostalAddress2.GetValue(),
            Edit_AlternativeParentPostalAddress3: Edit_AlternativeParentPostalAddress3.GetValue(),
            Edit_AlternativeParentPostalCode: Edit_AlternativeParentPostalCode.GetValue(),
            Edit_AlternativeParentGenderID: Edit_AlternativeParentGenderID.GetValue(),
            IsAddNewFamily: IsAddNewFamily
        },
        success: function (response) {
            if (IsEditFromButton === false) {
                if (IsEditFromInfoForm === true)
                    cbpFamilyInfo.PerformCallback();
                else
                    GridView.PerformCallback();
            }
            
            pcFamilyManagement.Hide();
        }
    });
}

function AddNewFamily() {

    $.ajax({
        type: 'POST',
        url: '/Shared/AddNewFamily',
        dataType: 'json',
        async: false,
        data: {
            Edit_FamilyCode: Edit_FamilyCode.GetValue(),
            Edit_ExternalAccNo: Edit_ExternalAccNo.GetValue(),
            Edit_MaritalStatusID: Edit_MaritalStatusID.GetValue(),
            Edit_LanguageID: Edit_LanguageID.GetValue(),
            Edit_PrimaryParentParentID: Edit_PrimaryParentParentID.GetValue(),
            Edit_PrimaryParentRelationID: Edit_PrimaryParentRelationID.GetValue(),
            Edit_PrimaryParentTitleID: Edit_PrimaryParentTitleID.GetValue(),
            Edit_PrimaryParentInitials: Edit_PrimaryParentInitials.GetValue(),
            Edit_PrimaryParentFullName: Edit_PrimaryParentFullName.GetValue(),
            Edit_PrimaryParentSurname: Edit_PrimaryParentSurname.GetValue(),
            Edit_PrimaryParentIDNumber: Edit_PrimaryParentIDNumber.GetValue(),
            Edit_PrimaryParentTelHome: Edit_PrimaryParentTelHome.GetValue(),
            Edit_PrimaryParentTelWork: Edit_PrimaryParentTelWork.GetValue(),
            Edit_PrimaryParentTelCell: Edit_PrimaryParentTelCell.GetValue(),
            Edit_PrimaryParentTelFax: Edit_PrimaryParentTelFax.GetValue(),
            Edit_PrimaryParentTelAlternative: Edit_PrimaryParentTelAlternative.GetValue(),
            Edit_PrimaryParentEmployer: Edit_PrimaryParentEmployer.GetValue(),
            Edit_PrimaryParentOccupation: Edit_PrimaryParentOccupation.GetValue(),
            Edit_PrimaryParentEmail: Edit_PrimaryParentEmail.GetValue(),
            Edit_PrimaryParentEmailWork: Edit_PrimaryParentEmailWork.GetValue(),
            Edit_PrimaryParentStreetAddress1: Edit_PrimaryParentStreetAddress1.GetValue(),
            Edit_PrimaryParentStreetAddress2: Edit_PrimaryParentStreetAddress2.GetValue(),
            Edit_PrimaryParentStreetAddress3: Edit_PrimaryParentStreetAddress3.GetValue(),
            Edit_PrimaryParentStreetCode: Edit_PrimaryParentStreetCode.GetValue(),
            Edit_PrimaryParentPostalAddress1: Edit_PrimaryParentPostalAddress1.GetValue(),
            Edit_PrimaryParentPostalAddress2: Edit_PrimaryParentPostalAddress2.GetValue(),
            Edit_PrimaryParentPostalAddress3: Edit_PrimaryParentPostalAddress3.GetValue(),
            Edit_PrimaryParentPostalCode: Edit_PrimaryParentPostalCode.GetValue(),
            Edit_PrimaryParentGenderID: Edit_PrimaryParentGenderID.GetValue(),
            Edit_SecondaryParentParentID: Edit_SecondaryParentParentID.GetValue(),
            Edit_SecondaryParentRelationID: Edit_SecondaryParentRelationID.GetValue(),
            Edit_SecondaryParentTitleID: Edit_SecondaryParentTitleID.GetValue(),
            Edit_SecondaryParentInitials: Edit_SecondaryParentInitials.GetValue(),
            Edit_SecondaryParentFullName: Edit_SecondaryParentFullName.GetValue(),
            Edit_SecondaryParentSurname: Edit_SecondaryParentSurname.GetValue(),
            Edit_SecondaryParentIDNumber: Edit_SecondaryParentIDNumber.GetValue(),
            Edit_SecondaryParentTelHome: Edit_SecondaryParentTelHome.GetValue(),
            Edit_SecondaryParentTelWork: Edit_SecondaryParentTelWork.GetValue(),
            Edit_SecondaryParentTelCell: Edit_SecondaryParentTelCell.GetValue(),
            Edit_SecondaryParentTelAlternative: Edit_SecondaryParentTelAlternative.GetValue(),
            Edit_SecondaryParentTelFax: Edit_SecondaryParentTelFax.GetValue(),
            Edit_SecondaryParentEmployer: Edit_SecondaryParentEmployer.GetValue(),
            Edit_SecondaryParentOccupation: Edit_SecondaryParentOccupation.GetValue(),
            Edit_SecondaryParentEmail: Edit_SecondaryParentEmail.GetValue(),
            Edit_SecondaryParentEmailWork: Edit_SecondaryParentEmailWork.GetValue(),
            Edit_SecondaryParentStreetAddress1: Edit_SecondaryParentStreetAddress1.GetValue(),
            Edit_SecondaryParentStreetAddress2: Edit_SecondaryParentStreetAddress2.GetValue(),
            Edit_SecondaryParentStreetAddress3: Edit_SecondaryParentStreetAddress3.GetValue(),
            Edit_SecondaryParentStreetCode: Edit_SecondaryParentStreetCode.GetValue(),
            Edit_SecondaryParentPostalAddress1: Edit_SecondaryParentPostalAddress1.GetValue(),
            Edit_SecondaryParentPostalAddress2: Edit_SecondaryParentPostalAddress2.GetValue(),
            Edit_SecondaryParentPostalAddress3: Edit_SecondaryParentPostalAddress3.GetValue(),
            Edit_SecondaryParentPostalCode: Edit_SecondaryParentPostalCode.GetValue(),
            Edit_SecondaryParentGenderID: Edit_SecondaryParentGenderID.GetValue(),
            Edit_AlternativeParentRelationID: Edit_AlternativeParentRelationID.GetValue(),
            Edit_AlternativeParentTitleID: Edit_AlternativeParentTitleID.GetValue(),
            Edit_AlternativeParentInitials: Edit_AlternativeParentInitials.GetValue(),
            Edit_AlternativeParentFullName: Edit_AlternativeParentFullName.GetValue(),
            Edit_AlternativeParentSurname: Edit_AlternativeParentSurname.GetValue(),
            Edit_AlternativeParentIDNumber: Edit_AlternativeParentIDNumber.GetValue(),
            Edit_AlternativeParentTelHome: Edit_AlternativeParentTelHome.GetValue(),
            Edit_AlternativeParentTelWork: Edit_AlternativeParentTelWork.GetValue(),
            Edit_AlternativeParentTelCell: Edit_AlternativeParentTelCell.GetValue(),
            Edit_AlternativeParentTelFax: Edit_AlternativeParentTelFax.GetValue(),
            Edit_AlternativeParentTelAlternative: Edit_AlternativeParentTelAlternative.GetValue(),
            Edit_AlternativeParentEmployer: Edit_AlternativeParentEmployer.GetValue(),
            Edit_AlternativeParentOccupation: Edit_AlternativeParentOccupation.GetValue(),
            Edit_AlternativeParentEmail: Edit_AlternativeParentEmail.GetValue(),
            Edit_AlternativeParentEmailWork: Edit_AlternativeParentEmailWork.GetValue(),
            Edit_AlternativeParentStreetAddress1: Edit_AlternativeParentStreetAddress1.GetValue(),
            Edit_AlternativeParentStreetAddress2: Edit_AlternativeParentStreetAddress2.GetValue(),
            Edit_AlternativeParentStreetAddress3: Edit_AlternativeParentStreetAddress3.GetValue(),
            Edit_AlternativeParentStreetCode: Edit_AlternativeParentStreetCode.GetValue(),
            Edit_AlternativeParentPostalAddress1: Edit_AlternativeParentPostalAddress1.GetValue(),
            Edit_AlternativeParentPostalAddress2: Edit_AlternativeParentPostalAddress2.GetValue(),
            Edit_AlternativeParentPostalAddress3: Edit_AlternativeParentPostalAddress3.GetValue(),
            Edit_AlternativeParentPostalCode: Edit_AlternativeParentPostalCode.GetValue(),
            Edit_AlternativeParentGenderID: Edit_AlternativeParentGenderID.GetValue(),
            IsAddNewFamily: IsAddNewFamily
        },
        success: function (response) {
            GridView.PerformCallback();
            IsAddNewFamily = false;
            pcFamilyManagement.Hide();
        }
    });
}

function SwitchPrimaryAndSecondaryParent(FamilyID) {

    $.ajax({
        type: 'POST',
        url: '/Shared/SwitchPrimaryAndSecondaryParent',
        dataType: 'json',
        async: false,
        data: {
            FamilyID: FamilyID
        },
        success: function (response) {
            cbpFamilyManagement.PerformCallback();
        }
    });
}

function SwithWithAlternativeParent(FamilyID, ActiveTabIndex) {

    $.ajax({
        type: 'POST',
        url: '/Shared/SwithWithAlternativeParent',
        dataType: 'json',
        async: false,
        data: {
            FamilyID: FamilyID,
            ActiveTabIndex: ActiveTabIndex
        },
        success: function (response) {
            cbpFamilyManagement.PerformCallback();
        }
    });
}

function onActiveTabChange(s, e) {

    switch (s.GetActiveTabIndex()) {
        case 0: {
            btnSwitchPrimaryAndSecondaryParent.SetEnabled(true);
            btnSetParentAsAlt.SetEnabled(true);
        }
            break;
        case 1: {
            btnSwitchPrimaryAndSecondaryParent.SetEnabled(true);
            btnSetParentAsAlt.SetEnabled(true);
        }
            break;
        case 2: {
            btnSwitchPrimaryAndSecondaryParent.SetEnabled(false);
            btnSetParentAsAlt.SetEnabled(false);
        }
            break;
        case 3: {
            btnSwitchPrimaryAndSecondaryParent.SetEnabled(false);
            btnSetParentAsAlt.SetEnabled(false);
        }
            break;
    }
}

function onFamilyManagementPopUp(s, e) {
    PrimaryParentParentID = Edit_PrimaryParentParentID.GetValue();
    SecondaryParentParentID = Edit_SecondaryParentParentID.GetValue();
    AlternativeParentParentID = Edit_AlternativeParentParentID.GetValue();
}

function ShowFamilyInfoForm(AddNewFamily, UseExemptionID, ShowEditButton, ID) {
    IsAddNewFamily = AddNewFamily;
    IsUseExemptionID = UseExemptionID;
    btnEditAccountInfoFromInfo.IsShowEditButton = ShowEditButton;
    FormFamilyID = ID;
    cbpFamilyInfo.PerformCallback();
    pcFamilyInfo.Show();
}

function ShowFamilyEditForm(AddNewFamily, EditFromInfoForm, UseExemptionID, ID) {
    IsAddNewFamily = AddNewFamily;
    IsEditFromInfoForm = EditFromInfoForm;
    IsUseExemptionID = UseExemptionID;
    FormFamilyID = ID;
    cbpFamilyManagement.PerformCallback();
    pcFamilyManagement.Show();
}
