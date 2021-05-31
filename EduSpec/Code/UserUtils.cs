using System;
using WebMatrix.WebData;
using System.Web;
using EduSpec.Models;

namespace EduSpec.Controllers
{
    public static class UserUtils
    {

        public class User
        {
            public int UserID { get; set; }
            public string UserName { get; set; }
            public int ImpersonationUserID { get; set; }
            public string ImpersonationUserName { get; set; }
        }

        public static User CurrentUser()
        {
            try
            {
                var Context = new EduSpecDataContext();
                return new User()
                {                   
                    UserID = WebSecurity.CurrentUserId,
                    UserName = WebSecurity.CurrentUserName,
                    ImpersonationUserID = Convert.ToInt32(HttpContext.Current.Session["ImpersonateInstUserID"]),
                    ImpersonationUserName = HttpContext.Current.Session["ImpersonateInstUserName"].ToString()                    
                };
               
            }
            catch
            {
                return new User() { UserID = -1, UserName = "", ImpersonationUserID = -1 };
            }
        }

    } 

}
