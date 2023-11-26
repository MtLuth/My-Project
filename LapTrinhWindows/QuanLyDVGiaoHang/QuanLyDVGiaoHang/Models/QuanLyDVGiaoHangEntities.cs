using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.IO;
using System.Linq;

namespace QuanLyDVGiaoHang.Models
{
    public partial class QuanLyDVGiaoHangEntities : DbContext
    {
        public QuanLyDVGiaoHangEntities()
            : base("name=QuanLyDVGiaoHangEntities")
        {
        }
        private static string GetConnectionString()
        {
            string relativePath = @"QuanLyDVGiaoHangg.mdf";
            string fullPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath));
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + fullPath + ";Integrated Security=True";
            return connectionString;
        }
        //public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<BANGGIA> BANGGIAs { get; set; }
        public virtual DbSet<DONHANG> DONHANGs { get; set; }
        public virtual DbSet<DS_DONHANGCANGIAONHAN> DS_DONHANGCANGIAONHAN { get; set; }
        public virtual DbSet<DS_DONHANGCANTRUNGCHUYEN> DS_DONHANGCANTRUNGCHUYEN { get; set; }
        public virtual DbSet<DS_DONHANGDENKHO> DS_DONHANGDENKHO { get; set; }
        public virtual DbSet<KHACHHANG> KHACHHANGs { get; set; }
        public virtual DbSet<KHOTRUNGCHUYEN> KHOTRUNGCHUYENs { get; set; }
        public virtual DbSet<LOAIHANGHOA> LOAIHANGHOAs { get; set; }
        public virtual DbSet<NHANVIEN> NHANVIENs { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<TK_KHACHHANG> TK_KHACHHANG { get; set; }
        public virtual DbSet<TK_NVGIAOHANG> TK_NVGIAOHANG { get; set; }
        public virtual DbSet<TK_NVTRUNGCHUYEN> TK_NVTRUNGCHUYEN { get; set; }
        public virtual DbSet<TK_QUANLY> TK_QUANLY { get; set; }
        public virtual DbSet<VAITRO> VAITROes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BANGGIA>()
                .Property(e => e.KhoiLuong)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DONHANG>()
                .Property(e => e.LoaiHangHoa)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DONHANG>()
                .Property(e => e.KhoiLuong)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DS_DONHANGCANGIAONHAN>()
                .Property(e => e.TKGiaoNhanHang)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DS_DONHANGCANTRUNGCHUYEN>()
                .Property(e => e.TKTrungChuyen)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DS_DONHANGDENKHO>()
                .Property(e => e.TKKiemTra)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KHACHHANG>()
                .Property(e => e.SoDT)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KHACHHANG>()
                .Property(e => e.Email)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KHOTRUNGCHUYEN>()
                .Property(e => e.MaKho)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KHOTRUNGCHUYEN>()
                .Property(e => e.HotLine)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<LOAIHANGHOA>()
                .Property(e => e.MaLoai)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<LOAIHANGHOA>()
                .HasMany(e => e.DONHANGs)
                .WithOptional(e => e.LOAIHANGHOA1)
                .HasForeignKey(e => e.LoaiHangHoa);

            modelBuilder.Entity<NHANVIEN>()
                .Property(e => e.MaVaiTro)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<NHANVIEN>()
                .Property(e => e.MaKho)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<NHANVIEN>()
                .Property(e => e.SoDT)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<NHANVIEN>()
                .HasMany(e => e.TK_NVGIAOHANG)
                .WithOptional(e => e.NHANVIEN)
                .HasForeignKey(e => e.MaNVGiaoHang);

            modelBuilder.Entity<NHANVIEN>()
                .HasMany(e => e.TK_NVTRUNGCHUYEN)
                .WithOptional(e => e.NHANVIEN)
                .HasForeignKey(e => e.MaNVTrungChuyen);

            modelBuilder.Entity<NHANVIEN>()
                .HasMany(e => e.TK_QUANLY)
                .WithOptional(e => e.NHANVIEN)
                .HasForeignKey(e => e.MaNVQuanLy);

            modelBuilder.Entity<TK_KHACHHANG>()
                .Property(e => e.TenDangNhap)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TK_KHACHHANG>()
                .Property(e => e.MatKhau)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TK_NVGIAOHANG>()
                .Property(e => e.TenDangNhap)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TK_NVGIAOHANG>()
                .Property(e => e.MatKhau)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TK_NVGIAOHANG>()
                .HasMany(e => e.DS_DONHANGCANGIAONHAN)
                .WithOptional(e => e.TK_NVGIAOHANG)
                .HasForeignKey(e => e.TKGiaoNhanHang);

            modelBuilder.Entity<TK_NVTRUNGCHUYEN>()
                .Property(e => e.TenDangNhap)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TK_NVTRUNGCHUYEN>()
                .Property(e => e.MatKhau)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TK_NVTRUNGCHUYEN>()
                .HasMany(e => e.DS_DONHANGCANTRUNGCHUYEN)
                .WithOptional(e => e.TK_NVTRUNGCHUYEN)
                .HasForeignKey(e => e.TKTrungChuyen);

            modelBuilder.Entity<TK_QUANLY>()
                .Property(e => e.TenDangNhap)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TK_QUANLY>()
                .Property(e => e.MatKhau)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TK_QUANLY>()
                .HasMany(e => e.DS_DONHANGDENKHO)
                .WithOptional(e => e.TK_QUANLY)
                .HasForeignKey(e => e.TKKiemTra);

            modelBuilder.Entity<VAITRO>()
                .Property(e => e.MaVaiTro)
                .IsFixedLength()
                .IsUnicode(false);
        }
    }
}
