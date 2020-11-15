using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShowDataWebApp
{
    public static class StaticUrlBase
    {
        public static string BaseApiUrl = "https://localhost:44345/";
        public static string ShowModelApiUrl = BaseApiUrl + "api/v1/ShowModel/";
        public static string DataOverviewApiUrl = BaseApiUrl + "api/v1/DataOverview/";
    }
}
