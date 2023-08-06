using System;
using System.Collections.Generic;
using System.Text;

namespace Employee.Shared
{
    public static class Constants
    {
        public const string BaseUrl = "https://gorest.co.in/public/v2";
        public const string GetUsers = BaseUrl + "/users";
        public const string AddUser = BaseUrl + "/users";
        public const string UpdateEmployee = BaseUrl + "/users/{0}";
        public const string DeleteEmployee = BaseUrl + "/users/{0}";
        public const string ApiKey = "0bf7fb56e6a27cbcadc402fc2fce8e3aa9ac2b40d4190698eb4e8df9284e2023";
    }
}
