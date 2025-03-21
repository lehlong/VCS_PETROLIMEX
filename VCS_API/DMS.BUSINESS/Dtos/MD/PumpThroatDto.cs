using System.ComponentModel.DataAnnotations;
using AutoMapper;
using System.Text.Json.Serialization;
using System.ComponentModel;

using Common;

namespace DMS.CORE.Entities.MD
{
    public class PumpThroatDto : BaseMdDto, IMapFrom, IDto
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

        [Description("Giàn bơm")]
        public string PumpRigCode { get; set; }

        [Description("Mặt hàng")]
        public string GoodsCode { get; set; }
        [Description("Công Suất")]
        public decimal? Capacity { get; set; }
        [Description("Mã tự động hóa")]
        public string? TdhCode { get; set; }
        [Description("Mã tự động hóa E5")]
        public string? TdhE5Code { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblMdPumpThroat, PumpThroatDto>().ReverseMap();
        }
    }

}
