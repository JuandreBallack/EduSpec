using DevExpress.Web;
using DevExpress.Web.Mvc;
using DevExpress.Web.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace EduSpec
{
    public enum BtnType
    {
        mbOK,
        mbCancel,
        mbYes,
        mbNo
    };
    public enum MessageType
    {
        mConfirmation,
        mWarning,
        mError,
        mInformation
    };

    internal class ButtonConfig
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public string ModelResult { get; set; }
    }


    public class MessageDlg
    {
        private static Dictionary<BtnType, ButtonConfig> ButtonsSettings = new Dictionary<BtnType, ButtonConfig>()
        {
            { BtnType.mbOK, new ButtonConfig { Name = "btnOK", Text = "OK", ModelResult = "mrOk" } },
            { BtnType.mbCancel, new ButtonConfig { Name = "btnCancelBtn", Text = "Cancel", ModelResult = "mrCancel" } },
            { BtnType.mbYes, new ButtonConfig { Name = "btnYes", Text = "Yes", ModelResult = "mrYes" } },
            { BtnType.mbNo, new ButtonConfig { Name = "btnNo", Text = "No", ModelResult = "mrNo" } },
        };

        public static MVCxPopupControl ShowMessageDlg(string PopupName, HtmlHelper htmlHelper, string _Message, BtnType[] _ButtonType, MessageType _MessageType)
        {
            return RenderPopup(PopupName, htmlHelper, _Message, _ButtonType, _MessageType, string.Empty);
        }

        public static MVCxPopupControl ShowMessageDlg(string PopupName, HtmlHelper htmlHelper, string _Message, BtnType[] _ButtonType, MessageType _MessageType, string onClick)
        {
            return RenderPopup(PopupName, htmlHelper, _Message, _ButtonType, _MessageType, onClick);
        }

        public static MVCxPopupControl ShowImputDlg (string PopupName, string Header, string InfoLableText, BtnType[] _ButtonType, HtmlHelper htmlHelper, string onClick)
        {
            return RenderInputDlg(PopupName, Header, InfoLableText, _ButtonType, htmlHelper, onClick);
        }
        private static MVCxPopupControl RenderPopup(string PopupName, HtmlHelper htmlHelper, string _Message, BtnType[] _ButtonType, MessageType _MessageType, string onClick)
        {
            htmlHelper.DevExpress().PopupControl(settings =>
                {
                    settings.Name = PopupName;
                    switch (_MessageType)
                    {
                        case MessageType.mConfirmation:
                            settings.HeaderText = "Confirmation";
                            break;
                        case MessageType.mWarning:
                            settings.HeaderText = "Warning";
                            break;
                        case MessageType.mError:
                            settings.HeaderText = "Error";
                            break;
                        case MessageType.mInformation:
                            settings.HeaderText = "Information";
                            break;
                    }

                    settings.AllowDragging = true;
                    settings.CloseAction = CloseAction.CloseButton;
                    settings.PopupAnimationType = AnimationType.Fade;
                    settings.Height = Unit.Pixel(130);
                    settings.Modal = true;
                    settings.ShowFooter = true;
                    settings.PopupHorizontalAlign = PopupHorizontalAlign.WindowCenter;
                    settings.PopupVerticalAlign = PopupVerticalAlign.WindowCenter;
                    settings.SetContent(() =>
                        {
                            htmlHelper.ViewContext.Writer.Write("<div style=\"width:300px; vertical-align:middle\">");
                            htmlHelper.DevExpress().Label(lbsettings =>
                                {
                                    lbsettings.Name = PopupName + "lbMessage";
                                    lbsettings.Text = _Message;
                                    lbsettings.PreRender = (sender, e) =>
                                        {
                                            var lb = (ASPxLabel)sender;
                                            lb.ClientInstanceName = PopupName + "lbMessage";
                                        };
                                }).GetHtml();
                            
                            htmlHelper.ViewContext.Writer.Write("</div>");
                        });

                    settings.SetFooterTemplateContent(c =>
                        {

                            htmlHelper.ViewContext.Writer.Write("<div style=\"overflow: hidden\"><div style=\"padding: 10px; float: right;\">");

                            foreach (BtnType button in _ButtonType)
                            {
                                var Button = ButtonsSettings[button];
                                htmlHelper.DevExpress().Button(buttonSettings =>
                                    {
                                        buttonSettings.Name = PopupName + Button.Name;
                                        buttonSettings.ControlStyle.CssClass = "button";
                                        buttonSettings.Width = 80;
                                        buttonSettings.Text = Button.Text;
                                        if (onClick == string.Empty)
                                        {
                                            buttonSettings.ClientSideEvents.Click = String.Format("function(s, e) {{{0}.Hide();}}", PopupName);
                                        }
                                        else
                                        {
                                            buttonSettings.ClientSideEvents.Click = String.Format("function(s, e) {{" + onClick + "(\"{0}\"); " + PopupName + ".Hide();}}", Button.ModelResult);
                                        }
                                    }).Render();
                            }

                            htmlHelper.ViewContext.Writer.Write("</div></div>");
                        });

                }).GetHtml();

            return null;
        }
        private static MVCxPopupControl RenderInputDlg(string PopupName, string Header, string InfoLableText, BtnType[] _ButtonType, HtmlHelper htmlHelper, string onClick)
        {
            htmlHelper.DevExpress().PopupControl(settings =>
            {
                settings.Name = PopupName;
                settings.AllowDragging = true;
                settings.CloseAction = CloseAction.CloseButton;
                settings.PopupAnimationType = AnimationType.Fade;
                settings.Height = Unit.Pixel(130);
                settings.Modal = true;
                settings.ShowFooter = true;
                settings.PopupHorizontalAlign = PopupHorizontalAlign.WindowCenter;
                settings.PopupVerticalAlign = PopupVerticalAlign.WindowCenter;
                settings.ClientSideEvents.Shown = "function(s,e){" + PopupName + "tbInput.SetFocus()}";
                settings.HeaderText = Header;

                settings.SetContent(() =>
                {
                    htmlHelper.ViewContext.Writer.Write("<div style=\"width:300px; vertical-align:middle\">");
                    htmlHelper.DevExpress().Label(lbsettings =>
                    {
                        lbsettings.Name = PopupName + "lbInfo";
                        lbsettings.ControlStyle.CssClass = "boldlabel";
                        lbsettings.Text = InfoLableText;
                        lbsettings.PreRender = (sender, e) =>
                        {
                            var lb = (ASPxLabel)sender;
                            lb.ClientInstanceName = PopupName + "lbInfo";
                        };
                    }).GetHtml();

                    htmlHelper.DevExpress().TextBox(tbsettings =>
                    {
                        tbsettings.Name = PopupName + "tbInput";
                        tbsettings.ControlStyle.CssClass = "formeditor";
                        tbsettings.Width = 300;
                    }).GetHtml();

                    htmlHelper.ViewContext.Writer.Write("</div>");
                });

                settings.SetFooterTemplateContent(c =>
                {

                    htmlHelper.ViewContext.Writer.Write("<div style=\"overflow: hidden\"><div style=\"padding: 10px; float: right;\">");

                    foreach (BtnType button in _ButtonType)
                    {
                        var Button = ButtonsSettings[button];
                        htmlHelper.DevExpress().Button(buttonSettings =>
                        {
                            buttonSettings.Name = PopupName + Button.Name;
                            buttonSettings.ControlStyle.CssClass = "button";
                            buttonSettings.Width = 80;
                            buttonSettings.Text = Button.Text;
                            if (onClick == string.Empty)
                            {
                                buttonSettings.ClientSideEvents.Click = String.Format("function(s, e) {{{0}.Hide();}}", PopupName);
                            }
                            else
                            {
                                buttonSettings.ClientSideEvents.Click = String.Format("function(s, e) {{" + onClick + "(\"{0}\"); " + PopupName + ".Hide();}}", Button.ModelResult);
                            }
                        }).Render();
                    }

                    htmlHelper.ViewContext.Writer.Write("</div></div>");
                });

            }).GetHtml();

            return null;
        }
    }
}
