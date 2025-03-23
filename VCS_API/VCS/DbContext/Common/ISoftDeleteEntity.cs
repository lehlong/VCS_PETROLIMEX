using System.ComponentModel.DataAnnotations.Schema;

namespace VCS.DbContext.Common
{
    public interface ISoftDeleteEntity
    {
        [Column(TypeName = "Number(1,0)")]
        public bool? IsDeleted { get; set; }
        public DateTime? DeleteDate { get; set; }
        public string? DeleteBy { get; set; }
    }
}
