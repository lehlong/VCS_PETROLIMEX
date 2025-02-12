using DMS.CORE.Common;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMS.CORE.Entities.BU
{
    [Table("T_BU_HEADER_TGBX")]
    public class TblBuHeaderTgbx : BaseEntity
    {
        [Key]
        [Column("Id", TypeName = "NVARCHAR(50)")]
        public string Id { get; set; }


        [Column("HeaderId", TypeName = "NVARCHAR(50)")]
        public string? HeaderId { get; set; }


        [Column("MaLenh", TypeName = "nvarchar(10)")]
        public string? MaLenh { get; set; }


        [Column("NgayXuat")]
        public DateTime? NgayXuat { get; set; }


        [Column("SoLenh", TypeName = "nvarchar(20)")]
        public string? SoLenh { get; set; }


        [Column("MaDonVi", TypeName = "nvarchar(10)")]
        public string? MaDonVi { get; set; }


        [Column("MaNguon", TypeName = "nvarchar(10)")]
        public string? MaNguon { get; set; }


        [Column("MaKho", TypeName = "nvarchar(10)")]
        public string? MaKho { get; set; }


        [Column("MaVanChuyen", TypeName = "nvarchar(10)")]
        public string? MaVanChuyen { get; set; }


        [Column("MaPhuongTien", TypeName = "nvarchar(10)")]
        public string? MaPhuongTien { get; set; }


        [Column("NguoiVanChuyen", TypeName = "nvarchar(50)")]
        public string? NguoiVanChuyen { get; set; }


        [Column("MaPhuongThucBan", TypeName = "nvarchar(10)")]
        public string? MaPhuongThucBan { get; set; }


        [Column("MaPhuongThucXuat", TypeName = "nvarchar(10)")]
        public string? MaPhuongThucXuat { get; set; }


        [Column("MaKhachHang", TypeName = "nvarchar(20)")]
        public string? MaKhachHang { get; set; }


        [Column("LoaiPhieu", TypeName = "nvarchar(10)")]
        public string? LoaiPhieu { get; set; }


        [Column("Niem", TypeName = "nvarchar(200)")]
        public string? Niem { get; set; }


        [Column("LuongGiamDinh", TypeName = "decimal(18, 2)")]
        public decimal? LuongGiamDinh { get; set; }


        [Column("NhietDoTaiTau", TypeName = "decimal(18, 2)")]
        public decimal? NhietDoTaiTau { get; set; }


        [Column("GhiChu", TypeName = "nvarchar(255)")]
        public string? GhiChu { get; set; }


        [Column("NgayHieuLuc")]
        public DateTime? NgayHieuLuc { get; set; }


        [Column("Status", TypeName = "nvarchar(10)")]
        public string? Status { get; set; }


        [Column("Number")]
        public int? Number { get; set; }


        [Column("SoLenhSAP", TypeName = "nvarchar(20)")]
        public string? SoLenhSAP { get; set; }
        [Column("Client", TypeName = "nvarchar(20)")]
        public string? Client { get; set; }
        [Column("HTTG", TypeName = "nvarchar(10)")]
        public string? HTTG { get; set; }
        [Column("Approved", TypeName = "nvarchar(10)")]
        public string? Approved { get; set; }
        [Column("User_Approve", TypeName = "nvarchar(50)")]
        public string? User_Approve { get; set; }
        [Column("Date_Approve")]
        public DateTime? Date_Approve { get; set; }
        [Column("EditApprove", TypeName = "nvarchar(10)")]
        public string? EditApprove { get; set; }
        [Column("NhaCungCap", TypeName = "nvarchar(50)")]
        public string? NhaCungCap { get; set; }
        [Column("AppDesc", TypeName = "nvarchar(255)")]
        public string? AppDesc { get; set; }
        [Column("AppN30", TypeName = "nvarchar(10)")]
        public string? AppN30 { get; set; }
        [Column("AppN30Date")]
        public DateTime? AppN30Date { get; set; }
        [Column("AppN30User", TypeName = "nvarchar(50)")]
        public string? AppN30User { get; set; }
        [Column("QCI_KG", TypeName = "decimal(18, 2)")]
        public decimal? QCI_KG { get; set; }
        [Column("QCI_NhietDo", TypeName = "decimal(18, 2)")]
        public decimal? QCI_NhietDo { get; set; }
        [Column("Slog", TypeName = "nvarchar(255)")]
        public string? Slog { get; set; }
        [Column("NgayHetHieuLuc")]
        public DateTime? NgayHetHieuLuc { get; set; }
        [Column("NgayTickKe")]
        public DateTime? NgayTickKe { get; set; }
        [Column("STO", TypeName = "nvarchar(20)")]
        public string? STO { get; set; }
        [Column("NguoiDaiDien", TypeName = "nvarchar(50)")]
        public string? NguoiDaiDien { get; set; }
        [Column("DonViCungCapVanTai", TypeName = "nvarchar(50)")]
        public string? DonViCungCapVanTai { get; set; }
        [Column("UserTickKe", TypeName = "nvarchar(50)")]
        public string? UserTickKe { get; set; }
        [Column("DiemTraHang", TypeName = "nvarchar(255)")]
        public string? DiemTraHang { get; set; }
        [Column("Tax", TypeName = "nvarchar(10)")]
        public string? Tax { get; set; }
        [Column("PaymentMethod", TypeName = "nvarchar(10)")]
        public string? PaymentMethod { get; set; }
        [Column("Term", TypeName = "nvarchar(10)")]
        public string? Term { get; set; }
        [Column("MaKhoNhap", TypeName = "nvarchar(10)")]
        public string? MaKhoNhap { get; set; }
        [Column("SoHopDong", TypeName = "nvarchar(20)")]
        public string? SoHopDong { get; set; }
        [Column("NgayHopDong")]
        public DateTime? NgayHopDong { get; set; }
        [Column("TyGia", TypeName = "decimal(18, 6)")]
        public decimal? TyGia { get; set; }
        [Column("SoTKQNhap", TypeName = "nvarchar(20)")]
        public string? SoTKQNhap { get; set; }
        [Column("SoTKQXuat", TypeName = "nvarchar(20)")]
        public string? SoTKQXuat { get; set; }
        [Column("SelfShipping", TypeName = "nvarchar(10)")]
        public string? SelfShipping { get; set; }
        [Column("PriceGroup", TypeName = "nvarchar(20)")]
        public string? PriceGroup { get; set; }
        [Column("Inco1", TypeName = "nvarchar(20)")]
        public string? Inco1 { get; set; }
        [Column("Inco2", TypeName = "nvarchar(20)")]
        public string? Inco2 { get; set; }
        [Column("SoPXK", TypeName = "nvarchar(20)")]
        public string? SoPXK { get; set; }
        [Column("NgayPXK")]
        public DateTime? NgayPXK { get; set; }
        [Column("MaTuyenDuong", TypeName = "nvarchar(10)")]
        public string? MaTuyenDuong { get; set; }
        [Column("Xuathanggui", TypeName = "nvarchar(10)")]
        public string? Xuathanggui { get; set; }
        [Column("So_TKTN", TypeName = "nvarchar(20)")]
        public string? So_TKTN { get; set; }
        [Column("So_TKTX", TypeName = "nvarchar(20)")]
        public string? So_TKTX { get; set; }
        [Column("Ngay_TKTX")]
        public DateTime? Ngay_TKTX { get; set; }
        [Column("DischargePoint", TypeName = "nvarchar(50)")]
        public string? DischargePoint { get; set; }
        [Column("DesDischargePoint", TypeName = "nvarchar(50)")]
        public string? DesDischargePoint { get; set; }
        [Column("BSART", TypeName = "nvarchar(10)")]
        public string? BSART { get; set; }
        [Column("BWART", TypeName = "nvarchar(10)")]
        public string? BWART { get; set; }
        [Column("VTWEG", TypeName = "nvarchar(10)")]
        public string? VTWEG { get; set; }
        [Column("TenKhoNhap", TypeName = "nvarchar(50)")]
        public string? TenKhoNhap { get; set; }
        [Column("Xitec_Option", TypeName = "nvarchar(10)")]
        public string? Xitec_Option { get; set; }
        [Column("SoLenhGoc", TypeName = "nvarchar(20)")]
        public string? SoLenhGoc { get; set; }
        [Column("Dem", TypeName = "nvarchar(10)")]
        public string? Dem { get; set; }
        [Column("DO1_MaNguon", TypeName = "nvarchar(10)")]
        public string? DO1_MaNguon { get; set; }
        [Column("HDChuyen", TypeName = "nvarchar(50)")]
        public string? HDChuyen { get; set; }
        [Column("BatchNum", TypeName = "nvarchar(20)")]
        public string? BatchNum { get; set; }
        [Column("PriceGroupDO1", TypeName = "nvarchar(20)")]
        public string? PriceGroupDO1 { get; set; }
        [Column("SOType", TypeName = "nvarchar(10)")]
        public string? SOType { get; set; }
        [Column("PrcingDate")]
        public DateTime? PrcingDate { get; set; }
        [Column("POType", TypeName = "nvarchar(10)")]
        public string? POType { get; set; }
        [Column("FromSoLenh", TypeName = "nvarchar(20)")]
        public string? FromSoLenh { get; set; }
        [Column("PTXuat_ID", TypeName = "nvarchar(20)")]
        public string? PTXuat_ID { get; set; }
        [Column("PTIEN", TypeName = "nvarchar(10)")]
        public string? PTIEN { get; set; }
        [Column("SCHUVEN", TypeName = "nvarchar(10)")]
        public string? SCHUVEN { get; set; }
        [Column("DO1_SoLenh", TypeName = "nvarchar(20)")]
        public string? DO1_SoLenh { get; set; }
        [Column("DO1_MaKhach", TypeName = "nvarchar(20)")]
        public string? DO1_MaKhach { get; set; }
        [Column("CardNum", TypeName = "nvarchar(20)")]
        public string? CardNum { get; set; }
        [Column("CardData", TypeName = "nvarchar(255)")]
        public string? CardData { get; set; }
        [Column("SoBienBanMau", TypeName = "nvarchar(20)")]
        public string? SoBienBanMau { get; set; }

    }
}
