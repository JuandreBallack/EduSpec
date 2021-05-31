using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Reflection;

namespace EduSpec.Models
{
    partial class EduSpecDataContext
    {
        [Function(Name = "dbo.MetaData_ViewProperties")]
        [ResultType(typeof(ViewID_ViewResult))]
        [ResultType(typeof(ViewDisplayProperties_ViewResult))]
        [ResultType(typeof(ViewGridColumns_ViewResult))]
        [ResultType(typeof(MenuButtons_ViewResult))]
        [ResultType(typeof(DefaultButtons_ViewResult))]
        [ResultType(typeof(ViewFieldGroups_ViewResult))]
        public IMultipleResults getViewProperties([Parameter(Name = "ViewName", DbType = "VarChar(100)")] string ViewName, [Parameter(Name = "UserId", DbType = "Int")] int UserId, [Parameter(Name = "IsXlsExport", DbType = "Bit")] System.Nullable<bool> IsXlsExport, [Parameter(Name="Parameters", DbType="VarChar(MAX)")] string parameters)
        {
            var result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), ViewName, UserId, IsXlsExport, parameters);
            return ((IMultipleResults)(result.ReturnValue));
        }

        [Function(Name = "dbo.RPT_Letterhead")]
        public ISingleResult<RPT_LetterheadResult> RPT_Letterhead([Parameter(Name = "InstID", DbType = "Int")] int instID, [Parameter(Name = "IsPrintLetterHead", DbType = "Bit")] bool isPrintLetterHead)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), instID, isPrintLetterHead);
            return ((ISingleResult<RPT_LetterheadResult>)(result.ReturnValue));
        }
    }

    public partial class MenuButtons_ViewResult
    {
        private int _ItemIndex;

        private int _ButtonID;

        private System.Nullable<int> _ViewID;

        private string _GridName;

        private string _ButtonFunction;

        private string _ButtonFunctionParameters;

        private string _Controller;

        private string _ButtonName;

        private string _ButtonCaption;

        private string _ButtonAction;

        private bool _IsOpenInNewWindow;

        private System.Nullable<int> _ButtonWidth;

        private string _ButtonImage;

        private System.Nullable<bool> _IsSubMenu;

        private int _SubMenuLevel;

        private System.Nullable<int> _ParentID;

        private string _ToolTip;

        public MenuButtons_ViewResult()
        {
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ItemIndex", DbType = "Int NOT NULL")]
        public int ItemIndex
        {
            get
            {
                return _ItemIndex;
            }
            set
            {
                if ((_ItemIndex != value))
                {
                    _ItemIndex = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ButtonID", DbType = "Int NOT NULL")]
        public int ButtonID
        {
            get
            {
                return _ButtonID;
            }
            set
            {
                if ((_ButtonID != value))
                {
                    _ButtonID = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ViewID", DbType = "Int")]
        public System.Nullable<int> ViewID
        {
            get
            {
                return _ViewID;
            }
            set
            {
                if ((_ViewID != value))
                {
                    _ViewID = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_GridName", DbType = "VarChar(100)")]
        public string GridName
        {
            get
            {
                return _GridName;
            }
            set
            {
                if ((_GridName != value))
                {
                    _GridName = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ButtonFunction", DbType = "VarChar(500)")]
        public string ButtonFunction
        {
            get
            {
                return _ButtonFunction;
            }
            set
            {
                if ((_ButtonFunction != value))
                {
                    _ButtonFunction = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ButtonFunctionParameters", DbType = "VarChar(250)")]
        public string ButtonFunctionParameters
        {
            get
            {
                return _ButtonFunctionParameters;
            }
            set
            {
                if ((_ButtonFunctionParameters != value))
                {
                    _ButtonFunctionParameters = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Controller", DbType = "VarChar(100)")]
        public string Controller
        {
            get
            {
                return _Controller;
            }
            set
            {
                if ((_Controller != value))
                {
                    _Controller = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ButtonName", DbType = "VarChar(100)")]
        public string ButtonName
        {
            get
            {
                return _ButtonName;
            }
            set
            {
                if ((_ButtonName != value))
                {
                    _ButtonName = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ButtonCaption", DbType = "VarChar(40)")]
        public string ButtonCaption
        {
            get
            {
                return _ButtonCaption;
            }
            set
            {
                if ((_ButtonCaption != value))
                {
                    _ButtonCaption = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ButtonAction", DbType = "VarChar(100)")]
        public string ButtonAction
        {
            get
            {
                return _ButtonAction;
            }
            set
            {
                if ((_ButtonAction != value))
                {
                    _ButtonAction = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_IsOpenInNewWindow", DbType = "Bit")]
        public bool IsOpenInNewWindow
        {
            get
            {
                return _IsOpenInNewWindow;
            }
            set
            {
                if ((_IsOpenInNewWindow != value))
                {
                    _IsOpenInNewWindow = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ButtonWidth", DbType = "Int")]
        public System.Nullable<int> ButtonWidth
        {
            get
            {
                return _ButtonWidth;
            }
            set
            {
                if ((_ButtonWidth != value))
                {
                    _ButtonWidth = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ButtonImage", DbType = "VarChar(100)")]
        public string ButtonImage
        {
            get
            {
                return _ButtonImage;
            }
            set
            {
                if ((_ButtonImage != value))
                {
                    _ButtonImage = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_IsSubMenu", DbType = "Bit")]
        public System.Nullable<bool> IsSubMenu
        {
            get
            {
                return _IsSubMenu;
            }
            set
            {
                if ((_IsSubMenu != value))
                {
                    _IsSubMenu = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_SubMenuLevel", DbType = "int")]
        public int SubMenuLevel
        {
            get
            {
                return _SubMenuLevel;
            }
            set
            {
                if ((_SubMenuLevel != value))
                {
                    _SubMenuLevel = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ParentID", DbType = "Int")]
        public System.Nullable<int> ParentID
        {
            get
            {
                return _ParentID;
            }
            set
            {
                if ((_ParentID != value))
                {
                    _ParentID = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ToolTip", DbType = "VarChar(200)")]
        public string ToolTip
        {
            get
            {
                return _ToolTip;
            }
            set
            {
                if ((_ToolTip != value))
                {
                    _ToolTip = value;
                }
            }
        }
    }

    public partial class ViewID_ViewResult
    {
        private System.Nullable<int> _ViewID;

        public ViewID_ViewResult()
        {
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ViewID", DbType = "Int")]
        public System.Nullable<int> ViewID
        {
            get
            {
                return _ViewID;
            }
            set
            {
                if ((_ViewID != value))
                {
                    _ViewID = value;
                }
            }
        }
    }

    public partial class ViewDisplayProperties_ViewResult
    {

        private string _ViewName;

        private string _GridName;

        private string _DetailGridName;

        private string _GridCaption;

        private string _KeyFieldName;

        private string _ProcName;

        private string _Controller;

        private string _Action;

        private string _Action_ADD;

        private string _Action_UPDATE;

        private string _Action_DELETE;

        private bool? _ShowCommandColumn;

        private bool? _ShowGridToolBar;

        private System.Nullable<bool> _ShowAdd;

        private System.Nullable<bool> _ShowEdit;

        private System.Nullable<bool> _ShowDelete;

        private System.Nullable<bool> _ShowGroupPanel;

        private System.Nullable<bool> _ShowFilterRow;

        private System.Nullable<bool> _ShowFilterRowMenu;

        private System.Nullable<int> _PagerPosition;

        private bool _EndlessPaging;

        private System.Nullable<bool> _ShowPagerFirstPageButton;

        private System.Nullable<bool> _ShowPagerLastPageButton;

        private System.Nullable<bool> _ShowPagerSizeItem;

        private string _PagerItems;

        private System.Nullable<int> _PagerPageSize;

        private System.Nullable<bool> _AutoHeight;

        private System.Nullable<bool> _IsViewHeaderVisible;

        private System.Nullable<int> _ViewHeaderHeight;

        private System.Nullable<bool> _IsViewFooterVisible;

        private System.Nullable<int> _ViewFooterHeight;

        private string _EditFormCaptionEdit;

        private string _EditFormCaptionAdd;

        private System.Nullable<bool> _IsReadOnly;

        private string _BeginCallBack;

        private string _FocusedRowChanged;

        private string _RowDblClick;

        private string _SelectionChange;

        private string _Init;

        private string _EndCallBack;

        private string _RowClick;



        public ViewDisplayProperties_ViewResult()
        {
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ViewName", DbType = "VarChar(100)")]
        public string ViewName
        {
            get
            {
                return this._ViewName;
            }
            set
            {
                if ((this._ViewName != value))
                {
                    this._ViewName = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_GridName", DbType = "VarChar(100)")]
        public string GridName
        {
            get
            {
                return this._GridName;
            }
            set
            {
                if ((this._GridName != value))
                {
                    this._GridName = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_DetailGridName", DbType = "VarChar(100)")]
        public string DetailGridName
        {
            get
            {
                return this._DetailGridName;
            }
            set
            {
                if ((this._DetailGridName != value))
                {
                    this._DetailGridName = value;
                }
            }
        }


        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_GridCaption", DbType = "VarChar(100)")]
        public string GridCaption
        {
            get
            {
                return this._GridCaption;
            }
            set
            {
                if ((this._GridCaption != value))
                {
                    this._GridCaption = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_KeyFieldName", DbType = "VarChar(100)")]
        public string KeyFieldName
        {
            get
            {
                return this._KeyFieldName;
            }
            set
            {
                if ((this._KeyFieldName != value))
                {
                    this._KeyFieldName = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ProcName", DbType = "VarChar(100)")]
        public string ProcName
        {
            get
            {
                return this._ProcName;
            }
            set
            {
                if ((this._ProcName != value))
                {
                    this._ProcName = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Controller", DbType = "VarChar(100)")]
        public string Controller
        {
            get
            {
                return this._Controller;
            }
            set
            {
                if ((this._Controller != value))
                {
                    this._Controller = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Action", DbType = "VarChar(100)")]
        public string Action
        {
            get
            {
                return this._Action;
            }
            set
            {
                if ((this._Action != value))
                {
                    this._Action = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Action_ADD", DbType = "VarChar(100)")]
        public string Action_ADD
        {
            get
            {
                return this._Action_ADD;
            }
            set
            {
                if ((this._Action_ADD != value))
                {
                    this._Action_ADD = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Action_UPDATE", DbType = "VarChar(100)")]
        public string Action_UPDATE
        {
            get
            {
                return this._Action_UPDATE;
            }
            set
            {
                if ((this._Action_UPDATE != value))
                {
                    this._Action_UPDATE = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Action_DELETE", DbType = "VarChar(100)")]
        public string Action_DELETE
        {
            get
            {
                return this._Action_DELETE;
            }
            set
            {
                if ((this._Action_DELETE != value))
                {
                    this._Action_DELETE = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ShowCommandColumn", DbType = "Bit")]
        public System.Nullable<bool> ShowCommandColumn
        {
            get
            {
                return this._ShowCommandColumn;
            }
            set
            {
                if ((this._ShowCommandColumn != value))
                {
                    this._ShowCommandColumn = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ShowGridToolBar", DbType = "Bit")]
        public System.Nullable<bool> ShowGridToolBar
        {
            get
            {
                return this._ShowGridToolBar;
            }
            set
            {
                if ((this._ShowGridToolBar != value))
                {
                    this._ShowGridToolBar = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ShowAdd", DbType = "Bit")]
        public System.Nullable<bool> ShowAdd
        {
            get
            {
                return this._ShowAdd;
            }
            set
            {
                if ((this._ShowAdd != value))
                {
                    this._ShowAdd = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ShowEdit", DbType = "Bit")]
        public System.Nullable<bool> ShowEdit
        {
            get
            {
                return this._ShowEdit;
            }
            set
            {
                if ((this._ShowEdit != value))
                {
                    this._ShowEdit = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ShowDelete", DbType = "Bit")]
        public System.Nullable<bool> ShowDelete
        {
            get
            {
                return this._ShowDelete;
            }
            set
            {
                if ((this._ShowDelete != value))
                {
                    this._ShowDelete = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ShowGroupPanel", DbType = "Bit")]
        public System.Nullable<bool> ShowGroupPanel
        {
            get
            {
                return this._ShowGroupPanel;
            }
            set
            {
                if ((this._ShowGroupPanel != value))
                {
                    this._ShowGroupPanel = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ShowFilterRow", DbType = "Bit")]
        public System.Nullable<bool> ShowFilterRow
        {
            get
            {
                return this._ShowFilterRow;
            }
            set
            {
                if ((this._ShowFilterRow != value))
                {
                    this._ShowFilterRow = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ShowFilterRowMenu", DbType = "Bit")]
        public System.Nullable<bool> ShowFilterRowMenu
        {
            get
            {
                return this._ShowFilterRowMenu;
            }
            set
            {
                if ((this._ShowFilterRowMenu != value))
                {
                    this._ShowFilterRowMenu = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_PagerPosition", DbType = "Int")]
        public System.Nullable<int> PagerPosition
        {
            get
            {
                return this._PagerPosition;
            }
            set
            {
                if ((this._PagerPosition != value))
                {
                    this._PagerPosition = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_EndlessPaging", DbType = "Bit")]
        public bool EndlessPaging
        {
            get
            {
                return this._EndlessPaging;
            }
            set
            {
                if ((this._EndlessPaging != value))
                {
                    this._EndlessPaging = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ShowPagerFirstPageButton", DbType = "Bit")]
        public System.Nullable<bool> ShowPagerFirstPageButton
        {
            get
            {
                return this._ShowPagerFirstPageButton;
            }
            set
            {
                if ((this._ShowPagerFirstPageButton != value))
                {
                    this._ShowPagerFirstPageButton = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ShowPagerLastPageButton", DbType = "Bit")]
        public System.Nullable<bool> ShowPagerLastPageButton
        {
            get
            {
                return this._ShowPagerLastPageButton;
            }
            set
            {
                if ((this._ShowPagerLastPageButton != value))
                {
                    this._ShowPagerLastPageButton = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ShowPagerSizeItem", DbType = "Bit")]
        public System.Nullable<bool> ShowPagerSizeItem
        {
            get
            {
                return this._ShowPagerSizeItem;
            }
            set
            {
                if ((this._ShowPagerSizeItem != value))
                {
                    this._ShowPagerSizeItem = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_PagerItems", DbType = "VarChar(50)")]
        public string PagerItems
        {
            get
            {
                return this._PagerItems;
            }
            set
            {
                if ((this._PagerItems != value))
                {
                    this._PagerItems = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_PagerPageSize", DbType = "Int")]
        public System.Nullable<int> PagerPageSize
        {
            get
            {
                return this._PagerPageSize;
            }
            set
            {
                if ((this._PagerPageSize != value))
                {
                    this._PagerPageSize = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_AutoHeight", DbType = "Bit")]
        public System.Nullable<bool> AutoHeight
        {
            get
            {
                return this._AutoHeight;
            }
            set
            {
                if ((this._AutoHeight != value))
                {
                    this._AutoHeight = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_IsViewHeaderVisible", DbType = "Bit")]
        public System.Nullable<bool> IsViewHeaderVisible
        {
            get
            {
                return this._IsViewHeaderVisible;
            }
            set
            {
                if ((this._IsViewHeaderVisible != value))
                {
                    this._IsViewHeaderVisible = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ViewHeaderHeight", DbType = "Int")]
        public System.Nullable<int> ViewHeaderHeight
        {
            get
            {
                return this._ViewHeaderHeight;
            }
            set
            {
                if ((this._ViewHeaderHeight != value))
                {
                    this._ViewHeaderHeight = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_IsViewFooterVisible", DbType = "Bit")]
        public System.Nullable<bool> IsViewFooterVisible
        {
            get
            {
                return this._IsViewFooterVisible;
            }
            set
            {
                if ((this._IsViewFooterVisible != value))
                {
                    this._IsViewFooterVisible = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ViewFooterHeight", DbType = "Int")]
        public System.Nullable<int> ViewFooterHeight
        {
            get
            {
                return this._ViewFooterHeight;
            }
            set
            {
                if ((this._ViewFooterHeight != value))
                {
                    this._ViewFooterHeight = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_EditFormCaptionEdit", DbType = "VarChar(20)")]
        public string EditFormCaptionEdit
        {
            get
            {
                return this._EditFormCaptionEdit;
            }
            set
            {
                if ((this._EditFormCaptionEdit != value))
                {
                    this._EditFormCaptionEdit = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_EditFormCaptionAdd", DbType = "VarChar(20)")]
        public string EditFormCaptionAdd
        {
            get
            {
                return this._EditFormCaptionAdd;
            }
            set
            {
                if ((this._EditFormCaptionAdd != value))
                {
                    this._EditFormCaptionAdd = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_IsReadOnly", DbType = "Bit")]
        public System.Nullable<bool> IsReadOnly
        {
            get
            {
                return this._IsReadOnly;
            }
            set
            {
                if ((this._IsReadOnly != value))
                {
                    this._IsReadOnly = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_BeginCallBack", DbType = "VarChar(500)")]
        public string BeginCallBack
        {
            get
            {
                return this._BeginCallBack;
            }
            set
            {
                if ((this._BeginCallBack != value))
                {
                    this._BeginCallBack = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_FocusedRowChanged", DbType = "VarChar(500)")]
        public string FocusedRowChanged
        {
            get
            {
                return this._FocusedRowChanged;
            }
            set
            {
                if ((this._FocusedRowChanged != value))
                {
                    this._FocusedRowChanged = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_RowDblClick", DbType = "VarChar(500)")]
        public string RowDblClick
        {
            get
            {
                return this._RowDblClick;
            }
            set
            {
                if ((this._RowDblClick != value))
                {
                    this._RowDblClick = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_SelectionChange", DbType = "VarChar(500)")]
        public string SelectionChange
        {
            get
            {
                return this._SelectionChange;
            }
            set
            {
                if ((this._SelectionChange != value))
                {
                    this._SelectionChange = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Init", DbType = "VarChar(500)")]
        public string Init
        {
            get
            {
                return this._Init;
            }
            set
            {
                if ((this._Init != value))
                {
                    this._Init = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_EndCallBack", DbType = "VarChar(500)")]
        public string EndCallBack
        {
            get
            {
                return this._EndCallBack;
            }
            set
            {
                if ((this._EndCallBack != value))
                {
                    this._EndCallBack = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_RowClick", DbType = "VarChar(500)")]
        public string RowClick
        {
            get
            {
                return this._RowClick;
            }
            set
            {
                if ((this._RowClick != value))
                {
                    this._RowClick = value;
                }
            }
        }
    }

    public partial class ViewGridColumns_ViewResult
    {
        private System.Nullable<int> _FieldPos;

        private string _FieldName;

        private string _FieldCaption;

        private string _CaptionGroup;

        private System.Nullable<bool> _IsVisable;

        private System.Nullable<int> _Control;

        private string _DisplayFormat;

        private System.Nullable<int> _ColomnWidth;

        private string _ListProcedure;

        private System.Nullable<bool> _IsReadOnly;

        private int _Alignment;

        public ViewGridColumns_ViewResult()
        {
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_FieldPos", DbType = "Int")]
        public System.Nullable<int> FieldPos
        {
            get
            {
                return _FieldPos;
            }
            set
            {
                if ((_FieldPos != value))
                {
                    _FieldPos = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_FieldName", DbType = "VarChar(50)")]
        public string FieldName
        {
            get
            {
                return _FieldName;
            }
            set
            {
                if ((_FieldName != value))
                {
                    _FieldName = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_FieldCaption", DbType = "VarChar(50)")]
        public string FieldCaption
        {
            get
            {
                return _FieldCaption;
            }
            set
            {
                if ((_FieldCaption != value))
                {
                    _FieldCaption = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_CaptionGroup", DbType = "VarChar(50)")]
        public string CaptionGroup
        {
            get
            {
                return _CaptionGroup;
            }
            set
            {
                if ((_CaptionGroup != value))
                {
                    _CaptionGroup = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_IsVisable", DbType = "Bit")]
        public System.Nullable<bool> IsVisable
        {
            get
            {
                return _IsVisable;
            }
            set
            {
                if ((_IsVisable != value))
                {
                    _IsVisable = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Control", DbType = "Int")]
        public System.Nullable<int> Control
        {
            get
            {
                return _Control;
            }
            set
            {
                if ((_Control != value))
                {
                    _Control = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_DisplayFormat", DbType = "VarChar(50)")]
        public string DisplayFormat
        {
            get
            {
                return _DisplayFormat;
            }
            set
            {
                if ((_DisplayFormat != value))
                {
                    _DisplayFormat = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ColomnWidth", DbType = "Int")]
        public System.Nullable<int> ColomnWidth
        {
            get
            {
                return _ColomnWidth;
            }
            set
            {
                if ((_ColomnWidth != value))
                {
                    _ColomnWidth = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ListProcedure", DbType = "VarChar(100)")]
        public string ListProcedure
        {
            get
            {
                return _ListProcedure;
            }
            set
            {
                if ((_ListProcedure != value))
                {
                    _ListProcedure = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_IsReadOnly", DbType = "Bit")]
        public System.Nullable<bool> IsReadOnly
        {
            get
            {
                return _IsReadOnly;
            }
            set
            {
                if ((_IsReadOnly != value))
                {
                    _IsReadOnly = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Alignment", DbType = "Int")]
        public int Alignment
        {
            get
            {
                return _Alignment;
            }
            set
            {
                if ((_Alignment != value))
                {
                    _Alignment = value;
                }
            }
        }
    }

    public partial class DefaultButtons_ViewResult
    {
        private int _ButtonID;

        private string _DefaultBtn;

        private bool _Value;

        public DefaultButtons_ViewResult()
        {
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ButtonID", DbType = "Int NOT NULL")]
        public int ButtonID
        {
            get
            {
                return _ButtonID;
            }
            set
            {
                if ((_ButtonID != value))
                {
                    _ButtonID = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_DefaultBtn", DbType = "VarChar(50)")]
        public string DefaultBtn
        {
            get
            {
                return _DefaultBtn;
            }
            set
            {
                if ((_DefaultBtn != value))
                {
                    _DefaultBtn = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Value", DbType = "Bit")]
        public bool Value
        {
            get
            {
                return _Value;
            }
            set
            {
                if ((_Value != value))
                {
                    _Value = value;
                }
            }
        }
    }

    public partial class ViewFieldGroups_ViewResult
    {
        private int _ID;

        private string _GroupCaption;

        public ViewFieldGroups_ViewResult()
        {
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ID", DbType = "Int NOT NULL")]
        public int ID
        {
            get
            {
                return _ID;
            }
            set
            {
                if ((_ID != value))
                {
                    _ID = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_GroupCaption", DbType = "VarChar(50)")]
        public string GroupCaption
        {
            get
            {
                return _GroupCaption;
            }
            set
            {
                if ((_GroupCaption != value))
                {
                    _GroupCaption = value;
                }
            }
        }
    }

    public partial class RPT_LetterheadResult
    {

        private string _InstitutionName;

        private string _StreetAddress;

        private string _PostalAddress;

        private string _Contact;

        private string _LogoUrl;

        private Byte[] _Logo;

        public RPT_LetterheadResult()
        {
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_InstitutionName", DbType = "VarChar(1000)")]
        public string InstitutionName
        {
            get
            {
                return this._InstitutionName;
            }
            set
            {
                if ((this._InstitutionName != value))
                {
                    this._InstitutionName = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_StreetAddress", DbType = "VarChar(8000)")]
        public string StreetAddress
        {
            get
            {
                return this._StreetAddress;
            }
            set
            {
                if ((this._StreetAddress != value))
                {
                    this._StreetAddress = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_PostalAddress", DbType = "VarChar(8000)")]
        public string PostalAddress
        {
            get
            {
                return this._PostalAddress;
            }
            set
            {
                if ((this._PostalAddress != value))
                {
                    this._PostalAddress = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Contact", DbType = "VarChar(181)")]
        public string Contact
        {
            get
            {
                return this._Contact;
            }
            set
            {
                if ((this._Contact != value))
                {
                    this._Contact = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_LogoUrl", DbType = "VarChar(100)")]
        public string LogoUrl
        {
            get
            {
                return this._LogoUrl;
            }
            set
            {
                if ((this._LogoUrl != value))
                {
                    this._LogoUrl = value;
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Logo", DbType = "VarBinary(MAX)")]
        public Byte[] Logo
        {
            get
            {
                return this._Logo;
            }
            set
            {
                if ((this._Logo != value))
                {
                    this._Logo = value;
                }
            }
        }
    }
}
