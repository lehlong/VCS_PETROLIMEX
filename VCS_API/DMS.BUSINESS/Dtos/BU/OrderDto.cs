using AutoMapper;
using Common;
using DMS.BUSINESS.Dtos.MD;
using DMS.CORE.Entities.BU;
using DMS.CORE.Entities.MD;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.BUSINESS.Dtos.BU
{
    public class OrderDto : BaseMdDto, IDto, IMapFrom
    {
        [Description("STT")]
        public int Stt { get; set; }
        [Key]
        [Description("Mã")]
        public string Id { get; set; }
        [Description("Phương tiện")]
        public string VehicleCode { get; set; }
        [Description("Tên")]
        public string Name { get; set; }
        [Description("header id")]
        public string HeaderId { get; set; }
        [Description("Lần gọi")]
        public string Count { get; set; }
        [Description("Thứ tự")]
        public string Order { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblBuOrder, OrderDto>().ReverseMap();
        }

    }
}
