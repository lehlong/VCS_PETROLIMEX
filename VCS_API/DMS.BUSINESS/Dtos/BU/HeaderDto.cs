using AutoMapper;
using DMS.CORE.Entities.MD;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using DMS.CORE.Entities.BU;
using Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMS.BUSINESS.Dtos.BU
{
    public class HeaderDto : BaseMdTemDto, IMapFrom, IDto
    {
        [JsonIgnore]
        [Description("Số thứ tự")]
        public int OrdinalNumber { get; set; }

        [Key]
        [Description("Mã")]
        public string? Id { get; set; }
        [Description("Mã phương tiện")]
        public string? VehicleCode { get; set; }
        [Description("Tên tài xế")]
        public string? VehicleName { get; set; }
        [Description("Mã Công ty")]
        public string? CompanyCode { get; set; }
        [Description("Mã Kho")]
        public string? WarehouseCode { get; set; }
        [Description("Trạng thái hàng chờ")]
        public string? StatusVehicle { get; set; }
        [Description("Trạng thái phương tiện")]
        public string? StatusProcess { get; set; }
        [Description("Số thứ tự")]
        public int Stt { get; set; }
        [Description("Check out")]
        public bool? IsCheckout { get; set; }
        [Description("Trạng thái mời")]
        public bool? IsVoice { get; set; }
        [Description("Trạng thái in vé")]
        public bool? IsPrint { get; set; }
        [Description("Thời gian ra")]
        public DateTime? TimeCheckout { get; set; }
        [Description("Ghi chú cổng vào")]
        public string? NoteIn { get; set; }
        [Description("Ghi chú cổng ra")]
        public string? NoteOut { get; set; }
        public bool? IsXeDauKeo { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblBuHeader, HeaderDto>().ReverseMap();
        }
    }

 }

