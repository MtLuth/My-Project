namespace QuanLyDVGiaoHang.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TK_KHACHHANG
    {
        public int? MaKhachHang { get; set; }

        [Key]
        [StringLength(30)]
        public string TenDangNhap { get; set; }

        [Required]
        [StringLength(30)]
        public string MatKhau { get; set; }

        public int? SoDu { get; set; }

        public virtual KHACHHANG KHACHHANG { get; set; }
    }
}
