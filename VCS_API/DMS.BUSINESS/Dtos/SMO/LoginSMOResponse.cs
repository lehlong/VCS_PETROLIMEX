using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.BUSINESS.Dtos.SMO
{
    public class LoginSMOResponseDto
    {
        public string JWTToken { get; set; }
    }
    public static class LoginSMOResponse
    {
        public static string JWTToken { get; set; }
    }
}
