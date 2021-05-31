using DevExpress.Web;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EduSpec
{
    public static class PopupUtils
    {
        public static PopupControlSettings SetPopupViewProperties(PopupControlSettings settings, string PopupName, string HeaderText)
        {
            settings.Name = PopupName;
            settings.AllowDragging = true;
            settings.CloseAction = CloseAction.CloseButton;
            settings.PopupAnimationType = AnimationType.Fade;
            settings.HeaderText = HeaderText;
            settings.Modal = true;
            settings.PopupHorizontalAlign = PopupHorizontalAlign.WindowCenter;
            settings.PopupVerticalAlign = PopupVerticalAlign.WindowCenter;
            //settings.ClientSideEvents.CloseUp = "function(s, e){ ASPxClientEdit.ClearEditorsInContainer(null, '', true); }";

            return settings;
        }
    }
}
