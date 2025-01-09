using AutoMapper;
using DMS.CORE.Entities.MD;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace DMS.BUSINESS.Dtos.MD
{
    public class SequenceDto : BaseMdDto, IDto, IMapFrom
    {

        [Description("STT")]
        public int OrdinalNumber { get; set; }
        [Key]
        [Description("Mã")]
        public string Code { get; set; }

        [Description("Lượt đợi")]
        public int STT { get; set; }

        [Description("Tên kho")]
        public string WarehouseCode { get; set; }
        [Description("Đơn vị")]
        public string OrgCode { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblMdSequence, SequenceDto>().ReverseMap();
        }
    }
}
