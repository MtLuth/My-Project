namespace QuanLyDVGiaoHang.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TK_QUANLY
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TK_QUANLY()
        {
            DS_DONHANGDENKHO = new HashSet<DS_DONHANGDENKHO>();
        }

        public int? MaNVQuanLy { get; set; }

        [Key]
        [StringLength(30)]
        public string TenDangNhap { get; set; }

        [Required]
        [StringLength(30)]
        public string MatKhau { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DS_DONHANGDENKHO> DS_DONHANGDENKHO { get; set; }

        public virtual NHANVIEN NHANVIEN { get; set; }
    }
}
