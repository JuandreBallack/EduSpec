using System;
using WebMatrix.WebData;

namespace EduSpec
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            WebSecurity.InitializeDatabaseConnection("EduSpecConnectionString", "UserProfile", "UserId", "UserName", autoCreateTables: true);
        }
    }
}
