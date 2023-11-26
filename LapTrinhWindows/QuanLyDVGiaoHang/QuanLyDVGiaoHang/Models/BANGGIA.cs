namespace QuanLyDVGiaoHang.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BANGGIA")]
    public partial class BANGGIA
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BANGGIA()
        {
            DONHANGs = new HashSet<DONHANG>();
        }

        [Key]
        [StringLength(30)]
        public string KhoiLuong { get; set; }

        public double? GiaNoiTinh { get; set; }

        public double? GiaNoiVung { get; set; }

        public double? GiaLienVung { get; set; }

        public double? GiaCachVung { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DONHANG> DONHANGs { get; set; }
    }
}
