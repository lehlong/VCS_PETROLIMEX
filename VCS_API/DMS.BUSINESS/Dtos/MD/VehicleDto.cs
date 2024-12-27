using System.ComponentModel.DataAnnotations;
using AutoMapper;
using System.Text.Json.Serialization;
using System.ComponentModel;

using Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMS.CORE.Entities.MD
{
    public class VehicleDto : BaseMdDto, IMapFrom, IDto
    {
        [JsonIgnore]
        [Key]
        public string Code { get; set; }
        public string? TransunitCode { get; set; }
        public string? Unit { get; set; }
        public string? OicPbatch { get; set; }
        public string? OicPtrip { get; set; }
        public decimal? Capacity { get; set; }
        public string? TransmodeCode { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblMdVehicle, VehicleDto>().ReverseMap();
        }
    }


}
