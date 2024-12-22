using System.ComponentModel.DataAnnotations;
using AutoMapper;
using System.Text.Json.Serialization;
using System.ComponentModel;

using Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMS.CORE.Entities.MD
{
    public class CameraDto : BaseMdDto, IMapFrom, IDto
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
        public string? OrgCode { get; set; }
        [Description("Kho")]
        public string? WarehouseCode { get; set; }
        [Description("IP Camera")]
        public string? Ip { get; set; }
        [Description("Tên đăng nhập")]
        public string? Username { get; set; }
        [Description("Mật khẩu")]
        public string? Password { get; set; }
        [Description("Luồng RTSP")]
        public string? Rtsp { get; set; }
        [Description("Luồng Stream")]
        public string? Stream { get; set; }
        [Description("Camera cổng vào")]
        public bool? IsIn { get; set; }
        [Description("Camera cổng ra")]
        public bool? IsOut { get; set; }
        [Description("Camera nhận diện")]
        public bool? IsRecognition { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblMdCamera, CameraDto>().ReverseMap();
        }
    }

}
