using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShowDataWebApp
{
    public static class StaticUrlBase
    {
        public static string BaseApiUrl = "https://localhost:44345/";
        public static string taskApiUrl = BaseApiUrl + "api/v1/task/";
        public static string ProjectApiUrl = BaseApiUrl + "api/v1/Projects/";
        public static string UserApiUrl = BaseApiUrl + "api/v1/Users/";
    }
}
