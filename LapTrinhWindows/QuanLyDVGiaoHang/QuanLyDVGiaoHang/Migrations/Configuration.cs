namespace QuanLyDVGiaoHang.Migrations
{
    using QuanLyDVGiaoHang.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.IO;

    internal sealed class Configuration : DbMigrationsConfiguration<QuanLyDVGiaoHang.Models.QuanLyDVGiaoHangEntities>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(QuanLyDVGiaoHang.Models.QuanLyDVGiaoHangEntities context)
        {
            // Thêm dữ liệu vào bảng BANGGIA
            context.BANGGIAs.AddOrUpdate(
                b => b.KhoiLuong,
                new BANGGIA
                {
                    KhoiLuong = "< 0.05",
                    GiaNoiTinh = 8000,
                    GiaNoiVung = 8000,
                    GiaLienVung = 10000,
                    GiaCachVung = 9100
                },
                new BANGGIA
                {
                    KhoiLuong = "0.05 - 0.1",
                    GiaNoiTinh = 8000,
                    GiaNoiVung = 11800,
                    GiaLienVung = 14000,
                    GiaCachVung = 13300
                },
                new BANGGIA
                {
                    KhoiLuong = "0.1 - 0.25",
                    GiaNoiTinh = 10000,
                    GiaNoiVung = 16500,
                    GiaLienVung = 23000,
                    GiaCachVung = 22000
                },
                new BANGGIA
                {
                    KhoiLuong = "0.25 - 0.5",
                    GiaNoiTinh = 12500,
                    GiaNoiVung = 23900,
                    GiaLienVung = 29900,
                    GiaCachVung = 28600
                },
                new BANGGIA
                {
                    KhoiLuong = "0.5 - 1",
                    GiaNoiTinh = 16000,
                    GiaNoiVung = 33200,
                    GiaLienVung = 43700,
                    GiaCachVung = 41800
                },
                new BANGGIA
                {
                    KhoiLuong = "1 - 1.5",
                    GiaNoiTinh = 19000,
                    GiaNoiVung = 40000,
                    GiaLienVung = 56400,
                    GiaCachVung = 53900
                },
                new BANGGIA
                {
                    KhoiLuong = "1.5 - 2",
                    GiaNoiTinh = 21000,
                    GiaNoiVung = 48400,
                    GiaLienVung = 68500,
                    GiaCachVung = 65500
                },
                new BANGGIA
                {
                    KhoiLuong = "+ 0.5",
                    GiaNoiTinh = 1700,
                    GiaNoiVung = 3500,
                    GiaLienVung = 8500,
                    GiaCachVung = 8100
                }
            );

            // Thêm dữ liệu vào bảng KHOTRUNGCHUYEN
            context.KHOTRUNGCHUYENs.AddOrUpdate(
                k => k.MaKho,
                new KHOTRUNGCHUYEN
                {
                    MaKho = "KHOSG1",
                    TenKho = "Kho Quận 1",
                    KhuVucQuanLy = "Hồ Chí Minh",
                    HotLine = "0865100206"
                },
                new KHOTRUNGCHUYEN
                {
                    MaKho = "KHOSG2",
                    TenKho = "Kho Thủ Đức 1",
                    KhuVucQuanLy = "Hồ Chí Minh",
                    HotLine = "0868333809"
                },
                new KHOTRUNGCHUYEN
                {
                    MaKho = "KHODN1",
                    TenKho = "Kho Đà Nẵng 1",
                    KhuVucQuanLy = "Đà Nẵng",
                    HotLine = "0353101379"
                },
                new KHOTRUNGCHUYEN
                {
                    MaKho = "KHOHN1",
                    TenKho = "Kho Hà Nội 1",
                    KhuVucQuanLy = "Hà Nội",
                    HotLine = "0869545350"
                },
                new KHOTRUNGCHUYEN
                {
                    MaKho = "KHOHN2",
                    TenKho = "Kho Hà Nội 2",
                    KhuVucQuanLy = "Hà Nội",
                    HotLine = "0865195265"
                },
                new KHOTRUNGCHUYEN
                {
                    MaKho = "KHONA1",
                    TenKho = "Kho Nghệ An 1",
                    KhuVucQuanLy = "Nghệ An",
                    HotLine = "0359415345"
                },
                new KHOTRUNGCHUYEN
                {
                    MaKho = "KHOPY1",
                    TenKho = "Kho Phú Yên 1",
                    KhuVucQuanLy = "Phú Yên",
                    HotLine = "0868515057"
                }
            );

            // Thêm dữ liệu vào bảng VAITRO
            context.VAITROes.AddOrUpdate(
                v => v.MaVaiTro,
                new VAITRO
                {
                    MaVaiTro = "VT_QL1",
                    TenVaiTro = "Quản lý"
                },
                new VAITRO
                {
                    MaVaiTro = "VT_GH1",
                    TenVaiTro = "Nhân viên giao hàng"
                },
                new VAITRO
                {
                    MaVaiTro = "VT_TC1",
                    TenVaiTro = "Nhân viên trung chuyển"
                }
            );

            // Thêm dữ liệu vào bảng NHANVIEN
            context.NHANVIENs.AddOrUpdate(
                n => n.MaNV,
                new NHANVIEN
                {
                    MaNV = 1,
                    MaVaiTro = "VT_QL1",
                    MaKho = "KHOSG1",
                    HoTen = "Nguyễn Admin",
                    NgaySinh = new DateTime(2003, 01, 01),
                    DiaChi = "KTX Khu B, Phường Linh Trung, Thành phố Thủ Đức, Thành phố Hồ Chí Minh",
                    SoDT = "0123456789"
                }
            );


            // Thêm dữ liệu vào bảng TK_QUANLY
            context.TK_QUANLY.AddOrUpdate(
                t => t.MaNVQuanLy,
                new TK_QUANLY
                {
                    MaNVQuanLy = 1,
                    TenDangNhap = "admin",
                    MatKhau = "123"
                }
            );
            context.SaveChanges();
            base.Seed(context);
        }
    }
    
}
