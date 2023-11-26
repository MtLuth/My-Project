namespace QuanLyDVGiaoHang.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DONHANG")]
    public partial class DONHANG
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DONHANG()
        {
            DS_DONHANGDENKHO = new HashSet<DS_DONHANGDENKHO>();
            DS_DONHANGCANTRUNGCHUYEN = new HashSet<DS_DONHANGCANTRUNGCHUYEN>();
            DS_DONHANGCANGIAONHAN = new HashSet<DS_DONHANGCANGIAONHAN>();
        }

        [Key]
        public int MaVanDon { get; set; }

        [Required]
        public string NguoiGui { get; set; }

        [Required]
        public string NguoiNhan { get; set; }

        [Required]
        public string DiaChiNguoiGui { get; set; }

        [Required]
        public string DiaChiNguoiNhan { get; set; }

        [Column(TypeName = "date")]
        public DateTime? NgayTaoDon { get; set; }

        [Required]
        public string ThongTinHangHoa { get; set; }

        [StringLength(6)]
        public string LoaiHangHoa { get; set; }

        [StringLength(30)]
        public string KhoiLuong { get; set; }

        public int? PhiVanChuyen { get; set; }

        public int? PhiCOD { get; set; }

        public string TinhTrang { get; set; }

        public virtual BANGGIA BANGGIA { get; set; }

        public virtual LOAIHANGHOA LOAIHANGHOA1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DS_DONHANGDENKHO> DS_DONHANGDENKHO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DS_DONHANGCANTRUNGCHUYEN> DS_DONHANGCANTRUNGCHUYEN { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DS_DONHANGCANGIAONHAN> DS_DONHANGCANGIAONHAN { get; set; }
    }
}
