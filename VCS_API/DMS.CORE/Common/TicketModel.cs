using DMS.CORE.Entities.BU;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.CORE.Common
{
    public class TicketModel
    {
        public string? CompanyName { get; set; }
        public string? TicketNumber { get; set; }
        public string? DateTime { get; set; }
        public string? Vehicle { get; set; }
        public string? DriverName { get; set; }
        public string? PtBan { get; set; }
        public string? CustmerName { get; set; }
        public string? ChuyenVt { get; set; }
        public List<DetailTicket> Details { get; set; } = new List<DetailTicket>(); 
    }

    public class DetailTicket
    {
        public string? SoLenh { get; set; }
        public string? MaTdh { get; set; }
        public string? HangHoa { get; set; }
        public string? MaBe { get; set; }   
        public string? Ngan { get; set; }
        public decimal? DungTich { get; set; }
        public decimal? NhietDo { get; set; }
        public string? GianHong { get; set; }
        public int Rowspan { get; set; } = 1;
    }
}
