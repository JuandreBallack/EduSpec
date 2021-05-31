using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using DevExpress.Web.Mvc.UI;
using DevExpress.Web.Mvc;


namespace CustomUtils
{    
    public static class EditorUtils
    {        
        public enum ButtonClientSideEvent
        {
            onClick,
            onInit
        };
        public enum TextBoxClientSideEvent
        {
            onGotFocus,
            onInit,
            onKeyDown,
            onKeyPress,
            onKeyUp,
            onLostFocus,
            onTextChanged,
            onUserInput,
            onValidation,
            onValueChanged
        };
        public enum DateEditClientSideEvent
        {
            onDateChanged,
            onInit
        };
        public enum SpinEditClientSideEvent
        {
            onInit,
            onKeyPress
        };
        public enum ComboboxClientSideEvent
        {
            onBeginCallBack,
            onEndCallBack,
            onInit,
            onSelectedIndexChanged
        };
        public enum CheckBoxClientSideEvent
        {
            onInit,
            onCheckedChanged
        }

        #region Devexpress Label
        public static void Label(HtmlHelper htmlHelper, string CssClass, string Text)
        {
            LabelSettings lbSettings = new LabelSettings
            {
                Text = Text
            };
            lbSettings.ControlStyle.CssClass = CssClass;
            DevExpressLabel(htmlHelper, lbSettings, null, null);
        }
        
        public static void Label(HtmlHelper htmlHelper, string CssClass, string Text, int Width)
        {
            LabelSettings lbSettings = new LabelSettings
            {
                Text = Text,
                Width = Width
            };
            lbSettings.ControlStyle.CssClass = CssClass;
            DevExpressLabel(htmlHelper, lbSettings, null, null);
        }
        public static HtmlString Label(HtmlHelper htmlHelper, string CssClass, Object Model, String DataField)
        {
            LabelSettings lbSettings = new LabelSettings
            {
                Name = DataField
            };
            lbSettings.ControlStyle.CssClass = CssClass;
            return DevExpressLabel(htmlHelper, lbSettings, Model, DataField);
        }
        public static void Label(HtmlHelper htmlHelper, string CssClass, Object Model, String DataField, int Width)
        {
            LabelSettings lbSettings = new LabelSettings
            {
                Name = DataField,
                Width = Width
            };
            lbSettings.ControlStyle.CssClass = CssClass;
            DevExpressLabel(htmlHelper, lbSettings, Model, DataField);
        }
        private static HtmlString DevExpressLabel(HtmlHelper htmlHelper, LabelSettings lbSettings, Object Model, String DataField)
        {
            if (Model != null)
            {
               return htmlHelper.DevExpress().Label(lbSettings).Bind(DataBinder.Eval(Model, DataField)).GetHtml();
            }
            else
            {
               return htmlHelper.DevExpress().Label(lbSettings).GetHtml();
            }            
        }

        #endregion

        #region Devexpress TextBox
        public static void TextBox(HtmlHelper htmlHelper, string Name, string CssClass, bool ClientVisable, int Width, Dictionary<TextBoxClientSideEvent, string> clientEvents)
        {
            TextBoxSettings textBoxSettings = new TextBoxSettings
            {
                Name = Name,
                Width = Width,
                ClientVisible = ClientVisable
            };
            textBoxSettings.ControlStyle.CssClass = CssClass;
            DevExpressTextBox(htmlHelper, textBoxSettings, null, null, clientEvents);
        }
        public static void TextBox(HtmlHelper htmlHelper, string CssClass, Object Model, string DataField, bool ClientVisable, int Width, Dictionary<TextBoxClientSideEvent, string> clientEvents)
        {
            TextBoxSettings textBoxSettings = new TextBoxSettings
            {
                Name = DataField,
                Width = Width,
                ClientVisible = ClientVisable
            };
            textBoxSettings.ControlStyle.CssClass = CssClass;
            DevExpressTextBox(htmlHelper, textBoxSettings, Model, DataField, clientEvents);
        }
        private static void DevExpressTextBox(HtmlHelper htmlHelper, TextBoxSettings textBoxSettings, Object Model, string DataField, Dictionary<TextBoxClientSideEvent, string> clientEvents)
        {
            if (clientEvents != null)
            {
                foreach (KeyValuePair<TextBoxClientSideEvent, string> Event in clientEvents)
                {
                    switch (Event.Key)
                    {
                        case TextBoxClientSideEvent.onGotFocus:
                            textBoxSettings.Properties.ClientSideEvents.GotFocus = Event.Value;
                            break;
                        case TextBoxClientSideEvent.onInit:
                            textBoxSettings.Properties.ClientSideEvents.Init = Event.Value;
                            break;
                        case TextBoxClientSideEvent.onKeyDown:
                            textBoxSettings.Properties.ClientSideEvents.KeyDown = Event.Value;
                            break;
                        case TextBoxClientSideEvent.onKeyPress:
                            textBoxSettings.Properties.ClientSideEvents.KeyPress = Event.Value;
                            break;
                        case TextBoxClientSideEvent.onKeyUp:
                            textBoxSettings.Properties.ClientSideEvents.KeyUp = Event.Value;
                            break;
                        case TextBoxClientSideEvent.onLostFocus:
                            textBoxSettings.Properties.ClientSideEvents.LostFocus = Event.Value;
                            break;
                        case TextBoxClientSideEvent.onTextChanged:
                            textBoxSettings.Properties.ClientSideEvents.TextChanged = Event.Value;
                            break;
                        case TextBoxClientSideEvent.onUserInput:
                            textBoxSettings.Properties.ClientSideEvents.UserInput = Event.Value;
                            break;
                        case TextBoxClientSideEvent.onValidation:
                            textBoxSettings.Properties.ClientSideEvents.Validation = Event.Value;
                            break;
                        case TextBoxClientSideEvent.onValueChanged:
                            textBoxSettings.Properties.ClientSideEvents.ValueChanged = Event.Value;
                            break;
                    }
                }
            }

            if (Model != null)
            {
                htmlHelper.DevExpress().TextBox(textBoxSettings).Bind(DataBinder.Eval(Model, DataField)).GetHtml();
            }
            else
            {
                htmlHelper.DevExpress().TextBox(textBoxSettings).GetHtml();
            }
        }
        #endregion
    }

    public static class CustomUtils
    {
        #region Html Table
        public static void BeginTable(HtmlHelper htmlHelper)
        {
            htmlHelper.ViewContext.Writer.Write("<table>");            
        }
        public static void BeginTable(HtmlHelper htmlHelper, string CssClass)
        {
            htmlHelper.ViewContext.Writer.Write(string.Format("<table class=\"{0}\">", CssClass));
        }
        public static void EndTable(HtmlHelper htmlHelper)
        {
            htmlHelper.ViewContext.Writer.Write("</table>");
        }

        #endregion
        #region Html Row
        public static void NewRow(HtmlHelper htmlHelper)
        {            
            htmlHelper.ViewContext.Writer.Write("<tr>");
        }
        public static void EndRow(HtmlHelper htmlHelper)
        {
            htmlHelper.ViewContext.Writer.Write("</tr>");
        }
        public static void EndAndNewRow(HtmlHelper htmlHelper)
        {
            htmlHelper.ViewContext.Writer.Write("</tr><tr>");
        }
        #endregion

        #region Html Column Header
        public static void NewColumnHeader(HtmlHelper htmlHelper, string Text)
        {
            htmlHelper.ViewContext.Writer.Write(string.Format("<th>\"><h3><strong>{0}</strong></h3></th>", Text));
        }
        public static void NewColumnHeader(HtmlHelper htmlHelper, string Text, string CssClass, int Width)
        {
            htmlHelper.ViewContext.Writer.Write(string.Format("<th class=\"{0}\"style=\"width:{1}px;\"><h3><strong>{2}</strong></h3></th>", CssClass, Width.ToString(), Text));
        }
        #endregion

        #region Html Column
        public static void NewColumn(HtmlHelper htmlHelper)
        {
            htmlHelper.ViewContext.Writer.Write("<td >");
        }
        public static void NewColumn(HtmlHelper htmlHelper, string CssClass, int Width)
        {
            htmlHelper.ViewContext.Writer.Write(string.Format("<td class=\"{0}\"style=\"width:{1}px; vertical-align: text-top;\">", CssClass, Width.ToString()));
        }
        public static void NewColumn(HtmlHelper htmlHelper, string CssClass)
        {
            htmlHelper.ViewContext.Writer.Write(string.Format("<td class=\"{0}\">", CssClass));
        }
        public static void EndColumn(HtmlHelper htmlHelper)
        {
            htmlHelper.ViewContext.Writer.Write("</td>");
        }
        public static void EndAndNewColumn(HtmlHelper htmlHelper)
        {
            htmlHelper.ViewContext.Writer.Write("</td><td>");
        }
        public static void EndAndNewColumn(HtmlHelper htmlHelper, string CssClass)
        {
            htmlHelper.ViewContext.Writer.Write(string.Format("</td><td class=\"{0}\">", CssClass));
        }
        public static void EndAndNewColumn(HtmlHelper htmlHelper, string CssClass, int Width)
        {
            htmlHelper.ViewContext.Writer.Write(string.Format("</td><td class=\"{0}\"style=\"width:{1}px; vertical-align: text-top;\">", CssClass, Width));
        }
        #endregion

        #region Divs

        public static void NewDiv(HtmlHelper htmlHelper)
        {
            htmlHelper.ViewContext.Writer.Write("<div>");
        }
        public static void NewDiv(HtmlHelper htmlHelper, string CssClass)
        {
            htmlHelper.ViewContext.Writer.Write(string.Format("<div class=\"{0}\">", CssClass));
        }
        public static void EndDiv(HtmlHelper htmlHelper)
        {
            htmlHelper.ViewContext.Writer.Write("</div>");
        }

        #endregion

        public static void EditForm(HtmlHelper htmlHelper)
        {
            htmlHelper.ViewContext.Writer.Write("<div class=\"edit_form\" style=\"width: 100%; margin:0px 5px 0px 5px;\">");
        }

        public static void Line(HtmlHelper htmlHelper)
        {
            htmlHelper.ViewContext.Writer.Write("<div class=\"line\" style=\"width: 100%; margin:0px 10px 10px 0px;\">");
        }
    }
}