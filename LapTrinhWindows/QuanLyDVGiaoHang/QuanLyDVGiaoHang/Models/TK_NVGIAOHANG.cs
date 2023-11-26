namespace QuanLyDVGiaoHang.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TK_NVGIAOHANG
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TK_NVGIAOHANG()
        {
            DS_DONHANGCANGIAONHAN = new HashSet<DS_DONHANGCANGIAONHAN>();
        }

        public int? MaNVGiaoHang { get; set; }

        [Key]
        [StringLength(30)]
        public string TenDangNhap { get; set; }

        [Required]
        [StringLength(30)]
        public string MatKhau { get; set; }

        [Required]
        public string KhuVucGiaoHang { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DS_DONHANGCANGIAONHAN> DS_DONHANGCANGIAONHAN { get; set; }

        public virtual NHANVIEN NHANVIEN { get; set; }
    }
}
