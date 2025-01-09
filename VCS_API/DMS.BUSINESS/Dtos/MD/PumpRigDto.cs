using System.ComponentModel.DataAnnotations;
using AutoMapper;
using System.Text.Json.Serialization;
using System.ComponentModel;

using Common;

namespace DMS.CORE.Entities.MD
{
    public class PumpRigDto : BaseMdDto, IMapFrom, IDto
    {
        [JsonIgnore]
        [Description("Số thứ tự")]
        public int OrdinalNumber { get; set; }

        [Key]
        [Description("Mã")]
        public string Code { get; set; }

        [Description("Tên")]
        public string Name { get; set; }
        [Description("Đơn vị")]
        public string OrgCode { get; set; }
        [Description("Kho")]
        public string WarehouseCode { get; set; }
       

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblMdPumpRig, PumpRigDto>().ReverseMap();
        }
    }

}
