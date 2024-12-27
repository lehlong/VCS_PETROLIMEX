using AutoMapper;
using Common;
using DMS.CORE.Entities.MD;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.BUSINESS.Dtos.MD
{
    public class WarehouseDto : BaseMdDto, IDto, IMapFrom
    {
        [Description("STT")]
        public int OrdinalNumber { get; set; }
        [Key]
        [Description("Mã kho")]
        public string Code { get; set; }

        [Description("Tên kho")]
        public string Name { get; set; }
        [Description("Đơn vị")]
        public string OrgCode { get; set; }
        [Description("Trung gian bơm xuất")]
        public string tgbx { get; set; }
        [Description("Tự động hóa")]
        public string tdh { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblMdWarehouse, WarehouseDto>().ReverseMap();
        }

    }
}
