using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCS.Services
{
    public class ResponseLoginSmoApi
    {
        public bool STATUS { get; set; }
        public int CODE { get; set; }
        public string DATA { get; set; }
        public string? MESSAGE { get; set; }
    }
    public class CheckInDetailModel
    {
        public string? LicensePlate { get; set; }
        public string? VehicleImagePath { get; set; }
        public string? PlateImagePath { get; set; }
        public List<DOSAPDataDto>? ListDOSAP { get; set; }
    }
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
        public string MODUL_TYPE { get; set; }
        public string CUSTOMER_CODE { get; set; }
        public string CUSTOMER_NAME { get; set; }
        public string PHONE { get; set; }
        public string EMAIL { get; set; }
        public string TAI_XE { get; set; }
        public List<LIST_MATERIAL> LIST_MATERIAL { get; set; }
    }

    public class LIST_MATERIAL
    {
        public string MATERIAL { get; set; }
        public decimal QUANTITY { get; set; }
        public string UNIT { get; set; }
    }
}
