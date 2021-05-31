using EduSpec.Models;
using System;
using System.Collections.Generic;
using System.Linq;


namespace EduSpec.Controllers
{
    [SessionExpire]
    public class ViewProperties
    {
        public IList<ViewID_ViewResult> ViewID { get; set; }
        public IList<ViewDisplayProperties_ViewResult> ViewDisplayPropeties { get; set; }
        public IEnumerable<ViewGridColumns_ViewResult> ViewGridColumns { get; set; }
        public IEnumerable<MenuButtons_ViewResult> MenuButtons { get; set; }
        public IEnumerable<DefaultButtons_ViewResult> DefaultButtons { get; set; }
        public IEnumerable<ViewFieldGroups_ViewResult> ViewFieldGroups { get; set; }

        public static ViewProperties viewProperties(string ViewName)
        {
            return getviewProperties(ViewName, -1, false, "");
        }

        public static ViewProperties viewProperties(string ViewName, int UserID)
        {
            return getviewProperties(ViewName, UserID, false, "");
        }

        public static ViewProperties viewProperties(string ViewName, int UserID, bool IsXlsExport)
        {
            return getviewProperties(ViewName, UserID, IsXlsExport, "");
        }

        //public static ViewProperties viewProperties(string ViewName, int UserID, bool IsXlsExport)
        //{
        //    return getviewProperties(ViewName, UserID, IsXlsExport, "");
        //}

        //public static ViewProperties viewProperties(string ViewName, int UserID, bool IsXlsExport, string GridCaptionParameters)
        //{
        //    return getviewProperties(ViewName, UserID, IsXlsExport, GridCaptionParameters);
        //}

        public static ViewProperties viewProperties(string ViewName, int UserID, string GridCaptionParameters)
        {
            return getviewProperties(ViewName, UserID, false, GridCaptionParameters);
        }
        public static ViewProperties getviewProperties(string ViewName, int UserID, bool IsXlsExport, string GridCaptionParameters)
        {
            IList<ViewID_ViewResult> viewID;
            IList<ViewDisplayProperties_ViewResult> viewDisplayPropeties;
            IEnumerable<ViewGridColumns_ViewResult> viewGridColumns;
            IEnumerable<MenuButtons_ViewResult> menuButtons;
            IEnumerable<DefaultButtons_ViewResult> defaultButtons;
            IEnumerable<ViewFieldGroups_ViewResult> viewFieldGroups;

            if (UserUtils.CurrentUser().ImpersonationUserID != -1)
                UserID = UserUtils.CurrentUser().ImpersonationUserID;

            using (var Context = new EduSpecDataContext())
            {
                var results = Context.getViewProperties(ViewName, UserID, IsXlsExport, GridCaptionParameters);
                viewID = results.GetResult<ViewID_ViewResult>().ToList();
                viewDisplayPropeties = results.GetResult<ViewDisplayProperties_ViewResult>().ToList();
                viewGridColumns = results.GetResult<ViewGridColumns_ViewResult>().ToList();
                menuButtons = results.GetResult<MenuButtons_ViewResult>().ToList();
                defaultButtons = results.GetResult<DefaultButtons_ViewResult>().ToList();
                viewFieldGroups = results.GetResult<ViewFieldGroups_ViewResult>().ToList();
            }
            return new ViewProperties { ViewID = viewID,
                                        ViewDisplayPropeties = viewDisplayPropeties,
                                        ViewGridColumns = viewGridColumns,
                                        MenuButtons = menuButtons,
                                        DefaultButtons = defaultButtons,
                                        ViewFieldGroups = viewFieldGroups };
        }
    }
}

