using DBL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL
{
    public class DBDichVu
    {
        DALayer db = null;
        public DBDichVu()
        {
            db = new DALayer();
        }
        public DataSet LayDichVu()
        {
            return db.ExecuteQueryDataSet(
                "select * from DICHVU", CommandType.Text, null);
        }
        public bool ThemDichVu(ref string err, string MaDV, string MaLoai, string TenDV, string Giatien, string SoLuong, byte[] HinhAnh)
        {
            return db.MyExecuteNonQuery("spThemDichVu",
                CommandType.StoredProcedure, ref err,
                new SqlParameter("@Ma_DV", MaDV),
                new SqlParameter("@Ma_LOAI", MaLoai),
                new SqlParameter("@ten_dich_vu", TenDV),
                new SqlParameter("@gia_tien", Giatien),
                new SqlParameter("@so_luong", SoLuong),
                new SqlParameter("@HinhAnh", SqlDbType.Image) { Value = HinhAnh });
        }
        // DELETE
        public bool XoaDichVu(ref string err, string MaDV)
        {
            return db.MyExecuteNonQuery("spXoaDichVu",
                CommandType.StoredProcedure, ref err,
                new SqlParameter("@Ma_DV", MaDV));
        }
        // UPDATE
        public bool CapNhatDichVu(ref string err, string MaDV, string MaLoai, string TenDV, string Giatien, string SoLuong, byte[] HinhAnh)
        {
            return db.MyExecuteNonQuery("spSuaDichVu",
                CommandType.StoredProcedure, ref err,
                new SqlParameter("@Ma_DV", MaDV),
                new SqlParameter("@Ma_LOAI", MaLoai),
                new SqlParameter("@ten_dich_vu", TenDV),
                new SqlParameter("@gia_tien", Giatien),
                new SqlParameter("@so_luong", SoLuong),
                new SqlParameter("@HinhAnh", SqlDbType.Image) { Value = HinhAnh });
        }
        public DataSet TimKiemDichVu(string MaDV, string MaLoai, string TenDV, string Giatien, string SoLuong,
           out string error)
        {
            string sqlString = "Select * From dbo.TimKiemDichVu(@Ma_DV, @Ma_LOAI, @ten_dich_vu, @gia_tien, @so_luong)";
            error = null;
            try
            {
                return db.ExecuteQueryDataSet(sqlString, CommandType.Text,
                new SqlParameter("@Ma_DV", (object)MaDV ?? DBNull.Value),
                new SqlParameter("@Ma_LOAI", (object)MaLoai ?? DBNull.Value),
                new SqlParameter("@ten_dich_vu", (object)TenDV ?? DBNull.Value),
                new SqlParameter("@gia_tien", (object)Giatien ?? DBNull.Value),
                new SqlParameter("@so_luong", (object)SoLuong ?? DBNull.Value));
            }
            catch (DataException ex)
            {
                error = ex.Message;
                return null;
            }
        }
        public DataSet TimKiemDichVuKH(string TenDV,
            out string error)
        {
            string sqlString = "SELECT * FROM TimKiemDichVuTheoTen(@TenDV)";
            error = null;
            try
            {
                return db.ExecuteQueryDataSet(sqlString, CommandType.Text,
                    new SqlParameter("@TenDV", (object)TenDV ?? DBNull.Value));
            }
            catch (DataException ex)
            {
                error = ex.Message;
                return null;
            }
        }
        public DataSet TimKiemDichVuTheoLoai(string MaLoai)
        {
            string sqlString = "SELECT * FROM TimKiemDichVuTheoLoai(@MaLoai)";
            return db.ExecuteQueryDataSet(sqlString, CommandType.Text,
                new SqlParameter("@MaLoai", (object)MaLoai ?? DBNull.Value));
        }
        public DataSet HienThiDanhSachDichVu()
        {
            return db.ExecuteQueryDataSet(
                "SELECT * FROM HienThiDanhSachDichVu()", CommandType.Text, null);
        }
        public DataSet HienThiDanhSachDichVuVanCon()
        {
            return db.ExecuteQueryDataSet(
                "SELECT * FROM HienThiDanhSachDichVuVanCon()", CommandType.Text, null);
        }
    }
}
