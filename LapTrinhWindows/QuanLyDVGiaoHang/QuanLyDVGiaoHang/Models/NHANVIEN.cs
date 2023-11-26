namespace QuanLyDVGiaoHang.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NHANVIEN")]
    public partial class NHANVIEN
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NHANVIEN()
        {
            TK_NVGIAOHANG = new HashSet<TK_NVGIAOHANG>();
            TK_NVTRUNGCHUYEN = new HashSet<TK_NVTRUNGCHUYEN>();
            TK_QUANLY = new HashSet<TK_QUANLY>();
        }

        [Key]
        public int MaNV { get; set; }

        [StringLength(6)]
        public string MaVaiTro { get; set; }

        [StringLength(6)]
        public string MaKho { get; set; }

        [StringLength(50)]
        public string HoTen { get; set; }

        [Column(TypeName = "date")]
        public DateTime NgaySinh { get; set; }

        public string DiaChi { get; set; }

        [Required]
        [StringLength(11)]
        public string SoDT { get; set; }

        public virtual KHOTRUNGCHUYEN KHOTRUNGCHUYEN { get; set; }

        public virtual VAITRO VAITRO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TK_NVGIAOHANG> TK_NVGIAOHANG { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TK_NVTRUNGCHUYEN> TK_NVTRUNGCHUYEN { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TK_QUANLY> TK_QUANLY { get; set; }
    }
}
