using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using DevExpress.Web;
using DevExpress.Web.Mvc.UI;

namespace EduSpec
{
    public class TableSpan
    {
        public int RowsSpan { get; set; }
        public int ColumnsSpan { get; set; }        
    }
    public enum ButtonClientSideEvent
    {
        onClick,
        onInit
    };
    public enum TextBoxClientSideEvent
    {
        onInit,
        onValidation,
        onKeyPress,
        onKeyUp,
        onKeyDown
    };
    public enum DateEditClientSideEvent
    {
        onDateChanged,
        onInit,
        onValidation
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
        onSelectedIndexChanged,
        onValidation
    };
    public enum CheckBoxClientSideEvent
    {
        onInit,
        onCheckedChanged
    }

    public enum RadioButtonClientSideEvent
    {
        onInit,
        onCheckedChanged
    }

    public static class EduSpecHtmlHelper
    {
        public static void eNewRow(HtmlHelper htmlHelper)
        {
            htmlHelper.ViewContext.Writer.Write("<tr>");
        }
        #region Devexpress Editors
        public static HtmlString Label(HtmlHelper htmlHelper, string Text, string CssClass)
        {
            htmlHelper.DevExpress().Label(lbsettings =>
            {
                lbsettings.ControlStyle.CssClass = CssClass;
                lbsettings.Text = Text;
                lbsettings.EncodeHtml = false;
            }).GetHtml();
            return null;
        }
        public static HtmlString Label(HtmlHelper htmlHelper, string Text, string Name, string CssClass)
        {
            htmlHelper.DevExpress().Label(lbsettings =>
            {
                lbsettings.ControlStyle.CssClass = CssClass;
                lbsettings.Name = Name;
                lbsettings.Text = Text;
                lbsettings.EncodeHtml = false;
            }).GetHtml();
            return null;
        }
        public static HtmlString Label(HtmlHelper htmlHelper,string Name, string Text, string CssClass, bool IsVisible)
        {
            htmlHelper.DevExpress().Label(lbsettings =>
            {
                //Test
                lbsettings.ControlStyle.CssClass = CssClass;
                lbsettings.Name = Name;
                lbsettings.Text = Text;
                lbsettings.EncodeHtml = false;
                lbsettings.ClientVisible = IsVisible;
                lbsettings.ClientEnabled = true;
            }).GetHtml();
            return null;
        }
        public static HtmlString Label(HtmlHelper htmlHelper, string Text, int width, string CssClass)
        {
            htmlHelper.DevExpress().Label(lbsettings =>
            {
                lbsettings.ControlStyle.CssClass = CssClass;
                lbsettings.Text = Text;
                lbsettings.Width = width;
                lbsettings.EncodeHtml = false;
            }).GetHtml();
            return null;
        }

        public static HtmlString Label(HtmlHelper htmlHelper, string Text, int TextSize, int Width, string CssClass)
        {
            htmlHelper.DevExpress().Label(lbsettings =>
            {
                lbsettings.ControlStyle.CssClass = CssClass;
                lbsettings.Text = Text;
                lbsettings.ControlStyle.Font.Size = TextSize;
                lbsettings.Width = Width;
                lbsettings.EncodeHtml = false;
            }).GetHtml();
            return null;
        }
        public static HtmlString Label(HtmlHelper htmlHelper, string CssClass, Object Model, String DataField)
        {
            htmlHelper.DevExpress().Label(lbsettings =>
            {
                lbsettings.Name = DataField;
                lbsettings.ControlStyle.CssClass = CssClass;
                lbsettings.EncodeHtml = false;
            }).Bind(DataBinder.Eval(Model, DataField)).GetHtml();
            return null;
        }

        public static HtmlString Label(HtmlHelper htmlHelper, string CssClass, Object Model, String DataField, string TextFormat)
        {
            if (TextFormat == "currency")
            {
                htmlHelper.DevExpress().Label(lbsettings =>
                {
                    lbsettings.Name = DataField;
                    lbsettings.Text = String.Format("{0:R ###,###,##0.00}", DataBinder.Eval(Model, DataField));
                    lbsettings.ControlStyle.CssClass = CssClass;
                    lbsettings.EncodeHtml = false;
                }).GetHtml();
            }
            else if (TextFormat == "date")
            {
                htmlHelper.DevExpress().Label(lbsettings =>
                {
                    lbsettings.Name = DataField;
                    lbsettings.Text = String.Format("{0:yyyy/MM/dd}", DataBinder.Eval(Model, DataField));
                    lbsettings.ControlStyle.CssClass = CssClass;
                    lbsettings.EncodeHtml = false;
                }).GetHtml();
            }
            return null;
        }

        public static HtmlString Label(HtmlHelper htmlHelper, string CssClass, int width, Object Model, String DataField)
        {
            htmlHelper.DevExpress().Label(lbsettings =>
            {
                lbsettings.Width = width;
                lbsettings.Name = DataField;
                lbsettings.ControlStyle.CssClass = CssClass;
                lbsettings.EncodeHtml = false;
            }).Bind(DataBinder.Eval(Model, DataField)).GetHtml();
            return null;
        }

        public static HtmlString Label(HtmlHelper htmlHelper, object Model, String DataField, string Text, string CssClass, bool IsVisible)
        {
            htmlHelper.DevExpress().Label(lbsettings =>
            {
                lbsettings.ControlStyle.CssClass = CssClass;
                lbsettings.Name = DataField;
                lbsettings.Text = Text;
                lbsettings.EncodeHtml = false;
                lbsettings.ClientVisible = IsVisible;
            }).Bind(DataBinder.Eval(Model, DataField)).GetHtml();
            return null;
        }

        public static HtmlString TextBox(HtmlHelper htmlHelper, string Name, string CssClass, bool ClientVisable, int Width, Dictionary<TextBoxClientSideEvent, string> clientEvents)
        {
            htmlHelper.DevExpress().TextBox(textBoxSettings =>
            {
                textBoxSettings.Name = Name;
                textBoxSettings.ControlStyle.CssClass = CssClass;
                textBoxSettings.ClientVisible = ClientVisable;
                textBoxSettings.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithText;
                textBoxSettings.Width = Width;
                if (clientEvents != null)
                {
                    foreach (KeyValuePair<TextBoxClientSideEvent, string> Event in clientEvents)
                    {
                        switch (Event.Key)
                        {
                            case TextBoxClientSideEvent.onInit:
                                textBoxSettings.Properties.ClientSideEvents.Init = Event.Value;
                                break;
                            case TextBoxClientSideEvent.onValidation:
                                textBoxSettings.Properties.ClientSideEvents.Validation = Event.Value;
                                break;
                            case TextBoxClientSideEvent.onKeyPress:
                                textBoxSettings.Properties.ClientSideEvents.KeyPress = Event.Value;
                                break;
                            case TextBoxClientSideEvent.onKeyUp:
                                textBoxSettings.Properties.ClientSideEvents.KeyUp = Event.Value;
                                break;
                            case TextBoxClientSideEvent.onKeyDown:
                                textBoxSettings.Properties.ClientSideEvents.KeyDown = Event.Value;
                                break;
                        }

                    }
                }
            }).GetHtml();
            return null;
        }
        public static HtmlString TextBox(HtmlHelper htmlHelper, string CssClass, Object Model, string DataField, bool ClientVisable, int Width, Dictionary<TextBoxClientSideEvent, string> clientEvents)
        {
            htmlHelper.DevExpress().TextBox(textBoxSettings =>
            {
                textBoxSettings.Name = DataField;
                textBoxSettings.ControlStyle.CssClass = CssClass;
                textBoxSettings.ClientVisible = ClientVisable;
                textBoxSettings.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;                
                textBoxSettings.Width = Width;
                //textBoxSettings.EncodeHtml = false;
                if (clientEvents != null)
                {
                    foreach (KeyValuePair<TextBoxClientSideEvent, string> Event in clientEvents)
                    {
                        switch (Event.Key)
                        {
                            case TextBoxClientSideEvent.onInit:
                                textBoxSettings.Properties.ClientSideEvents.Init = Event.Value;
                                break;
                            case TextBoxClientSideEvent.onValidation:
                                textBoxSettings.Properties.ClientSideEvents.Validation = Event.Value;
                                break;
                            case TextBoxClientSideEvent.onKeyPress:
                                textBoxSettings.Properties.ClientSideEvents.KeyPress = Event.Value;
                                break;
                            case TextBoxClientSideEvent.onKeyUp:
                                textBoxSettings.Properties.ClientSideEvents.KeyUp = Event.Value;
                                break;
                            case TextBoxClientSideEvent.onKeyDown:
                                textBoxSettings.Properties.ClientSideEvents.KeyDown = Event.Value;
                                break;
                        }

                    }
                }
            }).Bind(DataBinder.Eval(Model, DataField)).GetHtml();                        
            return null;
        }
        public static HtmlString TextBox(HtmlHelper htmlHelper, string CssClass, string Name, Object Model, string DataField, bool ClientVisable, int Width, Dictionary<TextBoxClientSideEvent, string> clientEvents)
        {
            htmlHelper.DevExpress().TextBox(textBoxSettings =>
            {
                textBoxSettings.Name = Name;
                textBoxSettings.ControlStyle.CssClass = CssClass;
                textBoxSettings.ClientVisible = ClientVisable;
                textBoxSettings.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
                textBoxSettings.Width = Width;
                if (clientEvents != null)
                {
                    foreach (KeyValuePair<TextBoxClientSideEvent, string> Event in clientEvents)
                    {
                        switch (Event.Key)
                        {
                            case TextBoxClientSideEvent.onInit:
                                textBoxSettings.Properties.ClientSideEvents.Init = Event.Value;
                                break;
                            case TextBoxClientSideEvent.onValidation:
                                textBoxSettings.Properties.ClientSideEvents.Validation = Event.Value;
                                break;
                            case TextBoxClientSideEvent.onKeyPress:
                                textBoxSettings.Properties.ClientSideEvents.KeyPress = Event.Value;
                                break;
                            case TextBoxClientSideEvent.onKeyUp:
                                textBoxSettings.Properties.ClientSideEvents.KeyUp = Event.Value;
                                break;
                            case TextBoxClientSideEvent.onKeyDown:
                                textBoxSettings.Properties.ClientSideEvents.KeyDown = Event.Value;
                                break;
                        }

                    }
                }
            }).Bind(DataBinder.Eval(Model, DataField)).GetHtml();
            return null;
        }
        public static HtmlString ComboBox(HtmlHelper htmlHelper, string Name, string CssClass, int Width, string BindProc, Dictionary<ComboboxClientSideEvent, string> clientEvents)
        {
            htmlHelper.DevExpress().ComboBox(comboBoxSettings =>
            {
                comboBoxSettings.Name = Name;
                comboBoxSettings.ControlStyle.CssClass = CssClass;
                comboBoxSettings.Width = Width;
                comboBoxSettings.Properties.TextField = "Description";
                comboBoxSettings.Properties.ValueField = "ID";
                comboBoxSettings.Properties.ValueType = typeof(Int32);
                if (clientEvents != null)
                {
                    foreach (KeyValuePair<ComboboxClientSideEvent, string> Event in clientEvents)
                    {
                        switch (Event.Key)
                        {
                            case ComboboxClientSideEvent.onInit:
                                comboBoxSettings.Properties.ClientSideEvents.Init = Event.Value;
                                break;
                            case ComboboxClientSideEvent.onSelectedIndexChanged:
                                comboBoxSettings.Properties.ClientSideEvents.SelectedIndexChanged = Event.Value;
                                break;
                            case ComboboxClientSideEvent.onBeginCallBack:
                                comboBoxSettings.Properties.ClientSideEvents.SelectedIndexChanged = Event.Value;
                                break;
                            case ComboboxClientSideEvent.onEndCallBack:
                                comboBoxSettings.Properties.ClientSideEvents.SelectedIndexChanged = Event.Value;
                                break;
                            case ComboboxClientSideEvent.onValidation:
                                comboBoxSettings.Properties.ClientSideEvents.Validation = Event.Value;
                                break;
                        }

                    }
                }
            }).BindList(GridUtils.getDropdownList(BindProc)).GetHtml();
            return null;
        }

        public static HtmlString ComboBox(HtmlHelper htmlHelper, string Name, string CssClass, int Width, string BindProc, bool IsVisible, Dictionary<ComboboxClientSideEvent, string> clientEvents)
        {
            htmlHelper.DevExpress().ComboBox(comboBoxSettings =>
            {
                comboBoxSettings.Name = Name;
                comboBoxSettings.ControlStyle.CssClass = CssClass;
                comboBoxSettings.Width = Width;
                comboBoxSettings.ClientVisible = IsVisible;
                comboBoxSettings.Properties.TextField = "Description";
                comboBoxSettings.Properties.ValueField = "ID";
                comboBoxSettings.Properties.ValueType = typeof(Int32);
                if (clientEvents != null)
                {
                    foreach (KeyValuePair<ComboboxClientSideEvent, string> Event in clientEvents)
                    {
                        switch (Event.Key)
                        {
                            case ComboboxClientSideEvent.onInit:
                                comboBoxSettings.Properties.ClientSideEvents.Init = Event.Value;
                                break;
                            case ComboboxClientSideEvent.onSelectedIndexChanged:
                                comboBoxSettings.Properties.ClientSideEvents.SelectedIndexChanged = Event.Value;
                                break;
                            case ComboboxClientSideEvent.onBeginCallBack:
                                comboBoxSettings.Properties.ClientSideEvents.SelectedIndexChanged = Event.Value;
                                break;
                            case ComboboxClientSideEvent.onEndCallBack:
                                comboBoxSettings.Properties.ClientSideEvents.SelectedIndexChanged = Event.Value;
                                break;
                            case ComboboxClientSideEvent.onValidation:
                                comboBoxSettings.Properties.ClientSideEvents.Validation = Event.Value;
                                break;
                        }

                    }
                }
            }).BindList(GridUtils.getDropdownList(BindProc)).GetHtml();
            return null;
        }

        public static HtmlString ComboBox(HtmlHelper htmlHelper, string Name, string CssClass, bool IsVisible, int Width, string BindProc, Dictionary<ComboboxClientSideEvent, string> clientEvents)
        {
            htmlHelper.DevExpress().ComboBox(comboBoxSettings =>
            {
                comboBoxSettings.Name = Name;
                comboBoxSettings.ControlStyle.CssClass = CssClass;
                comboBoxSettings.Width = Width;
                comboBoxSettings.Properties.TextField = "Description";
                comboBoxSettings.Properties.ValueField = "ID";
                comboBoxSettings.Properties.ValueType = typeof(Int32);
                comboBoxSettings.ClientVisible = IsVisible;
                if (clientEvents != null)
                {
                    foreach (KeyValuePair<ComboboxClientSideEvent, string> Event in clientEvents)
                    {
                        switch (Event.Key)
                        {
                            case ComboboxClientSideEvent.onInit:
                                comboBoxSettings.Properties.ClientSideEvents.Init = Event.Value;
                                break;
                            case ComboboxClientSideEvent.onSelectedIndexChanged:
                                comboBoxSettings.Properties.ClientSideEvents.SelectedIndexChanged = Event.Value;
                                break;
                            case ComboboxClientSideEvent.onBeginCallBack:
                                comboBoxSettings.Properties.ClientSideEvents.SelectedIndexChanged = Event.Value;
                                break;
                            case ComboboxClientSideEvent.onEndCallBack:
                                comboBoxSettings.Properties.ClientSideEvents.SelectedIndexChanged = Event.Value;
                                break;
                            case ComboboxClientSideEvent.onValidation:
                                comboBoxSettings.Properties.ClientSideEvents.Validation = Event.Value;
                                break;
                        }

                    }
                }
            }).BindList(GridUtils.getDropdownList(BindProc)).GetHtml();
            return null;
        }
        public static HtmlString ComboBox(HtmlHelper htmlHelper, string CssClass, Object Model, string DataField, int Width, string BindProc, Dictionary<ComboboxClientSideEvent, string> clientEvents)
        {
            htmlHelper.DevExpress().ComboBox(comboBoxSettings =>
            {
                comboBoxSettings.Name = DataField;
                comboBoxSettings.ControlStyle.CssClass = CssClass;
                comboBoxSettings.Width = Width;
                comboBoxSettings.Properties.TextField = "Description";
                comboBoxSettings.Properties.ValueField = "ID";
                comboBoxSettings.Properties.ValueType = typeof(Int32);
                if (clientEvents != null)
                {
                    foreach (KeyValuePair<ComboboxClientSideEvent, string> Event in clientEvents)
                    {
                        switch (Event.Key)
                        {
                            case ComboboxClientSideEvent.onInit:
                                comboBoxSettings.Properties.ClientSideEvents.Init = Event.Value;
                                break;
                            case ComboboxClientSideEvent.onSelectedIndexChanged:
                                comboBoxSettings.Properties.ClientSideEvents.SelectedIndexChanged = Event.Value;
                                break;
                            case ComboboxClientSideEvent.onBeginCallBack:
                                comboBoxSettings.Properties.ClientSideEvents.BeginCallback = Event.Value;
                                break;
                            case ComboboxClientSideEvent.onEndCallBack:
                                comboBoxSettings.Properties.ClientSideEvents.EndCallback = Event.Value;
                                break;
                            case ComboboxClientSideEvent.onValidation:
                                comboBoxSettings.Properties.ClientSideEvents.Validation = Event.Value;
                                break;
                        }

                    }
                }                
            }).BindList(GridUtils.getDropdownList(BindProc)).Bind(DataBinder.Eval(Model, DataField)).GetHtml();
            return null;
        }

        public static HtmlString ComboBox(HtmlHelper htmlHelper, string CssClass, Object Model, string DataField, bool IsVisible, int Width, string BindProc, Dictionary<ComboboxClientSideEvent, string> clientEvents)
        {
            htmlHelper.DevExpress().ComboBox(comboBoxSettings =>
            {
                comboBoxSettings.Name = DataField;
                comboBoxSettings.ControlStyle.CssClass = CssClass;
                comboBoxSettings.Width = Width;
                comboBoxSettings.ClientVisible = IsVisible;
                comboBoxSettings.Properties.TextField = "Description";
                comboBoxSettings.Properties.ValueField = "ID";
                comboBoxSettings.Properties.ValueType = typeof(Int32);
                if (clientEvents != null)
                {
                    foreach (KeyValuePair<ComboboxClientSideEvent, string> Event in clientEvents)
                    {
                        switch (Event.Key)
                        {
                            case ComboboxClientSideEvent.onInit:
                                comboBoxSettings.Properties.ClientSideEvents.Init = Event.Value;
                                break;
                            case ComboboxClientSideEvent.onSelectedIndexChanged:
                                comboBoxSettings.Properties.ClientSideEvents.SelectedIndexChanged = Event.Value;
                                break;
                            case ComboboxClientSideEvent.onBeginCallBack:
                                comboBoxSettings.Properties.ClientSideEvents.BeginCallback = Event.Value;
                                break;
                            case ComboboxClientSideEvent.onEndCallBack:
                                comboBoxSettings.Properties.ClientSideEvents.EndCallback = Event.Value;
                                break;
                            case ComboboxClientSideEvent.onValidation:
                                comboBoxSettings.Properties.ClientSideEvents.Validation = Event.Value;
                                break;
                        }

                    }
                }
            }).BindList(GridUtils.getDropdownList(BindProc)).Bind(DataBinder.Eval(Model, DataField)).GetHtml();
            return null;
        }

        public static HtmlString Button(HtmlHelper htmlHelper, string Name, string CssClass, string Text, int Width, Dictionary<ButtonClientSideEvent, string> clientEvents)
        {
            htmlHelper.DevExpress().Button(buttonSettings =>
            {
                buttonSettings.Name = Name;
                buttonSettings.ControlStyle.CssClass = CssClass;
                buttonSettings.Width = Width;
                buttonSettings.Text = Text;
                if(clientEvents != null)
                {
                    foreach (KeyValuePair<ButtonClientSideEvent, string> Event in clientEvents)
                    {
                        switch (Event.Key)
                        {
                            case ButtonClientSideEvent.onClick:
                                buttonSettings.ClientSideEvents.Click = Event.Value;
                                break;
                            case ButtonClientSideEvent.onInit:
                                buttonSettings.ClientSideEvents.Init = Event.Value;
                                break;
                        }

                    }
                }
            }).GetHtml();
            return null;
        }

	public static HtmlString Button(HtmlHelper htmlHelper, string Name, string CssClass, string Text, int Width, int Height, Dictionary<ButtonClientSideEvent, string> clientEvents)
        {
            htmlHelper.DevExpress().Button(buttonSettings =>
            {
                buttonSettings.Name = Name;
                buttonSettings.ControlStyle.CssClass = CssClass;
                buttonSettings.Width = Width;
                buttonSettings.Height = Height;
                buttonSettings.Text = Text;
                if (clientEvents != null)
                {
                    foreach (KeyValuePair<ButtonClientSideEvent, string> Event in clientEvents)
                    {
                        switch (Event.Key)
                        {
                            case ButtonClientSideEvent.onClick:
                                buttonSettings.ClientSideEvents.Click = Event.Value;
                                break;
                            case ButtonClientSideEvent.onInit:
                                buttonSettings.ClientSideEvents.Init = Event.Value;
                                break;
                        }

                    }
                }
            }).GetHtml();
            return null;
        }
        public static HtmlString DateEdit(HtmlHelper htmlHelper, string Name, string CssClass, int Width, Dictionary<DateEditClientSideEvent, string> clientEvents)
        {
            htmlHelper.DevExpress().DateEdit(deSettings =>
             {
                 deSettings.Name = Name;
                 deSettings.ControlStyle.CssClass = CssClass;
                 deSettings.Width = Width;
                 deSettings.ControlStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
                 deSettings.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
                 if (clientEvents != null)
                 {
                     foreach (KeyValuePair<DateEditClientSideEvent, string> Event in clientEvents)
                     {
                         switch (Event.Key)
                         {
                             case DateEditClientSideEvent.onDateChanged:
                                 deSettings.Properties.ClientSideEvents.DateChanged = Event.Value;
                                 break;
                             case DateEditClientSideEvent.onInit:
                                 deSettings.Properties.ClientSideEvents.Init = Event.Value;
                                 break;
                             case DateEditClientSideEvent.onValidation:
                                 deSettings.Properties.ClientSideEvents.Validation = Event.Value;
                                 break;
                         }

                     }
                 }
             }).GetHtml();            
            return null;
        }

        public static HtmlString DateEdit(HtmlHelper htmlHelper, string Name, string CssClass, int Width, bool IsVisible, Dictionary<DateEditClientSideEvent, string> clientEvents)
        {
            htmlHelper.DevExpress().DateEdit(deSettings =>
            {
                deSettings.Name = Name;
                deSettings.ControlStyle.CssClass = CssClass;
                deSettings.Width = Width;
                deSettings.Date = DateTime.Now;
                deSettings.ClientVisible = IsVisible;
                deSettings.ControlStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
                deSettings.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
                if (clientEvents != null)
                {
                    foreach (KeyValuePair<DateEditClientSideEvent, string> Event in clientEvents)
                    {
                        switch (Event.Key)
                        {
                            case DateEditClientSideEvent.onDateChanged:
                                deSettings.Properties.ClientSideEvents.DateChanged = Event.Value;
                                break;
                            case DateEditClientSideEvent.onInit:
                                deSettings.Properties.ClientSideEvents.Init = Event.Value;
                                break;
                            case DateEditClientSideEvent.onValidation:
                                deSettings.Properties.ClientSideEvents.Validation = Event.Value;
                                break;
                        }

                    }
                }
            }).GetHtml();
            return null;
        }

        public static HtmlString DateEdit(HtmlHelper htmlHelper, string CssClass, Object Model, string DataField, int Width, Dictionary<DateEditClientSideEvent, string> clientEvents)
        {
            htmlHelper.DevExpress().DateEdit(deSettings =>
            {
                deSettings.Name = DataField;
                deSettings.ControlStyle.CssClass = CssClass;
                deSettings.Width = Width;
                deSettings.ControlStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
                deSettings.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithTooltip;
                if (clientEvents != null)
                {
                    foreach (KeyValuePair<DateEditClientSideEvent, string> Event in clientEvents)
                    {
                        switch (Event.Key)
                        {
                            case DateEditClientSideEvent.onDateChanged:
                                deSettings.Properties.ClientSideEvents.DateChanged = Event.Value;
                                break;
                            case DateEditClientSideEvent.onInit:
                                deSettings.Properties.ClientSideEvents.Init = Event.Value;
                                break;
                            case DateEditClientSideEvent.onValidation:
                                deSettings.Properties.ClientSideEvents.Validation = Event.Value;
                                break;
                        }

                    }
                }
            }).Bind(DataBinder.Eval(Model, DataField)).GetHtml();
            return null;
        }
        public static HtmlString CurrencyEdit(HtmlHelper htmlHelper, string Name, string CssClass, int Width, Dictionary<SpinEditClientSideEvent, string> clientEvents)
        {
            htmlHelper.DevExpress().SpinEdit(seSettings =>
            {
                seSettings.Name = Name;
                seSettings.Properties.MaxValue = 100000;
                seSettings.Number = 0;
                seSettings.Properties.MinValue = 0;
                seSettings.ControlStyle.CssClass = CssClass;
                seSettings.Properties.SpinButtons.ShowIncrementButtons = false;
                seSettings.Width = Width;
                seSettings.Properties.DisplayFormatString = "c";
                seSettings.Properties.DecimalPlaces = 2;
                seSettings.ControlStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
                if (clientEvents != null)
                {
                    foreach (KeyValuePair<SpinEditClientSideEvent, string> Event in clientEvents)
                    {
                        switch (Event.Key)
                        {
                            case SpinEditClientSideEvent.onInit:
                                seSettings.Properties.ClientSideEvents.Init = Event.Value;
                                break;
                            case SpinEditClientSideEvent.onKeyPress:
                                seSettings.Properties.ClientSideEvents.KeyPress = Event.Value;
                                break;
                        }

                    }
                }
            }).GetHtml();
            return null;
        }

        public static HtmlString CurrencyEdit(HtmlHelper htmlHelper, string Name, string CssClass, int Width, bool IsVisible, Dictionary<SpinEditClientSideEvent, string> clientEvents)
        {
            htmlHelper.DevExpress().SpinEdit(seSettings =>
            {
                seSettings.Name = Name;
                seSettings.Properties.MaxValue = 100000;
                seSettings.Number = 0;
                seSettings.Properties.MinValue = 0;
                seSettings.ControlStyle.CssClass = CssClass;
                seSettings.Properties.SpinButtons.ShowIncrementButtons = false;
                seSettings.Width = Width;
                seSettings.ClientVisible = IsVisible;
                seSettings.Properties.DisplayFormatString = "c";
                seSettings.Properties.DecimalPlaces = 2;
                seSettings.ControlStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
                if (clientEvents != null)
                {
                    foreach (KeyValuePair<SpinEditClientSideEvent, string> Event in clientEvents)
                    {
                        switch (Event.Key)
                        {
                            case SpinEditClientSideEvent.onInit:
                                seSettings.Properties.ClientSideEvents.Init = Event.Value;
                                break;
                            case SpinEditClientSideEvent.onKeyPress:
                                seSettings.Properties.ClientSideEvents.KeyPress = Event.Value;
                                break;
                        }

                    }
                }
            }).GetHtml();
            return null;
        }

        public static HtmlString CurrencyEdit(HtmlHelper htmlHelper, string CssClass, Object Model, string DataField, int Width, Dictionary<SpinEditClientSideEvent, string> clientEvents)
        {
            htmlHelper.DevExpress().SpinEdit(seSettings =>
            {
                seSettings.Name = DataField;
                seSettings.Properties.MaxValue = 100000;
                seSettings.Number = 0;
                seSettings.Properties.MinValue = 0;
                seSettings.ControlStyle.CssClass = CssClass;
                seSettings.Properties.SpinButtons.ShowIncrementButtons = false;
                seSettings.Width = Width;
                seSettings.Properties.DisplayFormatString = "c";
                seSettings.Properties.DecimalPlaces = 2;
                seSettings.ControlStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
                if (clientEvents != null)
                {
                    foreach (KeyValuePair<SpinEditClientSideEvent, string> Event in clientEvents)
                    {
                        switch (Event.Key)
                        {
                            case SpinEditClientSideEvent.onInit:
                                seSettings.Properties.ClientSideEvents.Init = Event.Value;
                                break;
                            case SpinEditClientSideEvent.onKeyPress:
                                seSettings.Properties.ClientSideEvents.KeyPress = Event.Value;
                                break;
                        }

                    }
                }
            }).Bind(DataBinder.Eval(Model, DataField)).GetHtml();
            return null;
        }

        public static HtmlString SpinEdit(HtmlHelper htmlHelper, string CssClass, Object Model, string DataField, int Width, int MinValue, int MaxValue, bool ShowIncrementButtons, Dictionary<SpinEditClientSideEvent, string> clientEvents)
        {
            htmlHelper.DevExpress().SpinEdit(seSettings =>
            {
                seSettings.Name = DataField;
                seSettings.Properties.MaxValue = MaxValue;
                seSettings.Number = 0;
                seSettings.Properties.MinValue = MinValue;
                seSettings.ControlStyle.CssClass = CssClass;
                seSettings.Properties.SpinButtons.ShowIncrementButtons = ShowIncrementButtons;
                seSettings.Width = Width;
                seSettings.Properties.DecimalPlaces = 0;
                seSettings.ControlStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
                if (clientEvents != null)
                {
                    foreach (KeyValuePair<SpinEditClientSideEvent, string> Event in clientEvents)
                    {
                        switch (Event.Key)
                        {
                            case SpinEditClientSideEvent.onInit:
                                seSettings.Properties.ClientSideEvents.Init = Event.Value;
                                break;
                            case SpinEditClientSideEvent.onKeyPress:
                                seSettings.Properties.ClientSideEvents.KeyPress = Event.Value;
                                break;
                        }

                    }
                }
            }).Bind(DataBinder.Eval(Model, DataField)).GetHtml();
            return null;
        }

        public static HtmlString SpinEdit(HtmlHelper htmlHelper, string CssClass, Object Model, string DataField, bool ClientVisible, int Width, int MinValue, int MaxValue, bool ShowIncrementButtons, Dictionary<SpinEditClientSideEvent, string> clientEvents)
        {
            htmlHelper.DevExpress().SpinEdit(seSettings =>
            {
                seSettings.Name = DataField;
                seSettings.ClientVisible = ClientVisible;
                seSettings.Properties.MaxValue = MaxValue;
                seSettings.Number = 0;
                seSettings.Properties.MinValue = MinValue;
                seSettings.ControlStyle.CssClass = CssClass;
                seSettings.Properties.SpinButtons.ShowIncrementButtons = ShowIncrementButtons;
                seSettings.Width = Width;
                seSettings.Properties.DecimalPlaces = 0;
                seSettings.ControlStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
                if (clientEvents != null)
                {
                    foreach (KeyValuePair<SpinEditClientSideEvent, string> Event in clientEvents)
                    {
                        switch (Event.Key)
                        {
                            case SpinEditClientSideEvent.onInit:
                                seSettings.Properties.ClientSideEvents.Init = Event.Value;
                                break;
                            case SpinEditClientSideEvent.onKeyPress:
                                seSettings.Properties.ClientSideEvents.KeyPress = Event.Value;
                                break;
                        }

                    }
                }
            }).Bind(DataBinder.Eval(Model, DataField)).GetHtml();
            return null;
        }

        public static HtmlString SpinEdit(HtmlHelper htmlHelper, string CssClass, string Name, bool ClientVisible, int Width, int MinValue, int MaxValue, bool ShowIncrementButtons, Dictionary<SpinEditClientSideEvent, string> clientEvents)
        {
            htmlHelper.DevExpress().SpinEdit(seSettings =>
            {
                seSettings.Name = Name;
                seSettings.ClientVisible = ClientVisible;
                seSettings.Properties.MaxValue = MaxValue;
                seSettings.Number = 0;
                seSettings.Properties.MinValue = MinValue;
                seSettings.ControlStyle.CssClass = CssClass;
                seSettings.Properties.SpinButtons.ShowIncrementButtons = ShowIncrementButtons;
                seSettings.Width = Width;
                seSettings.Properties.DecimalPlaces = 0;
                seSettings.ControlStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
                if (clientEvents != null)
                {
                    foreach (KeyValuePair<SpinEditClientSideEvent, string> Event in clientEvents)
                    {
                        switch (Event.Key)
                        {
                            case SpinEditClientSideEvent.onInit:
                                seSettings.Properties.ClientSideEvents.Init = Event.Value;
                                break;
                            case SpinEditClientSideEvent.onKeyPress:
                                seSettings.Properties.ClientSideEvents.KeyPress = Event.Value;
                                break;
                        }

                    }
                }
            }).GetHtml();
            return null;
        }

        public static HtmlString Memo(HtmlHelper htmlHelper, string Name, string CssClass, int Width, int Height)
        {
            htmlHelper.DevExpress().Memo(memoSettings =>
            {
                memoSettings.Name = Name;
                memoSettings.ControlStyle.CssClass = CssClass;
                memoSettings.Width = Width;
                memoSettings.Height = Height;
            }).GetHtml();
            return null;
        }

        public static HtmlString Memo(HtmlHelper htmlHelper, string Name, string CssClass, int Width, int Height, bool IsValidate, bool ClientVisible)
        {
            if (IsValidate == true)
            {
                htmlHelper.DevExpress().Memo(memoSettings =>
                {
                    memoSettings.Properties.ValidationSettings.EnableCustomValidation = true;
                    memoSettings.Properties.ValidationSettings.ValidateOnLeave = true;
                    memoSettings.Properties.ValidationSettings.ErrorTextPosition = ErrorTextPosition.Top;
                    memoSettings.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithText;
                    memoSettings.Properties.ClientSideEvents.Validation = "OnRequiredFieldValidation";
                    memoSettings.Name = Name;
                    memoSettings.ControlStyle.CssClass = CssClass;
                    memoSettings.Width = Width;
                    memoSettings.Height = Height;
                    memoSettings.ClientVisible = ClientVisible;
                }).GetHtml();
            }
            else
            {
                htmlHelper.DevExpress().Memo(memoSettings =>
                {
                    memoSettings.Name = Name;
                    memoSettings.ControlStyle.CssClass = CssClass;
                    memoSettings.Width = Width;
                    memoSettings.Height = Height;
                    memoSettings.ClientVisible = ClientVisible;
                }).GetHtml();
            }
            return null;
        }

        public static HtmlString Memo(HtmlHelper htmlHelper, string CssClass, Object Model, string DataField, int Width, int Height)
        {
            htmlHelper.DevExpress().Memo(memoSettings =>
            {
                memoSettings.Name = DataField;
                memoSettings.ControlStyle.CssClass = CssClass;
                memoSettings.Width = Width;
                memoSettings.Height = Height;
            }).Bind(DataBinder.Eval(Model, DataField)).GetHtml();
            return null;
        }

        public static HtmlString Memo(HtmlHelper htmlHelper, string CssClass, Object Model, string DataField, int Width, int Height, bool IsValidate)
        {
            if (IsValidate == true)
            {
                htmlHelper.DevExpress().Memo(memoSettings =>
                {
                    memoSettings.Properties.ValidationSettings.EnableCustomValidation = true;
                    memoSettings.Properties.ValidationSettings.ValidateOnLeave = true;
                    memoSettings.Properties.ValidationSettings.ErrorTextPosition = ErrorTextPosition.Top;
                    memoSettings.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithText;
                    memoSettings.Properties.ClientSideEvents.Validation = "OnRequiredFieldValidation";
                    memoSettings.Name = DataField;
                    memoSettings.ControlStyle.CssClass = CssClass;
                    memoSettings.Width = Width;
                    memoSettings.Height = Height;
                }).Bind(DataBinder.Eval(Model, DataField)).GetHtml();
            }
            else
            {
                htmlHelper.DevExpress().Memo(memoSettings =>
                {
                    memoSettings.Name = DataField;
                    memoSettings.ControlStyle.CssClass = CssClass;
                    memoSettings.Width = Width;
                    memoSettings.Height = Height;
                }).Bind(DataBinder.Eval(Model, DataField)).GetHtml();
            }
            return null;
        }

        public static HtmlString Memo(HtmlHelper htmlHelper, string Name, string CssClass, Object Model, string DataField, int Width, int Height, bool ClientEnabled)
        {
            htmlHelper.DevExpress().Memo(memoSettings =>
            {
                memoSettings.Name = Name;
                memoSettings.ControlStyle.CssClass = CssClass;
                memoSettings.Width = Width;
                memoSettings.Height = Height;
                memoSettings.ClientEnabled = ClientEnabled;
            }).Bind(DataBinder.Eval(Model, DataField)).GetHtml();
            return null;
        }

        public static HtmlString Memo(HtmlHelper htmlHelper, string Name, string CssClass, int Width, int Height, bool ClientVisible)
        {
            htmlHelper.DevExpress().Memo(memoSettings =>
            {
                memoSettings.Name = Name;
                memoSettings.ControlStyle.CssClass = CssClass;
                memoSettings.Width = Width;
                memoSettings.Height = Height;
                memoSettings.ClientVisible = ClientVisible;
            }).GetHtml();
            return null;
        }
        public static HtmlString CheckBox(HtmlHelper htmlHelper, string Name, string CssClass, int Width, Dictionary<CheckBoxClientSideEvent, string> clientEvents)
        {
            htmlHelper.DevExpress().CheckBox(cbSettings =>
            {
                cbSettings.Name = Name;
                cbSettings.ControlStyle.CssClass = CssClass;
                cbSettings.Width = Width;
                cbSettings.Properties.ValueType = typeof(bool);
                if (clientEvents != null)
                {
                    foreach (KeyValuePair<CheckBoxClientSideEvent, string> Event in clientEvents)
                    {
                        switch (Event.Key)
                        {
                            case CheckBoxClientSideEvent.onInit:
                                cbSettings.Properties.ClientSideEvents.Init = Event.Value;
                                break;
                            case CheckBoxClientSideEvent.onCheckedChanged:
                                cbSettings.Properties.ClientSideEvents.CheckedChanged = Event.Value;
                                break;
                        }

                    }
                }
            }).GetHtml();
            return null;
        }
		public static HtmlString CheckBox(HtmlHelper htmlHelper, string Name, string CssClass, string Text, bool IsVisible, int Width, Dictionary<CheckBoxClientSideEvent, string> clientEvents)
        {
            htmlHelper.DevExpress().CheckBox(cbSettings =>
            {
                cbSettings.Name = Name;
                cbSettings.ControlStyle.CssClass = CssClass;
                cbSettings.Width = Width;
                cbSettings.Text = Text;
                cbSettings.ClientVisible = IsVisible;
                cbSettings.Properties.ValueType = typeof(bool);
                if (clientEvents != null)
                {
                    foreach (KeyValuePair<CheckBoxClientSideEvent, string> Event in clientEvents)
                    {
                        switch (Event.Key)
                        {
                            case CheckBoxClientSideEvent.onInit:
                                cbSettings.Properties.ClientSideEvents.Init = Event.Value;
                                break;
                            case CheckBoxClientSideEvent.onCheckedChanged:
                                cbSettings.Properties.ClientSideEvents.CheckedChanged = Event.Value;
                                break;
                        }

                    }
                }
            }).GetHtml();
            return null;
        }																																														
        public static HtmlString CheckBox(HtmlHelper htmlHelper, string CssClass, Object Model, string DataField, int Width, Dictionary<CheckBoxClientSideEvent, string> clientEvents)
        {
            htmlHelper.DevExpress().CheckBox(cbSettings =>
            {
                cbSettings.Name = DataField;
                cbSettings.ControlStyle.CssClass = CssClass;
                cbSettings.Width = Width;
                cbSettings.Properties.ValueType = typeof(bool);
                cbSettings.Properties.ValueUnchecked = false;
                if (clientEvents != null)
                {
                    foreach (KeyValuePair<CheckBoxClientSideEvent, string> Event in clientEvents)
                    {
                        switch (Event.Key)
                        {
                            case CheckBoxClientSideEvent.onInit:
                                cbSettings.Properties.ClientSideEvents.Init = Event.Value;
                                break;
                            case CheckBoxClientSideEvent.onCheckedChanged:
                                cbSettings.Properties.ClientSideEvents.CheckedChanged = Event.Value;
                                break;
                        }

                    }
                }
            }).Bind(DataBinder.Eval(Model, DataField)).GetHtml();
            return null;
        }
        public static HtmlString CheckBox(HtmlHelper htmlHelper, string CssClass, Object Model, string DataField, string Text, int Width, Dictionary<CheckBoxClientSideEvent, string> clientEvents)
        {
            htmlHelper.DevExpress().CheckBox(cbSettings =>
            {
                cbSettings.Name = DataField;
                cbSettings.ControlStyle.CssClass = CssClass;
                cbSettings.Width = Width;
                cbSettings.Text = Text;
                cbSettings.Properties.ValueType = typeof(bool);
                cbSettings.Properties.ValueUnchecked = false;
                if (clientEvents != null)
                {
                    foreach (KeyValuePair<CheckBoxClientSideEvent, string> Event in clientEvents)
                    {
                        switch (Event.Key)
                        {
                            case CheckBoxClientSideEvent.onInit:
                                cbSettings.Properties.ClientSideEvents.Init = Event.Value;
                                break;
                            case CheckBoxClientSideEvent.onCheckedChanged:
                                cbSettings.Properties.ClientSideEvents.CheckedChanged = Event.Value;
                                break;
                        }

                    }
                }
            }).Bind(DataBinder.Eval(Model, DataField)).GetHtml();
            return null;
        }
        public static HtmlString RadioButton(HtmlHelper htmlHelper, string CssClass, string DataField, string Text, int Width, bool isChecked, bool IsVisible, Dictionary<RadioButtonClientSideEvent, string> clientEvents)
        {
            htmlHelper.DevExpress().RadioButton(edtSettings =>
            {
                edtSettings.Name = DataField;
                edtSettings.ControlStyle.CssClass = CssClass;
                edtSettings.Width = Width;
                edtSettings.Checked = isChecked;
                edtSettings.ClientVisible = IsVisible;
                edtSettings.Text = Text;
                if (clientEvents != null)
                {
                    foreach (KeyValuePair<RadioButtonClientSideEvent, string> Event in clientEvents)
                    {
                        switch (Event.Key)
                        {
                            case RadioButtonClientSideEvent.onInit:
                                edtSettings.Properties.ClientSideEvents.Init = Event.Value;
                                break;
                            case RadioButtonClientSideEvent.onCheckedChanged:
                                edtSettings.Properties.ClientSideEvents.CheckedChanged = Event.Value;
                                break;
                        }
                    }
                }

            }).GetHtml();
            return null;
        }

        public static HtmlString RadioButton(HtmlHelper htmlHelper, string CssClass, string DataField, string Text, int Width, bool isChecked, bool IsVisible, string GroupName, Dictionary<RadioButtonClientSideEvent, string> clientEvents)
        {
            htmlHelper.DevExpress().RadioButton(edtSettings =>
            {
                edtSettings.Name = DataField;
                edtSettings.ControlStyle.CssClass = CssClass;
                edtSettings.Width = Width;
                edtSettings.Checked = isChecked;
                edtSettings.ClientVisible = IsVisible;
                edtSettings.Text = Text;
                edtSettings.GroupName = GroupName;
                if (clientEvents != null)
                {
                    foreach (KeyValuePair<RadioButtonClientSideEvent, string> Event in clientEvents)
                    {
                        switch (Event.Key)
                        {
                            case RadioButtonClientSideEvent.onInit:
                                edtSettings.Properties.ClientSideEvents.Init = Event.Value;
                                break;
                            case RadioButtonClientSideEvent.onCheckedChanged:
                                edtSettings.Properties.ClientSideEvents.CheckedChanged = Event.Value;
                                break;
                        }
                    }
                }

            }).GetHtml();
            return null;
        }

        #endregion

        #region Html Table
        public static HtmlString BeginTable(HtmlHelper htmlHelper)
        {
            htmlHelper.ViewContext.Writer.Write("<table>");
            return null;
        }
        public static HtmlString BeginTable(HtmlHelper htmlHelper, string CssClass)
        {
            htmlHelper.ViewContext.Writer.Write(string.Format("<table class=\"{0}\">", CssClass));
            return null;
        }
        public static HtmlString BeginTable(HtmlHelper htmlHelper, string CssClass, int Left_Margin, int Right_Margin, int Top_Margin, int Bottom_Margin)
        {
            htmlHelper.ViewContext.Writer.Write(string.Format("<table class=\"{0}\" style=\"margin-left:{1}px; margin-right:{2}px; margin-top:{3}px; margin-bottom:{4}px\">", CssClass,Left_Margin,Right_Margin,Top_Margin,Bottom_Margin));
            return null;
        }
        public static HtmlString EndTable(HtmlHelper htmlHelper)
        {
            htmlHelper.ViewContext.Writer.Write("</table>");
            return null;
        }

        public static HtmlString AddColumn(HtmlHelper htmlHelper)
        {
            htmlHelper.ViewContext.Writer.Write("<th width=auto></th>");
            return null;
        }

        public static HtmlString AddColumn(HtmlHelper htmlHelper, int Width)
        {
            htmlHelper.ViewContext.Writer.Write(string.Format("<th width = \"{0}\"></th>", Width));
            return null;
        }


        #endregion

        #region Html Row
        public static HtmlString NewRow(HtmlHelper htmlHelper)
        {
            htmlHelper.ViewContext.Writer.Write("<tr>");
            return null;
        }
        public static HtmlString EndRow(HtmlHelper htmlHelper)
        {
            htmlHelper.ViewContext.Writer.Write("</tr>");
            return null;
        }
        public static HtmlString EndAndNewRow(HtmlHelper htmlHelper)
        {
            htmlHelper.ViewContext.Writer.Write("</tr><tr>");
            return null;
        }
        #endregion

        #region Html Column Header
        public static HtmlString NewColumnHeader(HtmlHelper htmlHelper, string Text)
        {
            htmlHelper.ViewContext.Writer.Write(string.Format("<th>\"><h3><strong>{0}</strong></h3></th>",Text));
            return null;
        }
        public static HtmlString NewColumnHeader(HtmlHelper htmlHelper, string Text, string CssClass, int Width)
        {
            htmlHelper.ViewContext.Writer.Write(string.Format("<th class=\"{0}\"style=\"width:{1}px;\"><h3><strong>{2}</strong></h3></th>", CssClass, Width.ToString(), Text));
            return null;
        }
        #endregion

        #region Html Column
        public static HtmlString NewColumn(HtmlHelper htmlHelper)
        {
            htmlHelper.ViewContext.Writer.Write("<td >");
            return null;
        }
        public static HtmlString NewColumn(HtmlHelper htmlHelper, string CssClass, int Width)
        {
            htmlHelper.ViewContext.Writer.Write(string.Format("<td class=\"{0}\" style=\"width:{1}px; vertical-align: text-top;\">", CssClass, Width.ToString()));
            return null;
        }
        
        public static HtmlString NewColumn(HtmlHelper htmlHelper, string CssClass)
        {
            htmlHelper.ViewContext.Writer.Write(string.Format("<td class=\"{0}\" style=\"vertical-align: text-top;\">", CssClass));
            return null;
        }
        public static HtmlString NewColumn(HtmlHelper htmlHelper, string CssClass, TableSpan Span)
        {
            if (Span.RowsSpan > 1 && Span.ColumnsSpan > 1)
                htmlHelper.ViewContext.Writer.Write(string.Format("<td class=\"{0}\" rowspan=\"{1}\" colspan=\"{2}\">", CssClass, Span.RowsSpan, Span.ColumnsSpan));
            if (Span.RowsSpan <= 1 && Span.ColumnsSpan > 1)
                htmlHelper.ViewContext.Writer.Write(string.Format("<td class=\"{0}\" colspan=\"{1}\">", CssClass, Span.ColumnsSpan));
            if (Span.RowsSpan > 1 && Span.ColumnsSpan <= 1)
                htmlHelper.ViewContext.Writer.Write(string.Format("<td class=\"{0}\" rowspan=\"{1}\">", CssClass, Span.RowsSpan));
            return null;
        }
        public static HtmlString EndColumn(HtmlHelper htmlHelper)
        {
            htmlHelper.ViewContext.Writer.Write("</td>");
            return null;
        }
        public static HtmlString EndAndNewColumn(HtmlHelper htmlHelper, string CssClass, TableSpan Span)
        {
            if (Span.RowsSpan > 1 && Span.ColumnsSpan > 1)
                htmlHelper.ViewContext.Writer.Write(string.Format("</td><td class=\"{0}\" style=\"vertical-align: text-top;\" rowspan=\"{1}\" colspan=\"{2}\">", CssClass, Span.RowsSpan, Span.ColumnsSpan));
            if (Span.RowsSpan <= 1 && Span.ColumnsSpan > 1)
                htmlHelper.ViewContext.Writer.Write(string.Format("</td><td class=\"{0}\" style=\"vertical-align: text-top;\" colspan=\"{1}\">", CssClass, Span.ColumnsSpan));
            if (Span.RowsSpan > 1 && Span.ColumnsSpan <= 1)
                htmlHelper.ViewContext.Writer.Write(string.Format("</td><td class=\"{0}\" style=\"vertical-align: text-top;\" rowspan=\"{1}\">", CssClass, Span.RowsSpan));
            return null;
        }
        public static HtmlString EndAndNewColumn(HtmlHelper htmlHelper)
        {
            htmlHelper.ViewContext.Writer.Write("</td><td>");
            return null;
        }
        public static HtmlString EndAndNewColumn(HtmlHelper htmlHelper, string CssClass)
        {
            htmlHelper.ViewContext.Writer.Write(string.Format("</td><td class=\"{0}\">", CssClass));
            return null;
        }
        public static HtmlString EndAndNewColumn(HtmlHelper htmlHelper, string CssClass, int Width)
        {
            htmlHelper.ViewContext.Writer.Write(string.Format("</td><td class=\"{0}\"style=\"width:{1}px; vertical-align: text-top;\">", CssClass, Width));
            return null;
        }
        public static HtmlString EndAndNewColumn(HtmlHelper htmlHelper, int Span, string CssClass)
        {
            htmlHelper.ViewContext.Writer.Write(string.Format("</td><td class=\"{0}\" colspan=\"{1}\">", CssClass, Span));
            return null;
        }
        public static HtmlString EndAndNewColumn(HtmlHelper htmlHelper, int Span, string CssClass, int Width)
        {
            htmlHelper.ViewContext.Writer.Write(string.Format("</td><td class=\"{0}\"style=\"width:{1}px; vertical-align: text-top;\" colspan=\"{2}\">", CssClass, Width, Span));
            return null;
        }
        #endregion

        #region Divs

        public static HtmlString NewDiv(HtmlHelper htmlHelper)
        {
            htmlHelper.ViewContext.Writer.Write("<div>");
            return null;
        }
        public static HtmlString NewDiv(HtmlHelper htmlHelper, string CssClass)
        {
            htmlHelper.ViewContext.Writer.Write(string.Format("<div class=\"{0}\">", CssClass));
            return null;
        }
        public static HtmlString EndDiv(HtmlHelper htmlHelper)
        {
            htmlHelper.ViewContext.Writer.Write("</div>");
            return null;
        }

        public static HtmlString ID(HtmlHelper htmlHelper, string Name)
        {
            htmlHelper.ViewContext.Writer.Write("<div id =\"{0}\">", Name);
            return null;
        }
        
        #endregion

        public static HtmlString EditForm(HtmlHelper htmlHelper)
        {
            htmlHelper.ViewContext.Writer.Write("<div class=\"edit_form\" style=\"width: 100%; margin:0px 5px 0px 5px;\">");
            return null;
        }

        public static HtmlString Line(HtmlHelper htmlHelper)
        {
            htmlHelper.ViewContext.Writer.Write("<div class=\"line\" style=\"width: 100%; margin:0px 10px 10px 0px;\">");
            return null;
        }

    }
}