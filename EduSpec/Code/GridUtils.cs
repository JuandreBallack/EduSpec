using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using EduSpec.Models;
using System.Web.UI.WebControls;
using DevExpress.Web;
using EduSpec.Controllers;

namespace EduSpec
{
    public static class GridUtils
    {
        public class DropdownList
        {
            public Int32 ID { get; set; }
            public string Description { get; set; }
        }

        public static List<DropdownList> getDropdownList(string Method)
        {
            using (var Context = new EduSpecDataContext())
            {
                var toReturn = Context.ExecuteQuery<DropdownList>("EXEC " + Method).ToList();
                return toReturn;
            }
        }

        public static List<DropdownList> getInstDropdownList(string Method)
        {
            string InstID = System.Web.HttpContext.Current.Session["InstID"].ToString();
      
            using (var Context = new EduSpecDataContext())
            {
                var toReturn = Context.ExecuteQuery<DropdownList>(string.Format("EXEC {0} {1}", Method, InstID)).ToList();
                return toReturn;
            }
        }

        public static GridViewSettings SetGridViewProperties(GridViewSettings settings, MVCxGridViewCommandColumn commandColumn, object _ViewProperties)
        {
            if (_ViewProperties != null)
            {
                var viewProps = (ViewProperties)_ViewProperties;

                var _ViewDisplayProperties = (IList<ViewDisplayProperties_ViewResult>)viewProps.ViewDisplayPropeties.ToList();
                var _ViewGridColumns = (IEnumerable<ViewGridColumns_ViewResult>)viewProps.ViewGridColumns.ToList();
                var _ViewFieldGroups = (IEnumerable<ViewFieldGroups_ViewResult>)viewProps.ViewFieldGroups.ToList();
                var _ViewMenuButtons = (IEnumerable<MenuButtons_ViewResult>)viewProps.MenuButtons.ToList();
                var _ViewDefaultButtons = (IEnumerable<DefaultButtons_ViewResult>)viewProps.DefaultButtons.ToList();

                using (var Context = new EduSpecDataContext())
                {
                    var ViewDisplayProperties = _ViewDisplayProperties.FirstOrDefault();

                    settings.Name = ViewDisplayProperties.GridName;
                    settings.Caption = ViewDisplayProperties.GridCaption;
                    settings.KeyFieldName = ViewDisplayProperties.KeyFieldName;
                    settings.SettingsDataSecurity.AllowReadUnlistedFieldsFromClientApi = DevExpress.Utils.DefaultBoolean.True;

                    settings.CallbackRouteValues = new
                    { Controller = ViewDisplayProperties.Controller, Action = ViewDisplayProperties.Action };
                    if (ViewDisplayProperties.Action_ADD != null)
                    {
                        settings.SettingsEditing.AddNewRowRouteValues = new
                        { Controller = ViewDisplayProperties.Controller, Action = ViewDisplayProperties.Action_ADD };
                    }
                    if (ViewDisplayProperties.Action_UPDATE != null)
                    {
                        settings.SettingsEditing.UpdateRowRouteValues = new
                        { Controller = ViewDisplayProperties.Controller, Action = ViewDisplayProperties.Action_UPDATE };
                    }
                    if (ViewDisplayProperties.Action_DELETE != null)
                    {
                        settings.SettingsEditing.DeleteRowRouteValues = new
                        { Controller = ViewDisplayProperties.Controller, Action = ViewDisplayProperties.Action_DELETE };
                    }
                    settings.Width = Unit.Percentage(100);
                    settings.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                    settings.Settings.HorizontalScrollBarMode = ScrollBarMode.Visible;
                    settings.SettingsBehavior.AllowFocusedRow = true;
                    settings.Styles.AlternatingRow.Enabled = DevExpress.Utils.DefaultBoolean.True;
                    settings.Settings.ShowGroupPanel = Convert.ToBoolean(ViewDisplayProperties.ShowGroupPanel);
                    settings.Settings.ShowFilterRow = Convert.ToBoolean(ViewDisplayProperties.ShowFilterRow);
                    settings.Settings.ShowFilterRowMenu = Convert.ToBoolean(ViewDisplayProperties.ShowFilterRowMenu);
                    settings.SettingsResizing.ColumnResizeMode = ColumnResizeMode.Control;
                    settings.SettingsResizing.Visualization = ResizingMode.Live;
                    settings.SettingsBehavior.AllowEllipsisInText = true;

                    settings.ClientSideEvents.BeginCallback = ViewDisplayProperties.BeginCallBack;
                    settings.ClientSideEvents.FocusedRowChanged = ViewDisplayProperties.FocusedRowChanged;
                    settings.ClientSideEvents.RowDblClick = ViewDisplayProperties.RowDblClick;
                    settings.ClientSideEvents.SelectionChanged = ViewDisplayProperties.SelectionChange;
                    settings.ClientSideEvents.Init = ViewDisplayProperties.Init;
                    settings.ClientSideEvents.EndCallback = ViewDisplayProperties.EndCallBack;
                    settings.ClientSideEvents.RowClick = ViewDisplayProperties.RowClick;

                    commandColumn.Caption = "Action";
                    commandColumn.ButtonRenderMode = GridCommandButtonRenderMode.Image;
                    commandColumn.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                    commandColumn.Width = 60;

                    if (Convert.ToBoolean(ViewDisplayProperties.IsReadOnly) == false && ViewDisplayProperties.ShowCommandColumn == true)
                    {
                        settings.Styles.CommandColumn.Spacing = 2;
                        settings.Styles.CommandColumn.VerticalAlign = VerticalAlign.Middle;
                        settings.SettingsCommandButton.NewButton.Image.Url = "~/Content/GridImages/Add.png";
                        settings.SettingsCommandButton.EditButton.Image.Url = "~/Content/GridImages/Edit.png";
                        settings.SettingsCommandButton.DeleteButton.Image.Url = "~/Content/GridImages/Delete.png";
                        commandColumn.Visible = Convert.ToBoolean(ViewDisplayProperties.ShowCommandColumn);
                        commandColumn.ShowNewButtonInHeader = Convert.ToBoolean(ViewDisplayProperties.ShowAdd);
                        commandColumn.ShowEditButton = Convert.ToBoolean(ViewDisplayProperties.ShowEdit);
                        commandColumn.ShowDeleteButton = Convert.ToBoolean(ViewDisplayProperties.ShowDelete);
                        commandColumn.ShowClearFilterButton = true;
                    }

                    commandColumn.CellStyle.VerticalAlign = VerticalAlign.Middle;
                    settings.SettingsBehavior.ConfirmDelete = true;
                    settings.SettingsText.ConfirmDelete = "Are you sure you whant to Delete?";
                    settings.SettingsEditing.Mode = GridViewEditingMode.PopupEditForm;
                    settings.SettingsPopup.EditForm.Modal = true;
                    settings.SettingsPopup.EditForm.HorizontalAlign = PopupHorizontalAlign.WindowCenter;
                    settings.SettingsPopup.EditForm.VerticalAlign = PopupVerticalAlign.WindowCenter;
                    settings.SettingsPopup.EditForm.Width = 200;

                    settings.BeforeGetCallbackResult = (sender, e) =>
                    {
                        var s = sender as MVCxGridView;
                        if (s.IsEditing && !s.IsNewRowEditing)
                        {
                            if (ViewDisplayProperties.EditFormCaptionEdit == null)
                            {
                                s.SettingsText.PopupEditFormCaption = "Edit Form";
                            }
                            else
                            {
                                s.SettingsText.PopupEditFormCaption = ViewDisplayProperties.EditFormCaptionEdit;
                            }
                        }
                        else
                        {
                            if (ViewDisplayProperties.EditFormCaptionAdd == null)
                            {
                                s.SettingsText.PopupEditFormCaption = "Edit Form";
                            }
                            else
                            {
                                s.SettingsText.PopupEditFormCaption = ViewDisplayProperties.EditFormCaptionAdd;
                            }
                        }
                    };

                    if (ViewDisplayProperties.EndlessPaging == false)
                    {
                        settings.SettingsPager.Mode = GridViewPagerMode.ShowPager;
                        switch (ViewDisplayProperties.PagerPosition)
                        {
                            case 0:
                                settings.SettingsPager.Position = PagerPosition.Top;
                                break;
                            case 1:
                                settings.SettingsPager.Position = PagerPosition.Bottom;
                                break;
                            case 2:
                                settings.SettingsPager.Position = PagerPosition.TopAndBottom;
                                break;
                        }



                        settings.SettingsPager.FirstPageButton.Visible = Convert.ToBoolean(ViewDisplayProperties.ShowPagerFirstPageButton);
                        settings.SettingsPager.LastPageButton.Visible = Convert.ToBoolean(ViewDisplayProperties.ShowPagerLastPageButton);
                        settings.SettingsPager.PageSize = Convert.ToInt32(ViewDisplayProperties.PagerPageSize);
                        settings.SettingsPager.PageSizeItemSettings.Visible = Convert.ToBoolean(ViewDisplayProperties.ShowPagerSizeItem);
                        if (ViewDisplayProperties.PagerItems != null)
                        {
                            var namesArray = ViewDisplayProperties.PagerItems.Split(',');
                            settings.SettingsPager.PageSizeItemSettings.Items = namesArray;
                        }
                    }
                    else
                    {
                        settings.SettingsPager.Mode = GridViewPagerMode.EndlessPaging;
                        settings.SettingsPager.PageSize = Convert.ToInt32(ViewDisplayProperties.PagerPageSize);
                    }

                    if (ViewDisplayProperties.ShowGridToolBar == true)
                        SetMenuButtons(settings, _ViewDefaultButtons, _ViewMenuButtons);
                }
                
                

                var ColumnNameGroups = _ViewFieldGroups.ToList();
                var gridColumns = _ViewGridColumns.ToList();

                var ColumnIndex = 0;
                var ColumnCount = gridColumns.Count - 1;
                var CurrentGroupName = gridColumns[ColumnIndex].CaptionGroup;

                if (ColumnNameGroups.Count > 0)
                {
                    foreach (var GroupProperty in ColumnNameGroups)
                    {
                        settings.Columns.AddBand(Band =>
                            {
                                Band.Caption = GroupProperty.GroupCaption;
                                Band.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                                while (CurrentGroupName == GroupProperty.GroupCaption && ColumnIndex <= ColumnCount)
                                {
                                    Band.Columns.Add(column =>
                                        {
                                            column.PropertiesEdit.EncodeHtml = false;
                                            var property = gridColumns[ColumnIndex];
                                            SetColumnMetaData(column, property);
                                        });
                                    ColumnIndex = ColumnIndex + 1;
                                    if (ColumnIndex <= ColumnCount)
                                    {
                                        CurrentGroupName = gridColumns[ColumnIndex].CaptionGroup;
                                    }
                                }
                            });
                    }
                }
                else
                {
                    foreach (var property in gridColumns)
                    {
                        settings.Columns.Add(column =>
                            {
                                column.PropertiesEdit.EncodeHtml = false;
                                SetColumnMetaData(column, property);
                            });
                    }
                }
            }
            else
            {
                settings.Name = "NoGrid";
            }

            settings.CustomJSProperties = (sender, e) =>
            {
                e.Properties["cpFilterExpression"] = (sender as MVCxGridView).FilterExpression;
            };
            
            return settings;
        }

        public static void SetColumnMetaData(MVCxGridViewColumn GridViewColumn, ViewGridColumns_ViewResult ColumnProperty)
        {
            if (ColumnProperty.IsVisable == true)
            {
                GridViewColumn.FieldName = ColumnProperty.FieldName;
                GridViewColumn.Visible = true;
                GridViewColumn.ReadOnly = Convert.ToBoolean(ColumnProperty.IsReadOnly);
                GridViewColumn.Caption = ColumnProperty.FieldCaption;
                GridViewColumn.Width = (int)ColumnProperty.ColomnWidth;
                switch (ColumnProperty.Alignment)
                {
                    case 1:
                        GridViewColumn.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                        break;
                    case 2:
                        GridViewColumn.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                        break;
                    case 3:
                        GridViewColumn.CellStyle.HorizontalAlign = HorizontalAlign.Right;
                        break;
                }
                SetColumnControl(GridViewColumn, ColumnProperty.Control, ColumnProperty.ListProcedure);
                GridViewColumn.PropertiesEdit.DisplayFormatString = ColumnProperty.DisplayFormat;
                GridViewColumn.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
            }
            else
            {
                GridViewColumn.Visible = false;
            }
        }

        private static void SetColumnControl(MVCxGridViewColumn Column, int? ControlID, string ListProc)
        {
            switch (ControlID)
            {
                case 0:
                    Column.ColumnType = MVCxGridViewColumnType.TextBox;
                    break;
                case 1:
                    {
                        Column.ColumnType = MVCxGridViewColumnType.ComboBox;

                        var StatuscomboBoxProperties = Column.PropertiesEdit as ComboBoxProperties;
                        StatuscomboBoxProperties.ClientInstanceName = "cb" + Column.FieldName;
                        StatuscomboBoxProperties.DataSource = getDropdownList(ListProc);
                        StatuscomboBoxProperties.TextField = "Description";
                        StatuscomboBoxProperties.ValueField = "ID";
                        StatuscomboBoxProperties.ValueType = typeof(int);
                        StatuscomboBoxProperties.IncrementalFilteringMode = IncrementalFilteringMode.Contains;
                        StatuscomboBoxProperties.DropDownStyle = DropDownStyle.DropDown;
                        //StatuscomboBoxProperties.ClientSideEvents.SelectedIndexChanged = "function(s,e){cbClassID.PerformCallback(c)}";
                    }
                    break;
                case 2:
                    Column.ColumnType = MVCxGridViewColumnType.CheckBox;
                    break;
                case 3:
                    Column.ColumnType = MVCxGridViewColumnType.DateEdit;
                    break;
                case 4:
                    Column.ColumnType = MVCxGridViewColumnType.SpinEdit;
                    break;
            }
        }

        private static void SetMenuButtons(GridViewSettings settings, IEnumerable<DefaultButtons_ViewResult> DefaultButtons, IEnumerable<MenuButtons_ViewResult> CustomButtons)
        {   
            settings.Toolbars.Add(toolbar =>
            {
                foreach (var Btn in DefaultButtons)
                {                  
                    if(Btn.Value == true)
                        switch (Btn.ButtonID)
                        {
                        case 1:
                                toolbar.Items.Add(i => 
                                {
                                    i.Command = GridViewToolbarCommand.Refresh;
                                    i.Name = "btnToolbarRefresh";
                                    i.Image.IconID = DevExpress.Web.ASPxThemes.IconID.ActionsRefresh16x16;
                                    i.AdaptivePriority = 100;
                                    i.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                                    i.ItemStyle.VerticalAlign = VerticalAlign.Middle;
                                });                                
                            break;
                        case 2:
                                toolbar.Items.Add(i =>
                                {
                                    i.Command = GridViewToolbarCommand.New;
                                    i.Name = "btnToolbarNew";
                                    i.Image.IconID = DevExpress.Web.ASPxThemes.IconID.ActionsAdd16x16;
                                    i.AdaptivePriority = 100;
                                    i.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                                    i.ItemStyle.VerticalAlign = VerticalAlign.Middle;
                                });
                            break;
                        case 3:
                                toolbar.Items.Add(i =>
                                {
                                    i.Command = GridViewToolbarCommand.Edit;
                                    i.Name = "btnToolbarNew";
                                    i.Image.IconID = DevExpress.Web.ASPxThemes.IconID.EditEdit16x16;
                                    i.AdaptivePriority = 100;
                                    i.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                                    i.ItemStyle.VerticalAlign = VerticalAlign.Middle;
                                });
                            break;
                        case 4:
                                toolbar.Items.Add(i =>
                                {
                                    i.Command = GridViewToolbarCommand.Delete;
                                    i.Name = "btnToolbarDelete";
                                    i.Image.IconID = DevExpress.Web.ASPxThemes.IconID.EditDelete16x16;
                                    i.AdaptivePriority = 100;
                                    i.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                                    i.ItemStyle.VerticalAlign = VerticalAlign.Middle;
                                });
                            break;
                        case 5:
                                {
                                    toolbar.Items.Add(i =>
                                    {
                                        i.Text = "Export to";
                                        i.Image.IconID = DevExpress.Web.ASPxThemes.IconID.ActionsDownload32x32;
                                        i.BeginGroup = true;
                                        i.Items.Add(exportitem =>
                                        {
                                            exportitem.Name = "Pdf";
                                            exportitem.Text = "PDF";
                                            exportitem.Image.IconID = DevExpress.Web.ASPxThemes.IconID.ExportExporttopdf16x16office2013;
                                        });
                                        i.Items.Add(exportitem =>
                                        {
                                            exportitem.Name = "Xlsx";
                                            exportitem.Text = "XLSX";
                                            exportitem.Image.IconID = DevExpress.Web.ASPxThemes.IconID.ExportExporttoxlsx16x16office2013;
                                        });
                                        i.Items.Add(exportitem =>
                                        {
                                            exportitem.Name = "Xls";
                                            exportitem.Text = "XLS";
                                            exportitem.Image.IconID = DevExpress.Web.ASPxThemes.IconID.ExportExporttoxls16x16office2013;
                                        });
                                        i.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                                        i.ItemStyle.VerticalAlign = VerticalAlign.Middle;
                                    });
                                }
                            break;
                        }                

                }

                var MenuButtons = CustomButtons.FirstOrDefault();

                if (MenuButtons != null)
                {
                    if (MenuButtons.ButtonFunction != null)
                    {
                        settings.ClientSideEvents.ToolbarItemClick = String.Format(MenuButtons.ButtonFunction, MenuButtons.ButtonFunctionParameters);
                    }
                    var LastMenuItem = toolbar.Items.Count - 1;
                    int lastSubLevel = 0;
                    int ButtonID;
                    int? ParentButtonID;
                    int lastLevel = 0;
                    int MenuItemID = toolbar.Items.Count - 1;

                    foreach (var BtnProperty in CustomButtons)
                    {
                        if (BtnProperty.IsSubMenu == false)
                        {
                            LastMenuItem = LastMenuItem + 1;
                            ButtonID = BtnProperty.ButtonID;
                            ParentButtonID = BtnProperty.ParentID;

                            toolbar.Items.Add(toolbaritem =>
                            {
                                toolbaritem.Name = BtnProperty.ButtonName;
                                toolbaritem.Text = BtnProperty.ButtonCaption;
                                toolbaritem.ToolTip = BtnProperty.ToolTip;
                                toolbaritem.Image.Url = "~/Content/MenuButtons/" + BtnProperty.ButtonImage;
                                toolbaritem.Image.IconID = BtnProperty.ButtonImage;
                                if (BtnProperty.ButtonAction != null)
                                {
                                    toolbaritem.NavigateUrl = DevExpressHelper.GetUrl(new
                                    { Controller = BtnProperty.Controller, Action = BtnProperty.ButtonAction, target = "_blank" });
                                }
                                if (BtnProperty.IsOpenInNewWindow == true)
                                    toolbaritem.Target = "_blank";
                                toolbaritem.ItemStyle.Width = (int)BtnProperty.ButtonWidth;
                                toolbaritem.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                                toolbaritem.ItemStyle.VerticalAlign = VerticalAlign.Middle;
                            });
                        }
                        else
                        {
                            var menu = (MVCxGridViewToolbarItem)toolbar.Items[LastMenuItem];
                            
                            if (BtnProperty.SubMenuLevel == 0)
                            {                                
                                menu.Items.Add(toolbaritem =>
                                {
                                    toolbaritem.Name = BtnProperty.ButtonName;
                                    toolbaritem.Text = BtnProperty.ButtonCaption;
                                    toolbaritem.Image.IconID = BtnProperty.ButtonImage;
                                });
                                lastLevel = menu.Items.Count() - 1;
                            }                            
                            else
                            //if (BtnProperty.SubMenuLevel > lastLevel)
                            //{
                                switch(BtnProperty.SubMenuLevel)
                                {
                                    case 1:
                                        {
                                            var submenu = (MVCxGridViewToolbarItem)menu.Items[lastLevel];
                                            submenu.Items.Add(toolbaritem =>
                                            {
                                                toolbaritem.Name = BtnProperty.ButtonName;
                                                toolbaritem.Text = BtnProperty.ButtonCaption;
                                                toolbaritem.Image.IconID = BtnProperty.ButtonImage;
                                            });
                                        };
                                        lastSubLevel = menu.Items[lastLevel].Items.Count() - 1;
                                        break;
                                    case 2:
                                        {
                                            var submenu = (MVCxGridViewToolbarItem)menu.Items[lastLevel].Items[lastSubLevel];
                                            submenu.Items.Add(toolbaritem =>
                                            {
                                                toolbaritem.Name = BtnProperty.ButtonName;
                                                toolbaritem.Text = BtnProperty.ButtonCaption;
                                                toolbaritem.Image.IconID = BtnProperty.ButtonImage;
                                            });
                                        };
                                        break;
                                }
                                
                            //}
                            

                        }
                    }
                }
            });
        }
    }
}
