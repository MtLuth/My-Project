namespace QuanLyDVGiaoHang.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KHACHHANG")]
    public partial class KHACHHANG
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public KHACHHANG()
        {
            TK_KHACHHANG = new HashSet<TK_KHACHHANG>();
        }

        [Key]
        public int MaKhachHang { get; set; }

        [Required]
        public string TenNguoiBan { get; set; }

        [Required]
        public string TenCuaHang { get; set; }

        [Required]
        public string DiaChi { get; set; }

        [Required]
        [StringLength(10)]
        public string SoDT { get; set; }

        [Required]
        [StringLength(30)]
        public string Email { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TK_KHACHHANG> TK_KHACHHANG { get; set; }
    }
}
