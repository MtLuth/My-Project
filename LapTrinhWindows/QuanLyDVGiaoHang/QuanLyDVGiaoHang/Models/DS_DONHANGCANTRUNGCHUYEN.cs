namespace QuanLyDVGiaoHang.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DS_DONHANGCANTRUNGCHUYEN
    {
        [StringLength(30)]
        public string TKTrungChuyen { get; set; }

        public int? MaVanDon { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Ngay { get; set; }

        public TimeSpan? Gio { get; set; }

        public string TrangThai { get; set; }

        [Key]
        public int MaDS { get; set; }

        public virtual DONHANG DONHANG { get; set; }

        public virtual TK_NVTRUNGCHUYEN TK_NVTRUNGCHUYEN { get; set; }
    }
}
