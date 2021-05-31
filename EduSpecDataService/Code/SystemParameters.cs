using EduSpecDataService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduSpecDataService.Code
{
    class SystemParameters
    {

        public class SysParams
        {
            public string AccessDatabaseProvider { get; set; }
            public string AccessDatabaseFilePath { get; set; }
            public int InstID { get; set; }
            public string SASAMS_Password { get; set; }
            public string SQL_Server { get; set; }
            public string SQL_Database { get; set; }
            public string SQL_User { get; set; }
            public string SQL_Password { get; set; }
        }

        public static SysParams SystemParams()
        {
            var Context = new EduSpecDataServiceDataContext();
            var param = new fn_Get_SysParamsResult();
            param = Context.fn_Get_SysParams().FirstOrDefault();

            return new SysParams()
            {
                AccessDatabaseProvider = param.AccessDatabaseProvider,
                AccessDatabaseFilePath = param.AccessDatabaseFilePath,
                InstID = Convert.ToInt32(param.InstID),
                SASAMS_Password = param.SASAMS_Password,
                SQL_Server = param.SQL_Server,
                SQL_Database = param.SQL_Database,
                SQL_User = param.SQL_User,
                SQL_Password = param.SQL_Password
            };
        }
    }

}

