using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.BUSINESS.Dtos.BU
{
    public class HistoryDto
    {
        public string? VehicleCode { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? StatusVehicle { get; set; }
        public string? StatusProcess { get; set; }
        public string? NoteIn { get; set; }
        public string? NoteOut { get; set; }
        public string? Stt { get; set; } // Thêm STT
        public List<string> ImagesIn { get; set; } // Danh sách hình ảnh vào
        public List<string> ImagesOut { get; set; } // Danh sách hình ảnh ra
        public List<DetailDto> DetailDOs { get; set; } // Chi tiết DO
        public List<DetailTgbxDto> DetailTgbx { get; set; } // Chi tiết TGBX
    }

    public class DetailDto
    {
        public string Do1Sap { get; set; }
        public List<MaterialDto> Materials { get; set; }
    }

    public class MaterialDto
    {
        public string MaterialCode { get; set; }
        public decimal Quantity { get; set; }
        public string UnitCode { get; set; }
        public string MaterialName { get; set; } // Tên vật liệu
    }

    public class DetailTgbxDto
    {
        public string SoLenh { get; set; }
        public string MaterialName { get; set; }
        public decimal? TongXuat { get; set; }
        public string DonViTinh { get; set; }
    }
   
}
