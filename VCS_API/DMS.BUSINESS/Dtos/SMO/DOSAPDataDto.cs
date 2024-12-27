using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.BUSINESS.Dtos.SMO
{
    public class DOSAPDataDto
    {
        public bool STATUS { get; set; }
        public int CODE { get; set; }
        public Data DATA { get; set; }
        public string MESSAGE { get; set; }
    }

    public class Data
    {
        public string VEHICLE { get; set; }
        public List<DO> LIST_DO { get; set; }
    }

    public class DO
    {
        public string DO_NUMBER { get; set; }
        public string NGUON_HANG { get; set; }
        public string TANK_GROUP { get; set; }
        public List<LIST_MATERIAL> LIST_MATERIAL { get; set; }
    }

    public class LIST_MATERIAL
    {
        public string MATERIAL { get; set; }
        public float QUANTITY { get; set; }
        public string UNIT { get; set; }
    }
}
