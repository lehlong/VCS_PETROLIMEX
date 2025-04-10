using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DMS.CORE.Common;

namespace DMS.CORE.Entities.BU
{
    [Table("T_BU_TGBX_TICKET")]
    public class TblBuTgbxTicket : BaseEntity
    {
        [Key]
        [Column("Id", TypeName = "nvarchar(50)")]
        public string Id { get; set; }
        [Column("HeaderId")]
        public string HeaderId { get; set; }

        [Column("MaNgan", TypeName = "nvarchar(3)")]
        public string? MaNgan { get; set; }

        [Column("MaLenh", TypeName = "nvarchar(4)")]
        public string? MaLenh { get; set; }

        [Column("NgayXuat")]
        public DateTime? NgayXuat { get; set; }

        [Column("LineID", TypeName = "nvarchar(6)")]
        public string? LineID { get; set; }

        [Column("SoLuongDuXuat", TypeName = "decimal(18, 2)")]
        public decimal? SoLuongDuXuat { get; set; }

        [Column("SoLuongThucXuat", TypeName = "decimal(18, 2)")]
        public decimal? SoLuongThucXuat { get; set; }

        [Column("ThoiGianDau")]
        public DateTime? ThoiGianDau { get; set; }

        [Column("ThoiGianCuoi")]
        public DateTime? ThoiGianCuoi { get; set; }

        [Column("Sl_llkebd", TypeName = "decimal(18, 2)")]
        public decimal? SlLlkebd { get; set; }

        [Column("Sl_llkekt", TypeName = "decimal(18, 2)")]
        public decimal? SlLlkekt { get; set; }

        [Column("HeSo_k", TypeName = "decimal(6, 4)")]
        public decimal? HeSoK { get; set; }

        [Column("NhietDo", TypeName = "decimal(5, 2)")]
        public decimal? NhietDo { get; set; }

        [Column("TyTrong_15", TypeName = "decimal(6, 4)")]
        public decimal? TyTrong15 { get; set; }

        [Column("MaDanXuat", TypeName = "nvarchar(2)")]
        public string? MaDanXuat { get; set; }

        [Column("MaLoi", TypeName = "nvarchar(6)")]
        public string? MaLoi { get; set; }

        [Column("TrangThai", TypeName = "nvarchar(2)")]
        public string? TrangThai { get; set; }

        [Column("MaLuuLuongKe", TypeName = "nvarchar(30)")]
        public string? MaLuuLuongKe { get; set; }

        [Column("MaEntry", TypeName = "decimal(6, 0)")]
        public decimal? MaEntry { get; set; }

        [Column("MaLo", TypeName = "decimal(6, 0)")]
        public decimal? MaLo { get; set; }

        [Column("GhiChu", TypeName = "nvarchar(50)")]
        public string? GhiChu { get; set; }

        [Column("Status", TypeName = "char(2)")]
        public string? Status { get; set; }

        [Column("ERate", TypeName = "nvarchar(6)")]
        public string? ERate { get; set; }

        [Column("GV", TypeName = "real")]
        public float? GV { get; set; }

        [Column("GST", TypeName = "real")]
        public float? GST { get; set; }

        [Column("GVTOTAL_START", TypeName = "real")]
        public float? GvTotalStart { get; set; }

        [Column("GVTOTAL_END", TypeName = "real")]
        public float? GvTotalEnd { get; set; }

        [Column("GSTTOTAL_START", TypeName = "real")]
        public float? GstTotalStart { get; set; }

        [Column("GSTTOTAL_END", TypeName = "real")]
        public float? GstTotalEnd { get; set; }

        [Column("KF", TypeName = "real")]
        public float? KF { get; set; }

        [Column("KF_E", TypeName = "real")]
        public float? KfE { get; set; }

        [Column("TY_TRONG", TypeName = "real")]
        public float? TyTrong { get; set; }

        [Column("AVG_MF", TypeName = "real")]
        public float? AvgMf { get; set; }

        [Column("AVG_MF_E", TypeName = "real")]
        public float? AvgMfE { get; set; }

        [Column("AVG_CTL", TypeName = "real")]
        public float? AvgCtl { get; set; }

        [Column("AVG_CTL_E", TypeName = "real")]
        public float? AvgCtlE { get; set; }

        [Column("AVG_CTL_BASE", TypeName = "real")]
        public float? AvgCtlBase { get; set; }

        [Column("RTD_OFFSET", TypeName = "real")]
        public float? RtdOffset { get; set; }

        [Column("GV_E", TypeName = "real")]
        public float? GvE { get; set; }

        [Column("GST_E", TypeName = "real")]
        public float? GstE { get; set; }

        [Column("GVTOTAL_E_START", TypeName = "real")]
        public float? GvTotalEStart { get; set; }

        [Column("GVTOTAL_E_END", TypeName = "real")]
        public float? GvTotalEEnd { get; set; }

        [Column("GSTTOTAL_E_START", TypeName = "real")]
        public float? GstTotalEStart { get; set; }

        [Column("GSTTOTAL_E_END", TypeName = "real")]
        public float? GstTotalEEnd { get; set; }

        [Column("GV_BASE", TypeName = "real")]
        public float? GvBase { get; set; }

        [Column("GST_BASE", TypeName = "real")]
        public float? GstBase { get; set; }

        [Column("GVTOTAL_BASE_START", TypeName = "real")]
        public float? GvTotalBaseStart { get; set; }

        [Column("GVTOTAL_BASE_END", TypeName = "real")]
        public float? GvTotalBaseEnd { get; set; }

        [Column("GSTTOTAL_BASE_START", TypeName = "real")]
        public float? GstTotalBaseStart { get; set; }

        [Column("GSTTOTAL_BASE_END", TypeName = "real")]
        public float? GstTotalBaseEnd { get; set; }

        [Column("TYLE_TTE", TypeName = "real")]
        public float? TyleTte { get; set; }

        [Column("V_PRESET", TypeName = "real")]
        public float? VPreset { get; set; }

        [Column("TYLE_PRESET", TypeName = "real")]
        public float? TylePreset { get; set; }

        [Column("TYTRONG_BASE", TypeName = "real")]
        public float? TyTrongBase { get; set; }

        [Column("TYTRONG_E", TypeName = "real")]
        public float? TyTrongE { get; set; }

        [Column("NGAY_DKY")]
        public DateTime? NgayDky { get; set; }

        [Column("NGAY_BD")]
        public DateTime? NgayBd { get; set; }

        [Column("NGAY_KT")]
        public DateTime? NgayKt { get; set; }

        [Column("SO_CTU", TypeName = "char(7)")]
        public string? SoCtu { get; set; }

        [Column("MA_LENH", TypeName = "numeric(10, 0)")]
        public decimal? MaLenhNum { get; set; }

        [Column("CARD_DATA", TypeName = "char(7)")]
        public string? CardData1 { get; set; }

        [Column("MA_NGAN", TypeName = "tinyint")]
        public byte? MaNganByte { get; set; }

        [Column("MA_HHOA", TypeName = "char(7)")]
        public string? MaHhoa { get; set; }

        [Column("MA_HONG", TypeName = "tinyint")]
        public byte? MaHong { get; set; }

        [Column("MA_KHO", TypeName = "char(8)")]
        public string? MaKho { get; set; }

        [Column("NHIET_DOTB", TypeName = "real")]
        public float? NhietDoTb { get; set; }

        [Column("TRANG_THAI", TypeName = "tinyint")]
        public byte? TrangThaiByte { get; set; }

        [Column("SO_PTIEN", TypeName = "char(10)")]
        public string? SoPtien { get; set; }

        [Column("LAI_XE", TypeName = "char(20)")]
        public string? LaiXe { get; set; }

        [Column("TY_TRONGTB", TypeName = "real")]
        public float? TyTrongTb { get; set; }

        [Column("TY_TRONGTB_BASE", TypeName = "real")]
        public float? TyTrongTbBase { get; set; }

        [Column("TY_TRONGTB_E", TypeName = "real")]
        public float? TyTrongTbE { get; set; }

        [Column("MASS", TypeName = "real")]
        public float? Mass { get; set; }

        [Column("MASS_BASE", TypeName = "real")]
        public float? MassBase { get; set; }

        [Column("MASS_E", TypeName = "real")]
        public float? MassE { get; set; }

        [Column("MASSTOTAL_START", TypeName = "real")]
        public float? MassTotalStart { get; set; }

        [Column("MASSTOTAL_END", TypeName = "real")]
        public float? MassTotalEnd { get; set; }

        [Column("MASSTOTAL_BASE_START", TypeName = "real")]
        public float? MassTotalBaseStart { get; set; }

        [Column("MASSTOTAL_BASE_END", TypeName = "real")]
        public float? MassTotalBaseEnd { get; set; }

        [Column("MASSTOTAL_E_START", TypeName = "real")]
        public float? MassTotalEStart { get; set; }

        [Column("MASSTOTAL_E_END", TypeName = "real")]
        public float? MassTotalEEnd { get; set; }

        [Column("Createby", TypeName = "nvarchar(30)")]
        public string? CreateByName { get; set; }

        [Column("UpdatedBy", TypeName = "nvarchar(30)")]
        public string? UpdateByName { get; set; }

        [Column("CreateDate")]
        public DateTime? CreateDateTime { get; set; }

        [Column("UpdateDate")]
        public DateTime? UpdateDateTime { get; set; }

        [Column("DungTichNgan", TypeName = "int")]
        public int? DungTichNgan { get; set; }

        [Column("TableID", TypeName = "varchar(8)")]
        public string? TableId { get; set; }

        [Column("MaTuDongHoa", TypeName = "varchar(8)")]
        public string? MaTuDongHoa { get; set; }

        [Column("Row_id", TypeName = "int")]
        public int? RowId { get; set; }

        [Column("PhuongTien", TypeName = "nvarchar(50)")]
        public string? PhuongTien { get; set; }

        [Column("Record_Status", TypeName = "nvarchar(50)")]
        public string? RecordStatus { get; set; }

        [Column("DO_CREATE")]
        public DateTime? DoCreate { get; set; }

        [Column("SO_TT", TypeName = "int")]
        public int? SoTt { get; set; }

        [Column("FlagTankLine", TypeName = "nvarchar(1)")]
        public string? FlagTankLine { get; set; }

        [Column("GST_TDH", TypeName = "decimal(18, 2)")]
        public decimal? GstTdh { get; set; }

        [Column("L15", TypeName = "decimal(18, 3)")]
        public decimal? L15 { get; set; }

        [Column("KG", TypeName = "decimal(18, 3)")]
        public decimal? Kg { get; set; }

        [Column("BQGQ_NhietDo", TypeName = "decimal(18, 2)")]
        public decimal? BqgqNhietDo { get; set; }

        [Column("BQGQ_D15", TypeName = "decimal(18, 4)")]
        public decimal? BqgqD15 { get; set; }

        [Column("VCF", TypeName = "decimal(18, 4)")]
        public decimal? Vcf { get; set; }

        [Column("WCF", TypeName = "decimal(18, 4)")]
        public decimal? Wcf { get; set; }

        [Column("CardNum", TypeName = "nvarchar(100)")]
        public string? CardNum { get; set; }

        [Column("CardData", TypeName = "nvarchar(100)")]
        public string? CardData2 { get; set; }
    }
}
