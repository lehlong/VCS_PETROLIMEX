using DMS.BUSINESS.Dtos.AD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCS.FORM.Utilities
{
    public static class LoginResponse
    {
        public static string AccessToken { get; set; }
        public static DateTime ExpireDate { get; set; }
        public static string RefreshToken { get; set; }
        public static DateTime ExpireDateRefreshToken { get; set; }
        public static AccountLoginDto AccountInfo { get; set; }

        public static void Clear()
        {
            AccessToken = null;
            ExpireDate = DateTime.MinValue;
            RefreshToken = null;
            ExpireDateRefreshToken = DateTime.MinValue;
            AccountInfo = null;
        }
    }
}
