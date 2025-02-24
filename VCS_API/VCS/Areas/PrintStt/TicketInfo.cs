using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCS.APP.Areas.PrintStt
{
    public class TicketInfo
    {
        public string WarehouseName { get; set; }
        public string? Vehicle { get; set; }
        public List<string>? DO_Code { get; set; }
        public string STT { get; set; }
    }
}
