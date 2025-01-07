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
    public class CheckInDetailModel : IMapFrom
    {
        public string? LicensePlate { get; set; }
        public string? VehicleImagePath { get; set; }
        public string? PlateImagePath { get; set; }
        public List<DOSAPDataDto>? ListDOSAP { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblBuHeader, CheckInDetailModel>()
                .ForMember(d => d.LicensePlate, opt => opt.MapFrom(s => s.VehicleCode))
                .ForMember(d => d.VehicleImagePath, opt => opt.Ignore())
                .ForMember(d => d.PlateImagePath, opt => opt.Ignore())
                .ForMember(d => d.ListDOSAP, opt => opt.Ignore());
        }
    }
}
