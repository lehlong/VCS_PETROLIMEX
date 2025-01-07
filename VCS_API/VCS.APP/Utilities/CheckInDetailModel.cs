using AutoMapper;
using Common;
using DMS.BUSINESS.Dtos.SMO;
using DMS.CORE.Entities.BU;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCS.APP.Utilities
{
    public class CheckInDetailModel
    {
        public string? LicensePlate { get; set; }
        public string? VehicleImagePath { get; set; }
        public string? PlateImagePath { get; set; }
        public List<DOSAPDataDto>? ListDOSAP { get; set; }
    }
}
