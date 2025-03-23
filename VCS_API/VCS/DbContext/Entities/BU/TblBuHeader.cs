using VCS.DbContext.Common;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VCS.DbContext.Entities.BU
{
    [Table("T_BU_HEADER")]
    public class TblBuHeader : BaseEntity
    {
        [Key]
        [Column("ID", TypeName = "NVARCHAR(50)")]
        public string Id { get; set; }

        [Column("VEHICLE_CODE", TypeName = "NVARCHAR(50)")]
        public string VehicleCode { get; set; }
        [Column("VEHICLE_NAME", TypeName = "NVARCHAR(500)")]
        public string VehicleName { get; set; }

        [Column("COMPANY_CODE", TypeName = "NVARCHAR(50)")]
        public string CompanyCode { get; set; }
        [Column("WAREHOUSE_CODE", TypeName = "NVARCHAR(50)")]
        public string WarehouseCode { get; set; }

        //01-Xe đang trong hàng chờ, 02-Xe đã vào kho, 03- Xe đang lấy hàng, 04- Xe đã ra kho
        [Column("STATUS_VEHICLE", TypeName = "NVARCHAR(50)")]
        public string? StatusVehicle { get; set; }

        //00-Chưa xử lý, 01- Đã mời xe vào, 02- Chưa có ticket, 03- Đã xếp xe, 04 - Đã xử lý, 05 - Không xử lý
        [Column("STATUS_PROCESS", TypeName = "NVARCHAR(50)")]
        public string? StatusProcess { get; set; }
        [Column("STT")]
        public int Stt { get; set; }

        [Column("IS_CHECKOUT")]
        public bool? IsCheckout { get; set; }
        [Column("IS_VOICE")]
        public bool? IsVoice { get; set; }
        [Column("IS_PRINT")]
        public bool? IsPrint { get; set; }
        [Column("TIME_CHECKOUT")]
        public DateTime? TimeCheckout { get; set; }

        [Column("NOTE_IN", TypeName = "NVARCHAR(500)")]
        public string? NoteIn { get; set; }

        [Column("NOTE_OUT", TypeName = "NVARCHAR(500)")]
        public string? NoteOut { get; set; }
    }
}
