namespace QuanLyDVGiaoHang.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DS_DONHANGDENKHO
    {
        [StringLength(30)]
        public string TKKiemTra { get; set; }

        public int? MaVanDon { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Ngay { get; set; }

        public TimeSpan? Gio { get; set; }

        public string TuyenTrungChuyenTT { get; set; }

        [Key]
        public int MaDS { get; set; }

        public virtual DONHANG DONHANG { get; set; }

        public virtual TK_QUANLY TK_QUANLY { get; set; }
    }
}
