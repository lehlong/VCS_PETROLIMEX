using AutoMapper;
using Common;
using DMS.CORE.Entities.MD;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;
using DMS.CORE.Entities.AD;

namespace DMS.BUSINESS.Dtos.AD
{
    public class ConfigDisplayDto : BaseMdDto, IMapFrom, IDto
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }

        public string? OrgCode { get; set; }
        public string? WarehouseCode { get; set; }
        public int? Cfrom { get; set; }
        public int? Cto { get; set; }
        public string? Display { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblAdConfigDisplay, ConfigDisplayDto>().ReverseMap();
        }
    }
}
