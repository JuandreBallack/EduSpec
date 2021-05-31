using System;
using System.Configuration;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Reflection;

namespace EduspecService.Models
{
    partial class EduSpecDataDataContext
    {
        public EduSpecDataDataContext() :
            base(global::EduSpecService.StringCipher.Decrypt(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString(), "EduSpecService"), mappingSource)
        {
            OnCreated();
        }
    //}
    //partial class EduSpecDataDataContext
    //{
        [Function(Name = "dbo.RPT_Letterhead")]
        public ISingleResult<RPT_LetterheadResult> RPT_Letterhead([Parameter(Name = "InstID", DbType = "Int")] int? instID, [Parameter(Name = "IsPrintLetterHead", DbType = "Bit")] bool? isPrintLetterHead)
        {
            var result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), instID, isPrintLetterHead);
            return ((ISingleResult<RPT_LetterheadResult>)(result.ReturnValue));
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
