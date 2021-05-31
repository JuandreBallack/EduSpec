using DevExpress.Web.Mvc;
using EduSpec.Controllers;
using EduSpec.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EduSpec
{
    public class MenuButtonsUtils
    {
        public static MenuSettings SetMenuProperties(MenuSettings settings, object _ViewProperties)
        {
            var viewProps = (ViewProperties)_ViewProperties;
            var ButtonProperties = (IList<MenuButtons_ViewResult>)viewProps.MenuButtons.ToList();

            var Buttons = ButtonProperties.FirstOrDefault();

            settings.Name = "TopMenu";

            if (Buttons != null)
            {
                if (Buttons.ButtonFunction != null)
                {
                    settings.ClientSideEvents.ItemClick = String.Format(Buttons.ButtonFunction, Buttons.ButtonFunctionParameters);
                }
            }
            var LastMenuItem = -1;

            foreach (var BtnProperty in ButtonProperties)
            {
                if (BtnProperty.IsSubMenu == false)
                {
                    LastMenuItem = LastMenuItem + 1;
                    settings.Items.Add(item =>
                        {
                            item.Name = BtnProperty.ButtonName;
                            item.Text = BtnProperty.ButtonCaption;
                            item.ToolTip = BtnProperty.ToolTip;
                            item.Image.Url = "~/Content/MenuButtons/" + BtnProperty.ButtonImage;
                            if (BtnProperty.ButtonAction != null)
                            {
                                item.NavigateUrl = DevExpressHelper.GetUrl(new
                                { Controller = BtnProperty.Controller, Action = BtnProperty.ButtonAction,  target="_blank" });
                            }
                            if (BtnProperty.IsOpenInNewWindow == true)
                                item.Target = "_blank";
                            item.ItemStyle.Width = (int)BtnProperty.ButtonWidth;
                            item.ItemStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                            item.ItemStyle.VerticalAlign = System.Web.UI.WebControls.VerticalAlign.Middle;                            
                        });
                }
                else
                {
                    var menu = (MVCxMenuItem)settings.Items[LastMenuItem];
                    menu.Items.Add(subitem =>
                        {
                            subitem.Name = BtnProperty.ButtonName;
                            subitem.Text = BtnProperty.ButtonCaption;

                        });
                }
            }

            return settings;
        }
    }
}
