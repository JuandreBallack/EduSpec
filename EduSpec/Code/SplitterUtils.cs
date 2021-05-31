using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.Web.Mvc;
using System.Web.UI.WebControls;
using DevExpress.Utils;
using EduSpec.Models;
using EduSpec.Controllers;

namespace EduSpec
{
    public class SplitterUtils
    {
        public class SplitterLayout
        {
            public string GridName { get; set; }
            public string DetailGridName { get; set; }
            public bool ViewHeader { get; set; }
            public int HeaderHeight { get; set; }
            public bool ViewFooter { get; set; }
            public int FooterHeight { get; set; }
        }

        public static SplitterSettings SetSplitterProperties(SplitterSettings settings, bool ViewHeader, int HeaderHeight, bool ViewFooter, int FooterHeight)
        {
            var Layout = new SplitterLayout() { GridName = null, ViewHeader = ViewHeader, HeaderHeight = HeaderHeight, ViewFooter = ViewFooter, FooterHeight = FooterHeight };
            return getSplitterProperties(settings, Layout);
        }

        public static SplitterSettings SetSplitterProperties(SplitterSettings settings, object _ViewProperties)
        {
            var viewProps = (ViewProperties)_ViewProperties;
            IList<ViewDisplayProperties_ViewResult> _ViewDisplayProperties = (IList<ViewDisplayProperties_ViewResult>)viewProps.ViewDisplayPropeties.ToList();

            using (var Context = new EduSpecDataContext())
            {
                var ViewDisplayProperties = _ViewDisplayProperties.FirstOrDefault();
                var Layout = new SplitterLayout()
                {
                    GridName = ViewDisplayProperties.GridName,
                    DetailGridName = ViewDisplayProperties.DetailGridName,
                    ViewHeader = Convert.ToBoolean(ViewDisplayProperties.IsViewHeaderVisible),
                    HeaderHeight = Convert.ToInt32(ViewDisplayProperties.ViewHeaderHeight),
                    ViewFooter = Convert.ToBoolean(ViewDisplayProperties.IsViewFooterVisible),
                    FooterHeight = Convert.ToInt32(ViewDisplayProperties.ViewFooterHeight)
                };
                return getSplitterProperties(settings, Layout);
            }

        }

        public static SplitterSettings getSplitterProperties(SplitterSettings settings, SplitterLayout Layout)
        {
            settings.Name = "ViewSplitter";
            settings.Orientation = Orientation.Vertical;
            settings.FullscreenMode = false;
            settings.Height = Unit.Percentage(100);
            settings.Styles.Pane.Paddings.Padding = 2;
            settings.Styles.Pane.Border.BorderStyle = BorderStyle.None;            
            if (Layout.GridName != null)
                if (Layout.DetailGridName != null)
                {
                    settings.ClientSideEvents.PaneResized = String.Format("function(s, e) {{ if(e.pane.name == 'ViewMainContentPane') {0}.SetHeight(e.pane.GetClientHeight() - 2); if(e.pane.name == 'footerPane') {1}.SetHeight(e.pane.GetClientHeight() - 2); }}", Layout.GridName, Layout.DetailGridName);
                }
                else
                {
                    settings.ClientSideEvents.PaneResized = String.Format("function(s, e) {{ if(e.pane.name == 'ViewMainContentPane') {0}.SetHeight(e.pane.GetClientHeight()); }}", Layout.GridName);
                }


            settings.Panes.Add(headerPane =>
            {
                headerPane.Name = "headerPane";
                headerPane.Size = Convert.ToInt32(Layout.HeaderHeight);
                headerPane.Visible = Convert.ToBoolean(Layout.ViewHeader);
                //headerPane.ShowCollapseForwardButton = DefaultBoolean.True;
            });

            settings.Panes.Add(mainContentPane =>
            {
                mainContentPane.Name = "ViewMainContentPane";
                mainContentPane.Separator.Visible = DefaultBoolean.False;
            });

            settings.Panes.Add(footerPane =>
            {
                footerPane.Name = "footerPane";
                footerPane.Size = Convert.ToInt32(Layout.FooterHeight);
                footerPane.Visible = Convert.ToBoolean(Layout.ViewFooter);
                footerPane.PaneStyle.Paddings.PaddingBottom = 2;
                footerPane.Separator.Visible = DefaultBoolean.False;
                footerPane.ShowCollapseForwardButton = DefaultBoolean.True;
            });
            return settings;

        }
    }
}