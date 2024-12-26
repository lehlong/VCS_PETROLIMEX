using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCS.FORM.Model
{
    public class OrderListItem
    {
        public int Index { get; set; }
        public string OrderNumber { get; set; }
        public string Time { get; set; }
        public string LicensePlate { get; set; }
        public string Driver { get; set; }
        public string OrderInfo { get; set; }
        public string Status { get; set; }
    }
}
