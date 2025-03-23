using VCS.DbContext.Common;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VCS.DbContext.Entities.BU
{
    [Table("T_BU_DETAIL_TGBX")]
    public class TblBuDetailTgbx : BaseEntity
    {
        [Key]
        [Column("Id", TypeName = "nvarchar(50)")]
        public string Id { get; set; }


        [Column("HeaderId", TypeName = "nvarchar(50)")]
        public string? HeaderId { get; set; }


        [Column("LineID", TypeName = "NVARCHAR(6)")]
        public string? LineID { get; set; }


        [Column("MaLenh", TypeName = "NVARCHAR(4)")]
        public string? MaLenh { get; set; }


        [Column("NgayXuat")]
        public DateTime? NgayXuat { get; set; }


        [Column("SoLenh", TypeName = "NVARCHAR(10)")]
        public string? SoLenh { get; set; }


        [Column("TongXuat", TypeName = "DECIMAL(18,2)")]
        public decimal? TongXuat { get; set; }


        [Column("TongDuXuat", TypeName = "DECIMAL(18,2)")]
        public decimal? TongDuXuat { get; set; }

        [Column("MaHangHoa", TypeName = "NVARCHAR(18)")]
        public string MaHangHoa { get; set; }

        [Column("DonViTinh", TypeName = "NVARCHAR(3)")]
        public string? DonViTinh { get; set; }

        [Column("BeXuat", TypeName = "NVARCHAR(20)")]
        public string? BeXuat { get; set; }

        [Column("TableID", TypeName = "NVARCHAR(8)")]
        public string? TableID { get; set; }

        [Column("MeterId")]
        public string? MeterId { get; set; }


        [Column("QCI_KG", TypeName = "DECIMAL(18,2)")]
        public decimal? QCI_KG { get; set; }

        [Column("QCI_NhietDo", TypeName = "decimal(5, 2)")]
        public decimal? QCI_NhietDo { get; set; }


        [Column("QCI_TyTrong", TypeName = "decimal(10, 4)")]
        public decimal? QCI_TyTrong { get; set; }

        [Column("DonGia", TypeName = "decimal(18, 6)")]
        public decimal? DonGia { get; set; }
        [Column("CurrencyKey", TypeName = "varchar(5)")]
        public string? CurrencyKey { get; set; }
        [Column("VCF", TypeName = "decimal(6, 4)")]
        public decimal? VCF { get; set; }


        [Column("WCF", TypeName = "decimal(6, 4)")]
        public decimal? WCF { get; set; }
        [Column("NhietDo_BQGQ", TypeName = "decimal(6, 4)")]
        public decimal? NhietDo_BQGQ { get; set; }
        [Column("D15_BQGQ", TypeName = "decimal(6, 4)")]
        public decimal? D15_BQGQ { get; set; }


        [Column("KG", TypeName = "decimal(18, 2)")]
        public decimal? KG { get; set; }

        [Column("L15", TypeName = "decimal(18, 2)")]
        public decimal? L15 { get; set; }
        [Column("GiaCty", TypeName = "decimal(18, 6)")]
        public decimal? GiaCty { get; set; }
        [Column("PhiVT", TypeName = "decimal(18, 6)")]
        public decimal? PhiVT { get; set; }
        [Column("TheBVMT", TypeName = "decimal(18, 6)")]
        public decimal? TheBVMT { get; set; }
        [Column("BatchNum", TypeName = "nvarchar(200)")]
        public string? BatchNum { get; set; }
        [Column("TongSoTien", TypeName = "decimal(18, 6)")]
        public decimal? TongSoTien { get; set; }
        [Column("QCI_L15", TypeName = "decimal(18, 2)")]
        public decimal? QCI_L15 { get; set; }
        [Column("ChietKhau", TypeName = "decimal(15, 4)")]
        public decimal? ChietKhau { get; set; }

        [Column("IS_HAS_INVOICE")]
        public bool? IsHasInvoice { get; set; }
        [Column("ORDER_NAME", TypeName = "nvarchar(200)")]
        public string? OrderName { get; set; }

    }
}
