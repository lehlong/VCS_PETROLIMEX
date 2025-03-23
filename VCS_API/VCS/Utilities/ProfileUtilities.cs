using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VCS.DbContext.Entities.AD;

namespace VCS.APP.Utilities
{
    public static class ProfileUtilities
    {
        public static TblAdAccount? User { get; set; } = new TblAdAccount();
    }
}
